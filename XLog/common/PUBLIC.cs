using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;                      // 공통 인터페이스 , IDbConnection .. 등
using System.Data.Common;               // 추상 클래스 , DbConnection .. 등

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
    public class PUBLIC
    {
        public static double TIME_DELTA { get; set; }
        private static long mTickPrev { get; set; } = -1;
        private static long mTickStart { get; set; } = -1;

        public static double TIME_CHECK(long aTickStart = -1)
        {
            long sTickNow = System.DateTime.Now.Ticks;

            if (aTickStart != -1)
            {
                mTickStart = aTickStart;
                return 0;
            }

            if (mTickPrev == -1)
            {
                TIME_DELTA = 0;
            }
            else
            {
                TIME_DELTA = Math.Truncate((double)(sTickNow - mTickPrev) / 10000.0F) / 1000;
            }
            mTickPrev = sTickNow;

            return Math.Truncate((double)(sTickNow - mTickStart) / 10000.0F) / 1000;
        }
    
    } // class PUBLIC

//}
//namespace XLog.Data
//{
    public enum XDbConnType
    {
        ODBC		= 101,
        OLEDB		= 102,
        ORACLE		= 111,
        ALTIBASE	= 112,
        TIBERO		= 113,
        MSSQL		= 114,
        MYSQL		= 115,
        MONGODB		= 121,
        NONE		= 999
    }

    //public sealed class OleDbDataAdapter : DbDataAdapter, IDbDataAdapter, IDataAdapter, ICloneable
    public abstract class XDbBase
    {
        public string mConnStr { get; set; }
        public XDbConnType mConnType { get; set; } = XDbConnType.ORACLE;
        //public bool AutoCommit { get; set; }
        public DbConnection mConnection = null;
        public DbTransaction mSelectTransaction4alt = null;

        #region Abstract Functions

        public abstract DbConnection XDbConnection(string aConnStr);

        public abstract DbCommand XDbCommand(); // internal
        public abstract DbCommand XDbCommand(string commandText, DbConnection aConnection);
        public abstract DbCommand XDbCommand4StoredProcedure(string procName, DbConnection aConnection);

		public abstract IDataParameter XDbParameter(string parameterName, object parameterValue);
		public abstract IDataReader XDbDataReader();

        // public OleDbDataAdapter(OleDbCommand selectCommand);
        // public OleDbDataAdapter(string selectCommandText, OleDbConnection selectConnection);
        public abstract DbDataAdapter XDbDataAdapter();
        public abstract DbDataAdapter XDbDataAdapter(DbCommand aSelectCommand);
        public abstract DbDataAdapter XDbDataAdapter(string aSelectCommandText, DbConnection aConnection);

        public abstract DbTransaction XDbTransaction();

        #endregion
    }

    public class XDb : XDbBase
    {
        public XDb()
        {
            //this.mConnType = XDbConnType.ORACLE;
        }

        public XDb(XDbConnType connType_)
        {
            this.mConnType = connType_;
        }

        public override DbConnection XDbConnection(string aConnStr = null)
        {
            if (mConnection != null)
            {
                // TODO: 임시코드, 종료처리할지, 오류낼지.
                mConnection.Close();
                mConnection = null;
            }

            string sConnStr = null;

            if (aConnStr == null)
            {
                sConnStr = mConnStr;
            }
            else
            {
                sConnStr = aConnStr;
            }
                       
            if (mConnType == XDbConnType.ORACLE)
            {
                mConnection = new OracleConnection(sConnStr);
            }
            else if (mConnType == XDbConnType.ALTIBASE)
            {
                mConnection = new AltibaseConnection(sConnStr);
            }
            else if (mConnType == XDbConnType.MSSQL)
            {
                mConnection = new SqlConnection(sConnStr);
            }
            else if (mConnType == XDbConnType.TIBERO)
            {
                mConnection = new OleDbConnectionTbr(sConnStr);
            }
            else if (mConnType == XDbConnType.OLEDB)
            {
                mConnection = new OleDbConnection(sConnStr);
            }

            return mConnection;
        }

        public override DbCommand XDbCommand()
        {
            DbCommand sCommand = null;

            if (mConnType == XDbConnType.ORACLE)
            {
                sCommand = new OracleCommand();
            }
            else if (mConnType == XDbConnType.ALTIBASE)
            {
                sCommand = new AltibaseCommand();
            }
            else if (mConnType == XDbConnType.MSSQL)
            {
                sCommand = new SqlCommand();
            }
            else if (mConnType == XDbConnType.TIBERO)
            {
                sCommand = new OleDbCommandTbr();
            }
            else if (mConnType == XDbConnType.OLEDB)
            {
                sCommand = new OleDbCommand();
            }

            return sCommand;
        }

        public override DbCommand XDbCommand(string commandText, DbConnection aConnection = null)
        {
            DbCommand sCommand = XDbCommand();

            sCommand.CommandText = commandText;
            sCommand.CommandType = CommandType.Text;
            if (aConnection == null)
            {
                sCommand.Connection = mConnection;
            }
            else
            {
                sCommand.Connection = aConnection;
            }

            if ( false
				|| mConnType == XDbConnType.ALTIBASE
                //|| mConnType == XDbConnType.TIBERO
               )
            {
                // TODO: 제거해야함 - Altibase 에서 LOB 조회를 위한 임시코드.
                // (1) AutoCommit 에서 조회하면 오류 - {"LobLocator cannot span the transaction 332417."}
                // (2) (BUGBUG) 그러하다면, 해당 쿼리 이전에 다른 TX가 있으면 어찌 할 것인가?

                if (mSelectTransaction4alt == null)
                {
                    mSelectTransaction4alt = mConnection.BeginTransaction(); // [WHAT] mConnection.EnlistTransaction
                    sCommand.Transaction = mSelectTransaction4alt;
                }
                else
                {
                    mSelectTransaction4alt.Rollback();
                    //mSelectTransaction4alt.Commit();

                    mSelectTransaction4alt = mConnection.BeginTransaction();
                    //mSelectTransaction4alt.Connection.BeginTransaction(); // 이런식으로 작성하면, "개체의 현재상태가 유효하지 않습니다" 오류가 2번째 Rollback에서 발생
                }
            }

            return sCommand;
        }

        public override DbCommand XDbCommand4StoredProcedure(string procName, DbConnection aConnection = null)
        {
            DbCommand sCommand = XDbCommand(procName, aConnection);
            sCommand.CommandType = CommandType.StoredProcedure;

            return sCommand;
        }

        public override IDataParameter XDbParameter(string parameterName, object parameterValue)
        {
            IDataParameter sParameter = null;

            if (mConnType == XDbConnType.ORACLE)
            {
                sParameter = new OracleParameter(parameterName, parameterValue);
            }
            else if (mConnType == XDbConnType.ALTIBASE)
            {
                sParameter = new AltibaseParameter(parameterName, parameterValue);
            }
            else if (mConnType == XDbConnType.MSSQL)
            {
                sParameter = new SqlParameter(parameterName, parameterValue);
            }
            else if (mConnType == XDbConnType.TIBERO)
            {
                sParameter = new OleDbParameterTbr(parameterName, parameterValue);
            }
            else if (mConnType == XDbConnType.OLEDB)
            {
                sParameter = new OleDbParameter(parameterName, parameterValue);
            }

            return sParameter;
        }

        // not implemented, think more ..
        public override IDataReader XDbDataReader()
        {
            IDataReader sDataReader = null;

            //TODO: Do Something

            //if (mConnType == XDbConnType.ORACLE)
            //{
            //    sDataReader = new OracleDataReader();
            //}
            //else if (mConnType == XDbConnType.ALTIBASE)
            //{
            //    sDataReader = new AltibaseDataReader();
            //}
            //else if (mConnType == XDbConnType.MSSQL)
            //{
            //    sDataReader = new SqlDataReader();
            //}
            //else if (mConnType == XDbConnType.TIBERO)
            //{
            //    sDataReader = new OdbcDataReader(); // (X) OdbcDataReaderTbr
            //}
            //else if (mConnType == XDbConnType.OLEDB)
            //{
            //    sDataReader = new OdbcDataReader();
            //}

            //OleDbDataReader reader = rcmd.ExecuteReader();
            //if (reader.HasRows)
            //{
            //    DataTable rettbl = new DataTable();
            //    rettbl.Load(reader);
            //}
            //reader.Close();

            return sDataReader;
        }

        public override DbDataAdapter XDbDataAdapter()
        {
            DbDataAdapter sDataAdapter = null;

            if (mConnType == XDbConnType.ORACLE)
            {
                sDataAdapter = new OracleDataAdapter();
            }
            else if (mConnType == XDbConnType.ALTIBASE)
            {
                sDataAdapter = new AltibaseDataAdapter();
            }
            else if (mConnType == XDbConnType.MSSQL)
            {
                sDataAdapter = new SqlDataAdapter();
            }
            else if (mConnType == XDbConnType.TIBERO)
            {
                sDataAdapter = new OleDbDataAdapterTbr();
            }
            else if (mConnType == XDbConnType.OLEDB)
            {
                sDataAdapter = new OleDbDataAdapter();
            }
            
            return sDataAdapter;
        }


        public override DbDataAdapter XDbDataAdapter(DbCommand aSelectCommand)
        {
            DbDataAdapter sDataAdapter = XDbDataAdapter();

            sDataAdapter.SelectCommand = aSelectCommand;

            return sDataAdapter;
        }

        public override DbDataAdapter XDbDataAdapter(string aSelectCommandText, DbConnection aConnection)
        {
            DbDataAdapter sDataAdapter = XDbDataAdapter();

            sDataAdapter.SelectCommand.CommandText = aSelectCommandText;
            sDataAdapter.SelectCommand.CommandType = CommandType.Text;
            if (aConnection == null)
            {
                sDataAdapter.SelectCommand.Connection = mConnection;
            }
            else
            {
                sDataAdapter.SelectCommand.Connection = aConnection;
            }

            return sDataAdapter;
        }


        public override DbTransaction XDbTransaction()
        {
            DbTransaction sTransaction = null;

            //TODO: Do Something

            //if (mConnType == XDbConnType.ORACLE)
            //{
            //    sTransaction = new OracleTransaction();
            //}
            //else if (mConnType == XDbConnType.ALTIBASE)
            //{
            //    sTransaction = new AltibaseTransaction();
            //}
            //else if (mConnType == XDbConnType.MSSQL)
            //{
            //    sTransaction = new SqlTransaction();
            //}
            //else if (mConnType == XDbConnType.TIBERO)
            //{
            //    sTransaction = new OleDbTransaction(); // (X) OleDbTransactionTbr
            //}
            //else if (mConnType == XDbConnType.OLEDB)
            //{
            //    sTransaction = new OleDbTransaction();
            //}

            return sTransaction;        }
    }
}
