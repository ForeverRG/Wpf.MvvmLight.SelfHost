using NLog;

namespace Wpf.MvvmLight.SelfHost.Common.LogHelper.NLog
{
  public class NLogServer
  {
    public static readonly Logger logger = LogManager.GetCurrentClassLogger();
  }
}
