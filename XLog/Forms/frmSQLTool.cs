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
        private int nTabSeq = 0;
		//private ConcurrentStack<int> SelectedIndexStack = null;

		private struct Configure
		{
			public FormBorderStyle FormBorderStyle;
			public FormWindowState FormWindowState;
		};
		Configure configure;

		public frmSQLTool()
        {
            InitializeComponent();

			{
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

				//this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				//this.WindowState = FormWindowState.Maximized;
				//this.StartPosition = FormStartPosition.Manual;

				//var fullScrenn_bounds = Rectangle.Empty;

				//foreach (var screen in Screen.AllScreens)
				//{
				//	fullScrenn_bounds = Rectangle.Union(fullScrenn_bounds, screen.Bounds);
				//}
				//this.ClientSize = new Size(fullScrenn_bounds.Width, fullScrenn_bounds.Height);
				//this.Location = new Point(fullScrenn_bounds.Left, fullScrenn_bounds.Top);

				configure.FormBorderStyle = FormBorderStyle;
				configure.FormWindowState = FormWindowState.Normal;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			Keys key = keyData & ~(Keys.Alt | Keys.Shift | Keys.Control);

			switch (key)
			{
				case Keys.T:
					if (((keyData & Keys.Alt) != 0) || ((keyData & Keys.Shift) != 0)) break;
					if ((keyData & Keys.Control) != 0)
					{
						CreateTab();
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

		private void CreateTab()
		{
			nTabSeq++;
			//string sTmp = "SQL" + nTabSeq + "    ";
			string sTmp = "SQL" + nTabSeq;
			var tabPage = new TabPage(sTmp);
			var sqlTool = new SQLToolControl();

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
		}

		private void tab_SelectedIndexChanged(object sender, EventArgs e)
		{
			//SelectedIndexStack.Push(tab.SelectedIndex);
		}
	}
}
