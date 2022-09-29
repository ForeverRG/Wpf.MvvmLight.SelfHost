/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Wpf.MvvmLight.SelfHost"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using Wpf.MvvmLight.SelfHost.ViewModel.ASChildViewModel;

namespace Wpf.MvvmLight.SelfHost.ViewModel
{
  /// <summary>
  /// This class contains static references to all the view models in the
  /// application and provides an entry point for the bindings.
  /// </summary>
  public class ViewModelLocator
  {
    /// <summary>
    /// Initializes a new instance of the ViewModelLocator class.
    /// </summary>
    public ViewModelLocator()
    {
      ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

      //if (ViewModelBase.IsInDesignModeStatic)
      //{
      //    // Create design time view services and models
      //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
      //}
      //else
      //{
      //    // Create run time view services and models
      //    SimpleIoc.Default.Register<IDataService, DataService>();
      //}

      SimpleIoc.Default.Register<MainViewModel>();
      SimpleIoc.Default.Register<LayoutViewModel>();
      SimpleIoc.Default.Register<HomeViewModel>();
      SimpleIoc.Default.Register<SettingsViewModel>();
      SimpleIoc.Default.Register<AdvancedSettingsViewModel>();
      SimpleIoc.Default.Register<TCPAssistantViewModel>();
      SimpleIoc.Default.Register<RunAssistantViewModel>();
      SimpleIoc.Default.Register<PLCAssistantViewModel>();
      SimpleIoc.Default.Register<ModuleSettingsViewModel>();
      SimpleIoc.Default.Register<TrayViewModel>();
      SimpleIoc.Default.Register<JigViewModel>();
      SimpleIoc.Default.Register<ChannelMappingViewModel>();
      SimpleIoc.Default.Register<GripperLoadViewModel>();
    }

    public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
    public LayoutViewModel Layout => ServiceLocator.Current.GetInstance<LayoutViewModel>();
    public HomeViewModel Home => ServiceLocator.Current.GetInstance<HomeViewModel>();
    public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
    public AdvancedSettingsViewModel AdvancedSettings => ServiceLocator.Current.GetInstance<AdvancedSettingsViewModel>();
    public TCPAssistantViewModel TCPAssistant => ServiceLocator.Current.GetInstance<TCPAssistantViewModel>();
    public RunAssistantViewModel RunAssistant => ServiceLocator.Current.GetInstance<RunAssistantViewModel>();
    public PLCAssistantViewModel PLCAssistant => ServiceLocator.Current.GetInstance<PLCAssistantViewModel>();
    public JigViewModel Jig => ServiceLocator.Current.GetInstance<JigViewModel>();
    public ModuleSettingsViewModel ModuleSettings => ServiceLocator.Current.GetInstance<ModuleSettingsViewModel>();
    public TrayViewModel Tray => ServiceLocator.Current.GetInstance<TrayViewModel>();
    public ChannelMappingViewModel ChannelMapping => ServiceLocator.Current.GetInstance<ChannelMappingViewModel>();
    public GripperLoadViewModel GripperLoad => ServiceLocator.Current.GetInstance<GripperLoadViewModel>();

    public static void Cleanup()
    {
      // TODO Clear the ViewModels
    }
  }
}