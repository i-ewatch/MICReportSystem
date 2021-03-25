using MICReportSystem.Methods;
using MICReportSystem.Mysql_Module;
using System;

namespace MICReportSystem.Views
{
    public partial class ExportElectricSettingUserControl : DevExpress.XtraEditors.XtraUserControl
    {
        public ExportElectricSettingUserControl(ReportConfig reportConfig,ElectricConfig electricConfig,MysqlMethod mysql)
        {
            InitializeComponent();
            MysqlMethod = mysql;
            ReportConfig = reportConfig;
            groupControl1.Text = electricConfig.DeviceName;
            ElectricNotextEdit.Text = ReportConfig.ElectricNo;
            ElectricitySalePeriodtextEdit.Text = ReportConfig.ElectricitySalePeriod.ToString();
            StartingDatetimeEdit.EditValue = ReportConfig.StartingDate.ToString("yyyy/MM/dd");
            OfficialPricingStartDatetimeEdit.EditValue = ReportConfig.OfficialPricingStartDate.ToString("yyyy/MM/dd");
            PricStartTimetimeEdit.EditValue = ReportConfig.PricStartTime.ToString("yyyy/MM/dd");
            PricEndTimetimeEdit.EditValue = ReportConfig.PricEndTime.ToString("yyyy/MM/dd");
            ElectricityPurchaseRatetextEdit.Text = ReportConfig.ElectricityPurchaseRate.ToString("#.####");
            DeviceCapacitytextEdit.Text = ReportConfig.DeviceCapacity.ToString("#.###");
            PurchaseAndSaleCapacitytextEdit.Text = ReportConfig.PurchaseAndSaleCapacity.ToString("#.###");
            RatetextEdit.Text = ReportConfig.Ratio.ToString();
        }
        public ReportConfig ReportConfig { get; set; }
        public MysqlMethod MysqlMethod { get; set; }

        public void Inserter_ReportConfig()
        {
            ReportConfig.ElectricNo = ElectricNotextEdit.Text;
            ReportConfig.ElectricitySalePeriod =Convert.ToInt32(ElectricitySalePeriodtextEdit.Text);
            ReportConfig.StartingDate = Convert.ToDateTime(StartingDatetimeEdit.EditValue);
            ReportConfig.OfficialPricingStartDate = Convert.ToDateTime(OfficialPricingStartDatetimeEdit.EditValue);
            ReportConfig.PricStartTime = Convert.ToDateTime(PricStartTimetimeEdit.EditValue);
            ReportConfig.PricEndTime = Convert.ToDateTime(PricEndTimetimeEdit.EditValue);
            ReportConfig.ElectricityPurchaseRate = Convert.ToDecimal(ElectricityPurchaseRatetextEdit.Text);
            ReportConfig.DeviceCapacity = Convert.ToDecimal(DeviceCapacitytextEdit.Text);
            ReportConfig.PurchaseAndSaleCapacity = Convert.ToDecimal(PurchaseAndSaleCapacitytextEdit.Text);
            ReportConfig.Ratio = Convert.ToInt32(RatetextEdit.Text);
            MysqlMethod.Update_ReportConfig(ReportConfig);

        }
    }
}
