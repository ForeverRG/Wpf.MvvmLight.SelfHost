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

    private ManualResetEvent manualReset;  // ��ʼ���¼�������ʼ״̬Ϊ����ֹ

    public HomeViewModel()
    {
      manualReset = new ManualResetEvent(true);  // ��ʼ���¼�������ʼ״̬Ϊ��ֹ(������)
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
    /// ��ս�����־
    /// </summary>
    private void ClearUILog()
    {
      EventSignal.SendClearUILogSignal();
    }

    /// <summary>
    /// ��ʼ��Homeģ��
    /// </summary>
    public void InitHomeModel()
    {
      Home.IsEnableStartBtn = true;
      Home.IsEnableStopBtn = Home.IsEnableSuspendOrContinueBtn = false;
      Home.SCBtnContent = "��ͣ";
    }

    /// <summary>
    /// �����ܿ�
    /// </summary>
    private void Start()
    {
      Task.Run(async () =>
      {
        if (StartControlCenter()) // �������ط���
        {
          Home.IsEnableStartBtn = false;
          Home.IsEnableStopBtn = Home.IsEnableSuspendOrContinueBtn = true;
          await StartRunTask();
        }
      });
    }

    /// <summary>
    /// ��ʼ��������
    /// </summary>
    private async Task StartRunTask()
    {
      EventSignal.SendWriteLogSignal("��ʼ��������");
      await startEngine.StartRunTask();
    }

    /// <summary>
    /// ��ͣ������ܿ�
    /// </summary>
    private void SuspendOrContinue()
    {
      if (Home.SCBtnContent == "��ͣ")
      {
        Home.SCBtnContent = "����";
        manualReset.Reset();
        EventSignal.SendWriteLogSignal("�ܿ���ͣ����", LogLevel.Warn);
      }
      else
      {
        Home.SCBtnContent = "��ͣ";
        manualReset.Set();
        EventSignal.SendWriteLogSignal("�ܿؼ�������", LogLevel.Warn);
      }
    }

    /// <summary>
    /// ֹͣ�ܿ�
    /// </summary>
    private void Stop()
    {
      controlCenterHelper.StopRunTasksDequeueTask(); // ֹͣ��ʱ����
      controlCenterHelper.InitControlCenterRunningInstance(); // ��ʼ���ܿ�����ʵ��
      Home.ControlCenterServer?.Stop();
      Home.IsEnableStartBtn = true;
      Home.IsEnableStopBtn = Home.IsEnableSuspendOrContinueBtn = false;
      EventSignal.SendWriteAllLogSignal("�ܿ���ط�����ֹͣ����", LogLevel.Warn);
    }

    /// <summary>
    /// �����ܿط���
    /// </summary>
    private bool StartControlCenter()
    {
      EventSignal.SendWriteAllLogSignal("�ܿط���������......");
      Home.ControlCenterServer = new AppServer();  // �����ܿط������
      if (!Home.ControlCenterServer.Setup(2022))
      {
        EventSignal.SendWriteAllLogSignal("���ط���˿ڰ�ʧ�ܣ�����ö˿��Ƿ�ռ��");
        return false;
      }
      if (!Home.ControlCenterServer.Start())
      {
        EventSignal.SendWriteAllLogSignal("���ط�������ʧ��", LogLevel.Error);
        return false;
      }
      // ע���¼�
      Home.ControlCenterServer.NewSessionConnected += new SessionHandler<AppSession>(ControlCenterServer_NewSessionConnected);
      Home.ControlCenterServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(ControlCenterServer_NewRequestReceived);
      Home.ControlCenterServer.SessionClosed += ControlCenterServer_SessionClosed;

      EventSignal.SendWriteAllLogSignal("���ط��������ɹ�");
      return true;
    }


    /// <summary>
    /// ����Ự����
    /// </summary>
    /// <param name="session"></param>
    private void ControlCenterServer_NewSessionConnected(AppSession session)
    {
      string msg = string.Format($"�ͻ���[{session.LocalEndPoint.Address}:{session.RemoteEndPoint.Port}]����");
      EventSignal.SendWriteDebugLogSignal(msg);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="session"></param>
    /// <param name="requestInfo"></param>
    private void ControlCenterServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
    {
      EventSignal.SendWriteAllLogSignal($"�ͻ���[{session.LocalEndPoint.Address}:{session.LocalEndPoint.Port}]->�ܿ�:{requestInfo.Body}");
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
    /// ����Ự�ر�
    /// </summary>
    /// <param name="session"></param>
    /// <param name="value"></param>
    private void ControlCenterServer_SessionClosed(AppSession session, CloseReason value)
    {
      string msg = string.Format("�ͻ���{0}:{1}�Ͽ�", session.LocalEndPoint.Address.ToString(), session.RemoteEndPoint.Port);
      EventSignal.SendWriteDebugLogSignal(msg);
    }

    /// <summary>
    /// ׷�Ӷ�����
    /// </summary>
    private void AppendMultiRunTask()
    {
      EventSignal.SendWriteAllLogSignal($"׷�Ӷ�����");
      controlCenterHelper.RunTaskEnqueueManager.EnqueueMultiRunTask();
    }

    /// <summary>
    /// ׷�ӵ�����
    /// </summary>
    private void AppendSignleRunTask()
    {
      EventSignal.SendWriteAllLogSignal($"׷�ӵ�����");
      controlCenterHelper.RunTaskEnqueueManager.EnqueueSignleRunTask();
    }

    #region ����ź����

    /// <summary>
    /// ע���ź�
    /// </summary>
    public void RegisterMessageSignal()
    {
      MessengerInstance.Register<string>(this, "InitServerSignal", InitServerSlot);
      MessengerInstance.Register<string>(this, "StopServerSignal", StopServerSlot);
      MessengerInstance.Register<string>(this, "ReleaseResourcesSignal", ReleaseResourcesSlot);
    }

    /// <summary>
    /// �ͷ���Դ
    /// </summary>
    /// <param name="obj"></param>
    private void ReleaseResourcesSlot(string obj)
    {
      StopServerSlot(null);
    }

    /// <summary>
    /// ֹͣ�ܿط���
    /// </summary>
    /// <param name="obj"></param>
    private void StopServerSlot(string obj)
    {
      Stop();
    }

    /// <summary>
    /// ��ʼ�������������
    /// </summary>
    /// <param name="obj"></param>
    private void InitServerSlot(string obj)
    {
      Start();
    }

    /// <summary>
    /// ע���ź�
    /// </summary>
    public void UnregisterMessageSignal()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}