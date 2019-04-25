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
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoSD_Tool
{
    public partial class ArchiveForm : Form
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
        /// The table
        /// </summary>
        DataTable table = new DataTable();
        /// <summary>
        /// The binding source1
        /// </summary>
        private BindingSource bindingSourcePhyAtt = new BindingSource();
       
        /// <summary>
        /// List of physical attributes
        /// </summary>
        List<string> physicalAttributes = new List<string>();
        /// <summary>
        /// defined DB name and schema name
        /// </summary>
        private string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        private BindingSource bindingSource1 = new BindingSource();
        WaitForm waitForm = new WaitForm();
        LoadingForm loadform = new LoadingForm();
        StringBuilder deletedData = new StringBuilder();
        private List<string> sourceList;
        StringBuilder sourceIDs = new StringBuilder();
        StringBuilder dataSeriesIDs = new StringBuilder();
        DataTable DT = new DataTable();
        List<string> prodPrac;
        List<string> utilPrac;
        string commoditySubCommodityID = "-1";
        string physicalAttributeDetail = "";
        string prodPracticeID = "-1";
        string utilPracticeID = "-1";
        string statID = "-1";
        string sourceID = "-1";
        int totalDataValues = 0;
        public static string commodityName = "";
        public static string dataVaueSource = "";
        DataTable DTDataSeries = new DataTable();
        string inValidSourcesForDelete = "1,2,3,4,5,6,7,8,9,10,11,16,20,25,37";
        /// <summary>
        /// The current page
        /// </summary>
        int currentPage = 1;
        /// <summary>
        /// The items per page
        /// </summary>
        int itemsPerPage = 15;
        /// <summary>
        /// The total items
        /// </summary>
        int totalItems = 0;
        /// <summary>
        /// The total page
        /// </summary>
        int totalPage = 0;
        /// <summary>
        /// The data adapter
        /// </summary>
        private SqlDataAdapter dataAdapterForPaging = new SqlDataAdapter();

        string apGroupId = LandingScreen.GetGroupId(LandingScreen.newschemaName);

        private int columnNumInResultTable = 16;  // shows how many columns to display in data grid view
        int groupNum;   // group id of commodity
        private List<string> groups;  // store groups of commodities, refer to group LU table
        private List<string> groupIds;
        private List<string> commodities;  // store commodity names, refer to commoditySubCommodity LU table
        List<string> physicalAttributeCats; // store physical attribute categories, refer to physicalAttribute LU table
        private List<string> statTypes; // store stat types, refer to statType LU table
        private List<string> countryList;

        public ArchiveForm()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
            dataGridView.DataSource = bindingSource1;
        }

        // method connects db
        /// <summary>
        /// Connections this instance.
        /// </summary>
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


                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);


                DataTable result = new DataTable();
                dataAdapter.Fill(result);


                return result;

            }
            catch (Exception ex)
            {
                throw new Exception("Data processing unsuccessful, please contact technical team.");
            }
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
        /// action when loading the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Load(object sender, EventArgs e)
        {
            try
            {
                // fill group
                // int[] groupIDs = new int[4] { 1, 2, 3, 4 };   // group id used for VEG tool, refer to group LU table 
                // fill all the comboBox on the screen by loading DB
                //groupIds = new List<string>();

               // groupIds = getGroupIds();

                fillGroupCombobox();
                // load country, region, and state
                countryList = new List<string>();
                string sql = "select distinct ERSGeographyDimension_Country from " + schemaName + "ERSGeographyDimension_LU WHERE ERSGeographyDimension_ERSGeographyType_ID =1 ORDER BY ERSGeographyDimension_Country";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                countryList.Add("UNITED STATES");
                foreach (DataRow row in dr)
                {
                    countryList.Add(row["ERSGeographyDimension_Country"].ToString());
                }

                comboBox_country.DataSource = countryList;

                sql = "select distinct ERSGeographyDimension_Region from " + schemaName + "ERSGeographyDimension_LU ORDER BY ERSGeographyDimension_Region";
                DataTable dt2 = GetData(sql);
                comboBox_region.DataSource = dt2;
                comboBox_region.ValueMember = dt2.Columns[0].ColumnName;
                comboBox_region.DisplayMember = dt2.Columns[0].ColumnName;

                sql = "select distinct ERSGeographyDimension_State from " + schemaName + "ERSGeographyDimension_LU ORDER BY ERSGeographyDimension_State";
                DataTable dt3 = GetData(sql);
                comboBox_state.DataSource = dt3;
                comboBox_state.ValueMember = dt3.Columns[0].ColumnName;
                comboBox_state.DisplayMember = dt3.Columns[0].ColumnName;

                sql = "select distinct ERSGeographyDimension_County from " + schemaName + "ERSGeographyDimension_LU ORDER BY ERSGeographyDimension_County";
                DataTable dt4 = GetData(sql);
                comboBox_county.DataSource = dt4;
                comboBox_county.ValueMember = dt4.Columns[0].ColumnName;
                comboBox_county.DisplayMember = dt4.Columns[0].ColumnName;

                // set the font and size for data grid view
                dataGridView.RowsDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Regular);

                // clear all combobox when loading
                comboBox_commodity.Text = "";
                comboBox_country.Text = "";
                comboBox_group.Text = "";
                comboBox_phyAttrDetail.Text = "";
                comboBox_prodPrac.Text = "";
                comboBox_region.Text = "";
                comboBox_state.Text = "";
                comboBox_statType.Text = "";
                comboBox_unit.Text = "";
                comboBox_utilPrac.Text = "";
                comboBox_county.Text = "";
                button_next.Enabled = false;
                button_next.BackColor = Color.Gray;
                button_prev.Enabled = false;
                button_prev.BackColor = Color.Gray;

                //disbale comboboxes
                comboBox_commodity.Enabled = false;
                comboBox_phyAttrDetail.Enabled = false;
                comboBox_prodPrac.Enabled = false;
                comboBox_utilPrac.Enabled = false;
                comboBox_statType.Enabled = false;
                comboBox_unit.Enabled = false;
            }
            catch
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }
        }

        private void fillUtilPracComboBox(int groupID)  // fill utilization practice comboBox
        {
            utilPrac = new List<string>();
            string sql = "select [ERSUtilPractice_Desc] from " + schemaName + "ERSUtilPractice_LU WHERE ERSUtilPractice_ID IN (SELECT DISTINCT[ERSCommodity_ERSUtilPractice_ID] FROM " + schemaName + "[ERSCommodityDataSeries]   WHERE [ERSCommodity_ERSGroup_ID] = " + groupNum + " ) ORDER BY ERSUtilPractice_Desc";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            utilPrac.Add("");
            foreach (DataRow row in dr)
            {
                utilPrac.Add(row["ERSUtilPractice_Desc"].ToString());
            }
            comboBox_utilPrac.DataSource = utilPrac;
        }

        private void fillSourceCombobox(int groupID)
        {
            sourceList = new List<string>();
            string sql = "select DISTINCT ERSSource_Desc from " + schemaName + "ERSSource_LU WHERE ERSSource_ID IN (SELECT DISTINCT[ERSCommodity_ERSSource_ID] FROM  " + schemaName + "[ERSCommodityDataSeries]   WHERE [ERSCommodity_ERSGroup_ID] = " + groupNum + " ) ORDER BY ERSSource_Desc";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            sourceList.Add("");
            foreach (DataRow row in dr)
            {
                sourceList.Add(row["ERSSource_Desc"].ToString());
            }
            comboBox_source.DataSource = sourceList;
        }

        private string getUtilPracID(string utilPrac)   // translate utilization practice into util prac id
        {
            try
            {
                string sql = "select ERSUtilPractice_ID from " + schemaName + "ERSUtilPractice_LU where ERSUtilPractice_Desc = '" + utilPrac + "'";
                DataTable dt = GetData(sql);
                if (null != dt && dt.Rows.Count != 0)
                {
                    DataRow[] dr = dt.Select();
                    return dr[0]["ERSUtilPractice_ID"].ToString();
                }
                else
                    return "-1";
            }
            catch (Exception)
            {
                return "";
                // throw;
            }
        }




        private void fillProdPracComboBox(int groupID)  // fill production practice combobox
        {
            prodPrac = new List<string>();
            string sql = "select [ERSProdPractice_Desc] from " + schemaName + "ERSProdPractice_LU WHERE ERSProdPractice_ID IN (SELECT DISTINCT[ERSCommodity_ERSProdPractice_ID] FROM " + schemaName + "[ERSCommodityDataSeries]  WHERE [ERSCommodity_ERSGroup_ID] = " + groupNum + " ) ORDER BY ERSProdPractice_Desc";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            prodPrac.Add("");
            foreach (DataRow row in dr)
            {
                prodPrac.Add(row["ERSProdPractice_Desc"].ToString());
            }
            comboBox_prodPrac.DataSource = prodPrac;
        }

        private string getProdPracID(string prodPrac)
        {
            try
            {
                string sql = "select ERSProdPractice_ID from " + schemaName + "ERSProdPractice_LU where ERSProdPractice_Desc = '" + prodPrac + "'";
                DataTable dt = GetData(sql);
                if (null != dt && dt.Rows.Count != 0)
                {
                    DataRow[] dr = dt.Select();
                    return dr[0]["ERSProdPractice_ID"].ToString();
                }
                else
                    return "-1";
            }
            catch (Exception)
            {
                return "";
                // throw;
            }
        }

        private void fillStatTypeCombobox(int groupID)
        {
            statTypes = new List<string>();
            string sql = "select [ERSStatisticType_Attribute] from " + schemaName + "ERSStatisticType_LU WHERE ERSStatisticType_ID IN (SELECT DISTINCT[ERSCommodity_ERSStatisticType_ID] FROM  " + schemaName + "[ERSCommodityDataSeries]   WHERE [ERSCommodity_ERSGroup_ID] = " + groupNum + " ) ORDER BY ERSStatisticType_Attribute";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            statTypes.Add("");
            foreach (DataRow row in dr)
            {
                statTypes.Add(row["ERSStatisticType_Attribute"].ToString());
            }
            comboBox_statType.DataSource = statTypes;
        }

        /// <summary>
        /// translate stat type name into id
        /// </summary>
        /// <returns></returns>
        private string getStatTypeID()
        {
            string sql = "select ERSStatisticType_ID from " + schemaName + "ERSStatisticType_LU where ERSStatisticType_Attribute = '" + comboBox_statType.Text + "'";
            DataTable dt = GetData(sql);
            if (null != dt && dt.Rows.Count != 0)
            {
                DataRow[] dr = dt.Select();
                return dr[0]["ERSStatisticType_ID"].ToString();
            }
            else
                return "-1";
        }

        /// <summary>
        /// translate source name into id
        /// </summary>
        /// <returns></returns>
        private string getSourceID()
        {
            string sql = "select ERSSource_ID from " + schemaName + "ERSSource_LU where ERSSource_Desc = '" + comboBox_source.Text + "'";
            DataTable dt = GetData(sql);
            if (null != dt && dt.Rows.Count != 0)
            {
                sourceIDs = new StringBuilder();
                DataRow lastRow = dt.Rows[dt.Rows.Count - 1];
                string lastItem = lastRow[0].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    //dataSeriesID.Add(dr[0].ToString());
                    if (dr[0].ToString() != lastItem)
                        sourceIDs.Append(dr[0].ToString() + ",");
                    else sourceIDs.Append(dr[0].ToString());
                }
                return sourceIDs.ToString();
            }
            else
                return "-1";

        }

        private void fillUnit()  // fill unit combobox
        {
            string sql = "select[ERSUnit_Desc] from " + schemaName + "ERSUnit_LU WHERE ERSUnit_ID IN  (SELECT DISTINCT[ERSDataValues_ERSUnit_ID] FROM " + schemaName + "[ERSDataValues]) ORDER BY ERSUnit_Desc";
            DataTable dt = GetData(sql);
            //   unitTypeTable.Merge(dt);
            if (dt.Rows.Count == 0)
            {
                comboBox_unit.DataSource = null;
                return;
            }
            comboBox_unit.DataSource = dt;
            comboBox_unit.ValueMember = dt.Columns[0].ColumnName;
            comboBox_unit.DisplayMember = dt.Columns[0].ColumnName;
            comboBox_unit.SelectedItem = null;
        }

        /// <summary>
        /// search unit id with given unit name
        /// </summary>
        /// <param name="unitName"> unit name </param>
        /// <returns> unit id in db </returns>
        private string getUnitID(string unitName)
        {
            if (unitName == "")
            {
                return "-1";
            }

            try
            {
                string sql = "select ERSUnit_ID from " + schemaName + "ERSUnit_LU where ERSUnit_Desc = '" + unitName + "'";
                DataTable dt = GetData(sql);
                if (null != dt && dt.Rows.Count != 0)
                {
                    DataRow[] dr = dt.Select();
                    return dr[0]["ERSUnit_ID"].ToString();
                }
                else
                    return "-1";
            }
            catch (Exception)
            {
                return "";
                // throw;
            }
        }
        private void fillPhysicalAttribute(int groupID)
        {
            physicalAttributeCats = new List<string>();
            string sql = "select DISTINCT[ERSCommodity_PhysicalAttribute_Desc] from " + schemaName + "[ERSCommodityDataSeries] WHERE [ERSCommodity_ERSGroup_ID] = " + groupNum + " ORDER BY ERSCommodity_PhysicalAttribute_Desc";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            physicalAttributeCats.Add("");
            foreach (DataRow row in dr)
            {
                physicalAttributeCats.Add(row["ERSCommodity_PhysicalAttribute_Desc"].ToString());
            }
            comboBox_phyAttrDetail.DataSource = physicalAttributeCats;
            comboBox_phyAttrDetail.SelectedItem = null;
        }


        /// <summary>
        /// translate group names
        /// </summary>
        /// <param name="groupIDs"></param>
        private void fillGroupCombobox()
        {
            groupIds = new List<string>();
            groupIds = getGroupIds();
            if (groupIds.Count == 0)
                return;
            groups = new List<string>();
            foreach (string groupID in groupIds)
            {
                string sql = "select ERSGroup_Desc from " + schemaName + "ERSGroup_LU where ERSGroup_ID = " + groupID + " ORDER BY ERSGroup_Desc";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                foreach (DataRow row in dr)
                {
                    groups.Add(row["ERSGroup_Desc"].ToString());
                }
            }
            comboBox_group.DataSource = groups;
            comboBox_group.SelectedItem = null;
        }
        /*Retrieve groups according to logged in user
         */
        private List<string> getGroupIds()
        {
            groupIds = new List<string>();
            // get current user            
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            string sql = "select ERSGroup_ID from " + schemaName + "ERSGroup_LU where ERSGroup_Username like '%" + currentUser.Name + "%' and [ERSGroup_ID] IN (" + apGroupId + ")";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            foreach (DataRow row in dr)
            {
                groupIds.Add(row["ERSGroup_ID"].ToString());
            }
            return groupIds;
        }
        /// <summary>
        /// action when the retrieve button is click
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_retrieve_Click(object sender, EventArgs e)
        {
//            currentPage = 1;

//            // verify every input
//            if (comboBox_group.Text.Equals(""))
//            {
//                MessageBox.Show("Please choose a commodity group.");
//                return;
//            }
//            if (comboBox_commodity.Text.Equals(""))
//            {
//                MessageBox.Show("Please choose a commodity.");
//                return;
//            }


//            // get dataseries ID
//            // load variable used from gui
//            //string commoditySubCommodityID = getCommodityID(comboBox_commodity.Text);
//            //string physicalAttributeDetail = comboBox_phyAttrDetail.Text;
//            //int groupID = getGroupID(comboBox_group.Text);
//            //int prodPracticeID = int.Parse(getProdPracID(comboBox_prodPrac.Text));
//            //int utilPracticeID = int.Parse(getUtilPracID(comboBox_utilPrac.Text));
//            //int statID = int.Parse(getStatTypeID());
//            //string sourceID = getSourceID();
//            // look up dataseries id
//            List<string> dataSeriesID = getDataSeriesID(commoditySubCommodityID, physicalAttributeDetail, prodPracticeID, utilPracticeID, statID, sourceID);

//            // if o data series exists for current selection, return
//            if (dataSeriesID.Equals("") || null == dataSeriesID || dataSeriesID.Count == 0)
//            {
//                resetResult();
//                return;
//            }

//            // search values
//            int unitID = int.Parse(getUnitID(comboBox_unit.Text));
//            //int statID = int.Parse(getStatTypeID());
//            //int statID = 0;

//            // need to get geo id 
//            string geoID = getGeoID();

//            // need to get time id
//            string timeID = getTimeID();

//            try
//            {
//                // construct a sql statement of searching
//                string sqlStmt = getDataValues(dataSeriesID, unitID, statID, geoID, timeID);
//                if (sqlStmt == "")
//                {
//                    resetResult();
//                    return;
//                }
//                connection();
//                dataAdapterForPaging = new SqlDataAdapter(sqlStmt, con);
//            }
//            catch (Exception exception)
//            {
//                resetResult();
//                return;
//            }
//            // Create a command builder to generate SQL update, insert, and
//            // delete commands based on sqlStmt. These are used to
//            // update the database.
//            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapterForPaging);

//            // split pages
//            DataTable result = new DataTable();
//            DataTable resultPerPage = new DataTable();
//            // calculate the total number of items and total pages needed
//            dataAdapterForPaging.Fill(result);
//            totalItems = result.Rows.Count;

//            if (totalItems == 0)
//            {
//                MessageBox.Show("No record found with given selections, please adjust your searching criteria and try again.");
//                resetResult();
//                return;
//            }


//            columnNumInResultTable = result.Columns.Count;
//            totalPage = totalItems / itemsPerPage + 1;
//            // fill the first page
//            dataAdapterForPaging.Fill(0, totalItems, resultPerPage);

//            DataTable dtCloned = resultPerPage.Clone();
//            for (int i = 0; i < columnNumInResultTable; i++)
//                dtCloned.Columns[i].DataType = typeof(string);
//            foreach (DataRow row in resultPerPage.Rows)
//            {
//                dtCloned.ImportRow(row);
//            }

//            // translate unit id into unit name
//            // translate stattype id into stattype name
//            /*
//            foreach (DataRow row in dtCloned.Rows)
//            {
//                string statTypeID1 = row["StatType1"].ToString();
//                string statTypeID2 = row["StatType2"].ToString();
//                string statTypeID3 = row["StatType3"].ToString();
//                string unitID1 = row["Unit1"].ToString();
//                string unitID2 = row["Unit2"].ToString();
//                string unitID3 = row["Unit3"].ToString();
//                if (!statTypeID1.Equals(""))
//                {
//                    row["StatType1"] = getTypeName(statTypeID1);
//                }
//                if (!statTypeID2.Equals(""))
//                {
//                    row["StatType2"] = getTypeName(statTypeID2);
//                }
//                if (!statTypeID3.Equals(""))
//                {
//                    row["StatType3"] = getTypeName(statTypeID3);
//                }
//                if (!unitID1.Equals(""))
//                {
//                    row["Unit1"] = getUnitName(unitID1);
//                }
//                if (!unitID2.Equals(""))
//                {
//                    row["Unit2"] = getUnitName(unitID2);
//                }
//                if (!unitID3.Equals(""))
//                {
//                    row["Unit3"] = getUnitName(unitID3);
//                }
//                // filter out NULL in results
//                for (int i = 0; i < columnNumInResultTable; i++)
//                {
//                    if (row[i].Equals("NULL"))
//                        row[i] = "";
//                }
//            }
//*/

//            // show current page number and total page number
//            label_pageNum.Text = "Page " + currentPage + " of " + totalPage;

//            // pop to datagrid
//            bindingSource1.DataSource = dtCloned;

//            // set color of column
//            try
//            {
//                for (int i = 0; i < columnNumInResultTable; i++)
//                {

//                    dataGridView.Columns[i].DefaultCellStyle.ForeColor = Color.Gray;
//                    dataGridView.Columns[i].ReadOnly = true;
//                }
//            }
//            catch (Exception ex)
//            {

//            }
//            // set color of column
//            if (dataGridView.DataSource != null)
//            {
//                dataGridView.Columns["Value"].DefaultCellStyle.ForeColor = Color.Black;
//                dataGridView.Columns["Value"].DefaultCellStyle.BackColor = Color.White;
//            }

//            button_next.Enabled = true;
//            button_next.BackColor = Color.White;
//            button_prev.Enabled = true;
//            button_prev.BackColor = Color.White;


            currentPage = 1;
            dataAdapterForPaging = null;
            // validate input values!!!!!!!!!!!!
            if (comboBox_group.Text.Equals(""))
            {
                MessageBox.Show("Please choose a Group value to continue.");
                return;
            }
            if (comboBox_commodity.Text.Equals(""))
            {
                MessageBox.Show("Please choose a commodity to continue.");
                return;
            }
            // get dataseries ID
            // load variable used from gui


            int groupID = getGroupID(comboBox_group.Text);

            // search values
            int unitID = int.Parse(getUnitID(comboBox_unit.Text));

            //int statID = 0;

            // need to get geo id 
            string geoID = getGeoID();

            // need to get time id
            //string timeID = getTimeID();

            if (geoID.Equals("NO MATCH GEO ID"))
            {
                MessageBox.Show("System cannot find any data with given geography input, please adjust your search conditions and try again.");
                resetResult();
                return;
            }

            //if (timeID.Equals("NO MATCH TIME ID"))
            //{
            //    MessageBox.Show("System cannot find any data with given time input, please adjust your search conditions and try again.");
            //    resetResult();
            //    return;
            //}


            try
            {  // build a sql statement for searching
                waitForm.Show(this);
                waitForm.Refresh();
                string sqlcount = getDataValuesCount(dataSeriesIDs, unitID, geoID);
                waitForm.Hide();
                totalDataValues = 0;
                DataTable dt = GetData(sqlcount);
                DataRow[] dr = dt.Select();
                totalDataValues = (int)dr[0]["Records"];

                if (totalDataValues == 0)
                {
                    MessageBox.Show("No Records found. Please adjust filter conditions to view data.");
                    resetResult();
                    return;
                }
                else if (totalDataValues > 10000)
                {
                    MessageBox.Show("Total '" + totalDataValues + "' Data Values found. Please filter more to view data.");
                    resetResult();
                    return;
                }

                waitForm.Show(this);
                waitForm.Refresh();
                string sqlStmt = getDataValues(dataSeriesIDs, unitID, geoID);
                if (sqlStmt.Equals(""))
                    return;
                connection();
                dataAdapterForPaging = new SqlDataAdapter(sqlStmt, con);
            }
            catch (Exception exception)
            {
                waitForm.Hide();
                resetResult();
                return;
            }


            // Create a command builder to generate SQL update, insert, and
            // delete commands based on sqlStmt. These are used to
            // update the database.
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapterForPaging);

            // split pages

            DataTable resultPerPage = new DataTable();
            DataTable result = new DataTable();
            // get the total number of items and pages
            dataAdapterForPaging.Fill(result);
            DT = result;
            if (totalDataValues <= 10000)
            {
                totalItems = result.Rows.Count;

                if (totalItems == 0)  //no available data
                {
                    waitForm.Hide();
                    MessageBox.Show("No record found with given selections, please adjust your searching criteria and try again.");
                    resetResult();
                    return;
                }
                columnNumInResultTable = result.Columns.Count;
                totalPage = totalItems / itemsPerPage + 1;
                // load the content of the first page
                dataAdapterForPaging.Fill(0, totalItems, resultPerPage);

                DataTable dtCloned = resultPerPage.Clone();
                for (int i = 0; i < columnNumInResultTable; i++)
                    dtCloned.Columns[i].DataType = typeof(string);

                foreach (DataRow row in resultPerPage.Rows)
                {
                    dtCloned.ImportRow(row);
                }

                // display current page number and total page number
                label_pageNum.Text = "Page " + currentPage + " of " + totalPage;

                // pop to datagrid
                bindingSource1.DataSource = dtCloned;

                try
                {
                    for (int i = 0; i < columnNumInResultTable; i++)
                    {
                        dataGridView.Columns[i].DefaultCellStyle.ForeColor = Color.Gray;
                        dataGridView.Columns[i].ReadOnly = true;

                    }
                }
                catch (Exception ex)
                {

                }
                // set color of column
                if (dataGridView.DataSource != null)
                {
                    dataGridView.Columns["Value"].DefaultCellStyle.ForeColor = Color.Black;
                    dataGridView.Columns["Value"].DefaultCellStyle.BackColor = Color.White;
                    dataGridView.Columns["Value"].ReadOnly = false;
                }
                button_next.Enabled = true;
                button_next.BackColor = Color.White;
                button_prev.Enabled = true;
                button_prev.BackColor = Color.White;
            }

            waitForm.Hide();


        }
        /// <summary>
        /// get time dimension id
        /// </summary>
        /// <returns>
        /// if no match record, return a string "NO MATCH TIME ID"
        /// if all empty, means no search criteria for time, return "NO NEED TIME ID"
        /// else return time id
        /// </returns>
        //private string getTimeID()
        //{
        //    string yearNum = yearValue.Text;
        //    string monthNum = monthValue.Text;
        //    //string weekNum = weekValue.Text;

        //    if (yearNum.Equals("") && monthNum.Equals(""))
        //    {
        //        return "NO NEED TIME ID";
        //    }
        //    if (yearNum.Equals(""))
        //        yearNum = "";
        //    else
        //        yearNum = " and ERSTimeDimension_Year = '" + yearNum + "'";
        //    if (monthNum.Equals(""))
        //        monthNum = "";
        //    else
        //        monthNum = " and ERSTimeDimension_Month = '" + monthNum + "'";

        //    string sql = " Select ERSTimeDimension_ID from " + schemaName + "ERSTimeDimension_LU where 1=1 " + yearNum + monthNum;
        //    //System.Data.DataTable dt = GetData(sql);

        //    //if (null == dt || dt.Rows.Count <= 0)
        //    //{
        //    //    return "NO MATCH TIME ID";
        //    //}
        //    //else
        //    //{
        //    //    DataRow[] dr = dt.Select();
        //    //    return dr[0]["ERSTimeDimension_ID"].ToString();
        //    //}
        //    return sql;
        //}

        /// <summary>
        /// get geo dimension id
        /// </summary>
        /// <returns>
        /// if no match record, return a string "NO MATCH GEO ID"
        /// if all empty, means no search criteria for time, return "NO NEED GEO ID"
        /// else return GEO id
        /// </returns>
        private string getGeoID()
        {
            string country = comboBox_country.Text;
            string region = comboBox_region.Text;
            string state = comboBox_state.Text;
            string county = comboBox_county.Text;

            if (country.Equals("") && region.Equals("") && state.Equals("") && county.Equals(""))
            {
                return "NO NEED GEO ID";
            }

            // if anyone is empty, make it NULL in sql
            if (country.Equals(""))
                country = "";
            else
                country = " and ERSGeographyDimension_Country = '" + country + "'";
            if (region.Equals(""))
                region = "";
            else
                region = " and ERSGeographyDimension_Region = '" + region + "' ";
            if (state.Equals(""))
                state = "";
            else
                state = " and ERSGeographyDimension_State = '" + state + "' ";
            if (county.Equals(""))
                county = "";
            else
                county = " and ERSGeographyDimension_County = '" + county + "' ";

            string sql = "select ERSGeographyDimension_ID from " +
                schemaName + "ERSGeographyDimension_LU where 1=1 " + country +
                region +
                 state +
                county;
            //System.Data.DataTable dt = GetData(sql);

            //if (null == dt || dt.Rows.Count <= 0)
            //{
            //    return "NO MATCH GEO ID";
            //}
            //else
            //{
            //    DataRow[] dr = dt.Select();
            //    return dr[0]["ERSGeographyDimension_ID"].ToString();
            //}
            return sql;
        }

        /// <summary>
        /// TRANSLATE UNIT NAME WITH GIVEN UNIT ID
        /// </summary>
        /// <param name="unitID1"></param>
        /// <returns></returns>
        private string getUnitName(string unitID1)
        {
            string unitName = "";
            string sqlStr = "select ERSUnit_Desc from " +
                    schemaName + "ERSUnit_LU where ERSUnit_ID = " + unitID1;
            DataTable dt = GetData(sqlStr);
            // if exist, return ID
            if (dt.Rows.Count > 0)
            {
                DataRow[] rows = dt.Select();
                unitName = (rows[0]["ERSUnit_Desc"]).ToString();
            }

            return unitName;
        }
        /// <summary>
        /// TRANSLATE STAT TYPE NAME WITH GIVEN STAT TYPE ID
        /// </summary>
        /// <param name="unitID1"></param>
        /// <returns></returns>
        private string getTypeName(string statTypeID1)
        {
            string statTypename = "";
            string sqlStr = "select ERSStatisticType_Attribute from " +
                    schemaName + "ERSStatisticType_LU where ERSStatisticType_ID = " + statTypeID1;
            DataTable dt = GetData(sqlStr);
            // if exist, return ID
            if (dt.Rows.Count > 0)
            {
                DataRow[] rows = dt.Select();
                statTypename = (rows[0]["ERSStatisticType_Attribute"]).ToString();
            }
            return statTypename;
        }


        /// <summary>
        /// load data from datavalues table,
        /// return a string of sql only. Do not execute it.
        /// </summary>
        /// <param name="dataSeriesID">dataSeries ID</param>
        /// <param name="unitID">Unit ID</param>
        /// <param name="statID">StatType ID</param>
        /// <returns></returns>
        private string getDataValues(StringBuilder dataSeriesIDs, int unitID, string geoID)
        {

            if (geoID.Equals("NO MATCH GEO ID"))
            {
                MessageBox.Show("System cannot find any data with given geography input, please adjust your search conditions and try again.");
                return "";
            }

            //if (timeID.Equals("NO MATCH TIME ID"))
            //{
            //    MessageBox.Show("System cannot find any data with given time input, please adjust your search conditions and try again.");
            //    return "";
            //}

            string sql = "";


            if (geoID.Equals("NO NEED GEO ID")  && unitID != -1) // empty geo and time inputs
            {
                sql = "select " +
    "distinct " +
    "ERSDataValues_ID as CoSD_ID,  " +
    "ERSDataValues_AttributeValue as Value, " +
    "ERSUnit_LU.ERSUnit_Desc as Unit,  " +
    "ERSDataLifecyclePhase_Desc as  LifeCyclePhase,  " +
    "ERSSource_Desc as Source, " +
    "ERSTimeDimensionType_Desc as TimeType, " +
    "(SELECT CONVERT(varchar(20), ERSTimeDimension_Date,101)) as Date, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU, " +
    schemaName + "ERSTimeDimensionType_LU,  " +
    schemaName + "ERSDataLifecycle_LU,  " +
    schemaName + "ERSSource_LU  " +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSDataLifecycle_LU.ERSDataLifecyclePhase_ID = " + schemaName + "ERSDataValues.ERSDataValues_DataRowLifecyclePhaseID" +
      " and " + schemaName + "ERSSource_LU.ERSSource_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSSource_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_TimeDimensionType_ID = " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID"

       ;
            }
            else if (geoID.Equals("NO NEED GEO ID") && unitID == -1) // empty geo, time and unit inputs
            {
                sql = "select " +
    "distinct " +
    "ERSDataValues_ID as CoSD_ID,  " +
    "ERSDataValues_AttributeValue as Value, " +
    "ERSUnit_LU.ERSUnit_Desc as Unit,  " +
    "ERSDataLifecyclePhase_Desc as  LifeCyclePhase,  " +
    "ERSSource_Desc as Source, " +
    "ERSTimeDimensionType_Desc as TimeType, " +
    "(SELECT CONVERT(varchar(20), ERSTimeDimension_Date,101)) as Date, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU, " +
    schemaName + "ERSTimeDimensionType_LU,  " +
    schemaName + "ERSDataLifecycle_LU,  " +
    schemaName + "ERSSource_LU  " +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSDataLifecycle_LU.ERSDataLifecyclePhase_ID = " + schemaName + "ERSDataValues.ERSDataValues_DataRowLifecyclePhaseID" +
      " and " + schemaName + "ERSSource_LU.ERSSource_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSSource_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_TimeDimensionType_ID = " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID"
       ;
            }
            else if (geoID.Equals("NO NEED GEO ID") && unitID != -1)  // only time inputs
            {


                sql = "select " +
    "distinct " +
    "ERSDataValues_ID as CoSD_ID,  " +
    "ERSDataValues_AttributeValue as Value, " +
    "ERSUnit_LU.ERSUnit_Desc as Unit,  " +
    "ERSDataLifecyclePhase_Desc as  LifeCyclePhase,  " +
    "ERSSource_Desc as Source, " +
    "ERSTimeDimensionType_Desc as TimeType, " +
    "(SELECT CONVERT(varchar(20), ERSTimeDimension_Date,101)) as Date, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU, " +
    schemaName + "ERSTimeDimensionType_LU,  " +
    schemaName + "ERSDataLifecycle_LU,  " +
    schemaName + "ERSSource_LU  " +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSDataLifecycle_LU.ERSDataLifecyclePhase_ID = " + schemaName + "ERSDataValues.ERSDataValues_DataRowLifecyclePhaseID" +
      " and " + schemaName + "ERSSource_LU.ERSSource_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSSource_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_TimeDimensionType_ID = " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID" +
      " and " + schemaName 
       ;

            }
            else if (geoID.Equals("NO NEED GEO ID") && unitID == -1)  // only time and no Unit inputs
            {


                sql = "select " +
    "distinct " +
    "ERSDataValues_ID as CoSD_ID,  " +
    "ERSDataValues_AttributeValue as Value, " +
    "ERSUnit_LU.ERSUnit_Desc as Unit,  " +
    "ERSDataLifecyclePhase_Desc as  LifeCyclePhase,  " +
    "ERSSource_Desc as Source, " +
    "ERSTimeDimensionType_Desc as TimeType, " +
    "(SELECT CONVERT(varchar(20), ERSTimeDimension_Date,101)) as Date, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU, " +
    schemaName + "ERSTimeDimensionType_LU,  " +
    schemaName + "ERSDataLifecycle_LU,  " +
    schemaName + "ERSSource_LU  " +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSDataLifecycle_LU.ERSDataLifecyclePhase_ID = " + schemaName + "ERSDataValues.ERSDataValues_DataRowLifecyclePhaseID" +
      " and " + schemaName + "ERSSource_LU.ERSSource_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSSource_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_TimeDimensionType_ID = " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID" +
      " and " + schemaName 
       ;

            }
            else if (!geoID.Equals("NO NEED GEO ID")  && unitID != -1)  // only geo inputs
            {
                sql = "select " +
    "distinct " +
    "ERSDataValues_ID as CoSD_ID,  " +
    "ERSDataValues_AttributeValue as Value, " +
    "ERSUnit_LU.ERSUnit_Desc as Unit,  " +
    "ERSDataLifecyclePhase_Desc as  LifeCyclePhase,  " +
    "ERSSource_Desc as Source, " +
    "ERSTimeDimensionType_Desc as TimeType, " +
    "(SELECT CONVERT(varchar(20), ERSTimeDimension_Date,101)) as Date, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU, " +
    schemaName + "ERSTimeDimensionType_LU,  " +
    schemaName + "ERSDataLifecycle_LU,  " +
    schemaName + "ERSSource_LU  " +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSDataLifecycle_LU.ERSDataLifecyclePhase_ID = " + schemaName + "ERSDataValues.ERSDataValues_DataRowLifecyclePhaseID" +
      " and " + schemaName + "ERSSource_LU.ERSSource_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSSource_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_TimeDimensionType_ID = " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")"
       ;
            }
            else if (!geoID.Equals("NO NEED GEO ID") && unitID == -1)  // only geo and no unit inputs
            {
                sql = "select " +
    "distinct " +
    "ERSDataValues_ID as CoSD_ID,  " +
    "ERSDataValues_AttributeValue as Value, " +
    "ERSUnit_LU.ERSUnit_Desc as Unit,  " +
    "ERSDataLifecyclePhase_Desc as  LifeCyclePhase,  " +
    "ERSSource_Desc as Source, " +
    "ERSTimeDimensionType_Desc as TimeType, " +
    "(SELECT CONVERT(varchar(20), ERSTimeDimension_Date,101)) as Date, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU, " +
    schemaName + "ERSTimeDimensionType_LU,  " +
    schemaName + "ERSDataLifecycle_LU,  " +
    schemaName + "ERSSource_LU  " +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSDataLifecycle_LU.ERSDataLifecyclePhase_ID = " + schemaName + "ERSDataValues.ERSDataValues_DataRowLifecyclePhaseID" +
      " and " + schemaName + "ERSSource_LU.ERSSource_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSSource_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_TimeDimensionType_ID = " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")"
       ;
            }
            else if (unitID != -1)  // have both ...
            {
                sql = "select " +
    "distinct " +
    "ERSDataValues_ID as CoSD_ID,  " +
    "ERSDataValues_AttributeValue as Value, " +
    "ERSUnit_LU.ERSUnit_Desc as Unit,  " +
    "ERSDataLifecyclePhase_Desc as  LifeCyclePhase,  " +
    "ERSSource_Desc as Source, " +
    "ERSTimeDimensionType_Desc as TimeType, " +
    "(SELECT CONVERT(varchar(20), ERSTimeDimension_Date,101)) as Date, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU, " +
    schemaName + "ERSTimeDimensionType_LU,  " +
    schemaName + "ERSDataLifecycle_LU,  " +
    schemaName + "ERSSource_LU  " +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSDataLifecycle_LU.ERSDataLifecyclePhase_ID = " + schemaName + "ERSDataValues.ERSDataValues_DataRowLifecyclePhaseID" +
      " and " + schemaName + "ERSSource_LU.ERSSource_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSSource_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_TimeDimensionType_ID = " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")" +
      " and " + schemaName 
       ;

            }
            else if (unitID == -1)  // have both but not unit id ...
            {
                sql = "select " +
    "distinct " +
    "ERSDataValues_ID as CoSD_ID,  " +
    "ERSDataValues_AttributeValue as Value, " +
    "ERSUnit_LU.ERSUnit_Desc as Unit,  " +
    "ERSDataLifecyclePhase_Desc as  LifeCyclePhase,  " +
    "ERSSource_Desc as Source, " +
    "ERSTimeDimensionType_Desc as TimeType, " +
    "(SELECT CONVERT(varchar(20), ERSTimeDimension_Date,101)) as Date, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
    "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU, " +
    schemaName + "ERSTimeDimensionType_LU,  " +
    schemaName + "ERSDataLifecycle_LU,  " +
    schemaName + "ERSSource_LU  " +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSDataLifecycle_LU.ERSDataLifecyclePhase_ID = " + schemaName + "ERSDataValues.ERSDataValues_DataRowLifecyclePhaseID" +
      " and " + schemaName + "ERSSource_LU.ERSSource_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSSource_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_TimeDimensionType_ID = " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")" +
      " and " + schemaName 
       ;

            }

            Console.WriteLine(sql);
            return sql;
            //   DataTable result = GetData(sql);
            //   return result;
        }
        private string getDataValuesCount(StringBuilder dataSeriesIDs, int unitID, string geoID)
        {
            string sql = "";

            //        if (dataSeriesIDs.Length == 0 && geoID.Equals("NO NEED GEO ID") && timeID.Equals("NO NEED TIME ID") && unitID == -1)
            //        {
            //            dataSeriesIDs = getDataSeriesID("-1", "", "-1", "", "");

            //            sql = "select " +
            //"COUNT(*) as Records " +
            //"from  " +
            //schemaName + "ERSDataValues,  " +
            //schemaName + "ERSUnit_LU, " +
            //schemaName + "ERSGeographyDimension_LU, " +
            //schemaName + "ERSTimeDimension_LU  " +
            //" where " +
            // " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
            //  " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
            //  " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
            //  " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID"
            //   ;
            //        return sql;
            //        }


            if (dataSeriesIDs.Length == 0)
            {
                dataSeriesIDs = getDataSeriesID("-1", "", "-1", "", "", "");
            }

            if (geoID.Equals("NO NEED GEO ID") && unitID != -1) // empty geo and time inputs
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU  " +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID"
       ;
            }
            else if (geoID.Equals("NO NEED GEO ID") && unitID == -1) // empty geo, time and unit inputs
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU  " +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID"
       ;
            }
            else if (geoID.Equals("NO NEED GEO ID")  && unitID != -1)  // only time inputs
            {


                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU  " +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName 
       ;

            }
            else if (geoID.Equals("NO NEED GEO ID") && unitID == -1)  // only time and no Unit inputs
            {


                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU  " +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName 
       ;

            }
            else if (!geoID.Equals("NO NEED GEO ID")  && unitID != -1)  // only geo inputs
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU  " +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")"
       ;
            }
            else if (!geoID.Equals("NO NEED GEO ID")  && unitID == -1)  // only geo and no unit inputs
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU  " +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")"
       ;
            }
            else if (!geoID.Equals("NO NEED GEO ID")  && unitID != -1)  // have both ...
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU  " +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")" +
      " and " + schemaName 
       ;

            }
            else if (!geoID.Equals("NO NEED GEO ID") && unitID == -1)  // have both but not unit id ...
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU  " +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")" +
      " and " + schemaName 
                ;

            }

            return sql;

        }

        // load dataseriesID from dataseries table
        private List<string> getDataSeriesID(string commoditySubCommodityID, string physicalAttributeDetail, int prodPracticeID, int utilPracticeID, int statId, string sourceID)
        {
            string sql = "select ERSCommodity_ID from " + schemaName + "ERSCommodityDataSeries where " +
                " ERSCommoditySubCommodity_ID = '" + commoditySubCommodityID +"'"+
                  " and ERSCommodity_PhysicalAttribute_Desc = '" + physicalAttributeDetail + "'" +
                  " and ERSCommodity_ERSProdPractice_ID = " + prodPracticeID +
                      " and ERSCommodity_ERSUtilPractice_ID = " + utilPracticeID +
                      " and ERSCommodity_ERSStatisticType_ID = " + statId +
                      " and ERSCommodity_ERSSource_ID IN (" + sourceID + ")" +
                      " Order By [ERSCommodity_ID]";

            DataTable result = GetData(sql);
            List<string> dataSeriesID = new List<string>();

            if (result.Rows.Count == 0)  //no available dataseriesID
            {
                MessageBox.Show("No record found with given selections, please adjust your searching criteria and try again.");
            }
            else  // has at least one ID, need to take care multiple issue later!!!
            {
                DataRow[] rows = result.Select();
                foreach (DataRow row in rows)
                {
                    dataSeriesID.Add(row["ERSCommodity_ID"].ToString());
                }
            }
            return dataSeriesID;
        }

        /// <summary>
        /// CHANGING PAGE TO PREVIOUS ONE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_prev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                table = new DataTable();
                dataAdapterForPaging.Fill(itemsPerPage * (currentPage - 1), itemsPerPage, table);

                //translate table
                DataTable dtCloned = table.Clone();
                for (int i = 0; i < columnNumInResultTable; i++)
                    dtCloned.Columns[i].DataType = typeof(string);

                foreach (DataRow row in table.Rows)
                {
                    dtCloned.ImportRow(row);
                }

                // translate unit id into unit name
                // translate stattype id into stattype name
                /*
                foreach (DataRow row in dtCloned.Rows)
                {
                    string statTypeID1 = row["StatType1"].ToString();
                    string statTypeID2 = row["StatType2"].ToString();
                    string statTypeID3 = row["StatType3"].ToString();
                    string unitID1 = row["Unit1"].ToString();
                    string unitID2 = row["Unit2"].ToString();
                    string unitID3 = row["Unit3"].ToString();
                    if (!statTypeID1.Equals(""))
                    {
                        row["StatType1"] = getTypeName(statTypeID1);
                    }
                    if (!statTypeID2.Equals(""))
                    {
                        row["StatType2"] = getTypeName(statTypeID2);
                    }
                    if (!statTypeID3.Equals(""))
                    {
                        row["StatType3"] = getTypeName(statTypeID3);
                    }
                    if (!unitID1.Equals(""))
                    {
                        row["Unit1"] = getUnitName(unitID1);
                    }
                    if (!unitID2.Equals(""))
                    {
                        row["Unit2"] = getUnitName(unitID2);
                    }
                    if (!unitID3.Equals(""))
                    {
                        row["Unit3"] = getUnitName(unitID3);
                    }
                    // filter out NULL in results
                    for (int i = 0; i < columnNumInResultTable; i++)
                    {
                        if (row[i].Equals("NULL"))
                            row[i] = "";
                    }
                }
                */


                label_pageNum.Text = "Page " + currentPage + " of " + totalPage;

                // pop to datagrid
                bindingSource1.DataSource = dtCloned;

                // set color of column
                for (int i = 0; i < columnNumInResultTable; i++)
                {

                    dataGridView.Columns[i].DefaultCellStyle.ForeColor = Color.Gray;
                    dataGridView.Columns[i].ReadOnly = true;
                }
                // set color of column
                dataGridView.Columns["Value"].DefaultCellStyle.ForeColor = Color.Black;
                dataGridView.Columns["Value"].DefaultCellStyle.BackColor = Color.White;
                dataGridView.Columns["Value"].ReadOnly = false;

            }

        }

        /// <summary>
        /// CHANGING PAGE TO THE NEXT ONE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_next_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPage)
            {
                currentPage++;
                table = new DataTable();
                dataAdapterForPaging.Fill(itemsPerPage * (currentPage - 1), itemsPerPage, table);

                //translate table
                DataTable dtCloned = table.Clone();
                for (int i = 0; i < columnNumInResultTable; i++)
                    dtCloned.Columns[i].DataType = typeof(string);

                foreach (DataRow row in table.Rows)
                {
                    dtCloned.ImportRow(row);
                }

                // translate unit id into unit name
                // translate stattype id into stattype name
                /*
                foreach (DataRow row in dtCloned.Rows)
                {
                    string statTypeID1 = row["StatType1"].ToString();
                    string statTypeID2 = row["StatType2"].ToString();
                    string statTypeID3 = row["StatType3"].ToString();
                    string unitID1 = row["Unit1"].ToString();
                    string unitID2 = row["Unit2"].ToString();
                    string unitID3 = row["Unit3"].ToString();
                    if (!statTypeID1.Equals(""))
                    {
                        row["StatType1"] = getTypeName(statTypeID1);
                    }
                    if (!statTypeID2.Equals(""))
                    {
                        row["StatType2"] = getTypeName(statTypeID2);
                    }
                    if (!statTypeID3.Equals(""))
                    {
                        row["StatType3"] = getTypeName(statTypeID3);
                    }
                    if (!unitID1.Equals(""))
                    {
                        row["Unit1"] = getUnitName(unitID1);
                    }
                    if (!unitID2.Equals(""))
                    {
                        row["Unit2"] = getUnitName(unitID2);
                    }
                    if (!unitID3.Equals(""))
                    {
                        row["Unit3"] = getUnitName(unitID3);
                    }
                    // filter out NULL in results
                    for (int i = 0; i < columnNumInResultTable; i++)
                    {
                        if (row[i].Equals("NULL"))
                            row[i] = "";
                    }
                }
                */


                label_pageNum.Text = "Page " + currentPage + " of " + totalPage;

                // pop to datagrid
                bindingSource1.DataSource = dtCloned;

                // set color of column
                for (int i = 0; i < columnNumInResultTable; i++)
                {

                    dataGridView.Columns[i].DefaultCellStyle.ForeColor = Color.Gray;
                    dataGridView.Columns[i].ReadOnly = true;
                }
                // set color of column
                dataGridView.Columns["Value"].DefaultCellStyle.ForeColor = Color.Black;
                dataGridView.Columns["Value"].DefaultCellStyle.BackColor = Color.White;
                dataGridView.Columns["Value"].ReadOnly = false;
            }
        }
        /// <summary>
        /// ACTION OF DELETE BUTTON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string dataValueID = "";
            string DVId = "";
            if (null != dataGridView && dataGridView.SelectedRows.Count != 0)
            {
                // ask for conformation before delete
                if (MessageBox.Show("Do you really want to delete these values?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    if (MessageBox.Show("Data once deleted cannot be retrieved back. Do you want to continue?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        waitForm.Show(this);
                        waitForm.Refresh();
                        for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
                        {
                            //retrieving the data before deleting
                            deletedData = new StringBuilder();
                            dataValueID = Convert.ToString(dataGridView.SelectedRows[i].Cells["CoSD_Id"].Value);
                            string deletedValue = "Select * from " + schemaName + "ERSDataValues " + " where ERSDataValues_ID = " + dataValueID;

                            DVId = DVId + ","  + dataValueID ;


                            //DataTable dt = GetData(deletedValue);
                            //DataRow[] dr = dt.Select();
                            //foreach (DataRow row in dt.Rows)
                            //{
                            //    //sb.AppendLine(string.Join(",", row.ItemArray));
                            //    foreach (DataColumn column in dt.Columns)
                            //    {
                            //        deletedData.AppendLine(" " + column.ColumnName + ": " + row[column]);
                            //    }
                            //}
                            ////deleting the data
                            //string sql = "delete from " + schemaName + "ERSDataValues " +
                            //    " where ERSDataValues_ID = " + dataValueID;
                            //// execute
                            //GetData(sql);
                            //saving the deleted data in action log
                            saveAction("Data Value deleted : " + deletedData);
                        }

                        dataValueID = DVId.TrimStart(',');

                        string connectionString = "";

                        try
                        {
                            connectionString = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
                        }
                        catch (Exception ex)
                        {
                           // backgroundWorker.ReportProgress(0);
                            MessageBox.Show("Data deletion unsuccessful, please contact technical team.");
                            //saveAction(ex.Message);
                            return;
                        }

                        string commandText = "CoSD.sp_delete_datavalues";

                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            SqlCommand cmd = new SqlCommand(commandText, conn);
                            cmd.Parameters.Add("@datavalueid", SqlDbType.VarChar, 100).Value = dataValueID;
                            //Indefinite timeout
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;

                            try
                            {
                                conn.Open();
                                int resultdv = cmd.ExecuteNonQuery();
                                if (resultdv == -1)
                                {
                                    // backgroundWorker.ReportProgress(2);
                                    waitForm.Hide();
                                    MessageBox.Show(dataGridView.SelectedRows.Count + " Record(s) deleted successfully.");
                                    saveAction("Data value Record(s) deleted successfully");

                                }
                                else
                                {
                                    //backgroundWorker.ReportProgress(2);
                                    waitForm.Hide();
                                    MessageBox.Show("Data deletion unsuccessful");
                                    saveAction("Data value deletion unsuccessful");
                                }

                            }

                            catch (Exception ex)
                            {
                                // backgroundWorker.ReportProgress(0);
                                waitForm.Hide();
                                MessageBox.Show("Data deletion unsuccessful, please contact technical team. Error Details: \n" + ex.Message);

                                saveAction("Data values deletion" + ex.Message);
                            }

                            finally
                            {
                                conn.Close();
                            }


                        }

                        
                      
                        // message
                        //MessageBox.Show(dataGridView.SelectedRows.Count + " Record(s) deleted successfully.");

                        // refresh
                        button_retrieve_Click(null, null);
                        //     refreshDatagridview();
                    }
                }

            }
            else
                MessageBox.Show("Please select a row to delete");
        }
        /// <summary>
        /// METHOD USED TO REFRESH THE DATA GRID, RELOAD DATA
        /// </summary>
        /// 
        /*
        private void refreshDatagridview()
        {
            // get timeID, need or not?


            // get dataseries ID
            // load variable used from gui
            int commoditySubCommodityID = getCommodityID(comboBox_commodity.Text);
            int physicalAttributeTypeID = int.Parse(getPhyAttrCatID());
            string physicalAttributeDetail = (string)comboBox_phyAttrDetail.SelectedValue;
            if (physicalAttributeDetail.Equals("No Physical Attribute Detail"))
                physicalAttributeDetail = "NULL";
            int groupID = getGroupID(comboBox_group.Text);
            int prodPracticeID = int.Parse(getProdPracID(comboBox_prodPrac.Text));
            int utilPracticeID = int.Parse(getUtilPracID(comboBox_utilPrac.Text));
            string dataSeriesID = getDataSeriesID(commoditySubCommodityID, physicalAttributeTypeID, physicalAttributeDetail, prodPracticeID, utilPracticeID);


            // search values
            int unitID = int.Parse(getUnitID(comboBox_unit.Text));
            int statID = int.Parse(getStatTypeID());

            // need to get geo id 
            string geoID = getGeoID();

            // need to get time id
            string timeID = getTimeID();

            try
            {
                string sqlStmt = getDataValues(dataSeriesID, unitID, statID, geoID, timeID);
                connection();
                dataAdapterForPaging = new SqlDataAdapter(sqlStmt, con);
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
                return;
            }
            
            // Create a command builder to generate SQL update, insert, and
            // delete commands based on sqlStmt. These are used to
            // update the database.
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapterForPaging);

            // split pages
            DataTable result = new DataTable();
            DataTable resultPerPage = new DataTable();

            dataAdapterForPaging.Fill(result);
            totalItems = result.Rows.Count;
            totalPage = totalItems / itemsPerPage + 1;

            dataAdapterForPaging.Fill(0, itemsPerPage, resultPerPage);
            label_pageNum.Text = "Page " + currentPage + " of " + totalPage;

            // pop to datagrid
            bindingSource1.DataSource = resultPerPage;
        }
         */
        /// <summary>
        /// BACK TO HOME PAGE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_back_Click(object sender, EventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Close();
            this.Parent = null;
        }
        /// <summary>
        /// RESET EVERYTHING
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_reset_Click(object sender, EventArgs e)
        {

            comboBox_commodity.Text = "";
            comboBox_group.ResetText();
            comboBox_phyAttrDetail.Text = "";
            comboBox_prodPrac.Text = "";
            comboBox_statType.Text = "";
            comboBox_source.Text = "";
            comboBox_unit.Text = "";
            comboBox_utilPrac.Text = "";
            comboBox_country.Text = "";
            comboBox_region.Text = "";
            comboBox_state.Text = "";
            comboBox_county.Text = "";
            //monthValue.Text = "";
            //yearValue.Text = "";
            label_pageNum.Text = "";
            comboBox_phyAttrDetail.Enabled = true;
            bindingSource1.DataSource = null;
            button_Delete.Enabled = false;
            button_Delete.UseVisualStyleBackColor = false;
            button_next.Enabled = false;
            button_prev.Enabled = false;

            //disbale comboboxes
            comboBox_commodity.Enabled = false;
            comboBox_phyAttrDetail.Enabled = false;
            comboBox_prodPrac.Enabled = false;
            comboBox_utilPrac.Enabled = false;
            comboBox_statType.Enabled = false;
            comboBox_unit.Enabled = false;
            comboBox_source.Enabled = false;
        }

        /// <summary>
        /// WHEN GROUP GETS CHANGED
        /// CHANGE COMMODITY LIST WITH SELECTED GROUP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_group_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// FILL COMMODITY COMBOBOX WITH GIVEN GROUP NUMBER
        /// </summary>
        /// <param name="groupNum"></param>
        private void fillCommodityWithGroupID(int groupNum)
        {
            commodities = new List<string>();
            comboBox_commodity.Text = "";

            string sql = "select ERSCommoditySubCommodity_Desc from " + schemaName + "ERSCommoditySubCommodity_LU where ERSCommoditySubCommodity_GroupID = " + groupNum + " ORDER BY ERSCommoditySubCommodity_Desc";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            commodities.Add("");
            foreach (DataRow row in dr)
            {
                commodities.Add(row["ERSCommoditySubCommodity_Desc"].ToString());
            }

            comboBox_commodity.DataSource = commodities;
            comboBox_commodity.SelectedItem = null;
        }

        /// <summary>
        /// TRANSLATE GROUP NAME INTO GROUP ID
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private int getGroupID(string groupName)
        {
            if (groupName != null && groupName != "")
            {
                string sql = "select ERSGroup_ID from " + schemaName + "ERSGroup_LU where ERSGroup_Desc = '" + groupName + "'";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return (int)dr[0]["ERSGroup_ID"];
            }
            else
                return -1;

        }
        /// <summary>
        /// TRANSLATE COMMODITY NAME INTO COMMODITY ID
        /// </summary>
        /// <param name="commodityName"></param>
        /// <returns></returns>
        private string getCommodityID(string commodityName)
        {
            string sql = "select ERSCommoditySubCommodity_ID from " + schemaName + "ERSCommoditySubCommodity_LU where ERSCommoditySubCommodity_Desc = '" + commodityName + "'";
            DataTable dt = GetData(sql);
            if (null != dt && dt.Rows.Count != 0)
            {
                DataRow[] dr = dt.Select();
                return dr[0]["ERSCommoditySubCommodity_ID"].ToString();
            }
            else
                return "-1";
        }
        /// <summary>
        /// VALIDATE YEAR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void yearValue_Validating(object sender, CancelEventArgs e)
        //{
        //    try
        //    {
        //        // check if it has four digits
        //        if (!yearValue.Text.Equals("") && (int.Parse(yearValue.Text) <= 1000 || int.Parse(yearValue.Text) >= 9999))
        //        {
        //            errorProvider1.SetError(yearValue, "Invalid input!\nPlease type a year between 1000 and 9999.");
        //            yearValue.BackColor = Color.Red;
        //        }
        //        else
        //        {
        //            errorProvider1.SetError(yearValue, "");
        //            yearValue.BackColor = Color.White;
        //        }
        //    }
        //    catch (Exception)   // in case it could not get parsed to int
        //    {

        //    }
        //}
        ///// <summary>
        ///// VALIDATE YEAR
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void yearValue_TextChanged(object sender, EventArgs e)
        //{
        //    Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
        //    // check if it's a number
        //    if (!yearValue.Text.Equals("") && !numberchk.IsMatch(yearValue.Text))
        //    {
        //        errorProvider1.SetError(yearValue, "Invalid input!\nPlease type numeric characters only.");
        //        yearValue.BackColor = Color.Red;
        //        return;
        //    }
        //    else if (!yearValue.Text.Equals("") && numberchk.IsMatch(yearValue.Text))
        //    {
        //        errorProvider1.SetError(yearValue, "");
        //        yearValue.BackColor = Color.White;

        //    }
        //    else
        //    {
        //        errorProvider1.SetError(yearValue, "");
        //        yearValue.BackColor = Color.White;
        //    }
        //}

        private void resetResult()
        {
            label_pageNum.Text = "";
            bindingSource1.DataSource = null;
            button_next.Enabled = false;
            button_prev.Enabled = false;
            button_Delete.Enabled = false;

        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            button_Delete.Enabled = true;
            button_Delete.UseVisualStyleBackColor = true;
        }

        //private void monthValue_TextChanged(object sender, EventArgs e)
        //{
        //    Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
        //    // check if it's a number
        //    if (!monthValue.Text.Equals("") && !numberchk.IsMatch(monthValue.Text))
        //    {
        //        errorProvider1.SetError(monthValue, "Invalid input!\nPlease type numeric characters only.");
        //        monthValue.BackColor = Color.Red;
        //        return;
        //    }
        //    else if (!monthValue.Text.Equals("") && numberchk.IsMatch(monthValue.Text))
        //    {
        //        errorProvider1.SetError(monthValue, "");
        //        monthValue.BackColor = Color.White;

        //    }
        //    else
        //    {
        //        errorProvider1.SetError(monthValue, "");
        //        monthValue.BackColor = Color.White;
        //    }

        //    // check if it has four digits

        //    if (!monthValue.Text.Equals("") && (int.Parse(monthValue.Text) > 12 || int.Parse(monthValue.Text) < 1))
        //    {
        //        errorProvider1.SetError(monthValue, "Invalid input!\nPlease enter a month between 1 and 12.");
        //        monthValue.BackColor = Color.Red;
        //        return;
        //    }
        //    else
        //    {
        //        errorProvider1.SetError(monthValue, "");
        //        monthValue.BackColor = Color.White;
        //    }
        //}

        private void comboBox_group_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //start wait form
            loadform.Show(this);
            loadform.Refresh();


            Cursor.Current = Cursors.WaitCursor;
            groupNum = getGroupID(comboBox_group.Text);
            //   fillBy1ToolStripButton_Click(sender, e);
            //fillCommodityWithGroupID(groupNum);
            //fillPhysicalAttribute(groupNum);
            //fillUnit();
            //fillStatTypeCombobox(groupNum);
            //fillProdPracComboBox(groupNum);
            //fillUtilPracComboBox(groupNum);
            //fillSourceCombobox(groupNum);
            ////enable comboboxes
            //comboBox_commodity.Enabled = true;
            //comboBox_phyAttrDetail.Enabled = true;
            //comboBox_prodPrac.Enabled = true;
            //comboBox_utilPrac.Enabled = true;
            //comboBox_statType.Enabled = true;
            //comboBox_unit.Enabled = true;
            //comboBox_source.Enabled = true;
            fillCommodityComboBox(groupNum);
            fillPhysicalAttribute(groupNum);
            fillProdPracComboBox(groupNum);
            fillUtilPracComboBox(groupNum);
            fillStatTypeCombobox(groupNum);
            fillSourceCombobox(groupNum);
            fillUnit();

            //resetting dataseries
            dataSeriesIDs = new StringBuilder();



            //enable comboboxes
            comboBox_commodity.Enabled = true;
            comboBox_phyAttrDetail.Enabled = true;
            comboBox_prodPrac.Enabled = true;
            comboBox_utilPrac.Enabled = true;
            comboBox_statType.Enabled = true;
            comboBox_unit.Enabled = true;
            comboBox_source.Enabled = true;

            loadform.Hide();
        }

        private void label_Required_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// fill commodity comboBox with selected group
        /// </summary>
        /// <param name="groupNum"></param>
        private void fillCommodityComboBox(int groupNum)
        {
            commodities = new List<string>();

            //string sql = "select ERSCommoditySubCommodity_Desc from " + schemaName + "ERSCommoditySubCommodity_LU where ERSCommoditySubCommodity_GroupID = " + groupNum +
            //    " and ERSCommoditySubCommodity_ID IN (SELECT DISTINCT [ERSDataValues_ERSCommodity_ID] FROM " + schemaName + "[ERSDataValues]) ORDER BY ERSCommoditySubCommodity_Desc";

            string sql = "select ERSCommoditySubCommodity_Desc from " + schemaName + "ERSCommoditySubCommodity_LU where ERSCommoditySubCommodity_GroupID = " + groupNum +
                " and ERSCommoditySubCommodity_ID IN (SELECT DISTINCT ds.ERSCommoditySubCommodity_ID FROM " + schemaName +
                "[ERSDataValues] dv left join "+ schemaName + "[ERSCommodityDataSeries] ds on dv.ERSDataValues_ERSCommodity_ID=ds.ERSCommodity_ID) ORDER BY ERSCommoditySubCommodity_Desc";

            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            commodities.Add("");
            foreach (DataRow row in dr)
            {
                commodities.Add(row["ERSCommoditySubCommodity_Desc"].ToString());
            }

            comboBox_commodity.DataSource = commodities;
        }
        private void comboBox_country_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Disabling geo
            string selectedCountry = comboBox_country.SelectedItem.ToString();
            if (!selectedCountry.Equals("UNITED STATES"))
            {
                comboBox_region.Enabled = false;
                comboBox_state.Enabled = false;
                comboBox_county.Enabled = false;
            }
            else
            {
                comboBox_region.Enabled = true;
                comboBox_state.Enabled = true;
                comboBox_county.Enabled = true;
            }
        }

        private void comboBox_commodity_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text);
        }

        /// <summary>
        /// translate stat type name into id
        /// </summary>
        /// <returns></returns>
        private string getStatTypeID(string statType)
        {
            string sql = "select ERSStatisticType_ID from " + schemaName + "ERSStatisticType_LU where ERSStatisticType_Attribute = '" + statType + "'";
            DataTable dt = GetData(sql);
            if (null != dt && dt.Rows.Count != 0)
            {
                DataRow[] dr = dt.Select();
                return dr[0]["ERSStatisticType_ID"].ToString();
            }
            else
                return "-1";
        }

        /// <summary>
        /// translate stat type name into id
        /// </summary>
        /// <returns></returns>
        private string getSourceID(string source)
        {
            string sql = "select ERSSource_ID from " + schemaName + "ERSSource_LU where ERSSource_Desc = '" + source + "'";
            DataTable dt = GetData(sql);
            if (null != dt && dt.Rows.Count != 0)
            {
                sourceIDs = new StringBuilder();
                DataRow lastRow = dt.Rows[dt.Rows.Count - 1];
                string lastItem = lastRow[0].ToString();
                foreach (DataRow dr in dt.Rows)
                {
                    //dataSeriesID.Add(dr[0].ToString());
                    if (dr[0].ToString() != lastItem)
                        sourceIDs.Append(dr[0].ToString() + ",");
                    else sourceIDs.Append(dr[0].ToString());
                }
                return sourceIDs.ToString();
            }
            else
                return "-1";

        }

        // load dataseriesID from dataseries table
        private StringBuilder getDataSeriesID(string commoditySubCommodityID, string physicalAttributeDetail, string prodPractice, string utilPractice, string statType, string sourceID)
        {
            if (!commoditySubCommodityID.Equals("") && !commoditySubCommodityID.Equals("-1"))
                commoditySubCommodityID = " and CDS.ERSCommoditySubCommodity_ID = " + commoditySubCommodityID;
            else
                commoditySubCommodityID = "";
            if (!physicalAttributeDetail.Equals("") && !physicalAttributeDetail.Equals("-1"))
                physicalAttributeDetail = " and CDS.ERSCommodity_PhysicalAttribute_Desc = '" + physicalAttributeDetail + "'";
            else
                physicalAttributeDetail = "";
            if (!prodPractice.Equals("") && !prodPractice.Equals("-1"))
                prodPractice = " and CDS.ERSCommodity_ERSProdPractice_ID = " + prodPractice;
            else
                prodPractice = "";
            if (!utilPractice.Equals("") && !utilPractice.Equals("-1"))
                utilPractice = " and CDS.ERSCommodity_ERSUtilPractice_ID = " + utilPractice;
            else
                utilPractice = "";
            if (!statType.Equals("") && !statType.Equals("-1"))
                statType = " and CDS.[ERSCommodity_ERSStatisticType_ID] = " + statType;
            else
                statType = "";
            if (!sourceID.Equals("") && !sourceID.Equals("-1"))
                sourceID = " and CDS.[ERSCommodity_ERSSource_ID] IN (" + sourceID + ")";
            else
                sourceID = "";
            //Data Series query
            string sql = "select [ERSCommodity_ID] AS 'Data Series ID',[ERSCommoditySubCommodity_Desc] AS 'Commodity',[ERSCommodity_PhysicalAttribute_Desc] AS 'Physical Attribute',[ERSStatisticType_Attribute] 'Stat Type',[ERSProdPractice_Desc] AS 'Prod Practice',[ERSUtilPractice_Desc] AS 'Util Practice',[ERSCommodity_SourceSeriesID] AS 'SourceSeriesID',[ERSSource_Desc] AS 'Source Desc',[ERSSource_LongDesc] 'Source Long Desc' from " + schemaName + "ERSCommodityDataSeries CDS, " + schemaName + "[ERSCommoditySubCommodity_LU], " + schemaName + "ERSStatisticType_LU, " + schemaName + "ERSProdPractice_LU, " + schemaName + "ERSUtilPractice_LU, " + schemaName + "ERSSource_LU where [ERSCommodity_ERSGroup_ID] = " + groupNum +
                commoditySubCommodityID +
                physicalAttributeDetail +
                prodPractice +
                utilPractice +
                statType +
                sourceID +
                " and CDS.[ERSCommoditySubCommodity_ID] = " + schemaName + "[ERSCommoditySubCommodity_LU].[ERSCommoditySubCommodity_ID]" +
                " and CDS.[ERSCommodity_ERSStatisticType_ID] = " + schemaName + "[ERSStatisticType_LU].[ERSStatisticType_ID]" +
                " and CDS.[ERSCommodity_ERSProdPractice_ID] = " + schemaName + "[ERSProdPractice_LU].[ERSProdPractice_ID]" +
                " and CDS.[ERSCommodity_ERSUtilPractice_ID] = " + schemaName + "[ERSUtilPractice_LU].[ERSUtilPractice_ID]" +
                " and CDS.[ERSCommodity_ERSSource_ID] = " + schemaName + "[ERSSource_LU].[ERSSource_ID]" +
                " Order By [ERSCommodity_ID]"
                ;


            connection();
            con.Open();
            dataAdapterForPaging = new SqlDataAdapter(sql, con);
            dataAdapterForPaging.SelectCommand.ExecuteNonQuery();
            DTDataSeries = new DataTable();
            dataAdapterForPaging.Fill(DTDataSeries);

            if (DTDataSeries.Rows.Count == 0)
            {
                return new StringBuilder("-1");
            }

            //Comma seperated Data series ids
            dataSeriesIDs = new StringBuilder();
            DataRow lastRow = DTDataSeries.Rows[DTDataSeries.Rows.Count - 1];
            string lastItem = lastRow[0].ToString();
            foreach (DataRow dr in DTDataSeries.Rows)
            {
                //dataSeriesID.Add(dr[0].ToString());
                if (dr[0].ToString() != lastItem)
                    dataSeriesIDs.Append(dr[0].ToString() + ",");
                else dataSeriesIDs.Append(dr[0].ToString());
            }
            return dataSeriesIDs;
        }


        private void fillAllComboBoxes(string commoditySubCommodity, string physicalAttributeDetail, string prodPractice, string utilPractice, string statType, string source)
        {

            string commodityQuery = "";
            string physicalAttributeQuery = "";
            string prodPracQuery = "";
            string utilPracQuery = "";
            string statTypeQuery = "";
            string sourceQuery = "";
            string commoditySubQuery = "";
            string physicalAttributeSubQuery = "";
            string prodPracSubQuery = "";
            string utilPracSubQuery = "";
            string statTypeSubQuery = "";
            string sourceSubQuery = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (commoditySubCommodity != "")
                    commoditySubCommodityID = getCommodityID(commoditySubCommodity);
                else
                    commoditySubCommodityID = "-1";
                if (prodPractice != "")
                    prodPracticeID = getProdPracID(prodPractice);
                else
                    prodPracticeID = "-1";
                if (utilPractice != "")
                    utilPracticeID = getUtilPracID(utilPractice);
                else
                    utilPracticeID = "-1";
                if (statType != "-1")
                    statID = getStatTypeID(statType);
                else
                    statID = "-1";
                if (source != "-1")
                    sourceID = getSourceID(source);
                else
                    sourceID = "-1";
                // Generating subqueries
                if (commoditySubCommodityID != "-1")
                    commoditySubQuery = " and ERSCommoditySubCommodity_ID = " + commoditySubCommodityID + "";
                if (physicalAttributeDetail != "")
                    physicalAttributeSubQuery = " and ERSCommodity_PhysicalAttribute_Desc like '%" + physicalAttributeDetail + "%'";
                if (prodPracticeID != "-1")
                    prodPracSubQuery = " and ERSCommodity_ERSProdPractice_ID = " + prodPracticeID + "";
                if (utilPracticeID != "-1")
                    utilPracSubQuery = " and ERSCommodity_ERSUtilPractice_ID = " + utilPracticeID + "";
                if (statID != "-1")
                    statTypeSubQuery = " and ERSCommodity_ERSStatisticType_ID = " + statID + "";
                if (sourceID != "-1")
                    sourceSubQuery = " and ERSCommodity_ERSSource_ID IN (" + sourceID + ")";


                //Fill Commodity combobox
                if (commoditySubCommodityID == "-1")
                {
                    commodities = new List<string>();
                    commodityQuery = "select [ERSCommoditySubCommodity_Desc] from " + schemaName + "ERSCommoditySubCommodity_LU WHERE ERSCommoditySubCommodity_ID IN (SELECT DISTINCT[ERSCommoditySubCommodity_ID] FROM " + schemaName + "[ERSCommodityDataSeries] WHERE [ERSCommodity_ERSGroup_ID] IN (" + groupNum + ") " +
                                 commoditySubQuery +
                                 physicalAttributeSubQuery +
                                 prodPracSubQuery +
                                 utilPracSubQuery +
                                 statTypeSubQuery +
                                 sourceSubQuery +
                                 " ) ORDER BY [ERSCommoditySubCommodity_Desc]";
                    DataTable dt = GetData(commodityQuery);
                    if (dt.Rows.Count != 0)
                    {
                        DataRow[] dr = dt.Select();
                        commodities.Add("");
                        foreach (DataRow row in dr)
                        {
                            commodities.Add(row["ERSCommoditySubCommodity_Desc"].ToString());
                        }
                        comboBox_commodity.DataSource = commodities;
                    }
                    else
                        comboBox_commodity.DataSource = null;
                }

                //Fill physical attribute combox
                if (physicalAttributeDetail == "")
                {
                    physicalAttributeCats = new List<string>();
                    physicalAttributeQuery = "select DISTINCT [ERSCommodity_PhysicalAttribute_Desc] from " + schemaName + "[ERSCommodityDataSeries] WHERE [ERSCommodity_ERSGroup_ID] IN (" + groupNum + ") " +
                    commoditySubQuery +
                    physicalAttributeSubQuery +
                    prodPracSubQuery +
                    utilPracSubQuery +
                    statTypeSubQuery +
                    sourceSubQuery + " ORDER BY [ERSCommodity_PhysicalAttribute_Desc]";
                    DataTable dt = GetData(physicalAttributeQuery);
                    if (dt.Rows.Count != 0)
                    {
                        DataRow[] dr = dt.Select();
                        physicalAttributeCats.Add("");
                        foreach (DataRow row in dr)
                        {
                            physicalAttributeCats.Add(row["ERSCommodity_PhysicalAttribute_Desc"].ToString());
                        }
                        comboBox_phyAttrDetail.DataSource = physicalAttributeCats;
                    }
                    else
                        comboBox_phyAttrDetail.DataSource = null;
                }

                //Fill Prod practice
                if (prodPracticeID == "-1")
                {
                    prodPrac = new List<string>();
                    prodPracQuery = "select [ERSProdPractice_Desc] from " + schemaName + "ERSProdPractice_LU WHERE ERSProdPractice_ID IN (SELECT DISTINCT[ERSCommodity_ERSProdPractice_ID] FROM " + schemaName + "[ERSCommodityDataSeries] WHERE [ERSCommodity_ERSGroup_ID] IN (" + groupNum + ") " +
                                 commoditySubQuery +
                                 physicalAttributeSubQuery +
                                 prodPracSubQuery +
                                 utilPracSubQuery +
                                 statTypeSubQuery +
                                 sourceSubQuery +
                                 " )  ORDER BY [ERSProdPractice_Desc]";
                    DataTable dt = GetData(prodPracQuery);
                    if (dt.Rows.Count != 0)
                    {

                        DataRow[] dr = dt.Select();
                        prodPrac.Add("");
                        foreach (DataRow row in dr)
                        {
                            prodPrac.Add(row["ERSProdPractice_Desc"].ToString());
                        }
                        comboBox_prodPrac.DataSource = prodPrac;
                    }
                    else
                        comboBox_prodPrac.DataSource = null;
                }

                //Fill util practice
                if (utilPracticeID == "-1")
                {
                    utilPrac = new List<string>();
                    utilPracQuery = "select [ERSUtilPractice_Desc] from " + schemaName + "ERSUtilPractice_LU WHERE ERSUtilPractice_ID IN (SELECT DISTINCT[ERSCommodity_ERSUtilPractice_ID] FROM " + schemaName + "[ERSCommodityDataSeries] WHERE [ERSCommodity_ERSGroup_ID] IN (" + groupNum + ") " +
                                 commoditySubQuery +
                                 physicalAttributeSubQuery +
                                 prodPracSubQuery +
                                 utilPracSubQuery +
                                 statTypeSubQuery +
                                 sourceSubQuery +
                                 " ) ORDER BY [ERSUtilPractice_Desc] ";
                    DataTable dt = GetData(utilPracQuery);
                    if (dt.Rows.Count != 0)
                    {

                        DataRow[] dr = dt.Select();
                        utilPrac.Add("");
                        foreach (DataRow row in dr)
                        {
                            utilPrac.Add(row["ERSUtilPractice_Desc"].ToString());
                        }
                        comboBox_utilPrac.DataSource = utilPrac;
                    }
                    else
                        comboBox_utilPrac.DataSource = null;
                }

                //Fill stat type
                if (statID == "-1")
                {
                    statTypes = new List<string>();
                    statTypeQuery = "select [ERSStatisticType_Attribute] from " + schemaName + "ERSStatisticType_LU WHERE ERSStatisticType_ID IN (SELECT DISTINCT[ERSCommodity_ERSStatisticType_ID] FROM  " + schemaName + "[ERSCommodityDataSeries] WHERE [ERSCommodity_ERSGroup_ID] IN (" + groupNum + ") " +
                                 commoditySubQuery +
                                 physicalAttributeSubQuery +
                                 prodPracSubQuery +
                                 utilPracSubQuery +
                                 statTypeSubQuery +
                                 sourceSubQuery +
                                 " ) ORDER BY [ERSStatisticType_Attribute] ";
                    DataTable dt = GetData(statTypeQuery);
                    if (dt.Rows.Count != 0)
                    {
                        DataRow[] dr = dt.Select();
                        statTypes.Add("");
                        foreach (DataRow row in dr)
                        {
                            statTypes.Add(row["ERSStatisticType_Attribute"].ToString());
                        }
                        comboBox_statType.DataSource = statTypes;
                    }
                    else
                        comboBox_statType.DataSource = null;
                }

                //Fill source
                if (sourceID == "-1")
                {
                    sourceList = new List<string>();
                    sourceQuery = "select DISTINCT [ERSSource_Desc] from " + schemaName + "[ERSSource_LU] WHERE [ERSSource_ID] IN (SELECT DISTINCT [ERSCommodity_ERSSource_ID] FROM  " + schemaName + "[ERSCommodityDataSeries] WHERE [ERSCommodity_ERSGroup_ID] IN (" + groupNum + ") " +
                                 commoditySubQuery +
                                 physicalAttributeSubQuery +
                                 prodPracSubQuery +
                                 utilPracSubQuery +
                                 statTypeSubQuery +
                                 sourceSubQuery +
                                 " ) ORDER BY [ERSSource_Desc] ";
                    DataTable dt = GetData(sourceQuery);
                    if (dt.Rows.Count != 0)
                    {
                        DataRow[] dr = dt.Select();
                        sourceList.Add("");
                        foreach (DataRow row in dr)
                        {
                            sourceList.Add(row["ERSSource_Desc"].ToString());
                        }
                        comboBox_source.DataSource = sourceList;
                    }
                    else
                        comboBox_source.DataSource = null;
                }


                //Finding data series
                if (commoditySubCommodityID == "-1" && physicalAttributeDetail == "" && prodPracticeID == "-1" && utilPracticeID == "-1" && statID == "-1" && sourceID == "-1")
                {
                    dataSeriesIDs = new StringBuilder();
                    fillUnit();
                }
                else
                {
                    dataSeriesIDs = getDataSeriesID(commoditySubCommodityID, physicalAttributeDetail, prodPracticeID, utilPracticeID, statID, sourceID);

                    //Fill Unit
                    string sql = "select [ERSUnit_Desc] from " + schemaName + "ERSUnit_LU WHERE ERSUnit_ID IN (SELECT DISTINCT [ERSDataValues_ERSUnit_ID] FROM " + schemaName + "[ERSDataValues] where ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + "))";
                    DataTable dtUnit = GetData(sql);
                    //   unitTypeTable.Merge(dt);
                    if (dtUnit.Rows.Count != 0)
                    {
                        List<string> unitList = new List<string>();
                        DataRow[] dr = dtUnit.Select();
                        unitList.Add("");
                        foreach (DataRow row in dr)
                        {
                            unitList.Add(row["ERSUnit_Desc"].ToString());
                        }
                        comboBox_unit.DataSource = unitList;
                    }
                    else
                        comboBox_unit.DataSource = null;

                    //Fill Geo and time based on Data Series.
                    //fillGeoAndTime(dataSeriesIDs); 
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
            }
            Cursor.Current = Cursors.Default;
        }

        private void comboBox_statType_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text);
        }

        private void comboBox_phyAttrDetail_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text);
        }

        private void comboBox_prodPrac_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text);
        }

        private void comboBox_utilPrac_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text);
        }

        private void comboBox_source_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text);
        }


    }
}
