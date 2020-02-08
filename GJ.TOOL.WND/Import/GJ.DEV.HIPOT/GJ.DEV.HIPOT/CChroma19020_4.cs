using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;

namespace GJ.DEV.HIPOT
{
    /// <summary>
    /// 10通道高压机
    /// </summary>
    public class CChroma19020_4:IHP
    {
      #region 构造函数
      public CChroma19020_4(int idNo = 0, string name = "Chroma19020")
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
      private string _name = "Chroma19020";
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
      private int _stepNum = 1;
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
      public bool Open(string comName, out string er, string setting = "9600,n,8,1")
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
              this._uutMax = uutMax;

              this._stepNum = stepNum;

              this._uutMax = 4;

              this._stepNum = 1;

              string devName = string.Empty;

              if (!readIDN(out devName, out er))
                  return false;

              int LastIndex = devName.LastIndexOf(name);

              if (LastIndex < 0)
              {
                  er = "高压仪器" + "[" + name + "]" + "型号错误:" + devName;                       
                  return false;
              }

              if (!remote(out er))
                  return false;

              if (!writeCmd("*CLS", out er))  //清除错误
                  return false;

              if (!writeCmd("SYST:TCON:FAIL:OPER CONT", out er)) //不良继续测试
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

              string cmd = "MEM:STAT:DEF? \"" + proName + "\"";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
              {
                  er = "程序" + "[" + proName + "]" + "不存在.";                       
                  return false;
              }

              int proNo = System.Convert.ToInt32(rData);

              if (!writeCmd("*RCL " + proNo, out er))
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
              if (chanList.Count == 0)
              {
                  er = "无可设置通道";
                  return false;
              }

              string chanStr = string.Empty;

              for (int i = 0; i < chanList.Count; i++)
              {
                  if (i < chanList.Count - 1)
                      chanStr += chanList[i].ToString("D3") + ",";
                  else
                      chanStr += chanList[i].ToString("D3");
              }

              string cmd = "SYST:TCON:CHAN (@" + chanStr + ")";

              return writeCmd(cmd, out er);
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
              //删除原有参数设置

              string cmd = "SAF:SNUMB?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              if (!writeCmd("*CLS", out er))  //清除错误
                  return false;

              int stepNum = System.Convert.ToInt32(rData);

              for (int i = 1; i < stepNum + 1; i++)
                  if (!writeCmd("SAF:STEP1:DEL", out er))
                      return false;

              //加载新的参数设置

              stepNum = 0;

              for (int i = 0; i < step.Count; i++)
              {
                  string stepNo = (step[i].stepNo + 1).ToString();

                  switch (step[i].name)
                  {
                      case EStepName.AC:
                          //VOLTAGE
                          cmd = "SAF:STEP" + stepNo + ":AC " + ((int)(step[i].para[0].setVal * 1000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //HIGHT LIMIT
                          cmd = "SAF:STEP" + stepNo + ":AC:LIM:HIGH " + ((step[i].para[1].setVal / 1000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //LOW LIMIT
                          cmd = "SAF:STEP" + stepNo + ":AC:LIM:LOW " + ((step[i].para[2].setVal / 1000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //ARC LIMIT
                          cmd = "SAF:STEP" + stepNo + ":AC:LIM:ARC " + ((step[i].para[3].setVal / 1000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //RAMP TIME 
                          cmd = "SAF:STEP" + stepNo + ":AC:TIME:RAMP " + step[i].para[4].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //TEST TIME           
                          cmd = "SAF:STEP" + stepNo + ":AC:TIME " + step[i].para[5].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //FALL TIME 
                          cmd = "SAF:STEP" + stepNo + ":AC:TIME:FALL " + step[i].para[6].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          break;
                      case EStepName.DC:
                          //VOLTAGE
                          cmd = "SAF:STEP" + stepNo + ":DC " + ((int)(step[i].para[0].setVal * 1000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //HIGHT LIMIT
                          cmd = "SAF:STEP" + stepNo + ":DC:LIM:HIGH " + ((step[i].para[1].setVal / 1000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //LOW LIMIT
                          cmd = "SAF:STEP" + stepNo + ":DC:LIM:LOW " + ((step[i].para[2].setVal / 1000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //ARC LIMIT
                          cmd = "SAF:STEP" + stepNo + ":DC:LIM:ARC " + ((step[i].para[3].setVal / 1000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //RAMP TIME 
                          cmd = "SAF:STEP" + stepNo + ":DC:TIME:RAMP " + step[i].para[4].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //DWELL TIME     
                          cmd = "SAF:STEP" + stepNo + ":DC:TIME:DWEL " + step[i].para[5].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //TEST TIME 
                          cmd = "SAF:STEP" + stepNo + ":DC:TIME " + step[i].para[6].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //FALL TIME
                          cmd = "SAF:STEP" + stepNo + ":DC:TIME:FALL " + step[i].para[7].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          break;
                      case EStepName.IR:
                          //VOLTAGE
                          cmd = "SAF:STEP" + stepNo + ":IR " + ((int)(step[i].para[0].setVal * 1000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //LOW LIMIT
                          cmd = "SAF:STEP" + stepNo + ":IR:LIM:LOW " + ((int)(step[i].para[1].setVal * 1000000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //HIGHT LIMIT
                          cmd = "SAF:STEP" + stepNo + ":IR:LIM:HIGH " + ((int)(step[i].para[2].setVal * 1000000)).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //RAMP TIME   
                          cmd = "SAF:STEP" + stepNo + ":IR:TIME:RAMP " + step[i].para[3].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //TEST TIME 
                          cmd = "SAF:STEP" + stepNo + ":IR:TIME " + step[i].para[4].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //FALL TIME
                          cmd = "SAF:STEP" + stepNo + ":IR:TIME:FALL " + step[i].para[5].setVal.ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          break;
                      case EStepName.OSC:
                          //OPEN
                          cmd = "SAF:STEP" + stepNo + ":OSC:LIM:OPEN " + (step[i].para[0].setVal / 100).ToString();
                          if (!writeCmd(cmd, out er))
                              return false;
                          //SHORT
                          cmd = "SAF:STEP" + stepNo + ":OSC:LIM:SHOR " + (step[i].para[1].setVal / 100).ToString();
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
              if (saveToDev)
              {
                  if (!saveProgram(proName, out er))
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
      /// 启动测试
      /// </summary>
      public bool Start(out string er)
      {
          er = string.Empty;

          try
          {
              if (!writeCmd("*CLS", out er))  //清除错误
                  return false;

              if (!writeCmd("SAF:STAR", out er))
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
          return writeCmd("SAF:STOP", out er);
      }
      /// <summary>
      /// 读取测试状态
      /// </summary>
      /// <param name="status"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadStatus(out EHPStatus status, out string er)
      {
          status = EHPStatus.STOPPED;

          er = string.Empty;

          try
          {
              string cmd = "SAF:STAT?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;
              if (rData == EHPStatus.RUNNING.ToString())
                  status = EHPStatus.RUNNING;
              else
                  status = EHPStatus.STOPPED;
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

              //获取测试结果

              for (int uutNo = 0; uutNo < uutMax; uutNo++)
              {
                  string cmd = "SAF:CHAN" + (uutNo+1).ToString("D3") + ":RES:ALL?";

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
                          {
                              uut[uutNo].Step[i].Result = 0;
                          }
                          else
                          {
                              uut[uutNo].Result = resultCode;
                              uut[uutNo].Step[i].Result = resultCode;
                          }
                          uut[uutNo].Step[i].Code = C_HPCode[resultCode];
                      }
                      else
                      {
                          uut[uutNo].Result = resultCode;
                          uut[uutNo].Step[i].Result = resultCode;
                          uut[uutNo].Step[i].Code = "CODE ERROR";
                      }
                  }

                  //获取步骤模式
                  cmd = "SAF:CHAN" + (uutNo+1).ToString("D3") + ":RES:ALL:MODE?";

                  rData = string.Empty;

                  if (!sendCmdToHP(cmd, out rData, out er))
                      return false;

                  string[] stepModeList = rData.Split(',');

                  for (int i = 0; i < stepModeList.Length; i++)
                  {
                      switch (stepModeList[i])
                      {
                          case "AC":
                              uut[uutNo].Step[i].Name = EStepName.AC;
                              break;
                          case "DC":
                              uut[uutNo].Step[i].Name = EStepName.DC;
                              break;
                          case "IR":
                              uut[uutNo].Step[i].Name = EStepName.IR;
                              break;
                          case "OSC":
                              uut[uutNo].Step[i].Name = EStepName.OSC;
                              break;
                          default:
                              break;
                      }
                  }

                  //获取测试数据

                  cmd = "SAF:CHAN" + (uutNo+1).ToString("D3") + ":RES:ALL:MMET?";

                  rData = string.Empty;

                  if (!sendCmdToHP(cmd, out rData, out er))
                      return false;

                  string[] stepValList = rData.Split(',');

                  for (int i = 0; i < stepValList.Length; i++)
                  {
                      double stepData = System.Convert.ToDouble(stepValList[i]);

                      switch (uut[uutNo].Step[i].Name)
                      {
                          case EStepName.AC:
                              uut[uutNo].Step[i].Value = stepData * System.Math.Pow(10, 3);
                              uut[uutNo].Step[i].Unit = "mA";
                              break;
                          case EStepName.DC:
                              uut[uutNo].Step[i].Value = stepData * System.Math.Pow(10, 3);
                              uut[uutNo].Step[i].Unit = "mA";
                              break;
                          case EStepName.IR:
                              double R = stepData / System.Math.Pow(10, 6);
                              if (R < 1000)
                              {
                                  uut[uutNo].Step[i].Value = R;
                                  uut[uutNo].Step[i].Unit = "M0hm";
                              }
                              else
                              {
                                  uut[uutNo].Step[i].Value = R/1000;
                                  uut[uutNo].Step[i].Unit = "G0hm";
                              }
                              break;
                          case EStepName.OSC:
                              uut[uutNo].Step[i].Value = stepData * System.Math.Pow(10, 9);
                              uut[uutNo].Step[i].Unit = "nF";
                              break;
                          default:
                              break;
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
              string[] valList = rData.Split(',');
              if (valList.Length < 2)
                  return false;
              devName = valList[0] + valList[1];
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
              string cmd = "SYST:ERR?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              string[] codeList = rData.Split(',');

              int code = System.Convert.ToInt32(codeList[0]);

              if (code == 0)
                  errCode = "OK";
              else
                  errCode = "错误信息:" + codeList[1].Replace("\"", "") + "-" + code.ToString();

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
              string cmd = "SYST:LOCK:REQ?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;
              if (rData != "1")
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
                  return true;

              er = string.Empty;

              string cmd = "MEM:STAT:DEF? \"" + proName + "\"";

              string rData = string.Empty;

              int proNo = 0;

              if (sendCmdToHP(cmd, out rData, out er)) //程序名存在
              {
                  proNo = System.Convert.ToInt32(rData);

                  if (!writeCmd("*SAV " + proNo, out er))
                      return false;

                  if (!writeCmd("MEM:STAT:DEF \"" + proName + "\"," + proNo, out er))
                      return false;

              }
              else
              {
                  //查询存储空间
                  cmd = "MEM:NST?";
                  if (!sendCmdToHP(cmd, out rData, out er))
                      return false;
                  int maxProNo = System.Convert.ToInt32(rData);

                  //查询可用位置
                  cmd = "MEM:FREE:STAT?";
                  if (!sendCmdToHP(cmd, out rData, out er))
                      return false;
                  int proIndex = System.Convert.ToInt16(rData);

                  proNo = maxProNo - proIndex;

                  if (proNo == 0)
                  {
                      er = "高压机无可用存储程序,请删除多余程序";
                      return false;
                  }

                  writeCmd("*SAV " + proNo, out er);

                  writeCmd("MEM:STAT:DEF \"" + proName + "\"," + proNo, out er);
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
      /// 写命令
      /// </summary>
      /// <param name="wCmd"></param>
      /// <param name="delayMs"></param>
      private bool writeCmd(string wCmd, out string er, int delayMs = 5, int timeOutMs = 500)
      {
          er = string.Empty;

          try
          {
              string rData = string.Empty;

              er = string.Empty;

              sendCmdToHP(wCmd, out rData, out er, string.Empty);

              System.Threading.Thread.Sleep(delayMs);

              int doReady = 0;

              int waitTimes = System.Environment.TickCount;

              do
              {
                  if (!sendCmdToHP("*OPC?", out rData, out er))
                      continue;
                  doReady = System.Convert.ToInt32(rData);
              }
              while (doReady == 0 && System.Environment.TickCount - waitTimes < timeOutMs);

              if (doReady == 0)
              {
                  er = "写命令超时:" + wCmd;
                  return false;
              }

              string errcode = string.Empty;

              if (!readError(out errcode, out er))
                  return false;

              if (errcode != "OK")
              {
                  er = "写命令错误:" + "[" + wCmd + "]" + errcode;
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
      /// 发送和接收数据
      /// </summary>
      /// <param name="wAddr"></param>
      /// <param name="wData"></param>
      /// <param name="rLen"></param>
      /// <param name="rData"></param>
      /// <param name="er"></param>
      /// <param name="wTimeOut"></param>
      /// <returns></returns>
      private bool sendCmdToHP(string wData, out string rData, out string er, string rEOI = "\r\n", int wTimeOut = 1000)
      {
          rData = string.Empty;

          er = string.Empty;

          try
          {
              string recvData = string.Empty;
              wData += "\r\n";
              //wData += "\n";
              if (!com.send(wData, rEOI, out rData, out er, wTimeOut))
                  return false;
              rData = rData.Replace("\r", "");
              rData = rData.Replace("\n", "");
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
