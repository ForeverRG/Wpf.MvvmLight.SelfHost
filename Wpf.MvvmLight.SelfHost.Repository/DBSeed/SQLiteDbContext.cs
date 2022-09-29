using System;
using System.Configuration;
using SqlSugar;
using System.Diagnostics;
using System.Reflection;
using Wpf.MvvmLight.SelfHost.Model;
using System.ComponentModel.DataAnnotations;
using Wpf.MvvmLight.SelfHost.IRepository;

namespace Wpf.MvvmLight.SelfHost.Repository.DBSeed
{
  public class SQLiteDbContext
  {
    // 连接字符串
    public readonly string connStr = ConfigurationManager.ConnectionStrings["SQLiteConnStr"].ConnectionString;
    string _sConnPath = string.Format("{0}\\DB", System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));

    // orm实例
    public SqlSugarScope SqlSugarDB { get; private set; }

    public SQLiteDbContext()
    {
      SqlSugarDB = GetInstance();
    }

    public void InitDatabaseAndTables()
    {
      InitDB(false, 300,
        typeof(ControlCenter),
        typeof(PLC),
        typeof(Robot),
        typeof(TestBench),
        typeof(VisionModel),
        typeof(Jig),
        typeof(ModuleSettings),
        typeof(Tray),
        typeof(Config),
        typeof(Channel),
        typeof(ModuleType),
        typeof(GripperLoadInfo)
        );
    }

    /// <summary>
    /// 获取数据库对象实例
    /// </summary>
    /// <returns></returns>
    private SqlSugarScope GetInstance()
    {
      if (!System.IO.Directory.Exists(_sConnPath)) System.IO.Directory.CreateDirectory(_sConnPath);
      //创建数据库对象 SqlSugarScope
      SqlSugarScope scope = new SqlSugarScope(new ConnectionConfig()
      {
        ConnectionString = string.Format("DataSource={0}\\{1}", _sConnPath, connStr),//连接符字串
        DbType = DbType.Sqlite,
        IsAutoCloseConnection = true, //不设成true要手动close
        ConfigureExternalServices = new ConfigureExternalServices   // 可空类型自动化配置
        {
          EntityService = (c, p) =>
          {
            // int?  decimal?这种 isnullable=true
            if (c.PropertyType.IsGenericType &&
            c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
              p.IsNullable = true;
            }
            else if (c.PropertyType == typeof(string) &&
                     c.GetCustomAttribute<RequiredAttribute>() == null)
            { //string类型如果没有Required isnullable=true
              p.IsNullable = true;
            }
          }
        }
      });

      return scope;
    }

    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="backup"></param>
    /// <param name="stringDefaultLength"></param>
    /// <param name="types"></param>
    private void InitDB(bool backup = false, int stringDefaultLength = 500, params Type[] types)
    {
      SqlSugarDB.CodeFirst.SetStringDefaultLength(stringDefaultLength);

      SqlSugarDB.DbMaintenance.CreateDatabase();
      if (backup)
      {
        SqlSugarDB.CodeFirst.BackupTable().InitTables(types);
      }
      else
      {
        SqlSugarDB.CodeFirst.InitTables(types);
      }
    }

    #region 手动注入服务
    public IControlCenterRepository ControlCenterDb { get { return new ControlCenterRepository(SqlSugarDB); } }
    public IPLCRepository PLCDb { get { return new PLCRepository(SqlSugarDB); } }
    public IJigRepository JigDb { get { return new JigRepository(SqlSugarDB); } }
    public IRobotRepository RobotDb { get { return new RobotRepository(SqlSugarDB); } }
    public ITestBenchRepository TestBenchDb { get { return new TestBenchRepository(SqlSugarDB); } }
    public IVisionRepository VisionDb { get { return new VisionRepository(SqlSugarDB); } }
    public IModuleSettingsRepository ModuleSettingsDb { get { return new ModuleSettingsRepository(SqlSugarDB); } }
    public ITrayRepository TrayDb { get { return new TrayRepository(SqlSugarDB); } }
    public IConfigRepository ConfigDb { get { return new ConfigRepository(SqlSugarDB); } }
    public IChannelRepository ChannelDb { get { return new ChannelRepository(SqlSugarDB); } }
    public IModuleTypeRepository ModuleTypeDb { get { return new ModuleTypeRepository(SqlSugarDB); } }
    public IGripperLoadInfoRepository GripperLoadInfoDb { get { return new GripperLoadInfoRepository(SqlSugarDB); } }
    #endregion
  }
}
