using Quectel.ATE.UR.Common;
using Quectel.ATE.UR.Model;
using Quectel.ATE.UR.UI.Common.Helpers;
using Quectel.ATE.UR.UI.EventBus;
using Quectel.ATE.UR.UI.Model.Common;
using System.Threading.Tasks;

namespace Quectel.ATE.UR.UI.Common.RunTaskServices
{
  /// <summary>
  /// 模拟运行任务管家
  /// </summary>
  public class SimulateRunTaskManager
  {
    /// <summary>
    /// 模拟输出日志
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private async Task SimulateLog(string message)
    {
      await Task.Delay(1000);
      EventSignal.SendWriteLogSignal(message);
    }

    /// <summary>
    /// 创建模拟测试任务
    /// </summary>
    public void CreateSimulateTestTask(PLCHelper plcHelper, TestBenchHelper testBenchHelper, TestBench testBench)
    {
      if (testBench == null) // 必须是放到夹具的动作，才会创建模拟测试任务
      {
        EventSignal.SendWriteLogSignal($"非放置夹具动作，不创建任务");
        return;
      }
      EventSignal.SendWriteLogSignal($"测试台{testBench.Number}开始进行模拟测试");
      var testBenchNumber = testBench.Number;
      _ = Task.Run(() =>
      {
        while (true)
        {
          // 1.读可测试
          var canTest = plcHelper.ReadCanTest(testBenchNumber);
          if (canTest)
          {
            Task.Delay(10000).Wait(); // 模拟测试时间10s
            plcHelper.WriteTestResult(testBenchNumber, 2);
            testBenchHelper.SendToControlCenterSimulation($"TRD:TS{testBenchNumber}:PASS;");
            break;
          }
          Task.Delay(50).Wait();
        }
      });
    }

    /// <summary>
    /// 创建双A模式模拟测试任务
    /// </summary>
    public void CreateDoubleSocketSimulateTestTask(PLCHelper plcHelper, TestBenchHelper testBenchHelper, TestBench testBench)
    {
      if (testBench == null) // 必须是放到夹具的动作，才会创建模拟测试任务
      {
        EventSignal.SendWriteLogSignal($"非放置夹具动作，不创建任务");
        return;
      }
      EventSignal.SendWriteLogSignal($"测试台{testBench.Number}开始进行模拟测试");
      var testBenchNumber = testBench.Number;
      _ = Task.Run(() =>
      {
        while (true)
        {
          // 1.读可测试
          var canTest = plcHelper.ReadCanTest(testBenchNumber);
          if (canTest)
          {
            Task.Delay(5000).Wait();
            testBenchHelper.SendToControlCenterSimulation($"TRD:TS{testBenchNumber}.1:PASS;");
            Task.Delay(5000).Wait();
            testBenchHelper.SendToControlCenterSimulation($"TRD:TS{testBenchNumber}.2:PASS;");
            plcHelper.WriteTestResult(testBenchNumber, 2);
            break;
          }
          Task.Delay(50).Wait();
        }
      });
    }

    /// <summary>
    /// 创建金板模拟测试任务
    /// </summary>
    /// <param name="plcHelper"></param>
    /// <param name="testBenchHelper"></param>
    /// <param name="testBench"></param>
    public void CreateSimulateAlignLineLossTask(PLCHelper plcHelper, TestBenchHelper testBenchHelper, TestBench testBench)
    {
      if (testBench == null) // 必须是放到夹具的动作，才会创建模拟测试任务
      {
        EventSignal.SendWriteLogSignal($"非放置夹具动作，不创建任务");
        return;
      }
      EventSignal.SendWriteLogSignal($"测试台{testBench.Number}开始进行校线损");
      var testBenchNumber = testBench.Number;
      _ = Task.Run(() =>
      {
        while (true)
        {
          // 1.读可测试
          var canTest = plcHelper.ReadCanTest(testBenchNumber);
          if (canTest)
          {
            Task.Delay(10000).Wait(); // 模拟测试时间10s
            plcHelper.WriteTestResult(testBenchNumber, 2);
            testBenchHelper.SendToControlCenterSimulation($"CLOSS:TS{testBenchNumber}:PASS;");
            break;
          }
          Task.Delay(50).Wait();
        }
      });
    }

    /// <summary>
    /// 创建双A金板模拟测试任务
    /// </summary>
    /// <param name="plcHelper"></param>
    /// <param name="testBenchHelper"></param>
    /// <param name="testBench"></param>
    /// <param name="socketNumber"></param>
    public void CreateDoubleSocketSimulateAlignLineLossTask(PLCHelper plcHelper, TestBenchHelper testBenchHelper, TestBench testBench, int socketNumber)
    {
      if (testBench == null) // 必须是放到夹具的动作，才会创建模拟测试任务
      {
        EventSignal.SendWriteLogSignal($"非放置夹具动作，不创建任务");
        return;
      }
      EventSignal.SendWriteLogSignal($"测试台{testBench.Number}.{socketNumber}开始进行校线损");
      var testBenchNumber = testBench.Number;
      _ = Task.Run(() =>
      {
        while (true)
        {
          // 1.读可测试
          var canTest = plcHelper.ReadCanTest(testBenchNumber);
          if (canTest)
          {
            Task.Delay(10000).Wait(); // 模拟测试时间10s
            plcHelper.WriteTestResult(testBenchNumber, 2);
            testBenchHelper.SendToControlCenterSimulation($"CLOSS:TS{testBenchNumber}.{socketNumber}:PASS;");
            break;
          }
          Task.Delay(50).Wait();
        }
      });
    }
  }
}
