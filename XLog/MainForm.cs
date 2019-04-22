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
    public partial class MainForm : Form
    {
        QueryForm QueryF;

        public MainForm()
        {
            InitializeComponent();
        }

        private void queryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueryF = new QueryForm(); //폼2 객체 선언
            QueryF.MdiParent = this;

            QueryF.StartPosition = FormStartPosition.Manual;
            QueryF.Location = new Point(0, 0);
            //QueryF.StartPosition = FormStartPosition.CenterParent;

            QueryF.Show();
        }
    }
}
