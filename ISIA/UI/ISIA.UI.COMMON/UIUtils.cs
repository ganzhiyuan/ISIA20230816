using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISFA.UI.COMMON
{
   public static class UIUtils
    {
        public static string GetImagePath(this string currentPath)
        {
            //string imagePath = @"C:\" + currentPath;
            string imagePath = TAP.Base.Configuration.ConfigurationManager.Instance.FrameworkSection.Framework["ImagePath"].Path;
            return imagePath;
        }
    }
}
