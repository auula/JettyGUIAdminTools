using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jetty_GUI_Admin_Tools
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }
        //run_jetty_id
        string P_id = null;
        //jetty的开关防止重复启动 能被2正除
        long jetty_flag = 0;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        //窗口随意脱动

        private void main_MouseDown_1(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x0112, 0xF012, 0);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //程序exit
            OnFormClosing(null);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要退出系统吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                string Jetty_PATH = Set_OS_Path.GetSysEnvironmentByName("Jetty_PATH");
                string deleteflie = @Jetty_PATH + "\\start.bat";
                if (File.Exists(deleteflie))//必须判断要复制的文件是否存在
                {
                    utils.DeleteFile(deleteflie);
                }
                utils.kill_ID(P_id);
                outlog.out_log("退出程序并且停止线程", textBox1);
                System.Environment.Exit(0);
                Application.ExitThread();
            }
            outlog.out_log("退出程序，点击了取消按钮", textBox1);
        }
        //最小化
        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            outlog.out_log("最小化程序", textBox1);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://jq.qq.com/?_wv=1027&k=5Vd1Bpl");
            outlog.out_log("操作了官方QQ组件", textBox1);
        }

        private void 打赏codingToolStripMenuItem_Click(object sender, EventArgs e)

        {
            outlog.out_log("打开了打赏窗口", textBox1);
            pay pay = new pay();
            pay.Show();
            System.Media.SoundPlayer sp = new SoundPlayer();
            sp.Stream = Properties.Resources.pay;
            sp.Play();
        }

        private void 联系作者ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            outlog.out_log("点击了联系作者", textBox1);
            System.Diagnostics.Process.Start("https://wpa.qq.com/msgrd?v=3&uin=2420498526&site=qq&menu=yes");
        }

        private void 软件语言介绍ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            outlog.out_log("AI语音介绍", textBox1);
            AI语音介绍 aI = new AI语音介绍();
            aI.Show();
            
        }
        string www = null;
        

        private void main_Load(object sender, EventArgs e)
        {

            string txtContent = GetVersion("https://api.ilaok.com/jetty/version.txt");
            outlog.out_log("程序启动初始化检测版本，当前服务器版本为:"+txtContent);
            string L = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //MessageBox.Show(L);
            if (txtContent != L)
            {
                MessageBox.Show("软件已经发布更新!请去百度网盘下载！");
                System.Diagnostics.Process.Start("https://pan.baidu.com/s/1dGcJORn");
                System.Environment.Exit(0);
            }
            




            //获取系统变量
            string jh = Set_OS_Path.GetSysEnvironmentByName("JAVA_HOME");
           string cp = Set_OS_Path.GetSysEnvironmentByName("CLASS_PATH");
           //Set_OS_Path.SetPathAfter(jh + "\Bin");
            //MessageBox.Show(jh.ToString());
            //MessageBox.Show(cp.ToString());
            if (jh.Equals("")||cp.Equals("")) {
                outlog.out_log("MAIN检测系统环境变量，没有检测到！");
                MessageBox.Show("当前系统没有配置Java环境变量或者没有安装JDK！\n请安装JDK1.8以上版本并且配置环境变量！","警告:");
                System.Diagnostics.Process.Start("http://www.oracle.com/technetwork/java/javase/downloads/index.html");
                //停止当前程序的所有线程
                Application.ExitThread();
                //程序退出运行
                System.Environment.Exit(0);
            }
            outlog.out_log("MAIN检测系统环境变量，环境正常！" + jh + "    ||  "+cp);
            
            outlog.out_log("\nRun_Path = "+ System.IO.Directory.GetCurrentDirectory(), textBox1);
            outlog.out_log("\nJAVA_HOME = " + jh, textBox1);
            label1.Text ="OSversion:" +Environment.OSVersion.ToString();
            
            label2.Text ="公共语言运行时版本(CLR):" + Environment.Version.ToString();



            string[] port = new string[] {"80","8080","8088","8091" };
            for (int i=0;i<port.Length;i++)
            {
                comboBox1.Items.Add(port[i]);
            }
            www = "http://localhost:" + comboBox1.Text;
            label4.Text = www;
        }
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.eclipse.org/jetty/");
            outlog.out_log("打开了http://www.eclipse.org/jetty/", textBox1);
        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(www);
            outlog.out_log("打开了"+www, textBox1);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            string jetty = Set_OS_Path.GetSysEnvironmentByName("Jetty_PATH");
            if (jetty.Equals(""))
            {
                if (File.Exists(@System.IO.Directory.GetCurrentDirectory()+ "\\ICSharpCode.SharpZipLib.dll"))
                {
                    //MessageBox.Show("文件存在");
                }
                else
                {
                    byte[] temp = Properties.Resources.ICSharpCode_SharpZipLib;
                    System.IO.FileStream fileStream = new System.IO.FileStream(@System.IO.Directory.GetCurrentDirectory() + "\\ICSharpCode.SharpZipLib.dll", System.IO.FileMode.CreateNew);
                    fileStream.Write(temp, 0, (int)(temp.Length));
                    fileStream.Close();
                }
                utils.outfile();
            }
            else
            {
                MessageBox.Show("你的计算机已经安装Jetty！请不要重复安装！","提示:");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            string jetty = Set_OS_Path.GetSysEnvironmentByName("Jetty_PATH");
            if (jetty.Equals(""))
            {
                MessageBox.Show("请先安装Jetty服务器！","提示:");
            }
            else
            {
                if (jetty_flag % 2 == 0)
                {
                    //如果是true说明，可以启动Jetty
                    string Jetty_PATH = Set_OS_Path.GetSysEnvironmentByName("Jetty_PATH");
                    /*string cmdshell_cdpath = "cd " + Jetty_PATH + "\\demo-base\\";
                    string cmdshell_runjetty = "java -jar " + Jetty_PATH + "\\start.jar";
                   string cmdmess =utils.Run_CMD(cmdshell_cdpath,cmdshell_runjetty);
                    MessageBox.Show(cmdmess);*/
                    System.Diagnostics.ProcessStartInfo myStartInfo = new System.Diagnostics.ProcessStartInfo();
                    myStartInfo.FileName = @Jetty_PATH + "\\demo-base\\start_dome.bat";
                    //myStartInfo.FileName = "C:\\Users\\coding\\Desktop\\ip.bat";
                    System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                    myProcess.StartInfo = myStartInfo;
                    myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    myProcess.Start();
                    P_id = myProcess.Id.ToString();
                    //MessageBox.Show(P_id);
                    //"/c \"TASKKILL /F /PID" + p.Id + " /T\"";
                    System.Diagnostics.Process.Start("http://localhost:8080");
                    jetty_flag += 1;
                    label7.ForeColor = System.Drawing.Color.Green;
                    label7.Text = "正在运行";
                    label4.Text = "http://localhost:8080";
                    outlog.out_log("默认启动了jetty", textBox1);
                }
                else
                {
                    MessageBox.Show("Jetty正在运行！", "警告:");
                }
            }


            
        }

        private void button5_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择你需要部署的war包文件！";
            ofd.Filter = @"文本文件|*.war";
            ofd.ShowDialog();
            //MessageBox.Show(ofd.FileName);
            //ofd.FileNames;


           bool flag= utils.deploy_war(ofd.FileName.ToString(), System.IO.Path.GetFileNameWithoutExtension(ofd.FileName));

            if (flag)
            {
                MessageBox.Show("war包部署成功！请重新启动服务器！", "部署成功！");
                outlog.out_log("war包部署成功！请重新启动服务器！", textBox1);
            }

            /*MessageBox.Show(name[0].ToString());


            string name = null;
            string warpath = null;
            for (int i=0;i<war.Length;i++)
            {
                name = System.IO.Path.GetFileNameWithoutExtension(war[i]);
                warpath = war[i];
                MessageBox.Show("MZ"+name+"P"+warpath);
            }

            //string name=System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);
            //MessageBox.Show(name);
            */

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string jetty = Set_OS_Path.GetSysEnvironmentByName("Jetty_PATH");
            if (jetty.Equals(""))
            {
                MessageBox.Show("请先安装Jetty服务器！", "提示:");
            }
            else
            {
                Regex re = new Regex(@"^\d{4}$");//实例化一个Regex对象
                String PORT = comboBox1.Text;
                if (PORT.Equals("80"))
                {
                    start();
                    outlog.out_log("正常启动了Jetty了", textBox1);
                }
                else
                {
                    bool port = re.IsMatch(PORT);
                    //MessageBox.Show(port.ToString());
                    if (port)
                    {
                        start();
                        outlog.out_log("正常启动了Jetty了", textBox1);
                        //return;
                    }
                    else
                    {
                        MessageBox.Show("启动失败！你端口必须是4位数字或者是80端口！", "提示:");
                    }

                }
            }
            


            

        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            if (jetty_flag % 2 == 0)
            {
                //如果是true说明程序没有开始运行
                MessageBox.Show("请先运行Jetty！","警告:");
            }
            else
            {
                utils.kill_ID(P_id);
                MessageBox.Show("停止Jetty成功！","警告:");
                outlog.out_log("停止Jetty了", textBox1);
                jetty_flag +=1;
                label7.Text = "未运行";
                label7.ForeColor = System.Drawing.Color.Red;
            }
            
        }
        public  void start()
        {
            


            if (jetty_flag % 2 == 0)
            {
                string Jetty_PATH = Set_OS_Path.GetSysEnvironmentByName("Jetty_PATH");
                // 创建文件
                FileStream fs = new FileStream(@Jetty_PATH + "\\start.bat", FileMode.OpenOrCreate, FileAccess.ReadWrite); //可以指定盘符，也可以指定任意文件名，还可以为word等文件
                StreamWriter sw = new StreamWriter(fs); // 创建写入流
                sw.WriteLine("cd "+Jetty_PATH);
                sw.WriteLine("java -jar start.jar jetty.http.port=" + comboBox1.Text); // 写入Hello World
                sw.Close(); //关闭文件
                System.Diagnostics.ProcessStartInfo myStartInfo = new System.Diagnostics.ProcessStartInfo();
                myStartInfo.FileName = @Jetty_PATH + "\\start.bat";
                System.Diagnostics.Process myProcess = new System.Diagnostics.Process();
                myProcess.StartInfo = myStartInfo;
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                myProcess.Start();
                P_id = myProcess.Id.ToString();
                //MessageBox.Show(P_id);
                System.Diagnostics.Process.Start("http://localhost:"+comboBox1.Text+"/test");
                
                label4.Text = "http://localhost:"+comboBox1.Text;
                jetty_flag += 1;
                label7.ForeColor = System.Drawing.Color.Green;
                label7.Text = "正在运行";
            }
            else
            {
                MessageBox.Show("你已经启动了一台Jetty！请先关闭再启动！！", "警告:");  
            }


        }

        private void 软件详情ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 检测更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //不允许链接，你自己把链接贴进去。
            outlog.out_log("点击了检测更新！" +
                "", textBox1);
            string txtContent = GetVersion("https://api.ilaok.com/jetty/version.txt");

            string L = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            //MessageBox.Show(L);
            if (txtContent!=L)
            {
                MessageBox.Show("软件已经发布更新!请去百度网盘下载！");
                System.Diagnostics.Process.Start("https://pan.baidu.com/s/1dGcJORn");
            }
            else
            {
                MessageBox.Show("当前已是最新版本！如果有问题请联系软件开发者:丁烁 QQ：2420498526","coding提示你:");
            }
        }
        private string GetVersion(string strURL)
        {
            HttpWebRequest request;
            // 创建一个<a href="https://www.baidu.com/s?wd=HTTP%E8%AF%B7%E6%B1%82&tn=44039180_cpr&fenlei=mv6quAkxTZn0IZRqIHckPjm4nH00T1Y4m19Bmyw9nvDzPj6dPhmL0ZwV5Hcvrjm3rH6sPfKWUMw85HfYnjn4nH6sgvPsT6KdThsqpZwYTjCEQLGCpyw9Uz4Bmy-bIi4WUvYETgN-TLwGUv3EnWDkrjn1nHbsn1czPWDdPHTYPs" target="_blank" class="baidu-highlight">HTTP请求</a>
            request = (HttpWebRequest)WebRequest.Create(strURL);
            HttpWebResponse response;
            response = (HttpWebResponse)request.GetResponse();
            System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            string responseText = myreader.ReadToEnd();
            myreader.Close();
            return responseText;
        }

        private void 官方文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            
        }

        private void 官方文档ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://api.ilaok.com/jetty/Jetty%E5%9B%BE%E5%BD%A2%E5%8C%96%E7%AE%A1%E7%90%86%E5%B7%A5%E5%85%B7_%E4%BD%BF%E7%94%A8%E5%BF%85%E7%9C%8B.pdf");
        }
    }
}
