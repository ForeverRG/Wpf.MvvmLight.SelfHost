using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Wpf.MvvmLight.SelfHost.Common.ASChildModel
{
  public class ModuleSettingsModel : ObservableObject
  {
    private ObservableCollection<ModuleSettings> moduleSettings;
    private IList<string> moduleTypes;
    private IList<string> sides;

    public ObservableCollection<ModuleSettings> ModuleSettings
    {
      get { return moduleSettings; }
      set { moduleSettings = value; RaisePropertyChanged(); }
    }

    /// <summary>
    /// 机型
    /// </summary>
    public IList<string> ModuleTypes { get => moduleTypes; set { moduleTypes = value; RaisePropertyChanged(); } }

    /// <summary>
    /// 夹具端口
    /// </summary>
    public IList<string> Sides { get => sides; set => sides = value; }
  }
}
