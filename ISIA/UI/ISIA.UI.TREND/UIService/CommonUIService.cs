using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Data.Client;

namespace UIService
{
    public abstract class CommonUIService<Frm, Args, ArgPack> : IUIService<Frm, Args, ArgPack>
    {
        private Frm frmWork;
        private Args eventArgs;
        private ArgPack eventArgPack;
        BizDataClient bs = null;

        public Frm FrmWork { get => frmWork; set => frmWork = value; }
        public Args EventArgs { get => eventArgs; set => eventArgs = value; }
        public BizDataClient Bs { get => bs; set => bs = value; }
        public ArgPack EventArgPack { get => eventArgPack; set => eventArgPack = value; }

        public CommonUIService(Frm frm, Args args, ArgPack argPack)
        {
            this.FrmWork = frm;
            this.EventArgs = args;
            this.EventArgPack = argPack;
            Bs = new BizDataClient(frm);

        }
        public CommonUIService()
        {

        }
     

        public virtual ArgPack HandleArugument(Frm frm)
        {
            return default(ArgPack);
        }

        public virtual object GetData(ArgPack pack)
        {
            return null;
        }

        public virtual object ConvertData(object data)
        {
            return data;
        }

        public virtual void DisplayData(Frm frm, object data)
        {

        }

        public virtual void RunAsync()
        {
           
        }
      

        public void Run()
        {
            ArgPack pack = HandleArugument(frmWork);
            Object data = ConvertData(GetData(pack));
            DisplayData(frmWork, data);
        }


    }
}
