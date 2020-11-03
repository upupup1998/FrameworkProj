using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServ.Server
{
    class SocServer
    {
        private Socket socket = null;
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public SocServer(string ip, int port)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            socket.Listen(100);
            Console.WriteLine("Soc Service Start...");
        }
        public void StartAccept()
        {
            socket.BeginAccept(new AsyncCallback(NetCallBack), socket);
        }
        private void NetCallBack(IAsyncResult ar)
        {
            Socket server = ar.AsyncState as Socket;
            Socket client = server.EndAccept(ar);
            IPEndPoint iPEndPoint = client.RemoteEndPoint as IPEndPoint;
            string clientIP = iPEndPoint.Address.ToString();
            Client cc = new Client(client,clientIP);
            Console.WriteLine(clientIP + "connecting...");
            server.BeginAccept(new AsyncCallback(NetCallBack), socket);
            //client.Send(Encoding.UTF8.GetBytes("connect success"));

            //byte[] buffer = new byte[1024 * 1024];
            //int count = client.Receive(buffer);
            //string str = Encoding.UTF8.GetString(buffer, 0, count);


        }
       
      
    }
    class Client {
        private Socket clientSoc = null;
        private string name = null;
        byte[] buff = new byte[1024 * 1024];
        public Client(Socket soc,string name) {
            this.clientSoc = soc;
            this.name = name;
            Receive();
        }
        private void Receive() {
            clientSoc.BeginReceive(buff, 0,buff.Length,SocketFlags.None ,ReceiveCallBack, clientSoc);
        }
        private void ReceiveCallBack(IAsyncResult ar)
        {
            Socket ss = ar.AsyncState as Socket;
            int count =  clientSoc.EndReceive(ar);
            if (count == 0)
            {
                Close();
            }
            else {
                Console.WriteLine(name + " say :" + Encoding.UTF8.GetString(buff,0,count));
                clientSoc.BeginReceive(buff, 0, buff.Length, SocketFlags.None, ReceiveCallBack, clientSoc);
            }
        }

        public void Close() {
            Console.WriteLine(name+" disconnect...");
            clientSoc.Close();
            clientSoc.Dispose();
        }
    }
}
