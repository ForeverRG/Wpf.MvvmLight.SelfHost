using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using Wpf.MvvmLight.SelfHost.Api;
using Wpf.MvvmLight.SelfHost.EventBus;
using System.Windows;
using Wpf.MvvmLight.SelfHost.Repository.DBSeed;
using Wpf.MvvmLight.SelfHost.Tasks;
using System.Threading.Tasks;

namespace Wpf.MvvmLight.SelfHost
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow : MetroWindow, IEventBus
  {
    private HttpService _http;

    public MainWindow()
    {
      InitializeComponent();
      // 初始化数据库
      new SQLiteDbContext().InitDatabaseAndTables();

      RegisterMessageSignal();
      UnregisterMessageSignal();

      StartWebApi();
    }

    /// <summary>
    /// 开启后台任务调度
    /// </summary>
    private async Task StartTaskScheduler()
    {
      await QuartzTaskScheduler.CreateScheduler();
    }

    /// <summary>
    /// 启动web接口
    /// </summary>
    private void StartWebApi()
    {
      _http = new HttpService(9000);
      _http.StartHttpServer().Wait();
    }

    /// <summary>
    /// 注销消息信号
    /// </summary>
    public void UnregisterMessageSignal()
    {
      Unloaded += (sender, e) => Messenger.Default.Unregister(this);
    }

    /// <summary>
    /// 注册当前(this)对象注册的所有MVVMLight消息信号
    /// </summary>
    public void RegisterMessageSignal()
    {
      Messenger.Default.Register<string>(this, "ShowMessageSignal", ShowMessageSlot);
    }

    /// <summary>
    /// 显示消息弹窗
    /// </summary>
    /// <param name="msg"></param>
    private void ShowMessageSlot(string msg)
    {
      SaveSnackbar.MessageQueue.Enqueue(msg);
    }

    /// <summary>
    /// 窗口发送关闭释放相关资源
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainWindow_Closed(object sender, System.EventArgs e)
    {
      Messenger.Default.Send(string.Empty, "ReleaseResourcesSignal");
    }

    /// <summary>
    /// 窗口关闭前提醒
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      var result = MessageBox.Show("您确定要离开么?", "关闭程序", MessageBoxButton.YesNo);
      if (result == MessageBoxResult.No)
      {
        e.Cancel = true;
        return;
      }
      Application.Current.Shutdown();
      //Environment.Exit(0); // 强制退出所有线程
    }
  }
}
