using GalaSoft.MvvmLight;

namespace Wpf.MvvmLight.SelfHost.ViewModel
{
  public class PLCAssistantViewModel : ViewModelBase
  {
    private string text;

    public string Text
    {
      get { return text; }
      set { text = value; }
    }
    public PLCAssistantViewModel()
    {
      Text = "模块开发中...";
    }
  }
}
