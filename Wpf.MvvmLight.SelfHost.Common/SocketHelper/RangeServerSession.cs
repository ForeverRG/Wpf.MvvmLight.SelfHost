using GalaSoft.MvvmLight.Messaging;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System;

namespace Wpf.MvvmLight.SelfHost.Common
{
  public class RangeServerSession : AppSession<RangeServerSession, StringRequestInfo>
  {
    public override void Send(string message)
    {
      Messenger.Default.Send(message, "ServerSendMsgSignal");
      base.Send(message);
    }

    protected override void OnSessionStarted()
    {
      Messenger.Default.Send("会话已开始", "ServerSendMsgSignal");
      base.OnSessionStarted();
    }

    protected override void HandleUnknownRequest(StringRequestInfo requestInfo)
    {
      Messenger.Default.Send("未知请求", "ServerSendMsgSignal");
      base.HandleUnknownRequest(requestInfo);
    }

    protected override void HandleException(Exception e)
    {
      Messenger.Default.Send("会话异常", "ServerSendMsgSignal");
      base.HandleException(e);
    }

    protected override void OnSessionClosed(CloseReason reason)
    {
      Messenger.Default.Send("会话已关闭", "ServerSendMsgSignal");
      base.OnSessionClosed(reason);
    }
  }
}
