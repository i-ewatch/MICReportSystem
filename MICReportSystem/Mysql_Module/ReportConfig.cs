using System;

namespace MICReportSystem.Mysql_Module
{
    public class ReportConfig
    {
        /// <summary>
        /// 流水編號
        /// </summary>
        public int PK { get; set; }
        /// <summary>
        /// 總表旗標
        /// </summary>
        public bool TotalMeterFlag { get; set; }
        /// <summary>
        /// 通道編號
        /// </summary>
        public int GatewayIndex { get; set; }
        /// <summary>
        /// 設備編號
        /// </summary>
        public int DeviceIndex { get; set; }
        /// <summary>
        /// 發電設備(機)組代號
        /// </summary>
        public string ElectricNo { get; set; }
        /// <summary>
        /// 售電期限
        /// </summary>
        public int ElectricitySalePeriod { get; set; }
        /// <summary>
        /// 計價起始日
        /// </summary>
        public DateTime StartingDate { get; set; }
        /// <summary>
        /// 購電費率
        /// </summary>
        public decimal ElectricityPurchaseRate { get; set; }
        /// <summary>
        /// 正式購售電能日
        /// </summary>
        public DateTime OfficialPricingStartDate { get; set; }
        /// <summary>
        /// 計價起始時間
        /// </summary>
        public DateTime PricStartTime { get; set; }
        /// <summary>
        /// 計價結束時間
        /// </summary>
        public DateTime PricEndTime { get; set; }
        /// <summary>
        /// 裝置容量
        /// </summary>
        public int DeviceCapacity { get; set; }
        /// <summary>
        /// 購售電容量
        /// </summary>
        public int PurchaseAndSaleCapacity { get; set; }
        /// <summary>
        /// 比值
        /// </summary>
        public int Ratio { get; set; }
    }
}
