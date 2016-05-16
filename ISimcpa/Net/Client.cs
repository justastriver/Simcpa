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
	/// �ͻ������
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
		/// ��������ַ
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
		/// �����������˿�
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
		/// �Ƿ����������������״̬
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
			// TODO: �ڴ˴���ӹ��캯���߼�
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
                if(__net_bytes.Count > 4)//��������
                {
                    try
                    {
                        //��ȡ����
                        MemoryStream stream = null;
                        lock (dataLocker)
                        {
                            byte[] msg_len_bytes = new byte[4];
                            
                            Array.Copy(__net_bytes.ToArray(), msg_len_bytes, 4);
                            //if (BitConverter.IsLittleEndian)   //�жϼ�����ṹ�� endian ����
                            //    Array.Reverse(msg_len_bytes);    //ת������

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
		/// ���ӷ�����
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
				thread.Name = "��������";
				thread.Start();
                Thread thread_handle_net_recv_message = new Thread(new ThreadStart(HandleRecvMessage));
                thread_handle_net_recv_message.Start();
			}
		}

		/// <summary>
		/// ���ӷ�����
		/// </summary>
		/// <param name="hostAddress">��������ַ</param>
		/// <param name="port">�����������˿�</param>
		public void Connect(string hostAddress, int port)
		{
			this.hostAddress = hostAddress;
			this.port = port;
			Connect();
		}

		/// <summary>
		/// �ر��������������
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
                    Logger.Debug("���ڹر�Reader");
                }
                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                    Logger.Debug("���ڹر�writer");
                }
                

                if (client != null)
                {
                    try
                    {
                        Logger.Debug("����shutdown");
                        client.Shutdown(SocketShutdown.Both);
                    }
                    catch (System.Exception ex)
                    {
                        Logger.Warn("����shutdown�����쳣��"+ ex.Message);
                    }
                    
                    client.Close();
                    client = null;
                    Logger.Debug("���ڹر�client");
                }
            }
            catch (System.Exception ex)
            {
                Logger.Debug("�ر�socket client �����쳣��" + ex.Message);
            }
            Thread.Sleep(1000);
			
		}

        #region �������ݵĽӿ�
        /// <summary>
		/// �������������Ϣ
		/// </summary>
		/// <param name="data"></param>
		public void Write(string msg)
		{
			if (writer != null)
				writer.Write(msg);
		}
        /// <summary>
        /// �������������Ϣ
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
//                     if (BitConverter.IsLittleEndian)   //�жϼ�����ṹ�� endian ����
//                         Array.Reverse(msg_len_bytes);    //ת������

                    writer.Write(data, len);
                }
            }
            catch (System.Exception ex)
            {
                Logger.Error("������������,�쳣��Ϣ��" + ex.Message);
                return false;
            }
            return true;
        }
		
        #endregion

        #region ������
        /// <summary>
		/// ��ȡ��������������Ϣ
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

        #region ���������ݵ��̴߳�����
       
        /// <summary>
        /// ���������ݵ��̴߳�����
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
                        Logger.Error("��ȡ��������ʧ��,�쳣��Ϣ��" + ex.Message);
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
