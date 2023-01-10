using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ISIA.UI.BASE
{
    public class AppointmentInfo
    {
        public AppointmentInfo()
        {
            this.ResourceId = 0;
            this.Location = "";
            this.Subject = "";
        }

        #region Property Members

        [DataMember]
        public virtual int ResourceId { get; set; }

        [DataMember]
        public virtual DateTime StartDate { get; set; }

        [DataMember]
        public virtual DateTime EndDate { get; set; }

        [DataMember]
        public virtual string Location { get; set; }

        [DataMember]
        public virtual string Subject { get; set; }
        #endregion
    }
}
