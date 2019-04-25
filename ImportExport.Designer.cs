namespace CoSD_Tool
{
    partial class ImportExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportExport));
            this.importLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_back = new System.Windows.Forms.Button();
            this.button_importExcel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_excelPath = new System.Windows.Forms.TextBox();
            this.button_excelPath = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataseriesButton = new System.Windows.Forms.Button();
            this.businessrulesButton = new System.Windows.Forms.Button();
            this.importPanel = new System.Windows.Forms.Panel();
            this.textBox_successmsg = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_excelSheet = new CoSD_Tool.MyCombobox();
            this.label_ImportResultPath = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label_message = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.button_ChangePath = new System.Windows.Forms.Button();
            this.exportPanel = new System.Windows.Forms.Panel();
            this.exportResultLabel = new System.Windows.Forms.Label();
            this.exportLabel = new System.Windows.Forms.Label();
            this.exportLabel1 = new System.Windows.Forms.Label();
            this.label_Path = new System.Windows.Forms.Label();
            this.label_PageHeading = new System.Windows.Forms.Label();
            this.label_CoSDPath = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.importPanel.SuspendLayout();
            this.exportPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // importLabel
            // 
            this.importLabel.AutoSize = true;
            this.importLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.importLabel.Location = new System.Drawing.Point(189, 225);
            this.importLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.importLabel.Name = "importLabel";
            this.importLabel.Size = new System.Drawing.Size(387, 29);
            this.importLabel.TabIndex = 40;
            this.importLabel.Text = "Import Data from an Excel Sheet\r\n";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(14, 14);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(328, 146);
            this.pictureBox1.TabIndex = 38;
            this.pictureBox1.TabStop = false;
            // 
            // button_back
            // 
            this.button_back.BackColor = System.Drawing.Color.LightGray;
            this.button_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_back.ForeColor = System.Drawing.Color.Black;
            this.button_back.Location = new System.Drawing.Point(1454, 72);
            this.button_back.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(202, 35);
            this.button_back.TabIndex = 6;
            this.button_back.Text = "Back to Home Page";
            this.toolTip1.SetToolTip(this.button_back, "Back to Home page");
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // button_importExcel
            // 
            this.button_importExcel.Cursor = System.Windows.Forms.Cursors.Default;
            this.button_importExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_importExcel.ForeColor = System.Drawing.Color.Black;
            this.button_importExcel.Location = new System.Drawing.Point(144, 212);
            this.button_importExcel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_importExcel.Name = "button_importExcel";
            this.button_importExcel.Size = new System.Drawing.Size(433, 46);
            this.button_importExcel.TabIndex = 2;
            this.button_importExcel.Text = "Import my Excel data into CoSD";
            this.toolTip1.SetToolTip(this.button_importExcel, "Import selected Excel file into AP CoSD");
            this.button_importExcel.UseVisualStyleBackColor = true;
            this.button_importExcel.Click += new System.EventHandler(this.button_importExcel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 40);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 44;
            this.label1.Text = "Excel file path:";
            // 
            // textBox_excelPath
            // 
            this.textBox_excelPath.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.textBox_excelPath.Location = new System.Drawing.Point(144, 35);
            this.textBox_excelPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_excelPath.Name = "textBox_excelPath";
            this.textBox_excelPath.ReadOnly = true;
            this.textBox_excelPath.Size = new System.Drawing.Size(432, 26);
            this.textBox_excelPath.TabIndex = 45;
            // 
            // button_excelPath
            // 
            this.button_excelPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_excelPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_excelPath.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button_excelPath.Location = new System.Drawing.Point(586, 35);
            this.button_excelPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_excelPath.Name = "button_excelPath";
            this.button_excelPath.Size = new System.Drawing.Size(93, 42);
            this.button_excelPath.TabIndex = 1;
            this.button_excelPath.Text = "Browse";
            this.toolTip1.SetToolTip(this.button_excelPath, "Browse and select Excel file");
            this.button_excelPath.UseVisualStyleBackColor = true;
            this.button_excelPath.Click += new System.EventHandler(this.button_excelPath_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(144, 161);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(433, 28);
            this.progressBar1.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(53, 161);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 20);
            this.label4.TabIndex = 52;
            this.label4.Text = "Progress:";
            // 
            // dataseriesButton
            // 
            this.dataseriesButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dataseriesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataseriesButton.ForeColor = System.Drawing.Color.Black;
            this.dataseriesButton.Location = new System.Drawing.Point(183, 26);
            this.dataseriesButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataseriesButton.Name = "dataseriesButton";
            this.dataseriesButton.Size = new System.Drawing.Size(433, 46);
            this.dataseriesButton.TabIndex = 3;
            this.dataseriesButton.Text = "Export All Data Series";
            this.toolTip1.SetToolTip(this.dataseriesButton, "Export Data Series");
            this.dataseriesButton.UseVisualStyleBackColor = true;
            this.dataseriesButton.Click += new System.EventHandler(this.dataseriesButton_Click);
            // 
            // businessrulesButton
            // 
            this.businessrulesButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.businessrulesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.businessrulesButton.ForeColor = System.Drawing.Color.Black;
            this.businessrulesButton.Location = new System.Drawing.Point(183, 120);
            this.businessrulesButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.businessrulesButton.Name = "businessrulesButton";
            this.businessrulesButton.Size = new System.Drawing.Size(433, 46);
            this.businessrulesButton.TabIndex = 5;
            this.businessrulesButton.Text = "Export All Business Rules";
            this.toolTip1.SetToolTip(this.businessrulesButton, "Export Business Rules");
            this.businessrulesButton.UseVisualStyleBackColor = true;
            this.businessrulesButton.Click += new System.EventHandler(this.businessrulesButton_Click);
            // 
            // importPanel
            // 
            this.importPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.importPanel.Controls.Add(this.textBox_successmsg);
            this.importPanel.Controls.Add(this.label6);
            this.importPanel.Controls.Add(this.label3);
            this.importPanel.Controls.Add(this.label2);
            this.importPanel.Controls.Add(this.comboBox_excelSheet);
            this.importPanel.Controls.Add(this.label_ImportResultPath);
            this.importPanel.Controls.Add(this.button_excelPath);
            this.importPanel.Controls.Add(this.label4);
            this.importPanel.Controls.Add(this.label5);
            this.importPanel.Controls.Add(this.textBox_excelPath);
            this.importPanel.Controls.Add(this.button_importExcel);
            this.importPanel.Controls.Add(this.progressBar1);
            this.importPanel.Controls.Add(this.label1);
            this.importPanel.Controls.Add(this.label_message);
            this.importPanel.Controls.Add(this.label_status);
            this.importPanel.Location = new System.Drawing.Point(14, 269);
            this.importPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.importPanel.Name = "importPanel";
            this.importPanel.Size = new System.Drawing.Size(804, 697);
            this.importPanel.TabIndex = 53;
            this.importPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.importPanel_Paint);
            // 
            // textBox_successmsg
            // 
            this.textBox_successmsg.BackColor = System.Drawing.Color.White;
            this.textBox_successmsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_successmsg.ForeColor = System.Drawing.Color.Black;
            this.textBox_successmsg.Location = new System.Drawing.Point(14, 502);
            this.textBox_successmsg.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_successmsg.Multiline = true;
            this.textBox_successmsg.Name = "textBox_successmsg";
            this.textBox_successmsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_successmsg.Size = new System.Drawing.Size(766, 165);
            this.textBox_successmsg.TabIndex = 78;
            this.textBox_successmsg.Text = "Import Results will appear here.";
            this.textBox_successmsg.Visible = false;
            this.textBox_successmsg.TextChanged += new System.EventHandler(this.textBox_successmsg_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 308);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(349, 20);
            this.label6.TabIndex = 75;
            this.label6.Text = "* This functionality is for existing Data Series.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 278);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(804, 20);
            this.label3.TabIndex = 74;
            this.label3.Text = "* Please use approved upload template to import data. Sample upload template can " +
    "be found at below path.";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(34, 104);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 20);
            this.label2.TabIndex = 72;
            this.label2.Text = "Excel Sheet:";
            // 
            // comboBox_excelSheet
            // 
            this.comboBox_excelSheet.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBox_excelSheet.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBox_excelSheet.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox_excelSheet.DropDownHeight = 120;
            this.comboBox_excelSheet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_excelSheet.Enabled = false;
            this.comboBox_excelSheet.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_excelSheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_excelSheet.FormattingEnabled = true;
            this.comboBox_excelSheet.IntegralHeight = false;
            this.comboBox_excelSheet.ItemHeight = 25;
            this.comboBox_excelSheet.Location = new System.Drawing.Point(145, 98);
            this.comboBox_excelSheet.Margin = new System.Windows.Forms.Padding(6);
            this.comboBox_excelSheet.Name = "comboBox_excelSheet";
            this.comboBox_excelSheet.Size = new System.Drawing.Size(432, 33);
            this.comboBox_excelSheet.TabIndex = 71;
            // 
            // label_ImportResultPath
            // 
            this.label_ImportResultPath.AutoSize = true;
            this.label_ImportResultPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_ImportResultPath.ForeColor = System.Drawing.Color.Black;
            this.label_ImportResultPath.Location = new System.Drawing.Point(9, 339);
            this.label_ImportResultPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_ImportResultPath.Name = "label_ImportResultPath";
            this.label_ImportResultPath.Size = new System.Drawing.Size(339, 29);
            this.label_ImportResultPath.TabIndex = 58;
            this.label_ImportResultPath.Text = "\'D:\\CoSD Tool\\Import Results\\\'";
            this.label_ImportResultPath.Click += new System.EventHandler(this.label_ImportResultPath_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(14, 394);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(433, 29);
            this.label5.TabIndex = 49;
            this.label5.Text = "Import Results are saved at above path.\r\n";
            // 
            // label_message
            // 
            this.label_message.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_message.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label_message.Location = new System.Drawing.Point(14, 501);
            this.label_message.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_message.Name = "label_message";
            this.label_message.Size = new System.Drawing.Size(732, 168);
            this.label_message.TabIndex = 79;
            this.label_message.Text = "Success!";
            this.label_message.Visible = false;
            this.label_message.Click += new System.EventHandler(this.label_message_Click_1);
            // 
            // label_status
            // 
            this.label_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_status.ForeColor = System.Drawing.Color.Black;
            this.label_status.Location = new System.Drawing.Point(14, 445);
            this.label_status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(778, 174);
            this.label_status.TabIndex = 50;
            this.label_status.Text = "Success!";
            // 
            // button_ChangePath
            // 
            this.button_ChangePath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_ChangePath.Location = new System.Drawing.Point(341, 976);
            this.button_ChangePath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_ChangePath.Name = "button_ChangePath";
            this.button_ChangePath.Size = new System.Drawing.Size(116, 35);
            this.button_ChangePath.TabIndex = 70;
            this.button_ChangePath.Text = "Change Path";
            this.button_ChangePath.UseVisualStyleBackColor = true;
            this.button_ChangePath.Click += new System.EventHandler(this.button_ChangePath_Click);
            // 
            // exportPanel
            // 
            this.exportPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exportPanel.Controls.Add(this.businessrulesButton);
            this.exportPanel.Controls.Add(this.exportResultLabel);
            this.exportPanel.Controls.Add(this.exportLabel);
            this.exportPanel.Controls.Add(this.dataseriesButton);
            this.exportPanel.Location = new System.Drawing.Point(854, 269);
            this.exportPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.exportPanel.Name = "exportPanel";
            this.exportPanel.Size = new System.Drawing.Size(802, 697);
            this.exportPanel.TabIndex = 54;
            this.exportPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.exportPanel_Paint);
            // 
            // exportResultLabel
            // 
            this.exportResultLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportResultLabel.ForeColor = System.Drawing.Color.Black;
            this.exportResultLabel.Location = new System.Drawing.Point(12, 339);
            this.exportResultLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.exportResultLabel.Name = "exportResultLabel";
            this.exportResultLabel.Size = new System.Drawing.Size(778, 235);
            this.exportResultLabel.TabIndex = 50;
            this.exportResultLabel.Text = "D:\\CoSD Tool\\Export Results\\";
            // 
            // exportLabel
            // 
            this.exportLabel.AutoSize = true;
            this.exportLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportLabel.Location = new System.Drawing.Point(4, 300);
            this.exportLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.exportLabel.Name = "exportLabel";
            this.exportLabel.Size = new System.Drawing.Size(460, 29);
            this.exportLabel.TabIndex = 49;
            this.exportLabel.Text = "Export Results are saved under this path : ";
            // 
            // exportLabel1
            // 
            this.exportLabel1.AutoSize = true;
            this.exportLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exportLabel1.Location = new System.Drawing.Point(1130, 225);
            this.exportLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.exportLabel1.Name = "exportLabel1";
            this.exportLabel1.Size = new System.Drawing.Size(248, 29);
            this.exportLabel1.TabIndex = 55;
            this.exportLabel1.Text = "Export Data to Excel";
            // 
            // label_Path
            // 
            this.label_Path.AutoSize = true;
            this.label_Path.Location = new System.Drawing.Point(433, 81);
            this.label_Path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Path.Name = "label_Path";
            this.label_Path.Size = new System.Drawing.Size(210, 20);
            this.label_Path.TabIndex = 58;
            this.label_Path.Text = "Home -> Import-Export Data";
            // 
            // label_PageHeading
            // 
            this.label_PageHeading.AutoSize = true;
            this.label_PageHeading.Font = new System.Drawing.Font("Lucida Calligraphy", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PageHeading.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label_PageHeading.Location = new System.Drawing.Point(426, 12);
            this.label_PageHeading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_PageHeading.Name = "label_PageHeading";
            this.label_PageHeading.Size = new System.Drawing.Size(623, 68);
            this.label_PageHeading.TabIndex = 57;
            this.label_PageHeading.Text = "Import-Export Data";
            this.label_PageHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_CoSDPath
            // 
            this.label_CoSDPath.AutoSize = true;
            this.label_CoSDPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_CoSDPath.ForeColor = System.Drawing.Color.Black;
            this.label_CoSDPath.Location = new System.Drawing.Point(15, 980);
            this.label_CoSDPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_CoSDPath.Name = "label_CoSDPath";
            this.label_CoSDPath.Size = new System.Drawing.Size(310, 29);
            this.label_CoSDPath.TabIndex = 73;
            this.label_CoSDPath.Text = "Change CoSD Folder Path: ";
            // 
            // ImportExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1684, 1032);
            this.Controls.Add(this.label_CoSDPath);
            this.Controls.Add(this.label_Path);
            this.Controls.Add(this.button_ChangePath);
            this.Controls.Add(this.label_PageHeading);
            this.Controls.Add(this.exportLabel1);
            this.Controls.Add(this.exportPanel);
            this.Controls.Add(this.importPanel);
            this.Controls.Add(this.importLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_back);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ImportExport";
            this.Text = "Import my Excel sheet into CoSD";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ImportExport_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.importPanel.ResumeLayout(false);
            this.importPanel.PerformLayout();
            this.exportPanel.ResumeLayout(false);
            this.exportPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label importLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.Button button_importExcel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_excelPath;
        private System.Windows.Forms.Button button_excelPath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel importPanel;
        private System.Windows.Forms.Panel exportPanel;
        private System.Windows.Forms.Label exportResultLabel;
        private System.Windows.Forms.Label exportLabel;
        private System.Windows.Forms.Button dataseriesButton;
        private System.Windows.Forms.Label exportLabel1;
        private System.Windows.Forms.Button businessrulesButton;
        private System.Windows.Forms.Label label_Path;
        private System.Windows.Forms.Label label_PageHeading;
        private System.Windows.Forms.Button button_ChangePath;
        private System.Windows.Forms.Label label2;
        private CoSD_Tool.MyCombobox comboBox_excelSheet;
        private System.Windows.Forms.Label label_CoSDPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label_ImportResultPath;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_successmsg;
        private System.Windows.Forms.Label label_message;
    }
}