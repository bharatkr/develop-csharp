namespace CoSD_Tool
{
    partial class ConversionFactorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConversionFactorForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_back = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_next = new System.Windows.Forms.Button();
            this.button_prev = new System.Windows.Forms.Button();
            this.label_pageNum = new System.Windows.Forms.Label();
            this.button_updateconversion = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_reset = new System.Windows.Forms.Button();
            this.button_addconversion = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.Combobox_Group = new CoSD_Tool.MyCombobox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_commodity = new CoSD_Tool.MyCombobox();
            this.button_retrieveconversion = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox_addconversion = new System.Windows.Forms.GroupBox();
            this.button_submit = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.waterMarkTextBox_EndYear = new CoSD_Tool.WaterMarkTextBox();
            this.waterMarkTextBox_StartYear = new CoSD_Tool.WaterMarkTextBox();
            this.comboBox_commodityForAdd = new CoSD_Tool.MyCombobox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_source = new CoSD_Tool.WaterMarkTextBox();
            this.waterMarkTextBox_marketsegment = new CoSD_Tool.WaterMarkTextBox();
            this.waterMarkTextBox_conversionvalue = new CoSD_Tool.WaterMarkTextBox();
            this.waterMarkTextBox_description = new CoSD_Tool.WaterMarkTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.errorProviderCF = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label_Path = new System.Windows.Forms.Label();
            this.label_PageHeading = new System.Windows.Forms.Label();
            this.label_Required = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox_addconversion.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderCF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(18, 25);
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
            this.button_back.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_back.ForeColor = System.Drawing.Color.Black;
            this.button_back.Location = new System.Drawing.Point(1680, 79);
            this.button_back.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(202, 35);
            this.button_back.TabIndex = 35;
            this.button_back.Text = "Back to Home Page";
            this.toolTip1.SetToolTip(this.button_back, "Back to Home page");
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_next);
            this.groupBox1.Controls.Add(this.button_prev);
            this.groupBox1.Controls.Add(this.label_pageNum);
            this.groupBox1.Controls.Add(this.button_updateconversion);
            this.groupBox1.Controls.Add(this.dataGridView);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(741, 205);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(1142, 775);
            this.groupBox1.TabIndex = 82;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conversion Factor Data";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // button_next
            // 
            this.button_next.BackColor = System.Drawing.Color.Linen;
            this.button_next.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_next.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button_next.Location = new System.Drawing.Point(676, 671);
            this.button_next.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_next.Name = "button_next";
            this.button_next.Size = new System.Drawing.Size(130, 35);
            this.button_next.TabIndex = 17;
            this.button_next.Text = "Next";
            this.toolTip1.SetToolTip(this.button_next, "Next Page");
            this.button_next.UseVisualStyleBackColor = false;
            this.button_next.Visible = false;
            this.button_next.EnabledChanged += new System.EventHandler(this.button_next_EnabledChanged);
            this.button_next.Click += new System.EventHandler(this.button_next_Click_1);
            // 
            // button_prev
            // 
            this.button_prev.BackColor = System.Drawing.Color.Linen;
            this.button_prev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_prev.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button_prev.Location = new System.Drawing.Point(324, 671);
            this.button_prev.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_prev.Name = "button_prev";
            this.button_prev.Size = new System.Drawing.Size(130, 35);
            this.button_prev.TabIndex = 16;
            this.button_prev.Text = "Prev";
            this.toolTip1.SetToolTip(this.button_prev, "Previous Page");
            this.button_prev.UseVisualStyleBackColor = false;
            this.button_prev.Visible = false;
            this.button_prev.EnabledChanged += new System.EventHandler(this.button_prev_EnabledChanged);
            this.button_prev.Click += new System.EventHandler(this.button_prev_Click_1);
            // 
            // label_pageNum
            // 
            this.label_pageNum.AutoSize = true;
            this.label_pageNum.Location = new System.Drawing.Point(512, 674);
            this.label_pageNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_pageNum.Name = "label_pageNum";
            this.label_pageNum.Size = new System.Drawing.Size(62, 25);
            this.label_pageNum.TabIndex = 15;
            this.label_pageNum.Text = "Page";
            this.label_pageNum.Visible = false;
            // 
            // button_updateconversion
            // 
            this.button_updateconversion.BackColor = System.Drawing.Color.LightGray;
            this.button_updateconversion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_updateconversion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_updateconversion.ForeColor = System.Drawing.Color.MediumBlue;
            this.button_updateconversion.Location = new System.Drawing.Point(852, 639);
            this.button_updateconversion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_updateconversion.Name = "button_updateconversion";
            this.button_updateconversion.Size = new System.Drawing.Size(252, 98);
            this.button_updateconversion.TabIndex = 14;
            this.button_updateconversion.Text = "Update conversion factor values ";
            this.toolTip1.SetToolTip(this.button_updateconversion, "Select a row and update conversion factor values");
            this.button_updateconversion.UseVisualStyleBackColor = true;
            this.button_updateconversion.Click += new System.EventHandler(this.button_update_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(22, 29);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(1098, 575);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_reset);
            this.groupBox3.Controls.Add(this.button_addconversion);
            this.groupBox3.Controls.Add(this.tableLayoutPanel1);
            this.groupBox3.Controls.Add(this.button_retrieveconversion);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(18, 205);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox3.Size = new System.Drawing.Size(685, 254);
            this.groupBox3.TabIndex = 84;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Choose conversion factor values to add or edit";
            // 
            // button_reset
            // 
            this.button_reset.BackColor = System.Drawing.Color.LightGray;
            this.button_reset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_reset.Location = new System.Drawing.Point(26, 152);
            this.button_reset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(168, 86);
            this.button_reset.TabIndex = 2;
            this.button_reset.Text = "Reset All Fields";
            this.toolTip1.SetToolTip(this.button_reset, "Resets full page");
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_addconversion
            // 
            this.button_addconversion.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_addconversion.BackColor = System.Drawing.Color.LightGray;
            this.button_addconversion.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.button_addconversion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_addconversion.ForeColor = System.Drawing.Color.Black;
            this.button_addconversion.Location = new System.Drawing.Point(202, 152);
            this.button_addconversion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_addconversion.Name = "button_addconversion";
            this.button_addconversion.Size = new System.Drawing.Size(233, 86);
            this.button_addconversion.TabIndex = 85;
            this.button_addconversion.Text = "Add new conversion factor values";
            this.toolTip1.SetToolTip(this.button_addconversion, "Add new conversion factor values");
            this.button_addconversion.UseVisualStyleBackColor = true;
            this.button_addconversion.Click += new System.EventHandler(this.button_addconversion_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.63366F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.36634F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Combobox_Group, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_commodity, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(28, 34);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(606, 109);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(4, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 25);
            this.label3.TabIndex = 98;
            this.label3.Text = "Group*";
            // 
            // Combobox_Group
            // 
            this.Combobox_Group.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Combobox_Group.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.Combobox_Group.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.Combobox_Group.DropDownHeight = 120;
            this.Combobox_Group.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Combobox_Group.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Combobox_Group.FormattingEnabled = true;
            this.Combobox_Group.IntegralHeight = false;
            this.Combobox_Group.ItemHeight = 25;
            this.Combobox_Group.Items.AddRange(new object[] {
            "Mapping",
            "Exclusion"});
            this.Combobox_Group.Location = new System.Drawing.Point(229, 10);
            this.Combobox_Group.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Combobox_Group.Name = "Combobox_Group";
            this.Combobox_Group.Size = new System.Drawing.Size(368, 33);
            this.Combobox_Group.TabIndex = 1;
            this.Combobox_Group.SelectionChangeCommitted += new System.EventHandler(this.Combobox_Group_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 50);
            this.label2.TabIndex = 8;
            this.label2.Text = "Commodity, Subcommodity";
            // 
            // comboBox_commodity
            // 
            this.comboBox_commodity.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBox_commodity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBox_commodity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox_commodity.Enabled = false;
            this.comboBox_commodity.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_commodity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_commodity.FormattingEnabled = true;
            this.comboBox_commodity.IntegralHeight = false;
            this.comboBox_commodity.ItemHeight = 25;
            this.comboBox_commodity.Location = new System.Drawing.Point(228, 65);
            this.comboBox_commodity.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBox_commodity.Name = "comboBox_commodity";
            this.comboBox_commodity.Size = new System.Drawing.Size(370, 33);
            this.comboBox_commodity.TabIndex = 2;
            this.comboBox_commodity.SelectedIndexChanged += new System.EventHandler(this.comboBox_commodity_SelectedIndexChanged);
            // 
            // button_retrieveconversion
            // 
            this.button_retrieveconversion.BackColor = System.Drawing.Color.LightGray;
            this.button_retrieveconversion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_retrieveconversion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_retrieveconversion.Location = new System.Drawing.Point(440, 152);
            this.button_retrieveconversion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_retrieveconversion.Name = "button_retrieveconversion";
            this.button_retrieveconversion.Size = new System.Drawing.Size(195, 86);
            this.button_retrieveconversion.TabIndex = 2;
            this.button_retrieveconversion.Text = "Retrieve Conversion Values";
            this.toolTip1.SetToolTip(this.button_retrieveconversion, "Retrieve Conversion Factor values");
            this.button_retrieveconversion.UseVisualStyleBackColor = true;
            this.button_retrieveconversion.Click += new System.EventHandler(this.button_retrieveconversion_Click);
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(4, 111);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 25);
            this.label13.TabIndex = 6;
            this.label13.Text = "End Year ";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 62);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 25);
            this.label6.TabIndex = 1;
            this.label6.Text = "Start Year ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 209);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "Source";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 160);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(156, 25);
            this.label4.TabIndex = 12;
            this.label4.Text = "Market Segment";
            // 
            // groupBox_addconversion
            // 
            this.groupBox_addconversion.Controls.Add(this.button_submit);
            this.groupBox_addconversion.Controls.Add(this.label5);
            this.groupBox_addconversion.Controls.Add(this.tableLayoutPanel2);
            this.groupBox_addconversion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_addconversion.Location = new System.Drawing.Point(18, 471);
            this.groupBox_addconversion.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox_addconversion.Name = "groupBox_addconversion";
            this.groupBox_addconversion.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox_addconversion.Size = new System.Drawing.Size(685, 509);
            this.groupBox_addconversion.TabIndex = 96;
            this.groupBox_addconversion.TabStop = false;
            this.groupBox_addconversion.Text = "To add new conversion factor values ";
            // 
            // button_submit
            // 
            this.button_submit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_submit.BackColor = System.Drawing.Color.LightGray;
            this.button_submit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_submit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_submit.ForeColor = System.Drawing.Color.Black;
            this.button_submit.Location = new System.Drawing.Point(468, 408);
            this.button_submit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_submit.Name = "button_submit";
            this.button_submit.Size = new System.Drawing.Size(148, 68);
            this.button_submit.TabIndex = 87;
            this.button_submit.Text = "Submit";
            this.toolTip1.SetToolTip(this.button_submit, "Submit new conversion factor values");
            this.button_submit.UseVisualStyleBackColor = true;
            this.button_submit.Click += new System.EventHandler(this.button_submit_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(14, 405);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(300, 86);
            this.label5.TabIndex = 86;
            this.label5.Text = "Please select commodity, start year and end year to add new conversion factor val" +
    "ues.";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.63366F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.36634F));
            this.tableLayoutPanel2.Controls.Add(this.waterMarkTextBox_EndYear, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.waterMarkTextBox_StartYear, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.comboBox_commodityForAdd, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.textBox_source, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.waterMarkTextBox_marketsegment, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.waterMarkTextBox_conversionvalue, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.waterMarkTextBox_description, 1, 6);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(18, 34);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(606, 352);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // waterMarkTextBox_EndYear
            // 
            this.waterMarkTextBox_EndYear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.waterMarkTextBox_EndYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waterMarkTextBox_EndYear.Location = new System.Drawing.Point(225, 104);
            this.waterMarkTextBox_EndYear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.waterMarkTextBox_EndYear.Multiline = true;
            this.waterMarkTextBox_EndYear.Name = "waterMarkTextBox_EndYear";
            this.waterMarkTextBox_EndYear.Size = new System.Drawing.Size(374, 39);
            this.waterMarkTextBox_EndYear.TabIndex = 5;
            this.waterMarkTextBox_EndYear.WaterMarkColor = System.Drawing.Color.Gray;
            this.waterMarkTextBox_EndYear.WaterMarkText = "<Enter End Year in YYYY>";
            this.waterMarkTextBox_EndYear.TextChanged += new System.EventHandler(this.waterMarkTextBox_EndYear_TextChanged);
            // 
            // waterMarkTextBox_StartYear
            // 
            this.waterMarkTextBox_StartYear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.waterMarkTextBox_StartYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waterMarkTextBox_StartYear.Location = new System.Drawing.Point(225, 55);
            this.waterMarkTextBox_StartYear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.waterMarkTextBox_StartYear.Multiline = true;
            this.waterMarkTextBox_StartYear.Name = "waterMarkTextBox_StartYear";
            this.waterMarkTextBox_StartYear.Size = new System.Drawing.Size(374, 39);
            this.waterMarkTextBox_StartYear.TabIndex = 4;
            this.waterMarkTextBox_StartYear.WaterMarkColor = System.Drawing.Color.Gray;
            this.waterMarkTextBox_StartYear.WaterMarkText = "<Enter Start Year in YYYY>";
            this.waterMarkTextBox_StartYear.TextChanged += new System.EventHandler(this.waterMarkTextBox_StartYear_TextChanged);
            // 
            // comboBox_commodityForAdd
            // 
            this.comboBox_commodityForAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBox_commodityForAdd.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBox_commodityForAdd.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox_commodityForAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBox_commodityForAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_commodityForAdd.FormattingEnabled = true;
            this.comboBox_commodityForAdd.IntegralHeight = false;
            this.comboBox_commodityForAdd.ItemHeight = 25;
            this.comboBox_commodityForAdd.Location = new System.Drawing.Point(228, 8);
            this.comboBox_commodityForAdd.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.comboBox_commodityForAdd.Name = "comboBox_commodityForAdd";
            this.comboBox_commodityForAdd.Size = new System.Drawing.Size(370, 33);
            this.comboBox_commodityForAdd.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(4, 0);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(142, 50);
            this.label9.TabIndex = 99;
            this.label9.Text = "Commodity, Subcommodity";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(4, 311);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(109, 25);
            this.label8.TabIndex = 98;
            this.label8.Text = "Description";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(4, 258);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(168, 25);
            this.label7.TabIndex = 96;
            this.label7.Text = "Conversion Value";
            // 
            // textBox_source
            // 
            this.textBox_source.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox_source.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_source.Location = new System.Drawing.Point(225, 202);
            this.textBox_source.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_source.Multiline = true;
            this.textBox_source.Name = "textBox_source";
            this.textBox_source.Size = new System.Drawing.Size(374, 39);
            this.textBox_source.TabIndex = 7;
            this.textBox_source.WaterMarkColor = System.Drawing.Color.Gray;
            this.textBox_source.WaterMarkText = "<Enter source for conversion values>";
            // 
            // waterMarkTextBox_marketsegment
            // 
            this.waterMarkTextBox_marketsegment.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.waterMarkTextBox_marketsegment.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waterMarkTextBox_marketsegment.Location = new System.Drawing.Point(225, 153);
            this.waterMarkTextBox_marketsegment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.waterMarkTextBox_marketsegment.Multiline = true;
            this.waterMarkTextBox_marketsegment.Name = "waterMarkTextBox_marketsegment";
            this.waterMarkTextBox_marketsegment.Size = new System.Drawing.Size(374, 39);
            this.waterMarkTextBox_marketsegment.TabIndex = 6;
            this.waterMarkTextBox_marketsegment.WaterMarkColor = System.Drawing.Color.Gray;
            this.waterMarkTextBox_marketsegment.WaterMarkText = "<Enter market segement values>";
            // 
            // waterMarkTextBox_conversionvalue
            // 
            this.waterMarkTextBox_conversionvalue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.waterMarkTextBox_conversionvalue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waterMarkTextBox_conversionvalue.Location = new System.Drawing.Point(225, 251);
            this.waterMarkTextBox_conversionvalue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.waterMarkTextBox_conversionvalue.Multiline = true;
            this.waterMarkTextBox_conversionvalue.Name = "waterMarkTextBox_conversionvalue";
            this.waterMarkTextBox_conversionvalue.Size = new System.Drawing.Size(374, 39);
            this.waterMarkTextBox_conversionvalue.TabIndex = 8;
            this.waterMarkTextBox_conversionvalue.WaterMarkColor = System.Drawing.Color.Gray;
            this.waterMarkTextBox_conversionvalue.WaterMarkText = "<Enter conversion values>";
            this.waterMarkTextBox_conversionvalue.TextChanged += new System.EventHandler(this.waterMarkTextBox_conversionvalue_TextChanged);
            // 
            // waterMarkTextBox_description
            // 
            this.waterMarkTextBox_description.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.waterMarkTextBox_description.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.waterMarkTextBox_description.Location = new System.Drawing.Point(225, 305);
            this.waterMarkTextBox_description.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.waterMarkTextBox_description.Multiline = true;
            this.waterMarkTextBox_description.Name = "waterMarkTextBox_description";
            this.waterMarkTextBox_description.Size = new System.Drawing.Size(374, 36);
            this.waterMarkTextBox_description.TabIndex = 9;
            this.waterMarkTextBox_description.WaterMarkColor = System.Drawing.Color.Gray;
            this.waterMarkTextBox_description.WaterMarkText = "<Enter description>";
            // 
            // errorProviderCF
            // 
            this.errorProviderCF.ContainerControl = this;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label_Path
            // 
            this.label_Path.AutoSize = true;
            this.label_Path.Location = new System.Drawing.Point(737, 94);
            this.label_Path.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Path.Name = "label_Path";
            this.label_Path.Size = new System.Drawing.Size(211, 20);
            this.label_Path.TabIndex = 90;
            this.label_Path.Text = "Home -> Conversion Factors\r\n";
            this.label_Path.Click += new System.EventHandler(this.label_Path_Click);
            // 
            // label_PageHeading
            // 
            this.label_PageHeading.AutoSize = true;
            this.label_PageHeading.Font = new System.Drawing.Font("Lucida Calligraphy", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PageHeading.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label_PageHeading.Location = new System.Drawing.Point(723, 25);
            this.label_PageHeading.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_PageHeading.Name = "label_PageHeading";
            this.label_PageHeading.Size = new System.Drawing.Size(593, 68);
            this.label_PageHeading.TabIndex = 89;
            this.label_PageHeading.Text = "Conversion Factors";
            this.label_PageHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_PageHeading.Click += new System.EventHandler(this.label_PageHeading_Click);
            // 
            // label_Required
            // 
            this.label_Required.AutoSize = true;
            this.label_Required.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Required.Location = new System.Drawing.Point(613, 192);
            this.label_Required.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_Required.Name = "label_Required";
            this.label_Required.Size = new System.Drawing.Size(97, 20);
            this.label_Required.TabIndex = 97;
            this.label_Required.Text = "[* Required]";
            // 
            // ConversionFactorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1923, 1021);
            this.Controls.Add(this.label_Required);
            this.Controls.Add(this.label_Path);
            this.Controls.Add(this.groupBox_addconversion);
            this.Controls.Add(this.label_PageHeading);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ConversionFactorForm";
            this.Text = "Edit and Insert New Conversion Factor Values";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.KnowledgeBaseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox_addconversion.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderCF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_updateconversion;
        private System.Windows.Forms.Button button_retrieveconversion;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private MyCombobox comboBox_commodity;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button button_next;
        private System.Windows.Forms.Button button_prev;
        private System.Windows.Forms.Label label_pageNum;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private WaterMarkTextBox textBox_source;
        private WaterMarkTextBox waterMarkTextBox_marketsegment;
        private System.Windows.Forms.GroupBox groupBox_addconversion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button_addconversion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_submit;
        private System.Windows.Forms.Label label7;
        private WaterMarkTextBox waterMarkTextBox_conversionvalue;
        private System.Windows.Forms.ToolTip toolTip1;
        private WaterMarkTextBox waterMarkTextBox_description;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ErrorProvider errorProviderCF;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label_Path;
        private System.Windows.Forms.Label label_PageHeading;
        private System.Windows.Forms.Label label_Required;
        private System.Windows.Forms.Label label3;
        private MyCombobox Combobox_Group;
        private MyCombobox comboBox_commodityForAdd;
        private System.Windows.Forms.Label label9;
        private WaterMarkTextBox waterMarkTextBox_EndYear;
        private WaterMarkTextBox waterMarkTextBox_StartYear;

    }
}