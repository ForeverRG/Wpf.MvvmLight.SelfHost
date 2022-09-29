using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using Wpf.MvvmLight.SelfHost.Common;
using Wpf.MvvmLight.SelfHost.EventBus;

namespace Wpf.MvvmLight.SelfHost.SocketCommand
{
  public class Single
  {
    public class Multi : CommandBase<RangeServerSession, StringRequestInfo>
    {
      public override void ExecuteCommand(RangeServerSession session, StringRequestInfo requestInfo)
      {
        EventSignal.SendWriteLogSignal($"添加任务:{requestInfo.Key}-{string.Join(",", requestInfo.Parameters)}");
      }
    }
  }
}
