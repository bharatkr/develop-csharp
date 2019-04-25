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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;

namespace CoSD_Tool
{
    public partial class HelpFormula : Form
    {
        
        /// <summary>
        /// go back action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private string schemaName = Home.globalSchemaName;
        public string formulainputs = string.Empty;
        public string dataseriesID  = string.Empty;
        public string sampleformula = string.Empty;
        public int inputscount;
        private List<string> staticInputs = new List<string>();
        private List<string> dataseriesInputs = new List<string>();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private string sqlconn;
        private int loopcount = 1;
        private List<string> statatype;
        private List<string> Dataseries;
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString);
        private void button_back_Click(object sender, EventArgs e)
        {

            this.Hide();
            this.Parent = null;
        }

        public HelpFormula()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
            
        }

        private void HelpFormula_Load(object sender, EventArgs e)
        {
            fillStatisticType();
            fillDataseriesType();
            disablebuttons();
        }

        private void disablebuttons()
        {
            combobox_stattype.Enabled = false;
            combobox_dataseries.Enabled = false;
            Select_label.Visible = false;
            Select_label.Text = "Please Select Stat Type and Dataseries ID for Input: 1";
            button_generateformula.BackColor = Color.Gray;
            button_generateformula.Enabled = false;
            button_ADD.BackColor = Color.Gray;
            button_ADD.Enabled = false;
            textBox_Inputsdisplay.Visible = false;
            label_inputsselected.Visible = false;
            label_sampleformula.Visible = false;
            waterMarkTextBox_sampleformula.Visible = false;
            staticInputs.Clear();
            dataseriesInputs.Clear();
            formulainputs = "";
            dataseriesID = "";
          
            
            
        }

        private void fillStatisticType()
        {
            statatype = new List<string>();
            //string sql = @"SELECT DISTINCT [ERSSource_Desc] FROM " + schemaName + "[ERSSource_LU]";
            string sql = @"SELECT DISTINCT [ERSStatisticType_Attribute] FROM " + schemaName + "[ERSStatisticType_LU]";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            statatype.Add("");
            foreach (DataRow row in dr)
            {
                //SourceType.Add(row["ERSSource_Desc"].ToString());
                statatype.Add(row["ERSStatisticType_Attribute"].ToString());
            }
            combobox_stattype.DataSource = statatype;
            combobox_stattype.SelectedItem = null;

        }

        private void fillDataseriesType()
        {
            Dataseries = new List<string>();
            //string sql = @"SELECT DISTINCT [ERSSource_Desc] FROM " + schemaName + "[ERSSource_LU]";
            string sql = @"SELECT DISTINCT [ERSCommodity_ID] FROM " + schemaName + "[ERSCommodityDataSeries]";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            Dataseries.Add("");
            foreach (DataRow row in dr)
            {
                //SourceType.Add(row["ERSSource_Desc"].ToString());
                Dataseries.Add(row["ERSCommodity_ID"].ToString());
            }
            combobox_dataseries.DataSource = Dataseries;
            combobox_dataseries.SelectedItem = null;

        }

        // record user action in user log table
        private void saveAction(string action)
        {// get current user            
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();

            action = action.Replace("'", "''");

            string sql = "insert into " + schemaName + "ERSTool_ActionLog ( ERSToolActionLog_time,ERSToolActionLog_User,ERSToolActionLog_Desc) " +
                "values (SYSDATETIME(), " +
               "'" + currentUser.Name + "', " +
            "'''" + action +
            "''')";
            GetData(sql);
        }


        // method connects db
        /// <summary>
        /// Connections this instance.
        /// </summary>
        private void connection()
        {
            // connection string for linking db
            sqlconn = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            // init the connection to db
            con = new SqlConnection(sqlconn);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="selectCommand">The select command.</param>
        private System.Data.DataTable GetData(string selectCommand)
        {
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


                System.Data.DataTable result = new System.Data.DataTable();
                dataAdapter.Fill(result);

                return result;

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                return null;
            }
        }

        private void button_ADD_Click(object sender, EventArgs e)
        {
            if(combobox_stattype.Text.Equals(""))
            {
                MessageBox.Show("Please select the statistic type");
                return;
            }

            if (combobox_dataseries.Text.Equals(""))
            {
                MessageBox.Show("Please select data series ID number");
                return;
            }

            if (loopcount <= numericInputsCount.Value)
            {
                staticInputs.Add(combobox_stattype.Text);
                dataseriesInputs.Add(combobox_dataseries.Text);
                combobox_stattype.SelectedItem = null;
                combobox_dataseries.SelectedItem = null;
                combobox_dataseries.Refresh();
                if (loopcount == numericInputsCount.Value)
                {
                    combobox_stattype.SelectedItem = null;
                    combobox_dataseries.SelectedItem = null;
                    combobox_stattype.Enabled = false;
                    combobox_dataseries.Enabled = false;
                    Select_label.Visible = false;
                    button_generateformula.BackColor = Color.Brown;
                    button_generateformula.Enabled = true;
                    button_ADD.BackColor = Color.Gray;
                    button_ADD.Enabled = false;
                    numericInputsCount.Enabled = false;
                    for (int i = 0; i < numericInputsCount.Value; i++)
                    {
                        formulainputs = formulainputs + staticInputs[i] + "(" + dataseriesInputs[i] + ")" + ",";

                    }
                    for (int i = 0; i < numericInputsCount.Value; i++)
                    {
                        dataseriesID = dataseriesID + dataseriesInputs[i] + ",";

                    }
                    for (int i = 0; i < numericInputsCount.Value; i++)
                    {
                        sampleformula = sampleformula + staticInputs[i] + "(" + dataseriesInputs[i] + ")" + "+";

                    }
                    sampleformula = sampleformula.TrimEnd('+');
                    formulainputs = formulainputs.TrimEnd(',');
                    dataseriesID = dataseriesID.TrimEnd(',');
                    label_inputsselected.Visible = true;
                    label_sampleformula.Visible = true;
                    textBox_Inputsdisplay.Visible = true;
                    textBox_Inputsdisplay.Text = formulainputs;
                    waterMarkTextBox_sampleformula.Visible = true;
                    waterMarkTextBox_sampleformula.Text = sampleformula;
                    
                }
                loopcount = loopcount + 1;         
                Select_label.Text = "Please Select Stat Type and Dataseries ID for Input: " + loopcount;
                
            }
        
        }

        private void button_generateformula_Click(object sender, EventArgs e)
        {
            using (BusinessRulesInsertForm insertform = new BusinessRulesInsertForm())
            {
                inputscount = (int)numericInputsCount.Value;
                insertform.helpformulainputs = formulainputs;
                insertform.helpformuladataseries = dataseriesID;
                sampleformula = waterMarkTextBox_sampleformula.Text;
                insertform.helpsampleformula = sampleformula;
                insertform.helpinputscount = inputscount;
                this.Close();
                insertform.Show();
                

            }


           
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            disablebuttons();
            numericInputsCount.Value = 0;
            numericInputsCount.Enabled = true;
            loopcount = 1;
        }

        private void numericInputsCount_ValueChanged(object sender, EventArgs e)
        {
            if (numericInputsCount.Value == 0)
            {
                Select_label.Visible = false;
            }

            else
            {
                combobox_stattype.Enabled = true;
                combobox_dataseries.Enabled = true;
                button_ADD.BackColor = Color.Brown;
                button_ADD.Enabled = true;
                Select_label.Visible = true;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

       



       

        
    }
}
