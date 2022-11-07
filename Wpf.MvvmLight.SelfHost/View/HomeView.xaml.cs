using GalaSoft.MvvmLight.Messaging;
using Wpf.MvvmLight.SelfHost.Common.LogHelper.NLog;
using Wpf.MvvmLight.SelfHost.Common.Enum;
using Wpf.MvvmLight.SelfHost.Common.Model;
using Wpf.MvvmLight.SelfHost.EventBus;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Wpf.MvvmLight.SelfHost.EventBus.EventMessageModel;

namespace Wpf.MvvmLight.SelfHost.View
{
  public partial class HomeView : UserControl, IEventBus
  {
    public HomeView()
    {
      InitializeComponent();
      RegisterMessageSignal();
    }

    public void RegisterMessageSignal()
    {
      Messenger.Default.Register<UILog>(this, "WriteUILogSignal", WriteUILogSlot);
      Messenger.Default.Register<UILog>(this, "WriteNLogSignal", WriteNLogSlot);
      Messenger.Default.Register<string>(this, "ClearUILogSignal", ClearUILogSlot);
      Messenger.Default.Register<string>(this, "ShowMessageBoxSignal", ShowMessageBoxSlot);
      Messenger.Default.Register<MessageBoxTunnel>(this, "ShowMessageBoxTunnelSignal", ShowMessageBoxTunnelSlot);
    }

    /// <summary>
    /// 消息弹框
    /// </summary>
    /// <param name="message"></param>
    private void ShowMessageBoxSlot(string message)
    {
      MessageBox.Show(message);
    }

    /// <summary>
    /// 消息弹框并回传结果
    /// </summary>
    /// <param name="messageBoxTunnel"></param>
    private void ShowMessageBoxTunnelSlot(MessageBoxTunnel messageBoxTunnel)
    {
      var res = MessageBox.Show(messageBoxTunnel.Message);
      Messenger.Default.Send(res, messageBoxTunnel.EchoToken);
    }

    /// <summary>
    /// 清空界面log
    /// </summary>
    /// <param name="obj"></param>
    private void ClearUILogSlot(string obj)
    {
      RunLog.Document.Blocks.Clear();
    }

    /// <summary>
    /// 向NLog输出log
    /// </summary>
    /// <param name="obj"></param>
    private void WriteNLogSlot(UILog log)
    {
      switch (log.LogLevel)
      {
        case LogLevel.Info:
          NLogServer.logger.Info(log.Message);
          break;
        case LogLevel.Debug:
          NLogServer.logger.Debug(log.Message);
          break;
        case LogLevel.Error:
          NLogServer.logger.Error(log.Message);
          break;
        case LogLevel.Warn:
          NLogServer.logger.Warn(log.Message);
          break;
        case LogLevel.Trace:
          NLogServer.logger.Trace(log.Message);
          break;
        case LogLevel.Fatal:
          NLogServer.logger.Fatal(log.Message);
          break;
        default:
          NLogServer.logger.Info(log.Message);
          break;
      }
    }

    /// <summary>
    /// 向界面输出log
    /// </summary>
    /// <param name="log"></param>
    private void WriteUILogSlot(UILog log)
    {
      var logRun = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}][{log.LogLevel}]：{log.Message}";
      Brush brush;
      switch (log.LogLevel)
      {
        case LogLevel.Trace:
        case LogLevel.Info:
          brush = Brushes.Black;
          break;
        case LogLevel.Debug:
          brush = Brushes.Blue;
          break;
        case LogLevel.Warn:
          brush = Brushes.Orange;
          break;
        case LogLevel.Fatal:
        case LogLevel.Error:
          brush = Brushes.Red;
          break;
        default:
          brush = Brushes.Black;
          break;
      }
      if (RunLog.Document.Blocks.Count >= 200)
      {
        ClearUILogSlot(null);
      }
      RunLog.Document.Blocks.Add(new Paragraph(new Run(logRun) { Foreground = brush }));
      RunLog.ScrollToEnd();
    }

    public void UnregisterMessageSignal()
    {
      throw new System.NotImplementedException();
    }
  }
}