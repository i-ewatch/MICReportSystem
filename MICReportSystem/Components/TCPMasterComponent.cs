using MICReportSystem.Enums;
using MICReportSystem.Methods;
using MICReportSystem.Mysql_Module;
using MICReportSystem.Protocols;
using Serilog;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace MICReportSystem.Components
{
    public partial class TCPMasterComponent : Field4Component
    {
        public TCPMasterComponent(GatewayConfig gateway, MysqlMethod mysql)
        {
            InitializeComponent();
            GatewayConfig = gateway;
            MysqlMethod = mysql;
        }
        private GatewayConfig GatewayConfig { get; set; }
        public TCPMasterComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
        protected override void AfterMyWorkStateChanged(object sender, EventArgs e)
        {
            if (myWorkState)
            {
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
                        using (TcpClient client = new TcpClient(GatewayConfig.Location, Convert.ToInt32(GatewayConfig.Rate)))
                        {
                            master = Factory.CreateMaster(client);//建立TCP通訊
                            master.Transport.ReadTimeout = 500;
                            master.Transport.Retries = 1;
                            foreach (var item in ElectricAbsProtocols)
                            {
                                item.ReadData(master);
                                Thread.Sleep(10);
                            }
                        }
                        ComponentTime = DateTime.Now;
                    }
                    catch (ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"Modbus TCP 無法通訊 IP : {GatewayConfig.Location} Port : {GatewayConfig.Rate}");
                        foreach (var item in ElectricAbsProtocols)
                        {
                            item.ConnectFlag = false;
                        }
                    }
                }
                else
                {
                    Thread.Sleep(80);
                }
            }
        }
    }
}
