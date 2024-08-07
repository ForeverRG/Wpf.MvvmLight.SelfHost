﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Wpf.MvvmLight.SelfHost.EventBus;
using System;
using Wpf.MvvmLight.SelfHost.Common;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Wpf.MvvmLight.SelfHost.Common.HttpRequestHelper;

namespace Wpf.MvvmLight.SelfHost.ViewModel
{
  public class RunAssistantViewModel : ViewModelBase, IEventBus
  {
    private bool isCycleAddTask;
    public bool IsCycleAddTask { get => isCycleAddTask; set => isCycleAddTask = value; }

    public RelayCommand InitServerCommand { get; set; }
    public RelayCommand StopServerCommand { get; set; }
    public RelayCommand AddSingleTaskCommand { get; set; }
    public RelayCommand AddMultiTaskCommand { get; set; }
    public RelayCommand SendGetRequestCommand { get; set; }
    public RelayCommand SendPostRequestCommand { get; set; }
    public RelayCommand ClearDebugLogCommand { get; set; }

    public RunAssistantViewModel()
    {
      IsCycleAddTask = false;

      InitServerCommand = new RelayCommand(InitServer);
      StopServerCommand = new RelayCommand(StopServer);
      AddSingleTaskCommand = new RelayCommand(AddSingleTask);
      AddMultiTaskCommand = new RelayCommand(AddMultiTask);
      SendGetRequestCommand = new RelayCommand(SendGetRequest);
      SendPostRequestCommand = new RelayCommand(SendPostRequest);
      ClearDebugLogCommand = new RelayCommand(ClearDebugLog);
    }

    private async void SendPostRequest()
    {
      var response = await new RangeRestApi().PostAsync("http://127.0.0.1:9000/api/v1/home/echo", new { name = "Range post" });
      EventSignal.SendWriteDebugLogSignal($"获取POST请求响应：{response}");
    }

    private async void SendGetRequest()
    {
      var response = await new RangeRestApi().GetAsync("http://127.0.0.1:9000/api/v1/home/echo", new { name = "Range get" });
      EventSignal.SendWriteDebugLogSignal($"获取GET请求响应：{response}");
    }

    /// <summary>
    /// 清除调试日志
    /// </summary>
    private void ClearDebugLog()
    {
      MessengerInstance.Send(string.Empty, "ClearDebugLogSignal");
    }

    /// <summary>
    /// 双A校线损
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void AddMultiTask()
    {
      Task.Run(() =>
      {
        do
        {
          Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
          try
          {
            tcpClient.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2022));
            tcpClient.Send(Encoding.UTF8.GetBytes("Multi Append Multi Task\r\n"));
            tcpClient.Disconnect(true);
          }
          catch (Exception e)
          {
            EventSignal.SendWriteDebugLogSignal($"追加单任务失败:{e.Message}");
          }
          Task.Delay(1000).Wait();
        } while (IsCycleAddTask);
      });
    }

    /// <summary>
    /// 校线损(用于模拟)
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void AddSingleTask()
    {
      Task.Run(() =>
      {
        do
        {
          RangeClient client = new RangeClient();
          try
          {
            client.ConnectServer("127.0.0.1", 2022);
            client.Send("single", "hello");
            client.Disconnect();
          }
          catch (Exception e)
          {
            EventSignal.SendWriteDebugLogSignal($"追加多任务失败:{e.Message}");
          }
          Task.Delay(1000).Wait();
        } while (IsCycleAddTask);
      });
    }

    /// <summary>
    /// 停止总控服务
    /// </summary>
    private void StopServer()
    {
      MessengerInstance.Send(string.Empty, "StopServerSignal");
    }

    /// <summary>
    /// 初始化服务
    /// </summary>
    private void InitServer()
    {
      MessengerInstance.Send(string.Empty, "InitServerSignal");
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
