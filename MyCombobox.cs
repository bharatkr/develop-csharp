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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CoSD_Tool
{
    public class MyCombobox : ComboBox
    {
        protected override bool IsInputKey(Keys keyData)
            {
                switch ((keyData & (Keys.Alt | Keys.KeyCode)))
                {
                    case Keys.Enter:
                    case Keys.Escape:
                        if (this.DroppedDown)
                        {
                            this.DroppedDown = false;
                            return false;
                        }
                        break;
                }
                return base.IsInputKey(keyData);
            }
    }
}
