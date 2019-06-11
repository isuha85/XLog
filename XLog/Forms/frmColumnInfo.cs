using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Win32;
using Timer = System.Windows.Forms.Timer;

//using System.Threading.Tasks;
//using System.Configuration;
//using Sprache;

using System.Linq;
using System.Data;                          // 공통 인터페이스 , IDbConnection .. 등
using System.Data.Common;                   // 추상 클래스 , DbConnection .. 등
using System.Data.OleDb;                    // Any DB - oracle / SQL Server / tibero / ..
using System.Data.SqlClient;                // SQL Server
using Altibase.Data.AltibaseClient;         // altibase
using Tibero.DbAccess;                      // tibero
using Oracle.ManagedDataAccess.Client;      // Managed 드라이버 (32/64 bit에 무방), deprecated - using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Types;
using MySql.Data.MySqlClient;

namespace XLog
{
	public partial class frmColumnInfo : Form
	{
		private struct Configure
		{
			public Point LocationPre;
			public int HeightPre;
			public int WidthPre;

			// TODO: 휘발성으로, 재구동이후에는 유지되지 않는다. 설정파일 개념이 요구됨
			public bool checked_insertComma;
			public bool checked_lowerCase;
			public bool checked_showPK;
			public bool checked_showComment;

			public string selectedText;
			public DbConnection conn;
		};
		static Configure configure;

		static frmColumnInfo()
		{
			configure.LocationPre = new Point(-1, -1);
			configure.HeightPre = -1;
			configure.WidthPre = -1;

			configure.checked_insertComma = false;
			configure.checked_lowerCase = false;
			configure.checked_showPK = false;
			configure.checked_showComment = false;
		}

		public frmColumnInfo()
		{
			InitializeComponent();
			AutoScaleMode = AutoScaleMode.Inherit;

			//cbShowComment.Visible = false;
			cbShowPK.Visible = false;

			dataGridView.BackgroundColor = Color.White;

			cbInsertComma.Checked = configure.checked_insertComma;
			cbLowerCase.Checked = configure.checked_lowerCase;
			cbShowComment.Checked = configure.checked_showComment;

			//if (configure.LocationPre == new Point(0, 0))
			if (configure.LocationPre.X == -1)
			{
				// [TIP] 아래 구문이 _LOAD 함수에서 호출되면, 동작하지 않는다, 이유는 모름.
				StartPosition = FormStartPosition.CenterScreen;
				//StartPosition = FormStartPosition.CenterParent;
				//StartPosition = FormStartPosition.WindowsDefaultLocation;
			}
			else
			{
				StartPosition = FormStartPosition.Manual;
				Location = configure.LocationPre;
				Height = configure.HeightPre;
				Width = configure.WidthPre;
			}
		}

		private void frmColumnInfo_Load(object sender, EventArgs e)
		{
			// TODO: xx_LOAD 함수와 생성자의 역활차이는 뭘까?
		}

		public void ShowMain(string selectedText, DbConnection conn)
		{
			if ( conn != null )
			{
				SetDataTable(selectedText, conn);
				if ( dataGridView.RowCount > 0 )
				{
					Show();
				}
			}
		}

		#region SetDataTable
		public void SetDataTable(string selectedText, DbConnection conn)
		{
			configure.selectedText = selectedText;
			configure.conn = conn;

			if (conn is OracleConnection oracleConnection)
			{
				SetDataTable_(selectedText, oracleConnection); // 인자타입이 다름. 재귀호출 아니다.
			}
			else if (conn is AltibaseConnection altibaseConnection)
			{
				SetDataTable_(selectedText, altibaseConnection);
			}
			else if (conn is SqlConnection sqlConnection)
			{
				SetDataTable_(selectedText, sqlConnection);
			}
			//else if (conn is OleDbConnectionTbr oleDbConnectionTbr)
			//{
			//	SetDataTable_(selectedText, oleDbConnectionTbr);
			//}
			//else if (conn is OleDbConnection oleDbConnection)
			//{
			//	SetDataTable_(selectedText, oleDbConnection);
			//}

			Application.DoEvents();
		}

		static OracleCommand oraCmd1 = null;
		static OracleCommand oraCmd2 = null;
		static OracleCommand oraCmd3 = null;
		static OracleCommand oraCmd4 = null;
		private void SetDataTable_(string selectedText, OracleConnection conn)
		{
			string tname = null;
			string user = null;
			string object_type = null;

			//OracleCommand oracleCommandColumnInfo = null;
			OracleDataReader dataReader = null;

			if (oraCmd1 == null)
			{
				oraCmd1 = new OracleCommand();
				oraCmd1.Connection = conn;
			}
			if (oraCmd2 == null)
			{
				oraCmd2 = new OracleCommand();
				oraCmd2.Connection = conn;
			}
			if (oraCmd3 == null)
			{
				oraCmd3 = new OracleCommand();
				oraCmd3.Connection = conn;
			}
			if (oraCmd4 == null)
			{
				oraCmd4 = new OracleCommand();
				oraCmd4.Connection = conn;
			}

			try
			{
				var ix = selectedText.IndexOf('.');
				if (ix != -1)
				{
					user = selectedText.Substring(0, ix);
					tname = selectedText.Substring(ix + 1, selectedText.Length - ix - 1);
				}
				else
				{
					tname = selectedText;
				}


				// TODO: 'USERNAME' 을 매번 구하는 건 낭비이다.
				if (user == null)
				{
					oraCmd1.Parameters.Clear();
					oraCmd1.CommandText = @"select user from dual";
					user = (string)oraCmd1.ExecuteScalar();
				}

RETRY_CASE_1: // [재귀호출] U1.SYNONYM 인 경우가 중첩될 수 있다.
				if (dataReader != null)
				{
					//dataReader.Close();
					//dataReader.Dispose();
					dataReader = null; // null 처리시, GC가 처리대상이 됨
				}

				// CASE-1 : 해당 USER에서 검색
				if (object_type == null)
				{
					oraCmd2.Parameters.Clear();
					oraCmd2.CommandText =
		@"select decode(object_type, 'UNDEFINED', NULL, object_type) object_type " +
		"	from sys.all_objects " +
		"  where 2=2" +
		"	 and owner = :v_user " +
		"	 and object_name = :v_tname " +
		"	 and object_type <> 'SYNONYM' ";

					{
						OracleParameter v_user = new OracleParameter(":v_user", OracleDbType.Varchar2, 32);
						v_user.Value = user.ToUpper();
						oraCmd2.Parameters.Add(v_user);

						OracleParameter v_tname = new OracleParameter(":v_tname", OracleDbType.Varchar2, 32);
						v_tname.Value = tname.ToUpper();
						oraCmd2.Parameters.Add(v_tname);
					}

					object_type = (string)oraCmd2.ExecuteScalar();					
				}

				// CASE-2 : 아니면 SYNONYM 이다.
				if (object_type == null)
				{
					oraCmd3.Parameters.Clear();
					oraCmd3.CommandText =
		@"select table_owner, table_name, db_link " +
		"   from sys.all_synonyms " +
		"  where 2=2" +
		"	 and (owner = 'PUBLIC' or owner = :v_user) " +
		"	 and synonym_name = :v_tname " +
		"  order by decode(owner,'PUBLIC',NULL,owner) nulls last"; // 기본값이 NULL이 마지막에 조회되나 명시함
					oraCmd3.CommandText = "select * from ( " + oraCmd3.CommandText + " ) where rownum = 1";


					{
						OracleParameter v_user = new OracleParameter(":v_user", OracleDbType.Varchar2, 32);
						v_user.Value = user.ToUpper();
						oraCmd3.Parameters.Add(v_user);

						OracleParameter v_tname = new OracleParameter(":v_tname", OracleDbType.Varchar2, 32);
						v_tname.Value = tname.ToUpper();
						oraCmd3.Parameters.Add(v_tname);
					}

					dataReader = oraCmd3.ExecuteReader();

					if (dataReader.Read()) // 1건이 보장되므로 while 대신에 if 가능
					{
						// 필드 데이타 읽기
						user = dataReader["TABLE_OWNER"] as string;
						tname = dataReader["TABLE_NAME"] as string;

						if (null != dataReader["DB_LINK"] as string)
						{
							//MessageBox.Show("[Not Implemented - DB_Link");
							return; // 미구현
						}

						goto RETRY_CASE_1;
					}
					else
					{
						var _ParentForm = (Form)Thread.GetData(Thread.GetNamedDataSlot("ParentForm"));

						// 1안 - 범용적일듯
						using (new CenterWinDialog(_ParentForm))
						{
							MessageBox.Show("Object \"" + selectedText + "\" does not exist");
						}

						// 2안 - MessageBoxEx 에 국한됨.
						//MessageBoxEx.Show(_ParentForm, "\"" + selectedText + "\" does not exist");

						return; // 해당 객체가 없는 것임
					}
				}

				//if ( object_type == "VIEW" )
				//{
				//	return; // 미구현
				//}
				//else
				{
					// 아니면 테이블이라고 가정하자
					oraCmd4.Parameters.Clear();
					oraCmd4.CommandText =
@"SELECT "
+ "       COL.COLUMN_NAME \"NAME\", \n"
+ "       CASE  \n"
+ "        WHEN DATA_SCALE = 0 THEN DATA_TYPE||'('||DATA_PRECISION||')' \n"
+ "        WHEN DATA_SCALE != 0 AND DATA_TYPE IN ( 'NUMBER' ) THEN DATA_TYPE||'('||DATA_PRECISION||','||DATA_SCALE||')' \n"
+ "        WHEN DATA_TYPE IN ( 'FLOAT' ) THEN DATA_TYPE||'('||DATA_PRECISION||')' \n"
+ "        WHEN DATA_TYPE IN ( 'TIMESTAMP' ) THEN DATA_TYPE||'('||DATA_SCALE||')' \n"
+ "        WHEN DATA_TYPE IN ( 'CHAR','VARCHAR2', 'NCHAR', 'NVARCHAR2', 'RAW' ) THEN DATA_TYPE||'('||DECODE(CHAR_USED, 'C', CHAR_LENGTH, DATA_LENGTH)||')' \n"
+ "        --WHEN DATA_TYPE IN ( 'XMLTYPE','ANYDATA', 'ROWID', 'BLOB', 'CLOB', 'BINARY_DOUBLE', 'BINARY_FLOAT', 'LONG', 'ROWID' ) THEN DATA_TYPE \n"
+ "        ELSE DATA_TYPE \n"
+ "       END \"Type\", \n"
+ "       DECODE(COL.NULLABLE, 'N', 'Not Null', NULL) \"Nullable\", \n"
+ "       DATA_DEFAULT \"Default\", \n"
+ "       (SELECT COMMENTS \n"
+ "          FROM SYS.ALL_COL_COMMENTS CM \n"
+ "         WHERE 1 = :v_one \n"
+ "           AND COL.OWNER = CM.OWNER \n"
+ "           AND COL.TABLE_NAME = CM.TABLE_NAME \n"
+ "           AND COL.COLUMN_NAME = CM.COLUMN_NAME) \"Comment\" \n"
+ "  FROM SYS.ALL_TAB_COLUMNS COL \n"
+ " WHERE COL.OWNER = :v_user \n"
+ "   AND COL.TABLE_NAME = :v_tname \n"
+ " ORDER BY COLUMN_ID  \n"
;

					{
						OracleParameter v_one = new OracleParameter(":v_one", OracleDbType.Int32);
						v_one.Value = (configure.checked_showComment) ? 1 : 0;
						oraCmd4.Parameters.Add(v_one);

						OracleParameter v_user = new OracleParameter(":v_user", OracleDbType.Varchar2, 32);
						v_user.Value = user.ToUpper();
						oraCmd4.Parameters.Add(v_user);

						OracleParameter v_tname = new OracleParameter(":v_tname", OracleDbType.Varchar2, 32);
						v_tname.Value = tname.ToUpper();
						oraCmd4.Parameters.Add(v_tname);
					}

					//dataReader = cmd.ExecuteReader();
					OracleDataAdapter adapter = new OracleDataAdapter();
					adapter.SelectCommand = oraCmd4;

					DataTable dataTable = new DataTable();
					adapter.Fill(dataTable);
					dataGridView.DataSource = dataTable; // 화면에 연결
					this.Text = user + "." + tname;
				}
			}
			catch ( Exception ex )
			{
				MessageBox.Show(ex.ToString());
			}
			finally
			{
				if (dataReader != null)
				{
					//dataReader.Close();
					//dataReader.Dispose();
					dataReader = null; // null 처리시, GC가 처리대상이 됨
				}

			}
		}

		private void SetDataTable_(string selectedText, AltibaseConnection conn)
		{
			return;
		}

		private void SetDataTable_(string selectedText, SqlConnection conn)
		{
			return;
		}
		#endregion


		private void cbInsertComma_CheckStateChanged(object sender, EventArgs e)
		{
			configure.checked_insertComma = cbInsertComma.Checked;
		}

		private void cbLowerCase_CheckStateChanged(object sender, EventArgs e)
		{
			configure.checked_lowerCase = cbLowerCase.Checked;
		}

		private void cbShowPK_CheckStateChanged(object sender, EventArgs e)
		{
			configure.checked_showPK = cbShowPK.Checked;
		}

		private void cbShowComment_CheckStateChanged(object sender, EventArgs e)
		{
			configure.checked_showComment = cbShowComment.Checked;
			SetDataTable(configure.selectedText, configure.conn);
		}

		private void cbInsertComma_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void cbLowerCase_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void cbShowPK_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void cbShowComment_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void frmColumnInfo_FormClosing(object sender, FormClosingEventArgs e)
		{
			configure.LocationPre = this.Location;
			configure.HeightPre = this.Height;
			configure.WidthPre = this.Width;
		}

	}
	
}


/*
 * 
 * ## ORACLE - COLUMN_TYPE
 * 
SQL> select data_type, count(*) from sys.dba_tab_columns group by data_type having count(*) >= 20 order by 1;

DATA_TYPE                                  COUNT(*)
---------------------------------------- ----------
ANYDATA                                          72
AQ$_SIG_PROP                                     20
BINARY_DOUBLE                                    41
BLOB                                            122
CHAR                                            927
CLOB                                            862
DATE                                           3707
INTERVAL DAY(3) TO SECOND(0)                     72
KU$_DEFERRED_STG_T                               36
KU$_SCHEMAOBJ_T                                 289
KU$_STORAGE_T                                    38
LONG                                            260
NUMBER                                        50849
NVARCHAR2                                       100
RAW                                            1910
ROWID                                            83
STRINGLIST                                       22
TIMESTAMP(3)                                    125
TIMESTAMP(3) WITH TIME ZONE                      97
TIMESTAMP(6)                                    749
TIMESTAMP(6) WITH LOCAL TIME ZONE                20
TIMESTAMP(6) WITH TIME ZONE                     555
TIMESTAMP(9)                                     34
TIMESTAMP(9) WITH TIME ZONE                     100
VARCHAR2                                      47385
XMLTYPE                                         135

26 rows selected.

 * 
 */
