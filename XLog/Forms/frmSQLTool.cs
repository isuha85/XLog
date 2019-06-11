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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Win32;
using Timer = System.Windows.Forms.Timer;

using FastColoredTextBoxNS;

using System.Collections;
using System.Collections.Concurrent;

using FarsiLibrary.Win;
using FarsiLibrary.Win.Design;

/// TODO
/// +) TabControl 에 있는 Close('X') 버튼은, "원치않는 닫힘이 될수도 있겠음" , 확인창 추가하던지, 빼고 그냥 변경중표시('*') 정도를 하든지.

namespace XLog
{
    public partial class frmSQLTool : Form
    {
		//private ConcurrentStack<int> SelectedIndexStack = null;
		private struct Configure
		{
			public FormWindowState WindowState;
			public Point Location;
			public FormBorderStyle FormBorderStyle;
			public bool isMaximized;

			public int TabSeq;
		};
		static Configure configure;

		static frmSQLTool()
		{
			configure.TabSeq = 0;
		}

		public frmSQLTool()
        {
            InitializeComponent();

			{
				this.Width = 1024;
				this.Height = 768;

				AutoScaleMode = AutoScaleMode.Inherit;
				//this.Icon = new Icon(Properties.Resources.x128_01_main.ToString());
				//SelectedIndexStack = new ConcurrentStack<int>();
			}
		}

		private void SQLTool_Load(object sender, EventArgs e)
        {
			// 효과가 있는지는 모르겠음.
			{
				DoubleBuffered = true;
				SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				SetStyle(ControlStyles.DoubleBuffer, true);
				SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			}

			panel.Dock = DockStyle.Fill;
			//Win32API.SendMessage(this.tab.Handle, Win32API.SendMessageType.TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)16);
			CreateTab();

			// Configure
			{
				this.StartPosition = FormStartPosition.Manual; // 듀얼모니터 에서 필수라고 함 - https://outshine90.tistory.com/m/4

				configure.FormBorderStyle = FormBorderStyle;
				configure.WindowState = FormWindowState.Normal;
				configure.Location = this.Location;
				configure.isMaximized = false;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			Keys key = keyData & ~(Keys.Alt | Keys.Shift | Keys.Control);

			switch (key)
			{
				case Keys.F11:
				case Keys.F2:		// Toad
					if (((keyData & Keys.Alt) != 0) || ((keyData & Keys.Shift) != 0) || (keyData & Keys.Control) != 0) break;

					// TODO: [미구현] Shift+F2 : Grid Output창 전체화면 전환 ( 필요성을 아직 모르겠음 )
					{
						//this.Opacity = 0.0; // 완전투명
						this.Opacity = 0.1; // 거의투명

						// this.StartPosition = FormStartPosition.Manual; // 듀얼모니터 에서 필수라고 함 - https://outshine90.tistory.com/m/4
						if (configure.isMaximized)
						{
							this.WindowState = configure.WindowState;
							this.Visible = false; // [TIP] WindowState 라인하고 위치가 바뀌면, 오동작한다 - 그러나 이 코드가 별 도움은 안된다 

							this.Location = configure.Location;
							this.FormBorderStyle = configure.FormBorderStyle;
							configure.isMaximized = false;
						}
						else
						{
							this.Visible = false;

							configure.WindowState = this.WindowState;
							configure.Location = this.Location; // 최대화전 위치 백업
							configure.FormBorderStyle = this.FormBorderStyle;
							configure.isMaximized = true;

							this.Location = new Point(0, 0); // 눈피로 - 크롬처럼, 전체화면시에 (0.0) 으로 이동후 확장
							this.Refresh();
							this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
							this.WindowState = FormWindowState.Maximized;
						}

						this.Visible = true;
						this.Opacity = 1;
						this.Refresh();
						return true;
					}
					//break;
				case Keys.T:
					if ((keyData & Keys.Alt) != 0) break;
					if ((keyData & Keys.Control) != 0)
					{
						bool isCopy = false;
						if ((keyData & Keys.Shift) != 0)
						{
							isCopy = true;
						}

						CreateTab(isCopy);
						return true;
					}
					break;
				case Keys.F4:
					if ((keyData & Keys.Alt) != 0) break;

					if ((keyData & Keys.Control) != 0)
					{
						if ((keyData & Keys.Shift) != 0)
						{
							// TODO: 저장여부확인 (해당TAB)

							int NewIndex = tab.SelectedIndex;
							if (tab.SelectedIndex == tab.TabCount - 1)
							{
								NewIndex = NewIndex - 1;
							}

							tab.TabPages.RemoveAt(tab.SelectedIndex);
							tab.SelectedIndex = NewIndex;

							return true;
						}

						if (this.ParentForm != null)
						{
							// Child Form 인 경우만 종료한다.
							// TODO: 저장여부확인 (전역)
							this.Close();
						}
					}
					break;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void CreateTab(bool isCopy = false)
		{
			configure.TabSeq++;
			string sTmp = "SQL" + configure.TabSeq;
			var tabPage = new TabPage(sTmp);
			var sqlTool = new SQLToolControl();
			var selectedIndexPre = tab.SelectedIndex;

			{
				tabPage.Visible = false;

				//sqlTool.TopLevel = false; // WinForm 을 추가할때 필요함.
				sqlTool.Dock = DockStyle.Fill;
				tabPage.Controls.Add(sqlTool);
				//tab.Controls.Add(tabPage);

				tab.TabPages.Insert(tab.TabCount, tabPage);
				tab.SelectedIndex = tab.TabCount - 1;
				tab.Refresh();
			}

			if (isCopy)
			{
				foreach (Control control in tab.TabPages[selectedIndexPre].Controls) // [NOTE] 임의의 Control 찾기
				{
					if (control is SQLToolControl it)
					{
						//if (it.Name == "tb")
						sqlTool.SetTextBox(it.GetTextBox());
						break;
					}
				}
			}
		}

		private void tab_SelectedIndexChanged(object sender, EventArgs e)
		{
			//SelectedIndexStack.Push(tab.SelectedIndex);
		}
	}
}
