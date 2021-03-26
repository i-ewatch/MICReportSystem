using DevExpress.XtraReports.UI;
using MICReportSystem.Configuration;
using MICReportSystem.Methods;
using MICReportSystem.Mysql_Module;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace MICReportSystem.Views
{
    public partial class AnalysisXtraReport : DevExpress.XtraReports.UI.XtraReport
    {
        public AnalysisXtraReport()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 資料庫方法
        /// </summary>
        private MysqlMethod MysqlMethod { get; set; }
        /// <summary>
        /// 報表自動匯出資訊
        /// </summary>
        private XtraReportSetting XtraReportSetting { get; set; }
        /// <summary>
        /// 通道基本資訊
        /// </summary>
        private List<GatewayConfig> GatewayConfigs { get; set; } = new List<GatewayConfig>();
        /// <summary>
        /// 電表基本資訊
        /// </summary>
        private List<ElectricConfig> ElectricConfigs { get; set; } = new List<ElectricConfig>();
        /// <summary>
        /// 報表資訊
        /// </summary>
        private List<ReportConfig> ReportConfigs { get; set; } = new List<ReportConfig>();
        /// <summary>
        /// 總累積量數值
        /// </summary>
        private List<decimal> TotalkWh { get; set; } = new List<decimal>();
        public void create_XtraReport(MysqlMethod mysql, string TTime, ReportTitleSetting reportTitle)
        {
            MysqlMethod = mysql;
            ContractNoLabel.Text = reportTitle.ContractNo;          //契約編號
            ElectNoLabel.Text = reportTitle.ElectNo;                //電號
            var TaiwanDate = new System.Globalization.TaiwanCalendar();//民國轉換
            XtraReportSetting = InitialMethod.InitialXtraReportLoad();
            DateTime dateTime = Convert.ToDateTime(TTime);
            DateTime nowtime = DateTime.Now;
            ReportConfigs = MysqlMethod.Search_ReportConfig();//報表資訊
            GatewayConfigs = MysqlMethod.Search_GatewayConfig();
            foreach (var item in GatewayConfigs)
            {
                var configs = MysqlMethod.Search_ElectricConfig(item.GatewayIndex);
                ElectricConfigs.AddRange(configs);
            }
            string startime = startime = dateTime.AddMonths(-1).ToString("yyyyMMdd");
            string endtime = dateTime.AddDays(-1).ToString("yyyyMMdd");

            foreach (var item in ElectricConfigs)
            {
                var data = MysqlMethod.Search_ElectricSumTotal(startime, endtime, item.GatewayIndex, item.DeviceIndex);
                TotalkWh.Add(data);
            }

            var SumTotalkWh = TotalkWh[0] + TotalkWh[1];
            StartDatexrLabel1.Text = $"{TaiwanDate.GetYear(dateTime.AddMonths(-1))}";
            StartDatexrLabel2.Text = $"{dateTime.AddMonths(-1).ToString("MM")}";
            if (XtraReportSetting.Day.ToString().Length == 2)
            {
                StartDatexrLabel3.Text = $"{XtraReportSetting.Day}";
                //StartDatexrLabel1.Text = $"{TaiwanDate.GetYear(dateTime.AddMonths(-1))}年{dateTime.AddMonths(-1).ToString("MM")}月{ XtraReportSetting.Day}日";
            }
            else
            {
                StartDatexrLabel3.Text = $"0{XtraReportSetting.Day}";
                //StartDatexrLabel1.Text = $"{TaiwanDate.GetYear(dateTime.AddMonths(-1))}年{dateTime.AddMonths(-1).ToString("MM")}月0{ XtraReportSetting.Day}日";
            }

            EndDatexrLabel1.Text = $"{TaiwanDate.GetYear(dateTime.AddDays(-1))}";
            EndDatexrLabel2.Text = $"{dateTime.AddDays(-1).ToString("MM")}";
            EndDatexrLabel3.Text = $"{dateTime.AddDays(-1).ToString("dd")}";
            //EndDatexrLabel1.Text = $"{TaiwanDate.GetYear(dateTime.AddDays(-1))}年{dateTime.AddDays(-1).ToString("MM月dd日")}";
            CurrentMeterReadingDayxrLabel1.Text = $"{TaiwanDate.GetYear(nowtime)}";
            CurrentMeterReadingDayxrLabel2.Text = $"{nowtime.ToString("MM")}";
            CurrentMeterReadingDayxrLabel3.Text = $"{nowtime.ToString("dd")}";
            //CurrentMeterReadingDayxrLabel1.Text = $"{TaiwanDate.GetYear(nowtime)}年{nowtime.ToString("MM")}月{nowtime.ToString("dd")}日";//本期抄表日
            DateOfReportingxrLabel1.Text = $"{TaiwanDate.GetYear(nowtime)}";
            DateOfReportingxrLabel2.Text = $"{nowtime.ToString("MM")}";
            DateOfReportingxrLabel3.Text = $"{nowtime.ToString("dd")}";
            //DateOfReportingxrLabel1.Text = $"{TaiwanDate.GetYear(nowtime)}年{nowtime.ToString("MM")}月{nowtime.ToString("dd")}日";//填報日期
            if (ReportConfigs.Count > 0)
            {
                #region 第一顆電表
                xrTableCell7.Text = ReportConfigs[0].ElectricNo;
                xrTableCell12.Text = ReportConfigs[0].ElectricitySalePeriod.ToString();//售電期限
                xrTableCell17.Text = $"{TaiwanDate.GetYear(ReportConfigs[0].StartingDate)}.{ReportConfigs[0].StartingDate.Month}.{ReportConfigs[0].StartingDate.Day}";//計價起始日
                xrTableCell22.Text = $"{TaiwanDate.GetYear(ReportConfigs[0].OfficialPricingStartDate)}.{ReportConfigs[0].OfficialPricingStartDate.Month}.{ReportConfigs[0].OfficialPricingStartDate.Day}";//正是購售電能日
                xrTableCell52.Text = $"{TaiwanDate.GetYear(ReportConfigs[0].PricStartTime)}.{ReportConfigs[0].PricStartTime.Month}.{ReportConfigs[0].PricStartTime.Day}-{TaiwanDate.GetYear(ReportConfigs[0].PricEndTime)}.{ReportConfigs[0].PricEndTime.Month}.{ReportConfigs[0].PricEndTime.Day}";//計價起迄期間
                xrTableCell27.Text = ReportConfigs[0].ElectricityPurchaseRate.ToString("0.####");//購電費率
                xrTableCell32.Text = ReportConfigs[0].DeviceCapacity.ToString("0.###");//裝置容量
                xrTableCell37.Text = ReportConfigs[0].PurchaseAndSaleCapacity.ToString("0.###");//購售電容量
                xrTableCell42.Text = TotalkWh[0].ToString("0.##");//生產電度量
                if (TotalkWh[0] != 0)
                {
                    xrTableCell47.Text = $"{Convert.ToInt32((TotalkWh[0] / SumTotalkWh) * 100)}%";
                }
                #endregion

                #region 第二顆電表
                xrTableCell8.Text = ReportConfigs[1].ElectricNo;
                xrTableCell13.Text = ReportConfigs[1].ElectricitySalePeriod.ToString();//售電期限
                xrTableCell18.Text = $"{TaiwanDate.GetYear(ReportConfigs[1].StartingDate)}.{ReportConfigs[1].StartingDate.Month}.{ReportConfigs[1].StartingDate.Day}";//計價起始日
                xrTableCell23.Text = $"{TaiwanDate.GetYear(ReportConfigs[1].OfficialPricingStartDate)}.{ReportConfigs[1].OfficialPricingStartDate.Month}.{ReportConfigs[1].OfficialPricingStartDate.Day}";//正是購售電能日
                xrTableCell53.Text = $"{TaiwanDate.GetYear(ReportConfigs[1].PricStartTime)}.{ReportConfigs[1].PricStartTime.Month}.{ReportConfigs[1].PricStartTime.Day}-{TaiwanDate.GetYear(ReportConfigs[1].PricEndTime)}.{ReportConfigs[1].PricEndTime.Month}.{ReportConfigs[1].PricEndTime.Day}";//計價起迄期間
                xrTableCell28.Text = ReportConfigs[1].ElectricityPurchaseRate.ToString("#.####");//購電費率
                xrTableCell33.Text = ReportConfigs[1].DeviceCapacity.ToString("#.###");//裝置容量
                xrTableCell38.Text = ReportConfigs[1].PurchaseAndSaleCapacity.ToString("#.###");//購售電容量
                xrTableCell43.Text = TotalkWh[1].ToString("#.##");//生產電度量
                if (TotalkWh[1] != 0)
                {
                    xrTableCell48.Text = $"{Convert.ToInt32((TotalkWh[1] / SumTotalkWh) * 100)}%";
                }
                #endregion

                #region 合計
                xrTableCell35.Text = $"{(ReportConfigs[0].DeviceCapacity + ReportConfigs[1].DeviceCapacity).ToString("#.###")}";//裝置容量
                xrTableCell40.Text = $"{ (ReportConfigs[0].PurchaseAndSaleCapacity + ReportConfigs[1].PurchaseAndSaleCapacity).ToString("#.###")}";//購售電容量
                xrTableCell45.Text = $"{SumTotalkWh.ToString("#.##")}";
                #endregion
            }
        }
    }
}
