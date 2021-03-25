using MICReportSystem.Enums;
using MICReportSystem.Methods;
using MICReportSystem.Mysql_Module;
using MICReportSystem.Protocols;
using NModbus.Serial;
using Serilog;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace MICReportSystem.Components
{
    public partial class SerialportMasterComponent : Field4Component
    {
        public SerialportMasterComponent(GatewayConfig gateway,MysqlMethod mysql)
        {
            InitializeComponent();
            GatewayConfig = gateway;
            MysqlMethod = mysql;
        }
        private GatewayConfig GatewayConfig { get; set; }

        public SerialportMasterComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        protected override void AfterMyWorkStateChanged(object sender, EventArgs e)
        {
            if (myWorkState)
            {
                int Baudate = Convert.ToInt32(GatewayConfig.Rate.Split(',')[0]);
                int DataBits = Convert.ToInt32(GatewayConfig.Rate.Split(',')[1]);
                string parity = GatewayConfig.Rate.Split(',')[2];
                int stopbits = Convert.ToInt32(GatewayConfig.Rate.Split(',')[3]);

                RS485 = new SerialPort(GatewayConfig.Location);
                RS485.BaudRate = Baudate;
                RS485.DataBits = DataBits;
                switch (parity)
                {
                    case "N":
                        {
                            RS485.Parity = Parity.None;
                        }
                        break;
                    case "O":
                        {
                            RS485.Parity = Parity.Odd;
                        }
                        break;
                    case "E":
                        {
                            RS485.Parity = Parity.Even;
                        }
                        break;
                }
                RS485.StopBits = (StopBits)stopbits;
                var ElectricConfig = MysqlMethod.Search_ElectricConfig(GatewayConfig.GatewayIndex);
                foreach (var item in ElectricConfig)
                {
                    ElectricTypeEnum electricTypeEnum = (ElectricTypeEnum)item.ElectricTypeEnum;
                    switch (electricTypeEnum)
                    {
                        case ElectricTypeEnum.BAW_4C:
                            {
                                BAW_4CProtocol protocol = new BAW_4CProtocol()
                                {
                                    ID = (byte)item.DeviceID,
                                    GatewayIndex = item.GatewayIndex,
                                    DeviceIndex = item.DeviceIndex,
                                    ElectricTypeEnum = item.ElectricTypeEnum,
                                    LoopTypeEnum = item.LoopTypeEnum,
                                    PhaseTypeEnum = item.PhaseTypeEnum,
                                    PhaseAngleTypeEnum = item.PhaseAngleTypeEnum,
                                    MysqlMethod = MysqlMethod
                                };
                                ElectricAbsProtocols.Add(protocol);
                            }
                            break;
                    }
                }
                ComponentThread = new Thread(Analysis);
                ComponentThread.Start();
            }
            else
            {
                if (RS485.IsOpen)
                {
                    RS485.Close();
                }
                if (ComponentThread != null)
                {
                    ComponentThread.Abort();
                }
            }
        }
        private void Analysis()
        {
            while (myWorkState)
            {
                TimeSpan timeSpan = DateTime.Now.Subtract(ComponentTime);
                if (timeSpan.TotalMilliseconds >= 1000)
                {
                    try
                    {
                        #region Rs485通訊功能初始化
                        try
                        {
                            if (!RS485.IsOpen)
                                RS485.Open();
                        }
                        catch (ArgumentException)
                        {
                            Log.Error("通訊埠設定有誤");
                        }
                        catch (InvalidOperationException)
                        {
                            Log.Error("通訊埠被占用");
                        }
                        catch (IOException)
                        {
                            //Log.Error("通訊埠無效");
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex, "通訊埠發生不可預期的錯誤。");
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"Connect Comport error.");
                        throw;
                    }
                    try
                    {
                        master = ModbusFactoryExtensions.CreateRtuMaster(Factory, RS485);//建立RTU通訊
                        master.Transport.ReadTimeout = 500;
                        master.Transport.Retries = 1;
                        int Index = 0;
                        var ReportConfigs = MysqlMethod.Search_ReportConfig();
                        foreach (var item in ElectricAbsProtocols)
                        {
                            item.ReportConfig = ReportConfigs[Index];
                            item.ReadData(master);
                            Index++;
                            Thread.Sleep(10);
                        }
                    }
                    catch (ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"Connect to device({GatewayConfig.Location}) failed.");
                        foreach (var item in ElectricAbsProtocols)
                        {
                            item.ConnectFlag = false;
                        }
                    }
                    ComponentTime = DateTime.Now;
                }
                else
                {
                    Thread.Sleep(80);
                }
            }
        }
    }
}
