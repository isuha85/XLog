using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

using FastColoredTextBoxNS;

// https://qiita.com/motuo/items/5ffe1134d99ddf2e7b2d
//using PoorMansTSqlFormatterLib.Tokenizers;
//using PoorMansTSqlFormatterRedux.Tokenizers;		// .Net Core 호환 버전
//using PoorMansTSqlFormatterRedux.Parsers;
//using PoorMansTSqlFormatterRedux.Formatters;

using System.Diagnostics;                   // 특정시간 응답대기
using System.Configuration;

using System.Data.OleDb;                    // Any DB - oracle / SQL Server / tibero / ..
using Tibero.DbAccess;                      // tibero
using System.Data.SqlClient;                // SQL Server
using Altibase.Data.AltibaseClient;         // altibase

//using Oracle.DataAccessxx;                // Unmanaged 드라이버
using Oracle.ManagedDataAccess.Client;      // Managed 드라이버 (32/64 bit에 무방), deprecated - using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Types;

using System.Data.Common;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;


/*
 * #### SQL Server vs ORACLE | TIBERO | ALTIBASE
 * 
 * SqlConnection	    | OleDbConnection	    -> OracleConnection     | OleDbConnectionTbr	| AltibaseConnection
 * SqlCommand   	    | OleDbCommand    	    -> OracleCommand        | OleDbCommandTbr		| AltibaseCommand   
 * SqlCommandBuilder    | OleDbCommandBuilder   -> OracleCommandBuilder | OleDbCommandBuilderTbr|
 * SqlParameter         | OleDbParameter        -> OracleParameter      | OleDbParameterTbr     | AltibaseParameter
 * SqlDataAdapter       | OleDbDataAdapter      -> OracleDataAdapter    | OleDbDataAdapterTbr   | AltibaseDataAdapter
 * SqlDataReader	    | OleDbDataReader	    -> OracleDataReader     | OleDbDataReader		| AltibaseDataReader
 * SqlTransaction       | OleDbTransaction      -> OracleTransaction    | OleDbTransaction      | AltibaseTransaction
 * SqlDbType    	    | OleDbType			    -> OracleDbType         | OleDbTypeTbr          |                           
 * SqlException 	    |     				    -> OracleException      | Exception      
 *                      
 * [OracleLob]
 * ->
 * OleDbTypeTbr.LongVarChar (CLOB대응) 
 * OleDbTypeTbr.LongVarBinary (BLOB대응) 
 * 
 * [사용방법] reader에서 인은 후 GetString으로 인어들임 
 *       ex) if (reader.Read()) { string clob = reader.GetString(0); .. }
 * 
 * 출처: https://yaraba.tistory.com/346 
 * DOS> C:\WINDOWS\system32 > regsvr32 tbprov6.dll ( 파일이 N개임, Tibero 설치 pdf 참조할 것 
 */

namespace XLog
{
    public partial class SQLToolControl : UserControl
    {
        //private string connStr = "Data Source=192.168.56.201:1521/xe;User ID=US_GDMON;Password=US_GDMON";
        //private OracleConnection conn = null;
        //private OracleDataAdapter adapter = null;
        private DbConnection conn = null;                       // Abstract 클래스 (System.Data.Common)
        private DbDataAdapter adapter = null;
        XDb xDb = null;

        private bool bStop = false;

		//private int FETCH_SIZE = 1;
		//private int FETCH_SIZE = 2;		// [TIBERO] {"TBR-02040 (24000): Invalid cursor state."}
		//private int FETCH_SIZE = 10;
		private int FETCH_SIZE = 100;       // [TIBERO] {"보호된 메모리를 읽거나 쓰려고 했습니다. 대부분 이러한 경우는 다른 메모리가 손상되었음을 나타냅니다."}
		//private int FETCH_SIZE = 1000;

		private int JUMP_TO_ROW = 20;

        public SQLToolControl()
        {
            InitializeComponent();
        }

		private void SQLToolUserControl_Load(object sender, EventArgs e)
        {
            int margin_w = flowLayoutPanel1.Width;
            int w = this.Width - margin_w;
            int h = this.Height / 2;

            splitContainer1.Panel1.Size = new Size(w, h);
            splitContainer1.Panel2.Size = new Size(w, h);

            splitContainer1.Dock = DockStyle.Fill;
            tabControl1.Dock = DockStyle.Fill;

			// FCTB
			{
				tb.Dock = DockStyle.Fill;
				tb.Language = Language.SQL;
				tb.AcceptsTab = true;
				//fctb.WordWrap = false;
				//fctb.ReadOnly = true;
				//fctb.ShortcutsEnabled = true;
				//fctb.IndentBackColor = Color.Gray;
				tb.SelectionHighlightingForLineBreaksEnabled = true;

				tb.Text = @"select level from dual connect by level <= 1001";
				tb.Text = @"with x(c1) as ( select 1 c1 union all select c1 + 1 from x where c1 + 1 <= 1001 ) select c1, data from x, tb_clob option (maxrecursion 0)";
				tb.Text = @"with x as ( select level c1 from dual connect by level <= 1001) select c1, data from x, tb_clob";
				tb.Text = @"select c1, data from ( select level c1 from dual connect by level <= 1001) x, tb_clob";

				string myStr = null;
				myStr = "" +
					" -- 1;\r\n" +
					//" // 1; \r\n" +
					"select /* ; */ 1 from dual" +
					";"
					;
				myStr += "\r\nselect 2 from dual;  ";
				myStr += "\n\nselect 3 from dual;  ";
				//myStr = "select 1 from dual;";
				tb.Text = myStr;
			}

			// DataGridView 레코드 색상 번갈아서 바꾸기
			{
				dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
				dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine;
			}

			// TabControl
			{
				tabControl1.SelectedIndex = 2;
			}

			// toolStripProgressBar1
			{
                toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
                //toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = 100;
                toolStripProgressBar1.Step = 1;
                toolStripProgressBar1.Value = 0;

                toolStripStatusLabel4.Text = "";
            }
        } // Load

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			Keys key = keyData & ~(Keys.Alt | Keys.Shift | Keys.Control);

			switch (key)
			{
				//case Keys.M:		// 자바코드로 변환. 단축키실행 후 붙여넣기 
				//case Keys.Insert:	// 조회된 로우 복사 시 컬럼ID도 같이 복사
				//case Keys.F2:		// SQL 창 전체화면 전환 , Shift+F2 : Grid Output창 전체화면 전환
				//case Keys.F3:		// Find Next / Prev
				//case Keys.F4:		// 또는 Ctrl + Click
				//case Keys.F5:		// 모든 SQL문 실행
				//case Keys.F6:		// SQL 창간의 이동 (O) , 커서를 Editor와 Results 패널 사이로 전환 (T)
				//case Keys.F7:		// Clear
				//case Keys.F8:		// History, Alt+Up : History UP / Alt+Down : History DOWN
				//case Keys.F9:		// SQL Validate
				//case Keys.F10:		// Popup Menu
				case Keys.E:
					if ((keyData & Keys.Alt) != 0) break;
					if (((keyData & Keys.Control) != 0) && ((keyData & Keys.Shift) != 0))
					{
						// PLAN ON
						//doPlan(tb.SelectedText);
						return true;
					}
					else if ((keyData & Keys.Control) != 0)
					{
						// PLAN ONLY
						//doPlanOnly(tb.SelectedText);
						return true;
					}
					break;

				case Keys.L:        // Toad: 소문자 전환 단축키 
					if (((keyData & Keys.Alt) != 0) || ((keyData & Keys.Shift) != 0)) break;
					if ((keyData & Keys.Control) != 0)
					{
						SendKeys.SendWait("(^+U)"); // FCTB 자체구현 호출
						return true;
					}
					break;


				case Keys.R:		// Toad
					if (((keyData & Keys.Alt) != 0) || ((keyData & Keys.Shift) != 0)) break;
					if ((keyData & Keys.Control) != 0)
					{
						SendKeys.SendWait("(^H)"); // FCTB 자체구현 호출
						return true;
					}
					break;

				case Keys.B:		// Toad
				case Keys.OemMinus:	// Orange
					if (((keyData & Keys.Alt) != 0) || ((keyData & Keys.Shift) != 0)) break;
					
					tb.CommentSelected("--"); // FCTB가 토글 형태로 동작한다. 자체구현할지 말지. 생각해보자.
					break;
				case Keys.F:
					if ((keyData & Keys.Alt) != 0) break;
					if ( ((keyData & Keys.Control) != 0) && ((keyData & Keys.Shift) != 0) )
					{
						// TODO: 특정 선택 영역만 "포맷팅" 하는 기능 필요
						var code = tb.Text;

						// TODO: 공통설정화면으로 처리.
						var _formatterOptions = new PoorMansTSqlFormatterRedux.Formatters.TSqlStandardFormatterOptions
						{
							KeywordStandardization = true,
							IndentString = "\t",
							SpacesPerTab = 4,
							MaxLineWidth = 999,
							NewStatementLineBreaks = 2,
							NewClauseLineBreaks = 1,
							TrailingCommas = true,
							SpaceAfterExpandedComma = false,
							ExpandBetweenConditions = true,
							ExpandBooleanExpressions = true,
							ExpandCaseStatements = true,
							ExpandCommaLists = true,
							BreakJoinOnSections = false,
							UppercaseKeywords = true,
							ExpandInLists = true
						};

						//var tokenizer = new PoorMansTSqlFormatterLib.Tokenizers.TSqlStandardTokenizer();
						var tokenizer = new PoorMansTSqlFormatterRedux.Tokenizers.TSqlStandardTokenizer();
						var parser = new PoorMansTSqlFormatterRedux.Parsers.TSqlStandardParser();
						var formatter = new PoorMansTSqlFormatterRedux.Formatters.TSqlStandardFormatter(_formatterOptions);

						var tokenizedSQL = tokenizer.TokenizeSQL(code);
						var parsedSQL = parser.ParseSQL(tokenizedSQL);
						tb.Text = formatter.FormatSQLTree(parsedSQL);

						return true;
					}
					break;
				case Keys.Enter:    // Toad
				case Keys.K:		// Orange
					if (((keyData & Keys.Shift) != 0) || ((keyData & Keys.Alt) != 0)) break;
					if ((keyData & Keys.Control) != 0)
					{
						if (tb.SelectedText.Length > 0)
						{
							doResult(tb.SelectedText);
							return true;
						}

						var point = tb.PositionToPoint(tb.SelectionStart);
						var line = tb.YtoLineIndex(point.Y);

						char[] tirmChars = { '\r', '\n', '\t', ' ' };
						var sql = GetSqlFromLine(tb.Text, line).Trim(tirmChars);
						doResult(sql);

						// highlight
						{
							//TODO: GetRanges 에서 정규식으로만 검색이된다. 평문검색 옵션을 못찾아서, 임시처리
							var regexPattern = sql.Replace("*", "\\*").Replace("+", "\\+");
							Range[] ranges = tb.VisibleRange.GetRanges(regexPattern).ToArray();
							//Range[] ranges = tb.GetRanges(sql, RegexOptions.None).ToArray();

							//if (ranges.Length == 1) // TODO: 동일 SQL이 모두 선택됨 ( BUGBUG )
							{
								foreach (var r in ranges)
								{
									r.SetStyle(runSqlStyle);
								}
							}
						}

						return true;
					}
					break;
				case Keys.C:
					if (((keyData & Keys.Shift) != 0) || ((keyData & Keys.Control) != 0)) break;
					if ((keyData & Keys.Alt) != 0)
					{
						tb.VisibleRange.ClearStyle(runSqlStyle); // reset highlight

						string selectedText = null;
						var form = new frmColumnInfo();

						if (tb.SelectionLength != 0)
						{
							selectedText = tb.SelectedText;
						}
						else
						{
							char[] anyOf = { ' ', '\t', '\r', '\n', ';', '"', '\'' };
							int pos_start = -1;
							int pos_end = -1;

							pos_end = tb.Text.IndexOfAny(anyOf, tb.SelectionStart);
							if (pos_end == -1)
							{
								pos_end = tb.Text.Length;
							}

							pos_start = tb.Text.LastIndexOfAny(anyOf,
								(tb.SelectionStart > 0) ? tb.SelectionStart - 1 : 0
								);
							//if (pos_start == -1) pos_start = -1;	// 의미상으로는 필요한 코드이다.

							selectedText = tb.Text.Substring(pos_start + 1, pos_end - pos_start - 1);
						}

						if (selectedText.Length != 0)
						{
							//MessageBox.Show("[" + selectedText + "]");

							// [NOTE] 오류처리 편의상, 호출자를 알기위하여, Thread Local Storage (TLS) 변수를 사용했다
							Thread.SetData(Thread.GetNamedDataSlot("ParentForm"), this.ParentForm); // TLS 변수 내이밍을 _XX 로 할까?

							form.ShowMain(selectedText, conn);
						}

						return true;
					}
					break;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}
		
		#region Common Function for SQLTool

		/*
		 * 1. Trim
		 * 2. 사용자 SQL을 ';' 기준으로 분리
		 */
		private string GetSqlFromLine(string code_, int line_)
		{
			char semicolon = ';';
			int semicolon_pos = -1;

			char single_quotes = '\'';
			char[] comment_line_start = { '-', '-' };
			char[] comment_line_end = { '\r', '\n' };
			char[] comment_block_start = { '/', '*' };
			char[] comment_block_end = { '*', '/' };
			int single_quotes_start_pos = -1;
			int comment_line_start_pos = -1;
			int comment_block_start_pos = -1;

			string code = code_.Trim() + ";"; // 마지막 ';' 이 없는 경우에 대응
			int len = code.Length;

			semicolon_pos = code.IndexOf(";");
			// ';' 이 없으면, 하나의 SQL 문장이다.
			if (semicolon_pos == -1)
			{
				return code;
			}

			// 맨마지막에만 ';' 이면, 제거하고 바로 리턴.
			if (semicolon_pos == len - 1)
			{
				return code.Substring(0, len - 1);
			}

			// 주석, 문자열에 속하지 않은 최초의 ';'을 어떻게 찾을 것인가?
			// 범위가 len 대신 len - 1 인 것은, 2Byte 구분자 처리 코드를 간단하게 함.
			int pos;
			int pos_start = 0;
			int line = 0;
			for (pos = 0; pos < len - 1; pos++)
			{
				if (code[pos] == '\n')
				{
					line++;
				}

				// @ single_quotes
				if ((single_quotes_start_pos == -1) &&
					(code[pos] == single_quotes))
				{
					single_quotes_start_pos = pos;
					continue;
				}

				if (single_quotes_start_pos != -1) // 구간시작
				{
					if (code[pos] == single_quotes)
					{
						single_quotes_start_pos = -1;
					}

					// 구간내의 다른 문자는 무시
					continue;
				}

				// @ -- 주석
				if ((comment_line_start_pos == -1) &&
					(code[pos] == comment_line_start[0] && code[pos + 1] == comment_line_start[1]))
				{
					comment_line_start_pos = pos;
					continue;
				}

				if (comment_line_start_pos != -1) // 구간시작
				{
					if (code[pos] == comment_line_end[1])
					{
						comment_line_start_pos = -1;
					}

					// 구간내의 다른 문자는 무시
					continue;
				}

				// @ /* .. */ 주석
				if ((comment_block_start_pos == -1) &&
					(code[pos] == comment_block_start[0] && code[pos + 1] == comment_block_start[1]))
				{
					comment_block_start_pos = pos;
					continue;
				}

				if (comment_block_start_pos != -1) // 구간시작
				{
					if (code[pos] == comment_block_end[0] && code[pos + 1] == comment_block_end[1])
					{
						comment_block_start_pos = -1;
					}

					// 구간내의 다른 문자는 무시
					continue;
				}

				if (code[pos] == semicolon)
				{
					if ( line_ > line )
					{
						pos_start = pos + 1;
						continue;
					}

					return code.Substring(pos_start, pos - pos_start);
				}

			} // for (int pos = 0; pos < len; pos++)

			// 맨마지막에, ';' 이 있는 경우.
			if (code[pos] == semicolon)
			{
				return code.Substring(pos_start, pos - pos_start);
			}

			return code;
		}
				
		#endregion

		private void doResult(string sql_)
		{
			PUBLIC.TIME_CHECK(System.DateTime.Now.Ticks); // 기준시간 등록

			lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";
			//eTick = System.DateTime.Now.Ticks;
			//dTickSec = (double)(eTick - sTick) / 10000000.0F;
			//lbTime.Text = dTickSec + " sec";

			//this.Cursor = Cursors.WaitCursor;
			toolStripStatusLabel4.Text = "Running ..";
			lbRow.Text = "0 rows";

			// 출처: https://and0329.tistory.com/entry/C-과-오라클-데이터베이스-연동-방법 
			try
			{
				dataGridView.DataSource = null;     // (1) 결과창을 비워준다.
				tabControl1.SelectedIndex = 2;      // (2) 결과탭을 보여준디.
				Application.DoEvents();

				//adapter.SelectCommand = new OracleCommand(rtbSqlEdit.Text, (OracleConnection)conn);
				adapter.SelectCommand = xDb.XDbCommand(sql_, conn);

				DataTable dt = new DataTable();     // TODO: (BUGBUG) DataTable을 재사용하면, 컬럼명이 추가된다.
				DataTable dt2 = new DataTable();    // 처음 결과 셋을 일부만 저장하여 보여주는 Fake 코드 (출력성능이슈)

				//adapter.Fill(dt);
				//adapter.Fill(0, 10, dt);
				int sPos = 0;
				int rc;

				lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";

				//adapter.MissingMappingAction = MissingMappingAction.Error;		// {"TableMapping.DataSetTable=''인 TableMapping이 없습니다."}
				//adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
				while ((rc = adapter.Fill(sPos, FETCH_SIZE, dt)) > 0)
				{
					//sPos += FETCH_SIZE;
					sPos += rc;

					if (toolStripProgressBar1.Value <= toolStripProgressBar1.Maximum * 0.9)
					{
						toolStripProgressBar1.PerformStep();
					}

					//System.Threading.Thread.Sleep(100);

					//if ( (FETCH_SIZE > 1 && sPos == FETCH_SIZE) || (sPos == 100) )
					if (sPos == FETCH_SIZE)
					{
						// [NOTE] 대량 데이타 출력 성능을 위한 더미 코드.
						dt2 = dt.Copy();
						dataGridView.DataSource = XDb.ConvertDataTable(dt2);
					}

					lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";
					lbRow.Text = sPos + "+" + " rows";
					Application.DoEvents();         //TODO: [NOTE] 필수적임, DoEvents 삽입

					if (bStop)
					{
						break;
					}
				} // while

				//lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TICK_DELTA + ")";
				lbTime.Text = PUBLIC.TIME_CHECK() + " sec";
				lbRow.Text = sPos + " rows";

				// TODO: RAW 타입에 대해서 DataGridView 에서 Error
				dataGridView.DataSource = XDb.ConvertDataTable(dt);
			}
			catch (Exception ex)
			{
				MessageBoxEx.Show(this.ParentForm, ex.Message);
			}

			toolStripProgressBar1.Value = 0;

			if (bStop)
			{
				bStop = false;
				toolStripStatusLabel4.Text = "Stop";
			}
			else
			{
				toolStripStatusLabel4.Text = "Done";
			}
			//this.Cursor = Cursors.Default;
		} // doGo

		private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
				// [TIP] Win32에서 횐경변수는, 기본적으로 각각의 DLL단위로 다르게 설정된다.
				//System.Environment.SetEnvironmentVariable("ALTIBASE_PORT_NO", "20300", EnvironmentVariableTarget.Process);

				if (conn != null)
				{
					MessageBox.Show("(W) Already Connected.");
					return;
				}

				if (xDb == null)
                {
					XDbConnType sXDBConnType = XDbConnType.OLEDB;
					sXDBConnType = XDbConnType.TIBERO;				// LOB 조회 불가
					sXDBConnType = XDbConnType.ALTIBASE;            // LOB 조회시, BeginTransaction 를 호출해야하는 문제.
					sXDBConnType = XDbConnType.ORACLE;
					//sXDBConnType = XDbConnType.MSSQL;

					xDb = new XDb(sXDBConnType);
					switch (sXDBConnType)
					{
						case XDbConnType.ORACLE:
							xDb.mConnStr = "Data Source=192.168.56.201:1521/xe;User ID=US_GDMON;Password=US_GDMON"; // OK
							break;
						case XDbConnType.ALTIBASE:
							// (1) if Autocommit then {"LobLocator cannot span the transaction 332417."}
							xDb.mConnStr = "DSN=192.168.56.201;uid=sys;pwd=manager;NLS_USE=MS949;PORT=20300"; // OK
							break;
						case XDbConnType.TIBERO:
							// (2) {"TBR-02040 (24000): Invalid cursor state."}
							xDb.mConnStr = "Provider=tbprov.Tbprov.5;Data Source=tibero;User ID=sys;Password=tibero;";
							FETCH_SIZE = 1;
							break;
						case XDbConnType.MSSQL:
							xDb.mConnStr = "Server=192.168.56.201;Database=master;Uid=sa;Pwd=password12!";
							break;
						case XDbConnType.OLEDB:
							// (1) ALTIBASE의 경우 CLOB 조회 불가
							xDb.mConnStr = "Provider=tbprov.Tbprov;Data Source=tibero;User ID=sys;Password=tibero;"; 
							//xDb.mConnStr = "Provider=Altibase.OLEDB;Data Source=192.168.56.201;User ID=sys;Password=manager;Extended Properties='PORT=20300'"; // OK, But CLOB BUGBUG
							break;
						default:
							MessageBox.Show("[NEVER] " + sXDBConnType);
							return; // TODO: C# 강제종료는 어떻게? 
							break;
					}
					toolStripStatusLabel4.Text = "Connecting.. " + xDb.mConnType;
                    Application.DoEvents();
                }

                //conn = new OracleConnection(connStr);
                conn = xDb.XDbConnection();
                conn.Open();

                //adapter = new OracleDataAdapter();
                adapter = xDb.XDbDataAdapter();

				toolStripStatusLabel4.Text = "Connected " + xDb.mConnType;
				Application.DoEvents();
			}
			catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conn = null;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                //dgvGridResult.Dispose();
                adapter.Dispose();
                conn.Close();

                adapter = null;
                conn = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
			doResult(tb.Text);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            bStop = true;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int jumpToRow = dataGridView.Rows.Count - 1 - 1;

            dataGridView.FirstDisplayedScrollingRowIndex = jumpToRow;
            dataGridView.Rows[jumpToRow].Selected = true;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            int jumpToRow = 0;

            dataGridView.FirstDisplayedScrollingRowIndex = jumpToRow;
            dataGridView.Rows[jumpToRow].Selected = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int jumpToRow = dataGridView.FirstDisplayedScrollingRowIndex + JUMP_TO_ROW;

            if (jumpToRow > dataGridView.Rows.Count - 1)
            {
                jumpToRow = dataGridView.Rows.Count - 1;
            }

            dataGridView.FirstDisplayedScrollingRowIndex = jumpToRow;
            dataGridView.Rows[jumpToRow].Selected = true;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            int jumpToRow = dataGridView.FirstDisplayedScrollingRowIndex - JUMP_TO_ROW;

            if (jumpToRow < 0)
            {
                jumpToRow = 0;
            }

            dataGridView.FirstDisplayedScrollingRowIndex = jumpToRow;
            dataGridView.Rows[jumpToRow].Selected = true;
        }

        static int btnOpt_Click_Cnt = 0;
        private void btnOpt_Click(object sender, EventArgs e)
        {
            btnOpt_Click_Cnt++;

            if (btnOpt_Click_Cnt % 2 != 0)
            {
                if (btnOpt_Click_Cnt == 1)
                {
                    dgvTmp.DefaultCellStyle.ForeColor = dataGridView.DefaultCellStyle.ForeColor;
                    dgvTmp.RowsDefaultCellStyle.BackColor = dataGridView.RowsDefaultCellStyle.BackColor;
                    dgvTmp.GridColor = dataGridView.GridColor;
                    dgvTmp.BorderStyle = dataGridView.BorderStyle;
                }

                dataGridView.DefaultCellStyle.ForeColor = Color.Coral;
                dataGridView.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
                dataGridView.GridColor = Color.Blue;
                dataGridView.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                dataGridView.DefaultCellStyle.ForeColor = dgvTmp.DefaultCellStyle.ForeColor;
                dataGridView.RowsDefaultCellStyle.BackColor = dgvTmp.RowsDefaultCellStyle.BackColor;
                dataGridView.GridColor = dgvTmp.GridColor;
                dataGridView.BorderStyle = dgvTmp.BorderStyle;
            }
        } // btnOpt


		private Style sameWordsStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(50, Color.Gray)));
		private Style runSqlStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(50, Color.Blue)));
		DateTime lastNavigatedDateTime = DateTime.Now;

		private void tb_SelectionChangedDelayed(object sender, EventArgs e)
		{
			// TODO: 라이센스확인요, FCTB의 테스트 샘플 코드 사용함 - PowerfulCSharpEditor.cs 
			var tb = sender as FastColoredTextBox;
			//remember last visit time
			if (tb.Selection.IsEmpty && tb.Selection.Start.iLine < tb.LinesCount)
			{
				if (lastNavigatedDateTime != tb[tb.Selection.Start.iLine].LastVisit)
				{
					tb[tb.Selection.Start.iLine].LastVisit = DateTime.Now;
					lastNavigatedDateTime = tb[tb.Selection.Start.iLine].LastVisit;
				}
			}

			//highlight same words
			tb.VisibleRange.ClearStyle(sameWordsStyle);
			tb.VisibleRange.ClearStyle(runSqlStyle);

			if (!tb.Selection.IsEmpty)
				return;//user selected diapason

			//get fragment around caret
			var fragment = tb.Selection.GetFragment(@"\w");
			string text = fragment.Text;
			if (text.Length == 0)
				return;

			//highlight same words
			Range[] ranges = tb.VisibleRange.GetRanges("\\b" + text + "\\b").ToArray();

			if (ranges.Length > 1)
				foreach (var r in ranges)
					r.SetStyle(sameWordsStyle);
		}
		
		private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			dataGridView_DataError_(sender, e);
		}
		[System.Diagnostics.Conditional("DEBUG")]
		private void dataGridView_DataError_(object sender, DataGridViewDataErrorEventArgs e)
		{
			MessageBoxEx.Show(this.ParentForm, e.ToString());
			Debug.Assert(false);
		}

	} // class SQLToolUserControl
}
