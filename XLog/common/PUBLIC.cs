using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

}
