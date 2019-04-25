// ***********************************************************************
// Project          : Combined CoSD Tool
// Author           : Bharat Radhakrishnan, George Mason University
// Created          : 04-25-2019
// Version          : 5.0
// Update history   : Please refer to backup folder on MTEDs shared drive
// ***********************************************************************
// <copyright file="Delete.cs" company="USDA, ERS, GMU">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>
// This page allows ERS analysts to search data values based on the selections,
// and then delete rows of values.
// </summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mail;


//new
namespace CoSD_Tool
{
    public partial class FeedbackForm : Form
    {

        public FeedbackForm()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
            
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Parent = null;
        }

        private void button_Submit_Click(object sender, EventArgs e)
        {


                if (textBox_name.Text.Equals(""))
                {
                    MessageBox.Show("Please enter your name");
                    return;
                }

                if (textBox_Comments.Text.Equals(""))
                {
                    MessageBox.Show("Please enter your Comments");
                    return;
                }

                if (ReceiverDefaultmail.Text == "")
                {
                    MessageBox.Show("Please enter POC email");
                    return;
                }
            else
            {
                bool bCheck1 = IsValidEmail(ReceiverDefaultmail.Text);
                if (bCheck1)
                {
                    //valid
                }
                else
                {
                    MessageBox.Show("Please enter valid email PoC 1");
                    return;
                }

                bool bCheck2 = IsValidEmail(ReceiverEmail2.Text);
                if (bCheck2 || ReceiverEmail2.Text=="")
                {
                    //valid
                }
                else
                {
                    MessageBox.Show("Please enter valid email PoC 2");
                    return;
                }

            }

                
            try
           {

            // get current user
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            //  Console.WriteLine(currentUser.Name);
            // load outlook
            Microsoft.Office.Interop.Outlook.Application outlookApp = new Microsoft.Office.Interop.Outlook.Application();
            // create a new email
            Microsoft.Office.Interop.Outlook.MailItem mailItem = (Microsoft.Office.Interop.Outlook.MailItem)outlookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
            // set subject
            mailItem.Subject = "Feedback by: " + textBox_name.Text + " - " + textBox_subject.Text;

            //  mailItem.To = textBox_email.Text;
            if(ReceiverDefaultmail.Text!="")
            mailItem.Recipients.Add((ReceiverDefaultmail.Text).ToString());
            if (ReceiverEmail2.Text != "" && !ReceiverDefaultmail.Text.Equals(ReceiverEmail2.Text))
            mailItem.Recipients.Add((ReceiverEmail2.Text).ToString());
            
            // set email content
            mailItem.Body = "Dear CoSD Team," + "\n\n" +
                "The user \"" + currentUser.Name + "\"" + " has posted the following feedback: \n\n" +
                textBox_Comments.Text;

            // email sending scope

                mailItem.Send();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not send feedback. Please make sure the outlook is properly installed on your system.");
                
                return;
            }

            MessageBox.Show("Thank You!! Your Feedback has been emailed successfully to CoSD Team");
            
            }

        private void Clear_Button_Click(object sender, EventArgs e)
        {
            ReceiverEmail2.Text = null;
            textBox_name.Text = null;
            textBox_subject.Text = null;
            textBox_Comments.Text = null;

        }

        public static bool IsValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        private void ReceiverEmail2_TextChanged(object sender, EventArgs e)
        { }


           
    }
}