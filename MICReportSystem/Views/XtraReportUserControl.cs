using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraReports.UI;
using MICReportSystem.Configuration;
using MICReportSystem.Methods;
using System;
using System.IO;

namespace MICReportSystem.Views
{
    public partial class XtraReportUserControl : Field4UserControl
    {
        public XtraReportUserControl(MysqlMethod mysql)
        {
            InitializeComponent();
            StartdateEdit.Properties.ContextImageOptions.Image = imageCollection1.Images["calendar"];
            MysqlMethod = mysql;
        }
        /// <summary>
        /// 自動發送旗標
        /// </summary>
        public bool ExportFlag { get; set; }
        private void ShearsimpleButton_Click(object sender, EventArgs e)
        {
            XtraReportSetting = InitialMethod.InitialXtraReportLoad();
            ReportTitleSetting reportTitle = InitialMethod.InitialReportTitle();
            if (StartdateEdit.Text != "")
            {
                string ttime = string.Empty;
                if (XtraReportSetting.Day.ToString().Length == 2)
                {
                    ttime = Convert.ToDateTime(StartdateEdit.EditValue).ToString("yyyy/MM/") + $"{XtraReportSetting.Day} 00:00:00";
                }
                else
                {
                    ttime = Convert.ToDateTime(StartdateEdit.EditValue).ToString("yyyy/MM/") + $"0{XtraReportSetting.Day} 00:00:00";
                }
                XtraReport xtraReport = new XtraReport();
                AnalysisXtraReport analysisXtraReport = new AnalysisXtraReport();
                analysisXtraReport.create_XtraReport(MysqlMethod, ttime, reportTitle);
                analysisXtraReport.CreateDocument();
                xtraReport.Pages.AddRange(analysisXtraReport.Pages);
                documentViewer1.DocumentSource = xtraReport;
            }
            else
            {
                FlyoutAction action = new FlyoutAction();
                action.Caption = "電表資訊-查詢報表錯誤";
                action.Description = "請選擇條件再進行瀏覽";
                action.Commands.Add(FlyoutCommand.OK);
                FlyoutDialog.Show(FindForm(), action);
            }
        }
        /// <summary>
        /// 自動發送功能
        /// </summary>
        public override void TextChange()
        {
            XtraReportSetting = InitialMethod.InitialXtraReportLoad();
            ReportTitleSetting reportTitle = InitialMethod.InitialReportTitle();
            if (XtraReportSetting.AutoExport && DateTime.Now.Day == XtraReportSetting.Day)
            {
                if (!ExportFlag)
                {
                    string ttime = string.Empty;
                    if (XtraReportSetting.Day.ToString().Length == 2)
                    {
                        ttime = DateTime.Now.ToString("yyyy/MM/") + $"{XtraReportSetting.Day} 00:00:00";
                    }
                    else
                    {
                        ttime = DateTime.Now.ToString("yyyy/MM/") + $"0{XtraReportSetting.Day} 00:00:00";
                    }

                    AnalysisXtraReport analysisXtraReport = new AnalysisXtraReport();
                    analysisXtraReport.create_XtraReport(MysqlMethod, ttime, reportTitle);
                    analysisXtraReport.CreateDocument();
                    if (Directory.Exists($"{XtraReportSetting.Path}"))
                    {
                        string path = XtraReportSetting.Path + $"\\{DateTime.Now.ToString("yyyy-MM")}-產量紀錄表(一、二廠).docx";
                        analysisXtraReport.ExportToDocx(path);
                    }
                    ExportFlag = true;
                }
            }
            else
            {
                ExportFlag = false;
            }
        }
    }
}
