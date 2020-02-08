using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.ATD
{
    /// <summary>
    /// 寄存器类型
    /// </summary>
    public enum ERegType
    {
        X,
        Y,
        D
    }
    /// <summary>
    /// 相位
    /// </summary>
    public class CPhase
    {
        public double acv = 0;

        public double aci = 0;

        public double power = 0;

        public string name = string.Empty;

        public CPhase(string name)
        {
            this.name = name;
        }
        public override string ToString()
        {
            return name;
        }
    }

    public class CPara
    {
        public CPara()
        {
            Phase = new List<CPhase>();

            Phase.Add(new CPhase("A相"));

            Phase.Add(new CPhase("B相"));

            Phase.Add(new CPhase("C相")); 

        }

        public CPara Clone()
        {
            CPara pa = new CPara();

            pa.RunStatus = this.RunStatus;

            pa.AlarmCode = this.AlarmCode;

            for (int i = 0; i < pa.Phase.Count; i++)
            {
                pa.Phase[i].name = this.Phase[i].name;

                pa.Phase[i].acv = this.Phase[i].acv;

                pa.Phase[i].aci = this.Phase[i].aci;

                pa.Phase[i].power = this.Phase[i].power;
            }

            return pa;
        }

        public int RunStatus = 0;

        public int AlarmCode = 0;

        public List<CPhase> Phase = null;
    }
}
