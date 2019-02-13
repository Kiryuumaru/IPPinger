using IPPingerLib;
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
            try
            {
                Console.WriteLine("IP PINGER");
                Console.Write("Start ip: ");
                IPParser startPoint = new IPParser(IPAddress.Parse(Console.ReadLine()));

                Console.Write("End ip: ");
                IPParser endPoint = new IPParser(IPAddress.Parse(Console.ReadLine()));

                Console.WriteLine("\nPinging all ips . . .\n");
                Console.WriteLine("-------------Alive IPs-------------");

                while (startPoint.GreaterOrEqual(endPoint))
                {
                    Thread pinger = new Thread(() => PingerThread(startPoint.GetCopy().IPAddress));
                    pinger.Start();
                    Thread.Sleep(5);
                    startPoint.Inc();
                }

                while (ThreadCount > 0) { }
                Console.WriteLine("\n--------------END----------------");
                Console.ReadKey();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        public static int ThreadCount = 0;
        public static void PingerThread(IPAddress iPAddress)
        {
            ThreadCount++;
            Process proc = new Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.FileName = "IPPinger.exe";
            proc.StartInfo.Arguments = iPAddress.ToString() + " " + 5 + " " + 1000;
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            proc.Close();
            if(output != null) Console.WriteLine(output);
            ThreadCount--;
        }
    }
}


