using DevExpress.Utils;
using DevExpress.Utils.Win;
using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using MICReportSystem.Enums;
using MICReportSystem.Methods;
using MICReportSystem.Mysql_Module;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MICReportSystem.Views
{
    public partial class ChartUserControl : Field4UserControl
    {
        /// <summary>
        /// 限制查詢設備數量
        /// </summary>
        private int maxCheckedItemNumber = 2;
        public ChartUserControl(MysqlMethod mysql)
        {
            InitializeComponent();
            StartdateEdit.Properties.ContextImageOptions.Image = imageCollection1.Images["calendar"];
            EnddateEdit.Properties.ContextImageOptions.Image = imageCollection1.Images["calendar"];
            MysqlMethod = mysql;
            GatewayConfigs = MysqlMethod.Search_GatewayConfig();
            foreach (var Gatewayitem in GatewayConfigs)
            {
                var configs = MysqlMethod.Search_ElectricConfig(Gatewayitem.GatewayIndex);
                foreach (var Configitem in configs)
                {
                    ElectricConfigs.Add(Configitem);
                }
            }
            foreach (var ElectricConfigitem in ElectricConfigs)
            {
                CheckedListBoxItem item = new CheckedListBoxItem(ElectricConfigitem.DeviceName, false);
                item.Tag = ElectricConfigitem;
                DeviceCheckedcomboBoxEdit.Properties.Items.Add(item);
            }
            DeviceCheckedcomboBoxEdit.Popup += (s, e) =>
            {
                var f = (s as IPopupControl).PopupWindow as CheckedPopupContainerForm;
                var listBox = f.ActiveControl as CheckedListBoxControl;
                if (listBox != null)
                {
                    listBox.ItemChecking += listBox_ItemChecking;
                }
            };
            DeviceCheckedcomboBoxEdit.CloseUp += (s, e) =>
            {
                var f = (s as IPopupControl).PopupWindow as CheckedPopupContainerForm;
                var listBox = f.ActiveControl as CheckedListBoxControl;
                if (listBox != null)
                {
                    listBox.ItemChecking -= listBox_ItemChecking;
                }
            };
            DeviceCheckedcomboBoxEdit.Properties.SelectAllItemVisible = false;
        }
        /// <summary>
        /// 最大勾選數量判斷
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_ItemChecking(object sender, ItemCheckingEventArgs e)
        {
            var editor = sender as CheckedListBoxControl;
            if (e.NewValue == CheckState.Unchecked)
            {
                return;
            }
            e.Cancel = editor.CheckedItemsCount >= maxCheckedItemNumber ? true : false;
        }
        /// <summary>
        /// 查詢按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShearsimpleButton_Click(object sender, EventArgs e)
        {
            if (DeviceCheckedcomboBoxEdit.Text != "" && ValuecomboBoxEdit.Text != "" && StartdateEdit.Text != "" && EnddateEdit.Text != null && Convert.ToDateTime(StartdateEdit.EditValue) <= Convert.ToDateTime(EnddateEdit.EditValue))
            {
                if (LinechartControl.Series.Count >= 0)
                {
                    LinechartControl.Series.Clear();
                }
                if (gridView1.Columns.Count > 0)
                {
                    gridView1.Columns.Clear();
                }
                string starttime = ((DateTime)StartdateEdit.EditValue).ToString("yyyyMMdd000000");//起始時間
                string endtime = ((DateTime)EnddateEdit.EditValue).ToString("yyyyMMdd235959");//結束時間
                List<Series> Lineseries = new List<Series>();//曲線圖用
                ThreePhaseElectricMeter_Logs = new List<ThreePhaseElectricMeter_Log>(); //報表用
                ElectricSearchTypeEnum electricSearchTypeEnum = (ElectricSearchTypeEnum)ValuecomboBoxEdit.SelectedIndex;
                for (int i = 0; i < DeviceCheckedcomboBoxEdit.Properties.Items.Count; i++)
                {
                    if (DeviceCheckedcomboBoxEdit.Properties.Items[i].CheckState == CheckState.Checked)
                    {
                        ElectricConfig = (ElectricConfig)DeviceCheckedcomboBoxEdit.Properties.Items[i].Tag;

                        var data = MysqlMethod.Search_ThreePhaseElectricMeter_Log(starttime, endtime, ElectricConfig.GatewayIndex, ElectricConfig.DeviceIndex);
                        #region 報表資料整理
                        ThreePhaseElectricMeter_Logs.AddRange(data);
                        #endregion
                        #region 曲線圖資料整理
                        switch (electricSearchTypeEnum)
                        {
                            case ElectricSearchTypeEnum.Voltage:
                                {
                                    Series series1 = new Series($"{ElectricConfig.DeviceName} - R相電壓", viewType: ViewType.Line);
                                    series1.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd HH:mm:ss} " + "\n" + "數值:{V:0.00}V";
                                    series1.LegendTextPattern = "{A}";
                                    series1.ArgumentDataMember = "ttimen";
                                    series1.LabelsVisibility = DefaultBoolean.False;
                                    series1.ValueDataMembers.AddRange(new string[] { "rsv" });
                                    Lineseries.Add(series1);
                                    Series series2 = new Series($"{ElectricConfig.DeviceName} - S相電壓", viewType: ViewType.Line);
                                    series2.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd HH:mm:ss} " + "\n" + "數值:{V:0.00}V";
                                    series2.LegendTextPattern = "{A}";
                                    series2.ArgumentDataMember = "ttimen";
                                    series2.LabelsVisibility = DefaultBoolean.False;
                                    series2.ValueDataMembers.AddRange(new string[] { "stv" });
                                    Lineseries.Add(series2);
                                    Series series3 = new Series($"{ElectricConfig.DeviceName} - T相電壓", viewType: ViewType.Line);
                                    series3.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd HH:mm:ss} " + "\n" + "數值:{V:0.00}V";
                                    series3.LegendTextPattern = "{A}";
                                    series3.ArgumentDataMember = "ttimen";
                                    series3.LabelsVisibility = DefaultBoolean.False;
                                    series3.ValueDataMembers.AddRange(new string[] { "trv" });
                                    Lineseries.Add(series3);
                                }
                                break;
                            case ElectricSearchTypeEnum.Current:
                                {
                                    Series series1 = new Series($"{ElectricConfig.DeviceName}- R相電流", viewType: ViewType.Line);
                                    series1.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd HH:mm:ss} " + "\n" + "數值:{V:0.00}A";
                                    series1.LegendTextPattern = "{A}";
                                    series1.ArgumentDataMember = "ttimen";
                                    series1.LabelsVisibility = DefaultBoolean.False;
                                    series1.ValueDataMembers.AddRange(new string[] { "ra" });
                                    Lineseries.Add(series1);
                                    Series series2 = new Series($"{ElectricConfig.DeviceName} - S相電流", viewType: ViewType.Line);
                                    series2.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd HH:mm:ss} " + "\n" + "數值:{V:0.00}A";
                                    series2.LegendTextPattern = "{A}";
                                    series2.ArgumentDataMember = "ttimen";
                                    series2.LabelsVisibility = DefaultBoolean.False;
                                    series2.ValueDataMembers.AddRange(new string[] { "sa" });
                                    Lineseries.Add(series2);
                                    Series series3 = new Series($"{ElectricConfig.DeviceName} - T相電流", viewType: ViewType.Line);
                                    series3.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd HH:mm:ss} " + "\n" + "數值:{V:0.00}A";
                                    series3.LegendTextPattern = "{A}";
                                    series3.ArgumentDataMember = "ttimen";
                                    series3.LabelsVisibility = DefaultBoolean.False;
                                    series3.ValueDataMembers.AddRange(new string[] { "ta" });
                                    Lineseries.Add(series3);
                                }
                                break;
                            case ElectricSearchTypeEnum.kW:
                                {
                                    Series series1 = new Series($"{ElectricConfig.DeviceName} - 瞬間功率", viewType: ViewType.Line);
                                    series1.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd HH:mm:ss} " + "\n" + "數值:{V:0.00}kW";
                                    series1.LegendTextPattern = "{A}";
                                    series1.ArgumentDataMember = "ttimen";
                                    series1.LabelsVisibility = DefaultBoolean.False;
                                    series1.ValueDataMembers.AddRange(new string[] { "kw" });
                                    Lineseries.Add(series1);
                                }
                                break;
                            case ElectricSearchTypeEnum.kVAR:
                                {
                                    Series series1 = new Series($"{ElectricConfig.DeviceName} - 瞬間虛功率", viewType: ViewType.Line);
                                    series1.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd HH:mm:ss} " + "\n" + "數值:{V:0.00}kVAR";
                                    series1.LegendTextPattern = "{A}";
                                    series1.ArgumentDataMember = "ttimen";
                                    series1.LabelsVisibility = DefaultBoolean.False;
                                    series1.ValueDataMembers.AddRange(new string[] { "kvar" });
                                    Lineseries.Add(series1);
                                }
                                break;
                            case ElectricSearchTypeEnum.PF:
                                {
                                    Series series1 = new Series($"{ElectricConfig.DeviceName} - 功率因數", viewType: ViewType.Line);
                                    series1.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd HH:mm:ss} " + "\n" + "數值:{V:0.00}";
                                    series1.LegendTextPattern = "{A}";
                                    series1.ArgumentDataMember = "ttimen";
                                    series1.LabelsVisibility = DefaultBoolean.False;
                                    series1.ValueDataMembers.AddRange(new string[] { "pfe" });
                                    Lineseries.Add(series1);
                                }
                                break;
                            case ElectricSearchTypeEnum.HZ:
                                {
                                    Series series1 = new Series($"{ElectricConfig.DeviceName} - 頻率", viewType: ViewType.Line);
                                    series1.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd HH:mm:ss} " + "\n" + "數值:{V:0.00}HZ";
                                    series1.LegendTextPattern = "{A}";
                                    series1.ArgumentDataMember = "ttimen";
                                    series1.LabelsVisibility = DefaultBoolean.False;
                                    series1.ValueDataMembers.AddRange(new string[] { "hz" });
                                    Lineseries.Add(series1);
                                }
                                break;
                        }
                        #endregion
                    }
                }
                gridControl1.DataSource = ThreePhaseElectricMeter_Logs;
                LinechartControl.DataSource = ThreePhaseElectricMeter_Logs;
                #region 報表
                for (int i = 0; i < gridView1.Columns.Count; i++)
                {
                    gridView1.Columns[i].Visible = false;
                }
                switch (electricSearchTypeEnum)
                {
                    case ElectricSearchTypeEnum.Voltage:
                        {
                            gridView1.Columns["trv"].Visible = true;
                            gridView1.Columns["trv"].Caption = "T相電壓";
                            gridView1.Columns["stv"].Visible = true;
                            gridView1.Columns["stv"].Caption = "S相電壓";
                            gridView1.Columns["rsv"].Visible = true;
                            gridView1.Columns["rsv"].Caption = "R相電壓";
                        }
                        break;
                    case ElectricSearchTypeEnum.Current:
                        {
                            gridView1.Columns["ta"].Visible = true;
                            gridView1.Columns["ta"].Caption = "T相電流";
                            gridView1.Columns["sa"].Visible = true;
                            gridView1.Columns["sa"].Caption = "S相電流";
                            gridView1.Columns["ra"].Visible = true;
                            gridView1.Columns["ra"].Caption = "R相電流";
                        }
                        break;
                    case ElectricSearchTypeEnum.kW:
                        {
                            gridView1.Columns["kw"].Visible = true;
                            gridView1.Columns["kw"].Caption = "瞬間功率";
                        }
                        break;
                    case ElectricSearchTypeEnum.kVAR:
                        {
                            gridView1.Columns["kvar"].Visible = true;
                            gridView1.Columns["kvar"].Caption = "瞬間虛功率";
                        }
                        break;
                    case ElectricSearchTypeEnum.PF:
                        {
                            gridView1.Columns["pfe"].Visible = true;
                            gridView1.Columns["pfe"].Caption = "功率因數";
                        }
                        break;
                    case ElectricSearchTypeEnum.HZ:
                        {
                            gridView1.Columns["hz"].Visible = true;
                            gridView1.Columns["hz"].Caption = "頻率";
                        }
                        break;
                }
                gridView1.Columns["ttimen"].Visible = true;
                gridView1.Columns["ttimen"].Caption = "時間";
                gridView1.Columns["ttimen"].DisplayFormat.FormatString = "yyyy/MM/dd HH:mm:ss";
                gridView1.Columns["ttimen"].BestFit();
                gridView1.Columns["DeviceIndex"].Visible = true;
                gridView1.Columns["DeviceIndex"].Group();
                gridView1.CustomDrawGroupRow += (grids, gride) =>
                {
                    GridGroupRowInfo info = gride.Info as GridGroupRowInfo;
                    for (int i = 0; i < ElectricConfigs.Count; i++)
                    {
                        if (ElectricConfigs[i].DeviceIndex.ToString() == info.GroupValueText)
                        {
                            info.GroupText = ElectricConfigs[i].DeviceName;
                        }
                    }
                };
                #endregion
                #region 曲線圖
                LinechartControl.Legend.Direction = LegendDirection.TopToBottom;//曲線圖線條說明的排序
                LinechartControl.CrosshairOptions.CrosshairLabelMode = CrosshairLabelMode.ShowCommonForAllSeries; //顯示全部線條內容
                LinechartControl.CrosshairOptions.LinesMode = CrosshairLinesMode.Auto;//自動獲取點上面的數值
                LinechartControl.CrosshairOptions.GroupHeaderTextOptions.Font = new Font("微軟正黑體", 12);
                LinechartControl.CrosshairOptions.ShowArgumentLabels = true;//是否顯示Y軸垂直線
                LinechartControl.SideBySideEqualBarWidth = false;//線條是否需要相等寬度
                foreach (var item in Lineseries)
                {
                    LinechartControl.Series.AddRange(item);
                }
                #region 最後曲線圖顯示刻度
                if (LinechartControl.DataSource != null)
                {
                    XYDiagram diagram = (XYDiagram)LinechartControl.Diagram;
                    if (diagram != null)
                    {
                        diagram.EnableAxisXZooming = true;//放大縮小
                        diagram.EnableAxisXScrolling = true;//拖曳
                        diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute; // 顯示設定
                        diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute; // 刻度設定
                        diagram.AxisX.Label.TextPattern = "{A:yyyy-MM-dd HH:mm}";//X軸顯示
                        diagram.AxisX.WholeRange.SideMarginsValue = 0;//不需要邊寬
                        diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;
                    }
                }
                #endregion
                #endregion

            }
            else
            {
                FlyoutAction action = new FlyoutAction();
                action.Caption = "電表資訊-查詢報表錯誤";
                action.Description = "請選擇正確條件再進行查詢";
                action.Commands.Add(FlyoutCommand.OK);
                FlyoutDialog.Show(FindForm(), action);
            }
        }
        /// <summary>
        /// 匯出按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportsimpleButton_Click(object sender, EventArgs e)
        {
            if (gridView1.DataSource != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Xlsx|*xlsx";
                saveFileDialog.Title = "Export Data";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    gridView1.ExportToXlsx($"{saveFileDialog.FileName}.xlsx");
                }
            }
            else
            {
                FlyoutAction action = new FlyoutAction();
                action.Caption = "電表資訊-匯出報表錯誤";
                action.Description = "請查詢報表再進行匯出動作";
                action.Commands.Add(FlyoutCommand.OK);
                FlyoutDialog.Show(FindForm(), action);
            }
        }
    }
}
