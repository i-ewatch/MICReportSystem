using System;

namespace MICReportSystem.Mysql_Module
{
    /// <summary>
    /// 三相電力資訊
    /// </summary>
    public class ThreePhaseElectricMeter_Log
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
        /// R線電壓
        /// </summary>
        public decimal rv { get; set; }
        /// <summary>
        /// S線電壓
        /// </summary>
        public decimal sv { get; set; }
        /// <summary>
        /// T線電壓
        /// </summary>
        public decimal tv { get; set; }
        /// <summary>
        /// R相電壓
        /// </summary>
        public decimal rsv { get; set; }
        /// <summary>
        /// S相電壓
        /// </summary>
        public decimal stv { get; set; }
        /// <summary>
        /// T相電壓
        /// </summary>
        public decimal trv { get; set; }
        /// <summary>
        /// R線電流
        /// </summary>
        public decimal ra { get; set; }
        /// <summary>
        /// S線電流
        /// </summary>
        public decimal sa { get; set; }
        /// <summary>
        /// T線電流
        /// </summary>
        public decimal ta { get; set; }
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
