using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Fressage;
using System.IO;
using System.Reflection;
using TAP.UI;
using TAP.UIControls;

namespace ISIA.UI.BASE
{
    public class FormSetLanguage
    {
        public FormSetLanguage()
        {
            string language = InfoBase._USER_INFO.Language;
            EnumLanguage tmpLang = (EnumLanguage)Enum.Parse(typeof(EnumLanguage), InfoBase._USER_INFO.Language);
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            string tmpExecutablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\";
            string fressageFilePath = Path.Combine(tmpExecutablePath, "mnls", TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile);
            this._converter = new NeutralConverter(tmpLang, EnumUseFor.TEXT, false, fressageFilePath);
            this._translator = new TemplateConverter(tmpLang, false, fressageFilePath);
        }
        public FormSetLanguage(NeutralConverter tmpNeutralConverter)
        {
            this._converter = tmpNeutralConverter;
        }
        /// <summary>
        /// Converter for cross-language
        /// </summary>
        protected NeutralConverter _converter;

        /// <summary>
        /// Translator of cross-language
        /// </summary>
        protected TemplateConverter _translator;

        public void SetLanguage(List<dynamic> objs)
        {
            try
            {
                foreach (dynamic obj in objs)
                {
                    obj.Text = this._converter.ConvertPhrase(obj.Text);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void SetLanguage_TLabel(List<TAP.UIControls.BasicControlsDEV.TLabel> labels)
        {
            try
            {
                foreach (TAP.UIControls.BasicControlsDEV.TLabel item in labels)
                {
                    item.Text = this._converter.ConvertPhrase(item.Text);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void SetLanguage_LabelControl(List<DevExpress.XtraEditors.LabelControl> labels)
        {
            try
            {
                foreach (DevExpress.XtraEditors.LabelControl item in labels)
                {
                    item.Text = this._converter.ConvertPhrase(item.Text);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void SetLanguage_SimpleButton(List<DevExpress.XtraEditors.SimpleButton> buttons)
        {
            try
            {
                foreach (DevExpress.XtraEditors.SimpleButton item in buttons)
                {
                    item.Text = this._converter.ConvertPhrase(item.Text);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void SetLanguage_TButton(List<TAP.UIControls.BasicControlsDEV.TButton> buttons)
        {
            try
            {
                foreach (TAP.UIControls.BasicControlsDEV.TButton item in buttons)
                {
                    item.Text = this._converter.ConvertPhrase(item.Text);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void SetLanguage_TCheckBox(List<TAP.UIControls.BasicControlsDEV.TCheckBox> buttons)
        {
            try
            {
                foreach (TAP.UIControls.BasicControlsDEV.TCheckBox item in buttons)
                {
                    item.Text = this._converter.ConvertPhrase(item.Text);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
