namespace MICReportSystem.Enums
{
    /// <summary>
    /// 電表報表查詢類型
    /// </summary>
    public enum ElectricSearchTypeEnum
    {
        /// <summary>
        /// 電壓
        /// </summary>
        Voltage,
        /// <summary>
        /// 電流
        /// </summary>
        Current,
        /// <summary>
        /// 瞬間功率
        /// </summary>
        kW,
        /// <summary>
        /// 瞬間虛功率
        /// </summary>
        kVAR,
        /// <summary>
        /// 功率因數
        /// </summary>
        PF,
        /// <summary>
        /// 頻率
        /// </summary>
        HZ,
        /// <summary>
        /// 累積電量
        /// </summary>
        kWh
    }
}
