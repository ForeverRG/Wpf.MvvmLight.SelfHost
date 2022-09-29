using GalaSoft.MvvmLight;

namespace Wpf.MvvmLight.SelfHost.ViewModel
{
  public class TCPAssistantViewModel : ViewModelBase
  {
    private string text;

    public string Text
    {
      get { return text; }
      set { text = value; RaisePropertyChanged(); }
    }

    public TCPAssistantViewModel()
    {
      Text = "模块开发中...";
    }
  }
}
