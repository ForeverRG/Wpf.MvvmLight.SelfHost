using System;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using Wpf.MvvmLight.SelfHost.Common;
using Wpf.MvvmLight.SelfHost.Common.Enum;
using Wpf.MvvmLight.SelfHost.Common.Helpers;
using Wpf.MvvmLight.SelfHost.EventBus;

namespace Wpf.MvvmLight.SelfHost.ViewModel
{
  public class HomeViewModel : ViewModelBase, IEventBus
  {
    private StartEngine startEngine;

    public ControlCenterHelper controlCenterHelper;
    public RobotHelper robotHelper;

    private HomeModel home;
    public HomeModel Home { get => home; set { home = value; RaisePropertyChanged(); } }
    public RelayCommand StartCommand { get; set; }
    public RelayCommand StopCommand { get; set; }
    public RelayCommand SuspendOrContinueCommand { get; set; }
    public RelayCommand ClearUILogCommand { get; set; }

    private ManualResetEvent manualReset;  // 初始化事件锁，初始状态为非终止

    public HomeViewModel()
    {
      manualReset = new ManualResetEvent(true);  // 初始化事件锁，初始状态为终止(不阻塞)
      RegisterMessageSignal();

      Home = new HomeModel();
      InitHomeModel();

      StartCommand = new RelayCommand(Start);
      StopCommand = new RelayCommand(Stop);
      SuspendOrContinueCommand = new RelayCommand(SuspendOrContinue);
      ClearUILogCommand = new RelayCommand(ClearUILog);

      controlCenterHelper = new ControlCenterHelper();
      startEngine = new StartEngine(controlCenterHelper);
    }

    /// <summary>
    /// 清空界面日志
    /// </summary>
    private void ClearUILog()
    {
      EventSignal.SendClearUILogSignal();
    }

    /// <summary>
    /// 初始化Home模型
    /// </summary>
    public void InitHomeModel()
    {
      Home.IsEnableStartBtn = true;
      Home.IsEnableStopBtn = Home.IsEnableSuspendOrContinueBtn = false;
      Home.SCBtnContent = "暂停";
    }

    /// <summary>
    /// 启动总控
    /// </summary>
    private void Start()
    {
      Task.Run(async () =>
      {
        if (StartControlCenter()) // 启动本地服务
        {
          Home.IsEnableStartBtn = false;
          Home.IsEnableStopBtn = Home.IsEnableSuspendOrContinueBtn = true;
          await StartRunTask();
        }
      });
    }

    /// <summary>
    /// 开始运行任务
    /// </summary>
    private async Task StartRunTask()
    {
      EventSignal.SendWriteLogSignal("开始运行任务");
      await startEngine.StartRunTask();
    }

    /// <summary>
    /// 暂停或继续总控
    /// </summary>
    private void SuspendOrContinue()
    {
      if (Home.SCBtnContent == "暂停")
      {
        Home.SCBtnContent = "继续";
        manualReset.Reset();
        EventSignal.SendWriteLogSignal("总控暂停运行", LogLevel.Warn);
      }
      else
      {
        Home.SCBtnContent = "暂停";
        manualReset.Set();
        EventSignal.SendWriteLogSignal("总控继续运行", LogLevel.Warn);
      }
    }

    /// <summary>
    /// 停止总控
    /// </summary>
    private void Stop()
    {
      controlCenterHelper.StopRunTasksDequeueTask(); // 停止定时任务
      controlCenterHelper.InitControlCenterRunningInstance(); // 初始化总控运行实例
      Home.ControlCenterServer?.Stop();
      Home.IsEnableStartBtn = true;
      Home.IsEnableStopBtn = Home.IsEnableSuspendOrContinueBtn = false;
      EventSignal.SendWriteAllLogSignal("总控相关服务已停止运行", LogLevel.Warn);
    }

    /// <summary>
    /// 启动总控服务
    /// </summary>
    private bool StartControlCenter()
    {
      EventSignal.SendWriteAllLogSignal("总控服务启动中......");
      Home.ControlCenterServer = new AppServer();  // 开启总控服务监听
      if (!Home.ControlCenterServer.Setup(2022))
      {
        EventSignal.SendWriteAllLogSignal("本地服务端口绑定失败，请检查该端口是否被占用");
        return false;
      }
      if (!Home.ControlCenterServer.Start())
      {
        EventSignal.SendWriteAllLogSignal("本地服务启动失败", LogLevel.Error);
        return false;
      }
      // 注册事件
      Home.ControlCenterServer.NewSessionConnected += new SessionHandler<AppSession>(ControlCenterServer_NewSessionConnected);
      Home.ControlCenterServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(ControlCenterServer_NewRequestReceived);
      Home.ControlCenterServer.SessionClosed += ControlCenterServer_SessionClosed;

      EventSignal.SendWriteAllLogSignal("本地服务启动成功");
      return true;
    }


    /// <summary>
    /// 处理会话连接
    /// </summary>
    /// <param name="session"></param>
    private void ControlCenterServer_NewSessionConnected(AppSession session)
    {
      string msg = string.Format($"客户端[{session.LocalEndPoint.Address}:{session.RemoteEndPoint.Port}]接入");
      EventSignal.SendWriteDebugLogSignal(msg);
    }

    /// <summary>
    /// 处理请求
    /// </summary>
    /// <param name="session"></param>
    /// <param name="requestInfo"></param>
    private void ControlCenterServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
    {
      EventSignal.SendWriteAllLogSignal($"客户端[{session.LocalEndPoint.Address}:{session.LocalEndPoint.Port}]->总控:{requestInfo.Body}");
      switch (requestInfo.Key.ToUpper())
      {
        case "SINGLE":
          AppendSignleRunTask();
          break;
        case "MULTI":
          AppendMultiRunTask();
          break;
        default:
          break;
      }
    }

    /// <summary>
    /// 处理会话关闭
    /// </summary>
    /// <param name="session"></param>
    /// <param name="value"></param>
    private void ControlCenterServer_SessionClosed(AppSession session, CloseReason value)
    {
      string msg = string.Format("客户端{0}:{1}断开", session.LocalEndPoint.Address.ToString(), session.RemoteEndPoint.Port);
      EventSignal.SendWriteDebugLogSignal(msg);
    }

    /// <summary>
    /// 追加多任务
    /// </summary>
    private void AppendMultiRunTask()
    {
      EventSignal.SendWriteAllLogSignal($"追加多任务");
      controlCenterHelper.RunTaskEnqueueManager.EnqueueMultiRunTask();
    }

    /// <summary>
    /// 追加单任务
    /// </summary>
    private void AppendSignleRunTask()
    {
      EventSignal.SendWriteAllLogSignal($"追加单任务");
      controlCenterHelper.RunTaskEnqueueManager.EnqueueSignleRunTask();
    }

    #region 相关信号与槽

    /// <summary>
    /// 注册信号
    /// </summary>
    public void RegisterMessageSignal()
    {
      MessengerInstance.Register<string>(this, "InitServerSignal", InitServerSlot);
      MessengerInstance.Register<string>(this, "StopServerSignal", StopServerSlot);
      MessengerInstance.Register<string>(this, "ReleaseResourcesSignal", ReleaseResourcesSlot);
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="obj"></param>
    private void ReleaseResourcesSlot(string obj)
    {
      StopServerSlot(null);
    }

    /// <summary>
    /// 停止总控服务
    /// </summary>
    /// <param name="obj"></param>
    private void StopServerSlot(string obj)
    {
      Stop();
    }

    /// <summary>
    /// 初始化调试所需服务
    /// </summary>
    /// <param name="obj"></param>
    private void InitServerSlot(string obj)
    {
      Start();
    }

    /// <summary>
    /// 注销信号
    /// </summary>
    public void UnregisterMessageSignal()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}