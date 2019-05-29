using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Runtime.InteropServices;       // https://hot-key.tistory.com/4?category=1010793
using System.Data;                          // 공통 인터페이스 , IDbConnection .. 등
using System.Data.Common;                   // 추상 클래스 , DbConnection .. 등

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
	#region dlopen for Win32 (MFC)

	public class Win32API
	{
		public struct POINTCRT // GetCaretPos
		{
			public int x;
			public int y;
		};

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool SetCaretPos(int x, int y);

		[DllImport("user32.dll")]
		public static extern int GetCaretPos(out POINTCRT lpPoint);

		//HWND FindWindowA( LPCSTR lpClassName, LPCSTR lpWindowName );
		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		//int GetWindowTextA(HWND hWnd, LPSTR lpString, int nMaxCount);
		[DllImport("user32.dll")]
		public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		// BOOL SetWindowPos(HWND hWnd, LPRECT lpRect);
		[DllImport("user32.dll")]
		public static extern bool SetWindowPos(IntPtr hWnd, ref Rect lpRect);

		//BOOL GetWindowRect( HWND hWnd /* handle to window */, LPRECT lpRect /* window coordinates */ );
		[DllImport("user32.dll")]
		public static extern bool GetwindowRect(IntPtr hWnd, out Rect lpRect);

		//UNIT GetSystemDerictory(LPSTR lpBuffer, /* 경로병을 저장할 버퍼 */ UINT uSize /* 디렉토리 버퍼의 크기 */ );
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int GetSystemDirectory([MarshalAs(UnmanagedType.LPWStr, SizeConst = 256)]StringBuilder buffer, int size);

		// BOOL EnumWindows(WNDENUMPROC lpEnumFunc, LPARMAM IParam)
		// typedef BOOL (CALLBACK* WNDENUMPROC)(HWND, LPARAM);

		// FROM: https://kspil.tistory.com/8
		[StructLayout(LayoutKind.Explicit)]
		public struct Rect
		{
			[FieldOffset(0)] public int top;
			[FieldOffset(4)] public int left;
			[FieldOffset(8)] public int bottom;
			[FieldOffset(12)] public int right;
		}

		// FROM: https://searchcode.com/file/102430309/OpenTween/NativeMethods.cs

		public enum SendMessageType : uint
		{
			WM_SETREDRAW = 0x000B,
			WM_USER = 0x400,

			TCM_FIRST = 0x1300,
			TCM_SETMINTABWIDTH = TCM_FIRST + 49,

			LVM_FIRST = 0x1000,
			LVM_SETITEMSTATE = LVM_FIRST + 43,
			LVM_GETSELECTIONMARK = LVM_FIRST + 66,
			LVM_SETSELECTIONMARK = LVM_FIRST + 67,
		}

		[DllImport("user32.dll")]
		public extern static IntPtr SendMessage(
			IntPtr hwnd,
			SendMessageType wMsg,
			IntPtr wParam,
			IntPtr lParam);

		[DllImport("user32.dll")]
		public extern static IntPtr SendMessage(
			IntPtr hwnd,
			SendMessageType wMsg,
			IntPtr wParam,
			ref LVITEM lParam);


		// FROM: LVITEM structure (Windows)
		// http://msdn.microsoft.com/en-us/library/windows/desktop/bb774760%28v=vs.85%29.aspx
		[StructLayout(LayoutKind.Sequential)]
		[BestFitMapping(false, ThrowOnUnmappableChar = true)]
		public struct LVITEM
		{
			public uint mask;
			public int iItem;
			public int iSubItem;
			public LVIS state;
			public LVIS stateMask;
			public string pszText;
			public int cchTextMax;
			public int iImage;
			public IntPtr lParam;
			public int iIndent;
			public int iGroupId;
			public uint cColumns;
			public uint puColumns;
			public int piColFmt;
			public int iGroup;
		}

		// FROM: List-View Item States (Windows)
		// http://msdn.microsoft.com/en-us/library/windows/desktop/bb774733%28v=vs.85%29.aspx
		[Flags]
		public enum LVIS : uint
		{
			SELECTED = 0x02,
		}

	}

	#endregion


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
		
		#region Common Function For xx
		
		#endregion

	} // class PUBLIC

	#region Global Configure / Default Values

	public class Defaults
	{
		public const string Version = "0.1 (2019.05.23)";

		public const int Timeout = 60;
		public const string TimeFormat = "YYYY/MM/DD HH24:MI:SS";
		public const string DateFormat = "YYYY/MM/DD";
		public const string NullText = "(NULL)";

		//public const string SQL_ALT_C_ORA = "";
	}
	
	#endregion

	#region XDb

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

		public string mUserName = null;
		public string mSchemaName = null; // ORACLE의 경우 USER와 SCHEMA가 동일한 구조

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

		public XDb(XDbConnType type)
		{
			this.mConnType = type;
		}

		public override DbConnection XDbConnection(string connStr_ = null)
		{
			if (mConnection != null)
			{
				// TODO: 임시코드, 종료처리할지, 오류낼지.
				mConnection.Close();
				mConnection = null;
			}

			string connStr = null;

			if (connStr_ == null)
			{
				connStr = mConnStr;
			}
			else
			{
				connStr = connStr_;
			}

			if (mConnType == XDbConnType.ORACLE)
			{
				mConnection = new OracleConnection(connStr);
			}
			else if (mConnType == XDbConnType.ALTIBASE)
			{
				mConnection = new AltibaseConnection(connStr);
			}
			else if (mConnType == XDbConnType.MSSQL)
			{
				mConnection = new SqlConnection(connStr);
			}
			else if (mConnType == XDbConnType.TIBERO)
			{
				mConnection = new OleDbConnectionTbr(connStr);
			}
			else if (mConnType == XDbConnType.OLEDB)
			{
				mConnection = new OleDbConnection(connStr);
			}

			return mConnection;
		}

		public override DbCommand XDbCommand()
		{
			DbCommand cmd = null;

			if (mConnType == XDbConnType.ORACLE)
			{
				cmd = new OracleCommand();
			}
			else if (mConnType == XDbConnType.ALTIBASE)
			{
				cmd = new AltibaseCommand();
			}
			else if (mConnType == XDbConnType.MSSQL)
			{
				cmd = new SqlCommand();
			}
			else if (mConnType == XDbConnType.TIBERO)
			{
				cmd = new OleDbCommandTbr();
			}
			else if (mConnType == XDbConnType.OLEDB)
			{
				cmd = new OleDbCommand();
			}

			if (cmd is OracleCommand cmd_)
			{
				//TODO: 설정으로 처리 ( 현재는 용도는 없다 )
				//cmd_.InitialLOBFetchSize = 4000;
				cmd_.InitialLONGFetchSize = 4000;
			}

			return cmd;
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

			if (false
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
			DbDataAdapter adapter = null;

			if (mConnType == XDbConnType.ORACLE)
			{
				adapter = new OracleDataAdapter();
			}
			else if (mConnType == XDbConnType.ALTIBASE)
			{
				adapter = new AltibaseDataAdapter();
			}
			else if (mConnType == XDbConnType.MSSQL)
			{
				adapter = new SqlDataAdapter();
			}
			else if (mConnType == XDbConnType.TIBERO)
			{
				adapter = new OleDbDataAdapterTbr();
			}
			else if (mConnType == XDbConnType.OLEDB)
			{
				adapter = new OleDbDataAdapter();
			}

			return adapter;
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

			return sTransaction;
		}

		/*
		 * ## Copy DataTable for DataGridView 
		 * 
		 *	(1) typeof(Byte[])) : RAW ( ORACLE )
		 *	(2) .. 
		 *	
		 *	TODO: 함수위치를 XDb가 아닌 PUBLIC 으로 가야하나 ?? 
		 */
		public static DataTable ConvertDataTable(DataTable data)
		{
			bool find = false;

			for (int ic = 0; ic < data.Columns.Count; ic++)
			{
				if (data.Columns[ic].DataType == typeof(Byte[]))
				{
					find = true;
					break;
				}
			}

			if ( ! find )
			{
				return data; // TODO: 참조 리턴이 가능할까? 성능/메모리 2배 소요됨.
			}

			DataTable newData = new DataTable();
			Type dataType;

			for (int ic = 0; ic < data.Columns.Count; ic++)
			{
				dataType = data.Columns[ic].DataType;
				if (dataType == typeof(Byte[]))
				{
					dataType = typeof(String);
				}
				newData.Columns.Add(data.Columns[ic].ColumnName, dataType);
			}

			for (int ir = 0; ir < data.Rows.Count; ir++)
			{
				DataRow row = newData.NewRow();

				for (int ic = 0; ic < data.Columns.Count; ic++)
				{
					dataType = data.Columns[ic].DataType;

					if (!DBNull.Value.Equals(data.Rows[ir][ic]))
					{
						if (dataType == typeof(Byte[])) // WHEN ORACLE 'RAW' TYPE
						{
							Byte[] bytes = (Byte[])data.Rows[ir][ic];
							//row[ic] = System.Text.Encoding.UTF8.GetString(bytes);
							//row[ic] = Convert.ToBase64String(bytes);
							row[ic] = BitConverter.ToString(bytes).Replace("-", ""); // 16진수
						}
						else
						{
							row[ic] = data.Rows[ir][ic];
						}
					}
					else
					{
						row[ic] = data.Rows[ir][ic];
					}				
				}
				newData.Rows.Add(row);
			}
			
			return newData;
		}

		#endregion //XDb
	}

} // namespace XLog

