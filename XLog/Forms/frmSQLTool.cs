using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices; //  Do not forget this namespace or else DllImport won't work 

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

			// TODO: 여전히 깜박임. ㅠㅠ
			//this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
		}

        private void SQLTool_Load(object sender, EventArgs e)
        {
            panel1.Dock = DockStyle.Fill;

			Win32API.SendMessage(this.tab.Handle, Win32API.SendMessageType.TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)16);
			CreateTabPage();
		}

		private void CreateTabPage()
        {
            int lastIndex = tab.TabCount - 1;
            nTabSeq++;

            string sTmp = "SQL" + nTabSeq + "    ";
            TabPage myTabPage = new TabPage(sTmp);

            var myTabForm = new SQLToolControl();

            //myTabForm.TopLevel = false;
            myTabForm.Dock = DockStyle.Fill;
            myTabForm.Visible = true;
            myTabPage.Controls.Add(myTabForm);

            //tabControl1.TabPages.Add(myTabPage);
            tab.TabPages.Insert(lastIndex, myTabPage);
            tab.SelectedIndex = tab.TabCount - 2;

			// TODO: (BUGBUG) 탭추가시 깜박임 심하다.
			myTabForm.Show();
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
            // If the last TabPage is selected then Create a new TabPage
            if (tab.SelectedIndex == tab.TabPages.Count - 1)
			{
				if (tab.SelectedIndex != 0 )
				{
					CreateTabPage();
				}
				else
				{
					// TODO: SQL 창의 변경내용을 저장할것인 확인필요
					this.Visible = false;
					this.Close();
				}
			}
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
