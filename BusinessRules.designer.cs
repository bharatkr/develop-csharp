namespace CoSD_Tool
{
    partial class BusinessRules
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BusinessRules));
            this.button_back = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_DeleteRules = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.button_next = new System.Windows.Forms.Button();
            this.button_update = new System.Windows.Forms.Button();
            this.button_prev = new System.Windows.Forms.Button();
            this.label_pageNum = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button_reset = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_group1 = new System.Windows.Forms.ComboBox();
            this.comboBox_BusinessType1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_SourceType1 = new System.Windows.Forms.ComboBox();
            this.comboBox_Commodity1 = new System.Windows.Forms.ComboBox();
            this.comboBox_description1 = new System.Windows.Forms.ComboBox();
            this.comboBox1_stattype = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_DSid = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rowcount_label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_retreive = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.longwait_label = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button_executeBL = new System.Windows.Forms.Button();
            this.button_CV = new System.Windows.Forms.Button();
            this.button_TS = new System.Windows.Forms.Button();
            this.label_Path = new System.Windows.Forms.Label();
            this.label_PageHeading = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // button_back
            // 
            this.button_back.BackColor = System.Drawing.Color.LightGray;
            this.button_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_back.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_back.ForeColor = System.Drawing.Color.Black;
            this.button_back.Location = new System.Drawing.Point(1063, 36);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(143, 31);
            this.button_back.TabIndex = 13;
            this.button_back.Text = "Back to Home Page";
            this.toolTip1.SetToolTip(this.button_back, "Back to Home page");
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(27, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(255, 130);
            this.pictureBox1.TabIndex = 75;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_DeleteRules);
            this.groupBox3.Controls.Add(this.dataGridView);
            this.groupBox3.Controls.Add(this.button_next);
            this.groupBox3.Controls.Add(this.button_update);
            this.groupBox3.Controls.Add(this.button_prev);
            this.groupBox3.Controls.Add(this.label_pageNum);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(537, 130);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(784, 493);
            this.groupBox3.TabIndex = 79;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Check Rules to Edit or Execute";
            // 
            // button_DeleteRules
            // 
            this.button_DeleteRules.BackColor = System.Drawing.Color.LightGray;
            this.button_DeleteRules.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_DeleteRules.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.button_DeleteRules.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.button_DeleteRules.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_DeleteRules.ForeColor = System.Drawing.Color.Black;
            this.button_DeleteRules.Location = new System.Drawing.Point(29, 407);
            this.button_DeleteRules.Name = "button_DeleteRules";
            this.button_DeleteRules.Size = new System.Drawing.Size(168, 63);
            this.button_DeleteRules.TabIndex = 10;
            this.button_DeleteRules.Text = "Delete Business Rules";
            this.toolTip1.SetToolTip(this.button_DeleteRules, "Select a row and delete Business Rule");
            this.button_DeleteRules.UseVisualStyleBackColor = true;
            this.button_DeleteRules.Click += new System.EventHandler(this.button_DeleteRules_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(15, 22);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(779, 372);
            this.dataGridView.TabIndex = 82;
            this.dataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            // 
            // button_next
            // 
            this.button_next.BackColor = System.Drawing.Color.Gainsboro;
            this.button_next.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_next.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_next.ForeColor = System.Drawing.Color.Black;
            this.button_next.Location = new System.Drawing.Point(492, 417);
            this.button_next.Name = "button_next";
            this.button_next.Size = new System.Drawing.Size(87, 33);
            this.button_next.TabIndex = 8;
            this.button_next.Text = "Next";
            this.toolTip1.SetToolTip(this.button_next, "Next Page");
            this.button_next.UseVisualStyleBackColor = true;
            this.button_next.Visible = false;
            this.button_next.EnabledChanged += new System.EventHandler(this.button_next_EnabledChanged);
            this.button_next.Click += new System.EventHandler(this.button_next_Click);
            // 
            // button_update
            // 
            this.button_update.BackColor = System.Drawing.Color.LightGray;
            this.button_update.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_update.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_update.ForeColor = System.Drawing.Color.Black;
            this.button_update.Location = new System.Drawing.Point(594, 407);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(168, 63);
            this.button_update.TabIndex = 11;
            this.button_update.Text = "Update Business Rules";
            this.toolTip1.SetToolTip(this.button_update, "Select a row and update Business Rule.");
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            // 
            // button_prev
            // 
            this.button_prev.BackColor = System.Drawing.Color.Gainsboro;
            this.button_prev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_prev.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_prev.ForeColor = System.Drawing.Color.Black;
            this.button_prev.Location = new System.Drawing.Point(213, 417);
            this.button_prev.Name = "button_prev";
            this.button_prev.Size = new System.Drawing.Size(87, 33);
            this.button_prev.TabIndex = 9;
            this.button_prev.Text = "Prev";
            this.toolTip1.SetToolTip(this.button_prev, "Previous Page");
            this.button_prev.UseVisualStyleBackColor = true;
            this.button_prev.Visible = false;
            this.button_prev.EnabledChanged += new System.EventHandler(this.button_prev_EnabledChanged);
            this.button_prev.Click += new System.EventHandler(this.button_prev_Click);
            // 
            // label_pageNum
            // 
            this.label_pageNum.AutoSize = true;
            this.label_pageNum.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label_pageNum.Location = new System.Drawing.Point(375, 420);
            this.label_pageNum.Name = "label_pageNum";
            this.label_pageNum.Size = new System.Drawing.Size(62, 25);
            this.label_pageNum.TabIndex = 4;
            this.label_pageNum.Text = "Page";
            this.label_pageNum.Visible = false;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.LightGray;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(54, 666);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(174, 90);
            this.button1.TabIndex = 7;
            this.button1.Text = "Add/Update Business Rules ";
            this.toolTip1.SetToolTip(this.button1, "Add new Business Rules\r\n");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_reset
            // 
            this.button_reset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.button_reset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button_reset.Location = new System.Drawing.Point(17, 324);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(162, 64);
            this.button_reset.TabIndex = 12;
            this.button_reset.Text = "Reset All Fields";
            this.toolTip1.SetToolTip(this.button_reset, "Resets full page");
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Controls.Add(this.rowcount_label);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.button_retreive);
            this.groupBox2.Controls.Add(this.button_reset);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(14, 160);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Size = new System.Drawing.Size(515, 440);
            this.groupBox2.TabIndex = 78;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Choose Business Rule for Editing or Re-Execution";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.01826F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.98174F));
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_group1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_BusinessType1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label12, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_SourceType1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_Commodity1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_description1, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.comboBox1_stattype, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.textBox_DSid, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.label10, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.label7, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(17, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(487, 273);
            this.tableLayoutPanel1.TabIndex = 86;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(5, 32);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 25);
            this.label6.TabIndex = 26;
            this.label6.Text = "Commodity";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // comboBox_group1
            // 
            this.comboBox_group1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.comboBox_group1.FormattingEnabled = true;
            this.comboBox_group1.Location = new System.Drawing.Point(168, 114);
            this.comboBox_group1.Name = "comboBox_group1";
            this.comboBox_group1.Size = new System.Drawing.Size(284, 33);
            this.comboBox_group1.TabIndex = 24;
            // 
            // comboBox_BusinessType1
            // 
            this.comboBox_BusinessType1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.comboBox_BusinessType1.FormattingEnabled = true;
            this.comboBox_BusinessType1.Location = new System.Drawing.Point(168, 84);
            this.comboBox_BusinessType1.Name = "comboBox_BusinessType1";
            this.comboBox_BusinessType1.Size = new System.Drawing.Size(284, 33);
            this.comboBox_BusinessType1.TabIndex = 23;
            this.comboBox_BusinessType1.SelectedIndexChanged += new System.EventHandler(this.comboBox_BusinessType1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(5, 2);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 25);
            this.label2.TabIndex = 20;
            this.label2.Text = "Source Type";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label12.Location = new System.Drawing.Point(5, 145);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(109, 25);
            this.label12.TabIndex = 10;
            this.label12.Text = "Description";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(5, 81);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 30);
            this.label5.TabIndex = 18;
            this.label5.Text = "Business Logic Type";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(3, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 25);
            this.label4.TabIndex = 12;
            this.label4.Text = "Group";
            // 
            // comboBox_SourceType1
            // 
            this.comboBox_SourceType1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.comboBox_SourceType1.FormattingEnabled = true;
            this.comboBox_SourceType1.Location = new System.Drawing.Point(168, 3);
            this.comboBox_SourceType1.Name = "comboBox_SourceType1";
            this.comboBox_SourceType1.Size = new System.Drawing.Size(284, 33);
            this.comboBox_SourceType1.TabIndex = 22;
            this.comboBox_SourceType1.SelectedIndexChanged += new System.EventHandler(this.comboBox_SourceType1_SelectedIndexChanged);
            // 
            // comboBox_Commodity1
            // 
            this.comboBox_Commodity1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.comboBox_Commodity1.FormattingEnabled = true;
            this.comboBox_Commodity1.Location = new System.Drawing.Point(168, 32);
            this.comboBox_Commodity1.Name = "comboBox_Commodity1";
            this.comboBox_Commodity1.Size = new System.Drawing.Size(284, 33);
            this.comboBox_Commodity1.TabIndex = 27;
            this.comboBox_Commodity1.SelectedIndexChanged += new System.EventHandler(this.comboBox_Commodity1_SelectedIndexChanged);
            // 
            // comboBox_description1
            // 
            this.comboBox_description1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.comboBox_description1.FormattingEnabled = true;
            this.comboBox_description1.Location = new System.Drawing.Point(168, 145);
            this.comboBox_description1.Name = "comboBox_description1";
            this.comboBox_description1.Size = new System.Drawing.Size(284, 33);
            this.comboBox_description1.TabIndex = 25;
            // 
            // comboBox1_stattype
            // 
            this.comboBox1_stattype.BackColor = System.Drawing.SystemColors.HighlightText;
            this.comboBox1_stattype.FormattingEnabled = true;
            this.comboBox1_stattype.Location = new System.Drawing.Point(168, 184);
            this.comboBox1_stattype.Name = "comboBox1_stattype";
            this.comboBox1_stattype.Size = new System.Drawing.Size(284, 33);
            this.comboBox1_stattype.TabIndex = 31;
            this.comboBox1_stattype.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(3, 181);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(130, 25);
            this.label8.TabIndex = 30;
            this.label8.Text = "Statistic Type";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_DSid
            // 
            this.textBox_DSid.Location = new System.Drawing.Point(168, 244);
            this.textBox_DSid.Name = "textBox_DSid";
            this.textBox_DSid.Size = new System.Drawing.Size(284, 30);
            this.textBox_DSid.TabIndex = 32;
            this.textBox_DSid.TextChanged += new System.EventHandler(this.textBox_DSid_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(3, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 25);
            this.label9.TabIndex = 33;
            this.label9.Text = "Data Series";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(304, 221);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 20);
            this.label10.TabIndex = 34;
            this.label10.Text = "OR";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point(330, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(154, 20);
            this.label7.TabIndex = 29;
            this.label7.Text = "*filters output name";
            // 
            // rowcount_label
            // 
            this.rowcount_label.AutoSize = true;
            this.rowcount_label.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rowcount_label.Location = new System.Drawing.Point(193, 388);
            this.rowcount_label.Name = "rowcount_label";
            this.rowcount_label.Size = new System.Drawing.Size(24, 25);
            this.rowcount_label.TabIndex = 83;
            this.rowcount_label.Text = "0";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(-2, 390);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 23);
            this.label1.TabIndex = 82;
            this.label1.Text = "Number of formulas retrieved :";
            // 
            // button_retreive
            // 
            this.button_retreive.BackColor = System.Drawing.Color.LightGray;
            this.button_retreive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_retreive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button_retreive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_retreive.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button_retreive.Location = new System.Drawing.Point(252, 323);
            this.button_retreive.Name = "button_retreive";
            this.button_retreive.Size = new System.Drawing.Size(162, 65);
            this.button_retreive.TabIndex = 5;
            this.button_retreive.Text = "Retreive Business Rules";
            this.toolTip1.SetToolTip(this.button_retreive, "Retrieve Business Rules");
            this.button_retreive.UseVisualStyleBackColor = true;
            this.button_retreive.Click += new System.EventHandler(this.button_retreive_Click_1);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(248, 240);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(247, 20);
            this.label3.TabIndex = 21;
            this.label3.Text = "*With at least one of these word";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(5, 6);
            this.label20.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(103, 20);
            this.label20.TabIndex = 21;
            this.label20.Text = "Commodity";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // longwait_label
            // 
            this.longwait_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.longwait_label.ForeColor = System.Drawing.Color.Black;
            this.longwait_label.Location = new System.Drawing.Point(524, 626);
            this.longwait_label.Name = "longwait_label";
            this.longwait_label.Size = new System.Drawing.Size(715, 118);
            this.longwait_label.TabIndex = 84;
            this.longwait_label.Text = "Business rules execution under progress.\r\nPlease leave this window open and don\'t" +
    " use the tool until you see a success message!";
            this.longwait_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.longwait_label.Click += new System.EventHandler(this.longwait_label_Click);
            // 
            // button_executeBL
            // 
            this.button_executeBL.BackColor = System.Drawing.Color.LightGray;
            this.button_executeBL.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_executeBL.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button_executeBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_executeBL.ForeColor = System.Drawing.Color.Black;
            this.button_executeBL.Location = new System.Drawing.Point(254, 666);
            this.button_executeBL.Name = "button_executeBL";
            this.button_executeBL.Size = new System.Drawing.Size(174, 90);
            this.button_executeBL.TabIndex = 12;
            this.button_executeBL.Text = "Re-execute Selected Business Rules";
            this.toolTip1.SetToolTip(this.button_executeBL, "Re-execute Selected Business Rules");
            this.button_executeBL.UseVisualStyleBackColor = true;
            this.button_executeBL.Click += new System.EventHandler(this.button_executeBL_Click);
            // 
            // button_CV
            // 
            this.button_CV.BackColor = System.Drawing.Color.LightGray;
            this.button_CV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_CV.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_CV.ForeColor = System.Drawing.Color.Black;
            this.button_CV.Location = new System.Drawing.Point(54, 777);
            this.button_CV.Name = "button_CV";
            this.button_CV.Size = new System.Drawing.Size(174, 81);
            this.button_CV.TabIndex = 89;
            this.button_CV.Text = "View Constructed Variables\r\n";
            this.toolTip1.SetToolTip(this.button_CV, "View Constructed Variables");
            this.button_CV.UseVisualStyleBackColor = true;
            this.button_CV.Click += new System.EventHandler(this.button_CV_Click);
            // 
            // button_TS
            // 
            this.button_TS.BackColor = System.Drawing.Color.LightGray;
            this.button_TS.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_TS.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_TS.ForeColor = System.Drawing.Color.Black;
            this.button_TS.Location = new System.Drawing.Point(254, 777);
            this.button_TS.Name = "button_TS";
            this.button_TS.Size = new System.Drawing.Size(174, 81);
            this.button_TS.TabIndex = 90;
            this.button_TS.Text = "View Business Rules Tree Structure";
            this.toolTip1.SetToolTip(this.button_TS, "View Constructed Variables");
            this.button_TS.UseVisualStyleBackColor = true;
            this.button_TS.Click += new System.EventHandler(this.button_TS_Click);
            // 
            // label_Path
            // 
            this.label_Path.AutoSize = true;
            this.label_Path.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Path.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label_Path.Location = new System.Drawing.Point(522, 64);
            this.label_Path.Name = "label_Path";
            this.label_Path.Size = new System.Drawing.Size(196, 20);
            this.label_Path.TabIndex = 87;
            this.label_Path.Text = "Home -> Business Logic\r\n";
            // 
            // label_PageHeading
            // 
            this.label_PageHeading.AutoSize = true;
            this.label_PageHeading.Font = new System.Drawing.Font("Lucida Calligraphy", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PageHeading.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label_PageHeading.Location = new System.Drawing.Point(517, 19);
            this.label_PageHeading.Name = "label_PageHeading";
            this.label_PageHeading.Size = new System.Drawing.Size(447, 68);
            this.label_PageHeading.TabIndex = 86;
            this.label_PageHeading.Text = "Business Logic";
            this.label_PageHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.button2.Location = new System.Drawing.Point(1063, 83);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(145, 29);
            this.button2.TabIndex = 91;
            this.button2.Text = "View R Results";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.ErrorImage")));
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.InitialImage")));
            this.pictureBox2.Location = new System.Drawing.Point(1023, 83);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(34, 29);
            this.pictureBox2.TabIndex = 92;
            this.pictureBox2.TabStop = false;
            // 
            // BusinessRules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1368, 870);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_TS);
            this.Controls.Add(this.button_CV);
            this.Controls.Add(this.button_executeBL);
            this.Controls.Add(this.label_Path);
            this.Controls.Add(this.label_PageHeading);
            this.Controls.Add(this.longwait_label);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BusinessRules";
            this.Text = "Edit and Execute Business Rules";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.BusinessRules_Load);
            this.Click += new System.EventHandler(this.BusinessRules_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //Newly included...//
        //private AP_SADb_Tool.BusinessRules commodities;
        //Newly included...//   
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_next;
        private System.Windows.Forms.Button button_prev;
        private System.Windows.Forms.Label label_pageNum;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Label label5;
        //private MyCombobox comboBox_BusinessType;
        //private MyCombobox comboBox_group;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button button_retreive;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label rowcount_label;
        private System.Windows.Forms.Button button1;
        //private MyCombobox comboBox_description;
        private System.Windows.Forms.Button button_DeleteRules;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        //private MyCombobox comboBox_SourceType;
        private System.Windows.Forms.Label longwait_label;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label_Path;
        private System.Windows.Forms.Label label_PageHeading;
        private System.Windows.Forms.Button button_executeBL;
        private System.Windows.Forms.Button button_CV;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label20;
        //private MyCombobox comboBox_Commodity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_description1;
        private System.Windows.Forms.ComboBox comboBox_group1;
        private System.Windows.Forms.ComboBox comboBox_BusinessType1;
        private System.Windows.Forms.ComboBox comboBox_SourceType1;
        private System.Windows.Forms.ComboBox comboBox_Commodity1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_TS;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox1_stattype;
        private System.Windows.Forms.TextBox textBox_DSid;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}