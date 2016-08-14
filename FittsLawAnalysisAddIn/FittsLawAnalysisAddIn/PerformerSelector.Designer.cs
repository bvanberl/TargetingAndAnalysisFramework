namespace FittsLawAnalysisAddIn
{
    partial class PerformerSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PerformerSelector));
            this.performer_list = new System.Windows.Forms.CheckedListBox();
            this.return_button = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // performer_list
            // 
            this.performer_list.CheckOnClick = true;
            this.performer_list.FormattingEnabled = true;
            this.performer_list.Location = new System.Drawing.Point(39, 85);
            this.performer_list.Name = "performer_list";
            this.performer_list.Size = new System.Drawing.Size(242, 208);
            this.performer_list.TabIndex = 0;
            this.performer_list.ThreeDCheckBoxes = true;
            // 
            // return_button
            // 
            this.return_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.return_button.Location = new System.Drawing.Point(39, 299);
            this.return_button.Name = "return_button";
            this.return_button.Size = new System.Drawing.Size(242, 50);
            this.return_button.TabIndex = 1;
            this.return_button.Text = "Continue";
            this.return_button.UseVisualStyleBackColor = true;
            this.return_button.Click += new System.EventHandler(this.return_button_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBox1.Location = new System.Drawing.Point(39, 13);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(241, 66);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "Select one or more performers from the list below to view and compare their perfo" +
    "rmances.";
            // 
            // PerformerSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 370);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.return_button);
            this.Controls.Add(this.performer_list);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PerformerSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Performers";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox performer_list;
        private System.Windows.Forms.Button return_button;
        private System.Windows.Forms.TextBox textBox1;
    }
}