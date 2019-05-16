using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XLog
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			//[NOTE] Starting Position ..
			//Application.Run(new Form1()); return;
			//Application.Run(new SQLTool_Alt_C()); return;

			//Application.Run(new MainForm());
			//Application.Run(new QueryForm());
			Application.Run(new SQLTool());
        }

        /// 참고 URLs
        /// + https://iconverticons.com/online/
        ///     png 파일을 ico 으로 변환
        /// + 
    }
}
