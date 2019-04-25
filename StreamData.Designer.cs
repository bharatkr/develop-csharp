namespace CoSD_Tool
{
    partial class StreamData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StreamData));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_back = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.Success_Label = new System.Windows.Forms.Label();
            this.rowsretrievedlabel = new System.Windows.Forms.Label();
            this.label_rowsretrieved = new System.Windows.Forms.Label();
            this.label_dataretrievalresult = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.Trade_button = new System.Windows.Forms.Button();
            this.BLS_button = new System.Windows.Forms.Button();
            this.NASS_button = new System.Windows.Forms.Button();
            this.unsuccess_Label = new System.Windows.Forms.Label();
            this.label_Path = new System.Windows.Forms.Label();
            this.label_PageHeading = new System.Windows.Forms.Label();
            this.WASDE_button = new System.Windows.Forms.Button();
            this.AMS_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bls_label = new System.Windows.Forms.Label();
            this.wasde_label = new System.Windows.Forms.Label();
            this.Nass_label = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(18, 19);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(328, 146);
            this.pictureBox1.TabIndex = 34;
            this.pictureBox1.TabStop = false;
            // 
            // button_back
            // 
            this.button_back.BackColor = System.Drawing.Color.LightGray;
            this.button_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_back.ForeColor = System.Drawing.Color.Black;
            this.button_back.Location = new System.Drawing.Point(789, 79);
            this.button_back.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(174, 35);
            this.button_back.TabIndex = 35;
            this.button_back.Text = "Back to Home Page";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(217, 342);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(450, 20);
            this.label7.TabIndex = 78;
            this.label7.Text = "Choose data source to get the latest data into CoSD";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // Success_Label
            // 
            this.Success_Label.Font = new System.Drawing.Font("Microsoft YaHei UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Success_Label.ForeColor = System.Drawing.Color.Black;
            this.Success_Label.Location = new System.Drawing.Point(768, 569);
            this.Success_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Success_Label.Name = "Success_Label";
            this.Success_Label.Size = new System.Drawing.Size(382, 172);
            this.Success_Label.TabIndex = 77;
            this.Success_Label.Text = "Streaming ended with no errors!";
            this.Success_Label.Visible = false;
            this.Success_Label.Click += new System.EventHandler(this.Success_Label_Click);
            // 
            // rowsretrievedlabel
            // 
            this.rowsretrievedlabel.AutoSize = true;
            this.rowsretrievedlabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.rowsretrievedlabel.Font = new System.Drawing.Font("Britannic Bold", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rowsretrievedlabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.rowsretrievedlabel.Location = new System.Drawing.Point(846, 422);
            this.rowsretrievedlabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.rowsretrievedlabel.Name = "rowsretrievedlabel";
            this.rowsretrievedlabel.Size = new System.Drawing.Size(80, 82);
            this.rowsretrievedlabel.TabIndex = 76;
            this.rowsretrievedlabel.Text = "0";
            this.rowsretrievedlabel.Visible = false;
            // 
            // label_rowsretrieved
            // 
            this.label_rowsretrieved.AutoSize = true;
            this.label_rowsretrieved.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_rowsretrieved.Location = new System.Drawing.Point(764, 382);
            this.label_rowsretrieved.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_rowsretrieved.Name = "label_rowsretrieved";
            this.label_rowsretrieved.Size = new System.Drawing.Size(259, 25);
            this.label_rowsretrieved.TabIndex = 75;
            this.label_rowsretrieved.Text = "Number of rows retreived:";
            this.label_rowsretrieved.Visible = false;
            this.label_rowsretrieved.Click += new System.EventHandler(this.label_rowsretrieved_Click);
            // 
            // label_dataretrievalresult
            // 
            this.label_dataretrievalresult.AutoSize = true;
            this.label_dataretrievalresult.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_dataretrievalresult.Location = new System.Drawing.Point(764, 532);
            this.label_dataretrievalresult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_dataretrievalresult.Name = "label_dataretrievalresult";
            this.label_dataretrievalresult.Size = new System.Drawing.Size(223, 25);
            this.label_dataretrievalresult.TabIndex = 74;
            this.label_dataretrievalresult.Text = "Data streaming result:";
            this.label_dataretrievalresult.Visible = false;
            this.label_dataretrievalresult.Click += new System.EventHandler(this.label_dataretrievalresult_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.LightGray;
            this.button4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.Black;
            this.button4.Location = new System.Drawing.Point(482, 554);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(242, 132);
            this.button4.TabIndex = 73;
            this.button4.Text = "Get PS&&D Data";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Trade_button
            // 
            this.Trade_button.BackColor = System.Drawing.Color.LightGray;
            this.Trade_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Trade_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Trade_button.ForeColor = System.Drawing.Color.Black;
            this.Trade_button.Location = new System.Drawing.Point(482, 382);
            this.Trade_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Trade_button.Name = "Trade_button";
            this.Trade_button.Size = new System.Drawing.Size(242, 132);
            this.Trade_button.TabIndex = 72;
            this.Trade_button.Text = "Get U.S. Trade Data";
            this.Trade_button.UseVisualStyleBackColor = true;
            this.Trade_button.Click += new System.EventHandler(this.Trade_button_Click);
            // 
            // BLS_button
            // 
            this.BLS_button.BackColor = System.Drawing.Color.LightGray;
            this.BLS_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BLS_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BLS_button.ForeColor = System.Drawing.Color.Black;
            this.BLS_button.Location = new System.Drawing.Point(217, 554);
            this.BLS_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BLS_button.Name = "BLS_button";
            this.BLS_button.Size = new System.Drawing.Size(242, 132);
            this.BLS_button.TabIndex = 71;
            this.BLS_button.Text = "Get BLS Data";
            this.BLS_button.UseVisualStyleBackColor = true;
            this.BLS_button.Click += new System.EventHandler(this.BLS_button_Click);
            // 
            // NASS_button
            // 
            this.NASS_button.BackColor = System.Drawing.Color.LightGray;
            this.NASS_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NASS_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NASS_button.ForeColor = System.Drawing.Color.Black;
            this.NASS_button.Location = new System.Drawing.Point(217, 382);
            this.NASS_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NASS_button.Name = "NASS_button";
            this.NASS_button.Size = new System.Drawing.Size(242, 132);
            this.NASS_button.TabIndex = 70;
            this.NASS_button.Text = "Get NASS Data";
            this.NASS_button.UseVisualStyleBackColor = true;
            this.NASS_button.Click += new System.EventHandler(this.NASS_button_Click);
            // 
            // unsuccess_Label
            // 
            this.unsuccess_Label.AutoSize = true;
            this.unsuccess_Label.Font = new System.Drawing.Font("Microsoft YaHei UI", 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unsuccess_Label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.unsuccess_Label.Location = new System.Drawing.Point(778, 569);
            this.unsuccess_Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.unsuccess_Label.Name = "unsuccess_Label";
            this.unsuccess_Label.Size = new System.Drawing.Size(545, 56);
            this.unsuccess_Label.TabIndex = 81;
            this.unsuccess_Label.Text = "Streaming unsuccessfull.";
            this.unsuccess_Label.Visible = false;
            this.unsuccess_Label.Click += new System.EventHandler(this.unsuccess_Label_Click);
            // 
            // label_Path
            // 
            this.label_Path.AutoSize = true;
            this.label_Path.Location = new System.Drawing.Point(305, 255);
            this.label_Path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Path.Name = "label_Path";
            this.label_Path.Size = new System.Drawing.Size(165, 20);
            this.label_Path.TabIndex = 83;
            this.label_Path.Text = "Home -> Stream Data";
            this.label_Path.Click += new System.EventHandler(this.label_Path_Click);
            // 
            // label_PageHeading
            // 
            this.label_PageHeading.AutoSize = true;
            this.label_PageHeading.Font = new System.Drawing.Font("Lucida Calligraphy", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PageHeading.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label_PageHeading.Location = new System.Drawing.Point(297, 186);
            this.label_PageHeading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_PageHeading.Name = "label_PageHeading";
            this.label_PageHeading.Size = new System.Drawing.Size(407, 68);
            this.label_PageHeading.TabIndex = 82;
            this.label_PageHeading.Text = "Stream Data";
            this.label_PageHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_PageHeading.Click += new System.EventHandler(this.label_PageHeading_Click);
            // 
            // WASDE_button
            // 
            this.WASDE_button.BackColor = System.Drawing.Color.LightGray;
            this.WASDE_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WASDE_button.ForeColor = System.Drawing.Color.Black;
            this.WASDE_button.Location = new System.Drawing.Point(482, 721);
            this.WASDE_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.WASDE_button.Name = "WASDE_button";
            this.WASDE_button.Size = new System.Drawing.Size(242, 132);
            this.WASDE_button.TabIndex = 86;
            this.WASDE_button.Text = " Get WASDE Data";
            this.WASDE_button.UseVisualStyleBackColor = true;
            this.WASDE_button.Click += new System.EventHandler(this.WASDE_button_Click);
            // 
            // AMS_button
            // 
            this.AMS_button.BackColor = System.Drawing.Color.LightGray;
            this.AMS_button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AMS_button.ForeColor = System.Drawing.Color.Black;
            this.AMS_button.Location = new System.Drawing.Point(217, 721);
            this.AMS_button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AMS_button.Name = "AMS_button";
            this.AMS_button.Size = new System.Drawing.Size(242, 132);
            this.AMS_button.TabIndex = 85;
            this.AMS_button.Text = "Get AMS Data";
            this.AMS_button.UseVisualStyleBackColor = true;
            this.AMS_button.Click += new System.EventHandler(this.AMS_button_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(214, 1091);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(561, 20);
            this.label1.TabIndex = 87;
            this.label1.Text = "Trade, WASDE, BLS: Latest data ready on release day after 3pm.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(214, 1111);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(484, 20);
            this.label2.TabIndex = 88;
            this.label2.Text = "NASS: Latest data ready next morning after release day.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(214, 1070);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 20);
            this.label3.TabIndex = 89;
            this.label3.Text = "Data availability:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // bls_label
            // 
            this.bls_label.AutoSize = true;
            this.bls_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bls_label.Location = new System.Drawing.Point(326, 959);
            this.bls_label.Name = "bls_label";
            this.bls_label.Size = new System.Drawing.Size(279, 20);
            this.bls_label.TabIndex = 98;
            this.bls_label.Text = "No BLS records streamed till date";
            // 
            // wasde_label
            // 
            this.wasde_label.AutoSize = true;
            this.wasde_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wasde_label.Location = new System.Drawing.Point(356, 996);
            this.wasde_label.Name = "wasde_label";
            this.wasde_label.Size = new System.Drawing.Size(310, 20);
            this.wasde_label.TabIndex = 97;
            this.wasde_label.Text = "No WASDE records streamed till date";
            this.wasde_label.Click += new System.EventHandler(this.wasde_label_Click);
            // 
            // Nass_label
            // 
            this.Nass_label.AutoSize = true;
            this.Nass_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Nass_label.Location = new System.Drawing.Point(340, 921);
            this.Nass_label.Name = "Nass_label";
            this.Nass_label.Size = new System.Drawing.Size(293, 20);
            this.Nass_label.TabIndex = 96;
            this.Nass_label.Text = "No NASS records streamed till date";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(217, 959);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 20);
            this.label5.TabIndex = 100;
            this.label5.Text = "BLS status:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label6.Location = new System.Drawing.Point(217, 996);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 20);
            this.label6.TabIndex = 101;
            this.label6.Text = "WASDE status:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label4.Location = new System.Drawing.Point(217, 921);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 20);
            this.label4.TabIndex = 102;
            this.label4.Text = "NASS status:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label8.Location = new System.Drawing.Point(217, 1029);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 20);
            this.label8.TabIndex = 103;
            this.label8.Text = "Trade Status:";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(344, 1028);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(291, 20);
            this.label10.TabIndex = 105;
            this.label10.Text = "No Trade records streamed till date";
            // 
            // StreamData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1362, 1050);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bls_label);
            this.Controls.Add(this.wasde_label);
            this.Controls.Add(this.Nass_label);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.WASDE_button);
            this.Controls.Add(this.AMS_button);
            this.Controls.Add(this.label_Path);
            this.Controls.Add(this.label_PageHeading);
            this.Controls.Add(this.unsuccess_Label);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Success_Label);
            this.Controls.Add(this.rowsretrievedlabel);
            this.Controls.Add(this.label_rowsretrieved);
            this.Controls.Add(this.label_dataretrievalresult);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.Trade_button);
            this.Controls.Add(this.BLS_button);
            this.Controls.Add(this.NASS_button);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "StreamData";
            this.Text = "Streaming Data from Sources";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.StreamData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label Success_Label;
        private System.Windows.Forms.Label rowsretrievedlabel;
        private System.Windows.Forms.Label label_rowsretrieved;
        private System.Windows.Forms.Label label_dataretrievalresult;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button Trade_button;
        private System.Windows.Forms.Button BLS_button;
        private System.Windows.Forms.Button NASS_button;
        private System.Windows.Forms.Label unsuccess_Label;
        private System.Windows.Forms.Label label_Path;
        private System.Windows.Forms.Label label_PageHeading;
        private System.Windows.Forms.Button WASDE_button;
        private System.Windows.Forms.Button AMS_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label bls_label;
        private System.Windows.Forms.Label wasde_label;
        private System.Windows.Forms.Label Nass_label;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
    }
}