using System;

namespace MICReportSystem.Mysql_Module
{
    /// <summary>
    /// 累積量資訊
    /// </summary>
    public class ElectricTotal
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
        /// 起始累積功率1
        /// </summary>
        public decimal KwhStart1 { get; set; }
        /// <summary>
        /// 結束累積功率1
        /// </summary>
        public decimal KwhEnd1 { get; set; }
        /// <summary>
        /// 起始累積功率2
        /// </summary>
        public decimal KwhStart2 { get; set; }
        /// <summary>
        /// 結束累積功率2
        /// </summary>
        public decimal KwhEnd2 { get; set; }
        /// <summary>
        /// 當日累積量
        /// </summary>
        public decimal KwhTotal { get; set; }
    }
}
