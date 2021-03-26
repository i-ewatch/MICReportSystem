using MICReportSystem.Configuration;
using MICReportSystem.Methods;
using MICReportSystem.Mysql_Module;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MICReportSystem.Views
{
    public partial class SettingUserControl : Field4UserControl
    {
        /// <summary>
        /// 工作路徑
        /// </summary>
        private readonly string WorkPath = AppDomain.CurrentDomain.BaseDirectory;
        public SettingUserControl(MysqlMethod mysql, Form1 form1)
        {
            InitializeComponent();
            XtraReportSetting = InitialMethod.InitialXtraReportLoad();
            Form1 = form1;
            MysqlMethod = mysql;
            GatewayConfigs = MysqlMethod.Search_GatewayConfig();
            foreach (var Gatewayitem in GatewayConfigs)
            {
                var configs = MysqlMethod.Search_ElectricConfig(Gatewayitem.GatewayIndex);
                foreach (var Configitem in configs)
                {
                    ElectricConfigs.Add(Configitem);
                }
            }
            ReportTitleSetting reportTitle = InitialMethod.InitialReportTitle();
            ReportConfigs = MysqlMethod.Search_ReportConfig();
            ContractNoTextEdit.Text = reportTitle.ContractNo;
            ElectNoTextEdit.Text = reportTitle.ElectNo;
            AutotoggleSwitch.IsOn = XtraReportSetting.AutoExport;//自動匯出開關
            PathtextEdit.Text = XtraReportSetting.Path;//儲存路徑
            DaycomboBoxEdit.Text = XtraReportSetting.Day.ToString();//匯出時間

            int Index = 0;
            foreach (var ReportConfigitem in ReportConfigs)
            {
                var electricconfig = ElectricConfigs.Where(g => g.GatewayIndex == ReportConfigitem.GatewayIndex & g.DeviceIndex == ReportConfigitem.DeviceIndex).ToList()[0];
                ExportElectricSettingUserControl control = new ExportElectricSettingUserControl(ReportConfigitem, electricconfig, MysqlMethod) { Location = new Point(5 + 420 * Index, 5) };
                ReportviewpanelControl.Controls.Add(control);
                exportElectricSettingUserControls.Add(control);
                Index++;
            }

        }
        /// <summary>
        /// 主畫面繼承
        /// </summary>
        private Form1 Form1 { get; set; }
        /// <summary>
        /// 報表資訊
        /// </summary>
        private List<ReportConfig> ReportConfigs { get; set; } = new List<ReportConfig>();
        /// <summary>
        /// 報表資訊畫面
        /// </summary>
        private List<ExportElectricSettingUserControl> exportElectricSettingUserControls { get; set; } = new List<ExportElectricSettingUserControl>();
        /// <summary>
        /// 儲存路徑物件
        /// </summary>
        private FolderBrowserDialog SaveFileDialog = new FolderBrowserDialog();
        /// <summary>
        /// Logo路徑物件
        /// </summary>
        private OpenFileDialog LogoFileDialog = new OpenFileDialog();

        private void OKsimpleButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists($"{WorkPath}\\stf"))
                Directory.CreateDirectory($"{WorkPath}\\stf");
            string SettingPath = $"{WorkPath}\\stf\\reporttitle.json";
            ReportTitleSetting setting = new ReportTitleSetting()
            {
                ContractNo = ContractNoTextEdit.Text,
                ElectNo = ElectNoTextEdit.Text
            };
            string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
            File.WriteAllText(SettingPath, output);
            XtraReportSetting.AutoExport = AutotoggleSwitch.IsOn;
            XtraReportSetting.Path = PathtextEdit.Text;
            XtraReportSetting.Day = Convert.ToInt32(DaycomboBoxEdit.Text);
            InitialMethod.Save_XtraReportSetting(XtraReportSetting);
            foreach (var item in exportElectricSettingUserControls)
            {
                item.Inserter_ReportConfig();
            }
            Form1.accordionControl1.Enabled = true;
            Form1.FlyoutFlag = false;
            Form1.flyout.Close();
        }

        private void CancelsimpleButton_Click(object sender, EventArgs e)
        {
            Form1.accordionControl1.Enabled = true;
            Form1.FlyoutFlag = false;
            Form1.flyout.Close();
        }

        private void PathsimpleButton_Click(object sender, EventArgs e)
        {
            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (SaveFileDialog.Description != null)
                {
                    PathtextEdit.Text = Path.GetFullPath(SaveFileDialog.SelectedPath);
                }
            }
        }
    }
}
