using GalaSoft.MvvmLight;
using Wpf.MvvmLight.SelfHost.Model;
using Wpf.MvvmLight.SelfHost.EventBus;
using Wpf.MvvmLight.SelfHost.Common.ASChildModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Wpf.MvvmLight.SelfHost.IServices;

namespace Wpf.MvvmLight.SelfHost.ViewModel.ASChildViewModel
{
  public class JigViewModel : ViewModelBase, IEventBus
  {
    private IJigServices _jigServices;
    private IConfigServices _configServices;

    private JigModel _jigModel;

    public JigModel JigModel
    {
      get { return _jigModel; }
      set { _jigModel = value; RaisePropertyChanged(); }
    }

    public JigViewModel(IJigServices jigServices, IConfigServices configServices)
    {
      _jigServices = jigServices;
      _configServices = configServices;
      JigModel = new JigModel();

      RegisterMessageSignal();
      EventSignal.SendShowProgressViewSignal(true);
      Task.Run(async () =>
      {
        // 初始化夹具集合
        await InitJigModel();
        EventSignal.SendShowProgressViewSignal(false);
      });
    }

    /// <summary>
    /// 注册消息信号
    /// </summary>
    public void RegisterMessageSignal()
    {
      MessengerInstance.Register<string>(this, "SaveAdvancedSettingsSignal", SaveAdvancedSettingsSlot);
      MessengerInstance.Register<string>(this, "UpdateViewByModuleTypeSignal", UpdateViewByModuleTypeSlot);
    }

    /// <summary>
    /// 更新夹具界面
    /// </summary>
    /// <param name="obj"></param>
    private void UpdateViewByModuleTypeSlot(string obj)
    {
      Task.Run(async () =>
      {
        await InitJigModel();
      });
    }

    /// <summary>
    /// 注销消息信号
    /// </summary>
    public void UnregisterMessageSignal()
    {
      MessengerInstance.Unregister(this);
    }

    /// <summary>
    /// 保存配置
    /// </summary>
    /// <param name="msg"></param>
    private void SaveAdvancedSettingsSlot(string msg)
    {
      var moduleTypeConfig = _configServices.GetSettings(c => c.Key == "ModuleType").Result?.FirstOrDefault();
      var currentModuleType = moduleTypeConfig != null ? moduleTypeConfig.Value : string.Empty;
      var jigsSettings = JigModel.JigsSocket1.Concat(JigModel.JigsSocket2).Concat(JigModel.JigsQR);
      jigsSettings.ToList().ForEach(j => j.ModuleType = currentModuleType);
      _jigServices.SaveSettings(jigsSettings.ToList(), j => new { j.Id, j.ModuleType }).Wait();
      UpdateViewByModuleTypeSlot(null);
      MessengerInstance.Send("夹具配置保存成功!", "ShowMessageSignal");
    }

    /// <summary>
    /// 初始化夹具模型
    /// </summary>
    /// <returns></returns>
    private async Task InitJigModel()
    {
      var defaultJigsQR =
        new ObservableCollection<Jig>(Enumerable.Range(1, 16).Select(i => new Jig
        {
          Number = i,
          IsUsed = false,
          X = 0,
          Y = 0,
          Z = 0,
          OffsetX = 0,
          OffsetY = 0,
          OffsetRZ = 0,
          OffsetZ = 0,
          LoadOffsetX = 0,
          LoadOffsetY = 0,
          LoadOffsetRZ = 0,
          LoadOffsetZ = 0,
          RZ = 0,
          QR = string.Empty,
          Type = "QR"
        }));
      var defaultJigsSocket1 =
       new ObservableCollection<Jig>(Enumerable.Range(1, 16).Select(i => new Jig
       {
         Number = i,
         IsUsed = false,
         X = 0,
         Y = 0,
         Z = 0,
         OffsetX = 0,
         OffsetY = 0,
         OffsetRZ = 0,
         OffsetZ = 0,
         LoadOffsetX = 0,
         LoadOffsetY = 0,
         LoadOffsetRZ = 0,
         LoadOffsetZ = 0,
         RZ = 0,
         QR = string.Empty,
         Type = "Socket1"
       }));
      var defaultJigsSocket2 =
       new ObservableCollection<Jig>(Enumerable.Range(1, 16).Select(i => new Jig
       {
         Number = i,
         IsUsed = false,
         X = 0,
         Y = 0,
         Z = 0,
         OffsetX = 0,
         OffsetY = 0,
         OffsetRZ = 0,
         OffsetZ = 0,
         LoadOffsetX = 0,
         LoadOffsetY = 0,
         LoadOffsetRZ = 0,
         LoadOffsetZ = 0,
         RZ = 0,
         QR = string.Empty,
         Type = "Socket2"
       }));

      var moduleTypeConfig = _configServices.GetSettings(c => c.Key == "ModuleType").Result?.FirstOrDefault();
      var currentModuleType = moduleTypeConfig != null ? moduleTypeConfig.Value : string.Empty;
      var jigs = (await _jigServices.GetSettings(j => j.ModuleType == currentModuleType)).ToList();
      JigModel.JigsQR = jigs.Count == 0 ? defaultJigsQR : new ObservableCollection<Jig>(jigs.FindAll(j => j.Type == "QR"));
      JigModel.JigsSocket1 = jigs.Count == 0 ? defaultJigsSocket1 : new ObservableCollection<Jig>(jigs.FindAll(j => j.Type == "Socket1"));
      JigModel.JigsSocket2 = jigs.Count == 0 ? defaultJigsSocket2 : new ObservableCollection<Jig>(jigs.FindAll(j => j.Type == "Socket2"));
    }
  }
}
