using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        IPAddress address = IPAddress.Parse("192.168.137.1");
        private const int PORT_NUMBER = 3129;
        private const string RASP_IP = "192.168.137.48";
        Thread childThread;

        public Form1()
        {
            InitializeComponent();
            txtIP.Text = RASP_IP;
            txtPort.Text = PORT_NUMBER + "";
            //iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3129);
            try
            {
                ThreadStart childref = new ThreadStart(Server);
                Console.WriteLine("Trong Main Thread: tao Thread con.");
                childThread = new Thread(childref);
                childThread.Start();
            }

            catch (ThreadAbortException e)
            {
                Console.WriteLine("Thread Abort Exception!!!");
            }
            finally
            {
                Console.WriteLine("Khong the bat Thread Exception!!!");
            }
            
        }

        void Server()
        {
            TcpListener listener = new TcpListener(address, PORT_NUMBER);

            // 1. listen
            listener.Start();
            //Console.WriteLine("Server started on " + listener.LocalEndpoint);
            //Console.WriteLine("Waiting for a connection...");
            textReceive.Text += "Server started on ";

            while (true)
            {
                Socket socket = listener.AcceptSocket();
                Console.WriteLine("Connection received from " + socket.RemoteEndPoint);

                var stream = new NetworkStream(socket);
                var reader = new StreamReader(stream);
                // 2. receive
                string str = reader.ReadLine();

                textReceive.Text =  textReceive.Text + "\n[" + DateTime.Now.ToString() + "]Reveived:" + str;

                // 3. send
                 //writer.WriteLine("Hello " + str);
            }
        }

        private void text_TextChanged(object sender, EventArgs e)
        {
            textReceive.SelectionStart = textReceive.Text.Length;
            textReceive.ScrollToCaret();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();

            // 1. connect
            client.Connect(txtIP.Text, Convert.ToInt32(txtPort.Text));
            Stream stream = client.GetStream();
            var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            //Console.Write(s)
            // 2. send
            writer.WriteLine(txtSend.Text);
            stream.Close();
            client.Close();
            MessageBox.Show(" xong!");
        }

        private void textReceive_VScroll(object sender, EventArgs e)
        {
           // txtSend.Text = "ffs";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //childThread.Abort();

            Application.Exit();
        }
    }
}
