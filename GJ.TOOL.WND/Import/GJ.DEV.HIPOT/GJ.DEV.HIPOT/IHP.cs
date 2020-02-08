using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.DEV.HIPOT
{
    public interface IHP
    {
        #region 属性
        /// <summary>
        /// 设备ID
        /// </summary>
        int idNo
        { set; get; }
        /// <summary>
        /// 设备名称
        /// </summary>
        string name
        { set; get; }
        /// <summary>
        /// 连接状态
        /// </summary>
        bool conStatus
        { get; }
        /// <summary>
        /// 通道数
        /// </summary>
        int chanMax
        { get; }
        #endregion

        #region 方法
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="setting"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool Open(string comName, out string er, string setting);
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        void Close();
        /// <summary>
        /// 初始化设备
        /// </summary>
        /// <returns></returns>
        bool Init(out string er, int uutMax, int stepNum);
        /// <summary>
        /// 设置测试步骤
        /// </summary>
        /// <param name="step"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetTestPara(List<CHPPara.CStep> step, out string er, string proName, bool saveToDev);
        /// <summary>
        /// 启动测试
        /// </summary>
        bool Start(out string er);
        /// <summary>
        /// 停止测试
        /// </summary>
        bool Stop(out string er);
        /// <summary>
        /// 导入高压编辑程序
        /// </summary>
        /// <param name="proName"></param>
        /// <returns></returns>
        bool ImportProgram(string proName, out string er);
        /// <summary>
        /// 读测试步骤
        /// </summary>
        /// <param name="stepName"></param>
        /// <param name="er"></param>
        /// <param name="chan"></param>
        /// <returns></returns>
        bool ReadStepName(out List<EStepName> stepName, out string er, int chan = 1);
        /// <summary>
        /// 设置高压通道
        /// </summary>
        /// <param name="chanList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool SetChanEnable(List<int> chanList, out string er);
        /// <summary>
        /// 读设置值
        /// </summary>
        /// <param name="stepNo"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadStepSetting(int stepNo, out EStepName stepName, out List<double> stepVal, out string er);
        /// <summary>
        /// 读取状态
        /// </summary>
        /// <returns></returns>
        bool ReadStatus(out EHPStatus status, out string er);
        /// <summary>
        /// 获取测试通道结果
        /// </summary>
        /// <param name="uutMax"></param>
        /// <param name="stepMax"></param>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadResult(int uutMax, int stepMax, out List<CCHResult> uut, out string er);
        /// <summary>
        /// 获取测试结果
        /// </summary>
        /// <param name="result"></param>
        /// <param name="stepVal"></param>
        /// <returns></returns>
        bool ReadResult(int chan, out int chanResult,
                        out List<int> stepResult, out List<string> stepCode,
                        out List<EStepName> stepMode, out List<double> stepVal,
                        out List<string> stepUnit, out string er);
        /// <summary>
        /// 写入命令
        /// </summary>
        /// <param name="wCmd"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool WriteCmd(string wCmd, out string er);
        /// <summary>
        /// 读取命令
        /// </summary>
        /// <param name="wCmd"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        bool ReadCmd(string wCmd, out string rData, out string er);
        #endregion
    }
}
