// ***********************************************************************
// Project          : Combined CoSD Tool
// Author           : Bharat Radhakrishnan, George Mason University
// Created          : 04-25-2019
// Version          : 1.0
// Update history   : Please refer to backup folder on MTEDs shared drive
// ***********************************************************************
// <copyright file="ManageRepoForm.cs" company="USDA, ERS, GMU">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary>
//In this page, the analysts setup the privacy flags per data series, or per data commodity, or by stat type, geo or time. Then they can push the public data from the CoSD database to the Repository.
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoSD_Tool
{
    public partial class PrivacyForm : Form
    {
        string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        private SqlConnection con;    // db connection
        private string sqlconn;  // query and sql connection
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private BindingSource bindingSource_country = new BindingSource();
        private DataTable finalResult = new DataTable();
        private DataTable statResult = new DataTable();
        private List<string> countryList;
        private List<string> commodities;
        private List<string> statTypes;
        private List<string> source;
        private List<string> unit;
        private List<string> groupIds;
        WaitForm waitForm = new WaitForm();
        BackgroundWorker backgroundWorker;

        //DataTable dt = new DataTable();
        /*
1	Public
2	Private
3	Sensitive
4	Confidential
5	Repository
    */
        private int PRIVACY_PUBLIC = 1;
        private int PRIVACY_PRIVATE = 2;
        private int PUBLIC_REPOSITORY = 5;
      

        string apGroupId = LandingScreen.GetGroupId(LandingScreen.newschemaName);

        private DataRow[] finalResultRows;
        private DataTable dataReadyToPush = null;

        public PrivacyForm()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
            //comboBox_country.DataSource = bindingSource_country;

        }

        private void resetFields()
        {
            textBox1_DS.Text = "";
            comboBox_commodity.Text = "";
            comboBox1_unit.Text = "";
            comboBox_source.Text = "";
            comboBox_statType.Text = "";
        }

     


        private void fillStatTypeCombobox()
        {
            statTypes = new List<string>();
            foreach (string groupID in groupIds)
            {
                string sql = "select [ERSStatisticType_Attribute] from " + schemaName + "ERSStatisticType_LU WHERE ERSStatisticType_ID IN (SELECT DISTINCT[ERSCommodity_ERSStatisticType_ID] FROM  " + schemaName + "[ERSCommodityDataSeries]   WHERE [ERSCommodity_ERSGroup_ID] = " + groupID + " ) ";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
               // statTypes.Add("");
                foreach (DataRow row in dr)
                {
                    statTypes.Add(row["ERSStatisticType_Attribute"].ToString());
                }
            }

         
            comboBox_statType.DataSource = statTypes;
            comboBox_statType.SelectedItem = null;
        }

        /// <summary>
        /// load a list of commodity names with given group ids
        /// </summary>
        /// <param name="groupIDs">input group ids as parameter</param>
        private void fillCommodityCombobox()
        {
            commodities = new List<string>();
            foreach (string groupID in groupIds)
            {
                string sql = "select DISTINCT clu.ERSCommoditySubCommodity_Desc from " + schemaName + "ERSCommoditySubCommodity_LU clu"+","+ schemaName+ "ERSCommodityDataSeries cds"+","+ schemaName+ "ERSDataValues dv" +" where ERSCommoditySubCommodity_GroupID = " + groupID +
                    " and cds.ERSCommoditySubCommodity_ID=clu.ERSCommoditySubCommodity_ID"+
                    " and dv.ERSDataValues_ERSCommodity_ID=cds.ERSCommodity_ID";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
               // commodities.Add("");
                foreach (DataRow row in dr)
                {
                    commodities.Add(row["ERSCommoditySubCommodity_Desc"].ToString());
                }
            }

          
            comboBox_commodity.DataSource = commodities;
            comboBox_commodity.SelectedItem = null;


        }

        private void fillSourceCombobox()
        {
            source = new List<string>();
            foreach (string groupID in groupIds)
            {
                string sql = "select DISTINCT ERSSource_Desc from " + schemaName 
                    + "ERSSource_LU where ERSSource_ID IN (SELECT DISTINCT[ERSCommodity_ERSSource_ID] FROM  " + schemaName + "[ERSCommodityDataSeries]   WHERE [ERSCommodity_ERSGroup_ID] = " + groupID + " ) ORDER BY ERSSource_Desc";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
               // source.Add("");
                foreach (DataRow row in dr)
                {
                    source.Add(row["ERSSource_Desc"].ToString());
                }
            }
         
            
            comboBox_source.DataSource = source;
            comboBox_source.SelectedItem = null;

        }

        private void fillUnitCombobox()
        {
            unit = new List<string>();
            foreach (string groupID in groupIds)
            {
                string sql = "select distinct ERSUnit_Desc from " + schemaName
                    + "ERSUnit_LU ulu"+ ","+ schemaName+"ERSCommodityDataSeries cds"+","+ schemaName+"ERSDataValues dv " +
                    "where ulu.ERSUnit_ID=dv.ERSDataValues_ERSUnit_ID " +
                    "and cds.ERSCommodity_ID=dv.ERSDataValues_ERSCommodity_ID   "+
                    "and [ERSCommodity_ERSGroup_ID] = " + groupID;
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                // source.Add("");
                foreach (DataRow row in dr)
                {
                    unit.Add(row["ERSUnit_Desc"].ToString());
                }
            }


            comboBox1_unit.DataSource = unit;
            comboBox1_unit.SelectedItem = null;

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
            // connection string for linking db
           // sqlconn = ConfigurationManager.ConnectionStrings["AP_CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
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
                throw new Exception("Connection Error");
               
            }
        }

        private int getCommodityID(string commodityName)
        {
            if (commodityName != null && commodityName != "")
            {
                string sql = "select ERSCommoditySubCommodity_ID from " + schemaName + "ERSCommoditySubCommodity_LU where ERSCommoditySubCommodity_Desc = '" + commodityName + "'";
                DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return (int)dr[0]["ERSCommoditySubCommodity_ID"];
            }
            else

                return -1;
        }

        private void comboBox_commodity_TextChanged(object sender, EventArgs e)
        {
            ClearCmboboxesOnFirstEmpty(comboBox_commodity.Text);
        }

        private void ClearCmboboxesOnFirstEmpty(string comboValue)
        {
            if (comboValue == "")
            {
                fillCommodityCombobox();
                fillSourceCombobox();
                fillUnitCombobox();
                fillStatTypeCombobox();
            }
        }

        private void comboBox_commodity_SelectedIndexChanged(object sender, EventArgs e)
        {

            //filter stat type
           
                statTypes = new List<string>();
                foreach (string groupID in groupIds)
                {
                    string sql = "select [ERSStatisticType_Attribute] from " + schemaName + "ERSStatisticType_LU " +
                        "WHERE ERSStatisticType_ID IN (SELECT DISTINCT[ERSCommodity_ERSStatisticType_ID] FROM  "
                        + schemaName + "[ERSCommodityDataSeries]   WHERE [ERSCommodity_ERSGroup_ID] = " + groupID
                        + " and ERSCommoditySubCommodity_ID = " + getCommodityID(comboBox_commodity.Text) + " ) ";
                    DataTable dt = GetData(sql);
                    DataRow[] dr = dt.Select();
                    foreach (DataRow row in dr)
                    {
                        statTypes.Add(row["ERSStatisticType_Attribute"].ToString());
                    }
                }


                comboBox_statType.DataSource = statTypes;
                comboBox_statType.SelectedItem = null;

                //filter unit


                //filter source 
                source = new List<string>();
                foreach (string groupID in groupIds)
                {
                    string sql = "select DISTINCT ERSSource_Desc from " + schemaName
                       + "ERSSource_LU where ERSSource_ID IN (SELECT DISTINCT[ERSCommodity_ERSSource_ID] FROM  " + schemaName + "[ERSCommodityDataSeries]   WHERE [ERSCommodity_ERSGroup_ID] = " + groupID +
                        " and ERSCommoditySubCommodity_ID = " + getCommodityID(comboBox_commodity.Text) + " ) ORDER BY ERSSource_Desc";

                    DataTable dt = GetData(sql);
                    DataRow[] dr = dt.Select();
                    foreach (DataRow row in dr)
                    {
                        source.Add(row["ERSSource_Desc"].ToString());
                    }
                }


                comboBox_source.DataSource = source;
                comboBox_source.SelectedItem = null;

                unit = new List<string>();
                foreach (string groupID in groupIds)
                {
                    string sql = "select distinct ERSUnit_Desc from " + schemaName
                        + "ERSUnit_LU ulu" + "," + schemaName + "ERSCommodityDataSeries cds" + "," + schemaName + "ERSDataValues dv " +
                        " where ulu.ERSUnit_ID=dv.ERSDataValues_ERSUnit_ID " +
                        " and cds.ERSCommoditySubCommodity_ID= " + getCommodityID(comboBox_commodity.Text) +
                        " and cds.ERSCommodity_ID=dv.ERSDataValues_ERSCommodity_ID " +
                        " and [ERSCommodity_ERSGroup_ID] = " + groupID;


                    DataTable dt = GetData(sql);
                    DataRow[] dr = dt.Select();
                    foreach (DataRow row in dr)
                    {
                        unit.Add(row["ERSUnit_Desc"].ToString());
                    }
                }


                comboBox1_unit.DataSource = unit;
                comboBox1_unit.SelectedItem = null;
          
            
         
            


        }
        private void ReloadDropDown()
        {
            if(comboBox_commodity.Text.Equals(""))
            {
                comboBox_source.Items.Clear();
                comboBox_statType.Items.Clear();
                comboBox1_unit.Items.Clear();
            }
        }

        /// <summary>
        /// Handles the FormClosing event of the Upload control.
        /// When the "x" button on the top right corner gets clicked, it just hides the window,
        /// instead of disposing it or killing the process. 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>

        private void button_back_Click(object sender, EventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Close();
            this.Parent = null;
        }

        private void comboBox_statType_SelectedIndexChanged(object sender, EventArgs e)
        {


            //try
            //{
            //    // string statTypeID = comboBox_statType.SelectedValue.ToString();
            //    string statTypeID = getStatTypeID();
            //    string filter = "ERSDataValues_ERSStatisticType_ID1 = " + statTypeID + " OR ERSDataValues_ERSStatisticType_ID2 = " + statTypeID + " OR ERSDataValues_ERSStatisticType_ID3 = " + statTypeID;

            //    // calling select will NOT change in datatable
            //    // only return a sub set to rows
            //    // but do NOT modify the datatable here
            //    // cuz ppl may change criteria later to totally different thing
            //    int commodityid = 0;
            //    string commoditysubquery = "";
            //    if (comboBox_commodity.Text != "")
            //    {
            //         commodityid = getCommodityID(comboBox_commodity.Text);
            //         commoditysubquery = " and ERSCommoditySubCommodity_ID = " + commodityid;
            //    }

            //    string statypesubquery = "";
            //    if(comboBox_statType.Text !="")
            //    {
            //        statTypeID = getStatTypeID();
            //        statypesubquery = " and ERSCommodity_ERSStatisticType_ID = " + statTypeID;
            //    }
            //    //filter source 
            //    source = new List<string>();
            //    foreach (string groupID in groupIds)
            //    {
            //        string sql = "select DISTINCT ERSSource_Desc from " + schemaName
            //           + "ERSSource_LU where ERSSource_ID IN (SELECT DISTINCT[ERSCommodity_ERSSource_ID] FROM  " + schemaName + "[ERSCommodityDataSeries]   WHERE [ERSCommodity_ERSGroup_ID] = " + groupID +
            //          commoditysubquery + statypesubquery + ") ORDER BY ERSSource_Desc";

            //        DataTable dt = GetData(sql);
            //        DataRow[] dr = dt.Select();
            //        foreach (DataRow row in dr)
            //        {
            //            source.Add(row["ERSSource_Desc"].ToString());
            //        }
            //    }

         
            //    comboBox_source.DataSource = source;
            //    comboBox_source.SelectedItem = null;
                
            //    //filter unit
            //    unit = new List<string>();
            //    foreach (string groupID in groupIds)
            //    {
            //        string sql = "select distinct ERSUnit_Desc from " + schemaName
            //            + "ERSUnit_LU ulu" + "," + schemaName + "ERSCommodityDataSeries cds" + "," + schemaName + "ERSDataValues dv "+ "," + schemaName + "ERSStatisticType_LU slu" +
            //            " where ulu.ERSUnit_ID=dv.ERSDataValues_ERSUnit_ID " +
            //            "and cds.ERSCommoditySubCommodity_ID= " + getCommodityID(comboBox_commodity.Text) +
            //            " and cds.ERSCommodity_ID=dv.ERSDataValues_ERSCommodity_ID " +
            //            " and slu.ERSStatisticType_ID=" + statTypeID+
            //            " and [ERSCommodity_ERSGroup_ID] = " + groupID;
            //        DataTable dt = GetData(sql);
            //        DataRow[] dr = dt.Select();
            //        foreach (DataRow row in dr)
            //        {
            //            unit.Add(row["ERSUnit_Desc"].ToString());
            //        }
            //    }
            //    comboBox1_unit.DataSource = unit;
            //    comboBox1_unit.SelectedItem = null;


            //    finalResultRows = finalResult.Select(filter);

            //    //         Console.WriteLine("after "+finalResult.Rows.Count);

            //    //     finalResult = finalResultRows.CopyToDataTable();
            //    label_result.Text = finalResultRows.Length + "  Rows";
            //}
            //catch (Exception)
            //{
            //    return;
            //}
        }

        /// <summary>
        /// translate stat type name into id
        /// </summary>
        /// <returns></returns>
        private string getStatTypeID()
        {
            string sql = "select ERSStatisticType_ID from " + schemaName + "ERSStatisticType_LU where ERSStatisticType_Attribute = '" + comboBox_statType.Text + "'";
            DataTable dt = GetData(sql);
            if (dt.Rows.Count == 0)
            {
                return "-1";
            }
            DataRow[] dr = dt.Select();           
            return dr[0]["ERSStatisticType_ID"].ToString();
        }

        private string getSourceID()
        {
            string sql = "select ERSSource_ID from " + schemaName + "ERSSource_LU where ERSSource_Desc = '" + comboBox_source.Text + "'";
            DataTable dt = GetData(sql);
            if (dt.Rows.Count == 0)
            {
                return "-1";
            }
            DataRow[] dr = dt.Select();
            return dr[0]["ERSSource_ID"].ToString();
        }

        private string getUnitID()
        {
            string sql = "select ERSUnit_ID from " + schemaName + "ERSUnit_LU where ERSUnit_Desc = '" + comboBox1_unit.Text + "'";
            DataTable dt = GetData(sql);
            if (dt.Rows.Count == 0)
            {
                return "-1";
            }
            DataRow[] dr = dt.Select();
            return dr[0]["ERSUnit_ID"].ToString();
        }


     





        private void button_loadData_Click(object sender, EventArgs e)
        {
            // first validate input
            // if commodity is empty, return
            // if stat type is empty, return
            string sqlDataSeries = "";

            if (comboBox_commodity.Text.Equals("")&& textBox1_DS.Text.Equals(""))
            {
                MessageBox.Show("Please choose a commodity or Dataseries");
                resetFields();
                return;
            }
            if(!comboBox_statType.Text.Equals("")&& !textBox1_DS.Text.Equals(""))
            {
                MessageBox.Show("You choose statistic type or Dataseries");
                resetFields();
                return;
            }
            if(!comboBox1_unit.Text.Equals("")&& !textBox1_DS.Text.Equals(""))
            {
                MessageBox.Show("You choose Unit or Dataseries");
                resetFields();
                return;

            }
            if (!comboBox_source.Text.Equals("") && !textBox1_DS.Text.Equals(""))
            {
                MessageBox.Show("You choose Source or Dataseries");
                resetFields();
                return;

            }
            if (!comboBox_commodity.Text.Equals("")&&!textBox1_DS.Text.Equals(""))
            {
                MessageBox.Show("Please enter Data SeriesID or use the dropdown list");
                resetFields();
                return;
            }

            // first get geoID
            // then get timeID
          
            // NO SUCH GEO ID AND NO SUCH TIME ID
           

          

          


            // re-do 3 steps filtering, cuz finalResult is still the one filtered by commodity
            // filter by stat type
            string statTypeID = getStatTypeID();
          

            statResult = new DataTable();
            // get all data series id for such commodity and stattype
            if (textBox1_DS.Text.Equals(""))
            { //when selecting only commodity dropdown
                if (!comboBox_commodity.Text.Equals("") && comboBox_statType.Text.Equals(""))
                    sqlDataSeries = "select distinct ERSCommodity_ID from " + schemaName + "ERSCommodityDataSeries where ERSCommoditySubCommodity_ID = " + getCommodityID(comboBox_commodity.Text);
                // combo of commodity and stat type
                if (!comboBox_commodity.Text.Equals("") && !comboBox_statType.Text.Equals(""))
                    sqlDataSeries = "select distinct ERSCommodity_ID from " + schemaName + "ERSCommodityDataSeries cds" + "," + schemaName + "ERSDataValues dv" + " where cds.ERSCommoditySubCommodity_ID = " + getCommodityID(comboBox_commodity.Text) + " and cds.ERSCommodity_ERSStatisticType_ID = " + statTypeID + 
                            " and dv.ERSDataValues_ERSCommodity_ID=cds.ERSCommodity_ID";
                //combo of commodity and source
                if (!comboBox_commodity.Text.Equals("") && !comboBox_source.Text.Equals(""))
                    sqlDataSeries = "select distinct ERSCommodity_ID from " + schemaName + "ERSCommodityDataSeries cds" + "," + schemaName + "ERSDataValues dv" + " where cds.ERSCommoditySubCommodity_ID = " + getCommodityID(comboBox_commodity.Text) +  " and dv.ERSDataValues_ERSSource_ID=" + getSourceID() +
                            " and dv.ERSDataValues_ERSCommodity_ID=cds.ERSCommodity_ID";
                // combo of commodity and unit
                if (!comboBox_commodity.Text.Equals("") && !comboBox1_unit.Text.Equals(""))
                    sqlDataSeries = "select distinct ERSCommodity_ID from " + schemaName + "ERSCommodityDataSeries cds" + "," + schemaName + "ERSDataValues dv" + " where cds.ERSCommoditySubCommodity_ID = " + getCommodityID(comboBox_commodity.Text) + " and dv.ERSDataValues_ERSUnit_ID= " + getUnitID() +
                            " and dv.ERSDataValues_ERSCommodity_ID=cds.ERSCommodity_ID";
                // combo of commodity, stat type and unit
                if (!comboBox_commodity.Text.Equals("") && !comboBox_statType.Text.Equals("") && !comboBox1_unit.Text.Equals(""))
                    sqlDataSeries = "select distinct ERSCommodity_ID from " + schemaName + "ERSCommodityDataSeries cds" + "," + schemaName + "ERSDataValues dv" + " where cds.ERSCommoditySubCommodity_ID = " + getCommodityID(comboBox_commodity.Text) + " and cds.ERSCommodity_ERSStatisticType_ID = " + statTypeID + " and dv.ERSDataValues_ERSUnit_ID= " + getUnitID() +
                            " and dv.ERSDataValues_ERSCommodity_ID=cds.ERSCommodity_ID";
                // combo of commodity, stat type and source
                if (!comboBox_commodity.Text.Equals("") && !comboBox_statType.Text.Equals("")  && !comboBox_source.Text.Equals(""))
                    sqlDataSeries = "select distinct ERSCommodity_ID from " + schemaName + "ERSCommodityDataSeries cds" + "," + schemaName + "ERSDataValues dv" + " where cds.ERSCommoditySubCommodity_ID = " + getCommodityID(comboBox_commodity.Text) + " and cds.ERSCommodity_ERSStatisticType_ID = " + statTypeID + " and dv.ERSDataValues_ERSSource_ID=" + getSourceID() +
                            " and dv.ERSDataValues_ERSCommodity_ID=cds.ERSCommodity_ID";
                // combo of commodity, stat type and unit and source(all four)
                if (!comboBox_commodity.Text.Equals("") && !comboBox_statType.Text.Equals("") && !comboBox1_unit.Text.Equals("")&& !comboBox_source.Text.Equals(""))
                    sqlDataSeries = "select distinct ERSCommodity_ID from " + schemaName + "ERSCommodityDataSeries cds" + "," + schemaName + "ERSDataValues dv" + " where cds.ERSCommoditySubCommodity_ID = " + getCommodityID(comboBox_commodity.Text) + " and cds.ERSCommodity_ERSStatisticType_ID = " + statTypeID + " and dv.ERSDataValues_ERSUnit_ID= " + getUnitID() + " and dv.ERSDataValues_ERSSource_ID=" + getSourceID() +
                            " and dv.ERSDataValues_ERSCommodity_ID=cds.ERSCommodity_ID";


                DataTable dt = GetData(sqlDataSeries);

                DataRow[] dr = dt.Select();



                // count all data values for these data series IDs
                if (dt.Rows.Count < 1) // no such values
                {
                    label_result.Text = "0 Rows";
                    MessageBox.Show("No records found. Please adjust filter conditions and select different data.");
                    if (dataReadyToPush != null)
                        dataReadyToPush.Clear();
                    return;
                }
            }

            else
            {
                sqlDataSeries = "select  ERSDataValues_ERSCommodity_ID from " + schemaName + "ERSDataValues where ERSDataValues_ERSCommodity_ID = " + textBox1_DS.Text;
                DataTable dt = GetData(sqlDataSeries);

                DataRow[] dr = dt.Select();



                // count all data values for these data series IDs
                if (dt.Rows.Count < 1) // no such values
                {
                    label_result.Text = "0 Rows";
                    MessageBox.Show("No records found. Please adjust filter conditions and select different data.");
                    if (dataReadyToPush != null)
                        dataReadyToPush.Clear();
                    return;
                }
            }


            /// for each data series id, look up results

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //string dataSeriesID = dr[i]["ERSCommodity_ID"].ToString();
            String sql;
            
                sql = "select ERSDataValues_ID from " + schemaName + "ERSDataValues where ERSDataValues_ERSCommodity_ID IN (" + sqlDataSeries + ")";
                DataTable temp = GetData(sql);
                statResult.Merge(temp);
                dataReadyToPush = statResult;
            //}


           
            if (null != dataReadyToPush)
                label_result.Text = dataReadyToPush.Rows.Count + "  Rows";
            else
            {
                label_result.Text = "0  Rows";
                MessageBox.Show("No records found. Please adjust filter conditions and select different data.");
            }


        }

       

        private void button_apply_Click(object sender, EventArgs e) // this function applies the privacy setting to the data series, and inserts tat into the CoSD DB
        {


            if (comboBox_commodity.Text.Equals("")&&textBox1_DS.Text.Equals(""))
            {
                MessageBox.Show("Please choose a Commodity or Data series");
                resetFields();


                return;
            }
            if (label_result.Text.Equals(""))
            {
                MessageBox.Show("Please first select the data and press Locate Data");
                resetFields();
                return;
            }
            if (dataReadyToPush == null)
            {
                MessageBox.Show("No records to update. Please select different data.");
                return;
            }
            // check if any radio buttion is clicked
            if (textBox1_DS.Text.Equals(""))
            {
                if (MessageBox.Show("It may take few minutes to apply selected privacy. Do you really want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    if (dataReadyToPush.Rows.Count > 0)
                    {
                        waitForm.Show(this);
                        waitForm.Refresh();
                        // set privacy level
                        int privacyID = getPrivacyID();
                        DataTable dTable = new DataTable();
                        //
                        // dataReadyToPush is the final set after filter
                        // get all ids from it
                        // update one by one

                        DataRow[] rows = dataReadyToPush.Select();
                        try
                        {
                            foreach (DataRow row in rows)
                            {
                                string dataValueID = row["ERSDataValues_ID"].ToString();
                                string sql = "update " + schemaName + "ERSDataValues set ERSDataRowPrivacy_ID = " + privacyID +
                                    " where ERSDataValues_ID = " + dataValueID;
                                dTable = GetData(sql);
                                if (dTable == null || dTable.Equals(""))
                                {
                                    waitForm.Hide();
                                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                                    return;
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            waitForm.Hide();
                            MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                            //saveAction(ex.Message);
                            return;
                        }
                        waitForm.Hide();
                        MessageBox.Show("Privacy level has been applied Successfully.");
                        saveAction("Privacy level has been updated successfully");
                    }
                    else
                        MessageBox.Show("No records to update. Please select different data.");
                }
            }
            else
            {
                if (MessageBox.Show("It may take few minutes to apply selected privacy. Do you really want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    if (dataReadyToPush.Rows.Count > 0)
                    {
                        waitForm.Show(this);
                        waitForm.Refresh();
                        // set privacy level
                        int privacyID = getPrivacyID();
                        DataTable dTable = new DataTable();
                        //
                        // dataReadyToPush is the final set after filter
                        // get all ids from it
                        // update one by one

                        DataRow[] rows = dataReadyToPush.Select();
                        try
                        {
                            foreach (DataRow row in rows)
                            {
                                string dataValueID = textBox1_DS.Text;
                                string sql = "update " + schemaName + "ERSDataValues set ERSDataRowPrivacy_ID = " + privacyID +
                                    " where ERSDataValues_ERSCommodity_ID = " + dataValueID;
                                dTable = GetData(sql);
                                if (dTable == null || dTable.Equals(""))
                                {
                                    waitForm.Hide();
                                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                                    return;
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            waitForm.Hide();
                            MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                            //saveAction(ex.Message);
                            return;
                        }
                        waitForm.Hide();
                        MessageBox.Show("Privacy level has been applied Successfully.");
                        saveAction("Privacy level has been updated successfully");
                    }
                    else
                        MessageBox.Show("No records to update. Please select different data.");

                }
            
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


        private int getPrivacyID() // get the user's selection of privacy ID
        {

        
             if (radioButton_private.Checked == true)
            {
                return PRIVACY_PRIVATE;
            }
            else if (radioButton_public.Checked == true)
            {
                return PRIVACY_PUBLIC;
            }
            //PUBLIC_REPOSITORY
            
                else if (radioButton1.Checked == true)
            {
                return PUBLIC_REPOSITORY;
            }
    


            else return 0;
        }

        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void getMasterToCosd_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker.ReportProgress(1);
            
            string connectionString = ConfigurationManager.ConnectionStrings["AP_CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            //string connectionString = ConfigurationManager.ConnectionStrings["Master_CoSD_Tool.Properties.Settings.MasterCoSDConnectionString"].ConnectionString;
            string commandText = "CoSD.sp_master_to_ap";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(commandText, conn);
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
                    int count = cmd.ExecuteNonQuery();
                    int retunvalue = (int)cmd.Parameters["@Return"].Value;
                    if (count > 0 && retunvalue == 1)
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show(count + " Records processed Successfully");
                        saveAction("Master to CoSD. " + count + " Records processed Successfully");

                    }
                    else
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show("Data processing unsuccessful, please contact technical team.");

                    }

                }

                catch (Exception ex)
                {
                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                    //saveAction(ex.Message);

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

            if (e.ProgressPercentage==1)
            {
                waitForm.Show(this);
            }
            else if (e.ProgressPercentage == 0)
            {
                waitForm.Hide();
            }
        }


        private void button_MastertoCoSD_Click(object sender, EventArgs e)
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
            //string connectionString = ConfigurationManager.ConnectionStrings["Repository_CoSD_Tool.Properties.Settings.RepositoryCoSDConnectionString"].ConnectionString;
            //string commandText = "AP.sp_APCosdDataTables_to_Repository";
            string connectionString = ConfigurationManager.ConnectionStrings["AP_CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            string commandText = "CoSD.sp_ap_data_to_repository";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(commandText, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    conn.Open();
                    int count = cmd.ExecuteNonQuery();
                    if (count > 0)
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show(count + " Records processed Successfully");
                        saveAction("Master to Reposiroty. " + count + " Records processed Successfully");
                    }
                    else
                    {
                        backgroundWorker.ReportProgress(0);
                        MessageBox.Show("No Records processed");
                    }

                }

                catch (Exception ex)
                {
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


        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void CoSDtoRepository_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                waitForm.Show(this);
            }
            if (e.ProgressPercentage == 0)
            {
                waitForm.Hide();

            }
        }

        private void ManageRepoForm_Load(object sender, EventArgs e)
        {
            try
            {
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

               

                sql = "select distinct ERSGeographyDimension_Region from " + schemaName + "ERSGeographyDimension_LU ORDER BY ERSGeographyDimension_Region";
                DataTable dt2 = GetData(sql);
                

                sql = "select distinct ERSGeographyDimension_State from " + schemaName + "ERSGeographyDimension_LU ORDER BY ERSGeographyDimension_State";
           


                // load commodity
                groupIds = new List<string>();
                groupIds = getGroupIds();
                fillCommodityCombobox();
                fillStatTypeCombobox();
                fillSourceCombobox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                Console.Write(ex.Message);
            }
            dataReadyToPush = null;
            comboBox_commodity.Text = "";
            comboBox_statType.Text = "";
            comboBox_source.Text = "";
           
            label_result.Text = "";
        }

        private void button_CoSDtoRepository_Click(object sender, EventArgs e)
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

        private void comboBox_country_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

     

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox_source_SelectedIndexChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    string statTypeID = getStatTypeID();
              
            //    if(comboBox_source.Text!="")
            //    {
            //        string source = getSourceID();
            //    }
            //    int commodityid = 0;
            //    string commoditysubquery = "";
            //    if (comboBox_commodity.Text != "")
            //    {
            //        commodityid = getCommodityID(comboBox_commodity.Text);
            //        commoditysubquery = " and ERSCommoditySubCommodity_ID = " + commodityid;
            //    }

            //    string statypesubquery = "";
            //    if (comboBox_statType.Text != "")
            //    {
            //        statTypeID = getStatTypeID();
            //        statypesubquery = " and ERSCommodity_ERSStatisticType_ID = " + statTypeID;
            //    }

            //    unit = new List<string>();
            //    foreach (string groupID in groupIds)
            //    {
            //        string sql = "select distinct ERSUnit_Desc from " + schemaName
            //            + "ERSUnit_LU ulu" + "," + schemaName + "ERSCommodityDataSeries cds" + "," + schemaName + "ERSDataValues dv " + "," + schemaName + "ERSStatisticType_LU slu" +
            //            " where ulu.ERSUnit_ID=dv.ERSDataValues_ERSUnit_ID " +
            //            " and cds.ERSCommoditySubCommodity_ID= " + getCommodityID(comboBox_commodity.Text) +
            //            " and cds.ERSCommodity_ID=dv.ERSDataValues_ERSCommodity_ID " +
            //            " and slu.ERSStatisticType_ID= "+ statTypeID +
            //            " and cds.ERSCommodity_ERSSource_ID= " + source +
            //            " and [ERSCommodity_ERSGroup_ID] = " + groupID;
            //        DataTable dt = GetData(sql);
            //        DataRow[] dr = dt.Select();
            //        foreach (DataRow row in dr)
            //        {
            //            unit.Add(row["ERSUnit_Desc"].ToString());
            //        }
            //    }
            //    comboBox1_unit.DataSource = unit;
            //    comboBox1_unit.SelectedItem = null;
            //}

            //catch(Exception ee)
            //{
            //    return;
            //}
        }

        private void radioButton_public_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_DS_TextChanged(object sender, EventArgs e)
        {
            comboBox_commodity.Enabled = false;
            comboBox_commodity.Text = "";
            comboBox_source.Enabled = false;
            comboBox_source.Text = "";
            comboBox_statType.Enabled = false;
            comboBox_statType.Text = "";
            comboBox1_unit.Enabled = false;
            comboBox1_unit.Text = "";

            if(textBox1_DS.Text.Equals(""))
            {
                comboBox_commodity.Enabled = true;;
                comboBox_source.Enabled = true;;
                comboBox_statType.Enabled = true;;
                comboBox1_unit.Enabled = true;;

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fillCommodityCombobox();
            fillSourceCombobox();
            fillStatTypeCombobox();
            fillUnitCombobox();
            textBox1_DS.Text = "";
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }
    }
}
