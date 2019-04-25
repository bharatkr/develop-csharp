namespace CoSD_Tool
{
    partial class WaitBR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaitBR));
            this.wait_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // wait_label
            // 
            this.wait_label.AutoSize = true;
            this.wait_label.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wait_label.ForeColor = System.Drawing.Color.Green;
            this.wait_label.Location = new System.Drawing.Point(15, 24);
            this.wait_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.wait_label.Name = "wait_label";
            this.wait_label.Size = new System.Drawing.Size(401, 37);
            this.wait_label.TabIndex = 1;
            this.wait_label.Text = "Processing Data... Please Wait";
            // 
            // WaitBR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(518, 103);
            this.ControlBox = false;
            this.Controls.Add(this.wait_label);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "WaitBR";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Wait";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label wait_label;
    }
}