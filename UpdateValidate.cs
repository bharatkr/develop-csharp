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
    public partial class UpdateValidate : Form
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
        LoadingForm loadform = new LoadingForm();
        /// <summary>
        /// The table
        /// </summary>
        DataTable table = new DataTable();
        /// <summary>
        /// The binding source1
        /// </summary>
        private BindingSource bindingSourcePhyAtt = new BindingSource();
        /// <summary>
        /// List stores physical attributes
        /// </summary>
        List<string> physicalAttributes = new List<string>();
        /// <summary>
        /// stores DB names and schema names
        /// </summary>
        private string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        private BindingSource bindingSource1 = new BindingSource();

        PrivacyForm privacyForm = new PrivacyForm();

        string apGroupId =  LandingScreen.GetGroupId(LandingScreen.newschemaName);

        BackgroundWorker backgroundWorker = new BackgroundWorker();
        string commoditySubCommodityID = "-1";
        string physicalAttributeDetail = "";
        string prodPracticeID = "-1";
        string utilPracticeID = "-1";
        string statID = "-1";
        string sourceID = "-1";
        int totalDataValues = 0;
        public static string commodityName = "";
        public static string dataVaueSource = "";
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
        private int columnNumInResultTable = 0;
        private int groupNum;
        private List<string> groups;
        private List<string> groupIds;
        private bool editable = false;
        private List<string> commodities;
        private List<string> countryList;

        List<string> physicalAttributeCats;
        List<string> prodPrac;
        List<string> utilPrac;
        private List<string> statTypes;
        private List<string> sourceList;
        DataTable DT = new DataTable();
        DataTable DTDataSeries = new DataTable();
        WaitForm waitForm = new WaitForm();
        Mail_Update mailUpdate = new Mail_Update();
        Boolean validateall = false;
        StringBuilder dataSeriesIDs = new StringBuilder();
        StringBuilder dataValueIDs = new StringBuilder();
        StringBuilder sourceIDs = new StringBuilder();

       

        /// <summary>
        /// INITIALIZER
        /// </summary>
        public UpdateValidate()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
            dataGridView.DataSource = bindingSource1;
            groupNum = 0;
        }

        /// <summary>
        /// LOADING THE FORM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Load(object sender, EventArgs e)
        {
            try
            {
                // fill group
                //int[] groupIDs = new int[4] { 1, 2, 3, 4 };  // group ids filtered for tool, refer to group LU table
                // fill all the comboBoxes
                fillGroupCombobox();

              

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
                // set font and size for grid views
                dataGridView.RowsDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }

            // clear all combobox when loading
            comboBox_commodity.Text = "";
            comboBox_country.Text = "";
            comboBox_county.Text = "";
            comboBox_group.Text = "";
            comboBox_phyAttrDetail.Text = "";
            comboBox_prodPrac.Text = "";
            comboBox_region.Text = "";
            comboBox_state.Text = "";
            comboBox_statType.Text = "";
            comboBox_unit.Text = "";
            comboBox_timetype.Text = "";
            comboBox_utilPrac.Text = "";
            button_next.Enabled = false;
            button_next.BackColor = Color.Gray;
            button_prev.Enabled = false;
            button_prev.BackColor = Color.Gray;



        }

        private void fillUtilPracComboBox(int groupNum)
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

        private string getUtilPracID(string utilPrac)
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
                return "-1";
                // throw;
            }
        }

        private void fillProdPracComboBox(int groupNum)
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
                return "-1";
                // throw;
            }
        }

        private void fillStatTypeCombobox(int groupNum)
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

        private void fillSourceCombobox(int groupNum)
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

        private void fillUnit()
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

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void fillTimetype()
        {
            //string sql = "select[ERSTimeDimensionType_Desc] from " + schemaName + "[ERSTimeDimensionType_LU]";
            //    //WHERE ERSUnit_ID IN  (SELECT DISTINCT[ERSDataValues_ERSUnit_ID] FROM " + schemaName + "[ERSDataValues]) ORDER BY ERSUnit_Desc";
            //DataTable dt = GetData(sql);
            ////   unitTypeTable.Merge(dt);
            //if (dt.Rows.Count == 0)
            //{
            //    comboBox_timetype.DataSource = null;
            //    return;
            //}
            //comboBox_timetype.DataSource = dt;
            //comboBox_timetype.ValueMember = dt.Columns[0].ColumnName;
            //comboBox_timetype.DisplayMember = dt.Columns[0].ColumnName;
            //comboBox_timetype.SelectedItem = null;
            comboBox_timetype.Items.Clear();


            ComboboxItem item_year = new ComboboxItem();
            item_year.Text = "Year";
            item_year.Value = "17,18,23,24,25,26,32,33,37";

            ComboboxItem item_quarter = new ComboboxItem();
            item_quarter.Text = "Quarter";
            item_quarter.Value = "14,15";

            ComboboxItem item_month = new ComboboxItem();
            item_month.Text = "Month";
            item_month.Value = "11,12,13,16,19,20,21,22,30,35,36";

            ComboboxItem item_week = new ComboboxItem();
            item_week.Text = "Week";
            item_week.Value = "8,9,10";

            ComboboxItem item_day = new ComboboxItem();
            item_day.Text = "Day";
            item_day.Value = "1,2,3,4,5,6,7,29";

            comboBox_timetype.Items.Add(item_year);
            comboBox_timetype.Items.Add(item_quarter);
            comboBox_timetype.Items.Add(item_month);
            comboBox_timetype.Items.Add(item_week);
            comboBox_timetype.Items.Add(item_day);
            comboBox_timetype.SelectedItem = null;

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

        private void fillPhysicalAttribute(int groupNum)
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
        /// fill group combobox with given array of group ids
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
        // method connects db
        /// <summary>
        /// Connections this instance.
        /// </summary>
        private void connection()
        {
        
            sqlconn = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            // init the connection to db
            con = new SqlConnection(sqlconn);
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


                DataTable result1 = new DataTable();
                dataAdapter.Fill(result1);
                int r = result1.Rows.Count;

                return result1;

            }
            catch (SqlException ex)
            {
                throw new Exception("Connection Error");

            }
        }

        /// <summary>
        /// retrieve button feature
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_retrieve_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            dataAdapterForPaging = null;
            // validate input values!!!!!!!!!!!!
            if (comboBox_group.Text.Equals(""))
            {
                MessageBox.Show("Please choose a Group value to continue.");
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
            string timetypedesc = "";

            // need to get time id
            // string timeID = getTimeID();

            //to get time type
            //  string timetypedesc = comboBox_timetype.SelectedItem.ToString();
            if (comboBox_timetype.SelectedItem != null)
            {
                timetypedesc = (comboBox_timetype.SelectedItem as ComboboxItem).Value.ToString();
            }

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
                string sqlcount = getDataValuesCount(dataSeriesIDs, unitID, geoID, timetypedesc);
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
                    if (MessageBox.Show("Please filter further to view data. Total '" + totalDataValues + "' Data Values found. \n\nOR \n\nDo you want to set all data values within your filter as Valid?", "Validate Data Values", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        if (MessageBox.Show("Validating large number of records may take some time. Do you really want to continue?", "Validate Data Values", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {

                            validateall = true;
                        }
                        else
                        {
                            resetResult();
                            return;
                        }
                    }
                    else
                    {
                        resetResult();
                        return;
                    }
                }

                waitForm.Show(this);
                waitForm.Refresh();
                string sqlStmt = getDataValues(dataSeriesIDs, unitID, geoID, timetypedesc);
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
                button_validDone.Visible = true;
                button_next.Enabled = true;
                button_next.BackColor = Color.White;
                button_prev.Enabled = true;
                button_prev.BackColor = Color.White;
            }

            waitForm.Hide();

            if (validateall)
            {
                button_validDone_Click(sender, e);
            }

            //Reset validate flag
            validateall = false;
        }



        /// <summary>
        /// load time id
        /// </summary>
        /// <returns></returns>
        //private string getTimeID()
        //{
        //    //string yearNum = yearValue.Text;
        //    //string yearNumTo = yearValueTo.Text;
        //    //string monthNum = monthValue.Text;
        //    //string weekNum = weekValue.Text;

        //    if (yearNumTo.Equals("") && yearNum.Equals("") && monthNum.Equals(""))
        //    {
        //        return "NO NEED TIME ID";
        //    }

        //    if (yearNumTo.Equals("") && yearNum.Equals(""))
        //        yearNum = "";
        //    else if (!yearNum.Equals("") && yearNumTo.Equals(""))
        //        yearNum = " and ERSTimeDimension_Year = '" + yearNum + "'";
        //    else if (!yearNumTo.Equals("") && yearNum.Equals(""))
        //        yearNum = " and ERSTimeDimension_Year = '" + yearNumTo + "'";
        //    else
        //        yearNum = " and ERSTimeDimension_Year BETWEEN '" + yearNum + "' AND '" + yearNumTo + "'";

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
        /// load the geo id
        /// </summary>
        /// <returns></returns>
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
        /// translate unit id to unit name
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
        /// translate stat type id to stat type name
        /// </summary>
        /// <param name="statTypeID1"></param>
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
        /*
                private string getDataValues(string dataSeriesID, int unitID, int statID, string geoID, string timeID)
                {
                    if (geoID.Equals("NO MATCH GEO ID"))
                    {
                        throw new Exception("System cannot find any data with given geography input, please adjust your search conditions and try again.");
                    }

                    if (timeID.Equals("NO MATCH TIME ID"))
                    {
                        throw new Exception("System cannot find any data with given time input, please adjust your search conditions and try again.");
                    }

                    string sql = "";


                    if (geoID.Equals("NO NEED GEO ID") && timeID.Equals("NO NEED TIME ID")) // empty geo and time inputs
                    {
                        sql = "select " +
            "distinct " +
            "ERSDataValues_ERSStatisticType_ID1 as StatType1,  " +
            "ERSDataValues_ERSUnit_ID1 as Unit1,  " +
            "ERSDataValues_AttributeValue1 as Value1, " +
            "ERSDataValues_ERSStatisticType_ID2 as StatType2,  " +
            "ERSDataValues_ERSUnit_ID2 as Unit2,  " +
            "ERSDataValues_AttributeValue2 as Value2, " +
            "ERSDataValues_ERSStatisticType_ID3 as StatType3,  " +
            "ERSDataValues_ERSUnit_ID3 as Unit3,  " +
            "ERSDataValues_AttributeValue3 as Value3, " +
            "ERSDataValues_ID as CoSD_ID,  " +
             "ERSTimeDimension_LU.ERSTimeDimension_Year as Year, " +
            "ERSTimeDimension_LU.ERSTimeDimension_Month as Month, " +
            "ERSTimeDimension_LU.ERSTimeDimension_Week as Week, " +
            "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
            "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
            "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +

            "from  " +
            schemaName + "ERSDataValues,  " +
            schemaName + "ERSGeographyDimension_LU, " +
            schemaName + "ERSTimeDimension_LU  " +
        " where " +
             " ERSDataValues_ERSCommodity_ID = " + dataSeriesID +
              " and  " +
              "(  " +
              "(ERSDataValues_ERSUnit_ID1 = " + unitID + " and ERSDataValues_ERSStatisticType_ID1 = " + statID + " ) " +
              " or (ERSDataValues_ERSUnit_ID2 = " + unitID + " and  ERSDataValues_ERSStatisticType_ID2 = " + statID + " ) " +
              " or (ERSDataValues_ERSUnit_ID3 = " + unitID + " and ERSDataValues_ERSStatisticType_ID3 = " + statID + " ) " +
              ") " +
               " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
               " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID"
               ;
                    }
                    else if (geoID.Equals("NO NEED GEO ID") && !timeID.Equals("NO NEED TIME ID"))  // only time inputs
                    {


                        sql = "select " +
        "distinct " +
        "ERSDataValues_ERSStatisticType_ID1 as StatType1,  " +
        "ERSDataValues_ERSUnit_ID1 as Unit1,  " +
        "ERSDataValues_AttributeValue1 as Value1, " +
        "ERSDataValues_ERSStatisticType_ID2 as StatType2,  " +
        "ERSDataValues_ERSUnit_ID2 as Unit2,  " +
        "ERSDataValues_AttributeValue2 as Value2, " +
        "ERSDataValues_ERSStatisticType_ID3 as StatType3,  " +
        "ERSDataValues_ERSUnit_ID3 as Unit3,  " +
        "ERSDataValues_AttributeValue3 as Value3, " +
        "ERSDataValues_ID as CoSD_ID,  " +
        "ERSTimeDimension_LU.ERSTimeDimension_Year as Year, " +
        "ERSTimeDimension_LU.ERSTimeDimension_Month as Month, " +
        "ERSTimeDimension_LU.ERSTimeDimension_Week as Week, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +

        "from  " +
        schemaName + "ERSDataValues,  " +
        schemaName + "ERSGeographyDimension_LU, " +
        schemaName + "ERSTimeDimension_LU  " +
        " where " +
        " ERSDataValues_ERSCommodity_ID = " + dataSeriesID +
        " and  " +
        "(  " +
        "(ERSDataValues_ERSUnit_ID1 = " + unitID + " and ERSDataValues_ERSStatisticType_ID1 = " + statID + " ) " +
        " or (ERSDataValues_ERSUnit_ID2 = " + unitID + " and  ERSDataValues_ERSStatisticType_ID2 = " + statID + " ) " +
        " or (ERSDataValues_ERSUnit_ID3 = " + unitID + " and ERSDataValues_ERSStatisticType_ID3 = " + statID + " ) " +
        ") " +
        " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
        " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
        " and " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID = " + timeID
        ;

                    }
                    else if (!geoID.Equals("NO NEED GEO ID") && timeID.Equals("NO NEED TIME ID"))  // only geo inputs
                    {
                        sql = "select " +
        "distinct " +
        "ERSDataValues_ERSStatisticType_ID1 as StatType1,  " +
        "ERSDataValues_ERSUnit_ID1 as Unit1,  " +
        "ERSDataValues_AttributeValue1 as Value1, " +
        "ERSDataValues_ERSStatisticType_ID2 as StatType2,  " +
        "ERSDataValues_ERSUnit_ID2 as Unit2,  " +
        "ERSDataValues_AttributeValue2 as Value2, " +
        "ERSDataValues_ERSStatisticType_ID3 as StatType3,  " +
        "ERSDataValues_ERSUnit_ID3 as Unit3,  " +
        "ERSDataValues_AttributeValue3 as Value3, " +
        "ERSDataValues_ID as CoSD_ID,  " +
        "ERSTimeDimension_LU.ERSTimeDimension_Year as Year, " +
        "ERSTimeDimension_LU.ERSTimeDimension_Month as Month, " +
        "ERSTimeDimension_LU.ERSTimeDimension_Week as Week, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +

        "from  " +
        schemaName + "ERSDataValues,  " +
        schemaName + "ERSGeographyDimension_LU, " +
        schemaName + "ERSTimeDimension_LU  " +
        " where " +
        " ERSDataValues_ERSCommodity_ID = " + dataSeriesID +
        " and  " +
        "(  " +
        "(ERSDataValues_ERSUnit_ID1 = " + unitID + " and ERSDataValues_ERSStatisticType_ID1 = " + statID + " ) " +
        " or (ERSDataValues_ERSUnit_ID2 = " + unitID + " and  ERSDataValues_ERSStatisticType_ID2 = " + statID + " ) " +
        " or (ERSDataValues_ERSUnit_ID3 = " + unitID + " and ERSDataValues_ERSStatisticType_ID3 = " + statID + " ) " +
        ") " +
        " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
        " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
        " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID = " + geoID
        ;
                    }
                    else  // have both ...
                    {
                        sql = "select " +
        "distinct " +
        "ERSDataValues_ERSStatisticType_ID1 as StatType1,  " +
        "ERSDataValues_ERSUnit_ID1 as Unit1,  " +
        "ERSDataValues_AttributeValue1 as Value1, " +
        "ERSDataValues_ERSStatisticType_ID2 as StatType2,  " +
        "ERSDataValues_ERSUnit_ID2 as Unit2,  " +
        "ERSDataValues_AttributeValue2 as Value2, " +
        "ERSDataValues_ERSStatisticType_ID3 as StatType3,  " +
        "ERSDataValues_ERSUnit_ID3 as Unit3,  " +
        "ERSDataValues_AttributeValue3 as Value3, " +
        "ERSDataValues_ID as CoSD_ID,  " +
        "ERSTimeDimension_LU.ERSTimeDimension_Year as Year, " +
        "ERSTimeDimension_LU.ERSTimeDimension_Month as Month, " +
        "ERSTimeDimension_LU.ERSTimeDimension_Week as Week, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +

        "from  " +
        schemaName + "ERSDataValues,  " +
        schemaName + "ERSGeographyDimension_LU, " +
        schemaName + "ERSTimeDimension_LU  " +
        " where " +
        " ERSDataValues_ERSCommodity_ID = " + dataSeriesID +
        " and  " +
        "(  " +
        "(ERSDataValues_ERSUnit_ID1 = " + unitID + " and ERSDataValues_ERSStatisticType_ID1 = " + statID + " ) " +
        " or (ERSDataValues_ERSUnit_ID2 = " + unitID + " and  ERSDataValues_ERSStatisticType_ID2 = " + statID + " ) " +
        " or (ERSDataValues_ERSUnit_ID3 = " + unitID + " and ERSDataValues_ERSStatisticType_ID3 = " + statID + " ) " +
        ") " +
        " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
        " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
        " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID = " + geoID +
        " and " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID = " + timeID
        ;

                    }


                    Console.WriteLine(sql);
                    return sql;
                    //   DataTable result = GetData(sql);
                    //   return result;

                }
        */

        private string getDataValues(StringBuilder dataSeriesIDs, int unitID, string geoID, string timetypedesc)
        {

            if (geoID.Equals("NO MATCH GEO ID"))
            {
                MessageBox.Show("System cannot find any data with given geography input, please adjust your search conditions and try again.");
                return "";
            }



            string sql = "";
            string timetypequery = timetypedesc;
            if (timetypedesc != "")
            {
                timetypequery = " and " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID in (" + timetypequery + ")";
                //like " + "'%" + timetypedesc + "%'";
            }


            if (geoID.Equals("NO NEED GEO ID") && unitID != -1) // empty geo and time inputs
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
     + timetypequery
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
     + timetypequery;
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
      + timetypequery;

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
      + timetypequery;

            }
            else if (!geoID.Equals("NO NEED GEO ID") && unitID != -1)  // only geo inputs
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
     + timetypequery;
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
      + timetypequery;
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
     + timetypequery;

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
      + timetypequery;

            }

            Console.WriteLine(sql);
            return sql;
            //   DataTable result = GetData(sql);
            //   return result;

        }

        private string getDataValuesCount(StringBuilder dataSeriesIDs, int unitID, string geoID, string timetypedesc)
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

            string timetypequery = timetypedesc;
            if (timetypedesc != "")
            {
                timetypequery = " and " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID in (" + timetypequery + ")";
                //like " + "'%" + timetypedesc + "%'";
            }

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
    schemaName + "ERSTimeDimension_LU,  " +
   schemaName + "[ERSTimeDimensionType_LU]" +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID"
      + timetypequery;
            }
            else if (geoID.Equals("NO NEED GEO ID") && unitID == -1) // empty geo, time and unit inputs
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU,  " +
     schemaName + "[ERSTimeDimensionType_LU]" +
" where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID"
      + timetypequery;
            }
            else if (geoID.Equals("NO NEED GEO ID") && unitID != -1)  // only time inputs
            {


                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU,  " +
     schemaName + "[ERSTimeDimensionType_LU]" +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName
       + timetypequery;

            }
            else if (geoID.Equals("NO NEED GEO ID") && unitID == -1)  // only time and no Unit inputs
            {


                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU,  " +
     schemaName + "[ERSTimeDimensionType_LU]" +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName
       + timetypequery;
            }
            else if (!geoID.Equals("NO NEED GEO ID") && unitID != -1)  // only geo inputs
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU,  " +
    schemaName + "ERSTimeDimensionType_LU" +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")"
       + timetypequery;
            }
            else if (!geoID.Equals("NO NEED GEO ID") && unitID == -1)  // only geo and no unit inputs
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU,  " +
     schemaName + "[ERSTimeDimensionType_LU]" +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")"
        + timetypequery;
            }
            else if (!geoID.Equals("NO NEED GEO ID") && unitID != -1)  // have both ...
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU,  " +
     schemaName + "[ERSTimeDimensionType_LU]" +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + unitID +
     " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")" +
      " and " + schemaName
       + timetypequery;

            }
            else if (!geoID.Equals("NO NEED GEO ID") && unitID == -1)  // have both but not unit id ...
            {
                sql = "select " +
    "COUNT(*) as Records " +
    "from  " +
    schemaName + "ERSDataValues,  " +
    schemaName + "ERSUnit_LU, " +
    schemaName + "ERSGeographyDimension_LU, " +
    schemaName + "ERSTimeDimension_LU,  " +
     schemaName + "[ERSTimeDimensionType_LU]" +
    " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID IN (" + geoID + ")" +
      " and " + schemaName
             + timetypequery;

            }

            return sql;

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


        /// <summary>
        /// update values in db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // validate value before doing anything
            // only a cell that has value before can be updated
            // validate value before doing anything
            // only a cell that has value before can be updated
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();
            CellStyle.BackColor = Color.Yellow;
            dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = CellStyle;
            if (editable == false)
            {
                reloadCurrentpage();
                return;
            }


            // load new value
            string newValue = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            // new value needs to be a number
            Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            if (!numberchk.IsMatch(newValue))
            {
                MessageBox.Show("Please type a valid number value.");
                reloadCurrentpage();
                return;
            }

            // ask for conformation before delete
            if (MessageBox.Show("Do you really want to update this value?", "Confirm update", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // load ID
                string dataValueID = dataGridView.Rows[e.RowIndex].Cells["CoSD_Id"].Value.ToString();
                commodityName = "";
                dataVaueSource = "";
                string sql = "";
                //Load data
                try
                {
                    string sqlCommodity = "SELECT [ERSCommoditySubCommodity_Desc] FROM " + schemaName + "[AP_Taxonomy_withValues] where ERSDataValues_ID = " + dataValueID;
                    DataTable dt = GetData(sqlCommodity);
                    DataRow[] dr = dt.Select();
                    commodityName = dr[0]["ERSCommoditySubCommodity_Desc"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                }
                dataVaueSource = dataGridView.Rows[e.RowIndex].Cells["Source"].Value.ToString();


                if (dataVaueSource == "NASS Survey" || dataVaueSource == "NASS Census" || dataVaueSource == "US exports" || dataVaueSource == "US imports" ||
                    dataVaueSource == "WASDE" || dataVaueSource == "Market News" || dataVaueSource == "Marketing Orders" || dataVaueSource == "Producer Price Index Commodity Data" ||
                    dataVaueSource == "Consumer Price Index" || dataVaueSource == "PSD" || dataVaueSource == "FAS" || dataVaueSource == "Producer Price Index Industry Data" ||
                    dataVaueSource == "Consumer Price Index, Average Price Data" || dataVaueSource == "US imports and exports" || dataVaueSource == "BEA")
                {
                    try
                    {
                        mailUpdate.ShowDialog();

                        if (mailUpdate.mailSent() == "Sent")
                        {
                            //mailUpdate.Hide();
                            // build sql
                            sql = "update " + schemaName + "ERSDataValues set " +
                                " ERSDataValues_AttributeValue = '" + newValue + "' , [ERSDataValues_DataRowLifecyclePhaseID] = 9 " +
                                " where ERSDataValues_ID = " + dataValueID;
                            GetData(sql);
                            saveAction(sql);
                            MessageBox.Show("Value is updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Value is Not updated. Please contact technical team.");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                    }

                }
                else
                {
                    if (MessageBox.Show("This update cannot be undone.\n\nPress 'Yes' to continue.", "Confirm update", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        try
                        {
                            // build sql
                            sql = "update " + schemaName + "ERSDataValues set " +
                                " ERSDataValues_AttributeValue = '" + newValue + "'" +
                                " where ERSDataValues_ID = " + dataValueID;
                            GetData(sql);
                            saveAction(sql);
                            MessageBox.Show("Value is updated successfully.");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                        }
                    }
                }
            }
        }

        public string mailData()
        {
            return "Commodity-SubCommodity: " + commodityName + " and Source: " + dataVaueSource;
        }

        /// <summary>
        /// re-load the current page with current page number
        /// </summary>
        private void reloadCurrentpage()
        {

            table = new DataTable();
            dataAdapterForPaging.Fill(itemsPerPage * (currentPage - 1), itemsPerPage, table);


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

            // display the current page number and total page number
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
        /// <summary>
        /// move to previous page in data grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_prev_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                table = new DataTable();
                // load previous page content
                dataAdapterForPaging.Fill(itemsPerPage * (currentPage - 1), itemsPerPage, table);


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

                // display current page number and total page number
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
        /// move to the next page in data grid view
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

                // display page number
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
            try
            {
                GetData(sql);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void button_back_Click(object sender, EventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Close();
            this.Parent = null;
        }
        /// <summary>
        /// reset everything to empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_reset_Click(object sender, EventArgs e)
        {
            comboBox_commodity.Text = null;
            comboBox_group.SelectedItem = null;
            comboBox_phyAttrDetail.Text = null;
            comboBox_prodPrac.Text = null;
            comboBox_statType.Text = null;
            comboBox_source.Text = null;
            comboBox_unit.Text = null;
            comboBox_utilPrac.Text = null;
            comboBox_country.Text = "";
            comboBox_region.Text = "";
            comboBox_state.Text = "";
            comboBox_county.Text = "";
            //monthValue.Text = "";
            //yearValue.Text = "";
            //yearValueTo.Text = "";
            label_pageNum.Text = "";
            bindingSource1.DataSource = null;
            button_validDone.Visible = false;
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
        /// Time consuming stored procedure execution
        /// </summary>
        private void validDone_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dtGetData = new DataTable();
            backgroundWorker.ReportProgress(1);

            try
            {

                //Comma seperated Data series ids
                dataValueIDs = new StringBuilder();
                DataRow lastRow = DT.Rows[DT.Rows.Count - 1];
                string lastItem = lastRow["COSD_ID"].ToString();
                foreach (DataRow dr in DT.Rows)
                {
                    if (dr["COSD_ID"].ToString() != lastItem)
                        dataValueIDs.Append(dr["COSD_ID"].ToString() + ",");
                    else dataValueIDs.Append(dr["COSD_ID"].ToString());
                }
                string sql = "update " + schemaName + "ERSDataValues set " +
                        " ERSDataValues_DataRowLifecyclePhaseID = '3' " +
                        " where ERSDataValues_ID IN (" + dataValueIDs + " ) ";
                // execute
                dtGetData = GetData(sql);
                saveAction("Data Values Validated : " + dataValueIDs);
                if (dtGetData == null)
                {
                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                    return;
                }
                backgroundWorker.ReportProgress(2);
                MessageBox.Show((DT.Rows.Count) + " records Validated Successfully");
            }
            catch (Exception ex)
            {
                backgroundWorker.ReportProgress(0);
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }

        }


        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void validDone_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                waitForm.ShowDialog(this);
            }
            if (e.ProgressPercentage == 0)
            {
                waitForm.Hide();

            }
            if (e.ProgressPercentage == 2)
            {
                waitForm.Hide();

            }
        }


        /// <summary>
        /// clear data grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_validDone_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();
            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(validDone_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(validDone_ProgressChanged);

            backgroundWorker.WorkerReportsProgress = true;

            if (button_validDone.Visible)
            {
                // ask for conformation before delete
                if (MessageBox.Show("All pages will be Validated. Do you want to continue?", "Confirm validate", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    backgroundWorker.RunWorkerAsync();
                    // refresh
                    bindingSource1.DataSource = null;
                    label_pageNum.Text = "";
                    button_next.Enabled = false;
                    button_prev.Enabled = false;
                    //refreshDatagridview();
                    button_validDone.Visible = false;
                }
                else
                {
                    if (waitForm.Visible)
                        waitForm.Hide();
                }
            }
            else
            {
                backgroundWorker.RunWorkerAsync();
                // refresh
                bindingSource1.DataSource = null;
                label_pageNum.Text = "";
                button_next.Enabled = false;
                button_prev.Enabled = false;
                //refreshDatagridview();
                if (waitForm.Visible)
                    waitForm.Hide();
            }


        }



        /// <summary>
        /// fill commodity comboBox with selected group
        /// </summary>
        /// <param name="groupNum"></param>
        private void fillCommodityComboBox(int groupNum)
        {
            commodities = new List<string>();

            string sql = "select ERSCommoditySubCommodity_Desc from " + schemaName + "ERSCommoditySubCommodity_LU where ERSCommoditySubCommodity_GroupID = " + groupNum +
                " and ERSCommoditySubCommodity_ID IN (SELECT DISTINCT ds.ERSCommoditySubCommodity_ID FROM " + schemaName +
                "[ERSDataValues] dv left join " + schemaName + "[ERSCommodityDataSeries] ds on dv.ERSDataValues_ERSCommodity_ID=ds.ERSCommodity_ID) ORDER BY ERSCommoditySubCommodity_Desc";

            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            commodities.Add("");
            foreach (DataRow row in dr)
            {
                commodities.Add(row["ERSCommoditySubCommodity_Desc"].ToString());
            }

            comboBox_commodity.DataSource = commodities;
        }

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
        /// validating year input
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
        ///// validating year input
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void yearValueTo_Validating(object sender, CancelEventArgs e)
        //{
        //    try
        //    {
        //        // check if it has four digits
        //        if (!yearValueTo.Text.Equals("") && (int.Parse(yearValueTo.Text) <= 1000 || int.Parse(yearValueTo.Text) >= 9999))
        //        {
        //            errorProvider1.SetError(yearValueTo, "Invalid input!\nPlease type a year between 1000 and 9999.");
        //            yearValueTo.BackColor = Color.Red;
        //        }
        //        else
        //        {
        //            errorProvider1.SetError(yearValueTo, "");
        //            yearValueTo.BackColor = Color.White;
        //        }
        //    }
        //    catch (Exception)   // in case it could not get parsed to int
        //    {

        //    }
        //}
        ///// <summary>
        ///// validating year input
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
        ///// <summary>
        ///// validating year input
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void yearValueTo_TextChanged(object sender, EventArgs e)
        //{
        //    Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
        //    // check if it's a number
        //    if (!yearValueTo.Text.Equals("") && !numberchk.IsMatch(yearValueTo.Text))
        //    {
        //        errorProvider1.SetError(yearValueTo, "Invalid input!\nPlease type numeric characters only.");
        //        yearValueTo.BackColor = Color.Red;
        //        return;
        //    }
        //    else if (!yearValueTo.Text.Equals("") && numberchk.IsMatch(yearValueTo.Text))
        //    {
        //        errorProvider1.SetError(yearValueTo, "");
        //        yearValueTo.BackColor = Color.White;

        //    }
        //    else
        //    {
        //        errorProvider1.SetError(yearValueTo, "");
        //        yearValueTo.BackColor = Color.White;
        //    }
        //}
        /// <summary>
        /// validating week input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         /*
        private void weekValue_TextChanged(object sender, EventArgs e)
        {
            if (!weekValue.Text.Equals(""))
                monthValue.Enabled = false;
            else
                monthValue.Enabled = true;

            Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            // check if it's a number
            if (!weekValue.Text.Equals("") && !numberchk.IsMatch(weekValue.Text))
            {
                errorProvider1.SetError(weekValue, "Invalid input!\nPlease type numeric characters only.");
                weekValue.BackColor = Color.Red;
                return;
            }
            else if (!weekValue.Text.Equals("") && numberchk.IsMatch(weekValue.Text))
            {
                errorProvider1.SetError(weekValue, "");
                weekValue.BackColor = Color.White;

            }
            else
            {
                errorProvider1.SetError(weekValue, "");
                weekValue.BackColor = Color.White;

            }

            // check if it is between 1 and 52
            if (!weekValue.Text.Equals("") && (int.Parse(weekValue.Text) < 1 || int.Parse(weekValue.Text) > 52))
            {
                errorProvider1.SetError(weekValue, "Invalid input!\nPlease type a week number between 1 and 52.");
                weekValue.BackColor = Color.Red;
            }
            else
            {
                errorProvider1.SetError(weekValue, "");
                weekValue.BackColor = Color.White;
            }
        }
          */
        /// <summary>
        /// validating month values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*
        private void monthValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            // if select one value 
            // disable and clear weekValue input
            if (!monthValue.SelectedItem.Equals(""))
            {
                weekValue.Enabled = false;
                weekValue.Text = "";
            }
            else
                weekValue.Enabled = true;
        }
        */
        /// <summary>
        /// validating input
        /// only cells that have old values can be updated
        /// empty cells are not able to be added new values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string oldValue = dataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
            if (e.FormattedValue.ToString().Equals(oldValue))
            {
                return;
            }
            if (oldValue.Equals(""))
            {
                MessageBox.Show("Only non-empty cells can be edited.");
                editable = false;
            }
            else
                editable = true;
        }


        private void comboBox_commodity_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Refreshing all comboboxes


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (privacyForm.IsDisposed == true)
            {
                PrivacyForm MRF = new PrivacyForm();
                MRF.Show();
            }
            else
            {
                privacyForm.Show();
            }
        }

        private void update_button_Click(object sender, EventArgs e)
        {



        }

        private void comboBox_commodity_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        private void comboBox_commodity_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }

        private void fillAllComboBoxes(string commoditySubCommodity, string physicalAttributeDetail, string prodPractice, string utilPractice, string statType, string source, string timetype)
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

                    //Fill time type

                    //string timetypequery = "select [ERSTimeDimensionType_Desc] from " + schemaName + "[ERSTimeDimensionType_LU]";
                    //    //WHERE ERSUnit_ID IN (SELECT DISTINCT [ERSDataValues_ERSUnit_ID] FROM " + schemaName + "[ERSDataValues] where ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + "))";
                    //DataTable dtTimetype = GetData(timetypequery);

                    //if (dtTimetype.Rows.Count != 0)
                    //{
                    //    List<string> timetypeList = new List<string>();
                    //    DataRow[] dr = dtTimetype.Select();
                    //    timetypeList.Add("");
                    //    foreach (DataRow row in dr)
                    //    {
                    //        timetypeList.Add(row["ERSUnit_Desc"].ToString());
                    //    }
                    //    comboBox_timetype.DataSource = timetypeList;
                    //}
                    //else
                    //    comboBox_timetype.DataSource = null;

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
        /**
        private void fillGeoAndTime(StringBuilder dataSeriesIDs)
        {
            //Fill Country
            string sqlCountry = "select Distinct [ERSGeographyDimension_Country] from " + schemaName + "[ERSGeographyDimension_LU] WHERE [ERSGeographyDimension_ID] IN (SELECT DISTINCT [ERSDataValues_ERSGeography_ID] FROM " + schemaName + "[ERSDataValues] where ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + "))";
            DataTable dtCountry = GetData(sqlCountry);
            //   unitTypeTable.Merge(dt);
            if (dtCounty.Rows.Count != 0)
            {
                comboBox_county.DataSource = dtCounty;
                comboBox_county.ValueMember = dtCounty.Columns[0].ColumnName;
                comboBox_county.DisplayMember = dtCounty.Columns[0].ColumnName;
                comboBox_county.SelectedItem = null;
            }
            else
                comboBox_country.DataSource = null;
        }
        */
        private void resetResult()
        {
            bindingSource1.DataSource = null;
            label_pageNum.Text = "";
            button_validDone.Visible = false;
            button_next.Enabled = false;
            button_prev.Enabled = false;
        }


        private void comboBox_utilPrac_SelectionChangeCommitted(object sender, EventArgs e)
        {


        }

        private void comboBox_group_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //start wait form
            loadform.Show(this);
            loadform.Refresh();
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //Setting flag to load mothos to fill other comboboxes - false
                // need to translate group name into id
                groupNum = getGroupID(comboBox_group.Text);
                //fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text);
                //fillAllComboBoxes("-1", "-1", "-1", "-1", "-1");
                //reset comboboxes
                fillCommodityComboBox(groupNum);
                fillPhysicalAttribute(groupNum);
                fillProdPracComboBox(groupNum);
                fillUtilPracComboBox(groupNum);
                fillStatTypeCombobox(groupNum);
                fillSourceCombobox(groupNum);
                fillUnit();
                fillTimetype();

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
                comboBox_timetype.Enabled = true;
                comboBox_country.Enabled = true;
                comboBox_region.Enabled = true;
                comboBox_state.Enabled = true;
                comboBox_county.Enabled = true;

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
            }
            Cursor.Current = Cursors.Default;
            loadform.Hide();
        }


        private void comboBox_commodity_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text, comboBox_timetype.Text);
        }

        private void comboBox_phyAttrDetail_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text, comboBox_timetype.Text);
        }

        private void comboBox_prodPrac_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text, comboBox_timetype.Text);
        }

        private void comboBox_utilPrac_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text, comboBox_timetype.Text);
        }

        private void comboBox_statType_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text, comboBox_timetype.Text);
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

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_source_Validated(object sender, EventArgs e)
        {
            fillAllComboBoxes(comboBox_commodity.Text, comboBox_phyAttrDetail.Text, comboBox_prodPrac.Text, comboBox_utilPrac.Text, comboBox_statType.Text, comboBox_source.Text, comboBox_timetype.Text);
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCellStyle CellStyle = new DataGridViewCellStyle();
            CellStyle.BackColor = Color.Yellow;
            dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Style = CellStyle;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox_country_SelectionChangeCommitted(object sender, EventArgs e)
        {            //Disabling geo
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

        private void comboBox_country_Validated(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_statType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_group_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
