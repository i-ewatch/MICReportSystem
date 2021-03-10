
namespace MICReportSystem
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.fluentDesignFormControl1 = new DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl();
            this.SettingbarButtonItem = new DevExpress.XtraBars.BarButtonItem();
            this.fluentFormDefaultManager1 = new DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager(this.components);
            this.ButtonpanelControl = new DevExpress.XtraEditors.PanelControl();
            this.accordionControl1 = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.accordionControlElement1 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.LogopictureEdit = new DevExpress.XtraEditors.PictureEdit();
            this.ViewpanelControl = new DevExpress.XtraEditors.PanelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonpanelControl)).BeginInit();
            this.ButtonpanelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogopictureEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewpanelControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // fluentDesignFormControl1
            // 
            this.fluentDesignFormControl1.FluentDesignForm = this;
            this.fluentDesignFormControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.SettingbarButtonItem});
            this.fluentDesignFormControl1.Location = new System.Drawing.Point(0, 0);
            this.fluentDesignFormControl1.Manager = this.fluentFormDefaultManager1;
            this.fluentDesignFormControl1.Margin = new System.Windows.Forms.Padding(2);
            this.fluentDesignFormControl1.Name = "fluentDesignFormControl1";
            this.fluentDesignFormControl1.Size = new System.Drawing.Size(1918, 31);
            this.fluentDesignFormControl1.TabIndex = 2;
            this.fluentDesignFormControl1.TabStop = false;
            this.fluentDesignFormControl1.TitleItemLinks.Add(this.SettingbarButtonItem);
            // 
            // SettingbarButtonItem
            // 
            this.SettingbarButtonItem.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.SettingbarButtonItem.Id = 0;
            this.SettingbarButtonItem.Name = "SettingbarButtonItem";
            this.SettingbarButtonItem.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.SettingbarButtonItem_ItemClick);
            // 
            // fluentFormDefaultManager1
            // 
            this.fluentFormDefaultManager1.DockingEnabled = false;
            this.fluentFormDefaultManager1.Form = this;
            this.fluentFormDefaultManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.SettingbarButtonItem});
            this.fluentFormDefaultManager1.MaxItemId = 1;
            // 
            // ButtonpanelControl
            // 
            this.ButtonpanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ButtonpanelControl.Controls.Add(this.accordionControl1);
            this.ButtonpanelControl.Controls.Add(this.LogopictureEdit);
            this.ButtonpanelControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.ButtonpanelControl.Location = new System.Drawing.Point(0, 31);
            this.ButtonpanelControl.Name = "ButtonpanelControl";
            this.ButtonpanelControl.Size = new System.Drawing.Size(202, 1048);
            this.ButtonpanelControl.TabIndex = 3;
            // 
            // accordionControl1
            // 
            this.accordionControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.accordionControl1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement1});
            this.accordionControl1.Location = new System.Drawing.Point(0, 80);
            this.accordionControl1.Margin = new System.Windows.Forms.Padding(2);
            this.accordionControl1.Name = "accordionControl1";
            this.accordionControl1.ScrollBarMode = DevExpress.XtraBars.Navigation.ScrollBarMode.Touch;
            this.accordionControl1.Size = new System.Drawing.Size(202, 968);
            this.accordionControl1.TabIndex = 6;
            this.accordionControl1.ViewType = DevExpress.XtraBars.Navigation.AccordionControlViewType.HamburgerMenu;
            // 
            // accordionControlElement1
            // 
            this.accordionControlElement1.Name = "accordionControlElement1";
            this.accordionControlElement1.Text = "Element1";
            // 
            // LogopictureEdit
            // 
            this.LogopictureEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this.LogopictureEdit.Location = new System.Drawing.Point(0, 0);
            this.LogopictureEdit.MenuManager = this.fluentFormDefaultManager1;
            this.LogopictureEdit.Name = "LogopictureEdit";
            this.LogopictureEdit.Properties.AllowFocused = false;
            this.LogopictureEdit.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.LogopictureEdit.Properties.Appearance.Options.UseBackColor = true;
            this.LogopictureEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.LogopictureEdit.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.LogopictureEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.LogopictureEdit.Size = new System.Drawing.Size(202, 80);
            this.LogopictureEdit.TabIndex = 0;
            // 
            // ViewpanelControl
            // 
            this.ViewpanelControl.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.ViewpanelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ViewpanelControl.Location = new System.Drawing.Point(202, 31);
            this.ViewpanelControl.Name = "ViewpanelControl";
            this.ViewpanelControl.Size = new System.Drawing.Size(1716, 1048);
            this.ViewpanelControl.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "technology");
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1918, 1079);
            this.Controls.Add(this.ViewpanelControl);
            this.Controls.Add(this.ButtonpanelControl);
            this.Controls.Add(this.fluentDesignFormControl1);
            this.FluentDesignFormControl = this.fluentDesignFormControl1;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "MICReportSystem";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fluentDesignFormControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fluentFormDefaultManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonpanelControl)).EndInit();
            this.ButtonpanelControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LogopictureEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ViewpanelControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl fluentDesignFormControl1;
        private DevExpress.XtraBars.FluentDesignSystem.FluentFormDefaultManager fluentFormDefaultManager1;
        private DevExpress.XtraEditors.PanelControl ButtonpanelControl;
        private DevExpress.XtraEditors.PictureEdit LogopictureEdit;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement1;
        private DevExpress.XtraEditors.PanelControl ViewpanelControl;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraBars.BarButtonItem SettingbarButtonItem;
        private DevExpress.Utils.ImageCollection imageCollection1;
        public DevExpress.XtraBars.Navigation.AccordionControl accordionControl1;
    }
}

