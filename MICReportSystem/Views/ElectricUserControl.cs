using DevExpress.Utils;
using DevExpress.XtraCharts;
using MICReportSystem.Enums;
using MICReportSystem.Methods;
using MICReportSystem.Mysql_Module;
using MICReportSystem.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MICReportSystem.Views
{
    public partial class ElectricUserControl : Field4UserControl
    {
        /// <summary>
        /// 現在時間
        /// </summary>
        private DateTime TTime = DateTime.Now;
        public ElectricUserControl(ElectricConfig config, MysqlMethod mysql, List<AbsProtocol> electricAbsprocotols)
        {
            InitializeComponent();
            MysqlMethod = mysql;
            ElectricConfig = config;
            ElectricAbsProtocols = electricAbsprocotols;
            ElectricgroupControl.Text = ElectricConfig.DeviceName;
            ThreePhaseElectricMeter_Logs = MysqlMethod.Search_ThreePhaseElectricMeter_Log($"{TTime:yyyyMMdd000000}", $"{TTime:yyyyMMdd235959}", ElectricConfig.GatewayIndex, ElectricConfig.DeviceIndex);
            LinechartControl.DataSource = ThreePhaseElectricMeter_Logs;
            Series series = new Series($"{config.DeviceName}", viewType: ViewType.Line);
            series.CrosshairLabelPattern = "{S}" + "\n" + "時間:{A:HH:mm:ss} " + "\n" + "瞬間用電:{V:0.00}kW";
            series.LegendTextPattern = "{A}";
            series.ArgumentDataMember = "ttimen";
            series.ValueDataMembers.AddRange(new string[] { "kw" });
            series.LabelsVisibility = DefaultBoolean.False;
            LinechartControl.Series.Add(series);
            XYDiagram diagram = (XYDiagram)LinechartControl.Diagram;
            if (diagram != null)
            {
                diagram.EnableAxisXZooming = true;//放大縮小
                diagram.EnableAxisXScrolling = true;//拖曳
                diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute; // 顯示設定
                diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute; // 刻度設定
                diagram.AxisX.Label.TextPattern = "{A:HH:mm}";//X軸顯示
                diagram.AxisX.WholeRange.SideMarginsValue = 0;//不需要邊寬
                diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            }
            LinechartControl.CrosshairOptions.ShowArgumentLabels = false;//是否顯示Y軸垂直線
            LinechartControl.CrosshairOptions.ShowArgumentLine = false;//是否顯示Y軸垂直線
            LinechartControl.CrosshairOptions.ShowCrosshairLabels = true;//是否顯示Y軸垂直線
        }
        public override void TextChange()
        {
            TTime = DateTime.Now;
            ThreePhaseElectricMeter_Logs = MysqlMethod.Search_ThreePhaseElectricMeter_Log($"{TTime:yyyyMMdd000000}", $"{TTime:yyyyMMdd235959}", ElectricConfig.GatewayIndex, ElectricConfig.DeviceIndex);
            LinechartControl.DataSource = ThreePhaseElectricMeter_Logs;
            LinechartControl.Refresh();
            var data = ElectricAbsProtocols.Single(g => g.GatewayIndex == ElectricConfig.GatewayIndex & g.DeviceIndex == ElectricConfig.DeviceIndex);
            if (data.ConnectFlag)
            {
                ElectricTypeEnum electricTypeEnum = (ElectricTypeEnum)data.ElectricTypeEnum;
                switch (electricTypeEnum)
                {
                    case ElectricTypeEnum.BAW_4C:
                        {
                            ThreePhaseElectricMeterData threePhaseElectric = (ThreePhaseElectricMeterData)data;
                            rsvlabelControl.Text = threePhaseElectric.rsv.ToString("F2");
                            stvlabelControl.Text = threePhaseElectric.stv.ToString("F2");
                            trvlabelControl.Text = threePhaseElectric.trv.ToString("F2");
                            ralabelControl.Text = threePhaseElectric.ra.ToString("F2");
                            salabelControl.Text = threePhaseElectric.sa.ToString("F2");
                            talabelControl.Text = threePhaseElectric.ta.ToString("F2");
                            pfelabelControl.Text = threePhaseElectric.pfe.ToString("F3");
                            hzlabelControl.Text = threePhaseElectric.hz.ToString("F2");
                            kwlabelControl.Text = threePhaseElectric.kw.ToString("F2");
                            kwhlabelControl.Text = threePhaseElectric.kwh.ToString("F2");
                            kvarlabelControl.Text = threePhaseElectric.kvar.ToString("F2");
                            kvarhlabelControl.Text = threePhaseElectric.kvarh.ToString("F2");
                        }
                        break;
                }
            }
        }
    }
}
