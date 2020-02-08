using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.HIPOT
{
    /// <summary>
    /// 高压机型号
    /// </summary>
    public enum EHPType
    {
        /// <summary>
        /// 10通道高压机
        /// </summary>
        Chroma19020,
        /// <summary>
        /// 4通道高压机
        /// </summary>
        Chroma19020_4,
        /// <summary>
        /// 单通道高压机
        /// </summary>
        Chroma19032,
        /// <summary>
        /// 10通道高压机
        /// </summary>
        Chroma19020_GPIB,
        /// <summary>
        /// 8通道高压机
        /// </summary>
        Chroma19053_8,
        /// <summary>
        /// 1通道高压机
        /// </summary>
        Extech7100,
        /// <summary>
        /// 同惠8通道高压机
        /// </summary>
        TH9010,
        /// <summary>
        /// 特尔斯特4通道
        /// </summary>
        NS200,
        /// <summary>
        /// 华仪7440
        /// </summary>
        Extech7440,
        /// <summary>
        /// 华仪7440
        /// </summary>
        CExtech7440_GPIB,
        /// <summary>
        /// IO控制高压机
        /// </summary>
        PLC_COM
    }
    /// <summary>
    /// 高压机状态
    /// </summary>
    public enum EHPStatus
    {
        RUNNING,
        STOPPED,
        IDLE
    }
    /// <summary>
    /// 测试项目定义
    /// </summary>
    public enum EStepName
    {
        AC,
        DC,
        IR,
        OSC,
        GB,
        PA
    }
    /// <summary>
    /// 高压测试参数
    /// </summary>
    public class CHPPara
    {
        #region 高压项目定义
        /// <summary>
        /// AC项目
        /// </summary>
        public static string[] C_ACItem = new string[] { "VOLTAGE","HIGHT LIMIT","LOW LIMIT","ARC LIMIT",
                                                         "RAMP TIME","TEST TIME","FALL TIME" };
        /// <summary>
        /// AC单位 
        /// </summary>
        public static string[] C_ACUnit = new string[] { "kV(0.05-5kV)","mA(0.001-10mA)","mA(0-10mA)","mA(0,1-20mA)",
                                                 "s(0,0.1-999.9s)","s(0,0.03-999.9s)","s(0,0.1-999.9s)"};
        /// <summary>
        /// DC项目
        /// </summary>
        public static string[] C_DCItem = new string[] { "VOLTAGE","HIGHT LIMIT","LOW LIMIT","ARC LIMIT",
                                                         "RAMP TIME","DWELL TIME","TEST TIME","FALL TIME" };
        /// <summary>
        /// DC单位
        /// </summary>
        public static string[] C_DCUnit = new string[] { "kV(0.05-6kV)","mA(0.0001-5mA)","mA(0-5mA)","mA(0,1-10mA)",
                                                         "s(0,0.1-999.9s)","s(0,0.1-999.9s)","s(0,0.03-999.9s)","s(0,0.1-999.9s)"};
        /// <summary>
        /// IR项目
        /// </summary>
        public static string[] C_IRItem = new string[] { "VOLTAGE","LOW LIMIT","HIGHT LIMIT",
                                                         "RAMP TIME","TEST TIME","FALL TIME" };
        /// <summary>
        /// IR单位
        /// </summary>
        public static string[] C_IRUnit = new string[] { "kV(0.05-1kV)","M0hm(0.1M0hm-50G0hm)","M0hm(0-50G0hm)",
                                                         "s(0,0.1-999.9s)","s(0,0.3-999.9s)","s(0,0.1-999.9s)"};
        /// <summary>
        /// OSC项目
        /// </summary>
        public static string[] C_OSCItem = new string[] { "OPEN", "SHORT" };
        /// <summary>
        /// OSC单位
        /// </summary>
        public static string[] C_OSCUnit = new string[] { "%(10%-100%)", "%(0,100%-500%)" };
        /// <summary>
        /// GB项目
        /// </summary>
        public static string[] C_GBItem = new string[] { "CURRENT", "HIGHT LIMIT", "LOW LIMIT", "TEST TIME", "TWIN PORT" };
        /// <summary>
        /// GB单位
        /// </summary>
        public static string[] C_GBUnit = new string[] { "mA", "M0hm(0-510M0hm)", "M0hm(0-510M0hm)", "s(0-999.9s)", "ON=1;OFF=0" };
        /// <summary>
        /// 测试项目
        /// </summary>
        [Serializable]
        public class CItem
        {
            /// <summary>
            /// 项目名称
            /// </summary>
            public string name;
            /// <summary>
            /// 设定值
            /// </summary>
            public double setVal;
            /// <summary>
            /// 单位值
            /// </summary>
            public string unitDes;
        }
        /// <summary>
        /// 测试步骤
        /// </summary>
        [Serializable]
        public class CStep
        {
            /// <summary>
            /// 步骤
            /// </summary>
            public int stepNo;
            /// <summary>
            /// 测试项目
            /// </summary>
            public EStepName name;
            /// <summary>
            /// 项目描述
            /// </summary>
            public string des;
            /// <summary>
            /// 测试参数
            /// </summary>
            public List<CItem> para = new List<CItem>();
        }
        /// <summary>
        ///  项目值
        /// </summary>
        public class CVal
        {
            /// <summary>
            /// 测试项目
            /// </summary>
            public EStepName name;
            /// <summary>
            /// 测试结果
            /// </summary>
            public int result = 0;
            /// <summary>
            /// 错误信息
            /// </summary>
            public string code = "";
            /// <summary>
            /// 测试值
            /// </summary>
            public double value = 0;
            /// <summary>
            /// 测试单位
            /// </summary>
            public string unit = string.Empty;
            /// <summary>
            /// 测试值
            /// </summary>
            public double value1 = 0;
            /// <summary>
            /// 测试单位
            /// </summary>
            public string unit1 = string.Empty;
            /// <summary>
            /// 测试值
            /// </summary>
            public double value2 = 0;
            /// <summary>
            /// 测试单位
            /// </summary>
            public string unit2 = string.Empty;

        }
        /// <summary>
        /// 测试结果
        /// </summary>
        public class CStepVal
        {
            /// <summary>
            /// 测试通道
            /// </summary>
            public int chanNo;
            /// <summary>
            /// 测试结果
            /// </summary>
            public int result;
            /// <summary>
            /// 测试数据
            /// </summary>
            public List<CVal> mVal = new List<CVal>();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 增加测试项目
        /// </summary>
        /// <param name="stepName"></param>
        /// <param name="itemVal"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static CStep IniStep(EStepName stepName, int stepNo, List<double> itemVal)
        {
            try
            {

                CStep stepItem = new CStep();

                stepItem.stepNo = stepNo;

                stepItem.name = stepName;

                switch (stepName)
                {
                    case EStepName.AC:
                        stepItem.des = "交流电压耐压(AC)测试";
                        for (int i = 0; i < C_ACItem.Length; i++)
                        {
                            if (i < itemVal.Count)
                            {
                                CItem item = new CItem();
                                item.name = C_ACItem[i];
                                item.unitDes = C_ACUnit[i];
                                item.setVal = itemVal[i];
                                stepItem.para.Add(item);
                            }
                        }
                        break;
                    case EStepName.DC:
                        stepItem.des = "直流电压耐压(DC)测试";
                        for (int i = 0; i < C_DCItem.Length; i++)
                        {
                            if (i < itemVal.Count)
                            {
                                CItem item = new CItem();
                                item.name = C_DCItem[i];
                                item.unitDes = C_DCUnit[i];
                                item.setVal = itemVal[i];
                                stepItem.para.Add(item);
                            }
                        }
                        break;
                    case EStepName.IR:
                        stepItem.des = "绝缘阻抗(IR)测试";
                        for (int i = 0; i < C_IRItem.Length; i++)
                        {
                            if (i < itemVal.Count)
                            {
                                CItem item = new CItem();
                                item.name = C_IRItem[i];
                                item.unitDes = C_IRUnit[i];
                                item.setVal = itemVal[i];
                                stepItem.para.Add(item);
                            }
                        }
                        break;
                    case EStepName.GB:
                        stepItem.des = "接地电阻(GB)测试";
                        for (int i = 0; i < C_GBItem.Length; i++)
                        {
                            if (i < itemVal.Count)
                            {
                                CItem item = new CItem();
                                item.name = C_GBItem[i];
                                item.unitDes = C_GBUnit[i];
                                item.setVal = itemVal[i];
                                stepItem.para.Add(item);
                            }
                        }
                        break;
                    case EStepName.OSC:
                        stepItem.des = "开短路侦测(OS)测试";
                        stepItem.des = "绝缘阻抗(IR)测试";
                        for (int i = 0; i < C_OSCItem.Length; i++)
                        {
                            if (i < itemVal.Count)
                            {
                                CItem item = new CItem();
                                item.name = C_OSCItem[i];
                                item.unitDes = C_IRUnit[i];
                                item.setVal = itemVal[i];
                                stepItem.para.Add(item);
                            }
                        }
                        break;
                    default:
                        break;
                }

                return stepItem;
            }
            catch (Exception)
            {
                return null;
            }

        }
        /// <summary>
        /// 设置测试项目
        /// </summary>
        /// <param name="stepName"></param>
        /// <param name="itemVal"></param>
        /// <param name="step"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static CStep IniStep(EStepName stepName, int stepNo)
        {
            try
            {

                CStep stepItem = new CStep();

                stepItem.stepNo = stepNo;

                stepItem.name = stepName;

                switch (stepName)
                {
                    case EStepName.AC:
                        stepItem.des = "交流电压耐压(AC)测试";
                        for (int i = 0; i < C_ACItem.Length; i++)
                        {
                            CItem item = new CItem();
                            item.name = C_ACItem[i];
                            item.unitDes = C_ACUnit[i];
                            item.setVal = 0;
                            stepItem.para.Add(item);
                        }
                        break;
                    case EStepName.DC:
                        stepItem.des = "直流电压耐压(DC)测试";
                        for (int i = 0; i < C_DCItem.Length; i++)
                        {
                            CItem item = new CItem();
                            item.name = C_DCItem[i];
                            item.unitDes = C_DCUnit[i];
                            item.setVal = 0;
                            stepItem.para.Add(item);
                        }
                        break;
                    case EStepName.IR:
                        stepItem.des = "绝缘阻抗(IR)测试";
                        for (int i = 0; i < C_IRItem.Length; i++)
                        {
                            CItem item = new CItem();
                            item.name = C_IRItem[i];
                            item.unitDes = C_IRUnit[i];
                            item.setVal = 0;
                            stepItem.para.Add(item);
                        }
                        break;
                    case EStepName.GB:
                        stepItem.des = "接地电阻(GB)测试";
                        for (int i = 0; i < C_GBItem.Length; i++)
                        {
                            CItem item = new CItem();
                            item.name = C_GBItem[i];
                            item.unitDes = C_GBUnit[i];
                            item.setVal = 0;
                            stepItem.para.Add(item);
                        }
                        break;
                    case EStepName.OSC:
                        stepItem.des = "开短路侦测(OS)测试";
                        for (int i = 0; i < C_OSCItem.Length; i++)
                        {
                            CItem item = new CItem();
                            item.name = C_OSCItem[i];
                            item.unitDes = C_OSCUnit[i];
                            item.setVal = 0;
                            stepItem.para.Add(item);
                        }
                        break;
                    default:
                        break;
                }
                return stepItem;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
    /// <summary>
    /// 步骤结果
    /// </summary>
    public class CStepResult
    {
        /// <summary>
        /// 结果
        /// </summary>
        public int Result = 0;
        /// <summary>
        /// 测试代码
        /// </summary>
        public string Code = string.Empty;
        /// <summary>
        /// 项目名称
        /// </summary>
        public EStepName Name = EStepName.AC;
        /// <summary>
        /// 测试值
        /// </summary>
        public double Value = 0;
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit = string.Empty;
    }
    /// <summary>
    /// 通道结果
    /// </summary>
    public class CCHResult
    {
        /// <summary>
        /// 通道结果
        /// </summary>
        public int Result = 0;
        /// <summary>
        /// 步骤结果
        /// </summary>
        public List<CStepResult> Step = new List<CStepResult>(); 
    }
}
