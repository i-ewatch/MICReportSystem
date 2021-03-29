using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using MICReportSystem.Configuration;
using MICReportSystem.Enums;
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
            ReportTitleSetting reportTitle = InitialMethod.InitialReportTitleSetting();
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
            ReportTitleSetting reportTitle = InitialMethod.InitialReportTitleSetting();
            FileFormatSetting fileFormat = InitialMethod.InitialFileFormatSetting();
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
                        FileFormatTypeEnum fileFormatType = (FileFormatTypeEnum)fileFormat.FileFormat;
                        switch (fileFormatType)
                        {
                            case FileFormatTypeEnum.PDF:
                                {
                                    string path = XtraReportSetting.Path + $"\\Production Report(F1&F2)_{DateTime.Now.ToString("yyyyMMdd")}.pdf";
                                    analysisXtraReport.ExportToPdf(path);
                                    break;
                                }
                            case FileFormatTypeEnum.DOCX:
                                {
                                    string path = XtraReportSetting.Path + $"\\Production Report(F1&F2)_{DateTime.Now.ToString("yyyyMMdd")}.docx";
                                    DocxExportOptions options = new DocxExportOptions()
                                    {
                                        TableLayout = true
                                    };
                                    analysisXtraReport.ExportToDocx(path, options);
                                    break;
                                }
                            case FileFormatTypeEnum.XLSX:
                                {
                                    string path = XtraReportSetting.Path + $"\\Production Report(F1&F2)_{DateTime.Now.ToString("yyyyMMdd")}.xlsx";
                                    analysisXtraReport.ExportToXlsx(path);
                                    break;
                                }
                        }
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
