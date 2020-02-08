using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.COM
{
    /// <summary>
    /// 作者特性定义
    /// </summary>
    [AttributeUsage(AttributeTargets.All,AllowMultiple=true,Inherited =false)]
    public class Author:System.Attribute
    {
        #region 构造函数
        public Author(string name, string version, string verDate, string context = "")
        {
            this._name = name;
            this._version = version;
            this._verDate = verDate;
            this._context = context;
        }
        #endregion

        #region 字段
        private string _name;
        private string _version;
        private string _verDate;
        private string _context;
        #endregion

        #region 属性
        /// <summary>
        /// 作者
        /// </summary>
        public string name
        {
            get { return _name; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        public string version
        {
            get { return version; }
        }
        /// <summary>
        /// 修改日期
        /// </summary>
        public string verDate
        {
            get { return _verDate; }
        }
        /// <summary>
        /// 修改内容
        /// </summary>
        public string context
        {
            get { return _context; }
        }
        #endregion
        
    }

}
