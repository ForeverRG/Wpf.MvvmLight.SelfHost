using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.ObjectModel;

namespace Wpf.MvvmLight.SelfHost.Common.ASChildModel
{
  public class GripperLoadModel : ObservableObject
  {
    private ObservableCollection<GripperLoadInfo> gripperLoad;

    public ObservableCollection<GripperLoadInfo> GripperLoadList
    {
      get { return gripperLoad; }
      set { gripperLoad = value; RaisePropertyChanged(); }
    }
  }
}
