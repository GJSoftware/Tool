using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.COM
{
    /// <summary>
    /// 定义触发事件类
    /// </summary>
    /// <typeparam name="T">泛类事件类</typeparam>
    public class COnEvent<T> where T : EventArgs
    {
        /// <summary>
        /// 定义事件委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void OnEventHandler(object sender, T e);
        /// <summary>
        /// 定义事件
        /// </summary>
        public event OnEventHandler OnEvent;
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="e"></param>
        public void OnEvented(T e)
        {
            if (OnEvent != null)
                OnEvent(this, e);
        }
    }

}

