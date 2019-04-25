namespace CoSD_Tool
{
    partial class RepoManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RepoManagement));
            this.CoSDdoc_Button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button_back = new System.Windows.Forms.Button();
            this.label_Path = new System.Windows.Forms.Label();
            this.label_PageHeading = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.CC_Button = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // CoSDdoc_Button
            // 
            this.CoSDdoc_Button.BackColor = System.Drawing.Color.LightGray;
            this.CoSDdoc_Button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CoSDdoc_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CoSDdoc_Button.ForeColor = System.Drawing.Color.Black;
            this.CoSDdoc_Button.Location = new System.Drawing.Point(220, 298);
            this.CoSDdoc_Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CoSDdoc_Button.Name = "CoSDdoc_Button";
            this.CoSDdoc_Button.Size = new System.Drawing.Size(375, 99);
            this.CoSDdoc_Button.TabIndex = 1;
            this.CoSDdoc_Button.Text = "Pull latest CoSD Metadata";
            this.toolTip1.SetToolTip(this.CoSDdoc_Button, "Gets latest CoSD Metadata from\r\nMaster CoSD to Animal Products CoSD");
            this.CoSDdoc_Button.UseVisualStyleBackColor = true;
            this.CoSDdoc_Button.Click += new System.EventHandler(this.CoSDdoc_Button_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightGray;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(658, 298);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(375, 99);
            this.button1.TabIndex = 2;
            this.button1.Text = "Push Data to Repository";
            this.toolTip1.SetToolTip(this.button1, "Push latest data from\r\nAP CoSD to Repository CoSD");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_back
            // 
            this.button_back.BackColor = System.Drawing.Color.LightGray;
            this.button_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_back.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_back.ForeColor = System.Drawing.Color.Black;
            this.button_back.Location = new System.Drawing.Point(1038, 97);
            this.button_back.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(202, 35);
            this.button_back.TabIndex = 3;
            this.button_back.Text = "Back to Home Page";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // label_Path
            // 
            this.label_Path.AutoSize = true;
            this.label_Path.Location = new System.Drawing.Point(432, 154);
            this.label_Path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Path.Name = "label_Path";
            this.label_Path.Size = new System.Drawing.Size(248, 20);
            this.label_Path.TabIndex = 95;
            this.label_Path.Text = "Home -> Repository Management";
            this.label_Path.Click += new System.EventHandler(this.label_Path_Click);
            // 
            // label_PageHeading
            // 
            this.label_PageHeading.AutoSize = true;
            this.label_PageHeading.Font = new System.Drawing.Font("Lucida Calligraphy", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PageHeading.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label_PageHeading.Location = new System.Drawing.Point(424, 11);
            this.label_PageHeading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_PageHeading.Name = "label_PageHeading";
            this.label_PageHeading.Size = new System.Drawing.Size(588, 136);
            this.label_PageHeading.TabIndex = 94;
            this.label_PageHeading.Text = "Repository and CC \r\nManagement";
            this.label_PageHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CC_Button
            // 
            this.CC_Button.BackColor = System.Drawing.Color.LightGray;
            this.CC_Button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CC_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CC_Button.ForeColor = System.Drawing.Color.Black;
            this.CC_Button.Location = new System.Drawing.Point(450, 418);
            this.CC_Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CC_Button.Name = "CC_Button";
            this.CC_Button.Size = new System.Drawing.Size(375, 99);
            this.CC_Button.TabIndex = 98;
            this.CC_Button.Text = "Pull Cross Commodity data";
            this.toolTip1.SetToolTip(this.CC_Button, "Pull Cross Commodity data");
            this.CC_Button.UseVisualStyleBackColor = true;
            this.CC_Button.Click += new System.EventHandler(this.CC_Button_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(18, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(328, 146);
            this.pictureBox1.TabIndex = 96;
            this.pictureBox1.TabStop = false;
            // 
            // RepoManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1593, 551);
            this.Controls.Add(this.CC_Button);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label_Path);
            this.Controls.Add(this.label_PageHeading);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.CoSDdoc_Button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "RepoManagement";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Documentation";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CoSDdoc_Button;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.Label label_Path;
        private System.Windows.Forms.Label label_PageHeading;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button CC_Button;
    }
}