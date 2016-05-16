using System;
using System.IO;
using System.Net.Sockets;

namespace ISimcpa.Net
{
	/// <summary>
	/// ΪOnConnect��OnDisconnect�¼��ṩ����
	/// </summary>
	public class SocketEventArgs : System.EventArgs
	{
		private Socket socket = null;
		private string address = null;

		/// <summary>
		/// Զ�̼����IP��ַ�Ͷ˿�
		/// </summary>
		public string RemoteAddress
		{
			get
			{
				return address;
			}
		}

		public SocketEventArgs(Socket socket)
		{
			this.socket = socket;
			this.address = socket.RemoteEndPoint.ToString();
		}

		public SocketEventArgs(string ip, int port)
		{
			this.address = string.Format("{0}:{1}", ip, port);
		}
	}
}
