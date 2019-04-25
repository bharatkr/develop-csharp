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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.SqlServer.Dts.Runtime;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Reflection;

namespace CoSD_Tool
{
    public partial class BusinessRules : Form
    {
        /// <summary>
        /// The con
        /// </summary>
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString);
        /// <summary>
        /// The sqlconn
      
        private int count1 { get; set; }
        private double waitTime { get; set; }

            private string businessRuleID { get; set; }
            private string commoditySeriesID { get; set; }
            private string outputDestination { get; set; }
            private string formula { get; set; }
            private string inputUnit { get; set; }
            private string macro { get; set; }
            private string conversionFactor { get; set; }
            private string outputName { get; set; }
            private string outputUnit { get; set; }
            private string inputTimeDimensionType { get; set; }
            private string inputTimeDimensionTypeValue { get; set; }
            private string outputTimeDimensionType { get; set; }
            private string outputTimeDimensionTypeValue { get; set; }
            private string inputgeoType { get; set; }
            private string inputgeoValue { get; set; }
            private string outputgeoType { get; set; }
            private string outputgeoValue { get; set; }
            private string privacy { get; set; }
            private string BLType { get; set; }
            private string longDescription { get; set; }
            private string inputSources { get; set; }



            StringBuilder seperatedUnitIDs = null;
            StringBuilder seperatedGEOIDs = null;
            StringBuilder seperatedGEOTypeIDs = null;
            string errorMessage = "";
            string updateStatus = "";
        string dataSeriesSQL = "";
        StringBuilder dataSeriesIDs = new StringBuilder();
        DataTable DTDataSeries = new DataTable();
        private SqlDataAdapter dataAdapterForPaging = new SqlDataAdapter();
        bool insertNewDataSeries = false;


        private string sqlconn;  // query and sql connection
        WaitBR waitForm = new WaitBR();

        public BusinessRulesInsertForm businessrulesinsertForm = new BusinessRulesInsertForm();
        public ConstructedVariables constructedVariables = new ConstructedVariables();
        public BusinessRulesTree businessruletreeForm = new BusinessRulesTree();
        /// <summary>
        /// The data adapter
        /// </summary>
        private SqlDataAdapter SDA = new SqlDataAdapter();
        /// <summary>
        /// The table
        /// </summary>
        DataTable DT = new DataTable();
        SqlCommandBuilder scb = new SqlCommandBuilder();
        /// <summary>
        /// The binding source1
        /// </summary>
        private BindingSource bindingSourcePhyAtt = new BindingSource();
        List<string> physicalAttributes = new List<string>();
        private string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        private BindingSource bindingSource1 = new BindingSource();
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        StringBuilder deletedData = new StringBuilder();
        string apGroupId = LandingScreen.GetGroupId(LandingScreen.newschemaName);
        string cosdFolderPath = Home.globalCoSDPath;
        String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");
        Dictionary<string, string> newSeriesIds = null;
        DataSet ds = new DataSet();
        string failedFileName = "";
        int failedCount = 0;
        DataTable failedDataTable = new DataTable();
        /// <summary>
        /// The current page
        /// </summary>
        static int currentPageIndex = 0;
        /// <summary>
        /// The items per page
        /// </summary>
        private int pageSize = 13;
        /// <summary>
        /// The total items
        /// </summary>
        private List<string> commodities;
        private List<string> groups;
        private List<string> logictype;
        private List<string> groupIds;
        private List<string> SourceType;
        private List<string> descriptionList;
        private List<string> groupDesc;
        private List<string> statDesc;
        string sourceType = "";
        string businessType = "";
        string groupType = "";
        string commodity = "";
        string statType = "";
        string commodityDescription = "";
      

        private void connection()
        {
            // connection string for linking db
            sqlconn = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            // init the connection to db
            con = new SqlConnection(sqlconn);
        }

        public BusinessRules()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
            Load += new EventHandler(BusinessRules_Load);
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
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                if (ex.Message.Contains("duplicate"))
                {
                    errorMessage = "duplicate";
                }
                return null;
            }
        }



        private void BusinessRules_Load(object sender, System.EventArgs e)
        {

            //Set available users group to groupIds
            groupIds = new List<string>();
            groupIds = getGroupIds();

            if (groupIds.Count != 0)
            {
                // Load Page
                filllogictypeCombobox("", "", "", "");
                fillGroupCombobox("", "", "", "");
                fillSourceTypeCombobox("", "", "", "");
                fillDescriptionCombobox("", "", "", "");
                fillCommodityCombobox("", "", "", "");
                fillStatCombobox("", "", "", "");


            }



            disablebuttons();
            //this.Controls.Add(button_executeBL);
            comboBox_group1.Text = "";
            comboBox_BusinessType1.Text = "";
            //Listbox_Commodity.Text = "";
            comboBox_description1.Text = "";
            dataGridView.RowsDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Regular);


        }


        private void fillCommodityCombobox(string sourceType, string businessType, string group, string description)
        {
            commodities = new List<string>();

            string sql = @"WITH dataforids
    AS(SELECT
      value,
      ersbusinesslogic_inputdataseries
    FROM " + schemaName + "[ersbusinesslogic] " +
   " CROSS APPLY (SELECT " +
      "Seq = ROW_NUMBER() " +
     " OVER ( " +
      "ORDER BY (SELECT " +
      "  NULL) " +
      "), " +
     " Value = v.value('(./text())[1]', " +
     " 'varchar(max)') " +
   " FROM (VALUES ( " +
   " CONVERT(xml, '<x>' " +
   " + REPLACE(ersbusinesslogic_inputdataseries, " +
    "',' " +
    ", '</x><x>') " +
  "  + '</x>'))) x (n) " +
   " CROSS APPLY n.nodes('x') node (v)) B " +
    "where ERSBusinessLogic_InputDataSeries not like '%CV%') " +
   " select distinct ERSCommoditySubCommodity_Desc  from " + schemaName + " ERSCommoditySubCommodity_LU csc, " + schemaName + "ERSCommodityDataSeries cds, dataforids" +
   "   where csc.ERSCommoditySubCommodity_ID = cds.ERSCommoditySubCommodity_ID" +
   " and dataforids.Value = cds.ERSCommodity_ID";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            commodities.Add("");
            foreach (DataRow row in dr)
            {
                //SourceType.Add(row["ERSSource_Desc"].ToString());
                commodities.Add(row["ERSCommoditySubCommodity_Desc"].ToString());
            }

            comboBox_Commodity1.DataSource = commodities;


        }

        private void fillStatCombobox(string sourceType, string businessType, string group, string description)
        {
            statDesc = new List<string>();
   
            string sql = @"WITH dataforids
    AS(SELECT
      value,
      ersbusinesslogic_inputdataseries
    FROM "+schemaName +"[ersbusinesslogic] "+
   " CROSS APPLY (SELECT " +
      "Seq = ROW_NUMBER() " +
     " OVER ( " +
      "ORDER BY (SELECT " +
      "  NULL) " +
      "), " +
     " Value = v.value('(./text())[1]', " +
     " 'varchar(max)') " +
   " FROM (VALUES ( " +
   " CONVERT(xml, '<x>' " +
   " + REPLACE(ersbusinesslogic_inputdataseries, " +
    "',' " +
    ", '</x><x>') " +
  "  + '</x>'))) x (n) " +
   " CROSS APPLY n.nodes('x') node (v)) B " +
    "where ERSBusinessLogic_InputDataSeries not like '%CV%') " +
   " select distinct ERSStatisticType_Attribute  from "+schemaName+" ERSStatisticType_LU slu, cosd.ERSCommodityDataSeries cds, dataforids"+
   "   where cds.ERSCommodity_ERSStatisticType_ID = slu.ERSStatisticType_ID"+
   " and dataforids.Value = cds.ERSCommodity_ID";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            statDesc.Add("");
            foreach (DataRow row in dr)
            {
             
                statDesc.Add(row["ERSStatisticType_Attribute"].ToString());
            }

            comboBox1_stattype.DataSource = statDesc;
       

        }


        private void filllogictypeCombobox(string sourceType, string businessType, string group, string description)
        {
            logictype = new List<string>();
            string sql = @"SELECT DISTINCT [ERSBusinessLogic_Type] FROM " + schemaName + "[ERSBusinessLogic] WHERE 1=1 "+sourceType+businessType+group+description;

            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            logictype.Add("");
            foreach (DataRow row in dr)
            {
                logictype.Add(row["ERSBusinessLogic_Type"].ToString());
            }

            comboBox_BusinessType1.DataSource = logictype;
            //comboBox_BusinessType.SelectedItem = null;

        }

        private void fillSourceTypeCombobox(string sourceType, string businessType, string group, string description)
        {
            SourceType = new List<string>();
            //string sql = @"SELECT DISTINCT [ERSSource_Desc] FROM " + schemaName + "[ERSSource_LU]";
            string sql = @"SELECT DISTINCT slu.ERSSource_Desc FROM " + schemaName + "[ERSBusinessLogic] bl"+","+ schemaName+ "ERSSource_LU slu "
                + "where bl.ERSBusinessLogic_InputSourceID=slu.ERSSource_ID";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            SourceType.Add("");
            foreach (DataRow row in dr)
            {
                //SourceType.Add(row["ERSSource_Desc"].ToString());
                SourceType.Add(row["ERSSource_Desc"].ToString());
            }

            comboBox_SourceType1.DataSource = SourceType;
            //comboBox_SourceType.SelectedItem = null;

        }
        private void fillDescriptionCombobox(string sourceType, string businessType, string group, string description)
        {
            descriptionList = new List<string>();
            string sql = @"SELECT DISTINCT [ERSBusinessLogic_LongDesc] FROM " + schemaName + "[ERSBusinessLogic]  WHERE 1=1 "+sourceType+businessType+group+description;
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            descriptionList.Add("");
            foreach (DataRow row in dr)
            {
                descriptionList.Add(row["ERSBusinessLogic_LongDesc"].ToString());
            }

            comboBox_description1.DataSource = descriptionList;
            //comboBox_description.SelectedItem = null;

        }


        /// <summary>
        /// fill group combobox with given array of group ids
        /// </summary>
        /// <param name="groupIDs"></param>
        private void fillGroupCombobox(string sourceType, string businessType, string group, string description)
        {
            groups = new List<string>();
            groups.Add("");
            StringBuilder commaSeperated = new StringBuilder();
            string groupIDs = "";
            foreach (string groupID in groupIds)
            {
                commaSeperated.Append(groupID + ",");
            }
            groupIDs = commaSeperated.ToString();
            if (groupIDs.Length > 1)
            {
                groupIDs = groupIDs.Remove(groupIDs.Length - 1);
            }
            string sql = "select ERSGroup_Desc from " + schemaName + "ERSGroup_LU where ERSGroup_ID IN (SELECT [ERSBusinessLogic_GroupID] FROM " + schemaName + "[ERSBusinessLogic]  WHERE 1=1 "+sourceType+businessType+group+description+")";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            foreach (DataRow row in dr)
            {
                groups.Add(row["ERSGroup_Desc"].ToString());
            }
            
            comboBox_group1.DataSource = groups;
            //comboBox_group.SelectedItem = null;
        }
        /*Retrieve groups according to logged in user
         */
        private List<string> getGroupIds()
        {
            groupIds = new List<string>();
            // get current user            
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            string sql = "select ERSGroup_ID from " + schemaName + "ERSGroup_LU where ERSGroup_Username like '%" + currentUser.Name + "%'and [ERSGroup_ID] IN (" + apGroupId + ")";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            foreach (DataRow row in dr)
            {
                groupIds.Add(row["ERSGroup_ID"].ToString());
            }
            return groupIds;
        }
        //Code for Displaying page wise data...........
        public int PageSize
        {
            set
            {
                this.pageSize = value;
            }
            get
            {
                return this.pageSize;
            }
        }
        public DataTable DataSource
        {
            set
            {
                this.DT = value;
                if (DT != null)
                {
                    if (DT.Rows.Count == 0 || DT.Rows.Count <= pageSize)
                    {
                        button_prev.Enabled = false;
                        button_next.Enabled = false;
                    }
                }

            }
        }



        public void BindGrid()
        {
            if (DT.Rows.Count > 0)
            {

                DataTable tmptable = DT.Clone();
                int startIndex = currentPageIndex * pageSize;
                int endIndex = currentPageIndex * pageSize + pageSize;
                if (endIndex > DT.Rows.Count)
                {
                    endIndex = DT.Rows.Count;
                }
                for (int i = startIndex; i < endIndex; i++)
                {
                    DataRow newRow = tmptable.NewRow();
                    GetNewRow(ref newRow, DT.Rows[i]);
                    tmptable.Rows.Add(newRow);
                }
                dataGridView.DataSource = tmptable;
                label_pageNum.Text = (currentPageIndex + 1) + " / " + (int)Math.Ceiling(Convert.ToDecimal(DT.Rows.Count) / pageSize);
            }

        }



        private void GetNewRow(ref DataRow newRow, DataRow source)
        {
            foreach (DataColumn col in DT.Columns)
            {
                newRow[col.ColumnName] = source[col.ColumnName];
            }
        }



        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void executeBL_DoWork(object sender, DoWorkEventArgs e)
        {

            string msg = string.Empty;
            string wait_time = string.Empty;
            string data = string.Empty;
            string parameter = string.Empty;

            waitTime = 0.03 * count1;
            DataTable failedDataTable = new DataTable();
            //string failedFileName = "";
            failedCount = 0;
            string connectionString = "";
            string commandText = "";
            if (schemaName.Equals("AnimalProductsCoSD.CoSD."))
            {
                connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
                commandText = schemaName + "BusinessLogic";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(commandText, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    var returnParam = new SqlParameter
                    {
                        ParameterName = "@Return",
                        Direction = ParameterDirection.ReturnValue,
                    };
                    cmd.Parameters.Add(returnParam);

                    if (sourceType != "" || businessType != "" || groupType != "" || commodity != "" || commodityDescription != "" || statType != ""|| textBox_DSid.Text!="")
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            data = row["Rule ID"].ToString();
                            parameter = parameter + data + ",";
                        }
                    }
                    else
                    {
                        parameter = "all rules";
                    }


                    if (waitTime < 1.00)
                    {
                        wait_time = "Business rule execution may take more than 1 minute.";
                    }
                    else
                    {
                        wait_time = "Business rule execution may take more than " + Math.Round(waitTime) + " minutes.";
                    }

                    if (parameter == "all rules")
                    {
                        msg = "Are you sure you want to run all business rules of Animal Products." + wait_time + " Do you want to continue?";
                    }
                    else
                    {
                        msg = wait_time + " Do you want to continue?";
                    }


                    if (MessageBox.Show(msg, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        backgroundWorker.ReportProgress(1);
                        cmd.Parameters.Add("@BusinessToolID", SqlDbType.NVarChar, 160).Value = parameter;
                        DataTable dt = new DataTable();
                        try
                        {
                            conn.Open();
                            //count will be the number of rows updated. will be zero if no rows updated.


                            SDA.SelectCommand = cmd;
                            SDA.Fill(failedDataTable);

                            int retunvalue = (int)cmd.Parameters["@Return"].Value;

                            failedCount = failedDataTable.Rows.Count;


                            string failedbusinessrules = string.Empty;
                            string bid = string.Empty;

                            int check1 = 0;
                            int numberofwords = 0;
                            int no_of_fails = 0;

                            // List<BusinessRules> failedbusinessrules = new List<BusinessRules>();
                            for (int i = 0; i < failedDataTable.Rows.Count; i++)
                            {


                                bid = failedDataTable.Rows[i]["BusinessID"].ToString();
                                failedbusinessrules = failedbusinessrules + bid + ",";
                            }

                            if (parameter != "all rules")
                            {
                                string[] separatingChars = { "," };
                                string[] words = parameter.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
                                numberofwords = words.Length;

                                foreach (string s in words)
                                {
                                    check1 = failedbusinessrules.IndexOf(s + ',');
                                    if (check1 != -1)
                                        no_of_fails = no_of_fails + 1;
                                }
                            }
                            //if (retunvalue == 1)
                            //{
                            //    //Success
                            //    backgroundWorker.ReportProgress(2);
                            //}
                            //else if (retunvalue == 0 && failedCount > 0)
                            //{

                            //    saveFailedBusinessRules(failedDataTable);

                            //    backgroundWorker.ReportProgress(0);

                            //}
                            //else
                            //{
                            //    backgroundWorker.ReportProgress(0);
                            //    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                            //}


                            if (parameter != "all rules" & no_of_fails == 0 || (parameter == "all rules" & failedCount == 0))
                            {
                                //Success
                                backgroundWorker.ReportProgress(2);
                                // MessageBox.Show("Business Rules Executed Successfully! Please go to constructed variables to see the execution results.");
                            }
                            else if (parameter == "all rules" & failedCount > 0)
                            {

                                saveFailedBusinessRules(failedDataTable);
                                backgroundWorker.ReportProgress(0);

                            }
                            else if (parameter != "all rules" & no_of_fails > 0)
                            {
                                backgroundWorker.ReportProgress(3);

                                DataView dv = new DataView(failedDataTable);
                                dv.RowFilter = " BusinessID IN ( " + parameter + " )";
                                DataTable dtNew = dv.ToTable();

                                saveFailedBusinessRules(dtNew);

                                MessageBox.Show(" " + (numberofwords - dtNew.Rows.Count) + " out of " + numberofwords + " Business Rules Executed successfully!.\n\n Failed records are saved at this location:\n"
                            + " " + failedFileName + "\n\n *Please go to detailed tree structure to troubleshoot if needed.");
                                saveAction(numberofwords - no_of_fails + " out of " + numberofwords + " Business Rules Executed successfully!");
                            }
                            else
                            {
                                backgroundWorker.ReportProgress(0);
                                MessageBox.Show("Business rules unsuccessful, please contact technical team.");
                            }




                        }
                        catch (SqlException ex)
                        {
                            //MessageBox.Show("Update Failed coz " + ex.Message);
                            backgroundWorker.ReportProgress(0);
                            MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                            saveAction(ex.Message);

                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }

                if (schemaName.Equals("VegetablesCoSD.CoSD."))
                {
                    connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
                    commandText = schemaName + "BusinessLogic";
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        SqlCommand cmd = new SqlCommand(commandText, conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        var returnParam = new SqlParameter
                        {
                            ParameterName = "@Return",
                            Direction = ParameterDirection.ReturnValue,
                        };
                        cmd.Parameters.Add(returnParam);

                        if (sourceType != "" || businessType != "" || groupType != "" || commodity != "" || commodityDescription != "" || statType != ""|| textBox_DSid.Text != "")
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                data = row["Rule ID"].ToString();
                                parameter = parameter + data + ",";
                            }
                        }
                        else
                        {
                            parameter = "all rules";
                        }


                        if (waitTime < 1.00)
                        {
                            wait_time = "Business rule execution may take more than 1 minute.";
                        }
                        else
                        {
                            wait_time = "Business rule execution may take more than " + Math.Round(waitTime) + " minutes.";
                        }

                        if (parameter == "all rules")
                        {
                            msg = "Are you sure you want to run all business rules of Vegetables" + wait_time + " Do you want to continue?";
                        }
                        else
                        {
                            msg = wait_time + " Do you want to continue?";
                        }


                        if (MessageBox.Show(msg, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            backgroundWorker.ReportProgress(1);
                            cmd.Parameters.Add("@BusinessToolID", SqlDbType.NVarChar, 160).Value = parameter;
                            DataTable dt = new DataTable();
                            try
                            {
                                conn.Open();
                                //count will be the number of rows updated. will be zero if no rows updated.


                                SDA.SelectCommand = cmd;
                                SDA.Fill(failedDataTable);

                                int retunvalue = (int)cmd.Parameters["@Return"].Value;

                                failedCount = failedDataTable.Rows.Count;


                                string failedbusinessrules = string.Empty;
                                string bid = string.Empty;

                                int check1 = 0;
                                int numberofwords = 0;
                                int no_of_fails = 0;

                                // List<BusinessRules> failedbusinessrules = new List<BusinessRules>();
                                for (int i = 0; i < failedDataTable.Rows.Count; i++)
                                {


                                    bid = failedDataTable.Rows[i]["BusinessID"].ToString();
                                    failedbusinessrules = failedbusinessrules + bid + ",";
                                }

                                if (parameter != "all rules")
                                {
                                    string[] separatingChars = { "," };
                                    string[] words = parameter.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
                                    numberofwords = words.Length;

                                    foreach (string s in words)
                                    {
                                        check1 = failedbusinessrules.IndexOf(s + ',');
                                        if (check1 != -1)
                                            no_of_fails = no_of_fails + 1;
                                    }
                                }
                                //if (retunvalue == 1)
                                //{
                                //    //Success
                                //    backgroundWorker.ReportProgress(2);
                                //}
                                //else if (retunvalue == 0 && failedCount > 0)
                                //{

                                //    saveFailedBusinessRules(failedDataTable);

                                //    backgroundWorker.ReportProgress(0);

                                //}
                                //else
                                //{
                                //    backgroundWorker.ReportProgress(0);
                                //    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                                //}


                                if (parameter != "all rules" & no_of_fails == 0 || (parameter == "all rules" & failedCount == 0))
                                {
                                    //Success
                                    backgroundWorker.ReportProgress(2);
                                    // MessageBox.Show("Business Rules Executed Successfully! Please go to constructed variables to see the execution results.");
                                }
                                else if (parameter == "all rules" & failedCount > 0)
                                {

                                    saveFailedBusinessRules(failedDataTable);
                                    backgroundWorker.ReportProgress(0);

                                }
                                else if (parameter != "all rules" & no_of_fails > 0)
                                {
                                    backgroundWorker.ReportProgress(3);

                                    DataView dv = new DataView(failedDataTable);
                                    dv.RowFilter = " BusinessID IN ( " + parameter + " )";
                                    DataTable dtNew = dv.ToTable();

                                    saveFailedBusinessRules(dtNew);

                                    MessageBox.Show(" " + (numberofwords - dtNew.Rows.Count) + " out of " + numberofwords + " Business Rules Executed successfully!.\n\n Failed records are saved at this location:\n"
                                + " " + failedFileName + "\n\n *Please go to detailed tree structure to troubleshoot if needed.");
                                    saveAction(numberofwords - no_of_fails + " out of " + numberofwords + " Business Rules Executed successfully!");
                                }
                                else
                                {
                                    backgroundWorker.ReportProgress(0);
                                    MessageBox.Show("Business rules unsuccessful, please contact technical team.");
                                }




                            }
                            catch (SqlException ex)
                            {
                                //MessageBox.Show("Update Failed coz " + ex.Message);
                                backgroundWorker.ReportProgress(0);
                                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                                saveAction(ex.Message);

                            }
                            finally
                            {
                                conn.Close();
                            }
                        }
                    }

                    if (schemaName.Equals("FruitCoSD.CoSD."))
                    {
                        connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
                        commandText = schemaName+"BusinessLogic";
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            SqlCommand cmd = new SqlCommand(commandText, conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 0;
                            var returnParam = new SqlParameter
                            {
                                ParameterName = "@Return",
                                Direction = ParameterDirection.ReturnValue,
                            };
                            cmd.Parameters.Add(returnParam);

                            if (sourceType != "" || businessType != "" || groupType != "" || commodity != "" || commodityDescription != "" || statType != "" || textBox_DSid.Text != "")
                            {
                                foreach (DataRow row in ds.Tables[0].Rows)
                                {
                                    data = row["Rule ID"].ToString();
                                    parameter = parameter + data + ",";
                                }
                            }
                            else
                            {
                                parameter = "all rules";
                            }


                            if (waitTime < 1.00)
                            {
                                wait_time = "Business rule execution may take more than 1 minute.";
                            }
                            else
                            {
                                wait_time = "Business rule execution may take more than " + Math.Round(waitTime) + " minutes.";
                            }

                            if (parameter == "all rules")
                            {
                                msg = "Are you sure you want to run all business rules of Fruits" + wait_time + " Do you want to continue?";
                            }
                            else
                            {
                                msg = wait_time + " Do you want to continue?";
                            }


                            if (MessageBox.Show(msg, "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                            {
                                backgroundWorker.ReportProgress(1);
                                cmd.Parameters.Add("@BusinessToolID", SqlDbType.NVarChar, 160).Value = parameter;
                                DataTable dt = new DataTable();
                                try
                                {
                                    conn.Open();
                                    //count will be the number of rows updated. will be zero if no rows updated.


                                    SDA.SelectCommand = cmd;
                                    SDA.Fill(failedDataTable);

                                    int retunvalue = (int)cmd.Parameters["@Return"].Value;

                                    failedCount = failedDataTable.Rows.Count;


                                    string failedbusinessrules = string.Empty;
                                    string bid = string.Empty;

                                    int check1 = 0;
                                    int numberofwords = 0;
                                    int no_of_fails = 0;

                                    // List<BusinessRules> failedbusinessrules = new List<BusinessRules>();
                                    for (int i = 0; i < failedDataTable.Rows.Count; i++)
                                    {


                                        bid = failedDataTable.Rows[i]["BusinessID"].ToString();
                                        failedbusinessrules = failedbusinessrules + bid + ",";
                                    }

                                    if (parameter != "all rules")
                                    {
                                        string[] separatingChars = { "," };
                                        string[] words = parameter.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
                                        numberofwords = words.Length;

                                        foreach (string s in words)
                                        {
                                            check1 = failedbusinessrules.IndexOf(s + ',');
                                            if (check1 != -1)
                                                no_of_fails = no_of_fails + 1;
                                        }
                                    }
                                    //if (retunvalue == 1)
                                    //{
                                    //    //Success
                                    //    backgroundWorker.ReportProgress(2);
                                    //}
                                    //else if (retunvalue == 0 && failedCount > 0)
                                    //{

                                    //    saveFailedBusinessRules(failedDataTable);

                                    //    backgroundWorker.ReportProgress(0);

                                    //}
                                    //else
                                    //{
                                    //    backgroundWorker.ReportProgress(0);
                                    //    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                                    //}


                                    if (parameter != "all rules" & no_of_fails == 0 || (parameter == "all rules" & failedCount == 0))
                                    {
                                        //Success
                                        backgroundWorker.ReportProgress(2);
                                        // MessageBox.Show("Business Rules Executed Successfully! Please go to constructed variables to see the execution results.");
                                    }
                                    else if (parameter == "all rules" & failedCount > 0)
                                    {

                                        saveFailedBusinessRules(failedDataTable);
                                        backgroundWorker.ReportProgress(0);

                                    }
                                    else if (parameter != "all rules" & no_of_fails > 0)
                                    {
                                        backgroundWorker.ReportProgress(3);

                                        DataView dv = new DataView(failedDataTable);
                                        dv.RowFilter = " BusinessID IN ( " + parameter + " )";
                                        DataTable dtNew = dv.ToTable();

                                        saveFailedBusinessRules(dtNew);

                                        MessageBox.Show(" " + (numberofwords - dtNew.Rows.Count) + " out of " + numberofwords + " Business Rules Executed successfully!.\n\n Failed records are saved at this location:\n"
                                    + " " + failedFileName + "\n\n *Please go to detailed tree structure to troubleshoot if needed.");
                                        saveAction(numberofwords - no_of_fails + " out of " + numberofwords + " Business Rules Executed successfully!");
                                    }
                                    else
                                    {
                                        backgroundWorker.ReportProgress(0);
                                        MessageBox.Show("Business rules unsuccessful, please contact technical team.");
                                    }




                                }
                                catch (SqlException ex)
                                {
                                    //MessageBox.Show("Update Failed coz " + ex.Message);
                                    backgroundWorker.ReportProgress(0);
                                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                                    saveAction(ex.Message);

                                }
                                finally
                                {
                                    conn.Close();
                                }
                            }
                        }
                    

                    }
            }
        }

        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void executeBL_ProgressChanged(object sender, ProgressChangedEventArgs e)
            {
                if (e.ProgressPercentage == 1)
                {
                    button_executeBL.Text = "Processing...";
                    if (waitTime < 1.00)
                    {
                        longwait_label.Text = "Business rule execution may take more than 1 minute. Please leave this window open and don't use the tool until you see a success message!";
                    }
                    else
                    {
                        longwait_label.Text = "Business rule execution may take more than " + Math.Round(waitTime) + " minutes. Please leave this window open and don't use the tool until you see a success message!";
                    }
                    longwait_label.Visible = true;
                    longwait_label.Refresh();
                    waitForm.ShowDialog(this);
                }
                if (e.ProgressPercentage == 0)
                {
                    longwait_label.Visible = false;
                    button_executeBL.Text = "Re-execute Selected Business Rules";
                    button_executeBL.Refresh();
                    waitForm.Hide();
                    if (failedFileName != "")
                    {
                        if (MessageBox.Show("Business Rules Executed Successfully with " + (failedCount - 1)
                            + " failure(s).\n1. Please go to constructed variables to see the execution results.\n2. Failed records are saved at this location:\n"
                            + failedFileName + "\n\nPlease click OK to view more information on failed business rules.", "Confirmation", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            // Process.Start(failedFileName);
                            saveAction("Business Rules Executed Successfully with " + (failedCount - 1) + " failure(s).");
                        }
                        else
                            return;
                    }

                }
                if (e.ProgressPercentage == 2)                  //success
                {
                    longwait_label.Visible = false;
                    button_executeBL.Text = "Re-execute Selected Business Rules";
                    button_executeBL.Refresh();
                    waitForm.Hide();
                    MessageBox.Show("Business Rules Executed Successfully! Please go to constructed variables to see the execution results.");
                    saveAction("Business Rules Executed Successfully! Please go to constructed variables to see the execution results.");
                    button_CV.BackColor = Color.Yellow;



                }

                if (e.ProgressPercentage == 3)
                {
                    longwait_label.Visible = false;
                    button_executeBL.Text = "Re-execute Selected Business Rules";
                    button_executeBL.Refresh();
                    waitForm.Hide();
                    //MessageBox.Show("Business Rules Executed Successfully! Please go to constructed variables to see the execution results.");
                    button_TS.BackColor = Color.Crimson;


                }
            

        }

        private void button_executeBL_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();

            button_CV.BackColor = Color.LightGray;
            button_TS.BackColor = Color.LightGray;

            sourceType = comboBox_SourceType1.Text;
            businessType = comboBox_BusinessType1.Text;
            groupType = comboBox_group1.Text;
            commodity = comboBox_Commodity1.Text;
            commodityDescription = comboBox_description1.Text;

            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(executeBL_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(executeBL_ProgressChanged);

            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();
        }
        /*Retrieve groups according to logged in user
        */
        private string getGroupDesc()
        {
            groupDesc = new List<string>();
            // get current user            
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            string sql = "select ERSGroup_Desc from " + schemaName + "ERSGroup_LU where ERSGroup_Username like '%" + currentUser.Name + "%' and [ERSGroup_ID] IN (" + apGroupId + ")";
            System.Data.DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            foreach (DataRow row in dr)
            {
                groupDesc.Add(row["ERSGroup_Desc"].ToString());
            }
            StringBuilder group = new StringBuilder();
            if (groupDesc.Count != 0)
            {
                string lastItem = groupDesc[groupDesc.Count - 1];
                foreach (string item in groupDesc)
                {
                    if (item != lastItem)
                        group.Append("'" + item + "',");
                    else group.Append("'" + item + "'");
                }
                return group.ToString();
            }
            else
                return "''";
        }
        private void button_retreive_Click(object sender, EventArgs e)
        {
            // verify input before doing anything
            if (comboBox_SourceType1.Text.Equals("") && comboBox_Commodity1.Text.Equals("") && comboBox_BusinessType1.Text.Equals("") && comboBox_group1.Text.Equals("") && comboBox_description1.Text.Equals(""))
            {
                MessageBox.Show("Please select at least one value I amgere");
                return;
            }
            
            //start wait form
            waitForm.Show(this);
            waitForm.Refresh();

            //resetting page index
            currentPageIndex = 0;
            ds = new DataSet();
            sourceType = comboBox_SourceType1.Text;
            businessType = comboBox_BusinessType1.Text;
            groupType = comboBox_group1.Text;
            commodityDescription = comboBox_description1.Text;
            commodity = comboBox_Commodity1.Text;
       

            //
            button_TS.BackColor = Color.LightGray;
            
            try
            {
                //Only source input
                if (sourceType != "")
                    sourceType = " and [ERSBusinessLogic_InputSources] like '" + sourceType + "'";
                if (businessType != "")
                    businessType = " and [ERSBusinessLogic_Type] like '" + businessType + "'";
                if (groupType != "")
                    groupType = " and [ERSBusinessLogic_GroupID] IN (Select [ERSGroup_ID] FROM " + schemaName + "[ERSGroup_LU] WHERE [ERSGroup_Desc] like '" + groupType + "' )";
                else
                    groupType = " and [ERSBusinessLogic_GroupID] IN (Select [ERSGroup_ID] FROM " + schemaName + "[ERSGroup_LU] WHERE [ERSGroup_Desc] IN (" + getGroupDesc() + ") )";
                if (commodityDescription != "")
                    commodityDescription = " and [ERSBusinessLogic_LongDesc] like '%" + commodityDescription + "%'";
                if (commodity != "")
                    commodity = " and [ERSBusinessLogic_OutputName] like '%" + commodity + "%'";




                con.Open();
                //printing
                string sql = "SELECT ERSBusinessLogic_ID AS 'Rule ID' "
+ " ,[ERSBusinessLogic_Formula] 'Formula' "
+ " ,[ERSBusinessLogic_OutputName] AS 'Output Name' "
+ " ,[ERSBusinessLogic_OutputDestination] AS 'Output Destination' "
+ " ,REPLACE ( [ERSBusinessLogic_InputDataSeries] , ',' , ' ;' ) AS 'Commodity Series ID' "
+ " ,InputUnit.[ERSUnit_Desc] AS 'Input Unit' "
+ " ,[ERSConversionFactor_CF] AS 'Conversion Factor' "
+ " ,REPLACE ( [ERSBusinessLogic_MacroDesc] , 'Null' , '' ) AS 'Macro' "
+ " ,CONVERT(varchar(100), OutputUnit.[ERSUnit_Desc]) AS 'Output Unit' "
+ " ,TimeInput.[ERSTimeDimensionType_Desc] AS 'Input Time Dimension Type' "
+ " ,[ERSBusinessLogic_InputTimeDimensionValue] 'Input Time Value' "
+ " ,TimeOutput.[ERSTimeDimensionType_Desc] AS 'Output Time Dimension Type' "
+ " ,[ERSBusinessLogic_OutputTimeDimensionValue] AS 'Output Time Value' "
+ " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(InputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Input Geo Type' "
+ " ,REPLACE ( [ERSBusinessLogic_InputGeographyDimensionID] , ',' , ' ,' ) AS 'Input Geo Value' "
+ " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(OutputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Output Geo Type'"
+ " ,CONVERT(varchar(100), [ERSBusinessLogic_OutputGeographyDimensionID]) AS 'Output Geo Value' "
+ " ,[ERSDataPrivacy_Desc] AS 'Privacy' "
+ " ,[ERSBusinessLogic_Type] AS 'Type' "
+ " ,[ERSBusinessLogic_LongDesc] AS 'Long Description' "
+ " ,REPLACE ( [ERSBusinessLogic_InputSources] , ',' , ' ;' ) AS 'Input Sources' "
+ " FROM " + schemaName + "ERSBusinessLogic  "
+ " LEFT JOIN [CoSD].[ERSUnit_LU] AS InputUnit ON [ERSBusinessLogic_InputUnitID] like InputUnit.[ERSUnit_ID] "
+ " LEFT JOIN [CoSD].[ERSConversionFactors] ON [ERSBusinessLogic_ConvFactorID] = [ERSConversionFactorID] "
+ " LEFT JOIN [CoSD].[ERSUnit_LU] AS OutputUnit ON [ERSBusinessLogic_OutputUnitID] = OutputUnit.[ERSUnit_ID] "
+ " LEFT JOIN [CoSD].[ERSTimeDimensionType_LU] AS TimeInput ON [ERSBusinessLogic_InputTimeDimensionTypeID] = TimeInput.[ERSTimeDimensionType_ID] "
+ " LEFT JOIN [CoSD].[ERSTimeDimensionType_LU] AS TimeOutput ON [ERSBusinessLogic_OutputTimeDimensionTypeID] = TimeOutput.[ERSTimeDimensionType_ID] "
+ " LEFT JOIN [CoSD].[ERSDataPrivacy_LU] ON [ERSBusinessLogic_PrivacyID] = [ERSDataPrivacy_ID] "
+ " LEFT JOIN [CoSD].[ERSGeographyDimension_LU] AS InputGeo ON [ERSBusinessLogic_InputGeographyDimensionID] = CONVERT(varchar(100), InputGeo.ERSGeographyDimension_ID)"
+ " LEFT JOIN [CoSD].[ERSGeographyDimension_LU] AS OutputGeo ON [ERSBusinessLogic_OutputGeographyDimensionID] = OutputGeo.ERSGeographyDimension_ID"
+ " WHERE 1=1 " + sourceType + businessType + groupType + commodityDescription + commodity + statType+ " ORDER BY ERSBusinessLogic_SequenceID  ";

                SDA = new SqlDataAdapter(@"SELECT ERSBusinessLogic_ID AS 'Rule ID' "
+ " ,[ERSBusinessLogic_Formula] 'Formula' "
+ " ,[ERSBusinessLogic_OutputName] AS 'Output Name' "
+ " ,[ERSBusinessLogic_OutputDestination] AS 'Output Destination' "
+ " ,REPLACE ( [ERSBusinessLogic_InputDataSeries] , ',' , ' ;' ) AS 'Commodity Series ID' "
+ " ,InputUnit.[ERSUnit_Desc] AS 'Input Unit' "
+ " ,[ERSConversionFactor_CF] AS 'Conversion Factor' "
+ " ,REPLACE ( [ERSBusinessLogic_MacroDesc] , 'Null' , '' ) AS 'Macro' "
+ " ,CONVERT(varchar(100), OutputUnit.[ERSUnit_Desc]) AS 'Output Unit' "
+ " ,TimeInput.[ERSTimeDimensionType_Desc] AS 'Input Time Dimension Type' "
+ " ,[ERSBusinessLogic_InputTimeDimensionValue] 'Input Time Value' "
+ " ,TimeOutput.[ERSTimeDimensionType_Desc] AS 'Output Time Dimension Type' "
+ " ,[ERSBusinessLogic_OutputTimeDimensionValue] AS 'Output Time Value' "
+ " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(InputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Input Geo Type' "
+ " ,REPLACE ( [ERSBusinessLogic_InputGeographyDimensionID] , ',' , ' ,' ) AS 'Input Geo Value' "
+ " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(OutputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Output Geo Type'"
+ " ,CONVERT(varchar(100), [ERSBusinessLogic_OutputGeographyDimensionID]) AS 'Output Geo Value' "
+ " ,[ERSDataPrivacy_Desc] AS 'Privacy' "
+ " ,[ERSBusinessLogic_Type] AS 'Type' "
+ " ,[ERSBusinessLogic_LongDesc] AS 'Long Description' "
+ " ,REPLACE ( [ERSBusinessLogic_InputSources] , ',' , ' ;' ) AS 'Input Sources' "
+ " FROM " + schemaName + "ERSBusinessLogic  "
+ " LEFT JOIN [CoSD].[ERSUnit_LU] AS InputUnit ON [ERSBusinessLogic_InputUnitID] like InputUnit.[ERSUnit_ID] "
+ " LEFT JOIN [CoSD].[ERSConversionFactors] ON [ERSBusinessLogic_ConvFactorID] = [ERSConversionFactorID] "
+ " LEFT JOIN [CoSD].[ERSUnit_LU] AS OutputUnit ON [ERSBusinessLogic_OutputUnitID] = OutputUnit.[ERSUnit_ID] "
+ " LEFT JOIN [CoSD].[ERSTimeDimensionType_LU] AS TimeInput ON [ERSBusinessLogic_InputTimeDimensionTypeID] = TimeInput.[ERSTimeDimensionType_ID] "
+ " LEFT JOIN [CoSD].[ERSTimeDimensionType_LU] AS TimeOutput ON [ERSBusinessLogic_OutputTimeDimensionTypeID] = TimeOutput.[ERSTimeDimensionType_ID] "
+ " LEFT JOIN [CoSD].[ERSDataPrivacy_LU] ON [ERSBusinessLogic_PrivacyID] = [ERSDataPrivacy_ID] "
+ " LEFT JOIN [CoSD].[ERSGeographyDimension_LU] AS InputGeo ON [ERSBusinessLogic_InputGeographyDimensionID] = CONVERT(varchar(100), InputGeo.ERSGeographyDimension_ID)"
+ " LEFT JOIN [CoSD].[ERSGeographyDimension_LU] AS OutputGeo ON [ERSBusinessLogic_OutputGeographyDimensionID] = OutputGeo.ERSGeographyDimension_ID"
+ " WHERE 1=1 " + sourceType + businessType + groupType + commodityDescription + commodity + statType+" ORDER BY ERSBusinessLogic_SequenceID  ", con);

                SDA.SelectCommand.ExecuteNonQuery();
                DT = new DataTable();
                SDA.Fill(ds);
                ds.Tables[0].TableName = "Business Rules Export";
                if (ds.Tables[0].Rows.Count != 0)
                {
                    //Prepare Unit column
                    ds = prepareUnit(ds);

                    //Prepare rest of the fields.
                    ds = prepareOtherColumns(ds);

                }


                count1 = ds.Tables[0].Rows.Count;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dataGridView.DataSource = ds.Tables[0];
                    button_executeBL.Enabled = true;
                    button_executeBL.UseVisualStyleBackColor = true;
                    button_DeleteRules.UseVisualStyleBackColor = true;
                    button_DeleteRules.Enabled = true;
                    dataGridView.Columns[0].DefaultCellStyle.ForeColor = Color.Gray;
                    dataGridView.Columns[0].ReadOnly = true;
                    BindGrid();
                    label1.ForeColor = System.Drawing.Color.Black;
                    rowcount_label.ForeColor = System.Drawing.Color.Black;
                    label1.Text = "Number of formulas retrieved :";
                    rowcount_label.Text = count1.ToString();

                    if (Convert.ToDecimal(ds.Tables[0].Rows.Count) <= pageSize)
                    {

                        button_next.Enabled = false;
                        button_prev.Enabled = false;

                    }

                    else
                    {
                        button_next.Enabled = true;
                        button_prev.Enabled = false;

                    }
                    waitForm.Hide();

                }
                else
                {
                    waitForm.Hide();
                    MessageBox.Show("No Business Rules Found");
                    dataGridView.DataSource = ds.Tables[0];

                    longwait_label.Visible = false;
                    button_DeleteRules.Enabled = false;
                    button_DeleteRules.UseVisualStyleBackColor = false;
                    //button_DeleteRules.BackColor = Color.Gray;
                    button_executeBL.Enabled = false;
                    //button_executeBL.BackColor = Color.Gray;
                    button_update.Enabled = false;
                    //button_update.BackColor = Color.Gray;
                    label1.ForeColor = System.Drawing.Color.Red;
                    rowcount_label.ForeColor = System.Drawing.Color.Red;
                    rowcount_label.Text = count1.ToString();
                    button_prev.Enabled = false;
                    button_next.Enabled = false;
                    label_pageNum.Text = "0 / 0";

                }
            }
            catch (Exception ex)
            {
                waitForm.Hide();
                Console.WriteLine(ex.Message);
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                //if (ex.Message != "Connection Error")
                    saveAction(ex.Message);
            }
            finally
            {
                con.Close();
            }



        }


        private DataSet prepareOtherColumns(DataSet ds)
        {
            try
            {
                StringBuilder geoType = new StringBuilder();

                //Getting output geo value
                DataView viewOutputGeoValue = new DataView(ds.Tables["Business Rules Export"]);
                System.Data.DataTable distinctOutputGeoValues = viewOutputGeoValue.ToTable(true, "Output Geo Value");
                DataRow[] allOutputGeoValue = null;
                IList<string> OutputGeoValueidList = new List<string>();
                OutputGeoValueidList = (from x in distinctOutputGeoValues.AsEnumerable()
                                        select x.Field<string>("Output Geo Value")).ToList();

                string splitOutputGeoValue = string.Join(",", OutputGeoValueidList);

                string sqlOutputGeoValue = " SELECT ERSGeographyDimension_ID, "
+ " [ERSGeographyType_Desc], "
+ " CASE  "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID IN (1,2,7,8) THEN ERSGeographyDimension_Country "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 3 THEN ERSGeographyDimension_State "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 4 THEN ERSGeographyDimension_County "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 5 THEN ERSGeographyDimension_City "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 6 THEN ERSGeographyDimension_Region "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 10 THEN ERSGeographyDimension_City "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 11 THEN ERSGeographyDimension_State "
+ " ELSE NULL END AS GeographyValue "
+ " FROM  "+schemaName+"[ERSGeographyDimension_LU] "
+ " LEFT JOIN [CoSD].[ERSGeographyType_LU] ON ERSGeographyDimension_ERSGeographyType_ID = [ERSGeographyType_ID] "
+ " where ERSGeographyDimension_ID IN (SELECT [ERSBusinessLogic_OutputGeographyDimensionID] FROM " + schemaName + "[ERSBusinessLogic])";

                System.Data.DataTable OutputGeoValue = GetData(sqlOutputGeoValue);


                //Getting Input geo value
                DataView viewInputGeoValue = new DataView(ds.Tables["Business Rules Export"]);
                System.Data.DataTable distinctInputGeoValues = viewInputGeoValue.ToTable(true, "Input Geo Value");
                DataRow[] allInputGeoValue = null;
                IList<string> InputGeoValueidList = new List<string>();
                InputGeoValueidList = (from x in distinctInputGeoValues.AsEnumerable()
                                       select x.Field<string>("Input Geo Value")).ToList();

                string splitInputGeoValue = string.Join(",", OutputGeoValueidList);

                string sqlInputGeoValue = " SELECT ERSGeographyDimension_ID, "
+ " [ERSGeographyType_Desc], "
+ " CASE  "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID IN (1,2,7,8) THEN ERSGeographyDimension_Country "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 3 THEN ERSGeographyDimension_State "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 4 THEN ERSGeographyDimension_County "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 5 THEN ERSGeographyDimension_City "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 6 THEN ERSGeographyDimension_Region "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 10 THEN ERSGeographyDimension_City "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 11 THEN ERSGeographyDimension_State "
+ " ELSE NULL END AS GeographyValue "
+ " FROM  "+schemaName+"[ERSGeographyDimension_LU] "
+ " LEFT JOIN [CoSD].[ERSGeographyType_LU] ON ERSGeographyDimension_ERSGeographyType_ID = [ERSGeographyType_ID] "
+ " where CONVERT(varchar(100),ERSGeographyDimension_ID) IN (SELECT [ERSBusinessLogic_InputGeographyDimensionID] FROM " + schemaName + "[ERSBusinessLogic])";

                System.Data.DataTable InputGeoValue = GetData(sqlInputGeoValue);

                foreach (DataRow row in ds.Tables["Business Rules Export"].Rows)
                {
                    //output
                    geoType.Clear();
                    string blOutputGeo_id = row["Output Geo Value"].ToString();
                    allOutputGeoValue = OutputGeoValue.Select("ERSGeographyDimension_ID = '" + blOutputGeo_id + "'");
                    if (allOutputGeoValue.Length > 0)
                    {
                        row.SetField("Output Geo Value", allOutputGeoValue[0].ItemArray[2].ToString());
                        row.SetField("Output Geo Type", allOutputGeoValue[0].ItemArray[1].ToString());
                    }
                    allOutputGeoValue = null;

                    //Input
                    string blInputGeo_id = row["Input Geo Value"].ToString();
                    if (!blInputGeo_id.Contains(","))
                    {
                        allInputGeoValue = InputGeoValue.Select("ERSGeographyDimension_ID = '" + blInputGeo_id + "'");
                        if (allInputGeoValue.Length > 0)
                        {
                            row.SetField("Input Geo Value", allInputGeoValue[0].ItemArray[2].ToString());
                            row.SetField("Input Geo Type", allInputGeoValue[0].ItemArray[1].ToString());
                        }
                    }
                    else
                    {
                        string sql_CommaInputGeoValue = " SELECT ERSGeographyDimension_ID, "
+ " [ERSGeographyType_Desc], "
+ " CASE  "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID IN (1,2,7,8) THEN ERSGeographyDimension_Country "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 3 THEN ERSGeographyDimension_State "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 4 THEN ERSGeographyDimension_County "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 5 THEN ERSGeographyDimension_City "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 6 THEN ERSGeographyDimension_Region "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 10 THEN ERSGeographyDimension_City "
+ " WHEN ERSGeographyDimension_ERSGeographyType_ID = 11 THEN ERSGeographyDimension_State "
+ " ELSE NULL END AS GeographyValue "
+ " FROM  "+schemaName+"[ERSGeographyDimension_LU] "
+ " LEFT JOIN [CoSD].[ERSGeographyType_LU] ON ERSGeographyDimension_ERSGeographyType_ID = [ERSGeographyType_ID] "
+ " where ERSGeographyDimension_ID IN (" + blInputGeo_id + ")";

                        //converting the seperation
                        blInputGeo_id = blInputGeo_id.Replace(",", ";");

                        System.Data.DataTable CommaInputValue = GetData(sql_CommaInputGeoValue);
                        if (null != CommaInputValue && CommaInputValue.Rows.Count != 0)
                        {
                            DataRow[] dr = CommaInputValue.Select();

                            for (int i = 0; i < dr.Length; i++)
                            {
                                blInputGeo_id = blInputGeo_id.Replace(dr[i].ItemArray[0].ToString(), dr[i].ItemArray[2].ToString());
                                geoType.Append(dr[i].ItemArray[1].ToString() + "; ");
                                //row.SetField("Input Geo Type", dr[i].ItemArray[1].ToString());
                            }

                        }
                        row.SetField("Input Geo Value", blInputGeo_id);
                        row.SetField("Input Geo Type", geoType.ToString().Remove(geoType.ToString().Length - 2));

                    }
                    allInputGeoValue = null;

                    //Filling Status (New/Override)
                    //row.SetField("Status (New/Override)", "Override");

                    //Prepare Commodity Series ID
                    string commoditySeriesID = row["Commodity Series ID"].ToString();
                    string[] inputsArray = { };
                    inputsArray = commoditySeriesID.Replace(" ", string.Empty).Split(';');
                    string outputDestination = row["Output Destination"].ToString();

                    //get thousand number
                    int thousandNumber = getThoudandNumberView(outputDestination);

                    //Set output destination
                    outputDestination = getOutputDestinationView(outputDestination, thousandNumber);
                    row.SetField("Output Destination", outputDestination);

                    //get formula                    
                    string formula = row["Formula"].ToString();
                    formula = getFormulaView(formula, inputsArray, thousandNumber);
                    row.SetField("Formula", formula);

                    //prepare Commodity Series ID
                    commoditySeriesID = getCommoditySeriesIDView(commoditySeriesID, inputsArray, thousandNumber);
                    row.SetField("Commodity Series ID", commoditySeriesID);


                    //prepare Input TimeDimensionType
                    string inputTimeDimensionType = row["Input Time Dimension Type"].ToString();
                    inputTimeDimensionType = getTimeDimensionTypeView(inputTimeDimensionType);
                    row.SetField("Input Time Dimension Type", inputTimeDimensionType);

                    //prepare Output TimeDimensionType
                    string outputTimeDimensionType = row["Output Time Dimension Type"].ToString();
                    outputTimeDimensionType = getTimeDimensionTypeView(outputTimeDimensionType);
                    row.SetField("Output Time Dimension Type", outputTimeDimensionType);

                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }

        }


        private DataSet prepareUnit(DataSet ds)
        {
            try
            {
                IList<int> BLid = new List<int>();
                BLid = (from x in ds.Tables["Business Rules Export"].AsEnumerable()
                        where x.Field<string>("Input Unit") == null
                        select x.Field<int>("Rule ID")).ToList();


                if (BLid.Count != 0)
                {
                    string splitBLid = string.Join(",", BLid);

                    string sqlBl_Unit = "SELECT [ERSBusinessLogic_ID],[ERSBusinessLogic_InputUnitID] FROM  "+schemaName+"[ERSBusinessLogic] where ERSBusinessLogic_ID IN (" + splitBLid + ")";

                    System.Data.DataTable BL_Unit = GetData(sqlBl_Unit);

                    IList<string> unitId = BL_Unit.AsEnumerable().Select(x => x[1].ToString()).ToList();

                    string splitUnitid = string.Join(",", unitId);

                    string UnitDesc = "SELECT [ERSUnit_ID],[ERSUnit_Desc]FROM  "+schemaName+"[ERSUnit_LU] where ERSUnit_ID IN (" + splitUnitid + ")";

                    System.Data.DataTable unitTable = GetData(UnitDesc);

                    StringBuilder unitdesc = new StringBuilder();
                    DataRow[] resultBlUnit = null;
                    DataRow[] allResult = null;

                    foreach (DataRow row in BL_Unit.Rows)
                    {
                        //if (row["ERSBusinessLogic_ID"].ToString() == "2")
                        //    row.SetField("Input Unit", "Hello");
                        unitdesc.Clear();
                        string bl_unit = row["ERSBusinessLogic_InputUnitID"].ToString();

                        List<string> bl_unitList = bl_unit.Split(',').ToList();

                        foreach (string item in bl_unitList)
                        {
                            resultBlUnit = unitTable.Select("ERSUnit_ID = '" + item + "'");
                            unitdesc.Append(resultBlUnit[0].ItemArray[1].ToString() + "; ");
                            resultBlUnit = null;

                        }
                        row.SetField("ERSBusinessLogic_InputUnitID", unitdesc.ToString().Remove(unitdesc.ToString().Length - 2));
                    }

                    foreach (DataRow row in ds.Tables["Business Rules Export"].Rows)
                    {
                        string bl_id = row["Rule ID"].ToString();
                        allResult = BL_Unit.Select("ERSBusinessLogic_ID = '" + bl_id + "'");
                        if (allResult.Length > 0)
                            row.SetField("Input Unit", allResult[0].ItemArray[1].ToString());
                        allResult = null;

                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                return ds;
            }
        }

        //get time dimention type
        private string getTimeDimensionTypeView(string timeDimensionType)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!            
            try
            {
                timeDimensionType = timeDimensionType.Replace("ERS", "");

                //if (timeDimensionType.Equals(("ERSMonth"), StringComparison.OrdinalIgnoreCase))
                //{
                //    timeDimensionType = "Month";
                //}
                if (timeDimensionType.Equals(("CalendarYear"), StringComparison.OrdinalIgnoreCase))
                {
                    timeDimensionType = "Year";
                }
                //else if (timeDimensionType.Equals(("ERSMarketingYear"), StringComparison.OrdinalIgnoreCase))
                //{
                //    timeDimensionType = "Marketing Year";
                //}

                return timeDimensionType;
            }
            catch (Exception)
            {

                return timeDimensionType;
            }
        }
        //get formula
        private string getFormulaView(string formula, string[] inputsArray, int thousandNumber)
        {
            string newFormula = formula;
            int newThousandNumber = 0;
            int thousandNumber_N = 0;
            string sourceSeriesID = "";
            newSeriesIds = new Dictionary<string, string> { };

            try
            {

                for (int counter = 0; counter < inputsArray.Length; counter++)
                {
                    if (newFormula.Contains(inputsArray[counter]))
                    {
                        if ((inputsArray[counter].Contains("C") || inputsArray[0].Contains("c")))
                        {
                            newThousandNumber = Convert.ToInt32(Regex.Match(inputsArray[counter], @"\d+").Value) - thousandNumber;
                            newFormula = newFormula.Replace(inputsArray[counter], "[CV" + newThousandNumber + "]");
                        }
                        else
                        {
                            try
                            {
                                //Get commodity ID with stat type
                                string sqlcommodity_Stat = "SELECT [ERSStatisticType_Attribute],ERSCommodity_SourceSeriesID FROM " + schemaName + "[ERSCommodityDataSeries] LEFT JOIN [CoSD].[ERSStatisticType_LU] ON [ERSStatisticType_ID] = [ERSCommodity_ERSStatisticType_ID] where ERSCommodity_ID IN (SELECT [ERSCommodity_ID] FROM  " + schemaName + "[ERSCommodityDataSeries] where [ERSCommodity_ID] = " + inputsArray[counter] + ")";
                                System.Data.DataTable dtcommodity_Stat = GetData(sqlcommodity_Stat);
                                DataRow[] drcommodity_Stat = dtcommodity_Stat.Select();

                                newFormula = newFormula.Replace(drcommodity_Stat[0]["ERSStatisticType_Attribute"].ToString(), "");
                                sourceSeriesID = Regex.Match(drcommodity_Stat[0]["ERSCommodity_SourceSeriesID"].ToString(), @"\(([^)]*)\)").Groups[1].Value.Trim();
                                if (!sourceSeriesID.Equals("") && sourceSeriesID.Contains("N"))
                                {


                                    if (sourceSeriesID.Length == 5)
                                        thousandNumber_N = Convert.ToInt32(sourceSeriesID.Substring(1, 1));
                                    else if (sourceSeriesID.Length == 6)
                                        thousandNumber_N = Convert.ToInt32(sourceSeriesID.Substring(1, 2));
                                    else if (sourceSeriesID.Length == 7)
                                        thousandNumber_N = Convert.ToInt32(sourceSeriesID.Substring(1, 3));

                                    thousandNumber_N = thousandNumber_N * 1000;
                                    newThousandNumber = Convert.ToInt32(Regex.Match(sourceSeriesID, @"\d+").Value) - thousandNumber_N;
                                    newFormula = newFormula.Replace("(" + inputsArray[counter] + ")", "[N" + newThousandNumber + "]");
                                    //saving values
                                    newSeriesIds.Add(inputsArray[counter], "N" + newThousandNumber);
                                }
                                else
                                    newFormula = newFormula.Replace("(" + inputsArray[counter] + ")", "[" + inputsArray[counter] + "]");

                            }
                            catch (Exception)
                            {
                                return formula;
                            }

                        }
                    }
                }
                return newFormula;
            }
            catch (Exception)
            {
                return formula;
            }
        }


        // get inputs
        private string getCommoditySeriesIDView(string commoditySeriesID, string[] inputsArray, int thousandNumber)
        {
            string inputs = "";
            StringBuilder inputStringBuilder = new StringBuilder();
            int newThousandNumber = 0;
            try
            {
                for (int counter = 0; counter < inputsArray.Length; counter++)
                {
                    if (inputsArray[counter].Contains("C") || inputsArray[0].Contains("c"))
                    {
                        newThousandNumber = Convert.ToInt32(Regex.Match(inputsArray[counter], @"\d+").Value) - thousandNumber;
                        //inputs = inputs.Replace(inputsArray[counter], "CV" + newThousandNumber);
                        inputs = "CV" + newThousandNumber;
                    }
                    else
                    {
                        if (newSeriesIds.ContainsKey(inputsArray[counter]))
                            newSeriesIds.TryGetValue(inputsArray[counter], out inputs);
                        else
                            inputs = inputsArray[counter];

                    }

                    inputStringBuilder.Append(inputs);
                    if ((counter + 1) != inputsArray.Length)
                    {
                        inputStringBuilder.Append("; ");
                    }
                }

                return inputStringBuilder.ToString();
            }
            catch (Exception ex)
            {
                return commoditySeriesID;
            }
        }

        // get thousand number
        private int getThoudandNumberView(string outputDestination)
        {
            try
            {
                string outputDestinationtemp = outputDestination;
                int thousandNumber = 0;
                if (outputDestinationtemp.Contains("N"))
                {
                    outputDestinationtemp = Regex.Match(outputDestinationtemp, @"\(([^)]*)\)").Groups[1].Value.Trim();
                    if (outputDestinationtemp.Length == 5)
                        thousandNumber = Convert.ToInt32(outputDestinationtemp.Substring(1, 1)) * 1000;
                    else if (outputDestinationtemp.Length == 6)
                        thousandNumber = Convert.ToInt32(outputDestinationtemp.Substring(1, 2)) * 1000;
                    else if (outputDestinationtemp.Length == 7)
                        thousandNumber = Convert.ToInt32(outputDestinationtemp.Substring(1, 3)) * 1000;
                }
                else
                {
                    if (outputDestinationtemp.Length == 6)
                    {
                        thousandNumber = Convert.ToInt32(outputDestinationtemp.Substring(2, 1)) * 1000;
                    }
                    else if (outputDestinationtemp.Length == 7)
                        thousandNumber = Convert.ToInt32(outputDestinationtemp.Substring(2, 2)) * 1000;
                    else if (outputDestinationtemp.Length == 8)
                        thousandNumber = Convert.ToInt32(outputDestinationtemp.Substring(2, 3)) * 1000;
                }
                return thousandNumber;
            }
            catch (Exception)
            {
                return -1;
            }

            
        }
        //get output destination
        private string getOutputDestinationView(string outputDestination, int thousandNumber)
        {
            try
            {
                int outputDestinationNumber = Convert.ToInt32(Regex.Match(outputDestination, @"\d+").Value);
                outputDestinationNumber = outputDestinationNumber - thousandNumber;
                if (outputDestination.Contains("CV") && !outputDestination.Contains("N"))
                {
                    return "CV" + outputDestinationNumber;
                }
                else if (outputDestination.Contains("N"))
                {
                    return "N" + outputDestinationNumber;
                }
                else
                    return "";
            }
            catch (Exception)
            {
                return outputDestination;
            }
        }


        private void button_update_Click(object sender, EventArgs e)
        {
            con.Open();

            try
            {

           
                if (MessageBox.Show("Do you really want to Update these values?", "Confirm Update", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //start wait form
                    waitForm.Show(this);
                    waitForm.Refresh();

                    if (dataGridView.SelectedCells.Count > 0)
                    {
                       // int selectedrowindex = dataGridView.SelectedCells[0].RowIndex;
                        string status = "";

                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {

                           


                            businessRuleID = Convert.ToString(row.Cells["Rule ID"].Value);
                            commoditySeriesID = Convert.ToString(row.Cells["Commodity Series ID"].Value);
                            inputUnit = Convert.ToString(row.Cells["Input Unit"].Value);
                            conversionFactor = Convert.ToString(row.Cells["Conversion Factor"].Value);
                            macro = Convert.ToString(row.Cells["Macro"].Value);
                            outputName = Convert.ToString(row.Cells["Output Name"].Value);
                            outputUnit = Convert.ToString(row.Cells["Output Unit"].Value);
                            formula = Convert.ToString(row.Cells["Formula"].Value);
                            inputTimeDimensionType = Convert.ToString(row.Cells["Input Time Dimension Type"].Value);
                            inputTimeDimensionTypeValue = Convert.ToString(row.Cells["Input Time Value"].Value);
                            outputTimeDimensionType = Convert.ToString(row.Cells["Output Time Dimension Type"].Value);
                            outputTimeDimensionTypeValue = Convert.ToString(row.Cells["Output Time Value"].Value);
                            inputgeoType = Convert.ToString(row.Cells["Input Geo Type"].Value);
                            inputgeoValue = Convert.ToString(row.Cells["Input Geo Value"].Value);
                            outputgeoType = Convert.ToString(row.Cells["Output Geo Type"].Value);
                            outputgeoValue = Convert.ToString(row.Cells["Output Geo Value"].Value);
                            outputDestination = Convert.ToString(row.Cells["Output Destination"].Value);
                            privacy = Convert.ToString(row.Cells["Privacy"].Value);
                            BLType = Convert.ToString(row.Cells["Type"].Value);
                            longDescription = Convert.ToString(row.Cells["Long Description"].Value);
                            inputSources = Convert.ToString(row.Cells["Input Sources"].Value);

                            //get inputs count
                            //int inputsCount = 0;
                            string[] commoditySeriesArray = { };
                    try
                    {
                        commoditySeriesArray = commoditySeriesID.Replace(" ", string.Empty).Split(';');
                        Match matchcommoditySeriesID = Regex.Match(commoditySeriesID.Replace(" ", ""), @"^[0-9;CVN]*$");

                        if (!matchcommoditySeriesID.Success)
                        {
                            MessageBox.Show("Please enter Commodity Series ID in correct format. Only digits, CV, N and ';' is allowed.");
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Please use only ';'(Semi-Colon) to seperate Commodity Series IDs");
                        return;
                    }


                    //get thousandNumber for CV and N
                    int thousandNumber = 0;
                    try
                    {
                        thousandNumber = getThoudandNumber(businessRuleID, commoditySeriesArray);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Could not find correct thousand number. Please check the Business Rule ID and Output destination are correct.");
                        return;
                    }

                    //get Output Destination
                    try
                    {
                        int outputDestinationNumber = Convert.ToInt32(Regex.Match(outputDestination, @"\d+").Value);
                        outputDestination = getOutputDestination(businessRuleID, outputDestination, thousandNumber, outputDestinationNumber);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        return;
                    }

                    //get formula
                    //get inputs count and input array
                    int inputsCount = 0;
                    string[] inputsArray;
                    string pattern = @"\[(.*?)\]";
                    var matches = Regex.Matches(formula, pattern);
                    inputsArray = new string[matches.Count];
                    int counter = 0;
                    foreach (Match match in matches)
                    {
                        inputsArray[counter] = match.Groups[1].Value;
                        counter++;
                    }
                    inputsCount = inputsArray.Length;

                    //get keywords
                    //Store list of valid keywords for formula from CoSD Keywords.TXT
                    string[] keywords = getKeywords();

                    try
                    {
                        formula = getFormula(formula, commoditySeriesArray, thousandNumber, keywords);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Brackets"))
                            MessageBox.Show(ex.Message);
                        else if (ex.Message.Contains("keyword"))
                            MessageBox.Show(ex.Message);
                        else if (ex.Message.Contains("New data series"))
                            MessageBox.Show(ex.Message);
                        else if (ex.Message.Contains("Data series"))
                             MessageBox.Show(ex.Message);
                        else if (ex.Message.Contains("not used in formula"))
                             MessageBox.Show(ex.Message);
                        else
                             MessageBox.Show("Entered formula is not correct.");
                        return;
                    }


                    //get inputs
                    string inputs = "";
                    try
                    {
                        inputs = getInputs(inputsArray, thousandNumber);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("New data series"))
                            MessageBox.Show( ex.Message);
                        else if (ex.Message.Contains("make sure"))
                            MessageBox.Show( ex.Message);
                        else
                            MessageBox.Show("One of the input data series does not exist.");
                        return;
                    }


                    //Get input data series ID
                    try
                    {
                        commoditySeriesID = getInputDataSeriesID(inputs);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Please use only ';'(Semi-Colon) to seperate Commodity Series IDs. Make sure the CVs and Ns are in correct format. For example 'CV1' or 'N1'");
                        return;
                    }


                    //get Group ID
                    string groupID = "";
                    try
                    {
                        groupID = getGroupId(commoditySeriesID);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not find a data series. Please make sure all the input data series IDs are correct");
                        return;
                    }

                    // get Input unit
                    string InputUnitIDs = "";
                    try
                    {
                        InputUnitIDs = getUnitID(inputUnit.Trim());
                    }
                    catch (Exception)
                    {

                        if (inputUnit.Trim().Equals("1000"))
                        {
                            MessageBox.Show("The Input Unit is not correct. If you meant '$1,000' then change the excel cell format from Currency/Number to text.");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Please make sure the input Unit is correct. Use ';'(Semi-Colon) to seperate multiple Units");
                            return;
                        }

                    }

                    // get macro
                    if (!macro.Equals(""))
                    {

                        try
                        {
                            if (getMacro(macro.Trim()).Equals("Found"))
                            {
                                macro = macro.ToString().Trim();
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                    }
                    else
                        macro = "NULL";

                    // get conversion factor
                    string conversionFactorID = "";
                    if (!conversionFactor.Equals(""))
                    {
                        try
                        {
                            conversionFactorID = getConversionFactorID(conversionFactor.Trim());
                        }
                        catch (Exception)
                        {
                            //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                            MessageBox.Show("Please make sure the conversion factor is available in the database.");
                            return;
                        }
                    }
                    else if (conversionFactor.Equals("") && formula.Contains("conv"))
                    {
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        MessageBox.Show("Please provide the value of conversion factor.");
                        return;
                    }
                    else if (macro.Equals("") && formula.Contains("conv"))
                    {
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        MessageBox.Show("Please provide the macro description.");
                        return;
                    }
                    else
                        conversionFactorID = "NULL";


                    //get output unit
                    string outputUnitID = "";
                    try
                    {
                        outputUnitID = getUnitID(outputUnit.Trim());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Please make sure the output Unit is correct.");
                        return;
                    }

                    //get Input Time Dimension Type
                    string inputTimeDimensionTypeID = "";
                    try
                    {
                        inputTimeDimensionTypeID = getTimeDimensionTypeID(inputTimeDimensionType.Trim());
                    }
                    catch (Exception)
                    {
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        MessageBox.Show("Please make sure the Input Time Dimension Type is correct. Valid Inputs are Month, Year and Marketing Year");
                        return;
                    }

                    //get Output Time Dimension Type
                    string outputTimeDimensionTypeID = "";
                    try
                    {
                        outputTimeDimensionTypeID = getTimeDimensionTypeID(outputTimeDimensionType.Trim());
                    }
                    catch (Exception)
                    {
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        MessageBox.Show("Please make sure the Output Time Dimension Type is correct.");
                         return;
                    }




                    //get Input Geo Type and value
                    string inputgeoTypeID = "";
                    string inputgeoValueID = "";
                    try
                    {
                        inputgeoTypeID = getGeoTypeID(inputgeoType);

                    }
                    catch (Exception)
                    {
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        MessageBox.Show("Please enter a valid input Geo Type like Country, State or Region.");
                        return;

                    }
                    try
                    {
                        inputgeoValueID = getGeoValueID(inputgeoTypeID, inputgeoValue);

                    }
                    catch (Exception ex)
                    {
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        if (ex.Message.Contains("same number") || ex.Message.Contains("correct Geogrphy"))
                            MessageBox.Show(ex.Message);
                        else
                            MessageBox.Show("Please enter a valid input Geo Value for given Geo Type.");
                        return;

                    }



                    //get Output Geo Type and value
                    string outputgeoTypeID = "";
                    string outputgeoValueID = "";
                    try
                    {
                        outputgeoTypeID = getGeoTypeID(outputgeoType);

                    }
                    catch (Exception)
                    {
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        MessageBox.Show("Please enter a valid output Geo Type like Country, State or Region.");
                        return;

                    }
                    try
                    {
                        outputgeoValueID = getGeoValueID(outputgeoTypeID, outputgeoValue);

                    }
                    catch (Exception)
                    {
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        MessageBox.Show("Please enter a valid output Geo Value for given Geo Type.");
                        return;

                    }


                    //get privacy
                    string privacyID = "";

                    try
                    {
                        privacyID = getPrivacy(privacy.Trim());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Please enter a valid privacy type like Private or Public.");
                        return;
                    }



                            //get Input Sources
                            string inputSourcesID = "";
                            try
                            {
                                inputSourcesID = getInputSourcesID(inputSources, outputName);

                            }
                            catch (Exception ex)
                            {
                                //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                if (ex.Message.Contains("not found"))
                                MessageBox.Show(ex.Message);
                                else
                                MessageBox.Show("Source '" + inputSources + "' not found. Please enter a correct source description.");
                                return;

                            }

                           

                            updateStatus = updateBusinessRules(businessRuleID, commoditySeriesID, inputsCount, inputs, InputUnitIDs, conversionFactorID, macro, outputName, outputUnitID, formula, inputTimeDimensionTypeValue, inputTimeDimensionTypeID, outputTimeDimensionTypeValue, outputTimeDimensionTypeID, inputgeoValueID, outputgeoValueID, outputDestination, privacyID, BLType, longDescription, inputSources, inputSourcesID, groupID);
                            status = status + updateStatus;


                            //SqlDataAdapter SDA = new SqlDataAdapter(@"Update " + schemaName + "[ERSBusinessLogic] SET [ERSBusinessLogic_Formula] = '" + Formula + "',[ERSBusinessLogic_InputsCount] = " + numberofinputs + ",[ERSBusinessLogic_Inputs] = '" + Inputs + "',[ERSBusinessLogic_InputTimeDimensionValue] ='" + Time + "',[ERSBusinessLogic_Type]='" + BusinessLogicType + "',[ERSBusinessLogic_LongDesc]='" + Description + "',[ERSBusinessLogic_InputSources]='" + Source + "',[ERSBusinessLogic_OutputName]='" + Output + "',[ERSBusinessLogic_OutputDestination]='" + ConstructedVariable + "',[ERSBusinessLogic_OutputTimeDimensionValue]='" + OutputTime + "',[ERSBusinessLogic_PrivacyID]=" + Privacy + " WHERE [ERSBusinessLogic_ID] IN  (" + IDvalue + " )", con);
                            //SDA.SelectCommand.ExecuteNonQuery();
                            //MessageBox.Show("Updates successfully submitted to CoSD");
                            //string sql = "Update " + schemaName + "[ERSBusinessLogic] SET [ERSBusinessLogic_Formula] = '" + Formula + "',[ERSBusinessLogic_InputsCount] = " + numberofinputs + ",[ERSBusinessLogic_Inputs] = '" + Inputs + "',[ERSBusinessLogic_InputTimeDimensionValue] ='" + Time + "',[ERSBusinessLogic_Type]='" + BusinessLogicType + "',[ERSBusinessLogic_LongDesc]='" + Description + "',[ERSBusinessLogic_InputSources]='" + Source + "',[ERSBusinessLogic_OutputName]='" + Output + "',[ERSBusinessLogic_OutputDestination]='" + ConstructedVariable + "',[ERSBusinessLogic_OutputTimeDimensionValue]='" + OutputTime + "',[ERSBusinessLogic_PrivacyID]=" + Privacy + " WHERE [ERSBusinessLogic_ID] IN  (" + IDvalue + " )";
                            //saveAction(sql);
                            //scb = new SqlCommandBuilder(SDA);
                            //SDA.Update(DT);

                            // confirm

                        }

                        waitForm.Hide();

                        if (status.Contains("failed") || status.Contains("duplicate"))
                        {
                            MessageBox.Show("Update Failed. Please make sure the input data is correct or contact technical team.");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Business Rule updated successfully.");
                            return;
                        }
                        //else if (updateStatus == "duplicate")
                        //{
                        //    MessageBox.Show("Update Failed. Similar business rule already exist in the system.");
                        //    return;
                        //}

                    }
                }

                else
                {
                    return;
                }

            }

            catch (Exception ex)
            {
                waitForm.Hide();
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                //saveAction(ex.Message);
            }

            finally
            {

                con.Close();
                if (updateStatus == "success")
                button_retreive_Click(sender, e);
            }

        }

        private string[] getKeywords()
        {
            try
            {
                var assemblyKeywords = Assembly.GetExecutingAssembly();
                using (var resourceStreamKeywords = assemblyKeywords.GetManifestResourceStream("CoSD_Tool.Resources.Keywords.txt"))
                using (StreamReader readerKeywords = new StreamReader(resourceStreamKeywords))
                {
                    string keywords = readerKeywords.ReadToEnd();

                    return keywords.ToUpper().Split(',');
                }
            }
            catch (Exception)
            {
                throw new Exception("Error reading Keywords.txt from CoSD Tool. Please contact technical team");
            }
        }

        //update business rules
        private string updateBusinessRules(string businessLogicID, string commoditySeriesID, int inputsCount, string inputs, string InputUnitIDs, string conversionFactorID, string macro, string outputName, string outputUnitID, string formula, string inputTimeDimensionTypeValue, string inputTimeDimensionTypeID, string outputTimeDimensionTypeValue, string outputTimeDimensionTypeID, string inputgeoValueID, string outputgeoValueID, string outputDestination, string privacyID, string BLType, string longDescription, string inputSources, string inputSourcesID, string groupID)
        {
            string sql = "UPDATE [CoSD].[ERSBusinessLogic] SET " +
           "  [ERSBusinessLogic_Formula] = '" + formula + "'" +
           " ,[ERSBusinessLogic_InputsCount] = " + inputsCount +
           " ,[ERSBusinessLogic_Inputs] = '" + inputs + "'" +
           " ,[ERSBusinessLogic_ConvFactorID] = " + conversionFactorID +
           " ,[ERSBusinessLogic_MacroDesc] = '" + macro + "'" +
           " ,[ERSBusinessLogic_InputDataSeries] = '" + commoditySeriesID + "'" +
           " ,[ERSBusinessLogic_InputTimeDimensionValue] = '" + inputTimeDimensionTypeValue + "'" +
           " ,[ERSBusinessLogic_InputTimeDimensionTypeID] = " + inputTimeDimensionTypeID +
           " ,[ERSBusinessLogic_InputGeographyDimensionID] = '" + inputgeoValueID + "'" +
           " ,[ERSBusinessLogic_OutputGeographyDimensionID] = " + outputgeoValueID +
           " ,[ERSBusinessLogic_InputUnitID] = '" + InputUnitIDs + "'" +
           " ,[ERSBusinessLogic_Type] = '" + BLType + "'" +
           " ,[ERSBusinessLogic_PrivacyID] = " + privacyID +
           " ,[ERSBusinessLogic_LongDesc] = '" + longDescription + "'" +
           " ,[ERSBusinessLogic_InputSources] = '" + inputSources + "'" +
           " ,[ERSBusinessLogic_InputSourceID] = " + inputSourcesID +
           " ,[ERSBusinessLogic_OutputName] = '" + outputName + "'" +
           " ,[ERSBusinessLogic_OutputUnitID] = " + outputUnitID +
           " ,[ERSBusinessLogic_OutputDestination] = '" + outputDestination + "'" +
           " ,[ERSBusinessLogic_OutputTimeDimensionValue] = '" + outputTimeDimensionTypeValue + "'" +
           " ,[ERSBusinessLogic_OutputTimeDimensionTypeID] = " + outputTimeDimensionTypeID +
           " ,[ERSBusinessLogic_GroupID] = " + groupID +
           " WHERE [ERSBusinessLogic_ID] = " + businessLogicID;

            // execute
            try
            {
                System.Data.DataTable dt = GetData(sql);
                saveAction(sql);
                if (null != dt)
                {
                    return "success";
                }
                else if (errorMessage == "duplicate")
                {
                    errorMessage = "";
                    return "duplicate";
                }
                else
                    return "failed";

            }
            catch (SqlException ex)
            {
                saveAction("Update business rules via upload sheet action failed.");
               
               
            }
            return "success";

        }


        /// <summary>
        /// load Macro id with unit name
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private string getMacro(string macro)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!            
            try
            {
                // need to take care if not found !!!!!!!!!!!!!!!!!!
                string sql = "SELECT TOP 1 [ERSMacro_ID] FROM " + schemaName + "[ERSMacro_LU] where ERSMacro_Desc like '%" + macro + "%'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                if (dr.Length > 0)
                    return "Found";
                else
                {
                    throw new Exception("Macro description '" + macro + "' not Found. Please use correct macro description");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //get input sourcces
        private string getInputSourcesID(string inputSources, string outputName)
        {
            //making case insensitive
            inputSources = inputSources.ToUpper().Trim();
            outputName = outputName.ToUpper();
            string sqlgeoType = "";
            if (inputSources.Contains("MACRO"))
            {
                return "37";
            }
            if (inputSources.Contains(";"))
                inputSources = inputSources.Split(';')[0].Trim();

            sqlgeoType = "select [ERSSource_ID] from " + schemaName + "[ERSSource_LU] where [ERSSource_Desc] like '" + inputSources + "'";
            System.Data.DataTable dtType = GetData(sqlgeoType);
            DataRow[] drType = dtType.Select();
            if (drType.Length == 0)
            {
                sqlgeoType = "select [ERSSource_ID] from " + schemaName + "[ERSSource_LU] where [ERSSource_Abbrev] like '" + inputSources + "'";
                System.Data.DataTable dtAbbrev = GetData(sqlgeoType);
                DataRow[] drAbbrev = dtAbbrev.Select();
                if (drAbbrev.Length != 1)
                {
                    return getSourcesLookupTemp(inputSources, outputName);
                }
                else
                    return drAbbrev[0]["ERSSource_ID"].ToString();
            }
            else if (drType.Length > 1)
            {
                return getSourcesLookupTemp(inputSources, outputName);
            }
            else
                return drType[0]["ERSSource_ID"].ToString();



        }

        private string getSourcesLookupTemp(string inputSource, string outputName)
        {
            string sourceID = "";


            if (inputSource.Contains("ERS"))
            {
                sourceID = "12";
            }
            else
            {
                switch (inputSource)
                {
                    case "TRADE":
                        if (outputName.Contains("IMPORT") && outputName.Contains("EXPORT"))
                            sourceID = "25";
                        if (outputName.Contains("IMPORT"))
                            sourceID = "4";
                        else if (outputName.Contains("EXPORT"))
                            sourceID = "3";
                        else
                            sourceID = "25";
                        break;
                    case "NASS":
                        sourceID = "92";
                        break;
                    case "NASS data not available in Quick Stats":
                        sourceID = "39";
                        break;
                    case "NASS-CA":
                        sourceID = "78";
                        break;
                    case "CCAC":
                        if (inputSource.Contains("NASS-CA"))
                            sourceID = "88";
                        else if (inputSource.Contains("USDA-CA"))
                            sourceID = "89";
                        else sourceID = "88";
                        break;
                    case "NASS-FL":
                        sourceID = "96";
                        break;
                    case "NASS-NJ":
                        sourceID = "93";
                        break;
                    case "AMS":
                        sourceID = "82";
                        break;
                    case "BLS":
                        sourceID = "8";
                        break;
                    default:
                        sourceID = "";
                        break;

                }
            }
            return sourceID;

        }


        /// <summary>
        /// load getPrivacy id with unit name
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private string getPrivacy(string privacy)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!            
            try
            {
                // need to take care if not found !!!!!!!!!!!!!!!!!!
                string sql = "select [ERSDataPrivacy_ID] from " + schemaName + "[ERSDataPrivacy_LU] where [ERSDataPrivacy_Desc] like '" + privacy + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSDataPrivacy_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// get geo id with given country, region, state, county
        /// </summary>
        /// <param name="country"></param>
        /// <param name="region"></param>
        /// <param name="state"></param>
        /// <param name="county"></param>
        /// <returns></returns>
        private string getGeoValueID(string geoTypeID, string geoValue)
        {
            try
            {
                seperatedGEOIDs = new StringBuilder();
                seperatedGEOIDs.Clear();
                string sql = "";

                List<string> geoValueList = geoValue.Split(';').Select(p => p.Trim()).ToList();
                List<string> geoTypeList = geoTypeID.Split(';').Select(p => p.Trim()).ToList();

                if (geoValueList.Count != geoTypeList.Count)
                {
                    throw new Exception("Please provide same number of Geogrphy Types and Geography Values");
                }

                for (int counter = 0; counter < geoValueList.Count; counter++)
                {
                    sql = "SELECT [ERSGeographyDimension_ID] FROM " + schemaName + "[ERSGeographyDimension_LU] where ERSGeographyDimension_ERSGeographyType_ID = " + geoTypeList[counter] + " and (ERSGeographyDimension_Country like '" + geoValueList[counter] + "' OR ERSGeographyDimension_State like '" + geoValueList[counter] + "' OR ERSGeographyDimension_County like '" + geoValueList[counter] + "' OR ERSGeographyDimension_Region like '" + geoValueList[counter] + "' OR ERSGeographyDimension_City like '" + geoValueList[counter] + "')";

                    System.Data.DataTable dt = GetData(sql);
                    DataRow[] dr = dt.Select();
                    if (dr.Length==1)
                    seperatedGEOIDs.Append(dr[0]["ERSGeographyDimension_ID"].ToString() + ", ");
                    else 
                        throw new Exception("Please provide correct Geogrphy Types and Geography Values");
                }

                return seperatedGEOIDs.ToString().Remove(seperatedGEOIDs.ToString().Length - 2);

            }
            catch (Exception ex)      // if no existing geo id then insert one
            {
                throw new Exception(ex.Message);
            }
        }

        //get geo type
        private string getGeoTypeID(string geoType)
        {
            seperatedGEOTypeIDs = new StringBuilder();
            string sqlgeoType = "";
            try
            {
                if (geoType.Contains(";"))
                {
                    List<string> geoTypeList = geoType.Split(';').Select(p => p.Trim()).ToList();
                    foreach (string item in geoTypeList)
                    {
                        sqlgeoType = "select [ERSGeographyType_ID] from " + schemaName + "[ERSGeographyType_LU] where [ERSGeographyType_Desc] = '" + item + "'";
                        System.Data.DataTable dtType = GetData(sqlgeoType);
                        DataRow[] drType = dtType.Select();
                        seperatedGEOTypeIDs.Append(drType[0]["ERSGeographyType_ID"].ToString() + "; ");
                    }
                    return seperatedGEOTypeIDs.ToString().Remove(seperatedGEOTypeIDs.ToString().Length - 2);
                }
                else
                {
                    sqlgeoType = "select [ERSGeographyType_ID] from " + schemaName + "[ERSGeographyType_LU] where [ERSGeographyType_Desc] = '" + geoType + "'";

                    System.Data.DataTable dtType = GetData(sqlgeoType);
                    DataRow[] drType = dtType.Select();
                    return drType[0]["ERSGeographyType_ID"].ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        /// <summary>
        /// load TimeDimensionTypeID id with unit name
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private string getTimeDimensionTypeID(string timeDimensionType)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!            
            try
            {
                //if (timeDimensionType.Equals(("Month"), StringComparison.OrdinalIgnoreCase))
                //{
                //    timeDimensionType = "ERSMonth";
                //}
                if (timeDimensionType.Equals(("Year"), StringComparison.OrdinalIgnoreCase))
                {
                    timeDimensionType = "CalendarYear";
                }
                //else if (timeDimensionType.Equals(("Marketing Year"), StringComparison.OrdinalIgnoreCase))
                //{
                //    timeDimensionType = "ERSMarketingYear";
                //}

                timeDimensionType = "ERS" + timeDimensionType.Replace(" ", "");

                // need to take care if not found !!!!!!!!!!!!!!!!!!
                string sql = "select [ERSTimeDimensionType_ID] from " + schemaName + "[ERSTimeDimensionType_LU] where [ERSTimeDimensionType_Desc] = '" + timeDimensionType + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSTimeDimensionType_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// load ConversionFactorID id with unit name
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private string getConversionFactorID(string getconversionFactor)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!            
            try
            {
                // need to take care if not found !!!!!!!!!!!!!!!!!!
                string sql = "select [ERSConversionFactorID] from " + schemaName + "[ERSConversionFactors] where [ERSConversionFactor_CF] = '" + getconversionFactor + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSConversionFactorID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// load unit id with unit name
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private string getUnitID(string unit)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!       
            seperatedUnitIDs = new StringBuilder();
            try
            {
                if (unit.Contains(";"))
                {
                    List<string> unitList = unit.Split(';').Select(p => p.Trim()).ToList();
                    foreach (string item in unitList)
                    {
                        string sql = "select ERSUnit_ID from " + schemaName + "ERSUnit_LU where ERSUnit_Desc = '" + item + "'";
                        System.Data.DataTable dt = GetData(sql);
                        DataRow[] dr = dt.Select();
                        seperatedUnitIDs.Append(dr[0]["ERSUnit_ID"].ToString() + ", ");
                    }
                    return seperatedUnitIDs.ToString().Remove(seperatedUnitIDs.ToString().Length - 2);
                }
                else
                {
                    // need to take care if not found !!!!!!!!!!!!!!!!!!
                    string sql = "select ERSUnit_ID from " + schemaName + "ERSUnit_LU where ERSUnit_Desc = '" + unit + "'";
                    System.Data.DataTable dt = GetData(sql);
                    DataRow[] dr = dt.Select();
                    return dr[0]["ERSUnit_ID"].ToString();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string getGroupId(string commodityDataSeriesID)
        {
            string groupID = "0";
            string[] inputsArray = Array.ConvertAll(commodityDataSeriesID.Split(','), p => p.Trim());
            for (int counter = 0; counter < inputsArray.Length; counter++)
            {
                if (!(inputsArray[counter].Contains("C") || inputsArray[0].Contains("c") || inputsArray[counter].Contains("N") || inputsArray[0].Contains("n")))
                {
                    string sql = "SELECT [ERSCommodity_ERSGroup_ID] FROM " + schemaName + "[ERSCommodityDataSeries] WHERE ERSCommodity_ID = " + inputsArray[counter];
                    System.Data.DataTable dt = GetData(sql);
                    DataRow[] dr = dt.Select();
                    if (dr.Length > 0)
                    {
                        groupID = dr[0]["ERSCommodity_ERSGroup_ID"].ToString();
                        break;
                    }
                }
                else
                {
                    string sql = "SELECT [ERSBusinessLogic_GroupID] FROM " + schemaName + "[ERSBusinessLogic] where ERSBusinessLogic_OutputDestination like '" + inputsArray[counter] + "'";
                    System.Data.DataTable dt = GetData(sql);
                    DataRow[] dr = dt.Select();
                    if (dr.Length > 0)
                    {
                        groupID = dr[0]["ERSBusinessLogic_GroupID"].ToString();
                        break;
                    }
                }
            }
            return groupID;
        }


        private string getInputDataSeriesID(string commoditySeriesID)
        {
            string[] inputsArray = Array.ConvertAll(commoditySeriesID.Split(';'), p => p.Trim()).Distinct().ToArray();
            StringBuilder commoditySeriesIDBuilder = new StringBuilder();

            for (int counter = 0; counter < inputsArray.Length; counter++)
            {
                if (inputsArray[counter].Contains("("))
                {
                    //commoditySeriesID = commoditySeriesID.Replace(inputsArray[counter], );
                    commoditySeriesIDBuilder.Append(Regex.Match(inputsArray[counter], @"\(([^)]*)\)").Groups[1].Value + ", ");
                }
                else
                {
                    commoditySeriesIDBuilder.Append(inputsArray[counter] + ", ");
                }
            }

            //fix the format again
            commoditySeriesID = commoditySeriesIDBuilder.Replace(";", " ,").ToString();


            return commoditySeriesID.Remove(commoditySeriesID.Length - 2);
        }
        // get inputs
        private string getInputs(string[] inputsArray, int thousandNumber)
        {
            string statType = "";
            string inputs = "";
            int newThousandNumber = 0;
            StringBuilder commoditySeriesIDInputs = new StringBuilder();

            for (int counter = 0; counter < inputsArray.Length; counter++)
            {
                if (!(inputsArray[counter].Contains("C") || inputsArray[0].Contains("c") || inputsArray[counter].Contains("N") || inputsArray[0].Contains("n")))
                {
                    try
                    {
                        statType = getStatType(inputsArray[counter]);
                        //statType = statType + "(" + inputsArray[counter] + ")";
                        inputs = statType + "(" + inputsArray[counter] + ")";
                        commoditySeriesIDInputs.Append(inputs + "; ");
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Please make sure the data series ID " + inputsArray[counter] + " is correct.");
                    }
                }
                else if (inputsArray[counter].Contains("C") || inputsArray[0].Contains("c"))
                {
                    newThousandNumber = (thousandNumber * 1000) + Convert.ToInt32(Regex.Match(inputsArray[counter], @"\d+").Value);
                    inputs = "CV" + newThousandNumber;
                    commoditySeriesIDInputs.Append(inputs + "; ");
                }
                else if (inputsArray[counter].Contains("N") || inputsArray[0].Contains("n"))
                {
                    try
                    {
                        newThousandNumber = (thousandNumber * 1000) + Convert.ToInt32(Regex.Match(inputsArray[counter], @"\d+").Value);

                        //Get commodity ID with stat type
                        string sqlcommodity_Stat = "SELECT ([ERSStatisticType_Attribute] + '(' + CONVERT(varchar(100),[ERSCommodity_ID]) + ')') AS Commodity_Stat FROM " + schemaName + "[ERSCommodityDataSeries] LEFT JOIN [CoSD].[ERSStatisticType_LU] ON [ERSStatisticType_ID] = [ERSCommodity_ERSStatisticType_ID] where ERSCommodity_ID IN (SELECT [ERSCommodity_ID] FROM  " + schemaName + "[ERSCommodityDataSeries] where ERSCommodity_SourceSeriesID like '%(" + "N" + newThousandNumber + ")%')";
                        System.Data.DataTable dtcommodity_Stat = GetData(sqlcommodity_Stat);
                        DataRow[] drcommodity_Stat = dtcommodity_Stat.Select();

                        inputs = drcommodity_Stat[0]["Commodity_Stat"].ToString();
                        commoditySeriesIDInputs.Append(inputs + "; ");
                    }
                    catch (Exception e)
                    {
                        throw new Exception("New data series " + inputsArray[counter] + " not found. Please make sure the new data series is available before adding this rule.");
                    }
                }
            }

            return commoditySeriesIDInputs.ToString().Remove(commoditySeriesIDInputs.ToString().Length - 2);
        }
        /// <summary>
        /// load stat type
        /// </summary>
        /// <param name="statType"></param>
        /// <returns></returns>
        private string getStatType(string commodityID)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!
            try
            {
                string sql = "select [ERSStatisticType_Attribute] from " + schemaName + "[ERSCommodityDataSeries] LEFT JOIN [CoSD].[ERSStatisticType_LU] ON [ERSStatisticType_ID] = [ERSCommodity_ERSStatisticType_ID] where ERSCommodity_ID = '" + commodityID + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSStatisticType_Attribute"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
        //get formula
        private string getFormula(string formula, string[] inputsArray, int thousandNumber, string[] keywords)
        {
            string newFormula = formula;
            int newThousandNumber = 0;
            const char LeftParenthesis = '(';
            const char RightParenthesis = ')';
            const char LeftSquareParenthesis = '[';
            const char RightSquareParenthesis = ']';
            int LeftParenthesisCount = 0;
            int RightParenthesisCount = 0;
            int LeftSquareParenthesisCount = 0;
            int RightSquareParenthesisCount = 0;

            try
            {

                //Checking valid keywords in formula
                //char[] onlyLetters = formula.Where(Char.IsLetter).ToArray();
                List<string> keywordsInFormulaList = new List<string>();
                HashSet<string> keywordsInFormulaSet;
                Regex rgx = new Regex("[^a-zA-Z]");
                keywordsInFormulaList = rgx.Replace(formula, " ").ToUpper().Split(' ').ToList();
                keywordsInFormulaSet = new HashSet<string>(keywordsInFormulaList);
                foreach (string keywordFormula in keywordsInFormulaSet)
                {
                    if (!keywordFormula.Trim().Equals("") && !keywords.Contains(keywordFormula.Trim()))
                    {
                        throw new Exception("Please enter correct formula. The keyword '" + keywordFormula.Trim() + "' is not allowed. Please contact technical team if you need to add this to CoSD allowed keyword list");
                    }
                }

                //Checing validity of brackets
                for (int Index = 0; Index < formula.Length; Index++)
                {
                    switch (formula[Index])
                    {
                        case LeftParenthesis:
                            LeftParenthesisCount++;
                            continue;
                        case RightParenthesis:
                            RightParenthesisCount++;
                            continue;
                        case LeftSquareParenthesis:
                            LeftSquareParenthesisCount++;
                            continue;
                        case RightSquareParenthesis:
                            RightSquareParenthesisCount++;
                            continue;
                        default:
                            continue;
                    }
                }

                if ((LeftParenthesisCount != RightParenthesisCount) || (LeftSquareParenthesisCount != RightSquareParenthesisCount))
                {
                    throw new Exception("Please enter correct formula. The use of Brackets is not proper in the formula");
                    
                }

                for (int counter = 0; counter < inputsArray.Length; counter++)
                {
                    if (newFormula.Contains(inputsArray[counter]))
                    {
                        if ((inputsArray[counter].Contains("C") || inputsArray[0].Contains("c")))
                        {
                            newThousandNumber = (thousandNumber * 1000) + Convert.ToInt32(Regex.Match(inputsArray[counter], @"\d+").Value);
                            newFormula = newFormula.Replace(inputsArray[counter], "CV" + newThousandNumber);
                        }
                        else if (inputsArray[counter].Contains("N") || inputsArray[0].Contains("n"))
                        {
                            try
                            {
                                newThousandNumber = (thousandNumber * 1000) + Convert.ToInt32(Regex.Match(inputsArray[counter], @"\d+").Value);

                                //Get commodity ID with stat type
                                string sqlcommodity_Stat = "SELECT ([ERSStatisticType_Attribute] + '(' + CONVERT(varchar(100),[ERSCommodity_ID]) + ')') AS Commodity_Stat FROM " + schemaName + "[ERSCommodityDataSeries] LEFT JOIN [CoSD].[ERSStatisticType_LU] ON [ERSStatisticType_ID] = [ERSCommodity_ERSStatisticType_ID] where ERSCommodity_ID IN (SELECT [ERSCommodity_ID] FROM  " + schemaName + "[ERSCommodityDataSeries] where ERSCommodity_SourceSeriesID like '%(" + "N" + newThousandNumber + ")%')";
                                System.Data.DataTable dtcommodity_Stat = GetData(sqlcommodity_Stat);
                                DataRow[] drcommodity_Stat = dtcommodity_Stat.Select();

                                newFormula = newFormula.Replace(inputsArray[counter], drcommodity_Stat[0]["Commodity_Stat"].ToString());
                            }
                            catch (Exception)
                            {
                                throw new Exception("New data series " + inputsArray[counter] + " not found. Please make sure the new data series is available before adding this rule.");
                            }

                        }
                        else
                        {
                            try
                            {
                                //Get commodity ID with stat type
                                string sqlcommodity_Stat = "SELECT ([ERSStatisticType_Attribute] + '(' + CONVERT(varchar(100),[ERSCommodity_ID]) + ')') AS Commodity_Stat FROM " + schemaName + "[ERSCommodityDataSeries] LEFT JOIN [CoSD].[ERSStatisticType_LU] ON [ERSStatisticType_ID] = [ERSCommodity_ERSStatisticType_ID] where ERSCommodity_ID IN (SELECT [ERSCommodity_ID] FROM  " + schemaName + "[ERSCommodityDataSeries] where [ERSCommodity_ID] = " + inputsArray[counter] + ")";
                                System.Data.DataTable dtcommodity_Stat = GetData(sqlcommodity_Stat);
                                DataRow[] drcommodity_Stat = dtcommodity_Stat.Select();

                                newFormula = newFormula.Replace(inputsArray[counter], drcommodity_Stat[0]["Commodity_Stat"].ToString());
                            }
                            catch (Exception)
                            {
                                throw new Exception("Data series " + inputsArray[counter] + " not found. Please use an existing data series ID in formula");
                            }

                        }
                    }
                    else
                    {
                        throw new Exception("The input " + inputsArray[counter] + " is not used in formula. Please use a correct formula for this rule.");
                    }
                }

                //replace brackets
                newFormula = newFormula.Replace("[", "");
                newFormula = newFormula.Replace("]", "");
                return newFormula;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //get output destination
        private string getOutputDestination(string ruleID, string outputDestination, int thousandNumber, int numberToAdd)
        {
            try
            {
                thousandNumber = (thousandNumber * 1000) + numberToAdd;
                outputDestination = outputDestination.ToUpper();
                //int newThousandNumber = 0;

                if (Regex.Match(outputDestination, @"[CVN]").Value == null)
                {
                    throw new Exception("Please enter output destination in correct format. Valid format is 'CV' or 'N' followed by number like 'CV1'.");
                }
                else if (outputDestination.Contains("CV") && !outputDestination.Contains("N"))
                {
                    outputDestination = "CV" + thousandNumber;
                    return outputDestination;
                }
                else if (outputDestination.Contains("N"))
                {
                    try
                    {
                        //newThousandNumber = (thousandNumber * 1000) + Convert.ToInt32(Regex.Match(outputDestination, @"\d+").Value);

                        //Get commodity ID with stat type
                        string sqlcommodity_Stat = "SELECT [ERSCommodity_ID] FROM " + schemaName + "[ERSCommodityDataSeries] where ERSCommodity_SourceSeriesID like '%(" + "N" + thousandNumber + ")%'";
                        System.Data.DataTable dtcommodity_Stat = GetData(sqlcommodity_Stat);
                        DataRow[] drcommodity_Stat = dtcommodity_Stat.Select();

                        if (drcommodity_Stat.Length == 0)
                        {
                            throw new Exception("New data series " + outputDestination + " not found. Please make sure the new data series is available before adding this rule.");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }



                    return "CV(N" + thousandNumber + ")";
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private int getThoudandNumber(string businessRuleID, string[] inputsArray)
        {
            string sourceCVNumber = "";
            int thousandNumber = 0;
            if (!businessRuleID.Equals(""))
            {
                string sqlCVNumber = "SELECT [ERSBusinessLogic_OutputDestination] FROM " + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_ID] =  " + businessRuleID;
                System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                DataRow[] drCVNumber = dtCVNumber.Select();
                sourceCVNumber = drCVNumber[0]["ERSBusinessLogic_OutputDestination"].ToString();
                //  'CV(N154000) 2011: 4'
                //  'CV16004 2001'
                //checking the number in thousand
                if (sourceCVNumber.Contains("N"))
                {
                    sourceCVNumber = Regex.Match(sourceCVNumber, @"\(([^)]*)\)").Groups[1].Value.Trim();
                    if (sourceCVNumber.Length == 5)
                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(1, 1));
                    else if (sourceCVNumber.Length == 6)
                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(1, 2));
                    else if (sourceCVNumber.Length == 7)
                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(1, 3));
                }
                else
                {
                    if (sourceCVNumber.Length == 6)
                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(2, 1));
                    else if (sourceCVNumber.Length == 7)
                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(2, 2));
                    else if (sourceCVNumber.Length == 8)
                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(2, 3));
                }

            }

            return thousandNumber;
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Parent = null;

        }




        private void button_reset_Click(object sender, EventArgs e)
        {
            this.dataGridView.DataSource = null;
            this.dataGridView.Rows.Clear();
            ClearControls();
            disablebuttons();
            resetAllFields();

        }


        private void button_prev_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor currentCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;

                currentPageIndex--;
                if (currentPageIndex == 0)
                {
                    button_prev.Enabled = false;

                }
                else
                {
                    button_prev.Enabled = true;
                }
                button_next.Enabled = true;

                BindGrid();
                this.Cursor = currentCursor;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                return;
            }

        }

        private void button_next_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor currentCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;

                currentPageIndex++;
                if (currentPageIndex == (int)Math.Ceiling(Convert.ToDecimal(DT.Rows.Count) / pageSize) - 1)
                {
                    button_next.Enabled = false;

                }

                else if (Convert.ToDecimal(DT.Rows.Count) <= pageSize)
                {

                    button_next.Enabled = false;

                }

                else
                {
                    button_next.Enabled = true;

                }

                button_prev.Enabled = true;
                BindGrid();
                this.Cursor = currentCursor;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }

        }

        private void ClearControls()
        {
            try
            {
                comboBox_description1.SelectedItem = null;
                //Listbox_Commodity.SelectedItem = null;
                comboBox_group1.SelectedItem = null;
                comboBox_BusinessType1.SelectedItem = null;
                comboBox_SourceType1.SelectedItem = null;
                comboBox_description1.SelectedItem = null;
                comboBox_Commodity1.SelectedItem = null;
                rowcount_label.Text = "0";
                label_pageNum.Text = "Page";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }


        }

        private void disablebuttons()
        {

            longwait_label.Visible = false;
            button_DeleteRules.Enabled = false;
            button_DeleteRules.UseVisualStyleBackColor = false;
            button_executeBL.Enabled = false;
            button_executeBL.UseVisualStyleBackColor = false;
            //button_executeBL.BackColor = Color.Grayk
            button_update.Enabled = false;
            button_update.UseVisualStyleBackColor = false;
            //button_update.BackColor = Color.Gray;
            label1.ForeColor = System.Drawing.Color.Red;
            rowcount_label.ForeColor = System.Drawing.Color.Red;
            button_prev.Enabled = false;
            button_next.Enabled = false;
            button_prev.UseVisualStyleBackColor = false;
            button_next.UseVisualStyleBackColor = false;
            button_executeBL.Text = "Re-execute Selected Business Rules";
            button_executeBL.Refresh();
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //button_update.BackColor = Color.RoyalBlue;
            button_update.Enabled = true;
            button_update.UseVisualStyleBackColor = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            BusinessRulesInsertForm BlinsertForm = new BusinessRulesInsertForm();
            BlinsertForm.Show();
        }

        private void button_DeleteRules_Click(object sender, EventArgs e)
        {
            try
            {

                businessRuleID = "-1";
                string resequencingStatus = "";

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (dataGridView.SelectedRows.Count == 0)
                    {
                        MessageBox.Show("Please select a row to delete.");
                        return;
                    }

                    if (MessageBox.Show("Do you really want to Delete these values?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (MessageBox.Show("Data once deleted cannot be retrieved back. Do you want to continue?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                        {
                            for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
                            {
                                //Reorder sequence IDs
                                resequencingStatus = resqeuencing(Convert.ToString(dataGridView.SelectedRows[i].Cells["Rule ID"].Value));

                                if (!resequencingStatus.Equals("Updated"))
                                {
                                    throw new Exception("Error Upating Sequence. Please contact technical team.");
                                }

                                businessRuleID = businessRuleID + "," + Convert.ToString(dataGridView.SelectedRows[i].Cells["Rule ID"].Value);

                            }
                            //retrieve data before deleting
                            deletedData = new StringBuilder();
                            string deletedValue = "Select * From " + schemaName + "[ERSBusinessLogic] WHERE [ERSBusinessLogic_ID] IN  (" + businessRuleID + " )";
                            DataTable dt = GetData(deletedValue);
                            DataRow[] dr = dt.Select();
                            foreach (DataRow row in dt.Rows)
                            {
                                //sb.AppendLine(string.Join(",", row.ItemArray));
                                foreach (DataColumn column in dt.Columns)
                                {
                                    deletedData.AppendLine(" " + column.ColumnName + ": " + row[column]);
                                }
                            }

                            //delete the data
                            con.Open();
                            string sqlCV = "Delete From " + schemaName + "[ERSConstructedVariablesOutcomes] WHERE [ERSConstructedVariable_BusinessLogicID] IN  (" + businessRuleID + " )";
                            string sql = "Delete From " + schemaName + "[ERSBusinessLogic] WHERE [ERSBusinessLogic_ID] IN  (" + businessRuleID + " )";
                            SqlDataAdapter SDA_CV = new SqlDataAdapter("Delete From " + schemaName + "[ERSConstructedVariablesOutcomes] WHERE [ERSConstructedVariable_BusinessLogicID] IN  (" + businessRuleID + " )", con);
                            SqlDataAdapter SDA = new SqlDataAdapter("Delete From " + schemaName + "[ERSBusinessLogic] WHERE [ERSBusinessLogic_ID] IN  (" + businessRuleID + " )", con);
                            SDA_CV.SelectCommand.ExecuteNonQuery();
                            SDA.SelectCommand.ExecuteNonQuery();

                            //save deleted data.
                            saveAction("Business Rule Deleted : " + deletedData);


                            MessageBox.Show(dataGridView.SelectedRows.Count + " Selected Row(s) Deleted Successfully.");

                        }
                    }

                    else
                    {
                        return;
                    }

                }
                else
                {
                    MessageBox.Show("No Business Rules Found");


                }

            }

            catch (Exception ex)
            {
                if(ex.Message.Contains("Error Upating Sequence"))
                    MessageBox.Show(ex.Message);
                else
                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                //saveAction(ex.Message);
            }

            finally
            {
                con.Close();
                button_retreive_Click(sender, e);
            }
        }

        //Rordering the sequence
        private string resqeuencing(string ruleID)
        {
            Decimal sqeuenceIDPrevID;

            try
            {
                //Previous
                string sqlPrevID = "SELECT MAX([ERSBusinessLogic_SequenceID]) AS ERSBusinessLogic_SequenceID FROM " + schemaName + "[ERSBusinessLogic]  where ERSBusinessLogic_SequenceID < (SELECT [ERSBusinessLogic_SequenceID] FROM  " + schemaName + "[ERSBusinessLogic]  where ERSBusinessLogic_ID = " + ruleID + ")";
                System.Data.DataTable dtsqlPrevID = GetData(sqlPrevID);

                if (dtsqlPrevID==null)
                {
                    DataRow[] drsqlPrevID = dtsqlPrevID.Select();
                    sqeuenceIDPrevID = Convert.ToDecimal(drsqlPrevID[0]["ERSBusinessLogic_SequenceID"]);

                   

                    string sqlUpdateSeqID = "UPDATE " + schemaName + "[ERSBusinessLogic] SET [ERSBusinessLogic_SequenceID] = CASE WHEN [ERSBusinessLogic_SequenceID] > " + sqeuenceIDPrevID + " THEN [ERSBusinessLogic_SequenceID] -1 ELSE [ERSBusinessLogic_SequenceID] END";
                    System.Data.DataTable dtsqlUpdateSeq = GetData(sqlUpdateSeqID);
                }
                return "Updated";
            }
            catch (Exception ex)
            {
                return "Error Upating Sequence";
            }

        }
        private void button_prev_EnabledChanged(object sender, EventArgs e)
        {
            if (((Button)sender).Enabled)
            {
                button_prev.Text = "Prev";
                //button_prev.ForeColor = Color.Black;
                //button_prev.BackColor = Color.Linen;
            }
            else
            {
                button_prev.Text = "Prev";
                //button_prev.ForeColor = Color.Tomato;
                //button_prev.BackColor = Color.Gray;

            }
        }

        private void button_next_EnabledChanged(object sender, EventArgs e)
        {
            if (((Button)sender).Enabled)
            {
                button_next.Text = "Next";
                //button_next.ForeColor = Color.Black;
                //button_next.BackColor = Color.Linen;
            }
            else
            {
                button_next.Text = "Next";
                //button_next.ForeColor = Color.Tomato;
                //button_next.BackColor = Color.Gray;
            }
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

        private void fillAllComboBoxes(string sourceType, string businessType, string group, string description)
        {
            try
            {
                //getting all values
                string sourceTypeForAll = "";
                string businessTypeForAll = "";
                string groupForAll = "";
                string descriptionForAll = "";

                ////Set available users group to groupIds
                //groupIds = new List<string>();
                //groupIds = getGroupIds();

                if (groupIds.Count != 0)
                {
                    //prepare where consdition:
                    if (!sourceType.Equals(""))
                        sourceTypeForAll = " AND [ERSBusinessLogic_InputSources] like '" + sourceType + "' ";
                    if (!businessType.Equals(""))
                        businessTypeForAll = " AND [ERSBusinessLogic_Type] like '" + businessType + "'";
                    if(!group.Equals(""))
                        groupForAll = " AND [ERSBusinessLogic_GroupID] IN (SELECT [ERSGroup_ID] FROM " + schemaName + "[ERSGroup_LU] WHERE ERSGroup_Desc like '" + group + "')";
                    if(!description.Equals(""))
                        descriptionForAll = " AND [ERSBusinessLogic_LongDesc] like '" + description + "'";

                    //Filling the comboboxes
                    if (businessType.Equals(""))
                    filllogictypeCombobox(sourceTypeForAll, businessTypeForAll, groupForAll, descriptionForAll);
                    if (group.Equals(""))
                    fillGroupCombobox(sourceTypeForAll, businessTypeForAll, groupForAll, descriptionForAll);
                    if (sourceType.Equals(""))
                    fillSourceTypeCombobox(sourceTypeForAll, businessTypeForAll, groupForAll, descriptionForAll);
                    if (description.Equals(""))
                    fillDescriptionCombobox(sourceTypeForAll, businessTypeForAll, groupForAll, descriptionForAll);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }
        }

        private string saveFailedBusinessRules(DataTable failedDataTable)
        {
            //Save text file in case of business rules with exceptions.
            try
            {
                int i = 0;
                int count = 0;
                StreamWriter sw = null;

                if (DeSerializeSettingsPath<String>("CoSDSettings") != null)
                    cosdFolderPath = DeSerializeSettingsPath<String>("CoSDSettings");

                if (!cosdFolderPath.EndsWith("\\"))
                {
                    cosdFolderPath = cosdFolderPath + "\\";
                }

                // first check if target directory exist
                // create one if not exist
                if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Business Rules\\Execution Results")) // name of the error file and location
                {
                    Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Business Rules\\Execution Results");
                }

                string fileName = cosdFolderPath + "CoSD Tool\\Business Rules\\Execution Results\\Failed Business Rules_" + Date + ".txt";


                sw = new StreamWriter(fileName, false);

                //Header in test file.
                sw.WriteLine("Failed Business Rules:");
                sw.WriteLine();

                for (i = 0; i < failedDataTable.Columns.Count - 1; i++)
                {

                    sw.Write(failedDataTable.Columns[i].ColumnName + " ");

                }
                sw.Write(failedDataTable.Columns[i].ColumnName);
                sw.WriteLine();

                foreach (DataRow row in failedDataTable.Rows)
                {
                    //to skip last reduntant row.
                   
                    if (count != failedDataTable.Rows.Count)
                    {

                        object[] array = row.ItemArray;

                        for (i = 0; i < array.Length - 1; i++)
                        {
                            sw.Write(array[i].ToString() + "        ");
                        }
                        sw.Write(array[i].ToString());
                        sw.WriteLine();

                    }

                    count++;
                }
                //Extra information
                sw.WriteLine();
                sw.WriteLine("TroubleShoot:");
                sw.WriteLine();
                sw.WriteLine("Please check the list below for possible reasons that may have caused business rules to fail");
                sw.WriteLine("1. Check business formula for any mistakes.");
                sw.WriteLine("2. Check whether a valid time/geo dimension or unit value is entered for the business rule.");
                sw.WriteLine("3. Check whether the data is available for the data series used.");
                sw.WriteLine("4. Make sure all the New data series (Ns) are already in the DB.");
                sw.WriteLine();
                sw.WriteLine("You can re-execute business rules after making corrections. Please contact technical team in case issue persists.");
                sw.Close();
                failedFileName = fileName;
                return fileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not create text file for failed business rules. Please contact technical team.");
                return "";
            }

        }

        /// <summary>
        /// This method Deserializes an xml file into an String object.
        /// </summary>
        public T DeSerializeSettingsPath<T>(string fileName)
        {
            // first check if target directory exist
            // create one if not exist
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
        private void wait_label_Click(object sender, EventArgs e)
        {

        }

        private void longwait_label_Click(object sender, EventArgs e)
        {

        }

        private void BusinessRules_Load_1(object sender, EventArgs e)
        {

        }

        private void BusinessRules_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Don't throw an exception when we're done.
            e.ThrowException = false;
            MessageBox.Show("Please enter a valid value.");
            // If this is true, then the user is trapped in this cell.
            e.Cancel = false;
        }

        private void button_CV_Click(object sender, EventArgs e)
        {
            if (constructedVariables.IsDisposed == true)
            {
                ConstructedVariables CV = new ConstructedVariables();
                CV.Show();
            }
            else
            {
                constructedVariables.Show();
            }
        }

        private void comboBox_SourceType_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_SourceType1.Text, comboBox_BusinessType1.Text, comboBox_group1.Text, comboBox_description1.Text);
        }

        private void comboBox_BusinessType_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_SourceType1.Text, comboBox_BusinessType1.Text, comboBox_group1.Text, comboBox_description1.Text);
        }

        private void comboBox_group_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_SourceType1.Text, comboBox_BusinessType1.Text, comboBox_group1.Text, comboBox_description1.Text);
        }

        private void comboBox_description_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_SourceType1.Text, comboBox_BusinessType1.Text, comboBox_group1.Text, comboBox_description1.Text);
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button_TS_Click(object sender, EventArgs e)
        {
            BusinessRulesTree ViewTreeForm = new BusinessRulesTree();
            ViewTreeForm.Show();
        }

        private void resetAllFields()
        {

            comboBox1_stattype.Text = "";
            comboBox_BusinessType1.Text = "";
            comboBox_Commodity1.Text = "";
            comboBox_SourceType1.Text = "";
            comboBox_group1.Text = "";
            comboBox_description1.Text = "";
            textBox_DSid.Text = "";
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_Commodity1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox_SourceType1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
      

        private void button_retreive_Click_1(object sender, EventArgs e)
        {
            // verify input before doing anything
            if (comboBox_SourceType1.Text.Equals("") && comboBox_Commodity1.Text.Equals("") && comboBox_BusinessType1.Text.Equals("") && comboBox_group1.Text.Equals("") && comboBox_description1.Text.Equals("")&& comboBox1_stattype.Text.Equals("") &&textBox_DSid.Text.Equals(""))
            {
                MessageBox.Show("Please select at least one value");
                resetAllFields();
                return;
            }

            if (!comboBox_SourceType1.Text.Equals("") && !comboBox_Commodity1.Text.Equals("") && !comboBox_BusinessType1.Text.Equals("") && !comboBox_group1.Text.Equals("") && !comboBox_description1.Text.Equals("") && !comboBox1_stattype.Text.Equals("") && !textBox_DSid.Text.Equals(""))
            {
                MessageBox.Show("Please select only one value");
                resetAllFields();
                return;
            }

            if (!comboBox_SourceType1.Text.Equals("") && !textBox_DSid.Text.Equals(""))
            {
                MessageBox.Show("Please select only one value");
                resetAllFields();
                return;
            }

            if (!comboBox_Commodity1.Text.Equals("") && !textBox_DSid.Text.Equals(""))
            {
                MessageBox.Show("Please select only one value");
                resetAllFields();
                return;
            }
            if (!comboBox_BusinessType1.Text.Equals("") && !textBox_DSid.Text.Equals(""))
            {
                MessageBox.Show("Please select only one value");
                resetAllFields();
                return;
            }
            if (!comboBox_group1.Text.Equals("") && !textBox_DSid.Text.Equals(""))
            {
                MessageBox.Show("Please select only one value");
                resetAllFields();
                return;
            }

            if (!comboBox_description1.Text.Equals("") && !textBox_DSid.Text.Equals(""))
            {
                MessageBox.Show("Please select only one value");
                resetAllFields();
                return;
            }

            if (!comboBox1_stattype.Text.Equals("") && !textBox_DSid.Text.Equals(""))
            {
                MessageBox.Show("Please select only one value");
                resetAllFields();
                return;
            }



            //start wait form
            waitForm.Show(this);
            waitForm.Refresh();

            //resetting page index
            currentPageIndex = 0;
            ds = new DataSet();
            sourceType = comboBox_SourceType1.Text;
            businessType = comboBox_BusinessType1.Text;
            groupType = comboBox_group1.Text;
            commodityDescription = comboBox_description1.Text;
            commodity = comboBox_Commodity1.Text;
            string stat = comboBox1_stattype.Text;


            //
            button_TS.BackColor = Color.LightGray;

            try
            {
                //Only source input
                if (sourceType != "")
                    sourceType = " and [ERSBusinessLogic_InputSources] like '" + sourceType + "'";
                if (businessType != "")
                    businessType = " and [ERSBusinessLogic_Type] like '" + businessType + "'";
                if (groupType != "")
                    groupType = " and [ERSBusinessLogic_GroupID] IN (Select [ERSGroup_ID] FROM " + schemaName + "[ERSGroup_LU] WHERE [ERSGroup_Desc] like '" + groupType + "' )";
                else
                    groupType = " and [ERSBusinessLogic_GroupID] IN (Select [ERSGroup_ID] FROM " + schemaName + "[ERSGroup_LU] WHERE [ERSGroup_Desc] IN (" + getGroupDesc() + ") )";
                if (commodityDescription != "")
                    commodityDescription = " and [ERSBusinessLogic_LongDesc] like '%" + commodityDescription + "%'";
                if (commodity != "")
                    commodity = " and [ERSBusinessLogic_OutputName] like '%" + commodity + "%'";
                if (stat != "")
                    stat = " and [ERSBusinessLogic_Formula] like '%" + stat + "%'";



                con.Open();
               
                if (textBox_DSid.Text.Equals(""))
                {
                    string query = @"SELECT ERSBusinessLogic_ID AS 'Rule ID' "
      + " ,[ERSBusinessLogic_Formula] 'Formula' "
      + " ,[ERSBusinessLogic_OutputName] AS 'Output Name' "
      + " ,[ERSBusinessLogic_OutputDestination] AS 'Output Destination' "
      + " ,REPLACE ( [ERSBusinessLogic_InputDataSeries] , ',' , ' ;' ) AS 'Commodity Series ID' "
      + " ,InputUnit.[ERSUnit_Desc] AS 'Input Unit' "
      + " ,[ERSConversionFactor_CF] AS 'Conversion Factor' "
      + " ,REPLACE ( [ERSBusinessLogic_MacroDesc] , 'Null' , '' ) AS 'Macro' "
      + " ,CONVERT(varchar(100), OutputUnit.[ERSUnit_Desc]) AS 'Output Unit' "
      + " ,TimeInput.[ERSTimeDimensionType_Desc] AS 'Input Time Dimension Type' "
      + " ,[ERSBusinessLogic_InputTimeDimensionValue] 'Input Time Value' "
      + " ,TimeOutput.[ERSTimeDimensionType_Desc] AS 'Output Time Dimension Type' "
      + " ,[ERSBusinessLogic_OutputTimeDimensionValue] AS 'Output Time Value' "
      + " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(InputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Input Geo Type' "
      + " ,REPLACE ( [ERSBusinessLogic_InputGeographyDimensionID] , ',' , ' ,' ) AS 'Input Geo Value' "
      + " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(OutputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Output Geo Type'"
      + " ,CONVERT(varchar(100), [ERSBusinessLogic_OutputGeographyDimensionID]) AS 'Output Geo Value' "
      + " ,[ERSDataPrivacy_Desc] AS 'Privacy' "
      + " ,[ERSBusinessLogic_Type] AS 'Type' "
      + " ,[ERSBusinessLogic_LongDesc] AS 'Long Description' "
      + " ,REPLACE ( [ERSBusinessLogic_InputSources] , ',' , ' ;' ) AS 'Input Sources' "
      + " FROM " + schemaName + "ERSBusinessLogic  "
      + " LEFT JOIN [CoSD].[ERSUnit_LU] AS InputUnit ON [ERSBusinessLogic_InputUnitID] like InputUnit.[ERSUnit_ID] "
      + " LEFT JOIN [CoSD].[ERSConversionFactors] ON [ERSBusinessLogic_ConvFactorID] = [ERSConversionFactorID] "
      + " LEFT JOIN [CoSD].[ERSUnit_LU] AS OutputUnit ON [ERSBusinessLogic_OutputUnitID] = OutputUnit.[ERSUnit_ID] "
      + " LEFT JOIN [CoSD].[ERSTimeDimensionType_LU] AS TimeInput ON [ERSBusinessLogic_InputTimeDimensionTypeID] = TimeInput.[ERSTimeDimensionType_ID] "
      + " LEFT JOIN [CoSD].[ERSTimeDimensionType_LU] AS TimeOutput ON [ERSBusinessLogic_OutputTimeDimensionTypeID] = TimeOutput.[ERSTimeDimensionType_ID] "
      + " LEFT JOIN [CoSD].[ERSDataPrivacy_LU] ON [ERSBusinessLogic_PrivacyID] = [ERSDataPrivacy_ID] "
      + " LEFT JOIN [CoSD].[ERSGeographyDimension_LU] AS InputGeo ON [ERSBusinessLogic_InputGeographyDimensionID] = CONVERT(varchar(100), InputGeo.ERSGeographyDimension_ID)"
      + " LEFT JOIN [CoSD].[ERSGeographyDimension_LU] AS OutputGeo ON [ERSBusinessLogic_OutputGeographyDimensionID] = OutputGeo.ERSGeographyDimension_ID"
      + " WHERE 1=1 " + sourceType + businessType + groupType + commodityDescription + commodity + stat +" ORDER BY ERSBusinessLogic_SequenceID  ";

                    Console.Write(query);
                    SDA = new SqlDataAdapter(query, con);

                    SDA.SelectCommand.ExecuteNonQuery();
                    DT = new DataTable();
                    SDA.Fill(ds);
                    ds.Tables[0].TableName = "Business Rules Export";
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        //Prepare Unit column
                        ds = prepareUnit(ds);

                        //Prepare rest of the fields.
                        ds = prepareOtherColumns(ds);

                    }


                    count1 = ds.Tables[0].Rows.Count;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dataGridView.DataSource = ds.Tables[0];
                        button_executeBL.Enabled = true;
                        button_executeBL.UseVisualStyleBackColor = true;
                        button_DeleteRules.UseVisualStyleBackColor = true;
                        button_DeleteRules.Enabled = true;
                        dataGridView.Columns[0].DefaultCellStyle.ForeColor = Color.Gray;
                        dataGridView.Columns[0].ReadOnly = true;
                        BindGrid();
                        label1.ForeColor = System.Drawing.Color.Black;
                        rowcount_label.ForeColor = System.Drawing.Color.Black;
                        label1.Text = "Number of formulas retrieved :";
                        rowcount_label.Text = count1.ToString();

                        if (Convert.ToDecimal(ds.Tables[0].Rows.Count) <= pageSize)
                        {

                            button_next.Enabled = false;
                            button_prev.Enabled = false;

                        }

                        else
                        {
                            button_next.Enabled = true;
                            button_prev.Enabled = false;

                        }
                        waitForm.Hide();

                    }
                    else
                    {
                        waitForm.Hide();
                        MessageBox.Show("No Business Rules Found");
                        dataGridView.DataSource = ds.Tables[0];

                        longwait_label.Visible = false;
                        button_DeleteRules.Enabled = false;
                        button_DeleteRules.UseVisualStyleBackColor = false;
                        //button_DeleteRules.BackColor = Color.Gray;
                        button_executeBL.Enabled = false;
                        //button_executeBL.BackColor = Color.Gray;
                        button_update.Enabled = false;
                        //button_update.BackColor = Color.Gray;
                        label1.ForeColor = System.Drawing.Color.Red;
                        rowcount_label.ForeColor = System.Drawing.Color.Red;
                        rowcount_label.Text = count1.ToString();
                        button_prev.Enabled = false;
                        button_next.Enabled = false;
                        label_pageNum.Text = "0 / 0";

                    }
                }

                else
                {
                    string dataSeriesId = textBox_DSid.Text;
                    string query = "SELECT ERSBusinessLogic_ID AS 'Rule ID' "
     + " ,[ERSBusinessLogic_Formula] 'Formula' "
     + " ,[ERSBusinessLogic_OutputName] AS 'Output Name' "
     + " ,[ERSBusinessLogic_OutputDestination] AS 'Output Destination' "
     + " ,REPLACE ( [ERSBusinessLogic_InputDataSeries] , ',' , ' ;' ) AS 'Commodity Series ID' "
     + " ,InputUnit.[ERSUnit_Desc] AS 'Input Unit' "
     + " ,[ERSConversionFactor_CF] AS 'Conversion Factor' "
     + " ,REPLACE ( [ERSBusinessLogic_MacroDesc] , 'Null' , '' ) AS 'Macro' "
     + " ,CONVERT(varchar(100), OutputUnit.[ERSUnit_Desc]) AS 'Output Unit' "
     + " ,TimeInput.[ERSTimeDimensionType_Desc] AS 'Input Time Dimension Type' "
     + " ,[ERSBusinessLogic_InputTimeDimensionValue] 'Input Time Value' "
     + " ,TimeOutput.[ERSTimeDimensionType_Desc] AS 'Output Time Dimension Type' "
     + " ,[ERSBusinessLogic_OutputTimeDimensionValue] AS 'Output Time Value' "
     + " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(InputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Input Geo Type' "
     + " ,REPLACE ( [ERSBusinessLogic_InputGeographyDimensionID] , ',' , ' ,' ) AS 'Input Geo Value' "
     + " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(OutputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Output Geo Type'"
     + " ,CONVERT(varchar(100), [ERSBusinessLogic_OutputGeographyDimensionID]) AS 'Output Geo Value' "
     + " ,[ERSDataPrivacy_Desc] AS 'Privacy' "
     + " ,[ERSBusinessLogic_Type] AS 'Type' "
     + " ,[ERSBusinessLogic_LongDesc] AS 'Long Description' "
     + " ,REPLACE ( [ERSBusinessLogic_InputSources] , ',' , ' ;' ) AS 'Input Sources' "
     + " FROM " + schemaName + "ERSBusinessLogic  "
     + " LEFT JOIN [CoSD].[ERSUnit_LU] AS InputUnit ON [ERSBusinessLogic_InputUnitID] like InputUnit.[ERSUnit_ID] "
     + " LEFT JOIN [CoSD].[ERSConversionFactors] ON [ERSBusinessLogic_ConvFactorID] = [ERSConversionFactorID] "
     + " LEFT JOIN [CoSD].[ERSUnit_LU] AS OutputUnit ON [ERSBusinessLogic_OutputUnitID] = OutputUnit.[ERSUnit_ID] "
     + " LEFT JOIN [CoSD].[ERSTimeDimensionType_LU] AS TimeInput ON [ERSBusinessLogic_InputTimeDimensionTypeID] = TimeInput.[ERSTimeDimensionType_ID] "
     + " LEFT JOIN [CoSD].[ERSTimeDimensionType_LU] AS TimeOutput ON [ERSBusinessLogic_OutputTimeDimensionTypeID] = TimeOutput.[ERSTimeDimensionType_ID] "
     + " LEFT JOIN [CoSD].[ERSDataPrivacy_LU] ON [ERSBusinessLogic_PrivacyID] = [ERSDataPrivacy_ID] "
     + " LEFT JOIN [CoSD].[ERSGeographyDimension_LU] AS InputGeo ON [ERSBusinessLogic_InputGeographyDimensionID] = CONVERT(varchar(100), InputGeo.ERSGeographyDimension_ID)"
     + " LEFT JOIN [CoSD].[ERSGeographyDimension_LU] AS OutputGeo ON [ERSBusinessLogic_OutputGeographyDimensionID] = OutputGeo.ERSGeographyDimension_ID"
    + " WHERE ',' + ERSBusinessLogic_InputDataSeries + ',' LIKE '%,'"+ "+" +"'" + dataSeriesId +  "'"+ ""+" + ',%'";
                    SDA = new SqlDataAdapter(@query, con);

                    Console.WriteLine(query);
                    SDA.SelectCommand.ExecuteNonQuery();
                    DT = new DataTable();
                    SDA.Fill(ds);
                    ds.Tables[0].TableName = "Business Rules Export";
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        //Prepare Unit column
                        ds = prepareUnit(ds);

                        //Prepare rest of the fields.
                        ds = prepareOtherColumns(ds);

                    }


                    count1 = ds.Tables[0].Rows.Count;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dataGridView.DataSource = ds.Tables[0];
                        button_executeBL.Enabled = true;
                        button_executeBL.UseVisualStyleBackColor = true;
                        button_DeleteRules.UseVisualStyleBackColor = true;
                        button_DeleteRules.Enabled = true;
                        dataGridView.Columns[0].DefaultCellStyle.ForeColor = Color.Gray;
                        dataGridView.Columns[0].ReadOnly = true;
                        BindGrid();
                        label1.ForeColor = System.Drawing.Color.Black;
                        rowcount_label.ForeColor = System.Drawing.Color.Black;
                        label1.Text = "Number of formulas retrieved :";
                        rowcount_label.Text = count1.ToString();

                        if (Convert.ToDecimal(ds.Tables[0].Rows.Count) <= pageSize)
                        {

                            button_next.Enabled = false;
                            button_prev.Enabled = false;

                        }

                        else
                        {
                            button_next.Enabled = true;
                            button_prev.Enabled = false;

                        }
                        waitForm.Hide();

                    }
                    else
                    {
                        waitForm.Hide();
                        MessageBox.Show("No Business Rules Found");
                        dataGridView.DataSource = ds.Tables[0];

                        longwait_label.Visible = false;
                        button_DeleteRules.Enabled = false;
                        button_DeleteRules.UseVisualStyleBackColor = false;
                        //button_DeleteRules.BackColor = Color.Gray;
                        button_executeBL.Enabled = false;
                        //button_executeBL.BackColor = Color.Gray;
                        button_update.Enabled = false;
                        //button_update.BackColor = Color.Gray;
                        label1.ForeColor = System.Drawing.Color.Red;
                        rowcount_label.ForeColor = System.Drawing.Color.Red;
                        rowcount_label.Text = count1.ToString();
                        button_prev.Enabled = false;
                        button_next.Enabled = false;
                        label_pageNum.Text = "0 / 0";

                    }

                }
            }
            catch (Exception ex)
            {
                waitForm.Hide();
                Console.WriteLine(ex.Message);
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                //if (ex.Message != "Connection Error")
                saveAction(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void comboBox_BusinessType1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ViewR view = new ViewR();
            if (view.IsDisposed == true)
            {
                ViewR viewR = new ViewR();
                viewR.Show();
            }
            else
            {
                view.Show();
            }
        }

        private void textBox_DSid_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

