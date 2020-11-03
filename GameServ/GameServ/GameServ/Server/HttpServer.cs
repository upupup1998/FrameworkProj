using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameServ.Server
{
    class HttpServer
    {
        private Socket socket = null;
        private string contentRoot = "F:/Ackerman/FrameworkProj/GameServ"; //根目录名
        private string contentWant = "ackerman";
        /// <summary>
        /// init
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="目标文件夹"></param>
        public HttpServer(string ip,int port,string contentWant="ackerman") {
            contentWant = contentRoot + "/" + contentWant;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(ip),port));
            socket.Listen(100);
            Console.WriteLine("Http Service Start...");
        }
        public void StartAccept() {
            socket.BeginAccept(new AsyncCallback(NetCallBack), socket);
        }
        private void NetCallBack(IAsyncResult ar)
        {
            Socket server = ar.AsyncState as Socket;
            Socket client = server.EndAccept(ar);
            server.BeginAccept(new AsyncCallback(NetCallBack), socket);
            byte[] buffer = new byte[1024 * 1024];
            int count = client.Receive(buffer);
            string str = Encoding.UTF8.GetString(buffer, 0, count);
            Console.WriteLine("有人请求了:\r\n"+str);
            Resolve(client,str);
        }
        /// <summary>
        /// 解析请求字符串
        /// </summary>
        /// <param name="client"></param>
        /// <param name="str"></param>
        private void Resolve(Socket client,string str) {
           string s0 =  str.Split(new string[] { "\r\n" },StringSplitOptions.None)[0];//根据请求字符串获取第一行
           string path = s0.Split(' ')[1];//文件地址
            if (!path.Contains(contentWant))
            {
                Console.WriteLine("PATH!CONTAIN");
                SendError(client);
            }
            else {

            }
        }
        /// <summary>
        /// 响应
        /// </summary>
        private void SendError(Socket client) {
            StringBuilder sb = new StringBuilder();
            string content = "";
            sb.Append("HTTP/1.1 404 Not Found\r\n");
            
            client.Send(Encoding.UTF8.GetBytes(sb.ToString()));
            client.Close();
        }
    }
}
#region 浏览器发送请求 url="http://127.0.0.1:8081/ackerman/1.jpg"
    //GET /ackerman/1.jpg HTTP/1.1
    //Host: 127.0.0.1:8081
    //Connection: keep-alive
    //Upgrade-Insecure-Requests: 1
    //User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36
    //Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9
    //Sec-Fetch-Site: none
    //Sec-Fetch-Mode: navigate
    //Sec-Fetch-User: ?1
    //Sec-Fetch-Dest: document
    //Accept-Encoding: gzip, deflate, br
    //Accept-Language: zh,en-US;q=0.9,en;q=0.8,zh-TW;q=0.7,zh-CN;q=0.6
#endregion
