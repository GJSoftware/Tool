using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Xml;
using System.Net;
using GJ.COM;

namespace GJ.MES
{
    public class CWeb2
    {
        #region 常量定义
        /// <summary>
        /// 数据包头
        /// </summary>
        private const string C_XML_HEADER = "GJMES";
        /// <summary>
        /// 输入表单名
        /// </summary>
        private const string C_XML_BASE = "XmlBase";
        /// <summary>
        /// 返回表单名
        /// </summary>
        private const string C_XML_PARA = "XmlPara";
        #endregion

        #region 枚举
        /// <summary>
        /// 治具类型
        /// </summary>
        public enum EFixtureType
        { 
           禁用 = -1,
           正常 = 0,
           空治具 = 1,
           维修治具 = 2,
           重工治具 = 3,
           点检治具 = 4
        }
        /// <summary>
        /// 条码类型
        /// </summary>
        public enum ESnType
        { 
            外部条码,
            内部条码
        }
        /// <summary>
        /// 易损件类型
        /// </summary>
        public enum EPartType
        {
           治具,
           库位
        }
        /// <summary>
        /// 执行语句类型
        /// </summary>
        public enum ESqlCmdType
        {
            /// <summary>
            /// 不返回
            /// </summary>
            ExecuteNonQuery,
            /// <summary>
            /// 返回值
            /// </summary>
            ExecuteQuery
        }
        /// <summary>
        /// 内外条码匹配模式
        /// </summary>
        public enum EMapMode
        { 
           内码绑定到外码,
           外码绑定到内码
        }
        /// <summary>
        /// 内外码匹配范围
        /// </summary>
        public enum EMapRange
        { 
           治具与数据,
           治具,
           数据
        }
        #endregion

        #region 类定义
        /// <summary>
        /// 流程
        /// </summary>
        public class CFlow
        {  
            /// <summary>
            /// 流程编号
            /// </summary>
            public int Index { get; set; }
            /// <summary>
            /// 站别信息
            /// </summary>
            public List<CSTAT> StatList { get; set; }
        }
        /// <summary>
        /// 站别信息
        /// </summary>
        public class CSTAT
        {
            /// <summary>
            /// 站别编号
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 站别名称
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 站别状态 -1:禁用; 0:正常
            /// </summary>
            public int Status { get; set; }
        }
        /// <summary>
        /// 基本信息
        /// </summary>
        public class CUUT_Base
        {
            /// <summary>
            /// 流程编号
            /// </summary>
            public int FlowIndex = 0;
            /// <summary>
            /// 测试编号
            /// </summary>
            public int FlowId = 0;
            /// <summary>
            /// 工位名称
            /// </summary>
            public string FlowName = string.Empty;
            /// <summary>
            /// 测试工位Guid
            /// </summary>
            public string FlowGuid = string.Empty;
            /// <summary>
            /// 线体编号
            /// </summary>
            public int LineNo = 0;
            /// <summary>
            /// 线体名称
            /// </summary>
            public string LineName = "L1";
            /// <summary>
            /// 机种名称
            /// </summary>
            public string Model = string.Empty;
            /// <summary>
            /// 工单号
            /// </summary>
            public string OrderName = string.Empty;
            /// <summary>
            /// 连线模式 0:冠佳Web 1:客户MES
            /// </summary>
            public int MesFlag = 0;
            /// <summary>
            /// 外部条码:0 内部条码:1
            /// </summary>
            public ESnType SnType = ESnType.外部条码;
            /// <summary>
            /// 检查条码
            /// </summary>
            public int CheckSn = 1;
        }
        /// <summary>
        /// 条码信息
        /// </summary>
        public class CUUT_Para
        {
            /// <summary>
            /// 条码
            /// </summary>
            public string SerialNo = string.Empty;
            /// <summary>
            /// 内部条码
            /// </summary>
            public string InnerSn = string.Empty;
            /// <summary>
            /// 治具RFID
            /// </summary>
            public string IdCard = string.Empty;
            /// <summary>
            /// 治具槽位
            /// </summary>
            public int SlotNo = 0;
            /// <summary>
            /// 测试结果
            /// </summary>
            public int Result = 0;
            /// <summary>
            /// 测试参数
            /// </summary>
            public string TestData = string.Empty;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime = string.Empty;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime = string.Empty;
            /// <summary>
            /// 备注1
            /// </summary>
            public string Remark1 = string.Empty;
            /// <summary>
            /// 备注2
            /// </summary>
            public string Remark2 = string.Empty;
        }
        /// <summary>
        /// 产品信息
        /// </summary>
        public class CUUT
        {
            /// <summary>
            /// 基本信息
            /// </summary>
            public CUUT_Base Base = new CUUT_Base();
            /// <summary>
            /// 测试信息
            /// </summary>
            public CUUT_Para Para = new CUUT_Para();
        }
        /// <summary>
        /// 条码基本信息
        /// </summary>
        public class CSn_Base
        {
            /// <summary>
            /// 条码
            /// </summary>
            public string SerialNo = string.Empty;
            /// <summary>
            /// 内部条码
            /// </summary>
            public string InnerSn = string.Empty;      
            /// <summary>
            /// 线体编号
            /// </summary>
            public int LineNo = 0;
            /// <summary>
            /// 线体名称
            /// </summary>
            public string LineName = "L1";
            /// <summary>
            /// 机种名称
            /// </summary>
            public string Model = string.Empty;
            /// <summary>
            /// 工单号
            /// </summary>
            public string OrderName = string.Empty;
            /// <summary>
            /// 连线模式 0:冠佳Web 1:客户MES
            /// </summary>
            public int MesFlag = 0;
            /// <summary>
            /// 治具RFID
            /// </summary>
            public string IdCard = string.Empty;
            /// <summary>
            /// 治具槽位
            /// </summary>
            public int SlotNo = 0;
            /// <summary>
            /// 备注1
            /// </summary>
            public string Remark1 = string.Empty;
            /// <summary>
            /// 备注2
            /// </summary>
            public string Remark2 = string.Empty;
        }
        /// <summary>
        /// 条码测试信息
        /// </summary>
        public class CSn_Para
        {
            /// <summary>
            /// 条码
            /// </summary>
            public string SerialNo = string.Empty;
            /// <summary>
            /// 治具RFID
            /// </summary>
            public string IdCard = string.Empty;
            /// <summary>
            /// 治具槽位
            /// </summary>
            public int SlotNo = 0;
            /// <summary>
            /// 流程编号
            /// </summary>
            public int FlowIndex = 0;
            /// <summary>
            /// 测试编号
            /// </summary>
            public int FlowId = 0;
            /// <summary>
            /// 工位名称
            /// </summary>
            public string FlowName = string.Empty;
            /// <summary>
            /// 测试工位Guid
            /// </summary>
            public string FlowGuid = string.Empty;
            /// <summary>
            /// 测试结果
            /// </summary>
            public int Result = 0;
            /// <summary>
            /// 测试参数
            /// </summary>
            public string TestData = string.Empty;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime = string.Empty;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime = string.Empty;
            /// <summary>
            /// 备注1
            /// </summary>
            public string Remark1 = string.Empty;
            /// <summary>
            /// 备注2
            /// </summary>
            public string Remark2 = string.Empty;
        }
        /// <summary>
        /// 条码信息
        /// </summary>
        public class CSn
        {
            /// <summary>
            /// 基本信息
            /// </summary>
            public CSn_Base Base = new CSn_Base();
            /// <summary>
            /// 测试信息
            /// </summary>
            public List<CSn_Para> Para = new List<CSn_Para>();
        }
        /// <summary>
        /// 治具基本信息
        /// </summary>
        public class CFix_Base
        {
            /// <summary>
            /// 治具类型
            /// </summary>
            public EFixtureType FixtureType = EFixtureType.正常;
            /// <summary>
            /// 条码类型
            /// </summary>
            public ESnType SnType = ESnType.外部条码;
            /// <summary>
            /// 治具槽位数
            /// </summary>
            public int MaxSlot = 16;
            /// <summary>
            /// 治具ID
            /// </summary>
            public string IdCard = string.Empty;
            /// <summary>
            /// 线体编号
            /// </summary>
            public int LineNo = 0;
            /// <summary>
            /// 线体名称
            /// </summary>
            public string LineName = "L1";
            /// <summary>
            /// 机种名称
            /// </summary>
            public string Model = string.Empty;
            /// <summary>
            /// 工单号
            /// </summary>
            public string OrderName = string.Empty;
            /// <summary>
            /// 连线模式 0:冠佳Web 1:客户MES
            /// </summary>
            public int MesFlag = 0;
            /// <summary>
            /// 流程编号
            /// </summary>
            public int FlowIndex = 0;
            /// <summary>
            /// 工位名称
            /// </summary>
            public string FlowName = string.Empty;
            /// <summary>
            /// 测试工位Guid
            /// </summary>
            public string FlowGuid = string.Empty;
            /// <summary>
            /// 使用次数
            /// </summary>
            public int UseNum = 0;
            /// <summary>
            /// 测试次数
            /// </summary>
            public int TTNum = 0;
            /// <summary>
            /// 测试不良次数
            /// </summary>
            public int FailNum = 0;
            /// <summary>
            /// 测试连续不良次数
            /// </summary>
            public int ConFailNum = 0;
            /// <summary>
            /// 检查条码
            /// </summary>
            public int CheckSn = 1;
        }
        /// <summary>
        /// 治具信息
        /// </summary>
        public class CFix_Para
        {
            /// <summary>
            /// 治具槽位
            /// </summary>
            public int SlotNo = 0;
            /// <summary>
            /// 条码
            /// </summary>
            public string SerialNo = string.Empty;
            /// <summary>
            /// 内部条码
            /// </summary>
            public string InnerSn = string.Empty;
            /// <summary>
            /// 测试结果
            /// </summary>
            public int Result = 0;
            /// <summary>
            /// 流程编号
            /// </summary>
            public int FlowIndex = 0;
            /// <summary>
            /// 测试工位编号
            /// </summary>
            public int FlowId = 0;
            /// <summary>
            /// 工位名称
            /// </summary>
            public string FlowName = string.Empty;
            /// <summary>
            /// 测试工位Guid
            /// </summary>
            public string FlowGuid = string.Empty;
            /// <summary>
            /// 备注1
            /// </summary>
            public string Remark1 = string.Empty;
            /// <summary>
            /// 备注2
            /// </summary>
            public string Remark2 = string.Empty;
            /// <summary>
            /// 测试参数
            /// </summary>
            public string TestData = string.Empty;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime = string.Empty;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime = string.Empty;
        }
        /// <summary>
        /// 治具信息
        /// </summary>
        public class CFixture
        {
            /// <summary>
            /// 基本信息
            /// </summary>
            public CFix_Base Base = new CFix_Base();
            /// <summary>
            /// 槽位信息
            /// </summary>
            public List<CFix_Para> Para = new List<CFix_Para>();
        }
        /// <summary>
        /// 生产统计
        /// </summary>
        public class CYield_Base
        {
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime = string.Empty;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime = string.Empty;
            /// <summary>
            /// 流程编号
            /// </summary>
            public int FlowIndex = 0;
            /// <summary>
            /// 工位名称
            /// </summary>
            public string FlowName = string.Empty;
            /// <summary>
            /// 测试工位Guid
            /// </summary>
            public string FlowGuid = string.Empty;
            /// <summary>
            /// 线体编号
            /// </summary>
            public int LineNo = 0;
            /// <summary>
            /// 线体名称
            /// </summary>
            public string LineName = "L1";
            /// <summary>
            /// 机种名称
            /// </summary>
            public string Model = string.Empty;
            /// <summary>
            /// 工单号
            /// </summary>
            public string OrderName = string.Empty;
        }
        /// <summary>
        /// 生产统计参数
        /// </summary>
        public class CYield_Para
        {
            public int IdNo = 0;

            public string Name = string.Empty;

            public int TTNum = 0;

            public int FailNum = 0;
        }

        public class CAlarm_Base
        {
            /// <summary>
            /// 线体编号
            /// </summary>
            public int LineNo = 0;
            /// <summary>
            /// 线体名称
            /// </summary>
            public string LineName = string.Empty;
            /// <summary>
            /// 测试工位
            /// </summary>
            public string StatName = string.Empty;
            /// <summary>
            /// 工位GUID
            /// </summary>
            public string StatGuid = string.Empty;
            /// <summary>
            /// 开始日期
            /// </summary>
            public string StartTime = string.Empty;
            /// <summary>
            /// 结束日期
            /// </summary>
            public string EndTime =string.Empty;
            /// <summary>
            /// 报警代码
            /// </summary>
            public int ErrNo = 0;
            /// <summary>
            /// 报警或解除
            /// </summary>
            public int bAlarm = 0;
        }
        public class CAlarm_Para
        {
            public int ErrNo = 0;

            public string ErrCode = string.Empty;

            public int bAlarm = 0;

            public string AlarmInfo = string.Empty;

            public string Remark1 = string.Empty;

            public string Remark2 = string.Empty;
        }
        /// <summary>
        /// 报警记录
        /// </summary>
        public class CAlarm
        {
            public CAlarm_Base Base = new CAlarm_Base();

            public List<CAlarm_Para> Para = new List<CAlarm_Para>();
        }
        /// <summary>
        /// 报警记录
        /// </summary>
        public class CAlarmRecord
        {
            public int LineNo = 0;

            public string LineName = string.Empty;

            public string StatName = string.Empty;

            public string StatGuid = string.Empty;

            public int ErrNo = 0;

            public string ErrCode = string.Empty;

            public int bAlarm = 0;

            public string AlarmInfo = string.Empty;

            public string HappenTime = string.Empty;

            public string Remark1 = string.Empty;

            public string Remark2 = string.Empty;
        }
        /// <summary>
        /// 查询条码类
        /// </summary>
        public class CSn_Query
        {
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime = string.Empty;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime = string.Empty;
            /// <summary>
            /// 流程编号
            /// </summary>
            public int FlowIndex = 0;
            /// <summary>
            /// 工位名称
            /// </summary>
            public string FlowName = string.Empty;
            /// <summary>
            /// 条码类型
            /// </summary>
            public int SnType = 0;
            /// <summary>
            /// 条码
            /// </summary>
            public string SerialNo = string.Empty;
            /// <summary>
            /// 结果 -1:为PASS和FAIL
            /// </summary>
            public int Result = -1;
        }
        /// <summary>
        /// 治具查询条件
        /// </summary>
        public class CFixCondition
        {
            public int FlowIndex = 0;

            public string FlowName=string.Empty;

            public string IdCard = string.Empty;

            public int SlotNo = -1;
        }
        /// <summary>
        /// 治具使用次数
        /// </summary>
        public class CFixUseNum
        {
            /// <summary>
            /// 流程编号
            /// </summary>
            public int FlowIndex = 0;
            /// <summary>
            /// 流程名称
            /// </summary>
            public string FlowName = string.Empty;
            /// <summary>
            /// 治具RFID
            /// </summary>
            public string IdCard = string.Empty;
            /// <summary>
            /// 槽位编号
            /// </summary>
            public int SlotNo = 0;
            /// <summary>
            /// 测试总数
            /// </summary>
            public int TTNum = 0;
            /// <summary>
            /// 测试不良数
            /// </summary>
            public int FailNum = 0;
            /// <summary>
            /// 测试连续不良数
            /// </summary>
            public int ConFailNum = 0;
        }
        /// <summary>
        /// 易损件查询
        /// </summary>
        public class CPartCondition
        {
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime = string.Empty;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime = string.Empty;
            /// <summary>
            /// 易损件类型
            /// </summary>
            public EPartType PartType = EPartType.治具;
            /// <summary>
            /// 易损件名称
            /// </summary>
            public string PartName = string.Empty;
            /// <summary>
            /// 易损件编号
            /// </summary>
            public int PartSlotNo = -1;
            /// <summary>
            /// 测试总数
            /// </summary>
            public int TTNum = 0;
            /// <summary>
            /// 累计不良次数
            /// </summary>
            public int FailNum = 0;
            /// <summary>
            /// 连续不良次数
            /// </summary>
            public int ConFailNum = 0;
        }
        /// <summary>
        /// 易损件记录
        /// </summary>
        public class CPartRecord
        {
            public int LineNo = 0;

            public string LineName = string.Empty;

            public EPartType PartType = EPartType.库位;

            public string PartName = string.Empty;

            public int PartSlotNo = 0;

            public string PartCarrier = string.Empty;

            public string LocalName = string.Empty;

            public string AlarmInfo = string.Empty;

            public string AlarmTime = string.Empty;

            public int TTNum = 0;

            public int FailNum = 0;

            public int ConFailNum = 0;

            public string Remark1 = string.Empty;

            public string Remark2 = string.Empty;
        }
        /// <summary>
        /// 机型基本信息
        /// </summary>
        public class CModel_Base
        {
            public int LineNo = -1;

            public string LineName = string.Empty;

            public string StationName = string.Empty;
        }
        /// <summary>
        /// 机型参数
        /// </summary>
        public class CModel_Para
        {
            public string ModelName = string.Empty;

            public string ModelSn = string.Empty;

            public string ModelPara = string.Empty;

            public string ImportDate = string.Empty;

            public string ModifyDate = string.Empty;

            public string Remark1 = string.Empty;

            public string Remark2 = string.Empty;
        }
        /// <summary>
        /// 机型参数
        /// </summary>
        public class CModel
        {
            public CModel_Base Base = new CModel_Base();

            public List<CModel_Para> Para = new List<CModel_Para>();
        }
        /// <summary>
        /// 机型查询参数
        /// </summary>
        public class CModel_Condition
        {
            public int LineNo = -1;

            public string LineName = string.Empty;

            public string StationName = string.Empty;

            public string ModelName = string.Empty;

            public string ModelSn = string.Empty;

            public int Status = -1;
        }
        /// <summary>
        /// 内外条码匹配
        /// </summary>
        public class CMap_Base
        {
            public EMapMode MapMode = EMapMode.内码绑定到外码;

            public EMapRange MapRange = EMapRange.治具与数据;
        }
        public class CMap_Para
        {
            /// <summary>
            /// 外部条码
            /// </summary>
            public string SerialNo = string.Empty;
            /// <summary>
            /// 内部条码
            /// </summary>
            public string InnerSn = string.Empty;
        }
        /// <summary>
        /// 内外码匹配
        /// </summary>
        public class CMap
        {
            public CMap_Base Base = new CMap_Base();

            public List<CMap_Para> Para = new List<CMap_Para>();
        }
        #endregion

        #region 字段
        /// <summary>
        /// 通信时间(ms)
        /// </summary>
        public static long WaitTime = 0;
        /// <summary>
        /// 接口定义
        /// </summary>
        private static string _ulrWeb = string.Empty;
        /// <summary>
        /// 同步锁
        /// </summary>
        private static ReaderWriterLock webLock = new ReaderWriterLock();
        #endregion

        #region 基本方法
        /// <summary>
        /// 检查Web接口状态
        /// </summary>
        /// <param name="ulrWeb"></param>
        /// <param name="version"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool CheckSystem(string ulrWeb, out string version, out string er)
        {
            _ulrWeb = ulrWeb;

            version = string.Empty;

            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "CheckSystem";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                version = requestXml;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 检查Web接口状态
        /// </summary>
        /// <param name="ulrWeb"></param>
        /// <param name="version"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool QueryVersionRecord(out string verList, out string er)
        {
            verList = string.Empty;

            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "QueryVersionRecord";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                verList = requestXml;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 流程功能
        /// <summary>
        /// 获取多流程列表
        /// </summary>
        /// <param name="flowList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool QueryFlowList(out List<CFlow> flowList, out string er)
        {
       

            flowList = new List<CFlow>();

            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "QueryFlowList";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输出表单
                DataSet ds = null;                  

                if (!ReponseXml(requestName, requestXml, out reponseXml,out ds, out er))
                    return false;

                if (!ds.Tables.Contains(C_XML_PARA))
                    return true;

                int index = -1;

                CFlow flow = null;

                for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
                {
                    int flowIndex = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["flowIndex"].ToString());

                    int flowId = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["flowId"].ToString());

                    string FlowName = ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();

                    int flowDisable = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["flowDisable"].ToString());

                    if (index == -1)
                    {
                        flow = new CFlow();
                        flow.Index = flowIndex;
                        flow.StatList = new List<CSTAT>();
                        index = flowIndex;
                    }
                    else if (index != flowIndex)
                    {
                        flowList.Add(flow);
                        flow = new CFlow();
                        flow.Index = flowIndex;
                        flow.StatList = new List<CSTAT>();
                    }                   

                    CSTAT stat = new CSTAT();
                    stat.Id = flowId;
                    stat.Name = FlowName;
                    stat.Status = flowDisable;
                    flow.StatList.Add(stat); 

                    if (i == ds.Tables[C_XML_PARA].Rows.Count - 1)
                    {
                        flowList.Add(flow);
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 获取单流程列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="flow"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool QueryFlowList(int index, out CFlow flow, out string er)
        {

            flow = new CFlow();

            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "QueryFlowList"; 

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Rows.Add(index);

                //输入参数
                string requestXml = CXml.ConvertDataTableToXML(in_ds);

                string reponseXml = string.Empty;

                //输出表单
                DataSet ds = null;                   

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (!ds.Tables.Contains(C_XML_PARA))
                    return true;

                flow.Index = index;

                flow.StatList = new List<CSTAT>();

                for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
                {
                    int flowId = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["flowId"].ToString());

                    string FlowName = ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();

                    int flowDisable = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["flowDisable"].ToString());

                    CSTAT stat = new CSTAT();
                    stat.Id = flowId;
                    stat.Name = FlowName;
                    stat.Status = flowDisable;
                    flow.StatList.Add(stat);
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 更新多流程列表
        /// </summary>
        /// <param name="flowList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool UpdateFlowList(List<CFlow> flowList, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "UpdateFlowList";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Rows.Add(-1);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowId");
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowName");
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowDisable");

                for (int i = 0; i < flowList.Count; i++)
                {
                    for (int z = 0; z < flowList[i].StatList.Count; z++)
                    {
                        in_ds.Tables[C_XML_PARA].Rows.Add(flowList[i].Index, flowList[i].StatList[z].Id, 
                                                          flowList[i].StatList[z].Name, flowList[i].StatList[z].Status);
                    }
                }                

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 条码管控
        /// <summary>
        /// 条码投入生产
        /// </summary>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool BandSnToFlow(CUUT uut, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "BandSnToFlow";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowGuid");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("Model");
                in_ds.Tables[C_XML_BASE].Columns.Add("OrderName");
                in_ds.Tables[C_XML_BASE].Columns.Add("MesFlag");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Rows.Add(uut.Base.FlowIndex,uut.Base.FlowName,uut.Base.FlowGuid,uut.Base.LineNo,uut.Base.LineName,
                                                  uut.Base.Model,uut.Base.OrderName,uut.Base.MesFlag,(int)uut.Base.SnType);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("InnerSn");
                in_ds.Tables[C_XML_PARA].Columns.Add("IdCard");
                in_ds.Tables[C_XML_PARA].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark1");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark2");
                in_ds.Tables[C_XML_PARA].Rows.Add(uut.Para.SerialNo, uut.Para.InnerSn, uut.Para.IdCard,
                                                  uut.Para.SlotNo, uut.Para.Remark1,uut.Para.Remark2);                

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 条码投入生产
        /// </summary>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool ReworkSnFlow(CUUT uut, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "ReworkSnFlow";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowGuid");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Rows.Add(uut.Base.FlowIndex, uut.Base.FlowName, uut.Base.FlowGuid, (int)uut.Base.SnType);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("InnerSn");
                in_ds.Tables[C_XML_PARA].Columns.Add("IdCard");
                in_ds.Tables[C_XML_PARA].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark1");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark2");
                in_ds.Tables[C_XML_PARA].Rows.Add(uut.Para.SerialNo, uut.Para.InnerSn, uut.Para.IdCard,
                                                  uut.Para.SlotNo, uut.Para.Remark1, uut.Para.Remark2);

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 获取条码流程信息
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <param name="SnType">0:外部条码 1:内部条码</param>
        /// <returns></returns>
        public static bool GetSnFlowInfo(string serialNo, out CUUT uut, out string er,int SnType=0)
        {
            er = string.Empty;

            uut = new CUUT();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "GetSnFlowInfo";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Rows.Add(serialNo, SnType);

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (ds.Tables.Contains(C_XML_BASE))
                {
                    uut.Base.SnType = (ESnType)(System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["SnType"].ToString()));
                    uut.Base.FlowIndex = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["FlowIndex"].ToString());
                    uut.Base.FlowId = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["FlowId"].ToString());
                    uut.Base.FlowName = ds.Tables[C_XML_BASE].Rows[0]["FlowName"].ToString();
                    uut.Base.FlowGuid = ds.Tables[C_XML_BASE].Rows[0]["FlowGuid"].ToString();
                    uut.Base.LineNo = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["LineNo"].ToString());
                    uut.Base.LineName = ds.Tables[C_XML_BASE].Rows[0]["LineName"].ToString();
                    uut.Base.Model = ds.Tables[C_XML_BASE].Rows[0]["Model"].ToString();
                    uut.Base.OrderName = ds.Tables[C_XML_BASE].Rows[0]["OrderName"].ToString();
                    uut.Base.MesFlag = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["MesFlag"].ToString());
                }
                
                if (ds.Tables.Contains(C_XML_PARA))
                {
                    uut.Para.SerialNo = ds.Tables[C_XML_PARA].Rows[0]["SerialNo"].ToString();
                    uut.Para.InnerSn = ds.Tables[C_XML_PARA].Rows[0]["InnerSn"].ToString();
                    uut.Para.IdCard = ds.Tables[C_XML_PARA].Rows[0]["IdCard"].ToString();
                    uut.Para.SlotNo = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[0]["SlotNo"].ToString());
                    uut.Para.Remark1 = ds.Tables[C_XML_PARA].Rows[0]["Remark1"].ToString();
                    uut.Para.Remark2 = ds.Tables[C_XML_PARA].Rows[0]["Remark2"].ToString();
                }
                

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 更新条码过站结果
        /// </summary>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool UpdateSnResult(CUUT uut, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "UpdateSnResult";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowGuid");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Columns.Add("CheckSn");
                in_ds.Tables[C_XML_BASE].Rows.Add(uut.Base.FlowIndex, uut.Base.FlowName,uut.Base.FlowGuid,
                                                  (int)uut.Base.SnType,uut.Base.CheckSn);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("Result");
                in_ds.Tables[C_XML_PARA].Columns.Add("TestData");
                in_ds.Tables[C_XML_PARA].Columns.Add("StartTime");
                in_ds.Tables[C_XML_PARA].Columns.Add("EndTime");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark1");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark2");
                in_ds.Tables[C_XML_PARA].Rows.Add(uut.Para.SerialNo, uut.Para.Result, uut.Para.TestData,
                                                  uut.Para.StartTime, uut.Para.EndTime,
                                                  uut.Para.Remark1, uut.Para.Remark2);

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 获取条码流程信息
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <param name="SnType">0:外部条码 1:内部条码</param>
        /// <returns></returns>
        public static bool GetSnRecord(string serialNo, out CSn sn, out string er, int SnType = 0)
        {
            er = string.Empty;

            sn = new CSn();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "GetSnRecord";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Rows.Add(serialNo, SnType);

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (ds.Tables.Contains(C_XML_BASE))
                {
                    sn.Base.IdCard = ds.Tables[C_XML_BASE].Rows[0]["IdCard"].ToString();
                    sn.Base.SlotNo = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["SlotNo"].ToString());
                    sn.Base.SerialNo = ds.Tables[C_XML_BASE].Rows[0]["SerialNo"].ToString();
                    sn.Base.InnerSn = ds.Tables[C_XML_BASE].Rows[0]["InnerSn"].ToString();
                    sn.Base.LineNo = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["LineNo"].ToString());
                    sn.Base.LineName = ds.Tables[C_XML_BASE].Rows[0]["LineName"].ToString();
                    sn.Base.Model = ds.Tables[C_XML_BASE].Rows[0]["Model"].ToString();
                    sn.Base.OrderName = ds.Tables[C_XML_BASE].Rows[0]["OrderName"].ToString();
                    sn.Base.MesFlag = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["MesFlag"].ToString());                  
                }

                if (ds.Tables.Contains(C_XML_PARA))
                {
                    for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CSn_Para para = new CSn_Para();
                        para.IdCard = ds.Tables[C_XML_PARA].Rows[i]["IdCard"].ToString();
                        para.SlotNo = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["SlotNo"].ToString());
                        para.FlowIndex = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["FlowIndex"].ToString());
                        para.FlowId = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["FlowId"].ToString());
                        para.FlowName = ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();
                        para.FlowGuid = ds.Tables[C_XML_PARA].Rows[i]["FlowGuid"].ToString();
                        para.Result = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["Result"].ToString());
                        para.TestData = ds.Tables[C_XML_PARA].Rows[i]["TestData"].ToString();
                        para.StartTime = ds.Tables[C_XML_PARA].Rows[i]["StartTime"].ToString();
                        para.EndTime = ds.Tables[C_XML_PARA].Rows[i]["EndTime"].ToString();
                        para.Remark1 = ds.Tables[C_XML_PARA].Rows[i]["Remark1"].ToString();
                        para.Remark2 = ds.Tables[C_XML_PARA].Rows[i]["Remark2"].ToString();
                        sn.Para.Add(para);  
                    }                    
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 获取条码流程信息
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <param name="SnType">0:外部条码 1:内部条码</param>
        /// <returns></returns>
        public static bool CheckSn(string flowName,string serialNo, out string er, int SnType = 0,int flowIndex=0)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "CheckSn";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Rows.Add(flowIndex,flowName, serialNo, SnType);

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 获取条码流程信息
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <param name="SnType">0:外部条码 1:内部条码</param>
        /// <returns></returns>
        public static bool QuerySnRecord(CSn_Query condition, out List<CSn_Para> snList, out string er)
        {
            er = string.Empty;

            snList = new List<CSn_Para>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "QuerySnRecord";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("StartTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("EndTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("Result");
                in_ds.Tables[C_XML_BASE].Rows.Add(
                                                  condition.StartTime,condition.EndTime,
                                                  condition.FlowIndex,condition.FlowName,
                                                  condition.SnType, condition.SerialNo,
                                                  condition.Result
                                                 );

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (ds.Tables.Contains(C_XML_PARA))
                {
                    for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CSn_Para para = new CSn_Para();
                        para.SerialNo = ds.Tables[C_XML_PARA].Rows[i]["SerialNo"].ToString();
                        para.IdCard = ds.Tables[C_XML_PARA].Rows[i]["IdCard"].ToString();
                        para.SlotNo = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["SlotNo"].ToString());
                        para.FlowIndex = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["FlowIndex"].ToString());
                        para.FlowId = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["FlowId"].ToString());
                        para.FlowName = ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();
                        para.FlowGuid = ds.Tables[C_XML_PARA].Rows[i]["FlowGuid"].ToString();
                        para.Result = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["Result"].ToString());
                        para.TestData = ds.Tables[C_XML_PARA].Rows[i]["TestData"].ToString();
                        para.StartTime = ds.Tables[C_XML_PARA].Rows[i]["StartTime"].ToString();
                        para.EndTime = ds.Tables[C_XML_PARA].Rows[i]["EndTime"].ToString();
                        para.Remark1 = ds.Tables[C_XML_PARA].Rows[i]["Remark1"].ToString();
                        para.Remark2 = ds.Tables[C_XML_PARA].Rows[i]["Remark2"].ToString();
                        snList.Add(para); 
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 治具管控
        /// <summary>
        /// 治具绑定条码
        /// </summary>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool BandSnToFixture(CFixture fixture, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "BandSnToFixture";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowGuid");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("Model");
                in_ds.Tables[C_XML_BASE].Columns.Add("OrderName");
                in_ds.Tables[C_XML_BASE].Columns.Add("MesFlag");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("MaxSlot");
                in_ds.Tables[C_XML_BASE].Columns.Add("FixtureType");
                in_ds.Tables[C_XML_BASE].Columns.Add("CheckSn");
                in_ds.Tables[C_XML_BASE].Rows.Add(fixture.Base.FlowIndex, fixture.Base.FlowName, fixture.Base.FlowGuid, fixture.Base.LineNo, fixture.Base.LineName,
                                                  fixture.Base.Model, fixture.Base.OrderName, fixture.Base.MesFlag, (int)fixture.Base.SnType,
                                                  fixture.Base.IdCard, fixture.Base.MaxSlot,(int)fixture.Base.FixtureType,fixture.Base.CheckSn);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("InnerSn");               
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark1");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark2");
                for (int i = 0; i < fixture.Para.Count; i++)
			    {
			       in_ds.Tables[C_XML_PARA].Rows.Add(fixture.Para[i].SlotNo,
                                                     fixture.Para[i].SerialNo, fixture.Para[i].InnerSn,
                                                     fixture.Para[i].Remark1, fixture.Para[i].Remark2);
			    }
              

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 重组治具绑定信息
        /// </summary>
        /// <param name="uut">重工或点检:点检治具需带结果</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool RegroupSnToFixture(CFixture fixture, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "RegroupSnToFixture";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("Model");
                in_ds.Tables[C_XML_BASE].Columns.Add("OrderName");
                in_ds.Tables[C_XML_BASE].Columns.Add("MesFlag");
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("MaxSlot");
                in_ds.Tables[C_XML_BASE].Columns.Add("FixtureType");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Rows.Add(fixture.Base.LineNo, fixture.Base.LineName,
                                                  fixture.Base.Model, fixture.Base.OrderName, fixture.Base.MesFlag,
                                                  fixture.Base.IdCard, fixture.Base.MaxSlot, 
                                                  (int)fixture.Base.FixtureType, (int)fixture.Base.SnType,
                                                  fixture.Base.FlowIndex);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("InnerSn");
                in_ds.Tables[C_XML_PARA].Columns.Add("Result");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark1");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark2");
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowName");
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowGuid");
                for (int i = 0; i < fixture.Para.Count; i++)
                {
                    in_ds.Tables[C_XML_PARA].Rows.Add(fixture.Para[i].SlotNo,
                                                      fixture.Para[i].SerialNo, fixture.Para[i].InnerSn,
                                                      fixture.Para[i].Result,
                                                      fixture.Para[i].Remark1, fixture.Para[i].Remark2,
                                                      fixture.Para[i].FlowName, fixture.Para[i].FlowGuid);                                                      
                }


                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 通过治具ID获取治具ID基本信息
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetFixtureInfo(string idCard, out CFix_Base fixture, out string er)
        {
            er = string.Empty;

            fixture = new CFix_Base();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "GetFixtureInfo";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard);

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (ds.Tables.Contains(C_XML_BASE))
                {
                    if (ds.Tables[C_XML_BASE].Rows.Count > 0)
                    {
                        fixture.IdCard = ds.Tables[C_XML_BASE].Rows[0]["IdCard"].ToString();
                        fixture.FixtureType = (EFixtureType)(System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["FixtureType"].ToString()));
                        fixture.SnType = (ESnType)(System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["SnType"].ToString()));
                        fixture.MaxSlot = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["MaxSlot"].ToString());
                        fixture.LineNo = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["LineNo"].ToString());
                        fixture.LineName = ds.Tables[C_XML_BASE].Rows[0]["LineName"].ToString();
                        fixture.Model = ds.Tables[C_XML_BASE].Rows[0]["Model"].ToString();
                        fixture.OrderName = ds.Tables[C_XML_BASE].Rows[0]["OrderName"].ToString();
                        fixture.MesFlag = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["MesFlag"].ToString());
                        fixture.FlowIndex = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["FlowIndex"].ToString());
                        fixture.FlowName = ds.Tables[C_XML_BASE].Rows[0]["FlowName"].ToString();
                        fixture.FlowGuid = ds.Tables[C_XML_BASE].Rows[0]["FlowGuid"].ToString();
                        fixture.UseNum = System.Convert.ToInt32(ds.Tables[C_XML_BASE].Rows[0]["UseNum"].ToString());
                        fixture.TTNum = System.Convert.ToInt32(ds.Tables[C_XML_BASE].Rows[0]["TTNum"].ToString());
                        fixture.FailNum = System.Convert.ToInt32(ds.Tables[C_XML_BASE].Rows[0]["FailNum"].ToString());
                        fixture.ConFailNum = System.Convert.ToInt32(ds.Tables[C_XML_BASE].Rows[0]["ConFailNum"].ToString());
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 通过治具ID获取治具ID绑定信息
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetFixtureInfoByIdCard(string idCard, out CFixture fixture, out string er)
        {
            er = string.Empty;

            fixture = new CFixture();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "GetFixtureInfoByIdCard";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard);

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (ds.Tables.Contains(C_XML_BASE))
                {
                    fixture.Base.IdCard = ds.Tables[C_XML_BASE].Rows[0]["IdCard"].ToString();
                    fixture.Base.FixtureType = (EFixtureType)(System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["FixtureType"].ToString()));
                    fixture.Base.SnType = (ESnType)(System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["SnType"].ToString()));
                    fixture.Base.MaxSlot = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["MaxSlot"].ToString());
                    fixture.Base.LineNo = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["LineNo"].ToString());
                    fixture.Base.LineName = ds.Tables[C_XML_BASE].Rows[0]["LineName"].ToString();
                    fixture.Base.Model = ds.Tables[C_XML_BASE].Rows[0]["Model"].ToString();
                    fixture.Base.OrderName = ds.Tables[C_XML_BASE].Rows[0]["OrderName"].ToString();
                    fixture.Base.MesFlag = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["MesFlag"].ToString());
                    fixture.Base.FlowIndex = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["FlowIndex"].ToString());
                    fixture.Base.FlowName = ds.Tables[C_XML_BASE].Rows[0]["FlowName"].ToString();
                    fixture.Base.FlowGuid = ds.Tables[C_XML_BASE].Rows[0]["FlowGuid"].ToString();                 
                }

                if (ds.Tables.Contains(C_XML_PARA))
                {
                    for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CFix_Para para = new CFix_Para();
                        para.SlotNo = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["SlotNo"].ToString());
                        para.SerialNo = ds.Tables[C_XML_PARA].Rows[i]["SerialNo"].ToString();
                        para.InnerSn = ds.Tables[C_XML_PARA].Rows[i]["InnerSn"].ToString();
                        para.Result = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["Result"].ToString());
                        para.FlowIndex = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["FlowIndex"].ToString());
                        para.FlowId = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["FlowId"].ToString());
                        para.FlowName = ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();
                        para.FlowGuid = ds.Tables[C_XML_PARA].Rows[i]["FlowGuid"].ToString();
                        para.Remark1 = ds.Tables[C_XML_PARA].Rows[i]["Remark1"].ToString();
                        para.Remark2 = ds.Tables[C_XML_PARA].Rows[i]["Remark2"].ToString();
                        fixture.Para.Add(para);
                    }                    
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 通过条码获取治具ID绑定信息
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <param name="snType">0:外部条码 1:内部条码</param>
        /// <returns></returns>
        public static bool GetFixtureInfoBySn(string serialNo, out CFixture fixture, out string er,int snType=0)
        {
            er = string.Empty;

            fixture = new CFixture();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "GetFixtureInfoBySn";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Rows.Add(serialNo,snType);

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (ds.Tables.Contains(C_XML_BASE))
                {
                    fixture.Base.IdCard = ds.Tables[C_XML_BASE].Rows[0]["IdCard"].ToString();
                    fixture.Base.FixtureType = (EFixtureType)(System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["FixtureType"].ToString()));
                    fixture.Base.SnType = (ESnType)(System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["SnType"].ToString()));
                    fixture.Base.MaxSlot = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["MaxSlot"].ToString());
                    fixture.Base.LineNo = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["LineNo"].ToString());
                    fixture.Base.LineName = ds.Tables[C_XML_BASE].Rows[0]["LineName"].ToString();
                    fixture.Base.Model = ds.Tables[C_XML_BASE].Rows[0]["Model"].ToString();
                    fixture.Base.OrderName = ds.Tables[C_XML_BASE].Rows[0]["OrderName"].ToString();
                    fixture.Base.MesFlag = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["MesFlag"].ToString());
                    fixture.Base.FlowIndex = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["FlowIndex"].ToString());
                    fixture.Base.FlowName = ds.Tables[C_XML_BASE].Rows[0]["FlowName"].ToString();
                    fixture.Base.FlowGuid = ds.Tables[C_XML_BASE].Rows[0]["FlowGuid"].ToString();
                }

                if (ds.Tables.Contains(C_XML_PARA))
                {
                    for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CFix_Para para = new CFix_Para();
                        para.SlotNo = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["SlotNo"].ToString());
                        para.SerialNo = ds.Tables[C_XML_PARA].Rows[i]["SerialNo"].ToString();
                        para.InnerSn = ds.Tables[C_XML_PARA].Rows[i]["InnerSn"].ToString();
                        para.Result = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["Result"].ToString());
                        para.FlowIndex = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["FlowIndex"].ToString());
                        para.FlowId = System.Convert.ToInt16(ds.Tables[C_XML_PARA].Rows[i]["FlowId"].ToString());
                        para.FlowName = ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();
                        para.FlowGuid = ds.Tables[C_XML_PARA].Rows[i]["FlowGuid"].ToString();
                        para.Remark1 = ds.Tables[C_XML_PARA].Rows[i]["Remark1"].ToString();
                        para.Remark2 = ds.Tables[C_XML_PARA].Rows[i]["Remark2"].ToString();
                        fixture.Para.Add(para);
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 更新条码过站结果
        /// </summary>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool UpdateFixtureResult(CFixture fixture, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "UpdateFixtureResult";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowGuid");
                in_ds.Tables[C_XML_BASE].Columns.Add("SnType");
                in_ds.Tables[C_XML_BASE].Columns.Add("CheckSn");
                in_ds.Tables[C_XML_BASE].Rows.Add(fixture.Base.IdCard, fixture.Base.FlowIndex,
                                                  fixture.Base.FlowName, fixture.Base.FlowGuid,
                                                  (int)fixture.Base.SnType, fixture.Base.CheckSn);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("Result");
                in_ds.Tables[C_XML_PARA].Columns.Add("TestData");
                in_ds.Tables[C_XML_PARA].Columns.Add("StartTime");
                in_ds.Tables[C_XML_PARA].Columns.Add("EndTime");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark1");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark2");
                for (int i = 0; i < fixture.Para.Count; i++)
                {
                    if (fixture.Base.SnType == 0)
                    {
                        in_ds.Tables[C_XML_PARA].Rows.Add(fixture.Para[i].SlotNo, fixture.Para[i].SerialNo,
                                                          fixture.Para[i].Result, fixture.Para[i].TestData,
                                                          fixture.Para[i].StartTime, fixture.Para[i].EndTime,
                                                          fixture.Para[i].Remark1, fixture.Para[i].Remark2);
                    }
                    else
                    {
                        in_ds.Tables[C_XML_PARA].Rows.Add(fixture.Para[i].SlotNo, fixture.Para[i].InnerSn,
                                                             fixture.Para[i].Result, fixture.Para[i].TestData,
                                                             fixture.Para[i].StartTime, fixture.Para[i].EndTime,
                                                             fixture.Para[i].Remark1, fixture.Para[i].Remark2);
                    }
                }
               

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 内外条码匹配
        /// <summary>
        /// 条码投入生产
        /// </summary>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool SerialNoMappingInnerSn(CMap map, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "SerialNoMappingInnerSn";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("MapMode");
                in_ds.Tables[C_XML_BASE].Columns.Add("MapRange");
                in_ds.Tables[C_XML_BASE].Rows.Add((int)map.Base.MapMode,(int)map.Base.MapRange);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("InnerSn");
                for (int i = 0; i < map.Para.Count; i++)
                {
                    in_ds.Tables[C_XML_PARA].Rows.Add(map.Para[i].SerialNo, map.Para[i].InnerSn);
                }

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 生产统计
        public static bool QueryProductivity(CYield_Base inputYield, out List<CYield_Para> outputYield, out string er)
        {
            outputYield = new List<CYield_Para>();

            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "QueryProductivity";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("StartTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("EndTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowGuid");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("OrderName");
                in_ds.Tables[C_XML_BASE].Columns.Add("Model");
                in_ds.Tables[C_XML_BASE].Rows.Add(inputYield.StartTime,inputYield.EndTime,inputYield.FlowIndex,
                                                  inputYield.FlowName,inputYield.FlowGuid,inputYield.LineNo,
                                                  inputYield.LineName,inputYield.OrderName,inputYield.Model);
                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (ds.Tables.Contains(C_XML_PARA))
                {
                    for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
			        {
			            CYield_Para para = new CYield_Para();

                        para.IdNo = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["IdNo"].ToString());

                        para.Name = ds.Tables[C_XML_PARA].Rows[i]["Name"].ToString();

                        para.TTNum = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["TTNum"].ToString());

                        para.FailNum = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["FailNum"].ToString());

                        outputYield.Add(para); 
			        }                  
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 治具统计
        /// <summary>
        /// 查询治具条码使用次数
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="idCardList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool QueryIdCardUseNum(CFixCondition condition, out List<CFixUseNum> idCardList, out string er)
        {
            idCardList = new List<CFixUseNum>();

            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "QueryIdCardUseNum";

                //输入参数
                string requestXml = string.Empty;
                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowIndex");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_BASE].Rows.Add(condition.FlowIndex, condition.FlowName, condition.IdCard,condition.SlotNo);
                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (ds.Tables.Contains(C_XML_PARA))
                {
                    for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
			        {
                        CFixUseNum fix = new CFixUseNum();

                        fix.FlowIndex = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["FlowIndex"].ToString());

                        fix.FlowName = ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();

                        fix.IdCard = ds.Tables[C_XML_PARA].Rows[i]["IdCard"].ToString();

                        fix.SlotNo = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["SlotNo"].ToString());

                        fix.TTNum = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["TTNum"].ToString());

                        fix.FailNum = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["FailNum"].ToString());

                        fix.ConFailNum = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["ConFailNum"].ToString());

                        idCardList.Add(fix); 
			        }                  
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 报警列表
        /// <summary>
        /// 记录报警信息
        /// </summary>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool InsertAlarmRecord(CAlarm alarm, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "InsertAlarmRecord";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Columns.Add("StatGuid");
                in_ds.Tables[C_XML_BASE].Rows.Add(alarm.Base.LineNo,alarm.Base.LineName,alarm.Base.StatName,alarm.Base.StatGuid);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("ErrNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("ErrCode");
                in_ds.Tables[C_XML_PARA].Columns.Add("bAlarm");
                in_ds.Tables[C_XML_PARA].Columns.Add("AlarmInfo");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark1");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark2");
                for (int i = 0; i < alarm.Para.Count; i++)
                {
                    in_ds.Tables[C_XML_PARA].Rows.Add(alarm.Para[i].ErrNo, alarm.Para[i].ErrCode,
                                                 alarm.Para[i].bAlarm, alarm.Para[i].AlarmInfo,
                                                 alarm.Para[i].Remark1, alarm.Para[i].Remark2);
                }               

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 获取报警记录
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="alarmList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetAlarmRecord(CAlarm_Base condition, out List<CAlarmRecord> alarmList, out string er)
        {
            alarmList = new List<CAlarmRecord>();

            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "GetAlarmRecord";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("StartTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("EndTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Columns.Add("StatGuid");
                in_ds.Tables[C_XML_BASE].Columns.Add("ErrNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("bAlarm");
                in_ds.Tables[C_XML_BASE].Rows.Add(
                                                   condition.StartTime, condition.EndTime,
                                                   condition.StatName,condition.StatGuid,
                                                   condition.ErrNo,condition.bAlarm
                                                 );
                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (ds.Tables.Contains(C_XML_PARA))
                {
                    for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CAlarmRecord alarm = new CAlarmRecord();
                        alarm.LineNo = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["LineNo"].ToString());
                        alarm.LineName = ds.Tables[C_XML_PARA].Rows[i]["LineName"].ToString();
                        alarm.StatName = ds.Tables[C_XML_PARA].Rows[i]["StatName"].ToString();
                        alarm.StatGuid = ds.Tables[C_XML_PARA].Rows[i]["StatGuid"].ToString();
                        alarm.ErrNo = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["ErrNo"].ToString());
                        alarm.ErrCode = ds.Tables[C_XML_PARA].Rows[i]["ErrCode"].ToString();
                        alarm.bAlarm = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["bAlarm"].ToString());
                        alarm.AlarmInfo = ds.Tables[C_XML_PARA].Rows[i]["AlarmInfo"].ToString();
                        alarm.HappenTime = ds.Tables[C_XML_PARA].Rows[i]["HappenTime"].ToString();
                        alarm.Remark1 = ds.Tables[C_XML_PARA].Rows[i]["Remark1"].ToString();
                        alarm.Remark2 = ds.Tables[C_XML_PARA].Rows[i]["Remark2"].ToString();
                        alarmList.Add(alarm); 
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 易损件记录
        /// <summary>
        /// 查询治具条码使用次数
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="idCardList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool InsertFailPartRecord(List<CPartRecord> partRecords, out string er)
        {
         
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "InsertFailPartRecord";

                //输入参数
                string requestXml = string.Empty;
                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("PartType");
                in_ds.Tables[C_XML_BASE].Columns.Add("PartName");
                in_ds.Tables[C_XML_BASE].Columns.Add("PartSlotNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("PartCarrier");
                in_ds.Tables[C_XML_BASE].Columns.Add("LocalName");
                in_ds.Tables[C_XML_BASE].Columns.Add("AlarmInfo");
                in_ds.Tables[C_XML_BASE].Columns.Add("TTNum");
                in_ds.Tables[C_XML_BASE].Columns.Add("FailNum");
                in_ds.Tables[C_XML_BASE].Columns.Add("ConFailNum");
                for (int i = 0; i < partRecords.Count; i++)
                {
                    in_ds.Tables[C_XML_BASE].Rows.Add(
                                                      partRecords[i].LineNo, partRecords[i].LineName,
                                                      partRecords[i].PartType, partRecords[i].PartName,
                                                      partRecords[i].PartSlotNo, partRecords[i].PartCarrier,
                                                      partRecords[i].LocalName, partRecords[i].AlarmInfo,
                                                      partRecords[i].TTNum, partRecords[i].FailNum,
                                                      partRecords[i].ConFailNum
                                                      );
                }
                
                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 查询治具条码使用次数
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="idCardList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool QueryFailPartRecord(CPartCondition condition, out List<CPartRecord> partRecords, out string er)
        {

            er = string.Empty;

            partRecords = new List<CPartRecord>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "QueryFailPartRecord";

                //输入参数
                string requestXml = string.Empty;
                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("StartTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("EndTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("PartType");
                in_ds.Tables[C_XML_BASE].Columns.Add("PartName");
                in_ds.Tables[C_XML_BASE].Columns.Add("PartSlotNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("TTNum");
                in_ds.Tables[C_XML_BASE].Columns.Add("FailNum");
                in_ds.Tables[C_XML_BASE].Columns.Add("ConFailNum");
                in_ds.Tables[C_XML_BASE].Rows.Add(
                                                  condition.StartTime, condition.EndTime,
                                                  (int)condition.PartType,condition.PartName,
                                                  condition.PartSlotNo, condition.TTNum,
                                                  condition.FailNum, condition.ConFailNum 
                                                  );
                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (ds.Tables.Contains(C_XML_PARA))
                {
                    for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CPartRecord part = new CPartRecord();
                        part.LineNo = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["LineNo"].ToString());
                        part.LineName = ds.Tables[C_XML_PARA].Rows[i]["LineName"].ToString();
                        part.PartType = (EPartType)System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["PartType"].ToString());
                        part.PartName = ds.Tables[C_XML_PARA].Rows[i]["PartName"].ToString();
                        part.PartSlotNo = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["PartSlotNo"].ToString());
                        part.PartCarrier = ds.Tables[C_XML_PARA].Rows[i]["PartCarrier"].ToString();
                        part.LocalName = ds.Tables[C_XML_PARA].Rows[i]["LocalName"].ToString();
                        part.AlarmInfo = ds.Tables[C_XML_PARA].Rows[i]["AlarmInfo"].ToString();
                        part.AlarmTime = ds.Tables[C_XML_PARA].Rows[i]["AlarmTime"].ToString();
                        part.TTNum = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["TTNum"].ToString());
                        part.FailNum = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["FailNum"].ToString());
                        part.ConFailNum = System.Convert.ToInt32(ds.Tables[C_XML_PARA].Rows[i]["ConFailNum"].ToString());
                        part.Remark1 = ds.Tables[C_XML_PARA].Rows[i]["Remark1"].ToString();
                        part.Remark2 = ds.Tables[C_XML_PARA].Rows[i]["Remark2"].ToString();
                        partRecords.Add(part);
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 机型信息
        /// <summary>
        /// 插入机型信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool InsertModelInfo(CModel model, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "InsertModelInfo";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("StationName");
                in_ds.Tables[C_XML_BASE].Rows.Add(model.Base.LineNo, model.Base.LineName,model.Base.StationName);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("ModelName");
                in_ds.Tables[C_XML_PARA].Columns.Add("ModelSn");
                in_ds.Tables[C_XML_PARA].Columns.Add("ModelPara");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark1");
                in_ds.Tables[C_XML_PARA].Columns.Add("Remark2");

                for (int i = 0; i < model.Para.Count; i++)
                {
                    in_ds.Tables[C_XML_PARA].Rows.Add(model.Para[i].ModelName, model.Para[i].ModelSn, model.Para[i].ModelPara,
                                                      model.Para[i].Remark1, model.Para[i].Remark2);
                }
               

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 获取机种信息
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="model"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetModelInfo(CModel_Condition condition,out bool IsExist, out CModel model, out string er)
        {
            er = string.Empty;

            IsExist = true;

            model = new CModel();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "GetModelInfo";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("StationName");
                in_ds.Tables[C_XML_BASE].Columns.Add("ModelName");
                in_ds.Tables[C_XML_BASE].Columns.Add("ModelSn");
                in_ds.Tables[C_XML_BASE].Columns.Add("Status");
                in_ds.Tables[C_XML_BASE].Rows.Add(
                                                  condition.LineNo, condition.LineName,
                                                  condition.StationName, condition.ModelName,
                                                  condition.ModelSn, condition.Status
                                                 );

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;
                
                if (ds.Tables.Contains(C_XML_BASE))
                {
                    model.Base.LineNo = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["LineNo"].ToString());
                    model.Base.LineName = ds.Tables[C_XML_BASE].Rows[0]["LineName"].ToString();
                    model.Base.StationName = ds.Tables[C_XML_BASE].Rows[0]["StationName"].ToString();
                    int Count = System.Convert.ToInt16(ds.Tables[C_XML_BASE].Rows[0]["Count"].ToString());
                    if (Count == 0)
                    {
                        IsExist = false;
                        return true;
                    }
                }

                if (ds.Tables.Contains(C_XML_PARA))
                {
                    for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CModel_Para para = new CModel_Para();
                        para.ModelName = ds.Tables[C_XML_PARA].Rows[i]["ModelName"].ToString();
                        para.ModelSn = ds.Tables[C_XML_PARA].Rows[i]["ModelSn"].ToString();
                        para.ModelPara = ds.Tables[C_XML_PARA].Rows[i]["ModelPara"].ToString();
                        para.ImportDate = ds.Tables[C_XML_PARA].Rows[i]["ImportDate"].ToString();
                        para.ModifyDate = ds.Tables[C_XML_PARA].Rows[i]["ModifyDate"].ToString();
                        para.Remark1 = ds.Tables[C_XML_PARA].Rows[i]["Remark1"].ToString();
                        para.Remark2 = ds.Tables[C_XML_PARA].Rows[i]["Remark2"].ToString();
                        model.Para.Add(para);
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        /// <summary>
        /// 更新机种信息状态
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="model"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool UpdateModelInfoStatus(CModel_Condition condition, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "UpdateModelInfoStatus";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("StationName");
                in_ds.Tables[C_XML_BASE].Columns.Add("ModelName");
                in_ds.Tables[C_XML_BASE].Columns.Add("Status");
                in_ds.Tables[C_XML_BASE].Rows.Add(
                                                  condition.LineNo, condition.LineName,
                                                  condition.StationName, condition.ModelName,
                                                  condition.Status
                                                 );

                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region 执行SQL语句
        /// <summary>
        /// 更新条码过站结果
        /// </summary>
        /// <param name="uut"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool ExcuteSQLCmd(ESqlCmdType CmdType, List<string> CmdList, out List<DataTable> dtList, out string er)
        {
            er = string.Empty;

            dtList = new List<DataTable>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                //接口函数
                string requestName = "ExcuteSQLCmd";

                //输入参数
                string requestXml = string.Empty;

                string reponseXml = string.Empty;

                //输入表单
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("CmdType");
                in_ds.Tables[C_XML_BASE].Rows.Add((int)CmdType);

                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("Command");
                for (int i = 0; i < CmdList.Count; i++)
                {
                    in_ds.Tables[C_XML_PARA].Rows.Add(CmdList[i]); 
                }
                
                //输入参数
                requestXml = CXml.ConvertDataTableToXML(in_ds);

                //输出表单
                DataSet ds = null;

                if (!ReponseXml(requestName, requestXml, out reponseXml, out ds, out er))
                    return false;

                if (CmdType == ESqlCmdType.ExecuteQuery)
                {
                    if (ds.Tables.Contains(C_XML_PARA) && ds.Tables[C_XML_PARA].Columns.Contains("TableXml"))
                    {
                        for (int i = 0; i < ds.Tables[C_XML_PARA].Rows.Count; i++)
                        {
                            string tbXml = ds.Tables[C_XML_PARA].Rows[i]["TableXml"].ToString();

                            DataSet tb = CXml.ConvertXMLToDataSet(tbXml);

                            if (tb != null)
                            {
                                dtList.Add(tb.Tables[0]); 
                            }
                        }
                    }
                }

                er = "【Request】" + "\r\n";
                er += requestXml + "\r\n";
                er += "【Reponse】" + "\r\n";
                er += reponseXml;

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
            finally
            {
                webLock.ReleaseWriterLock();
            }
        }
        #endregion

        #region XML请求响应格式
        /// <summary>
        /// 应答请求消息
        /// </summary>
        /// <param name="strXml"></param>
        /// <param name="dataXml"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private static bool ReponseXml(string requestName, string requestXml, out string reponseXml,out DataSet ds, out string er)
        {
            reponseXml = string.Empty;

            er = string.Empty;

            ds = null;

            Stopwatch watcher = new Stopwatch();

            watcher.Start();

            try
            {
                string dataXml = string.Empty;

                //格式化XML
                string strXML = FormatRequestXml(requestName, requestXml);

                if (!CNet.PostMessageToHttp(_ulrWeb, strXML, out reponseXml, out er))
                    return false;

                if (!FormatReponseXml(reponseXml, out dataXml, out er))
                    return false;

                if (dataXml == string.Empty)
                {
                    er = CLanguage.Lan("数据错误") + ":" + reponseXml;
                    return false;
                }

                reponseXml = dataXml;

                ds = CXml.ConvertXMLToDataSet(reponseXml);

                if (ds == null)
                {
                    er = CLanguage.Lan("数据错误") + ":" + reponseXml;
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
                watcher.Stop();

                WaitTime = watcher.ElapsedMilliseconds;
            }
        }
        /// <summary>
        /// 初始化Http请求Xml
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="requestXml"></param>
        /// <returns></returns>
        private static string FormatRequestXml(string requestName, string requestXml)
        {
            try
            {
                requestXml = requestXml.Replace("&", "&amp;");
                requestXml = requestXml.Replace("<", "&lt;");
                requestXml = requestXml.Replace(">", "&gt;");
                requestXml = requestXml.Replace("'", "&apos;");
                requestXml = requestXml.Replace("\"", "&quot;"); //((char)34) 双引号

                string webXml = string.Empty;
                webXml += "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + "\r\n";
                webXml += "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance" +
                          "\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema" + "\" xmlns:soap=\"" +
                          "http://schemas.xmlsoap.org/soap/envelope/" + "\">" + "\r\n";
                webXml += "<soap:Body>" + "\r\n";
                webXml += "<" + requestName + " xmlns=\"" + "http://tempuri.org/" + "\">" + "\r\n";
                webXml += "<xmlRequest>" + requestXml + "</xmlRequest>" + "\r\n";
                webXml += "</" + requestName + ">" + "\r\n";
                webXml += "</soap:Body>" + "\r\n";
                webXml += "</soap:Envelope>";
                return webXml;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 解析响应Xml数据
        /// </summary>
        /// <param name="reponseXml"></param>
        /// <param name="xmlData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private static bool FormatReponseXml(string reponseXml, out string xmlData, out  string er)
        {
            xmlData = string.Empty;

            er = string.Empty;

            try
            {
                string result = string.Empty;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reponseXml);
                XmlElement rootElem = doc.DocumentElement;   //获取根节点  
                XmlNodeList xnl = rootElem.ChildNodes;       //得到根节点的所有子节点
                if (xnl.Count == 0)
                {
                    er = "服务器应答超时:" + rootElem.InnerXml;
                    return false;
                }
                foreach (XmlNode xn1 in xnl)
                {
                    // 将节点转换为元素，便于得到节点的属性值
                    XmlElement xe = (XmlElement)xn1;
                    reponseXml = xe.InnerXml;
                    XmlDocument doc1 = new XmlDocument();
                    doc1.LoadXml(reponseXml);
                    XmlElement rootElem1 = doc1.DocumentElement;   //获取根节点
                    XmlNodeList xnl1 = rootElem1.ChildNodes;
                    if (xnl1.Count < 3)
                    {
                        er = "服务器应答数据错误:" + rootElem1.InnerXml;
                        return false;
                    }
                    xmlData = xnl1.Item(0).InnerText;
                    result = xnl1.Item(1).InnerText;
                    er = xnl1.Item(2).InnerText;
                }
                if (result.ToUpper() != "TRUE")
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
