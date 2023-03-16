using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAP.UI
{
    /// <summary>
    /// 继承基类右击菜单接口规范
    /// </summary>
    public interface PopMenuInterface
    {
        /// <summary>
        /// 根据groupidc从查询对应显示右击一级菜单及子菜单，返回值用于基类SetPopupMenuItem调用参数
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>groupDisplayName对应子类菜单包含link</returns>
       List<LinkInfo> GetLinkAge(string groupId);
    }

    public class LinkInfo
    {
        public string GROUPID { get; set; }
        public string GROUPNAME { get; set; }
        public string UI { get; set; }
        public string TAGETUI { get; set; }
        public string TAGETUINAME { get; set; }
        public string PARAMETERLIST { get; set; }
        public string ISALIVE { get; set; }
        public List<LinkInfo> list { get; set; }

        public string LASTEVENTCOMMENT { get; set; }
        public string LASTEVENT { get; set; }
        public string LASTEVENTFLAG { get; set; }
        public string LASTEVENTTIME { get; set; }
        public string LASTEVENTCODE { get; set; }
        public string DESCRIPTION { get; set; }

    }
}
