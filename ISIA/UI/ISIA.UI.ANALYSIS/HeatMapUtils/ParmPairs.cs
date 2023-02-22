using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analysis.Correlation
{
  public  class ParmPairs
    {
        private string paramX;

        private string paramY;
        public string ParamX { get => paramX; set => paramX = value; }
        public string ParamY { get => paramY; set => paramY = value; }

        public override bool Equals(object obj)
        {
            ParmPairs parm = obj as ParmPairs;
            return parm.paramX.Equals(this.paramX) && parm.paramY.Equals(this.paramY);
        }
    }

}
