using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Quectel.ATE.UR.UI.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using static Quectel.ATE.UR.UI.EventBus.MessageModel;

namespace Quectel.ATE.UR.UI.ViewModel
{
  public class VisionAssistantViewModel : ViewModelBase, IEventBus
  {
    public IList<int> JigNumberList => new List<int>(Enumerable.Range(1, 16));
    public IList<int> TrayNumberList => new List<int>(Enumerable.Range(1, 50));

    private string robotCmd;
    public string RobotCmd
    {
      get { return robotCmd; }
      set { robotCmd = value; RaisePropertyChanged(); }
    }

    private string jigNum;
    public string JigNum { get => jigNum; set { jigNum = value; RaisePropertyChanged(); } }

    private string trayNum;
    public string TrayNum { get => trayNum; set { trayNum = value; RaisePropertyChanged(); } }

    private KeyValuePair<string, string> trayParameter;
    private KeyValuePair<string, string> cycleParameter;
    private KeyValuePair<string, string> lineLossParameter;
    private KeyValuePair<string, string> qRParameter;
    private KeyValuePair<string, string> socket1Parameter;
    private KeyValuePair<string, string> socket2Parameter;

    public KeyValuePair<string, string> TrayParameter { get => trayParameter; set { trayParameter = value; RaisePropertyChanged(); } }

    public KeyValuePair<string, string> CycleParameter { get => cycleParameter; set { cycleParameter = value; RaisePropertyChanged(); } }

    public KeyValuePair<string, string> LineLossParameter { get => lineLossParameter; set { lineLossParameter = value; RaisePropertyChanged(); } }

    public KeyValuePair<string, string> QRParameter { get => qRParameter; set { qRParameter = value; RaisePropertyChanged(); } }

    public KeyValuePair<string, string> Socket1Parameter { get => socket1Parameter; set { socket1Parameter = value; RaisePropertyChanged(); } }

    public KeyValuePair<string, string> Socket2Parameter { get => socket2Parameter; set { socket2Parameter = value; RaisePropertyChanged(); } }

    public RelayCommand InitServerCommand { get; set; }
    public RelayCommand ReadJigQrCommand { get; set; }
    public RelayCommand PositioningRightSocket1Command { get; set; }
    public RelayCommand PositioningRightSocket2Command { get; set; }
    public RelayCommand PositioningLeftSocket1Command { get; set; }
    public RelayCommand PositioningLeftSocket2Command { get; set; }
    public RelayCommand SendToRobotCommand { get; set; }
    public RelayCommand StopServerCommand { get; set; }
    public RelayCommand<string> RobotMoveToCommand { get; set; }
    public RelayCommand SingleActionCommand { get; set; }
    public RelayCommand DoubleSocketSingleActionCommand { get; set; }
    public RelayCommand AlignLineLossCommand { get; set; }
    public RelayCommand DoubleSocketAlignLineLossCommand { get; set; }

    public VisionAssistantViewModel()
    {
      TrayParameter = new KeyValuePair<string, string>("Tray", TrayNum);
      CycleParameter = new KeyValuePair<string, string>("Cycle", string.Empty);
      LineLossParameter = new KeyValuePair<string, string>("LineLoss", string.Empty);
      QRParameter = new KeyValuePair<string, string>("QR", JigNum);
      Socket1Parameter = new KeyValuePair<string, string>("Socket1", JigNum);
      Socket2Parameter = new KeyValuePair<string, string>("Socket2", JigNum);

      InitServerCommand = new RelayCommand(InitServer);
      ReadJigQrCommand = new RelayCommand(ReadJigQr);
      PositioningRightSocket1Command = new RelayCommand(PositioningRightSocket1);
      PositioningRightSocket2Command = new RelayCommand(PositioningRightSocket2);
      PositioningLeftSocket1Command = new RelayCommand(PositioningLeftSocket1);
      PositioningLeftSocket2Command = new RelayCommand(PositioningLeftSocket2);
      SendToRobotCommand = new RelayCommand(SendToRobot);
      StopServerCommand = new RelayCommand(StopServer);
      RobotMoveToCommand = new RelayCommand<string>(RobotMoveTo);
      SingleActionCommand = new RelayCommand(SingleAction);
      DoubleSocketSingleActionCommand = new RelayCommand(DoubleSocketSingleAction);
      AlignLineLossCommand = new RelayCommand(AlignLineLoss);
      DoubleSocketAlignLineLossCommand = new RelayCommand(DoubleSocketAlignLineLoss);
    }

    /// <summary>
    /// 双A校线损
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void DoubleSocketAlignLineLoss()
    {
      MessengerInstance.Send(string.Empty, "DoubleSocketAlignLineLossSignal");
    }

    /// <summary>
    /// 校线损(用于模拟)
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void AlignLineLoss()
    {
      MessengerInstance.Send(string.Empty, "AlignLineLossSignal");
    }

    /// <summary>
    /// 双A模式的单次动作取放(用于模拟)
    /// </summary>
    private void DoubleSocketSingleAction()
    {
      MessengerInstance.Send(string.Empty, "DoubleSocketSingleActionSignal");
    }

    /// <summary>
    /// 单次动作取放(用于模拟)
    /// </summary>
    private void SingleAction()
    {
      MessengerInstance.Send(string.Empty, "SingleActionSignal");
    }

    /// <summary>
    /// 移动机器人
    /// </summary>
    /// <param name="kvp">key:类型，value:位置</param>
    private void RobotMoveTo(string key)
    {
      switch (key)
      {
        case "Tray":
        case "Cycle":
        case "LineLoss":
          MessengerInstance.Send(new KeyValuePair<string, string>(key, TrayNum), "RobotMoveToSignal");
          break;
        case "QR":
        case "Socket1":
        case "Socket2":
          MessengerInstance.Send(new KeyValuePair<string, string>(key, JigNum), "RobotMoveToSignal");
          break;
        default:
          break;
      }
    }

    /// <summary>
    /// 停止总控服务
    /// </summary>
    private void StopServer()
    {
      MessengerInstance.Send(string.Empty, "StopServerSignal");
    }

    /// <summary>
    /// 定位左侧夹具socket2
    /// </summary>
    private void PositioningLeftSocket2()
    {
      MessengerInstance.Send(new PositioningSocket { JigNumber = 1, SocketType = "Socket2" }, "PositioningLeftSocketSignal");
    }

    /// <summary>
    /// 定位左侧夹具socket1
    /// </summary>
    private void PositioningLeftSocket1()
    {
      MessengerInstance.Send(new PositioningSocket { JigNumber = 1, SocketType = "Socket1" }, "PositioningLeftSocketSignal");
    }

    /// <summary>
    /// 定位右侧夹具socket2
    /// </summary>
    private void PositioningRightSocket2()
    {
      MessengerInstance.Send(new PositioningSocket { JigNumber = 9, SocketType = "Socket2" }, "PositioningRightSocketSignal");
    }

    /// <summary>
    /// 定位右侧夹具Socket1
    /// </summary>
    private void PositioningRightSocket1()
    {
      MessengerInstance.Send(new PositioningSocket { JigNumber = 9, SocketType = "Socket1" }, "PositioningRightSocketSignal");
    }

    /// <summary>
    /// 读取夹具二维码
    /// </summary>
    private void ReadJigQr()
    {
      MessengerInstance.Send(new PositioningSocket { JigNumber = 1, SocketType = "QR" }, "ReadJigQrSignal");
    }

    /// <summary>
    /// 初始化服务
    /// </summary>
    private void InitServer()
    {
      MessengerInstance.Send(string.Empty, "InitServerSignal");
    }

    /// <summary>
    /// 给机器人发送指令
    /// </summary>
    private void SendToRobot()
    {
      MessengerInstance.Send(RobotCmd, "SendToRobotSignal");
    }

    /// <summary>
    /// 注册信号
    /// </summary>
    public void RegisterMessageSignal()
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// 注销信号
    /// </summary>
    public void UnregisterMessageSignal()
    {
      throw new NotImplementedException();
    }
  }
}
