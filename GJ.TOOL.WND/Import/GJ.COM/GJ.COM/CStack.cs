using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.COM
{
    /// <summary>
    /// 链表类
    /// </summary>
    class CCStackNode<T>
    {
        public CCStackNode(T sender)
        {
            _value = sender;
        }
        /// <summary>
        /// 栈顶
        /// </summary>
        public CCStackNode<T> top;
        /// <summary>
        /// 栈底
        /// </summary>
        public CCStackNode<T> bottom;
        public T _value;
    }
    /// <summary>
    /// 堆栈:先进后出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CStack<T>
    {
        #region 字段
        /// <summary>
        /// 栈数量
        /// </summary>
        private int _count;
        /// <summary>
        /// 栈顶
        /// </summary>
        private CCStackNode<T> _top;
        /// <summary>
        /// 栈底
        /// </summary>
        private CCStackNode<T> _bottom;  
        #endregion

        #region 属性
        /// <summary>
        /// 栈数量
        /// </summary>
        public int count
        {
            get { return _count; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 压入堆栈
        /// </summary>
        /// <param name="newValue"></param>
        public void Push(T sender)
        {
            CCStackNode<T> newNode = new CCStackNode<T>(sender);

            if (count == 0)
            {
                _top = newNode;
                _bottom = newNode;
                _bottom.top = newNode;
            }
            else
            {
                _top.top = newNode;  //当前节点值
                newNode.bottom = _top;      //下一节点
                _top = newNode;           //当前节点
            }
            _count++;
            return;
        }
        /// <summary>
        /// 弹出堆栈
        /// </summary>
        /// <param name="newValue"></param>
        public T Pop()
        {
            T ret=default(T);

            if (count != 0)
            {
                ret = _top._value; 
                if (_top == _bottom)
                {                    
                    _count--;
                    return ret;
                }
                _top.bottom.top = _top; //下一节点
                _top = _top.bottom;          //当前节点
                _count--;                
            }
            return ret;
        }
        #endregion
    }

}
