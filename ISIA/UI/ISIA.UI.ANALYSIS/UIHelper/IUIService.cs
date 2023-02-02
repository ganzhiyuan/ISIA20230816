﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIHelper
{
    public interface IUIService<Frm,Args> :IRun
    {
        void HandleArugument(Frm frm);
        Object GetData(Args args);
        Object ConvertData(Object data);
        void DisplayData(Frm frm, Object data);

    }
}
