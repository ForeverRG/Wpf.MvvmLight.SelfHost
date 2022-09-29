using SuperSocket.ClientEngine;
using System;
using System.Net;
using System.Text;
using System.Threading;

namespace Wpf.MvvmLight.SelfHost.Common
{
  public class RangeClient
  {
    private AsyncTcpSession client;
    private ManualResetEventSlim manualReset;

    public RangeClient()
    {
      client = new AsyncTcpSession();
      manualReset = new ManualResetEventSlim(false);

      client.Connected += Client_Connected;
      client.DataReceived += Client_DataReceived;
      client.Closed += Client_Closed;
      client.Error += Client_Error;
    }

    private void Client_Error(object sender, ErrorEventArgs e)
    {
    }

    private void Client_Closed(object sender, System.EventArgs e)
    {
    }

    private void Client_DataReceived(object sender, DataEventArgs e)
    {
    }

    private void Client_Connected(object sender, System.EventArgs e)
    {
      manualReset.Set();
    }

    public void ConnectServer(string ip, int port)
    {
      client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
    }

    public void Send(string key, string data)
    {
      //while (!client.IsConnected) { }
      var msgBytes = Encoding.UTF8.GetBytes($"{key} {data}{Environment.NewLine}");
      manualReset.Wait();
      client.Send(msgBytes, 0, msgBytes.Length);
    }

    public void Disconnect()
    {
      //while (!client.IsConnected) { }
      client.Close();
    }
  }
}
