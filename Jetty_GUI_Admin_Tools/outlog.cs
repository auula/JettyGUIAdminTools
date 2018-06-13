using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jetty_GUI_Admin_Tools
{
    class outlog
    {
        //窗口日志
        public static void out_log(String log, TextBox text) {
            text.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff --->")+log+"\r\n");
            log_file(log);
        }
        public static void out_log(String log)
        {
            //text.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff --->") + log + "\r\n");
            log_file(log);
        }
        //文件日志
        public static void log_file(String LOG) {
            String logPath = System.IO.Directory.GetCurrentDirectory();
            System.IO.StreamWriter inpu = System.IO.File.AppendText(logPath+"//System.log");
            inpu.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff |--->") + LOG + "\r\n");
            inpu.Close();
            inpu.Dispose();
        }
    }
}
