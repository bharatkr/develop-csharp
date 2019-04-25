namespace CoSD_Tool
{
    partial class HelpFormula
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpFormula));
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_back = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numericInputsCount = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.Select_label = new System.Windows.Forms.Label();
            this.button_generateformula = new System.Windows.Forms.Button();
            this.button_ADD = new System.Windows.Forms.Button();
            this.button_reset = new System.Windows.Forms.Button();
            this.label_inputsselected = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.waterMarkTextBox_sampleformula = new CoSD_Tool.WaterMarkTextBox();
            this.label_sampleformula = new System.Windows.Forms.Label();
            this.combobox_stattype = new CoSD_Tool.MyCombobox();
            this.combobox_dataseries = new CoSD_Tool.MyCombobox();
            this.textBox_Inputsdisplay = new CoSD_Tool.WaterMarkTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericInputsCount)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 180);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(375, 25);
            this.label2.TabIndex = 40;
            this.label2.Text = "Use this window to build your formula:";
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
            this.button_back.Location = new System.Drawing.Point(1096, 74);
            this.button_back.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(174, 35);
            this.button_back.TabIndex = 9;
            this.button_back.Text = "Back to Home Page";
            this.toolTip1.SetToolTip(this.button_back, "Back to Home page");
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 49);
            this.label1.TabIndex = 44;
            this.label1.Text = "Select Number of Inputs to the formula:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(4, 238);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(242, 32);
            this.label4.TabIndex = 52;
            this.label4.Text = "Commodity DataseriesID:";
            // 
            // numericInputsCount
            // 
            this.numericInputsCount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericInputsCount.Location = new System.Drawing.Point(258, 22);
            this.numericInputsCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.numericInputsCount.Name = "numericInputsCount";
            this.numericInputsCount.Size = new System.Drawing.Size(246, 26);
            this.numericInputsCount.TabIndex = 1;
            this.numericInputsCount.ValueChanged += new System.EventHandler(this.numericInputsCount_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(4, 188);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 25);
            this.label5.TabIndex = 54;
            this.label5.Text = "Statistic Type:";
            // 
            // Select_label
            // 
            this.Select_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Select_label.ForeColor = System.Drawing.Color.Black;
            this.Select_label.Location = new System.Drawing.Point(576, 388);
            this.Select_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Select_label.Name = "Select_label";
            this.Select_label.Size = new System.Drawing.Size(237, 80);
            this.Select_label.TabIndex = 85;
            this.Select_label.Text = "Please Select Statistic Type and Dataseries ID for Input : 1";
            this.Select_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_generateformula
            // 
            this.button_generateformula.BackColor = System.Drawing.Color.LightGray;
            this.button_generateformula.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_generateformula.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_generateformula.Location = new System.Drawing.Point(232, 574);
            this.button_generateformula.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_generateformula.Name = "button_generateformula";
            this.button_generateformula.Size = new System.Drawing.Size(190, 63);
            this.button_generateformula.TabIndex = 7;
            this.button_generateformula.Text = "Generate formula";
            this.toolTip1.SetToolTip(this.button_generateformula, "Generate formula");
            this.button_generateformula.UseVisualStyleBackColor = true;
            this.button_generateformula.Click += new System.EventHandler(this.button_generateformula_Click);
            // 
            // button_ADD
            // 
            this.button_ADD.BackColor = System.Drawing.Color.LightGray;
            this.button_ADD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_ADD.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ADD.Location = new System.Drawing.Point(384, 494);
            this.button_ADD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_ADD.Name = "button_ADD";
            this.button_ADD.Size = new System.Drawing.Size(142, 43);
            this.button_ADD.TabIndex = 6;
            this.button_ADD.Text = "Add inputs";
            this.toolTip1.SetToolTip(this.button_ADD, "Add input to build formula");
            this.button_ADD.UseVisualStyleBackColor = true;
            this.button_ADD.Click += new System.EventHandler(this.button_ADD_Click);
            // 
            // button_reset
            // 
            this.button_reset.BackColor = System.Drawing.Color.LightGray;
            this.button_reset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_reset.Location = new System.Drawing.Point(100, 494);
            this.button_reset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(168, 42);
            this.button_reset.TabIndex = 8;
            this.button_reset.Text = "Reset all fields";
            this.toolTip1.SetToolTip(this.button_reset, "Resets full page");
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // label_inputsselected
            // 
            this.label_inputsselected.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_inputsselected.AutoSize = true;
            this.label_inputsselected.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_inputsselected.Location = new System.Drawing.Point(4, 82);
            this.label_inputsselected.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_inputsselected.Name = "label_inputsselected";
            this.label_inputsselected.Size = new System.Drawing.Size(153, 25);
            this.label_inputsselected.TabIndex = 130;
            this.label_inputsselected.Text = "Input\'s selected:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.14493F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.85507F));
            this.tableLayoutPanel1.Controls.Add(this.waterMarkTextBox_sampleformula, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_sampleformula, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.combobox_stattype, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.combobox_dataseries, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericInputsCount, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label_inputsselected, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox_Inputsdisplay, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(22, 205);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.13953F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.86047F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(508, 280);
            this.tableLayoutPanel1.TabIndex = 132;
            // 
            // waterMarkTextBox_sampleformula
            // 
            this.waterMarkTextBox_sampleformula.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.waterMarkTextBox_sampleformula.Location = new System.Drawing.Point(258, 133);
            this.waterMarkTextBox_sampleformula.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.waterMarkTextBox_sampleformula.Name = "waterMarkTextBox_sampleformula";
            this.waterMarkTextBox_sampleformula.Size = new System.Drawing.Size(244, 26);
            this.waterMarkTextBox_sampleformula.TabIndex = 3;
            this.waterMarkTextBox_sampleformula.WaterMarkColor = System.Drawing.Color.Gray;
            this.waterMarkTextBox_sampleformula.WaterMarkText = "<sample formula>";
            // 
            // label_sampleformula
            // 
            this.label_sampleformula.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_sampleformula.AutoSize = true;
            this.label_sampleformula.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_sampleformula.Location = new System.Drawing.Point(4, 133);
            this.label_sampleformula.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_sampleformula.Name = "label_sampleformula";
            this.label_sampleformula.Size = new System.Drawing.Size(154, 25);
            this.label_sampleformula.TabIndex = 136;
            this.label_sampleformula.Text = "Sample formula:";
            // 
            // combobox_stattype
            // 
            this.combobox_stattype.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.combobox_stattype.DropDownHeight = 120;
            this.combobox_stattype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combobox_stattype.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.combobox_stattype.FormattingEnabled = true;
            this.combobox_stattype.IntegralHeight = false;
            this.combobox_stattype.Location = new System.Drawing.Point(259, 186);
            this.combobox_stattype.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.combobox_stattype.Name = "combobox_stattype";
            this.combobox_stattype.Size = new System.Drawing.Size(244, 28);
            this.combobox_stattype.TabIndex = 4;
            // 
            // combobox_dataseries
            // 
            this.combobox_dataseries.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.combobox_dataseries.DropDownHeight = 120;
            this.combobox_dataseries.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.combobox_dataseries.FormattingEnabled = true;
            this.combobox_dataseries.IntegralHeight = false;
            this.combobox_dataseries.Location = new System.Drawing.Point(259, 240);
            this.combobox_dataseries.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.combobox_dataseries.Name = "combobox_dataseries";
            this.combobox_dataseries.Size = new System.Drawing.Size(244, 28);
            this.combobox_dataseries.TabIndex = 5;
            // 
            // textBox_Inputsdisplay
            // 
            this.textBox_Inputsdisplay.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox_Inputsdisplay.Location = new System.Drawing.Point(258, 82);
            this.textBox_Inputsdisplay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_Inputsdisplay.Name = "textBox_Inputsdisplay";
            this.textBox_Inputsdisplay.Size = new System.Drawing.Size(244, 26);
            this.textBox_Inputsdisplay.TabIndex = 2;
            this.textBox_Inputsdisplay.WaterMarkColor = System.Drawing.Color.Gray;
            this.textBox_Inputsdisplay.WaterMarkText = "<inputs>";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(42, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(331, 25);
            this.label6.TabIndex = 134;
            this.label6.Text = "Instructions to build your formula:";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(402, 0);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(194, 28);
            this.label13.TabIndex = 143;
            this.label13.Text = "**Example formula**";
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(368, 28);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(420, 51);
            this.label16.TabIndex = 144;
            this.label16.Text = "Boneless retail per capita of pork multiplied by a conversion factor for all quar" +
    "ters in 2015";
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Black;
            this.label17.Location = new System.Drawing.Point(210, 28);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(148, 28);
            this.label17.TabIndex = 145;
            this.label17.Text = "Description :";
            this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(232, 97);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(126, 28);
            this.label19.TabIndex = 155;
            this.label19.Text = "Formula :";
            this.label19.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(368, 97);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(369, 28);
            this.label20.TabIndex = 156;
            this.label20.Text = "Boneless retail per capita (430) * conv";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(48, 88);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(411, 78);
            this.label15.TabIndex = 141;
            this.label15.Text = "Choose the variable from  the  statistic  type dropdown list (if your desired inp" +
    "ut is   not  in the list, please contact your data coordinator)";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(48, 25);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(411, 52);
            this.label8.TabIndex = 45;
            this.label8.Text = "Please choose the number of variables (inputs) in your formula";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.Black;
            this.label26.Location = new System.Drawing.Point(9, 88);
            this.label26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(36, 28);
            this.label26.TabIndex = 154;
            this.label26.Text = "2.)";
            this.label26.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Black;
            this.label18.Location = new System.Drawing.Point(9, 25);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(36, 28);
            this.label18.TabIndex = 146;
            this.label18.Text = "1.)";
            this.label18.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(54, 288);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(358, 54);
            this.label10.TabIndex = 136;
            this.label10.Text = "If your formula has a conversion factor, please use the keyword \'conv\' for that v" +
    "ariable in the formula";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(54, 348);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(390, 75);
            this.label9.TabIndex = 135;
            this.label9.Text = "If your formula has a macro-econ variable, please use the keyword  \'macro\'  for  " +
    " that variable in the formula";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(54, 226);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(380, 54);
            this.label11.TabIndex = 137;
            this.label11.Text = "Modify your auto-generated formula in the \'Sample Formula\' field";
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(54, 166);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(380, 54);
            this.label14.TabIndex = 140;
            this.label14.Text = "For each input choose the data series ID associated with it";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(54, 429);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(404, 54);
            this.label12.TabIndex = 142;
            this.label12.Text = "When you are done, click on \'Generate formula\'";
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.Black;
            this.label25.Location = new System.Drawing.Point(9, 166);
            this.label25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(36, 28);
            this.label25.TabIndex = 153;
            this.label25.Text = "3.)";
            this.label25.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(9, 226);
            this.label24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(36, 28);
            this.label24.TabIndex = 152;
            this.label24.Text = "4.)";
            this.label24.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.Location = new System.Drawing.Point(9, 288);
            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(36, 28);
            this.label23.TabIndex = 151;
            this.label23.Text = "5.)";
            this.label23.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Black;
            this.label22.Location = new System.Drawing.Point(9, 348);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(36, 28);
            this.label22.TabIndex = 150;
            this.label22.Text = "6.)";
            this.label22.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.Location = new System.Drawing.Point(9, 429);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(36, 28);
            this.label21.TabIndex = 149;
            this.label21.Text = "7.)";
            this.label21.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label26);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label25);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Location = new System.Drawing.Point(818, 160);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(462, 508);
            this.panel1.TabIndex = 157;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.label20);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.label17);
            this.panel2.Location = new System.Drawing.Point(483, 677);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(796, 182);
            this.panel2.TabIndex = 158;
            // 
            // HelpFormula
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1298, 865);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.button_ADD);
            this.Controls.Add(this.Select_label);
            this.Controls.Add(this.button_generateformula);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_back);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "HelpFormula";
            this.Text = "Formula Creation Page CoSD";
            this.Load += new System.EventHandler(this.HelpFormula_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericInputsCount)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericInputsCount;
        private System.Windows.Forms.Label label5;
        private MyCombobox combobox_dataseries;
        private MyCombobox combobox_stattype;
        private System.Windows.Forms.Label Select_label;
        private System.Windows.Forms.Button button_generateformula;
        private System.Windows.Forms.Button button_ADD;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Label label_inputsselected;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private WaterMarkTextBox textBox_Inputsdisplay;
        private System.Windows.Forms.Label label6;
        private WaterMarkTextBox waterMarkTextBox_sampleformula;
        private System.Windows.Forms.Label label_sampleformula;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}