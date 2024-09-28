using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Linq.Expressions;
using System.IO;

namespace twitchBot
{
    public partial class Form1 : Form
    {
        private Socket serverSocket;
        private byte[] buffer = new byte[1024];
        private System.Timers.Timer messageTimer;
        private ChibiController chibi;
        Random random = new Random();
        private bool boolAction = false;
        Thread pythonThread ;
        public Form1()
        {
            InitializeComponent();
            this.TransparencyKey = this.BackColor;
            chibi = new ChibiController(pictureBox1,this);

            messageTimer = new System.Timers.Timer(10000); 
            messageTimer.Elapsed += MessageTimerElapsed; // 設定事件處理程序
            messageTimer.AutoReset = true;
            messageTimer.Start();
        }
        private void MessageTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            chibi.ChibiWalk();
            
            // 生成一個隨機走路待機時間
            messageTimer.Interval = random.Next(15000, 30000);
            for (int i = 0; i < 50; i++)
            {
                //判斷chibi對於視窗兩側的位置
                if((pictureBox1.Location.X<10 && !chibi.boolRightSide) || (pictureBox1.Location.X + pictureBox1.Width > this.Width -10 && chibi.boolRightSide) || boolAction)
                {
                    break;
                }
                Thread.Sleep(120);
                pictureBox1.Invoke((MethodInvoker)delegate {
                    if(chibi.boolRightSide)
                    {
                        pictureBox1.Location = new System.Drawing.Point(pictureBox1.Location.X + 3, pictureBox1.Location.Y);
                    }
                    else
                    {
                        pictureBox1.Location = new System.Drawing.Point(pictureBox1.Location.X - 3, pictureBox1.Location.Y);
                    }
                });
            }
            if(!boolAction)
            {
                chibi.ChibiReset();
            }   
        }
        private void StartServer()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 15003);
            serverSocket.Bind(endPoint);
            serverSocket.Listen(1); 

            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            if (serverSocket == null) // 檢查是否已經關閉了 Server Socket
            {
                return;
            }
            Socket clientSocket = serverSocket.EndAccept(ar);
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
        }


        private object messageLock = new object();

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                int bytesRead = clientSocket.EndReceive(ar);
                byte[] dataBuffer = new byte[bytesRead];
                Array.Copy(buffer, dataBuffer, bytesRead);
                string data = Encoding.ASCII.GetString(dataBuffer);
                string messageBuffer = string.Empty;

                boolAction = true;
                messageTimer.Stop();

                lock (messageLock)
                {
                    messageBuffer += data;
                }

                while (messageBuffer.Contains("\n"))
                {
                    int separatorIndex = messageBuffer.IndexOf("\n");

                    if (separatorIndex >= 0) // 確保分隔符存在
                    {
                        string message;
                        lock (messageLock)
                        {
                            message = messageBuffer.Substring(0, separatorIndex);
                            messageBuffer = messageBuffer.Substring(separatorIndex + 1);
                        }
                        switch (message)
                        {
                            case "confuse":
                                chibi.ChibiConfuse();
                                Thread.Sleep(4600);
                                chibi.ChibiReset();
                                break;
                            case "headwear":
                                if (chibi.boolHeadwear == true)
                                {
                                    chibi.ChibiHeadwear();
                                    Thread.Sleep(3840);
                                }
                                else
                                {
                                    chibi.ChibiHeadwear();
                                    Thread.Sleep(2700);
                                }
                                chibi.ChibiReset();
                                break;
                            case "walk":
                                chibi.ChibiWalk();
                                for (int i = 0; i < 50; i++)
                                {
                                    Thread.Sleep(120);
                                    pictureBox1.Invoke((MethodInvoker)delegate {
                                        pictureBox1.Location = new System.Drawing.Point(pictureBox1.Location.X - 3, pictureBox1.Location.Y);
                                    });
                                }
                                chibi.ChibiReset();
                                break;
                            case string s when s.StartsWith("abc"):
                                chibi.ChibiConfuse();
                                Thread.Sleep(4600);
                                chibi.ChibiReset();
                                break;
                        }
                    }
                }
                messageTimer.Start();
                boolAction = false;
                messageBuffer = string.Empty;
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
            }
            catch (System.Net.Sockets.SocketException)
            {
                //避免關閉視窗時報錯用
            }
        }

        private void startPython()
        {
            // 開始執行 Python
            try
            {
                /*
                // string appPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))); //獲得路徑
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                string scriptPath = Path.Combine(appPath, @"..\..\python\test.py");
                string absolutePath = Path.GetFullPath(scriptPath);
                */

                string relativePath = @"..\..\python\test.py";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = relativePath,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process process = new Process
                {
                    StartInfo = startInfo
                };
                process.Start();
                process.WaitForExit();
            }
            catch (Win32Exception ex)
            {
                Console.WriteLine("執行 Python 時出現異常：");
                Console.WriteLine(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            StartServer();
            pythonThread = new Thread(() => startPython());
            pythonThread.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                serverSocket.Close();
                serverSocket = null;

                Process[] runningPythonProcesses = Process.GetProcessesByName("python"); // 根據名稱找到所有運行的 Python

                foreach (Process pythonProcess in runningPythonProcesses)
                {
                    pythonProcess.Kill(); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("關閉 Python 執行時出現異常：");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
