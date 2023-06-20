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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn2
{
    public partial class Form2 : Form
    {
        private TcpListener listener;
        private TcpClient client;
        private Thread clientThread;
        private bool connected;

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        public Form2()
        {
            InitializeComponent();
            connected = true;
            Thread thread = new Thread(ListenerThread);
            thread.Start();
        }
        private void ListenerThread()
        {
            try
            {
                listener = new TcpListener(IPAddress.Any, 1234);
                listener.Start();
                client = listener.AcceptTcpClient();
                connected = true;
                clientThread = new Thread(new ThreadStart(ClientThread));
                clientThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClientThread()
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                while (connected)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string data = System.Text.Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        string[] coordinates = data.Split(',');
                        int x = int.Parse(coordinates[0]);
                        int y = int.Parse(coordinates[1]);
                        SetCursorPos(x, y);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Thread listenerThread = new Thread(new ThreadStart(ListenerThread));
            listenerThread.Start();
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            connected = false;
            if (client != null)
            {
                client.Close();
            }
            if (listener != null)
            {
                listener.Stop();
            }
            if (clientThread != null && clientThread.IsAlive)
            {
                clientThread.Abort();
            }
        }
    }
}
