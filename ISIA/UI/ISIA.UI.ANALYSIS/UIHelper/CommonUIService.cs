using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAP.Data.Client;

namespace UIHelper
{
   public abstract class CommonUIService<T, K> : IUIService<T, K>   
    {
        private T frm;
        private K args;
        BizDataClient bs = null;


        public CommonUIService(T frm, K args)
        {
            this.Frm = frm;
            this.Args = args;
            bs = new BizDataClient(frm);

        }
        public CommonUIService()
        {
           
        }

        public T Frm { get => frm; set => frm = value; }
        public K Args { get => args; set => args = value; }

        public virtual object ConvertData(object data) {
            return data;
        }

        public virtual void DisplayData(T frm, object data) {
            
        }


        public virtual object GetData(K args)
        {
            return null;
        }


        public virtual void HandleArugument(T frm)
        {

        }

        public void Run()
        {
            HandleArugument(frm);
            Object data= ConvertData(GetData(args));
            DisplayData(frm, data);
        }
       

    }
}
