using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chubbseg.Utilities.Helpers
{
    public class IPTransform
    {
        public IPTransform()
        {
           
        }

        public string NormalizeIp(string? ip)
        {
            if (string.IsNullOrEmpty(ip))
                return "0.0.0.0";

            // Si viene en IPv6 (::1) convertir a IPv4 localhost
            if (ip == "::1")
                return "127.0.0.1";

            // Si es IPv6 con puerto o con prefijos intentar parsear
            if (System.Net.IPAddress.TryParse(ip, out var ipAddress))
            {
                if (ipAddress.IsIPv4MappedToIPv6)
                {
                    return ipAddress.MapToIPv4().ToString();
                }
            }

            return ip;
        }

    }
}
