using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using System.Runtime.InteropServices;       // https://hot-key.tistory.com/4?category=1010793
using System.Data;                          // 공통 인터페이스 , IDbConnection .. 등
using System.Data.Common;                   // 추상 클래스 , DbConnection .. 등

using System.Diagnostics;                   // 특정시간 응답대기, Debug.Assert, Debug.WriteLine, ..
using System.Configuration;

using System.Text.RegularExpressions;
using Sprache;

using System.Data.OleDb;                    // Any DB - oracle / SQL Server / tibero / ..
using Tibero.DbAccess;                      // tibero
using System.Data.SqlClient;                // SQL Server
using Altibase.Data.AltibaseClient;         // altibase

//using Oracle.DataAccessxx;                // Unmanaged 드라이버
using Oracle.ManagedDataAccess.Client;      // Managed 드라이버 (32/64 bit에 무방), deprecated - using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Types;

namespace XLog
{
	public partial class frmColumnInfo : Form
	{
		// TODO: 휘발성으로, 재구동이후에는 유지되지 않는다. 설정파일 개념이 요구됨
		public static bool checked_insertComma { get; set; } = false;
		public static bool checked_lowerCase { get; set; } = false;
		public static bool checked_showPK { get; set; } = false;
		public static bool checked_showComment { get; set; } = false;

		public static Point preLocation { get; set; } = new Point(-1,-1);
		public static int preHeight { get; set; } = -1;
		public static int preWidth { get; set; } = -1;

		//private DataTable datatable;

		public frmColumnInfo()
		{
			InitializeComponent();

			//cbShowComment.Visible = false;
			cbShowPK.Visible = false;

			dataGrdiView.BackgroundColor = Color.White;

			cbInsertComma.Checked = checked_insertComma;
			cbLowerCase.Checked = checked_lowerCase;
			cbShowComment.Checked = checked_showComment;

			if (preLocation.X == -1)
			{
				//form.StartPosition = FormStartPosition.CenterParent;
				StartPosition = FormStartPosition.CenterScreen;
				//frmColumnInfo.preLocation = form.Location;
			}
			else
			{
				StartPosition = FormStartPosition.Manual;
				Location = preLocation;
				Height = preHeight;
				Width = preWidth;
			}
		}

		public void ShowMain(string selectedText, DbConnection conn)
		{
			if ( conn != null )
			{
				SetDataTable(selectedText, conn);
				if ( dataGrdiView.RowCount > 0 )
				{
					Show();
				}
			}
		}

		#region SetDataTable
		public void SetDataTable(string selectedText, DbConnection conn)
		{
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

		private void SetDataTable_(string selectedText, OracleConnection conn)
		{
			string sql = null;
			string tname = null;
			string user = null;
			string object_type = null;

			OracleCommand cmd = null;
			OracleDataReader dataReader = null;

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

				cmd = new OracleCommand();
				cmd.Connection = conn;

				// TODO: 'USERNAME' 을 매번 구하는 건 낭비이다.
				if (user == null)
				{
					cmd.Parameters.Clear();
					cmd.CommandText = @"select user from dual";
					user = (string)cmd.ExecuteScalar();
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
					cmd.Parameters.Clear();
					cmd.CommandText =
		@"select decode(object_type, 'UNDEFINED', NULL, object_type) object_type " +
		"	from sys.all_objects " +
		"  where 2=2" +
		"	 and owner = :v_user " +
		"	 and object_name = :v_tname " +
		"	 and object_type <> 'SYNONYM' ";

					{
						OracleParameter v_user = new OracleParameter(":v_user", OracleDbType.Varchar2, 32);
						v_user.Value = user.ToUpper();
						cmd.Parameters.Add(v_user);

						OracleParameter v_tname = new OracleParameter(":v_tname", OracleDbType.Varchar2, 32);
						v_tname.Value = tname.ToUpper();
						cmd.Parameters.Add(v_tname);
					}

					object_type = (string)cmd.ExecuteScalar();					
				}

				// CASE-2 : 아니면 SYNONYM 이다.
				if (object_type == null)
				{
					cmd.Parameters.Clear();
					cmd.CommandText =
		@"select table_owner, table_name, db_link " +
		"   from sys.all_synonyms " +
		"  where 2=2" +
		"	 and (owner = 'PUBLIC' or owner = :v_user) " +
		"	 and synonym_name = :v_tname " +
		"  order by decode(owner,'PUBLIC',NULL,owner) nulls last"; // 기본값이 NULL이 마지막에 조회되나 명시함
					cmd.CommandText = "select * from ( " + cmd.CommandText + " ) where rownum = 1";


					{
						OracleParameter v_user = new OracleParameter(":v_user", OracleDbType.Varchar2, 32);
						v_user.Value = user.ToUpper();
						cmd.Parameters.Add(v_user);

						OracleParameter v_tname = new OracleParameter(":v_tname", OracleDbType.Varchar2, 32);
						v_tname.Value = tname.ToUpper();
						cmd.Parameters.Add(v_tname);
					}

					dataReader = cmd.ExecuteReader();

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
					cmd.Parameters.Clear();
					cmd.CommandText =
@"SELECT "
+ "       COL.COLUMN_NAME \"NAME\", \n"
+ "       CASE  \n"
+ "        WHEN DATA_SCALE = 0 THEN DATA_TYPE||'('||DATA_PRECISION||')' \n"
+ "        WHEN DATA_SCALE != 0 AND DATA_TYPE IN ( 'NUMBER' ) THEN DATA_TYPE||'('||DATA_PRECISION||','||DATA_SCALE||')' \n"
+ "        WHEN DATA_TYPE IN ( 'FLOAT' ) THEN DATA_TYPE||'('||DATA_PRECISION||')' \n"
+ "        WHEN DATA_TYPE IN ( 'TIMESTAMP' ) THEN DATA_TYPE||'('||DATA_SCALE||')' \n"
+ "        WHEN DATA_TYPE IN ( 'CHAR','VARCHAR2', 'NCHAR', 'NVARCHAR2', 'RAW' ) THEN DATA_TYPE||'('||DECODE(CHAR_USED, 'C', CHAR_LENGTH, DATA_LENGTH)||')' \n"
+ "        --WHEN DATA_TYPE IN ( 'XMLTYPE','ANYATA', 'ROWID', 'BLOB', 'CLOB', 'BINARY_DOUBLE', 'BINARY_FLOAT', 'LONG', 'ROWID' ) THEN DATA_TYPE \n"
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
						v_one.Value = (checked_showComment) ? 1 : 0;
						cmd.Parameters.Add(v_one);

						OracleParameter v_user = new OracleParameter(":v_user", OracleDbType.Varchar2, 32);
						v_user.Value = user.ToUpper();
						cmd.Parameters.Add(v_user);

						OracleParameter v_tname = new OracleParameter(":v_tname", OracleDbType.Varchar2, 32);
						v_tname.Value = tname.ToUpper();
						cmd.Parameters.Add(v_tname);
					}

					//dataReader = cmd.ExecuteReader();
					OracleDataAdapter adapter = new OracleDataAdapter();
					adapter.SelectCommand = cmd;

					DataTable dataTable = new DataTable();
					adapter.Fill(dataTable);
					dataGrdiView.DataSource = dataTable; // 화면에 연결
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
			checked_insertComma = cbInsertComma.Checked;
		}

		private void cbLowerCase_CheckStateChanged(object sender, EventArgs e)
		{
			checked_lowerCase = cbLowerCase.Checked;
		}

		private void cbShowPK_CheckStateChanged(object sender, EventArgs e)
		{
			checked_showPK = cbShowPK.Checked;
		}

		private void cbShowComment_CheckStateChanged(object sender, EventArgs e)
		{
			checked_showComment = cbShowComment.Checked;
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
			frmColumnInfo.preLocation = this.Location;
			frmColumnInfo.preHeight = this.Height;
			frmColumnInfo.preWidth = this.Width;
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
