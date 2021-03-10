using MICReportSystem.Methods;
using MICReportSystem.Protocols;
using NModbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace MICReportSystem.Components
{
    public class Field4Component : Component
    {
        /// <summary>
        /// 總通訊物件
        /// </summary>
        public List<AbsProtocol> ElectricAbsProtocols { get; set; } = new List<AbsProtocol>();
        /// <summary>
        /// 資料庫方法
        /// </summary>
        public MysqlMethod MysqlMethod { get; set; }
        /// <summary>
        /// 通訊建置類別(通用)
        /// </summary>
        public ModbusFactory Factory = new ModbusFactory();
        /// <summary>
        /// 通訊物件(通用)
        /// </summary>
        public IModbusMaster master;
        #region 初始功能
        /// <summary>
        /// 執行緒
        /// </summary>
        public Thread ComponentThread { get; set; }
        /// <summary>
        /// 時間
        /// </summary>
        public DateTime ComponentTime { get; set; }
        public Field4Component()
        {
            OnMyWorkStateChanged += new MyWorkStateChanged(AfterMyWorkStateChanged);
        }
        protected void WhenMyWorkStateChange()
        {
            OnMyWorkStateChanged?.Invoke(this, null);
        }
        public delegate void MyWorkStateChanged(object sender, EventArgs e);
        public event MyWorkStateChanged OnMyWorkStateChanged;
        /// <summary>
        /// 系統工作路徑
        /// </summary>
        protected readonly string WorkPath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 通訊功能啟動判斷旗標
        /// </summary>
        protected bool myWorkState;
        /// <summary>
        /// 通訊功能啟動旗標
        /// </summary>
        public bool MyWorkState
        {
            get { return myWorkState; }
            set
            {
                if (value != myWorkState)
                {
                    myWorkState = value;
                    WhenMyWorkStateChange();
                }
            }
        }
        /// <summary>
        /// 執行續工作狀態改變觸發事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void AfterMyWorkStateChanged(object sender, EventArgs e) { }
        #endregion
    }
}
