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
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Microsoft.SqlServer.Dts.Runtime;

namespace CoSD_Tool
{
    public partial class ConversionFactorForm : Form
    {
        private List<string> Commodity;
        private List<string> CommodityForAdd;
        private List<string> startyear;
        private List<string> endyear;
        private int selectedrowindex;
        private string schemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        private SqlDataAdapter SDA = new SqlDataAdapter();
        private SqlDataAdapter SDAGetData = new SqlDataAdapter();
        private string commodityID;
        private string startyearID;
        private string EndyearID;
        private string dataLifeCycleID;
        private List<string> groupIds;
        private List<string> groups;

        WaitForm waitForm = new WaitForm();

        private string conversionFactorID { get; set; }
        private string commodityDescription { get; set; }
        private string startYearValue { get; set; }
        private string endYearValue { get; set; }
        private string description { get; set; }
        private string actualValue { get; set; }
        private string conversionSource { get; set; }
        private string marketSegment { get; set; }
        private string dataLifeCycleDesc { get; set; }

        DataTable DT;
        SqlCommandBuilder scb;
        /// <summary>
        /// The con
        /// </summary>
        private SqlConnection con =  new SqlConnection(ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString);
        /// <summary>
        /// The sqlconn
        /// </summary>
        private string sqlconn;  // query and sql connection
        private BindingSource bindingsource = new BindingSource();

        static int currentPageIndex = 0;

        string fruitGroupId = LandingScreen.GetGroupId(LandingScreen.newschemaName);

        /// <summary>
        /// The items per page
        /// </summary>
        private int pageSize = 14;

        public ConversionFactorForm()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();

        }


        private void KnowledgeBaseForm_Load(object sender, EventArgs e)
        {
            try
            {
                disablebuttons();
                fillgroupCombobox();
                //fillcommodityCombobox();
                //fillstartyearCombobox();
                //fillendyearCombobox();
                dataGridView.RowsDefaultCellStyle.Font = new Font("Tahoma", 9.75F, FontStyle.Regular);
            }
            catch
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }
        }

        private void ClearControls()
        {
            try
            {
                comboBox_commodity.Text = null;
                waterMarkTextBox_StartYear.Text = null;
                waterMarkTextBox_EndYear.Text = null;
                Combobox_Group.Text = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }


        }
        private void disablebuttons()
        {

            button_prev.Enabled = false;
            button_prev.BackColor = Color.Gray;
            groupBox_addconversion.Visible = false;
            button_next.Enabled = false;
            button_next.BackColor = Color.Gray;
            button_updateconversion.Enabled = false;
            button_updateconversion.BackColor = Color.Gray;
            ClearControls();
        }



        private void button_back_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Parent = null;
        }

        private void fillgroupCombobox()
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
            Combobox_Group.DataSource = groups;
            Combobox_Group.SelectedItem = null;

        }
        private void fillcommodityCombobox(int groupNum)
        {
            Commodity = new List<string>();

            string sql = "select ERSCommoditySubCommodity_Desc from " + schemaName + "ERSCommoditySubCommodity_LU where ERSCommoditySubCommodity_GroupID = " + groupNum + " and [ERSCommoditySubCommodity_ID] IN (SELECT distinct [ERSConversionFactor_ERSCommodity_ID] FROM " + schemaName + "[ERSConversionFactors]) ORDER BY ERSCommoditySubCommodity_Desc";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            Commodity.Add("");
            foreach (DataRow row in dr)
            {
                Commodity.Add(row["ERSCommoditySubCommodity_Desc"].ToString());
            }

            comboBox_commodity.DataSource = Commodity;

        }

        private void fillcommodityComboboxForAdd(string groupNum)
        {
            CommodityForAdd = new List<string>();

            string sql = "select ERSCommoditySubCommodity_Desc from " + schemaName + "ERSCommoditySubCommodity_LU where ERSCommoditySubCommodity_GroupID IN( " + groupNum + ") ORDER BY ERSCommoditySubCommodity_Desc";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            CommodityForAdd.Add("");
            foreach (DataRow row in dr)
            {
                CommodityForAdd.Add(row["ERSCommoditySubCommodity_Desc"].ToString());
            }

            comboBox_commodityForAdd.DataSource = CommodityForAdd;

        }
        /*Retrieve groups according to logged in user
        */
        private List<string> getGroupIds()
        {
            groupIds = new List<string>();
            // get current user            
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            string sql = "select ERSGroup_ID from " + schemaName + "ERSGroup_LU where ERSGroup_Username like '%" + currentUser.Name + "%'and [ERSGroup_ID] IN (" + fruitGroupId + ")";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            foreach (DataRow row in dr)
            {
                groupIds.Add(row["ERSGroup_ID"].ToString());
            }
            return groupIds;
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
                SDAGetData = new SqlDataAdapter(selectCommand, con);


                // Create a command builder to generate SQL update, insert, and
                // delete commands based on selectCommand. These are used to
                // update the database.
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(SDAGetData);


                DataTable result = new DataTable();
                SDAGetData.Fill(result);

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

        private void button_prev_Click_1(object sender, EventArgs e)
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

        private void button_next_Click_1(object sender, EventArgs e)
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

        private void button_update_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView.SelectedCells.Count > 0)
                {
                    selectedrowindex = dataGridView.SelectedCells[0].RowIndex;

                    DataGridViewRow selectedRow = dataGridView.Rows[selectedrowindex];

                    conversionFactorID = Convert.ToString(selectedRow.Cells["Conversion FactorID"].Value);
                    commodityDescription = Convert.ToString(selectedRow.Cells["Commodity Description"].Value);
                    startYearValue = Convert.ToString(selectedRow.Cells["Start Year"].Value);
                    endYearValue = Convert.ToString(selectedRow.Cells["End Year"].Value);
                    description = Convert.ToString(selectedRow.Cells["Description"].Value);
                    actualValue = selectedRow.Cells["Actual Value"].Value.ToString();
                    conversionSource = Convert.ToString(selectedRow.Cells["Conversion source"].Value);
                    marketSegment = Convert.ToString(selectedRow.Cells["Market segment"].Value);

                    SubqueryexecutionForUpdation();

                    con.Open();

                    if (MessageBox.Show("Do you really want to Update these values?", "Confirm Update", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SqlDataAdapter SDA;
                        string sql;

                        if (!commodityID.Equals(""))
                            commodityID = " [ERSConversionFactor_ERSCommodity_ID] = " + commodityID + ", ";
                        if (!startyearID.Equals(""))
                            startyearID = "[ERSConversionFactor_StartYear_ERSTimeDimension_ID] = '" + startyearID + "',";
                        if (!EndyearID.Equals(""))
                            EndyearID = "[ERSConversionFactor_EndYear_ERSTimeDimension_ID] ='" + EndyearID + "',";
                        if (!actualValue.Equals(""))
                            actualValue = "[ERSConversionFactor_CF] ='" + actualValue + "',";
                        if (!conversionSource.Equals(""))
                            conversionSource = "[ERSConversionFactor_CFSource] ='" + conversionSource + "',";
                        if (!marketSegment.Equals(""))
                            marketSegment = "[ERSConversionFactor_CFMarketSegment]='" + marketSegment + "'";

                        sql = "Update " + schemaName + "[ERSConversionFactors] SET " + commodityID + startyearID + EndyearID + actualValue + conversionSource + marketSegment + " WHERE [ERSConversionFactorID] IN  (" + conversionFactorID + " )";
                        SDA = new SqlDataAdapter(@"Update " + schemaName + "[ERSConversionFactors] SET " + commodityID + startyearID + EndyearID + actualValue + conversionSource + marketSegment + " WHERE [ERSConversionFactorID] IN  (" + conversionFactorID + " )", con);


                        SDA.SelectCommand.ExecuteNonQuery();
                        MessageBox.Show("Updates successfully submitted to CoSD");
                        //string sql = "Update " + schemaName + "[ERSConversionFactors] SET [ERSConversionFactorID] = '" + conversionFactorID + "',[ERSConversionFactor_ERSCommodity_ID] = " + commodityID + ",[ERSConversionFactor_StartYear_ERSTimeDimension_ID] = '" + startyearID + "',[ERSConversionFactor_EndYear_ERSTimeDimension_ID] ='" + EndyearID + "',[ERSConversionFactor_Desc]='" + description + "',[ERSConversionFactor_CF]='" + actualValue + "',[ERSConversionFactor_CFSource]='" + conversionSource + "',[ERSConversionFactor_CFMarketSegment]='" + marketSegment + "',[ERSConversionFactor_DataLifecycleID]=" + dataLifeCycleID + " WHERE [ERSConversionFactorID] IN  (" + conversionFactorID + " )";
                        saveAction(sql);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                //saveAction(ex.Message);
            }

            finally
            {
                con.Close();
                button_retrieveconversion_Click(sender, e);
            }

        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            button_updateconversion.Enabled = true;
            button_updateconversion.UseVisualStyleBackColor = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.dataGridView.DataSource = null;
            this.dataGridView.Rows.Clear();
            disablebuttons();
            label_pageNum.Text = "Page";
            callingNull();

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

        private Boolean validateYear()
        {
            Regex numberValidate = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

            // check if it's a number
            if ((!waterMarkTextBox_StartYear.Text.Equals("") && !numberValidate.IsMatch(waterMarkTextBox_StartYear.Text)))
            {
                MessageBox.Show("Invalid Start Year!\nPlease type numeric characters only (four digits year value).");
                return false;
            }
            else if ((!waterMarkTextBox_StartYear.Text.Equals("")) && (int.Parse(waterMarkTextBox_StartYear.Text) <= 1000 || int.Parse(waterMarkTextBox_StartYear.Text) >= 9999))
            {
                MessageBox.Show("Invalid Start Year!\nPlease type four digits year value.");
                return false;
            }
            else if ((!waterMarkTextBox_EndYear.Text.Equals("") && !numberValidate.IsMatch(waterMarkTextBox_EndYear.Text)))
            {
                MessageBox.Show("Invalid End Year!\nPlease type numeric characters only (four digits year value).");
                return false;
            }
            else if ((!waterMarkTextBox_EndYear.Text.Equals("")) && (int.Parse(waterMarkTextBox_EndYear.Text) <= 1000 || int.Parse(waterMarkTextBox_EndYear.Text) >= 9999))
            {
                MessageBox.Show("Invalid End Year!\nPlease type four digits year value.");
                return false;
            }
            else
                return true;
        }

        private void button_retrieveconversion_Click(object sender, EventArgs e)
        {

            /*setting fields*/
            //button_next.Enabled = true;
            //button_prev.Enabled = true;
            currentPageIndex = 0;
            label_pageNum.Text = "Page";
            string commodity_CF = "";
            string startYear_CF = "";
            string endYear_CF = "";
            string group_CF = "";



            if (Combobox_Group.Text.Equals(""))
            {
                MessageBox.Show("Please select a Group");
                return;
            }

            ////Validate start and end year
            //if (!validateYear())
            //    return;

            //start wait form
            waitForm.Show(this);
            waitForm.Refresh();

            try
            {
                con.Open();
                if (comboBox_commodity.Text == "" && waterMarkTextBox_EndYear.Text == "" && waterMarkTextBox_StartYear.Text == "")
                {
                    group_CF = " AND CV.ERSConversionFactor_ERSCommodity_ID IN (SELECT [ERSCommoditySubCommodity_ID] FROM  " + schemaName + "[ERSCommoditySubCommodity_LU] where [ERSCommoditySubCommodity_GroupID] IN (SELECT ERSGroup_ID FROM  " + schemaName + "[ERSGroup_LU] where ERSGroup_Desc like '" + Combobox_Group.Text + "')) ";
                }
                //else
                //{
                //    if (comboBox_commodity.Text != "")
                //    {
                //        commodity_CF = " CV.ERSConversionFactor_ERSCommodity_ID IN(SELECT ERSCommoditySubCommodity_ID FROM " + schemaName + "[ERSCommoditySubCommodity_LU] WHERE ERSCommoditySubCommodity_Desc = '" + comboBox_commodity.Text + "')";
                //    }
                //    if (waterMarkTextBox_EndYear.Text != "")
                //    {
                //        endYear_CF = " AND CV.ERSConversionFactor_EndYear_ERSTimeDimension_ID IN(SELECT TD.ERSTimeDimension_ID FROM " + schemaName + "[ERSTimeDimension_LU] TD WHERE TD.ERSTimeDimension_Year = '" + waterMarkTextBox_EndYear.Text + "')";
                //    }
                //    if (waterMarkTextBox_StartYear.Text != "")
                //    {
                //        startYear_CF = " AND CV.ERSConversionFactor_StartYear_ERSTimeDimension_ID IN (SELECT TD.ERSTimeDimension_ID FROM " + schemaName + "[ERSTimeDimension_LU] TD WHERE TD.ERSTimeDimension_Year = '" + waterMarkTextBox_StartYear.Text + "' )  ";
                //    }
                //}
                if (comboBox_commodity.Text != "")
                {
                    commodity_CF = " AND CV.ERSConversionFactor_ERSCommodity_ID IN(SELECT ERSCommoditySubCommodity_ID FROM " + schemaName + "[ERSCommoditySubCommodity_LU] WHERE ERSCommoditySubCommodity_Desc = '" + comboBox_commodity.Text + "')";
                }
                //else
                //{
                //    waitForm.Hide();
                //    MessageBox.Show("Please select any other combinations for retrieving the values");
                //    dataGridView.DataSource = null;
                //    return;
                //}

                //printing
                string sql = "SELECT CV.[ERSConversionFactorID] AS 'Conversion FactorID', " +
" ERSCommoditySubCommodity_Desc 'Commodity Description', " +
" StartYear.ERSTimeDimension_Year AS 'Start Year', " +
" EndYear.ERSTimeDimension_Year AS 'End Year', " +
" CV.[ERSConversionFactor_Desc] AS 'Description', " +
" CV.[ERSConversionFactor_CF] AS 'Actual Value', " +
" CV.[ERSConversionFactor_CFSource] AS 'Conversion source', " +
" CV.[ERSConversionFactor_CFMarketSegment] AS 'Market segment' " +
" FROM " + schemaName + "[ERSConversionFactors] CV " +
" Left JOIN " + schemaName + "[ERSCommoditySubCommodity_LU] ON CV.[ERSConversionFactor_ERSCommodity_ID] = [ERSCommoditySubCommodity_ID] " +
" Left JOIN " + schemaName + "ERSTimeDimension_LU StartYear ON CV.ERSConversionFactor_StartYear_ERSTimeDimension_ID = StartYear.ERSTimeDimension_ID " +
" Left JOIN " + schemaName + "ERSTimeDimension_LU EndYear ON CV.ERSConversionFactor_EndYear_ERSTimeDimension_ID = EndYear.ERSTimeDimension_ID " +
" WHERE " + group_CF + commodity_CF + startYear_CF + endYear_CF;

                SqlDataAdapter SDA = new SqlDataAdapter(@"SELECT CV.[ERSConversionFactorID] AS 'Conversion FactorID', " +
" ERSCommoditySubCommodity_Desc 'Commodity Description', " +
" StartYear.ERSTimeDimension_Year AS 'Start Year', " +
" EndYear.ERSTimeDimension_Year AS 'End Year', " +
" CV.[ERSConversionFactor_Desc] AS 'Description', " +
" CV.[ERSConversionFactor_CF] AS 'Actual Value', " +
" CV.[ERSConversionFactor_CFSource] AS 'Conversion source', " +
" CV.[ERSConversionFactor_CFMarketSegment] AS 'Market segment' " +
" FROM " + schemaName + "[ERSConversionFactors] CV " +
" Left JOIN " + schemaName + "[ERSCommoditySubCommodity_LU] ON CV.[ERSConversionFactor_ERSCommodity_ID] = [ERSCommoditySubCommodity_ID] " +
" Left JOIN " + schemaName + "ERSTimeDimension_LU StartYear ON CV.ERSConversionFactor_StartYear_ERSTimeDimension_ID = StartYear.ERSTimeDimension_ID " +
" Left JOIN " + schemaName + "ERSTimeDimension_LU EndYear ON CV.ERSConversionFactor_EndYear_ERSTimeDimension_ID = EndYear.ERSTimeDimension_ID " +
" WHERE 1=1 " + group_CF + commodity_CF + startYear_CF + endYear_CF, con);

                SDA.SelectCommand.ExecuteNonQuery();
                DT = new DataTable();
                SDA.Fill(DT);
                dataGridView.DataSource = DT;
                dataGridView.Columns[0].DefaultCellStyle.ForeColor = Color.Gray;
                dataGridView.Columns[0].ReadOnly = true;
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

                if (DT.Rows.Count == 0)
                {
                    waitForm.Hide();
                    MessageBox.Show("Please select any other combinations for retrieving the values");
                    dataGridView.DataSource = null;
                    return;
                }
            }

            catch (Exception ex)
            {
                waitForm.Hide();
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                dataGridView.DataSource = null;
            }

            finally
            {
                waitForm.Hide();
                con.Close();

            }

        }

        private void button_addconversion_Click(object sender, EventArgs e)
        {
            groupBox_addconversion.Visible = true;
            fillcommodityComboboxForAdd(LandingScreen.GetGroupId(LandingScreen.newschemaName));
        }

        private void SubquereyexecutionForInsertion()
        {
            try
            {
                con.Open();
                string sql = "SELECT [ERSCommoditySubCommodity_ID] FROM " + schemaName + "[ERSCommoditySubCommodity_LU] where ERSCommoditySubCommodity_Desc like '" + comboBox_commodityForAdd.Text + "'";
                SqlCommand cmd1 = new SqlCommand(@"SELECT [ERSCommoditySubCommodity_ID] FROM " + schemaName + "[ERSCommoditySubCommodity_LU] where ERSCommoditySubCommodity_Desc like '" + comboBox_commodityForAdd.Text + "'", con);

                SqlCommand cmd2 = new SqlCommand(@"SELECT [ERSTimeDimension_ID] FROM " + schemaName + "[ERSTimeDimension_LU] where ERSTimeDimension_Year = " + waterMarkTextBox_StartYear.Text + " and ERSTimeDimension_TimeDimensionType_ID = 17", con);

                SqlCommand cmd3 = new SqlCommand(@"SELECT [ERSTimeDimension_ID] FROM " + schemaName + "[ERSTimeDimension_LU] where ERSTimeDimension_Year = " + waterMarkTextBox_EndYear.Text + " and ERSTimeDimension_TimeDimensionType_ID = 17", con);


                commodityID = Convert.ToString(cmd1.ExecuteScalar());
                startyearID = Convert.ToString(cmd2.ExecuteScalar());
                EndyearID = Convert.ToString(cmd3.ExecuteScalar());
            }

            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }

            finally
            {
                con.Close();

            }

        }

        private void SubqueryexecutionForUpdation()
        {
            try
            {
                con.Open();
                if (!commodityDescription.Equals(""))
                {
                    SqlCommand cmd1 = new SqlCommand(@"SELECT [ERSCommoditySubCommodity_ID] FROM " + schemaName + "[ERSCommoditySubCommodity_LU] where ERSCommoditySubCommodity_Desc like '" + commodityDescription + "'", con);
                    commodityID = Convert.ToString(cmd1.ExecuteScalar());
                }
                else
                    commodityID = "";
                if (!startYearValue.Equals(""))
                {
                    SqlCommand cmd2 = new SqlCommand(@"SELECT [ERSTimeDimension_ID] FROM " + schemaName + "[ERSTimeDimension_LU] where ERSTimeDimension_Year = " + startYearValue + " and ERSTimeDimension_TimeDimensionType_ID = 17", con);
                    startyearID = Convert.ToString(cmd2.ExecuteScalar());
                }
                else
                    startyearID = "";

                if (!endYearValue.Equals(""))
                {
                    SqlCommand cmd3 = new SqlCommand(@"SELECT [ERSTimeDimension_ID] FROM " + schemaName + "[ERSTimeDimension_LU] where ERSTimeDimension_Year = " + endYearValue + " and ERSTimeDimension_TimeDimensionType_ID = 17", con);
                    EndyearID = Convert.ToString(cmd3.ExecuteScalar());
                }
                else
                    EndyearID = "";



            }

            catch (Exception ex)
            {
                MessageBox.Show("Data processing unsuccessful, please contact technical team.");
            }

            finally
            {
                con.Close();

            }

        }

        private void button_submit_Click(object sender, EventArgs e)
        {
            Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");

            if (comboBox_commodityForAdd.Text.Equals(""))
            {
                MessageBox.Show("Please select commodity");
                return;
            }
            if (!CommodityForAdd.Contains(comboBox_commodityForAdd.Text))
            {
                MessageBox.Show("Please select a different commodity");
                return;
            }

            if (waterMarkTextBox_StartYear.Text.Equals(""))
            {
                MessageBox.Show("Please enter start year");
                return;
            }

            if (waterMarkTextBox_EndYear.Text.Equals(""))
            {
                MessageBox.Show("Please enter end year");
                return;
            }
            if (waterMarkTextBox_marketsegment.Text.Equals(""))
            {
                MessageBox.Show("Please enter Market Segment value");
                return;
            }
            if (textBox_source.Text.Equals(""))
            {
                MessageBox.Show("Please enter Source value");
                return;
            }
            // check if it's a number
            if (waterMarkTextBox_conversionvalue.Text.Equals(""))
            {
                MessageBox.Show("Please enter conversion value");
                return;
            }
            else if (!numberchk.IsMatch(waterMarkTextBox_conversionvalue.Text))
            {
                MessageBox.Show("Please enter correct conversion value");
                return;
            }

            //validate start and end year
            if (!validateYear())
                return;

            try
            {
                SubquereyexecutionForInsertion();
                if (startyearID.Equals(""))
                {
                    MessageBox.Show("Please enter a valid Start Year");
                    return;
                }
                else if (EndyearID.Equals(""))
                {
                    MessageBox.Show("Please enter a valid End Year");
                    return;
                }
                con.Open();

                SqlCommand cmd = new SqlCommand(@"Insert INTO " + schemaName + "[ERSConversionFactors] (ERSConversionFactor_ERSCommodity_ID,ERSConversionFactor_StartYear_ERSTimeDimension_ID,ERSConversionFactor_EndYear_ERSTimeDimension_ID,ERSConversionFactor_Desc,ERSConversionFactor_CF,ERSConversionFactor_CFSource,ERSConversionFactor_CFMarketSegment) VALUES (@commodityID, @startyear, @endyear, @convdesc, @convCVvalue, @CFsource, @CFmarketSegment)", con);

                cmd.Parameters.Add("@commodityID", SqlDbType.NVarChar, 160).Value = commodityID;
                if (startyearID == "")
                {
                    cmd.Parameters.Add("@startyear", SqlDbType.Int).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@startyear", SqlDbType.Int).Value = startyearID;
                }

                if (EndyearID == "")
                {
                    cmd.Parameters.Add("@endyear", SqlDbType.NVarChar, 160).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@endyear", SqlDbType.NVarChar, 160).Value = EndyearID;
                }
                if (waterMarkTextBox_description.Text == "")
                    cmd.Parameters.Add("@convdesc", SqlDbType.NVarChar, 160).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@convdesc", SqlDbType.NVarChar, 160).Value = waterMarkTextBox_description.Text;
                if (null != waterMarkTextBox_conversionvalue && waterMarkTextBox_conversionvalue.Text != "")
                    cmd.Parameters.Add("@convCVvalue", SqlDbType.NVarChar, 160).Value = waterMarkTextBox_conversionvalue.Text;
                if (textBox_source.Text == "")
                    cmd.Parameters.Add("@CFsource", SqlDbType.NVarChar, 160).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@CFsource", SqlDbType.NVarChar, 160).Value = textBox_source.Text;
                if (waterMarkTextBox_marketsegment.Text == "")
                    cmd.Parameters.Add("@CFmarketSegment", SqlDbType.NVarChar, 160).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@CFmarketSegment", SqlDbType.NVarChar, 160).Value = waterMarkTextBox_marketsegment.Text;

                int rows = cmd.ExecuteNonQuery();
                MessageBox.Show("New Conversion Factor values are Inserted Successfully.");
                saveAction("New Conversion Factor values are Inserted Successfully.");
                callingNull();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("FOREIGN KEY"))
                {
                    MessageBox.Show("Incorrect data selected");
                }
                else if (ex.Message.Contains("duplicate"))
                {
                    MessageBox.Show("Record already present. Please enter new data.");
                }
                else
                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                //saveAction(ex.Message);
            }

            finally
            {
                con.Close();
                if (!Combobox_Group.Text.Equals(""))
                    button_retrieveconversion_Click(sender, e);
            }

        }

        private void callingNull()
        {

            //ClearControls();
            waterMarkTextBox_marketsegment.Text = "";
            textBox_source.Text = "";
            waterMarkTextBox_conversionvalue.Text = "";
            waterMarkTextBox_description.Text = "";
            waterMarkTextBox_StartYear.Text = "";
            waterMarkTextBox_EndYear.Text = "";
            comboBox_commodityForAdd.Text = "";
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

        private void comboBox_commodity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void waterMarkTextBox_StartYear_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }


        private void waterMarkTextBox_StartYear_TextChanged(object sender, EventArgs e)
        {
            Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            // check if it's a number
            if (!waterMarkTextBox_StartYear.Text.Equals("") && !numberchk.IsMatch(waterMarkTextBox_StartYear.Text))
            {
                errorProviderCF.SetError(waterMarkTextBox_StartYear, "Invalid input!\nPlease type numeric characters only (four digits year value).");
                waterMarkTextBox_StartYear.BackColor = Color.Red;
                return;
            }
            else if (!waterMarkTextBox_StartYear.Text.Equals("") && numberchk.IsMatch(waterMarkTextBox_StartYear.Text))
            {
                errorProviderCF.SetError(waterMarkTextBox_StartYear, "");
                waterMarkTextBox_StartYear.BackColor = Color.White;

            }
            else
            {
                errorProviderCF.SetError(waterMarkTextBox_StartYear, "");
                waterMarkTextBox_StartYear.BackColor = Color.White;
            }
        }

        private void waterMarkTextBox_EndYear_TextChanged(object sender, EventArgs e)
        {
            Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            // check if it's a number
            if (!waterMarkTextBox_EndYear.Text.Equals("") && !numberchk.IsMatch(waterMarkTextBox_EndYear.Text))
            {
                errorProviderCF.SetError(waterMarkTextBox_EndYear, "Invalid input!\nPlease type numeric characters only (four digits year value).");
                waterMarkTextBox_EndYear.BackColor = Color.Red;
                return;
            }
            else if (!waterMarkTextBox_EndYear.Text.Equals("") && numberchk.IsMatch(waterMarkTextBox_EndYear.Text))
            {
                errorProviderCF.SetError(waterMarkTextBox_StartYear, "");
                waterMarkTextBox_EndYear.BackColor = Color.White;

            }
            else
            {
                errorProviderCF.SetError(waterMarkTextBox_EndYear, "");
                waterMarkTextBox_EndYear.BackColor = Color.White;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void waterMarkTextBox_conversionvalue_TextChanged(object sender, EventArgs e)
        {
            Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            // check if it's a number
            if (!waterMarkTextBox_conversionvalue.Text.Equals("") && !numberchk.IsMatch(waterMarkTextBox_conversionvalue.Text))
            {
                errorProvider1.SetError(waterMarkTextBox_conversionvalue, "Invalid input!\nPlease type numeric characters only.");
                waterMarkTextBox_conversionvalue.BackColor = Color.Red;
                return;
            }
            else if (!waterMarkTextBox_conversionvalue.Text.Equals("") && numberchk.IsMatch(waterMarkTextBox_conversionvalue.Text))
            {
                errorProvider1.SetError(waterMarkTextBox_conversionvalue, "");
                waterMarkTextBox_conversionvalue.BackColor = Color.White;

            }
            else
            {
                errorProvider1.SetError(waterMarkTextBox_conversionvalue, "");
                waterMarkTextBox_conversionvalue.BackColor = Color.White;
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

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Don't throw an exception when we're done.
            e.ThrowException = false;
            MessageBox.Show("Please enter a valid value.");
            // If this is true, then the user is trapped in this cell.
            e.Cancel = false;
        }

        private void Combobox_Group_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int groupNum = getGroupID(Combobox_Group.SelectedItem.ToString());
            fillcommodityCombobox(groupNum);
            comboBox_commodity.Enabled = true;
        }

        private void fillALLCombobox(string commodityDescription, string startYear, string endYear)
        {

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


    }
}
