using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.Dev.PLC
{
    public class CPLCConArgs:EventArgs 
    {
        public readonly int idNo;
        public readonly string name;
        public readonly string conStatus;
        public readonly bool bErr;
        public CPLCConArgs(int idNo,string name,string conStatus, bool bErr = false)
        {
            this.idNo = idNo;
            this.name = name;
            this.conStatus = conStatus;
            this.bErr = bErr;
        }
    }
    public class CPLCDataArgs : EventArgs
    {
        public readonly int idNo;
        public readonly string name;
        public readonly string rData;
        public readonly bool bErr;
        public readonly bool bComplete;
        public CPLCDataArgs(int idNo, string name, string rData, bool bComplete = true, bool bErr = false)
        {
            this.idNo = idNo;
            this.name = name;
            this.rData = rData;
            this.bComplete = bComplete;
            this.bErr = bErr;  
        }
    }
}
