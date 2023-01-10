using System;
using System.Collections.Generic;
using System.Text;

namespace TAP.UI
{
    /// <summary>
    /// This enumeratior is message type list.
    /// </summary>
    public enum EnumMsgType
    {
        /// <summary>
        /// Information
        /// </summary>
        INFORMATION,

        /// <summary>
        /// Confirm
        /// </summary>
        CONFIRM,

        /// <summary>
        /// Warning
        /// </summary>
        WARNING,

        /// <summary>
        /// Error
        /// </summary>
        ERROR
    }
    /// <summary>
    /// This enumeratior is Message text list.
    /// </summary>
    public enum EnumMessage_text
    {
        /// <summary>
        /// Save
        /// </summary>
        SAVE,

        /// <summary>
        /// Delete
        /// </summary>
        DELETE,

        /// <summary>
        /// Insert
        /// </summary>
        INSERT,

        /// <summary>
        /// Update
        /// </summary>
        UPDATE
    }

    /// <summary>
    /// This enumeratior is data object list used by user interface
    /// </summary>
    public enum EnumDataObject
    {
        /// <summary>
        /// Model set
        /// </summary>
        MODELSET,

        /// <summary>
        /// Model list
        /// </summary>
        MODELLIST,

        /// <summary>
        /// Data Set
        /// </summary>
        DATASET,

        /// <summary>
        /// String
        /// </summary>
        STRING,

        /// <summary>
        /// None
        /// </summary>
        NONE
    }
}
