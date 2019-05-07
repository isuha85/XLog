using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

namespace XLog
{
    public partial class QueryForm2 : Form
    {
        private int dbConType = 1;   // 1: Oracle.ManagedDataAccess.Client
        private const int FETCH_SIZE = 1000;
        private bool bStop = false;

        private string oraConStr = "Data Source=192.168.56.201:1521/xe;User ID=US_GDMON;Password=US_GDMON";
        private OracleConnection oraCon = null;
        private OracleDataAdapter oraAdapter = null;
        private DataTable dtEmpty = null;

        public QueryForm2()
        {
            InitializeComponent();
        }

        private void QueryForm2_Load(object sender, EventArgs e)
        {
            int margin_w = flowLayoutPanel1.Width;
            int w = this.Width - margin_w;
            int h = this.Height / 2;

            splitContainer1.Panel1.Size = new Size(w, h);
            splitContainer1.Panel2.Size = new Size(w, h);

            splitContainer1.Dock = DockStyle.Fill;
            tabControl1.Dock = DockStyle.Fill;

            rtbSqlEdit.Text = "select TO_CHAR(SYSDATE, 'YYYY/MM/DD HH24:MI:SS') from dual";
        } // QueryForm2_Load

        private void QueryForm2_Resize(object sender, EventArgs e)
        {
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            PUBLIC.TIME_CHECK(System.DateTime.Now.Ticks); // 기준시간 등록

            lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";

            toolStripStatusLabel4.Text = "Running ..";
            lbRow.Text = "0 rows";

            // 출처: https://and0329.tistory.com/entry/C-과-오라클-데이터베이스-연동-방법 
            try
            {
                dgvResult.DataSource = dtEmpty;     // (1) 결과창을 비워준다.
                tabControl1.SelectedIndex = 0;      // (2) 결과탭을 보여준디.

                Application.DoEvents();
                oraAdapter.SelectCommand = new OracleCommand(rtbSqlEdit.Text, oraCon);

                DataTable dt = new DataTable();     // TODO: (BUGBUG) DataTable을 재사용하면, 컬럼명이 추가된다.

                int sPos = 0;
                int rc;

                lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";
                while ((rc = oraAdapter.Fill(sPos, FETCH_SIZE, dt)) > 0)
                {
                    //sPos += FETCH_SIZE;
                    sPos += rc;

                    lbTime.Text = PUBLIC.TIME_CHECK() + " sec (" + PUBLIC.TIME_DELTA + ")";
                    lbRow.Text = sPos + "+" + " rows";
                    Application.DoEvents();         //TODO: [NOTE] 필수적임, DoEvents 삽입

                    if (bStop)
                    {
                        break;
                    }
                } // while

                lbTime.Text = PUBLIC.TIME_CHECK() + " sec";
                lbRow.Text = sPos + " rows";
                dgvResult.DataSource = dt;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                MessageBox.Show(ex.Message);
            }

            if (bStop)
            {
                bStop = false;
                toolStripStatusLabel4.Text = "Stop";
            }
            else
            {
                toolStripStatusLabel4.Text = "Done";
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                if (oraCon != null)
                {
                    MessageBox.Show("(W) Already Connected.");
                    return;
                }

                oraCon = new OracleConnection(oraConStr);
                oraCon.Open();
                oraAdapter = new OracleDataAdapter();
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
                oraAdapter.Dispose();
                oraCon.Close();

                oraAdapter = null;
                oraCon = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                TEST_OleDb();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

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

        private void TEST_OleDb()
        {
            string strConn;

            /*
             * ## ALTIBASE
             * 
             * {"'AltibaseProv' 공급자는 로컬 컴퓨터에 등록할 수 없습니다."}
             * {"'Altibase.OLEDB' 공급자는 로컬 컴퓨터에 등록할 수 없습니다."}	
             * {"Connection string pair [/?PORT = {20300/?};] ignored."}
             * {"Neither PORT_NO in the connection string nor the ALTIBASE_PORT_NO environment variable has been set."}
             */
            //strConn = "DSN=192.168.56.201;uid=sys;pwd=manager;NLS_USE=MS949;PORT=20300";
            //strConn = "Provider=Altibase.OLEDB;Data Source=altibase_odbc;"; // ADO 연결 문자열 ( X - ADO.net 연결 문자열 )
            strConn = "Provider=Altibase.OLEDB;Data Source=192.168.56.201;User ID=sys;Password=manager;Extended Properties='PORT=20300'"; // OK

            /*
             * ## SQL Server
             * 
             * {"[DBNETLIB][ConnectionOpen (Connect()).]SQL Server가 없거나 액세스할 수 없습니다."}
             */
            //strConn = "Provider=SQLOLEDB;Data Source=192.168.56.201,1433;Initial Catalog=mydb;user Id=sa;Password=password12!;"; // OK

            /*
             * ## ORACLE
             * 
             * (1) MSDAORA is deprecated
             *     {"'MSDAORA' 공급자는 로컬 컴퓨터에 등록할 수 없습니다."}
             *     
             *     오라클홈 -> network -> admin -> sqlnet.ora 파일
             *     SQLNET.AUTHENTICATION_SERVICES=(NTS)
             *     ==> 아래처럼 수정하세요
             *     SQLNET.AUTHENTICATION_SERVICES = (none)
             *     SQLNET.AUTHENTICATION = (none)
             *     
             * (2) 32bit App에서 64bit 모듈 로딩시
             *     {"Error while trying to retrieve text for error ORA-01019"}
             *     {"'ORAOLEDB.ORACLE' 공급자는 로컬 컴퓨터에 등록할 수 없습니다."}
             */
            //strConn = "Provider=MSDAORA;data source=xe;User ID=scott;Password=tiger";
            //strConn = "Provider=MSDAORA;Data Source=//192.168.56.201/xe;User Id=scott;Password=tiger;";
            //strConn = "Provider=ORAOLEDB.ORACLE;Data Source=//192.168.56.201/xe;User Id=scott;Password=tiger;"; // OK
            //strConn = "Provider=ORAOLEDB.ORACLE;User ID=scott;password=tiger;Data Source=xe;"; // OK

            /*
             * ## TIBERO
             * 
             * (1) Location 인자 무시됨
             *     Data Source 인자 없으면 오류, TBR-02131: Generic I/O error 
             *     Data Source=192.168.56.201,8629 이런 용법도 불가.
             * (2) User ID 및 Password 인자는 유효함, TBR-17001 (08001): Login failed: invalid user name or password.
             * (3) tbprov.Tbprov 혹은 tbprov.Tbprov.5 모두 가능
             */
            //strConn = "Provider=tbprov.Tbprov.5;Location=192.168.56.201,8629;Data Source=tibero;User ID=sys;Password=tibero"; 
            //strConn = "Provider=tbprov.Tbprov;Data Source=tibero;User ID=sys;Password=tibero;"; // OK

            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();

            {
                OleDbCommand rcmd = new OleDbCommand("select 1 from dual union all select 2 from dual", conn);

                OleDbDataReader reader = rcmd.ExecuteReader();
                if (reader.HasRows)
                {
                    dgvResult.DataSource = dtEmpty;

                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    dgvResult.DataSource = dt;
                }
                reader.Close();
            }
            
            conn.Close();
        }

        private void TEST_OleDbTSql() // SQL Server
        {
            string strConn;
            //strConn = "Data Source=localhost;Initial Catalog=northwind;User ID=sa;Password=password12!;Integrated Security=SSPI;";
            strConn = "Server=192.168.56.201;Database=mydb;Uid=sa;Pwd=password12!";

            SqlConnection conn = new SqlConnection(strConn);
            //SqlConnection conn = new SqlConnection();
            //conn.ConnectionString = strConn;
            conn.Open();

            SqlCommand cmd = new SqlCommand("select count(*) from T1", conn);
            if (Convert.ToInt32(cmd.ExecuteScalar()) <= 0)
            {
                cmd.CommandText = "create table T1 (i1 int)";
                cmd.ExecuteNonQuery();
            }

            conn.Close();
        }


        private void TEST_OleDbOra()
        {
            // OracleConnection conn = new OracleConnection(strConn);
        }


        private void TEST_OleDbTbr()
        {
            //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["tibero5_oledb"].ConnectionString;
            //OleDbConnectionTbr myConnection = new OleDbConnectionTbr(connectionString);

            string strConn;
            strConn = "Provider=tbprov.Tbprov.5;Location=127.0.0.1,8629;Data Source=tibero;User ID=sys;Password=tibero";
            strConn = "Provider=tbprov.Tbprov.5;Location=192.168.56.201,8629;Data Source=mydb;User ID=sys;Password=tibero";
            strConn = "Provider = tbprov.Tbprov;Data Source = tibero;User ID = sys;Password = tibero;";            

            OleDbConnectionTbr conn = new OleDbConnectionTbr(strConn);
            conn.Open();
            conn.Close();

            conn.Open();

            OleDbCommandTbr cmd = new OleDbCommandTbr("SELECT COUNT(*) cnt FROM ALL_TABLES WHERE table_name = 'TAGS_STRING'", conn);
            if (Convert.ToInt32(cmd.ExecuteScalar()) <= 0)
            {
                cmd.CommandText = "CREATE TABLE tags_string (tagname VARCHAR(16), dt DATE, val VARCHAR(255), " +
                    "PRIMARY KEY(tagname, dt));";
                cmd.ExecuteNonQuery();
            }

            {
                cmd = new OleDbCommandTbr("INSERT INTO TAGS_STRING (tagname, dt, val) VALUES (:name, :dt, :val)", conn);
                cmd.Parameters.Add(":name", OleDbTypeTbr.VarChar, 16);
                cmd.Parameters.Add(":dt", OleDbTypeTbr.DBTimeStamp);
                cmd.Parameters.Add(":val", OleDbTypeTbr.VarChar, 255);
                cmd.Prepare();
                cmd.Parameters[0].Value = 1; // tagid;
                cmd.Parameters[1].Value = "11:20:30"; // dtNow;
                cmd.Parameters[2].Value = 1; // strvalue;
                cmd.ExecuteNonQuery();
            }

            {
                OleDbCommandTbr rcmd = new OleDbCommandTbr("SELECT * FROM (SELECT t.*, ROW_NUMBER() OVER (ORDER BY t.dt DESC, t.tagname) rnum FROM alarm t " +
    ") WHERE rnum BETWEEN 1 AND 30", conn);

                // [NOTE] Not OleDbDataReaderTbr But System.Data.OleDb.OleDbDataReader

                OleDbDataReader reader = rcmd.ExecuteReader();
                if (reader.HasRows)
                {
                    DataTable rettbl = new DataTable();
                    rettbl.Load(reader);
                }
                reader.Close();
            }
        
        }
    }
}
