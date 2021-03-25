using DevExpress.Utils;
using DevExpress.XtraCharts;
using MICReportSystem.Methods;
using MICReportSystem.Mysql_Module;
using MICReportSystem.Protocols;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace MICReportSystem.Views
{
    public partial class MainUserControl : Field4UserControl
    {
        /// <summary>
        /// 現在時間
        /// </summary>
        private DateTime TTime = DateTime.Now;
        /// <summary>
        /// 總曲線圖勾選物件
        /// </summary>
        private SeriesCollection Series { get { return MonthchartControl.Series; } }
        /// <summary>
        /// 曲線圖顯示點為最大值
        /// </summary>
        private const int othersSeriesIndex = 32;

        public MainUserControl(MysqlMethod mysql, List<AbsProtocol> electricAbsprotocols)
        {
            InitializeComponent();
            MysqlMethod = mysql;
            ElectricAbsProtocols = electricAbsprotocols;
            GatewayConfigs = MysqlMethod.Search_GatewayConfig();
            foreach (var Gatewayitem in GatewayConfigs)
            {
                var configs = MysqlMethod.Search_ElectricConfig(Gatewayitem.GatewayIndex);
                foreach (var Configitem in configs)
                {
                    ElectricConfigs.Add(Configitem);
                }
            }
            #region 月累積量
            Month_ElectricTotal();
            foreach (var ElectricConfigitem in ElectricConfigs)
            {
                Series series = new Series($"{ElectricConfigitem.DeviceName}", viewType: ViewType.Bar);
                series.DataSource = ElectricTotals.Where(g => g.GatewayIndex == ElectricConfigitem.GatewayIndex & g.DeviceIndex == ElectricConfigitem.DeviceIndex).ToList();
                series.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:yyyy-MM-dd} " + "\n" + "用電量:{V:0.00}kWh";
                series.LegendTextPattern = "{A}";
                series.ArgumentDataMember = "ttimen";
                series.ValueDataMembers.AddRange(new string[] { "KwhTotal" });
                series.CheckedInLegend = true;
                ((BarSeriesLabel)series.Label).ShowForZeroValues = true;
                MonthchartControl.Series.Add(series);
            }
            MonthchartControl.LegendItemChecked += (s, e) => //曲線圖 Series勾選功能
            {
                Series checkedSeries = e.CheckedElement as Series;
                if (checkedSeries == null || Series.IndexOf(checkedSeries) != othersSeriesIndex)
                    return;
                for (int i = 0; i < Series.Count; i++)
                    if (i < othersSeriesIndex)
                        Series[i].Visible = e.NewCheckState;
            };
            MonthchartControl.Legend.MarkerMode = LegendMarkerMode.CheckBox;
            MonthchartControl.Legend.Border.Visibility = DefaultBoolean.False;
            MonthchartControl.Legend.BackColor = Color.Transparent;
            MonthchartControl.Legend.Direction = LegendDirection.BottomToTop;
            MonthchartControl.CrosshairOptions.CrosshairLabelMode = CrosshairLabelMode.ShowCommonForAllSeries;
            MonthchartControl.CrosshairOptions.LinesMode = CrosshairLinesMode.Auto;
            MonthchartControl.CrosshairOptions.ShowArgumentLabels = true;
            XYDiagram diagram = (XYDiagram)MonthchartControl.Diagram;
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Day; // 顯示設定
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Day; // 刻度設定
            diagram.AxisX.WholeRange.SideMarginsValue = 1;//不需要邊寬
            #endregion
            #region 分電表顯示
            int ElectricInt = 0;
            foreach (var ElectricConfigitem in ElectricConfigs)
            {
                ElectricUserControl electric = new ElectricUserControl(ElectricConfigitem, mysql, ElectricAbsProtocols) { Location = new Point(5 + (ElectricInt * 855), 5) };
                ElectricpanelControl.Controls.Add(electric);
                Field4UserControls.Add(electric);
                ElectricInt++;
            }
            #endregion
        }
        public override void TextChange()
        {
            TimeSpan BartimeSpan = DateTime.Now.Subtract(TTime);
            if (BartimeSpan.TotalMilliseconds >= 60000)
            {
                Month_ElectricTotal();
                int Index = 0;
                foreach (var ElectricConfigitem in ElectricConfigs)
                {
                    MonthchartControl.Series[Index].DataSource = ElectricTotals.Where(g => g.GatewayIndex == ElectricConfigitem.GatewayIndex & g.DeviceIndex == ElectricConfigitem.DeviceIndex).ToList();
                    Index++;
                }
                MonthchartControl.Refresh();
            }         
            foreach (var item in Field4UserControls)
            {
                item.TextChange();
            }
        }
        /// <summary>
        /// 月累積量公式
        /// </summary>
        private void Month_ElectricTotal()
        {
            TTime = DateTime.Now;
            ElectricTotals.Clear();
            foreach (var ElectricConfigitem in ElectricConfigs)
            {
                var ElectricTotal = MysqlMethod.Search_ElectricTotal($"{TTime:yyyyMM01}", $"{TTime:yyyyMM31}", ElectricConfigitem.GatewayIndex, ElectricConfigitem.DeviceIndex);
                for (int i = 0; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
                {
                    foreach (var item in ElectricTotal)
                    {
                        if (i >= 9)
                        {
                            if (item.ttime == DateTime.Now.ToString("yyyyMM") + $"{i + 1}")
                            {
                                ElectricTotals.Add(item);
                            }
                            else
                            {
                                ElectricTotal electricTotal = new ElectricTotal()
                                {
                                    ttimen = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/") + $"{i + 1}" + " 00:00:00"),
                                    GatewayIndex = ElectricConfigitem.GatewayIndex,
                                    DeviceIndex = ElectricConfigitem.DeviceIndex,
                                    KwhTotal = 0
                                };
                                ElectricTotals.Add(electricTotal);
                            }
                        }
                        else
                        {
                            if (item.ttime == DateTime.Now.ToString("yyyyMM") + $"0{i + 1}")
                            {
                                ElectricTotals.Add(item);
                            }
                            else
                            {
                                ElectricTotal electricTotal = new ElectricTotal()
                                {
                                    ttimen = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/") + $"0{i + 1}" + " 00:00:00"),
                                    GatewayIndex = ElectricConfigitem.GatewayIndex,
                                    DeviceIndex = ElectricConfigitem.DeviceIndex,
                                    KwhTotal = 0
                                };
                                ElectricTotals.Add(electricTotal);
                            }
                        }

                    }
                }
            }
        }
    }
}
