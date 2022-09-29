using GalaSoft.MvvmLight.Messaging;
using Quectel.ATE.UR.UI.Model.Common;
using System;
using System.Windows.Controls;

namespace Quectel.ATE.UR.UI.View
{
  /// <summary>
  /// VisionAssistant.xaml 的交互逻辑
  /// </summary>
  public partial class VisionAssistantView : UserControl
  {
    public VisionAssistantView()
    {
      InitializeComponent();

      Messenger.Default.Register<UILog>(this, "WriteDebugLogSignal", WriteDebugLogSlot);
    }

    private void WriteDebugLogSlot(UILog log)
    {
      if (DebugTextBox.LineCount >= 500)
      {
        DebugTextBox.Clear();
      }
      DebugTextBox.Text += string.Format("[{0}]:{1}\r\n", DateTime.Now.ToString(), log.Message);
      DebugTextBox.ScrollToEnd();
    }
  }
}
