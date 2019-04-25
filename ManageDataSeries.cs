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
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoSD_Tool
{
    public partial class HandEntered : Form
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
        /// The data adapter
        /// </summary>
        private SqlDataAdapter dataAdapterForPaging = new SqlDataAdapter();

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
        /// <summary>
        /// define schema name for database
        /// </summary>
        private string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        /// <summary>
        /// flag used for recording if a new time dimension is inserted
        /// </summary>
        private bool newTimeDimension = false;
        /// <summary>
        /// flag used for recording if a new data series is inserted
        /// </summary>
        private bool newDataSeries = false;
        /// <summary>
        /// list used for storing physical attributes
        /// </summary>
        List<string> physicalAttributes = new List<string>();

        StringBuilder group = new StringBuilder();
        /// <summary>
        /// list used for storing commodity ids
        /// </summary>
        /// 
        static int currentPageIndex = 0;
        /// <summary>
        /// The items per page
        /// </summary>
        private int pageSize = 14;
        List<string> commodities;
        List<string> groupIds;
        List<string> groups;
        List<string> dataSeriesID = new List<string>();
        string dataSeriesSQL = "";
        StringBuilder dataSeriesIDs = new StringBuilder();
        DataTable DTDataSeries = new DataTable();

        WaitForm waitForm = new WaitForm();
        /// <summary>
        /// List used for storing physical attribute categories
        /// </summary>
        List<string> physicalAttribute;

        string apGroupId = LandingScreen.GetGroupId(LandingScreen.newschemaName);


        /// <summary>
        /// variable flag for specifying analysts' ability to insert new dataseries (data series are better created through the 
        /// request form on this page.)
        /// </summary>
        bool insertNewDataSeries = false;

        private List<string> statTypes;

        // initializer
        public HandEntered()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
        }



        /// <summary>
        /// Reset all inputs
        /// 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            ResetTextBoxes(this);
        }

        /// <summary>
        /// Reset all inputs
        /// </summary>
        /// <param name="ctrl">The source of the event.</param>
        public void ResetTextBoxes(Control ctrl)
        {
            // clear every textbox
            foreach (Control c in ctrl.Controls)
            {
                TextBox tb = c as TextBox;
                if (tb != null && !tb.Name.Equals("textBox_email"))
                    tb.Clear();
                else
                    ResetTextBoxes(c);
            }

            // clear all dropdown lists
            comboBox_Group.Text = "";
            comboBox_commodity.Text = "";
            comboBox_phyAttr.Text = "";
            comboBox_prodPrac.Text = "";
            comboBox_utilPrac.Text = "";
            comboBox_statType.Text = "";
            labelDataSeriesResult.Visible = false;
            groupBox_summary.Visible = false;
            label_request.Visible = false;
            button_prev.Enabled = false;
            //button_prev.BackColor = Color.Gray;
            button_next.Enabled = false;
            //button_next.BackColor = Color.Gray;
            dataGridView.DataSource = null;


        }


        /// <summary>
        /// Pre fill all comboBoxes with items loaded from db
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void Upload_Load(object sender, EventArgs e)
        {

            try
            {
                fillGroupCombobox();

                // load group
                //groupIds = new List<string>();
                //groupIds = getGroupIds();
                if (groupIds.Count != 0)
                {
                    group = new StringBuilder();
                    string lastItem = groupIds[groupIds.Count - 1];
                    foreach (string item in groupIds)
                    {
                        if (item != lastItem)
                            group.Append(item + ",");
                        else group.Append(item);
                    }
                    // load commodity
                    fillCommodityCombobox(group);
                    fillPhysicalAttribute();
                    fillProdPracComboBox();
                    fillUtilPracComboBox();
                    fillStatTypeCombobox();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }
            //startup page
            //label23.Visible = true;
            //label24.Visible = true;
            //label25.Visible = true;
            //  groupBox_Request.Visible = true;
            //   button_sendEmail.Visible = true;
            // set everything to empty when open
            ResetTextBoxes(this);
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
            groups.Add("");
            foreach (string groupID in groupIds)
            {
                string sql = "select ERSGroup_Desc from " + schemaName + "ERSGroup_LU where ERSGroup_ID = " + groupID;
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                foreach (DataRow row in dr)
                {
                    groups.Add(row["ERSGroup_Desc"].ToString());
                }
            }
            comboBox_Group.DataSource = groups;
            comboBox_Group.SelectedItem = null;
        }
        /// <summary>
        /// load units for combobox
        /// </summary>
        /// <param name="unitTypes"> given an array of unit types</param>
        /// <param name="comboBox"> assign it to a combobox</param>
        private void fillWeightCombobox(string[] unitTypes, ComboBox comboBox) // fills the combobox with the queried units
        {
            DataTable unitTypeTable = new DataTable();
            foreach (string unitType in unitTypes)
            {
                // look up unit to filter units for weight
                string sql = "select distinct ERSUnit_Desc from " + schemaName + "ERSUnit_LU where ERSUnit_Type = '" + unitType + "' ";
                DataTable dt = GetData(sql);
                unitTypeTable.Merge(dt);
            }

            // if no results
            // set empty and return
            if (unitTypeTable.Rows.Count == 0)
            {
                comboBox.DataSource = null;
                return;
            }
            comboBox.DataSource = unitTypeTable;
            comboBox.ValueMember = unitTypeTable.Columns[0].ColumnName;
            comboBox.DisplayMember = unitTypeTable.Columns[0].ColumnName;

        }

        /// <summary>
        /// load a list of commodity names with given group ids specified in a variable earlier
        /// </summary>
        /// <param name="groupIDs">input group ids as parameter</param>
        private void fillCommodityCombobox(StringBuilder group)
        {
            commodities = new List<string>();
            string sql = @"SELECT [ERSCommoditySubCommodity_Desc] FROM " + schemaName + "[ERSCommoditySubCommodity_LU] WHERE [ERSCommoditySubCommodity_GroupID] IN ( " + group + " ) ORDER BY [ERSCommoditySubCommodity_Desc]";

            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            foreach (DataRow row in dr)
            {
                commodities.Add(row["ERSCommoditySubCommodity_Desc"].ToString());
            }
            comboBox_commodity.DataSource = commodities;
            comboBox_commodity.SelectedItem = null;

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
        ///  Fills out combo box of physical attribute categories
        /// </summary>
        private void fillPhysicalAttribute()
        {
            physicalAttribute = new List<string>();
            try
            {
                string sql = "select DISTINCT[ERSCommodity_PhysicalAttribute_Desc] from " + schemaName + "[ERSCommodityDataSeries] ORDER BY [ERSCommodity_PhysicalAttribute_Desc]";
                //    DataTable dt = GetData(sql);
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                foreach (DataRow row in dr)
                {
                    physicalAttribute.Add(row["ERSCommodity_PhysicalAttribute_Desc"].ToString());
                }
                comboBox_phyAttr.DataSource = physicalAttribute;
                comboBox_phyAttr.SelectedItem = null;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        private void fillProdPracComboBox()
        {
            string sql = "select [ERSProdPractice_Desc] from " + schemaName + "ERSProdPractice_LU ORDER BY [ERSProdPractice_Desc]";
            DataTable dt = GetData(sql);
            //   unitTypeTable.Merge(dt);
            if (dt.Rows.Count == 0)
            {
                comboBox_prodPrac.DataSource = null;
                return;
            }
            comboBox_prodPrac.DataSource = dt;
            comboBox_prodPrac.ValueMember = dt.Columns[0].ColumnName;
            comboBox_prodPrac.DisplayMember = dt.Columns[0].ColumnName;
            comboBox_prodPrac.SelectedItem = null;
        }
        private void fillUtilPracComboBox()
        {
            string sql = "select [ERSUtilPractice_Desc] from " + schemaName + "ERSUtilPractice_LU ORDER BY [ERSUtilPractice_Desc]";
            DataTable dt = GetData(sql);
            //   unitTypeTable.Merge(dt);
            if (dt.Rows.Count == 0)
            {
                comboBox_utilPrac.DataSource = null;
                return;
            }
            comboBox_utilPrac.DataSource = dt;
            comboBox_utilPrac.ValueMember = dt.Columns[0].ColumnName;
            comboBox_utilPrac.DisplayMember = dt.Columns[0].ColumnName;
            comboBox_utilPrac.SelectedItem = null;
        }

        private void fillStatTypeCombobox()
        {
            statTypes = new List<string>();
            string sql = "select [ERSStatisticType_Attribute] from " + schemaName + "ERSStatisticType_LU ORDER BY [ERSStatisticType_Attribute]";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            foreach (DataRow row in dr)
            {
                statTypes.Add(row["ERSStatisticType_Attribute"].ToString());
            }
            comboBox_statType.DataSource = statTypes;
            comboBox_statType.SelectedItem = null;
        }

        private void fillUnit()
        {

            //string lastItem = dataSeriesID[dataSeriesID.Count - 1];
            //foreach (string item in dataSeriesID)
            //{
            //    if (item != lastItem)
            //        dataSeriesIDs.Append(item + ",");
            //    else dataSeriesIDs.Append(item);
            //}

            StringBuilder dataSeriesIDs = new StringBuilder();
            DataRow lastRow = DT.Rows[DT.Rows.Count - 1];
            string lastItem = lastRow[0].ToString();

            if (textBox_DSid.Text != "")
            {
                dataSeriesIDs = dataSeriesIDs.Append(textBox_DSid.Text.ToString());
            }
            else
            {
                foreach (DataRow dr in DT.Rows)
                {
                    //dataSeriesID.Add(dr[0].ToString());

                    //if (dr[0].ToString() != lastItem)
                    dataSeriesIDs.Append(dr[0].ToString() + ",");
                    // else dataSeriesIDs.Append(dr[0].ToString());

                }

                dataSeriesIDs.Length--;

            }





            string sql = "select [ERSUnit_Desc] from " + schemaName + "ERSUnit_LU WHERE ERSUnit_ID IN (SELECT DISTINCT [ERSDataValues_ERSUnit_ID] FROM "
                + schemaName + "[ERSDataValues] where ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + "))";
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
        /// Handles the FormClosing event of the Upload control.
        /// When the "x" button on the top right corner gets clicked, it just hides the window,
        /// instead of disposing (killing the process) it. 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>



        // method connects db
        /// <summary>
        /// Connections this instance.
        /// </summary>
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


        /// <summary>
        /// execute sql command
        /// if select, then return DataTable result
        /// </summary>
        /// <param name="selectCommand">The sql command.</param>
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
            catch (SqlException ex)
            {
                throw new Exception("Connection Error");
            }
        }
        /// <summary>
        /// search unit id with given unit name
        /// </summary>
        /// <param name="unitName"> unit name </param>
        /// <returns> unit id in db </returns>
        private string getUnitID(string unitName)
        {
            try
            {
                string sql = "select ERSUnit_ID from " + schemaName + "ERSUnit_LU where ERSUnit_Desc = '" + unitName + "'";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSUnit_ID"].ToString();
            }
            catch (Exception)
            {
                return "";
                //   throw;
            }
        }
        /// <summary>
        /// translate commodity name into id
        /// </summary>
        /// <returns>commodity id</returns>
        private string getGroupID()
        {
            string groupName = comboBox_Group.Text;

            if (groups.Contains(groupName))
            {
                string sql = "select [ERSGroup_ID] from " + schemaName + "[ERSGroup_LU] where [ERSGroup_Desc] = '" + groupName + "'";
                DataTable dt = GetData(sql);
                if (null != dt && dt.Rows.Count != 0)
                {
                    DataRow[] dr = dt.Select();
                    return dr[0]["ERSGroup_ID"].ToString();
                }
                else
                    return "-1";
            }
            else
                return "-1";

        }
        /// <summary>
        /// translate commodity name into id
        /// </summary>
        /// <returns>commodity id</returns>
        private string getCommodityID()
        {
            string commodityName = comboBox_commodity.Text;

            if (commodities.Contains(commodityName))
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
            else
                return "-1";

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
                return "";
                // throw;
            }
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
        /// before upload anything
        /// check if any field is empty or invalid
        /// </summary>
        private bool validateNoEmpty()
        {
            if (comboBox_commodity.Text.Equals(""))
            {
                MessageBox.Show("Please select a commodity");
                return false;
            }

            if (comboBox_phyAttr.Text.Equals(""))
            {
                MessageBox.Show("Please select a physical attribute");
                return false;
            }

            return true;
        }


        /// <summary>
        /// get the dataseries ID if exist
        /// based on comboBox selections
        /// if not exist
        /// create one
        /// and return the dataseries ID
        /// </summary>
        /// <param name="commoditySubCommodityID"> commodity id </param>
        /// <param name="physicalAttributeTypeID">physical attribute type </param>
        /// <param name="physicalAttributeDetail">physical attribute detail </param>
        private string getDataSeriesID(string groupID, string commoditySubCommodityID, string physicalAttributeDetail, string prodPractice, string utilPractice, string statType, bool insertNew, string dataseriesID)
        {
            if (!groupID.Equals("") && !groupID.Equals("-1"))
                groupID = " and CDS.ERSCommodity_ERSGroup_ID = " + groupID;
            if (!commoditySubCommodityID.Equals("") && !commoditySubCommodityID.Equals("-1"))
                commoditySubCommodityID = " and CDS.ERSCommoditySubCommodity_ID = " + commoditySubCommodityID;
            if (!physicalAttributeDetail.Equals("") && !physicalAttributeDetail.Equals("-1"))
                physicalAttributeDetail = " and CDS.ERSCommodity_PhysicalAttribute_Desc like '%" + physicalAttributeDetail + "%'";
            if (!prodPractice.Equals("") && !prodPractice.Equals("-1"))
                prodPractice = " and CDS.ERSCommodity_ERSProdPractice_ID = " + prodPractice;
            if (!utilPractice.Equals("") && !utilPractice.Equals("-1"))
                utilPractice = " and CDS.ERSCommodity_ERSUtilPractice_ID = " + utilPractice;
            if (!statType.Equals("") && !statType.Equals("-1"))
                statType = " and CDS.[ERSCommodity_ERSStatisticType_ID] = " + statType;
            if (!dataseriesID.Equals("") && !dataseriesID.Equals("-1"))
                dataseriesID = " and CDS.[ERSCommodity_ID] = " + dataseriesID;

            string sql = "select [ERSCommodity_ID] AS 'Data Series ID',[ERSCommoditySubCommodity_Desc] AS 'Commodity',[ERSCommodity_PhysicalAttribute_Desc] AS 'Physical Attribute',[ERSStatisticType_Attribute] 'Stat Type',[ERSProdPractice_Desc] AS 'Prod Practice',[ERSUtilPractice_Desc] AS 'Util Practice',[ERSCommodity_SourceSeriesID] AS 'SourceSeriesID',[ERSSource_Desc] AS 'Source Desc',[ERSSource_LongDesc] 'Source Long Desc' from " + schemaName + "ERSCommodityDataSeries CDS, " + schemaName + "[ERSCommoditySubCommodity_LU], " + schemaName + "ERSStatisticType_LU, " + schemaName + "ERSProdPractice_LU, " + schemaName + "ERSUtilPractice_LU, " + schemaName + "ERSSource_LU where 1=1 " +
            groupID +
            commoditySubCommodityID +
            physicalAttributeDetail +
            prodPractice +
            utilPractice +
            statType +
            dataseriesID +
            " and CDS.[ERSCommoditySubCommodity_ID] = " + schemaName + "[ERSCommoditySubCommodity_LU].[ERSCommoditySubCommodity_ID]" +
            " and CDS.[ERSCommodity_ERSStatisticType_ID] = " + schemaName + "[ERSStatisticType_LU].[ERSStatisticType_ID]" +
            " and CDS.[ERSCommodity_ERSProdPractice_ID] = " + schemaName + "[ERSProdPractice_LU].[ERSProdPractice_ID]" +
            " and CDS.[ERSCommodity_ERSUtilPractice_ID] = " + schemaName + "[ERSUtilPractice_LU].[ERSUtilPractice_ID]" +
            " and CDS.[ERSCommodity_ERSSource_ID] = " + schemaName + "[ERSSource_LU].[ERSSource_ID]" +
            " Order By [ERSCommodity_ID]"
            ;

            return sql;

        }


        private string getDataValues(string groupID, string commoditySubCommodityID, string physicalAttributeDetail, string prodPractice, string utilPractice, string statType, bool insertNew, string dataseriesID)
        {
            if (!groupID.Equals("") && !groupID.Equals("-1"))
                groupID = " and CDS.ERSCommodity_ERSGroup_ID = " + groupID;
            if (!commoditySubCommodityID.Equals("") && !commoditySubCommodityID.Equals("-1"))
                commoditySubCommodityID = " and CDS.ERSCommoditySubCommodity_ID = " + commoditySubCommodityID;
            if (!physicalAttributeDetail.Equals("") && !physicalAttributeDetail.Equals("-1"))
                physicalAttributeDetail = " and CDS.ERSCommodity_PhysicalAttribute_Desc like '%" + physicalAttributeDetail + "%'";
            if (!prodPractice.Equals("") && !prodPractice.Equals("-1"))
                prodPractice = " and CDS.ERSCommodity_ERSProdPractice_ID = " + prodPractice;
            if (!utilPractice.Equals("") && !utilPractice.Equals("-1"))
                utilPractice = " and CDS.ERSCommodity_ERSUtilPractice_ID = " + utilPractice;
            if (!statType.Equals("") && !statType.Equals("-1"))
                statType = " and CDS.[ERSCommodity_ERSStatisticType_ID] = " + statType;
            if (!dataseriesID.Equals("") && !dataseriesID.Equals("-1"))
                dataseriesID = " and CDS.[ERSCommodity_ID] = " + dataseriesID;

            string dssql = "select [ERSCommodity_ID] AS 'Data Series ID',[ERSCommoditySubCommodity_Desc] AS 'Commodity',[ERSCommodity_PhysicalAttribute_Desc] AS 'Physical Attribute',[ERSStatisticType_Attribute] 'Stat Type',[ERSProdPractice_Desc] AS 'Prod Practice',[ERSUtilPractice_Desc] AS 'Util Practice',[ERSCommodity_SourceSeriesID] AS 'SourceSeriesID',[ERSSource_Desc] AS 'Source Desc',[ERSSource_LongDesc] 'Source Long Desc' from " + schemaName + "ERSCommodityDataSeries CDS, " + schemaName + "[ERSCommoditySubCommodity_LU], " + schemaName + "ERSStatisticType_LU, " + schemaName + "ERSProdPractice_LU, " + schemaName + "ERSUtilPractice_LU, " + schemaName + "ERSSource_LU where 1=1 " +
                groupID +
                commoditySubCommodityID +
                physicalAttributeDetail +
                prodPractice +
                utilPractice +
                statType +
                dataseriesID +
                " and CDS.[ERSCommoditySubCommodity_ID] = " + schemaName + "[ERSCommoditySubCommodity_LU].[ERSCommoditySubCommodity_ID]" +
                " and CDS.[ERSCommodity_ERSStatisticType_ID] = " + schemaName + "[ERSStatisticType_LU].[ERSStatisticType_ID]" +
                " and CDS.[ERSCommodity_ERSProdPractice_ID] = " + schemaName + "[ERSProdPractice_LU].[ERSProdPractice_ID]" +
                " and CDS.[ERSCommodity_ERSUtilPractice_ID] = " + schemaName + "[ERSUtilPractice_LU].[ERSUtilPractice_ID]" +
                " and CDS.[ERSCommodity_ERSSource_ID] = " + schemaName + "[ERSSource_LU].[ERSSource_ID]" +
                " Order By [ERSCommodity_ID]"
                ;

            connection();
            con.Open();
            dataAdapterForPaging = new SqlDataAdapter(dssql, con);
            dataAdapterForPaging.SelectCommand.ExecuteNonQuery();
            DTDataSeries = new DataTable();
            dataAdapterForPaging.Fill(DTDataSeries);

            if (DTDataSeries.Rows.Count == 0)
            {
                new StringBuilder("-1");
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

            if (textBox_DSid.Text != "" && dataSeriesIDs == null)
            {
                dataSeriesIDs.Append("0");
            }



            string sql = "select " +
        "distinct " +
        "ERSCommodity_ID as Data_Series_ID, " +
        "ERSDataValues_ID as CoSD_ID,  " +
        "ERSDataValues_AttributeValue as Value, " +
        "ERSUnit_LU.ERSUnit_Desc as Unit,  " +
        "ERSDataLifecyclePhase_Desc as  LifeCyclePhase,  " +
        "ERSSource_Desc as Source, " +
        "ERSTimeDimensionType_Desc as TimeType, " +
        // "(SELECT CONVERT(varchar(20), ERSTimeDimension_Date,101)) as Date, " +
        "ERSTimeDimension_Date as Date, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_State as State, " +
        "ERSGeographyDimension_LU.ERSGeographyDimension_Region as Region " +
        "from  " +
        schemaName + "ERSDataValues,  " +
        schemaName + "ERSCommodityDataSeries,  " +
        schemaName + "ERSUnit_LU, " +
        schemaName + "ERSGeographyDimension_LU, " +
        schemaName + "ERSTimeDimension_LU, " +
        schemaName + "ERSTimeDimensionType_LU,  " +
        schemaName + "ERSDataLifecycle_LU,  " +
        schemaName + "ERSSource_LU " +
        " where " +
     " ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs + ")" +
      " and " + schemaName + "ERSCommodityDataSeries.ERSCommodity_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSCommodity_ID" +
      " and " + schemaName + "ERSUnit_LU.ERSUnit_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSUnit_ID" +
      " and " + schemaName + "ERSDataLifecycle_LU.ERSDataLifecyclePhase_ID = " + schemaName + "ERSDataValues.ERSDataValues_DataRowLifecyclePhaseID" +
      " and " + schemaName + "ERSSource_LU.ERSSource_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSSource_ID" +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSGeography_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_ID = " + schemaName + "ERSDataValues.ERSDataValues_ERSTimeDimension_ID" +
      " and " + schemaName + "ERSTimeDimension_LU.ERSTimeDimension_TimeDimensionType_ID = " + schemaName + "ERSTimeDimensionType_LU.ERSTimeDimensionType_ID"
      + " order by Date asc ";


            return sql;

        }

        // send email for new data series function. Uses the windows (USDA) login username to send emails through outlook 
        private void button_sendEmail_Click(object sender, EventArgs e)
        {

            // verification before everything
            if (textBox_CommodityName.Text.Equals("") && textBox_PhyAttr.Text.Equals("")
                && textBox_Unit.Text.Equals("") && textBox_other.Text.Equals("") && textBox_other2.Text.Equals(""))
            {
                MessageBox.Show("Please fill at least one text box for requested information.");
                return;
            }
            try
            {
                // get current user            
                System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                //  Console.WriteLine(currentUser.Name);
                // load outlook
                Microsoft.Office.Interop.Outlook.Application outlookApp = new Microsoft.Office.Interop.Outlook.Application();
                // create a new email
                Microsoft.Office.Interop.Outlook.MailItem mailItem = (Microsoft.Office.Interop.Outlook.MailItem)outlookApp.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);
                // set subject
                mailItem.Subject = "[AP tool] New Variable Request";

                //  mailItem.To = textBox_email.Text;
                mailItem.Recipients.Add(textBox_email.Text);
                if (!waterMarkTextBox.Text.Equals(""))
                    mailItem.Recipients.Add(waterMarkTextBox.Text);

                // set email content
                mailItem.Body = "Dear Data Coordinator," + "\n\n" +
                    "The user \"" + currentUser.Name + "\"" + " has requested the following variables: \n\n" +
                    "Request information: \n" +
                    "Commodity: " + textBox_CommodityName.Text + "\n" +
                    "Physical Attribute: " + textBox_PhyAttr.Text + "\n" +
                    "Unit: " + textBox_Unit.Text + "\n" +
                    "Utilization Practice: " + textBox_other.Text + "\n" +
                    "Production Practice: " + textBox_other2.Text + "\n" +
                    "Requestor Comments: \n\"" + textBox_comments.Text + "\"\n" + "\n" +
                    "Requestor: " + currentUser.Name + "\n" + "\n" + "Thank you!";


                // email sending scope

                mailItem.Send();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not recognize"))
                    MessageBox.Show("Unable to send request. Please make sure the email address is correct.");
                else
                    MessageBox.Show("Unable to send request. Please make sure the Outlook is open and Logged-In.");
                return;
            }

            MessageBox.Show("Thank you for your request. Your request has been sent to the coordinators. The coordinators will review the request and get back to you asap!");

            // clear this section after send
            textBox_CommodityName.Text = "";
            textBox_PhyAttr.Text = "";
            textBox_Unit.Text = "";
            textBox_other.Text = "";
            textBox_other2.Text = "";
            textBox_comments.Text = "";

        }

        // record user action
        private void saveAction(string action)
        {
            // get current user and inserts the user log into the log table           
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
        /// button goes back to home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_back_Click(object sender, EventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Close();
            this.Parent = null;
        }

        // math that is used in the validation window

        /// <summary>
        /// calculate the sum of a given array
        /// </summary>
        /// <param name="values">an array of values</param>
        /// <returns></returns>
        public decimal Sum(decimal[] values)
        {
            decimal result = 0;

            for (int i = 0; i < values.Length; i++)
            {
                result += values[i];
            }

            return result;
        }
        /// <summary>
        /// calculate the average of a given array
        /// </summary>
        /// <param name="values">an array of values</param>
        /// <returns></returns>
        public decimal Average(decimal[] values)
        {
            decimal sum = Sum(values);
            decimal result = (decimal)sum / values.Length;
            return result;
        }
        /// <summary>
        /// calculate the median of a given array
        /// </summary>
        /// <param name="values">an array of values</param>
        /// <returns></returns>
        public double Median(double[] xs)
        {
            Array.Sort(xs);
            return xs[xs.Length / 2];
        }
        /// <summary>
        /// calculate the standard deviation of a given array
        /// </summary>
        /// <param name="values">an array of values</param>
        /// <returns></returns>
        private double getStandardDeviation(double[] doubleList)
        {
            double average = doubleList.Average();
            double sumOfSquaresOfDifferences = doubleList.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / doubleList.Length);
            return sd;
        }

        /// <summary>
        /// Validating the value of WEIGHT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_validation2_Click(object sender, EventArgs e)
        {
            // update for adding unit of  weight
            if (comboBox_unit.Text.Equals(""))
            {
                MessageBox.Show("Please select a Unit");
                return;
            }

            waitForm.Show(this);
            waitForm.Refresh();
            validatingData();
            waitForm.Hide();
            waitForm.Refresh();

        }

        /// <summary>
        /// Data validating function
        /// do statistical validation based on user inputs
        /// will display results
        /// </summary>
        /// <param name="commoditySubCommodityID">commodity id</param>
        /// <param name="physicalAttributeTypeID">physical attribute category</param>
        /// <param name="physicalAttributeDetail">physical attribute details</param>
        /// <param name="statTypeID">stat type id</param>
        private void validatingData()
        {
            string unitID = getUnitID(comboBox_unit.Text);

            StringBuilder dataSeriesIDs = new StringBuilder();
            DataRow lastRow = DT.Rows[DT.Rows.Count - 1];
            string lastItem = lastRow[0].ToString();
            foreach (DataRow dr in DT.Rows)
            {
                //dataSeriesID.Add(dr[0].ToString());
                //if (dr[0].ToString() != lastItem)
                dataSeriesIDs.Append(dr[0].ToString() + ",");
                // else dataSeriesIDs.Append(dr[0].ToString());
            }

            dataSeriesIDs.Length--;


            // search value 1
            string verifySql1 = "select ERSDataValues_AttributeValue as results " +
            "FROM " + schemaName + "ERSDataValues " +
            "where ERSDataValues_ERSCommodity_ID IN (" + dataSeriesIDs +
            ") and ERSDataValues_ERSUnit_ID = " + unitID;

            DataTable verifyResult1 = GetData(verifySql1);
            if (verifyResult1 == null)
                return;

            if (verifyResult1.Rows.Count == 0)
            {
                MessageBox.Show("No similar records with the selected Unit");
                return;
            }

            DataRow[] rows = verifyResult1.Select();
            // save results to an array
            double[] results = new double[verifyResult1.Rows.Count];
            // assign values
            for (int i = 0; i < verifyResult1.Rows.Count; i++)
            {
                if ((rows[i]["results"]).ToString() != "")
                    results[i] = Double.Parse((rows[i]["results"]).ToString());
            }

            // set result messages
            double avg = results.Average();
            double median = Median(results);
            double max = results.Max();
            double min = results.Min();
            double mode = results.GroupBy(n => n).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault();
            double sd = getStandardDeviation(results);
            string sdStr = sd.ToString("0.0000");

            string cardinality = getCardinality(results);

            //////////////////////////////////////////////////////////////////////////
            showChart(min.ToString(), max.ToString(), avg.ToString(), median.ToString(), mode.ToString(), sdStr, cardinality, results); // creates the chart for the validation window


        }
        /// <summary>
        /// method used to show chart of data validation and diagram
        /// </summary>
        /// <param name="min">min value</param>
        /// <param name="max">max value</param>
        /// <param name="avg">avg value</param>
        /// <param name="median">median value</param>
        /// <param name="mode">mode value</param>
        /// <param name="std">standard deviation</param>
        /// <param name="cardinality">cardinality level</param>
        /// <param name="results">results</param>
        private void showChart(string min, string max, string avg, string median, string mode, string std, string cardinality, double[] results)
        {
            Chart chart = new Chart();
            ///////////////////////////////////

            chart.label_range.Text = min + " - " + max;
            chart.label_average.Text = avg;
            chart.label_median.Text = median;
            chart.label_mode.Text = mode;
            chart.label_std.Text = std;
            chart.label_cardinality.Text = cardinality;


            chart.results = results;
            //chart.message = message;
            chart.Show();
        }

        /// <summary>
        /// calculate cardinality of a given array
        /// </summary>
        /// <param name="results">an array of values</param>
        /// <returns>return cardinality level</returns>
        private string getCardinality(double[] results)
        {
            string result = "";

            // remove 


            double[] resultsNoDup = results.Distinct().ToArray();   // stores how many distinct values
                                                                    //      Array.Sort(resultsNoDup);

            int[] occuranceOfArray = new int[resultsNoDup.Length];

            // Define the tolerance for variation in their values 
            double difference = .0001;

            // calculate occurance of each element
            int occurance = 0;
            for (int i = 0; i < resultsNoDup.Length; i++)
            {
                occurance = 0;
                foreach (double j in results)       // calculate occurance of original array
                {
                    if (Math.Abs(resultsNoDup[i] - j) <= difference)
                        occurance++;

                }
                occuranceOfArray[i] = occurance;
            }

            // calculate cardinality
            double cardinality = (double)resultsNoDup.Length / (double)results.Length;
            /// for cardinality 0.66-1: HIGH cardinality, contains very unique, few duplicate values
            /// An example of a data table column with high-cardinality would be a USERS table with a column named USER_ID
            /// 0.33-0.66: NORMAL cardinality
            /// values that are somewhat uncommon
            /// 0-0.33: LOW cardinality
            /// very few unique values
            /// 

            // build result message
            if (cardinality > 0.66)
                result = "HIGH";
            else if (cardinality > 0.33)
                result = "NORMAL";
            else
                result = "LOW";

            result += "\n";
            result += "Top three common values with occurrence:\n";

            // gte top three
            int[] occuranceOfArraySorted = (int[])occuranceOfArray.Clone();
            Array.Sort(occuranceOfArraySorted);
            int largestOccurNum = occuranceOfArraySorted[occuranceOfArraySorted.Length - 1];
            int secondLargestOccurNum;
            if (occuranceOfArraySorted.Length >= 2)
                secondLargestOccurNum = occuranceOfArraySorted[occuranceOfArraySorted.Length - 2];
            else
                secondLargestOccurNum = -1;  // -1 means no such number
            int thirdLargestOccurNum;
            if (occuranceOfArraySorted.Length >= 3)
                thirdLargestOccurNum = occuranceOfArraySorted[occuranceOfArraySorted.Length - 3];
            else
                thirdLargestOccurNum = -1; // -1 means no such number
            // find their indexes
            int topIndex = -1;
            int secondIndex = -1;
            int thirdIndex = -1;
            for (int i = 0; i < occuranceOfArray.Length; i++)
            {
                if (occuranceOfArray[i] == largestOccurNum)
                    topIndex = i;
            }
            if (secondLargestOccurNum != -1)
            {
                for (int i = 0; i < occuranceOfArray.Length; i++)
                {
                    if (occuranceOfArray[i] == secondLargestOccurNum && i != topIndex)
                        secondIndex = i;
                }
            }
            if (thirdLargestOccurNum != -1)
            {
                for (int i = 0; i < occuranceOfArray.Length; i++)
                {
                    if (occuranceOfArray[i] == thirdLargestOccurNum && i != topIndex && i != secondIndex)
                        thirdIndex = i;
                }
            }

            // continue to build msg
            result += "  Value: " + resultsNoDup[topIndex] + "      Occurrence: " + largestOccurNum + "\n";
            if (secondIndex != -1)
                result += "  Value: " + resultsNoDup[secondIndex] + "      Occurrence: " + secondLargestOccurNum + "\n";
            else
                result += "  Value: n/a      Occurrence: n/a \n";
            if (thirdIndex != -1)
                result += "  Value: " + resultsNoDup[thirdIndex] + "      Occurrence: " + thirdLargestOccurNum + "\n";
            else
                result += "  Value: n/a      Occurrence: n/a \n";
            return result;

        }

     

        private void button1_Click(object sender, EventArgs e)
        {

            currentPageIndex = 0;
            label_pageNum.Text = "Page";
            string groupID = "";
            string commoditySubCommodityID = "";
            string physicalAttributeDetail = "";
            string prodPracticeID = "";
            string utilPracticeID = "";
            string statID = "";
            string dataseriesID = "";
            groupBox_ViewDataSeries.Text = "View Data Series";
            labelDataSeriesResult.Text = "";

            // verify input before doing anything
            if (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("") && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals(""))
            {
                MessageBox.Show("Please select at least one value");
                return;
            }

            //Clearing the fields    
            label23.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            groupBox_Request.Visible = false;
            button_sendEmail.Visible = false;
            // second step find data series

            // load variable used from gui
            if (!comboBox_Group.Text.Equals(""))
                groupID = getGroupID();
            if (!comboBox_commodity.Text.Equals(""))
                commoditySubCommodityID = getCommodityID();
            //int physicalAttributeTypeID = int.Parse(getPhyAttrCatID());
            if (!comboBox_phyAttr.Text.Equals(""))
                physicalAttributeDetail = comboBox_phyAttr.Text;
            if (!comboBox_prodPrac.Text.Equals(""))
                prodPracticeID = getProdPracID(comboBox_prodPrac.Text);
            if (!comboBox_utilPrac.Text.Equals(""))
                utilPracticeID = getUtilPracID(comboBox_utilPrac.Text);
            if (!comboBox_statType.Text.Equals(""))
                statID = getStatTypeID();
            if (!textBox_DSid.Text.Equals(""))
                dataseriesID = textBox_DSid.Text;


            try
            {  // build a sql statement for searching

                waitForm.Show(this);
                waitForm.Refresh();

                dataSeriesSQL = getDataSeriesID(groupID, commoditySubCommodityID, physicalAttributeDetail, prodPracticeID, utilPracticeID, statID, insertNewDataSeries, dataseriesID);
                if (dataSeriesSQL.Equals(""))
                    return;
                connection();
                con.Open();
                dataAdapterForPaging = new SqlDataAdapter(dataSeriesSQL, con);
                dataAdapterForPaging.SelectCommand.ExecuteNonQuery();
                DT = new DataTable();
                dataAdapterForPaging.Fill(DT);
                dataGridView.DataSource = DT;
                dataGridView.ReadOnly = true;
             

                if (DT.Rows.Count != 0)
                {
                    labelDataSeriesResult.Text = "Data Series are Available";

                    labelDataSeriesResult.ForeColor = System.Drawing.Color.DarkGreen;
                    labelDataSeriesResult.Visible = true;
                    label_request.Visible = false;
                    groupBox_summary.Visible = true;


                    groupBox_ViewDataSeries.Visible = true;
                    label23.Visible = false;
                    label24.Visible = false;
                    label25.Visible = false;
                    groupBox_Request.Visible = false;
                    button_sendEmail.Visible = false;
                    //fill Unit combo
                    fillUnit();
                }
                else
                {

                    labelDataSeriesResult.Text = "Data Series are Not Available";
                    labelDataSeriesResult.ForeColor = System.Drawing.Color.Red;
                    labelDataSeriesResult.Visible = true;
                    groupBox_summary.Visible = false;
                    label_request.Visible = true;


                }

                //BindGrid();
                /*disableing next button*/
                if ((currentPageIndex + 1) == (int)Math.Ceiling(Convert.ToDecimal(DT.Rows.Count) / pageSize))
                {
                    button_next.Enabled = false;

                }
                else if (Convert.ToDecimal(DT.Rows.Count) <= pageSize)
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
            catch (Exception ex)
            {

                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }

            finally
            {
                waitForm.Hide();
                waitForm.Refresh();
                con.Close();


            }


          


        }

       

        private List<string> pushMetaData()
        {
            connection();
            con.Open();
            dataAdapterForPaging = new SqlDataAdapter(dataSeriesSQL, con);
            dataAdapterForPaging.SelectCommand.ExecuteNonQuery();
            DT = new DataTable();
            dataAdapterForPaging.Fill(DT);
            dataGridView.DataSource = DT;
            dataGridView.ReadOnly = true;
            //Stroing dataseries id in list for further use
            dataSeriesID = new List<string>();
            foreach (DataRow dr in DT.Rows)
            {
                dataSeriesID.Add(dr[0].ToString());
            }
            return dataSeriesID;
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

        private void labelDataSeriesResult_Click(object sender, EventArgs e)
        {

        }

        private void button_ViewDataSeries_Click(object sender, EventArgs e)
        {

        }

        private void button_next_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor currentCursor = this.Cursor;
                this.Cursor = Cursors.WaitCursor;
                //if (currentPageIndex == (int)Math.Ceiling(Convert.ToDecimal(DT.Rows.Count) / pageSize))
                //{
                //    button_next.Enabled = false;
                //    return;

                //}

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
            }
        }

        private void button_next_EnabledChanged(object sender, EventArgs e)
        {
            if (((Button)sender).Enabled)
            {
                button_next.Text = "Next";
                button_next.ForeColor = Color.Black;
                button_next.BackColor = Color.Linen;
            }
            else
            {
                button_next.Text = "Next";
                button_next.ForeColor = Color.DimGray;
                button_next.BackColor = Color.Gray;
            }
        }

        private void button_prev_EnabledChanged(object sender, EventArgs e)
        {
            if (((Button)sender).Enabled)
            {
                button_prev.Text = "Prev";
                button_prev.ForeColor = Color.Black;
                button_prev.BackColor = Color.Linen;
            }
            else
            {
                button_prev.Text = "Prev";
                button_prev.ForeColor = Color.DimGray;
                button_prev.BackColor = Color.Gray;
            }
        }

        private void button_RequestDataSeries_Click(object sender, EventArgs e)
        {
            //making the fields visible

            groupBox_ViewDataSeries.Visible = false;

            label23.Visible = true;
            label24.Visible = true;
            label25.Visible = true;
            groupBox_Request.Visible = true;
            button_sendEmail.Visible = true;

        }


        private void RequestNew_Click(object sender, EventArgs e)
        {
            //making the fields visible

            groupBox_ViewDataSeries.Visible = false;

            label23.Visible = true;
            label24.Visible = true;
            label25.Visible = true;
            groupBox_Request.Visible = true;
            button_sendEmail.Visible = true;
            groupBox_summary.Visible = false;
        }

        private void label_request_Click(object sender, EventArgs e)
        {

        }

        private void groupBox_Request_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox_ViewDataSeries_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void uploadGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void button_viewdata_Click(object sender, EventArgs e)
        {
            currentPageIndex = 0;
            label_pageNum.Text = "Page";
            string groupID = "";
            string commoditySubCommodityID = "";
            string physicalAttributeDetail = "";
            string prodPracticeID = "";
            string utilPracticeID = "";
            string statID = "";
            string dataseriesID = "";
            groupBox_ViewDataSeries.Text = "View Data Values";
            // verify input before doing anything
            //if (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("") 
            //    && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals(""))
            //{
            //    MessageBox.Show("Please select at least one value");
            //    return;
            //}

            if (
                (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (!comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && !comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && !comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && !comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && !comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && !comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||

                 (!comboBox_Group.Text.Equals("") && !comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (!comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && !comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (!comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && !comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (!comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && !comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (!comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && !comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||

                (comboBox_Group.Text.Equals("") && !comboBox_commodity.Text.Equals("") && !comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && !comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && !comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && !comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && !comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && !comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && !comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||

                (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && !comboBox_phyAttr.Text.Equals("") && !comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && !comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && !comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && !comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && !comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||

                 (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && !comboBox_prodPrac.Text.Equals("")
                && !comboBox_utilPrac.Text.Equals("") && comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||
                (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && !comboBox_prodPrac.Text.Equals("")
                && comboBox_utilPrac.Text.Equals("") && !comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals("")) ||

                 (comboBox_Group.Text.Equals("") && comboBox_commodity.Text.Equals("") && comboBox_phyAttr.Text.Equals("") && comboBox_prodPrac.Text.Equals("")
                && !comboBox_utilPrac.Text.Equals("") && !comboBox_statType.Text.Equals("") && textBox_DSid.Text.Equals(""))

                )
            {
                MessageBox.Show("Please select atleast 3 values.\n\rOR\n\rEnter data series id.");
                return;
            }



            //Clearing the fields    
            label23.Visible = false;
            label24.Visible = false;
            label25.Visible = false;
            groupBox_Request.Visible = false;
            button_sendEmail.Visible = false;
            // second step find data series

            // load variable used from gui
            if (!comboBox_Group.Text.Equals(""))
                groupID = getGroupID();
            if (!comboBox_commodity.Text.Equals(""))
                commoditySubCommodityID = getCommodityID();
            //int physicalAttributeTypeID = int.Parse(getPhyAttrCatID());
            if (!comboBox_phyAttr.Text.Equals(""))
                physicalAttributeDetail = comboBox_phyAttr.Text;
            if (!comboBox_prodPrac.Text.Equals(""))
                prodPracticeID = getProdPracID(comboBox_prodPrac.Text);
            if (!comboBox_utilPrac.Text.Equals(""))
                utilPracticeID = getUtilPracID(comboBox_utilPrac.Text);
            if (!comboBox_statType.Text.Equals(""))
                statID = getStatTypeID();
            if (!textBox_DSid.Text.Equals(""))
                dataseriesID = textBox_DSid.Text;

            try
            {  // build a sql statement for searching

                waitForm.Show(this);
                waitForm.Refresh();

                dataSeriesSQL = getDataValues(groupID, commoditySubCommodityID, physicalAttributeDetail, prodPracticeID, utilPracticeID, statID, insertNewDataSeries, dataseriesID);
                if (dataSeriesSQL.Equals(""))
                {
                    dataGridView.Rows.Clear();
                    dataGridView.Refresh();
                    return;
                }
                connection();
                con.Open();
                dataAdapterForPaging = new SqlDataAdapter(dataSeriesSQL, con);
                dataAdapterForPaging.SelectCommand.ExecuteNonQuery();
                DT = new DataTable();
                dataAdapterForPaging.Fill(DT);
                dataGridView.DataSource = DT;
                dataGridView.ReadOnly = true;


                if (DT.Rows.Count != 0)
                {
                    labelDataSeriesResult.Text = "Data Values are Available";

                    labelDataSeriesResult.ForeColor = System.Drawing.Color.DarkGreen;
                    labelDataSeriesResult.Visible = true;
                    label_request.Visible = false;
                    groupBox_summary.Visible = true;


                    groupBox_ViewDataSeries.Visible = true;
                    label23.Visible = false;
                    label24.Visible = false;
                    label25.Visible = false;
                    groupBox_Request.Visible = false;
                    button_sendEmail.Visible = false;
                    //fill Unit combo
                    fillUnit();
                }
                else
                {

                    labelDataSeriesResult.Text = "Data Values are Not Available";
                    labelDataSeriesResult.ForeColor = System.Drawing.Color.Red;
                    labelDataSeriesResult.Visible = true;
                    groupBox_summary.Visible = false;
                    label_request.Visible = true;


                }

                //BindGrid();
                /*disableing next button*/
                if ((currentPageIndex + 1) == (int)Math.Ceiling(Convert.ToDecimal(DT.Rows.Count) / pageSize))
                {
                    button_next.Enabled = false;

                }
                else if (Convert.ToDecimal(DT.Rows.Count) <= pageSize)
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
            catch (Exception ex)
            {
                dataGridView.DataSource = null;
                //MessageBox.Show("Please Check Data Series.");
                labelDataSeriesResult.Text = "Data Values are Not Available";
                labelDataSeriesResult.ForeColor = System.Drawing.Color.Red;
                labelDataSeriesResult.Visible = true;
                groupBox_summary.Visible = false;
                label_request.Visible = true;
            }

            finally
            {
                waitForm.Hide();
                waitForm.Refresh();
                con.Close();


            }


        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox_DSid_TextChanged(object sender, EventArgs e)
        {
            Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            // check if it's a number
            if (!textBox_DSid.Text.Equals("") && !numberchk.IsMatch(textBox_DSid.Text))
            {
                errorProvider1.SetError(textBox_DSid, "Invalid input!\nPlease type numeric characters only.");
                textBox_DSid.BackColor = Color.Red;
                return;
            }
            else if (!textBox_DSid.Text.Equals("") && numberchk.IsMatch(textBox_DSid.Text))
            {
                errorProvider1.SetError(textBox_DSid, "");
                textBox_DSid.BackColor = Color.White;

            }
            else
            {
                errorProvider1.SetError(textBox_DSid, "");
                textBox_DSid.BackColor = Color.White;
            }

        }

        private void textBox_email_TextChanged(object sender, EventArgs e)
        {

        }
    }
}