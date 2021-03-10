namespace MICReportSystem.Mysql_Module
{
    /// <summary>
    /// 電表資訊
    /// </summary>
    public class ElectricConfig
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
        /// 設備ID
        /// </summary>
        public int DeviceID { get; set; }
        /// <summary>
        /// 電表類型
        /// </summary>
        public int ElectricTypeEnum { get; set; }
        /// <summary>
        /// 迴路: 0 = 第一迴路,1 = 第二迴路,2 = 第三迴路,3 = 第四迴路
        /// </summary>
        public int LoopTypeEnum { get; set; }
        /// <summary>
        /// 相位: 0 = 三相, 1 = 單相
        /// </summary>
        public int PhaseTypeEnum { get; set; }
        /// <summary>
        /// 相位角: 0 = R相,1 = S相,2 = T相
        /// </summary>
        public int PhaseAngleTypeEnum { get; set; }
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string DeviceName { get; set; }
    }
}
