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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoSD_Tool
{
    public partial class  RepoManagement: Form
    {

        string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        WaitForm waitForm = new WaitForm();
        private string sqlconn;  // query and sql connection
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private SqlConnection con;    // db connection
        int count = 0;

        public RepoManagement()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
        }

        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void getMasterToCosd_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker.ReportProgress(1);
            string commandText = "";// "CoSD.sp_master_to_ap";
            count = 0;
            if(schemaName.Equals("AnimalProductsCoSD.CoSD."))
            {
                commandText = schemaName + "sp_master_to_ap";
            }
            if (schemaName.Equals("FruitCoSD.CoSD."))
            {
                commandText = schemaName + "sp_master_to_fruit";
            }
            if (schemaName.Equals("VegetablesCoSD.CoSD."))
            {
                commandText = schemaName + "sp_master_to_veg";
            }
           
            string connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
           

            

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(commandText, conn);
                //Indefinite timeout
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                var returnParam = new SqlParameter
                {
                    ParameterName = "@Return",
                    Direction = ParameterDirection.ReturnValue,
                };
                cmd.Parameters.Add(returnParam);

                try
                {
                    conn.Open();
                    count = cmd.ExecuteNonQuery();
                    int retunvalue = (int)cmd.Parameters["@Return"].Value;
                    if (count > 0 && retunvalue == 1)
                    {                                                
                        saveAction("Master to CoSD. " + count + " Records processed Successfully");
                        backgroundWorker.ReportProgress(2);

                    }
                    else
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show("No Records processed.");

                    }

                }

                catch (Exception ex)
                {
                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                    saveAction("Master to CoSD: " + ex.Message);

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
        private void waitForm_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (e.ProgressPercentage == 1)
            {
                waitForm.ShowDialog(this);
            }
            else if (e.ProgressPercentage == 0)
            {
                waitForm.Hide();
            }
            else if (e.ProgressPercentage == 2)
            {
                waitForm.Hide();
                MessageBox.Show(count + " Records processed Successfully");
            }
        }
        private void CoSDdoc_Button_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();
            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(getMasterToCosd_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler
                 (waitForm_ProgressChanged);
            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();

            //m_oWorker.CancelAsync();
        }

        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void CoSDtoRepository_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker.ReportProgress(1);
            count = 0;

            string connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            string commandText = "";
            if (schemaName.Equals("AnimalProductsCoSD.CoSD."))
            {
                commandText = schemaName + "sp_ap_data_to_repository";
            }
            if (schemaName.Equals("FruitCoSD.CoSD."))
            {
                commandText = schemaName + "sp_master_to_fruit";
            }
            if (schemaName.Equals("VegetablesCoSD.CoSD."))
            {
                commandText = schemaName + "sp_veg_data_to_repository";
            }
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(commandText, conn);
                //Indefinite timeout
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                var returnParam = new SqlParameter
                {
                    ParameterName = "@Return",
                    Direction = ParameterDirection.ReturnValue,
                };
                cmd.Parameters.Add(returnParam);
                try
                {
                    conn.Open();
                    count = cmd.ExecuteNonQuery();
                    int retunvalue = (int)cmd.Parameters["@Return"].Value;
                    if (count > 0 && retunvalue==1)
                    {
                        saveAction("Master to Repository. " + count + " Records processed Successfully");
                        backgroundWorker.ReportProgress(2);                                                
                    }
                    else
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show("No Records processed.");
                    }

                }

                catch (Exception ex)
                {
                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                    saveAction("Master to Repository: " + ex.Message);
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
        private void CoSDtoRepository_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                waitForm.ShowDialog(this);
            }
            else if (e.ProgressPercentage == 0)
            {
                waitForm.Hide();

            }
            else if (e.ProgressPercentage == 2)
            {
                waitForm.Hide();
                MessageBox.Show(count + " Records processed Successfully");
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();
            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(CoSDtoRepository_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(CoSDtoRepository_ProgressChanged);

            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();
        }
        // record user action
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


        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="selectCommand">The select command.</param>
        private DataTable GetData(string selectCommand)
        {
            try
            {
                connection();

                Console.WriteLine(selectCommand);
                // Create a new data adapter based on the specified query.
                dataAdapter = new SqlDataAdapter(selectCommand, con);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);


                DataTable result = new DataTable();
                dataAdapter.Fill(result);

                // see whether get any results
                //  int resultCount = result.Rows.Count;
                //Console.WriteLine(resultCount);

                return result;

            }
            catch (SqlException ex)
            {
                return null;
            }
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

        private void CC_Button_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();
            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(getCCToCoSD_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler
                 (waitForm_ProgressChanged);
            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();
        }

        private void getCCToCoSD_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker.ReportProgress(1);
            count = 0;
            string schema = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
            //string connectionString = ConfigurationManager.ConnectionStrings["Master_CoSD_Tool.Properties.Settings.MasterCoSDConnectionString"].ConnectionString;
            string connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            string commandText = " ";


            if (schema.Equals("AnimalProductsCoSD.CoSD."))
            {
                commandText = schema+"sp_cc_to_ap";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    //Indefinite timeout
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    var returnParam = new SqlParameter
                    {
                        ParameterName = "@Return",
                        Direction = ParameterDirection.ReturnValue,
                    };
                    cmd.Parameters.Add(returnParam);

                    try
                    {
                        conn.Open();
                        count = cmd.ExecuteNonQuery();
                        int retunvalue = (int)cmd.Parameters["@Return"].Value;
                        if (count > 0 && retunvalue == 1)
                        {
                            saveAction("CC to AP. " + count + " Records processed Successfully");
                            backgroundWorker.ReportProgress(2);

                        }
                        else
                        {
                            backgroundWorker.ReportProgress(0);
                            MessageBox.Show("No Records processed.");

                        }

                    }

                    catch (Exception ex)
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                        saveAction("CC to AP: " + ex.Message);

                    }

                    finally
                    {
                        conn.Close();
                    }


                }
            }

            if (schema.Equals("VegetablesCoSD.CoSD."))
            {
               
                commandText = schema+"sp_cc_to_veg";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    //Indefinite timeout
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    var returnParam = new SqlParameter
                    {
                        ParameterName = "@Return",
                        Direction = ParameterDirection.ReturnValue,
                    };
                    cmd.Parameters.Add(returnParam);

                    try
                    {
                        conn.Open();
                        count = cmd.ExecuteNonQuery();
                        int retunvalue = (int)cmd.Parameters["@Return"].Value;
                        if (count > 0 && retunvalue == 1)
                        {
                            saveAction("CC to AP. " + count + " Records processed Successfully");
                            backgroundWorker.ReportProgress(2);

                        }
                        else
                        {
                            backgroundWorker.ReportProgress(0);
                            MessageBox.Show("No Records processed.");

                        }

                    }

                    catch (Exception ex)
                    {
                        backgroundWorker.ReportProgress(0);
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                        saveAction("CC to Veg: " + ex.Message);

                    }

                    finally
                    {
                        conn.Close();
                    }


                }
            }

            if(schema.Equals("FruitsCoSD.CoSD."))
            {
                MessageBox.Show("The procedure for this Commodity does not exist");
            }
        }

    }
}
