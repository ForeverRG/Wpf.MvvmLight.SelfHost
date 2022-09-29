using GalaSoft.MvvmLight.Messaging;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;

namespace Wpf.MvvmLight.SelfHost.Common
{
  public class RangeSocket
  {
    private readonly IBootstrap bootstrap = BootstrapFactory.CreateBootstrap();

    public RangeSocket()
    {
      InitSocket();
    }

    /// <summary>
    /// 初始化socket
    /// </summary>
    public void InitSocket()
    {
      if (!bootstrap.Initialize())
      {
        Messenger.Default.Send("Socket初始化失败,请检查相关配置", "ServerSendMsgSignal");
      }
    }

    /// <summary>
    /// 启动服务
    /// </summary>
    public bool Start()
    {
      var result = bootstrap.Start();
      if (result == StartResult.Failed)
      {
        return false;
      }
      // 多个服务
      foreach (var server in bootstrap.AppServers)
      {
        if (server.State == ServerState.Running)
        {
          Messenger.Default.Send($"服务{server.Name}启动成功", "ServerSendMsgSignal");
        }
        else
        {
          Messenger.Default.Send($"服务{server.Name}启动失败", "ServerSendMsgSignal");
        }
      }
      return true;
    }

    /// <summary>
    /// 停止服务
    /// </summary>
    public void Stop()
    {
      bootstrap.Stop();
    }
  }
}
