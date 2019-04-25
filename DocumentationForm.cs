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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoSD_Tool
{
    public partial class DocumentationForm : Form
    {
        public DocumentationForm()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
        }

        private void CoSDdoc_Button_Click(object sender, EventArgs e)
        {
            // Navigate to a URL.
            try
            {
                System.Diagnostics.Process.Start("http://connecters/content/28469/cosd");
            }
            catch (Exception)
            {
                MessageBox.Show("Please make sure that an Internet Browser is installed on your system.\nContact technical team for further assistance");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string resource = "The CoSD Manual.pdf";
            //    string filename = Path.Combine(Path.GetTempPath(), resource);
            //    System.IO.File.WriteAllBytes(filename, global::AP_CoSD_Tool.Properties.Resources.The_CoSD_Manual);
            //    System.Diagnostics.Process.Start(filename); 

            //}
            //catch (Exception ex)
            //{           
            //    MessageBox.Show("Please make sure that pdf reader is installed on your system.\nContact technical team for further assistance");
            //}
            // Navigate to a URL.
            try
            {
                System.Diagnostics.Process.Start("http://connecters/content/28469/cosd");
            }
            catch (Exception)
            {
                MessageBox.Show("Please make sure that an Internet Browser is installed on your system.\nContact technical team for further assistance");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button_back_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Parent = null;
        }

        private void label_ApCoSD_Click(object sender, EventArgs e)
        {

        }

        private void label_Path_Click(object sender, EventArgs e)
        {

        }

        private void update_groupBox_Enter(object sender, EventArgs e)
        {

        }
    }
}
