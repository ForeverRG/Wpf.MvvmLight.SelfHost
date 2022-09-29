using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.EventBus;
using Wpf.MvvmLight.SelfHost.View;

namespace Wpf.MvvmLight.SelfHost.ViewModel
{
  public class MainViewModel : ViewModelBase, IEventBus
  {
    private bool isOpen;

    public bool IsOpen
    {
      get { return isOpen; }
      set { isOpen = value; RaisePropertyChanged(); }
    }

    private object dialogContent;

    public object DialogContent
    {
      get { return dialogContent; }
      set { dialogContent = value; RaisePropertyChanged(); }
    }

    public MainViewModel()
    {
      IsOpen = false;
      DialogContent = null;

      RegisterMessageSignal();
    }

    public void RegisterMessageSignal()
    {
      MessengerInstance.Register<bool>(this, "ShowProgressViewSignal", ShowProgressViewSlot);
    }

    private void ShowProgressViewSlot(bool isOpen)
    {
      IsOpen = isOpen;
      DialogContent = isOpen ? new ProgressView() : null;
    }

    public void UnregisterMessageSignal()
    {
      throw new System.NotImplementedException();
    }
  }
}