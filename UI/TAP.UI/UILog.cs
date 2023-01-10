using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAP.UI
{
    /// <summary>
    /// This is UI log class
    /// </summary>
    public class UILog:LogBase
    {
        /// <summary>
        /// This method makes log file path
        /// </summary>
        /// <param name="objectName">Object name</param>
        /// <param name="logType">Log type</param>
        /// <returns>If 'TURE', making log file path succeed.</returns>
        protected override bool MakeFilePath(string objectName, string logType)
        {
            #region Make File Path

            try
            {
                //  TAP.Base.Configuration.ConfigurationManager.Instance.LocalLogSection.LocalLog["APP"]

                if (!TAP.Base.Configuration.ConfigurationManager.Instance.LogSection.Log[logType.ToUpper()].Logging)
                    return false;

                this.MakeFilePath(
                    TAP.Base.Configuration.ConfigurationManager.Instance.LogSection.Log[logType].Logging,
                    objectName,
                    System.IO.Path.Combine(TAP.Base.Configuration.ConfigurationManager.Instance.AppSection.MDIName, TAP.Base.Configuration.ConfigurationManager.Instance.FrameworkSection.Framework["UILog"].Path),
                    TAP.Base.Configuration.ConfigurationManager.Instance.LogSection.Log[logType].Path,
                    TAP.Base.Configuration.ConfigurationManager.Instance.LogSection.Log[logType].Extension,
                    TAP.Base.Configuration.ConfigurationManager.Instance.LogSection.Log[logType].MaxSize
                    );

                return true;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #endregion
        }
    }
}
