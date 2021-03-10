namespace MICReportSystem.Configuration
{
    public class XtraReportSetting
    {
        /// <summary>
        /// 自動匯出旗標
        /// </summary>
        public bool AutoExport { get; set; }
        /// <summary>
        /// 儲存路徑
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// Logo路徑
        /// </summary>
        public string LogoPath { get; set; }
        /// <summary>
        /// 自動匯出時間日期
        /// </summary>
        public int Day { get; set; }
    }
}
