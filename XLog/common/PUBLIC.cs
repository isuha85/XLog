using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Runtime.InteropServices;		// https://hot-key.tistory.com/4?category=1010793

using System.Data;							// 공통 인터페이스 , IDbConnection .. 등
using System.Data.Common;					// 추상 클래스 , DbConnection .. 등

using System.Diagnostics;                   // 특정시간 응답대기, Debug.Assert, Debug.WriteLine, ..
using System.Configuration;

using System.Text.RegularExpressions;
using Sprache;

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
		#region Common Function For TIME

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

		#endregion

		#region Common Function For HotKeys

		public const int HOTKEY_ID = 31197;		// Any number to use to identify the hotkey instance
		public const int WM_HOTKEY = 0x0312;

		public enum KeyModifiers
		{
			None = 0,
			Alt = 1,
			Control = 2,
			Shift = 4,
			Windows = 8
		}
		
		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);
		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		#endregion

		#region Common Function For SQLTools

		public enum SimpleStripCommentsType
		{
			SQL		= 1,
			CXX		= 2,
			CXX_SQL	= 4,
			NONE	= 999
		}

		// For SQLTool

		public static string SimpleStripComments(string code, SimpleStripCommentsType k = SimpleStripCommentsType.SQL)
		{
			string re = null;

			switch (k)
			{
				case SimpleStripCommentsType.SQL:
					re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|--*\n|/\*(?s:.*?)\*/";          // SQL
					break;
				case SimpleStripCommentsType.CXX:
					re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*\n|/\*(?s:.*?)\*/";          // C++
					break;
				case SimpleStripCommentsType.CXX_SQL:
					re = @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*\n|--.*\n|/\*(?s:.*?)\*/";   // SQL + C++
					break;
				default:
					return "Not Implemented";
					break;
			}
			return Regex.Replace(code, re, "$1");
		}

		/*
		 * 1. Trim
		 * 2. 사용자 SQL을 ';' 기준으로 분리
		 */
		public static string SimpleStripSQL(string code_)
		{
			char semicolon = ';';
			int semicolon_pos = -1;

			char single_quotes = '\'';
			char[] comment_line_start = { '-', '-' };
			char[] comment_line_end = { '\r', '\n' };
			char[] comment_block_start = { '/', '*' };
			char[] comment_block_end = { '*', '/' };
			int single_quotes_start_pos = -1;
			int comment_line_start_pos = -1;
			int comment_block_start_pos = -1;

			//char double_qutoes = '"';
			//char backslash = '\\';
			//int double_qutoes_start_pos = -1;
			//int backslash_start_pos = -1;

			string code = code_.Trim();
			int len = code.Length;
			semicolon_pos = code.IndexOf(";");

			// ';' 이 없으면, 하나의 SQL 문장이다.
			if (semicolon_pos == -1) return code;

			// 맨마지막에만 ';' 이면, 제거하고 바로 리턴.
			if (semicolon_pos == len -1)
			{
				return code.Substring(0, len - 1);
			}

			// 주석, 문자열에 속하지 않은 최초의 ';'을 어떻게 찾을 것인가?
			// 범위가 len 대신 len - 1 인 것은, 2Byte 구분자 처리 코드를 간단하게 함.
			int pos;
			for (pos = 0; pos < len - 1; pos++)
			{
				// @ single_quotes
				if ((single_quotes_start_pos == -1) &&
					(code[pos] == single_quotes))
				{
					single_quotes_start_pos = pos;
					continue;
				}

				if (single_quotes_start_pos != -1) // 구간시작
				{
					if (code[pos] == single_quotes)
					{
						single_quotes_start_pos = -1;
					}

					// 구간내의 다른 문자는 무시
					continue;
				}
				
				// @ -- 주석
				if ((comment_line_start_pos == -1) &&
					(code[pos] == comment_line_start[0] && code[pos+1] == comment_line_start[1]))
				{
					comment_line_start_pos = pos;
					continue;
				}

				if (comment_line_start_pos != -1) // 구간시작
				{
					if (code[pos] == comment_line_end[1])
					{
						comment_line_start_pos = -1;
					}

					// 구간내의 다른 문자는 무시
					continue;
				}

				// @ /* .. */ 주석
				if ((comment_block_start_pos == -1) &&
					(code[pos] == comment_block_start[0] && code[pos + 1] == comment_block_start[1]))
				{
					comment_block_start_pos = pos;
					continue;
				}

				if (comment_block_start_pos != -1) // 구간시작
				{
					if (code[pos] == comment_block_end[0] && code[pos + 1] == comment_block_end[1])
					{
						comment_block_start_pos = -1;
					}

					// 구간내의 다른 문자는 무시
					continue;
				}

				if (code[pos] == semicolon)
				{
					return code.Substring(0, pos);
				}

			} // for (int pos = 0; pos < len; pos++)

			// 맨마지막에, ';' 이 있는 경우.
			if (code[pos] == semicolon)
			{
				return code.Substring(0, pos);
			}

			return code;
		}

		#endregion

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

        public override DbCommand XDbCommand(string commandText_, DbConnection aConnection = null)
        {
            DbCommand sCommand = XDbCommand();
			string commandText = commandText_.Trim();

			// 맨마지막에만 ';' 이면, 제거
			{
				int len = commandText.Length;
				if (commandText.IndexOf(";") == len - 1)
				{
					commandText = commandText.Substring(0, len - 1);
				}
			}

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



} // namespace XLog

