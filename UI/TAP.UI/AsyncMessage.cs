using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TAP.UI
{
    /// <summary>
    /// This is message of asynchronous process
    /// </summary>
    public static class AsyncMessage
    {
        /// <summary>
        /// Message
        /// </summary>
        public static string Message { get; set; }

        /// <summary>
        /// Progress
        /// </summary>
        public static long Progress { get; set; }
    }
}
