using System;
using System.Collections.Generic;
using TAP;

namespace ISIA.INTERFACE.ARGUMENTSPACK
{
    [Serializable]
    public class MessageArgsPack
    {
        #region Fields
        private ArgumentPack argsPack = new ArgumentPack();
        private string _Message_Group_Name;
        private string _Description;
        private string _Message_Name;
        private string _Message_Group_ID;
        private string _Message_KR;
        private string _Message_EN;
        private string _Message_CN;
        private string _Update_User;
        private string _Update_Time;
        private List<MessageArgsPack> _MessageArgsPacks = new List<MessageArgsPack>();
        #endregion

        #region Properties
        public string Message_Group_Name
        {
            get
            {
                return _Message_Group_Name;
            }

            set
            {
                _Message_Group_Name = value;
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        public string Message_Name
        {
            get
            {
                return _Message_Name;
            }

            set
            {
                _Message_Name = value;
            }
        }

        public string Message_Group_ID
        {
            get
            {
                return _Message_Group_ID;
            }

            set
            {
                _Message_Group_ID = value;
            }
        }

        public string Message_KR
        {
            get
            {
                return _Message_KR;
            }

            set
            {
                _Message_KR = value;
            }
        }

        public string Message_EN
        {
            get
            {
                return _Message_EN;
            }

            set
            {
                _Message_EN = value;
            }
        }

        public string Message_CN
        {
            get
            {
                return _Message_CN;
            }

            set
            {
                _Message_CN = value;
            }
        }

        public List<MessageArgsPack> MessageArgsPacks
        {
            get
            {
                return _MessageArgsPacks;
            }

            set
            {
                _MessageArgsPacks = value;
            }
        }

        public string Update_User
        {
            get
            {
                return _Update_User;
            }

            set
            {
                _Update_User = value;
            }
        }

        public string Update_Time
        {
            get
            {
                return _Update_Time;
            }

            set
            {
                _Update_Time = value;
            }
        }
        #endregion
        public ArgumentPack getPack()
        {
            argsPack.ClearArguments();
            argsPack.AddArgument("arguments", typeof(MessageArgsPack), this);
            return argsPack;
        }
    }
}
