using ISIA.UI.BASE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TAP;
using TAP.Data.DataBase;
using TAP.Fressage;
using TAP.UI;

namespace ISIA.UI.ADMINISTRATOE
{
    public partial class FrmLanguageManagement : DockUIBase1T1
    {

        DataSet tmpDs = new DataSet();
        public FrmLanguageManagement()
        {
            InitializeComponent();
        }

        private void FrmLanguageManagement_Load(object sender, EventArgs e)
        {
            BindComboBox();
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

                //for (int i = 0; i < tmp.Length; i++)
                //{
                //    if (i.Equals(0))
                //        tmp[i] = "ALL";
                //    else
                //        tmp[i] = tmpPartOfSpeech[i - 1];
                //}

                #endregion

                #region Bind
                foreach (var item in tmpPartOfSpeech)
                {
                    this.cmbPos.Properties.Items.Add(item);
                }
                

                #endregion

                return;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }

        private void LoadData()
        {
            #region Load Data

            GetCodeByCodeAndPos(txtCode.Text,cmbPos.Text);

            #endregion
        }

        private DataSet GetCodeByCodeAndPos(string code,string pos)
        {
            tmpDs = new DataSet();
            try
            {
                Sqlite database = GetDataBase();
                StringBuilder tmpSql = new StringBuilder();
                tmpSql.Append(" SELECT * FROM [CODECOLLECTION] where 1=1");
                if (!string.IsNullOrEmpty(code))
                {
                    tmpSql.AppendFormat(" and code ='{0}'", code);
                }
                if (!string.IsNullOrEmpty(pos))
                {
                    tmpSql.AppendFormat(" and POS ='{0}'", pos);
                }

                database.Select(tmpSql.ToString(), "data", ref tmpDs, new ArgumentPack(), CommandType.Text);

                //string updateSql = " UPDATE .....SQL TYPE...";

                //database.ExecuteNonQuery(new string[] { updateSql });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return tmpDs;
        }

        private static Sqlite GetDataBase()
        {
            string tmpConnString = "Data Source={0};Version =3;";

            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            string tmpExecutablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";

            string codeCollectionPath = Path.Combine(tmpExecutablePath, "mnls", TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile);

            tmpConnString = string.Format(tmpConnString, codeCollectionPath);

            Sqlite database = new Sqlite(tmpConnString, EnumConnectionMethod.CONNECTION_STRING);
            return database;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            tmpDs = new DataSet();
            LoadData();
            gridControl1.DataSource = null;
            gridControl1.DataSource = tmpDs.Tables[0];
            gridView1.BestFitColumns();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            int[] i = gridView1.GetSelectedRows();
            if (i == null && i.Count() < 1)
            {
                return;
            }
            string tmpMessage = _translator.ConvertGeneralTemplate(EnumVerbs.UPDATE, EnumGeneralTemplateType.CONFIRM, "");
            DialogResult dialog = TAP.UI.TAPMsgBox.Instance.ShowMessage(Text, EnumMsgType.CONFIRM, tmpMessage);
            if (dialog.ToString() == "Yes")
            {
                Sqlite database = GetDataBase();
                foreach (var item in i)
                {
                    StringBuilder tmpSql = new StringBuilder();
                    DataRow dr = gridView1.GetDataRow(item) as DataRow;
                    tmpSql.AppendFormat(" DELETE FROM CODECOLLECTION WHERE CODE='{0}' AND POS='{1}'", dr["CODE"], dr["POS"]);

                    database.ExecuteNonQuery(new string[] { tmpSql.ToString() });
                }
            }
            TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Success.");
            btnSelect_Click(null, null);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (gridView1.RowCount>0)
            {
                DataTable dt = gridControl1.DataSource as DataTable;
                DataRow dr = dt.NewRow();
                dt.Rows.InsertAt(dr,0);
                gridView1.FocusedRowHandle = 0;
            }
            else
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("CODE");
                dt.Columns.Add("POS");
                dt.Columns.Add("EN");
                dt.Columns.Add("KR");
                dt.Columns.Add("CN");
                dt.Columns.Add("CC");
                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);
                this.gridControl1.DataSource = dt;
                this.gridView1.PopulateColumns();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int[] i = gridView1.GetSelectedRows();
            if (i == null && i.Count() < 1)
            {
                return;
            }
            Sqlite database = GetDataBase();
            foreach (var item in i)
            {
                DataRow dr = gridView1.GetDataRow(item) as DataRow;
                StringBuilder tmpSql = new StringBuilder();
                if (dr != null)
                {
                    DataSet ds = GetCodeByCodeAndPos(dr["CODE"].ToString(), dr["POS"].ToString());
                    if (ds == null || ds.Tables == null || ds.Tables[0].Rows.Count == 0)
                    {
                        tmpSql.AppendFormat(" insert into [CODECOLLECTION] (CODE,POS,EN,KR,CN,CC) VALUES ('{0}','{1}','{2}' ,'{3}' ,'{4}' ,'{5}')", dr["CODE"], dr["POS"], dr["EN"], dr["KR"], dr["CN"], dr["CC"]);
                        
                        database.ExecuteNonQuery(new string[] { tmpSql.ToString() });
                    }
                    else
                    {
                        tmpSql.AppendFormat(" update [CODECOLLECTION] set EN='{0}',KR='{1}',CN='{2}' ,CC='{3}' ", dr["EN"], dr["KR"], dr["CN"], dr["CC"]);
                        tmpSql.AppendFormat(" where CODE='{0}' AND POS='{1}'", dr["CODE"], dr["POS"]);
                        database.ExecuteNonQuery(new string[] { tmpSql.ToString() });
                    }
                }

            }
            TAP.UI.TAPMsgBox.Instance.ShowMessage(this.Text, EnumMsgType.ERROR, "Success.");
            btnSelect_Click(null, null);
            //try
            //{
            //    tmpSql.Append(" update [CODECOLLECTION] set where 1=1");
            //    if (string.IsNullOrEmpty(txtCode.Text))
            //    {
            //        tmpSql.AppendFormat(" and code ={0}", txtCode.Text);
            //    }
            //    if (string.IsNullOrEmpty(cmbPos.Text))
            //    {
            //        tmpSql.AppendFormat(" and POS ={0}", cmbPos.Text);
            //    }

            //    database.Select(tmpSql.ToString(), "data", ref tmpDs, new ArgumentPack(), CommandType.Text);

            //    //string updateSql = " UPDATE .....SQL TYPE...";

            //    //database.ExecuteNonQuery(new string[] { updateSql });
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}
        }
    }
}
