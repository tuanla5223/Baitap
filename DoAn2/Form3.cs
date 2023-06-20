using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn2
{
    public partial class Form3 : Form
    {
        private TcpClient client;
        private bool connected;

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        public Form3()
        {
            InitializeComponent();
            connected = false;
        }
        private void Connect()
        {
            try
            {
                client = new TcpClient();
                client.Connect(IPAddress.Parse(textBox1.Text), 8080);
                connected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendCoordinates(int x, int y)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                string data = x.ToString() + "," + y.ToString();
                byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Form3_MouseMove(object sender, MouseEventArgs e)
        {
            if (connected)
            {
                SendCoordinates(e.X, e.Y);
            }
        }
        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            connected = false;
            if (client != null)
            {
                client.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connect();
        }
    }
}
