namespace CoSD_Tool
{
    partial class BusinessRulesTree
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BusinessRulesTree));
            this.label_PageHeading = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.listBox_outputname = new System.Windows.Forms.ListBox();
            this.comboBox_Commodity1 = new System.Windows.Forms.ComboBox();
            this.rowcount_label = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button_retreive = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.button_next = new System.Windows.Forms.Button();
            this.button_prev = new System.Windows.Forms.Button();
            this.label_pageNum = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox_constructorvar = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button_next1 = new System.Windows.Forms.Button();
            this.button_prev1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_back = new System.Windows.Forms.Button();
            this.label_count = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBox_constructorvar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_PageHeading
            // 
            this.label_PageHeading.AutoSize = true;
            this.label_PageHeading.Font = new System.Drawing.Font("Lucida Calligraphy", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_PageHeading.ForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.label_PageHeading.Location = new System.Drawing.Point(574, 68);
            this.label_PageHeading.Name = "label_PageHeading";
            this.label_PageHeading.Size = new System.Drawing.Size(602, 68);
            this.label_PageHeading.TabIndex = 90;
            this.label_PageHeading.Text = "Business Rules Tree";
            this.label_PageHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_PageHeading.Click += new System.EventHandler(this.label_PageHeading_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(590, 139);
            this.label1.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(586, 21);
            this.label1.TabIndex = 91;
            this.label1.Text = "Home -> Business Logic -> Business rules tree";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox2.Controls.Add(this.rowcount_label);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.button_retreive);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(38, 182);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox2.Size = new System.Drawing.Size(520, 345);
            this.groupBox2.TabIndex = 92;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Choose Business Rule ";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.01826F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.98174F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.listBox_outputname, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_Commodity1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 32);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(493, 228);
            this.tableLayoutPanel1.TabIndex = 84;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 121);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 25);
            this.label2.TabIndex = 29;
            this.label2.Text = "Output name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 7);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 25);
            this.label6.TabIndex = 26;
            this.label6.Text = "Commodity";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBox_outputname
            // 
            this.listBox_outputname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_outputname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox_outputname.FormattingEnabled = true;
            this.listBox_outputname.ItemHeight = 25;
            this.listBox_outputname.Location = new System.Drawing.Point(170, 70);
            this.listBox_outputname.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBox_outputname.Name = "listBox_outputname";
            this.listBox_outputname.Size = new System.Drawing.Size(320, 154);
            this.listBox_outputname.TabIndex = 30;
            // 
            // comboBox_Commodity1
            // 
            this.comboBox_Commodity1.FormattingEnabled = true;
            this.comboBox_Commodity1.Location = new System.Drawing.Point(170, 4);
            this.comboBox_Commodity1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBox_Commodity1.Name = "comboBox_Commodity1";
            this.comboBox_Commodity1.Size = new System.Drawing.Size(319, 33);
            this.comboBox_Commodity1.TabIndex = 27;
            this.comboBox_Commodity1.SelectedIndexChanged += new System.EventHandler(this.comboBox_Commodity1_SelectedIndexChanged);
            // 
            // rowcount_label
            // 
            this.rowcount_label.AutoSize = true;
            this.rowcount_label.Location = new System.Drawing.Point(286, 376);
            this.rowcount_label.Name = "rowcount_label";
            this.rowcount_label.Size = new System.Drawing.Size(24, 25);
            this.rowcount_label.TabIndex = 83;
            this.rowcount_label.Text = "0";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(74, 376);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(246, 29);
            this.label8.TabIndex = 82;
            this.label8.Text = "Number of formulas retrieved :";
            // 
            // button_retreive
            // 
            this.button_retreive.BackColor = System.Drawing.Color.LightGray;
            this.button_retreive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_retreive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.button_retreive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_retreive.Location = new System.Drawing.Point(114, 268);
            this.button_retreive.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_retreive.Name = "button_retreive";
            this.button_retreive.Size = new System.Drawing.Size(288, 49);
            this.button_retreive.TabIndex = 5;
            this.button_retreive.Text = "View Business Rules && Tree";
            this.button_retreive.UseVisualStyleBackColor = true;
            this.button_retreive.Click += new System.EventHandler(this.button_retreive_Click_1);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView);
            this.groupBox3.Controls.Add(this.button_next);
            this.groupBox3.Controls.Add(this.button_prev);
            this.groupBox3.Controls.Add(this.label_pageNum);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(608, 182);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(1388, 450);
            this.groupBox3.TabIndex = 93;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "View Business rules";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(8, 31);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.Size = new System.Drawing.Size(1374, 361);
            this.dataGridView.TabIndex = 82;
            // 
            // button_next
            // 
            this.button_next.BackColor = System.Drawing.Color.Gainsboro;
            this.button_next.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_next.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_next.ForeColor = System.Drawing.Color.Black;
            this.button_next.Location = new System.Drawing.Point(750, 414);
            this.button_next.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_next.Name = "button_next";
            this.button_next.Size = new System.Drawing.Size(98, 29);
            this.button_next.TabIndex = 8;
            this.button_next.Text = "Next";
            this.button_next.UseVisualStyleBackColor = true;
            this.button_next.Visible = false;
            // 
            // button_prev
            // 
            this.button_prev.BackColor = System.Drawing.Color.Gainsboro;
            this.button_prev.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_prev.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_prev.ForeColor = System.Drawing.Color.Black;
            this.button_prev.Location = new System.Drawing.Point(436, 414);
            this.button_prev.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_prev.Name = "button_prev";
            this.button_prev.Size = new System.Drawing.Size(98, 29);
            this.button_prev.TabIndex = 9;
            this.button_prev.Text = "Prev";
            this.button_prev.UseVisualStyleBackColor = true;
            this.button_prev.Visible = false;
            // 
            // label_pageNum
            // 
            this.label_pageNum.AutoSize = true;
            this.label_pageNum.Location = new System.Drawing.Point(619, 418);
            this.label_pageNum.Name = "label_pageNum";
            this.label_pageNum.Size = new System.Drawing.Size(62, 25);
            this.label_pageNum.TabIndex = 4;
            this.label_pageNum.Text = "Page";
            this.label_pageNum.Visible = false;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.White;
            this.treeView1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.ForeColor = System.Drawing.Color.Green;
            this.treeView1.FullRowSelect = true;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(9, 29);
            this.treeView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(510, 535);
            this.treeView1.TabIndex = 94;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // groupBox_constructorvar
            // 
            this.groupBox_constructorvar.Controls.Add(this.dataGridView1);
            this.groupBox_constructorvar.Controls.Add(this.button_next1);
            this.groupBox_constructorvar.Controls.Add(this.button_prev1);
            this.groupBox_constructorvar.Controls.Add(this.label3);
            this.groupBox_constructorvar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox_constructorvar.Location = new System.Drawing.Point(608, 704);
            this.groupBox_constructorvar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox_constructorvar.Name = "groupBox_constructorvar";
            this.groupBox_constructorvar.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox_constructorvar.Size = new System.Drawing.Size(1388, 444);
            this.groupBox_constructorvar.TabIndex = 95;
            this.groupBox_constructorvar.TabStop = false;
            this.groupBox_constructorvar.Text = "View Constructed Variables";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(8, 31);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1374, 355);
            this.dataGridView1.TabIndex = 82;
            // 
            // button_next1
            // 
            this.button_next1.BackColor = System.Drawing.Color.Gainsboro;
            this.button_next1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_next1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_next1.ForeColor = System.Drawing.Color.Black;
            this.button_next1.Location = new System.Drawing.Point(658, 394);
            this.button_next1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_next1.Name = "button_next1";
            this.button_next1.Size = new System.Drawing.Size(98, 29);
            this.button_next1.TabIndex = 8;
            this.button_next1.Text = "Next";
            this.button_next1.UseVisualStyleBackColor = true;
            this.button_next1.Visible = false;
            // 
            // button_prev1
            // 
            this.button_prev1.BackColor = System.Drawing.Color.Gainsboro;
            this.button_prev1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_prev1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_prev1.ForeColor = System.Drawing.Color.Black;
            this.button_prev1.Location = new System.Drawing.Point(344, 394);
            this.button_prev1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button_prev1.Name = "button_prev1";
            this.button_prev1.Size = new System.Drawing.Size(98, 29);
            this.button_prev1.TabIndex = 9;
            this.button_prev1.Text = "Prev";
            this.button_prev1.UseVisualStyleBackColor = true;
            this.button_prev1.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(526, 398);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Page";
            this.label3.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(14, 26);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(287, 119);
            this.pictureBox1.TabIndex = 76;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(38, 559);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(525, 589);
            this.groupBox1.TabIndex = 97;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Double click on desired CVs";
            // 
            // button_back
            // 
            this.button_back.BackColor = System.Drawing.Color.LightGray;
            this.button_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_back.ForeColor = System.Drawing.Color.Black;
            this.button_back.Location = new System.Drawing.Point(1491, 125);
            this.button_back.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(249, 35);
            this.button_back.TabIndex = 99;
            this.button_back.Text = "Back to Business Rule Page";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // label_count
            // 
            this.label_count.AutoSize = true;
            this.label_count.Location = new System.Drawing.Point(1044, 164);
            this.label_count.Name = "label_count";
            this.label_count.Size = new System.Drawing.Size(91, 20);
            this.label_count.TabIndex = 100;
            this.label_count.Text = "label_count";
            // 
            // BusinessRulesTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1924, 1050);
            this.Controls.Add(this.label_count);
            this.Controls.Add(this.button_back);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox_constructorvar);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label_PageHeading);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "BusinessRulesTree";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBox_constructorvar.ResumeLayout(false);
            this.groupBox_constructorvar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label_PageHeading;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_Commodity1;
        private System.Windows.Forms.Label rowcount_label;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button_retreive;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button button_next;
        private System.Windows.Forms.Button button_prev;
        private System.Windows.Forms.Label label_pageNum;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox_outputname;
        private System.Windows.Forms.GroupBox groupBox_constructorvar;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button_next1;
        private System.Windows.Forms.Button button_prev1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.Label label_count;
    }
}