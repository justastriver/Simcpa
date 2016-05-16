using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Net.Sockets;

namespace ISimcpa.Net
{
	/// <summary>
	/// Reader 的摘要说明。
	/// </summary>
	internal class Reader
	{
		private NetworkStream stream = null;
		private string strMsg = "";
		private bool hasRead = false;

		public Reader(Socket socket)
		{
			stream = new NetworkStream(socket);
		}

		/// <summary>
		/// 读取当前连接客户发来的信息
		/// </summary>
		/// <returns></returns>
		public string Read()
		{
			if (!hasRead)
			{
				StreamReader reader = new StreamReader(stream);
                
				strMsg = reader.ReadLine();
				hasRead = true;

			}
			return strMsg;
		}
        public string Read(Socket socket)
        {
            if (!hasRead)
            {
                //StreamReader reader = new StreamReader(stream);
                //strMsg = reader.ReadToEnd();
                //strMsg = reader.ReadLine();
                //hasRead = true;

                
                int buflen = socket.Available;
                Byte[] buffer = new Byte[buflen];
                int recv_bytes = socket.Receive(buffer, 0, buflen, SocketFlags.None);
                if (recv_bytes < buflen)
                {

                }
                strMsg = System.Text.Encoding.Default.GetString(buffer);
                hasRead = true;
            }
            return strMsg;
        }
        /// <summary>
        /// 读取当前连接客户发来的信息
        /// </summary>
        /// <returns></returns>
        public Stream ReadStream(Socket socket)
        {
            //return stream;
            int buflen = socket.Available;
            Byte[] buffer = new Byte[buflen];
            int recv_bytes = socket.Receive(buffer, 0, buflen, SocketFlags.None);
            MemoryStream data = new MemoryStream(buffer);
            return data;
        }

		internal void Close()
		{
			stream.Close();
		}

	}
}
