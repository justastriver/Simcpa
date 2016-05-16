using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ISimcpa.Util;
using System.IO;
using System.Collections.Generic;

namespace ISimcpa.Net
{
    public delegate void SocketEventHandler(object sender, SocketEventArgs e);
    //public delegate void ReceiveEventHandler(object sender, ReceiveEventArgs e);
    public delegate void ReceiveEventHandler(object sender, Stream stream);

    /// <summary>
	/// 客户端组件
	/// </summary>
	public class Client
	{
		private Socket client = null;
		private Thread thread = null;
		private bool isConnected = false;
		private Reader reader = null;
		private Writer writer = null;

		private string hostAddress = null;
		private int port = 0;
		public event ReceiveEventHandler OnReceive = null;
        //public event ReceiveEventHandler2 OnReceive2 = null;
		public event SocketEventHandler OnServerClosed = null;

        private readonly object locker = new object();
        private readonly object dataLocker = new object();

        private int MAX_BUF_RECV_LEN = 100 * 1024;
        private List<byte> __net_bytes = new List<byte>();
        private int MAX_MEM_SIZE = 20 * 1024 * 1024;//20Mb


		/// <summary>
		/// 服务器地址
		/// </summary>
		public string HostAddress
		{
			get
			{
				return hostAddress;
			}
			set
			{
				hostAddress = value;
			}
		}

		/// <summary>
		/// 服务器监听端口
		/// </summary>
		public int Port
		{
			get
			{
				return port;
			}
			set
			{
				port = value;
			}
		}

		/// <summary>
		/// 是否与服务器处于连接状态
		/// </summary>
		public bool IsConnected
		{
			get
			{
				return isConnected;
			}
		}

		public Client()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		public Client(string hostAddress, int port)
		{
			this.hostAddress = hostAddress;
			this.port = port;
		}
        
        private void HandleRecvMessage()
        {
            int counter = 0;
            int err_counter = 0;
            while(isConnected)
            {
                if(__net_bytes.Count > 4)//不用锁定
                {
                    try
                    {
                        //读取长度
                        MemoryStream stream = null;
                        lock (dataLocker)
                        {
                            byte[] msg_len_bytes = new byte[4];
                            
                            Array.Copy(__net_bytes.ToArray(), msg_len_bytes, 4);
                            //if (BitConverter.IsLittleEndian)   //判断计算机结构的 endian 设置
                            //    Array.Reverse(msg_len_bytes);    //转换排序

                            int len = System.BitConverter.ToInt32(msg_len_bytes, 0);
                            if (len > 0 && len < __net_bytes.Count)
                            {
                                stream = new MemoryStream(__net_bytes.GetRange(4, len).ToArray());
                                __net_bytes.RemoveRange(0, len + 4);
                                counter++;
                            }
                            else
                            {
                                err_counter++;
                            }
                            if (__net_bytes.Count>MAX_MEM_SIZE)
                            {
                                __net_bytes.Clear();
                                Logger.Error("memory over flow, just clear the buffer");
                            }

                        }
                        if (null != stream)
                        {
                            OnReceive(this, stream);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Logger.Error("handle message failed: " + ex.Message);
                    }
                    
                }
                Thread.Sleep(100);
            }
            Logger.Debug("recv counter: " + counter.ToString() + ", err counter: " + err_counter.ToString());
            
            //ReceiveEventArgs e = new ReceiveEventArgs(client, null);           
            //OnReceive2(this, new MemoryStream(buff,0,recv_len));
        }
		/// <summary>
		/// 连接服务器
		/// </summary>
		public void Connect()
		{
            
			client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			client.Connect(new IPEndPoint(IPAddress.Parse(hostAddress), port));
			isConnected = true;
			reader = new Reader(client);
			writer = new Writer(client);
			if (OnReceive != null)
			{
				thread = new Thread(new ThreadStart(ReceiveData));
				thread.Name = "接收数据";
				thread.Start();
                Thread thread_handle_net_recv_message = new Thread(new ThreadStart(HandleRecvMessage));
                thread_handle_net_recv_message.Start();
			}
		}

		/// <summary>
		/// 连接服务器
		/// </summary>
		/// <param name="hostAddress">服务器地址</param>
		/// <param name="port">服务器监听端口</param>
		public void Connect(string hostAddress, int port)
		{
			this.hostAddress = hostAddress;
			this.port = port;
			Connect();
		}

		/// <summary>
		/// 关闭与服务器的连接
		/// </summary>
		public void Close()
		{
			isConnected = false;
            Thread.Sleep(5000);
            try
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                    Logger.Debug("正在关闭Reader");
                }
                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                    Logger.Debug("正在关闭writer");
                }
                

                if (client != null)
                {
                    try
                    {
                        Logger.Debug("正在shutdown");
                        client.Shutdown(SocketShutdown.Both);
                    }
                    catch (System.Exception ex)
                    {
                        Logger.Warn("正在shutdown出现异常："+ ex.Message);
                    }
                    
                    client.Close();
                    client = null;
                    Logger.Debug("正在关闭client");
                }
            }
            catch (System.Exception ex)
            {
                Logger.Debug("关闭socket client 出现异常：" + ex.Message);
            }
            Thread.Sleep(1000);
			
		}

        #region 发送数据的接口
        /// <summary>
		/// 向服务器发送信息
		/// </summary>
		/// <param name="data"></param>
		public void Write(string msg)
		{
			if (writer != null)
				writer.Write(msg);
		}
        /// <summary>
        /// 向服务器发送信息
        /// </summary>
        /// <param name="data"></param>
        public bool Write(Byte[] data, long len)
        {
            
            try
            {
                if (writer != null)
                {

//                     byte[] msg_len_bytes = new byte[4];
//                     Array.Copy(__net_bytes.ToArray(), msg_len_bytes, 4);
//                     if (BitConverter.IsLittleEndian)   //判断计算机结构的 endian 设置
//                         Array.Reverse(msg_len_bytes);    //转换排序

                    writer.Write(data, len);
                }
            }
            catch (System.Exception ex)
            {
                Logger.Error("发送网络数据,异常信息：" + ex.Message);
                return false;
            }
            return true;
        }
		
        #endregion

        #region 读数据
        /// <summary>
		/// 读取服务器发来的信息
		/// </summary>
		/// <returns></returns>
		public string Read()
		{
			string returnValue = null;
			if (reader != null)
				returnValue = reader.Read();
			return returnValue;
		}
        #endregion

        #region 读网络数据的线程处理函数
       
        /// <summary>
        /// 读网络数据的线程处理函数
        /// </summary>
        private void ReceiveData()
		{
            int recv_len = 0;
            byte[] buff = new byte[MAX_BUF_RECV_LEN];
            while (isConnected)
            {
                lock (locker)
                {
                    try
                    {
                        recv_len = client.Receive(buff, System.Net.Sockets.SocketFlags.None);
                        
                        if (recv_len != 0)
                        {
                            byte[] tmp = new byte[recv_len];
                            Array.Copy(buff, tmp, recv_len);
                            lock (dataLocker)
                            {
                                __net_bytes.AddRange(tmp);
                                tmp = null;
                            }
                           
                        }
                        else
                        {
                            isConnected = false;
                            if (OnServerClosed != null)
                                OnServerClosed(this, new SocketEventArgs(hostAddress, port));
                        }
                       
                    }
                    catch (System.Exception ex)
                    {
                        Logger.Error("读取网络数据失败,异常信息：" + ex.Message);
                        isConnected = false;
                        if (OnServerClosed != null)
                            OnServerClosed(this, new SocketEventArgs(hostAddress, port));
                        //break;
                    }

                }
            }
            buff = null;
			//Close();
        }
        #endregion

    }
}
