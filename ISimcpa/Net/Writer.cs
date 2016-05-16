using System;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ISimcpa.Net
{
	/// <summary>
	/// Writer 的摘要说明。
	/// </summary>
	internal class Writer
	{

		private NetworkStream stream = null;

		public Writer(Socket socket)
		{
			stream = new NetworkStream(socket);
		}

		/// <summary>
		/// 发送信息
		/// </summary>
		/// <param name="msg"></param>
		public void Write(string msg)
		{
			StreamWriter writer = new StreamWriter(stream);
			writer.WriteLine(msg);
			writer.Flush();
		}
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="msg"></param>
        public void Write(Byte[] data, long len)
        {
           
            stream.Write(data, 0, (int)len);
            stream.Flush();
        }

		internal void Close()
		{
			stream.Close();
		}
	}
}
