using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIClient
{
    public partial class GUIClient : Form
    {
        UdpClient client;
        public GUIClient()
        {
            InitializeComponent();
            client = new UdpClient("76.190.248.85");
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            client.sendMessage("up");
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            client.sendMessage("right");
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            client.sendMessage("down");
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            client.sendMessage("left");
        }

        private void zoomInButton_Click(object sender, EventArgs e)
        {
            client.sendMessage("zoomin");
        }

        private void zoomOutButton_Click(object sender, EventArgs e)
        {
            client.sendMessage("zoomout");
        }

        private void GUIClient_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void GUIClient_MouseDown(object sender, MouseEventArgs e)
        {
            this.Location = new Point(e.X, e.Y);
        }


    }
}
