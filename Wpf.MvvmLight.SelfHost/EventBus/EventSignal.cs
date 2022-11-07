using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Wpf.MvvmLight.SelfHost.Common.Enum;
using Wpf.MvvmLight.SelfHost.Common.Model;
using System.Windows;
using Wpf.MvvmLight.SelfHost.EventBus.EventMessageModel;

namespace Wpf.MvvmLight.SelfHost.EventBus
{
  public class EventSignal
  {
    public EventSignal()
    {
      DispatcherHelper.Initialize();
    }

    /// <summary>
    /// 显示加载框信号
    /// </summary>
    /// <param name="flag">是否显示</param>
    public static void SendShowProgressViewSignal(bool flag)
    {
      Messenger.Default.Send(flag, "ShowProgressViewSignal");
    }

    /// <summary>
    /// 输出log信号
    /// </summary>
    /// <param name="uiLog">log对象信息</param>
    /// <param name="logType">输出日志类型</param>
    public static void SendWriteLogSignal(string logMessage, LogLevel logLevel = LogLevel.Info, LogOutputType logType = LogOutputType.All)
    {
      UILog uiLog = new UILog { Message = logMessage, LogLevel = logLevel };
      switch (logType)
      {
        case LogOutputType.UILog:
          DispatcherHelper.CheckBeginInvokeOnUI(() => Messenger.Default.Send(uiLog, "WriteUILogSignal"));
          break;
        case LogOutputType.NLog:
          Messenger.Default.Send(uiLog, "WriteNLogSignal");
          break;
        case LogOutputType.All:
          DispatcherHelper.CheckBeginInvokeOnUI(() => Messenger.Default.Send(uiLog, "WriteUILogSignal"));
          Messenger.Default.Send(uiLog, "WriteNLogSignal");
          break;
        default:
          break;
      }
    }

    /// <summary>
    /// 调试窗口输出log信号
    /// </summary>
    /// <param name="uiLog"></param>
    public static void SendWriteDebugLogSignal(string logMessage, LogLevel logLevel = LogLevel.Info)
    {
      UILog uiLog = new UILog { Message = logMessage, LogLevel = logLevel };
      DispatcherHelper.CheckBeginInvokeOnUI(() => Messenger.Default.Send(uiLog, "WriteDebugLogSignal"));
    }

    /// <summary>
    /// 在所有log窗口中输出log信号
    /// </summary>
    /// <param name="logMessage"></param>
    /// <param name="logLevel"></param>
    /// <param name="logType"></param>
    public static void SendWriteAllLogSignal(string logMessage, LogLevel logLevel = LogLevel.Info, LogOutputType logType = LogOutputType.All)
    {
      SendWriteLogSignal(logMessage, logLevel, logType);
      SendWriteDebugLogSignal(logMessage, logLevel);
    }

    /// <summary>
    /// 清空log
    /// </summary>
    public static void SendClearUILogSignal()
    {
      Messenger.Default.Send(string.Empty, "ClearUILogSignal");
    }

    /// <summary>
    /// 显示消息弹框
    /// </summary>
    /// <param name="msg">弹框消息</param>
    public static void SendShowMessageBoxSignal(string msg)
    {
      Messenger.Default.Send(msg, "ShowMessageBoxSignal");
    }

    /// <summary>
    /// 显示消息弹框
    /// </summary>
    /// <param name="messageBoxTunnel">弹框消息与回传标识</param>
    public static void SendShowMessageBoxSignal(MessageBoxTunnel messageBoxTunnel)
    {
      Messenger.Default.Send(messageBoxTunnel, "ShowMessageBoxTunnelSignal");
    }
    /// <summary>
    /// 发送回传消息弹框的处理结果信号
    /// </summary>
    /// <param name="result"></param>
    public static void SendEchoMessageBoxResultSignal(MessageBoxResult result)
    {
      Messenger.Default.Send(result, "EchoMessageBoxResult");
    }
  }
}
