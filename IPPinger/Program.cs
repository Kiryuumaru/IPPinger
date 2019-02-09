using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace IPPinger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("IP PINGER");
                Console.Write("Start ip: ");
                IPParser startPoint = new IPParser(IPAddress.Parse(Console.ReadLine()));

                Console.Write("End ip: ");
                IPParser endPoint = new IPParser(IPAddress.Parse(Console.ReadLine()));


                Console.WriteLine("\nPinging all ips . . .\n");
                Console.WriteLine("-------------Alive IPs-------------");

                while(startPoint.GreaterOrEqual(endPoint))
                {
                    Thread pinger = new Thread(() => PingerThread(startPoint.GetCopy().IPAddress));
                    pinger.Start();
                    Thread.Sleep(5);
                    startPoint.Inc();
                }

                while(ThreadCount > 0) { }
                Console.WriteLine("\n--------------END----------------");
                Console.ReadKey();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public static int ThreadCount = 0;
        public static void PingerThread(IPAddress iPAddress)
        {
            ThreadCount++;
            int retry = 5;
            Ping ping = new Ping();
            while (retry > 0)
            {
                PingReply reply = ping.Send(iPAddress, 1000);
                if (reply != null)
                {
                    if (reply.Status == IPStatus.Success)
                    {
                        IPHostEntry ip = Dns.GetHostEntry(iPAddress.ToString());
                        string hostname = ip.HostName;
                        Console.WriteLine(iPAddress + " - " + hostname);
                        break;
                    }
                }
                retry--;
            }
            ThreadCount--;
        }
    }

    public class IPParser
    {
        public IPParser(IPAddress iPAddress)
        {
            string ip = iPAddress.ToString();
            Octet1 = Convert.ToInt16(ip.Substring(0, ip.IndexOf('.')));
            ip = ip.Substring(ip.IndexOf('.') + 1);
            Octet2 = Convert.ToInt16(ip.Substring(0, ip.IndexOf('.')));
            ip = ip.Substring(ip.IndexOf('.') + 1);
            Octet3 = Convert.ToInt16(ip.Substring(0, ip.IndexOf('.')));
            ip = ip.Substring(ip.IndexOf('.') + 1);
            Octet4 = Convert.ToInt16(ip);
        }

        public int Octet1 { get; set; }
        public int Octet2 { get; set; }
        public int Octet3 { get; set; }
        public int Octet4 { get; set; }

        public IPAddress IPAddress
        {
            get
            {
                return IPAddress.Parse(
                    Octet1.ToString() + "." +
                    Octet2.ToString() + "." +
                    Octet3.ToString() + "." +
                    Octet4.ToString()
                    );
            }
        }

        public IPParser GetCopy()
        {
            return new IPParser(IPAddress);
        }

        public void Inc()
        {
            if (Octet4 < 255)
            {
                Octet4++;
            }
            else if (Octet3 < 255)
            {
                Octet4 = 0;
                Octet3++;
            }
            else if (Octet2 < 255)
            {
                Octet3 = 0;
                Octet2++;
            }
            else if (Octet1 < 255)
            {
                Octet2 = 0;
                Octet1++;
            }
        }

        public bool GreaterOrEqual(IPParser iPParser)
        {
            long ip1 =
                Octet1 * 1000000000 +
                Octet2 * 1000000 +
                Octet3 * 1000 +
                Octet4 * 1;
            long ip2 =
                iPParser.Octet1 * 1000000000 +
                iPParser.Octet2 * 1000000 +
                iPParser.Octet3 * 1000 +
                iPParser.Octet4 * 1;

            if (ip1 <= ip2) return true;
            else return false;
        }
    }
}
