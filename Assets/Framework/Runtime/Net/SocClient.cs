using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

 class SocClient 
{
    private bool isConnect = false;//是否连接上
    Socket _socket = null;
    byte[] bf = new byte[1024*64];
    public SocClient(string ip,int port) {
        if (isConnect == true) return;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
        isConnect = true;
        _socket.BeginReceive(bf, 0, bf.Length, SocketFlags.None, callback, null);
    }
    private void callback(IAsyncResult ar)
    {
        
        int cc = _socket.EndReceive(ar);
        if (cc == 0)
        {
            Debug.Log("Server is disconnect...");
            _socket.Close();
        }
        else
        {
            Bufferbyte bufferbyte = new Bufferbyte();
            bufferbyte.WriteBytes(bf);
            EventDispatch.DispatchEvent(bufferbyte.ReadString(),bufferbyte);
            _socket.BeginReceive(bf, 0, bf.Length, SocketFlags.None, callback, _socket);
            _socket.Close();
        }

    }
    public void SendMessage(Bufferbyte bufferbyte)
    {
        if (!isConnect)
        {
            Debug.Log("Network error");
        }
        else
        {
            bufferbyte.SendMessage(_socket);
        }
      
    }
    public void Dispose() {
        if (!isConnect)
        {
            Debug.Log("Network error");
        }
        else {
            _socket.Close();
            _socket.Dispose();
            _socket = null;
        }
        
    }
}
