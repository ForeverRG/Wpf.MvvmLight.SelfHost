using Wpf.MvvmLight.SelfHost.Api.Model;
using Wpf.MvvmLight.SelfHost.Repository.DBSeed;
using SqlSugar;
using System.Web.Http;

namespace Wpf.MvvmLight.SelfHost.Api.Controllers
{
  [Route("api/[controller]/[action]")]
  public class DbFirstController : ApiController
  {
    private readonly SqlSugarScope _sqlSugarClient;
    private readonly bool _isDevEnv; // 是否为开发环境
    private readonly string _connID; // 数据库连接ID

    /// <summary>
    /// 构造函数
    /// </summary>
    public DbFirstController()
    {
      _sqlSugarClient = new SQLiteDbContext().SqlSugarDB;
      _isDevEnv = true;
      _connID = "sqlite";
    }

    /// <summary>
    /// 获取 整体框架 文件(主库)(一般可用第一次生成)
    /// </summary>
    /// <returns></returns>
    [Route("api/[controller]/allframefiles")]
    [HttpGet]
    public HttpMessage<string> GetFrameFiles()
    {
      var data = new HttpMessage<string>() { Success = true, Msg = "" };
      if (_isDevEnv)
      {
        data.Response += @"file path is:C:\my-file\ || ";
        data.Response += $"Controller层生成：{FrameSeed.CreateControllers(_sqlSugarClient)} || ";
        data.Response += $"Model层生成：{FrameSeed.CreateModels(_sqlSugarClient, _connID)} || ";
        data.Response += $"IRepositorys层生成：{FrameSeed.CreateIRepositorys(_sqlSugarClient, _connID)} || ";
        data.Response += $"IServices层生成：{FrameSeed.CreateIServices(_sqlSugarClient, _connID)} || ";
        data.Response += $"Repository层生成：{FrameSeed.CreateRepository(_sqlSugarClient, _connID)} || ";
        data.Response += $"Services层生成：{FrameSeed.CreateServices(_sqlSugarClient, _connID)} || ";
      }
      else
      {
        data.Success = false;
        data.Msg = "当前不处于开发模式，代码生成不可用！";
      }
      return data;
    }

    /// <summary>
    /// 获取仓储层和服务层(需指定表名和数据库)
    /// </summary>
    /// <param name="tableNames">需要生成的表名</param>
    /// <returns></returns>
    [HttpPost]
    public HttpMessage<string> GetFrameFilesByTableNames([FromBody] string[] tableNames)
    {
      var data = new HttpMessage<string>() { Success = true, Msg = "" };
      if (_isDevEnv)
      {
        data.Response += $"IRepositorys层生成：{FrameSeed.CreateIRepositorys(_sqlSugarClient, _connID, false, tableNames)} || ";
        data.Response += $"IServices层生成：{FrameSeed.CreateIServices(_sqlSugarClient, _connID, false, tableNames)} || ";
        data.Response += $"Repository层生成：{FrameSeed.CreateRepository(_sqlSugarClient, _connID, false, tableNames)} || ";
        data.Response += $"Services层生成：{FrameSeed.CreateServices(_sqlSugarClient, _connID, false, tableNames)} || ";
      }
      else
      {
        data.Success = false;
        data.Msg = "当前不处于开发模式，代码生成不可用！";
      }

      return data;
    }

    /// <summary>
    /// 获取实体(需指定表名和数据库)
    /// </summary>
    /// <param name="tableNames">需要生成的表名</param>
    /// <returns></returns>
    [HttpPost]
    public HttpMessage<string> GetFrameFilesByTableNamesForEntity([FromBody] string[] tableNames)
    {
      var data = new HttpMessage<string>() { Success = true, Msg = "" };
      if (_isDevEnv)
      {
        data.Response += $"Models层生成：{FrameSeed.CreateModels(_sqlSugarClient, _connID, false, tableNames)}";
      }
      else
      {
        data.Success = false;
        data.Msg = "当前不处于开发模式，代码生成不可用！";
      }
      return data;
    }

    /// <summary>
    /// 获取控制器(需指定表名和数据库)
    /// </summary>
    /// <param name="tableNames">需要生成的表名</param>
    /// <returns></returns>
    [HttpPost]
    public HttpMessage<string> GetFrameFilesByTableNamesForController([FromBody] string[] tableNames)
    {
      var data = new HttpMessage<string>() { Success = true, Msg = "" };
      if (_isDevEnv)
      {
        data.Response += $"Controllers层生成：{FrameSeed.CreateControllers(_sqlSugarClient, _connID, false, tableNames)}";
      }
      else
      {
        data.Success = false;
        data.Msg = "当前不处于开发模式，代码生成不可用！";
      }
      return data;
    }

    /// <summary>
    /// DbFrist 根据数据库表名 生成整体框架,包含Model层(一般可用第一次生成)
    /// </summary>
    /// <param name="tableNames">需要生成的表名</param>
    /// <returns></returns>
    [HttpPost]
    public HttpMessage<string> GetAllFrameFilesByTableNames([FromBody] string[] tableNames)
    {
      var data = new HttpMessage<string>() { Success = true, Msg = "" };
      if (_isDevEnv)
      {
        data.Response += $"Controller层生成：{FrameSeed.CreateControllers(_sqlSugarClient, _connID, false, tableNames)} || ";
        data.Response += $"Model层生成：{FrameSeed.CreateModels(_sqlSugarClient, _connID, false, tableNames)} || ";
        data.Response += $"IRepositorys层生成：{FrameSeed.CreateIRepositorys(_sqlSugarClient, _connID, false, tableNames)} || ";
        data.Response += $"IServices层生成：{FrameSeed.CreateIServices(_sqlSugarClient, _connID, false, tableNames)} || ";
        data.Response += $"Repository层生成：{FrameSeed.CreateRepository(_sqlSugarClient, _connID, false, tableNames)} || ";
        data.Response += $"Services层生成：{FrameSeed.CreateServices(_sqlSugarClient, _connID, false, tableNames)} || ";
      }
      else
      {
        data.Success = false;
        data.Msg = "当前不处于开发模式，代码生成不可用！";
      }
      return data;
    }
  }
}