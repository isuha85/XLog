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
    public partial class SQLTool : Form
    {
        public SQLTool()
        {
            InitializeComponent();
        }

        private void SQLTool_Load(object sender, EventArgs e)
        {
            panel1.Dock = DockStyle.Fill;
        }
    }
}
