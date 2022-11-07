using System.Collections.Generic;
using System.Net;

namespace Wpf.MvvmLight.SelfHost.Common
{
  public class NetworkUtils
  {
    /// <summary>
    /// 获取本机所有ip地址
    /// </summary>
    /// <param name="netType">"InterNetwork":ipv4地址，"InterNetworkV6":ipv6地址</param>
    /// <returns>ip地址集合</returns>
    public static List<string> GetLocalIpAddress(string netType)
    {
      string hostName = Dns.GetHostName();                    //获取主机名称  
      IPAddress[] addresses = Dns.GetHostAddresses(hostName); //解析主机IP地址  

      List<string> IPList = new List<string>();
      if (netType == string.Empty)
      {
        for (int i = 0; i < addresses.Length; i++)
        {
          IPList.Add(addresses[i].ToString());
        }
      }
      else
      {
        //AddressFamily.InterNetwork表示此IP为IPv4,
        //AddressFamily.InterNetworkV6表示此地址为IPv6类型
        for (int i = 0; i < addresses.Length; i++)
        {
          if (addresses[i].AddressFamily.ToString() == netType)
          {
            IPList.Add(addresses[i].ToString());
          }
        }
      }
      return IPList;
    }
  }
}
