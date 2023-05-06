using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TAP;
using TAP.Base.Configuration;
using TAP.Fressage;

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

            try
            {
                tmpSql = " SELECT ID, CODE, POS, " + UICommon.Instance.SupportedLanguage + ", CC FROM [CODECOLLECTION] ";

                if (! this.comboBoxPartOfSpeech.SelectedItem.ToString().Equals("ALL"))
                {
                    tmpSql += string.Format(" WHERE [POS] = '{0}' ", this.comboBoxPartOfSpeech.SelectedItem.ToString());
                }

                //tmpDs = UICommon.Instance.DataBase._dbAccessor.Select(tmpSql);
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