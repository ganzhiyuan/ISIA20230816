using System;
using System.Collections.Generic;
using System.Text;

using TAP;
using TAP.Base.Configuration;
using TAP.Fressage;

namespace TAP.Fressage.UI
{
    public class UICommon
    {
        #region Fields

        private string _dataFile;
        private string _supportedLanguage;
        private CodeLoader _dataBase;

        #endregion

        #region Properties

        public string DataFile
        {
            #region Data File

            get
            {
                return this._dataFile;
            }
            set
            {
                string tmpCurrDataFile = string.Empty;

                try
                {
                    tmpCurrDataFile = this._dataFile;

                    _dataFile = value;
                    _dataBase = CodeLoader.GetInstance(EnumLanguage.EN,_dataFile);
                    TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile = _dataFile;
                }
                catch(System.Exception ex)
                {
                    _dataFile = tmpCurrDataFile;
                    _dataBase = CodeLoader.GetInstance(EnumLanguage.EN, _dataFile);
                    TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile = _dataFile;

                    throw ex;
                }
            }

            #endregion
        }

        public CodeLoader DataBase { get { return this._dataBase; } }

        public string SupportedLanguage
        {
            #region Supported Language

            get { return this._supportedLanguage; }
            set
            {
                string tmpCurrentSupported = string.Empty;

                try
                {
                    tmpCurrentSupported = _supportedLanguage;
                    _supportedLanguage = value;

                    TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.SupportedLanguage = _supportedLanguage;
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }

            #endregion
        }
        
        #endregion

        #region Creator

        /// <summary>
        /// Static accessor of UICommon<br></br>
        /// TapBase의 정적 접근자 
        /// </summary>
        public static UICommon Instance
        {
            get
            {
                if (instant == null)
                    instant = new UICommon();

                return instant;
            }
        }

        /// <summary>
        /// This is UICommon-Typed global varaiable.<br></br>
        /// UICommon 타입의 전역변수입니다. 
        /// </summary>
        private static UICommon instant;

        private UICommon()
        {
            this._dataFile = TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.LocalFile;
            this._dataBase = CodeLoader.GetInstance(EnumLanguage.EN, _dataFile);
            this._supportedLanguage = TAP.Base.Configuration.ConfigurationManager.Instance.CorssLanguageSection.SupportedLanguage;
        }

        #endregion
    }
}
