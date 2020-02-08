using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.COM
{
    /// <summary>
    /// 节点类
    /// </summary>
    public class CQueueNode<T>
    {
        public CQueueNode(T sender)
        {
            _value = sender;
        }
        /// <summary>
        /// 队前
        /// </summary>
        public CQueueNode<T> fore; 
        /// <summary>
        /// 队后
        /// </summary>
        public CQueueNode<T> back; 
        /// <summary>
        /// 队员
        /// </summary>
        public T _value;
    }
    /// <summary>
    /// 队列类:先进先出
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CQueue<T>
    {
        #region 字段
        /// <summary>
        /// 队头
        /// </summary>
        private CQueueNode<T> _head;
        /// <summary>
        /// 队尾
        /// </summary>
        private CQueueNode<T> _tail;
        /// <summary>
        /// 队员数量
        /// </summary>
        private int _count = 0;
        #endregion

        #region 属性
        /// <summary>
        /// 队员数量
        /// </summary>
        public int count
        {
            get { return _count; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="newValue"></param>
        public void EnQueue(T sender)
        {
            try
            {
                CQueueNode<T> newNode = new CQueueNode<T>(sender);
                if (count == 0)
                {
                    _head = newNode;
                    _tail = newNode;
                }
                else
                {
                    _tail.back = newNode;
                    newNode.fore = _tail;
                    _tail = newNode;
                }
                _count++;
            }
            catch (Exception)
            {                
                throw;
            }           
        }
        /// <summary>
        /// 出队
        /// </summary>
        public T DeQueue()
        {
            if (count != 0)
            {
                T queue = _head._value;
                if (_head == _tail)
                {
                    _count--;
                    return queue;
                }
                _head = _head.back;
                _count--;
                return queue;
            }
            return default(T);
        }
        /// <summary>
        /// 返回编号队员
        /// </summary>
        /// <param name="curNo">0,1,2...</param>
        /// <returns></returns>
        public T RtnMember(int idNo)
        {
            try
            {
                if(_count==0 || idNo > _count-1)
                    return default(T);
 
                if (idNo == 0)   //队头
                    return _head._value;
                else if (idNo == _count - 1) //队尾
                    return _tail._value;
                else
                {
                    CQueueNode<T> node = _head.back;
                    for (int i = 1; i < idNo; i++)
                    {
                        node = node.back;
                    }
                    return node._value; 
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }
        /// <summary>
        /// 返回编号队头
        /// </summary>
        /// <returns></returns>
        public T RtnHeader()
        {
            try
            {
                if (_count == 0)
                    return default(T);
                return _head._value;
            }
            catch (Exception)
            {                
                throw;
            }
        }
        /// <summary>
        /// 返回编号队尾
        /// </summary>
        /// <returns></returns>
        public T RtnTailer()
        {
            try
            {
                if (_count == 0)
                    return default(T);
                return _tail._value;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

    }
}
