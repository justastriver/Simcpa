using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ISimcpa.Net
{
	/// <summary>
	/// 为OnReceive事件提供数据
	/// </summary>
	public class ReceiveEventArgs: EventArgs
	{
		private Socket socket = null;
		private ArrayList clients = null;
		private Reader reader = null;
		private Writer writer = null;
  //      private byte[] __bytes_recv = null;
		/// <summary>
		/// 当前通信端的IP地址和端口
		/// </summary>
		public string RemoteAddress
		{
			get
			{
				return socket.RemoteEndPoint.ToString();
			}
		}

		
		public ReceiveEventArgs(Socket socket, ArrayList sockets)
		{
			this.socket = socket;
			this.clients = sockets;
			reader = new Reader(this.socket);
			writer = new Writer(this.socket);
//             __bytes_recv = new byte[len];
//             Array.Copy(data, __bytes_recv, len);
           
		}

		/// <summary>
		/// 向当前客户发送信息
		/// </summary>
		/// <param name="msg">发送的信息</param>
		public void Write(string msg)
		{
			writer.Write(msg);
		}

		/// <summary>
		/// 读取当前连接客户发来的信息
		/// </summary>
		/// <returns></returns>
		public string Read()
		{
			return reader.Read(socket);
		}
        /// <summary>
        /// 读取当前连接客户发来的信息
        /// </summary>
        /// <returns></returns>
        public Stream ReadStream()
        {
            return reader.ReadStream(socket);
        }

       
	}
}
