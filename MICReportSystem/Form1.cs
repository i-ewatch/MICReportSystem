using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using MICReportSystem.Components;
using MICReportSystem.Configuration;
using MICReportSystem.Enums;
using MICReportSystem.Methods;
using MICReportSystem.Mysql_Module;
using MICReportSystem.Protocols;
using MICReportSystem.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MICReportSystem
{
    public partial class Form1 : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        /// <summary>
        /// 初始路徑
        /// </summary>
        private string MyWorkPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 錯誤資訊
        /// </summary>
        private string ErrorStr { get; set; } = string.Empty;
        /// <summary>
        /// 錯誤泡泡視窗
        /// </summary>
        public FlyoutPanel ErrorflyoutPanel;
        /// <summary>
        /// 浮動視窗
        /// </summary>
        public FlyoutDialog flyout { get; set; }
        /// <summary>
        /// 浮動視窗旗標
        /// </summary>
        public bool FlyoutFlag { get; set; }
        #region JSON
        /// <summary>
        /// 資料庫資訊
        /// </summary>
        private SystemSetting SystemSetting { get; set; }
        /// <summary>
        /// 按鈕資訊
        /// </summary>
        private ButtonSetting ButtonSetting { get; set; }
        /// <summary>
        /// 匯出報表資訊
        /// </summary>
        private XtraReportSetting XtraReportSetting { get; set; }
        #endregion
        #region Method
        /// <summary>
        /// 資料庫方法
        /// </summary>
        private MysqlMethod MysqlMethod { get; set; }
        /// <summary>
        /// 按鈕方法
        /// </summary>
        private ButtonMethod ButtonMethod { get; set; }
        #endregion
        #region Views
        /// <summary>
        /// 切換畫面物件
        /// </summary>
        private NavigationFrame NavigationFrame { get; set; }
        /// <summary>
        /// 總畫面物件
        /// </summary>
        private List<Field4UserControl> Field4UserControls { get; set; } = new List<Field4UserControl>();
        #endregion
        #region Component
        /// <summary>
        /// 通道資訊
        /// </summary>
        private List<GatewayConfig> GatewayConfigs { get; set; }
        /// <summary>
        /// 總通道物件
        /// </summary>
        private List<Field4Component> Field4Components { get; set; } = new List<Field4Component>();
        /// <summary>
        /// 總通訊物件
        /// </summary>
        private List<AbsProtocol> ElectricAbsProtocols { get; set; } = new List<AbsProtocol>();
        /// <summary>
        /// 月報表畫面(自動匯出檔案)
        /// </summary>
        private XtraReportUserControl xtraReportUserControl { get; set; }
        #endregion
        public Form1()
        {
            #region Serilog initial
            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}\\log\\log-.txt",
                                      rollingInterval: RollingInterval.Day,
                                      outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                        .CreateLogger();        //宣告Serilog初始化
            #endregion

            #region Loading initial
            FluentSplashScreenOptions op = new FluentSplashScreenOptions();
            op.Title = "帆宣自動化抄表系統";
            op.Subtitle = "Automatic Meter Reading System";
            op.LeftFooter = "Copyright © 2021 SIN MAO Energy CO., LTD." + Environment.NewLine + "All Rights reserved.";
            op.LoadingIndicatorType = FluentLoadingIndicatorType.Dots;
            op.OpacityColor = Color.FromArgb(62, 91, 135);
            op.Opacity = 130;
            SplashScreenManager.ShowFluentSplashScreen(
                            op,
                            parentForm: this,
                            useFadeIn: true,
                            useFadeOut: true
                        );
            #endregion

            #region 載入資料庫JSON
            op.RightFooter = $"載入資料庫資訊";
            SplashScreenManager.Default.SendCommand(FluentSplashScreenCommand.UpdateOptions, op);
            SystemSetting = InitialMethod.SystemLoad();
            Thread.Sleep(1000);
            #endregion

            #region 載入按鈕JSON
            op.RightFooter = $"載入按鈕資訊";
            SplashScreenManager.Default.SendCommand(FluentSplashScreenCommand.UpdateOptions, op);
            ButtonSetting = InitialMethod.InitialButtonLoad();
            Thread.Sleep(1000);
            #endregion

            #region 載入匯出報表JSON
            op.RightFooter = $"載入匯出報表資訊";
            SplashScreenManager.Default.SendCommand(FluentSplashScreenCommand.UpdateOptions, op);
            XtraReportSetting = InitialMethod.InitialXtraReportLoad();
            Thread.Sleep(1000);
            #endregion

            #region JSON錯誤資訊檢查
            if (SystemSetting == null && ButtonSetting == null && XtraReportSetting == null)
            {
                ErrorStr = "資料庫與按鈕Json錯誤";
            }
            else if (SystemSetting != null && ButtonSetting == null && XtraReportSetting != null)
            {
                ErrorStr = "按鈕Json錯誤";
            }
            else if (SystemSetting == null && ButtonSetting != null && XtraReportSetting != null)
            {
                ErrorStr = "資料庫Json錯誤";
            }
            else if (SystemSetting != null && ButtonSetting != null && XtraReportSetting == null)
            {
                ErrorStr = "匯出報表Json錯誤";
            }
            if (ErrorStr == "")
            {
                op.RightFooter = $"載入完成";
                SplashScreenManager.Default.SendCommand(FluentSplashScreenCommand.UpdateOptions, op);
                Thread.Sleep(1000);
                SplashScreenManager.CloseForm();
            }
            else
            {
                op.RightFooter = $"{ErrorStr}";
                SplashScreenManager.Default.SendCommand(FluentSplashScreenCommand.UpdateOptions, op);
                Thread.Sleep(5000);
                SplashScreenManager.CloseForm();
            }
            #endregion

            InitializeComponent();
            if (ErrorStr == "")
            {
                Change_Logo();//載入Logo
                SettingbarButtonItem.ImageOptions.Image = imageCollection1.Images["technology"];//設定按鈕圖
                #region 建立資料庫物件
                MysqlMethod = new MysqlMethod(SystemSetting);
                if (SystemSetting != null)
                {
                    GatewayConfigs = MysqlMethod.Search_GatewayConfig();
                }
                #endregion

                #region 建立通訊
                if (GatewayConfigs != null)
                {
                    foreach (var item in GatewayConfigs)
                    {
                        GatewayTypeEnum gatewayType = (GatewayTypeEnum)item.GatewayTypeEnum;
                        switch (gatewayType)
                        {
                            case GatewayTypeEnum.ModbusRTU:
                                {
                                    SerialportMasterComponent serialport = new SerialportMasterComponent(item, MysqlMethod) { MysqlMethod = MysqlMethod };
                                    serialport.MyWorkState = true;
                                    Field4Components.Add(serialport);
                                }
                                break;
                            case GatewayTypeEnum.ModbusTCP:
                                {
                                    TCPMasterComponent TCP = new TCPMasterComponent(item, MysqlMethod) { MysqlMethod = MysqlMethod };
                                    TCP.MyWorkState = true;
                                    Field4Components.Add(TCP);
                                }
                                break;
                        }
                    }
                }
                #endregion

                #region 建立按鈕物件
                NavigationFrame = new NavigationFrame() { Dock = DockStyle.Fill };
                NavigationFrame.Parent = ViewpanelControl;
                ButtonMethod = new ButtonMethod() { Form1 = this, navigationFrame = NavigationFrame };
                ButtonMethod.AccordionLoad(accordionControl1, ButtonSetting);
                #endregion

                #region 建立畫面
                foreach (var Componentitem in Field4Components)
                {
                    foreach (var Absprotocolitem in Componentitem.ElectricAbsProtocols)
                    {
                        ElectricAbsProtocols.Add(Absprotocolitem);
                    }
                }
                #region 主畫面
                MainUserControl main = new MainUserControl(MysqlMethod, ElectricAbsProtocols) { Dock = DockStyle.Fill };
                NavigationFrame.AddPage(main);
                Field4UserControls.Add(main);
                #endregion
                #region 報表畫面
                ChartUserControl chart = new ChartUserControl(MysqlMethod) { Dock = DockStyle.Fill };
                NavigationFrame.AddPage(chart);
                Field4UserControls.Add(chart);
                #endregion
                #region 月報表畫面
                xtraReportUserControl = new XtraReportUserControl(MysqlMethod) { Dock = DockStyle.Fill };
                NavigationFrame.AddPage(xtraReportUserControl);
                #endregion
                #endregion
            }
            timer1.Interval = 1000;
            timer1.Enabled = true;
        }

        #region 更改Logo
        /// <summary>
        /// 更改Logo
        /// </summary>
        public void Change_Logo()
        {
            if (!Directory.Exists($"{MyWorkPath}\\Images"))
                Directory.CreateDirectory($"{MyWorkPath}\\Images");
            if (File.Exists($"{MyWorkPath}\\Images\\Logo.png"))
            {
                LogopictureEdit.Image = Image.FromFile($"{MyWorkPath}\\Images\\Logo.png");
            }
        }
        #endregion

        #region 視窗關閉事件
        /// <summary>
        /// 視窗關閉事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var item in Field4Components)
            {
                item.MyWorkState = false;
            }
        }
        #endregion

        #region 時間執行緒
        /// <summary>
        /// 時間執行緒
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ErrorStr == "")
            {
                if (Field4UserControls.Count > ButtonMethod.ViewIndex)
                {
                    Field4UserControls[ButtonMethod.ViewIndex].TextChange();
                }
                xtraReportUserControl.TextChange();
            }
            else
            {
                this.Close();
            }
            ComponentFail();
        }
        #endregion

        #region  畫面初始化
        /// <summary>
        /// 畫面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            Location = new Point(0, 0);
        }
        #endregion

        #region 設定按鈕觸發
        /// <summary>
        /// 設定按鈕觸發
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!FlyoutFlag)
            {
                accordionControl1.Enabled = false;
                FlyoutFlag = true;
                PanelControl panelControl = new PanelControl()
                {
                    Size = new Size(850, 572)
                };
                flyout = new FlyoutDialog(this, panelControl);
                flyout.Properties.Style = FlyoutStyle.Popup;
                SettingUserControl settingUserControl = new SettingUserControl(MysqlMethod, this);
                settingUserControl.Parent = panelControl;
                flyout.Show();
            }
        }
        #endregion

        #region 通訊錯誤泡泡視窗
        /// <summary>
        /// 通訊錯誤泡泡視窗
        /// </summary>
        public void ComponentFail()
        {
            if (ElectricAbsProtocols.Count > 0)
            {
                var absprotocol = ElectricAbsProtocols.Where(g => g.ConnectFlag == false).ToList();
                if (absprotocol.Count > 0)
                {
                    if (ErrorflyoutPanel == null)
                    {
                        ErrorflyoutPanel = new FlyoutPanel()
                        {
                            OwnerControl = this,
                            Size = new Size(1920, 68)
                        };
                        LabelControl label = new LabelControl() { Size = new Size(1920, 63) };
                        label.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                        label.Appearance.Font = new Font("微軟正黑體", 30);
                        label.Appearance.ForeColor = Color.White;
                        label.Appearance.BackColor = Color.Red;
                        label.AutoSizeMode = LabelAutoSizeMode.None;
                        label.Text = "通訊異常 !!";
                        ErrorflyoutPanel.Controls.Add(label);
                        ErrorflyoutPanel.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Bottom;
                        ErrorflyoutPanel.ShowPopup();
                    }
                    return;
                }
            }
            if (ErrorflyoutPanel != null)
            {
                ErrorflyoutPanel.HidePopup();
                ErrorflyoutPanel = null;
            }
        }
        #endregion
    }
}
