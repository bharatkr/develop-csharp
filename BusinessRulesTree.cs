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
    public partial class BusinessRulesTree : Form
    {

        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString);
        private double waitTime { get; set; }

        private string businessRuleID { get; set; }

        StringBuilder Businessruledata = new StringBuilder();

        private string sqlconn;  // query and sql connection
        WaitBR waitForm = new WaitBR();

        private SqlDataAdapter SDA = new SqlDataAdapter();
        DataTable DT = new DataTable();
        DataTable DTCV = new DataTable();

        SqlCommandBuilder scb = new SqlCommandBuilder();

        LoadingForm loadform = new LoadingForm();

        private string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        private string groupID = LandingScreen.GetGroupId(LandingScreen.newschemaName);
        private int count1 { get; set; }

        static int currentPageIndex = 0;
        /// <summary>
        /// The items per page
        /// </summary>
        private int pageSize = 13;
        /// <summary>
        /// The total items
        /// </summary>
        private List<string> commodities;
        private List<string> outputnames;
        private List<string> parentlist;

        private Point dragStartLocation;

        string commodity = "";
        string outputname = "";
        string errorMessage = "";
        Dictionary<string, string> newSeriesIds = null;

        DataSet ds = new DataSet();

        public BusinessRulesTree()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
            Load += new EventHandler(BusinessRulesTree_Load);
            treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
            treeView1.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseDoubleClick);
            label_count.Text = "";
        }

        private void BusinessRulesTree_Load(object sender, System.EventArgs e)
        {
            fillCommodityCombobox("", "", "", "");
            fillOutputnameListBox("", "", "", "");
        }


        void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
          
           

            //resetting page index
            currentPageIndex = 0;

            if (e.Node.Parent == null && e.Node.Nodes.Count > 0)
            {
               // waitForm.Hide();
                MessageBox.Show("Only child nodes must be selected", "Warning");
                treeView1.SelectedNode = e.Node.Nodes[0];
                dataGridView1.DataSource = null;
                return;
            }
            

            try
            {
                //start wait form
                waitForm.Show(this);
                waitForm.Refresh();

                string selectednode = "";
                selectednode = e.Node.Text;
                selectednode = selectednode.Substring(selectednode.LastIndexOf("#") + 2);
                selectednode = selectednode.Replace(")", "");

                con.Open();

                string sql = "select  [ERSConstructedVariable_BusinessLogicID] AS RuleID "
                   + ",[ERSConstructedVariable_OutputDestination] as OutputDestination" +        
        ",ISNULL(CONVERT(varchar(20), ERSConstructedVariable_OutputValue), 'Erros in formula')  AS OutputValue " +
         ",ERSUnit_LU.ERSUnit_Desc as Unit " +
        ",[ERSConstructedVariable_TimeDimensionDate] AS Date " +
        ",[ERSConstructedVariable_OutputName] AS OutputName " +
        ",[ERSConstructedVariable_LongDescription] AS LongDescription " +
        ",[ERSConstructedVariable_InputSources] AS Source " +
        ",ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country " +
        ",ERSGeographyDimension_LU.ERSGeographyDimension_State as State " +
        ",[ERSConstructedVariable_ExecutionDate] AS ExecutionTime" +

        " FROM " + schemaName + "ERSBusinessLogic, " + schemaName + "ERSConstructedVariablesOutcomes, " + schemaName + "ERSGeographyDimension_LU, " + schemaName + "ERSUnit_LU WHERE 1=1 " + " and " +
        "ERSConstructedVariable_BusinessLogicID = " + selectednode +
        " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSConstructedVariablesOutcomes.ERSConstructedVariable_OutputGeographyDimensionID" +
        " and " + schemaName + "ERSBusinessLogic.ERSBusinessLogic_ID = " + schemaName + "ERSConstructedVariablesOutcomes.ERSConstructedVariable_BusinessLogicID" +
        " and " + schemaName + "ERSBusinessLogic.ERSBusinessLogic_InputUnitID = " + schemaName + "ERSUnit_LU.ERSUnit_ID";


                SDA = new SqlDataAdapter(@"select  [ERSConstructedVariable_BusinessLogicID] AS RuleID " +
        ",[ERSConstructedVariable_OutputDestination] as OutputDestination" +
        ",ISNULL(CONVERT(varchar(20), ERSConstructedVariable_OutputValue), 'Erros in formula')  AS OutputValue " +
        ",ERSUnit_LU.ERSUnit_Desc as Unit " +
        ",[ERSConstructedVariable_TimeDimensionDate] AS Date " +
        ",[ERSConstructedVariable_OutputName] AS OutputName " +
        ",[ERSConstructedVariable_LongDescription] AS LongDescription " +
        ",[ERSConstructedVariable_InputSources] AS Source " +
        ",ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country " +
        ",ERSGeographyDimension_LU.ERSGeographyDimension_State as State " +
        ",[ERSConstructedVariable_ExecutionDate] AS ExecutionTime" +

        " FROM " + schemaName + "ERSBusinessLogic, " + schemaName + "ERSConstructedVariablesOutcomes, " + schemaName + "ERSGeographyDimension_LU, " + schemaName + "ERSUnit_LU WHERE 1=1 " + " and " +
        "ERSConstructedVariable_BusinessLogicID = " + selectednode +
        " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSConstructedVariablesOutcomes.ERSConstructedVariable_OutputGeographyDimensionID" +
        " and " + schemaName + "ERSBusinessLogic.ERSBusinessLogic_ID = " + schemaName + "ERSConstructedVariablesOutcomes.ERSConstructedVariable_BusinessLogicID" +
        " and " + schemaName + "ERSBusinessLogic.ERSBusinessLogic_InputUnitID = " + schemaName + "ERSUnit_LU.ERSUnit_ID"
         , con);

                SDA.SelectCommand.ExecuteNonQuery();
                DTCV = new DataTable();
                SDA.Fill(DTCV);
                count1 = DTCV.Rows.Count;
                if (DTCV.Rows.Count > 0)
                {
                    dataGridView1.DataSource = DTCV;
                    dataGridView1.ReadOnly = true;
                    //BindGrid();

                    if (Convert.ToDecimal(DTCV.Rows.Count) <= pageSize)
                    {

                        button_next1.Enabled = false;
                        button_prev1.Enabled = false;

                    }

                    else
                    {
                        button_next1.Enabled = true;
                        button_prev1.Enabled = false;

                    }


                    waitForm.Hide();

                }
                else
                {
                    waitForm.Hide();
                    MessageBox.Show("No Constructed Variables Found");
                    dataGridView1.DataSource = DTCV;
                    //button_DeleteRules.BackColor = Color.Gray;
                    button_prev1.Enabled = false;
                    button_next1.Enabled = false;
                    label_pageNum.Text = "0 / 0";

                }
            }

            catch (Exception ex)
            {
                waitForm.Hide();
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }
            finally
            {
                con.Close();
            }
        }

        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (dragStartLocation == Point.Empty || dragStartLocation == e.Location)
            {
                dragStartLocation = Point.Empty;
                this.Text = "Business Rules Tree - " + e.Node.Text;
            }
        }



        private void button_retreive_Click_1(object sender, EventArgs e)
        {
           

            if (comboBox_Commodity1.Text.Equals(""))
            {
                MessageBox.Show("Please select commodity");
                return;
            }

            if (listBox_outputname.SelectedItem.ToString().Equals(""))
            {
                MessageBox.Show("Please select output name");
                return;
            }
            //start wait form
          
            waitForm.Show(this);
            waitForm.Refresh();
            dataGridView1.DataSource = null;
          
            //resetting page index
            currentPageIndex = 0;
            ds = new DataSet();
            commodity = comboBox_Commodity1.Text;
            outputname = listBox_outputname.SelectedItem.ToString();
            //
            //  button_TS.BackColor = Color.LightGray;

            try
            {

                if (commodity != "--Select All--")
                {
                    commodity = " and [ERSBusinessLogic_OutputName] like '%" + commodity + "%'";
                }
                else
                {
                    commodity = "";
                }

                if (outputname != "--Select All--")
                {
                    outputname = " and [ERSBusinessLogic_OutputName] = '" + outputname + "'";
                }
                else
                {
                    outputname = "";
                }

                con.Open();
                //printing

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
+ " WHERE 1=1 " + commodity + outputname + " ORDER BY ERSBusinessLogic_SequenceID  ", con);

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
                    dataGridView.Columns[0].DefaultCellStyle.ForeColor = Color.Gray;
                    dataGridView.Columns[0].ReadOnly = true;
                    BindGrid();
                    label1.ForeColor = System.Drawing.Color.Black;
                    rowcount_label.ForeColor = System.Drawing.Color.Black;
                  //  label1.Text = "Number of formulas retrieved : " + count1.ToString();
                    label_count.Text = "Number of formulas retrieved : " + count1.ToString();
                    //rowcount_label.Text = count1.ToString();

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
                   
                }
                else
                {
                    waitForm.Hide();
                    MessageBox.Show("No Business Rules Found");
                    dataGridView.DataSource = ds.Tables[0];

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
                //MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                MessageBox.Show("No Business Rules Found");
                //if (ex.Message != "Connection Error")
                //saveAction(ex.Message);
            }

            ///////////added by gayatri nambiar 
            //foreach(DataRow r in ds.Tables[0].Rows)
            //{

            
            treeView1.Nodes.Clear();
            string CommodityIDActualval = "0";
            businessRuleID = "0";
            string resequencingStatus = "";
            parentlist = new List<string>();
            parentlist.Add("");
            for (int i = 0; i < dataGridView.Rows.Count; i++)
                {

                try
                {
                  
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                    
                        //Reorder sequence IDs

                        resequencingStatus = resqeuencing(Convert.ToString(dataGridView.Rows[i].Cells["Rule ID"].Value));

                        if (!resequencingStatus.Equals("Updated"))
                        {
                            throw new Exception("Error Upating Sequence. Please contact technical team.");
                        }

                        businessRuleID = Convert.ToString(dataGridView.Rows[i].Cells["Rule ID"].Value);
                        //retrieve data
                        Businessruledata = new StringBuilder();
                        string brValue = "Select * From " + schemaName + "[ERSBusinessLogic] WHERE [ERSBusinessLogic_ID] IN  (" + businessRuleID + " )";
                        DataTable dt = GetData(brValue);
                        DataRow[] dr = dt.Select();
                       
                        CommodityIDActualval = dr[0][6].ToString();
                        //  string checkids = CommodityIDActualval.Replace(",", "").ToString();
                      
                        // start off by adding a base treeview node
                        if (!parentlist.Contains(CommodityIDActualval))
                        { 
                            TreeNode mainNode = new TreeNode();
                            
                            mainNode = treeView1.Nodes.Add(" DS # " + CommodityIDActualval.ToString());
                            
                            parentlist.Add(CommodityIDActualval.ToString());

                            PopulateTreeView(CommodityIDActualval, mainNode);
                        }
                    

                    }

                    else
                    {
                        MessageBox.Show("No rows Found");
                    }
                
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Error Upating Sequence"))
                        MessageBox.Show(ex.Message);
                    else
                        MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                    //saveAction(ex.Message);
                }

                /////////
                finally
                {
                    con.Close();
                }

                
            }

            waitForm.Hide();
        }


        private void PopulateTreeView(string parentId, TreeNode parentNode)

        {

            String Seqchildc = "SELECT * FROM " + schemaName + "[ERSBusinessLogic] WHERE ERSBusinessLogic_InputDataSeries  = '" + parentId + "'";

            //SqlDataAdapter dachildmnuc = new SqlDataAdapter(Seqchildc, con);

            // DataTable dtchildc = new DataTable();
            DataTable dtchildc = GetData(Seqchildc);
            // DataRow[] dr = dtchildc.Select();

            // dachildmnuc.Fill(dtchildc);

            TreeNode childNode;


            string outputdest ="";
            string Nlabel = "";
            string OutputbusinessRuleID = "";

            foreach (DataRow dr in dtchildc.Rows)

            {
                 outputdest = dr["ERSBusinessLogic_OutputDestination"].ToString();
                OutputbusinessRuleID = dr["ERSBusinessLogic_ID"].ToString();

                if (outputdest.Contains("N"))
                {
                    String CommodityIdN = "SELECT * FROM " + schemaName + "[ERSCommodityDataSeries] WHERE [ERSCommodity_SourceSeriesID] like '%" + outputdest.Substring(2) + "%'";
                    DataTable dtcommN = GetData(CommodityIdN);
                    DataRow[] drN = dtcommN.Select();
                    outputdest = drN[0][0].ToString();
                    Nlabel = "N # ";
                }

                if (parentNode == null)
                {
                     childNode = treeView1.Nodes.Add(outputdest);
                }
                else
                {
                    childNode = parentNode.Nodes.Add(Nlabel + outputdest + " (BR # " + OutputbusinessRuleID + ")");
                  
                    PopulateTreeView(outputdest, childNode);
                    parentlist.Add(outputdest.ToString());
                }
                
           
            }
        }



        private void fillCommodityCombobox(string sourceType, string businessType, string group, string description)
        {
            commodities = new List<string>();
            //string sql = @"SELECT DISTINCT [ERSSource_Desc] FROM " + schemaName + "[ERSSource_LU]";
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
    " select distinct ERSCommoditySubCommodity_Desc  from " + schemaName + " ERSCommoditySubCommodity_LU csc, " + schemaName +"ERSCommodityDataSeries cds, dataforids" +
    "   where csc.ERSCommoditySubCommodity_ID = cds.ERSCommoditySubCommodity_ID" +
    " and dataforids.Value = cds.ERSCommodity_ID";

            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            commodities.Add("");
            commodities.Add("--Select All--");
            foreach (DataRow row in dr)
            {
                //SourceType.Add(row["ERSSource_Desc"].ToString());
                commodities.Add(row["ERSCommoditySubCommodity_Desc"].ToString());
            }

            comboBox_Commodity1.DataSource = commodities;
            //comboBox_SourceType.SelectedItem = null;

        }

        private void fillOutputnameListBox(string sourceType, string businessType, string group, string description)
        {
            outputnames = new List<string>();
            string sql = @"SELECT DISTINCT ERSBusinessLogic_OutputName FROM " + schemaName +
                "[ERSBusinessLogic] order by ERSBusinessLogic_OutputName";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            outputnames.Add("");
            outputnames.Add("--Select All--");
            foreach (DataRow row in dr)
            {
             outputnames.Add(row["ERSBusinessLogic_OutputName"].ToString());
            }

            listBox_outputname.DataSource = outputnames;
            //comboBox_SourceType.SelectedItem = null;

        }


        private void GetNewRow(ref DataRow newRow, DataRow source)
        {
            foreach (DataColumn col in DT.Columns)
            {
                newRow[col.ColumnName] = source[col.ColumnName];
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


        private void connection()
        {
            // connection string for linking db
            sqlconn = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            // init the connection to db
            con = new SqlConnection(sqlconn);
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
                if (ex.Message.Contains("duplicate"))
                {
                    errorMessage = "duplicate";
                }
                return null;
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
+ " FROM  " + schemaName + "[ERSGeographyDimension_LU] "
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
+ " FROM  " + schemaName + "[ERSGeographyDimension_LU] "
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
+ " FROM  " + schemaName + "[ERSGeographyDimension_LU] "
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
                            newThousandNumber = Convert.ToInt32(Regex.Match(inputsArray[counter], @"\d+").Value); 
                                //- thousandNumber;
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
                                    newThousandNumber = Convert.ToInt32(Regex.Match(sourceSeriesID, @"\d+").Value);
                                        //- thousandNumber_N;
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
                        newThousandNumber = Convert.ToInt32(Regex.Match(inputsArray[counter], @"\d+").Value); 
                            //- thousandNumber;
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
        private string getOutputDestinationView(string outputDestination, int thousandNumber)
        {
            try
            {
                int outputDestinationNumber = Convert.ToInt32(Regex.Match(outputDestination, @"\d+").Value);
                //outputDestinationNumber = outputDestinationNumber - thousandNumber;
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

                    string sqlBl_Unit = "SELECT [ERSBusinessLogic_ID],[ERSBusinessLogic_InputUnitID] FROM  " + schemaName + "[ERSBusinessLogic] where ERSBusinessLogic_ID IN (" + splitBLid + ")";

                    System.Data.DataTable BL_Unit = GetData(sqlBl_Unit);

                    IList<string> unitId = BL_Unit.AsEnumerable().Select(x => x[1].ToString()).ToList();

                    string splitUnitid = string.Join(",", unitId);

                    string UnitDesc = "SELECT [ERSUnit_ID],[ERSUnit_Desc]FROM  " + schemaName + "[ERSUnit_LU] where ERSUnit_ID IN (" + splitUnitid + ")";

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

     

  


        //Rordering the sequence
        private string resqeuencing(string ruleID)
        {
            Decimal sqeuenceIDPrevID;

            try
            {
                //Previous
                string sqlPrevID = "SELECT MAX([ERSBusinessLogic_SequenceID]) AS ERSBusinessLogic_SequenceID FROM " + schemaName + "[ERSBusinessLogic]  where ERSBusinessLogic_SequenceID < (SELECT [ERSBusinessLogic_SequenceID] FROM  " + schemaName + "[ERSBusinessLogic]  where ERSBusinessLogic_ID = " + ruleID + ")";
                System.Data.DataTable dtsqlPrevID = GetData(sqlPrevID);

                if (dtsqlPrevID == null)
                {
                    DataRow[] drsqlPrevID = dtsqlPrevID.Select();
                    sqeuenceIDPrevID = Convert.ToDecimal(drsqlPrevID[0]["ERSBusinessLogic_SequenceID"]);

                    ////Update Sequence IDs
                    //string sqlGetID = "SELECT ERSBusinessLogic_ID FROM " + schemaName + "[ERSBusinessLogic] WHERE ERSBusinessLogic_SequenceID > " + sqeuenceIDPrevID + " and [ERSBusinessLogic_ID] NOT IN ("+ruleID+") ORDER BY [ERSBusinessLogic_SequenceID]";
                    ////Update Sequence IDs
                    //System.Data.DataTable dtsqlsqlGetID = GetData(sqlGetID);
                    //if (dtsqlsqlGetID.Rows.Count > 0)
                    //{
                    //    DataRow[] drsqlsqlGetID = dtsqlsqlGetID.Select();
                    //    foreach (DataRow row in drsqlsqlGetID)
                    //    {
                    //        sqeuenceIDPrevID++;
                    //        string sqlUpdateSeqID = "UPDATE " + schemaName + "[ERSBusinessLogic] SET [ERSBusinessLogic_SequenceID] = " + sqeuenceIDPrevID + " WHERE ERSBusinessLogic_ID = " + row["ERSBusinessLogic_ID"].ToString();
                    //        System.Data.DataTable dtsqlUpdateSeq = GetData(sqlUpdateSeqID);
                    //    }

                    //}

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

        private void label_PageHeading_Click(object sender, EventArgs e)
        {

        }

        private void label_Path_Click(object sender, EventArgs e)
        {

        }

        private void label_ApCoSD_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox_Commodity1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((comboBox_Commodity1.SelectedItem.ToString() != "") && (comboBox_Commodity1.SelectedItem.ToString() != "--Select All--"))
            {
                listBox_outputname.ClearSelected();
                outputnames = new List<string>();
                string sql = @"SELECT DISTINCT ERSBusinessLogic_OutputName FROM " + schemaName +
                    "[ERSBusinessLogic] where ERSBusinessLogic_OutputName like '%" + comboBox_Commodity1.SelectedItem.ToString() + "%' order by ERSBusinessLogic_OutputName";
                DataTable dtop = GetData(sql);
                DataRow[] drop = dtop.Select();
                outputnames.Add("");
                outputnames.Add("--Select All--");
                foreach (DataRow row in drop)
                {
                    outputnames.Add(row["ERSBusinessLogic_OutputName"].ToString());
                }

                listBox_outputname.DataSource = outputnames;
            }
            else if (comboBox_Commodity1.SelectedItem.ToString() == "--Select All--")
            {
                string sql = @"SELECT DISTINCT ERSBusinessLogic_OutputName FROM " + schemaName +
                   "[ERSBusinessLogic] order by ERSBusinessLogic_OutputName";
                DataTable dtop = GetData(sql);
                DataRow[] drop = dtop.Select();
                outputnames.Add("");
                outputnames.Add("--Select All--");
                foreach (DataRow row in drop)
                {
                    outputnames.Add(row["ERSBusinessLogic_OutputName"].ToString());
                }

                listBox_outputname.DataSource = outputnames;
            }
            else if (comboBox_Commodity1.SelectedItem.ToString() == "")
            {
                listBox_outputname.DataSource = null;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void button_back_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Parent = null;
        }
    }
}
