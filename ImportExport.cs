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
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using Drawing = System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppForm = System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;


//new
namespace CoSD_Tool
{
    public partial class ImportExport : Form
    {
        private bool insertNewDataSeriesFlag = false;  // the flag that control whether the analyst is allowed to add new data series
        private bool newDataSeries = false; // records if a new dataserie is added
        private string filePath;   // store the path of excel file
        private OleDbConnection Econ; // used for loading excel file
        private SqlConnection con;    // db connection
        private string constr, Query, sqlconn;  // query and sql connection
        private string schemaName = Home.globalSchemaName;
        private string DboSchemaName = Home.globalDBOSchemaName;
        int successNum = 0;
        private List<string> groupDesc;
        private List<string> groupID;
        WaitForm waitForm = new WaitForm();
        System.Data.DataTable DT = new System.Data.DataTable();
        string apGroupId = Home.globalapGroupId;
        string errorMessage = "";
        BackgroundWorker backgroundWorker = new BackgroundWorker();
        String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");
        string cosdFolderPath = Home.globalCoSDPath;
        List<string> sheetNames = new List<string>();
        Dictionary<string, string> newSeriesIds = null;
        /// <summary>
        /// The data adapter
        /// </summary>
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();

        //Indexes for columns
        int seriesIDIndex = 0;
        int timeTypeIndex = 0;
        int timeValueIndex = 0;
        int geoTypeIndex = 0;
        int geoValueIndex = 0;
        int valueIndex = 0;
        int collectionIndex = 0;
        int statusIndex = 0;
        int statTypeIndex = 0;
        int unitIndex = 0;
        int commoditySubCommodityIndex = 0;
        int physicalAttributeIndex = 0;
        int prodPracticeIndex = 0;
        int utilPracticeIndex = 0;
        int sourceDescriptionIndex = 0;
        int sourceLongDescriptionIndex = 0;
        int columnsCount = 0;
        int rowsCount = 0;
        int Lastrow = 0;
        int fullRow = 0;

        public ImportExport()
        {
            this.Icon = Drawing.Icon.ExtractAssociatedIcon(AppForm.Application.ExecutablePath);
            InitializeComponent();
            label_status.Text = "";
            label_message.Text = "";
            textBox_successmsg.Text = "";

            if (DeSerializeSettingsPath<String>("CoSDSettings") != null)
                cosdFolderPath = DeSerializeSettingsPath<String>("CoSDSettings");

            if (!cosdFolderPath.EndsWith("\\"))
            {
                cosdFolderPath = cosdFolderPath + "\\";
            }
            try
            {
                ////Saving sample import sheet
                string resource = "Sample Import Sheet.xlsx";
                string filename = Path.Combine(cosdFolderPath + "CoSD Tool\\Import Results\\", resource);

                if (!File.Exists(filename))
                {
                    Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Import Results\\");
                    var assembly = Assembly.GetExecutingAssembly();
                    using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CoSD_Tool.Resources.Sample Import Sheet.xlsx"))
                    using (var targetStream = File.OpenWrite(cosdFolderPath + "CoSD Tool\\Import Results\\Sample Import Sheet.xlsx"))
                    {
                        resourceStream.CopyTo(targetStream);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not find the path : " + cosdFolderPath + ". Please change path in tool to save CoSD results.\nPath can be changed in 'Automated Validation' OR 'Import/Export Data' page");
            }

        }


        //-----------------------------------------------------------------------------BULK UPLOADING-OLD CODE. Doesnt process anything, it just loads from Excel to DB, requires the Access plugin as wel.
        //-----------------------------------------------------------------------------

        // method links excel files
        private void ExcelConn(string FilePath)
        {
            // connection string for linking excel files
            constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", FilePath);
            // init the connection to excel files
            Econ = new OleDbConnection(constr);
        }

        // main function for uploading data
        private void InsertExcelRecords(string FilePath, string sheet)
        {
            // link excel
            ExcelConn(FilePath);
            // set up string for loading data from excel
            Query = string.Format
            ("Select [ERSStatisticType_ID],[ERSStatisticType_Attribute],[ERSStatisticType_Mapping] FROM [{0}]", sheet + "$");
            OleDbCommand Ecom = new OleDbCommand(Query, Econ);
            Econ.Open();
            // loading data from excel
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            Econ.Close();
            try
            {
                oda.Fill(ds);

            }
            catch (Exception ex)
            {
                // error handler for loading excel
                label_status.ForeColor = Color.Red;
                label_status.Text = "Failed!";
                MessageBox.Show("Invalid Excel file or sheet name.", "AP CoSD", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }


            System.Data.DataTable Exceldt = ds.Tables[0];
            // connect to db
            connection();
            //creating object of SqlBulkCopy    
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            //assigning Destination table name    
            objbulk.DestinationTableName = "AP_ToolCoSD.CoSD.st";
            //Mapping Table column    
            objbulk.ColumnMappings.Add("ERSStatisticType_ID", "ERSStatisticType_ID");
            objbulk.ColumnMappings.Add("ERSStatisticType_Attribute", "ERSStatisticType_Attribute");
            objbulk.ColumnMappings.Add("ERSStatisticType_Mapping", "ERSStatisticType_Mapping");
            //inserting Datatable Records to DataBase    
            try
            {
                con.Open();
                objbulk.WriteToServer(Exceldt);
                label_status.ForeColor = Color.ForestGreen;
                label_status.Text = "Success!";

            }
            catch (Exception ex)
            {
                // error handler
                label_status.ForeColor = Color.Red;
                label_status.Text = "Failed!";
            }
            finally
            {
                con.Close();
                this.label_status.Image = null;
            }
        }

        //-----------------------------------------------------------------------------BULK UPLOADING-OLD CODE ABOVE. Doesnt process anything, it just loads from Excel to DB, requires the Access plugin as wel.
        //-----------------------------------------------------------------------------

        /// <summary>
        /// load path of excel file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_excelPath_Click(object sender, EventArgs e)
        {

            textBox_successmsg.Visible = false;
            label_message.Text = "";
            label_status.Text = "";
            //enable sheet drop down
            comboBox_excelSheet.Enabled = true;

            comboBox_excelSheet.DataSource = null;
            sheetNames = new List<string>();
            // set filter to only allow excel files
            openFileDialog1.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm"; //only can chse excel files
            openFileDialog1.FileName = "Import Sheet.xls";
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                filePath = openFileDialog1.FileName;
                textBox_excelPath.Text = filePath;
                Cursor.Current = Cursors.WaitCursor;
                object misValue = System.Reflection.Missing.Value;
                var oExcel = new Microsoft.Office.Interop.Excel.Application();

                var oBook = oExcel.Workbooks.Open(filePath);
                //loads a book - above - and a sheet - below -

                sheetNames.Add(Text = "");
                sheetNames.Add(Text = "--Select all Sheets--");

                foreach (Worksheet worksheet in oBook.Worksheets)
                {

                    sheetNames.Add(worksheet.Name);
                }


                comboBox_excelSheet.DataSource = sheetNames;

                oBook.Close(false, misValue, misValue);
                oExcel.Quit();

                Cursor.Current = Cursors.Default;
            }


        }
        /// <summary>
        /// import excel data to db
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_importExcel_Click(object sender, EventArgs e)
        {
            var countcomboitems = 3;
            string finalmessage = "";

            textBox_successmsg.Visible = false;
            label_message.Visible = false;

            if (comboBox_excelSheet.SelectedIndex == 1)
            {
                countcomboitems = comboBox_excelSheet.Items.Count;
            }


            if (filePath == null || filePath == "")
            {
                MessageBox.Show("Please choose an Excel file to Import.");
                return;
            }

            if (comboBox_excelSheet.Text == "")
            {
                MessageBox.Show("Please choose an Excel sheet/sheets.");
                return;
            }

            // ask for conformation before proceed
            if (MessageBox.Show("Importing excel may take few minutes to complete. Do you really want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                for (int j = 1; j < countcomboitems - 1; j++)
                {
                    try
                    {
                        label_status.Text = "Uploading " + j + " of " + (countcomboitems - 2) + "...";
                        //+ "\r\n" + "\r\n"  + label_message.Text ;

                        Range range = null;
                        String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");


                        /// FILE FORMAT:
                        ///A    ERSCommodityName	
                        ///B    ERSPhysicalAttribute_Category	
                        ///C    ERSPhysicalAttribute_Desc	
                        ///D    ERSProdPractice	
                        ///E    ERSUtilPractice	
                        ///F    ERSStatisticType	
                        ///G    ERSUnit	
                        ///H    Country	
                        ///I    Region	
                        ///J    State	
                        ///K    County	
                        ///L    Year	
                        ///M    Month	
                        ///N    Week	
                        ///O    Value
                        var oExcel = new Microsoft.Office.Interop.Excel.Application();

                        var oBook = oExcel.Workbooks.Open(filePath);

                        int index = 0;

                        //loads a book - above - and a sheet - below -
                        //index = 1 + comboBox_excelSheet.SelectedIndex;
                        if (comboBox_excelSheet.SelectedIndex == 1)
                        {
                            index = j;
                        }
                        else
                        {
                            index = comboBox_excelSheet.SelectedIndex - 1;
                        }

                        var sheet = (Microsoft.Office.Interop.Excel.Worksheet)oBook.Worksheets.get_Item(index);

                        // var cellValue = (string)(sheet.Cells[10, 2] as Excel.Range).Value;

                        // e.g. Read value in A2 cell 
                        //var cellValue = sheet.get_Range("F1").Value;

                        // get a range to work with
                        //range = sheet.get_Range("A1", System.Reflection.Missing.Value);
                        // get the end of values to the right (will stop at the first empty cell)
                        //range = range.get_End(XlDirection.xlToRight);
                        // get the end of values toward the bottom, looking in the last column (will stop at first empty cell)
                        //range = range.get_End(XlDirection.xlDown);

                        // get the address of the bottom, right cell (last)
                        //string downAddress = range.get_Address(
                        //    false, false, XlReferenceStyle.xlA1,
                        //    Type.Missing, Type.Missing);

                        // Get the range, then values from a1
                        //range = sheet.get_Range("A1", downAddress);
                        range = sheet.UsedRange;
                        object[,] values = (object[,])range.Value2;

                        //counts number of columns in sheet
                        //columnsCount = sheet.UsedRange.Columns.Count;
                        sheet.Columns.ClearFormats();

                        columnsCount = sheet.UsedRange.Columns.Count;
                        rowsCount = sheet.UsedRange.Rows.Count;


                        fullRow = sheet.Rows.Count;
                        Lastrow = sheet.Cells[fullRow, 1].End(Excel.XlDirection.xlUp).Row;

                        // for each row
                        // skip first line, header
                        progressBar1.Value = 0; //Progress bar
                        progressBar1.Maximum = Lastrow - 1;
                        //progressBar1.Maximum = range.Rows.Count;

                        progressBar1.Step = 1;

                        int total = Lastrow - 1;

                        // check the excel file has correct format
                        // if not a correct formated excel
                        // show error message
                        if (!validExcelFormat(values))
                        {
                            MessageBox.Show("Please check the format of selected Excel file - consult with MTED's data coordinator");
                            label_status.Text = "Import failed. Please check the format of selected Excel file.";
                            oBook.Close(false, Type.Missing, Type.Missing);
                            oExcel.Quit();

                            return;
                        }

                        //adding Error Deatails column.
                        sheet.Cells[1, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                        sheet.Cells[1, columnsCount + 1] = "Error Details";
                        for (int i = 2; i <= Lastrow; i++)
                        {

                            progressBar1.PerformStep();
                            // progressBar1.Increment();

                            //check the value of status
                            if (null != values[i, statusIndex])
                            {

                                // get unit
                                string unit = "";
                                if (null != values[i, unitIndex])
                                    unit = values[i, unitIndex].ToString();
                                string unitID = "";
                                try
                                {
                                    unitID = getUnitID(unit.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, unitIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }
                                // first convert commodity name to id
                                string commodityName = "";
                                if (null != values[i, commoditySubCommodityIndex])
                                    commodityName = values[i, commoditySubCommodityIndex].ToString();
                                string commodityID = "";
                                try
                                {
                                    commodityID = getCommodityID(commodityName.Trim());
                                }
                                catch (Exception)
                                {

                                    sheet.Cells[i, commoditySubCommodityIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }




                                // get physical attr desc
                                string phyAttrDesc;
                                if (values[i, physicalAttributeIndex] == null)
                                    phyAttrDesc = "";
                                else if (values[i, physicalAttributeIndex] != null && values[i, physicalAttributeIndex].Equals("No Physical Attribute"))
                                    phyAttrDesc = "";
                                else
                                    phyAttrDesc = values[i, physicalAttributeIndex].ToString().Trim();

                                // get prod practice
                                string prodPrac = "";
                                if (null != values[i, prodPracticeIndex])
                                    prodPrac = values[i, prodPracticeIndex].ToString();
                                string prodPracID = "";
                                try
                                {
                                    prodPracID = getProdPracID(prodPrac.Trim());
                                }
                                catch (Exception)
                                {

                                    sheet.Cells[i, prodPracticeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }
                                // get util practice
                                string utilPrac = "";
                                if (null != values[i, utilPracticeIndex])
                                    utilPrac = values[i, utilPracticeIndex].ToString();
                                string utilPracID = "";
                                try
                                {
                                    utilPracID = getUtilpracID(utilPrac.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, utilPracticeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }
                                // get stat type
                                string statType = "";
                                if (null != values[i, statTypeIndex])
                                    statType = values[i, statTypeIndex].ToString();
                                string statTypeID = "";
                                try
                                {
                                    statTypeID = getStatTypeID(statType.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, statTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }

                                // get source
                                string sourceDesc = "";
                                string sourceLongDesc = "";
                                if (null != values[i, sourceDescriptionIndex])
                                    sourceDesc = values[i, sourceDescriptionIndex].ToString();
                                if (null != values[i, sourceLongDescriptionIndex])
                                    sourceLongDesc = values[i, sourceLongDescriptionIndex].ToString();
                                string sourceID = "";
                                try
                                {
                                    sourceID = getSourceID(sourceDesc.Trim(), sourceLongDesc.Trim());
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, sourceDescriptionIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                }



                                //      Console.WriteLine(statTypeID);

                                //      Console.WriteLine(unitID);
                                string dataSeriesID = "";
                                // get data series id
                                // be careful to the bool flag

                                dataSeriesID = getDataSeriesID(commodityID, phyAttrDesc, prodPracID, utilPracID, statTypeID, sourceID, insertNewDataSeriesFlag);
                                if (dataSeriesID == null || dataSeriesID == "")
                                {
                                    sheet.Cells[i, statTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, commoditySubCommodityIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, physicalAttributeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, prodPracticeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, utilPracticeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, sourceDescriptionIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Data Series not found";
                                    continue;
                                }


                                //  Console.WriteLine(dataSeriesID);

                                // get geo id
                                string geoType = "";
                                string geoValue = "";
                                if (null != values[i, geoTypeIndex])
                                    geoType = values[i, geoTypeIndex].ToString().Trim();
                                if (null != values[i, geoValueIndex])
                                    geoValue = values[i, geoValueIndex].ToString().Trim();
                                string geoID = "";
                                try
                                {
                                    if (dataSeriesID != null || dataSeriesID != "")
                                    {
                                        geoID = getGeoID(geoType, geoValue);
                                    }

                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, geoTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, geoValueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    continue;
                                }
                                // Console.WriteLine(geoID);
                                // get time id
                                string timeType = "";
                                string timeValue = "";
                                //Regex timechk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                                if (null != values[i, timeTypeIndex])
                                    timeType = values[i, timeTypeIndex].ToString().Trim();
                                if (null != values[i, timeValueIndex])
                                    timeValue = values[i, timeValueIndex].ToString().Trim();
                                string timeID = "";
                                try
                                {

                                    if (timeValue == "")
                                        throw new Exception();
                                    else
                                        timeID = getTimeID(timeType, timeValue);
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, timeTypeIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red); // marking of the new created excel sheet with errors on the C drive
                                    sheet.Cells[i, timeValueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    continue;
                                }


                                // get collection id
                                string collectionValue = "";
                                //Regex timechk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                                if (null != values[i, collectionIndex])
                                    collectionValue = values[i, collectionIndex].ToString().Trim();
                                string collectionID = "";
                                try
                                {

                                    if (collectionValue == "")
                                        throw new Exception();
                                    else
                                        collectionID = getCollectionID(collectionValue);
                                }
                                catch (Exception)
                                {
                                    sheet.Cells[i, collectionIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red); // marking of the new created excel sheet with errors                        
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found";
                                    continue;
                                }



                                //     Console.WriteLine(timeID);
                                // insert
                                string value = "";
                                if (null != values[i, valueIndex])
                                    value = values[i, valueIndex].ToString().Trim();

                                // verify value is valid
                                Regex numberchk = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
                                if (!value.Equals("") && !numberchk.IsMatch(value))
                                {
                                    sheet.Cells[i, valueIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                    sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                    sheet.Cells[i, columnsCount + 1] = "Values marked in RED color are not found and Value field is not a number.";
                                    continue;
                                }

                                try
                                {
                                    //waitForm.Show(this);
                                    string insertStatus = "";
                                    Boolean updateStatus = false;
                                    // the actual insert of data values in the database
                                    // if ((dataSeriesID != null || dataSeriesID != "") && (null != values[i, statusIndex] && values[i, statusIndex].Equals("New")))
                                    if ((dataSeriesID != null || dataSeriesID != "") && values[i, statusIndex].Equals("New"))
                                    {
                                        insertStatus = insertDataValues(dataSeriesID, timeID, unitID, geoID, sourceID, value, collectionID);
                                        if (insertStatus == "failed")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "Inserion Failed. Please contact technical team.";

                                        }
                                        else if (insertStatus == "success")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "Record inserted successfully.";
                                        }
                                        else if (insertStatus == "duplicate")
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "Insertion Failed. Cannot insert duplicate record.";
                                        }
                                    }

                                    //waitForm.Hide();
                                    // the actual update of data values in the database
                                    if ((dataSeriesID != null || dataSeriesID != "") && (null != values[i, statusIndex] && values[i, statusIndex].Equals("Override")))
                                    {
                                        updateStatus = updateDataValues(dataSeriesID, timeID, unitID, geoID, sourceID, value, collectionID);
                                        if (updateStatus == false)
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "Update Failed. Please contact technical team.";
                                        }
                                        else if (updateStatus == true)
                                        {
                                            for (int k = 1; k <= columnsCount; k++)
                                                sheet.Cells[i, k].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                                            sheet.Cells[i, columnsCount + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
                                            sheet.Cells[i, columnsCount + 1] = "Record updated successfully.";
                                        }
                                    }

                                    //waitForm.Hide();

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
                        if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Import Results")) // name of the error file and location
                        {
                            Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Import Results");
                        }

                        string fileName = cosdFolderPath + "CoSD Tool\\Import Results\\ImportResult_" + Date + ".xls";
                        try
                        {
                            oBook.SaveAs(fileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                            false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                            oBook.Close();
                            oExcel.Quit();

                            if (successNum == total)
                            {


                                if (comboBox_excelSheet.SelectedIndex != 1)
                                {
                                    label_message.Visible = true;
                                    label_message.Text = successNum + " of " + total + " records have been imported to the database.\r\n" + "Please check " + fileName + "\r\n" + "for details.";
                                }
                                else
                                {
                                    finalmessage = finalmessage + "Sheet " + j + " results: " + total + " record have been imported to the database.\r\n" + "Please check " + fileName + "\r\n" + "for details.\r\n";
                                }

                            }
                            else

                                // label_status.Text = "Sheet " + j + " results: " + successNum + " of " + total + " records have been imported. Please check " + fileName + " for any failed records.";
                                if (comboBox_excelSheet.SelectedIndex != 1)
                            {
                                label_message.Visible = true;
                                label_message.Text = successNum + " of " + total + " records have been imported.\r\n" + "Please check " + fileName + "\r\n" + "for any failed records.";
                            }
                            //  textBox_successmsg.Text = textBox_successmsg.Text + "Sheet " + j + " results: " + successNum + " of " + total + " records have been imported. Please check " + fileName + " for any failed records.\r\n";
                            else
                            {
                                finalmessage = finalmessage + "Sheet " + j + " results: " + successNum + " of " + total + " records have been imported.\r\n" + "Please check " + fileName + "\r\n" + "for any failed records.\r\n";
                            }

                            successNum = 0;
                            total = 0;

                            if (j == countcomboitems - 2)
                            {
                                label_status.Text = "Upload completed!";
                            }

                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show("Unable to create result excel file. Please contact technical team");
                            MessageBox.Show("Unable to create result excel file. Please contact technical team. Error Details:\n" + ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        label_status.Text = "Import Failed!";
                        //MessageBox.Show("Unable to create result excel file. Please contact technical team");
                        MessageBox.Show("Import Failed. Error Details:\n" + ex.Message);
                    }
                }

                if (comboBox_excelSheet.SelectedIndex == 1)
                {
                    textBox_successmsg.Visible = true;
                    textBox_successmsg.Text = finalmessage;
                }
            }
        }

        private bool validExcelFormat(object[,] values) // validating all the excel cells if they are in the correct format
        {
   

            try
            {
                Boolean valid = false;

                //get index of each column
                for (int i = 1; i <= columnsCount; i++)
                {
                    if (values[1, i].ToString().Equals("Time Type"))
                    {
                        timeTypeIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Time Value"))
                    {
                        timeValueIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Geo Type"))
                    {
                        geoTypeIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Geo Value"))
                    {
                        geoValueIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Value"))
                    {
                        valueIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Collection"))
                    {
                        collectionIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Status (New/Override)"))
                    {
                        statusIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Stat Type"))
                    {
                        statTypeIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Unit"))
                    {
                        unitIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("CommoditySubCommodity"))
                    {
                        commoditySubCommodityIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Physical Attribute"))
                    {
                        physicalAttributeIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Prod Practice"))
                    {
                        prodPracticeIndex = i;
                        valid = true;
                    }

                    else if (values[1, i].ToString().Equals("Util Practice"))
                    {
                        utilPracticeIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Source Description"))
                    {
                        sourceDescriptionIndex = i;
                        valid = true;
                    }
                    else if (values[1, i].ToString().Equals("Source Long Description"))
                    {
                        sourceLongDescriptionIndex = i;
                        valid = true;
                    }
                    else
                        valid = false;
                }

                if (!valid || timeTypeIndex == 0 || timeValueIndex == 0 || geoTypeIndex == 0 || geoValueIndex == 0 || valueIndex == 0 || collectionIndex == 0 || statusIndex == 0 || statTypeIndex == 0 || unitIndex == 0 || commoditySubCommodityIndex == 0 || physicalAttributeIndex == 0 || prodPracticeIndex == 0 || utilPracticeIndex == 0 || sourceDescriptionIndex == 0 || sourceLongDescriptionIndex == 0 || columnsCount == 0)
                    return false;

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// insert a new row to db
        /// </summary>
        /// <param name="dataSeriesID"> data series id </param>
        /// <param name="timeDeminsionID"> time id </param>
        /// <param name="unitID1"> unit id </param>
        /// <param name="statTypeID1"> stat type id </param>
        /// <param name="geoID"> geo id </param>
        /// <param name="inputValue"> value </param>
        private string insertDataValues(string dataSeriesID, string timeDeminsionID,
            string unitID1,
            string geoID,
            string sourceID,
            string inputValue,
            string collectionID)
        {

            string sql = "insert into " + schemaName +
    "ERSDataValues " +
    "(ERSDataValues_ERSSource_ID, " +
    "ERSDataValues_ERSOutput_ID, " +
    "ERSDataValues_ERSDataFeedType_ID, " +
    "ERSDataValues_ERSGeography_ID, " +
     "ERSDataValues_ERSCollection_ID, " +
     "ERSDataRowPrivacy_ID, " +
     "ERSDataValues_ERSUnit_ID, ERSDataValues_AttributeValue, " +
"ERSDataValues_ERSTimeDimension_ID, ERSDataValues_ERSCommodity_ID,ERSDataValues_DataRowLifecyclePhaseID ) " +
    "values " +
    "(" +
    sourceID + "," +
    "9" + "," +
     "5" + "," +
    geoID + "," +
    collectionID + "," +
    "1" + "," + unitID1 + ", '" + inputValue + "', " +
     timeDeminsionID + "," +
    dataSeriesID + "," + "9" +
    ")";

            // execute
            try
            {
                System.Data.DataTable dt = GetData(sql);
                //saveAction(sql);
                if (null != dt)
                {
                    successNum++;
                    saveAction("Data values inserted successfully");
                    return "success";
                }
                else if (errorMessage == "duplicate")
                {
                    errorMessage = "";
                    saveAction("Duplicate data values attempted to insert");
                    return "duplicate";
                }
                else
                    saveAction("Data values insertion failed");
                return "failed";

            }
            catch (SqlException ex)
            {
                //saveAction("Import excel action failed.");
                return "failed";

            }

            //         MessageBox.Show("Your input has been successfully uploaded to CoSD database.");
        }

        /// <summary>
        /// insert a new row to db
        /// </summary>
        /// <param name="dataSeriesID"> data series id </param>
        /// <param name="timeDeminsionID"> time id </param>
        /// <param name="unitID1"> unit id </param>
        /// <param name="statTypeID1"> stat type id </param>
        /// <param name="geoID"> geo id </param>
        /// <param name="inputValue"> value </param>
        private Boolean updateDataValues(string dataSeriesID, string timeDeminsionID,
            string unitID1,
            string geoID,
            string sourceID,
            string inputValue,
            string collectionID)
        {

            string sql = "UPDATE " + schemaName +
    "ERSDataValues SET ERSDataValues_AttributeValue = " + inputValue + " Where [ERSDataValues_ERSCommodity_ID] = " + dataSeriesID + " AND [ERSDataValues_ERSTimeDimension_ID] = " + timeDeminsionID +
    " AND [ERSDataValues_ERSUnit_ID] = " + unitID1 + " AND [ERSDataValues_ERSGeography_ID] = " + geoID + " AND [ERSDataValues_ERSSource_ID] = " + sourceID + " AND [ERSDataValues_ERSCollection_ID] = " + collectionID;

            // execute
            try
            {
                System.Data.DataTable dt = GetData(sql);
                saveAction("Data values updated successfully");
                if (null == dt)
                    return false;
                else
                {
                    // if everything works fine
                    // step 1
                    successNum++;
                    return true;
                }
            }
            catch (SqlException ex)
            {

                return false;
            }

        }
        /// <summary>
        /// load time dimention id with given year, month, week
        /// </summary>
        /// <param name="yearNum"></param>
        /// <param name="monthNum"></param>
        /// <param name="weekNum"></param>
        /// <returns></returns>
        private string getTimeID(string timeType, string timeValue)
        {
            string timeTypeID = "";

            //converting case
            timeType = timeType.ToUpper();
            try
            {
                /*Time type and value formats : 
                 *  Year: 2015
                    Month: Jan 2015
                    Day: 2/5/2015
                    Quarter: Q1 2015
                    Week: Wk28 2014
                 */
                if (timeType == "YEAR")
                {
                    timeType = "ERSCalendarYear";
                    timeValue = " ERSTimeDimension_Year = " + timeValue;
                }
                else if (timeType == "MARKETING YEAR")
                {
                    timeType = "ERSMarketingYear";
                    timeValue = " ERSTimeDimension_Year = " + timeValue;
                }
                else if (timeType == "MONTH")
                {
                    timeType = "ERSMonth";
                    double timeValueconverted = Convert.ToDouble(timeValue);
                    DateTime timevalueconv = DateTime.FromOADate(timeValueconverted);               //converting back excel serial number to date
                    timeValue = timevalueconv.ToString("MMM yyyy");
                    timeValue = timeValue.ToUpper();
                    if (timeValue.Contains("JAN"))
                        timeValue = " ERSTimeDimension_Month = 1 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("FEB"))
                        timeValue = " ERSTimeDimension_Month = 2 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("MAR"))
                        timeValue = " ERSTimeDimension_Month = 3 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("APR"))
                        timeValue = " ERSTimeDimension_Month = 4 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("MAY"))
                        timeValue = " ERSTimeDimension_Month = 5 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("JUN"))
                        timeValue = " ERSTimeDimension_Month = 6 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("JUL"))
                        timeValue = " ERSTimeDimension_Month = 7 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("AUG"))
                        timeValue = " ERSTimeDimension_Month = 8 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("SEP"))
                        timeValue = " ERSTimeDimension_Month = 9 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("OCT"))
                        timeValue = " ERSTimeDimension_Month = 10 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("NOV"))
                        timeValue = " ERSTimeDimension_Month = 11 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                    if (timeValue.Contains("DEC"))
                        timeValue = " ERSTimeDimension_Month = 12 and ERSTimeDimension_Year = " + timeValue.Split(' ')[1];
                }
                else if (timeType == "DAY")
                {
                    timeType = "ERSDayDate";
                    double timeValueconverted = Convert.ToDouble(timeValue);
                    DateTime timevalueconv = DateTime.FromOADate(timeValueconverted);
                    //  string timeval1 = timevalueconv.ToString().Substring(0, 10)
                    string timeval = timevalueconv.ToString();
                    var timeval1 = timeval.Split(new[] { ' ' }, 2);
                    string[] date = timeval1[0].Split('/');

                    // Converting date format (MM/DD/YYYY)12/01/1866 -->(YYYY-MM-DD)1866-12-01
                    if (date.Length == 3)
                        timeValue = date[2] + "-" + date[0] + "-" + date[1];
                    else
                        throw new Exception();

                    timeValue = " ERSTimeDimension_Date = '" + timeValue + "'";
                }
                else if (timeType == "QUARTER")
                {
                    timeType = "ERSCalendarYearQuarter";

                    string[] quarter = timeValue.Split(' ');

                    if (quarter.Length == 2)
                        timeValue = " ERSTimeDimension_Desc like '%" + quarter[1] + "%" + quarter[0] + "%'";
                    else
                        throw new Exception();
                }
                else if (timeType == "WEEK")                //Modified by gayatri on 10/11/2017
                {
                    timeType = "ERSWeek";
                    //string[] week = timeValue.Split('-');

                    //if (week.Length == 2 && week[0].Contains("WK"))
                    //{
                    //        week[0] = week[0].Remove(0,2);
                    //    string[] monthyear = week[1].Split('/');
                    //        timeValue = " ERSTimeDimension_Desc like '%Y: " + monthyear[1] + "%M: " + monthyear[0] + "%W: " + week[0] + "%'";                      
                    //}
                    //else
                    //    throw new Exception();

                    timeValue = " ERSTimeDimension_Desc like '%" + timeValue + "%'";

                }                                           ////////////
                else
                    throw new Exception();

                string sqltimeType = "select [ERSTimeDimensionType_ID] from " + schemaName + "[ERSTimeDimensionType_LU] where [ERSTimeDimensionType_Desc] = '" + timeType + "'";

                System.Data.DataTable dtType = GetData(sqltimeType);
                DataRow[] drType = dtType.Select();
                timeTypeID = drType[0]["ERSTimeDimensionType_ID"].ToString();

                string sql = "select ERSTimeDimension_ID from " + schemaName + "ERSTimeDimension_LU where  [ERSTimeDimension_TimeDimensionType_ID] = " + timeTypeID + " AND " + timeValue;

                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSTimeDimension_ID"].ToString();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }


        /*
        *****************Below code is commented intentioanlly*************************
        /// <summary>
        /// create a new entry of TimeDimension
        /// based on input
        /// if not exist
        /// </summary>
        private void insertIntoTimeDimension(string week, string month, string year, int timeType)
        {
            string sql = "";
            if (timeType.Equals(9))
            {
                sql = "insert into " + schemaName + "ERSTimeDimension_LU " + " (ERSTimeDimension_Week, ERSTimeDimension_Month, ERSTimeDimension_Year, ERSTimeDimension_TimeDimensionType_ID) values ( "

                    + "N'" + week + "', "
                     + "N'NULL', "
                     + "N'" + year + "', "
                    + timeType + ")";
            }

            else if (timeType.Equals(11))
            {
                sql = "insert into " + schemaName + "ERSTimeDimension_LU " + " (ERSTimeDimension_Week, ERSTimeDimension_Month, ERSTimeDimension_Year, ERSTimeDimension_TimeDimensionType_ID) values ( "
                      + "N'NULL', "
                    + "N'" + month + "', "
                      + "N'" + year + "', "
                     + timeType + ")";
            }
            else
            {
                sql = "insert into " + schemaName + "ERSTimeDimension_LU " + " (ERSTimeDimension_Week, ERSTimeDimension_Month, ERSTimeDimension_Year, ERSTimeDimension_TimeDimensionType_ID) values ( "
                      + "N'NULL', "
                      + "N'NULL', "
                     + "N'" + year + "', "
                     + timeType + ")";
            }

            // insert new timeDimension
            GetData(sql);
            saveAction(sql);
            //newTimeDimension = true;

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"Insert INTO " + schemaName + "[ERSTimeDimension_LU] (ERSTimeDimension_Month, ERSTimeDimension_Year, ERSTimeDimension_TimeDimensionType_ID) VALUES (@month, @year, @timeType)", con);
                if (month == "")
                {
                    cmd.Parameters.Add("@month", SqlDbType.NVarChar, 160).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@month", SqlDbType.NVarChar, 160).Value = month;
                }
                if (year == "")
                {
                    cmd.Parameters.Add("@year", SqlDbType.NVarChar, 160).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@region", SqlDbType.NVarChar, 160).Value = year;
                }

                cmd.Parameters.Add("@timeType", SqlDbType.Int).Value = timeType;

                int rows = cmd.ExecuteNonQuery();

                sql = "insert into " + schemaName + "ERSTimeDimension_LU " + " (ERSTimeDimension_Week, ERSTimeDimension_Month, ERSTimeDimension_Year, ERSTimeDimension_TimeDimensionType_ID) values ( "

                    + "N'" + week + "', "
                    + "N'" + month + "', "
                     + "N'" + year + "', "
                    + timeType + ")";

                // save action log for new geoDimension
                saveAction(sql);
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

        */
        /// <summary>
        /// get geo id with given country, region, state, county
        /// </summary>
        /// <param name="country"></param>
        /// <param name="region"></param>
        /// <param name="state"></param>
        /// <param name="county"></param>
        /// <returns></returns>
        private string getGeoID(string geoType, string geoValue)
        {
            // 
            string geoTypeID = "";

            string sqlgeoType = "select [ERSGeographyType_ID] from " + schemaName + "[ERSGeographyType_LU] where [ERSGeographyType_Desc] = '" + geoType + "'";

            try
            {
                System.Data.DataTable dtType = GetData(sqlgeoType);
                DataRow[] drType = dtType.Select();
                geoTypeID = drType[0]["ERSGeographyType_ID"].ToString();

                string allGeoValues = geoValue;
                string[] geoValueSeprated = allGeoValues.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


                if (geoValueSeprated.Length == 1)
                    geoValue = "AND [ERSGeographyDimension_Country] = '" + geoValueSeprated[0].Trim() + "'";
                else if (geoValueSeprated.Length == 2 & geoTypeID == "6")
                    geoValue = "AND [ERSGeographyDimension_Country] = '" + geoValueSeprated[0].Trim() + "' AND [ERSGeographyDimension_Region] = '" + geoValueSeprated[1].Trim() + "'";


                else if (geoValueSeprated.Length == 2 & geoTypeID == "3")
                    geoValue = "AND [ERSGeographyDimension_Country] = '" + geoValueSeprated[0].Trim() + "' AND [ERSGeographyDimension_State] = '" + geoValueSeprated[1].Trim() + "'";



                else if (geoValueSeprated.Length == 3 & geoTypeID == "4")
                    geoValue = "AND [ERSGeographyDimension_Country] = '" + geoValueSeprated[0].Trim() + "' AND [ERSGeographyDimension_State] = '" + geoValueSeprated[1].Trim() + "' and [ERSGeographyDimension_County] = '" + geoValueSeprated[2].Trim() + "'";

                else if (geoValueSeprated.Length == 3 & geoTypeID == "5")
                    geoValue = "AND [ERSGeographyDimension_Country] = '" + geoValueSeprated[0].Trim() + "' AND [ERSGeographyDimension_State] = '" + geoValueSeprated[1].Trim() + "' and [ERSGeographyDimension_City] = '" + geoValueSeprated[2].Trim() + "'";

                else if (geoValueSeprated.Length == 4)
                    geoValue = "AND [ERSGeographyDimension_Country] = '" + geoValueSeprated[0].Trim() + "' AND [ERSGeographyDimension_State] = '" + geoValueSeprated[1].Trim() + "' and [ERSGeographyDimension_County] = '" + geoValueSeprated[2].Trim() + "' and [ERSGeographyDimension_Region] = '" + geoValueSeprated[3].Trim() + "'";
                else if (geoValueSeprated.Length == 5 && geoValueSeprated[3].Equals("NULL"))
                    geoValue = "AND [ERSGeographyDimension_Country] = '" + geoValueSeprated[0].Trim() + "' AND [ERSGeographyDimension_State] = '" + geoValueSeprated[1].Trim() + "' and [ERSGeographyDimension_County] = '" + geoValueSeprated[2].Trim() + "' and [ERSGeographyDimension_Region] IS NULL and [ERSGeographyDimension_City] = '" + geoValueSeprated[4].Trim() + "'";
                else if (geoValueSeprated.Length == 5)
                    geoValue = "AND [ERSGeographyDimension_Country] = '" + geoValueSeprated[0].Trim() + "' AND [ERSGeographyDimension_State] = '" + geoValueSeprated[1].Trim() + "' and [ERSGeographyDimension_County] = '" + geoValueSeprated[2].Trim() + "' and [ERSGeographyDimension_Region] = '" + geoValueSeprated[3].Trim() + "' and [ERSGeographyDimension_City] = '" + geoValueSeprated[4].Trim() + "'";



                string sql = "select ERSGeographyDimension_ID from " + schemaName + "ERSGeographyDimension_LU where [ERSGeographyDimension_ERSGeographyType_ID] = '" + geoTypeID + "' " + geoValue;

                System.Data.DataTable dtID = GetData(sql);
                DataRow[] drID = dtID.Select();
                return drID[0]["ERSGeographyDimension_ID"].ToString();
            }
            catch (Exception)      // if no existing geo id then insert one
            {
                throw;
            }
        }


        /// <summary>
        /// get collectio id 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="region"></param>
        /// <param name="state"></param>
        /// <param name="county"></param>
        /// <returns></returns>
        private string getCollectionID(string collectionValue)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!
            try
            {
                string sql = "select ERSCollection_ID from " + schemaName + "ERSCollection_LU where ERSCollection_Desc = '" + collectionValue + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSCollection_ID"].ToString();
            }
            catch (Exception)      // if no existing geo id then insert one
            {
                throw;
            }
        }
        /*
        ****************Below code has been commented intentionally ******************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <param name="region"></param>
        /// <param name="state"></param>
        /// <param name="county"></param>
        /// <param name="geoType"></param>
        private void insertIntoGeoDimension(string country, string region, string state, string county, int geoType, string geodesc)
        {

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"Insert INTO " + schemaName + "[ERSGeographyDimension_LU] (ERSGeographyDimension_Country, ERSGeographyDimension_Region, ERSGeographyDimension_State, ERSGeographyDimension_County,ERSGeographyDimension_City, ERSGeographyDimension_ERSGeographyType_ID, ERSGeographyDimension_Desc) VALUES (@country, @region, @state, @county, @city, @geoType, @geodesc)", con);
                if (country == "")
                {
                    cmd.Parameters.Add("@country", SqlDbType.NVarChar, 160).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@country", SqlDbType.NVarChar, 160).Value = country;
                }
                if (region == "")
                {
                    cmd.Parameters.Add("@region", SqlDbType.NVarChar, 160).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@region", SqlDbType.NVarChar, 160).Value = region;
                }
                if (state == "")
                {
                    cmd.Parameters.Add("@state", SqlDbType.NVarChar, 160).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@state", SqlDbType.NVarChar, 160).Value = state;
                }
                if (county == "")
                {
                    cmd.Parameters.Add("@county", SqlDbType.NVarChar, 160).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@county", SqlDbType.NVarChar, 160).Value = county;
                }

                cmd.Parameters.Add("@city", SqlDbType.NVarChar, 160).Value = DBNull.Value;

                cmd.Parameters.Add("@geoType", SqlDbType.Int).Value = geoType;

                cmd.Parameters.Add("@geodesc", SqlDbType.NVarChar, 160).Value = geodesc;

                int rows = cmd.ExecuteNonQuery();

                string sql = "insert into " + schemaName + "ERSGeographyDimension_LU " +
                " (ERSGeographyDimension_Country, ERSGeographyDimension_Region, ERSGeographyDimension_State, ERSGeographyDimension_County,ERSGeographyDimension_City, ERSGeographyDimension_ERSGeographyType_ID) values ( "
                + "N'" + country + "', "
                + "N'" + region + "', "
                + "N'" + state + "', "
                + "N'" + county + "', "
                + "N'NULL', "
                + geoType + ")";

            // save action log for new geoDimension
            saveAction(sql);
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
        */
        // load dataseriesID from dataseries table
        private string getDataSeriesID(string commoditySubCommodityID, string physicalAttributeDetail, string prodPracticeID, string utilPracticeID, string statTypeID, string sourceID, bool insertNew)
        {

            if (physicalAttributeDetail == "")
            {
                physicalAttributeDetail = " IN ('','No Physical Attribute')";
            }
            else
            {
                physicalAttributeDetail = " IN ('" + physicalAttributeDetail + "')";
            }

            try
            {
                string sql = "select ERSCommodity_ID from " + schemaName + "ERSCommodityDataSeries where " +
                    " ERSCommoditySubCommodity_ID = " + commoditySubCommodityID +
                      " and ERSCommodity_PhysicalAttribute_Desc " + physicalAttributeDetail +
                      " and ERSCommodity_ERSProdPractice_ID = " + prodPracticeID +
                      " and ERSCommodity_ERSStatisticType_ID = " + statTypeID +
                      " and ERSCommodity_ERSUtilPractice_ID = " + utilPracticeID +
                      " and ERSCommodity_ERSSource_ID = " + sourceID;

                string dataSeriesID;
                System.Data.DataTable result = GetData(sql);

                //data series not found
                if (result == null || result.Rows.Count == 0)
                    return "";

                DataRow[] rows = result.Select();
                Console.WriteLine(rows.Length);
                for (int i = 0; i < rows.Length; i++)
                {
                    Console.WriteLine(rows[i]["ERSCommodity_ID"]);
                };
                dataSeriesID = (rows[0]["ERSCommodity_ID"]).ToString();

                return dataSeriesID;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// create a new entry of dataseries table
        /// based on input
        /// </summary>
        private void insertIntoDataSeries(string commodityID, string phyAttrDesc, string prodPracID, string utilPracID, string statTypeID)
        {

            //       string sql = "insert into " + schemaName + "ERSCommodityDataSeries (ERSCommoditySubCommodity_ID,ERSCommodity_ERSSector_ID,ERSCommodity_ERSGroup_ID, ERSCommodity_ERSPhysicalAttribute_ID," +
            //           "ERSCommodity_PhysicalAttribute_Desc,ERSCommodity_ERSProdPractice_ID,ERSCommodity_ERSUtilPractice_ID,ERSCommodity_ERSHS_ID,ERSCommodity_HS10,ERSCommodity_HS10_LongDesc,ERSCommodity_ERSTradeImEx_ID) values (" +
            //commodityID +
            //", " + "2" +
            //", " + "9" +
            //", " + physicalAttributeTypeID +
            // ", " + physicalAttributeDetail +
            // ", " + prodPracticeID +
            //", " + utilPracticeID +
            //", " + "1" +
            // ", " + "N'NULL'" +
            //  ", " + "N'NULL'" +
            //   ", " + "4" +
            // ")";
            //       Console.WriteLine(sql);
            //       GetData(sql);  // execute insert sql command
            //       saveAction(sql);
            //       newDataSeries = true;
        }
        /// <summary>
        /// load unit id with unit name
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private string getUnitID(string unit)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!
            try
            {
                string sql = "select ERSUnit_ID from " + schemaName + "ERSUnit_LU where ERSUnit_Desc = '" + unit + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSUnit_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// load stat type id
        /// </summary>
        /// <param name="statType"></param>
        /// <returns></returns>
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
        /// <summary>
        /// load production prac id
        /// </summary>
        /// <param name="prodPrac"></param>
        /// <returns></returns>
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
        /// <summary>
        /// load physical attribute id
        /// </summary>
        /// <param name="phyAttrCat"></param>
        /// <returns></returns>
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
        /// <summary>
        /// load commodity id
        /// </summary>
        /// <param name="commodityName"></param>
        /// <returns></returns>
        private string getCommodityID(string commodityName)
        {
            try
            {
                string sql = "select ERSCommoditySubCommodity_ID from " + schemaName + "ERSCommoditySubCommodity_LU where ERSCommoditySubCommodity_Desc = '" + commodityName + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSCommoditySubCommodity_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// load source id
        /// </summary>
        /// <param name="commodityName"></param>
        /// <returns></returns>
        private string getSourceID(string sourceDesc, string sourceLongDesc)
        {
            try
            {
                string sql = "select [ERSSource_ID] from " + schemaName + "[ERSSource_LU] where [ERSSource_Desc] = '" + sourceDesc + "'  AND [ERSSource_LongDesc] = '" + sourceLongDesc + "'";
                System.Data.DataTable dt = GetData(sql);
                DataRow[] dr = dt.Select();
                return dr[0]["ERSSource_ID"].ToString();
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// go back action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_back_Click(object sender, EventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Close();
            this.Parent = null;
        }

        // record user action in user log table
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


        // method connects db
        /// <summary>
        /// Connections this instance.
        /// </summary>
        private void connection()
        {
            // connection string for linking db
            sqlconn = ConfigurationManager.ConnectionStrings["CoSD_Tool.Properties.Settings.AP_ToolCoSDConnectionString"].ConnectionString;
            // init the connection to db
            con = new SqlConnection(sqlconn);
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="selectCommand">The select command.</param>
        private System.Data.DataTable GetData(string selectCommand)
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
                System.Data.DataTable result = new System.Data.DataTable();
                dataAdapter.Fill(result);
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
        //----- OLD DISCARD BUTTON---- NOT IN USE NOW

        /// <summary>
        /// discard button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_discardImport_Click(object sender, EventArgs e)
        {
            //Hiding the window, because closing it makes the window unaccessible.
            this.Hide();
            this.Parent = null;
            //   e.Cancel = true; //hides the form, cancels closing event
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
            }
            return group.ToString();
        }

        //get group ID
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
            StringBuilder group = new StringBuilder();
            if (groupID.Count != 0)
            {
                string lastItem = groupID[groupID.Count - 1];
                foreach (string item in groupID)
                {
                    if (item != lastItem)
                        group.Append("'" + item + "',");
                    else group.Append("'" + item + "'");
                }
            }
            return group.ToString();
        }

        private void disable_buttons()
        {
            button_excelPath.Enabled = false;
            button_back.Enabled = false;
            button_ChangePath.Enabled = false;
            button_back.Enabled = false;
            button_importExcel.Enabled = false;
            businessrulesButton.Enabled = false;
            dataseriesButton.Enabled = false;
        }
        private void enable_buttons()
        {
            button_excelPath.Enabled = true;
            button_back.Enabled = true;
            button_ChangePath.Enabled = true;
            button_back.Enabled = true;
            button_importExcel.Enabled = true;
            businessrulesButton.Enabled = true;
            dataseriesButton.Enabled = true;
        }

        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void dataseries_DoWork(object sender, DoWorkEventArgs e)
        {
            if (MessageBox.Show("This process may take more than 2 minutes. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");
                this.Invoke(new MethodInvoker(delegate {
                    disable_buttons();

                }));

                DataSet ds = new DataSet();
                string groupValues = getGroupDesc();
                backgroundWorker.ReportProgress(1);
                try
                {
                    connection();
                    con.Open();

                    SqlDataAdapter SDA1 = new SqlDataAdapter(@"SELECT * FROM " + schemaName + "[DataSeries_DistinctComplete] WHERE [ERSGroup_Desc] IN (" + groupValues + ")", con);

                    //SDA1.SelectCommand.CommandTimeout = 300;

                    SDA1.SelectCommand.ExecuteNonQuery();
                    DT = new System.Data.DataTable();
                    SDA1.Fill(ds);
                    ds.Tables[0].TableName = "DataSeries";

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

                if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Export Results")) // name of the error file and location
                {
                    Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Export Results");
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
                    xlWorkBook.SaveAs(cosdFolderPath + "CoSD Tool\\Export Results\\DataSeries" + "_" + Date + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                    string filePath = @cosdFolderPath + "CoSD Tool\\Export Results\\DataSeries" + "_" + Date + ".xls";
                    xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                    foreach (System.Data.DataTable table in ds.Tables)
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
                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("There was a problem in creating excel file. Please contact the technical team");
                    return;
                }

                this.Invoke(new MethodInvoker(delegate {
                    enable_buttons();

                }));

                backgroundWorker.ReportProgress(2);

            }
        }


        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void dataseries_ProgressChanged(object sender, ProgressChangedEventArgs e)
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
                exportResultLabel.Text = cosdFolderPath + "CoSD Tool\\Export Results\\DataSeries" + "_" + Date + ".xls";
                waitForm.Hide();
                MessageBox.Show("Data Series Excel file created Successfully");

            }
        }

        private void dataseriesButton_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();
            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(dataseries_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(dataseries_ProgressChanged);

            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();
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

        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void datavalues_DoWork(object sender, DoWorkEventArgs e)
        {
            if (MessageBox.Show("This process will take more than 10 minutes. You cannot use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");
                DataSet ds = new DataSet();
                string groupValues = getGroupDesc();
                backgroundWorker.ReportProgress(1);
                try
                {
                    connection();
                    con.Open();
                    SqlDataAdapter SDA1 = new SqlDataAdapter(@"SELECT * FROM " + schemaName + "[AP_Taxonomy_withValues]  WHERE [ERSGroup_Desc] IN (" + groupValues + ")", con);

                    SDA1.SelectCommand.CommandTimeout = 600;

                    SDA1.SelectCommand.ExecuteNonQuery();
                    DT = new System.Data.DataTable();
                    SDA1.Fill(ds);
                    ds.Tables[0].TableName = "DataValues";

                }
                catch (Exception)
                {

                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("Data processing unsuccessful, please contact technical team.");
                    return;
                }
                finally
                {
                    con.Close();
                }

                if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Export Results")) // name of the error file and location
                {
                    Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Export Results");
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
                    xlWorkBook.SaveAs(cosdFolderPath + "CoSD Tool\\Export Results\\DataValues" + "_" + Date + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                    string filePath = @cosdFolderPath + "CoSD Tool\\Export Results\\DataValues" + "_" + Date + ".xls";
                    xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                    foreach (System.Data.DataTable table in ds.Tables)
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
                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("There was a problem in creating excel file. Please contact the technical team");
                    return;
                }

                //success
                backgroundWorker.ReportProgress(2);

            }
        }


        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void datavalues_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                waitForm.Show(this);
            }
            if (e.ProgressPercentage == 0)
            {
                waitForm.Hide();

            }
            if (e.ProgressPercentage == 2)
            {
                exportResultLabel.Text = cosdFolderPath + "CoSD Tool\\Export Results\\DataValues" + "_" + Date + ".xls";
                waitForm.Hide();
                MessageBox.Show("Data Values Excel file created Successfully");

            }
        }


        private void datavaluesButton_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();
            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(datavalues_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(datavalues_ProgressChanged);

            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();

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
                        //to make the type and value order same
                        geoType = new StringBuilder(blInputGeo_id);
                        System.Data.DataTable CommaInputValue = GetData(sql_CommaInputGeoValue);
                        if (null != CommaInputValue && CommaInputValue.Rows.Count != 0)
                        {
                            DataRow[] dr = CommaInputValue.Select();

                            for (int i = 0; i < dr.Length; i++)
                            {
                                blInputGeo_id = blInputGeo_id.Replace(dr[i].ItemArray[0].ToString(), dr[i].ItemArray[2].ToString());
                                geoType.Replace(dr[i].ItemArray[0].ToString(), dr[i].ItemArray[1].ToString());
                                //row.SetField("Input Geo Type", dr[i].ItemArray[1].ToString());
                            }

                        }
                        row.SetField("Input Geo Value", blInputGeo_id);
                        row.SetField("Input Geo Type", geoType.ToString());

                    }
                    allInputGeoValue = null;

                    //Filling Status (New/Override)
                    row.SetField("Status (New/Override)", "Override");

                    //Prepare Commodity Series ID
                    string commoditySeriesID = row["Commodity Series ID"].ToString();
                    string[] inputsArray = { };
                    inputsArray = commoditySeriesID.Replace(" ", string.Empty).Split(';');
                    string outputDestination = row["Output Destination"].ToString();

                    //get thousand number
                    int thousandNumber = getThoudandNumber(outputDestination);

                    //Set output destination
                    outputDestination = getOutputDestination(outputDestination, thousandNumber);
                    row.SetField("Output Destination", outputDestination);

                    //get formula                    
                    string formula = row["Formula"].ToString();
                    formula = getFormula(formula, inputsArray, thousandNumber);
                    row.SetField("Formula", formula);

                    //prepare Commodity Series ID

                    commoditySeriesID = getCommoditySeriesID(commoditySeriesID, inputsArray, thousandNumber);
                    row.SetField("Commodity Series ID", commoditySeriesID);


                    //prepare Input TimeDimensionType
                    string inputTimeDimensionType = row["Input Time Dimension Type"].ToString();
                    inputTimeDimensionType = getTimeDimensionType(inputTimeDimensionType);
                    row.SetField("Input Time Dimension Type", inputTimeDimensionType);

                    //prepare Output TimeDimensionType
                    string outputTimeDimensionType = row["Output Time Dimension Type"].ToString();
                    outputTimeDimensionType = getTimeDimensionType(outputTimeDimensionType);
                    row.SetField("Output Time Dimension Type", outputTimeDimensionType);

                }
                return ds;
            }
            catch (Exception ex)
            {
                //throw new Exception("Error in Geography Description"); 
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
                        select x.Field<int>("Import/Export Rule ID")).ToList();


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
                        string bl_id = row["Import/Export Rule ID"].ToString();
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
        private string getTimeDimensionType(string timeDimensionType)
        {
            // need to take care if not found !!!!!!!!!!!!!!!!!!            
            try
            {
                timeDimensionType = timeDimensionType.Replace("ERS", "");

                //if (timeDimensionType.Equals(("ERSMonth"), StringComparison.OrdinalIgnoreCase))
                //{
                //    timeDimensionType = "Month";
                //}
                if (timeDimensionType.Equals(("CalendarYear")))
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
        private string getFormula(string formula, string[] inputsArray, int thousandNumber)
        {
            string newFormula = formula;
            int newThousandNumber = 0;
            int thousandNumber_N = 0;
            string sourceSeriesID = "";
            newSeriesIds = new Dictionary<string, string> { };
            List<string> commoditySeriesIds = new List<string>();

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
                                commoditySeriesIds.Add(drcommodity_Stat[0]["ERSStatisticType_Attribute"].ToString());

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

                                //newFormula = newFormula.Replace("(","");
                                //newFormula = newFormula.Replace(")", "");
                            }
                            catch (Exception)
                            {
                                //throw new Exception("Data series " + inputsArray[counter] + " not found.");
                                continue;
                            }

                        }
                    }
                }

                //replace stat types to empty in formula
                if (commoditySeriesIds.Count > 0)
                {
                    commoditySeriesIds.Sort((x, y) => y.Length - x.Length);

                    foreach (string item in commoditySeriesIds)
                    {
                        newFormula = newFormula.Replace(item, "");
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
        private string getCommoditySeriesID(string commoditySeriesID, string[] inputsArray, int thousandNumber)
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
        private int getThoudandNumber(string outputDestination)
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
        private string getOutputDestination(string outputDestination, int thousandNumber)
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
                    return outputDestination;
            }
            catch (Exception)
            {
                return outputDestination;
            }
        }
        /// <summary>
        /// Time consuming stored procedure execution
        /// </summary>
        private void businessrules_DoWork(object sender, DoWorkEventArgs e)
        {
            if (MessageBox.Show("This process may take more than 1 minute. Please do not use the tool until processing finishes.\n\nDo you want to continue?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                String Date = DateTime.Now.ToString("MM.dd.yyy_HH.mm.ss");

                this.Invoke(new MethodInvoker(delegate {
                    disable_buttons();

                }));

                DataSet ds = new DataSet();
                string groupValues = getGroupID();
                if (null == groupValues || groupValues.Equals(""))
                {
                    groupValues = "0";
                }
                backgroundWorker.ReportProgress(1);
                try
                {
                    connection();
                    con.Open();

                    //sql for printing
                    string sql = "SELECT ERSBusinessLogic_ID AS 'Import/Export Rule ID' "
+ " ,REPLACE ( [ERSBusinessLogic_InputDataSeries] , ',' , ' ;' ) AS 'Commodity Series ID' "
+ " ,InputUnit.[ERSUnit_Desc] AS 'Input Unit' "
+ " ,[ERSConversionFactor_CF] AS 'Conversion Factor' "
+ " ,REPLACE ( [ERSBusinessLogic_MacroDesc] , 'Null' , '' ) AS 'Macro' "
+ " ,[ERSBusinessLogic_OutputName] AS 'Output Name' "
+ " ,CONVERT(varchar(100), OutputUnit.[ERSUnit_Desc]) AS 'Output Unit' "
+ " ,[ERSBusinessLogic_Formula] 'Formula' "
+ " ,TimeInput.[ERSTimeDimensionType_Desc] AS 'Input Time Dimension Type' "
+ " ,[ERSBusinessLogic_InputTimeDimensionValue] 'Input Time Value' "
+ " ,TimeOutput.[ERSTimeDimensionType_Desc] AS 'Output Time Dimension Type' "
+ " ,[ERSBusinessLogic_OutputTimeDimensionValue] AS 'Output Time Value' "
+ " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(InputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Input Geo Type' "
+ " ,REPLACE ( [ERSBusinessLogic_InputGeographyDimensionID] , ',' , ' ;' ) AS 'Input Geo Value' "
+ " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(OutputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Output Geo Type'"
+ " ,CONVERT(varchar(100), [ERSBusinessLogic_OutputGeographyDimensionID]) AS 'Output Geo Value' "
+ " ,[ERSBusinessLogic_OutputDestination] AS 'Output Destination' "
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
+ " Where [ERSBusinessLogic_GroupID] IN (" + groupValues + ") ORDER BY ERSBusinessLogic_SequenceID ";

                    SqlDataAdapter SDA1 = new SqlDataAdapter(@"SELECT ERSBusinessLogic_ID AS 'Import/Export Rule ID' "
+ " ,REPLACE ( [ERSBusinessLogic_InputDataSeries] , ',' , ' ;' ) AS 'Commodity Series ID' "
+ " ,InputUnit.[ERSUnit_Desc] AS 'Input Unit' "
+ " ,[ERSConversionFactor_CF] AS 'Conversion Factor' "
+ " ,REPLACE ( [ERSBusinessLogic_MacroDesc] , 'NULL' , '' ) AS 'Macro' "
+ " ,[ERSBusinessLogic_OutputName] AS 'Output Name' "
+ " ,CONVERT(varchar(100), OutputUnit.[ERSUnit_Desc]) AS 'Output Unit' "
+ " ,[ERSBusinessLogic_Formula] 'Formula' "
+ " ,TimeInput.[ERSTimeDimensionType_Desc] AS 'Input Time Dimension Type' "
+ " ,[ERSBusinessLogic_InputTimeDimensionValue] 'Input Time Value' "
+ " ,TimeOutput.[ERSTimeDimensionType_Desc] AS 'Output Time Dimension Type' "
+ " ,[ERSBusinessLogic_OutputTimeDimensionValue] AS 'Output Time Value' "
+ " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(InputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Input Geo Type' "
+ " ,REPLACE ( [ERSBusinessLogic_InputGeographyDimensionID] , ',' , ' ,' ) AS 'Input Geo Value' "
+ " ,(SELECT [ERSGeographyType_Desc] FROM " + schemaName + "[ERSGeographyType_LU] WHERE ERSGeographyType_ID IN(OutputGeo.ERSGeographyDimension_ERSGeographyType_ID)) AS 'Output Geo Type'"
+ " ,CONVERT(varchar(100), [ERSBusinessLogic_OutputGeographyDimensionID]) AS 'Output Geo Value' "
+ " ,[ERSBusinessLogic_OutputDestination] AS 'Output Destination' "
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
+ " Where [ERSBusinessLogic_GroupID] IN (" + groupValues + ") ORDER BY ERSBusinessLogic_SequenceID ", con);
                    SDA1.SelectCommand.CommandTimeout = 600;
                    SDA1.SelectCommand.ExecuteNonQuery();
                    DT = new System.Data.DataTable();

                    //Adding Columns to import
                    //System.Data.DataTable importTable = new System.Data.DataTable("Business Rules Import");

                    //importTable.ImportRow(ds.Tables["Business Rules Export"].Rows[0]);
                    //ds.Tables.Add(importTable);

                    SDA1.Fill(ds);
                    ds.Tables[0].TableName = "Business Rules Export";
                    //Adding Columns
                    ds.Tables["Business Rules Export"].Columns.Add("Status (New/Override)", typeof(string)).SetOrdinal(0);

                    con.Close();

                    if (ds.Tables["Business Rules Export"].Rows.Count > 0)
                    {
                        //Prepare Unit column
                        ds = prepareUnit(ds);

                        //Prepare rest of the fields.
                        ds = prepareOtherColumns(ds);
                    }


                }
                catch (Exception ex)
                {

                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("Data processing unsuccessful, please contact technical team." + ex.Message);
                    return;
                }
                finally
                {
                    con.Close();
                }

                if (!Directory.Exists(cosdFolderPath + "CoSD Tool\\Export Results")) // name of the error file and location
                {
                    Directory.CreateDirectory(cosdFolderPath + "CoSD Tool\\Export Results");
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
                    Range range = null;
                    object misValue = System.Reflection.Missing.Value;
                    xlWorkBook = xlApp.Workbooks.Add(misValue);
                    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    xlWorkBook.SaveAs(cosdFolderPath + "CoSD Tool\\Export Results\\BusinessRules" + "_" + Date + ".xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                    string filePath = @cosdFolderPath + "CoSD Tool\\Export Results\\BusinessRules" + "_" + Date + ".xls";
                    xlWorkBook = xlApp.Workbooks.Open(filePath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, false, false);
                    foreach (System.Data.DataTable table in ds.Tables)
                    {

                        Excel.Worksheet excelWorkSheet = xlWorkBook.Sheets.Add();
                        excelWorkSheet.Name = table.TableName;

                        range = excelWorkSheet.UsedRange;
                        range.NumberFormat = "@";
                        for (int i = 1; i < table.Columns.Count + 1; i++)
                        {
                            excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                            Excel.Range formatRange;
                            formatRange = excelWorkSheet.get_Range("a1");
                            formatRange.EntireRow.Font.Bold = true;
                            //formatRange.EntireColumn.NumberFormat = "@";
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
                    backgroundWorker.ReportProgress(0);
                    MessageBox.Show("There was a problem in creating excel file. Please contact the technical team");
                    return;
                }

                //success
                this.Invoke(new MethodInvoker(delegate {
                    enable_buttons();

                }));
                backgroundWorker.ReportProgress(2);
          

            }
        }


        /// <summary>
        /// Shows and hides the wait form based on progress report.
        /// </summary>
        private void businessrules_ProgressChanged(object sender, ProgressChangedEventArgs e)
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
                exportResultLabel.Text = cosdFolderPath + "CoSD Tool\\Export Results\\BusinessRules" + "_" + Date + ".xls";
                waitForm.Hide();
                MessageBox.Show("Business Rules Excel file created Successfully");

            }
        }

        private void businessrulesButton_Click(object sender, EventArgs e)
        {
            // Background worker thread that executed stored procedures on sepreate thread to increase performance.
            backgroundWorker = new BackgroundWorker();
            //Method call to execute SP
            backgroundWorker.DoWork += new DoWorkEventHandler(businessrules_DoWork);
            //Method call to track progress and wait form
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(businessrules_ProgressChanged);

            backgroundWorker.WorkerReportsProgress = true;

            backgroundWorker.RunWorkerAsync();
        }

        private void ImportExport_Load(object sender, EventArgs e)
        {
            if (DeSerializeSettingsPath<String>("CoSDSettings") != null)
                cosdFolderPath = DeSerializeSettingsPath<String>("CoSDSettings");

            if (!cosdFolderPath.EndsWith("\\"))
            {
                cosdFolderPath = cosdFolderPath + "\\";
            }
            label_ImportResultPath.Text = cosdFolderPath + "CoSD Tool" + "\\Import Results";
            exportResultLabel.Text = cosdFolderPath + "CoSD Tool" + "\\Export Results";
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
                    label_ImportResultPath.Text = cosdFolderPath + "CoSD Tool" + "\\Import Results";
                    exportResultLabel.Text = cosdFolderPath + "CoSD Tool" + "\\Export Results";
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

        private void label_message_Click(object sender, EventArgs e)
        {

        }

        private void exportPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void importPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox_successmsg_TextChanged(object sender, EventArgs e)
        {

        }

        private void label_message_Click_1(object sender, EventArgs e)
        {

        }

        private void button_ChangePathImport_Click(object sender, EventArgs e)
        {
            button_ChangePath_Click(sender, e);
        }

        private void label_ImportResultPath_Click(object sender, EventArgs e)
        {

        }

    }
}
