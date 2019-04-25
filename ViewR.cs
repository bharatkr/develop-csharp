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
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CoSD_Tool
{
    public partial class ViewR : Form
    {

        /// <summary>
        /// The con
        /// </summary>
        private SqlConnection con;    // db connection
        /// <summary>
        /// The sqlconn
        /// </summary>
        private string sqlconn;  // query and sql connection
        /// <summary>
        /// The data adapter
        /// </summary>
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        /// <summary>
        /// The data adapter
        /// </summary>
        private SqlDataAdapter dataAdapterForPaging = new SqlDataAdapter();
        DataTable DT;
        private string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);

        public ViewR()
        {
            InitializeComponent();
        }

        private void connection()
        {
            try
            {
                // connection string for linking db
                sqlconn = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString; // each window had a con string to the DB, server name in config file
                                                                                                                                                   // init the connection to db
                con = new SqlConnection(sqlconn);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                return;
            }
        }


        private DataTable GetData(string selectCommand)
        {
            try
            {
                connection();

                Console.WriteLine(selectCommand);

                dataAdapter = new SqlDataAdapter(selectCommand, con);


                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                DataTable result = new DataTable();
                dataAdapter.Fill(result);

                return result;

            }
            catch (SqlException ex)
            {
                Console.Write(ex.Message);
                throw new Exception("Connection Error");

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Equals(""))
            {
                MessageBox.Show("Please enter the data series value");
            }

            else
            {

                string dataSeriesSQL = "select R_DataSeriesID as DataSeriesID,R_TimeFrequency as Time,R_Date as Date,R_GeographyType as GeoType,R_City as City,R_County as County,R_State as State" +
", R_Region as Region,R_Country as Country,R_Unit as Unit,R_Value as Value  from " + schemaName + "ERSConstructedVariablesOutcomesR where R_DataSeriesID = " + textBox1.Text;
                connection();
                con.Open();
                dataAdapterForPaging = new SqlDataAdapter(dataSeriesSQL, con);
                dataAdapterForPaging.SelectCommand.ExecuteNonQuery();
                DT = new DataTable();
                dataAdapterForPaging.Fill(DT);
                dataGridView1.DataSource = DT;
                dataGridView1.ReadOnly = true;


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            dataGridView1.DataSource = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Parent = null;
        }
    }
}
