using MICReportSystem.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Text;

namespace MICReportSystem.Methods
{
    public class InitialMethod
    {
        /// <summary>
        /// 初始路徑
        /// </summary>
        private static string MyWorkPath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

        #region  MariaDB資訊Json 建檔與讀取
        /// <summary>
        /// MariaDB資訊Json 建檔與讀取
        /// </summary>
        /// <returns></returns>
        public static SystemSetting SystemLoad()
        {
            SystemSetting setting = null;
            if (!Directory.Exists($"{MyWorkPath}\\stf"))
                Directory.CreateDirectory($"{MyWorkPath}\\stf");
            string SettingPath = $"{MyWorkPath}\\stf\\System.json";
            try
            {
                if (File.Exists(SettingPath))
                {
                    string json = File.ReadAllText(SettingPath, Encoding.UTF8);
                    setting = JsonConvert.DeserializeObject<SystemSetting>(json);
                }
                else
                {
                    SystemSetting Setting = new SystemSetting()
                    {
                        DataSource = "127.0.0.1",
                        InitialCatalog = "MIC",
                        UserID = "root",
                        Password = "1234"
                    };
                    setting = Setting;
                    string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
                    File.WriteAllText(SettingPath, output);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, " MariaDB資訊設定載入錯誤");
            }
            return setting;
        }
        #endregion
        #region 按鈕Json 建檔與讀取
        /// <summary>
        /// 按鈕Json 建檔與讀取
        /// </summary>
        /// <returns></returns>
        public static ButtonSetting InitialButtonLoad()
        {
            ButtonSetting setting = null;
            if (!Directory.Exists($"{MyWorkPath}\\stf"))
                Directory.CreateDirectory($"{MyWorkPath}\\stf");
            string SettingPath = $"{MyWorkPath}\\stf\\button.json";
            try
            {
                if (File.Exists(SettingPath))
                {
                    string json = File.ReadAllText(SettingPath, Encoding.UTF8);
                    setting = JsonConvert.DeserializeObject<ButtonSetting>(json);
                }
                else
                {
                    ButtonSetting Setting = new ButtonSetting()
                    {
                        //群組與列表按鈕設定
                        ButtonGroupSettings =
                        {
                            new ButtonGroupSetting()
                            {
                                // 0 = 群組，1 = 列表
                                ButtonStyle = 1,
                                //群組名稱
                                GroupName = "群組名稱",
                                // 群組標註
                                GroupTag = 0,
                                //列表按鈕設定
                                ButtonItemSettings=
                                {
                                    new ButtonItemSetting()
                                    {
                                        //列表名稱
                                        ItemName = "列表名稱",
                                        //列表標註
                                        ItemTag = 0,
                                        //控制畫面顯示
                                        ControlVisible = true
                                    }
                                }
                            }
                        }
                    };
                    setting = Setting;
                    string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
                    File.WriteAllText(SettingPath, output);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "按鈕載入失敗");
            }
            return setting;
        }
        #endregion
        #region 報表匯出Json 建檔與讀取
        /// <summary>
        /// 報表匯出Json 建檔與讀取
        /// </summary>
        /// <returns></returns>
        public static XtraReportSetting InitialXtraReportLoad()
        {
            XtraReportSetting setting = null;
            if (!Directory.Exists($"{MyWorkPath}\\stf"))
                Directory.CreateDirectory($"{MyWorkPath}\\stf");
            string SettingPath = $"{MyWorkPath}\\stf\\XtraReport.json";
            try
            {
                if (File.Exists(SettingPath))
                {
                    string json = File.ReadAllText(SettingPath, Encoding.UTF8);
                    setting = JsonConvert.DeserializeObject<XtraReportSetting>(json);
                }
                else
                {
                    XtraReportSetting Setting = new XtraReportSetting()
                    {
                        AutoExport = false,
                        Path = "儲存路徑",
                        Day = 1
                    };
                    setting = Setting;
                    string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
                    File.WriteAllText(SettingPath, output);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "報表匯出載入失敗");
            }
            return setting;
        }

        public static void Save_XtraReportSetting(XtraReportSetting setting)
        {
            if (!Directory.Exists($"{MyWorkPath}\\stf"))
                Directory.CreateDirectory($"{MyWorkPath}\\stf");
            string SettingPath = $"{MyWorkPath}\\stf\\XtraReport.json";
            string output = JsonConvert.SerializeObject(setting, Formatting.Indented, new JsonSerializerSettings());
            File.WriteAllText(SettingPath, output);
        }
        #endregion
    }
}
