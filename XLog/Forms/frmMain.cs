using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XLog
{
    public partial class frmMain : Form
    {
        frmSQLTool QueryF;

        public frmMain()
        {
            InitializeComponent();

			// FROM: https://happyguy81.tistory.com/56
			// TODO: 영향도 확인요 (Win10에서 배율조정시)
			//AutoScaleMode = AutoScaleMode.Font;	// default
			//AutoScaleMode = AutoScaleMode.None;
			AutoScaleMode = AutoScaleMode.Dpi;
		}

		private void queryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueryF = new frmSQLTool(); //폼2 객체 선언
            QueryF.MdiParent = this;

            QueryF.StartPosition = FormStartPosition.Manual;
            QueryF.Location = new Point(0, 0);
            //QueryF.StartPosition = FormStartPosition.CenterParent;

            QueryF.Show();
        }
    }
}
