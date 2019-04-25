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
    public partial class Mail_Update : Form
    {
        private string mailStatus = "NotSent";
        //UpdateValidate updateValidate = new UpdateValidate();

        public Mail_Update()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
            mailStatus = "NotSent";
            
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Parent = null;
        }

        private void button_Submit_Click(object sender, EventArgs e)
        {
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
            mailItem.Subject = "Data Value Updated via CoSD Tool";

            //  mailItem.To = textBox_email.Text;
            if(ReceiverDefaultmail.Text!="")
            mailItem.Recipients.Add((ReceiverDefaultmail.Text).ToString());
            
            // set email content
            
            mailItem.Body = "Dear Data Coordinator," + "\n\n" +
                "The user \"" + currentUser.Name + "\"" + " found a Data Value error in Animal Products CoSD.\nThe value is from an ISD-maintained database, it is updated and marked as ERS modified.\nDetails:\nCommodity-Subcommodity: "+UpdateValidate.commodityName+"\nSource: "+UpdateValidate.dataVaueSource+"\n\n User Comment: \n" +
                textBox_Comments.Text;

            // email sending scope
                
                mailItem.Send();
                mailStatus = "Sent";
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not send mail. Please make sure the outlook is properly installed on your system.");
                
                return;
            }
            
            }

        public string mailSent()
        {
            if (mailStatus == "Sent"){
                mailStatus = "";
                return "Sent";
            }
            else
                return "NotSent";
        }

        private void Clear_Button_Click(object sender, EventArgs e)
        {

        }

        private void label_VegetableCoSD_Click(object sender, EventArgs e)
        {

        }



    }
}