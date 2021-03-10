
namespace MICReportSystem.Views
{
    partial class MainUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.MonthchartControl = new DevExpress.XtraCharts.ChartControl();
            this.ElectricpanelControl = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.MonthchartControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ElectricpanelControl)).BeginInit();
            this.SuspendLayout();
            // 
            // MonthchartControl
            // 
            this.MonthchartControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.MonthchartControl.Legend.Name = "Default Legend";
            this.MonthchartControl.Location = new System.Drawing.Point(0, 0);
            this.MonthchartControl.Name = "MonthchartControl";
            this.MonthchartControl.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.MonthchartControl.Size = new System.Drawing.Size(1716, 656);
            this.MonthchartControl.TabIndex = 0;
            chartTitle1.Font = new System.Drawing.Font("Tahoma", 28F);
            chartTitle1.Text = "本月發電量";
            chartTitle1.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(12)))), ((int)(((byte)(12)))));
            this.MonthchartControl.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // ElectricpanelControl
            // 
            this.ElectricpanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ElectricpanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ElectricpanelControl.Location = new System.Drawing.Point(0, 656);
            this.ElectricpanelControl.Name = "ElectricpanelControl";
            this.ElectricpanelControl.Size = new System.Drawing.Size(1716, 392);
            this.ElectricpanelControl.TabIndex = 2;
            // 
            // MainUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ElectricpanelControl);
            this.Controls.Add(this.MonthchartControl);
            this.Name = "MainUserControl";
            this.Size = new System.Drawing.Size(1716, 1048);
            ((System.ComponentModel.ISupportInitialize)(this.MonthchartControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ElectricpanelControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl MonthchartControl;
        private DevExpress.XtraEditors.PanelControl ElectricpanelControl;
    }
}
