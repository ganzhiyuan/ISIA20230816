using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ISIA.UI.BASE
{
    public class AppResourceInfo
    {
        public AppResourceInfo()
        {
            this.ID = 0;
            this.Caption = "";            
        }

        #region Property Members

        [DataMember]
        public virtual int ID { get; set; }
       
        [DataMember]
        public virtual string Caption { get; set; }       
        #endregion
    }
}
