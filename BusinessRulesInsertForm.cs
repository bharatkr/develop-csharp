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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.SqlServer.Dts.Runtime;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.IO;

using Drawing = System.Drawing;
using AppForm = System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.InteropServices;


namespace CoSD_Tool
{
    public partial class BusinessRulesInsertForm : Form
    { 
        public string helpformulainputs;
        public string helpformuladataseries;
        public string helpsampleformula;
        public int helpinputscount;
        private List<string> ConversionsValue;
        private List<string> ConversionsDescription;
        private List<string> TimeInput;
        private List<string> OutputTime;
        private List<string> MacroValue;
        private List<string> MacroDescription;
        private List<string> geographyCountry;
        private List<string> geographyRegion;
        private List<string> geographyState;
        private List<string> geographyCounty;
        private List<string> inputUnit;
        private List<string> outputUnit;
        private List<string> privacylevel;
        private List<string> inputsources;
        private List<string> groupinvolved;
        private List<string> Businesslogic;
        private List<string> InputTimeValue;
        private List<string> groups;
        private List<string> groupIds;
      
        private List<string> OutputTimeValue;
        List<string> sheetNames = new List<string>();
        private string IDvalue { get; set; }
        string errorMessage = "";
        private int cnvvalue;
        private string macrovalue;
        private int inputTimetype;
        private int outputTimetype;
        private int inputUnitID;
        private int OutputUnitID;
        private int groupID;
        private int privacyID;
        private int geographyID;
        private int outputGeographyID;
        private int sourceID;
        private string CVvalue;
        private string maxCVvalue;
        private int number1;
        private string cvValueresultstring;
        private string sequenceIDNext = "";
        private string sequenceIDPrev = "";
        private string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        private SqlDataAdapter SDA = new SqlDataAdapter();
        private SqlDataAdapter SDAGetData = new SqlDataAdapter();
        private System.Data.DataTable DT = new System.Data.DataTable();
        SqlCommandBuilder scb = new SqlCommandBuilder();
        string apGroupId = LandingScreen.GetGroupId(LandingScreen.newschemaName);
        StringBuilder seperatedUnitIDs = null;
        StringBuilder seperatedGEOIDs = null;
        StringBuilder seperatedGEOTypeIDs = null;
        string cosdFolderPath = Home.globalCoSDPath;
        private string filePath;   // store the path of business rule excel file
        private string filePathNS; // store the path of new data series excel file
        private StringBuilder errorMessage_BLUpload = new StringBuilder();
        private StringBuilder errorMessage_NDSUpload = new StringBuilder();
        private int countAllerrorMsg;
       
        //Comment by Gowtham for testing
        //HelpFormula helpformula = new HelpFormula();
        /// <summary>
        /// The con
        /// </summary>
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString);
        /// <summary>
        /// The sqlconn
        /// </summary>
        private string sqlconn;  // query and sql connection
        private BindingSource bindingsource = new BindingSource();
        int successNum = 0;
        int statusIndex = 0;
        int ruleIDIndex = 0;
        int columnsCount = 0;
        int rowsCount = 0;
        int commoditySeriesIDIndex = 0;
        int inputUnitIndex = 0;
        int conversionFactorIndex = 0;
        int macroIndex = 0;
        int outputNameIndex = 0;
        int outputUnitIndex = 0;
        int formulaIndex = 0;
        int inputTimeDimensionTypeIndex = 0;
        int inputTimeValueIndex = 0;
        int outputTimeDimensionTypeIndex = 0;
        int outputTimeValueIndex = 0;
        int inputGeoTypeIndex = 0;
        int inputGeoValueIndex = 0;
        int outputGeoTypeIndex = 0;
        int outputGeoValueIndex = 0;
        int outputDestinationIndex = 0;
        int privacyIndex = 0;
        int typeIndex = 0;
        int longDescriptionIndex = 0;
        int inputSourcesIndex = 0;
        int Lastrow = 0;
        int fullRow = 0;
        string errmsg = "";

        //new data series
        int statusIndexNS = 0;
        int NewDataseriesIndexNS = 0;
        int newidIndexNS = 0;
        int commoditySeriesIDIndexNS = 0;
        int dataseriesdescrIndexNS = 0;
        int sectorIndexNS = 0;
        int groupIndexNS = 0;
        int subcommodityIndexNS = 0;
        int sourceseriesLongDescIndexNS = 0;
        int stattypeIndexNS = 0;
        int unitIndexNS = 0;
        int physicalAtttypeIndexNS = 0;
        int physicalAttDescIndexNS = 0;
        int prodpracIndexNS = 0;
        int utilpracIndexNS = 0;
        int sourceIndexNS = 0;

        //


        private void BusinessRulesInsertForm_Load(object sender, EventArgs e)
        {
            try
            {
                
                tabPage2.Visible = false; 

                // to disable horizontal scroll
                int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;

                tableLayoutPanel1.Padding = new Padding(0, 0, vertScrollWidth, 0);

            }
            catch (Exception ex)
            {
                return;
            }
        }

        /*Below method is used to enable and disbale conversion factor and macro.
         * 
         */
        //public void disableConvMacro()
        //{

        //    if (textBox_businessFormula.Text.Contains("conv"))
        //    {
        //        comboBox_ConfacValue.Enabled = true;
        //        combobox_conversionDescription.Enabled = true;
        //        comboBox_MacroDescription.Enabled = false;
        //    }
        //    else if (textBox_businessFormula.Text.Contains("macro"))
        //    {
        //        comboBox_ConfacValue.Enabled = false;
        //        combobox_conversionDescription.Enabled = false;
        //        comboBox_MacroDescription.Enabled = true;
        //    }
        //    else if (textBox_businessFormula.Text.Contains("conv") && textBox_businessFormula.Text.Contains("macro"))
        //    {
        //        comboBox_ConfacValue.Enabled = true;
        //        combobox_conversionDescription.Enabled = true;
        //        comboBox_MacroDescription.Enabled = true;

        //    }

        //}

        //public void helpformulafillmethod()
        //{
        //    if (helpformulainputs=="")
        //    {
        //        textBox_Inputs.Text = "";
        //    }

        //    else
        //    {
        //        textBox_Inputs.Text = helpformulainputs;
        //    }

        //    if (helpformuladataseries == "")
        //    {
        //        textBox_DataseriesID.Text = "";
        //    }

        //    else
        //    {
        //        textBox_DataseriesID.Text = helpformuladataseries;
        //    }

        //    if (helpsampleformula == "")
        //    {
        //        textBox_businessFormula.Text = "";
        //    }

        //    else
        //    {
        //        textBox_businessFormula.Text = helpsampleformula;
        //    }

        //    if (helpinputscount > 0)
        //    {
        //        numericUpDown_inputscount.Value = helpinputscount;
        //    }

        //}

        public BusinessRulesInsertForm()
        {
            this.Icon = Drawing.Icon.ExtractAssociatedIcon(AppForm.Application.ExecutablePath);
            InitializeComponent();
            label_status.Text = "";
            label_status1.Text = "";
            // label_returnedNS.Text = "";

            textBox_newdataseries.Text = "";

            if (DeSerializeSettingsPath<String>("CoSDSettings") != null)
                cosdFolderPath = DeSerializeSettingsPath<String>("CoSDSettings");

            if (!cosdFolderPath.EndsWith("\\"))
            {
                cosdFolderPath = cosdFolderPath + "\\";
            }
            try
            {
                ////Saving sample import sheet
                string resource = "Business Rules_Upload.xlsx";
                string filename = Path.Combine(cosdFolderPath + "CoSD Tool\\Business Rules\\", resource);

                if (!File.Exists(filename))
                {
                    Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Business Rules\\");
                    var assembly = Assembly.GetExecutingAssembly();
                    using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CoSD_Tool.Resources.Business Rules_Upload.xlsx"))
                    using (var targetStream = File.OpenWrite(cosdFolderPath + "CoSD Tool\\Business Rules\\Business Rules_Upload.xlsx"))
                    {
                        resourceStream.CopyTo(targetStream);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not find the path : " + cosdFolderPath + ". Please change path in tool to save CoSD results.");
            }
        }

        //private void disablebuttons()
        //{
        //    tableLayoutPanel_inputdetails.Visible = false;
        //    tableLayoutPanel_outputdetails.Visible = false;
        //    tableLayoutPanel_OutputGeo.Visible = false;
        //    label_inputvariable.Visible = false;
        //    label_outputvariable.Visible = false;
        //    button_savenewrules.Visible = false;




        //}

        //private void fillConversionFactorValueCombobox()
        //{
        //    try
        //    {
        //        ConversionsValue = new List<string>();
        //        string sql = @"SELECT DISTINCT [ERSConversionFactor_CF] FROM " + schemaName + "[ERSConversionFactors]";
        //        System.Data.DataTable dt = GetData(sql);
        //        DataRow[] dr = dt.Select();
        //        ConversionsValue.Add("");
        //        foreach (DataRow row in dr)
        //        {
        //            ConversionsValue.Add(row["ERSConversionFactor_CF"].ToString());
        //        }

        //        comboBox_ConfacValue.DataSource = ConversionsValue;
        //        comboBox_ConfacValue.SelectedItem = null;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Data processing unsuccessful, please contact technical team.");
        //    }
        //}

        //private void fillConversionFactorDescriptionCombobox()
        //{
        //    ConversionsDescription = new List<string>();

        //    string sql = "";

        //    try
        //    {
        //        sql = @"SELECT DISTINCT [ERSConversionFactor_Desc] FROM " + schemaName + "[ERSConversionFactors]";

        //        System.Data.DataTable dt = GetData(sql);
        //        if (null != dt)
        //        {
        //            DataRow[] dr = dt.Select();
        //            ConversionsDescription.Add("");
        //            foreach (DataRow row in dr)
        //            {
        //                ConversionsDescription.Add(row["ERSConversionFactor_Desc"].ToString());
        //            }


        //            combobox_conversionDescription.DataSource = ConversionsDescription;
        //            combobox_conversionDescription.SelectedItem = null;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Data processing unsuccessful, please contact technical team.");
        //    }

        //}

        //private void fillBusinessogictypeCombobox()
        //{
        //    Businesslogic = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSBusinessLogic_Type] FROM " + schemaName + "[ERSBusinessLogic]";
        //    Businesslogic.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        Businesslogic.Add(row["ERSBusinessLogic_Type"].ToString());
        //    }

        //    Businesslogic.Add("Other");
        //    combobox_businesslogicType.DataSource = Businesslogic;
        //    combobox_businesslogicType.SelectedItem = null;

        //}

        //private void fillMacroTypeDescriptionCombobox()
        //{
        //    MacroDescription = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSMacro_Desc] FROM " + schemaName + "[ERSMacro_LU]";

        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    MacroDescription.Add(" ");
        //    foreach (DataRow row in dr)
        //    {
        //        MacroDescription.Add(row["ERSMacro_Desc"].ToString());
        //    }

        //    comboBox_MacroDescription.DataSource = MacroDescription;
        //    comboBox_MacroDescription.SelectedItem = null;

        //}

        //private void fillInputTimeDimensionTypeCombobox()
        //{
        //    TimeInput = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSTimeDimensionType_Desc] FROM " + schemaName + "[ERSTimeDimensionType_LU]";
        //    TimeInput.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        TimeInput.Add(row["ERSTimeDimensionType_Desc"].ToString());
        //    }

        //    comboBox_IPtimeDType.DataSource = TimeInput;
        //    comboBox_IPtimeDType.SelectedItem = null;

        //}
        //private void fillOutputTimeDimensionTypeCombobox()
        //{
        //    OutputTime = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSTimeDimensionType_Desc] FROM " + schemaName + "[ERSTimeDimensionType_LU]";
        //    OutputTime.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        OutputTime.Add(row["ERSTimeDimensionType_Desc"].ToString());
        //    }

        //    comboBox_OPtimeDtype.DataSource = OutputTime;
        //    comboBox_OPtimeDtype.SelectedItem = null;

        //}

        //private void fillGeographyDimensionCountryTypeCombobox()
        //{
        //    geographyCountry = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSGeographyDimension_Country] FROM " + schemaName + "[ERSGeographyDimension_LU]";
        //    geographyCountry.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        geographyCountry.Add(row["ERSGeographyDimension_Country"].ToString());
        //    }

        //    comboBox_country.DataSource = geographyCountry;
        //    comboBox_country.SelectedItem = null;

        //}


        //private void fillGeographyDimensionRegionTypeCombobox()
        //{
        //    geographyRegion = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSGeographyDimension_Region] FROM " + schemaName + "[ERSGeographyDimension_LU]";
        //    geographyRegion.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        geographyRegion.Add(row["ERSGeographyDimension_Region"].ToString());
        //    }

        //    comboBox_region.DataSource = geographyRegion;
        //    comboBox_region.SelectedItem = null;

        //}

        //private void fillGeographyDimensionStateTypeCombobox()
        //{
        //    geographyState = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSGeographyDimension_State] FROM " + schemaName + "[ERSGeographyDimension_LU]";
        //    geographyState.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        geographyState.Add(row["ERSGeographyDimension_State"].ToString());
        //    }

        //    comboBox_state.DataSource = geographyState;
        //    comboBox_state.SelectedItem = null;

        //}

        //private void fillGeographyDimensionCountyTypeCombobox()
        //{
        //    geographyCounty = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSGeographyDimension_County] FROM " + schemaName + "[ERSGeographyDimension_LU]";
        //    geographyCounty.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        geographyCounty.Add(row["ERSGeographyDimension_County"].ToString());
        //    }

        //    comboBox_county.DataSource = geographyCounty;
        //    comboBox_county.SelectedItem = null;

        //}

        //private void fillOutputGeographyDimensionCountryTypeCombobox()
        //{
        //    geographyCountry = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSGeographyDimension_Country] FROM " + schemaName + "[ERSGeographyDimension_LU]";
        //    geographyCountry.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        geographyCountry.Add(row["ERSGeographyDimension_Country"].ToString());
        //    }

        //    combobox_outputCountry.DataSource = geographyCountry;
        //    combobox_outputCountry.SelectedItem = null;

        //}


        //private void fillOutputGeographyDimensionRegionTypeCombobox()
        //{
        //    geographyRegion = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSGeographyDimension_Region] FROM " + schemaName + "[ERSGeographyDimension_LU]";
        //    geographyRegion.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        geographyRegion.Add(row["ERSGeographyDimension_Region"].ToString());
        //    }

        //    combobox_outputRegion.DataSource = geographyRegion;
        //    combobox_outputRegion.SelectedItem = null;

        //}

        //private void fillOutputGeographyDimensionStateTypeCombobox()
        //{
        //    geographyState = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSGeographyDimension_State] FROM " + schemaName + "[ERSGeographyDimension_LU]";
        //    geographyState.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        geographyState.Add(row["ERSGeographyDimension_State"].ToString());
        //    }

        //    combobox_outputState.DataSource = geographyState;
        //    combobox_outputState.SelectedItem = null;

        //}

        //private void fillOutputGeographyDimensionCountyTypeCombobox()
        //{
        //    geographyCounty = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSGeographyDimension_County] FROM " + schemaName + "[ERSGeographyDimension_LU]";
        //    geographyCounty.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        geographyCounty.Add(row["ERSGeographyDimension_County"].ToString());
        //    }

        //    combobox_outputCounty.DataSource = geographyCounty;
        //    combobox_outputCounty.SelectedItem = null;

        //}


        //private void fillInputUnitCombobox()
        //{
        //    inputUnit = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSUnit_Desc] FROM " + schemaName + "[ERSUnit_LU]";
        //    inputUnit.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        inputUnit.Add(row["ERSUnit_Desc"].ToString());
        //    }

        //    comboBox_inputunit.DataSource = inputUnit;
        //    comboBox_inputunit.SelectedItem = null;

        //}

        //private void fillOutputUnitCombobox()
        //{
        //    outputUnit = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSUnit_Desc] FROM " + schemaName + "[ERSUnit_LU]";
        //    outputUnit.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        outputUnit.Add(row["ERSUnit_Desc"].ToString());
        //    }

        //    comboBox_outputUnit.DataSource = outputUnit;
        //    comboBox_outputUnit.SelectedItem = null;

        //}

        //private void fillPrivacyLevelCombobox()
        //{
        //    privacylevel = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSDataPrivacy_Desc] FROM " + schemaName + "[ERSDataPrivacy_LU]";
        //    privacylevel.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        privacylevel.Add(row["ERSDataPrivacy_Desc"].ToString());
        //    }

        //    comboBox_privacylevel.DataSource = privacylevel;
        //    comboBox_privacylevel.SelectedItem = null;

        //}


        //private void fillInputSourcesCombobox()
        //{
        //    inputsources = new List<string>();
        //    string sql = @"SELECT DISTINCT [ERSSource_Desc] FROM " + schemaName + "[ERSSource_LU]";
        //    inputsources.Add(" ");
        //    System.Data.DataTable dt = GetData(sql);
        //    DataRow[] dr = dt.Select();
        //    foreach (DataRow row in dr)
        //    {
        //        inputsources.Add(row["ERSSource_Desc"].ToString());
        //    }

        //    comboBox_InputSources.DataSource = inputsources;
        //    comboBox_InputSources.SelectedItem = null;

        //}

        //private void fillGroupInvolvedCombobox()
        //{
        //    groupIds = new List<string>();

        //    groupIds = getGroupIds();

        //    groups = new List<string>();
        //    foreach (string groupID in groupIds)
        //    {
        //        string sql = "select ERSGroup_Desc from " + schemaName + "ERSGroup_LU where ERSGroup_ID = " + groupID;
        //        System.Data.DataTable dt = GetData(sql);
        //        DataRow[] dr = dt.Select();
        //        foreach (DataRow row in dr)
        //        {
        //            groups.Add(row["ERSGroup_Desc"].ToString());
        //        }
        //    }
        //    comboBox_groupinvolved.DataSource = groups;
        //    comboBox_groupinvolved.SelectedItem = null;

        //}
        /*Retrieve groups according to logged in user
         */
        private List<string> getGroupIds()
        {
            groupIds = new List<string>();
            // get current user            
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            string sql = "select ERSGroup_ID from " + schemaName + "ERSGroup_LU where ERSGroup_Username like '%" + currentUser.Name + "%' and [ERSGroup_ID] IN (" + apGroupId + ")";
            System.Data.DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            foreach (DataRow row in dr)
            {
                groupIds.Add(row["ERSGroup_ID"].ToString());
            }
            return groupIds;
        }
        //private void fillInputTimeValueCombobox()
        //{
        //    InputTimeValue = new List<string>();
        //    string[] ArrayInputTime = new string[4] { "all months", "all years", "year-to-date months", "previous month" };
        //     InputTimeValue.Add(" ");
        //    for(int i=0; i<=3; i++)
        //    {
        //    InputTimeValue.Add(ArrayInputTime[i]);
        //    }
           
        //    combobox_inputTimeDimensionValue.DataSource = InputTimeValue;
        //    combobox_inputTimeDimensionValue.SelectedItem = null;

        //}

        //private void fillOutputTimeValueCombobox()
        //{
        //    OutputTimeValue = new List<string>();
        //    string[] ArrayOutputTime = new string[5] { "all months", "all years", "previous year", "current month", "current year" };
        //    OutputTimeValue.Add(" ");
        //    for (int i = 0; i <= 4; i++)
        //    {
        //        OutputTimeValue.Add(ArrayOutputTime[i]);
        //    }

        //    combobox_outputTimeDimensionValue.DataSource = OutputTimeValue;
        //    combobox_outputTimeDimensionValue.SelectedItem = null;

        //}


        private void connection()
        {
            // connection string for linking db
            sqlconn = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString; 
            // init the connection to db
            con = new SqlConnection(sqlconn);
        }

        private System.Data.DataTable GetData(string selectCommand)
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
                SDAGetData = new SqlDataAdapter(selectCommand, con);


                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(SDAGetData);


                System.Data.DataTable result = new System.Data.DataTable();
                SDAGetData.Fill(result);

                // see whether get any results
                //  int resultCount = result.Rows.Count;
                //Console.WriteLine(resultCount);

                return result;

            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("duplicate"))
                {
                    errorMessage = "duplicate";
                }
                return null;
            }
        }


        //private string SubquereyexecutionForInsertion()
        //{
        //    //This method call selects the next CV ID for the new business rule based on conditions. 
        //    string cvStatus = getMaxCvValue();
        //    if (cvStatus != "")
        //    {
        //        return cvStatus;
        //    }

        //    try
        //    {
        //        con.Open();
        //        string geo;
        //        SqlCommand cmd1, cmd2, cmd3, cmd4, cmd5, cmd6, cmd7, cmd8, cmd9;
        //        cmd1 = new SqlCommand(@"(Select ERSConversionFactorID FROM " + schemaName + "[ERSConversionFactors] WHERE [ERSConversionFactor_CF] =@conversionvalue AND [ERSConversionFactor_Desc] = @convDescription)", con);
        //        cmd3 = new SqlCommand(@"(Select ERSTimeDimensionType_ID FROM " + schemaName + "[ERSTimeDimensionType_LU] WHERE [ERSTimeDimensionType_Desc] = @inputTime)", con);
        //        cmd4 = new SqlCommand(@"(Select ERSTimeDimensionType_ID FROM " + schemaName + "[ERSTimeDimensionType_LU] WHERE [ERSTimeDimensionType_Desc] = @outputTime)", con);
        //        cmd5 = new SqlCommand(@"(Select ERSUnit_ID FROM " + schemaName + "[ERSUnit_LU] WHERE [ERSUnit_Desc] = @inputUnit)", con);
        //        cmd6 = new SqlCommand(@"(Select ERSUnit_ID FROM " + schemaName + "[ERSUnit_LU] WHERE [ERSUnit_Desc] = @outputUnit)", con);
        //        cmd7 = new SqlCommand(@"(Select ERSGroup_ID FROM " + schemaName + "[ERSGroup_LU] WHERE [ERSGroup_Desc] = @groupID)", con);
        //        cmd8 = new SqlCommand(@"(Select ERSDataPrivacy_ID FROM " + schemaName + "[ERSDataPrivacy_LU] WHERE [ERSDataPrivacy_Desc] = @privacyID)", con);
        //        cmd9 = new SqlCommand(@"(Select [ERSSource_ID] FROM " + schemaName + "[ERSSource_LU] WHERE [ERSSource_Desc] = @sourceDesc)", con);

        //        if (comboBox_ConfacValue.Text == "" && combobox_conversionDescription.Text != "")
        //        {
        //            cmd1 = new SqlCommand(@"(Select ERSConversionFactorID FROM " + schemaName + "[ERSConversionFactors] WHERE [ERSConversionFactor_CF] IS NULL AND [ERSConversionFactor_Desc] = @convDescription)", con);
        //            cmd1.Parameters.Add("@convDescription", SqlDbType.NVarChar, 160).Value = combobox_conversionDescription.Text;
        //        }
        //        else if (comboBox_ConfacValue.Text != "" && combobox_conversionDescription.Text != "")
        //        {
        //            cmd1 = new SqlCommand(@"(Select ERSConversionFactorID FROM " + schemaName + "[ERSConversionFactors] WHERE [ERSConversionFactor_CF] =@conversionvalue AND [ERSConversionFactor_Desc] = @convDescription)", con);
        //            cmd1.Parameters.Add("@conversionvalue", SqlDbType.NVarChar, 160).Value = comboBox_ConfacValue.Text;
        //            cmd1.Parameters.Add("@convDescription", SqlDbType.NVarChar, 160).Value = combobox_conversionDescription.Text;
        //        }

        //        else if (combobox_conversionDescription.Text == "" && comboBox_ConfacValue.Text != "")
        //        {
        //            cmd1 = new SqlCommand(@"(Select ERSConversionFactorID FROM " + schemaName + "[ERSConversionFactors] WHERE [ERSConversionFactor_CF] =@conversionvalue AND [ERSConversionFactor_Desc] IS NULL)", con);
        //            cmd1.Parameters.Add("@conversionvalue", SqlDbType.NVarChar, 160).Value = comboBox_ConfacValue.Text;
        //        }
        //        else if (combobox_conversionDescription.Text == "" && comboBox_ConfacValue.Text == "")
        //        {
        //            cmd1 = new SqlCommand(@"(Select ERSConversionFactorID FROM " + schemaName + "[ERSConversionFactors] WHERE [ERSConversionFactor_CF] IS NULL AND [ERSConversionFactor_Desc] IS NULL)", con);
        //        }

        //        // for macrovalues we have the below conditions

        //        //if (comboBox_MacroValue.Text == "")
        //        //{
        //        //    cmd2.Parameters.Add("@macrovalue", SqlDbType.NVarChar, 160).Value = DBNull.Value;
        //        //}
        //        //else
        //        //{
        //        //    cmd2.Parameters.Add("@macrovalue", SqlDbType.NVarChar, 160).Value = comboBox_MacroValue.Text;
        //        //}

        //        //if (comboBox_MacroDescription.Text == "")
        //        //{
        //        //    cmd2.Parameters.Add("@macroDescription", SqlDbType.NVarChar, 160).Value = DBNull.Value;
        //        //}
        //        //else
        //        //{
        //        //    cmd2.Parameters.Add("@macroDescription", SqlDbType.NVarChar, 160).Value = comboBox_MacroDescription.Text;
        //        //}
        //        // for inputtime type ID we have the below conditions

        //        if (comboBox_IPtimeDType.Text == "")
        //        {
        //            cmd3.Parameters.Add("@inputTime", SqlDbType.NVarChar, 160).Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            cmd3.Parameters.Add("@inputTime", SqlDbType.NVarChar, 160).Value = comboBox_IPtimeDType.Text;
        //        }

        //        // for outputTime type ID we have the below conditions

        //        if (comboBox_OPtimeDtype.Text == "")
        //        {
        //            cmd4.Parameters.Add("@outputTime", SqlDbType.NVarChar, 160).Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            cmd4.Parameters.Add("@outputTime", SqlDbType.NVarChar, 160).Value = comboBox_OPtimeDtype.Text;
        //        }
        //        // for InputUnit ID we have the below conditions

        //        if (comboBox_inputunit.Text == "")
        //        {
        //            cmd5.Parameters.Add("@inputUnit", SqlDbType.NVarChar, 160).Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            cmd5.Parameters.Add("@inputUnit", SqlDbType.NVarChar, 160).Value = comboBox_inputunit.Text;
        //        }
        //        // for OutputUnit ID we have the below conditions

        //        if (comboBox_outputUnit.Text == "")
        //        {
        //            cmd6.Parameters.Add("@outputUnit", SqlDbType.NVarChar, 160).Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            cmd6.Parameters.Add("@outputUnit", SqlDbType.NVarChar, 160).Value = comboBox_outputUnit.Text;
        //        }

        //        // for group ID we have the below conditions

        //        if (comboBox_groupinvolved.Text == "")
        //        {
        //            cmd7.Parameters.Add("@groupID", SqlDbType.NVarChar, 160).Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            cmd7.Parameters.Add("@groupID", SqlDbType.NVarChar, 160).Value = comboBox_groupinvolved.Text;
        //        }
        //        // for privacy ID we have the below conditions

        //        if (comboBox_privacylevel.Text == "")
        //        {
        //            cmd8.Parameters.Add("@privacyID", SqlDbType.NVarChar, 160).Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            cmd8.Parameters.Add("@privacyID", SqlDbType.NVarChar, 160).Value = comboBox_privacylevel.Text;
        //        }
        //        if (comboBox_InputSources.Text == "")
        //        {
        //            cmd9.Parameters.Add("@sourceDesc", SqlDbType.NVarChar, 160).Value = DBNull.Value;
        //        }
        //        else
        //        {
        //            cmd9.Parameters.Add("@sourceDesc", SqlDbType.NVarChar, 160).Value = comboBox_InputSources.Text;
        //        }
        //        cnvvalue = Convert.ToInt32(cmd1.ExecuteScalar());             
        //        //macrovalue = Convert.ToString(cmd2.ExecuteScalar());             
        //        inputTimetype = Convert.ToInt32(cmd3.ExecuteScalar());
        //        outputTimetype = Convert.ToInt32(cmd4.ExecuteScalar());
        //        inputUnitID = Convert.ToInt32(cmd5.ExecuteScalar()); 
        //        OutputUnitID = Convert.ToInt32(cmd6.ExecuteScalar());
        //        groupID = Convert.ToInt32(cmd7.ExecuteScalar());
        //        privacyID = Convert.ToInt32(cmd8.ExecuteScalar());
        //        sourceID = Convert.ToInt32(cmd9.ExecuteScalar());
        //        geo = getInputGeoID();
        //        if (geo.Equals("NOT FILTER BY GEO"))
        //        {
        //            return "Please select input geography information";
        //        }
        //        else if (geo.Equals("NOT FOUND"))
        //        {
        //            return "No records found for selected input geography";
        //        }
        //        geographyID = Convert.ToInt32(geo);

        //        geo = getOutputGeoID();
        //        if (geo.Equals("NOT FILTER BY GEO"))
        //        {
        //            return "Please select output geography information";
        //        }
        //        else if (geo.Equals("NOT FOUND"))
        //        {
        //            return "No records found for selected output geography";
        //        }
        //        outputGeographyID = Convert.ToInt32(geo);
                
        //        return "";
        //    }

        //    catch (Exception ex)
        //    {
        //        return "Data processing unsuccessful, please contact technical team.";
        //    }

        //    finally
        //    {
        //        con.Close();

        //    }

        //}

        //private string getInputGeoID()
        //{
        //    string result = "";
        //    string country = comboBox_country.Text.ToString();
        //    string region = comboBox_region.Text.ToString();
        //    string state = comboBox_state.Text.ToString();
        //    string county = comboBox_county.Text.ToString();
        //    // if all empty
        //    // means doesn't care about geo
        //    // include all geo -- don't filter by geo
        //    if (country.Equals("") && region.Equals("") && state.Equals("") && county.Equals(""))
        //    {
        //        return "NOT FILTER BY GEO";
        //    }

        //    if (country.Equals(""))
        //        country = "IS NULL";
        //    else
        //        country = "= '" + country + "' ";
        //    if (region.Equals(""))
        //        region = "IS NULL";
        //    else
        //        region = "= '" + region + "' ";
        //    if (state.Equals(""))
        //        state = "IS NULL";
        //    else
        //        state = "= '" + state + "' ";
        //    if (county.Equals(""))
        //        county = "IS NULL";
        //    else
        //        county = "= '" + county + "' ";
        //    //  Console.WriteLine(country + region + state);
        //    string sql = "select ERSGeographyDimension_ID from " + schemaName + "ERSGeographyDimension_LU where ERSGeographyDimension_Country " + country + " and ERSGeographyDimension_Region " + region + " and ERSGeographyDimension_State " + state + " and ERSGeographyDimension_County "+county;
        //    System.Data.DataTable dt = GetData(sql);
        //    if (dt == null)
        //        return "NOT FOUND";
        //    DataRow[] dr = dt.Select();
        //    if (dt.Rows.Count > 0)
        //        result = dr[0]["ERSGeographyDimension_ID"].ToString();
        //    else
        //        result = "NOT FOUND";

        //    return result;
        //}

        //private string getOutputGeoID()
        //{
        //    string result = "";
        //    string country = combobox_outputCountry.Text.ToString();
        //    string region = combobox_outputRegion.Text.ToString();
        //    string state = combobox_outputState.Text.ToString();
        //    string county = combobox_outputCounty.Text.ToString();
        //    // if all empty
        //    // means doesn't care about geo
        //    // include all geo -- don't filter by geo
        //    if (country.Equals("") && region.Equals("") && state.Equals("") && county.Equals(""))
        //    {
        //        return "NOT FILTER BY GEO";
        //    }

        //    if (country.Equals(""))
        //        country = "IS NULL";
        //    else
        //        country = "= '" + country + "' ";
        //    if (region.Equals(""))
        //        region = "IS NULL";
        //    else
        //        region = "= '" + region + "' ";
        //    if (state.Equals(""))
        //        state = "IS NULL";
        //    else
        //        state = "= '" + state + "' ";
        //    if (county.Equals(""))
        //        county = "IS NULL";
        //    else
        //        county = "= '" + county + "' ";
        //    //  Console.WriteLine(country + region + state);
        //    string sql = "select ERSGeographyDimension_ID from " + schemaName + "ERSGeographyDimension_LU where ERSGeographyDimension_Country " + country + " and ERSGeographyDimension_Region " + region + " and ERSGeographyDimension_State " + state + " and ERSGeographyDimension_County " + county;
        //    System.Data.DataTable dt = GetData(sql);
        //    if (dt == null)
        //        return "NOT FOUND";
        //    DataRow[] dr = dt.Select();
        //    if (dt.Rows.Count > 0)
        //        result = dr[0]["ERSGeographyDimension_ID"].ToString();
        //    else
        //        result = "NOT FOUND";

        //    return result;
        //}

        /**
 * This method selects the next CV ID for the new business rule based on conditions. 
 **/
        //private string getMaxCvValue()
        //{
        //    String[] dataseriesID = textBox_DataseriesID.Text.Split(',');

        //    //List<String> dataseriesIDList = new List<string>();

        //    try
        //    {

        //        string sql = "SELECT [ERSCommodity_ID] FROM " + schemaName + "[ERSCommodityDataSeries] where ERSCommoditySubCommodity_ID IN (SELECT [ERSCommoditySubCommodity_ID] FROM "+schemaName+"[ERSCommodityDataSeries] where ERSCommodity_ID = " + dataseriesID[0] + ")";
        //        System.Data.DataTable dataSeriesTable = GetData(sql);
        //        if (dataSeriesTable.Rows.Count == 0)
        //        {
        //            return "Data Series Not Found";
        //        }

        //        DataRow[] dataSeriesRow = dataSeriesTable.Select();

        //        StringBuilder query = new StringBuilder();
        //        con.Open();
        //        foreach (DataRow row in dataSeriesRow)
        //        {

        //            /**
        //             * For example - 
        //             * SELECT MAX([ERSBusinessLogic_OutputDestination]) FROM [AnimalProductsCoSD].[CoSD].[ERSBusinessLogic] 
        //                WHERE [ERSBusinessLogic_OutputDestination] NOT like '%N%' AND
        //                (ERSBusinessLogic_InputDataSeries like ' 157 ' or 
        //                ERSBusinessLogic_InputDataSeries like ' 157,%' or
        //                ERSBusinessLogic_InputDataSeries like '%,157,%' or
        //                ERSBusinessLogic_Inpu\DataSeries like '%,157 ')
        //             **/
        //            query.Append("SELECT MAX(CONVERT(INT, RIGHT([ERSBusinessLogic_OutputDestination], LEN([ERSBusinessLogic_OutputDestination])-2))) FROM " + schemaName + "[ERSBusinessLogic] WHERE [ERSBusinessLogic_OutputDestination] NOT like '%N%' AND (LTRIM ( RTRIM ([ERSBusinessLogic_InputDataSeries])) like '" + row["ERSCommodity_ID"].ToString() + "' or LTRIM([ERSBusinessLogic_InputDataSeries]) like '" + row["ERSCommodity_ID"].ToString() + ",%' or ERSBusinessLogic_InputDataSeries like '%," + row["ERSCommodity_ID"].ToString() + ",%' or RTRIM([ERSBusinessLogic_InputDataSeries]) like '%," + row["ERSCommodity_ID"].ToString() + "')");
        //            SqlCommand cmd1 = new SqlCommand(@"SELECT MAX(CONVERT(INT, RIGHT([ERSBusinessLogic_OutputDestination], LEN([ERSBusinessLogic_OutputDestination])-2))) FROM " + schemaName + "[ERSBusinessLogic] WHERE [ERSBusinessLogic_OutputDestination] NOT like '%N%' AND (LTRIM ( RTRIM ([ERSBusinessLogic_InputDataSeries])) like '" + row["ERSCommodity_ID"].ToString() + "' or LTRIM([ERSBusinessLogic_InputDataSeries]) like '" + row["ERSCommodity_ID"].ToString() + ",%' or ERSBusinessLogic_InputDataSeries like '%," + row["ERSCommodity_ID"].ToString() + ",%' or RTRIM([ERSBusinessLogic_InputDataSeries]) like '%," + row["ERSCommodity_ID"].ToString() + "')", con);
        //            CVvalue = Convert.ToString(cmd1.ExecuteScalar());
        //            if (CVvalue != "")
        //                break;

        //        }
        //        //Conditions to select the way in which next cv value will be incremented.
        //        if (CVvalue != null && CVvalue != "")
        //        {
        //            cvValueresultstring = Regex.Match(CVvalue, @"\d+").Value;
        //            number1 = Int32.Parse(cvValueresultstring);
        //            number1 = number1 + 1;
        //            maxCVvalue = "CV" + number1;
        //            return "";

        //        }
        //        else
        //        {
        //            SqlCommand cmd1 = new SqlCommand(@"SELECT MAX(CONVERT(INT, RIGHT([ERSBusinessLogic_OutputDestination], LEN([ERSBusinessLogic_OutputDestination])-2))) FROM " + schemaName + "[ERSBusinessLogic] WHERE [ERSBusinessLogic_OutputDestination] NOT like '%N%'", con);
        //            CVvalue = Convert.ToString(cmd1.ExecuteScalar());
        //            cvValueresultstring = Regex.Match(CVvalue, @"\d+").Value;
        //            number1 = Int32.Parse(cvValueresultstring);
        //            if (number1 > 1 && number1 < 1000)
        //                number1 = 1000;
        //            else if (number1 >= 1000)
        //                number1 = (((number1 / 1000) + 1) * 1000);
        //            maxCVvalue = "CV" + number1;
        //            return "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Data Processing unsuccessfull. Please contact technical team";
        //    }

        //    finally
        //    {
        //        con.Close();

        //    }

        //}
        //private void button_savenewrules_Click(object sender, EventArgs e)
        //{

        //    if (textBox_businessFormula.Text.Equals(Convert.ToString(0)))
        //    {
        //        MessageBox.Show("Please enter the formula to be inserted.");
        //        return;
        //    }

        //    if (numericUpDown_inputscount.Text.Equals(""))
        //    {
        //        MessageBox.Show("Please enter inputs count for the formula.");
        //        return;
        //    }

        //    if (textBox_Inputs.Text.Equals(""))
        //    {
        //        MessageBox.Show("Please enter inputs for the formula.");
        //        return;
        //    }


        //    if (combobox_inputTimeDimensionValue.Text.Equals(""))
        //    {
        //        MessageBox.Show("Please enter the input time dimension.");
        //        return;
        //    }

        //    if (combobox_outputTimeDimensionValue.Text.Equals(""))
        //    {
        //        MessageBox.Show("Please enter the output time dimension.");
        //        return;
        //    }

        //    if (textBox_DataseriesID.Text.Equals(""))
        //    {
        //        MessageBox.Show("Please enter data series ID for the formula.");
        //        return;
        //    }

        //    if (comboBox_inputunit.Text.Equals(""))
        //    {
        //        MessageBox.Show("Please select input unit for the inputs.");
        //        return;
        //    }


        //    if (combobox_businesslogicType.Text.Equals(""))
        //    {
        //        MessageBox.Show("Please enter business logic type.");
        //        return;
        //    }

        //    if (comboBox_privacylevel.Text.Equals(""))
        //    {
        //        MessageBox.Show("Please select privacy level.");
        //        return;
        //    }


        //    if (textBox_OutputName.Text.Equals(""))
        //    {
        //        MessageBox.Show("Please enter output name.");
        //        return;
        //    }

        //    if (comboBox_outputUnit.Text.Equals(""))
        //    {
        //        MessageBox.Show("Please select output unit for the inputs.");
        //        return;
        //    }

        //    if (combobox_businesslogicType.Text.Equals("Other"))
        //    {
        //        combobox_businesslogicType.Text = textBox_businesslogicType.Text;

        //    }

        //    string status = "";

        //    status = SubquereyexecutionForInsertion();

        //    if (status != "")
        //    {
        //        MessageBox.Show(status);
        //        return;
        //    }

           
        //    try
        //    {
        //        con.Open();
        //        if (MessageBox.Show("Do you really want to Insert these values?", "Confirm Insert", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        {

        //            SqlCommand cmd = new SqlCommand(@"Insert INTO " + schemaName + "[ERSBusinessLogic] (ERSBusinessLogic_Formula,ERSBusinessLogic_InputsCount,ERSBusinessLogic_Inputs,ERSBusinessLogic_ConvFactorID,ERSBusinessLogic_MacroDesc,ERSBusinessLogic_InputDataSeries,ERSBusinessLogic_InputTimeDimensionValue,ERSBusinessLogic_InputTimeDimensionTypeID,ERSBusinessLogic_InputGeographyDimensionID,ERSBusinessLogic_OutputGeographyDimensionID,ERSBusinessLogic_InputUnitID,ERSBusinessLogic_Type,ERSBusinessLogic_PrivacyID,ERSBusinessLogic_LongDesc,ERSBusinessLogic_InputSources,ERSBusinessLogic_InputSourceID,ERSBusinessLogic_OutputName,ERSBusinessLogic_OutputUnitID,ERSBusinessLogic_OutputDestination,ERSBusinessLogic_OutputTimeDimensionValue,ERSBusinessLogic_OutputTimeDimensionTypeID,ERSBusinessLogic_GroupID) VALUES (@BLformula, @inputscount, @inputs, @conversionfac, @macrodesc, @Dataseries, @inputtimevalue, @inputTimetype, @inputgeoID, @outputgeoID, @inputUnitID, @Businelogictype, @privacyID, @longDesc, @Inputsource, @InputsourceID, @outputname, @outputUnitID, @outputDestination, @outputTimevalue, @ouputTimeType, @groupID)", con);
        //            cmd.Parameters.Add("@BLformula", SqlDbType.NVarChar, 160).Value = textBox_businessFormula.Text;
        //            cmd.Parameters.Add("@inputscount", SqlDbType.Int).Value = numericUpDown_inputscount.Value;
        //            cmd.Parameters.Add("@inputs", SqlDbType.NVarChar, 160).Value = textBox_Inputs.Text;
        //            if (cnvvalue <= 0)
        //            {
        //                cmd.Parameters.Add("@conversionfac", SqlDbType.Int).Value = DBNull.Value;

        //            }

        //            else
        //            {
        //                cmd.Parameters.Add("@conversionfac", SqlDbType.Int).Value = cnvvalue;
        //            }

        //            if (comboBox_MacroDescription.Text == "")
        //            {
        //                cmd.Parameters.Add("@macrodesc", SqlDbType.Int).Value = DBNull.Value;

        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("@macrodesc", SqlDbType.Int).Value = comboBox_MacroDescription.Text;
        //            }
        //            cmd.Parameters.Add("@Dataseries", SqlDbType.NVarChar, 160).Value = textBox_DataseriesID.Text;
        //            cmd.Parameters.Add("@inputtimevalue", SqlDbType.NVarChar, 160).Value = combobox_inputTimeDimensionValue.Text;
        //            if (inputTimetype <= 0)
        //            {
        //                cmd.Parameters.Add("@inputTimetype", SqlDbType.Int).Value = DBNull.Value;

        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("@inputTimetype", SqlDbType.Int).Value = inputTimetype;
        //            }

        //            if (geographyID <= 0)
        //            {
        //                cmd.Parameters.Add("@inputgeoID", SqlDbType.Int).Value = DBNull.Value;

        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("@inputgeoID", SqlDbType.Int).Value = geographyID;
        //            }
        //            if (outputGeographyID <= 0)
        //            {
        //                cmd.Parameters.Add("@outputgeoID", SqlDbType.Int).Value = DBNull.Value;

        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("@outputgeoID", SqlDbType.Int).Value = outputGeographyID;
        //            }
                    
        //            if (inputUnitID <= 0)
        //            {
        //                cmd.Parameters.Add("@inputUnitID", SqlDbType.Int).Value = DBNull.Value;

        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("@inputUnitID", SqlDbType.Int).Value = inputUnitID;
        //            }
        //            cmd.Parameters.Add("@Businelogictype", SqlDbType.NVarChar, 160).Value = combobox_businesslogicType.Text;
        //            if (privacyID <= 0)
        //            {
        //                cmd.Parameters.Add("@privacyID", SqlDbType.Int).Value = DBNull.Value;

        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("@privacyID", SqlDbType.Int).Value = privacyID;
        //            }
                    
        //            cmd.Parameters.Add("@longDesc", SqlDbType.NVarChar, 160).Value = textBox_longDescription.Text;
        //            cmd.Parameters.Add("@Inputsource", SqlDbType.NVarChar, 160).Value = comboBox_InputSources.Text;
        //            if (sourceID <= 0)
        //                cmd.Parameters.Add("@InputsourceID", SqlDbType.Int, 160).Value = DBNull.Value;
        //            else
        //                cmd.Parameters.Add("@InputsourceID", SqlDbType.Int, 160).Value = sourceID;

        //            cmd.Parameters.Add("@outputname", SqlDbType.NVarChar, 160).Value = textBox_OutputName.Text;
        //            if (OutputUnitID <= 0)
        //            {
        //                cmd.Parameters.Add("@outputUnitID", SqlDbType.Int).Value = DBNull.Value;

        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("@outputUnitID", SqlDbType.Int).Value = OutputUnitID;
        //            }
        //            cmd.Parameters.Add("@outputDestination", SqlDbType.NVarChar, 160).Value = maxCVvalue;
        //            cmd.Parameters.Add("@outputTimevalue", SqlDbType.NVarChar, 160).Value = combobox_outputTimeDimensionValue.Text;
        //            if (outputTimetype <= 0)
        //            {
        //                cmd.Parameters.Add("@ouputTimeType", SqlDbType.Int).Value = DBNull.Value;

        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("@ouputTimeType", SqlDbType.Int).Value = outputTimetype;
        //            }
        //            cmd.Parameters.Add("@groupID", SqlDbType.Int).Value = groupID;
                        
                    
        //                int rows = cmd.ExecuteNonQuery();
        //                MessageBox.Show("New Business Rule is Inserted Successfully.");
        //                saveAction("New Business Rule is Inserted Successfully.");
        //                callingNull();
                    

        //        }

        //        else
        //        {
        //            return;
        //        }

        //    }

        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Data processing unsuccessful, please contact technical team.");
        //        //saveAction(ex.Message);
        //    }

        //    finally
        //    {
        //        con.Close();

        //    }

        //}


        //private void callingNull()
        //{
        //    comboBox_ConfacValue.SelectedItem = null;
        //    combobox_conversionDescription.SelectedItem = null;
        //    //comboBox_MacroValue.SelectedItem = null;
        //    comboBox_MacroDescription.SelectedItem = null;
        //    comboBox_IPtimeDType.SelectedItem = null;
        //    combobox_inputTimeDimensionValue.SelectedItem = null;
        //    combobox_outputTimeDimensionValue.SelectedItem = null;
        //    combobox_businesslogicType.SelectedItem = null;
        //    comboBox_OPtimeDtype.SelectedItem = null;
        //    comboBox_country.Text = null;
        //    comboBox_region.Text = null;
        //    comboBox_state.Text = null;
        //    comboBox_county.Text = null;
        //    combobox_outputCountry.Text = null;
        //    combobox_outputRegion.Text = null;
        //    combobox_outputState.Text = null;
        //    combobox_outputCounty.Text = null;
        //    comboBox_inputunit.SelectedItem = null;
        //    comboBox_privacylevel.SelectedItem = null;
        //    comboBox_InputSources.SelectedItem = null;
        //    comboBox_outputUnit.SelectedItem = null;
        //    comboBox_groupinvolved.SelectedItem = null;
        //    textBox_businessFormula.Text = Convert.ToString(0);
        //    textBox_Inputs.Text = "";
        //    combobox_inputTimeDimensionValue.Text = "";
        //    combobox_outputTimeDimensionValue.Text = "";
        //    textBox_businesslogicType.Text = "";
        //    textBox_DataseriesID.Text = "";
        //    textBox_OutputName.Text = "";
        //    textBox_longDescription.Text = "";
        //    numericUpDown_inputscount.Value = 0;
        //}
        
        private void button_back_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Parent = null;
        }

        //private void combobox_businesslogicType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (combobox_businesslogicType.Text == "Other")
        //    {
        //        textBox_businesslogicType.Visible = true;
        //    }

        //    else
        //    {
        //        textBox_businesslogicType.Visible = false;
        //    }
        //}

        //private void button_helpTowriteFormula_Click(object sender, EventArgs e)

        //{
        //    //Comment by Gowtham for testing
        //    //if (helpformula.IsDisposed == true)
        //    //{
        //    //    HelpFormula helpformula2 = new HelpFormula();
        //    //    helpformula2.Show();
        //    //    this.Close();
                
        //    //}

        //    //else
        //    //{

        //    //    helpformula.Show();
        //    //    this.Close();
                
        //    //}



        //    using (HelpFormula helpformula = new HelpFormula())
        //    {
        //        if (helpformula.ShowDialog() == DialogResult.OK)
        //        {
        //            helpformulainputs = helpformula.formulainputs;
        //            helpsampleformula = helpformula.sampleformula;
        //            helpformuladataseries = helpformula.dataseriesID;
        //            helpinputscount = helpformula.inputscount;
        //            helpformulafillmethod();

        //        }

        //        else
        //        {
        //            helpformulainputs = helpformula.formulainputs;
        //            helpsampleformula = helpformula.sampleformula;
        //            helpformuladataseries = helpformula.dataseriesID;
        //            helpinputscount = helpformula.inputscount;
        //            helpformulafillmethod();

        //        }
        //    }



        //}


        private void tableLayoutPanel_inputdetails_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        //private void comboBox_ConfacValue_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    ConversionsDescription = new List<string>();

        //    string sql = "";

        //    if (comboBox_ConfacValue.Text == null || comboBox_ConfacValue.Text.Equals(""))
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        sql = @"SELECT DISTINCT [ERSConversionFactor_Desc] FROM " + schemaName + "[ERSConversionFactors] WHERE [ERSConversionFactor_CF] = '" + comboBox_ConfacValue.Text + "'";

        //        System.Data.DataTable dt = GetData(sql);
        //        if (null != dt)
        //        {
        //            DataRow[] dr = dt.Select();
        //            ConversionsDescription.Add("");
        //            foreach (DataRow row in dr)
        //            {
        //                ConversionsDescription.Add(row["ERSConversionFactor_Desc"].ToString());
        //            }


        //            combobox_conversionDescription.DataSource = ConversionsDescription;
        //            combobox_conversionDescription.SelectedItem = null;
        //        }
        //    }
        //}

        //private void textBox_businessFormula_TextChanged(object sender, EventArgs e)
        //{
        //    //enable and disbale conversion factor and macro.
        //    disableConvMacro();
        //}

        private void comboBox_ConfacValue_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void combobox_conversionDescription_SelectedValueChanged(object sender, EventArgs e)
        {
            /*
                try
                {
                    ConversionsValue = new List<string>();

                    string sql = @"SELECT DISTINCT [ERSConversionFactor_CF] FROM " + schemaName + "[ERSConversionFactors] WHERE ERSConversionFactor_Desc = '" + combobox_conversionDescription.Text+"'";
                    DataTable dt = GetData(sql);
                    DataRow[] dr = dt.Select();
                    ConversionsValue.Add("");
                    foreach (DataRow row in dr)
                    {
                        ConversionsValue.Add(row["ERSConversionFactor_CF"].ToString());
                    }

                    comboBox_ConfacValue.DataSource = ConversionsValue;
                    comboBox_ConfacValue.SelectedItem = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                }
             */
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
            try
            {
                GetData(sql);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void combobox_conversionDescription_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void label_Path_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button_excelPath_Click(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }


        private void button2_Click(object sender, EventArgs e)
        {
            //enable sheet drop down
            comboBox_excelSheet.Enabled = true;

            comboBox_excelSheet.DataSource = null;
            sheetNames = new List<string>();
            // set filter to only allow excel files
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"; //only can chse excel files
            openFileDialog1.FileName = "BusinessRules_Upload.xls";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                filePath = openFileDialog1.FileName;
                textBox_excelPath.Text = filePath;
                Cursor.Current = Cursors.WaitCursor;
                object misValue = System.Reflection.Missing.Value;
                var oExcel = new Microsoft.Office.Interop.Excel.Application();
                //var workbook = oExcel.Workbooks;
                var oBook = oExcel.Workbooks.Open(filePath);
                //loads a book - above - and a sheet - below -
                foreach (Microsoft.Office.Interop.Excel.Worksheet worksheet in oBook.Worksheets)
                {
                    sheetNames.Add(worksheet.Name);
                }

                comboBox_excelSheet.DataSource = sheetNames;
                oBook.Close(false, misValue, misValue);
                oExcel.Quit();

                Cursor.Current = Cursors.Default;
            }
        }

        private int CheckForError()
        {
            Microsoft.Office.Interop.Excel.Range range = null;
            String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");

            int index = 0;

            var oExcel = new Microsoft.Office.Interop.Excel.Application();
            //var workbooks = oExcel.Workbooks;
            var oBook = oExcel.Workbooks.Open(filePath);

            index = 1 + comboBox_excelSheet.SelectedIndex;

            var sheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.get_Item(index);

            range = sheet.UsedRange;
            //clear junk in excel sheet
            sheet.Columns.ClearFormats();
            sheet.Rows.ClearFormats();
            //range.WrapText = true;
            object[,] values = (object[,])range.Value2;


            columnsCount = sheet.UsedRange.Columns.Count;

            rowsCount = sheet.UsedRange.Rows.Count;

            fullRow = sheet.Rows.Count;
            Lastrow = sheet.Cells[fullRow, 1].End(Excel.XlDirection.xlUp).Row;


            //progressBar1.Value = 0; //Progress bar
            //progressBar1.Maximum = Lastrow - 1;

            //progressBar1.Step = 1;

            int total = Lastrow - 1;

            //adding Error Deatails column.
            sheet.Cells[1, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
            sheet.Cells[1, columnsCount + 1] = "Error Details";


            string[] keywords = getKeywords();
            int checkonceN = 1;
            int checkonceCV = 1;
            countAllerrorMsg = 0;

            for (int i = 2; i <= Lastrow; i++)
            {

                //clear old messages
                errorMessage_BLUpload.Clear();
                sequenceIDNext = "";
                sequenceIDPrev = "";

                progressBar1.PerformStep();
                //check the value of status
                if (null != values[i, statusIndex] && !values[i, statusIndex].ToString().Trim().Equals(""))
                {
                    //get Status (New/Override)
                    String status = "";
                    if (null != values[i, statusIndex])
                        status = values[i, statusIndex].ToString().Trim();

                    //get Business Rule ID
                    string businessRuleID = "";
                    string prevRuleID = "";
                    string nextRuleID = "";
                    if (null != values[i, ruleIDIndex])
                        businessRuleID = values[i, ruleIDIndex].ToString();

                    if (businessRuleID.Equals("") && status.Equals("Override", StringComparison.InvariantCultureIgnoreCase))
                    {
                        sheet.Cells[i, ruleIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "-- Please enter business rule Id which needs to be updated";
                        errorMessage_BLUpload.Append("-- Please enter business rule Id which needs to be updated");
                        countAllerrorMsg = countAllerrorMsg + 1;
                        continue;
                    }
                    else if (!(status.Equals("New", StringComparison.InvariantCultureIgnoreCase) || status.Equals("Override", StringComparison.InvariantCultureIgnoreCase) ||
                    status.Equals("", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        sheet.Cells[i, statusIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append("--Incorrect format. Status field expected to be New/Override or blank.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                        continue;
                    }

                    //get previous rule ID and assign it to business rule id for further use
                    decimal sequenceID = 0.0M;
                    try
                    {
                        if (!businessRuleID.Equals("") && status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
                        {
                            //prevRuleID = businessRuleID.Split('&')[0].Trim();
                            nextRuleID = businessRuleID.Trim();



                            //get sequence ID                                
                            if (null != nextRuleID)
                            {
                                try
                                {
                                    //Get Previous rule id
                                    //next valid rule id
                                    string sqlprevRuleID = "SELECT [ERSBusinessLogic_ID] from  " + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_SequenceID] = (SELECT Max([ERSBusinessLogic_SequenceID]) AS ERSBusinessLogic_SequenceID FROM  " + schemaName + "[ERSBusinessLogic]  where ERSBusinessLogic_SequenceID < (SELECT [ERSBusinessLogic_SequenceID] AS ERSBusinessLogic_SequenceID FROM  " + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_ID] = " + nextRuleID + "))";
                                    System.Data.DataTable dtsqlprevRuleID = GetData(sqlprevRuleID);
                                    DataRow[] drsqlprevRuleID = dtsqlprevRuleID.Select();
                                    prevRuleID = drsqlprevRuleID[0]["ERSBusinessLogic_ID"].ToString();

                                    //get sequence
                                    //sequenceID = getSequenceID(prevRuleID, nextRuleID);
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.Equals("does not exist"))
                                    {
                                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                                        countAllerrorMsg = countAllerrorMsg + 1;
                                    }
                                    else
                                    {
                                        sheet.Cells[i, statusIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        errorMessage_BLUpload.Append(" -- Could not upload the BL Rule. Error occurred  in sequencing. Please contact technical team");
                                        countAllerrorMsg = countAllerrorMsg + 1;
                                    }

                                }

                            }
                        }
                        else if (businessRuleID.Equals("") && status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
                        {
                            try
                            {
                                sequenceID = getSequenceID("", "");
                            }
                            catch (Exception)
                            {
                                sheet.Cells[i, statusIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                errorMessage_BLUpload.Append(" -- Could not upload the BL Rule. Error occurred  in sequencing. Please contact technical team");
                                countAllerrorMsg = countAllerrorMsg + 1;
                            }
                        }
                        else if (!businessRuleID.Equals("") && status.Equals("Override", StringComparison.InvariantCultureIgnoreCase))
                        {
                            prevRuleID = businessRuleID;
                        }

                    }
                    catch (Exception)
                    {
                        errorMessage_BLUpload.Append(" -- Please enter correct Import/Export Rule ID");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //get Commodity Series ID
                    string commoditySeriesID = "";
                    if (null != values[i, commoditySeriesIDIndex])
                        commoditySeriesID = values[i, commoditySeriesIDIndex].ToString();


                    Match matchcommoditySeriesID = Regex.Match(commoditySeriesID.Replace(" ", ""), @"^[0-9;CVN]*$");

                    if (!matchcommoditySeriesID.Success)
                    {
                        errorMessage_BLUpload.Append(" -- Please enter Commodity Series ID in correct format. Only digits, CV, N and ';' is allowed.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }


                    //if (!System.Text.RegularExpressions.Regex.IsMatch(commoditySeriesID.Replace(" ",""), "^\\d*[;]*[C]*[V]*[N]*\\d*$"))
                    //{
                    //    errorMessage_BLUpload.Append(" -- Please enter Commodity Series ID in correct format. Only numbers and ';' is allowed.");
                    //}

                    //get inputs count
                    //int inputsCount = 0;
                    string[] commoditySeriesArray = { };
                    try
                    {
                        commoditySeriesArray = commoditySeriesID.Replace(" ", string.Empty).Split(';');
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, commoditySeriesIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        errorMessage_BLUpload.Append(" -- Please use only ';'(Semi-Colon) to seperate Commodity Series IDs");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    string outputDestinationval = "";
                    if (null != values[i, outputDestinationIndex])
                        outputDestinationval = values[i, outputDestinationIndex].ToString().Trim();
                    else
                    {
                        errorMessage_BLUpload.Append("Please enter output destination");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }



                    //get thousandNumber for CV and N
                    // int thousandNumber=0;
                    int thousandNumber;

                    try
                    {
                        //  thousandNumber = getThoudandNumber(prevRuleID, status, commoditySeriesArray);

                        thousandNumber = getThousandNumberForerror(prevRuleID, status, commoditySeriesArray, outputDestinationval, checkonceN, checkonceCV);
                        if (outputDestinationval.Contains("N"))
                        {
                            checkonceN = 2;
                            //  checkonceCV = 0;
                        }
                        else
                        {
                            checkonceCV = 2;
                            //  checkonceN = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        sheet.Cells[i, businessRuleID].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        if (ex.Message.Contains("Businees Rule ID"))
                            errorMessage_BLUpload.Append(" -- " + ex.Message);
                        errorMessage_BLUpload.Append(" -- Could not find correct thousand number. Please check the Business Rule ID and Output destination are correct.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                        thousandNumber = 0;
                    }

                    //get Output Destination
                    string outputDestination = "";
                    try
                    {
                        if (null != values[i, outputDestinationIndex])
                            outputDestination = values[i, outputDestinationIndex].ToString().Trim();
                        else
                        {
                            errorMessage_BLUpload.Append("Please enter output destination");
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                        int outputDestinationNumber = Convert.ToInt32(Regex.Match(outputDestination, @"\d+").Value);
                        outputDestination = getOutputDestination(status, prevRuleID, outputDestination, thousandNumber, outputDestinationNumber);
                    }
                    catch (Exception ex)
                    {
                        sheet.Cells[i, outputDestinationIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //get formula
                    string formula = "";
                    if (null != values[i, formulaIndex])
                        formula = values[i, formulaIndex].ToString();
                    else
                    {
                        errorMessage_BLUpload.Append("Please enter formula");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }
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

                    try
                    {

                        formula = getFormula(formula, commoditySeriesArray, thousandNumber, keywords);
                    }
                    catch (Exception ex)
                    {
                        sheet.Cells[i, formulaIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        if (ex.Message.Contains("Brackets"))
                        {
                            errorMessage_BLUpload.Append(" -- " + ex.Message);
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                        else if (ex.Message.Contains("keyword"))
                        {
                            errorMessage_BLUpload.Append(" -- " + ex.Message);
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                        else if (ex.Message.Contains("New data series"))
                        {
                            errorMessage_BLUpload.Append(" -- " + ex.Message);
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                        else if (ex.Message.Contains("Data series"))
                        {
                            errorMessage_BLUpload.Append(" -- " + ex.Message);
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                        else if (ex.Message.Contains("not used in formula"))
                        {
                            errorMessage_BLUpload.Append(" -- " + ex.Message);
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                        else
                        {
                            errorMessage_BLUpload.Append(" -- Entered formula is not correct.");
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                    }


                    //get inputs
                    string inputs = "";
                    try
                    {
                        inputs = getInputs(inputsArray, thousandNumber);
                    }
                    catch (Exception ex)
                    {
                        sheet.Cells[i, commoditySeriesIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        if (ex.Message.Contains("New data series"))
                        {
                            errorMessage_BLUpload.Append(" -- " + ex.Message);
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                        else if (ex.Message.Contains("make sure"))
                        {
                            errorMessage_BLUpload.Append(" -- " + ex.Message);
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                        else
                        {
                            errorMessage_BLUpload.Append(" -- One of the input data series does not exist.");
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                    }

                    //Get input data series ID
                    try
                    {
                        commoditySeriesID = getInputDataSeriesID(inputs);
                    }
                    catch (Exception ex)
                    {
                        sheet.Cells[i, commoditySeriesIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        errorMessage_BLUpload.Append(" -- Please use only ';'(Semi-Colon) to seperate Commodity Series IDs. Make sure the CVs and Ns are in correct format. For example 'CV1' or 'N1'");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }


                    //get Group ID
                    string groupID = "";
                    try
                    {
                        groupID = getGroupId(commoditySeriesID, sequenceID);
                    }
                    catch (Exception ex)
                    {
                        sheet.Cells[i, commoditySeriesIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        errorMessage_BLUpload.Append("Could not find a data series. Please make sure all the input data series IDs are correct");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    // get Input unit
                    string inputUnit = "";
                    if (null != values[i, inputUnitIndex])
                        inputUnit = values[i, inputUnitIndex].ToString();
                    else
                    {
                        errorMessage_BLUpload.Append("Please enter Input Unit");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }
                    string InputUnitIDs = "";
                    try
                    {
                        InputUnitIDs = getUnitID(inputUnit.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, inputUnitIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";

                        if (inputUnit.Trim().Equals("1000"))
                        {
                            errorMessage_BLUpload.Append(" -- The Input Unit is not correct. If you meant '$1,000' then change the excel cell format from Currency/Number to text.");
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                        else
                        {
                            errorMessage_BLUpload.Append(" -- Please make sure the input Unit is correct. Use ';'(Semi-Colon) to seperate multiple Units");
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }

                    }

                    // get macro
                    string macro = "";
                    if (null != values[i, macroIndex])
                    {

                        try
                        {
                            if (getMacro(values[i, macroIndex].ToString().Trim()).Equals("Found"))
                            {
                                macro = values[i, macroIndex].ToString().Trim();
                            }

                        }
                        catch (Exception ex)
                        {
                            sheet.Cells[i, macroIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                            errorMessage_BLUpload.Append(" -- " + ex.Message);
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                    }
                    else
                        macro = "NULL";

                    // get conversion factor
                    string conversionFactor = "";
                    string conversionFactorID = "";
                    if (null != values[i, conversionFactorIndex])
                    {
                        conversionFactor = values[i, conversionFactorIndex].ToString();
                        try
                        {
                            conversionFactorID = getConversionFactorID(conversionFactor.Trim());
                        }
                        catch (Exception)
                        {
                            sheet.Cells[i, conversionFactorIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                            //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                            errorMessage_BLUpload.Append(" -- Please make sure the conversion factor is correct.");
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                    }
                    else if (null == values[i, conversionFactorIndex] && formula.Contains("conv"))
                    {
                        sheet.Cells[i, conversionFactorIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please provide the value of conversion factor.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }
                    else if (null == values[i, macroIndex] && formula.Contains("conv"))
                    {
                        sheet.Cells[i, macroIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please provide the macro description.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }
                    else
                        conversionFactorID = "NULL";


                    //get output name
                    string outputName = "";
                    if (values[i, outputNameIndex] != null)
                    {
                        outputName = values[i, outputNameIndex].ToString();
                    }
                    else
                    {
                        errorMessage_BLUpload.Append("Please enter Output Name");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }
                    //get output unit
                    string outputUnit = "";
                    if (null != values[i, outputUnitIndex])
                        outputUnit = values[i, outputUnitIndex].ToString();
                    else
                    {
                        errorMessage_BLUpload.Append("Please enter Output Unit");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }
                    string outputUnitID = "";
                    try
                    {
                        outputUnitID = getUnitID(outputUnit.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, outputUnitIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please make sure the output Unit is correct.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }


                    //get Input Time Dimension Type
                    string inputTimeDimensionType = "";
                    if (null != values[i, inputTimeDimensionTypeIndex])
                        inputTimeDimensionType = values[i, inputTimeDimensionTypeIndex].ToString();
                    string inputTimeDimensionTypeID = "";
                    try
                    {
                        inputTimeDimensionTypeID = getTimeDimensionTypeID(inputTimeDimensionType.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, inputTimeDimensionTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please make sure the Input Time Dimension Type is correct. Valid Inputs are Month, Year and Marketing Year");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //get Input Time Value
                    string inputTimeDimensionTypeValue = "";
                    if (null != values[i, inputTimeValueIndex])
                    {
                        inputTimeDimensionTypeValue = values[i, inputTimeValueIndex].ToString();
                    }
                    else
                    {
                        sheet.Cells[i, inputTimeValueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please make sure the Input Time Value is correct");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //get Output Time Dimension Type
                    string outputTimeDimensionType = "";
                    if (null != values[i, outputTimeDimensionTypeIndex])
                        outputTimeDimensionType = values[i, outputTimeDimensionTypeIndex].ToString();
                    string outputTimeDimensionTypeID = "";
                    try
                    {
                        outputTimeDimensionTypeID = getTimeDimensionTypeID(outputTimeDimensionType.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, outputTimeDimensionTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please make sure the Output Time Dimension Type is correct.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //get Output Time Value
                    string outputTimeDimensionTypeValue = "";
                    if (null != values[i, outputTimeValueIndex])
                    {
                        outputTimeDimensionTypeValue = values[i, outputTimeValueIndex].ToString();
                    }
                    else
                    {
                        sheet.Cells[i, outputTimeValueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please make sure the Output Time Value is correct");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //get Input Geo Type and value
                    string inputgeoType = "";
                    string inputgeoValue = "";
                    if (null != values[i, inputGeoTypeIndex])
                        inputgeoType = values[i, inputGeoTypeIndex].ToString().Trim();
                    if (null != values[i, inputGeoValueIndex])
                        inputgeoValue = values[i, inputGeoValueIndex].ToString().Trim();
                    string inputgeoTypeID = "";
                    string inputgeoValueID = "";
                    try
                    {
                        inputgeoTypeID = getGeoTypeID(inputgeoType);

                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, inputGeoTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please enter a valid input Geo Type like Country, State or Region.");
                        countAllerrorMsg = countAllerrorMsg + 1;

                    }
                    try
                    {
                        inputgeoValueID = getGeoValueID(inputgeoTypeID, inputgeoValue);

                    }
                    catch (Exception ex)
                    {
                        sheet.Cells[i, inputGeoValueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        if (ex.Message.Contains("same number") || ex.Message.Contains("correct Geogrphy"))
                        {
                            errorMessage_BLUpload.Append(ex.Message);
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                        else
                        {
                            errorMessage_BLUpload.Append(" -- Please enter a valid input Geo Value for given Geo Type.");
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }

                    }

                    //get Output Geo Type and value
                    string outputgeoType = "";
                    string outputgeoValue = "";
                    if (null != values[i, outputGeoTypeIndex])
                        outputgeoType = values[i, outputGeoTypeIndex].ToString().Trim();
                    if (null != values[i, outputGeoValueIndex])
                        outputgeoValue = values[i, outputGeoValueIndex].ToString().Trim();
                    string outputgeoTypeID = "";
                    string outputgeoValueID = "";
                    try
                    {
                        outputgeoTypeID = getGeoTypeID(outputgeoType);

                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, outputGeoTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please enter a valid output Geo Type like Country, State or Region.");
                        countAllerrorMsg = countAllerrorMsg + 1;

                    }
                    try
                    {
                        outputgeoValueID = getGeoValueID(outputgeoTypeID, outputgeoValue);

                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, outputGeoValueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please enter a valid output Geo Value for given Geo Type.");
                        countAllerrorMsg = countAllerrorMsg + 1;

                    }

                    //get privacy
                    string privacy = "";
                    if (null != values[i, privacyIndex])
                        privacy = values[i, privacyIndex].ToString();
                    string privacyID = "";

                    try
                    {
                        privacyID = getPrivacy(privacy.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, privacyIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please enter a valid privacy type like Private or Public.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //get BLType
                    string BLType = "";
                    if (null != values[i, typeIndex])
                    {
                        BLType = values[i, typeIndex].ToString();
                    }
                    else
                    {
                        sheet.Cells[i, typeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please enter a valid Business Logic Type.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //get Long Description
                    string longDescription = "";
                    if (null != values[i, longDescriptionIndex])
                    {
                        longDescription = values[i, longDescriptionIndex].ToString();
                    }
                    else
                    {
                        sheet.Cells[i, longDescriptionIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append(" -- Please enter a valid long description.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //get Input Sources
                    string inputSources = "";
                    if (null != values[i, inputSourcesIndex])
                        inputSources = values[i, inputSourcesIndex].ToString();
                    string inputSourcesID = "";
                    try
                    {
                        inputSourcesID = getInputSourcesID(inputSources, outputName);

                    }
                    catch (Exception ex)
                    {
                        sheet.Cells[i, inputSourcesIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        if (ex.Message.Contains("not found")) ;
                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                        errorMessage_BLUpload.Append(" -- Source '" + inputSources + "' not found. Please enter a correct source description.");
                        countAllerrorMsg = countAllerrorMsg + 1;

                    }


                    string duplicate = DuplicateCheckBR(commoditySeriesID, inputsCount, inputs, InputUnitIDs, conversionFactorID, macro, outputName, outputUnitID,
                        formula, inputTimeDimensionTypeValue, inputTimeDimensionTypeID, outputTimeDimensionTypeValue, outputTimeDimensionTypeID, inputgeoValueID,
                        outputgeoValueID, outputDestination, privacyID, BLType, longDescription, inputSources, inputSourcesID, groupID);

                    if (duplicate != "0")
                    {
                        //for (int j = 3; j <= columnsCount; j++)
                        //{
                        sheet.Cells[i, outputNameIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        //}
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_BLUpload.Append("--Output name exists in database. Please choose a unique one.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //Savign error messages in sheet
                    sheet.Cells[i, columnsCount + 1] = errorMessage_BLUpload.ToString();

                    string errorstatus = errorMessage_BLUpload.ToString();

                    if (errorstatus == "")
                    {
                        for (int k = 1; k <= columnsCount; k++)
                            sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "No errors";
                    }
                }
            }

            int result = countAllerrorMsg;
            if (result > 0)
            {
                // first check if target directory exist
                // create one if not exist
                if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Business Rules")) // name of the error file and location
                {
                    Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Business Rules");
                }

                string fileName = cosdFolderPath + "CoSD Tool\\Business Rules\\BusinessRules_Upload_" + Date + ".xls";
                try
                {
                    oBook.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    oBook.Close();
                    oExcel.Quit();

                    //if (successNum == total)
                    //{
                    //    if (total == 1)
                    //        label_status.Text = total + " record(s) uploaded to the database. Please check " + fileName + " for details.";
                    //    label_status.Text = total + " record(s) uploaded to the database. Please check " + fileName + " for details.";
                    //}
                    //else
                    label_status.Text = successNum + " record(s) uploaded. Please check " + fileName + " for any failed records.";
                    progressBar1.Step = Lastrow * 2;
                    progressBar1.PerformStep();
                    successNum = 0;
                    total = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to create result excel file. Please contact technical team\nError Details" + ex.Message);
                    //MessageBox.Show("Could Not save result file" + ex.Message);
                }

            }

            return result;
        }


        private int CheckForErrorNS()
        {

            countAllerrorMsg = 0;

            Microsoft.Office.Interop.Excel.Range range = null;
            String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");

            int index = 0;

            var oExcel = new Microsoft.Office.Interop.Excel.Application();
            //var workbooks = oExcel.Workbooks;
            var oBook = oExcel.Workbooks.Open(filePathNS);

            index = 1 + comboBox_excelSheet1.SelectedIndex;

            var sheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.get_Item(index);

            range = sheet.UsedRange;
            //clear junk in excel sheet
            sheet.Columns.ClearFormats();
            sheet.Rows.ClearFormats();
            //range.WrapText = true;
            object[,] values = (object[,])range.Value2;


            columnsCount = sheet.UsedRange.Columns.Count;

            rowsCount = sheet.UsedRange.Rows.Count;

            fullRow = sheet.Rows.Count;
            Lastrow = sheet.Cells[fullRow, 1].End(Excel.XlDirection.xlUp).Row;


            //progressBar2.Value = 1; //Progress bar
            //progressBar2.Maximum = (Lastrow - 1)*2;

            // progressBar2.Step = 1;

            int total = Lastrow - 1;

            //adding Error Deatails column.
            sheet.Cells[1, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
            sheet.Cells[1, columnsCount + 1] = "Error Details";

            //Store list of valid keywords for formula from CoSD Keywords.TXT
            string[] keywords = getKeywords();


            int checkonceN = 1;

            for (int i = 2; i <= Lastrow; i++)
            {

                //clear old messages
                errorMessage_NDSUpload.Clear();
                progressBar2.PerformStep();
                //progressBar2.Increment(1);


                //check the value of status
                if (null != values[i, statusIndexNS])
                {

                    //get new id 

                    string newid;
                    if (values[i, newidIndexNS] == null)
                        newid = "";
                    else
                        newid = values[i, newidIndexNS].ToString().Trim();


                    //get new data series id

                    string newDSid;
                    if (values[i, NewDataseriesIndexNS] == null)
                        newDSid = "";
                    else
                        newDSid = values[i, NewDataseriesIndexNS].ToString().Trim();


                    //get status 

                    string statusNS;
                    if (values[i, statusIndexNS] == null)
                        statusNS = "";
                    else
                        statusNS = values[i, statusIndexNS].ToString().Trim();


                    if (statusNS == "Override" && newDSid == "")
                    {
                        sheet.Cells[i, NewDataseriesIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Please input data series id to override");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }
                    else if (statusNS == "Override" && newDSid != "")
                    {
                        string checknsid = getNewDataSeriesID(newDSid);                //checks if the overriding data series id exists in table

                        if (checknsid == "0")
                        {
                            sheet.Cells[i, NewDataseriesIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                            sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                            errorMessage_NDSUpload.Append("--Please input correct data series id to override. Data series id not found.");
                            countAllerrorMsg = countAllerrorMsg + 1;
                        }
                    }
                    else if (statusNS == "New" && newDSid != "")
                    {
                        sheet.Cells[i, NewDataseriesIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Incorrect format. Input data series id field expected to be blank.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }
                    else if (!(statusNS.Trim() == "New" || statusNS.Trim() == "Override" || statusNS.Trim() == ""))
                    {
                        sheet.Cells[i, statusIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Incorrect format. Status field expected to be New/Override or blank.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }



                    // get unit
                    string unit = "";
                    if (null != values[i, unitIndexNS])
                        unit = values[i, unitIndexNS].ToString();
                    string unitID = "";
                    try
                    {
                        unitID = getUnitID(unit.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, unitIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Error in Input unit.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //


                    //get group id

                    string group = "";
                    if (null != values[i, groupIndexNS])
                        group = values[i, groupIndexNS].ToString();
                    string groupID = "";
                    try
                    {
                        groupID = getgroupIDfromdesc(group.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, groupIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Error in Group id.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }



                    // get sub commodity id
                    string commodityName = "";
                    if (null != values[i, subcommodityIndexNS])
                        commodityName = values[i, subcommodityIndexNS].ToString();
                    string commodityID = "";
                    try
                    {
                        commodityID = getCommodityID(commodityName.Trim(), groupID);
                    }
                    catch (Exception)
                    {

                        sheet.Cells[i, subcommodityIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Error in commodity id.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    // get physical attr id
                    string phyattr = "";
                    if (null != values[i, physicalAtttypeIndexNS])
                        phyattr = values[i, physicalAtttypeIndexNS].ToString();
                    string phyattrid = "";
                    try
                    {
                        phyattrid = getPhyAttrCatID(phyattr.Trim());
                    }
                    catch (Exception)
                    {

                        sheet.Cells[i, physicalAtttypeIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Error in physical attribute.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    // get physical attr desc
                    string phyAttrDesc;
                    if (values[i, physicalAttDescIndexNS] == null)
                        phyAttrDesc = "";
                    //else if (values[i, physicalAttributeIndex] != null && values[i, physicalAttributeIndex].Equals("No Physical Attribute"))
                    //    phyAttrDesc = "";
                    else
                        phyAttrDesc = values[i, physicalAttDescIndexNS].ToString().Trim();

                    // get prod practice
                    string prodPrac = "";
                    if (null != values[i, prodpracIndexNS])
                        prodPrac = values[i, prodpracIndexNS].ToString();
                    string prodPracID = "";
                    try
                    {
                        prodPracID = getProdPracID(prodPrac.Trim());
                    }
                    catch (Exception)
                    {

                        sheet.Cells[i, prodpracIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Error in production practice.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }


                    // get util practice
                    string utilPrac = "";
                    if (null != values[i, utilpracIndexNS])
                        utilPrac = values[i, utilpracIndexNS].ToString();
                    string utilPracID = "";
                    try
                    {
                        utilPracID = getUtilpracID(utilPrac.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, utilpracIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Error in util practice.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }
                    // get stat type
                    string statType = "";
                    if (null != values[i, stattypeIndexNS])
                        statType = values[i, stattypeIndexNS].ToString();
                    string statTypeID = "";
                    try
                    {
                        statTypeID = getStatTypeID(statType.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, stattypeIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Error in stat type.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    // get source
                    string sourceDesc = "";

                    if (null != values[i, sourceIndexNS])
                        sourceDesc = values[i, sourceIndexNS].ToString();


                    string sourceID = "";
                    try
                    {
                        sourceID = getSourceID(sourceDesc.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, sourceIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Error in source id.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }

                    //get source long desc

                    string sourceLongDesc = "";
                    if (null != values[i, sourceseriesLongDescIndexNS])
                        sourceLongDesc = values[i, sourceseriesLongDescIndexNS].ToString();


                    // get sector id

                    string sector = "";
                    if (null != values[i, sectorIndexNS])
                        sector = values[i, sectorIndexNS].ToString();
                    string sectorID = "";
                    try
                    {
                        sectorID = getSectorID(sector.Trim());
                    }
                    catch (Exception)
                    {
                        sheet.Cells[i, sectorIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Error in sector id.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }


                    // int thousandNumber;

                    // thousandNumber = getThousandForNewDataSeries(newDSid, statusNS, checkonceN);

                    //progressBar2.PerformStep();

                    string commodityseries = "";
                    if (null != values[i, commoditySeriesIDIndexNS])
                        commodityseries = values[i, commoditySeriesIDIndexNS].ToString();
                    string[] commodities = commodityseries.Split(';');
                    string commodityid = "";
                    int countcatch = 0;


                    foreach (var c in commodities)
                    {
                        try
                        {
                            commodityid = getCommoditySeriesID(c);

                        }
                        catch
                        {
                            countcatch = countcatch + 1;
                        }
                    }


                    if (countcatch > 0)
                    {
                        sheet.Cells[i, commoditySeriesIDIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Error in commodity series.");       //Please use only ';'(Semi-Colon) to seperate Commodity Series IDs
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }


                    // get category desc
                    string categoryDesc;
                    if (values[i, dataseriesdescrIndexNS] == null)
                        categoryDesc = "";
                    else
                        categoryDesc = values[i, dataseriesdescrIndexNS].ToString().Trim();


                    string duplicate = DuplicateCheck(newid, unitID, commodityID, phyattrid, phyAttrDesc, prodPracID, utilPracID, statTypeID, sourceID, sourceLongDesc, sectorID, groupID, categoryDesc);

                    if (duplicate != "0")
                    {
                        for (int j = 3; j <= columnsCount; j++)
                        {
                            sheet.Cells[i, j].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                        }
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                        errorMessage_NDSUpload.Append("--Duplicate row.");
                        countAllerrorMsg = countAllerrorMsg + 1;
                    }


                    //Saving error messages in sheet
                    sheet.Cells[i, columnsCount + 1] = errorMessage_NDSUpload.ToString();

                    string errorstatus = errorMessage_NDSUpload.ToString();

                    if (errorstatus == "")
                    {
                        for (int k = 1; k <= columnsCount; k++)
                            sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[i, columnsCount + 1] = "No errors";

                    }

                }

            }


            int result = countAllerrorMsg;
            if (result > 0)
            {

                // first check if target directory exist
                // create one if not exist
                if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Business Rules")) // name of the error file and location
                {
                    Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Business Rules");
                }

                string fileName = cosdFolderPath + "CoSD Tool\\Business Rules\\NewDataSeries_Upload_" + Date + ".xls";
                try
                {
                    oBook.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                    false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    oBook.Close();
                    oExcel.Quit();

                    //if (successNum == total)
                    //{
                    //    if (total == 1)
                    //        label_status1.Text = total + " record(s) uploaded to the database. Please check " + fileName + " for details.";
                    //    label_status1.Text = total + " record(s) uploaded to the database. Please check " + fileName + " for details.";
                    //}
                    //else
                    label_status1.Text = successNum + " record(s) uploaded. Please check " + fileName + " for any failed records.";
                    progressBar2.Step = Lastrow * 2;
                    progressBar2.PerformStep();
                    successNum = 0;
                    total = 0;
                    textBox_newdataseries.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to create result excel file. Please contact technical team\nError Details" + ex.Message);
                    //MessageBox.Show("Could Not save result file" + ex.Message);
                }

            }


            return result;
        }


        private void button_importExcel_Click(object sender, EventArgs e)
        {


            if (filePath == null || filePath == "")
            {
                MessageBox.Show("Please choose an Excel file to Upload.");
                return;
            }
            if (comboBox_excelSheet.Text == "")
            {
                MessageBox.Show("Please choose a sheet to Upload.");
                return;
            }

            // ask for conformation before proceed
            if (MessageBox.Show("Uploading excel may take few minutes to complete. Do you really want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    label_status.Text = "Uploading ...";

                    progressBar1.Value = 0; //Progress bar

                    Microsoft.Office.Interop.Excel.Range range = null;
                    String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");

                    int index = 0;

                    var oExcel = new Microsoft.Office.Interop.Excel.Application();
                    //var workbooks = oExcel.Workbooks;
                    var oBook = oExcel.Workbooks.Open(filePath);

                    index = 1 + comboBox_excelSheet.SelectedIndex;

                    var sheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.get_Item(index);

                    range = sheet.UsedRange;
                    //clear junk in excel sheet
                    sheet.Columns.ClearFormats();
                    sheet.Rows.ClearFormats();
                    //range.WrapText = true;
                    object[,] values = (object[,])range.Value2;


                    columnsCount = sheet.UsedRange.Columns.Count;

                    rowsCount = sheet.UsedRange.Rows.Count;

                    fullRow = sheet.Rows.Count;
                    Lastrow = sheet.Cells[fullRow, 1].End(Excel.XlDirection.xlUp).Row;



                    progressBar1.Maximum = (Lastrow - 1) * 2;

                    progressBar1.Step = 1;

                    int total = Lastrow - 1;


                    if (!validExcelFormat(values))
                    {
                        MessageBox.Show("Please check the format of selected Excel file.\nPlease refer to Business rules upload sheet in COSD folder for correct format.");
                        label_status.Text = "Upload failed. Please check the format of selected Excel file.";
                        oBook.Close(false, Type.Missing, Type.Missing);
                        oExcel.Quit();
                        //Marshal.ReleaseComObject(oBook);
                        //Marshal.ReleaseComObject(workbooks);
                        //Marshal.ReleaseComObject(oExcel);

                        return;
                    }

                    //adding Error Deatails column.
                    sheet.Cells[1, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    sheet.Cells[1, columnsCount + 1] = "Error Details";

                    //Store list of valid keywords for formula from CoSD Keywords.TXT
                    string[] keywords = getKeywords();
                    int checkonceN = 1;
                    int checkonceCV = 1;

                    int check = CheckForError();

                    checkonceN = 1;
                    checkonceCV = 1;

                    if (check == 0)
                    {
                        for (int i = 2; i <= Lastrow; i++)
                        {

                            //clear old messages
                            errorMessage_BLUpload.Clear();
                            sequenceIDNext = "";
                            sequenceIDPrev = "";

                            progressBar1.PerformStep();
                            //check the value of status
                            if (null != values[i, statusIndex] && !values[i, statusIndex].ToString().Trim().Equals(""))
                            {
                                //get Status (New/Override)
                                String status = "";
                                if (null != values[i, statusIndex])
                                    status = values[i, statusIndex].ToString().Trim();

                                //get Business Rule ID
                                string businessRuleID = "";
                                string prevRuleID = "";
                                string nextRuleID = "";
                                if (null != values[i, ruleIDIndex])
                                    businessRuleID = values[i, ruleIDIndex].ToString();
                                //if (!businessRuleID.Equals("") && !businessRuleID.Contains("&") && status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
                                //{
                                //    sheet.Cells[i, ruleIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                //    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                //    errorMessage_BLUpload.Append("-> Please enter business rule Ids between which this rule needs to be inserted. For example '100 and 101'");
                                //}
                                if (businessRuleID.Equals("") && status.Equals("Override", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    sheet.Cells[i, ruleIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "-- Please enter business rule Id which needs to be updated";
                                    //errorMessage_BLUpload.Append("-- Please enter business rule Id which needs to be updated");
                                    continue;
                                }

                                //get previous rule ID and assign it to business rule id for further use
                                decimal sequenceID = 0.0M;
                                try
                                {
                                    if (!businessRuleID.Equals("") && status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        //prevRuleID = businessRuleID.Split('&')[0].Trim();
                                        nextRuleID = businessRuleID.Trim();



                                        //get sequence ID                                
                                        if (null != nextRuleID)
                                        {
                                            try
                                            {
                                                //Get Previous rule id
                                                //next valid rule id
                                                string sqlprevRuleID = "SELECT [ERSBusinessLogic_ID] from  " + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_SequenceID] = (SELECT Max([ERSBusinessLogic_SequenceID]) AS ERSBusinessLogic_SequenceID FROM  " + schemaName + "[ERSBusinessLogic]  where ERSBusinessLogic_SequenceID < (SELECT [ERSBusinessLogic_SequenceID] AS ERSBusinessLogic_SequenceID FROM  " + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_ID] = " + nextRuleID + "))";
                                                System.Data.DataTable dtsqlprevRuleID = GetData(sqlprevRuleID);
                                                DataRow[] drsqlprevRuleID = dtsqlprevRuleID.Select();
                                                prevRuleID = drsqlprevRuleID[0]["ERSBusinessLogic_ID"].ToString();

                                                //get sequence
                                                sequenceID = getSequenceID(prevRuleID, nextRuleID);
                                            }
                                            catch (Exception ex)
                                            {
                                                if (ex.Message.Equals("does not exist"))
                                                    errorMessage_BLUpload.Append(" -- " + ex.Message);
                                                else
                                                {
                                                    sheet.Cells[i, statusIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                                    errorMessage_BLUpload.Append(" -- Could not upload the BL Rule. Error occurred  in sequencing. Please contact technical team");
                                                }

                                            }

                                        }
                                    }
                                    else if (businessRuleID.Equals("") && status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        try
                                        {
                                            sequenceID = getSequenceID("", "");
                                        }
                                        catch (Exception)
                                        {
                                            sheet.Cells[i, statusIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            errorMessage_BLUpload.Append(" -- Could not upload the BL Rule. Error occurred  in sequencing. Please contact technical team");
                                        }
                                    }
                                    else if (!businessRuleID.Equals("") && status.Equals("Override", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        prevRuleID = businessRuleID;
                                    }

                                }
                                catch (Exception)
                                {
                                    errorMessage_BLUpload.Append(" -- Please enter correct Import/Export Rule ID");
                                }

                                //get Commodity Series ID
                                string commoditySeriesID = "";
                                if (null != values[i, commoditySeriesIDIndex])
                                    commoditySeriesID = values[i, commoditySeriesIDIndex].ToString();


                                Match matchcommoditySeriesID = Regex.Match(commoditySeriesID.Replace(" ", ""), @"^[0-9;CVN]*$");

                                if (!matchcommoditySeriesID.Success)
                                {
                                    errorMessage_BLUpload.Append(" -- Please enter Commodity Series ID in correct format. Only digits, CV, N and ';' is allowed.");
                                }


                                //if (!System.Text.RegularExpressions.Regex.IsMatch(commoditySeriesID.Replace(" ",""), "^\\d*[;]*[C]*[V]*[N]*\\d*$"))
                                //{
                                //    errorMessage_BLUpload.Append(" -- Please enter Commodity Series ID in correct format. Only numbers and ';' is allowed.");
                                //}

                                //get inputs count
                                //int inputsCount = 0;
                                string[] commoditySeriesArray = { };
                                try
                                {
                                    commoditySeriesArray = commoditySeriesID.Replace(" ", string.Empty).Split(';');
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, commoditySeriesIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    errorMessage_BLUpload.Append(" -- Please use only ';'(Semi-Colon) to seperate Commodity Series IDs");
                                }

                                string outputDestinationval = "";
                                if (null != values[i, outputDestinationIndex])
                                    outputDestinationval = values[i, outputDestinationIndex].ToString().Trim();
                                else
                                {
                                    errorMessage_BLUpload.Append("Please enter output destination");
                                }



                                //get thousandNumber for CV and N
                                // int thousandNumber=0;
                                int thousandNumber;


                                try
                                {
                                    //  thousandNumber = getThoudandNumber(prevRuleID, status, commoditySeriesArray);

                                    thousandNumber = getThoudandNumber(prevRuleID, status, commoditySeriesArray, outputDestinationval, checkonceN, checkonceCV);
                                    if (outputDestinationval.Contains("N"))
                                    {
                                        checkonceN = 2;
                                        // checkonceCV = 0;
                                    }
                                    else
                                    {
                                        checkonceCV = 2;
                                        // checkonceN = 0;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    sheet.Cells[i, businessRuleID].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    if (ex.Message.Contains("Businees Rule ID"))
                                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                                    errorMessage_BLUpload.Append(" -- Could not find correct thousand number. Please check the Business Rule ID and Output destination are correct.");
                                    thousandNumber = 0;
                                }

                                //get Output Destination
                                string outputDestination = "";
                                try
                                {
                                    if (null != values[i, outputDestinationIndex])
                                        outputDestination = values[i, outputDestinationIndex].ToString().Trim();
                                    else
                                    {
                                        errorMessage_BLUpload.Append("Please enter output destination");
                                    }
                                    int outputDestinationNumber = Convert.ToInt32(Regex.Match(outputDestination, @"\d+").Value);
                                    outputDestination = getOutputDestination(status, prevRuleID, outputDestination, thousandNumber, outputDestinationNumber);
                                }
                                catch (Exception ex)
                                {
                                    sheet.Cells[i, outputDestinationIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    errorMessage_BLUpload.Append(" -- " + ex.Message);
                                }

                                //get formula
                                string formula = "";
                                if (null != values[i, formulaIndex])
                                    formula = values[i, formulaIndex].ToString();
                                else
                                {
                                    errorMessage_BLUpload.Append("Please enter formula");
                                }
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



                                int thousandNumberNew = 0;

                                try
                                {
                                    thousandNumberNew = getThoudandNumberNew(prevRuleID, status, commoditySeriesArray, outputDestinationval, checkonceN, checkonceCV);
                                    formula = getFormula(formula, commoditySeriesArray, thousandNumberNew, keywords);
                                }
                                catch (Exception ex)
                                {
                                    sheet.Cells[i, formulaIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    if (ex.Message.Contains("Brackets"))
                                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                                    else if (ex.Message.Contains("keyword"))
                                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                                    else if (ex.Message.Contains("New data series"))
                                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                                    else if (ex.Message.Contains("Data series"))
                                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                                    else if (ex.Message.Contains("not used in formula"))
                                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                                    else
                                        errorMessage_BLUpload.Append(" -- Entered formula is not correct.");
                                }


                                //get inputs
                                string inputs = "";
                                try
                                {
                                    inputs = getInputs(inputsArray, thousandNumberNew);
                                }
                                catch (Exception ex)
                                {
                                    sheet.Cells[i, commoditySeriesIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    if (ex.Message.Contains("New data series"))
                                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                                    else if (ex.Message.Contains("make sure"))
                                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                                    else
                                        errorMessage_BLUpload.Append(" -- One of the input data series does not exist.");
                                }

                                //Get input data series ID
                                try
                                {
                                    commoditySeriesID = getInputDataSeriesID(inputs);
                                }
                                catch (Exception ex)
                                {
                                    sheet.Cells[i, commoditySeriesIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    errorMessage_BLUpload.Append(" -- Please use only ';'(Semi-Colon) to seperate Commodity Series IDs. Make sure the CVs and Ns are in correct format. For example 'CV1' or 'N1'");
                                }


                                //get Group ID
                                string groupID = "";
                                try
                                {
                                    groupID = getGroupId(commoditySeriesID, sequenceID);
                                }
                                catch (Exception ex)
                                {
                                    sheet.Cells[i, commoditySeriesIDIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    errorMessage_BLUpload.Append("Could not find a data series. Please make sure all the input data series IDs are correct");
                                }

                                // get Input unit
                                string inputUnit = "";
                                if (null != values[i, inputUnitIndex])
                                    inputUnit = values[i, inputUnitIndex].ToString();
                                else
                                {
                                    errorMessage_BLUpload.Append("Please enter Input Unit");
                                }
                                string InputUnitIDs = "";
                                try
                                {
                                    InputUnitIDs = getUnitID(inputUnit.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, inputUnitIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";

                                    if (inputUnit.Trim().Equals("1000"))
                                    {
                                        errorMessage_BLUpload.Append(" -- The Input Unit is not correct. If you meant '$1,000' then change the excel cell format from Currency/Number to text.");
                                    }
                                    else
                                    {
                                        errorMessage_BLUpload.Append(" -- Please make sure the input Unit is correct. Use ';'(Semi-Colon) to seperate multiple Units");
                                    }

                                }

                                // get macro
                                string macro = "";
                                if (null != values[i, macroIndex])
                                {

                                    try
                                    {
                                        if (getMacro(values[i, macroIndex].ToString().Trim()).Equals("Found"))
                                        {
                                            macro = values[i, macroIndex].ToString().Trim();
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        sheet.Cells[i, macroIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        errorMessage_BLUpload.Append(" -- " + ex.Message);
                                    }
                                }
                                else
                                    macro = "NULL";

                                // get conversion factor
                                string conversionFactor = "";
                                string conversionFactorID = "";
                                if (null != values[i, conversionFactorIndex])
                                {
                                    conversionFactor = values[i, conversionFactorIndex].ToString();
                                    try
                                    {
                                        conversionFactorID = getConversionFactorID(conversionFactor.Trim());
                                    }
                                    catch (Exception)
                                    {
                                        sheet.Cells[i, conversionFactorIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                        sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                        //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                        errorMessage_BLUpload.Append(" -- Please make sure the conversion factor is correct.");
                                    }
                                }
                                else if (null == values[i, conversionFactorIndex] && formula.Contains("conv"))
                                {
                                    sheet.Cells[i, conversionFactorIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please provide the value of conversion factor.");
                                }
                                else if (null == values[i, macroIndex] && formula.Contains("conv"))
                                {
                                    sheet.Cells[i, macroIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please provide the macro description.");
                                }
                                else
                                    conversionFactorID = "NULL";


                                //get output name
                                string outputName = "";
                                if (values[i, outputNameIndex] != null)
                                {
                                    outputName = values[i, outputNameIndex].ToString();
                                }
                                else
                                {
                                    errorMessage_BLUpload.Append("Please enter Output Name");
                                }
                                //get output unit
                                string outputUnit = "";
                                if (null != values[i, outputUnitIndex])
                                    outputUnit = values[i, outputUnitIndex].ToString();
                                else
                                {
                                    errorMessage_BLUpload.Append("Please enter Output Unit");
                                }
                                string outputUnitID = "";
                                try
                                {
                                    outputUnitID = getUnitID(outputUnit.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, outputUnitIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please make sure the output Unit is correct.");
                                }


                                //get Input Time Dimension Type
                                string inputTimeDimensionType = "";
                                if (null != values[i, inputTimeDimensionTypeIndex])
                                    inputTimeDimensionType = values[i, inputTimeDimensionTypeIndex].ToString();
                                string inputTimeDimensionTypeID = "";
                                try
                                {
                                    inputTimeDimensionTypeID = getTimeDimensionTypeID(inputTimeDimensionType.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, inputTimeDimensionTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please make sure the Input Time Dimension Type is correct. Valid Inputs are Month, Year and Marketing Year");
                                }

                                //get Input Time Value
                                string inputTimeDimensionTypeValue = "";
                                if (null != values[i, inputTimeValueIndex])
                                {
                                    inputTimeDimensionTypeValue = values[i, inputTimeValueIndex].ToString();
                                }
                                else
                                {
                                    sheet.Cells[i, inputTimeValueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please make sure the Input Time Value is correct");
                                }

                                //get Output Time Dimension Type
                                string outputTimeDimensionType = "";
                                if (null != values[i, outputTimeDimensionTypeIndex])
                                    outputTimeDimensionType = values[i, outputTimeDimensionTypeIndex].ToString();
                                string outputTimeDimensionTypeID = "";
                                try
                                {
                                    outputTimeDimensionTypeID = getTimeDimensionTypeID(outputTimeDimensionType.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, outputTimeDimensionTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please make sure the Output Time Dimension Type is correct.");
                                }

                                //get Output Time Value
                                string outputTimeDimensionTypeValue = "";
                                if (null != values[i, outputTimeValueIndex])
                                {
                                    outputTimeDimensionTypeValue = values[i, outputTimeValueIndex].ToString();
                                }
                                else
                                {
                                    sheet.Cells[i, outputTimeValueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please make sure the Output Time Value is correct");
                                }

                                //get Input Geo Type and value
                                string inputgeoType = "";
                                string inputgeoValue = "";
                                if (null != values[i, inputGeoTypeIndex])
                                    inputgeoType = values[i, inputGeoTypeIndex].ToString().Trim();
                                if (null != values[i, inputGeoValueIndex])
                                    inputgeoValue = values[i, inputGeoValueIndex].ToString().Trim();
                                string inputgeoTypeID = "";
                                string inputgeoValueID = "";
                                try
                                {
                                    inputgeoTypeID = getGeoTypeID(inputgeoType);

                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, inputGeoTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please enter a valid input Geo Type like Country, State or Region.");

                                }
                                try
                                {
                                    inputgeoValueID = getGeoValueID(inputgeoTypeID, inputgeoValue);

                                }
                                catch (Exception ex)
                                {
                                    sheet.Cells[i, inputGeoValueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    if (ex.Message.Contains("same number") || ex.Message.Contains("correct Geogrphy"))
                                        errorMessage_BLUpload.Append(ex.Message);
                                    else
                                        errorMessage_BLUpload.Append(" -- Please enter a valid input Geo Value for given Geo Type.");

                                }

                                //get Output Geo Type and value
                                string outputgeoType = "";
                                string outputgeoValue = "";
                                if (null != values[i, outputGeoTypeIndex])
                                    outputgeoType = values[i, outputGeoTypeIndex].ToString().Trim();
                                if (null != values[i, outputGeoValueIndex])
                                    outputgeoValue = values[i, outputGeoValueIndex].ToString().Trim();
                                string outputgeoTypeID = "";
                                string outputgeoValueID = "";
                                try
                                {
                                    outputgeoTypeID = getGeoTypeID(outputgeoType);

                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, outputGeoTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please enter a valid output Geo Type like Country, State or Region.");

                                }
                                try
                                {
                                    outputgeoValueID = getGeoValueID(outputgeoTypeID, outputgeoValue);

                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, outputGeoValueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please enter a valid output Geo Value for given Geo Type.");

                                }

                                //get privacy
                                string privacy = "";
                                if (null != values[i, privacyIndex])
                                    privacy = values[i, privacyIndex].ToString();
                                string privacyID = "";

                                try
                                {
                                    privacyID = getPrivacy(privacy.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, privacyIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please enter a valid privacy type like Private or Public.");
                                }

                                //get BLType
                                string BLType = "";
                                if (null != values[i, typeIndex])
                                {
                                    BLType = values[i, typeIndex].ToString();
                                }
                                else
                                {
                                    sheet.Cells[i, typeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please enter a valid Business Logic Type.");
                                }

                                //get Long Description
                                string longDescription = "";
                                if (null != values[i, longDescriptionIndex])
                                {
                                    longDescription = values[i, longDescriptionIndex].ToString();
                                }
                                else
                                {
                                    sheet.Cells[i, longDescriptionIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    errorMessage_BLUpload.Append(" -- Please enter a valid long description.");
                                }

                                //get Input Sources
                                string inputSources = "";
                                if (null != values[i, inputSourcesIndex])
                                    inputSources = values[i, inputSourcesIndex].ToString();
                                string inputSourcesID = "";
                                try
                                {
                                    inputSourcesID = getInputSourcesID(inputSources, outputName);

                                }
                                catch (Exception ex)
                                {
                                    sheet.Cells[i, inputSourcesIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    //sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    if (ex.Message.Contains("not found")) ;
                                    errorMessage_BLUpload.Append(" -- " + ex.Message);
                                    errorMessage_BLUpload.Append(" -- Source '" + inputSources + "' not found. Please enter a correct source description.");

                                }

                                //Savign error messages in sheet
                                sheet.Cells[i, columnsCount + 1] = errorMessage_BLUpload.ToString();

                                //insertor update
                                try
                                {
                                    string insertStatus = "";
                                    string updateStatus = "";
                                    //insert new rules
                                    if (status.Equals("New", StringComparison.OrdinalIgnoreCase) && errorMessage_BLUpload.Length == 0)
                                    {
                                        insertStatus = insertBusinessRules_UploadSheet(commoditySeriesID, inputsCount, inputs, InputUnitIDs, conversionFactorID, macro, outputName, outputUnitID, formula, inputTimeDimensionTypeValue, inputTimeDimensionTypeID, outputTimeDimensionTypeValue, outputTimeDimensionTypeID, inputgeoValueID, outputgeoValueID, outputDestination, privacyID, BLType, longDescription, inputSources, inputSourcesID, groupID, sequenceID);
                                        if (insertStatus == "failed")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "Inserion Failed. Please make sure the input data is correct or contact technical team.";

                                        }
                                        else if (insertStatus == "success")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "Business Rule inserted successfully.";
                                        }
                                        else if (insertStatus == "duplicate")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "Insertion Failed. Cannot insert duplicate business rule.";
                                        }
                                    }
                                    //Update existing rules
                                    else if (status.Equals("Override", StringComparison.OrdinalIgnoreCase) && errorMessage_BLUpload.Length == 0)
                                    {
                                        updateStatus = updateBusinessRules_UploadSheet(businessRuleID, commoditySeriesID, inputsCount, inputs, InputUnitIDs, conversionFactorID, macro, outputName, outputUnitID, formula, inputTimeDimensionTypeValue, inputTimeDimensionTypeID, outputTimeDimensionTypeValue, outputTimeDimensionTypeID, inputgeoValueID, outputgeoValueID, outputDestination, privacyID, BLType, longDescription, inputSources, inputSourcesID, groupID);
                                        if (updateStatus == "failed")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                            sheet.Cells[i, columnsCount + 1] = "Override Failed. Please make sure the input data is correct or contact technical team.";

                                        }
                                        else if (updateStatus == "success")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                            sheet.Cells[i, columnsCount + 1] = "Business Rule updated successfully.";
                                        }
                                        else if (updateStatus == "duplicate")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                            sheet.Cells[i, columnsCount + 1] = "Override Failed. Similar business rule already exist in the system.";
                                        }
                                    }

                                }
                                catch (Exception)
                                {
                                    for (int k = 1; k <= columnsCount; k++)
                                        sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    continue;

                                }

                            }

                        }
                        //check ends

                        // first check if target directory exist
                        // create one if not exist
                        if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Business Rules")) // name of the error file and location
                        {
                            Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Business Rules");
                        }

                        string fileName = cosdFolderPath + "CoSD Tool\\Business Rules\\BusinessRules_Upload_" + Date + ".xls";
                        try
                        {
                            oBook.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                            false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                            oBook.Close();
                            oExcel.Quit();

                            if (successNum == total)
                            {
                                if (total == 1)
                                    label_status.Text = successNum + " record(s) uploaded to the database. Please check " + fileName + " for details.";
                                label_status.Text = successNum + " record(s) uploaded to the database. Please check " + fileName + " for details.";
                            }
                            else
                                label_status.Text = successNum + " record(s) uploaded. Please check " + fileName + " for any failed records.";
                            successNum = 0;
                            total = 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to create result excel file. Please contact technical team\nError Details" + ex.Message);
                            //MessageBox.Show("Could Not save result file" + ex.Message);
                        }
                    }

                    else
                    {

                        oBook.Close(false, Type.Missing, Type.Missing);
                        oExcel.Quit();
                    }
                }
                catch (Exception ex)
                {
                    label_status.Text = "Upload Failed!";
                    MessageBox.Show("Upload failed. Please contact technical team.\nError Details" + ex.Message);
                    //MessageBox.Show("Exception at " + ex.Message);
                }


            }
        }


        //insert business rules
        private string insertBusinessRules_UploadSheet(string commoditySeriesID, int inputsCount, string inputs, string InputUnitIDs, string conversionFactorID, string macro, string outputName, string outputUnitID, string formula, string inputTimeDimensionTypeValue, string inputTimeDimensionTypeID, string outputTimeDimensionTypeValue, string outputTimeDimensionTypeID, string inputgeoValueID, string outputgeoValueID, string outputDestination, string privacyID, string BLType, string longDescription, string inputSources, string inputSourcesID, string groupID, decimal sequenceID)
        {


            string sql = "INSERT INTO " + schemaName + "[ERSBusinessLogic] " +
           " ([ERSBusinessLogic_Formula] " +
           " ,[ERSBusinessLogic_InputsCount] " +
           " ,[ERSBusinessLogic_Inputs] " +
           " ,[ERSBusinessLogic_ConvFactorID] " +
           " ,[ERSBusinessLogic_MacroDesc] " +
           " ,[ERSBusinessLogic_InputDataSeries] " +
           " ,[ERSBusinessLogic_InputTimeDimensionValue] " +
           " ,[ERSBusinessLogic_InputTimeDimensionTypeID] " +
           " ,[ERSBusinessLogic_InputGeographyDimensionID] " +
           " ,[ERSBusinessLogic_OutputGeographyDimensionID] " +
           " ,[ERSBusinessLogic_InputUnitID] " +
           " ,[ERSBusinessLogic_Type] " +
           " ,[ERSBusinessLogic_PrivacyID] " +
           " ,[ERSBusinessLogic_LongDesc] " +
           " ,[ERSBusinessLogic_InputSources] " +
           " ,[ERSBusinessLogic_InputSourceID] " +
           " ,[ERSBusinessLogic_OutputName] " +
           " ,[ERSBusinessLogic_OutputUnitID] " +
           " ,[ERSBusinessLogic_OutputDestination] " +
           " ,[ERSBusinessLogic_OutputTimeDimensionValue] " +
           " ,[ERSBusinessLogic_OutputTimeDimensionTypeID] " +
           " ,[ERSBusinessLogic_GroupID] " +
           " ,[ERSBusinessLogic_SequenceID]) " +
           " VALUES " +
           " ( "+
           "'" + formula + "'," +
                 inputsCount + "," +
           "'" + inputs + "'," +
                 conversionFactorID + "," +
           "'" + macro + "'," +
           "'" + commoditySeriesID + "'," +
           "'" + inputTimeDimensionTypeValue + "'," +
                 inputTimeDimensionTypeID + "," +
           "'" + inputgeoValueID + "'," +
                 outputgeoValueID + "," +
           "'" + InputUnitIDs + "'," +
           "'" + BLType + "'," +
                 privacyID + "," +
           "'" + longDescription + "'," +
           "'" + inputSources + "'," +
                 inputSourcesID + "," +
           "'" + outputName + "'," +
                 outputUnitID + "," +
           "'" + outputDestination + "'," +
           "'" + outputTimeDimensionTypeValue + "'," +
                 outputTimeDimensionTypeID + "," +
                 groupID + "," +
                 sequenceID +
           " )";

            // execute
            try
            {
                System.Data.DataTable dt = GetData(sql);
                saveAction(sql);
                if (null != dt)
                {
                    successNum++;
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
                //saveAction("Import business rules via upload sheet action failed.");
                return "failed";

            }

        }

        //update business rules
        private string updateBusinessRules_UploadSheet(string businessLogicID, string commoditySeriesID, int inputsCount, string inputs, string InputUnitIDs, string conversionFactorID, string macro, string outputName, string outputUnitID, string formula, string inputTimeDimensionTypeValue, string inputTimeDimensionTypeID, string outputTimeDimensionTypeValue, string outputTimeDimensionTypeID, string inputgeoValueID, string outputgeoValueID, string outputDestination, string privacyID, string BLType, string longDescription, string inputSources, string inputSourcesID, string groupID)
        {
            string sql = "UPDATE " + schemaName + "[ERSBusinessLogic] SET " +
           "  [ERSBusinessLogic_Formula] = '" + formula + "'" +
           " ,[ERSBusinessLogic_InputsCount] = " + inputsCount +
           " ,[ERSBusinessLogic_Inputs] = '" + inputs + "'" +
           " ,[ERSBusinessLogic_ConvFactorID] = "+conversionFactorID +
           " ,[ERSBusinessLogic_MacroDesc] = '" + macro + "'" +
           " ,[ERSBusinessLogic_InputDataSeries] = '" + commoditySeriesID + "'" +
           " ,[ERSBusinessLogic_InputTimeDimensionValue] = '" + inputTimeDimensionTypeValue + "'" +
           " ,[ERSBusinessLogic_InputTimeDimensionTypeID] = " + inputTimeDimensionTypeID +
           " ,[ERSBusinessLogic_InputGeographyDimensionID] = '" + inputgeoValueID + "'" +
           " ,[ERSBusinessLogic_OutputGeographyDimensionID] = " +outputgeoValueID+
           " ,[ERSBusinessLogic_InputUnitID] = '" + InputUnitIDs + "'" +
           " ,[ERSBusinessLogic_Type] = '" + BLType + "'" +
           " ,[ERSBusinessLogic_PrivacyID] = " +privacyID + 
           " ,[ERSBusinessLogic_LongDesc] = '" + longDescription + "'" +
           " ,[ERSBusinessLogic_InputSources] = '" + inputSources + "'" +
           " ,[ERSBusinessLogic_InputSourceID] = " +inputSourcesID + 
           " ,[ERSBusinessLogic_OutputName] = '" + outputName + "'" +
           " ,[ERSBusinessLogic_OutputUnitID] = " +outputUnitID + 
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
                    successNum++;
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
                //saveAction("Update business rules via upload sheet action failed.");
                return "failed";

            }

        }


        private bool validExcelFormat(object[,] values) // validating all the excel cells if they are in the correct format
        {
            /// FILE FORMAT:
            //statusIndex
            //insertPositionIndex
            //businessLogicIDIndex
            //commoditySeriesIDIndex
            //inputUnitIndex
            //conversionFactorIndex
            //macroIndex
            //outputNameIndex
            //outputUnitIndex
            //formulaIndex
            //inputTimeDimensionTypeIndex
            //inputTimeValueIndex
            //outputTimeDimensionTypeIndex
            //outputTimeValueIndex
            //inputGeoTypeIndex
            //inputGeoValueIndex
            //outputGeoTypeIndex
            //outputGeoValueIndex
            //outputDestinationIndex
            //privacyIndex
            //typeIndex
            //longDescriptionIndex
            //inputSourcesIndex
            try
            {
                Boolean valid = false;

                //get index of each column
                for (int i = 1; i <= columnsCount; i++)
                {
                    if (values[1, i].ToString().Equals("Status (New/Override)"))
                    {
                        statusIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Import/Export Rule ID"))
                    {
                        ruleIDIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Commodity Series ID"))
                    {
                        commoditySeriesIDIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Input Unit"))
                    {
                        inputUnitIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Conversion Factor"))
                    {
                        conversionFactorIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Macro"))
                    {
                        macroIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Output Name"))
                    {
                        outputNameIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Output Unit"))
                    {
                        outputUnitIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Formula"))
                    {
                        formulaIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Input Time Dimension Type"))
                    {
                        inputTimeDimensionTypeIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Input Time Value"))
                    {
                        inputTimeValueIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Output Time Dimension Type"))
                    {
                        outputTimeDimensionTypeIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Output Time Value"))
                    {
                        outputTimeValueIndex = i;
                        valid = true;
                    }

                    else if (values[1, i].ToString().Equals("Input Geo Type"))
                    {
                        inputGeoTypeIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Input Geo Value"))
                    {
                        inputGeoValueIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Output Geo Type"))
                    {
                        outputGeoTypeIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Output Geo Value"))
                    {
                        outputGeoValueIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Output Destination"))
                    {
                        outputDestinationIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Privacy"))
                    {
                        privacyIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Type"))
                    {
                        typeIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Long Description"))
                    {
                        longDescriptionIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Input Sources"))
                    {
                        inputSourcesIndex = i;
                        valid = true;
                    }
                    else
                        valid = false;
                }

                if (!valid || statusIndex == 0 || ruleIDIndex == 0 || commoditySeriesIDIndex == 0 || inputUnitIndex == 0 || conversionFactorIndex == 0 || macroIndex == 0 || outputNameIndex == 0 || outputUnitIndex == 0 || formulaIndex == 0 || inputTimeDimensionTypeIndex == 0 || inputTimeValueIndex == 0 || outputTimeDimensionTypeIndex == 0 || outputTimeValueIndex == 0 || inputGeoTypeIndex == 0 || inputGeoValueIndex == 0 || outputGeoTypeIndex == 0 || outputGeoValueIndex == 0
                    || outputDestinationIndex == 0 || privacyIndex == 0 || typeIndex == 0 || longDescriptionIndex == 0 || inputSourcesIndex == 0)
                    return false;

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

//

        private bool validExcelFormatNS(object[,] values) // validating all the excel cells if they are in the correct format for new data series
        {
            /// FILE FORMAT:
            //new data series

            //statusIndexNS
            //NewDataseriesIndexNS
            //newidIndexNS
            //commodityseriesidIndexNS
            //dataseriesdescrIndexNS
            //sectorIndexNS
            //groupIndexNS
            //subcommodityIndexNS 
            //sourceseriesLongDescIndexNS 
            //stattypeIndexNS 
            //unitIndexNS 
            //physicalAtttypeIndexNS 
            //physicalAttDescIndexNS 
            //prodpracIndexNS 
            //utilpracIndexNS 
            //sourceIndexNS 
            try
            {
                Boolean valid = false;

                //get index of each column
                for (int i = 1; i <= columnsCount; i++)
                {
                    if (values[1, i].ToString().Equals("Status (New/Override)"))
                    {
                        statusIndexNS = i;
                        valid = true;
                    }
                   else if (values[1, i].ToString().Equals("New data series ID"))
                    {
                        NewDataseriesIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("New ID"))
                    {
                        newidIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Original Commodity Series ERS ID"))
                    {
                        commoditySeriesIDIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERSCommodity_DataSeriesCategory_Desc"))
                    {
                        dataseriesdescrIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERSSector_Desc"))
                    {
                        sectorIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERSGroup_Desc"))
                    {
                        groupIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERS Commodity,Subcommodity"))
                    {
                        subcommodityIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERSCommodity_SourceSeriesID_LongDesc"))
                    {
                        sourceseriesLongDescIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERSStatisticType_Attribute"))
                    {
                        stattypeIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERSUnit_Desc"))
                    {
                        unitIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERSPhysicalAttribute_Type"))
                    {
                        physicalAtttypeIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERSCommodity_PhysicalAttribute_Desc"))
                    {
                        physicalAttDescIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERSProdPractice_Desc"))
                    {
                        prodpracIndexNS = i;
                        valid = true;
                    }

                    else if (values[1, i].ToString().Equals("ERSUtilPractice_Desc"))
                    {
                        utilpracIndexNS = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("ERSSource_Desc"))
                    {
                        sourceIndexNS = i;
                        valid = true;
                    }
                  
                }

                if (!valid || statusIndexNS == 0 || NewDataseriesIndexNS == 0 || newidIndexNS == 0 || commoditySeriesIDIndexNS == 0 || sectorIndexNS == 0 || groupIndexNS == 0 ||
                    subcommodityIndexNS == 0 || sourceseriesLongDescIndexNS == 0 || stattypeIndexNS == 0 || unitIndexNS == 0 || physicalAtttypeIndexNS == 0 || physicalAttDescIndexNS == 0 || prodpracIndexNS == 0 ||
                    utilpracIndexNS == 0 || sourceIndexNS == 0)
                    return false;

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        //

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

        //get next sequence ID
        private decimal getSequenceID(string prevRuleID, string nextRuleID)
        {
            try
            {
                //decimal sequenceIDNextID;
                Decimal sqeuenceIDPrevID;
                //string sqeuenceIDPrevIDPart2WithoutZero = "";
                //int sequenceIDNextPart1;
                //int sequenceIDNextPart2;
                //int sequenceIDPrevPart1;
                //int sequenceIDPrevPart2;
                string sequenceIDNew;
                Decimal sequenceIDNewDecimal;
                Decimal sequenceIDUpdate = 0;

                if (!(prevRuleID.Equals("") && nextRuleID.Equals("")))
                {
                    //Previous sequence id
                    string sqlPrevID = "SELECT [ERSBusinessLogic_SequenceID] AS ERSBusinessLogic_SequenceID FROM  " + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_ID] = " + prevRuleID;
                    System.Data.DataTable dtsqlPrevID = GetData(sqlPrevID);
                    DataRow[] drsqlPrevID = dtsqlPrevID.Select();
                    sqeuenceIDPrevID = Convert.ToDecimal(drsqlPrevID[0]["ERSBusinessLogic_SequenceID"]);

                    sequenceIDNewDecimal = sqeuenceIDPrevID + 1;
                    sequenceIDUpdate = sequenceIDNewDecimal;

                    //next valid rule id
                    //string sqlValidRuleID = "SELECT [ERSBusinessLogic_ID] from  " + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_SequenceID] = (SELECT Min([ERSBusinessLogic_SequenceID]) AS ERSBusinessLogic_SequenceID FROM  " + schemaName + "[ERSBusinessLogic]  where ERSBusinessLogic_SequenceID > " + sqeuenceIDPrevID + ")";
                    //System.Data.DataTable dtsqlValidRuleID = GetData(sqlValidRuleID);
                    //DataRow[] drsqlValidRuleID = dtsqlValidRuleID.Select();
                    //nextValidRuleID = drsqlValidRuleID[0]["ERSBusinessLogic_ID"].ToString();


                    //if (!nextValidRuleID.Equals(nextRuleID))
                    //{
                    //    throw new Exception("Rule IDs are not in order. Please enter two rule IDs which are next to each other");
                    //}

                    //Update Sequence IDs
                    //string sqlGetID = "SELECT ERSBusinessLogic_ID FROM " + schemaName + "[ERSBusinessLogic] WHERE ERSBusinessLogic_SequenceID > " + sqeuenceIDPrevID + " ORDER BY [ERSBusinessLogic_SequenceID]";
                    //Update Sequence IDs
                    //System.Data.DataTable dtsqlsqlGetID = GetData(sqlGetID);
                    //if (dtsqlsqlGetID.Rows.Count > 0)
                    //{
                    //    DataRow[] drsqlsqlGetID = dtsqlsqlGetID.Select();
                    //    foreach (DataRow row in drsqlsqlGetID)
                    //    {
                            
                    //        sequenceIDUpdate++;
                    //        string sqlUpdateSeqID = "UPDATE " + schemaName + "[ERSBusinessLogic] SET [ERSBusinessLogic_SequenceID] = " + sequenceIDUpdate + " WHERE ERSBusinessLogic_ID = " + row["ERSBusinessLogic_ID"].ToString();
                    //        System.Data.DataTable dtsqlUpdateSeq = GetData(sqlUpdateSeqID);
                    //    }

                    //}                    
                    string sqlUpdateSeqID = "UPDATE " + schemaName + "[ERSBusinessLogic] SET [ERSBusinessLogic_SequenceID] = CASE WHEN [ERSBusinessLogic_SequenceID] > " + sqeuenceIDPrevID + " THEN [ERSBusinessLogic_SequenceID] + 1 ELSE [ERSBusinessLogic_SequenceID] END";
                    System.Data.DataTable dtsqlUpdateSeq = GetData(sqlUpdateSeqID);
                }
                else
                {
                    try
                    {
                        string sqlMaxID = "SELECT (MAX([ERSBusinessLogic_SequenceID]) + 1) AS ERSBusinessLogic_SequenceID FROM " + schemaName + "[ERSBusinessLogic]";
                        System.Data.DataTable dtsqlMaxID = GetData(sqlMaxID);
                        DataRow[] drsqlMaxID = dtsqlMaxID.Select();
                        sequenceIDNew = drsqlMaxID[0]["ERSBusinessLogic_SequenceID"].ToString();
                        sequenceIDNewDecimal = Math.Round(Convert.ToDecimal(sequenceIDNew), 6);
                    }
                    catch
                    {
                        sequenceIDNewDecimal = 1;
                    }
                    
                }

                //Validate sequence ID
                string sqlValidate = "SELECT COUNT(ERSBusinessLogic_ID) AS Status FROM " + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_SequenceID] = " + sequenceIDNewDecimal;
                System.Data.DataTable dtValidate = GetData(sqlValidate);
                DataRow[] drValidate = dtValidate.Select();
                string validateSequence = drValidate[0]["Status"].ToString();
                if (validateSequence == "0")
                    return sequenceIDNewDecimal;
                else
                    throw new Exception("Sequence ID already exist");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                List<string> keywordsInFormulaList  = new List<string>();
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

                          //  int checkrepeatingmatch = Regex.Matches(newFormula, inputsArray[counter]).Count;

                            int arrayindex = newFormula.IndexOf("[" + inputsArray[counter] + "]");

                            var aStringBuilder = new StringBuilder(newFormula);
                            aStringBuilder.Remove(arrayindex, inputsArray[counter].Length+2);
                            aStringBuilder.Insert(arrayindex, "[CV" + newThousandNumber + "]");
                            newFormula = aStringBuilder.ToString();

                            //if (checkrepeatingmatch > 0)
                            //{
                            //    newFormula = newFormula.Replace(inputsArray[counter], "CV" + newThousandNumber);
                            //}
                            //else
                            //{
                            //    newFormula = newFormula.Replace(inputsArray[counter], "CV" + newThousandNumber);
                            //}
                            
                        }
                        else if (inputsArray[counter].Contains("N") || inputsArray[0].Contains("n"))                            
                        {
                            try
                            {
                                newThousandNumber = (thousandNumber * 1000) + Convert.ToInt32(Regex.Match(inputsArray[counter], @"\d+").Value);

                                //Get commodity ID with stat type
                                string sqlcommodity_Stat = "SELECT ([ERSStatisticType_Attribute] + '(' + CONVERT(varchar(100),[ERSCommodity_ID]) + ')') AS Commodity_Stat FROM " + schemaName + "[ERSCommodityDataSeries] LEFT JOIN " + schemaName + "[ERSStatisticType_LU] ON [ERSStatisticType_ID] = [ERSCommodity_ERSStatisticType_ID] where ERSCommodity_ID IN (SELECT [ERSCommodity_ID] FROM  " + schemaName + "[ERSCommodityDataSeries] where ERSCommodity_SourceSeriesID like '%(" + "N" + newThousandNumber + ")%')";
                                System.Data.DataTable dtcommodity_Stat = GetData(sqlcommodity_Stat);
                                DataRow[] drcommodity_Stat = dtcommodity_Stat.Select();

                                int arrayindex1 = newFormula.IndexOf(inputsArray[counter]);

                                var aStringBuilder1 = new StringBuilder(newFormula);
                                aStringBuilder1.Remove(arrayindex1, inputsArray[counter].Length);
                                aStringBuilder1.Insert(arrayindex1, drcommodity_Stat[0]["Commodity_Stat"].ToString());
                                newFormula = aStringBuilder1.ToString();


                               // newFormula = newFormula.Replace(inputsArray[counter], drcommodity_Stat[0]["Commodity_Stat"].ToString());
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
                                string sqlcommodity_Stat = "SELECT ([ERSStatisticType_Attribute] + '(' + CONVERT(varchar(100),[ERSCommodity_ID]) + ')') AS Commodity_Stat FROM " + schemaName + "[ERSCommodityDataSeries] LEFT JOIN " + schemaName + "[ERSStatisticType_LU] ON [ERSStatisticType_ID] = [ERSCommodity_ERSStatisticType_ID] where ERSCommodity_ID IN (SELECT [ERSCommodity_ID] FROM  " + schemaName + "[ERSCommodityDataSeries] where [ERSCommodity_ID] = " + inputsArray[counter] + ")";
                                System.Data.DataTable dtcommodity_Stat = GetData(sqlcommodity_Stat);
                                DataRow[] drcommodity_Stat = dtcommodity_Stat.Select();

                                // newFormula = newFormula.Replace(inputsArray[counter], drcommodity_Stat[0]["Commodity_Stat"].ToString());


                                int arrayindex2 = newFormula.IndexOf(inputsArray[counter]);

                                var aStringBuilder2 = new StringBuilder(newFormula);
                                aStringBuilder2.Remove(arrayindex2, inputsArray[counter].Length);
                                aStringBuilder2.Insert(arrayindex2, drcommodity_Stat[0]["Commodity_Stat"].ToString());
                                newFormula = aStringBuilder2.ToString();
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


        private string getInputDataSeriesID(string commoditySeriesID)
        {
            string[] inputsArray = Array.ConvertAll(commoditySeriesID.Split(';'), p => p.Trim()).Distinct().ToArray();
            StringBuilder commoditySeriesIDBuilder = new StringBuilder();

            for (int counter = 0; counter < inputsArray.Length; counter++)
            {
                if (inputsArray[counter].Contains("("))
                {
                    //commoditySeriesID = commoditySeriesID.Replace(inputsArray[counter], );
                    commoditySeriesIDBuilder.Append(Regex.Match(inputsArray[counter], @"\(([^)]*)\)").Groups[1].Value + ",");
                }
                else
                {
                    commoditySeriesIDBuilder.Append(inputsArray[counter] + ",");
                }
            }

            //fix the format again
            commoditySeriesID = commoditySeriesIDBuilder.Replace(";", ",").ToString();


            return commoditySeriesID.Remove(commoditySeriesID.Length - 1);
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
                        commoditySeriesIDInputs.Append(inputs + ";");
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
                    commoditySeriesIDInputs.Append(inputs + ";");
                }
                else if (inputsArray[counter].Contains("N") || inputsArray[0].Contains("n"))
                {
                    try
                    {
                        newThousandNumber = (thousandNumber * 1000) + Convert.ToInt32(Regex.Match(inputsArray[counter], @"\d+").Value);

                        //Get commodity ID with stat type
                        string sqlcommodity_Stat = "SELECT ([ERSStatisticType_Attribute] + '(' + CONVERT(varchar(100),[ERSCommodity_ID]) + ')') AS Commodity_Stat FROM " + schemaName + "[ERSCommodityDataSeries] LEFT JOIN " + schemaName + "[ERSStatisticType_LU] ON [ERSStatisticType_ID] = [ERSCommodity_ERSStatisticType_ID] where ERSCommodity_ID IN (SELECT [ERSCommodity_ID] FROM  " + schemaName + "[ERSCommodityDataSeries] where ERSCommodity_SourceSeriesID like '%(" + "N" + newThousandNumber + ")%')";
                        System.Data.DataTable dtcommodity_Stat = GetData(sqlcommodity_Stat);
                        DataRow[] drcommodity_Stat = dtcommodity_Stat.Select();

                        inputs = drcommodity_Stat[0]["Commodity_Stat"].ToString();
                        commoditySeriesIDInputs.Append(inputs + ";");
                    }
                    catch (Exception e)
                    {
                        throw new Exception("New data series " + inputsArray[counter] + " not found. Please make sure the new data series is available before adding this rule.");
                    }
                }
            }

            return commoditySeriesIDInputs.ToString().Remove(commoditySeriesIDInputs.ToString().Length - 1);
        }

        private string getGroupId(string commodityDataSeriesID, decimal sequenceid)
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
                       // first_grp_id = groupID;
                        break;
                    }
                }
                else
                {

                    //string sql = "SELECT [ERSBusinessLogic_GroupID] FROM " + schemaName + "[ERSBusinessLogic] where ERSBusinessLogic_OutputDestination like '" + inputsArray[counter] + "'";
                    string sql = "SELECT top(1) [ERSBusinessLogic_GroupID] FROM " + schemaName + "[ERSBusinessLogic] WHERE ERSBusinessLogic_SequenceID <= " + sequenceid + "order by  ERSBusinessLogic_SequenceID desc";
                    System.Data.DataTable dt = GetData(sql);
                    DataRow[] dr = dt.Select();
                    if (dr.Length > 0)
                    {
                        groupID = dr[0]["ERSBusinessLogic_GroupID"].ToString();
                        break;
                    }

                    //groupID = first_grp_id;
                }
            }
            return groupID;
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
                string sql = "select [ERSStatisticType_Attribute] from " + schemaName + "[ERSCommodityDataSeries] LEFT JOIN " + schemaName + "[ERSStatisticType_LU] ON [ERSStatisticType_ID] = [ERSCommodity_ERSStatisticType_ID] where ERSCommodity_ID = '" + commodityID + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSStatisticType_Attribute"].ToString();
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
                if (timeDimensionType.Equals(("Year")))
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


        //        //original code
        //        private int getThoudandNumber(string businessRuleID, string status, string[] inputsArray)
        //        {
        //            string sourceCVNumber = "";
        //            int thousandNumber = 0;
        //            string outputDestination;
        //            if (!businessRuleID.Equals("") && status=="Override")
        //            {
        //                try
        //                {
        //                    string sqlCVNumber = "SELECT [ERSBusinessLogic_OutputDestination] FROM " + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_ID] =  " + businessRuleID;
        //                    System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
        //                    DataRow[] drCVNumber = dtCVNumber.Select();
        //                    sourceCVNumber = drCVNumber[0]["ERSBusinessLogic_OutputDestination"].ToString();
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw new Exception("Businees Rule ID does not exist.");
        //                }

        //                //  'CV(N154000) 2011: 4'
        //                //  'CV16004 2001'
        //                //checking the number in thousand
        //                if (sourceCVNumber.Contains("N"))
        //                {
        //                    sourceCVNumber = Regex.Match(sourceCVNumber, @"\(([^)]*)\)").Groups[1].Value.Trim();
        //                    if (sourceCVNumber.Length == 5)
        //                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(1, 1));
        //                    else if (sourceCVNumber.Length == 6)
        //                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(1, 2));
        //                    else if (sourceCVNumber.Length == 7)
        //                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(1, 3));
        //                }
        //                else
        //                {
        //                    if (sourceCVNumber.Length == 6)
        //                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(2, 1));
        //                    else if (sourceCVNumber.Length == 7)
        //                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(2, 2));
        //                    else if (sourceCVNumber.Length == 8)
        //                        thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(2, 3));
        //                }

        //            }
        //            else if (businessRuleID.Equals("") && status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
        //            {

        //                for (int counter = 0; counter < inputsArray.Length; counter++)
        //                {
        //                    if (!(inputsArray[counter].Contains("C") || inputsArray[0].Contains("c") || inputsArray[counter].Contains("N") || inputsArray[0].Contains("n")))
        //                    {
        //                        string sqlNewNumber = "SELECT [ERSCommodity_ID] FROM " + schemaName + "[ERSCommodityDataSeries] where ERSCommoditySubCommodity_ID IN (SELECT [ERSCommoditySubCommodity_ID] FROM " + schemaName + "[ERSCommodityDataSeries] where ERSCommodity_ID = " + inputsArray[counter] + ")";
        //                        System.Data.DataTable sqlNewNumberTable = GetData(sqlNewNumber);
        //                        DataRow[] drNewNumber = sqlNewNumberTable.Select();

        //                        for (int row = 0; row < drNewNumber.Length; row++)
        //                        {
        //                            string sqlOutputDestinationNumber = "SELECT [ERSBusinessLogic_OutputDestination] FROM " + schemaName + "[ERSBusinessLogic] where " +
        //" REPLACE(ERSBusinessLogic_InputDataSeries, ' ', '') like '" + drNewNumber[row]["ERSCommodity_ID"].ToString() + ",%'  " +
        //" OR REPLACE(ERSBusinessLogic_InputDataSeries, ' ', '') like '%," + drNewNumber[row]["ERSCommodity_ID"].ToString() + "'  " +
        //" OR REPLACE(ERSBusinessLogic_InputDataSeries, ' ', '') like '%," + drNewNumber[row]["ERSCommodity_ID"].ToString() + ",%' ";
        //                            System.Data.DataTable sqlOutputDestinationNumberTable = GetData(sqlOutputDestinationNumber);
        //                            DataRow[] drOutputDestinationNumber = sqlOutputDestinationNumberTable.Select();

        //                            if (drOutputDestinationNumber.Length > 0)
        //                            {
        //                                outputDestination = drOutputDestinationNumber[0]["ERSBusinessLogic_OutputDestination"].ToString();

        //                                if (outputDestination.Contains("N"))
        //                                {
        //                                    outputDestination = Regex.Match(outputDestination, @"\(([^)]*)\)").Groups[1].Value.Trim();
        //                                    if (outputDestination.Length == 5)
        //                                        thousandNumber = Convert.ToInt32(outputDestination.Substring(1, 1));
        //                                    else if (outputDestination.Length == 6)
        //                                        thousandNumber = Convert.ToInt32(outputDestination.Substring(1, 2));
        //                                    else if (outputDestination.Length == 7)
        //                                        thousandNumber = Convert.ToInt32(outputDestination.Substring(1, 3));
        //                                }
        //                                else
        //                                {
        //                                    if (outputDestination.Length == 6)
        //                                        thousandNumber = Convert.ToInt32(outputDestination.Substring(2, 1));
        //                                    else if (outputDestination.Length == 7)
        //                                        thousandNumber = Convert.ToInt32(outputDestination.Substring(2, 2));
        //                                    else if (outputDestination.Length == 8)
        //                                        thousandNumber = Convert.ToInt32(outputDestination.Substring(2, 3));
        //                                }

        //                                break;
        //                            }
        //                            else
        //                            {
        //                                string sqlCVNumber = "SELECT MAX(convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM " + schemaName + "[ERSBusinessLogic]";
        //                                System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
        //                                DataRow[] drCVNumber = dtCVNumber.Select();
        //                                sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
        //                                if (sourceCVNumber.Length == 4)
        //                                    thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
        //                                if (sourceCVNumber.Length == 5)
        //                                    thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
        //                                if (sourceCVNumber.Length == 6)
        //                                    thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));

        //                                thousandNumber = thousandNumber + 1;
        //                            }
        //                        }

        //                        break;
        //                    }
        //                    else
        //                    {
        //                        string sqlCVNumber = "SELECT TOP 1 convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')','')) AS CVNUMBER FROM " + schemaName + "[ERSBusinessLogic] ORDER BY [ERSBusinessLogic_SequenceID] DESC";


        //                        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
        //                        DataRow[] drCVNumber = dtCVNumber.Select();
        //                        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
        //                        if (sourceCVNumber.Length == 4)
        //                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
        //                        if (sourceCVNumber.Length == 5)
        //                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
        //                        if (sourceCVNumber.Length == 6)
        //                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));


        //                    }
        //                }

        //            }

        //            return thousandNumber;
        //        }




        ////


        private int getThousandNumberForerror(string businessRuleID, string status, string[] inputsArray, string outputDestinationval, int checkonceN, int checkonceCV)
        {
            string sourceCVNumber = "";
            int thousandNumber = 0;

            if (businessRuleID.Equals("") && status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
            {
                //if (checkonceN == 1)
                //{

                    if (outputDestinationval.Contains("N"))
                    {
                        string sqlCVNumber = "SELECT MAX(convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                                + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_OutputDestination] like '%CV(N%'";
                        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                        DataRow[] drCVNumber = dtCVNumber.Select();
                        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                        if (sourceCVNumber.Length == 4)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                        if (sourceCVNumber.Length == 5)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                        if (sourceCVNumber.Length == 6)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));

                        thousandNumber = thousandNumber + 1;

                    }

                //}



                //if (checkonceCV == 1)
                //{
                    if (!outputDestinationval.Contains("N"))
                    {
                        string sqlCVNumber = "SELECT MAX(convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                        + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_OutputDestination] not like '%CV(N%'";
                        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                        DataRow[] drCVNumber = dtCVNumber.Select();
                        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                        if (sourceCVNumber.Length == 4)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                        if (sourceCVNumber.Length == 5)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                        if (sourceCVNumber.Length == 6)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));

                        thousandNumber = thousandNumber + 1;
                    }

                //}


                //if (checkonceN > 1 && checkonceCV == 0)
                //{
                //    if (outputDestinationval.Contains("N"))
                //    {
                //        string sqlCVNumber = "SELECT MAX(convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                //                + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_OutputDestination] like '%CV(N%'";
                //        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                //        DataRow[] drCVNumber = dtCVNumber.Select();
                //        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                //        if (sourceCVNumber.Length == 4)
                //            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                //        if (sourceCVNumber.Length == 5)
                //            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                //        if (sourceCVNumber.Length == 6)
                //            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));


                //    }
                //}



                //if (checkonceCV > 1 && checkonceN == 0)
                //{
                //    if (!outputDestinationval.Contains("N"))
                //    {
                //        string sqlCVNumber = "SELECT MAX(convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                //        + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_OutputDestination] not like '%CV(N%'";
                //        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                //        DataRow[] drCVNumber = dtCVNumber.Select();
                //        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                //        if (sourceCVNumber.Length == 4)
                //            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                //        if (sourceCVNumber.Length == 5)
                //            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                //        if (sourceCVNumber.Length == 6)
                //            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));
                //    }
                //}


            }

            else if (!businessRuleID.Equals(""))
            {

                string sqlCVNumber = "SELECT Top 1 (convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                                            + schemaName + "[ERSBusinessLogic] where ERSBusinessLogic_ID = " + businessRuleID;
                System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                DataRow[] drCVNumber = dtCVNumber.Select();
                sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                if (sourceCVNumber.Length == 4)
                    thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                if (sourceCVNumber.Length == 5)
                    thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                if (sourceCVNumber.Length == 6)
                    thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));

            }

            return thousandNumber;
        }



        private int getThoudandNumber(string businessRuleID, string status, string[] inputsArray, string outputDestinationval, int checkonceN, int checkonceCV)
        {
            string sourceCVNumber = "";
            int thousandNumber = 0;

            if (businessRuleID.Equals("") && status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
            {
                if (checkonceN == 1)
                {

                    if (outputDestinationval.Contains("N"))
                    {
                        string sqlCVNumber = "SELECT MAX(convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                                + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_OutputDestination] like '%CV(N%'";
                        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                        DataRow[] drCVNumber = dtCVNumber.Select();
                        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                        if (sourceCVNumber.Length == 4)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                        if (sourceCVNumber.Length == 5)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                        if (sourceCVNumber.Length == 6)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));

                        thousandNumber = thousandNumber + 1;

                    }

                }
                 if (checkonceCV == 1)
                {
                    if (outputDestinationval.Contains("N")==false)
                    {
                        string sqlCVNumber = "SELECT MAX(convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                        + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_OutputDestination] not like '%CV(N%'";
                        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                        DataRow[] drCVNumber = dtCVNumber.Select();
                        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                        if (sourceCVNumber.Length == 4)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                        if (sourceCVNumber.Length == 5)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                        if (sourceCVNumber.Length == 6)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));

                        thousandNumber = thousandNumber + 1;
                    }

                }


                if (checkonceN > 1)
                {
                    if (outputDestinationval.Contains("N"))
                    {
                        string sqlCVNumber = "SELECT MAX(convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                                + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_OutputDestination] like '%CV(N%'";
                        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                        DataRow[] drCVNumber = dtCVNumber.Select();
                        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                        if (sourceCVNumber.Length == 4)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                        if (sourceCVNumber.Length == 5)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                        if (sourceCVNumber.Length == 6)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));


                    }
                }

                if (checkonceCV > 1)
                {
                    if (outputDestinationval.Contains("N")==false)
                    {
                        string sqlCVNumber = "SELECT MAX(convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                        + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_OutputDestination] not like '%CV(N%'";
                        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                        DataRow[] drCVNumber = dtCVNumber.Select();
                        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                        if (sourceCVNumber.Length == 4)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                        if (sourceCVNumber.Length == 5)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                        if (sourceCVNumber.Length == 6)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));
                    }
                }


            }

            

             if(!businessRuleID.Equals(""))
            {
                string sqlCVNumber;

                if(outputDestinationval.Contains("N") == true)
                {
                    sqlCVNumber = "SELECT max (convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                                           + schemaName + "[ERSBusinessLogic] where ERSBusinessLogic_ID = " + businessRuleID + "and ERSBusinessLogic_OutputDestination like '%CV(N%'";
                }
                else
                {
                    sqlCVNumber = "SELECT max (convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination], 'CV', ''), '(N', ''), ')', ''))) AS CVNUMBER FROM "
                   + schemaName + "[ERSBusinessLogic] where ERSBusinessLogic_ID <= " + businessRuleID + "and ERSBusinessLogic_OutputDestination  not like '%CV(N%'";
                }
                
                System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                DataRow[] drCVNumber = dtCVNumber.Select();
                sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                if (sourceCVNumber.Length == 4)
                    thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                if (sourceCVNumber.Length == 5)
                    thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                if (sourceCVNumber.Length == 6)
                    thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));
                if (sourceCVNumber.Length == 1)
                    thousandNumber = Convert.ToInt32(sourceCVNumber);

            }


            return thousandNumber;
        }

        /// /

        private int getThoudandNumberNew(string businessRuleID, string status, string[] inputsArray, string outputDestinationval, int checkonceN, int checkonceCV)
        {
            string sourceCVNumber = "";
            int thousandNumber = 0;
            int countinput = 0;
            try
            {

                for (int counter = 0; counter < inputsArray.Length; counter++)
            {
                inputsArray[counter].Contains("CV");
                countinput = countinput + 1;
            }

                if (businessRuleID.Equals("") && status.Equals("New", StringComparison.InvariantCultureIgnoreCase))
                {



                    if (countinput > 0)
                    {
                        string sqlCVNumber = "SELECT MAX(convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                        + schemaName + "[ERSBusinessLogic] where [ERSBusinessLogic_OutputDestination] not like '%CV(N%'";
                        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                        DataRow[] drCVNumber = dtCVNumber.Select();
                        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                        if (sourceCVNumber.Length == 4)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                        if (sourceCVNumber.Length == 5)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                        if (sourceCVNumber.Length == 6)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));


                    }

                }

                    if (!businessRuleID.Equals(""))
                    {
                        string sqlCVNumber;

                        if (countinput > 0)
                        {
                            sqlCVNumber = "SELECT max (convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination], 'CV', ''), '(N', ''), ')', ''))) AS CVNUMBER FROM "
                          + schemaName + "[ERSBusinessLogic] where ERSBusinessLogic_ID <= " + businessRuleID + "and ERSBusinessLogic_OutputDestination  not like '%CV(N%'";

                        }
                        else
                        {
                            sqlCVNumber = "SELECT max (convert(int, REPLACE(REPLACE(REPLACE([ERSBusinessLogic_OutputDestination],'CV',''),'(N',''),')',''))) AS CVNUMBER FROM "
                                                   + schemaName + "[ERSBusinessLogic] where ERSBusinessLogic_ID = " + businessRuleID + "and ERSBusinessLogic_OutputDestination like '%CV(N%'";
                        }

                        System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                        DataRow[] drCVNumber = dtCVNumber.Select();
                        sourceCVNumber = drCVNumber[0]["CVNUMBER"].ToString();
                        if (sourceCVNumber.Length == 4)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 1));
                        if (sourceCVNumber.Length == 5)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 2));
                        if (sourceCVNumber.Length == 6)
                            thousandNumber = Convert.ToInt32(sourceCVNumber.Substring(0, 3));
                        if (sourceCVNumber.Length == 1)
                            thousandNumber = Convert.ToInt32(sourceCVNumber);

                    }
            }
            catch
            {
                thousandNumber = 1;
            }


            return thousandNumber;
        }



        //get output destination
        private string getOutputDestination(string status, string ruleID, string outputDestination, int thousandNumber, int numberToAdd)
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
                            errmsg = errmsg + "fail,";
                            throw new Exception("New data series " + outputDestination + " not found. Please make sure the new data series is available before adding this rule.");
                            
                        }
                        else
                        {
                            errmsg = errmsg + "pass,";
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
                    if (dr.Length == 1)
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



        private string getNewDataSeriesID(string dsid)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!       
           
            try
            {
                        string sql = "select ERSCommodity_ID from " + schemaName + "ERSCommodityDataSeries where ERSCommodity_ID = '" + dsid + "' and ERSCommodity_SourceSeriesID like '%N%'";
                        System.Data.DataTable dt = GetData(sql);
                        DataRow[] dr = dt.Select();
                        return dr[0]["ERSCommodity_ID"].ToString();
                    
                 
            }
            catch (Exception)
            {
                return "0";
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

        private void label49_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
                    label_ImportResultPath.Text = cosdFolderPath + "CoSD Tool" + "\\Business Rules Upload";
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
                MessageBox.Show("Data Processing unsuccessful. Please contact technical team.");
            }
        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void button_Browse1_Click(object sender, EventArgs e)
        {
            //enable sheet drop down
            comboBox_excelSheet1.Enabled = true;
          
            comboBox_excelSheet1.DataSource = null;
            sheetNames = new List<string>();
            // set filter to only allow excel files
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"; //only can chse excel files
            openFileDialog1.FileName = "NewDataSeries_Upload.xls";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                filePathNS = openFileDialog1.FileName;
                textBox_excelPath1.Text = filePathNS;
                Cursor.Current = Cursors.WaitCursor;
                object misValue = System.Reflection.Missing.Value;
                var oExcel = new Microsoft.Office.Interop.Excel.Application();
                //var workbook = oExcel.Workbooks;
                var oBook = oExcel.Workbooks.Open(filePathNS);
                //loads a book - above - and a sheet - below -
                foreach (Microsoft.Office.Interop.Excel.Worksheet worksheet in oBook.Worksheets)
                {
                    sheetNames.Add(worksheet.Name);
                }

                comboBox_excelSheet1.DataSource = sheetNames;
                oBook.Close(false, misValue, misValue);
                oExcel.Quit();

                Cursor.Current = Cursors.Default;
            }
        }


        private string getCommodityID(string commodityName, string groupid)
        {
            try
            {
                string sql = "select ERSCommoditySubCommodity_ID from " + schemaName + "ERSCommoditySubCommodity_LU where ERSCommoditySubCommodity_Desc = '" + commodityName + "' and [ERSCommoditySubCommodity_GroupID] = " + groupid;
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSCommoditySubCommodity_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }

        }


        private string getPhyAttrCatID(string phyAttrCat)
        {

            try
            {
                string sql = "select ERSPhysicalAttribute_ID from " + schemaName + "ERSPhysicalAttribute_LU where ERSPhysicalAttribute_Desc = '" + phyAttrCat + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSPhysicalAttribute_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }


        private string getProdPracID(string prodPrac)
        {
            try
            {
                string sql = "select ERSProdPractice_ID from " + schemaName + "ERSProdPractice_LU where ERSProdPractice_Desc = '" + prodPrac + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSProdPractice_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }


        private string getStatTypeID(string statType)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!
            try
            {
                string sql = "select ERSStatisticType_ID from " + schemaName + "ERSStatisticType_LU where ERSStatisticType_Attribute = '" + statType + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSStatisticType_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// load util practice id
        /// </summary>
        /// <param name="utilPrac"></param>
        /// <returns></returns>
        private string getUtilpracID(string utilPrac)
        {
            // if not found 
            try
            {
                string sql = "select ERSUtilPractice_ID from " + schemaName + "ERSUtilPractice_LU where ERSUtilPractice_Desc = '" + utilPrac + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSUtilPractice_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }


        private string getSourceID(string sourceDesc)
        {
            try
            {
                string sql = "select [ERSSource_ID] from " + schemaName + "[ERSSource_LU] where [ERSSource_Desc] = '" + sourceDesc + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSSource_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private string getSectorID(string sectordesc)
        {
            try
            {
                string sql = "select [ERSSector_ID] from " + schemaName + "[ERSSector_LU] where [ERSSector_Desc] like '%" + sectordesc + "%'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSSector_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }

        }


        private string getgroupIDfromdesc(string groupdesc)
        {
            
            try
            {
                string sql = "select [ERSGroup_ID] from " + schemaName + "[ERSGroup_LU] where [ERSGroup_Desc] = '" + groupdesc + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSGroup_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }

        }


        private string getCommoditySeriesID(string commodityid)
        {
            try
            {
               
                //commodityid = commodityid.Trim(';');
             
                string sql = "select [ERSCommodity_ID] from " + schemaName + "[ERSCommodityDataSeries] where [ERSCommodity_ID] in (" + commodityid + ")";
               
               
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSCommodity_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void button_importExcel1_Click(object sender, EventArgs e)                  //import new data series
        {


            if (filePathNS == null || filePathNS == "")
            {
                MessageBox.Show("Please choose an Excel file to Upload.");
                return;
            }
            if (comboBox_excelSheet1.Text == "")
            {
                MessageBox.Show("Please choose a sheet to Upload.");
                return;
            }

            // ask for conformation before proceed
            if (MessageBox.Show("Uploading excel may take few minutes to complete. Do you really want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                try
                {

                    label_status1.Text = "Uploading ...";

                    progressBar2.Value = 1; //Progress bar

                    textBox_newdataseries.Text = " ";

                    Microsoft.Office.Interop.Excel.Range range = null;
                    String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");

                    int index = 0;

                    var oExcel = new Microsoft.Office.Interop.Excel.Application();
                    //var workbooks = oExcel.Workbooks;
                    var oBook = oExcel.Workbooks.Open(filePathNS);

                    index = 1 + comboBox_excelSheet1.SelectedIndex;

                    var sheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.get_Item(index);

                    range = sheet.UsedRange;
                    //clear junk in excel sheet
                    sheet.Columns.ClearFormats();
                    sheet.Rows.ClearFormats();
                    //range.WrapText = true;
                    object[,] values = (object[,])range.Value2;


                    columnsCount = sheet.UsedRange.Columns.Count;

                    rowsCount = sheet.UsedRange.Rows.Count;

                    fullRow = sheet.Rows.Count;
                    Lastrow = sheet.Cells[fullRow, 1].End(Excel.XlDirection.xlUp).Row;

                    progressBar2.Maximum = (Lastrow - 1) * 2;
                    progressBar2.Step = 1;

                    int total = Lastrow - 1;


                    if (!validExcelFormatNS(values))
                    {
                        MessageBox.Show("Please check the format of selected Excel file.\nPlease refer to New data series upload sheet in COSD folder for correct format.");
                        label_status1.Text = "Upload failed. Please check the format of selected Excel file.";
                        oBook.Close(false, Type.Missing, Type.Missing);
                        oExcel.Quit();
                        //Marshal.ReleaseComObject(oBook);
                        //Marshal.ReleaseComObject(workbooks);
                        //Marshal.ReleaseComObject(oExcel);

                        return;
                    }

                    //adding Error Deatails column.
                    sheet.Cells[1, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                    sheet.Cells[1, columnsCount + 1] = "Error Details";

                    //Store list of valid keywords for formula from CoSD Keywords.TXT
                    string[] keywords = getKeywords();

                    progressBar2.PerformStep();

                    int check = CheckForErrorNS();

                    int checkonceN = 1;
                    string returnNS = "";
                    string idInserted = "";

                    if (check == 0)
                    {
                        for (int i = 2; i <= Lastrow; i++)
                        {

                            progressBar2.PerformStep();
                            //  progressBar2.Increment(1);

                            //check the value of status
                            if (null != values[i, statusIndexNS])
                            {

                                //get new id 

                                string newid;
                                if (values[i, newidIndexNS] == null)
                                    newid = "";
                                else
                                    newid = values[i, newidIndexNS].ToString().Trim();


                                //get new data series id

                                string newDSid;
                                if (values[i, NewDataseriesIndexNS] == null)
                                    newDSid = "";
                                else
                                    newDSid = values[i, NewDataseriesIndexNS].ToString().Trim();

                                //get status 

                                string statusNS;
                                if (values[i, statusIndexNS] == null)
                                    statusNS = "";
                                else
                                    statusNS = values[i, statusIndexNS].ToString().Trim();


                                // get unit
                                string unit = "";
                                if (null != values[i, unitIndexNS])
                                    unit = values[i, unitIndexNS].ToString();
                                string unitID = "";
                                try
                                {
                                    unitID = getUnitID(unit.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, unitIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }

                                //


                                //get group id

                                string group = "";
                                if (null != values[i, groupIndexNS])
                                    group = values[i, groupIndexNS].ToString();
                                string groupID = "";
                                try
                                {
                                    groupID = getgroupIDfromdesc(group.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, groupIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }





                                // get sub commodity id
                                string commodityName = "";
                                if (null != values[i, subcommodityIndexNS])
                                    commodityName = values[i, subcommodityIndexNS].ToString();
                                string commodityID = "";
                                try
                                {
                                    commodityID = getCommodityID(commodityName.Trim(), groupID);
                                }
                                catch (Exception)
                                {

                                    sheet.Cells[i, subcommodityIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }

                                // get physical attr id
                                string phyattr = "";
                                if (null != values[i, physicalAtttypeIndexNS])
                                    phyattr = values[i, physicalAtttypeIndexNS].ToString();
                                string phyattrid = "";
                                try
                                {
                                    phyattrid = getPhyAttrCatID(phyattr.Trim());
                                }
                                catch (Exception)
                                {

                                    sheet.Cells[i, physicalAtttypeIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }



                                // get physical attr desc
                                string phyAttrDesc;
                                if (values[i, physicalAttDescIndexNS] == null)
                                    phyAttrDesc = "";

                                else
                                    phyAttrDesc = values[i, physicalAttDescIndexNS].ToString().Trim();




                                // get prod practice
                                string prodPrac = "";
                                if (null != values[i, prodpracIndexNS])
                                    prodPrac = values[i, prodpracIndexNS].ToString();
                                string prodPracID = "";
                                try
                                {
                                    prodPracID = getProdPracID(prodPrac.Trim());
                                }
                                catch (Exception)
                                {

                                    sheet.Cells[i, prodpracIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }


                                // get util practice
                                string utilPrac = "";
                                if (null != values[i, utilpracIndexNS])
                                    utilPrac = values[i, utilpracIndexNS].ToString();
                                string utilPracID = "";
                                try
                                {
                                    utilPracID = getUtilpracID(utilPrac.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, utilpracIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }
                                // get stat type
                                string statType = "";
                                if (null != values[i, stattypeIndexNS])
                                    statType = values[i, stattypeIndexNS].ToString();
                                string statTypeID = "";
                                try
                                {
                                    statTypeID = getStatTypeID(statType.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, stattypeIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }

                                // get source
                                string sourceDesc = "";

                                if (null != values[i, sourceIndexNS])
                                    sourceDesc = values[i, sourceIndexNS].ToString();


                                string sourceID = "";
                                try
                                {
                                    sourceID = getSourceID(sourceDesc.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, sourceIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }

                                //get source long desc

                                string sourceLongDesc = "";
                                if (null != values[i, sourceseriesLongDescIndexNS])
                                    sourceLongDesc = values[i, sourceseriesLongDescIndexNS].ToString();


                                // get sector id

                                string sector = "";
                                if (null != values[i, sectorIndexNS])
                                    sector = values[i, sectorIndexNS].ToString();
                                string sectorID = "";
                                try
                                {
                                    sectorID = getSectorID(sector.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, sectorIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }




                                int thousandNumber;

                                thousandNumber = getThousandForNewDataSeries(newDSid, statusNS, checkonceN);




                                string commodityseries = "";
                                if (null != values[i, commoditySeriesIDIndexNS])
                                    commodityseries = values[i, commoditySeriesIDIndexNS].ToString();
                                string[] commodities = commodityseries.Split(';');
                                string commodityid = "";
                                int countcatch = 0;


                                foreach (var c in commodities)
                                {
                                    try
                                    {
                                        commodityid = getCommoditySeriesID(c);

                                    }
                                    catch
                                    {
                                        countcatch = countcatch + 1;
                                    }
                                }


                                if (countcatch > 0)
                                {
                                    sheet.Cells[i, commoditySeriesIDIndexNS].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }


                                // get category desc
                                string categoryDesc;
                                if (values[i, dataseriesdescrIndexNS] == null)
                                    categoryDesc = "";
                                else
                                    categoryDesc = values[i, dataseriesdescrIndexNS].ToString().Trim();



                                //creating commodity_sourceseriesid



                                string csourceseries = "";

                                int math = Convert.ToInt32(thousandNumber * 1000) + Convert.ToInt32(newid.Trim('N'));

                                csourceseries = "(N" + math.ToString() + ") " + commodityseries;

                                //


                                checkonceN = 2;

                                try
                                {

                                    string insertStatus = "";
                                    string updateStatus = "";
                                    string Nvalue = csourceseries;


                                    int indexSourceSeries = 0;

                                    indexSourceSeries = Nvalue.IndexOf(")");
                                    if (indexSourceSeries > 0)
                                        Nvalue = Nvalue.Substring(0, indexSourceSeries + 1);

                                    //insert to data series

                                    if (newDSid == "" && statusNS == "New")
                                    {
                                        insertStatus = insertNewDataSeries(unitID, commodityID, phyattrid, phyAttrDesc, prodPracID, utilPracID, statTypeID, sourceID, sourceLongDesc, sectorID, groupID, categoryDesc, csourceseries);



                                        if (insertStatus == "failed")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "Insertion Failed. Please make sure the input data is correct or contact technical team.";


                                        }
                                        else if (insertStatus == "success")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "New data series inserted successfully.";


                                        }

                                        else if (insertStatus == "duplicate")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "Insertion Failed. Cannot insert duplicate data series.";
                                        }

                                        idInserted = getInsertedId(csourceseries);

                                        //returnNS = returnNS + ", " + idInserted + "-" + Nvalue;

                                        returnNS = returnNS + newid + ":  " + idInserted + " - " + Nvalue + "\r\n";
                                    }

                                    else if (newDSid != "" && statusNS == "Override")
                                    {
                                        updateStatus = UpdateNewDataSeries(newDSid, unitID, commodityID, phyattrid, phyAttrDesc, prodPracID, utilPracID, statTypeID, sourceID, sourceLongDesc, sectorID, groupID, categoryDesc, csourceseries);

                                        if (updateStatus == "failed")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                            sheet.Cells[i, columnsCount + 1] = "Override Failed. Please make sure the input data is correct or contact technical team.";

                                        }
                                        else if (updateStatus == "success")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                            sheet.Cells[i, columnsCount + 1] = "New data series updated successfully.";
                                        }

                                        else if (updateStatus == "duplicate")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);

                                            sheet.Cells[i, columnsCount + 1] = "Override Failed. Similar data series already exist in the system.";
                                        }

                                        idInserted = getInsertedId(csourceseries);

                                        // returnNS = returnNS + ", " + idInserted + "-" + Nvalue;
                                        returnNS = returnNS + newid + ":  " + idInserted + " - " + Nvalue + "\r\n";
                                    }




                                }
                                catch (Exception)
                                {
                                    for (int k = 1; k <= columnsCount; k++)
                                        sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    continue;
                                }

                            }
                        }


                        // first check if target directory exist
                        // create one if not exist
                        if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Business Rules")) // name of the error file and location
                        {
                            Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Business Rules");
                        }

                        string fileName = cosdFolderPath + "CoSD Tool\\Business Rules\\NewDataSeries_Upload_" + Date + ".xls";
                        try
                        {
                            oBook.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                            false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                            oBook.Close();
                            oExcel.Quit();

                            //if (successNum == total)
                            //{
                            //if (total == 1)
                            label_status1.Text = successNum + " record(s) uploaded to the database. Please check " + fileName + " for details.";
                            textBox_newdataseries.Visible = true;
                            textBox_newdataseries.Text = "IDs of New data series inserted / overrided: \r\n" + returnNS.TrimStart();
                            // }
                            //else
                            //     label_status1.Text = successNum + " of " + total + " records have been uploaded. Please check " + fileName + " for any failed records.";
                            //     label_returnedNS.Text = "Ids of New data series inserted: " + returnNS.Substring(1);
                            successNum = 0;
                            total = 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to create result excel file. Please contact technical team\nError Details" + ex.Message);
                            //MessageBox.Show("Could Not save result file" + ex.Message);
                        }


                    }

                    else
                    {

                        oBook.Close(false, Type.Missing, Type.Missing);
                        oExcel.Quit();
                    }

                }
                catch (Exception ex)
                {
                    label_status1.Text = "Upload Failed!";
                    MessageBox.Show("Upload failed. Please contact technical team.\nError Details" + ex.Message);
                    //MessageBox.Show("Exception at " + ex.Message);
                }


            }

            button_Browse.Enabled = true;
            textBox_excelPath.Enabled = true;
            label_uploadBR.Enabled = true;
            button_importExcel.Enabled = true;

        }


        private int getThousandForNewDataSeries(string newDSid, string statusNS, int checkonceN)
        {
            int thousandNumber = 0;
            string sourceseriesNumber = "";
            int indexSS = 0;
            try
            {

                if (statusNS == "New")
            {

                    string sqlCVNumber = "select top (1) [ERSCommodity_ID],[ERSCommodity_SourceSeriesID], ERSCommodity_SourceSeriesID as NewSeriesNUMBER FROM "
                                        + schemaName + "[ERSCommodityDataSeries] where ERSCommodity_SourceSeriesID like '%(N%' order by ERSCommodity_ID desc";
                System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                DataRow[] drNewSeriesNumber = dtCVNumber.Select();
                sourceseriesNumber = drNewSeriesNumber[0]["NewSeriesNUMBER"].ToString();

               
                indexSS = sourceseriesNumber.IndexOf(")");
                if (indexSS > 0)
                    sourceseriesNumber = sourceseriesNumber.Substring(2, indexSS - 2);

                if (sourceseriesNumber.Length == 4)
                {
                    sourceseriesNumber = sourceseriesNumber.Substring(0, 1);
                    thousandNumber = Convert.ToInt32(sourceseriesNumber);
                }
                   
                else if(sourceseriesNumber.Length == 5)
                {
                    sourceseriesNumber = sourceseriesNumber.Substring(0, 2);
                    thousandNumber = Convert.ToInt32(sourceseriesNumber);
                }
                    
            
                if (checkonceN == 1)
                {
                    thousandNumber = thousandNumber + 1;
                }
              

            }

            else if (statusNS == "Override")
            {
                string sqlCVNumber = "select top (1) [ERSCommodity_ID],[ERSCommodity_SourceSeriesID], ERSCommodity_SourceSeriesID as NewSeriesNUMBER FROM "
                                     + schemaName + "[ERSCommodityDataSeries] where [ERSCommodity_ID] = " + newDSid;
                System.Data.DataTable dtCVNumber = GetData(sqlCVNumber);
                DataRow[] drNewSeriesNumber = dtCVNumber.Select();
                sourceseriesNumber = drNewSeriesNumber[0]["NewSeriesNUMBER"].ToString();

         
                indexSS = sourceseriesNumber.IndexOf(")");

                if (indexSS > 0)
                    sourceseriesNumber = sourceseriesNumber.Substring(2, indexSS - 2);

                if (sourceseriesNumber.Length == 4)
                {
                    sourceseriesNumber = sourceseriesNumber.Substring(0, 1);
                    thousandNumber = Convert.ToInt32(sourceseriesNumber);
                }
                    
                else if (sourceseriesNumber.Length == 5)
                    sourceseriesNumber = sourceseriesNumber.Substring(0, 2);
                thousandNumber = Convert.ToInt32(sourceseriesNumber);
            }

            }
            catch
            {
                thousandNumber = 1;
            }

            return thousandNumber;
        }


         private string getInsertedId(string csourceseries)
        {

            string sql = "select [ERSCommodity_ID] from " + schemaName + "[ERSCommodityDataSeries] where [ERSCommodity_SourceSeriesID] like '%" + csourceseries + "%'";
          
            // execute
            try
            {
                System.Data.DataTable dt = GetData(sql);

              
                // get current user            
                System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
               
             
                DataRow[] dr = dt.Select();

                return dr[0]["ERSCommodity_ID"].ToString();

            }
            catch (SqlException ex)
            {

                return "0";

            }
        }

        private string DuplicateCheck(string newid,string unitID, string commodityID, string phyattrid, string phyAttrDesc,
            string prodPracID, string utilPracID, string statTypeID, string sourceID, string sourceLongDesc,
            string sectorID, string groupID, string categoryDesc)
        {
            string sql = "select count(*) as Duplicates from " + schemaName +
    "ERSCommodityDataSeries where " +
    " ERSCommoditySubCommodity_ID = " + commodityID + " and " +
    "ERSCommodity_ERSSector_ID = " + sectorID + " and " +
    "ERSCommodity_ERSGroup_ID = " + groupID + " and " +
    "ERSCommodity_ERSPhysicalAttribute_ID = " + phyattrid + " and " +
     "ERSCommodity_PhysicalAttribute_Desc = '" + phyAttrDesc + "' and " +
     "ERSCommodity_ERSStatisticType_ID = " + statTypeID + " and " +
     "ERSCommodity_ERSProdPractice_ID = " + prodPracID + " and " +
    "ERSCommodity_ERSUtilPractice_ID = " + utilPracID + " and " +
    "ERSCommodity_DataSeriesCategory_Desc = '" + categoryDesc + "' and " +
    "ERSCommodity_SourceSeriesID_LongDesc= '" + sourceLongDesc + "' and " +
    "ERSCommodity_ERSSource_ID = " + sourceID + " and " +
    "((SUBSTRING([ERSCommodity_SourceSeriesID] , (CHARINDEX(')', [ERSCommodity_SourceSeriesID]) - 1), 1) = " + newid.Substring(1) +                                       //matches the char after ")" for sourceseries
    ") or (SUBSTRING([ERSCommodity_SourceSeriesID] , (CHARINDEX(')', [ERSCommodity_SourceSeriesID]) - 2), 2) =" + newid.Substring(1) + "))";


            try
            {
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["Duplicates"].ToString();

            }
            catch
            {
                return "0";
            }
        }


        //checks duplicate values for business rules   (ignoring columns with CVs))
        private string DuplicateCheckBR(string commoditySeriesID, int inputsCount, string inputs, string InputUnitIDs, string conversionFactorID, string macro, string outputName, string outputUnitID,
                        string formula, string inputTimeDimensionTypeValue, string inputTimeDimensionTypeID, string outputTimeDimensionTypeValue, string outputTimeDimensionTypeID, string inputgeoValueID,
                        string outputgeoValueID, string outputDestination, string privacyID, string BLType, string longDescription, string stringinputSources, string inputSourcesID, string groupID)
        {

            if (conversionFactorID == "NULL")
            {
                conversionFactorID = "IS NULL";
            }
            else
            {
                conversionFactorID = "=" + conversionFactorID;
            }



            string sql = "select count(*) as Duplicates from " + schemaName +
    "ERSBusinessLogic where " +
    "ERSBusinessLogic_InputsCount = " + inputsCount + " and " +
     "ERSBusinessLogic_ConvFactorID " + conversionFactorID + " and " +
     "ERSBusinessLogic_MacroDesc = '" + macro + "' and " +
    "ERSBusinessLogic_InputTimeDimensionValue = '" + inputTimeDimensionTypeValue + "' and " +
    "ERSBusinessLogic_InputTimeDimensionTypeID = " + inputTimeDimensionTypeID + " and " +
    "ERSBusinessLogic_InputGeographyDimensionID = " + inputgeoValueID + " and " +
    "ERSBusinessLogic_OutputGeographyDimensionID = " + outputgeoValueID + " and " +
     "ERSBusinessLogic_InputUnitID = '" + InputUnitIDs + "' and " +
    "ERSBusinessLogic_Type = '" + BLType + "' and " +
     "ERSBusinessLogic_PrivacyID= " + privacyID + " and " +
    "ERSBusinessLogic_LongDesc = '" + longDescription + "' and " +
     "ERSBusinessLogic_InputSources = '" + stringinputSources + "' and " +
    "ERSBusinessLogic_InputSourceID = " + inputSourcesID + " and " +
     "ERSBusinessLogic_OutputName = '" + outputName + "' and " +
    "ERSBusinessLogic_OutputUnitID = " + outputUnitID + " and " +
     "ERSBusinessLogic_OutputTimeDimensionValue = '" + outputTimeDimensionTypeValue + "' and " +
    "ERSBusinessLogic_OutputTimeDimensionTypeID = " + outputTimeDimensionTypeID + " and " +
     "ERSBusinessLogic_GroupID = " + groupID;

            try
            {
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["Duplicates"].ToString();

            }
            catch
            {
                return "0";
            }
        }

        private string insertNewDataSeries(string unitID, string commodityID, string phyattrid, string phyAttrDesc,
            string prodPracID, string utilPracID, string statTypeID, string sourceID, string sourceLongDesc,
            string sectorID, string groupID, string categoryDesc, string csourceseries)
        {

            string sql = "insert into " + schemaName +
    "ERSCommodityDataSeries " +
    "(ERSCommoditySubCommodity_ID, " +
    "ERSCommodity_ERSSector_ID, " +
    "ERSCommodity_ERSGroup_ID, " +
    "ERSCommodity_ERSPhysicalAttribute_ID, " +
     "ERSCommodity_PhysicalAttribute_Desc, " +
     "ERSCommodity_ERSStatisticType_ID, " +
     "ERSCommodity_ERSProdPractice_ID, " +
    "ERSCommodity_ERSUtilPractice_ID, ERSCommodity_SourceSeriesID,ERSCommodity_SourceSeriesID_LongDesc,ERSCommodity_DataSeriesCategory_Desc,ERSCommodity_ERSSource_ID ) " +
    "values " +
    "(" +
    commodityID + "," + sectorID + "," +  groupID  + "," + phyattrid + ", '" + phyAttrDesc + "'," + statTypeID + "," + prodPracID + "," + utilPracID +
    ",'" + csourceseries + "', '" + sourceLongDesc + "', '" + categoryDesc + "'," + sourceID +
    
    ")";

            // execute
            try
            {
                System.Data.DataTable dt = GetData(sql);
               
                if (null != dt)
                {
                    successNum++;
                    saveAction("New data series inserted successfully");
                    return "success";
                }
                else if (errorMessage == "duplicate")
                {
                   // errorMessage = "";
                    saveAction("Duplicate data series attempted to insert");
                    return "duplicate";
                }
                else
                    saveAction("Data values insertion failed");
                    return "failed";

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return "failed";

            }

           
        }


        private string UpdateNewDataSeries(string newDSid, string unitID, string subcommodityID, string phyattrid, string phyAttrDesc,
                  string prodPracID, string utilPracID, string statTypeID, string sourceID, string sourceLongDesc,
                  string sectorID, string groupID, string categoryDesc, string csourceseries)
        {

            string sql = "update " + schemaName +
    "ERSCommodityDataSeries " +
    "SET ERSCommoditySubCommodity_ID = " + subcommodityID + ", " +
    "ERSCommodity_ERSSector_ID = " + sectorID + ", " +
    "ERSCommodity_ERSGroup_ID = " + groupID + ", " +
    "ERSCommodity_ERSPhysicalAttribute_ID = " + phyattrid + ", " +
     "ERSCommodity_PhysicalAttribute_Desc =  '" + phyAttrDesc + "', " +
     "ERSCommodity_ERSStatisticType_ID = " + statTypeID + ", " +
     "ERSCommodity_ERSProdPractice_ID = " + prodPracID + ", " +
    "ERSCommodity_ERSUtilPractice_ID = " + utilPracID +  ", " +
    "ERSCommodity_SourceSeriesID = '" + csourceseries + "', " +
    "ERSCommodity_SourceSeriesID_LongDesc = '" + sourceLongDesc + "', " +
    "ERSCommodity_DataSeriesCategory_Desc = '" + categoryDesc + "', " +
    "ERSCommodity_ERSSource_ID= " + sourceID +
    " where [ERSCommodity_ID] = " + newDSid;

            // execute
            try
            {
                System.Data.DataTable dt = GetData(sql);

                if (null != dt)
                {
                    successNum++;
                    saveAction("New data series updated successfully");
                    return "success";
                }
                else if (errorMessage == "duplicate")
                {
                    errorMessage = "";
                    saveAction("Duplicate data series attempted to insert");
                    return "duplicate";
                }
                else
                    saveAction("New data series update failed");
                    return "failed";

            }
            catch (SqlException ex)
            {

                return "failed";

            }


        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            button_Browse.Enabled = true;
            textBox_excelPath.Enabled = true;
            label_uploadBR.Enabled = true;
            button_importExcel.Enabled = true;
        }

        private void label43_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
