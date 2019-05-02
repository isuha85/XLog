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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //tabControl1.AllowDrop = true;
            //tabControl1.TabStop = false;
            //tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            //tabControl1.ItemSize = new System.Drawing.Size(0, 24);

            this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.DoubleBuffer, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);


        }

        private void tabControl1_DragOver(object sender, DragEventArgs e)
        {
        } // DragOver

    }
}
