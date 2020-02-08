using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.COM
{
    /// <summary>
    /// 时钟
    /// </summary>
    public class CTimer
    {
        #region 枚举
        /// <summary>
        /// 时间单位
        /// </summary>
        public enum EUNIT
        { 
          秒,
          分钟,
          小时,
          天数
        }
        #endregion

        #region 方法
        /// <summary>
        /// 日期差
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static int DateDiff(string startTime, string endTime = "", EUNIT unit = EUNIT.秒)
        {
            try
            {
                if (endTime == "")
                    endTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                TimeSpan s1 = new TimeSpan(System.Convert.ToDateTime(startTime).Ticks);
                TimeSpan s2 = new TimeSpan(System.Convert.ToDateTime(endTime).Ticks);
                TimeSpan s = s2.Subtract(s1);
                double val = 0;

                switch (unit)
                {
                    case EUNIT.秒:
                        val = s.TotalSeconds;
                        break;
                    case EUNIT.分钟:
                        val = s.TotalMinutes;
                        break;
                    case EUNIT.小时:
                        val = s.TotalHours;
                        break;
                    case EUNIT.天数:
                        val = s.TotalDays;
                        break;
                    default:
                        break;
                }

                return (int)val;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// 堵塞延时
        /// </summary>
        /// <param name="msTimes"></param>
        public static void DelayMs(int msTimes)
        {
            System.Threading.Thread.Sleep(msTimes);
        }
        /// <summary>
        /// 等待延时
        /// </summary>
        /// <param name="msTimes"></param>
        public static void WaitMs(int msTimes)
        {
            int nowTimes = System.Environment.TickCount;

            do
            {
                System.Windows.Forms.Application.DoEvents();

            } while (System.Environment.TickCount - nowTimes < msTimes);
        }
        #endregion

    }
}
