using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Oracle.ManagedDataAccess.Client;      // using System.Data.OracleClient;
using System.Diagnostics;                   // 특정시간 응답대기


namespace XLog
{
    public partial class QueryForm : Form
    {
        //소스 이름과 유저 이름, 패스워드를 입력함 (나중에 오라클 연결할 때 sql문 사용)
        //Data Source  = 본인의 아이피 주소:포트번호/orcl 이다!

        private string connStr = "Data Source=192.168.56.201:1521/xe;User ID=US_GDMON;Password=US_GDMON";
        private OracleConnection conn = null;
        private OracleDataAdapter adapter = null;
        private DataTable dt = null;

        private const int FETCH_SIZE = 1000;
        private const int JUMP_TO_ROW = 20;

        public QueryForm()
        {
            InitializeComponent();
            //MessageBox.Show("OKT-01: " + flowLayoutPanel1.Width);
        }

        private void QueryForm_Load(object sender, EventArgs e)
        {
            //MessageBox.Show("OKT-02: " + flowLayoutPanel1.Width);

            int margin_w = flowLayoutPanel1.Width;
            int w = this.Width - margin_w;
            int h = this.Height / 2;

            panel1.Size = new Size(w, h);
            panel2.Size = new Size(w, h);

            //panel1.Dock = DockStyle.Top;
            panel2.Dock = DockStyle.Bottom;

            richTextBox1.Dock = DockStyle.Fill;
            dataGridView1.Dock = DockStyle.Fill;

            richTextBox2.Visible = false;
            richTextBox2.Dock = DockStyle.Fill;

            richTextBox1.Text = "select level from dual connect by level <= 1000";

            // dataGridView1
            dataGridView2.Visible = false; // Just For Temp Copy
            if (false)
            {
                // MakeReadOnly
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.ReadOnly = true;
            }
            else
            {                
                // Tip 12 - DataGridView 레코드 색상 번갈아서 바꾸기
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine;
            }

            // toolStripProgressBar1
            {
                toolStripProgressBar1.Style = ProgressBarStyle.Continuous;
                //toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = 100;
                toolStripProgressBar1.Step = 1;
                toolStripProgressBar1.Value = 0;

                toolStripStatusLabel4.Text = "";
            }

        } // QueryForm_Load

        private void QueryForm_Resize(object sender, EventArgs e)
        {
            int margin_w = flowLayoutPanel1.Width;
            int w = this.Width - margin_w;
            int h = this.Height / 2;

            panel1.Size = new Size(w, h);
            panel2.Size = new Size(w, h);

            // TODO: Why Not 
            w = this.Width - toolStripStatusLabel1.Width - toolStripStatusLabel2.Width - toolStripStatusLabel3.Width - toolStripStatusLabel4.Width;
            if ( w < 10 )  w = 10;
            toolStripProgressBar1.Width = w;

        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.WaitCursor;
            toolStripStatusLabel4.Text = "Running ..";

            // 출처: https://and0329.tistory.com/entry/C-과-오라클-데이터베이스-연동-방법 
            try
            {
                //dt.DataSet.Clear();
                //dt.clear();
                //dt.dispose();

                //adapter.Dispose();
                //dataGridView1.SelectAll();
                //dataGridView1.ClearSelection();

                adapter.SelectCommand = new OracleCommand(richTextBox1.Text, conn);
                //adapter.SelectCommand = new OracleCommand("SELECT 1 AS NO FROM DUAL", conn);

                DataTable dt = new DataTable();     //TODO: (BUGBUG) DataTable을 재사용하면, 컬럼명이 추가된다.

                //adapter.Fill(dt);
                //adapter.Fill(0, 10, dt);
                int sPos = 0;
                
                while ( adapter.Fill(sPos, FETCH_SIZE, dt) > 0 )
                {
                    sPos += FETCH_SIZE;

                    if (toolStripProgressBar1.Value <= toolStripProgressBar1.Maximum * 0.9 )
                    {
                        toolStripProgressBar1.PerformStep();
                    }

                    //System.Threading.Thread.Sleep(100);
                    Application.DoEvents();    //TODO: [NOTE] 필수적임, DoEvents 삽입
                }


                // DB
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
            }

            toolStripProgressBar1.Value = 0;
            toolStripStatusLabel4.Text = "";
            //this.Cursor = Cursors.Default;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn != null)
                {
                    MessageBox.Show("(W) Already Connected.");
                    return;
                }

                conn = new OracleConnection(connStr);
                conn.Open();
                adapter = new OracleDataAdapter();
                dt = new DataTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                //dataGridView1.Dispose();
                dt.Dispose();
                adapter.Dispose();
                conn.Close();

                dt = null;
                adapter = null;
                conn = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int jumpToRow = dataGridView1.Rows.Count - 1 - 1;

            dataGridView1.FirstDisplayedScrollingRowIndex = jumpToRow;
            dataGridView1.Rows[jumpToRow].Selected = true;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            int jumpToRow = 0;

            dataGridView1.FirstDisplayedScrollingRowIndex = jumpToRow;
            dataGridView1.Rows[jumpToRow].Selected = true;

        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            int jumpToRow = dataGridView1.FirstDisplayedScrollingRowIndex + JUMP_TO_ROW;

            if ( jumpToRow > dataGridView1.Rows.Count - 1 )
            {
                jumpToRow = dataGridView1.Rows.Count - 1;
            }

            dataGridView1.FirstDisplayedScrollingRowIndex = jumpToRow;
            dataGridView1.Rows[jumpToRow].Selected = true;

            //int jumpToRow = 20;
            //if (dataGridView1.Rows.Count >= jumpToRow && jumpToRow >= 1)
            //{
            //    dataGridView1.FirstDisplayedScrollingRowIndex = jumpToRow;
            //    dataGridView1.Rows[jumpToRow].Selected = true;
            //}
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            int jumpToRow = dataGridView1.FirstDisplayedScrollingRowIndex - JUMP_TO_ROW;

            if (jumpToRow < 0)
            {
                jumpToRow = 0;
            }

            dataGridView1.FirstDisplayedScrollingRowIndex = jumpToRow;
            dataGridView1.Rows[jumpToRow].Selected = true;
        }

        static int btnOpt_Click_Cnt = 0;
        private void btnOpt_Click(object sender, EventArgs e)
        {


            btnOpt_Click_Cnt++;

            if (btnOpt_Click_Cnt % 2 != 0)
            {
                if (btnOpt_Click_Cnt == 1 )
                {
                    dataGridView2.DefaultCellStyle.ForeColor = dataGridView1.DefaultCellStyle.ForeColor;
                    dataGridView2.RowsDefaultCellStyle.BackColor = dataGridView1.RowsDefaultCellStyle.BackColor;
                    dataGridView2.GridColor = dataGridView1.GridColor;
                    dataGridView2.BorderStyle = dataGridView1.BorderStyle;
                }

                dataGridView1.DefaultCellStyle.ForeColor = Color.Coral;
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
                dataGridView1.GridColor = Color.Blue;
                dataGridView1.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                dataGridView1.DefaultCellStyle.ForeColor = dataGridView2.DefaultCellStyle.ForeColor;
                dataGridView1.RowsDefaultCellStyle.BackColor = dataGridView2.RowsDefaultCellStyle.BackColor;
                dataGridView1.GridColor = dataGridView2.GridColor;
                dataGridView1.BorderStyle = dataGridView2.BorderStyle;
            }
        }       
    }
}
