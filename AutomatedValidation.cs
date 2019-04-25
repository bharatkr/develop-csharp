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
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;


namespace CoSD_Tool
{
    public partial class AutomatedValidation : Form
    {
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString);
        private string sqlconn;  // query and sql connection
        public BusinessRulesInsertForm businessrulesinsertForm = new BusinessRulesInsertForm();
        /// <summary>
        /// The data adapter
        /// </summary>
        private SqlDataAdapter SDA = new SqlDataAdapter();
        private static SqlDataAdapter SDA1;
        string apGroupId = Home.globalapGroupId;
        private List<string> groupID;
        /// <summary>
        /// The table
        /// </summary>
        DataTable DT = new DataTable();
        SqlCommandBuilder scb = new SqlCommandBuilder();
        private string schemaName = Home.globalSchemaName;
        WaitForm waitForm = new WaitForm();
        string cosdFolderPath = Home.globalCoSDPath;
        string cosdSettingsPath = Home.globalCoSDPath;
        Boolean flagSystemSuccess = false;
        BackgroundWorker backgroundWorker;

        Boolean summaryStatisticsSuccess = false;

        String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");

        public AutomatedValidation()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Parent = null;
            flagSystemSuccess = false;
            summaryStatisticsSuccess = false;
            validationLabel.Visible = false;
            validateData_button.Visible = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void summaryStatistics_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (MessageBox.Show("This process may take more than 2 minutes. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Invoke(new MethodInvoker(delegate {
                        disable_buttons();

                    }));
                    backgroundWorker.ReportProgress(1);
                    String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");
                    DataSet ds = new DataSet();
                    //StoredProcedure Execution for the validation engine
                    string connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
                    string commandText = "CoSD.ValidationEngine-SummaryStatistics";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(commandText, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        try
                        {
                            conn.Open();
                            SDA.SelectCommand = cmd;
                            SDA.Fill(ds);
                            ds.Tables[0].TableName = "Data_values";
                            ds.Tables[1].TableName = "Cons_vars";
                            ds.Tables[2].TableName = "Conv_factors";
                            ds.Tables[3].TableName = "Data_series";
                         
                        }
                        catch (SqlException ex)
                        {
                            backgroundWorker.ReportProgress(0);
                            summaryStatisticsSuccess = false;
                            MessageBox.Show("Validation failed please contact ERS Technical Team");
                            //saveAction(ex.Message);
                            Stats_button.Text = "Summary Statistics Validation";
                            return;
                        }
                        finally
                        {
                            conn.Close();
                        }


                        if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Validation Results")) // name of the error file and location
                        {
                            Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Validation Results");
                        }

                        Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                        if (xlApp == null)
                        {
                            backgroundWorker.ReportProgress(0);
                            summaryStatisticsSuccess = false;
                            MessageBox.Show("Excel is not properly installed!!");
                            return;
                        }

                        try
                        {
                            Excel.Workbook xlWorkBook;
                            Excel.Worksheet xlWorkSheet;
                            object misValue = System.Reflection.Missing.Value;
                            xlWorkBook = xlApp.Workbooks.Add(misValue);
                            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                            xlWorkBook.SaveAs(cosdFolderPath + "CoSD Tool\\Validation Results\\SummaryStatistics" + "_" + Date + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                            string filePath = @cosdFolderPath + "CoSD Tool\\Validation Results\\SummaryStatistics" + "_" + Date + ".xls";
                            xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                            //It supperesses the Microsoft Compatibility message
                            xlWorkBook.CheckCompatibility = false;
                            foreach (DataTable table in ds.Tables)
                            {

                                Excel.Worksheet excelWorkSheet = xlWorkBook.Sheets.Add();
                                excelWorkSheet.Name = table.TableName;

                                if (table.TableName == "Data_values")
                                {
                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet.Cells[2, i] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet.get_Range("A2");
                                        formatRange.EntireRow.Font.Bold = true;

                                    }

                                }

                                if (table.TableName == "Cons_vars")
                                {
                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet.Cells[2, i] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet.get_Range("A2");
                                        formatRange.EntireRow.Font.Bold = true;

                                    }

                                }


                                if (table.TableName == "Conv_factors")
                                {
                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet.Cells[2, i] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet.get_Range("A2");
                                        formatRange.EntireRow.Font.Bold = true;

                                    }

                                }


                                if (table.TableName == "Data_series")
                                {
                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet.Cells[2, i] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet.get_Range("A2");
                                        formatRange.EntireRow.Font.Bold = true;

                                    }

                                }


                                if (table.Rows.Count == 0)
                                {
                                    excelWorkSheet.Cells[4, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    excelWorkSheet.get_Range("B4", "E4").Merge();
                                    excelWorkSheet.Cells[4, 2] = excelWorkSheet.get_Range("B4", "E4");
                                    excelWorkSheet.Cells[4, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                    excelWorkSheet.Cells[4, 2] = "No data found";

                                }

                                if (table.TableName == "Data_values")
                                {
                                    int l = 3;
                                    excelWorkSheet.get_Range("A1", "C1").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet.Cells[1, 1] = "Summary statistics for data values";

                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet.Cells[l, k + 1] = table.Rows[j].ItemArray[k].ToString();
                                            excelWorkSheet.Cells[l, k + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                        }
                                        l = l + 1;
                                    }

                                }

                                if (table.TableName == "Cons_vars")
                                {
                                    int l = 3;
                                    excelWorkSheet.get_Range("A1", "C1").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet.Cells[1, 1] = "Summary statistics  for constructed variables";

                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet.Cells[l, k + 1] = table.Rows[j].ItemArray[k].ToString();
                                            excelWorkSheet.Cells[l, k + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                        }
                                        l = l + 1;
                                    }

                                }

                                if (table.TableName == "Conv_factors")
                                {
                                    int l = 3;
                                    excelWorkSheet.get_Range("A1", "C1").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet.Cells[1, 1] = "Summary statistics  for conversion factors";

                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet.Cells[l, k + 1] = table.Rows[j].ItemArray[k].ToString();
                                            excelWorkSheet.Cells[l, k + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                        }
                                        l = l + 1;
                                    }

                                }

                                if (table.TableName == "Data_series")
                                {
                                    int l = 3;
                                    excelWorkSheet.get_Range("A1", "C1").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet.Cells[1, 1] = "List of data series per commodity";

                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet.Cells[l, k + 1] = table.Rows[j].ItemArray[k].ToString();
                                            excelWorkSheet.Cells[l, k + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                        }
                                        l = l + 1;
                                    }

                                }


                            }

                            xlWorkBook.Sheets["Sheet1"].Delete();
                            xlWorkBook.Close(true, misValue, misValue);
                            xlApp.Quit();

                            releaseObject(xlWorkSheet);
                            releaseObject(xlWorkBook);
                            releaseObject(xlApp);
                            this.Invoke(new MethodInvoker(delegate {
                                enable_buttons();

                            }));
                        }
                        catch (Exception ex)
                        {
                            backgroundWorker.ReportProgress(0);
                            summaryStatisticsSuccess = false;
                            MessageBox.Show("Data processing unsuccessful. Please contact technical team.");
                            return;
                        }
                    }
                    backgroundWorker.ReportProgress(2);
                    summaryStatisticsSuccess = true;



                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void summaryStatistics_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                Stats_button.Text = "Summary Statictics Processing...";

                button_flagsystem.Enabled = false;
                button_technicalvalidation.Enabled = false;
                Stats_button.Enabled = false;
                ActionLog_button.Enabled = false;

                Stats_button.Refresh();
                waitForm.ShowDialog(this);
            }
            if (e.ProgressPercentage == 0)
            {
                Stats_button.Text = "Summary Statistics Validation";

                button_flagsystem.Enabled = true;
                button_technicalvalidation.Enabled = true;
                Stats_button.Enabled = true;
                ActionLog_button.Enabled = true;

                waitForm.Hide();

            }
            if (e.ProgressPercentage == 2)
            {
                button_flagsystem.Enabled = true;
                button_technicalvalidation.Enabled = true;
                Stats_button.Enabled = true;
                ActionLog_button.Enabled = true;

                Stats_button.Text = "Summary Statistics Validation";
                label1.Text = cosdFolderPath + "CoSD Tool\\Validation Results\\SummaryStatistics" + "_" + Date + ".xls";
                if (flagSystemSuccess == true || summaryStatisticsSuccess == true)
                {
                    validateData_button.Visible = true;
                    validationLabel.Visible = true;
                }
                waitForm.Hide();
                MessageBox.Show("Summary Statistics Excel file created Successfully ");

            }
        }

        private void Stats_button_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();


            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(summaryStatistics_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(summaryStatistics_ProgressChanged);

            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();
            button_back.Enabled = true;
            button_ChangePath.Enabled = true;



        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void saveAction(string action)
        {
            // get current user            
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();

            action = action.Replace("'", "''");

            string sql = "insert into " + schemaName + "ERSTool_ActionLog ( ERSToolActionLog_time,ERSToolActionLog_User,ERSToolActionLog_Desc) " +
                "values (SYSDATETIME(), " +
               "'" + currentUser.Name + "', " +
            "'''" + action +
            "''')";
            GetData(sql);
        }

        private void connection()
        {
            try
            {
                // connection string for linking db
                sqlconn = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
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
                throw new Exception("Data processing unsuccessful, please contact technical team.");

            }
        }

        /** Commented below code for future reference
        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private string actionLog_DoWork()
        {

                String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");
                DataSet ds = new DataSet();
                try
                {
                    con.Open();
                    SDA1 = new SqlDataAdapter(@"SELECT [ERSToolActionLog_time],[ERSToolActionLog_User],[ERSToolActionLog_Desc] FROM " + schemaName + "[ERSTool_ActionLog]", con);
                    SDA1.SelectCommand.ExecuteNonQuery();
                    
                    DT = new DataTable();
                    SDA1.Fill(ds);
                    ds.Tables[0].TableName = "Action_Log";

                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("cancelled"))
                        return "Data processing cancelled by User";
                    return "Data processing unsuccessful, please contact technical team.";
                }
                finally
                {
                    con.Close();
                }

                if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Action Log")) // name of the error file and location
                {
                    Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Action Log");
                }

                Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                if (xlApp == null)
                {
                    return "Excel is not properly installed!!";
                }
                try
                {
                    Excel.Workbook xlWorkBook;
                    Excel.Worksheet xlWorkSheet;
                    object misValue = System.Reflection.Missing.Value;
                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    xlWorkBook.SaveAs(cosdFolderPath + "CoSD Tool\\Action Log\\Action Log" + "_" + Date + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                    string filePath = @cosdFolderPath + "CoSD Tool\\Action Log\\Action Log" + "_" + Date + ".xls";
                    xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                    foreach (DataTable table in ds.Tables)
                    {

                        Excel.Worksheet excelWorkSheet = xlWorkBook.Sheets.Add();
                        excelWorkSheet.Name = table.TableName;

                        for (int i = 1; i < table.Columns.Count + 1; i++)
                        {
                            excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                            Excel.Range formatRange;
                            formatRange = excelWorkSheet.get_Range("a1");
                            formatRange.EntireRow.Font.Bold = true;

                        }

                        for (int j = 0; j < table.Rows.Count; j++)
                        {
                            for (int k = 0; k < table.Columns.Count; k++)
                            {
                                excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                            }
                        }
                    }

                    xlWorkBook.Sheets["Sheet1"].Delete();
                    xlWorkBook.Close(true, misValue, misValue);
                    xlApp.Quit();

                    releaseObject(xlWorkSheet);
                    releaseObject(xlWorkBook);
                    releaseObject(xlApp);
                }
                catch (Exception ex)
                {
                    return "There was a problem in creating excel file. Please contact the technical team";
                }
                return "Action Log Excel file created Successfully";
                
            
        }
        private async void CallactionLogAsync()
        {
            try
            {
                if (MessageBox.Show("This process may take more than 2 minutes. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    waitForm.ShowDialog(this);
                    ActionLog_button.Text = " Action Log Processing...";
                    var result = await actionLogAsync();
                    waitForm.Hide();
                    ActionLog_button.Text = "Extract Action Log";
                    if (result.Equals("Action Log Excel file created Successfully"))
                    {
                        label1.Text = cosdFolderPath + "CoSD Tool\\Action Log\\Action Log" + "_" + Date + ".xls";
                        ActionLog_button.Text = "Extract Action Log";
                    }
                    MessageBox.Show(result);
                }
            }catch(Exception ex)
            {
                return;
            }
        }

        private Task<string> actionLogAsync()
        {
            return Task.Run(() => actionLog_DoWork());
        }
        private void ActionLog_button_Click(object sender, EventArgs e)
        {
                CallactionLogAsync();
        }
        **/
        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void actionLog_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (MessageBox.Show("This process may take more than 2 minutes. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Invoke(new MethodInvoker(delegate {
                        disable_buttons();

                    }));
                    backgroundWorker.ReportProgress(1);
                    String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");
                    DataSet ds = new DataSet();
                    try
                    {
                        con.Open();
                        SDA1 = new SqlDataAdapter(@"SELECT TOP 3000 [ERSToolActionLog_ID], [ERSToolActionLog_time],[ERSToolActionLog_User],[ERSToolActionLog_Desc] FROM " + schemaName + "[ERSTool_ActionLog] order by [ERSToolActionLog_ID] desc", con);
                        SDA1.SelectCommand.ExecuteNonQuery();

                        DT = new DataTable();
                        SDA1.Fill(ds);
                        ds.Tables[0].TableName = "Action_Log";
                      

                    }
                    catch (Exception ex)
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                        return;
                    }
                    finally
                    {
                        con.Close();
                   
                    }

                    if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Action Log")) // name of the error file and location
                    {
                        Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Action Log");
                    }

                    Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                    if (xlApp == null)
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show("Excel is not properly installed!!");
                        return;
                    }
                    try
                    {
                        Excel.Workbook xlWorkBook;
                        Excel.Worksheet xlWorkSheet;
                        object misValue = System.Reflection.Missing.Value;

                        xlWorkBook = xlApp.Workbooks.Add(misValue);
                        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                        xlWorkBook.SaveAs(cosdFolderPath + "CoSD Tool\\Action Log\\Action Log" + "_" + Date + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                        string filePath = @cosdFolderPath + "CoSD Tool\\Action Log\\Action Log" + "_" + Date + ".xls";
                        xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                        foreach (DataTable table in ds.Tables)
                        {

                            Excel.Worksheet excelWorkSheet = xlWorkBook.Sheets.Add();
                            excelWorkSheet.Name = table.TableName;

                            for (int i = 1; i < table.Columns.Count + 1; i++)
                            {
                                excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                                Excel.Range formatRange;
                                formatRange = excelWorkSheet.get_Range("a1");
                                formatRange.EntireRow.Font.Bold = true;

                            }

                            for (int j = 0; j < table.Rows.Count; j++)
                            {
                                for (int k = 0; k < table.Columns.Count; k++)
                                {
                                    excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                                }
                            }
                        }

                        xlWorkBook.Sheets[2].Delete();
                        xlWorkBook.Close(true, misValue, misValue);
                        xlApp.Quit();

                        releaseObject(xlWorkSheet);
                        releaseObject(xlWorkBook);
                        releaseObject(xlApp);
                        this.Invoke(new MethodInvoker(delegate {
                            enable_buttons();

                        }));
                    }
                    catch (Exception ex)
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show("There was a problem in creating excel file. Please contact the technical team");
                        return;
                    }
                    //success
                    backgroundWorker.ReportProgress(2);
                }
            }
            catch (Exception ex)
            {
                backgroundWorker.ReportProgress(0);
                return;
            }
        }
        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void actionLog_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (e.ProgressPercentage == 1)
            {
                ActionLog_button.Text = " Action Log Processing...";

                button_flagsystem.Enabled = false;
                button_technicalvalidation.Enabled = false;
                Stats_button.Enabled = false;
                ActionLog_button.Enabled = false;

                ActionLog_button.Refresh();
                waitForm.ShowDialog(this);
            }
            if (e.ProgressPercentage == 0)
            {
                ActionLog_button.Text = "Extract Action Log";

                button_flagsystem.Enabled = true;
                button_technicalvalidation.Enabled = true;
                Stats_button.Enabled = true;
                ActionLog_button.Enabled = true;

                ActionLog_button.Refresh();
                waitForm.Hide();

            }
            if (e.ProgressPercentage == 2)
            {
                button_flagsystem.Enabled = true;
                button_technicalvalidation.Enabled = true;
                Stats_button.Enabled = true;
                ActionLog_button.Enabled = true;

                ActionLog_button.Text = "Extract Action Log";
                label1.Text = cosdFolderPath + "CoSD Tool\\Action Log\\Action Log" + "_" + Date + ".xls";
                ActionLog_button.Refresh();
                waitForm.Hide();
                MessageBox.Show("Action Log Excel file created Successfully");

            }
        }

        private void ActionLog_button_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();

            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(actionLog_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(actionLog_ProgressChanged);

            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();
        

        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void ValidationForm_Load(object sender, EventArgs e)
        {
            if (DeSerializeSettingsPath<String>("CoSDSettings") != null)
                cosdFolderPath = DeSerializeSettingsPath<String>("CoSDSettings");
            if (!cosdFolderPath.EndsWith("\\"))
            {
                cosdFolderPath = cosdFolderPath + "\\";
            }
            label1.Text = cosdFolderPath + "CoSD Tool";
        }

        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void flagsystemValidation_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (MessageBox.Show("This process may take more than 2 minutes. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Invoke(new MethodInvoker(delegate {
                        disable_buttons();

                    }));
                    DataSet ds = new DataSet();
                    backgroundWorker.ReportProgress(1);
                    //StoredProcedure Execution for the validation engine
                    string connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
                    string commandText = "CoSD.ValidationEngine-Flagsystem";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(commandText, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        try
                        {
                            conn.Open();
                            SDA.SelectCommand = cmd;
                            SDA.Fill(ds);
                            ds.Tables[0].TableName = "ConsVar-Outliers";
                            ds.Tables[1].TableName = "DV-MonthlyValidation";
                            ds.Tables[2].TableName = "DV-QuarterlyValidation";
                            ds.Tables[3].TableName = "DV-YearlyValidation";

                        }
                        catch (SqlException ex)
                        {
                            backgroundWorker.ReportProgress(0);
                            flagSystemSuccess = false;
                            MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                            //saveAction(ex.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }


                        if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Validation Results")) // name of the error file and location
                        {
                            Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Validation Results");
                        }

                        Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                        if (xlApp == null)
                        {
                            backgroundWorker.ReportProgress(0);
                            flagSystemSuccess = false;
                            MessageBox.Show("Excel is not properly installed!!");
                            return;
                        }

                        try
                        {
                            Excel.Workbook xlWorkBook;
                            Excel.Worksheet xlWorkSheet;
                            object misValue = System.Reflection.Missing.Value;
                            xlWorkBook = xlApp.Workbooks.Add(misValue);
                            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                            xlWorkBook.SaveAs(cosdFolderPath + "CoSD Tool\\Validation Results\\FlagValidation" + "_" + Date + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                            string filePath = @cosdFolderPath + "CoSD Tool\\Validation Results\\FlagValidation" + "_" + Date + ".xls";
                            xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                            //It supperesses the Microsoft Compatibility message
                            xlWorkBook.CheckCompatibility = false;
                            foreach (DataTable table in ds.Tables)
                            {

                                Excel.Worksheet excelWorkSheet = xlWorkBook.Sheets.Add();
                                excelWorkSheet.Name = table.TableName;
                                if (table.TableName == "ConsVar-Outliers")
                                {
                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet.Cells[8, i] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet.get_Range("A8");
                                        formatRange.EntireRow.Font.Bold = true;

                                    }
                                }
                                else
                                {
                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet.get_Range("a1");
                                        formatRange.EntireRow.Font.Bold = true;

                                    }
                                }

                                if (table.Rows.Count == 0)
                                {
                                    if (table.TableName == "ConsVar-Outliers")
                                    {
                                        excelWorkSheet.Cells[4, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        excelWorkSheet.get_Range("B4", "E4").Merge();
                                        excelWorkSheet.Cells[4, 2] = excelWorkSheet.get_Range("B4", "E4");
                                        excelWorkSheet.Cells[4, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                        excelWorkSheet.Cells[4, 2] = "No Outliers found in constructed variables";
                                    }

                                    if (table.TableName == "DV-MonthlyValidation")
                                    {
                                        excelWorkSheet.Cells[4, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        excelWorkSheet.get_Range("B4", "E4").Merge();
                                        excelWorkSheet.Cells[4, 2] = excelWorkSheet.get_Range("B4", "E4");
                                        excelWorkSheet.Cells[4, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                        excelWorkSheet.Cells[4, 2] = "Monthly data not yet available";
                                    }



                                    if (table.TableName == "DV-QuarterlyValidation")
                                    {
                                        excelWorkSheet.Cells[4, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        excelWorkSheet.get_Range("B4", "E4").Merge();
                                        excelWorkSheet.Cells[4, 2] = excelWorkSheet.get_Range("B4", "E4");
                                        excelWorkSheet.Cells[4, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                        excelWorkSheet.Cells[4, 2] = "Quarterly data not yet available";
                                    }


                                    if (table.TableName == "DV-YearlyValidation")
                                    {
                                        excelWorkSheet.Cells[4, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        excelWorkSheet.get_Range("B4", "E4").Merge();
                                        excelWorkSheet.Cells[4, 2] = excelWorkSheet.get_Range("B4", "E4");
                                        excelWorkSheet.Cells[4, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                        excelWorkSheet.Cells[4, 2] = "Yearly data not yet available";
                                    }



                                }

                                if (table.TableName == "ConsVar-Outliers")
                                {
                                    int l = 9;
                                    excelWorkSheet.get_Range("A1", "L1").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet.Cells[1, 1] = "This sheet shows the potential outliers in constructed variable table and it will help you to identify the validity of business logic";
                                    excelWorkSheet.get_Range("A2", "L2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet.Cells[2, 1] = "The outlier is calculated based on Mean and standard deviation method";
                                    excelWorkSheet.get_Range("A3", "L3").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet.Cells[3, 1] = "Reference links:";
                                    excelWorkSheet.get_Range("A4", "L4").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet.Cells[4, 1] = "1.)";
                                    excelWorkSheet.get_Range("A5", "L5").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet.Cells[5, 1] = "2.)";
                                    excelWorkSheet.get_Range("A6", "L6").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet.Cells[6, 1] = "3.)";
                                    Excel.Range formatRange2;
                                    formatRange2 = excelWorkSheet.get_Range("B4");
                                    excelWorkSheet.Hyperlinks.Add(formatRange2, "https://docs.oracle.com/cd/E17236_01/epm.1112/cb_statistical/frameset.htm?ch07s02s10s01.html", Type.Missing, "https://docs.oracle.com/cd/E17236_01/epm.1112/cb_statistical/frameset.htm?ch07s02s10s01.html", "https://docs.oracle.com/cd/E17236_01/epm.1112/cb_statistical/frameset.htm?ch07s02s10s01.html");

                                    Excel.Range formatRange3;
                                    formatRange3 = excelWorkSheet.get_Range("B5");
                                    excelWorkSheet.Hyperlinks.Add(formatRange3, "http://dnr.wi.gov/regulations/labcert/documents/guidance/-outliers_proc.pdf", Type.Missing, "http://dnr.wi.gov/regulations/labcert/documents/guidance/-outliers_proc.pdf", "http://dnr.wi.gov/regulations/labcert/documents/guidance/-outliers_proc.pdf");

                                    Excel.Range formatRange4;
                                    formatRange4 = excelWorkSheet.get_Range("B6");
                                    excelWorkSheet.Hyperlinks.Add(formatRange4, "https://arxiv.org/ftp/arxiv/papers/0907/0907.5155.pdf", Type.Missing, "https://arxiv.org/ftp/arxiv/papers/0907/0907.5155.pdf", "https://arxiv.org/ftp/arxiv/papers/0907/0907.5155.pdf");


                                    Excel.Range formatRange1;
                                    formatRange1 = excelWorkSheet.get_Range("A3");
                                    formatRange1.EntireRow.Font.Bold = true;

                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet.Cells[l, k + 1] = table.Rows[j].ItemArray[k].ToString();

                                        }
                                        l = l + 1;
                                    }

                                }
                                else
                                {
                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();

                                        }
                                    }
                                }

                                if (table.TableName != "ConsVar-Outliers")
                                {

                                    for (int j = 2; j < table.Rows.Count + 2; j++)
                                    {
                                        var cellvalue = (string)(excelWorkSheet.Cells[j, 7] as Excel.Range).Value;
                                        if (cellvalue == "Red")
                                        {
                                            excelWorkSheet.Cells[j, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        }

                                        else if (cellvalue == "Blue")
                                        {
                                            excelWorkSheet.Cells[j, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Blue);

                                        }

                                        else if (cellvalue == "Yellow")
                                        {
                                            excelWorkSheet.Cells[j, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                        }

                                        else if (cellvalue == "Orange")
                                        {
                                            excelWorkSheet.Cells[j, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Orange);

                                        }

                                        else
                                        {
                                            excelWorkSheet.Cells[j, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Green);
                                        }

                                    }
                                }

                            }

                            xlWorkBook.Sheets["Sheet1"].Delete();
                            xlWorkBook.Close(true, misValue, misValue);
                            xlApp.Quit();

                            releaseObject(xlWorkSheet);
                            releaseObject(xlWorkBook);
                            releaseObject(xlApp);
                            this.Invoke(new MethodInvoker(delegate {
                                enable_buttons();

                            }));
                        }
                        catch (Exception ex)
                        {
                            backgroundWorker.ReportProgress(0);
                            flagSystemSuccess = false;
                            MessageBox.Show(" There was a problem in creating excel file. Please contact the techincal team ");
                            return;
                        }
                    }
                    //Success
                    backgroundWorker.ReportProgress(2);
                    flagSystemSuccess = true;
                    if (flagSystemSuccess == true && summaryStatisticsSuccess == true)
                    {
                        validateData_button.Visible = true;
                        validationLabel.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void flagsystemValidation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (e.ProgressPercentage == 1)
            {
                button_flagsystem.Text = "Flag Validation Processing...";


                button_flagsystem.Enabled = false;
                button_technicalvalidation.Enabled = false;
                Stats_button.Enabled = false;
                ActionLog_button.Enabled = false;

                button_flagsystem.Refresh();
                waitForm.ShowDialog(this);
            }
            if (e.ProgressPercentage == 0)
            {
                button_flagsystem.Text = "Flag System Validaton";

                button_flagsystem.Enabled = true;
                button_technicalvalidation.Enabled = true;
                Stats_button.Enabled = true;
                ActionLog_button.Enabled = true;

                button_flagsystem.Refresh();
                waitForm.Hide();

            }
            if (e.ProgressPercentage == 2)
            {
                button_flagsystem.Text = "Flag System Validaton";
                button_flagsystem.Enabled = true;
                button_technicalvalidation.Enabled = true;
                Stats_button.Enabled = true;
                ActionLog_button.Enabled = true;
                label1.Text = cosdFolderPath + "CoSD Tool\\Validation Results\\FlagValidation" + "_" + Date + ".xls";
                button_flagsystem.Refresh();
                waitForm.Hide();
                if (flagSystemSuccess == true || summaryStatisticsSuccess == true)
                {
                    validateData_button.Visible = true;
                    validationLabel.Visible = true;
                }
                MessageBox.Show("Flag system validation excel file created successfully");

            }
        }

        private void button_flagsystem_Click_1(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.


            backgroundWorker = new BackgroundWorker();


            //Method call to execute Stored Procedure
            backgroundWorker.DoWork += new DoWorkEventHandler(flagsystemValidation_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(flagsystemValidation_ProgressChanged);
            //Progress reporing is turned on
            backgroundWorker.WorkerReportsProgress = true;
            //Starting the process in background.
            backgroundWorker.RunWorkerAsync();
            this.Invoke(new MethodInvoker(delegate {
                enable_buttons();

            }));

        }



        private void validateData_button_Click(object sender, EventArgs e)
        {
            DataTable dtGetData = new DataTable();

            // ask for conformation before validation
            if (MessageBox.Show("All Data Values will be validated. Do you really want to continue?", "Confirm Validate", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                waitForm.Show(this);
                waitForm.Refresh();

                string groupValues = getGroupID();
                if (groupValues == "" || groupValues == null)
                {
                    waitForm.Hide();
                    waitForm.Refresh();
                    MessageBox.Show("Could not validate Data Values. No group is available for the user.");
                    return;
                }
                string sql = "UPDATE " + schemaName + "ERSDataValues set " + "ERSDataValues_DataRowLifecyclePhaseID = '3' WHERE  ERSDataValues_DataRowLifecyclePhaseID IN ('1','2') and [ERSDataValues_ERSCommodity_ID] IN (SELECT [ERSCommodity_ID] FROM " + schemaName + "[ERSCommodityDataSeries] where ERSCommodity_ERSGroup_ID IN (" + groupValues + "))";
                // execute
                dtGetData = GetData(sql);
                saveAction(sql);
                if (dtGetData == null)
                {
                    waitForm.Hide();
                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                    return;
                }


                waitForm.Hide();
                this.TopMost = true;
                MessageBox.Show("All Data Values Validated Successfully");
            }
        }
        /*Retrieve groups according to logged in user
*/
        private string getGroupID()
        {
            groupID = new List<string>();
            // get current user            
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            string sql = "select [ERSGroup_ID] from " + schemaName + "ERSGroup_LU where ERSGroup_Username like '%" + currentUser.Name + "%' and [ERSGroup_ID] IN (" + apGroupId + ")";
            System.Data.DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            foreach (DataRow row in dr)
            {
                groupID.Add(row["ERSGroup_ID"].ToString());
            }
            if (groupID.Count == 0)
                return "";
            StringBuilder group = new StringBuilder();
            string lastItem = groupID[groupID.Count - 1];
            foreach (string item in groupID)
            {
                if (item != lastItem)
                    group.Append(item + ",");
                else group.Append(item);
            }

            return group.ToString();
        }
        private void label56_Click(object sender, EventArgs e)
        {

        }

        private void button_technicalvalidation_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();


            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(technicalvalidation_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler
                 (technicalvalidation_ProgressChanged);
            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();
         



        }

        public void disable_buttons()
        {
            button_back.Enabled = false;
            button_ChangePath.Enabled = false;
            validateData_button.Enabled = false;
        }
        public void enable_buttons()
        {
            button_back.Enabled = true;
            button_ChangePath.Enabled = true;
            validateData_button.Enabled = true;
        }

        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void technicalvalidation_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (MessageBox.Show("This process may take more than 10 minutes. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Invoke(new MethodInvoker(delegate {
                        disable_buttons();
                       
                    }));
                    // int countdataset;
                    int loopcount = 1;
                    String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");
                    DataSet ds = new DataSet();
                    backgroundWorker.ReportProgress(1);
                    //StoredProcedure Execution for the validation engine
                    string connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
                    string commandText = "CoSD.ValidationEngine-TechnicalValidation";

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(commandText, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        try
                        {
                            conn.Open();
                            SDA.SelectCommand = cmd;
                            SDA.Fill(ds);

                            ds.Tables[0].TableName = "Duplicate-DS";
                            ds.Tables[1].TableName = "Duplicate-Commoditysub";
                            ds.Tables[2].TableName = "Duplicate-geodimension";
                            ds.Tables[3].TableName = "Duplicate-Macro";
                            ds.Tables[4].TableName = "Duplicate-Conversion";


                            ds.Tables[5].TableName = "No.of.DVperDS";
                            ds.Tables[6].TableName = "No.of.DVpercommodity";
                            ds.Tables[7].TableName = "No.of.DVperprodpractice";
                            ds.Tables[8].TableName = "No.of.DVperUtilizationpractice";
                            ds.Tables[9].TableName = "No.of.DVperunit";
                            ds.Tables[10].TableName = "No.of.DVperHScodes";

                            ds.Tables[11].TableName = "CommodityDataCount";
                            ds.Tables[12].TableName = "CommoditynotinDV";
                            ds.Tables[13].TableName = "TimeRangeForAllCommodity";

                            ds.Tables[14].TableName = "CountryCount";
                            ds.Tables[15].TableName = "CountryList";
                            ds.Tables[16].TableName = "StateCount";
                            ds.Tables[17].TableName = "StateList";
                            ds.Tables[18].TableName = "GeoDuplicates";

                            ds.Tables[19].TableName = "DataseriesNotinDatavalues";
                            ds.Tables[20].TableName = "DS-IDvalidation";
                            // ds.Tables[21].TableName = "DV-IDvalidation";

                            //countdataset = ds.Tables.Count;
                         
                        }
                        catch (SqlException ex)
                        {
                            backgroundWorker.ReportProgress(0);
                            MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                            //saveAction(ex.Message);
                        }

                        finally
                        {
                            conn.Close();
                        }


                        if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Validation Results")) // name of the error file and location
                        {
                            Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Validation Results");
                        }

                        Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                        if (xlApp == null)
                        {
                            backgroundWorker.ReportProgress(0);
                            MessageBox.Show("Excel is not properly installed!!");
                            return;
                        }

                        try
                        {
                            Excel.Workbook xlWorkBook;
                            Excel.Worksheet xlWorkSheet;
                            object misValue = System.Reflection.Missing.Value;
                            xlWorkBook = xlApp.Workbooks.Add(misValue);
                            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                            xlWorkBook.SaveAs(cosdFolderPath + "CoSD Tool\\Validation Results\\TechnicalValidation" + "_" + Date + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                            string filePath = @cosdFolderPath + "CoSD Tool\\Validation Results\\TechnicalValidation" + "_" + Date + ".xls";
                            xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                            //It supperesses the Microsoft Compatibility message
                            xlWorkBook.CheckCompatibility = false;

                            //For sheet5------------------------------>
                            Excel.Worksheet excelWorkSheet5 = xlWorkBook.Sheets.Add();
                            excelWorkSheet5.Name = "Duplicates Validation";
                            loopcount = 1;
                            foreach (DataTable table in ds.Tables)
                            {
                                if (loopcount > 5)
                                {
                                    break;
                                }


                                if (table.TableName == "Duplicate-DS")
                                {

                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet5.Cells[3, i] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet5.get_Range("A3");
                                        formatRange.EntireRow.Font.Bold = true;

                                    }
                                }

                                if (table.TableName == "Duplicate-Commoditysub")
                                {
                                    int ji = 16;
                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet5.Cells[3, ji] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet5.get_Range("A3");
                                        formatRange.EntireRow.Font.Bold = true;
                                        ji = ji + 1;
                                    }
                                }

                                if (table.TableName == "Duplicate-geodimension")
                                {
                                    int ji = 24;
                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet5.Cells[3, ji] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet5.get_Range("A3");
                                        formatRange.EntireRow.Font.Bold = true;
                                        ji = ji + 1;
                                    }
                                }

                                if (table.TableName == "Duplicate-Macro")
                                {
                                    int ji = 33;
                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet5.Cells[3, ji] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet5.get_Range("A3");
                                        formatRange.EntireRow.Font.Bold = true;
                                        ji = ji + 1;
                                    }
                                }

                                if (table.TableName == "Duplicate-Conversion")
                                {
                                    int ji = 42;
                                    for (int i = 1; i < table.Columns.Count + 1; i++)
                                    {
                                        excelWorkSheet5.Cells[3, ji] = table.Columns[i - 1].ColumnName;
                                        Excel.Range formatRange;
                                        formatRange = excelWorkSheet5.get_Range("A3");
                                        formatRange.EntireRow.Font.Bold = true;
                                        ji = ji + 1;
                                    }
                                }


                                if (table.Rows.Count == 0)
                                {
                                    if (table.TableName == "Duplicate-DS")
                                    {
                                        excelWorkSheet5.Cells[6, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        excelWorkSheet5.Cells[6, 5] = "No duplicates found";
                                    }
                                    if (table.TableName == "Duplicate-Commoditysub")
                                    {
                                        excelWorkSheet5.Cells[6, 17].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        excelWorkSheet5.Cells[6, 17] = "No duplicates found";
                                    }
                                    if (table.TableName == "Duplicate-geodimension")
                                    {
                                        excelWorkSheet5.Cells[6, 26].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        excelWorkSheet5.Cells[6, 26] = "No duplicates found";
                                    }
                                    if (table.TableName == "Duplicate-Macro")
                                    {
                                        excelWorkSheet5.Cells[6, 35].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        excelWorkSheet5.Cells[6, 35] = "No duplicates found";
                                    }
                                    if (table.TableName == "Duplicate-Conversion")
                                    {
                                        excelWorkSheet5.Cells[6, 44].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        excelWorkSheet5.Cells[6, 44] = "No duplicates found";
                                    }

                                }

                                if (table.TableName == "Duplicate-DS")
                                {
                                    int l = 4;
                                    excelWorkSheet5.get_Range("A2", "C2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet5.Cells[2, 1] = "Duplicate data series";

                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet5.Cells[l, k + 1] = table.Rows[j].ItemArray[k].ToString();
                                            excelWorkSheet5.Cells[l, k + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                        }
                                        l = l + 1;
                                    }

                                }

                                if (table.TableName == "Duplicate-Commoditysub")
                                {
                                    int l = 4;
                                    int la = 16;

                                    excelWorkSheet5.get_Range("P2", "R2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet5.Cells[2, 16] = "Duplicate sub commodity values";

                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet5.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                            excelWorkSheet5.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.RosyBrown);
                                            la = la + 1;
                                        }
                                        l = l + 1;
                                        la = 16;
                                    }
                                }


                                if (table.TableName == "Duplicate-geodimension")
                                {
                                    int l = 4;
                                    int la = 24;
                                    excelWorkSheet5.get_Range("X2", "Z2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet5.Cells[2, 24] = "Geo dimension duplicate Values";
                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet5.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                            excelWorkSheet5.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Linen);
                                            la = la + 1;
                                        }
                                        l = l + 1;
                                        la = 24;
                                    }
                                }


                                if (table.TableName == "Duplicate-Macro")
                                {
                                    int l = 4;
                                    int la = 33;
                                    excelWorkSheet5.get_Range("AG2", "AI2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet5.Cells[2, 33] = "Macro lookup duplicate values";

                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet5.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                            excelWorkSheet5.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                                            la = la + 1;
                                        }
                                        l = l + 1;
                                        la = 33;
                                    }

                                }

                                if (table.TableName == "Duplicate-Conversion")
                                {
                                    int l = 4;
                                    int la = 42;

                                    excelWorkSheet5.get_Range("AP2", "AS2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    excelWorkSheet5.Cells[2, 42] = "Conversion lookup duplicate values";

                                    for (int j = 0; j < table.Rows.Count; j++)
                                    {
                                        for (int k = 0; k < table.Columns.Count; k++)
                                        {
                                            excelWorkSheet5.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                            excelWorkSheet5.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.RosyBrown);
                                            la = la + 1;
                                        }
                                        l = l + 1;
                                        la = 42;
                                    }
                                }

                                loopcount = loopcount + 1;
                            }




                            //For sheet4------------------------------>
                            Excel.Worksheet excelWorkSheet4 = xlWorkBook.Sheets.Add();
                            excelWorkSheet4.Name = "DataValuesCount Validation";
                            loopcount = 1;
                            foreach (DataTable table in ds.Tables)
                            {
                                if (loopcount > 5)
                                {
                                    if (loopcount > 11)
                                    {
                                        break;
                                    }


                                    if (table.TableName == "No.of.DVperDS")
                                    {

                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet4.Cells[3, i] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet4.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;

                                        }
                                    }

                                    if (table.TableName == "No.of.DVpercommodity")
                                    {
                                        int ji = 7;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet4.Cells[3, ji] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet4.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;
                                            ji = ji + 1;
                                        }
                                    }

                                    if (table.TableName == "No.of.DVperprodpractice")
                                    {
                                        int ja = 11;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet4.Cells[3, ja] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet4.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;
                                            ja = ja + 1;
                                        }
                                    }


                                    if (table.TableName == "No.of.DVperUtilizationpractice")
                                    {
                                        int ji = 15;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet4.Cells[3, ji] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet4.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;
                                            ji = ji + 1;
                                        }
                                    }

                                    if (table.TableName == "No.of.DVperunit")
                                    {
                                        int ji = 19;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet4.Cells[3, ji] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet4.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;
                                            ji = ji + 1;
                                        }
                                    }

                                    if (table.TableName == "No.of.DVperHScodes")
                                    {
                                        int ja = 23;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet4.Cells[3, ja] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet4.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;
                                            ja = ja + 1;
                                        }
                                    }



                                    if (table.Rows.Count == 0)
                                    {
                                        if (table.TableName == "No.of.DVperDS")
                                        {
                                            excelWorkSheet4.Cells[6, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet4.Cells[6, 1] = "Data not availabel";
                                        }
                                        if (table.TableName == "No.of.DVpercommodity")
                                        {
                                            excelWorkSheet4.Cells[6, 8].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet4.Cells[6, 8] = "Data not availabel";
                                        }
                                        if (table.TableName == "No.of.DVperprodpractice")
                                        {
                                            excelWorkSheet4.Cells[6, 12].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet4.Cells[6, 12] = "Data not availabel";
                                        }
                                        if (table.TableName == "No.of.DVperUtilizationpractice")
                                        {
                                            excelWorkSheet4.Cells[6, 16].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet4.Cells[6, 16] = "Data not availabel";
                                        }
                                        if (table.TableName == "No.of.DVperunit")
                                        {
                                            excelWorkSheet4.Cells[6, 20].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet4.Cells[6, 20] = "Data not availabel";
                                        }
                                        if (table.TableName == "No.of.DVperHScodes")
                                        {
                                            excelWorkSheet4.Cells[6, 24].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet4.Cells[6, 24] = "Data not availabel";
                                        }
                                    }

                                    if (table.TableName == "No.of.DVperDS")
                                    {
                                        int l = 4;
                                        excelWorkSheet4.get_Range("A2", "C2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet4.Cells[2, 1] = "Number Of Data Values Per Data Series";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet4.Cells[l, k + 1] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet4.Cells[l, k + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                            }
                                            l = l + 1;
                                        }

                                    }

                                    if (table.TableName == "No.of.DVpercommodity")
                                    {
                                        int l = 4;
                                        int la = 7;

                                        excelWorkSheet4.get_Range("G2", "I2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet4.Cells[2, 7] = "Number of Data Values Per Commodity";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet4.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet4.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.RosyBrown);
                                                la = la + 1;
                                            }
                                            l = l + 1;
                                            la = 7;
                                        }
                                    }


                                    if (table.TableName == "No.of.DVperprodpractice")
                                    {
                                        int l = 4;
                                        int la = 11;
                                        excelWorkSheet4.get_Range("K2", "M2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet4.Cells[2, 11] = "Number of Data Values Per Production Practice";
                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet4.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet4.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Linen);
                                                la = la + 1;
                                            }
                                            l = l + 1;
                                            la = 11;
                                        }
                                    }


                                    if (table.TableName == "No.of.DVperUtilizationpractice")
                                    {
                                        int l = 4;
                                        int la = 15;
                                        excelWorkSheet4.get_Range("O2", "Q2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet4.Cells[2, 15] = "Number of Data Values Per Utilization Practice";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet4.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet4.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                                                la = la + 1;
                                            }
                                            l = l + 1;
                                            la = 15;
                                        }

                                    }

                                    if (table.TableName == "No.of.DVperunit")
                                    {
                                        int l = 4;
                                        int la = 19;

                                        excelWorkSheet4.get_Range("S2", "U2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet4.Cells[2, 19] = "Number of Data Values per Unit";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet4.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet4.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.RosyBrown);
                                                la = la + 1;
                                            }
                                            l = l + 1;
                                            la = 19;
                                        }
                                    }


                                    if (table.TableName == "No.of.DVperHScodes")
                                    {
                                        int l = 4;
                                        int la = 23;
                                        excelWorkSheet4.get_Range("W2", "Y2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet4.Cells[2, 23] = "Number of Data Values per HS Code";
                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet4.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet4.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Linen);
                                                la = la + 1;
                                            }
                                            l = l + 1;
                                            la = 23;
                                        }
                                    }

                                }
                                loopcount = loopcount + 1;
                            }

                            //For sheet3------------------------------>
                            Excel.Worksheet excelWorkSheet2 = xlWorkBook.Sheets.Add();
                            excelWorkSheet2.Name = "Commodity Validation";
                            loopcount = 1;
                            foreach (DataTable table in ds.Tables)
                            {
                                if (loopcount > 11)
                                {
                                    if (loopcount > 14)
                                    {
                                        break;
                                    }

                                    if (table.TableName == "CommodityDataCount")
                                    {
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet2.Cells[3, i] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet2.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;

                                        }
                                    }

                                    if (table.TableName == "CommoditynotinDV")
                                    {
                                        int ji = 11;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet2.Cells[3, ji] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet2.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;
                                            ji = ji + 1;
                                        }
                                    }



                                    if (table.TableName == "TimeRangeForAllCommodity")
                                    {
                                        int ji = 16;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet2.Cells[3, ji] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet2.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;
                                            ji = ji + 1;
                                        }
                                    }

                                    if (table.Rows.Count == 0)
                                    {

                                        if (table.TableName == "CommodityDataCount")
                                        {
                                            excelWorkSheet2.Cells[6, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet2.Cells[6, 1] = "Data not availabel";
                                        }

                                        if (table.TableName == "CommoditynotinDV")
                                        {
                                            excelWorkSheet2.Cells[6, 12].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet2.Cells[6, 12] = "No data available";
                                        }

                                        if (table.TableName == "TimeRangeForAllCommodity")
                                        {
                                            excelWorkSheet2.Cells[6, 17].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet2.Cells[6, 17] = "Timerange for the commodities are not available";
                                        }

                                    }


                                    if (table.TableName == "CommodityDataCount")
                                    {
                                        int l = 4;

                                        excelWorkSheet2.get_Range("A2", "C2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet2.Cells[2, 1] = "Commodity Data Counts per Data Series";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet2.Cells[l, k + 1] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet2.Cells[l, k + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                                            }
                                            l = l + 1;
                                        }
                                    }


                                    if (table.TableName == "CommoditynotinDV")
                                    {
                                        int l = 4;
                                        int la = 11;

                                        excelWorkSheet2.get_Range("K2", "M2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet2.Cells[2, 11] = "Commodities Not in Data Values";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet2.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet2.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Linen);
                                                la = la + 1;
                                            }
                                            l = l + 1;
                                            la = 11;
                                        }
                                    }

                                    if (table.TableName == "TimeRangeForAllCommodity")
                                    {
                                        int l = 4;
                                        int la = 16;

                                        excelWorkSheet2.get_Range("Q2", "S2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet2.Cells[2, 16] = "Time Dimension Range for All Commodities";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet2.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet2.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.RosyBrown);
                                                la = la + 1;
                                            }
                                            l = l + 1;
                                            la = 16;
                                        }
                                    }


                                }

                                loopcount = loopcount + 1;
                            }

                            //For sheet2------------------------------>
                            Excel.Worksheet excelWorkSheet1 = xlWorkBook.Sheets.Add();
                            excelWorkSheet1.Name = "Geo Validation";
                            loopcount = 1;
                            foreach (DataTable table in ds.Tables)
                            {
                                if (loopcount > 14)
                                {
                                    if (loopcount > 19)
                                    {
                                        break;
                                    }


                                    if (table.TableName == "CountryCount")
                                    {

                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet1.Cells[3, i] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet1.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;

                                        }
                                    }

                                    if (table.TableName == "CountryList")
                                    {

                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet1.Cells[4, i] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet1.get_Range("A4");
                                            formatRange.EntireRow.Font.Bold = true;

                                        }
                                    }

                                    if (table.TableName == "StateCount")
                                    {

                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet1.Cells[3, 3] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet1.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;

                                        }
                                    }

                                    if (table.TableName == "StateList")
                                    {
                                        int ji = 3;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet1.Cells[4, ji] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet1.get_Range("A4");
                                            formatRange.EntireRow.Font.Bold = true;
                                            ji = ji + 1;
                                        }
                                    }

                                    if (table.TableName == "GeoDuplicates")
                                    {
                                        int jh = 5;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet1.Cells[4, jh] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet1.get_Range("A4");
                                            formatRange.EntireRow.Font.Bold = true;
                                            jh = jh + 1;
                                        }
                                    }

                                    if (table.Rows.Count == 0)
                                    {
                                        if (table.TableName == "CountryList")
                                        {
                                            excelWorkSheet1.Cells[6, 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet1.Cells[6, 1] = "No Countries are availabel";
                                        }
                                        if (table.TableName == "StateList")
                                        {
                                            excelWorkSheet1.Cells[6, 3].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet1.Cells[6, 3] = "No states are availabel";
                                        }
                                        if (table.TableName == "GeoDuplicates")
                                        {
                                            excelWorkSheet1.Cells[6, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet1.Cells[6, 5] = "No geoduplicates are availabel";
                                        }

                                    }

                                    if (table.TableName == "CountryList")
                                    {
                                        int l = 5;
                                        excelWorkSheet1.get_Range("A2", "C2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet1.Cells[2, 1] = "Geograpy Country Validation";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet1.Cells[l, k + 1] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet1.Cells[l, k + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                            }
                                            l = l + 1;
                                        }

                                    }

                                    if (table.TableName == "CountryCount")
                                    {
                                        int l = 3;
                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet1.Cells[l, 2] = table.Rows[j].ItemArray[k].ToString();

                                            }
                                            l = l + 1;
                                        }
                                    }


                                    if (table.TableName == "StateCount")
                                    {
                                        int l = 3;
                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet1.Cells[l, 4] = table.Rows[j].ItemArray[k].ToString();

                                            }
                                            l = l + 1;
                                        }
                                    }



                                    if (table.TableName == "StateList")
                                    {
                                        int lj = 5;
                                        excelWorkSheet1.get_Range("C2", "E2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet1.Cells[2, 3] = "Geography State Validation";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet1.Cells[lj, 3] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet1.Cells[lj, 3].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.RosyBrown);

                                            }
                                            lj = lj + 1;
                                        }
                                    }



                                    if (table.TableName == "GeoDuplicates")
                                    {
                                        int lk = 5;
                                        excelWorkSheet1.get_Range("E2", "H2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet1.Cells[2, 5] = "Geograpy Dimension Duplicates";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet1.Cells[lk, 5] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet1.Cells[lk, 5].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Linen);

                                            }
                                            lk = lk + 1;
                                        }

                                    }




                                }
                                loopcount = loopcount + 1;
                            }

                            //For sheet1------------------------------>
                            Excel.Worksheet excelWorkSheet = xlWorkBook.Sheets.Add();
                            excelWorkSheet.Name = "DataSeries Validation";
                            loopcount = 1;
                            foreach (DataTable table in ds.Tables)
                            {
                                if (loopcount > 19)
                                {
                                    if (loopcount > 22)
                                    {
                                        break;
                                    }

                                    if (table.TableName == "DataseriesNotinDatavalues")
                                    {

                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet.Cells[3, i] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;

                                        }
                                    }

                                    if (table.TableName == "DS-IDvalidation")
                                    {
                                        int li = 6;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet.Cells[3, li] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;
                                            li = li + 1;
                                        }

                                    }

                                    if (table.TableName == "DV-IDvalidation")
                                    {
                                        int ki = 11;
                                        for (int i = 1; i < table.Columns.Count + 1; i++)
                                        {
                                            excelWorkSheet.Cells[3, ki] = table.Columns[i - 1].ColumnName;
                                            Excel.Range formatRange;
                                            formatRange = excelWorkSheet.get_Range("A3");
                                            formatRange.EntireRow.Font.Bold = true;
                                            ki = ki + 1;
                                        }
                                    }


                                    if (table.Rows.Count == 0)
                                    {
                                        if (table.TableName == "DataseriesNotinDatavalues")
                                        {
                                            excelWorkSheet.Cells[5, 2].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet.get_Range("B5", "D5").Merge();
                                            excelWorkSheet.Cells[5, 2] = excelWorkSheet.get_Range("B5", "D5");
                                            excelWorkSheet.Cells[5, 2].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                            excelWorkSheet.Cells[5, 2] = "No mismatch found in Datavalues";
                                        }

                                        if (table.TableName == "DS-IDvalidation")
                                        {
                                            excelWorkSheet.Cells[5, 7].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet.get_Range("G5", "I5").Merge();
                                            excelWorkSheet.Cells[5, 7] = excelWorkSheet.get_Range("G5", "I5");
                                            excelWorkSheet.Cells[5, 7].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                            excelWorkSheet.Cells[5, 7] = "No IDmismatch found in Dataseries";
                                        }
                                        if (table.TableName == "DV-IDvalidation")
                                        {
                                            excelWorkSheet.Cells[5, 12].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            excelWorkSheet.get_Range("L5", "N5").Merge();
                                            excelWorkSheet.Cells[5, 12] = excelWorkSheet.get_Range("L5", "N5");
                                            excelWorkSheet.Cells[5, 12].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                                            excelWorkSheet.Cells[5, 12] = "No IDmismatch found in Datavalues";
                                        }
                                    }
                                    if (table.TableName == "DataseriesNotinDatavalues")
                                    {
                                        int l = 4;
                                        excelWorkSheet.get_Range("A2", "C2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet.Cells[2, 1] = "Data Series not in Data Values";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet.Cells[l, k + 1] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet.Cells[l, k + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                                            }
                                            l = l + 1;
                                        }

                                    }


                                    if (table.TableName == "DS-IDvalidation")
                                    {
                                        int l = 4;
                                        int la = 6;
                                        excelWorkSheet.get_Range("F2", "H2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet.Cells[2, 6] = "Unmatched lookup values in Data Series";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet.Cells[l, la] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet.Cells[l, la].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.RosyBrown);
                                                la = la + 1;
                                            }
                                            l = l + 1;
                                            la = 6;

                                        }

                                    }

                                    if (table.TableName == "DV-IDvalidation")
                                    {
                                        int l = 4;
                                        int lb = 11;
                                        excelWorkSheet.get_Range("K2", "M2").Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        excelWorkSheet.Cells[2, 11] = "Unmatched lookup values in Data Values";

                                        for (int j = 0; j < table.Rows.Count; j++)
                                        {
                                            for (int k = 0; k < table.Columns.Count; k++)
                                            {
                                                excelWorkSheet.Cells[l, lb] = table.Rows[j].ItemArray[k].ToString();
                                                excelWorkSheet.Cells[l, lb].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightCyan);
                                                lb = lb + 1;
                                            }
                                            l = l + 1;
                                            lb = 11;
                                        }

                                    }


                                }


                                loopcount = loopcount + 1;
                            }

                            xlWorkBook.Sheets["Sheet1"].Delete();
                            xlWorkBook.Close(true, misValue, misValue);
                            xlApp.Quit();

                            releaseObject(xlWorkSheet);
                            releaseObject(xlWorkBook);
                            releaseObject(xlApp);
                            this.Invoke(new MethodInvoker(delegate {
                                enable_buttons();

                            }));
                        }


                        catch (Exception ex)
                        {
                            backgroundWorker.ReportProgress(0);
                            MessageBox.Show(" There was a problem in creating excel file. Please contact the techincal team ");
                            return;
                        }

                    }
                    backgroundWorker.ReportProgress(2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void technicalvalidation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (e.ProgressPercentage == 1)
            {
                button_technicalvalidation.Text = "Technical Validation Processing...";

                button_flagsystem.Enabled = false;
                button_technicalvalidation.Enabled = false;
                Stats_button.Enabled = false;
                ActionLog_button.Enabled = false;

                button_technicalvalidation.Refresh();
                waitForm.ShowDialog(this);
            }
            else if (e.ProgressPercentage == 0)
            {
                button_technicalvalidation.Text = "Sanity Check/\nTechnical Validation";

                button_flagsystem.Enabled = true;
                button_technicalvalidation.Enabled = true;
                Stats_button.Enabled = true;
                ActionLog_button.Enabled = true;

                button_technicalvalidation.Refresh();
                waitForm.Hide();
            }
            else if (e.ProgressPercentage == 2)
            {

                button_flagsystem.Enabled = true;
                button_technicalvalidation.Enabled = true;
                Stats_button.Enabled = true;
                ActionLog_button.Enabled = true;

                label1.Text = cosdFolderPath + "CoSD Tool\\Validation Results\\TechnicalValidation" + "_" + Date + ".xls";
                button_technicalvalidation.Text = "Sanity Check/\nTechnical Validation";
                button_technicalvalidation.Refresh();
                waitForm.Hide();
                MessageBox.Show("Technical validation excel file created successfully");
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void button_ChangePath_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowNewFolderButton = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    cosdFolderPath = fbd.SelectedPath;
                    if (!cosdFolderPath.EndsWith("\\"))
                    {
                        cosdFolderPath = cosdFolderPath + "\\";
                    }
                    label1.Text = cosdFolderPath + "CoSD Tool";
                }

                if (DeSerializeSettingsPath<String>("CoSDSettings") == null)
                {
                    SerializeSettingsPath(cosdFolderPath, "CoSDSettings");
                }
                else
                {
                    SerializeSettingsPath(cosdFolderPath, "CoSDSettings");
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// This method Serializes an String object.
        /// </summary>
        public void SerializeSettingsPath<T>(T serializePath, string fileName)
        {
            // first check if target directory exist
            // create one if not exist
            try
            {
                if (Directory.Exists(Home.globalCoSDPath + "\\"))
                {
                    fileName = Home.globalCoSDPath + "\\" + fileName;
                }
                else if (Directory.Exists(Home.globalCoSDPathE + "\\"))
                {
                    fileName = Home.globalCoSDPathE + "\\" + fileName;
                }
                else if (Directory.Exists(Home.globalCoSDPathC + "\\"))
                {
                    fileName = Home.globalCoSDPathC + "\\" + fileName;
                }
                else
                    return;

                if (serializePath == null) { return; }


                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializePath.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializePath);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// This method Deserializes an xml file into an String object.
        /// </summary>
        public T DeSerializeSettingsPath<T>(string fileName)
        {

            T objectOut = default(T);

            try
            {
                // first check if target directory exist
                // create one if not exist

                if (Directory.Exists(Home.globalCoSDPath + "\\"))
                {
                    fileName = Home.globalCoSDPath + "\\" + fileName;
                }
                else if (Directory.Exists(Home.globalCoSDPathE + "\\"))
                {
                    fileName = Home.globalCoSDPathE + "\\" + fileName;
                }
                else if (Directory.Exists(Home.globalCoSDPathC + "\\"))
                {
                    fileName = Home.globalCoSDPathC + "\\" + fileName;
                }
                else
                    return objectOut;

                if (string.IsNullOrEmpty(fileName)) { return default(T); }

                string attributeXml = string.Empty;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
            }

            return objectOut;
        }

        public void cancelActionLog()
        {

            //try
            //{
            //    CallactionLogAsync(new CancellationToken(true));
            //}
            //catch (Exception ex)
            //{

            //}
            //cTokenSource.Cancel();
        }

        private void AutomatedValidation_Activated(object sender, EventArgs e)
        {

        }


    }
}
