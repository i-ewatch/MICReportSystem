using Dapper;
using MICReportSystem.Configuration;
using MICReportSystem.Mysql_Module;
using MICReportSystem.Protocols;
using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MICReportSystem.Methods
{
    public class MysqlMethod
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="setting">資料庫資訊</param>
        public MysqlMethod(SystemSetting setting)
        {
            if (setting != null)
            {
                scsb = new MySqlConnectionStringBuilder()
                {
                    Database = setting.InitialCatalog + "db",
                    Server = setting.DataSource,
                    UserID = setting.UserID,
                    Password = setting.Password,
                    CharacterSet = "utf8"
                };
                logscsb = new MySqlConnectionStringBuilder()
                {
                    Database = setting.InitialCatalog + "log",
                    Server = setting.DataSource,
                    UserID = setting.UserID,
                    Password = setting.Password,
                    CharacterSet = "utf8"
                };
            }
        }
        /// <summary>
        /// MICDB資料庫
        /// </summary>
        public MySqlConnectionStringBuilder scsb { get; set; }
        /// <summary>
        /// MICLOG資料庫
        /// </summary>
        public MySqlConnectionStringBuilder logscsb { get; set; }

        /// <summary>
        /// 查詢通道資訊
        /// </summary>
        /// <returns></returns>
        public List<GatewayConfig> Search_GatewayConfig()
        {
            List<GatewayConfig> configs = null;
            try
            {
                using (var conn = new MySqlConnection(scsb.ConnectionString))
                {
                    string sql = string.Empty;
                    sql = "SELECT * FROM GatewayConfig";
                    configs = conn.Query<GatewayConfig>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "通道資訊查詢失敗");
            }
            return configs;
        }
        /// <summary>
        /// 查詢電表資訊
        /// </summary>
        /// <param name="GatewayIndex"></param>
        /// <returns></returns>
        public List<ElectricConfig> Search_ElectricConfig(int GatewayIndex)
        {
            List<ElectricConfig> configs = null;
            try
            {
                using (var conn = new MySqlConnection(scsb.ConnectionString))
                {
                    string sql = string.Empty;
                    sql = "SELECT * FROM ElectricConfig WHERE GatewayIndex = @GatewayIndex";
                    configs = conn.Query<ElectricConfig>(sql, new { GatewayIndex }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "電表資訊查詢失敗");
            }
            return configs;
        }
        #region 三相電力資訊
        /// <summary>
        /// 查詢三相電力歷史資訊
        /// </summary>
        /// <param name="StartTime">起始時間</param>
        /// <param name="EndTime">結束時間</param>
        /// <param name="GatewayIndex">通道編號</param>
        /// <param name="DeviceIndex">設備編號</param>
        /// <returns></returns>
        public List<ThreePhaseElectricMeter_Log> Search_ThreePhaseElectricMeter_Log(string StartTime, string EndTime, int GatewayIndex, int DeviceIndex)
        {
            List<ThreePhaseElectricMeter_Log> logs = null;
            try
            {
                using (var conn = new MySqlConnection(logscsb.ConnectionString))
                {
                    string sql = string.Empty;
                    sql = "SELECT * FROM ThreePhaseElectricMeter_Log WHERE ttime >= @StartTime AND ttime <= @EndTime AND GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex";
                    logs = conn.Query<ThreePhaseElectricMeter_Log>(sql, new { StartTime, EndTime, GatewayIndex, DeviceIndex }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "三相電力查詢失敗");
            }
            return logs;
        }
        /// <summary>
        /// 查詢累積電量歷史資訊
        /// </summary>
        /// <param name="StartTime">起始時間</param>
        /// <param name="EndTime">結束時間</param>
        /// <param name="GatewayIndex">通道編號</param>
        /// <param name="DeviceIndex">設備編號</param>
        /// <returns></returns>
        public List<ElectricTotal> Search_ElectricTotal(string StartTime, string EndTime, int GatewayIndex, int DeviceIndex)
        {
            List<ElectricTotal> logs = null;
            try
            {
                using (var conn = new MySqlConnection(logscsb.ConnectionString))
                {
                    string sql = string.Empty;
                    sql = "SELECT * FROM ElectricTotal WHERE ttime >= @StartTime AND ttime <= @EndTime AND GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex";
                    logs = conn.Query<ElectricTotal>(sql, new { StartTime, EndTime, GatewayIndex, DeviceIndex }).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "累積電量歷史資訊查詢失敗");
            }
            return logs;
        }
        /// <summary>
        /// 查詢加總累積電量資訊
        /// </summary>
        /// <param name="StartTime">起始時間</param>
        /// <param name="EndTime">結束時間</param>
        /// <param name="GatewayIndex">通道編號</param>
        /// <param name="DeviceIndex">設備編號</param>
        /// <returns></returns>
        public decimal Search_ElectricSumTotal(string StartTime, string EndTime, int GatewayIndex, int DeviceIndex)
        {
            decimal logs = 0;
            try
            {
                using (var conn = new MySqlConnection(logscsb.ConnectionString))
                {
                    string sql = string.Empty;
                    sql = "SELECT SUM(KwhTotal) AS KwhTotal FROM ElectricTotal WHERE ttime >= @StartTime AND ttime <= @EndTime AND GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex";
                    logs = conn.QuerySingle<ElectricTotal>(sql, new { StartTime, EndTime, GatewayIndex, DeviceIndex }).KwhTotal;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "加總累積電量資訊查詢失敗");
            }
            return logs;
        }
        /// <summary>
        /// 新增三相電力歷史資訊
        /// </summary>
        /// <param name="log">三相電錶歷史資訊</param>
        /// <returns></returns>
        public bool Insert_ThreePhaseElectricMeter_Log(ThreePhaseElectricMeterData log)
        {
            DateTime dateTime = DateTime.Now;
            string ttime = dateTime.ToString("yyyyMMddHHmm00");
            DateTime ttimen = Convert.ToDateTime($"{dateTime:yyyy/MM/dd HH:mm:00}");
            bool InsertFlag = false;
            bool ProcedureFlag = false;
            try
            {
                using (var conn = new MySqlConnection(logscsb.ConnectionString))
                {
                    string InsertString = "INSERT IGNORE INTO ThreePhaseElectricMeter_Log (ttime,ttimen,GatewayIndex,DeviceIndex,rv,sv,tv,rsv,stv,trv,ra,sa,ta,kw,kwh,kvar,kvarh,pfe,kva,kvah,hz)VALUES (@ttime,@ttimen,@GatewayIndex,@DeviceIndex,@rv,@sv,@tv,@rsv,@stv,@trv,@ra,@sa,@ta,@kw,@kwh,@kvar,@kvarh,@pfe,@kva,@kvah,@hz)";
                    string Procedurestring = "ElectricdailykwhProcedure";
                    conn.Execute(InsertString, new { ttime, ttimen, log.GatewayIndex, log.DeviceIndex, log.rv, log.sv, log.tv, log.rsv, log.stv, log.trv, log.ra, log.sa, log.ta, log.kw, log.kwh, log.kvar, log.kvarh, log.pfe, log.kva, log.kvah, log.hz });
                    InsertFlag = true;
                    var affectedRows = conn.Execute(Procedurestring, new { nowTime = ttime, gatewayIndex1 = log.GatewayIndex, deviceIndex1 = log.DeviceIndex, nowKwh = log.kwh }, commandType: System.Data.CommandType.StoredProcedure);
                    if (affectedRows > 0)
                    {
                        ProcedureFlag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (!InsertFlag && !ProcedureFlag)
                {
                    Log.Error(ex, "三相電力存取失敗");
                }
                else if (InsertFlag && !ProcedureFlag)
                {
                    Log.Error(ex, "三相電力累積量存取失敗");
                }
                else if (!InsertFlag && ProcedureFlag)
                {
                    Log.Error(ex, "三相電力累積量存取失敗");
                }
            }
            if (InsertFlag && ProcedureFlag)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 報表設定功能
        /// <summary>
        /// 查詢報表設定
        /// </summary>
        /// <returns></returns>
        public List<ReportConfig> Search_ReportConfig()
        {
            List<ReportConfig> configs = null;
            try
            {
                using (var conn = new MySqlConnection(scsb.ConnectionString))
                {
                    string sql = string.Empty;
                    sql = "SELECT * FROM ReportConfig ";
                    configs = conn.Query<ReportConfig>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "報表設定查詢失敗");
            }
            return configs;
        }
        /// <summary>
        /// 更新報表設定
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool Update_ReportConfig(ReportConfig config)
        {
            bool Flag = false;
            try
            {
                using (var conn = new MySqlConnection(scsb.ConnectionString))
                {
                    string sql = string.Empty;
                    sql = "UPDATE  ReportConfig  SET " +
                        "ElectricNo = @ElectricNo, " +
                        "ElectricitySalePeriod = @ElectricitySalePeriod, " +
                        "StartingDate = @StartingDate, " +
                        "OfficialPricingStartDate = @OfficialPricingStartDate, " +
                        "PricStartTime = @PricStartTime, " +
                        "PricEndTime = @PricEndTime, " +
                        "DeviceCapacity = @DeviceCapacity, " +
                        "PurchaseAndSaleCapacity = @PurchaseAndSaleCapacity, " +
                        "ElectricityPurchaseRate = @ElectricityPurchaseRate," +
                        "Ratio = @Ratio " +
                        "WHERE PK = @PK AND GatewayIndex = @GatewayIndex AND DeviceIndex = @DeviceIndex";
                    conn.Execute(sql, new { config.ElectricNo, config.ElectricitySalePeriod, config.StartingDate, config.OfficialPricingStartDate, config.PricStartTime, config.PricEndTime, config.DeviceCapacity, config.PurchaseAndSaleCapacity,config.ElectricityPurchaseRate, config.Ratio, config.PK, config.GatewayIndex, config.DeviceIndex });
                    Flag = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "報表設定失敗");
            }
            return Flag;
        }
        #endregion
    }
}
