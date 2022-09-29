namespace Wpf.MvvmLight.SelfHost.Model.GlobalVariable
{
  /// <summary>
  /// PLC信号量地址
  /// </summary>
  public class PLCAddress
  {
    /// <summary>
    /// D32区地址
    /// </summary>
    public class D32
    {

      #region 说明
      //  心跳信号
      //  初始化PLC	1：初始化
      //  启动PLC	1：启动PLC
      //  Tray_1_Finish	1：测试Tray盘测试完成
      //  Tray_2_Finish	1：测试Tray盘测试完成
      //  抓手相机光源	0：光源OFF  1:光源ON
      //  NG_1_Full	1：NG1处满料
      //  NG_2_Full	1：NG2处满料
      #endregion

      /// <summary>
      /// 机种数据
      /// </summary>
      public const string D3200 = "D3200";
      /// <summary>
      /// 心跳信号
      /// </summary>
      public const string D3201 = "D3201";
      /// <summary>
      /// 初始化PLC
      /// </summary>
      public const string D3202 = "D3001";
      /// <summary>
      /// 料盘穴数
      /// </summary>
      public const string D3203 = "D3203";
      /// <summary>
      /// 报警
      /// </summary>
      public const string D3204 = "D3204";
      /// <summary>
      /// PC启动Robot
      /// </summary>
      public const string D3206 = "D3206";
      /// <summary>
      /// 启动PLC
      /// </summary>
      public const string D3208 = "D3208";
      /// <summary>
      /// 机种数据反馈
      /// </summary>
      public const string D3210 = "D3210";
      /// <summary>
      /// PLC运行代码
      /// </summary>
      public const string D3211 = "D3211";
      /// <summary>
      /// 料盘穴数反馈
      /// </summary>
      public const string D3213 = "D3213";
      /// <summary>
      /// 上光源
      /// </summary>
      public const string D3214 = "D3214";
      /// <summary>
      /// 下光源
      /// </summary>
      public const string D3215 = "D3215";
      /// <summary>
      /// 前后条光
      /// </summary>
      public const string D3217 = "D3217";
      /// <summary>
      /// 左右条光
      /// </summary>
      public const string D3218 = "D3218";
      ///// <summary>
      ///// 启动PLC
      ///// </summary>
      //public const string D3002 = "D3002";
      ///// <summary>
      ///// Tray_1_Finish
      ///// </summary>
      //public const string D3003 = "D3003";
      ///// <summary>
      ///// Tray_2_Finish
      ///// </summary>
      //public const string D3004 = "D3004";
      ///// <summary>
      ///// 抓手相机光源
      ///// </summary>
      //public const string D3005 = "D3005";
      ///// <summary>
      ///// NG_1_Full
      ///// </summary>
      //public const string D3006 = "D3006";
      ///// <summary>
      ///// NG_2_Full
      ///// </summary>
      //public const string D3007 = "D3007";
    }

    /// <summary>
    /// D31区地址
    /// </summary>
    public class D31
    {
      #region 说明
      //  PLC模式	0：手动模式 1：自动模式
      //  PLC初始化状态	0：未初始化 1：初始化完成
      //  PLC状态	0：未运行 1：运行中 2：报警
      //  Tray_1_OK	1：测试Tray盘有Tray
      //  Tray_2_OK	1：测试Tray盘有Tray
      //  抓手相机光源	0：光源OFF _OK    1:光源ON_OK
      //  NG_1_Can_Put	1：NG1可放模块
      //  NG_2_Can_Put	1：NG2可放模块
      #endregion

      /// <summary>
      /// PLC模式
      /// </summary>
      public const string D3100 = "D3100";
      /// <summary>
      /// PLC初始化状态
      /// </summary>
      public const string D3101 = "D3101";
      /// <summary>
      /// PLC状态
      /// </summary>
      public const string D3102 = "D3102";
      /// <summary>
      /// Tray_1_OK
      /// </summary>
      public const string D3103 = "D3103";
      /// <summary>
      /// Tray_2_OK
      /// </summary>
      public const string D3104 = "D3104";
      /// <summary>
      /// 抓手相机光源
      /// </summary>
      public const string D3105 = "D3105";
      /// <summary>
      /// NG_1_Can_Put
      /// </summary>
      public const string D3106 = "D3106";
      /// <summary>
      /// NG_2_Can_Put
      /// </summary>
      public const string D3107 = "D3107";

      public const string D3108 = "D3108";
    }

    #region D4区操作
    //  设备同时只能生产一种类型的芯片（机种数据请参考前页规划）但可以对每个检测工位设置不同的工序，其他数据区为备用，该数据区可以随时读写
    //  1.工序：0=不使用，1=1序，2=2序。
    //  2.工件放置：0=机器人未放入，1=机器人放入
    //  3.测试结果定义：0未测试，1，测试中，2测试OK，3测试NG，PC负责写入，当工件OFF的时候，PLC负责清除
    //  4.5V_02控制：0=OFF；1=ON(备用）
    //  5.DTR，RTS控制：0=拉低；1=拉高(备用）
    //  6.USB BOOT拉高使模块进入紧急下载，0=FF；1=ON(备用）
    //  7.5V控制：0=OFF；1=ON

    /// <summary>
    /// D40区：写工序（后面跟具体工位号）
    /// </summary>
    public const string D40 = "D40";
    /// <summary>
    /// D41区：写工件放置（后面跟具体工位号）
    /// </summary>
    public const string D41 = "D41";
    /// <summary>
    /// D42区：写测试结果（后面跟具体工位号）
    /// </summary>
    public const string D42 = "D42";
    /// <summary>
    /// D43区：写5v控制（端口2供电备用）开关（后面跟具体工位号）
    /// </summary>
    public const string D43 = "D43";
    /// <summary>
    /// D44区：写5v控制开关（后面跟具体工位号）
    /// </summary>
    public const string D44 = "D44";
    /// <summary>
    /// D45区：写USB BOOT（备用）（后面跟具体工位号）
    /// </summary>
    public const string D45 = "D45";
    /// <summary>
    /// D46区：写DTR控制（备用）（后面跟具体工位号）	
    /// </summary>
    public const string D46 = "D46";
    /// <summary>
    /// D47区：写RTS控制（备用）（后面跟具体工位号）
    /// </summary>
    public const string D47 = "D47";
    /// <summary>
    /// D48区：写Reload（后面跟具体工位号）
    /// </summary>
    public const string D48 = "D48";
    #endregion

    #region D5区操作
    //  该数据区是生产用数据缓存
    //  PC可以在该区域内读取当前PLC正在运行的参数。
    //  （虽然PC可以对该区域进行写操作，但请不要进行写操作）
    //  PC可以通过读取的数据进行监控
    //  1.有工件定义：0无工件，1有工件，由PLC控制
    //  2.可测试定义：0气缸未到位，1气缸到位，且工件未完成测试，由PLC控制
    //  3.可取工件：0=Robot不可取，1=Robot可取工件
    //  4.可放工件：0=Robot不可放，1=Robot可放工件

    /// <summary>
    /// D50区：读是否有工件（后面跟具体工位号）
    /// </summary>
    public const string D50 = "D50";
    /// <summary>
    /// D51区：读是否可测试（后面跟具体工位号）
    /// </summary>
    public const string D51 = "D51";
    /// <summary>
    /// D52区：读是否可取工件（后面跟具体工位号）
    /// </summary>
    public const string D52 = "D52";
    /// <summary>
    /// D53区：读是否可放工件（后面跟具体工位号）
    /// </summary>
    public const string D53 = "D53";
    /// <summary>
    /// D54区：读端口号（后面跟具体工位号）
    /// </summary>
    public const string D54 = "D54";
    #endregion
  }
}

