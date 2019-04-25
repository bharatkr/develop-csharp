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
using ZedGraph;

namespace CoSD_Tool
{
    public partial class Chart : Form
    {
        public string message = "";
        public double[] y = new double[10];
        public double[] results;

        private ZedGraph.ZedGraphControl zg1;

        public Chart()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
        }

        private void Chart_Load(object sender, EventArgs e)
        {
            CreateGraph(zg1);
            SetSize();
        }

        // Build the Chart
        private void CreateGraph(ZedGraphControl zg1)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zg1.GraphPane;

            // Set the Titles on the chart
            myPane.Title.Text = "Data Histogram";
            myPane.XAxis.Title.Text = "Data Intervals";
            myPane.YAxis.Title.Text = "Frequency of Values";

            // build labels with ranges of values
            // 10% as a bar (to show 10 bars)
            Array.Sort(results);
            double min = results[0];
            double max = results[results.Length - 1];
            // divide into 10 bars 
            double interval = (double)(max - min) / (double)10; //show 10 bars
            string[] labels = new String[10];//show 10 lables
            for (int i = 0; i < 10; i++)//show 10 intervals
                labels[i] = (min + interval * i ).ToString() + "\n|\n" + (min + interval * (i + 1)).ToString();

            foreach(double val in results)
            {
                for(int i = 9; i>=0; i--)
                {
                    if (val > min + interval * (i) || Math.Abs(val - (min + interval * (i))) <= 0.0001)   // means >=
                    {
                        y[i]++;
                        break;
                    }
                }
            }


            // Generate a red bar with "Curve 1" in the legend
            BarItem myBar = myPane.AddBar("Occurence", null, y,
                                                        Color.Blue);
            myBar.Bar.Fill = new Fill(Color.Blue, Color.White,
                                                        Color.Blue);



            // Draw the X tics between the labels instead of 
            // at the labels
            myPane.XAxis.MajorTic.IsBetweenLabels = true;

            // Set the XAxis labels
            myPane.XAxis.Scale.TextLabels = labels;
            myPane.XAxis.Scale.FontSpec.Size = 10;
            // Set the XAxis to Text type
            myPane.XAxis.Type = AxisType.Text;

            // Fill the Axis and Pane backgrounds
            myPane.Chart.Fill = new Fill(Color.White,
                  Color.FromArgb(255, 255, 166), 90F);
            myPane.Fill = new Fill(Color.FromArgb(250, 250, 255));

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zg1.AxisChange();


        }

        private void SetSize()//set size for the chart inside the validation window
        {
            zg1.Location = new Point(10, 110);
            // Leave a small margin around the outside of the control
            zg1.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 120);
        }

        private void label_Path_Click(object sender, EventArgs e)
        {

        }
    }
}
