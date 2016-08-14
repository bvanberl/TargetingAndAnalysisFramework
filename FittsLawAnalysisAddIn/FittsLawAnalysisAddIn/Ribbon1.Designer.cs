namespace FittsLawAnalysisAddIn
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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
            this.tab1 = this.Factory.CreateRibbonTab();
            this.data_retrieval_group = this.Factory.CreateRibbonGroup();
            this.get_performances_button = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.data_retrieval_group.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.data_retrieval_group);
            this.tab1.Label = "Fitts\' Law Analysis";
            this.tab1.Name = "tab1";
            // 
            // data_retrieval_group
            // 
            this.data_retrieval_group.Items.Add(this.get_performances_button);
            this.data_retrieval_group.Label = "Data Retrieval";
            this.data_retrieval_group.Name = "data_retrieval_group";
            // 
            // get_performances_button
            // 
            this.get_performances_button.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.get_performances_button.Image = global::FittsLawAnalysisAddIn.Properties.Resources.Trendline_Scatter_Graph_01_512;
            this.get_performances_button.Label = "Get Performances";
            this.get_performances_button.Name = "get_performances_button";
            this.get_performances_button.ShowImage = true;
            this.get_performances_button.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.get_performances_button_Click);
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.data_retrieval_group.ResumeLayout(false);
            this.data_retrieval_group.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup data_retrieval_group;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton get_performances_button;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
