using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jetty_GUI_Admin_Tools
{
    public partial class AI语音介绍 : Form
    {
        public AI语音介绍()
        {
            InitializeComponent();
        }

        private void AI语音介绍_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.ai;
            System.Media.SoundPlayer sp = new SoundPlayer();
            sp.Stream = Properties.Resources.hi;
            sp.Play();
        }
    }
}
