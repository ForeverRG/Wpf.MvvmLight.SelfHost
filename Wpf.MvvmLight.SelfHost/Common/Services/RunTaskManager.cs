using Quectel.ATE.UR.Model;
using Quectel.ATE.UR.Model.Enum;
using Quectel.ATE.UR.Services;
using Quectel.ATE.UR.UI.Common.Enum;
using Quectel.ATE.UR.UI.Common.Helpers;
using Quectel.ATE.UR.UI.Model.Common;
using Quectel.ATE.UR.UI.Model.RunningInstance;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quectel.ATE.UR.UI.Common.RunTaskServices
{
  /// <summary>
  /// 运行任务管家
  /// </summary>
  public class RunTaskManager
  {
    private readonly ControlCenterRunningInstance controlCenterRunningInstance;
    private readonly JigServices jigServices;

    public RunTaskManager(ControlCenterRunningInstance controlCenterRunningInstance)
    {
      jigServices = new JigServices();
      this.controlCenterRunningInstance = controlCenterRunningInstance;
    }

    #region 追加运行任务

    /// <summary>
    /// 追加运行任务
    /// </summary>
    /// <param name="testSequence"></param>
    /// <param name="testBenchWithJig"></param>
    /// <returns></returns>
    public void RunTaskEnqueue(TestSequence testSequence, TestBenchWithJig testBenchWithJig)
    {
      if (!controlCenterRunningInstance.RunTaskQueueDic.ContainsKey(-1))
      {
        controlCenterRunningInstance.RunTaskQueueDic.TryAdd(-1, new ConcurrentQueue<KeyValuePair<TestSequence, TestBenchWithJig>>());
      }
      controlCenterRunningInstance.RunTaskQueueDic[-1].Enqueue(new KeyValuePair<TestSequence, TestBenchWithJig>(testSequence, testBenchWithJig));
      controlCenterRunningInstance.TestModulePosition++;
    }

    /// <summary>
    /// 追加夹具到夹具的任务
    /// </summary>
    /// <param name="testBenchWithJigFrom">起始夹具</param>
    /// <param name="testBenchWithJigTo">目标夹具</param>
    public void Jig2JigRunTaskEnqueue(TestBenchWithJig testBenchWithJigFrom, TestBenchWithJig testBenchWithJigTo, RobotHelper robotHelper)
    {
      var coordinate = controlCenterRunningInstance.WholeTestTrayCoordinates[controlCenterRunningInstance.TestModulePosition - 1];
      RunTask runTask = new RunTask();
      runTask.TaskFlag = 1;
      // 从tray盘取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = $"{(int)MoveModulePosition.TestTray}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = coordinate
        });
      // 从夹具取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = testBenchWithJigFrom.Jig.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = testBenchWithJigFrom.Jig.X + testBenchWithJigFrom.Jig.OffsetX, Y = testBenchWithJigFrom.Jig.Y + testBenchWithJigFrom.Jig.OffsetY, Z = testBenchWithJigFrom.Jig.Z + testBenchWithJigFrom.Jig.OffsetZ, RZ = testBenchWithJigFrom.Jig.RZ + testBenchWithJigFrom.Jig.OffsetRZ },
          TestBenchWithJig = testBenchWithJigFrom
        });
      // tray盘放到夹具
      runTask.TaskToQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchWithJigFrom.Jig.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = testBenchWithJigFrom.Jig.X + testBenchWithJigFrom.Jig.OffsetX, Y = testBenchWithJigFrom.Jig.Y + testBenchWithJigFrom.Jig.OffsetY, Z = testBenchWithJigFrom.Jig.Z + testBenchWithJigFrom.Jig.OffsetZ, RZ = testBenchWithJigFrom.Jig.RZ + testBenchWithJigFrom.Jig.OffsetRZ },
          TestBenchWithJig = testBenchWithJigFrom
        });
      // 夹具放到夹具
      runTask.TaskToQue.Enqueue(
       new TaskTo
       {
         PosNum = testBenchWithJigTo.Jig.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
         Action = $"{(int)MoveModuleAction.Put}",
         Coordinates = new Coordinates { X = testBenchWithJigTo.Jig.X + testBenchWithJigTo.Jig.OffsetX, Y = testBenchWithJigTo.Jig.Y + testBenchWithJigTo.Jig.OffsetY, Z = testBenchWithJigTo.Jig.Z + testBenchWithJigTo.Jig.OffsetZ, RZ = testBenchWithJigTo.Jig.RZ + testBenchWithJigTo.Jig.OffsetRZ },
         TestBenchWithJig = testBenchWithJigTo
       });
      controlCenterRunningInstance.RunTasks.Enqueue(runTask);
      if (string.IsNullOrEmpty(robotHelper.GetGripper())) // 所有抓手都空闲
      {
        controlCenterRunningInstance.TestModulePosition++;
      }
    }

    /// <summary>
    /// 追加夹具到tray盘的任务
    /// </summary>
    /// <param name="testBenchWithJigFrom">起始夹具</param>
    /// <param name="isPass">测试是否pass</param>
    public void Jig2TrayRunTaskEnqueue(TestBenchWithJig testBenchWithJigFrom, bool isPass, RobotHelper robotHelper)
    {
      var coordinate = controlCenterRunningInstance.WholeTestTrayCoordinates[controlCenterRunningInstance.TestModulePosition - 1];
      RunTask runTask = new RunTask();
      runTask.TaskFlag = testBenchWithJigFrom.TestBench.Sequence == "1" ? 3 : 5;
      // 从tray取
      runTask.TaskFromQue.Enqueue(
       new TaskFrom
       {
         PosNum = $"{(int)MoveModulePosition.TestTray}",
         Action = $"{(int)MoveModuleAction.Pick}",
         Coordinates = coordinate
       });
      // 从夹具取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = testBenchWithJigFrom.Jig.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = testBenchWithJigFrom.Jig.X + testBenchWithJigFrom.Jig.OffsetX, Y = testBenchWithJigFrom.Jig.Y + testBenchWithJigFrom.Jig.OffsetY, Z = testBenchWithJigFrom.Jig.Z + testBenchWithJigFrom.Jig.OffsetZ, RZ = testBenchWithJigFrom.Jig.RZ + testBenchWithJigFrom.Jig.OffsetRZ },
          TestBenchWithJig = testBenchWithJigFrom
        });
      runTask.TaskToQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchWithJigFrom.Jig.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = testBenchWithJigFrom.Jig.X + testBenchWithJigFrom.Jig.OffsetX, Y = testBenchWithJigFrom.Jig.Y + testBenchWithJigFrom.Jig.OffsetY, Z = testBenchWithJigFrom.Jig.Z + testBenchWithJigFrom.Jig.OffsetZ, RZ = testBenchWithJigFrom.Jig.RZ + testBenchWithJigFrom.Jig.OffsetRZ },
          TestBenchWithJig = testBenchWithJigFrom
        });
      // 测试pass放到目标tray盘为pass盘，否则为ng盘
      if (isPass)
      {
        var passCoordinate = controlCenterRunningInstance.WholePassTrayCoordinates[controlCenterRunningInstance.PassModulePosition - 1];
        runTask.TaskToQue.Enqueue(new TaskTo
        {
          PosNum = $"{(int)MoveModulePosition.PassTray}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = passCoordinate.X, Y = passCoordinate.Y, Z = passCoordinate.Z, RZ = passCoordinate.RZ }
        });
        controlCenterRunningInstance.PassModulePosition++;
      }
      else
      {
        var ngCoordinate = testBenchWithJigFrom.Jig.Number < 9 ? controlCenterRunningInstance.LeftNGTrayCoordinates[controlCenterRunningInstance.LeftNGModulePosition - 1] : controlCenterRunningInstance.RightNGTrayCoordinates[controlCenterRunningInstance.RightNGModulePosition - 1];
        runTask.TaskToQue.Enqueue(new TaskTo
        {
          PosNum = testBenchWithJigFrom.Jig.Number < 9 ? $"{(int)MoveModulePosition.LeftNG}" : $"{ (int)MoveModulePosition.RightNG }",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = ngCoordinate.X, Y = ngCoordinate.Y, Z = ngCoordinate.Z, RZ = ngCoordinate.RZ }
        });
      }
      controlCenterRunningInstance.RunTasks.Enqueue(runTask);

      if (string.IsNullOrEmpty(robotHelper.GetGripper())) // 所有抓手都空闲
      {
        controlCenterRunningInstance.TestModulePosition++;
      }
    }

    /// <summary>
    /// 追加夹具到金板盘的任务(校线损)
    /// </summary>
    /// <param name="testBenchWithJigFrom">起始夹具</param>
    /// <param name="isPass">测试是否pass</param>
    public void AlignLineLossJig2GoldTrayRunTaskEnqueue(TestBenchWithJig testBenchWithJigFrom, bool isPass)
    {
      RunTask runTask = new RunTask();
      runTask.TaskFlag = 9;
      // 从夹具取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = testBenchWithJigFrom.Jig.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = testBenchWithJigFrom.Jig.X + testBenchWithJigFrom.Jig.OffsetX, Y = testBenchWithJigFrom.Jig.Y + testBenchWithJigFrom.Jig.OffsetY, Z = testBenchWithJigFrom.Jig.Z + testBenchWithJigFrom.Jig.OffsetZ, RZ = testBenchWithJigFrom.Jig.RZ + testBenchWithJigFrom.Jig.OffsetRZ },
          TestBenchWithJig = testBenchWithJigFrom
        });
      // 校线损pass/fail放到金板盘
      if (isPass)
      {
        var passCoordinate = controlCenterRunningInstance.GoldTrayCoordinates;
        runTask.TaskToQue.Enqueue(new TaskTo
        {
          PosNum = $"{(int)MoveModulePosition.GoldTray}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = passCoordinate.X, Y = passCoordinate.Y, Z = passCoordinate.Z, RZ = passCoordinate.RZ }
        });
      }
      else
      {
        var ngCoordinate = controlCenterRunningInstance.GoldTrayCoordinates;
        runTask.TaskToQue.Enqueue(new TaskTo
        {
          PosNum = $"{(int)MoveModulePosition.GoldTray}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = ngCoordinate.X, Y = ngCoordinate.Y, Z = ngCoordinate.Z, RZ = ngCoordinate.RZ }
        });
      }
      controlCenterRunningInstance.RunTasks.Enqueue(runTask);
    }

    /// <summary>
    /// 追加夹具到夹具的任务(校线损)
    /// </summary>
    /// <param name="testBenchWithJigFrom">起始夹具</param>
    /// <param name="testBenchWithJigTo">目标夹具</param>
    public void AlignLineLossJig2JigRunTaskEnqueue(TestBenchWithJig testBenchWithJigFrom, TestBenchWithJig testBenchWithJigTo)
    {
      RunTask runTask = new RunTask();
      runTask.TaskFlag = 10;
      // 从夹具取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = testBenchWithJigFrom.Jig.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = testBenchWithJigFrom.Jig.X + testBenchWithJigFrom.Jig.OffsetX, Y = testBenchWithJigFrom.Jig.Y + testBenchWithJigFrom.Jig.OffsetY, Z = testBenchWithJigFrom.Jig.Z + testBenchWithJigFrom.Jig.OffsetZ, RZ = testBenchWithJigFrom.Jig.RZ + testBenchWithJigFrom.Jig.OffsetRZ },
          TestBenchWithJig = testBenchWithJigFrom
        });
      // 夹具放到夹具
      runTask.TaskToQue.Enqueue(
       new TaskTo
       {
         PosNum = testBenchWithJigTo.Jig.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
         Action = $"{(int)MoveModuleAction.Put}",
         Coordinates = new Coordinates { X = testBenchWithJigTo.Jig.X + testBenchWithJigTo.Jig.OffsetX, Y = testBenchWithJigTo.Jig.Y + testBenchWithJigTo.Jig.OffsetY, Z = testBenchWithJigTo.Jig.Z + testBenchWithJigTo.Jig.OffsetZ, RZ = testBenchWithJigTo.Jig.RZ + testBenchWithJigTo.Jig.OffsetRZ },
         TestBenchWithJig = testBenchWithJigTo
       });
      controlCenterRunningInstance.RunTasks.Enqueue(runTask);
    }

    /// <summary>
    /// 追加夹具socket1到socket2的任务(双A校线损)
    /// </summary>
    /// <param name="testBenchFrom">起始夹具</param>
    /// <param name="isPass">测试是否pass</param>
    public void DoubleSocketAlignLineLossSocket1ToSocket2RunTaskEnqueue(TestBench testBenchFrom)
    {
      RunTask runTask = new RunTask();
      runTask.TaskFlag = 12;
      var socket1Coordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket1").Result.FirstOrDefault();
      var socket2Coordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket2").Result.FirstOrDefault();
      // 从socket1取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = socket1Coordinates.X + socket1Coordinates.OffsetX, Y = socket1Coordinates.Y + socket1Coordinates.OffsetY, Z = socket1Coordinates.Z + socket1Coordinates.OffsetZ, RZ = socket1Coordinates.RZ + socket1Coordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      // 往socket2放
      runTask.TaskToQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = socket2Coordinates.X + socket2Coordinates.OffsetX, Y = socket2Coordinates.Y + socket2Coordinates.OffsetY, Z = socket2Coordinates.Z + socket2Coordinates.OffsetZ, RZ = socket2Coordinates.RZ + socket2Coordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      controlCenterRunningInstance.RunTasks.Enqueue(runTask);
    }

    /// <summary>
    /// 追加夹具到夹具的任务(双A校线损)
    /// </summary>
    /// <param name="testBenchFrom">起始夹具</param>
    /// <param name="testBenchTo">终止夹具</param>
    public void DoubleSocketAlignLineLossJig2JigRunTaskEnqueue(TestBench testBenchFrom, TestBench testBenchTo)
    {
      RunTask runTask = new RunTask();
      runTask.TaskFlag = 13;
      var fromSocket2Coordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket2").Result.FirstOrDefault();
      var toSocket1Coordinates = jigServices.GetSettings(j => j.Number == testBenchTo.Number && j.Type == "Socket1").Result.FirstOrDefault();
      // 从前序夹具socket2取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = fromSocket2Coordinates.X + fromSocket2Coordinates.OffsetX, Y = fromSocket2Coordinates.Y + fromSocket2Coordinates.OffsetY, Z = fromSocket2Coordinates.Z + fromSocket2Coordinates.OffsetZ, RZ = fromSocket2Coordinates.RZ + fromSocket2Coordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      // 往后序夹具socket1放
      runTask.TaskToQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = toSocket1Coordinates.X + toSocket1Coordinates.OffsetX, Y = toSocket1Coordinates.Y + toSocket1Coordinates.OffsetY, Z = toSocket1Coordinates.Z + toSocket1Coordinates.OffsetZ, RZ = toSocket1Coordinates.RZ + toSocket1Coordinates.OffsetRZ },
          TestBench = testBenchTo
        });
      controlCenterRunningInstance.RunTasks.Enqueue(runTask);
    }

    /// <summary>
    /// 追加夹具到金板盘的任务(双A校线损)
    /// </summary>
    /// <param name="testBenchFrom">起始夹具</param>
    /// <param name="testBenchTo">终止夹具</param>
    public void DoubleSocketAlignLineLossJig2GoldTrayRunTaskEnqueue(TestBench testBenchFrom, bool isPass)
    {
      RunTask runTask = new RunTask();
      runTask.TaskFlag = 14;
      var fromSocket2Coordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket2").Result.FirstOrDefault();
      // 从夹具socket2取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = fromSocket2Coordinates.X + fromSocket2Coordinates.OffsetX, Y = fromSocket2Coordinates.Y + fromSocket2Coordinates.OffsetY, Z = fromSocket2Coordinates.Z + fromSocket2Coordinates.OffsetZ, RZ = fromSocket2Coordinates.RZ + fromSocket2Coordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      // 校线损pass/fail放到金板盘
      if (isPass)
      {
        var passCoordinate = controlCenterRunningInstance.GoldTrayCoordinates;
        runTask.TaskToQue.Enqueue(new TaskTo
        {
          PosNum = $"{(int)MoveModulePosition.GoldTray}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = passCoordinate.X, Y = passCoordinate.Y, Z = passCoordinate.Z, RZ = passCoordinate.RZ }
        });
      }
      else
      {
        var ngCoordinate = controlCenterRunningInstance.GoldTrayCoordinates;
        runTask.TaskToQue.Enqueue(new TaskTo
        {
          PosNum = $"{(int)MoveModulePosition.GoldTray}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = ngCoordinate.X, Y = ngCoordinate.Y, Z = ngCoordinate.Z, RZ = ngCoordinate.RZ }
        });
      }
      controlCenterRunningInstance.RunTasks.Enqueue(runTask);
    }

    /// <summary>
    /// 追加夹具到tray盘的任务(双A模式)
    /// </summary>
    /// <param name="testBenchFrom">起始夹具</param>
    /// <param name="testResult">测试结果</param>
    public void DoubleSocketJig2TrayRunTaskEnqueue(TestBench testBenchFrom, KeyValuePair<string, Dictionary<string, string>> testResult, RobotHelper robotHelper)
    {
      var coordinate = controlCenterRunningInstance.WholeTestTrayCoordinates[controlCenterRunningInstance.TestModulePosition - 1];
      RunTask runTask = new RunTask();
      var socket1Coordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket1").Result.FirstOrDefault();
      var socket2Coordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket2").Result.FirstOrDefault();
      runTask.TaskFlag = testBenchFrom.Sequence == "1" ? 6 : 7;
      // 从tray取
      runTask.TaskFromQue.Enqueue(
       new TaskFrom
       {
         PosNum = $"{(int)MoveModulePosition.TestTray}",
         Action = $"{(int)MoveModuleAction.Pick}",
         Coordinates = coordinate
       });
      // 从夹具Socket1取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = socket1Coordinates.X + socket1Coordinates.OffsetX, Y = socket1Coordinates.Y + socket1Coordinates.OffsetY, Z = socket1Coordinates.Z + socket1Coordinates.OffsetZ, RZ = socket1Coordinates.RZ + socket1Coordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      // 从夹具Socket2取
      runTask.TaskFromQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = socket2Coordinates.X + socket2Coordinates.OffsetX, Y = socket2Coordinates.Y + socket2Coordinates.OffsetY, Z = socket2Coordinates.Z + socket2Coordinates.OffsetZ, RZ = socket2Coordinates.RZ + socket2Coordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      // 往夹具Socket1放
      runTask.TaskToQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = socket1Coordinates.X + socket1Coordinates.OffsetX, Y = socket1Coordinates.Y + socket1Coordinates.OffsetY, Z = socket1Coordinates.Z + socket1Coordinates.OffsetZ, RZ = socket1Coordinates.RZ + socket1Coordinates.OffsetRZ },
          TestBench = testBenchFrom,
          TestResult = testResult
        });
      // 往夹具Socket2放
      runTask.TaskToQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = socket2Coordinates.X + socket2Coordinates.OffsetX, Y = socket2Coordinates.Y + socket2Coordinates.OffsetY, Z = socket2Coordinates.Z + socket2Coordinates.OffsetZ, RZ = socket2Coordinates.RZ + socket2Coordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      // 测试pass放到目标tray盘为pass盘，否则为ng盘
      var passResult = testResult.Value.Where(k => k.Value == "PASS");
      var ngResult = testResult.Value.Where(k => k.Value != "PASS");
      if (passResult.Count() > 0)
      {
        passResult.ToList().ForEach(kvp =>
        {
          var passCoordinate = controlCenterRunningInstance.WholePassTrayCoordinates[controlCenterRunningInstance.PassModulePosition - 1];
          runTask.TaskToQue.Enqueue(new TaskTo
          {
            PosNum = $"{(int)MoveModulePosition.PassTray}",
            Action = $"{(int)MoveModuleAction.Put}",
            Coordinates = new Coordinates { X = passCoordinate.X, Y = passCoordinate.Y, Z = passCoordinate.Z, RZ = passCoordinate.RZ }
          });
          controlCenterRunningInstance.PassModulePosition++;
        });
      }
      if (ngResult.Count() > 0)
      {
        ngResult.ToList().ForEach(kvp =>
        {
          var ngCoordinate = testBenchFrom.Number < 9 ? controlCenterRunningInstance.LeftNGTrayCoordinates[controlCenterRunningInstance.LeftNGModulePosition - 1] : controlCenterRunningInstance.RightNGTrayCoordinates[controlCenterRunningInstance.RightNGModulePosition - 1];
          runTask.TaskToQue.Enqueue(new TaskTo
          {
            PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftNG}" : $"{ (int)MoveModulePosition.RightNG }",
            Action = $"{(int)MoveModuleAction.Put}",
            Coordinates = new Coordinates { X = ngCoordinate.X, Y = ngCoordinate.Y, Z = ngCoordinate.Z, RZ = ngCoordinate.RZ }
          });
        });
      }
      controlCenterRunningInstance.RunTasks.Enqueue(runTask);
      if (string.IsNullOrEmpty(robotHelper.GetGripper())) // 所有抓手都空闲
      {
        controlCenterRunningInstance.TestModulePosition++;
      }
    }

    /// <summary>
    /// 追加夹具到夹具的任务(双A模式)
    /// </summary>
    /// <param name="testBenchFrom">起始夹具</param>
    /// <param name="testBenchTo">目标夹具</param>
    public void DoubleSocketJig2JigRunTaskEnqueue(TestBench testBenchFrom, TestBench testBenchTo, RobotHelper robotHelper)
    {
      var coordinate = controlCenterRunningInstance.WholeTestTrayCoordinates[controlCenterRunningInstance.TestModulePosition - 1];
      RunTask runTask = new RunTask();
      var socket1FromCoordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket1").Result.FirstOrDefault();
      var socket2FromCoordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket2").Result.FirstOrDefault();
      var socket1ToCoordinates = jigServices.GetSettings(j => j.Number == testBenchTo.Number && j.Type == "Socket1").Result.FirstOrDefault();
      var socket2ToCoordinates = jigServices.GetSettings(j => j.Number == testBenchTo.Number && j.Type == "Socket2").Result.FirstOrDefault();
      runTask.TaskFlag = 8;
      // 从tray盘取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = $"{(int)MoveModulePosition.TestTray}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = coordinate
        });
      // 从夹具Socket1取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = socket1FromCoordinates.X + socket1FromCoordinates.OffsetX, Y = socket1FromCoordinates.Y + socket1FromCoordinates.OffsetY, Z = socket1FromCoordinates.Z + socket1FromCoordinates.OffsetZ, RZ = socket1FromCoordinates.RZ + socket1FromCoordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      // 从夹具Socket2取
      runTask.TaskFromQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = socket2FromCoordinates.X + socket2FromCoordinates.OffsetX, Y = socket2FromCoordinates.Y + socket2FromCoordinates.OffsetY, Z = socket2FromCoordinates.Z + socket2FromCoordinates.OffsetZ, RZ = socket2FromCoordinates.RZ + socket2FromCoordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      // tray盘放到夹具Socket1
      runTask.TaskToQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = socket1FromCoordinates.X + socket1FromCoordinates.OffsetX, Y = socket1FromCoordinates.Y + socket1FromCoordinates.OffsetY, Z = socket1FromCoordinates.Z + socket1FromCoordinates.OffsetZ, RZ = socket1FromCoordinates.RZ + socket1FromCoordinates.OffsetRZ },
          TestBench = testBenchFrom,
        });
      // tray盘放到夹具Socket2
      runTask.TaskToQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = socket2FromCoordinates.X + socket2FromCoordinates.OffsetX, Y = socket2FromCoordinates.Y + socket2FromCoordinates.OffsetY, Z = socket2FromCoordinates.Z + socket2FromCoordinates.OffsetZ, RZ = socket2FromCoordinates.RZ + socket2FromCoordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      // 夹具Socket1放到夹具Socket1
      runTask.TaskToQue.Enqueue(
       new TaskTo
       {
         PosNum = testBenchTo.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
         Action = $"{(int)MoveModuleAction.Put}",
         Coordinates = new Coordinates { X = socket1ToCoordinates.X + socket1ToCoordinates.OffsetX, Y = socket1ToCoordinates.Y + socket1ToCoordinates.OffsetY, Z = socket1ToCoordinates.Z + socket1ToCoordinates.OffsetZ, RZ = socket1ToCoordinates.RZ + socket1ToCoordinates.OffsetRZ },
         TestBench = testBenchTo,
       });
      // 夹具Socket2放到夹具Socket2
      runTask.TaskToQue.Enqueue(
       new TaskTo
       {
         PosNum = testBenchTo.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
         Action = $"{(int)MoveModuleAction.Put}",
         Coordinates = new Coordinates { X = socket2ToCoordinates.X + socket2ToCoordinates.OffsetX, Y = socket2ToCoordinates.Y + socket2ToCoordinates.OffsetY, Z = socket2ToCoordinates.Z + socket2ToCoordinates.OffsetZ, RZ = socket2ToCoordinates.RZ + socket2ToCoordinates.OffsetRZ },
         TestBench = testBenchTo,
       });
      controlCenterRunningInstance.RunTasks.Enqueue(runTask);
      if (string.IsNullOrEmpty(robotHelper.GetGripper())) // 所有抓手都空闲
      {
        controlCenterRunningInstance.TestModulePosition++;
      }
    }

    /// <summary>
    /// 追加前序夹具直接放模块到后序夹具的任务(双A模式)
    /// </summary>
    /// <param name="testBenchFrom">起始夹具</param>
    /// <param name="testBenchTo">目标夹具</param>
    public void DoubleSocketPutInJigRunTaskEnqueue(TestBench testBenchFrom, TestBench testBenchTo, RobotHelper robotHelper)
    {
      var coordinate = controlCenterRunningInstance.WholeTestTrayCoordinates[controlCenterRunningInstance.TestModulePosition - 1];
      RunTask runTask = new RunTask();
      var socket1FromCoordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket1").Result.FirstOrDefault();
      var socket2FromCoordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket2").Result.FirstOrDefault();
      var socket1ToCoordinates = jigServices.GetSettings(j => j.Number == testBenchTo.Number && j.Type == "Socket1").Result.FirstOrDefault();
      var socket2ToCoordinates = jigServices.GetSettings(j => j.Number == testBenchTo.Number && j.Type == "Socket2").Result.FirstOrDefault();
      runTask.TaskFlag = 15;
      // 夹具Socket1放到夹具Socket1
      runTask.TaskToQue.Enqueue(
       new TaskTo
       {
         PosNum = testBenchTo.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
         Action = $"{(int)MoveModuleAction.Put}",
         Coordinates = new Coordinates { X = socket1ToCoordinates.X + socket1ToCoordinates.OffsetX, Y = socket1ToCoordinates.Y + socket1ToCoordinates.OffsetY, Z = socket1ToCoordinates.Z + socket1ToCoordinates.OffsetZ, RZ = socket1ToCoordinates.RZ + socket1ToCoordinates.OffsetRZ },
         TestBench = testBenchTo,
       });
      // 夹具Socket2放到夹具Socket2
      runTask.TaskToQue.Enqueue(
       new TaskTo
       {
         PosNum = testBenchTo.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
         Action = $"{(int)MoveModuleAction.Put}",
         Coordinates = new Coordinates { X = socket2ToCoordinates.X + socket2ToCoordinates.OffsetX, Y = socket2ToCoordinates.Y + socket2ToCoordinates.OffsetY, Z = socket2ToCoordinates.Z + socket2ToCoordinates.OffsetZ, RZ = socket2ToCoordinates.RZ + socket2ToCoordinates.OffsetRZ },
         TestBench = testBenchTo,
       });
      // 从tray盘取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = $"{(int)MoveModulePosition.TestTray}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = coordinate
        });
      // tray盘放到夹具Socket1
      runTask.TaskToQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = socket1FromCoordinates.X + socket1FromCoordinates.OffsetX, Y = socket1FromCoordinates.Y + socket1FromCoordinates.OffsetY, Z = socket1FromCoordinates.Z + socket1FromCoordinates.OffsetZ, RZ = socket1FromCoordinates.RZ + socket1FromCoordinates.OffsetRZ },
          TestBench = testBenchFrom,
        });
      // tray盘放到夹具Socket2
      runTask.TaskToQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Put}",
          Coordinates = new Coordinates { X = socket2FromCoordinates.X + socket2FromCoordinates.OffsetX, Y = socket2FromCoordinates.Y + socket2FromCoordinates.OffsetY, Z = socket2FromCoordinates.Z + socket2FromCoordinates.OffsetZ, RZ = socket2FromCoordinates.RZ + socket2FromCoordinates.OffsetRZ },
          TestBench = testBenchFrom
        });

      controlCenterRunningInstance.RunTasks.Enqueue(runTask);
      if (string.IsNullOrEmpty(robotHelper.GetGripper())) // 所有抓手都空闲
      {
        controlCenterRunningInstance.TestModulePosition++;
      }
    }

    /// <summary>
    /// 追加从夹具抓取模块的任务(双A模式)
    /// </summary>
    /// <param name="testBenchFrom">起始夹具</param>
    /// <param name="testBenchTo">目标夹具</param>
    public void DoubleSocketTakeFromJigRunTaskEnqueue(TestBench testBenchFrom, RobotHelper robotHelper)
    {
      var coordinate = controlCenterRunningInstance.WholeTestTrayCoordinates[controlCenterRunningInstance.TestModulePosition - 1];
      RunTask runTask = new RunTask();
      var socket1FromCoordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket1").Result.FirstOrDefault();
      var socket2FromCoordinates = jigServices.GetSettings(j => j.Number == testBenchFrom.Number && j.Type == "Socket2").Result.FirstOrDefault();
      runTask.TaskFlag = 11;
      // 从夹具Socket1取
      runTask.TaskFromQue.Enqueue(
        new TaskFrom
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = socket1FromCoordinates.X + socket1FromCoordinates.OffsetX, Y = socket1FromCoordinates.Y + socket1FromCoordinates.OffsetY, Z = socket1FromCoordinates.Z + socket1FromCoordinates.OffsetZ, RZ = socket1FromCoordinates.RZ + socket1FromCoordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      // 从夹具Socket2取
      runTask.TaskFromQue.Enqueue(
        new TaskTo
        {
          PosNum = testBenchFrom.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
          Action = $"{(int)MoveModuleAction.Pick}",
          Coordinates = new Coordinates { X = socket2FromCoordinates.X + socket2FromCoordinates.OffsetX, Y = socket2FromCoordinates.Y + socket2FromCoordinates.OffsetY, Z = socket2FromCoordinates.Z + socket2FromCoordinates.OffsetZ, RZ = socket2FromCoordinates.RZ + socket2FromCoordinates.OffsetRZ },
          TestBench = testBenchFrom
        });
      controlCenterRunningInstance.RunTasks.Enqueue(runTask);
      controlCenterRunningInstance.TakeFromJigRunTask = runTask;  // 存储队列信息用于后序取放动作
      if (string.IsNullOrEmpty(robotHelper.GetGripper())) // 所有抓手都空闲
      {
        controlCenterRunningInstance.TestModulePosition++;
      }
    }

    /// <summary>
    /// 追加待测tray盘到夹具的任务
    /// </summary>
    /// <param name="testBenchWithJigTo">目标夹具</param>
    //public void RunTaskEnqueue(TestBenchWithJig testBenchWithJigTo)
    //{
    //  var coordinate = ControlCenterRunningInstance.TestTrayCoordinates[ControlCenterRunningInstance.TestModulePosition];
    //  RunTask runTask = new RunTask();
    //  runTask.TaskFlag = 2;
    //  runTask.TaskFromQue = new TaskFrom
    //  {
    //    PosNum = $"{(int)MoveModulePosition.TestTray}",
    //    Action = $"{(int)MoveModuleAction.Pick}",
    //    Coordinates = new Coordinates { X = coordinate.X, Y = coordinate.Y, Z = coordinate.Z, RZ = coordinate.RZ }
    //  };
    //  runTask.TaskToQue = new TaskTo
    //  {
    //    PosNum = testBenchWithJigTo.Jig.Number < 9 ? $"{(int)MoveModulePosition.LeftJig}" : $"{(int)MoveModulePosition.RightJig}",
    //    Action = $"{(int)MoveModuleAction.Put}",
    //    Coordinates = new Coordinates { X = testBenchWithJigTo.Jig.X, Y = testBenchWithJigTo.Jig.Y, Z = testBenchWithJigTo.Jig.Z, RZ = testBenchWithJigTo.Jig.RZ }
    //  };
    //  ControlCenterRunningInstance.RunTasks.Enqueue(runTask);
    //  ControlCenterRunningInstance.TestModulePosition++;
    //}

    /// <summary>
    /// 追加待测盘到ng盘任务
    /// </summary>
    //public void RunTaskEnqueue()
    //{
    //  var coordinate = ControlCenterRunningInstance.TestTrayCoordinates[ControlCenterRunningInstance.TestModulePosition];
    //  RunTask runTask = new RunTask();
    //  runTask.TaskFlag = 4;
    //  runTask.TaskFromQue = new TaskFrom
    //  {
    //    PosNum = $"{(int)MoveModulePosition.TestTray}",
    //    Action = $"{(int)MoveModuleAction.Pick}",
    //    Coordinates = new Coordinates { X = coordinate.X, Y = coordinate.Y, Z = coordinate.Z, RZ = coordinate.RZ }
    //  };
    //  // TODO:判断ng盘是否放满
    //  var ngCoordinate = ControlCenterRunningInstance.RightNGTrayCoordinates[ControlCenterRunningInstance.TestModulePosition];
    //  runTask.TaskToQue = new TaskTo
    //  {
    //    PosNum = $"{(int)MoveModulePosition.RightNG}",
    //    Action = $"{(int)MoveModuleAction.Put}",
    //    Coordinates = new Coordinates { X = ngCoordinate.X, Y = ngCoordinate.Y, Z = ngCoordinate.Z, RZ = ngCoordinate.RZ }
    //  };
    //  ControlCenterRunningInstance.RunTasks.Enqueue(runTask);
    //  ControlCenterRunningInstance.TestModulePosition++;
    //}

    /// <summary>
    /// 等待追加tray盘/夹具到夹具的任务到任务队列
    /// </summary>
    /// <param name="testBenchHelper"></param>
    /// <param name="testSequence"></param>
    /// <param name="currentTestBenchWithJig"></param>
    public void WaitForRunTaskEnqueue(TestBenchHelper testBenchHelper, TestSequence testSequence, TestBenchWithJig currentTestBenchWithJig, RobotHelper robotHelper)
    {
      _ = Task.Run(() =>
      {
        TestBenchWithJig testBenchWithJig = null;
        while (testBenchWithJig == null)
        {
          testBenchWithJig = testBenchHelper.GetTestBenchWithJigBySequence(testSequence).Result;
          Task.Delay(500).Wait(); // 添加延迟避免因频繁获取数据导致异常
        }
        Jig2JigRunTaskEnqueue(currentTestBenchWithJig, testBenchWithJig, robotHelper);
      });
    }

    /// <summary>
    /// 等待追加tray盘/夹具到夹具的任务到任务队列(双A模式)
    /// </summary>
    /// <param name="testBenchHelper"></param>
    /// <param name="testSequence"></param>
    /// <param name="currentTestBenchWithJig"></param>
    public void WaitForRunTaskEnqueue(TestBenchHelper testBenchHelper, TestSequence testSequence, TestBench currentTestBench, RobotHelper robotHelper)
    {
      _ = Task.Run(() =>
      {
        TestBench testBench = null;
        while (testBench == null)
        {
          testBench = testBenchHelper.GetTestBenchBySequence(testSequence).Result;
          Task.Delay(500).Wait(); // 添加延迟避免因频繁获取数据导致异常
        }
        DoubleSocketPutInJigRunTaskEnqueue(currentTestBench, testBench, robotHelper);
        // DoubleSocketJig2JigRunTaskEnqueue(currentTestBench, testBench, robotHelper);
      });
    }

    /// <summary>
    /// 等待追加从夹具抓取模块的任务到任务队列(双A模式)
    /// </summary>
    /// <param name="robotHelper"></param>
    public Task WaitForRunTaskEnqueue(TestBench currentTestBench, RobotHelper robotHelper)
    {
      return Task.Run(() =>
       {
         var isGripperFree = false;
         while (!isGripperFree)
         {
           isGripperFree = string.IsNullOrEmpty(robotHelper.GetGripper());
           Task.Delay(500).Wait();
         }
         DoubleSocketTakeFromJigRunTaskEnqueue(currentTestBench, robotHelper);
       });
    }

    #endregion
  }
}
