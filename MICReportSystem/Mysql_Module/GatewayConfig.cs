namespace MICReportSystem.Mysql_Module
{
    /// <summary>
    /// 通道資訊
    /// </summary>
    public class GatewayConfig
    {
        /// <summary>
        /// 流水編號
        /// </summary>
        public int PK { get; set; }
        /// <summary>
        /// 通道編號
        /// </summary>
        public int GatewayIndex { get; set; }
        /// <summary>
        /// 通訊類型 0 = Modbus RTU, 1 = Modbus TCP
        /// </summary>
        public int GatewayTypeEnum { get; set; }
        /// <summary>
        /// 通訊: COM 或 IP
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 通道: 9600,8,N,1 或 502
        /// </summary>
        public string Rate { get; set; } 
        /// <summary>
        /// 通道名稱
        /// </summary>
        public string GatewayName { get; set; }
    }
}
