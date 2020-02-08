using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.Serialization;
namespace GJ.SFCS
{
    public class CSFCS
    {
        #region 实体类定义
        /// <summary>
    /// 条码检查类
    /// </summary>
        [DataContract]
        public class CSnInfo
        {
            [DataMember]
            public string StatName { get; set; }
            [DataMember]
            public string SerialNo { get; set; }
            [DataMember]
            public string OrderName { get; set; }
            [DataMember]
            public string UserId { get; set; }
           [DataMember]
            public string Remark1 { get; set; }
            [DataMember]
            public string Remark2 { get; set; }
        }
        /// <summary>
    /// 测试项目
    /// </summary>
        [DataContract]
        public class CSnItem
        {
            [DataMember]
            public int IdNo { get; set; }
            [DataMember]
            public string Name { get; set; }
            [DataMember]
            public string Desc { get; set; }
            [DataMember]
            public string UpLimit { get; set; }
            [DataMember]
            public string LowLimit { get; set; }
            [DataMember]
            public string Value { get; set; }
            [DataMember]
            public string Unit { get; set; }
            [DataMember]
            public int Result { get; set; }
            [DataMember]
            public string ErroCode { get; set; }
            [DataMember]
            public string ErrInfo { get; set; }
            [DataMember]
            public string Remark1 { get; set; }
            [DataMember]
            public string Remark2 { get; set; }
        }
        [DataContract]
        public class CSnData
        {
            [DataMember]
            public string DeviceId { get; set; }
            [DataMember]
            public string DeviceName { get; set; }
            [DataMember]
            public string StatName { get; set; }
            [DataMember]
            public string StatDesc { get; set; }
            [DataMember]
            public string OrderName { get; set; }
            [DataMember]
            public string Fixture { get; set; }
            [DataMember]
            public string LocalName { get; set; }
            [DataMember]
            public string Model { get; set; }
            [DataMember]
            public string SerialNo { get; set; }
            [DataMember]
            public string StartTime { get; set; }
            [DataMember]
            public string EndTime { get; set; }
            [DataMember]
            public int Result { get; set; }
            [DataMember]
            public string Remark1 { get; set; }
            [DataMember]
            public string Remark2 { get; set; }
            [DataMember]
            public List<CSnItem> Item { get; set; }
        }
        #endregion

        #region 构造函数
        public CSFCS(int idNo, string name,string className,string plugFolder = "SFCS")
        {
            this.idNo = idNo;

            this.name = name;

            this.dllFile = Environment.CurrentDirectory + "\\" + plugFolder + "\\" + "GJ." + className + ".SFCS.dll";
            
            PlugBase<ISFCS> plugIn = new PlugBase<ISFCS>(plugFolder);

            com = plugIn.GetClass(className);

            if (com != null)
            {
                message = "Initialize dynamic library file[" + dllFile + "]OK";

                state = EMesState.正常;
            }
            else
            {
                state = EMesState.异常错误;

                message = "Initialize dynamic library file[" + dllFile + "]NG";
            }
        }
        public override string ToString()
        {
            return name;
        }
        #endregion

        #region 字段
        private int idNo = 0;
        private string name = string.Empty;
        private string dllFile = string.Empty;
        private EMesState state = EMesState.未加载;
        private string message = string.Empty;
        private ISFCS com = null;
        private ReaderWriterLock comlock = new ReaderWriterLock();
        #endregion

        #region 属性
        public EMesState State
        {
            get { return state; }
        }
        public string Message
        {
            get { return message; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Start(out string er)
        {
            er = string.Empty;

            try
            {
                comlock.AcquireWriterLock(-1);

                if (com == null)
                {
                    er = "Failed to initialize dynamic library file" + "[" + dllFile + "]";

                    state = EMesState.异常错误;

                    message = er;

                    return false;
                }

                if (!com.Start(out state, out er))
                {
                    message = er;

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                comlock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Close(out string er)
        {
            er = string.Empty;

            try
            {
                comlock.AcquireWriterLock(-1);

                if (!com.Close(out er))
                {
                    message = er;

                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                comlock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 检查条码
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool CheckSn(CSnInfo sn, out string er)
        {
            er = string.Empty;

            try
            {
                comlock.AcquireWriterLock(-1);

                if (com == null)
                {
                    er = "Failed to initialize dynamic library file" + "[" + dllFile + "]";

                    state = EMesState.异常错误;

                    message = er;

                    return false;
                }

                if (!com.CheckSn(sn, out state, out er))
                {
                    message = er;

                    return false;
                }

                message = er;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                comlock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 条码过站
        /// </summary>
        /// <param name="data"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool TranSnData(CSnData data, out string er)
        {
            er = string.Empty;

            try
            {
                comlock.AcquireWriterLock(-1);

                if (com == null)
                {
                    er = "Failed to initialize dynamic library file" + "[" + dllFile + "]";

                    state = EMesState.异常错误;

                    message = er;

                    return false;
                }

                if (!com.TranSn(data, out state, out er))
                {
                    message = er;

                    return false;
                }

                message = er;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                comlock.ReleaseWriterLock();
            }
        }
        #endregion
        
    }
}
