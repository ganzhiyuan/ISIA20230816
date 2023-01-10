using System;
using System.Collections.Generic;
using System.Text;

using TAP;
using TAP.Models;
using TAP.Models.UIBasic;
using TAP.Models.User;

namespace TAP.UI
{
    /// <summary>
    /// This class is information of UI.
    /// </summary>
    public class InfoBase
    {
        #region Fields

        /// <summary>
        /// MDI Infomation
        /// </summary>
        public static MDIBasicModelSet _MDI_INFO;

        ///// <summary>
        ///// Menu Information
        ///// </summary>
        //public static MainMenuBasicModelSet _MENU_INFO;

        /// <summary>
        /// User information
        /// </summary>
        public static UserModel _USER_INFO;

        /// <summary>
        /// User group information
        /// </summary>
        public static List<UserGroupModel> _USER_GROUP;

        /// <summary>
        /// User Group Member Infomation
        /// </summary>
        public static GroupMemberModel _USER_GROUP_MEMBER;

        /// <summary>
        /// Excuting UI type name
        /// </summary>
        public static string FormTypeName = string.Empty;

        /// <summary>
        /// ID of Excuting UI
        /// </summary>
        public static string FormInstanceID = string.Empty;

        /// <summary>
        /// Receive port for WorkState
        /// </summary>
        public static int WorkStateReceiverPortNo = -1;

        /// <summary>
        /// Command in excuting
        /// </summary>
        public static string CurrentCommand = string.Empty;

        /// <summary>
        /// User name
        /// </summary>
        public static string UserID = string.Empty;

        #endregion
    }
}
