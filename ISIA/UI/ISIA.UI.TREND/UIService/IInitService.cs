﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIHelper
{
    public interface IInitService<T>
    {
        void InitializeData(T frm);
    }
}
