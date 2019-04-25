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
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace CoSD_Tool
{
    public partial class ConstructedVariables : Form
    {
        /// <summary>
        /// The con
        /// </summary>
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString);
        /// <summary>
        /// The sqlconn
        /// </summary>
        private string IDvalue { get; set; }
        private string Formula { get; set; }
        private string numberofinputs { get; set; }
        private string Inputs { get; set; }
        private string Time { get; set; }
        private string BusinessLogicType { get; set; }
        private string Privacy { get; set; }
        private string Description { get; set; }
        private string Source { get; set; }
        private string Output { get; set; }
        private string ConstructedVariable { get; set; }
        private string OutputTime { get; set; }
        private int count1{ get; set; }
        private double waitTime { get; set; }


        private string sqlconn;  // query and sql connection
        WaitForm waitForm = new WaitForm();

        public BusinessRulesInsertForm businessrulesinsertForm = new BusinessRulesInsertForm();
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
        string failedFileName = "";
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
        private List<string> outputName;
        private List<string> source;


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
            catch (SqlException ex)
            {
                throw new Exception("Connection Error");
            }
        }




        public ConstructedVariables()
        {
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
            Load += new EventHandler(Constructed_VariablesLoad);


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

        private void fillOutputNameCombobox()
        {
            outputName = new List<string>();
            string sql = @"SELECT DISTINCT ERSConstructedVariable_OutputName FROM " + schemaName + "[ERSConstructedVariablesOutcomes] order by ERSConstructedVariable_OutputName ";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            outputName.Add("");
            foreach (DataRow row in dr)
            {
                outputName.Add(row["ERSConstructedVariable_OutputName"].ToString());
            }

            comboBox_OutputName.DataSource = outputName;
            comboBox_OutputName.DropDownWidth = DropDownWidth(comboBox_OutputName);
            this.comboBox_OutputName.DrawMode = DrawMode.OwnerDrawFixed;
            this.comboBox_OutputName.DrawItem += new DrawItemEventHandler(comboBox_OutputName_DrawItem);
            comboBox_OutputName.SelectedItem = null;

        }

        int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0;
            int temp = 0;
            Label label1 = new Label();

            foreach (var obj in myCombo.Items)
            {
                label1.Text = obj.ToString();
                temp = label1.PreferredWidth;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            label1.Dispose();
            return maxWidth;
        }

        void comboBox_OutputName_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = this.comboBox_OutputName.GetItemText(comboBox_OutputName.Items[e.Index]);
            e.DrawBackground();
            using (SolidBrush br = new SolidBrush(e.ForeColor))
            { e.Graphics.DrawString(text, e.Font, br, e.Bounds); }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            { this.toolTip1.Show(text, comboBox_OutputName, e.Bounds.Right, e.Bounds.Bottom); }
            else { this.toolTip1.Hide(comboBox_OutputName); }
            e.DrawFocusRectangle();
        }

        private void fillSourceCombobox()
        {
            source = new List<string>();
            string sql = @"SELECT DISTINCT ERSConstructedVariable_InputSources FROM " + schemaName + "ERSConstructedVariablesOutcomes";
            DataTable dt = GetData(sql);
            DataRow[] dr = dt.Select();
            source.Add("");
            foreach (DataRow row in dr)
            {
                source.Add(row["ERSConstructedVariable_InputSources"].ToString());
            }

            comboBox_Source.DataSource = source;
            comboBox_Source.SelectedItem = null;

        }

        private void GetNewRow(ref DataRow newRow, DataRow source)
        {
            foreach (DataColumn col in DT.Columns)
            {
                newRow[col.ColumnName] = source[col.ColumnName];
            }
        }



        private void button_back_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Parent = null;

        }




        private void button_reset_Click(object sender, EventArgs e)
        {
            this.dataGridView.DataSource = null;
            this.dataGridView.Columns.Clear();
            this.dataGridView.Rows.Clear();

            //Reset fields.
            comboBox_OutputName.SelectedItem = null;
            comboBox_Source.SelectedItem = null;
            button_prev.Enabled = false;
            button_next.Enabled = false;
            label_pageNum.Text = "Page";

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

        private void button1_Click(object sender, EventArgs e)
        {
            //if (businessrulesinsertForm.IsDisposed == true)
            //{
            //    BusinessRulesInsertForm BlinsertForm = new BusinessRulesInsertForm();
            //    BlinsertForm.Show();
            //}
            //else
            //{
            //    businessrulesinsertForm.Show();
            //}
            BusinessRulesInsertForm BlinsertForm = new BusinessRulesInsertForm();
            BlinsertForm.Show();
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


        private void wait_label_Click(object sender, EventArgs e)
        {

        }

        private void longwait_label_Click(object sender, EventArgs e)
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

        private void Constructed_VariablesLoad(object sender, EventArgs e)
        {
            //Filling Comboboxes
            fillOutputNameCombobox();
            fillSourceCombobox();
        }

        private void button_retreive_Click(object sender, EventArgs e)
        {

            if (comboBox_OutputName.Text == "" && comboBox_Source.Text == "")
            {
                MessageBox.Show("Please select at least one input value.");
                return;
            }

            //start wait form
            waitForm.Show(this);
            waitForm.Refresh();

            //resetting page index
            currentPageIndex = 0;

            string outputName = comboBox_OutputName.Text;
            string source = comboBox_Source.Text;
            try
            {
                //Only output input
                if (outputName != "")
                    outputName = " and ERSConstructedVariable_OutputName like '%" + outputName + "%' ";
                //Only source 
                if (source != "")
                    source = " and ERSConstructedVariable_InputSources = '" + source + "' ";


                con.Open();

                string sql = "select  [ERSConstructedVariable_BusinessLogicID] AS RuleID " + ",[ERSConstructedVariable_ExecutionDate] AS ExecutionTime" +
      ",ISNULL(CONVERT(varchar(20), ERSConstructedVariable_OutputValue), 'Error in Business rule')  AS OutputValue " +
      ",[ERSConstructedVariable_TimeDimensionDate] AS Date " +
      ",[ERSConstructedVariable_OutputName] AS OutputName " +
      ",[ERSConstructedVariable_LongDescription] AS LongDescription " +
      ",[ERSConstructedVariable_InputSources] AS Source " +
      ",ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country " +
      ",ERSGeographyDimension_LU.ERSGeographyDimension_State as State " +
      " FROM " + schemaName + "ERSBusinessLogic, " + schemaName + "ERSConstructedVariablesOutcomes, " + schemaName + "ERSGeographyDimension_LU WHERE 1=1 " + outputName + source +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSConstructedVariablesOutcomes.ERSConstructedVariable_OutputGeographyDimensionID" +
      " and " + schemaName + "ERSBusinessLogic.ERSBusinessLogic_ID = " + schemaName + "ERSConstructedVariablesOutcomes.ERSConstructedVariable_BusinessLogicID";

                SDA = new SqlDataAdapter(@"select  [ERSConstructedVariable_BusinessLogicID] AS RuleID " + ",[ERSConstructedVariable_ExecutionDate] AS ExecutionTime" +
      ",ISNULL(CONVERT(varchar(30), ERSConstructedVariable_OutputValue), 'Error in Business rule')  AS OutputValue " +
      ",[ERSConstructedVariable_TimeDimensionDate] AS Date " +
      ",[ERSConstructedVariable_OutputName] AS OutputName " +
      ",[ERSConstructedVariable_LongDescription] AS LongDescription " +
      ",[ERSConstructedVariable_InputSources] AS Source "+
      ",ERSGeographyDimension_LU.ERSGeographyDimension_Country as Country " +
      ",ERSGeographyDimension_LU.ERSGeographyDimension_State as State " +
      " FROM " + schemaName + "ERSBusinessLogic, " + schemaName + "ERSConstructedVariablesOutcomes, " + schemaName + "ERSGeographyDimension_LU WHERE 1=1 " + outputName + source +
      " and " + schemaName + "ERSGeographyDimension_LU.ERSGeographyDimension_ID = " + schemaName + "ERSConstructedVariablesOutcomes.ERSConstructedVariable_OutputGeographyDimensionID" +
      " and " + schemaName + "ERSBusinessLogic.ERSBusinessLogic_ID = " + schemaName + "ERSConstructedVariablesOutcomes.ERSConstructedVariable_BusinessLogicID"
      , con);

                SDA.SelectCommand.ExecuteNonQuery();
                DT = new DataTable();
                SDA.Fill(DT);
                count1 = DT.Rows.Count;
                if (DT.Rows.Count > 0)
                {
                    dataGridView.DataSource = DT;
                    dataGridView.ReadOnly = true;
                    //BindGrid();

                    if (Convert.ToDecimal(DT.Rows.Count) <= pageSize)
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
                    MessageBox.Show("No Constructed Variables Found");
                    dataGridView.DataSource = DT;
                    //button_DeleteRules.BackColor = Color.Gray;
                    button_prev.Enabled = false;
                    button_next.Enabled = false;
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

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }





    }
}

