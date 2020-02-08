using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GJ.DEV.COM;
namespace GJ.DEV.HIPOT
{
    public class CTH9010:IHP
    {
      #region 构造函数
      public CTH9010(int idNo = 0, string name = "TH9010")
      {
          this._idNo = idNo;
          this._name = name;          
          com = new CSerialPort(_idNo, _name, EDataType.ASCII格式);
      }
      public override string ToString()
      {
          return _name;
      }
      #endregion

      #region 字段
      private int _idNo = 0;
      private string _name = "CTH9010";
      private bool _conStatus = false;
      private CSerialPort com = null;
      /// <summary>
      /// 负载通道
      /// </summary>
      private int _chanMax = 8;
      /// <summary>
      /// 测试产品数量
      /// </summary>
      private int _uutMax = 8;
      /// <summary>
      /// 测试步骤数量
      /// </summary>
      private int _stepNum = 1;
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

          _uutMax = uutMax;

          _stepNum = stepNum;

          try
          {
              string devName = string.Empty;

              if (!readIDN(out devName, out er))
                  return false;

              int LastIndex = devName.LastIndexOf(name);

              if (LastIndex < 0)
              {
                  er = "高压仪器" + "[" + name + "]" + "型号错误:" + devName;                       
                  return false;
              }

              //判断是否在测试界面?
              string rData = string.Empty;

              if (sendCmdToHP("DISPlay:PAGE?", out rData, out er))
              {
                  if (rData != "Meas")
                  {
                      if (!writeCmd("DISPlay:PAGE MEASurement", out er))
                          return false;
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
              {
                  er = "文件序号不存在";
                  return false;
              }

              int FileNo = 0;

              for (int i = 0; i < proName.Length; i++)
              {
                  char s = System.Convert.ToChar(proName.Substring(i, 1));

                  if (!Char.IsNumber(s))
                  {
                      er = "文件序号必须为数字[1-20]";
                      return true;
                  }
              }

              FileNo = System.Convert.ToInt16(proName);

              if (FileNo < 1 || FileNo > 20)
              {
                  er = "文件序号必须为[1-20]";
                  return true;
              }

              string cmd = "MMEM:LOAD " + FileNo.ToString();

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

          string rData = string.Empty;

          try
          {
              //删除原有参数设置

              Stop(out er);

              System.Threading.Thread.Sleep(50);

              if (sendCmdToHP("DISPlay:PAGE?", out rData, out er))
              {
                  if (rData != "Mset")
                  {
                      if (!writeCmd("DISPlay:PAGE MSETup", out er))
                          return false;
                  }
              }
             
              System.Threading.Thread.Sleep(1000);
              
              string wPara = string.Empty;

              string wCmd = "FETCH:AUTO OFF";
              if (!writeCmd(wCmd, out er))  //关闭自动回复
                  return false;

               wCmd = "FUNC:SOUR:STEP NEW";
              if (!writeCmd(wCmd, out er))  //清除错误
                  return false;

              System.Threading.Thread.Sleep(1000);

              //加载新的参数设置

              for (int i = 0; i < step.Count; i++)
              {
                  if (i > 0)
                  {
                      wCmd = "FUNC:SOUR:STEP INS";
                      if (!writeCmd(wCmd, out er)) return false;
                  }

                  System.Threading.Thread.Sleep(500);

                  string stepNo = (i + 1).ToString();

                  switch (step[i].name)
                  {
                      case EStepName.AC:
                          //VOLTAGE
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":AC:VOLT";
                          wPara=((int)(step[i].para[0].setVal * 1000)).ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //HIGHT LIMIT
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":AC:UPPC";
                          wPara = step[i].para[1].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //LOW LIMIT
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":AC:LOWC";
                          wPara = step[i].para[2].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //ARC LIMIT
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":AC:ARC";
                          wPara = step[i].para[3].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //RAMP TIME 
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":AC:RTIM";
                          wPara = step[i].para[4].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //TEST TIME           
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":AC:TTIM";
                          wPara = step[i].para[5].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //FALL TIME 
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":AC:FTIM";
                          wPara = step[i].para[6].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                         
                          break;
                      case EStepName.DC:
                          //VOLTAGE
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":DC:VOLT";
                          wPara=((int)(step[i].para[0].setVal * 1000)).ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //HIGHT LIMIT
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":DC:UPPC";
                          wPara = step[i].para[1].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //LOW LIMIT
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":DC:LOWC";
                          wPara = step[i].para[2].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //ARC LIMIT
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":DC:ARC";
                          wPara = step[i].para[3].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //RAMP TIME 
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":DC:RTIM";
                          wPara = step[i].para[4].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //DWELL TIME     
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":DC:WTIM";
                          wPara = step[i].para[5].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //TEST TIME 
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":DC:TTIM";
                          wPara = step[i].para[6].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //FALL TIME
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":DC:FTIM";
                          wPara = step[i].para[7].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          break;
                      case EStepName.IR:
                          //VOLTAGE
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":IR:VOLT";
                          wPara = ((int)(step[i].para[0].setVal * 1000)).ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //LOW LIMIT
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":IR:LOWC";
                          wPara =step[i].para[1].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //HIGHT LIMIT
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":IR:UPPC";
                          wPara = step[i].para[2].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //RAMP TIME   
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":IR:RTIM";
                          wPara = step[i].para[3].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //TEST TIME 
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":IR:TTIM";
                          wPara = step[i].para[4].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //FALL TIME
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":IR:FTIM";
                          wPara = step[i].para[5].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          break;
                      case EStepName.OSC:
                          //OPEN
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":OS:OPEN";
                          wPara = step[i].para[0].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
                              return false;
                          //SHORT
                          wCmd = "FUNC:SOUR:STEP " + stepNo + ":OS:SHOT";
                          wPara = step[i].para[1].setVal.ToString();
                          if (!writePara(wCmd, wPara, out er))
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

              //设置为测试界面
              if (sendCmdToHP("DISPlay:PAGE?", out rData, out er))
              {
                  if (rData != "Meas")
                  {
                      if (!writeCmd("DISPlay:PAGE MEASurement", out er))
                          return false;
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

          string rData = string.Empty;

          try
          {
              if (!writeCmd("FUNC:STARt", out er))        
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
          er = string.Empty;
          return writeCmd("FUNC:STOP", out er);
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
              string cmd = "FETCh?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              string[] StepArray = rData.Split((char)32);  //空格

              if (StepArray[0] == "BUSY")
              {
                  status = EHPStatus.RUNNING; 
              }
              else
              {
                  status = EHPStatus.STOPPED; 
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
              string cmd = "FETCh?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              string[] StepArray = rData.Split((char)32);  //空格

              if (StepArray.Length < stepMax)
              {
                  er = "接收数据步骤错误:" + rData;
                  return false;
              }

              for (int i = 0; i < stepMax; i++)
              {
                  string[] StrName = StepArray[i].Split(':');

                  string StepName = StrName[0];  //STEP1,2,3..

                  if (StepName.Substring(0, 4) != "STEP")
                  {
                      er = "测试步骤名称错误:" + rData;
                      return false;
                  }

                  string ItemName = StrName[1];  //AC,DC,IR

                  EStepName eItemName = EStepName.AC;

                  switch (ItemName)
                  {
                      case "AC":
                          eItemName = EStepName.AC;
                          break;
                      case "DC":
                          eItemName = EStepName.DC;
                          break;
                      case "IR":
                          eItemName = EStepName.IR;
                          break;
                      case "OS":
                          eItemName = EStepName.OSC;
                          break;
                      default:
                          er = "测试项目错误:" + rData;
                          return false;
                  }

                  string ItemVal = StrName[2];

                  string[] CHVals = ItemVal.Split(';');

                  if (CHVals.Length < uutMax)
                  {
                      er = "测试通道数错误:" + rData;
                      return false;
                  }

                  for (int z = 0; z < uutMax; z++)  //产品数据
                  {
                      string[] uutVals = CHVals[z].Split(',');

                      if (uutVals.Length < 4)
                      {
                          er = "测试结果错误:" + rData;
                          return false;
                      }

                      int CH = System.Convert.ToInt16(uutVals[0]);

                      double Volt = System.Convert.ToDouble(uutVals[1]);

                      double value = System.Convert.ToDouble(uutVals[2]);

                      string Result = uutVals[3];

                      if (Result == "PASS")
                      {
                          uut[z].Step[i].Result = 0;
                          uut[z].Step[i].Code = Result;
                      }
                      else
                      {
                          uut[z].Result = 1; 
                          uut[z].Step[i].Result = 1;
                          uut[z].Step[i].Code = Result;
                      }

                      uut[z].Step[i].Name = eItemName;

                      switch (eItemName)
                      {
                          case EStepName.AC:
                              uut[z].Step[i].Value = value;
                              uut[z].Step[i].Unit = "mA";
                              break;
                          case EStepName.DC:
                              uut[z].Step[i].Value = value;
                              uut[z].Step[i].Unit = "mA";
                              break;
                          case EStepName.IR:
                              if (value < 1000)
                              {
                                  uut[z].Step[i].Value = value;
                                  uut[z].Step[i].Unit = "M0hm";
                              }
                              else
                              {
                                  uut[z].Step[i].Value = value/1000;
                                  uut[z].Step[i].Unit = "G0hm";
                              }
                              break;
                          case EStepName.OSC:
                              uut[z].Step[i].Value = value;
                              uut[z].Step[i].Unit = "nF";
                              break;
                          default:
                              uut[z].Step[i].Value = value;
                              uut[z].Step[i].Unit = "NA";
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
              string cmd = "FETCh?";

              string rData = string.Empty;

              if (!sendCmdToHP(cmd, out rData, out er))
                  return false;

              string[] StepArray = rData.Split((char)32);  //空格

              if (StepArray.Length < _stepNum)
              {
                  er = "接收数据步骤错误:" + rData;
                  return false;              
              }

              for (int i = 0; i < StepArray.Length; i++)
              {
                  string[] StrName = StepArray[i].Split(':');

                  string StepName = StrName[0];  //STEP1,2,3..

                  if (StepName.Substring(0, 4) != "STEP")
                  {
                      er = "测试步骤名称错误:" + rData;
                      return false;
                  }

                  string ItemName = StrName[1];  //AC,DC,IR

                  if (!Enum.IsDefined(typeof(EStepName), ItemName))
                  {
                      er = "测试项目错误:" + rData;
                      return false;
                  }

                  EStepName eItemName = (EStepName)Enum.Parse(typeof(EStepName), ItemName);

                  string ItemVal = StrName[2];

                  string[] CHVals = ItemVal.Split(';');

                  string[] _CHVals = new string[1] { "" };

                  string[] CHVals2 = CHVals.Except(_CHVals).ToArray();

                  if (CHVals2.Length != _uutMax)
                  {
                      er = "测试通道数错误:" + rData;
                      return false;
                  }

                  for (int z = 0; z < CHVals2.Length; z++)  //产品数据
                  {
                      string[] uutVals = CHVals2[z].Split(',');

                      if (uutVals.Length < 4)
                      {
                          er = "测试结果错误:" + rData;
                          return false;
                      }

                      int CH = System.Convert.ToInt32(uutVals[0]);

                      double Volt = System.Convert.ToDouble(uutVals[1]);

                      double value = System.Convert.ToDouble(uutVals[2]);

                      string Result = uutVals[3];

                      if (chan != CH)
                          continue;

                      if (Result == "PASS")
                      {
                          stepResult.Add(0);
                          stepCode.Add(Result); 
                      }
                      else
                      {
                          chanResult = 1;
                          stepResult.Add(1);
                          stepCode.Add(Result); 
                      }

                      stepMode.Add(eItemName); 

                      switch (eItemName)
	                  {
		                  case EStepName.AC:
                               stepVal.Add(value);
                               stepUnit.Add("mA");
                               break;
                          case EStepName.DC:
                               stepVal.Add(value);
                               stepUnit.Add("mA");
                               break;
                          case EStepName.IR:
                              if (value < 1000)
                              {
                                  stepVal.Add(value);
                                  stepUnit.Add("M0hm");
                              }
                              else
                              {
                                  stepVal.Add(value / 1000);
                                  stepUnit.Add("G0hm");
                              }
                               break;
                          case EStepName.OSC:
                               stepVal.Add(value);
                               stepUnit.Add("nF");
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

              devName = valList[1];

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
              {
                  er = "文件序号不存在";
                  return false;
              }

              int FileNo = 0;

              for (int i = 0; i < proName.Length; i++)
              {
                  char s = System.Convert.ToChar(proName.Substring(i, 1));

                  if (!Char.IsNumber(s))
                  {
                      er = "文件序号必须为数字[1-20]";
                      return true;
                  }
              }

              FileNo = System.Convert.ToInt16(proName);

              if (FileNo < 1 || FileNo > 20)
              {
                  er = "文件序号必须为[1-20]";
                  return true;
              }
              
              er = string.Empty;

              string cmd = "MMEM:SAVE " + FileNo.ToString();

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
      /// 写测试项目
      /// </summary>
      /// <param name="wCmd"></param>
      /// <param name="wPara"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      private bool writePara(string wCmd, string wPara, out string er)
      {
          er = string.Empty;

          try
          {
               string rData=string.Empty;

               string Cmd = string.Empty;

               Cmd = wCmd + " " + wPara;
               
               if (!writeCmd(Cmd, out er))
                     return false;

               System.Threading.Thread.Sleep(20);

               Cmd = wCmd + "?";

               if (!sendCmdToHP(Cmd, out rData, out er, "\n"))
                   return false;
             
               if (System.Convert.ToDouble(wPara) !=System.Convert.ToDouble(rData))
                   return false;

               return true;
          }
          catch (Exception)
          {
              
              throw;
          }
      }
      /// <summary>
      /// 写命令
      /// </summary>
      /// <param name="wCmd"></param>
      /// <param name="delayMs"></param>
      private bool writeCmd(string wCmd, out string er, int delayMs = 300, int timeOutMs = 1000)
      {
         
          er = string.Empty;

          try
          {
              string rData = string.Empty;

              er = string.Empty;

              sendCmdToHP(wCmd, out rData, out er, string.Empty);

              System.Threading.Thread.Sleep(delayMs);

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
      private bool sendCmdToHP(string wData, out string rData, out string er, string rEOI = "\n", int wTimeOut = 1000)
      {
          rData = string.Empty;
          
          er = string.Empty;

          try
          {
              string recvData = string.Empty;
              wData += "\n";
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
