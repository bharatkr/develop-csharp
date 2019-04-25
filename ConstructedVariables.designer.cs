namespace CoSD_Tool
{
    partial class ConstructedVariables
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConstructedVariables));
            this.button_back = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.button_next = new System.Windows.Forms.Button();
            this.button_prev = new System.Windows.Forms.Button();
            this.label_pageNum = new System.Windows.Forms.Label();
            this.button_reset = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.comboBox_OutputName = new CoSD_Tool.MyCombobox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox_Source = new CoSD_Tool.MyCombobox();
            this.button_retreive = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label_Path = new System.Windows.Forms.Label();
            this.label_PageHeading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_back
            // 
            this.button_back.BackColor = System.Drawing.Color.LightGray;
            this.button_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_back.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_back.ForeColor = System.Drawing.Color.Black;
            this.button_back.Location = new System.Drawing.Point(960, 70);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(135, 23);
            this.button_back.TabIndex = 13;
            this.button_back.Text = "Back to Prev Page";
            this.toolTip1.SetToolTip(this.button_back, "Back to Prev Page\r\n");
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(27, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(255, 95);
            this.pictureBox1.TabIndex = 75;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView);
            this.groupBox3.Controls.Add(this.button_next);
            this.groupBox3.Controls.Add(this.button_prev);
            this.groupBox3.Controls.Add(this.label_pageNum);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(417, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(660, 383);
            this.groupBox3.TabIndex = 79;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Check Rules to Edit or Execute";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(7, 21);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(643, 317);
            this.dataGridView.TabIndex = 82;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);
            this.dataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView_DataError);
            // 
            // button_next
            // 
            this.button_next.BackColor = System.Drawing.Color.Gainsboro;
            this.button_next.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_next.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_next.ForeColor = System.Drawing.Color.Black;
            this.button_next.Location = new System.Drawing.Point(426, 344);
            this.button_next.Name = "button_next";
            this.button_next.Size = new System.Drawing.Size(87, 23);
            this.button_next.TabIndex = 8;
            this.button_next.Text = "Next";
            this.toolTip1.SetToolTip(this.button_next, "Next Page");
            this.button_next.UseVisualStyleBackColor = true;
            this.button_next.Visible = false;
            this.button_next.EnabledChanged += new System.EventHandler(this.button_next_EnabledChanged);
            this.button_next.Click += new System.EventHandler(this.button_next_Click);
            // 
            // button_prev
            // 
            this.button_prev.BackColor = System.Drawing.Color.Gainsboro;
            this.button_prev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_prev.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_prev.ForeColor = System.Drawing.Color.Black;
            this.button_prev.Location = new System.Drawing.Point(146, 344);
            this.button_prev.Name = "button_prev";
            this.button_prev.Size = new System.Drawing.Size(87, 23);
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
            this.label_pageNum.Location = new System.Drawing.Point(272, 347);
            this.label_pageNum.Name = "label_pageNum";
            this.label_pageNum.Size = new System.Drawing.Size(62, 25);
            this.label_pageNum.TabIndex = 4;
            this.label_pageNum.Text = "Page";
            this.label_pageNum.Visible = false;
            // 
            // button_reset
            // 
            this.button_reset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_reset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.button_reset.Location = new System.Drawing.Point(12, 110);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(162, 49);
            this.button_reset.TabIndex = 12;
            this.button_reset.Text = "Reset All Fields";
            this.toolTip1.SetToolTip(this.button_reset, "Resets full page");
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Controls.Add(this.button_retreive);
            this.groupBox2.Controls.Add(this.button_reset);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(27, 118);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox2.Size = new System.Drawing.Size(371, 166);
            this.groupBox2.TabIndex = 78;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Choose Values";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.47303F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 72.52697F));
            this.tableLayoutPanel1.Controls.Add(this.comboBox_OutputName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_Source, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 22);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(351, 66);
            this.tableLayoutPanel1.TabIndex = 84;
            // 
            // comboBox_OutputName
            // 
            this.comboBox_OutputName.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBox_OutputName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBox_OutputName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox_OutputName.DropDownWidth = 400;
            this.comboBox_OutputName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_OutputName.FormattingEnabled = true;
            this.comboBox_OutputName.IntegralHeight = false;
            this.comboBox_OutputName.Location = new System.Drawing.Point(109, 8);
            this.comboBox_OutputName.Name = "comboBox_OutputName";
            this.comboBox_OutputName.Size = new System.Drawing.Size(229, 33);
            this.comboBox_OutputName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 50);
            this.label2.TabIndex = 20;
            this.label2.Text = "Output Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(5, 50);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 16);
            this.label5.TabIndex = 18;
            this.label5.Text = "Source";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox_Source
            // 
            this.comboBox_Source.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBox_Source.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox_Source.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Source.FormattingEnabled = true;
            this.comboBox_Source.IntegralHeight = false;
            this.comboBox_Source.Location = new System.Drawing.Point(109, 53);
            this.comboBox_Source.Name = "comboBox_Source";
            this.comboBox_Source.Size = new System.Drawing.Size(229, 33);
            this.comboBox_Source.TabIndex = 12;
            // 
            // button_retreive
            // 
            this.button_retreive.BackColor = System.Drawing.Color.LightGray;
            this.button_retreive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_retreive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button_retreive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_retreive.Location = new System.Drawing.Point(188, 110);
            this.button_retreive.Name = "button_retreive";
            this.button_retreive.Size = new System.Drawing.Size(162, 49);
            this.button_retreive.TabIndex = 5;
            this.button_retreive.Text = "View \r\nConstructed Variables";
            this.toolTip1.SetToolTip(this.button_retreive, "View Constructed Variables");
            this.button_retreive.UseVisualStyleBackColor = true;
            this.button_retreive.Click += new System.EventHandler(this.button_retreive_Click);
            // 
            // label_Path
            // 
            this.label_Path.AutoSize = true;
            this.label_Path.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Path.Location = new System.Drawing.Point(301, 57);
            this.label_Path.Name = "label_Path";
            this.label_Path.Size = new System.Drawing.Size(388, 40);
            this.label_Path.TabIndex = 87;
            this.label_Path.Text = "Home -> Business Logic -> Constructed Variables\r\n\r\n";
            // 
            // label_PageHeading
            // 
            this.label_PageHeading.AutoSize = true;
            this.label_PageHeading.Font = new System.Drawing.Font("Lucida Calligraphy", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PageHeading.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label_PageHeading.Location = new System.Drawing.Point(288, 12);
            this.label_PageHeading.Name = "label_PageHeading";
            this.label_PageHeading.Size = new System.Drawing.Size(676, 68);
            this.label_PageHeading.TabIndex = 86;
            this.label_PageHeading.Text = "Constructed Variables";
            this.label_PageHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConstructedVariables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1109, 781);
            this.Controls.Add(this.label_Path);
            this.Controls.Add(this.label_PageHeading);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConstructedVariables";
            this.Text = "View Constructed Variables";
            this.Load += new System.EventHandler(this.Constructed_VariablesLoad);
            this.Click += new System.EventHandler(this.BusinessRules_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.Label label5;
        private MyCombobox comboBox_Source;
        private System.Windows.Forms.Button button_retreive;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private MyCombobox comboBox_OutputName;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label_Path;
        private System.Windows.Forms.Label label_PageHeading;
        
    }
}