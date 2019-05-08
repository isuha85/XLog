using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Diagnostics;                   // 특정시간 응답대기
using System.Configuration;

using System.Data.OleDb;                    // Any DB - oracle / SQL Server / tibero / ..
using Tibero.DbAccess;                      // tibero
using System.Data.SqlClient;                // SQL Server
using Altibase.Data.AltibaseClient;         // altibase

//using Oracle.DataAccessxx;                // Unmanaged 드라이버
using Oracle.ManagedDataAccess.Client;      // Managed 드라이버 (32/64 bit에 무방), deprecated - using System.Data.OracleClient;
using Oracle.ManagedDataAccess.Types;

using System.Data.Common;

/*
 * #### SQL Server vs ORACLE | TIBERO | ALTIBASE
 * 
 * SqlConnection	| OleDbConnection	-> OracleConnection | OleDbConnectionTbr	| AltibaseConnection
 * SqlCommand   	| OleDbCommand    	-> OracleCommand    | OleDbCommandTbr		| AltibaseCommand   
 * SqlDbType    	|     				-> OracleDbType     | OleDbTypeTbr                              
 * SqlException 	|     				-> OracleException  | Exception                                 
 * SqlDataReader	| OleDbDataReader	-> OracleDataReader | OleDbDataReader		| AltibaseDataReader
 * 
 * 출처: https://yaraba.tistory.com/346 
 * DOS> C:\WINDOWS\system32 > regsvr32 tbprov6.dll ( 파일이 N개임, Tibero 설치 pdf 참조할 것 
 */

namespace XLog
{
    public partial class TSQLToolUserControl : UserControl
    {
        private string connStr = "Data Source=192.168.56.201:1521/xe;User ID=US_GDMON;Password=US_GDMON";

        //private OracleConnection conn = null;
        //private OracleDataAdapter adapter = null;
        private DbConnection conn = null;
        private DbDataAdapter adapter = null;
        XDb xDb = null;

        private DataTable dtEmpty = null;
        private bool bStop = false;

        private const int FETCH_SIZE = 1000;
        private const int FETCH_SIZE_FOR_GRID = FETCH_SIZE * 2; // 성능을 위해, 초기 일부 데이타만 화면 갱신한다.
        private const int JUMP_TO_ROW = 20;

        public TSQLToolUserControl()
        {
            InitializeComponent();
        }

        private void SQLToolUserControl_Load(object sender, EventArgs e)
        {
            int margin_w = flowLayoutPanel1.Width;
            int w = this.Width - margin_w;
            int h = this.Height / 2;

            splitContainer1.Panel1.Size = new Size(w, h);
            splitContainer1.Panel2.Size = new Size(w, h);

            splitContainer1.Dock = DockStyle.Fill;
            tabControl1.Dock = DockStyle.Fill;

            rtbSqlEdit.Text = "select level from dual connect by level <= 10001";
            rtbSqlEdit.Text = "with x as ( select level c1 from dual connect by level <= 10001) select c1, data from x, tb_clob";
            rtbSqlEdit.Text = "select c1, data from ( select level c1 from dual connect by level <= 10001) x, tb_clob";

            // Tip 12 - DataGridView 레코드 색상 번갈아서 바꾸기
            dgvResult.RowsDefaultCellStyle.BackColor = Color.White;
            dgvResult.AlternatingRowsDefaultCellStyle.BackColor = Color.Aquamarine;

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
        } // Load

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (xDb == null)
                {
                    xDb = new XDb(XDbConnType.OLEDB);
                    xDb.mConnStr = "Provider=Altibase.OLEDB;Data Source=192.168.56.201;User ID=sys;Password=manager;Extended Properties='PORT=20300'"; // OK

                    //xDb = new XDb(XDbConnType.ORACLE);
                    //xDb.mConnStr = "Data Source=192.168.56.201:1521/xe;User ID=US_GDMON;Password=US_GDMON";

                }

                if (conn != null)
                {
                    MessageBox.Show("(W) Already Connected.");
                    return;
                }

                //conn = new OracleConnection(connStr);
                conn = xDb.XDbConnection();
                conn.Open();

                //adapter = new OracleDataAdapter();
                adapter = xDb.XDbDataAdapter();
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
                //dgvGridResult.Dispose();
                adapter.Dispose();
                conn.Close();

                adapter = null;
                conn = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
                dgvResult.DataSource = dtEmpty;     // (1) 결과창을 비워준다.
                tabControl1.SelectedIndex = 0;      // (2) 결과탭을 보여준디.
                Application.DoEvents();

                //adapter.SelectCommand = new OracleCommand(rtbSqlEdit.Text, (OracleConnection)conn);
                adapter.SelectCommand = xDb.XDbCommand(rtbSqlEdit.Text, conn);

                DataTable dt = new DataTable();     // TODO: (BUGBUG) DataTable을 재사용하면, 컬럼명이 추가된다.
                DataTable dt2 = new DataTable();    // 처음 결과 셋을 일부만 저장하여 보여주는 Fake 코드 (출력성능이슈)

                //adapter.Fill(dt);
                //adapter.Fill(0, 10, dt);
                int sPos = 0;
                int rc;

                lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";
                while ((rc = adapter.Fill(sPos, FETCH_SIZE, dt)) > 0)
                {
                    //sPos += FETCH_SIZE;
                    sPos += rc;

                    if (toolStripProgressBar1.Value <= toolStripProgressBar1.Maximum * 0.9)
                    {
                        toolStripProgressBar1.PerformStep();
                    }

                    //System.Threading.Thread.Sleep(100);

                    if (sPos == FETCH_SIZE)
                    {
                        // [NOTE] 대량 데이타 출력 성능을 위한 더미 코드.
                        dt2 = dt.Copy();
                        dgvResult.DataSource = dt2;
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
                dgvResult.DataSource = dt;
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
        } // btnGO

        private void btnStop_Click(object sender, EventArgs e)
        {
            bStop = true;
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            int jumpToRow = dgvResult.Rows.Count - 1 - 1;

            dgvResult.FirstDisplayedScrollingRowIndex = jumpToRow;
            dgvResult.Rows[jumpToRow].Selected = true;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            int jumpToRow = 0;

            dgvResult.FirstDisplayedScrollingRowIndex = jumpToRow;
            dgvResult.Rows[jumpToRow].Selected = true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int jumpToRow = dgvResult.FirstDisplayedScrollingRowIndex + JUMP_TO_ROW;

            if (jumpToRow > dgvResult.Rows.Count - 1)
            {
                jumpToRow = dgvResult.Rows.Count - 1;
            }

            dgvResult.FirstDisplayedScrollingRowIndex = jumpToRow;
            dgvResult.Rows[jumpToRow].Selected = true;
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            int jumpToRow = dgvResult.FirstDisplayedScrollingRowIndex - JUMP_TO_ROW;

            if (jumpToRow < 0)
            {
                jumpToRow = 0;
            }

            dgvResult.FirstDisplayedScrollingRowIndex = jumpToRow;
            dgvResult.Rows[jumpToRow].Selected = true;
        }

        static int btnOpt_Click_Cnt = 0;
        private void btnOpt_Click(object sender, EventArgs e)
        {
            btnOpt_Click_Cnt++;

            if (btnOpt_Click_Cnt % 2 != 0)
            {
                if (btnOpt_Click_Cnt == 1)
                {
                    dgvTmp.DefaultCellStyle.ForeColor = dgvResult.DefaultCellStyle.ForeColor;
                    dgvTmp.RowsDefaultCellStyle.BackColor = dgvResult.RowsDefaultCellStyle.BackColor;
                    dgvTmp.GridColor = dgvResult.GridColor;
                    dgvTmp.BorderStyle = dgvResult.BorderStyle;
                }

                dgvResult.DefaultCellStyle.ForeColor = Color.Coral;
                dgvResult.RowsDefaultCellStyle.BackColor = Color.AliceBlue;
                dgvResult.GridColor = Color.Blue;
                dgvResult.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                dgvResult.DefaultCellStyle.ForeColor = dgvTmp.DefaultCellStyle.ForeColor;
                dgvResult.RowsDefaultCellStyle.BackColor = dgvTmp.RowsDefaultCellStyle.BackColor;
                dgvResult.GridColor = dgvTmp.GridColor;
                dgvResult.BorderStyle = dgvTmp.BorderStyle;
            }
        } // btnOpt
    } // class SQLToolUserControl
}
