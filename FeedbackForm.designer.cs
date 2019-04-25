namespace CoSD_Tool
{
    partial class FeedbackForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeedbackForm));
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_back = new System.Windows.Forms.Button();
            this.button_Submit = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Name_label = new System.Windows.Forms.Label();
            this.textBox_Comments = new System.Windows.Forms.TextBox();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.textBox_subject = new System.Windows.Forms.TextBox();
            this.Clear_Button = new System.Windows.Forms.Button();
            this.Default_email = new System.Windows.Forms.Label();
            this.Sender_email = new System.Windows.Forms.Label();
            this.ReceiverDefaultmail = new System.Windows.Forms.TextBox();
            this.ReceiverEmail2 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 191);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 20);
            this.label2.TabIndex = 40;
            this.label2.Text = "Feedback Form";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(14, 14);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(327, 141);
            this.pictureBox1.TabIndex = 38;
            this.pictureBox1.TabStop = false;
            // 
            // button_back
            // 
            this.button_back.BackColor = System.Drawing.Color.LightGray;
            this.button_back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_back.ForeColor = System.Drawing.Color.Black;
            this.button_back.Location = new System.Drawing.Point(737, 74);
            this.button_back.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(202, 35);
            this.button_back.TabIndex = 8;
            this.button_back.Text = "Back to Home Page";
            this.toolTip1.SetToolTip(this.button_back, "Back to Home Page");
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // button_Submit
            // 
            this.button_Submit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Submit.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button_Submit.Location = new System.Drawing.Point(716, 615);
            this.button_Submit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Submit.Name = "button_Submit";
            this.button_Submit.Size = new System.Drawing.Size(224, 42);
            this.button_Submit.TabIndex = 6;
            this.button_Submit.Text = "Send Email";
            this.toolTip1.SetToolTip(this.button_Submit, "Send Email");
            this.button_Submit.UseVisualStyleBackColor = true;
            this.button_Submit.Click += new System.EventHandler(this.button_Submit_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Modern No. 20", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 448);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 31);
            this.label5.TabIndex = 49;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 430);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 25);
            this.label1.TabIndex = 54;
            this.label1.Text = "Comments :";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(26, 288);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 25);
            this.label4.TabIndex = 52;
            this.label4.Text = "Subject :";
            // 
            // Name_label
            // 
            this.Name_label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Name_label.AutoSize = true;
            this.Name_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name_label.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name_label.Location = new System.Drawing.Point(26, 250);
            this.Name_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Name_label.Name = "Name_label";
            this.Name_label.Size = new System.Drawing.Size(70, 25);
            this.Name_label.TabIndex = 44;
            this.Name_label.Text = "Name:";
            // 
            // textBox_Comments
            // 
            this.textBox_Comments.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox_Comments.Location = new System.Drawing.Point(233, 424);
            this.textBox_Comments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_Comments.Multiline = true;
            this.textBox_Comments.Name = "textBox_Comments";
            this.textBox_Comments.Size = new System.Drawing.Size(705, 179);
            this.textBox_Comments.TabIndex = 5;
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(233, 245);
            this.textBox_name.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(324, 26);
            this.textBox_name.TabIndex = 1;
            // 
            // textBox_subject
            // 
            this.textBox_subject.Location = new System.Drawing.Point(233, 288);
            this.textBox_subject.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_subject.Name = "textBox_subject";
            this.textBox_subject.Size = new System.Drawing.Size(324, 26);
            this.textBox_subject.TabIndex = 2;
            // 
            // Clear_Button
            // 
            this.Clear_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Clear_Button.Location = new System.Drawing.Point(233, 615);
            this.Clear_Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Clear_Button.Name = "Clear_Button";
            this.Clear_Button.Size = new System.Drawing.Size(93, 42);
            this.Clear_Button.TabIndex = 7;
            this.Clear_Button.Text = "Clear";
            this.toolTip1.SetToolTip(this.Clear_Button, "Clear");
            this.Clear_Button.UseVisualStyleBackColor = true;
            this.Clear_Button.Click += new System.EventHandler(this.Clear_Button_Click);
            // 
            // Default_email
            // 
            this.Default_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Default_email.Location = new System.Drawing.Point(26, 329);
            this.Default_email.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Default_email.Name = "Default_email";
            this.Default_email.Size = new System.Drawing.Size(129, 31);
            this.Default_email.TabIndex = 59;
            this.Default_email.Text = "Email PoC 1: ";
            // 
            // Sender_email
            // 
            this.Sender_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sender_email.Location = new System.Drawing.Point(26, 374);
            this.Sender_email.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Sender_email.Name = "Sender_email";
            this.Sender_email.Size = new System.Drawing.Size(141, 34);
            this.Sender_email.TabIndex = 60;
            this.Sender_email.Text = "Email PoC 2 : ";
            // 
            // ReceiverDefaultmail
            // 
            this.ReceiverDefaultmail.ForeColor = System.Drawing.Color.Black;
            this.ReceiverDefaultmail.Location = new System.Drawing.Point(233, 329);
            this.ReceiverDefaultmail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ReceiverDefaultmail.Name = "ReceiverDefaultmail";
            this.ReceiverDefaultmail.Size = new System.Drawing.Size(324, 26);
            this.ReceiverDefaultmail.TabIndex = 3;
            this.ReceiverDefaultmail.Text = "feras.batarseh@ers.usda.gov";
            // 
            // ReceiverEmail2
            // 
            this.ReceiverEmail2.Location = new System.Drawing.Point(233, 378);
            this.ReceiverEmail2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ReceiverEmail2.Name = "ReceiverEmail2";
            this.ReceiverEmail2.Size = new System.Drawing.Size(324, 26);
            this.ReceiverEmail2.TabIndex = 4;
            this.ReceiverEmail2.TextChanged += new System.EventHandler(this.ReceiverEmail2_TextChanged);
            // 
            // FeedbackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(957, 708);
            this.Controls.Add(this.ReceiverEmail2);
            this.Controls.Add(this.ReceiverDefaultmail);
            this.Controls.Add(this.Sender_email);
            this.Controls.Add(this.Default_email);
            this.Controls.Add(this.Clear_Button);
            this.Controls.Add(this.textBox_subject);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_Comments);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Name_label);
            this.Controls.Add(this.button_Submit);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_back);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FeedbackForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Feedback Form for AP CoSD";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.Button button_Submit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Name_label;
        private System.Windows.Forms.TextBox textBox_Comments;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.TextBox textBox_subject;
        private System.Windows.Forms.Button Clear_Button;
        private System.Windows.Forms.Label Default_email;
        private System.Windows.Forms.Label Sender_email;
        private System.Windows.Forms.TextBox ReceiverDefaultmail;
        private System.Windows.Forms.TextBox ReceiverEmail2;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}