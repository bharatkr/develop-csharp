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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoSD_Tool
{
    public partial class Popup : Form
    {
        public Popup()
        {
            InitializeComponent();
        }

        public string EnteredText
        {
            get
            {
                return (labelmsg.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            labelmsg.Text = "OK";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            labelmsg.Text = "Cancel";
            this.Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Popup_Load(object sender, EventArgs e)
        {

        }
    }
}
