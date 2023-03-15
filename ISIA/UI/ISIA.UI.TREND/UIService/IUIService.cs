using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIService
{
    public interface IUIService<Frm, Args, ArgPack> : IRun
    {
        ArgPack HandleArugument(Frm frm);
        Object GetData(ArgPack pack);
        Object ConvertData(Object data);
        void DisplayData(Frm frm, Object data);

    }
}
