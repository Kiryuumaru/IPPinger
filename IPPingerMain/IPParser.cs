using System;
using System.Net;

namespace IPPingerLib
{
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
