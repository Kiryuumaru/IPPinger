using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace IPPingerMain
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string iPAddress = args[0];
            int retry = Convert.ToInt16(args[1]);
            int timeout = Convert.ToInt16(args[2]);
            try
            {
                Ping ping = new Ping();
                while (retry > 0)
                {
                    PingReply reply = ping.Send(iPAddress, timeout);
                    if (reply != null)
                    {
                        if (reply.Status == IPStatus.Success)
                        {
                            IPHostEntry ip = Dns.GetHostEntry(iPAddress.ToString());
                            string hostname = ip.HostName;
                            Console.Write(iPAddress + " - " + hostname);
                            return;
                        }
                    }
                    retry--;
                }
            }
            catch { }
            Console.Write(iPAddress + " is dead");
        }
    }
}
