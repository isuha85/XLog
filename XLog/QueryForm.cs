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
        private DataTable dtEmpty = null;
        private bool bStop = false;

        private const int FETCH_SIZE = 1000;
        private const int FETCH_SIZE_FOR_GRID = FETCH_SIZE * 2; // 성능을 위해, 초기 일부 데이타만 화면 갱신한다.
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

            richTextBox1.Text = "select level from dual connect by level <= 10001";
            richTextBox1.Text = "with x as ( select level c1 from dual connect by level <= 10001) select c1, data from x, tb_clob";

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
            w = this.Width - lbPos.Width - lbTime.Width - lbRow.Width - toolStripStatusLabel4.Width;
            if ( w < 10 )  w = 10;
            toolStripProgressBar1.Width = w;

        }
        
        private void btnGo_Click(object sender, EventArgs e)
        {
            PUBLIC.TIME_CHECK(System.DateTime.Now.Ticks); // 기준시간 등록

            lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";
            //eTick = System.DateTime.Now.Ticks;
            //dTickSec = (double)(eTick - sTick) / 10000000.0F;
            //lbTime.Text = dTickSec + " sec";

            //this.Cursor = Cursors.WaitCursor;
            toolStripStatusLabel4.Text = "Running ..";
            lbRow.Text = "0 rows";

            // 출처: https://and0329.tistory.com/entry/C-과-오라클-데이터베이스-연동-방법 
            try
            {
                dataGridView1.DataSource = dtEmpty; // (1) 결과창을 비워준다.
                Application.DoEvents();

                //dt.DataSet.Clear();
                //dt.clear();
                //dt.dispose();

                //adapter.Dispose();
                //dataGridView1.SelectAll();
                //dataGridView1.ClearSelection();

                adapter.SelectCommand = new OracleCommand(richTextBox1.Text, conn);
                //adapter.SelectCommand = new OracleCommand("SELECT 1 AS NO FROM DUAL", conn);

                DataTable dt = new DataTable();     // TODO: (BUGBUG) DataTable을 재사용하면, 컬럼명이 추가된다.
                DataTable dt2 = new DataTable();    // 처음 결과 셋을 일부만 저장하여 보여주는 Fake 코드 (출력성능이슈)

                //adapter.Fill(dt);
                //adapter.Fill(0, 10, dt);
                int sPos = 0;
                int rc;

                lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";
                while ( ( rc = adapter.Fill(sPos, FETCH_SIZE, dt) ) > 0 )
                {
                    //sPos += FETCH_SIZE;
                    sPos += rc;

                    if (toolStripProgressBar1.Value <= toolStripProgressBar1.Maximum * 0.9 )
                    {
                        toolStripProgressBar1.PerformStep();
                    }

                    //System.Threading.Thread.Sleep(100);

                    if (sPos == FETCH_SIZE)
                    {
                        // [NOTE] 대량 데이타 출력 성능을 위한 더미 코드.
                        dt2 = dt.Copy();
                        dataGridView1.DataSource = dt2;
                    }

                    lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";
                    lbRow.Text = sPos + "+" + " rows";
                    Application.DoEvents();         //TODO: [NOTE] 필수적임, DoEvents 삽입

                    if (bStop)
                    {
                        break;
                    }
                } // while

                //lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TICK_DELTA + ")";
                lbTime.Text = PUBLIC.TIME_CHECK() + " sec";
                lbRow.Text = sPos + " rows";
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
            }

            toolStripProgressBar1.Value = 0;

            if (bStop)
            {
                bStop = false;
                toolStripStatusLabel4.Text = "Stop";
            }
            else
            {
                toolStripStatusLabel4.Text = "Done";
            }
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

        private void btnStop_Click(object sender, EventArgs e)
        {
            bStop = true;
        }
    }
}
