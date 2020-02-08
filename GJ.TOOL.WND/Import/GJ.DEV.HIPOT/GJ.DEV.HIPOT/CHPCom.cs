using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;

namespace GJ.DEV.HIPOT
{
    public class CHPCom
    {
      #region 构造函数
      public CHPCom(EHPType hpType, int idNo = 0, string name = "IHP")
      {
        this._idNo = idNo;

        this._name = name;

        this._devType = hpType;

        //反射获取PLC类型

        string plcModule = "C" + _devType.ToString();

        Assembly asb = Assembly.GetAssembly(typeof(IHP));

        Type[] types = asb.GetTypes();

        object[] parameters = new object[2];

        parameters[0] = _idNo;

        parameters[1] = _name;

        foreach (Type t in types)
        {
            if (t.Name == plcModule && t.GetInterface("IHP") != null)
            {
                _devHP = (IHP)asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
                break;
            }
        }
      }
      public override string ToString()
      {
          return _name;
      }
      #endregion

      #region 字段
      private int _idNo = 0;
      private string _name = string.Empty;
      private bool _conStatus = false;
      private EHPType _devType = EHPType.Chroma19020;
      private IHP _devHP = null;
      private ReaderWriterLock idLock = new ReaderWriterLock();
      #endregion

      #region 属性
      /// <summary>
      /// 编号
      /// </summary>
      public int idNo
      {
          set
          {
              _idNo = value;
          }
          get
          {
              return _idNo;
          }
      }
      /// <summary>
      /// 名称
      /// </summary>
      public string name
      {
          set
          {
              _name = value;
          }
          get
          {
              return _name;
          }
      }
      /// <summary>
      /// 连接状态
      /// </summary>
      public bool conStatus
      {
          get
          {
              return _conStatus;
          }
      }
      /// <summary>
      /// 设备通道数
      /// </summary>
      public int chanMax
      {
          get {
                 if (_devHP != null)
                 {
                   return _devHP.chanMax;       
                 }
                 return 0;
              }
      }
      #endregion

      #region 方法
      /// <summary>
      /// 打开串口
      /// </summary>
      /// <param name="comName"></param>
      /// <param name="setting"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Open(string comName, out string er, string setting)
      {
          return _devHP.Open(comName, out er, setting);
      }
      /// <summary>
      /// 关闭串口
      /// </summary>
      /// <returns></returns>
      public void Close()
      {
          _devHP.Close();
      }
      /// <summary>
      /// 初始化设备
      /// </summary>
      /// <param name="uutMax">测试产品数</param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool Init(out string er, int uutMax=8,int stepNum=1)
      {
          return _devHP.Init(out er, uutMax, stepNum);
      }
      /// <summary>
      /// 设置测试步骤
      /// </summary>
      /// <param name="step"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetTestPara(List<CHPPara.CStep> step, out string er, string proName, bool saveToDev)
      {
          return _devHP.SetTestPara(step, out er, proName, saveToDev); 
      }
      /// <summary>
      /// 启动测试
      /// </summary>
      public bool Start(out string er)
      {
          return _devHP.Start(out er); 
      }
      /// <summary>
      /// 停止测试
      /// </summary>
      public bool Stop(out string er)
      {
          return _devHP.Stop(out er);
      }
      /// <summary>
      /// 导入高压编辑程序
      /// </summary>
      /// <param name="proName"></param>
      /// <returns></returns>
      public bool ImportProgram(string proName, out string er)
      {
          return _devHP.ImportProgram(proName, out er); 
      }
      /// <summary>
      /// 读测试步骤
      /// </summary>
      /// <param name="stepName"></param>
      /// <param name="er"></param>
      /// <param name="chan"></param>
      /// <returns></returns>
      public bool ReadStepName(out List<EStepName> stepName, out string er, int chan = 1)
      {
          return _devHP.ReadStepName(out stepName, out er, chan); 
      }
      /// <summary>
      /// 设置高压通道
      /// </summary>
      /// <param name="chanList"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool SetChanEnable(List<int> chanList, out string er)
      {
          return _devHP.SetChanEnable(chanList, out er);  
      }
      /// <summary>
      /// 读设置值
      /// </summary>
      /// <param name="stepNo"></param>
      /// <param name="er"></param>
      /// <returns></returns>
      public bool ReadStepSetting(int stepNo, out EStepName stepName, out List<double> stepVal, out string er)
      {
          return _devHP.ReadStepSetting(stepNo, out stepName, out stepVal, out er);   
      }
      /// <summary>
      /// 读取状态
      /// </summary>
      /// <returns></returns>
      public bool ReadStatus(out EHPStatus status, out string er)
      {
          return _devHP.ReadStatus(out status, out er);  
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
          return _devHP.ReadResult(uutMax, stepMax, out uut, out er);  
      }
      /// <summary>
      /// 获取测试结果
      /// </summary>
      /// <param name="result"></param>
      /// <param name="stepVal"></param>
      /// <returns></returns>
      public bool ReadResult(int chan, out int chanResult,
                             out List<int> stepResult, out List<string> stepCode,
                             out List<EStepName> stepMode, out List<double> stepVal,
                             out List<string> stepUnit, out string er)
      {
          return _devHP.ReadResult(chan, out chanResult, out stepResult, out stepCode,
                                        out stepMode, out stepVal, out stepUnit, out er);    
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
              if (!_devHP.WriteCmd(wCmd, out er))
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
              if (!_devHP.ReadCmd(wCmd, out rData, out er))
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
