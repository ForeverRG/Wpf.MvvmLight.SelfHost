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
    /// ��ʾ���ؿ��ź�
    /// </summary>
    /// <param name="flag">�Ƿ���ʾ</param>
    public static void SendShowProgressViewSignal(bool flag)
    {
      Messenger.Default.Send(flag, "ShowProgressViewSignal");
    }

    /// <summary>
    /// ���log�ź�
    /// </summary>
    /// <param name="uiLog">log������Ϣ</param>
    /// <param name="logType">�����־����</param>
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
    /// ���Դ������log�ź�
    /// </summary>
    /// <param name="uiLog"></param>
    public static void SendWriteDebugLogSignal(string logMessage, LogLevel logLevel = LogLevel.Info)
    {
      UILog uiLog = new UILog { Message = logMessage, LogLevel = logLevel };
      DispatcherHelper.CheckBeginInvokeOnUI(() => Messenger.Default.Send(uiLog, "WriteDebugLogSignal"));
    }

    /// <summary>
    /// ������log���������log�ź�
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
    /// ���log
    /// </summary>
    public static void SendClearUILogSignal()
    {
      Messenger.Default.Send(string.Empty, "ClearUILogSignal");
    }

    /// <summary>
    /// ��ʾ��Ϣ����
    /// </summary>
    /// <param name="msg">������Ϣ</param>
    public static void SendShowMessageBoxSignal(string msg)
    {
      Messenger.Default.Send(msg, "ShowMessageBoxSignal");
    }

    /// <summary>
    /// ��ʾ��Ϣ����
    /// </summary>
    /// <param name="messageBoxTunnel">������Ϣ��ش���ʶ</param>
    public static void SendShowMessageBoxSignal(MessageBoxTunnel messageBoxTunnel)
    {
      Messenger.Default.Send(messageBoxTunnel, "ShowMessageBoxTunnelSignal");
    }
    /// <summary>
    /// ���ͻش���Ϣ����Ĵ������ź�
    /// </summary>
    /// <param name="result"></param>
    public static void SendEchoMessageBoxResultSignal(MessageBoxResult result)
    {
      Messenger.Default.Send(result, "EchoMessageBoxResult");
    }
  }
}
