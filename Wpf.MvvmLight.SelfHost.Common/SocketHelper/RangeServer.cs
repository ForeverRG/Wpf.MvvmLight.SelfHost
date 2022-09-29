using GalaSoft.MvvmLight.Messaging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;

namespace Wpf.MvvmLight.SelfHost.Common
{
  public class RangeServer : AppServer<RangeServerSession>
  {
    protected override bool Setup(IRootConfig rootConfig, IServerConfig config)
    {
      return base.Setup(rootConfig, config);
    }
    protected override void OnStarted()
    {
      Messenger.Default.Send("服务已启动", "ServerSendMsgSignal");
      base.OnStarted();
    }
    protected override void OnStopped()
    {
      Messenger.Default.Send("服务已停止", "ServerSendMsgSignal");
      base.OnStopped();
    }
    protected override void OnNewSessionConnected(RangeServerSession session)
    {
      string msg = string.Format("客户端接入:{0}:{1} session:{2} 时间：{3}", session.LocalEndPoint.Address.ToString(), session.RemoteEndPoint.Port, session.SessionID, DateTime.Now);
      Messenger.Default.Send(msg, "ServerSendMsgSignal");
      base.OnNewSessionConnected(session);
    }
    protected override void OnSessionClosed(RangeServerSession session, CloseReason reason)
    {
      string msg = string.Format("链接关闭:{0}:{1} session:{2} 时间：{3}", session.LocalEndPoint.Address.ToString(), session.RemoteEndPoint.Port, session.SessionID, DateTime.Now);
      Messenger.Default.Send(msg, "ServerSendMsgSignal");
      base.OnSessionClosed(session, reason);
    }
  }
}
