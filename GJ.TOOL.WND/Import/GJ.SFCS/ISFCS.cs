using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace GJ.SFCS
{
    #region 枚举
    /// <summary>
    /// 状态
    /// </summary>
    public enum EMesState
    { 
        未加载 = 0,
        异常错误=100,
        正常 = 200,
        网络异常=400
    }
    #endregion

    #region 接口定义
    /// <summary>
    /// 接口类
    /// </summary>
    public interface ISFCS
    {
        bool Start(out EMesState status, out string er);

        bool CheckSn(CSFCS.CSnInfo sn, out EMesState status, out string er);

        bool TranSn(CSFCS.CSnData data, out EMesState status, out string er);

        bool Close(out string er);
    }
    #endregion

}
