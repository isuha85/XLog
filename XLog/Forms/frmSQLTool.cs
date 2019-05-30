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

using FarsiLibrary.Win;
using FarsiLibrary.Win.Design;

/// TODO
/// +) TabControl 에 있는 Close('X') 버튼은, "원치않는 닫힘이 될수도 있겠음" , 확인창 추가하던지, 빼고 그냥 변경중표시('*') 정도를 하든지.

namespace XLog
{
    public partial class frmSQLTool : Form
    {
        private int nTabSeq = 0;

        public frmSQLTool()
        {
            InitializeComponent();
			//this.Icon = new Icon(Properties.Resources.x128_01_main.ToString());

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
			AddTab();
		}

		private void AddTab()
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

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			Keys key = keyData & ~(Keys.Alt | Keys.Shift | Keys.Control);

			switch (key)
			{
				case Keys.T:
					if (((keyData & Keys.Alt) != 0) || ((keyData & Keys.Shift) != 0)) break;
					if ((keyData & Keys.Control) != 0)
					{
						AddTab();
						return true;
					}
					break;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            //// Process MouseDown event only till (tabControl.TabPages.Count - 1) excluding the last TabPage
            //for (var i = 0; i < this.tab.TabPages.Count - 1; i++)
            //{
            //    var tabRect = this.tab.GetTabRect(i);
            //    tabRect.Inflate(-2, -2);
            //    var closeImage = new Bitmap(Properties.Resources.x16_01_close);
            //    var imageRect = new Rectangle(
            //        (tabRect.Right - closeImage.Width),
            //        tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
            //        closeImage.Width,
            //        closeImage.Height);
            //    if (imageRect.Contains(e.Location))
            //    {
            //        tab.TabPages.RemoveAt(i);
            //        break;
            //    }
            //}
        }
    }
}
