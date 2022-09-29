using GalaSoft.MvvmLight.Messaging;
using Wpf.MvvmLight.SelfHost.Common.Model;
using System;
using System.Windows.Controls;

namespace Wpf.MvvmLight.SelfHost.View
{
  /// <summary>
  /// VisionAssistant.xaml 的交互逻辑
  /// </summary>
  public partial class RunAssistantView : UserControl
  {
    public RunAssistantView()
    {
      InitializeComponent();

      Messenger.Default.Register<UILog>(this, "WriteDebugLogSignal", WriteDebugLogSlot);
      Messenger.Default.Register<string>(this, "ClearDebugLogSignal", ClearDebugLogSlot);
    }

    private void ClearDebugLogSlot(string obj)
    {
      DebugLog.Clear();
    }

    private void WriteDebugLogSlot(UILog log)
    {
      if (DebugLog.LineCount >= 500)
      {
        DebugLog.Clear();
      }
      DebugLog.Text += $"[{DateTime.Now}]:{log.Message}{Environment.NewLine}";
      DebugLog.ScrollToEnd();
    }
  }
}
