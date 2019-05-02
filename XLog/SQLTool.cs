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
    public partial class SQLTool : Form
    {
        private int nTabSeq = 0;

        public SQLTool()
        {
            InitializeComponent();

            //this.Icon = new Icon(Properties.Resources.x128_01_main.ToString());

            // TODO: 여전히 깜박임. ㅠㅠ
            //this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);

        }

        private void CreateTabPage()
        {
            int lastIndex = tabControl1.TabCount - 1;
            if (tabControl1.SelectedIndex != lastIndex)
            {
                return;
            }

            nTabSeq++;

            string sTmp = "SQL" + nTabSeq + "    ";
            TabPage myTabPage = new TabPage(sTmp);

            var myTabForm = new TUserSQL();

            myTabForm.Show();
            //myTabForm.TopLevel = false;
            myTabForm.Dock = DockStyle.Fill;
            myTabForm.Visible = true;
            myTabPage.Controls.Add(myTabForm);

            //tabControl1.TabPages.Add(myTabPage);
            tabControl1.TabPages.Insert(lastIndex, myTabPage);
            tabControl1.SelectedIndex = tabControl1.TabCount - 2;
        }

        private void SQLTool_Load(object sender, EventArgs e)
        {
            panel1.Dock = DockStyle.Fill;

            tabControl1.SelectedIndex = 0;
            //tabControl1_Selecting(sender, null);
            CreateTabPage();
            tabControl1_HandleCreated(sender, e);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
        private const int TCM_SETMINTABWIDTH = 0x1300 + 49;
        private void tabControl1_HandleCreated(object sender, EventArgs e)
        {
            SendMessage(this.tabControl1.Handle, TCM_SETMINTABWIDTH, IntPtr.Zero, (IntPtr)16);
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // https://social.technet.microsoft.com/wiki/contents/articles/50957.c-winform-tabcontrol-with-add-and-close-button.aspx#Caution_Hot_Stuff
            try
            {
                var tabPage = this.tabControl1.TabPages[e.Index];
                var tabRect = this.tabControl1.GetTabRect(e.Index);
                tabRect.Inflate(-2, -2);
                if (e.Index == this.tabControl1.TabCount - 1) // Add button to the last TabPage only
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

        //private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        //{
        //    CreateTabPage();
        //}

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If the last TabPage is selected then Create a new TabPage
            if (tabControl1.SelectedIndex == tabControl1.TabPages.Count - 1)
                CreateTabPage();
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            // Process MouseDown event only till (tabControl.TabPages.Count - 1) excluding the last TabPage
            for (var i = 0; i < this.tabControl1.TabPages.Count - 1; i++)
            {
                var tabRect = this.tabControl1.GetTabRect(i);
                tabRect.Inflate(-2, -2);
                var closeImage = new Bitmap(Properties.Resources.x16_01_close);
                var imageRect = new Rectangle(
                    (tabRect.Right - closeImage.Width),
                    tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
                    closeImage.Width,
                    closeImage.Height);
                if (imageRect.Contains(e.Location))
                {
                    this.tabControl1.TabPages.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
