using MathLibrary;
using MICReportSystem.Methods;
using NModbus;

namespace MICReportSystem.Protocols
{
    public abstract class AbsProtocol
    {
        /// <summary>
        /// 數學公式
        /// </summary>
        public MathClass MathClass { get; set; } = new MathClass();
        /// <summary>
        /// Mysql方法
        /// </summary>
        public MysqlMethod MysqlMethod { get; set; }
        /// <summary>
        /// 連線旗標
        /// </summary>
        public bool ConnectFlag { get; set; }
        /// <summary>
        /// 設備ID
        /// </summary>
        public byte ID { get; set; }
        /// <summary>
        /// 通道編號
        /// </summary>
        public int GatewayIndex { get; set; }
        /// <summary>
        /// 設備編號
        /// </summary>
        public int DeviceIndex { get; set; }
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
        /// 讀取功能
        /// </summary>
        /// <param name="master">通道資訊</param>
        public virtual void ReadData(IModbusMaster master) {  }
    }
}
