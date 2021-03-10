using System;

namespace MICReportSystem.Mysql_Module
{
    /// <summary>
    /// 單相電力資訊
    /// </summary>
    public class SinglePhaseElectricMeter_Log
    {
        /// <summary>
        /// 時間字串
        /// </summary>
        public string ttime { get; set; }
        /// <summary>
        /// 時間類型
        /// </summary>
        public DateTime ttimen { get; set; }
        /// <summary>
        /// 通道編號
        /// </summary>
        public int GatewayIndex { get; set; }
        /// <summary>
        /// 設備編號
        /// </summary>
        public int DeviceIndex { get; set; }
        /// <summary>
        /// 電壓
        /// </summary>
        public decimal v { get; set; }
        /// <summary>
        /// 電流
        /// </summary>
        public decimal ra { get; set; }
        /// <summary>
        /// 即時功率
        /// </summary>
        public decimal kw { get; set; }
        /// <summary>
        /// 累積功率
        /// </summary>
        public decimal kwh { get; set; }
        /// <summary>
        /// 即時虛功率
        /// </summary>
        public decimal kvar { get; set; }
        /// <summary>
        /// 累積虛功率
        /// </summary>
        public decimal kvarh { get; set; }
        /// <summary>
        /// 功率因數
        /// </summary>
        public decimal pfe { get; set; }
        /// <summary>
        /// 即時視在功率
        /// </summary>
        public decimal kva { get; set; }
        /// <summary>
        /// 累績視在功率
        /// </summary>
        public decimal kvah { get; set; }
        /// <summary>
        /// 頻率
        /// </summary>
        public decimal hz { get; set; }
    }
}
