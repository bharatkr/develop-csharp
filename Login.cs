// ***********************************************************************
// Project          : Combined Tool Landing Page
// Author           : Bharat Radhakrishnan, George Mason University
// Created          : 04-25-2019
// Version          : 1.0
// Update history   : Please refer to backup folder on MTEDs shared drive
// ***********************************************************************
// <copyright file="Login.cs" company="USDA, ERS, GMU">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>
// This page merges the three tools we have for all three commodities
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
    public partial class LandingScreen : Form
    {
       

        public LandingScreen()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
        }

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


        string schemaName = "AnimalProductsCoSD.CoSD.";
       public static string newschemaName = "";
        string apGroupId = "1,2,3,4,19,6";
        string vegGroupId = "9,11,14";
        string fruitGroupId = "10,20,21,22";
     
    

        private BindingSource bindingSource1 = new BindingSource();
        /// <summary>
        /// The table used to get data results
        /// </summary>
        DataTable table = new DataTable();
        DataTable DT;
        /// <summary>
        /// The binding source for physical attributes
        /// </summary>
        private BindingSource bindingSourcePhyAtt = new BindingSource();
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
            DataTable result= new DataTable(); ;
            try
            {
                connection();

                Console.WriteLine(selectCommand);
                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(selectCommand, con);


                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                DataTable queryresult = new DataTable();
                dataAdapter.Fill(queryresult);

                return queryresult;

            }
            catch (SqlException ex)
            {
                
                return result;
                

            }

         
        }

        private void button1_Click(object sender, EventArgs e)
        {
           System.Security.Principal.WindowsIdentity currentUser = 
                System.Security.Principal.WindowsIdentity.GetCurrent();

            bool animalProductsradio = radioButton1.Checked;
            bool vegetablesradio = radioButton2.Checked;
            bool fruitsradio = radioButton3.Checked;

            if (animalProductsradio)
            {
                string sql = "select ERSGroup_ID from " + schemaName + "ERSGroup_LU where ERSGroup_Username " +
                    "like '%" + currentUser.Name + "%' and [ERSGroup_ID] IN (" + apGroupId + ")";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();

                if (dr.Length == 0)
                {

                    MessageBox.Show("You do not have access to the DB or this sector");
                }

                else
                {
                    newschemaName = "AnimalProductsCoSD.CoSD.";
                    string localSchemaName = GetSchemaName(newschemaName);
                    new Home(localSchemaName).Show();
                    Program.OpenDetailFormOnClose = true;

                    this.Close();
                }
            }

            if (vegetablesradio)
            {
                string sql = "select ERSGroup_ID from "+"VegetablesCoSD.CoSD."+"ERSGroup_LU where ERSGroup_Username like '%" + currentUser.Name + "%' and [ERSGroup_ID] IN (" + vegGroupId + ")";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();

                if (dr.Length == 0)
                {

                    MessageBox.Show("You do not have access to the DB or this sector");
                }

                else
                {
                    newschemaName = "VegetablesCoSD.CoSD.";
                    string localSchemaName = GetSchemaName(newschemaName);
                    new Home(localSchemaName).Show();
                    Program.OpenDetailFormOnClose = true;

                    this.Close();
                }
            }

            if (fruitsradio)
            {
                string sql = "select ERSGroup_ID from " +"FruitCoSD.CoSD."+ "ERSGroup_LU where ERSGroup_Username like '%" + currentUser.Name + "%' and [ERSGroup_ID] IN (" + fruitGroupId + ")";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();

                if (dr.Length == 0)
                {

                    MessageBox.Show("You do not have access to the DB or this sector");
                }

                else
                {
                    newschemaName = "FruitCoSD.CoSD.";
                    string localSchemaName = GetSchemaName(newschemaName);
                    new Home(localSchemaName).Show();
                    Program.OpenDetailFormOnClose = true;

                    this.Close();

                }
                
            }


        }

        public static string GetSchemaName(string schemname)
        {
            return schemname;
        }

        public static string GetGroupId(string schemname)
        {
            string groupId="";
            if (schemname.Equals("AnimalProductsCoSD.CoSD."))
                groupId= "1,2,3,4,6,15,19";
            if (schemname.Equals("VegetablesCoSD.CoSD."))
                groupId= "9,11,14";
            if (schemname.Equals("FruitCoSD.CoSD."))
                groupId= "10,20,21,22";

            return groupId;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
