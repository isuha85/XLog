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
			Win32API.SendMessage(this.tab.Handle, Win32API.SendMessageType.TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)16);
			CreateTabPage();
		}

		private void CreateTabPage()
        {
            int lastIndex = tab.TabCount - 1;
            nTabSeq++;

            string sTmp = "SQL" + nTabSeq + "    ";

            var tabPage = new TabPage(sTmp);
            var sqlTool = new SQLToolControl();
			
			{
				tabPage.Visible = false;
				//sqlTool.TopLevel = false; // WinForm 을 추가할때 필요함.
				sqlTool.Dock = DockStyle.Fill;
				tabPage.Controls.Add(sqlTool);
				//tab.Controls.Add(tabPage);
				tab.TabPages.Insert(lastIndex, tabPage);
				tab.SelectedIndex = tab.TabCount - 2;
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
						CreateTabPage();
						return true;
					}
					break;
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // https://social.technet.microsoft.com/wiki/contents/articles/50957.c-winform-tabcontrol-with-add-and-close-button.aspx#Caution_Hot_Stuff
            try
            {
                var tabPage = this.tab.TabPages[e.Index];
                var tabRect = this.tab.GetTabRect(e.Index);
                tabRect.Inflate(-2, -2);
                if (e.Index == this.tab.TabCount - 1) // Add button to the last TabPage only
                {
                    var addImage = new Bitmap(Properties.Resources.x16_01_add);
                    e.Graphics.DrawImage(addImage,
                        tabRect.Left + (tabRect.Width - addImage.Width) / 2,
                        tabRect.Top + (tabRect.Height - addImage.Height) / 2);

                }
                else // draw Close button to all other TabPages
                {
                    var closeImage = new Bitmap(Properties.Resources.x16_01_close);
                    e.Graphics.DrawImage(closeImage,
                        (tabRect.Right - closeImage.Width),
                        tabRect.Top + (tabRect.Height - closeImage.Height) / 2);
                    TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font,
                        tabRect, tabPage.ForeColor, TextFormatFlags.Left);
                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
		
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
   //         // If the last TabPage is selected then Create a new TabPage
   //         if (tab.SelectedIndex == tab.TabPages.Count - 1)
			//{
			//	if (tab.SelectedIndex != 0 )
			//	{
			//		CreateTabPage();
			//	}
			//	else
			//	{
			//		// TODO: SQL 창의 변경내용을 저장할것인 확인필요
			//		this.Visible = false;
			//		this.Close();
			//	}
			//}
		}

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            // Process MouseDown event only till (tabControl.TabPages.Count - 1) excluding the last TabPage
            for (var i = 0; i < this.tab.TabPages.Count - 1; i++)
            {
                var tabRect = this.tab.GetTabRect(i);
                tabRect.Inflate(-2, -2);
                var closeImage = new Bitmap(Properties.Resources.x16_01_close);
                var imageRect = new Rectangle(
                    (tabRect.Right - closeImage.Width),
                    tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
                    closeImage.Width,
                    closeImage.Height);
                if (imageRect.Contains(e.Location))
                {
                    this.tab.TabPages.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
