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
using System.Data.SqlClient;
using Microsoft.SqlServer.Dts.Runtime;
using System.Configuration;
using System.Windows.Forms;

namespace CoSD_Tool
{
    public partial class StreamData : Form
    {

        private SqlDataAdapter SDA = new SqlDataAdapter();
        //WaitForm waitForm = new WaitForm();
        StreamingForm streamForm = new StreamingForm();
        private string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        // private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString);
         private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString);

        private string sqlconn;  // query and sql connection
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        public StreamData()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
            Getlabels();
        }


        private void Getlabels()
        {

            string nass = @"SELECT top (1)[ERSToolActionLog_time],[ERSToolActionLog_User],[ERSToolActionLog_Desc] FROM " + schemaName +
                "[ERSTool_ActionLog] WHERE ERSToolActionLog_Desc like '%NASS%' order by ERSToolActionLog_ID desc ";

            string wasde = @"SELECT top (1)[ERSToolActionLog_time],[ERSToolActionLog_User],[ERSToolActionLog_Desc] FROM " + schemaName +
                "[ERSTool_ActionLog] WHERE ERSToolActionLog_Desc like '%WASDE%' order by ERSToolActionLog_ID desc ";

            string bls = @"SELECT top (1)[ERSToolActionLog_time],[ERSToolActionLog_User],[ERSToolActionLog_Desc] FROM " + schemaName +
                "[ERSTool_ActionLog] WHERE ERSToolActionLog_Desc like '%BLS%' order by ERSToolActionLog_ID desc ";

            string trade = @"SELECT top (1)[ERSToolActionLog_time],[ERSToolActionLog_User],[ERSToolActionLog_Desc] FROM " + schemaName +
                "[ERSTool_ActionLog] WHERE ERSToolActionLog_Desc like '%TRADE%' order by ERSToolActionLog_ID desc ";

            DataTable dt1 = GetData(nass);
            DataRow[] dr1 = dt1.Select();

            DataTable dt2 = GetData(wasde);
            DataRow[] dr2 = dt2.Select();

            DataTable dt3 = GetData(bls);
            DataRow[] dr3 = dt3.Select();

            DataTable dt4 = GetData(trade);
            DataRow[] dr4= dt4.Select();

            foreach (DataRow row in dr1)
            {
                if (row["ERSToolActionLog_Desc"].ToString().Contains("Clicked"))
                    Nass_label.Text = row["ERSToolActionLog_User"].ToString() + " started streaming NASS records on " + row["ERSToolActionLog_time"].ToString();

                else if (row["ERSToolActionLog_Desc"].ToString().Contains("Successfull"))
                    Nass_label.Text = row["ERSToolActionLog_User"].ToString() + " successfully streamed NASS records on " + row["ERSToolActionLog_time"].ToString();

                else if (row["ERSToolActionLog_Desc"].ToString().Contains("No Record"))
                    Nass_label.Text = row["ERSToolActionLog_User"].ToString() + " streamed NASS records on " + row["ERSToolActionLog_time"] + ", data was up to date".ToString();

                else
                    Nass_label.Text = row["ERSToolActionLog_User"].ToString() + " unsuccessfully streamed NASS records on " + row["ERSToolActionLog_time"].ToString();
            }

            foreach (DataRow row in dr2)
            {
                if (row["ERSToolActionLog_Desc"].ToString().Contains("Clicked"))
                    wasde_label.Text = row["ERSToolActionLog_User"].ToString() + " started streaming WASDE records on " + row["ERSToolActionLog_time"].ToString();

                else if (row["ERSToolActionLog_Desc"].ToString().Contains("Successfull"))
                    wasde_label.Text = row["ERSToolActionLog_User"].ToString() + " successfully streamed WASDE records on " + row["ERSToolActionLog_time"].ToString();

                else if (row["ERSToolActionLog_Desc"].ToString().Contains("No Record"))
                    wasde_label.Text = row["ERSToolActionLog_User"].ToString() + " streamed WASDE records on " + row["ERSToolActionLog_time"] + ", data was up to date".ToString();

                else
                    wasde_label.Text = row["ERSToolActionLog_User"].ToString() + " unsuccessfully streamed WASDE records on " + row["ERSToolActionLog_time"].ToString();
            }

            foreach (DataRow row in dr3)
            {
                if (row["ERSToolActionLog_Desc"].ToString().Contains("Clicked"))
                    bls_label.Text = row["ERSToolActionLog_User"].ToString() + " started streaming BLS records on " + row["ERSToolActionLog_time"].ToString();

                else if (row["ERSToolActionLog_Desc"].ToString().Contains("Successfull"))
                    bls_label.Text = row["ERSToolActionLog_User"].ToString() + " successfully streamed BLS records on " + row["ERSToolActionLog_time"].ToString();

                else if (row["ERSToolActionLog_Desc"].ToString().Contains("No Record"))
                    bls_label.Text = row["ERSToolActionLog_User"].ToString() + " streamed BLS records on " + row["ERSToolActionLog_time"] + ", data was up to date".ToString();

                else
                    bls_label.Text = row["ERSToolActionLog_User"].ToString() + " unsuccessfully streamed BLS records on " + row["ERSToolActionLog_time"].ToString();
            }

            foreach (DataRow row in dr4)
            {
                if (row["ERSToolActionLog_Desc"].ToString().Contains("Clicked"))
                    label10.Text = row["ERSToolActionLog_User"].ToString() + " started streaming Trade records on " + row["ERSToolActionLog_time"].ToString();

                else if (row["ERSToolActionLog_Desc"].ToString().Contains("Successfull"))
                    label10.Text = row["ERSToolActionLog_User"].ToString() + " successfully streamed Trade records on " + row["ERSToolActionLog_time"].ToString();

                else if (row["ERSToolActionLog_Desc"].ToString().Contains("No Record"))
                    label10.Text = row["ERSToolActionLog_User"].ToString() + " streamed Trade records on " + row["ERSToolActionLog_time"] + ", data was up to date".ToString();

                else
                    label10.Text = row["ERSToolActionLog_User"].ToString() + " unsuccessfully streamed Trade records on " + row["ERSToolActionLog_time"].ToString();
            }

        }

        private void disablebuttonsStreamData()
        {
            unsuccess_Label.Visible = false;
            Success_Label.Visible = false;
            rowsretrievedlabel.Visible = false;
            Success_Label.Refresh();
            label_dataretrievalresult.Visible = false;
            label_rowsretrieved.Visible = false;

        }

        private void button_back_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Parent = null;
            this.Controls.Clear();
            this.InitializeComponent();


        }
        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void NASS_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker.ReportProgress(1);
            string parameter = string.Empty;
            string connectionString = "";
            saveAction("Clicked NASS streaming");
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            }
            catch (Exception ex)
            {
                backgroundWorker.ReportProgress(0);
                MessageBox.Show("Data streaming unsuccessful, please contact technical team.");
                //saveAction(ex.Message);
                return;
            }

            string commandText = schemaName+"sp_NASS_Streaming";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(commandText, conn);
                //Indefinite timeout
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                //For AP
               // parameter = "1";
               // cmd.Parameters.Add("@sector_id", SqlDbType.NVarChar, 160).Value = parameter;
                try
                {
                    conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        backgroundWorker.ReportProgress(2);
                        MessageBox.Show(count + " NASS records streamed Successfully");
                        saveAction(count + " NASS records streamed Successfully");

                    }
                    else
                    {
                        backgroundWorker.ReportProgress(2);
                        MessageBox.Show("No new records found");
                        saveAction("NASS:No new records found");

                    }

                }

                catch (Exception ex)
                {
                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("Data streaming unsuccessful, please contact technical team. Error Details: \n" + ex.Message);

                    saveAction("NASS:" + ex.Message);
                }

                finally
                {
                    conn.Close();
                }


            }
        }


        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void NASS_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                NASS_button.Enabled = false;
                WASDE_button.Enabled = false;
                BLS_button.Enabled = false;

                streamForm.Show(this);
            }
            if (e.ProgressPercentage == 0)
            {
                NASS_button.Enabled = true;
                WASDE_button.Enabled = true;
                BLS_button.Enabled = true;
                streamForm.Hide();
                unsuccess_Label.Visible = true;
                Success_Label.Visible = false;
                label_dataretrievalresult.Visible = true;

            }
            if (e.ProgressPercentage == 2)
            {
                NASS_button.Enabled = true;
                WASDE_button.Enabled = true;
                BLS_button.Enabled = true;
                streamForm.Hide();
                Success_Label.Visible = true;

            }
        }

        private void NASS_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to stream latest data from the source. This process will take few minutes. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Background worker thread that executed stored procedures on sepreate thread to increase performance.
                backgroundWorker = new BackgroundWorker();

               
                //Method call to execute SP
                backgroundWorker.DoWork += new DoWorkEventHandler(NASS_DoWork);
                //Method call to track progress and wait form
                backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(NASS_ProgressChanged);

                backgroundWorker.WorkerReportsProgress = true;

                backgroundWorker.RunWorkerAsync();

            }
            //MessageBox.Show("Data streaming is under development and testing.");
            //unsuccess_Label.Visible = true;
            //Success_Label.Visible = false;
            //label_dataretrievalresult.Visible = true;
        }


        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void Trade_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker.ReportProgress(1);
            string connectionString = "";
            saveAction("Clicked Trade streaming");
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            }
            catch (Exception ex)
            {
                backgroundWorker.ReportProgress(0);
                MessageBox.Show("Data streaming unsuccessful, please contact technical team.");
                //saveAction(ex.Message);
                return;
            }

            string commandText = schemaName+"sp_Trade_Streaming";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(commandText, conn);
                //Indefinite timeout
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        backgroundWorker.ReportProgress(2);
                        MessageBox.Show(count + " Trade records streamed Successfully");
                        saveAction(count + " Trade records streamed Successfully");

                    }
                    else
                    {
                        backgroundWorker.ReportProgress(2);
                        MessageBox.Show("No new records found");
                        saveAction("Trade: No new records found");
                    }

                }

                catch (Exception ex)
                {
                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("Data streaming unsuccessful, please contact technical team. Error Details: \n" + ex.Message);
                    saveAction("Trade:" + ex.Message);
                }

                finally
                {
                    conn.Close();
                }


            }
        }


        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void Trade_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                streamForm.Show(this);
            }
            if (e.ProgressPercentage == 0)
            {
                streamForm.Hide();
                unsuccess_Label.Visible = true;
                Success_Label.Visible = false;
                label_dataretrievalresult.Visible = true;

            }
            if (e.ProgressPercentage == 2)
            {
                streamForm.Hide();
                Success_Label.Visible = true;

            }
        }

        private void Trade_button_Click(object sender, EventArgs e)
        {
            //Background worker thread that executed stored procedures on sepreate thread to increase performance.
            if (MessageBox.Show("You are about to stream latest data from the source. This process will take few minutes. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                backgroundWorker = new BackgroundWorker();
                //Method call to execute SP
                backgroundWorker.DoWork += new DoWorkEventHandler(Trade_DoWork);
                //Method call to track progress and wait form
                backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(Trade_ProgressChanged);

                backgroundWorker.WorkerReportsProgress = true;

                backgroundWorker.RunWorkerAsync();
                //MessageBox.Show("Data streaming is under development and testing.");
                //unsuccess_Label.Visible = true;
                //Success_Label.Visible = false;
                //label_dataretrievalresult.Visible = true;
            }
        }

        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void BLS_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker.ReportProgress(1);
            string connectionString = "";
            saveAction("Clicked BLS streaming");
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            }
            catch (Exception ex)
            {
                backgroundWorker.ReportProgress(0);
                MessageBox.Show("Data streaming unsuccessful, please contact technical team.");
                //saveAction(ex.Message);
                return;
            }

            string commandText = schemaName+"sp_BLS_Streaming";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(commandText, conn);
                //Indefinite timeout
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        backgroundWorker.ReportProgress(2);
                        MessageBox.Show(count + " BLS records streamed Successfully");
                        saveAction(count + " BLS records streamed Successfully");

                    }
                    else
                    {
                        backgroundWorker.ReportProgress(2);
                        MessageBox.Show("No new records found");
                        saveAction("BLS: No new records found");
                    }

                }

                catch (Exception ex)
                {
                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("Data streaming unsuccessful, please contact technical team. Error Details: \n" + ex.Message);

                    saveAction("BLS:" + ex.Message);
                }

                finally
                {
                    conn.Close();
                }


            }
        }


        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void BLS_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                NASS_button.Enabled = false;
                WASDE_button.Enabled = false;
                BLS_button.Enabled = false;
                streamForm.Show(this);
            }
            if (e.ProgressPercentage == 0)
            {
                NASS_button.Enabled = true;
                WASDE_button.Enabled = true;
                BLS_button.Enabled = true;
                streamForm.Hide();
                unsuccess_Label.Visible = true;
                Success_Label.Visible = false;
                label_dataretrievalresult.Visible = true;

            }
            if (e.ProgressPercentage == 2)
            {
                NASS_button.Enabled = true;
                WASDE_button.Enabled = true;
                BLS_button.Enabled = true;
                streamForm.Hide();
                Success_Label.Visible = true;

            }
        }

        private void BLS_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to stream latest data from the source. This process will take few minutes. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Background worker thread that executed stored procedures on sepreate thread to increase performance.
                backgroundWorker = new BackgroundWorker();
                
                //Method call to execute SP
                backgroundWorker.DoWork += new DoWorkEventHandler(BLS_DoWork);
                //Method call to track progress and wait form
                backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(BLS_ProgressChanged);

                backgroundWorker.WorkerReportsProgress = true;

                backgroundWorker.RunWorkerAsync();

            }

            //MessageBox.Show("Data streaming is under development and testing.");
            //unsuccess_Label.Visible = true;
            //Success_Label.Visible = false;
            //label_dataretrievalresult.Visible = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Data streaming is under development and testing.");
            unsuccess_Label.Visible = true;
            Success_Label.Visible = false;
            label_dataretrievalresult.Visible = true;
        }

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
        private DataTable GetData(string selectCommand)
        {
            try
            {
                // Specify a connection string. Replace the given value with a 
                // valid connection string for a Northwind SQL Server sample
                // database accessible to your system.
                //             String connectionString =
                //               "Integrated Security=SSPI;Persist Security Info=False;" +
                //             "Initial Catalog=Northwind;Data Source=localhost";
                connection();

                Console.WriteLine(selectCommand);
                // Create a new data adapter based on the specified query.
                SDA = new SqlDataAdapter(selectCommand, con);


                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(SDA);


                DataTable result = new DataTable();
                SDA.Fill(result);

                // see whether get any results
                //  int resultCount = result.Rows.Count;
                //Console.WriteLine(resultCount);

                return result;

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                return null;
            }
        }

        private void connection()
        {
            // connection string for linking db
            sqlconn = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            // init the connection to db
            con = new SqlConnection(sqlconn);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Success_Label_Click(object sender, EventArgs e)
        {

        }

        private void unsuccess_Label_Click(object sender, EventArgs e)
        {

        }

        private void label_dataretrievalresult_Click(object sender, EventArgs e)
        {

        }

        private void label_rowsretrieved_Click(object sender, EventArgs e)
        {

        }

        private void label_PageHeading_Click(object sender, EventArgs e)
        {

        }

        private void label_Path_Click(object sender, EventArgs e)
        {

        }

        private void label_ApCoSD_Click(object sender, EventArgs e)
        {

        }

        private void AMS_button_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Data streaming is under development and testing.");
            unsuccess_Label.Visible = true;
            label_dataretrievalresult.Visible = true;
            Success_Label.Visible = false;
        }

        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void WASDE_DoWork(object sender, DoWorkEventArgs e)
        {
            if (MessageBox.Show("You are about to stream latest data from the source. This process will take few minutes. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                backgroundWorker.ReportProgress(1);
            string parameter = string.Empty;
            string connectionString = "";
            saveAction("Clicked WASDE streaming");
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            }
            catch (Exception ex)
            {
                backgroundWorker.ReportProgress(0);
                MessageBox.Show("Data streaming unsuccessful, please contact technical team.");
                //saveAction(ex.Message);
                return;
            }

            string commandText = "CoSD.sp_WASDE_Streaming";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    //Indefinite timeout
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    //For AP
                    parameter = "1";
                    cmd.Parameters.Add("@paramComm", SqlDbType.NVarChar, 160).Value = parameter;
                    try
                    {
                        conn.Open();
                        int count = cmd.ExecuteNonQuery();
                        if (count > 0)
                        {
                            backgroundWorker.ReportProgress(2);
                            MessageBox.Show(count + " WASDE records streamed Successfully");
                            saveAction(count + " WASDE records streamed Successfully");

                        }
                        else
                        {
                            backgroundWorker.ReportProgress(2);
                            MessageBox.Show("No new records found");
                            saveAction("WASDE: No new records found");
                        }

                    }

                    catch (Exception ex)
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show("Data streaming unsuccessful, please contact technical team. Error Details: \n" + ex.Message);

                        saveAction("WASDE:" + ex.Message);
                        //saveAction(ex.Message);
                    }

                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }


        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void WASDE_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                NASS_button.Enabled = false;
                WASDE_button.Enabled = false;
                BLS_button.Enabled = false;
                streamForm.Show(this);
            }
            if (e.ProgressPercentage == 0)
            {
                NASS_button.Enabled = true;
                WASDE_button.Enabled = true;
                BLS_button.Enabled = true;
                streamForm.Hide();
                unsuccess_Label.Visible = true;
                Success_Label.Visible = false;
                label_dataretrievalresult.Visible = true;

            }
            if (e.ProgressPercentage == 2)
            {
                NASS_button.Enabled = true;
                WASDE_button.Enabled = true;
                BLS_button.Enabled = true;
                streamForm.Hide();
                Success_Label.Visible = true;

            }
        }

        private void WASDE_button_Click(object sender, EventArgs e)
        {
            //Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();
           
            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(WASDE_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(WASDE_ProgressChanged);

            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();


            //MessageBox.Show("Data streaming is under development and testing.");
            //unsuccess_Label.Visible = true;
            //Success_Label.Visible = false;
            //label_dataretrievalresult.Visible = true;
        }

        private void StreamData_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void wasde_label_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
