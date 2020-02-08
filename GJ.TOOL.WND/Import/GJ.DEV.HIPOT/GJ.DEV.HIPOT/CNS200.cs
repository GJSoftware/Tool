using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GJ.DEV.COM;

namespace GJ.DEV.HIPOT
{
    /// <summary>
    /// 特尔斯特9034-4通道-->深圳璟浩
    /// </summary>
    public class CNS200:IHP
    {
      #region 构造函数
        public CNS200(int idNo = 0, string name = "NS200")
      {
          this._idNo = idNo;
          this._name = name;          
          com = new CSerialPort(_idNo, _name, EDataType.ASCII格式);

          C_HPCode = new Dictionary<int, string>();
          C_HPCode.Add(112, "STOP");
          C_HPCode.Add(115, "TESTING");
          C_HPCode.Add(116, "PASS");
          C_HPCode.Add(33, "HIGH FAIL");
          C_HPCode.Add(49, "HIGH FAIL");
          C_HPCode.Add(65, "HIGH FAIL");
          C_HPCode.Add(34, "LOW FAIL");
          C_HPCode.Add(50, "LOW FAIL");
          C_HPCode.Add(66, "LOW FAIL");
          C_HPCode.Add(35, "ARC FAIL");
          C_HPCode.Add(51, "ARC FAIL");
          C_HPCode.Add(36, "OCP");
          C_HPCode.Add(52, "OCP");
          C_HPCode.Add(68, "OCP");
          C_HPCode.Add(100, "OCP");
          C_HPCode.Add(97, "SHORT FAIL");
          C_HPCode.Add(98, "OPEN FAIL");
      }
      public override string ToString()
      {
          return _name;
      }
      #endregion

      #region 字段
      private int _idNo = 0;
      private string _name = "NS200";
      private bool _conStatus = false;
      private CSerialPort com = null;
      /// <summary>
      /// 负载通道
      /// </summary>
      private int _chanMax = 4;
      /// <summary>
      /// 测试产品数量
      /// </summary>
      private int _uutMax = 4;
      /// <summary>
      /// 测试步骤数量
      /// </summary>
      private int _stepNum = 9;
      /// <summary>
      /// 错误代码
      /// </summary>
      private Dictionary<int, string> C_HPCode = null;
      #endregion
      
      #region 属性
      /// <summary>
      /// 编号
      /// </summary>
      public int idNo
      {
          get { return _idNo; }
          set { _idNo = value; }
      }
      /// <summary>
      /// 名称
      /// </summary>
      public string name
      {
          get { return _name; }
          set { _name = value; }
      }
      /// <summary>
      /// 状态
      /// </summary>
      public bool conStatus
      {
          get { return _conStatus; }
      }
      /// <summary>
      /// 字节长度
      /// </summary>
      public int chanMax
      {
          get { return _chanMax; }
      }
      #endregion

      #region 方法
      /// <summary>
      /// 打开串口
      /// </summary>
      /// <param name="comName"></param>
      /// <param name="er"></param>
      /// <param name="setting"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er, string setting = "9600,N,8,1")
      {
          er = string.Empty;

          try
          {
              if (com != null)
              {
                  com.close();
                  com = null;
              }

              com = new CSerialPort(_idNo, _name, EDataType.ASCII格式);

              if (!com.open(comName, out er, setting))
                  return false;

              _conStatus = true;

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 关闭串口
      /// </summary>
      public void Close()
      {
          if (com == null)
              return;

          com.close();

          com = null;

          _conStatus = false;
      }
      /// <summary>
      /// 初始化设备
      /// </summary>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Init(out string er, int uutMax, int stepNum)
      {
          er = string.Empty;

          try
          {
              _uutMax = uutMax;

              string devName = string.Empty;

              if (!readIDN(out devName, out er))
                  return false;

              int LastIndex = devName.LastIndexOf(name);

              if (LastIndex < 0)
              {
                  er = "高压仪器" + "[" + name + "]" + "型号错误:" + devName;                       
                  return false;
              }

              if (!writeCmd("TESTOperation:MEASure", out er))
                  return false;

              if (!writeCmd("TESTOperation:LOCK", out er))  //测试失败停止
                  return false;

              //if (!writeCmd("PRETest:TESTfail:STOP0", out er))  //测试失败停止
              //    return false;

              //if (!writeCmd("PRETest:TESTfail:RESET0", out er)) //测试失败复位
              //    return false;

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 导入程序
      /// </summary>
      /// <param name="proName"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ImportProgram(string proName, out string er)
      {
          er = string.Empty;

          try
          {

              if (proName == string.Empty)
                  return true;

              string cmd = "LOAD:BUFFer" + proName;

              string rData = string.Empty;

              if (!writeCmd(cmd, out er))
              {
                  er = "程序" + "[" + proName + "]" + "不存在.";                       
                  return false;
              }

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 获取测试步骤
      /// </summary>
      /// <param name="stepName"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadStepName(out List<EStepName> stepName, out string er, int chan = 1)
      {
          er = string.Empty;

          stepName = new List<EStepName>();

          try
          {
              //获取步骤模式
              string cmd = "SAF:CHAN" + chan.ToString("D3") + ":RES:ALL:MODE?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              string[] stepModeList = rData.Split(',');

              for (int i = 0; i < stepModeList.Length; i++)
              {
                  switch (stepModeList[i])
                  {
                      case "AC":
                          stepName.Add(EStepName.AC);
                          break;
                      case "DC":
                          stepName.Add(EStepName.DC);
                          break;
                      case "IR":
                          stepName.Add(EStepName.IR);
                          break;
                      case "OSC":
                          stepName.Add(EStepName.OSC);
                          break;
                      default:
                          stepName.Add(EStepName.PA);
                          break;
                  }
              }
              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取测试步骤设置值
      /// </summary>
      /// <param name="stepNo"></param>
      /// <param name="rStepVal"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadStepSetting(int stepNo, out EStepName stepName, out List<double> stepVal, out string er)
      {
          stepName = EStepName.AC;

          stepVal = new List<double>();

          er = string.Empty;

          try
          {
              string cmd = "SAF:STEP" + stepNo.ToString() + ":SET?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              string[] stepList = rData.Split(',');

              if (stepList.Length < 4)
              {
                  er = "获取步骤数据错误:" + rData;
                  return false;
              }
              switch (stepList[2])
              {
                  case "AC":
                      stepName = EStepName.AC;
                      for (int i = 0; i < stepList.Length - 5; i++)
                          stepVal.Add(System.Convert.ToDouble(stepList[i + 3]));
                      if (stepVal.Count != CHPPara.C_ACItem.Length)
                      {
                          er = "获取步骤数据错误:" + rData;
                          return false;
                      }
                      stepVal[0] = stepVal[0] / 1000;//VOLTAGE
                      stepVal[1] = stepVal[1] * 1000;//HIGHT LIMIT
                      stepVal[2] = stepVal[2] * 1000;//LOW LIMIT
                      stepVal[3] = stepVal[3] * 1000;//ARC LIMIT
                      stepVal[4] = stepVal[4];//RAMP TIME 
                      stepVal[5] = stepVal[5];//TEST TIME                        
                      stepVal[6] = stepVal[6];//FALL TIME 
                      break;
                  case "DC":
                      stepName = EStepName.DC;
                      for (int i = 0; i < stepList.Length - 5; i++)
                          stepVal.Add(System.Convert.ToDouble(stepList[i + 3]));
                      if (stepVal.Count != CHPPara.C_DCItem.Length)
                      {
                          er = "获取步骤数据错误:" + rData;
                          return false;
                      }
                      stepVal[0] = stepVal[0] / 1000;//VOLTAGE
                      stepVal[1] = stepVal[1] * 1000;//HIGHT LIMIT
                      stepVal[2] = stepVal[2] * 1000;//LOW LIMIT
                      stepVal[3] = stepVal[3] * 1000;//ARC LIMIT
                      stepVal[4] = stepVal[4];//RAMP TIME 
                      stepVal[5] = stepVal[5];//DWELL TIME                        
                      stepVal[6] = stepVal[6];//TEST TIME 
                      stepVal[7] = stepVal[7]; //FALL TIME
                      break;
                  case "IR":
                      stepName = EStepName.IR;
                      for (int i = 0; i < stepList.Length - 7; i++)
                          stepVal.Add(System.Convert.ToDouble(stepList[i + 3]));
                      if (stepVal.Count != CHPPara.C_IRItem.Length)
                      {
                          er = "获取步骤数据错误:" + rData;
                          return false;
                      }
                      stepVal[0] = stepVal[0] / 1000;//VOLTAGE
                      stepVal[1] = stepVal[1] / 1000000;//LOW LIMIT
                      stepVal[2] = stepVal[2] / 1000000;//HIGHT LIMIT
                      stepVal[3] = stepVal[3];//RAMP TIME                    
                      stepVal[4] = stepVal[4];//TEST TIME 
                      stepVal[5] = stepVal[5]; //FALL TIME
                      break;
                  case "OSC":
                      stepName = EStepName.OSC;
                      for (int i = 0; i < stepList.Length - 5; i++)
                          stepVal.Add(System.Convert.ToDouble(stepList[i + 3]));
                      if (stepVal.Count != CHPPara.C_OSCItem.Length)
                      {
                          er = "获取步骤数据错误:" + rData;
                          return false;
                      }
                      stepVal[0] = stepVal[0] * 100;     //OPEN
                      stepVal[1] = stepVal[1] * 100;     //SHORT
                      break;
                  default:
                      stepName = EStepName.PA;
                      break;
              }
              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 设置高压通道
      /// </summary>
      /// <param name="chanList"></param>
      /// <returns></returns>
      public bool SetChanEnable(List<int> chanList, out string er)
      {
          er = string.Empty;

          try
          {
              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 设置测试步骤
      /// </summary>
      /// <param name="step"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetTestPara(List<CHPPara.CStep> step, out string er, string proName, bool saveToDev)
      {
          er = string.Empty;

          try
          {

              //加载
              if (saveToDev)
              {
                  if (!saveProgram(proName, out er))
                      return false;
              }

              //删除原有参数设置1-9

              string rData = string.Empty;

              string cmd = string.Empty;

              for (int i = 0; i < _stepNum; i++)
              {
                  cmd = "TESTset:STEP1:DELETE";

                  if (!writeCmd(cmd, out er))
                      return false;
              }

              for (int i = 0; i < step.Count; i++)
              {
                  string stepNo = (step[i].stepNo + 1).ToString();

                  double value = 0;

                  switch (step[i].name)
                  {
                      case EStepName.AC:
                          //ITEM
                          cmd = "TESTset:STEP" + stepNo + ":ITEM:AC";
                          if (!writeCmd(cmd, out er))
                              return false;
                          //VOLTAGE --KV
                          value = step[i].para[0].setVal;
                          if (value < 0.05)
                              value = 0.05;
                          if (value > 5)
                              value = 5;
                          cmd = "TESTset:STEP" + stepNo + ":VOLTage:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //HIGHT LIMIT
                          value = step[i].para[1].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":CURRentHL:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //LOW LIMIT
                          value = step[i].para[2].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":CURRentLL:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //ARC LIMIT
                          value = step[i].para[3].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":ARCCurrent:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //RAMP TIME 
                           value = step[i].para[4].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":TIMErise:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //TEST TIME           
                           value = step[i].para[5].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":TIMEtest:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //设定输出通道
                          cmd = "TESTset:STEP" + stepNo + ":CHANel:1234";
                          if (!writeCmd(cmd, out er))
                              return false;
                          //设置高压端口 -A:0000 B:0001
                          cmd = "TESTset:STEP" + stepNo + ":PORTHigh:0001";
                          if (!writeCmd(cmd, out er))
                              return false;
                          break;
                      case EStepName.DC:
                          //ITEM
                          cmd = "TESTset:STEP" + stepNo + ":ITEM:DC";
                          if (!writeCmd(cmd, out er))
                              return false;
                          //VOLTAGE
                           value = step[i].para[0].setVal;
                          if (value < 0.05)
                              value = 0.05;
                          if (value > 6)
                              value = 6;
                          cmd = "TESTset:STEP" + stepNo + ":VOLTage:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //HIGHT LIMIT
                          value = step[i].para[1].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":CURRentHL:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //LOW LIMIT
                          value = step[i].para[2].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":CURRentLL:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //ARC LIMIT
                           value = step[i].para[3].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":ARCCurrent:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //RAMP TIME 
                          value = step[i].para[4].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":TIMErise:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //TEST TIME 
                           value = step[i].para[6].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":TIMEtest:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //设定输出通道
                          cmd = "TESTset:STEP" + stepNo + ":CHANel:1234";
                          if (!writeCmd(cmd, out er))
                              return false;
                          //设置高压端口 -A:0000 B:0001
                          cmd = "TESTset:STEP" + stepNo + ":PORTHigh:0001";
                          if (!writeCmd(cmd, out er))
                              return false;
                          break;
                      case EStepName.IR:
                          //ITEM
                          cmd = "TESTset:STEP" + stepNo + ":ITEM:IR";
                          if (!writeCmd(cmd, out er))
                              return false;
                          //VOLTAGE
                          value = step[i].para[0].setVal;
                          if (value < 0.05)
                              value = 0.05;
                          if (value > 1)
                              value = 1;
                          cmd = "TESTset:STEP" + stepNo + ":VOLTage:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //LOW LIMIT
                          value = step[i].para[1].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":IRLL:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //HIGHT LIMIT
                          value = step[i].para[2].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":IRHL:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //RAMP TIME   
                           value = step[i].para[3].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":TIMErise:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //TEST TIME 
                           value = step[i].para[4].setVal;
                          cmd = "TESTset:STEP" + stepNo + ":TIMEtest:" + value.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //量程0-AUTO 1:固定
                          cmd = "TESTset:STEP" + stepNo + ":IRRange:0";
                          if (!writeCmd(cmd, out er))
                              return false;
                          //设定输出通道
                          cmd = "TESTset:STEP" + stepNo + ":CHANel:1234";
                          if (!writeCmd(cmd, out er))
                              return false;
                          //设置高压端口 -A:0000 B:0001
                          cmd = "TESTset:STEP" + stepNo + ":PORTHigh:0001";
                          if (!writeCmd(cmd, out er))
                              return false;
                          break;
                      case EStepName.OSC:
                          //ITEM
                          cmd = "TESTset:STEP" + stepNo + ":ITEM:OS";
                          if (!writeCmd(cmd, out er))
                              return false;
                          //OPEN
                          cmd = "TESTset:STEP" + stepNo + ":CAPALimited:" + step[i].para[0].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //SHORT
                          cmd = "TESTset:STEP" + stepNo + ":RESILimited:" + step[i].para[1].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //设定输出通道
                          cmd = "TESTset:STEP" + stepNo + ":CHANel:1234";
                          if (!writeCmd(cmd, out er))
                              return false;
                          //设置高压端口 -A:0000 B:0001
                          cmd = "TESTset:STEP" + stepNo + ":PORTHigh:0001";
                          if (!writeCmd(cmd, out er))
                              return false;
                          break;
                      case EStepName.GB:
                          break;
                      case EStepName.PA:
                          break;
                      default:
                          break;
                  }
              }

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 启动测试
      /// </summary>
      public bool Start(out string er)
      {
          er = string.Empty;

          try
          {

              if (!writeCmd("TESTOperation:TEST:1", out er))
                  return false;

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 停止测试
      /// </summary>
      public bool Stop(out string er)
      {
          if (!writeCmd("TESTOperation:TEST:0", out er))
              return false;

          return true;
      }
      /// <summary>
      /// 读取测试状态
      /// </summary>
      /// <param name="status"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadStatus(out EHPStatus status, out string er)
      {
          status = EHPStatus.IDLE;

          er = string.Empty;

          try
          {
              string info = string.Empty;

              string cmd = "*TESTStates?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              info = rData;

              if (rData.LastIndexOf("IDLE") >= 0)
              {
                  status = EHPStatus.IDLE;
              }
              else if (rData.LastIndexOf("BUSY") >= 0)
              {
                  status = EHPStatus.RUNNING;
              }
              else if(rData.LastIndexOf("OVER")>=0)
              {
                  status = EHPStatus.STOPPED;
              }

              er = rData;
              
              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取测试结果
      /// </summary>
      /// <param name="uutMax"></param>
      /// <param name="stepMax"></param>
      /// <param name="uut"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadResult(int uutMax, int stepMax, out List<CCHResult> uut, out string er)
      {
          er = string.Empty;

          uut = new List<CCHResult>();

          try
          {
              //测试通道数据
              for (int uutNo = 0; uutNo < uutMax; uutNo++)
              {
                  uut.Add(new CCHResult());
                  for (int i = 0; i < stepMax; i++)
                      uut[uutNo].Step.Add(new CStepResult());
              }

             //Thread.Sleep(2000);

              string rData = string.Empty;

              string cmd = "TESTResult:ALLGet";

              if (!sendCmdToHP(cmd, out rData, out er))
                 return false;
              
              //获取测试结果

              int count = rData.IndexOf("NS200:");

              if (count < 0)
              {
                  er = rData;
                  return false;
              }

              rData = rData.Substring(count, rData.Length - count);

              //if (rData.LastIndexOf("NS200:OK") < 0)
              //{
              //    er = rData;
              //    return false;
              //}

              er = rData;

              string rVal = rData.Replace("NS200:","");

              rVal = rVal.Replace("\n", "");

              string[] stepResultList = rVal.Split((char)13);

              int stepNo = 0;

              for (int i = 0; i < stepResultList.Length; i++)
              {
                  if (stepResultList[i].LastIndexOf("STEP") >= 0)
                    stepNo++;
              }

              for (int i = 0; i < stepNo; i++)
              {
                  string[] str1 = stepResultList[i].Split(';');

                  if (str1.Length != _chanMax + 1)
                      return false;

                  string strValue = string.Empty;

                  double value = 0;

                  string unit = string.Empty;

                  //步骤及电压值STEP1:V:3.300kV
                  string[] str2 = str1[0].Split(':');

                  if (str2.Length != 3)
                      return false;

                  if (!GetValueAndUnit(str2[2], out value, out unit))
                      return false;

                  //测试步骤解析CH1: 4.731uA,L;CH2: 0.000uA,L;CH3:35.631uA,L;CH4:25.892uA,L

                  for (int ch = 0; ch < _chanMax; ch++)
                  {
                      string str3 = str1[ch + 1];

                      string[] str4 = str3.Split(':');

                      string strCH = str4[0].Replace("CH", "");

                      int uutNo = System.Convert.ToInt16(strCH) - 1;

                      string[] str5 = str4[1].Split(',');

                      if (str5.Length > 1)
                      {
                          if (!GetValueAndUnit(str5[0], out value, out unit))
                              return false;

                          uut[uutNo].Step[i].Code = str5[1];

                          uut[uutNo].Step[i].Value = value;

                          uut[uutNo].Step[i].Unit = unit;

                          if (uut[uutNo].Step[i].Code == "T")
                          {
                              uut[uutNo].Step[i].Result = 0;
                          }
                          else
                          {
                              uut[uutNo].Step[i].Result = 1;
                              uut[uutNo].Result = 1;
                          }
                      }
                      else
                      {
                          if (str5[0] == "NG")
                          {
                              uut[uutNo].Step[i].Code = str5[0];
                              uut[uutNo].Step[i].Result = 1;
                              uut[uutNo].Result = 1;
                          }
                      }

                  }
              }
  
              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取测试结果
      /// </summary>
      /// <param name="chan"></param>
      /// <param name="chanResult"></param>
      /// <param name="stepResult"></param>
      /// <param name="stepCode"></param>
      /// <param name="stepMode"></param>
      /// <param name="stepVal"></param>
      /// <param name="stepUnit"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadResult(int chan, out int chanResult,
                             out List<int> stepResult, out List<string> stepCode,
                             out List<EStepName> stepMode, out List<double> stepVal,
                             out List<string> stepUnit, out string er)
      {

          chanResult = 0;

          stepResult = new List<int>();

          stepCode = new List<string>();

          stepMode = new List<EStepName>();

          stepVal = new List<double>();

          stepUnit = new List<string>();


          er = string.Empty;

          try
          {
              //获取测试结果

              string cmd = "SAF:CHAN" + chan.ToString("D3") + ":RES:ALL?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              string[] stepResultList = rData.Split(',');

              int stepNo = stepResultList.Length;

              for (int i = 0; i < stepNo; i++)
              {
                  int resultCode = System.Convert.ToInt32(stepResultList[i]);

                  if (C_HPCode.ContainsKey(resultCode))
                  {
                      if (C_HPCode[resultCode] == "PASS")
                          stepResult.Add(0);
                      else
                      {
                          chanResult = resultCode;
                          stepResult.Add(resultCode);
                      }
                      stepCode.Add(C_HPCode[resultCode]);
                  }
                  else
                  {
                      chanResult = 1;
                      stepResult.Add(-1);
                      stepCode.Add("CODE ERROR");
                  }
              }

              //获取步骤模式
              cmd = "SAF:CHAN" + chan.ToString("D3") + ":RES:ALL:MODE?";

              rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              string[] stepModeList = rData.Split(',');

              for (int i = 0; i < stepModeList.Length; i++)
              {
                  switch (stepModeList[i])
                  {
                      case "AC":
                          stepMode.Add(EStepName.AC);
                          break;
                      case "DC":
                          stepMode.Add(EStepName.DC);
                          break;
                      case "IR":
                          stepMode.Add(EStepName.IR);
                          break;
                      case "OSC":
                          stepMode.Add(EStepName.OSC);
                          break;
                      default:
                          stepMode.Add(EStepName.PA);
                          break;
                  }
              }

              //获取测试数据

              cmd = "SAF:CHAN" + chan.ToString("D3") + ":RES:ALL:MMET?";

              rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              string[] stepValList = rData.Split(',');

              for (int i = 0; i < stepValList.Length; i++)
              {
                  double stepData = System.Convert.ToDouble(stepValList[i]);

                  switch (stepMode[i])
                  {
                      case EStepName.AC:
                          stepVal.Add(stepData * System.Math.Pow(10, 3));
                          stepUnit.Add("mA");
                          break;
                      case EStepName.DC:
                          stepVal.Add(stepData * System.Math.Pow(10, 3));
                          stepUnit.Add("mA");
                          break;
                      case EStepName.IR:
                          double R = stepData / System.Math.Pow(10, 6);
                          if (R < 1000)
                          {
                              stepVal.Add(R);
                              stepUnit.Add("M0hm");
                          }
                          else
                          {
                              stepVal.Add(R / 1000);
                              stepUnit.Add("G0hm");
                          }
                          break;
                      case EStepName.OSC:
                          stepVal.Add(stepData * System.Math.Pow(10, 9));
                          stepUnit.Add("nF");
                          break;
                      case EStepName.GB:
                          break;
                      case EStepName.PA:
                          break;
                      default:
                          break;
                  }
              }
              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 写入命令
      /// </summary>
      /// <param name="wCmd"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool WriteCmd(string wCmd, out string er)
      {
          er = string.Empty;

          try
          {
              if (!writeCmd(wCmd, out er))
                  return false;

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读取命令
      /// </summary>
      /// <param name="wCmd"></param>
      /// <param name="rData"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadCmd(string wCmd, out string rData, out string er)
      {
          er = string.Empty;

          rData = string.Empty;

          try
          {
              if (!sendCmdToHP(wCmd, out rData, out er))
                  return false;

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 分解数据
      /// </summary>
      /// <param name="strValue"></param>
      /// <param name="value"></param>
      /// <param name="unit"></param>
      /// <returns></returns>
      private bool GetValueAndUnit(string strValue, out double value, out string unit)
      {
          value = 0;

          unit = "";

          try
          {
              string str = string.Empty;

              for (int i = 0; i < strValue.Length; i++)
              {
                  char c = System.Convert.ToChar(strValue.Substring(i, 1));

                  if (Char.IsLetterOrDigit(c) || Char.IsNumber(c) || (c==(char)46))
                       str+=strValue.Substring(i, 1);
              }
              
              string s = string.Empty;

              for (int i = 0; i < str.Length; i++)
              {
                  char c = System.Convert.ToChar(str.Substring(i, 1));

                  if (!Char.IsNumber(c) && (c!=(char)46))
                      break;

                  s += str.Substring(i, 1);                  
              }

              value = System.Convert.ToDouble(s);

              unit = str.Substring(s.Length, str.Length - s.Length);

              return true;
          }
          catch (Exception)
          {
              return false;
          }
      }
      #endregion

      #region 仪器通信
      /// <summary>
      /// 读取仪器设备
      /// </summary>
      /// <param name="devName"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      private bool readIDN(out string devName, out string er)
      {
          devName = string.Empty;

          er = string.Empty;

          try
          {
              string cmd = "*IDN?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              er = rData;

              string[] cmdList = rData.Split('\r');

              if (cmdList.Length < 1)
                  return false;

              string[] valList = cmdList[0].Split(':');

              if (valList.Length < 2)
                  return false;

              devName = valList[0];

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 读错误信息
      /// </summary>
      /// <param name="errCode"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      private bool readError(out string errCode, out string er)
      {
          errCode = string.Empty;

          er = string.Empty;

          try
          {

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }

      }
      /// <summary>
      /// 远程控制
      /// </summary>
      /// <param name="er"></param>
      /// <returns></returns>
      private bool remote(out string er)
      {
          er = string.Empty;

          try
          {

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 保存机种文件到高压机种
      /// </summary>
      /// <param name="proName"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      private bool saveProgram(string proName, out string er)
      {
          er = string.Empty;

          try
          {
              if (proName == string.Empty)
              {
                  proName = "1";
              }
              else
              {
                  for (int i = 0; i < proName.Length; i++)
                  {
                      char c = System.Convert.ToChar(proName.Substring(i, 1));

                      if (!Char.IsNumber(c))
                      {
                          proName = "1";
                          break;
                      }
                  }
              }

              er = string.Empty;

              string rData = string.Empty;

              string cmd = "LOAD:BUFFer" + proName;

              if (!writeCmd(cmd, out er))
                  return false;

              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }
      }
      /// <summary>
      /// 写命令
      /// </summary>
      /// <param name="wCmd"></param>
      /// <param name="delayMs"></param>
      private bool writeCmd(string wCmd, out string er, int delayMs = 5, int timeOutMs = 2000)
      {
          er = string.Empty;

          try
          {
              string rData = string.Empty;

              er = string.Empty;

              if (!sendCmdToHP(wCmd, out rData, out er))
                  return false;

              er = rData;

              if (rData.IndexOf("NS200:OK") >= 0)
                  return true;

              return false;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }          
      }
      /// <summary>
      /// 发送和接收数据
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wData"></param>
      /// <param name="rLen"></param>
      /// <param name="rData"></param>
      /// <param name="er"></param>
      /// <param name="wTimeOut"></param>
      /// <returns></returns>
      private bool sendCmdToHP(string wData, out string rData, out string er, string rEOI = "\r\n", int wTimeOut = 2000)
      {
          rData = string.Empty;

          er = string.Empty;

          try
          {
              Thread.Sleep(30);

              string recvData = string.Empty;

              wData = "{" + wData +  "}";

              if (!com.send(wData, rEOI, out rData, out er, wTimeOut))
                  return false;
              
              return true;
          }
          catch (Exception ex)
          {
              er = ex.ToString();
              return false;
          }         
      }
      #endregion
    }
}
