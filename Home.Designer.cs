namespace CoSD_Tool
{
    partial class Home
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.upload_button = new System.Windows.Forms.Button();
            this.update_button = new System.Windows.Forms.Button();
            this.delete_button = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uploadToExcel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_exit = new System.Windows.Forms.Button();
            this.button_manageRepo = new System.Windows.Forms.Button();
            this.Getsource_button = new System.Windows.Forms.Button();
            this.Validation_button = new System.Windows.Forms.Button();
            this.Businesslogic_button = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_conversionfactor = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.documentation_button = new System.Windows.Forms.Button();
            this.feedback_button = new System.Windows.Forms.Button();
            this.label_PageHeading = new System.Windows.Forms.Label();
            this.label_Version = new System.Windows.Forms.Label();
            this.button1_back = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // upload_button
            // 
            this.upload_button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.upload_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.upload_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upload_button.Location = new System.Drawing.Point(494, 372);
            this.upload_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.upload_button.Name = "upload_button";
            this.upload_button.Size = new System.Drawing.Size(266, 120);
            this.upload_button.TabIndex = 0;
            this.upload_button.Text = "View && Manage Data";
            this.toolTip1.SetToolTip(this.upload_button, "View data series or request new one.");
            this.upload_button.UseVisualStyleBackColor = false;
            this.upload_button.Click += new System.EventHandler(this.upload_button_Click);
            // 
            // update_button
            // 
            this.update_button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.update_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.update_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.update_button.Location = new System.Drawing.Point(494, 501);
            this.update_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.update_button.Name = "update_button";
            this.update_button.Size = new System.Drawing.Size(266, 120);
            this.update_button.TabIndex = 1;
            this.update_button.Text = "Update && Validate Data";
            this.update_button.UseVisualStyleBackColor = false;
            this.update_button.Click += new System.EventHandler(this.update_button_Click);
            // 
            // delete_button
            // 
            this.delete_button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.delete_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.delete_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.delete_button.Location = new System.Drawing.Point(768, 501);
            this.delete_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(266, 120);
            this.delete_button.TabIndex = 3;
            this.delete_button.Text = "Archive Data";
            this.toolTip1.SetToolTip(this.delete_button, "Delete data");
            this.delete_button.UseVisualStyleBackColor = false;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 14);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(328, 146);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // uploadToExcel
            // 
            this.uploadToExcel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.uploadToExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uploadToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uploadToExcel.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.uploadToExcel.Location = new System.Drawing.Point(768, 372);
            this.uploadToExcel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.uploadToExcel.Name = "uploadToExcel";
            this.uploadToExcel.Size = new System.Drawing.Size(266, 120);
            this.uploadToExcel.TabIndex = 6;
            this.uploadToExcel.Text = "Import/Export\r\nData";
            this.toolTip1.SetToolTip(this.uploadToExcel, "Import data from excel sheet or export data.");
            this.uploadToExcel.UseVisualStyleBackColor = false;
            this.uploadToExcel.Click += new System.EventHandler(this.uploadToExcel_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(22, 249);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(337, 29);
            this.label3.TabIndex = 10;
            this.label3.Text = "Welcome to the CoSD Tool!";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(22, 300);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(984, 52);
            this.label4.TabIndex = 11;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // button_exit
            // 
            this.button_exit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_exit.Location = new System.Drawing.Point(926, 14);
            this.button_exit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_exit.Name = "button_exit";
            this.button_exit.Size = new System.Drawing.Size(117, 60);
            this.button_exit.TabIndex = 14;
            this.button_exit.Text = "EXIT";
            this.toolTip1.SetToolTip(this.button_exit, "Exit tool");
            this.button_exit.UseVisualStyleBackColor = true;
            this.button_exit.Click += new System.EventHandler(this.button_exit_Click);
            // 
            // button_manageRepo
            // 
            this.button_manageRepo.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button_manageRepo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_manageRepo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_manageRepo.Location = new System.Drawing.Point(766, 631);
            this.button_manageRepo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_manageRepo.Name = "button_manageRepo";
            this.button_manageRepo.Size = new System.Drawing.Size(266, 120);
            this.button_manageRepo.TabIndex = 15;
            this.button_manageRepo.Text = "Repository && CC Management";
            this.toolTip1.SetToolTip(this.button_manageRepo, "Get Metadata or Push data to Repository");
            this.button_manageRepo.UseVisualStyleBackColor = false;
            this.button_manageRepo.Click += new System.EventHandler(this.button_manageRepo_Click);
            // 
            // Getsource_button
            // 
            this.Getsource_button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Getsource_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Getsource_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Getsource_button.Location = new System.Drawing.Point(219, 368);
            this.Getsource_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Getsource_button.Name = "Getsource_button";
            this.Getsource_button.Size = new System.Drawing.Size(266, 120);
            this.Getsource_button.TabIndex = 16;
            this.Getsource_button.Text = "Get Source Data";
            this.toolTip1.SetToolTip(this.Getsource_button, "Stream data from various sources");
            this.Getsource_button.UseVisualStyleBackColor = false;
            this.Getsource_button.Click += new System.EventHandler(this.Getsource_button_Click);
            // 
            // Validation_button
            // 
            this.Validation_button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Validation_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Validation_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Validation_button.Location = new System.Drawing.Point(219, 499);
            this.Validation_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Validation_button.Name = "Validation_button";
            this.Validation_button.Size = new System.Drawing.Size(266, 120);
            this.Validation_button.TabIndex = 21;
            this.Validation_button.Text = "Automated Validation";
            this.Validation_button.UseVisualStyleBackColor = false;
            this.Validation_button.Click += new System.EventHandler(this.Validation_button_Click);
            // 
            // Businesslogic_button
            // 
            this.Businesslogic_button.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Businesslogic_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Businesslogic_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Businesslogic_button.Location = new System.Drawing.Point(492, 629);
            this.Businesslogic_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Businesslogic_button.Name = "Businesslogic_button";
            this.Businesslogic_button.Size = new System.Drawing.Size(266, 120);
            this.Businesslogic_button.TabIndex = 20;
            this.Businesslogic_button.Text = "Execute Business Logic";
            this.toolTip1.SetToolTip(this.Businesslogic_button, "View or execute Business Rules");
            this.Businesslogic_button.UseVisualStyleBackColor = false;
            this.Businesslogic_button.Click += new System.EventHandler(this.Businesslogic_button_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 682);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(184, 20);
            this.label8.TabIndex = 29;
            this.label8.Text = "Modify and Publish Data:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 559);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(190, 20);
            this.label7.TabIndex = 28;
            this.label7.Text = "Check and Validate Data:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 422);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 20);
            this.label5.TabIndex = 27;
            this.label5.Text = "Retreive and Enter Data:";
            // 
            // button_conversionfactor
            // 
            this.button_conversionfactor.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button_conversionfactor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_conversionfactor.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_conversionfactor.Location = new System.Drawing.Point(217, 628);
            this.button_conversionfactor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_conversionfactor.Name = "button_conversionfactor";
            this.button_conversionfactor.Size = new System.Drawing.Size(266, 121);
            this.button_conversionfactor.TabIndex = 30;
            this.button_conversionfactor.Text = "Conversion Factors";
            this.toolTip1.SetToolTip(this.button_conversionfactor, "View and Update Conversion Factors");
            this.button_conversionfactor.UseVisualStyleBackColor = false;
            this.button_conversionfactor.Click += new System.EventHandler(this.button_conversionfactor_Click);
            // 
            // documentation_button
            // 
            this.documentation_button.BackColor = System.Drawing.Color.SeaShell;
            this.documentation_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.documentation_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentation_button.Location = new System.Drawing.Point(399, 759);
            this.documentation_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.documentation_button.Name = "documentation_button";
            this.documentation_button.Size = new System.Drawing.Size(206, 52);
            this.documentation_button.TabIndex = 31;
            this.documentation_button.Text = "Documentation";
            this.toolTip1.SetToolTip(this.documentation_button, "Documentation");
            this.documentation_button.UseVisualStyleBackColor = false;
            this.documentation_button.Click += new System.EventHandler(this.documentation_button_Click);
            // 
            // feedback_button
            // 
            this.feedback_button.BackColor = System.Drawing.Color.SeaShell;
            this.feedback_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.feedback_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.feedback_button.Location = new System.Drawing.Point(640, 759);
            this.feedback_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.feedback_button.Name = "feedback_button";
            this.feedback_button.Size = new System.Drawing.Size(206, 52);
            this.feedback_button.TabIndex = 32;
            this.feedback_button.Text = "Feedback";
            this.toolTip1.SetToolTip(this.feedback_button, "Provide feedback on tool.");
            this.feedback_button.UseVisualStyleBackColor = false;
            this.feedback_button.Click += new System.EventHandler(this.feedback_button_Click);
            // 
            // label_PageHeading
            // 
            this.label_PageHeading.AutoSize = true;
            this.label_PageHeading.Font = new System.Drawing.Font("Castellar", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PageHeading.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label_PageHeading.Location = new System.Drawing.Point(593, 14);
            this.label_PageHeading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_PageHeading.Name = "label_PageHeading";
            this.label_PageHeading.Size = new System.Drawing.Size(208, 62);
            this.label_PageHeading.TabIndex = 48;
            this.label_PageHeading.Text = "Home";
            this.label_PageHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.BackColor = System.Drawing.Color.White;
            this.label_Version.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Version.ForeColor = System.Drawing.Color.Black;
            this.label_Version.Location = new System.Drawing.Point(22, 165);
            this.label_Version.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(171, 25);
            this.label_Version.TabIndex = 51;
            this.label_Version.Text = "Tool Version 5.0";
            // 
            // button1_back
            // 
            this.button1_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1_back.Location = new System.Drawing.Point(926, 97);
            this.button1_back.Name = "button1_back";
            this.button1_back.Size = new System.Drawing.Size(118, 63);
            this.button1_back.TabIndex = 52;
            this.button1_back.Text = "Landing Screen";
            this.button1_back.UseVisualStyleBackColor = true;
            this.button1_back.Click += new System.EventHandler(this.button1_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1056, 948);
            this.Controls.Add(this.button1_back);
            this.Controls.Add(this.label_Version);
            this.Controls.Add(this.label_PageHeading);
            this.Controls.Add(this.feedback_button);
            this.Controls.Add(this.documentation_button);
            this.Controls.Add(this.button_conversionfactor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Validation_button);
            this.Controls.Add(this.Businesslogic_button);
            this.Controls.Add(this.Getsource_button);
            this.Controls.Add(this.button_manageRepo);
            this.Controls.Add(this.button_exit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.uploadToExcel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.delete_button);
            this.Controls.Add(this.update_button);
            this.Controls.Add(this.upload_button);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Home";
            this.Text = "CoSD Tool";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button upload_button;
        private System.Windows.Forms.Button update_button;
        private System.Windows.Forms.Button delete_button;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button uploadToExcel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_exit;
        private System.Windows.Forms.Button button_manageRepo;
        private System.Windows.Forms.Button Getsource_button;
        private System.Windows.Forms.Button Validation_button;
        private System.Windows.Forms.Button Businesslogic_button;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_conversionfactor;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button documentation_button;
        private System.Windows.Forms.Button feedback_button;
        private System.Windows.Forms.Label label_PageHeading;
        private System.Windows.Forms.Label label_Version;
        private System.Windows.Forms.Button button1_back;
    }
}

