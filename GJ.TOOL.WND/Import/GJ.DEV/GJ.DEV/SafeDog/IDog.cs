using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aladdin.HASP;

namespace GJ.DEV.SafeDog
{
    public interface IDog
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
        #endregion

        #region 共享方法
        /// <summary>
        /// 打开加密狗
        /// </summary>
        /// <param name="tz">厂商</param>
        /// <param name="pwr">天数</param>
        /// <param name="er">错误代码</param>
        /// <returns></returns>
        HaspStatus ClassCS(int tz, out int pwr, out int er);
        /// <summary>
        /// 返回ID
        /// </summary>
        /// <param name="tz">厂商</param>
        /// <param name="id">ID</param>
        /// <param name="er">错误代码</param>
        /// <returns></returns>
        HaspStatus time(int tz, out string id, out string er);
        /// <summary>
        /// 返回天数
        /// </summary>
        /// <param name="tz">厂商</param>
        /// <param name="pwr">激活码</param>
        /// <param name="day">天数</param>
        /// <returns></returns>
        HaspStatus ActivationTime(int tz, string pwr, out int day);
        #endregion

        #region 专用功能
        /// <summary>
        /// 密码狗检索
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        bool check_safe_dog(int tz, out int leftDays, out string idno, out string er);
        /// <summary>
        /// 激活
        /// </summary>
        /// <returns></returns>
        bool check_safe_dog(int EID, string pwr, out int leftDays, out string er);
        #endregion

    }
}
