using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jetty_GUI_Admin_Tools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Timer timer = null;
        private void Form1_Load(object sender, EventArgs e)
        {
            outlog.out_log("启动了软件");
            pictureBox1.Image = Properties.Resources.logo;
            // 3000 毫秒，即3秒
            this.timer = new Timer();
            this.timer.Interval = 5000;
            // 设置运行
            this.timer.Enabled = true;
            this.timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            this.Hide();
            main m = new main();
            m.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            outlog.out_log("启动窗口点击了退出按钮");
            System.Environment.Exit(0);
            Application.Exit();
        }
    }
}
