namespace CoSD_Tool
{
    partial class Mail_Update
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mail_Update));
            this.button_Submit = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Comments = new System.Windows.Forms.TextBox();
            this.Clear_Button = new System.Windows.Forms.Button();
            this.Default_email = new System.Windows.Forms.Label();
            this.ReceiverDefaultmail = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_Submit
            // 
            this.button_Submit.BackColor = System.Drawing.Color.LightGray;
            this.button_Submit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_Submit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Submit.ForeColor = System.Drawing.Color.Black;
            this.button_Submit.Location = new System.Drawing.Point(706, 358);
            this.button_Submit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_Submit.Name = "button_Submit";
            this.button_Submit.Size = new System.Drawing.Size(224, 43);
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
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 198);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 25);
            this.label1.TabIndex = 54;
            this.label1.Text = "Additional Comments :";
            // 
            // textBox_Comments
            // 
            this.textBox_Comments.Location = new System.Drawing.Point(224, 165);
            this.textBox_Comments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox_Comments.Multiline = true;
            this.textBox_Comments.Name = "textBox_Comments";
            this.textBox_Comments.Size = new System.Drawing.Size(704, 179);
            this.textBox_Comments.TabIndex = 5;
            // 
            // Clear_Button
            // 
            this.Clear_Button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Clear_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Clear_Button.Location = new System.Drawing.Point(224, 358);
            this.Clear_Button.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Clear_Button.Name = "Clear_Button";
            this.Clear_Button.Size = new System.Drawing.Size(93, 43);
            this.Clear_Button.TabIndex = 7;
            this.Clear_Button.Text = "Clear";
            this.toolTip1.SetToolTip(this.Clear_Button, "Clear");
            this.Clear_Button.UseVisualStyleBackColor = true;
            this.Clear_Button.Click += new System.EventHandler(this.Clear_Button_Click);
            // 
            // Default_email
            // 
            this.Default_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Default_email.Location = new System.Drawing.Point(16, 128);
            this.Default_email.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Default_email.Name = "Default_email";
            this.Default_email.Size = new System.Drawing.Size(129, 31);
            this.Default_email.TabIndex = 59;
            this.Default_email.Text = "Email PoC";
            // 
            // ReceiverDefaultmail
            // 
            this.ReceiverDefaultmail.ForeColor = System.Drawing.Color.Black;
            this.ReceiverDefaultmail.Location = new System.Drawing.Point(224, 128);
            this.ReceiverDefaultmail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ReceiverDefaultmail.Name = "ReceiverDefaultmail";
            this.ReceiverDefaultmail.Size = new System.Drawing.Size(324, 26);
            this.ReceiverDefaultmail.TabIndex = 3;
            this.ReceiverDefaultmail.Text = "feras.batarseh@ers.usda.gov";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 29);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(827, 50);
            this.label2.TabIndex = 60;
            this.label2.Text = "This Value is from an ISD-maintained database, it will be marked as ERS modified " +
    "and \r\nan email will be sent to data coordinator saying you\'ve found an error in " +
    "Animal Products CoSD\r\n";
            // 
            // Mail_Update
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(948, 482);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ReceiverDefaultmail);
            this.Controls.Add(this.Default_email);
            this.Controls.Add(this.Clear_Button);
            this.Controls.Add(this.textBox_Comments);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button_Submit);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Mail_Update";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mail Update";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Submit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Comments;
        private System.Windows.Forms.Button Clear_Button;
        private System.Windows.Forms.Label Default_email;
        private System.Windows.Forms.TextBox ReceiverDefaultmail;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;

    }
}