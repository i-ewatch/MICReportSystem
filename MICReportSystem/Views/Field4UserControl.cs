using DevExpress.XtraEditors;
using MICReportSystem.Configuration;
using MICReportSystem.Methods;
using MICReportSystem.Mysql_Module;
using MICReportSystem.Protocols;
using System.Collections.Generic;

namespace MICReportSystem.Views
{
    public class Field4UserControl: XtraUserControl
    {
        /// <summary>
        /// 匯出報表JSON
        /// </summary>
        public XtraReportSetting XtraReportSetting { get; set; }

        /// <summary>
        /// MariaDB資料庫方法
        /// </summary>
        public MysqlMethod MysqlMethod { get; set; }
        /// <summary>
        /// 通訊數值
        /// </summary>
        public List<AbsProtocol> ElectricAbsProtocols { get; set; } = new List<AbsProtocol>();
        /// <summary>
        /// 總通道資訊
        /// </summary>
        public List<GatewayConfig> GatewayConfigs { get; set; } = new List<GatewayConfig>();
        /// <summary>
        /// 總電表資訊
        /// </summary>
        public List<ElectricConfig> ElectricConfigs { get; set; } = new List<ElectricConfig>();
        /// <summary>
        /// 累積量數值
        /// </summary>
        public List<ElectricTotal> ElectricTotals { get; set; } = new List<ElectricTotal>();
        /// <summary>
        /// 總子畫面物件
        /// </summary>
        public List<Field4UserControl> Field4UserControls { get; set; } = new List<Field4UserControl>();
        /// <summary>
        /// 電表歷史資料
        /// </summary>
        public List<ThreePhaseElectricMeter_Log> ThreePhaseElectricMeter_Logs { get; set; } = new List<ThreePhaseElectricMeter_Log>();
        /// <summary>
        /// 分電表資訊
        /// </summary>
        public ElectricConfig ElectricConfig { get; set; }
        /// <summary>
        /// 顯示變更
        /// </summary>
        public virtual void TextChange() { }
    }
}
