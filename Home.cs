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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoSD_Tool
{
    public partial class Home : Form
    {
        // global variable defines DB name and Schema name that the tool connects
        // note that: the database needs to exactly match the CoSD design (tables and columns)
        //public static string globalSchemaName = "AP_ToolCoSD.CoSD.";
        public static string globalSchemaName = LandingScreen.GetSchemaName(LandingScreen.newschemaName);
        // global variable defines DB name and Schema name that the tool connects
        // note that: the database needs to exactly match the CoSD design (tables and columns)
        //public static string globalDBOSchemaName = "AP_ToolCoSD.dbo.";
        public static string globalDBOSchemaName = "AnimalProductsCoSD.dbo.";

        public static string globalapGroupId = LandingScreen.GetGroupId(LandingScreen.newschemaName);

        public static string globalCoSDPath = "C:";
       public static string globalCoSDPathC = Path.GetTempPath();
        public static string globalCoSDPathE = "E:";

         // public static string globalSchemaName = "AnimalProductsCoSD.CoSD.";

        HandEntered uploadForm = new HandEntered();
        UpdateValidate updateForm = new UpdateValidate();
        DocumentationForm documentationForm = new DocumentationForm();
        ArchiveForm DeleteForm = new ArchiveForm();
        ImportExport updateFromExcel = new ImportExport();
        PrivacyForm privacyForm = new PrivacyForm();
        RepoManagement manageRepoForm= new RepoManagement();
        BusinessRules BusinessRules = new BusinessRules();
        AutomatedValidation Validate = new AutomatedValidation();
        ConversionFactorForm conversion = new ConversionFactorForm();
        FeedbackForm Doc = new FeedbackForm();
        StreamData StreamData = new StreamData();
        LandingScreen login = new LandingScreen();
        public Home(string schemaname)
        {
            globalSchemaName = schemaname;
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            InitializeComponent();
           

        }

     


        private void upload_button_Click(object sender, EventArgs e)
        {

            if (uploadForm.IsDisposed == true)
            {
                HandEntered UPLOADform = new HandEntered();
                UPLOADform.Show();
            }

            else
            {

                uploadForm.Show();

            }
        }

        private void update_button_Click(object sender, EventArgs e)
        {
            if (updateForm.IsDisposed == true)
            {
                UpdateValidate UPDATEform = new UpdateValidate();
                UPDATEform.Show();
            }


            else
            {
                updateForm.Show();
            }

        }



        private void delete_button_Click(object sender, EventArgs e)
        {
            if (DeleteForm.IsDisposed == true)
            {
                ArchiveForm DelForm = new ArchiveForm();
                DelForm.Show();
            }
            else
            {
                DeleteForm.Show();
            }
        }

        private void uploadToExcel_Click(object sender, EventArgs e)
        {
            if (updateFromExcel.IsDisposed == true)
            {
                ImportExport Importexcel = new ImportExport();
                Importexcel.Show();

            }
            else
            {
                updateFromExcel.Show();
            }
        }

        /// <summary>
        /// Handles the Click event of the button_exit control.
        /// Exit the entire program.
        /// *Todo*: need to upload user actions to DB.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void button_exit_Click(object sender, EventArgs e)
        {
            // save user actions to db
            // exit the program
            this.Close();
        }

        private void button_manageRepo_Click(object sender, EventArgs e)
        {
            if (manageRepoForm.IsDisposed == true)
            {
                RepoManagement MRF = new RepoManagement();
                MRF.Show();
            }
            else
            {
                manageRepoForm.Show();
            }
        }

        private void Getsource_button_Click(object sender, EventArgs e)
        {
            if (StreamData.IsDisposed == true)
            {
                StreamData SD = new StreamData();
                SD.Show();
            }
            else
            {
                StreamData.Show();
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Doc.IsDisposed == true)
            {
                FeedbackForm DCF = new FeedbackForm();
                DCF.Show();
            }
            else
            {

                Doc.Show();
            }
        }

        private void Validation_button_Click(object sender, EventArgs e)
        {
            if (Validate.IsDisposed == true)
            {
                AutomatedValidation VL = new AutomatedValidation();
                VL.Show();
            }

            else
            {
                Validate.Show();
            }
        }

        private void Businesslogic_button_Click(object sender, EventArgs e)
        {
            if (BusinessRules.IsDisposed == true)
            {
                BusinessRules BL = new BusinessRules();
                BL.Show();
            }

            else
            {
                BusinessRules.Show();
            }
        }

        private void button_conversionfactor_Click(object sender, EventArgs e)
        {
            if (conversion.IsDisposed == true)
            {
                ConversionFactorForm conversionfactor = new ConversionFactorForm();
                conversionfactor.Show();
            }

            else
            {
                conversion.Show();
            }
        }

        private void documentation_button_Click(object sender, EventArgs e)
        {
            if (documentationForm.IsDisposed == true)
            {
                DocumentationForm docForm = new DocumentationForm();
                docForm.Show();
            }

            else
            {

                documentationForm.Show();

            }
        }

        private void feedback_button_Click(object sender, EventArgs e)
        {
            if (Doc.IsDisposed == true)
            {
                FeedbackForm DCF = new FeedbackForm();
                DCF.Show();
            }
            else
            {

                Doc.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            login.Show();

        }
    }
}
