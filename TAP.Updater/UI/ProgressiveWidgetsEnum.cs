﻿namespace TAP.UPDATER {
    /// <summary>
    /// The class with the enums containing instances' descriptions of each used Widget.
    /// </summary>
    public static class ProgressiveWidgetsEnum {
        /// <summary>
        /// The enum containing instances' descriptions of each used <c>Label</c>.
        /// </summary>
        public enum Label {
            /// <summary>
            /// An instance description of the informative logger.
            /// </summary>
            InformativeLogger,

            /// <summary>
            /// An instance description of the downloader logger.
            /// </summary>
            DownloadLogger,

            /// <summary>
            /// An instance description of the file count logger.
            /// </summary>
            FileCountLogger,

            /// <summary>
            /// An instance description of the download speed logger.
            /// </summary>
            DownloadSpeedLogger
        };

        /// <summary>
        /// The enum containing instances' descriptions of each used <c>ProgressBar</c>.
        /// </summary>
        public enum ProgressBar {
            /// <summary>
            /// An instance description of the whole progress bar.
            /// </summary>
            WholeProgressBar,

            /// <summary>
            /// An instance description of the downloader progress bar.
            /// </summary>
            DownloadProgressBar
        };
    }
}
