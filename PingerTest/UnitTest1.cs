using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PingerTest
{
    [TestClass]
    public class UnitTest1
    {
        private static IPAddress ip;
        private static int timeout;
        private static byte[] buffer;
        private static PingOptions options;
        [TestMethod]
        public void TestMethod1()
        {
            ip = new IPAddress(address: new byte[] { 172, 217, 20, 14 });
            string hostname = "172.217.20.14";
            buffer = new byte[10];
            options = new PingOptions() { DontFragment = true, Ttl = 128 };
            Ping ping = new Ping();
            timeout = 10;
            PingReply reply = ping.Send(hostname, timeout, buffer, options);
            using (TcpClient client = new TcpClient(hostname = "74.125.232.240", port: 443))
            {
                //client.BeginConnect()
                bool flag;
                if (client.Connected)
                    flag = true;
            }
            HttpListener httpListener = new HttpListener();
            //WebRequest request = WebRequest.Create(hostname);
            WebRequest request1 = WebRequest.Create("http://metanit.com/sharp/patterns/2.4.php");
            request1.Timeout = 10;
            request1.GetRequestStream();
        }
    }
}
