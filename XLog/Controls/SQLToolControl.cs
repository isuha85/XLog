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

using FastColoredTextBoxNS;
using WeifenLuo.WinFormsUI.Docking;

// https://qiita.com/motuo/items/5ffe1134d99ddf2e7b2d
//using PoorMansTSqlFormatterLib.Tokenizers;
//using PoorMansTSqlFormatterRedux.Tokenizers;		// .Net Core 호환 버전
//using PoorMansTSqlFormatterRedux.Parsers;
//using PoorMansTSqlFormatterRedux.Formatters;

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
		private struct Configure
		{
			public int Panel2Size;
			public int Panel2SizePre;
			public DateTime Panel2SizeDateTime;
			public double SplitterScale;

			public DateTime bindRowsLastAddTime;
		};
		Configure configure;

		DockContent doc1 = null; // WeifenLuo.WinFormsUI.Docking.DockContent
		DockContent doc2 = null;		

		Style sameWordsStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(50, Color.Gray)));
		Style runSqlStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(50, Color.Blue)));
		DateTime lastNavigatedDateTime = DateTime.Now;


		//string connStr = "Data Source=192.168.56.201:1521/xe;User ID=US_GDMON;Password=US_GDMON";
		//OracleConnection conn = null;
		//OracleDataAdapter adapter = null;
		DbConnection conn = null;                       // Abstract 클래스 (System.Data.Common)
        DbDataAdapter adapter = null;
        XDb xDb = null;

        bool bStop = false;

		//int FETCH_SIZE = 1;
		//int FETCH_SIZE = 2;		// [TIBERO] {"TBR-02040 (24000): Invalid cursor state."}
		//int FETCH_SIZE = 10;
		int FETCH_SIZE = 100;       // [TIBERO] {"보호된 메모리를 읽거나 쓰려고 했습니다. 대부분 이러한 경우는 다른 메모리가 손상되었음을 나타냅니다."}
		//int FETCH_SIZE = 1000;

		int JUMP_TO_ROW = 20;

        public SQLToolControl()
        {
            InitializeComponent();

			//AutoScaleMode = AutoScaleMode.Inherit;
			AutoScaleMode = AutoScaleMode.Dpi;
		}

		// [NOTE] 자동생성된 Control을 'ref' 지정으로 public 으로 노출
		public ref FastColoredTextBox GetTextBox()
		{
			return ref tb;
		}

		private void SQLToolUserControl_Load(object sender, EventArgs e)
        {
			// "tabPage.Visible = false" 가 효과가 높고, DoubleBuffered 효과는 잘 모르겠지만 넣어둠
			{
				Visible = false;
				DoubleBuffered = true;

				typeof(DataGridView).InvokeMember("DoubleBuffered",
					BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
					null, tb, new object[] { true });  // fctb 을 설정해준다.
			}

			// DockPanel
			{
				this.dockPanel.Dock = DockStyle.Fill;
				this.dockPanel.DocumentStyle = DocumentStyle.DockingSdi;
				//this.dockPanel.DocumentStyle = DocumentStyle.DockingWindow;

				doc1 = new DockContent(); // WeifenLuo.WinFormsUI.Docking.DockContent
				doc2 = new DockContent();

				doc2.Text = "Bind Variables";

				doc1.Show(dockPanel, DockState.Document);
				doc1.Controls.Add(tb);
				doc1.AllowEndUserDocking = false;

				//doc2.Show(this.dockPanel, DockState.DockRightAutoHide);
				doc2.Show(dockPanel, DockState.DockRight);
				doc2.Controls.Add(dgvBind);
				//doc2.Controls.Add(comboBox);
				doc2.AllowEndUserDocking = false; // TODO: 고정하고 싶은데. 영향이 없음. (BUGBUG)
				//doc3.Show(doc1.Pane, null); // 이렇게 사용하면, TabControl 대체가 가능함
				//doc4.Show(doc1.Pane, null);

				tb.Dock = DockStyle.Fill;
				dgvBind.Dock = DockStyle.Fill;
			}

			// resize
			{
				//int margin_w = flowLayoutPanel.Width;
				//int w = this.Width - margin_w;
				//int h = configure.Panel2MinSize;
				//splitContainer.Panel1.Size = new Size(w, this.Height - h);
				//splitContainer.Panel2.Size = new Size(w, h);				

				configure.SplitterScale = 0.60;
				splitContainer.SplitterDistance = (int)(splitContainer.Height * configure.SplitterScale);
				configure.Panel2Size = splitContainer.Height - splitContainer.SplitterDistance;
				configure.Panel2SizePre = configure.Panel2Size;
				configure.Panel2SizeDateTime = DateTime.Now;

				splitContainer.Dock = DockStyle.Fill;
				tabControl.Dock = DockStyle.Fill;
			}

			// FCTB
			{
				//tb.Selection.ColumnSelectionMode = true; // 설정하지 않아도, Alt+MOUSE 로 컬럼모드가 가능하다.
				tb.Dock = DockStyle.Fill;
				tb.Language = Language.SQL;
				tb.AcceptsTab = true;
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
				myStr = "select /* :v1 , :v9 */ sysdate from dual where '1' = :v1 and 1 = :v2;";
				myStr += "\n";
				tb.Text = myStr;

				Visible = true;
			}

			// 나머지 출력용 FCTB
			{
				List<FastColoredTextBox> TBs = new List<FastColoredTextBox>();
				TBs.Add(tbPlan);
				TBs.Add(tbServerOutput);
				TBs.Add(tbTextOutput);

				foreach (FastColoredTextBox TB in TBs)
				{
					TB.Dock = DockStyle.Fill;
					TB.Language = Language.SQL;
					TB.ReadOnly = true;
					TB.Text = "";
					TB.ShowLineNumbers = false;
				}

			}

			// dgvBind
			{
				dgvBind.Dock = DockStyle.Fill;
				dgvBind.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
				dgvBind.AutoGenerateColumns = false;

				//doc2.Width = (int)(splitContainer.Panel1.Width * 0.4); // 효과없음.
				dockPanel.DockRightPortion = 0.3; // def: 0.25
				doc2.Hide();
#if DEBUG
				//ShowBindList(); // 테스트 편의상
#endif
			}

			// DataGridView 레코드 색상 번갈아서 바꾸기
			{
				dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
				dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine;
			}

			// TabControl
			{
				tabControl.SelectedIndex = 2;
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

			flowLayoutPanel.Visible = false;
		} // Load

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			Keys key = keyData & ~(Keys.Alt | Keys.Shift | Keys.Control);

			switch (key)
			{
				//case Keys.Escape:	// 열린 Popup 창 닫기
				//case Keys.M:		// 자바코드로 변환. 단축키실행 후 붙여넣기 
				//case Keys.Insert:	// 조회된 로우 복사 시 컬럼ID도 같이 복사
				//case Keys.F3:		// Find Next / Prev
				//case Keys.F4:		// 또는 Ctrl + Click
				//case Keys.F5:		// 모든 SQL문 실행
				//case Keys.F6:		// SQL 창간의 이동 (O) , 커서를 Editor와 Results 패널 사이로 전환 (T)
				//case Keys.F7:		// Clear
				//case Keys.F8:		// History, Alt+Up : History UP / Alt+Down : History DOWN
				//case Keys.F9:		// SQL Validate
				//case Keys.F10:	// Popup Menu

				case Keys.N:        // Toad
					if ((keyData & Keys.Alt) != 0) break;
					if ((keyData & Keys.Control) != 0)
					{
						if ((keyData & Keys.Shift) != 0)
						{
							flowLayoutPanel.Visible = (flowLayoutPanel.Visible) ? false : true;
							return true;
						}
						DisconnectDB();
						ConnectDB();
						return true;
					}
					break;

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
						// TODO: Ctrl + U 는 여러번 수행 무방하나, Ctrl + L 은 여러번 수행시 "ㅣ" 이라는 글자가 입력됨. (BUGBUG)
						SendKeys.SendWait("(^+U)"); // FCTB 자체구현 호출
						return true;
					}
					break;

				case Keys.R:        // Toad
					if (((keyData & Keys.Alt) != 0) || ((keyData & Keys.Shift) != 0)) break;
					if ((keyData & Keys.Control) != 0)
					{
						SendKeys.SendWait("(^H)"); // FCTB 자체구현 호출
						return true;
					}
					break;

				case Keys.B:        // Toad
				case Keys.OemMinus: // Orange, FCTB에서 직전 커서 위치로 이동 기능이 막히게됨.
					if ((keyData & Keys.Alt) != 0)
					{
						if (((keyData & Keys.Control) != 0) || ((keyData & Keys.Shift) != 0)) break;

						// [NEW] Bind Variables DockPanel
						{
							if ( doc2.Visible )
							{
								doc2.Hide();
								doc2.Visible = false;
							}
							else
							{
								ShowBindList();
							}
						}

						return true;
					}
					else if ((keyData & Keys.Control) != 0)
					{
						string commentPrefix = "--";
						var oldStartPlace = tb.Selection.Start; // 영역선택후 MOUSE UP 위치
						var oldEndPlace = tb.Selection.End;     // MOUSE DOWN 위치  

						int movePosition = 0;
						if ((keyData & Keys.Shift) != 0)
						{
							movePosition = commentPrefix.Length * (-1);
							tb.RemoveLinePrefix(commentPrefix);
						}
						else
						{
							movePosition = commentPrefix.Length;
							tb.InsertLinePrefix(commentPrefix);
						}

						// [NOTE] 선택영역이 Drag된 방향 구하는 조건
						//if ((oldStartPoint.Y > oldEndPoint.Y) || (oldStartPoint.Y == oldEndPoint.Y && oldStartPoint.X > oldEndPoint.X))

						tb.Selection.Start = tb.PositionToPlace(tb.PlaceToPosition(oldStartPlace) + movePosition);
						tb.Selection.End = tb.PositionToPlace(tb.PlaceToPosition(oldEndPlace) + movePosition);
						tb.Select();

						return true;
					}
					break;
				case Keys.F:
					if ((keyData & Keys.Alt) != 0) break;
					if ( ((keyData & Keys.Control) != 0) && ((keyData & Keys.Shift) != 0) )
					{
						if (tb.SelectionLength > 0 )
						{
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
								UppercaseKeywords = false,
								ExpandInLists = true
							};

							//var tokenizer = new PoorMansTSqlFormatterLib.Tokenizers.TSqlStandardTokenizer();
							var tokenizer = new PoorMansTSqlFormatterRedux.Tokenizers.TSqlStandardTokenizer();
							var parser = new PoorMansTSqlFormatterRedux.Parsers.TSqlStandardParser();
							var formatter = new PoorMansTSqlFormatterRedux.Formatters.TSqlStandardFormatter(_formatterOptions);

							// TODO: 특정 선택 영역만 "포맷팅" 하는 기능 필요
							var oldText = tb.Text;
							var oldSelectionStart = tb.SelectionStart;
							var oldSelectionEnd = oldSelectionStart + tb.SelectionLength;
							var oldSelectionLength = tb.SelectionLength;

							char[] tirmChars = { '\r', '\n'};
							var tokenizedSQL = tokenizer.TokenizeSQL(tb.SelectedText);
							var parsedSQL = parser.ParseSQL(tokenizedSQL);
							//var formatSQL = formatter.FormatSQLTree(parsedSQL).TrimEnd(tirmChars);
							var formatSQL = formatter.FormatSQLTree(parsedSQL);
							formatSQL = formatSQL.Substring(0, formatSQL.Length - 2);

							if (oldSelectionStart == 0 )
							{
								//tb.Text = ""; // [NOTE] 이런식으로 코딩하면, UNDO 버퍼에 '가비지'가 생긴다.
								tb.Text = formatSQL + oldText.Substring(oldSelectionEnd, oldText.Length - oldSelectionEnd);
							}
							else
							{
								tb.Text = oldText.Substring(0, oldSelectionStart - 0) + formatSQL + oldText.Substring(oldSelectionEnd, oldText.Length - oldSelectionEnd);
							}

							tb.SelectionStart = oldSelectionStart + formatSQL.Length + 3;
							tb.SelectionLength = 0;
						}

						return true;
					}
					break;

				case Keys.Z:
					if (((keyData & Keys.Alt) != 0) || ((keyData & Keys.Shift) != 0)) break;
					if ((keyData & Keys.Control) != 0)
					{
						var oldSelectionStart = tb.SelectionStart;

						// FCTB - '취소'이후 변경된 영역이 선택되므로, 이를 재처리
						tb.ProcessKey(keyData);

						tb.SelectionStart = oldSelectionStart - 1; // 왜 (-1) 이 필요한지 이해가 안되나, 필요함.
						tb.SelectionLength = 0;
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
							ShowResult(tb.SelectedText);
							return true;
						}

						var point = tb.PositionToPoint(tb.SelectionStart);
						var line = tb.YtoLineIndex(point.Y);

						char[] tirmChars = { '\r', '\n', '\t', ' ' };
						var sql = GetSqlFromLine(tb.Text, line).Trim(tirmChars); // 커서위치에 적절한 SQL을 선택한다.

						ShowResult(sql);

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
				case Keys.C: // 컬럼정보 Popup
					if (((keyData & Keys.Shift) != 0) || ((keyData & Keys.Control) != 0)) break;
					if ((keyData & Keys.Alt) != 0)
					{
						tb.VisibleRange.ClearStyle(runSqlStyle); // reset highlight

						string selectedText = null;

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

							new frmColumnInfo().ShowMain(selectedText, conn);
						}

						return true;
					}
					break;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private List<BindType> bindTypes = new List<BindType>();
		private List<BindRow> bindRows = new List<BindRow>();

		private void SetBindColumnHeader()
		{
			{
				DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
				nameColumn.Name = "No";
				nameColumn.DataPropertyName = "No";
				nameColumn.ReadOnly = true;
				dgvBind.Columns.Add(nameColumn);
				dgvBind.Columns[0].Visible = false; // 숨김 컬럼
			}

			{
				DataGridViewTextBoxColumn nameColumn = new DataGridViewTextBoxColumn();
				nameColumn.Name = "Name";
				nameColumn.DataPropertyName = "Name";
				nameColumn.ReadOnly = true;
				dgvBind.Columns.Add(nameColumn);
			}

			{
				DataGridViewTextBoxColumn valueColumn = new DataGridViewTextBoxColumn();
				valueColumn.Name = "Value";
				valueColumn.DataPropertyName = "Value";
				//idColumn.ReadOnly = true;
				dgvBind.Columns.Add(valueColumn);
			}

			{
				DataGridViewComboBoxColumn typeColumn = new DataGridViewComboBoxColumn();
				foreach (BindType e in bindTypes) typeColumn.Items.Add(e);  // Populate the combo boxdrop-down list with objects. 

				// Add "unassigned" to the drop-down list and display it for empty AssignedTo values or when the user presses CTRL+0. 
				//typeColumn.Items.Add("unassigned");
				//typeColumn.DefaultCellStyle.NullValue = "unassigned";
				typeColumn.Items.Add("ANYDATA");
				typeColumn.DefaultCellStyle.NullValue = "ANYDATA";

				typeColumn.Name = "Type";
				typeColumn.DataPropertyName = "Type";
				typeColumn.AutoComplete = true;
				typeColumn.DisplayMember = "Name";
				typeColumn.ValueMember = "Self";

				dgvBind.Columns.Add(typeColumn);

				// Add a button column. 
				//DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
				//buttonColumn.HeaderText = "";
				//buttonColumn.Name = "Status Request";
				//buttonColumn.Text = "Request Status";
				//buttonColumn.UseColumnTextForButtonValue = true;
				//dgvBind.Columns.Add(buttonColumn);

				// Add a CellClick handler to handle clicks in the button column.
				//dgvBind.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
			}
		}

		private void dgvBind_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			// do Something..
		}

		public class BindRow : ICloneable
		{
			public BindRow(int no)
			{
				No = no;
			}

			public int No { get; set; }
			public string Name { get; set; }
			public string Value { get; set; }
			public BindType Type { get; set; }

			public object Clone() // This is a deep copy implementation of Clone
			{
				return new BindRow(this.No)
				{
					Name = this.Name,
					Value = this.Value,
					Type = this.Type, //TODO: 이건 ICloneable 구현하지 않았는데도, 동작한다? Self 때문인지는 확인안됨.
				};
			}
		}

		public class BindType
		{
			public BindType(String name)
			{
				Name = name;
			}

			public String Name { get; set; }

			// TODO: 하나의 약속인듯 - 이해는 안되지만 없으면 런타임 오류 발생.
			public BindType Self
			{
				get { return this; }
			}
		}

		// TODO: 현재 STMT의 변수만 보일지, 다 보일지, 선택/결정 필요
		private void ShowBindList()
		{
			var bindRowsPre = new List<BindRow>();
			foreach (var item in bindRows)
			{
				bindRowsPre.Add((BindRow)item.Clone());
			}

			// 최초수행
			if (bindTypes.Count == 0)
			{
				// BindType 초기화
				// bindType[0] , bindType[1] 의 차이는 초기 선택값의 위치
				bindTypes.Add(new BindType("VARCHAR"));
				bindTypes.Add(new BindType("NUMBER"));
				bindTypes.Add(new BindType("CHAR"));
				bindTypes.Add(new BindType("RefCursor"));
				
#if DEBUG
				bindTypes.Add(new BindType("NCHAR")); // 미구현
				bindTypes.Add(new BindType("NVARCHAR"));
				//bindTypes.Add(new BindType("ROWID"));
#endif
				SetBindColumnHeader();
			}
			else
			{
				dgvBind.DataSource = null; // bindRows가 변경되어도, 이 Line 이 없으면 감지하지 못한다
				bindRows.Clear();
			}

			var text = tb.Text;
			var regexPattern = ":[a-zA-Z][a-zA-Z0-9_]*";
			//var regexPattern = ":[a-zA-Z]\\w*";

			text = text + "\n";
			text = Regex.Replace(text, "--.*\n", "");
			text = Regex.Replace(text, "--.*\r", ""); // WHEN MACOS
			text = Regex.Replace(text, "/\\*(.|\r|\n)*?\\*/", ""); // [NOTE] .+ is greedy , Change it to .+? ( Not Greedy )
			text = Regex.Replace(text, "'(.|\r|\n)*?'", "");

			var mc = Regex.Matches(text, regexPattern);
			if (mc.Count == 0) return;

			var set = new HashSet<string>();
			int ix = 0;

			foreach (Match m in mc)
			{
				if ( set.Add(m.Value) )
				{
					BindRow it = bindRowsPre.Find(x => x.Name == m.Value); // [기능] 사용자가 설정한 이전 값이 있으면 유지
					if ( it != null )
					{
						bindRows.Add(new BindRow(ix++) { Name = m.Value, Value = it.Value, Type = it.Type });
					}
					else
					{
						// 신규 변수가 발견되면, "SQLExecute" 하지 않고 멈춘다.
						configure.bindRowsLastAddTime = DateTime.Now;
						bindRows.Add(new BindRow(ix++) { Name = m.Value, Value = "", Type = bindTypes[0] });
					}
				}
				else
				{
					// ignore duplication
				}
			} // foreach
			dgvBind.DataSource = bindRows;

			//if ( dgvBind.RowCount > 0 )
			{
				doc2.Visible = true;
				doc2.Show();
			}
		}

		private void ShowResult(string sql_)
		{
			List<string> prefix4SPs = new List<string>();
			prefix4SPs.Add("EXEC ");
			prefix4SPs.Add("EXECUTE ");
			prefix4SPs.Add("CALL ");

			foreach(string prefix4SP in prefix4SPs)
			{
				if (sql_.IndexOf(prefix4SP, StringComparison.OrdinalIgnoreCase) == 0) // [USAGE] exec SP1();
				{
					ShowResult4SP(sql_.Substring(prefix4SP.Length, sql_.Length - prefix4SP.Length));
					return;
				}
			}

			PUBLIC.TIME_CHECK(System.DateTime.Now.Ticks); // 기준시간 등록

			lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";

			toolStripStatusLabel4.Text = "Running ..";
			lbRow.Text = "0 rows";

			// 출처: https://and0329.tistory.com/entry/C-과-오라클-데이터베이스-연동-방법 
			try
			{
				dataGridView.DataSource = null;     // (1) 결과창을 비워준다.
				tabControl.SelectedIndex = 2;       // (2) 결과탭을 보여준디.
				Application.DoEvents();

				adapter.SelectCommand = xDb.XDbCommand(sql_, conn);

				do
				{
					// TODO: 전체 BindVar 얻는 부분과 유사한 코드, 정리요
					var text = sql_;
					var regexPattern = ":[a-zA-Z][a-zA-Z0-9_]*";

					text = text + "\n";
					text = Regex.Replace(text, "--.*\n", "");
					text = Regex.Replace(text, "--.*\r", ""); // WHEN MACOS
					text = Regex.Replace(text, "/\\*(.|\r|\n)*?\\*/", ""); // [NOTE] .+ is greedy , Change it to .+? ( Not Greedy )
					text = Regex.Replace(text, "'(.|\r|\n)*?'", "");

					var mc = Regex.Matches(text, regexPattern);
					if (mc.Count == 0)
					{
						break;
					}
					else
					{
						ShowBindList();
						var now = DateTime.Now;
						if (configure.bindRowsLastAddTime.AddSeconds(1) >= now)
						{
							// dgvBind 에 새로운 변수가 등록되었다						
							toolStripStatusLabel4.Text = "Bind variable not declared"; // SP2-0552: Bind variable "V1" not declared.
							return;
						}
					}

					var set = new HashSet<string>();
					foreach (Match m in mc)
					{
						set.Add(m.Value); // 중복제거
					} // foreach

					foreach (BindRow m in bindRows)
					{
						if (!set.Contains(m.Name)) continue;

						if (adapter.SelectCommand is OracleCommand cmd)
						{
							OracleDbType dbType = OracleDbType.Varchar2;

							if (m.Type.Name == "NUMBER")
							{
								dbType = OracleDbType.Decimal;
							}
							else if (m.Type.Name == "CHAR")
							{
								dbType = OracleDbType.Char;
							}
							//else if (m.Type.Name == "RefCursor")
							//{
							//	dbType = OracleDbType.RefCursor;
							//}
							else if (m.Type.Name == "NCHAR")
							{
								dbType = OracleDbType.NChar;
							}
							else if (m.Type.Name == "NVARCHAR")
							{
								dbType = OracleDbType.NVarchar2;
							}
							cmd.Parameters.Add(new OracleParameter(m.Name, dbType, 32) { Value = m.Value });
						}
					}

					break;
				} while (true);

				DataTable dt = new DataTable();     // TODO: (BUGBUG) DataTable을 재사용하면, 컬럼명이 추가된다.
				DataTable dt2 = new DataTable();    // 처음 결과 셋을 일부만 저장하여 보여주는 Fake 코드 (출력성능이슈)

				int sPos = 0;
				int rc;

				lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";

				while ((rc = adapter.Fill(sPos, FETCH_SIZE, dt)) > 0)
				{
					sPos += rc;

					if (toolStripProgressBar1.Value <= toolStripProgressBar1.Maximum * 0.9)
					{
						toolStripProgressBar1.PerformStep();
					}

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

				dataGridView.DataSource = XDb.ConvertDataTable(dt); // RAW 타입에 대해서 DataGridView 에서 Error
			}
			catch (Exception ex)
			{
				MessageBoxEx.Show(this.ParentForm, ex.Message);
			}

			//finally
			{
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
			}
		} // ShowResult


		static bool isServerOutputOn = true;
		private void ShowResult4SP(string sql_)
		{
			if (!(conn is OracleConnection oc))
			{
				// TODO: 미구현
				toolStripStatusLabel4.Text = "Not Supported DBMS";
				return;
			}

			//sql_ = "SP1"; //_OKT_TRY
			PUBLIC.TIME_CHECK(System.DateTime.Now.Ticks); // 기준시간 등록

			lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";
			toolStripStatusLabel4.Text = "Running ..";
			lbRow.Text = "0 rows";

			//DbDataAdapter l_adapter = null;
			string refCursorName = null;

			try
			{
				dataGridView.DataSource = null;     // (1) 결과창을 비워준다.
				tabControl.SelectedIndex = 2;       // (2) 결과탭을 보여준디.
				Application.DoEvents();

				//var cmd = xDb.XDbCommand(sql_, conn);
				OracleCommand cmd = new OracleCommand();
				cmd.Connection = (OracleConnection)conn;				
				cmd.BindByName = true;
				//cmd.CommandType = CommandType.StoredProcedure; // [NOTE] 이 방식은 ProcedureName만 전달하고, Bind 변수 이름을 DB Meta에서 조회 필요
				//cmd.CommandType = CommandType.Text; // default

				if (isServerOutputOn)
				{
					// FROM: https://stackoverflow.com/questions/27837853/how-to-get-oracle-stored-procedure-standard-output-in-c-sharp ( Thx !! )
					cmd.CommandText = "BEGIN DBMS_OUTPUT.ENABLE (32767); END;"; // set serveroutput on;
					cmd.ExecuteNonQuery();
				}

				cmd.CommandText = "BEGIN " + sql_ + " END;";
				cmd.Parameters.Clear();

				do
				{
					// TODO: 전체 BindVar 얻는 부분과 유사한 코드, 정리요
					var text = sql_;
					var regexPattern = ":[a-zA-Z][a-zA-Z0-9_]*";

					text = text + "\n";
					text = Regex.Replace(text, "--.*\n", "");
					text = Regex.Replace(text, "--.*\r", ""); // WHEN MACOS
					text = Regex.Replace(text, "/\\*(.|\r|\n)*?\\*/", ""); // [NOTE] .+ is greedy , Change it to .+? ( Not Greedy )
					text = Regex.Replace(text, "'(.|\r|\n)*?'", "");

					var mc = Regex.Matches(text, regexPattern);
					if (mc.Count == 0)
					{
						break;
					}
					else
					{
						ShowBindList();
						var now = DateTime.Now;
						if (configure.bindRowsLastAddTime.AddSeconds(1) >= now)
						{
							// dgvBind 에 새로운 변수가 등록되었다						
							toolStripStatusLabel4.Text = "Bind variable not declared"; // SP2-0552: Bind variable "V1" not declared.
							return;
						}
					}

					var set = new HashSet<string>();
					foreach (Match m in mc)
					{
						set.Add(m.Value); // 중복제거
					} // foreach

					foreach (BindRow m in bindRows)
					{
						if (!set.Contains(m.Name)) continue;

						//if (cmd_ is OracleCommand cmd)
						{
							OracleDbType dbType = OracleDbType.Varchar2;

							if (m.Type.Name == "RefCursor")
							{
								refCursorName = m.Name;
							}
							else
							{
								if (m.Type.Name == "NUMBER")
								{
									dbType = OracleDbType.Decimal;
								}
								else if (m.Type.Name == "CHAR")
								{
									dbType = OracleDbType.Char;
								}
								cmd.Parameters.Add(new OracleParameter(m.Name, dbType, 32) { Value = m.Value });
							}
						}
					}

					break;
				} while (true);

				if (refCursorName != null)
				{
					((OracleCommand)cmd).Parameters.Add(refCursorName, OracleDbType.RefCursor).Direction = ParameterDirection.Output;
					//((OracleCommand)cmd).Parameters.Add("I_EMP_ID", OracleDbType.Varchar2).Value = "asdf";
					//((OracleCommand)cmd).Parameters.Add("OUT_DATA", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

					cmd.ExecuteNonQuery();

					OracleDataReader reader = ((OracleRefCursor)cmd.Parameters[refCursorName].Value).GetDataReader();
					//OracleDataReader reader = ((OracleRefCursor)cmd.Parameters["OUT_DATA"].Value).GetDataReader();
					DataTable dataTable = new DataTable();
					dataTable.Load(reader);
					dataGridView.DataSource = XDb.ConvertDataTable(dataTable); // RAW 타입에 대해서 DataGridView 에서 Error
					
					lbRow.Text = dataTable.Rows.Count + " rows";
				}
				else
				{
					cmd.ExecuteNonQuery();

					lbRow.Text = "- rows";
				}

				// Get ServerOutput
				if (isServerOutputOn)
				{
					cmd.CommandText = "BEGIN DBMS_OUTPUT.GET_LINE (:buffer, :status); END;";
					cmd.Parameters.Clear();
					cmd.Parameters.Add("buffer", OracleDbType.Varchar2, 32767).Direction = System.Data.ParameterDirection.Output;
					cmd.Parameters.Add("status", OracleDbType.Int32).Direction = System.Data.ParameterDirection.Output;

					List<string> buffers = new List<string>();
					string buffer = null;
					int status = 0;
					while (status == 0)
					{
						cmd.ExecuteNonQuery();

						buffer = cmd.Parameters[0].Value.ToString();
						status = int.Parse(cmd.Parameters[1].Value.ToString());
						buffers.Add(buffer);
					}
				}

				lbTime.Text = PUBLIC.TIME_CHECK() + " sec";
			}
			catch (Exception ex)
			{
				MessageBoxEx.Show(this.ParentForm, ex.Message);
			}

			//finally
			{
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
			}
		} // ShowResult4SP


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

		private void ConnectDB()
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
							xDb.ConnStr = "Data Source=192.168.56.201:1521/xe;User ID=US_GDMON;Password=US_GDMON"; // OK
							break;
						case XDbConnType.ALTIBASE:
							// (1) if Autocommit then {"LobLocator cannot span the transaction 332417."}
							xDb.ConnStr = "DSN=192.168.56.201;uid=sys;pwd=manager;NLS_USE=MS949;PORT=20300"; // OK
							break;
						case XDbConnType.TIBERO:
							// (2) {"TBR-02040 (24000): Invalid cursor state."}
							xDb.ConnStr = "Provider=tbprov.Tbprov.5;Data Source=tibero;User ID=sys;Password=tibero;";
							FETCH_SIZE = 1;
							break;
						case XDbConnType.MSSQL:
							xDb.ConnStr = "Server=192.168.56.201;Database=master;Uid=sa;Pwd=password12!";
							break;
						case XDbConnType.OLEDB:
							// (1) ALTIBASE의 경우 CLOB 조회 불가
							xDb.ConnStr = "Provider=tbprov.Tbprov;Data Source=tibero;User ID=sys;Password=tibero;"; 
							//xDb.mConnStr = "Provider=Altibase.OLEDB;Data Source=192.168.56.201;User ID=sys;Password=manager;Extended Properties='PORT=20300'"; // OK, But CLOB BUGBUG
							break;
						default:
							MessageBoxEx.Show("[NEVER] " + sXDBConnType);
							return; // TODO: C# 강제종료는 어떻게? 
							//break;
					}
					toolStripStatusLabel4.Text = "Connecting.. " + xDb.ConnType;
                    Application.DoEvents();
                }

                //conn = new OracleConnection(connStr);
                conn = xDb.XDbConnection();
                conn.Open();

                //adapter = new OracleDataAdapter();
                adapter = xDb.XDbDataAdapter();

				toolStripStatusLabel4.Text = "Connected " + xDb.ConnType;
				Application.DoEvents();
			}
			catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                conn = null;
            }
        }

		private void DisconnectDB()
		{
			if (conn != null)
			{
				//dgvGridResult.Dispose();
				adapter.Dispose();
				conn.Close();

				adapter = null;
				conn = null;
			}
		}

		private void btnOpen_Click(object sender, EventArgs e)
		{
			ConnectDB();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			DisconnectDB();
		}

        private void btnGo_Click(object sender, EventArgs e)
        {
			ShowResult(tb.Text);
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

        private void btnOpt_Click(object sender, EventArgs e)
        {	
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

		private void tb_SelectionChangedDelayed(object sender, EventArgs e)
		{
			tb.VisibleRange.ClearStyle(sameWordsStyle);
			tb.VisibleRange.ClearStyle(runSqlStyle);
		}

		private void tb_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (tb.SelectedText.Trim().Length == 0) return;

			// TODO: 라이센스확인요, FCTB의 테스트 샘플 코드 사용함 - PowerfulCSharpEditor.cs 
			//remember last visit time
			if (tb.Selection.IsEmpty && tb.Selection.Start.iLine < tb.LinesCount)
			{
				if (lastNavigatedDateTime != tb[tb.Selection.Start.iLine].LastVisit)
				{
					tb[tb.Selection.Start.iLine].LastVisit = DateTime.Now;
					lastNavigatedDateTime = tb[tb.Selection.Start.iLine].LastVisit;
				}
			}

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

		private void SQLToolControl_Resize(object sender, EventArgs e)
		{
		}

		private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
		{
			//if (configure.SplitterDistanceDateTime.AddSeconds(1) >= now)
			{
				configure.Panel2SizeDateTime = DateTime.Now;
				configure.Panel2SizePre = configure.Panel2Size;
				configure.Panel2Size = splitContainer.Height - splitContainer.SplitterDistance;
			}
		}

		private void splitContainer_Resize(object sender, EventArgs e)
		{
			// [TIP] 크기조정시 SplitterMoved -> Resize 순서로 호출됨을 확인, 구분할 수 있는 조건이 없어서, 변경시간을 이용함.
			if ( configure.Panel2SizeDateTime.AddSeconds(1) >= DateTime.Now )
			{
				if (splitContainer.Height >= configure.Panel2SizePre + splitContainer.Panel1MinSize)
				{
					splitContainer.SplitterDistance = splitContainer.Height - configure.Panel2SizePre;
				}
			}
		}

	} // class SQLToolUserControl
}
