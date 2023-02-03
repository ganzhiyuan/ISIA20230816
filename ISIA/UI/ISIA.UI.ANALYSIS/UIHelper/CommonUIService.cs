using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Data.Client;

namespace UIHelper
{
   public abstract class CommonUIService<Frm, Args, ArgPack> : IUIService<Frm, Args, ArgPack>   
    {
        private Frm frmWork;
        private Args eventArgs;
        BizDataClient bs = null;

        public Frm FrmWork { get => frmWork; set => frmWork = value; }
        public Args EventArgs { get => eventArgs; set => eventArgs = value; }

        public CommonUIService(Frm frm, Args args)
        {
            this.FrmWork = frm;
            this.EventArgs = args;
            bs = new BizDataClient(frm);

        }
        public CommonUIService()
        {

        }



        public virtual object ConvertData(object data) {
            return data;
        }

        public virtual void DisplayData(Frm frm, object data) {
            
        }


        public virtual object GetData(ArgPack pack)
        {
            return null;
        }


        public virtual ArgPack HandleArugument(Frm frm)
        {
            return default(ArgPack);
        }

        public void Run()
        {
            ArgPack pack=HandleArugument(frmWork);
            Object data= ConvertData(GetData(pack));
            DisplayData(frmWork, data);
        }
       

    }
}
