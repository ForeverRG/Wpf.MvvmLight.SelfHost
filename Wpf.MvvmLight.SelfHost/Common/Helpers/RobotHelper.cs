using Wpf.MvvmLight.SelfHost.Common.Model;

namespace Wpf.MvvmLight.SelfHost.Common.Helpers
{
  /// <summary>
  /// 机器人操作帮助类
  /// </summary>
  public class RobotHelper
  {
    public RobotRunningInstance RobotRunningInstance { get; private set; }

    private readonly RangeServerSession tcpClient;

    public RobotHelper()
    {
      RobotRunningInstance = new RobotRunningInstance();
    }
  }
}


