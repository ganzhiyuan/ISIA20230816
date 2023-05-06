using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using TAP;
using TAP.Base.Configuration;
using TAP.Fressage;
using TAP.Data.DataBase;

namespace TAP.Fressage.UI
{
    public partial class FormCodeList : Form
    {
        #region Fields

        #endregion

        public FormCodeList()
        {
            InitializeComponent();
        }

        #region Initialize

        private void InitializeUI()
        {
            #region Initialize UI

            try
            {
                this.BindComboBox();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void BindComboBox()
        {
            #region Bind Combo Box

            string[] tmp = null;
            string[] tmpPartOfSpeech = null;

            try
            {
                #region Get Source

                tmpPartOfSpeech = Enum.GetNames(typeof(EnumPartOfSpeech));
                tmp = new string[tmpPartOfSpeech.Length + 1];

                for (int i = 0; i < tmp.Length; i++)
                {
                    if (i.Equals(0))
                        tmp[i] = "ALL";
                    else
                        tmp[i] = tmpPartOfSpeech[i - 1];
                }

                #endregion

                #region Bind

                this.comboBoxPartOfSpeech.DataSource = null;
                this.comboBoxPartOfSpeech.BeginUpdate();
                this.comboBoxPartOfSpeech.DataSource = tmp;
                this.comboBoxPartOfSpeech.EndUpdate();
                this.comboBoxPartOfSpeech.SelectedIndex = 0;

                #endregion

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        #endregion

        #region Load Data

        private void LoadData()
        {
            #region Load Data

            string tmpSql = string.Empty;
            DataSet tmpDs = null;
            

            string tmpConnString = "Data Source={0};Version =3;";

            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            string tmpExecutablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

            string codeCollectionPath = Path.Combine(tmpExecutablePath, "mnls", TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile);

            tmpConnString = string.Format(tmpConnString, codeCollectionPath);

            Sqlite database = new Sqlite(tmpConnString, EnumConnectionMethod.CONNECTION_STRING);

            try
            {
                tmpSql = " SELECT * FROM [CODECOLLECTION] ";


                database.Select(tmpSql, "data", ref tmpDs, new ArgumentPack(), CommandType.Text);

                string updateSql = " UPDATE .....SQL TYPE...";

                database.ExecuteNonQuery(new string[] { updateSql });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void Display(DataSet ds)
        {
            #region Display

            try
            {
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }



        #endregion
    }
}