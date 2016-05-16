using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ISimcpa.Net
{
	/// <summary>
	/// ΪOnReceive�¼��ṩ����
	/// </summary>
	public class ReceiveEventArgs: EventArgs
	{
		private Socket socket = null;
		private ArrayList clients = null;
		private Reader reader = null;
		private Writer writer = null;
  //      private byte[] __bytes_recv = null;
		/// <summary>
		/// ��ǰͨ�Ŷ˵�IP��ַ�Ͷ˿�
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
		/// ��ǰ�ͻ�������Ϣ
		/// </summary>
		/// <param name="msg">���͵���Ϣ</param>
		public void Write(string msg)
		{
			writer.Write(msg);
		}

		/// <summary>
		/// ��ȡ��ǰ���ӿͻ���������Ϣ
		/// </summary>
		/// <returns></returns>
		public string Read()
		{
			return reader.Read(socket);
		}
        /// <summary>
        /// ��ȡ��ǰ���ӿͻ���������Ϣ
        /// </summary>
        /// <returns></returns>
        public Stream ReadStream()
        {
            return reader.ReadStream(socket);
        }

       
	}
}
