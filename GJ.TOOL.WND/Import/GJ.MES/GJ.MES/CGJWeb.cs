using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using System.Net;
using System.Threading;

namespace GJ.MES
{
    /// <summary>
    /// 冠佳WebService
    /// 版本:V1.0.1
    /// </summary>
    public class CGJWeb
    {
        #region 常量
        /// <summary>
        /// 数据包头
        /// </summary>
        private const string C_XML_HEADER = "GJMES";
        /// <summary>
        /// 输入基础表单名
        /// </summary>
        private const string C_XML_BASE = "XmlBase";
        /// <summary>
        /// 返回参数表单名
        /// </summary>
        private const string C_XML_PARA = "XmlPara";
        #endregion

        #region 枚举
        /// <summary>
        /// 治具状态
        /// </summary>
        public enum EFIX_STATUS
        { 
          禁用=-1,
          正常=0,
          空治具=1
        }
        /// <summary>
        ///站别运行状态
        /// </summary>
        public enum ESTAT_RUN
        {
           空闲,
           运行,
           停止,
           报警
        }
        /// <summary>
        /// 治具运行状态
        /// </summary>
        public enum EFIX_RUN
        { 
          空闲,
          进站,
          测试,
          良品出站,
          不良出站
        }
        #endregion

        #region 类定义
        /// <summary>
        /// 流程
        /// </summary>
        public class CFLOW
        {
            public int Id=0;
            public string Name=string.Empty;
            public int Disable=0;
        }
        /// <summary>
        /// 工位
        /// </summary>
        public class CSTAT
        {
            public int Id=0;
            public string Name=string.Empty;
            public int Disable=0;
        }
        /// <summary>
        /// 工位与流程
        /// </summary>
        public class CSTAT_FLOW
        {
            public int StatId=0;
            public string StatName=string.Empty;
            public int FlowId=0;
            public string FlowName=string.Empty;
            public int Disable=0;
        }
        /// <summary>
        /// 治具信息
        /// </summary>
        public class CFIX_INFO
        {
            /// <summary>
            /// 治具ID
            /// </summary>
            public string IdCard=string.Empty;
            /// <summary>
            /// 最大槽位数
            /// </summary>
            public int MaxSlot=16;
            /// <summary>
            /// 使用状态
            /// </summary>
            public EFIX_STATUS UseStatus = EFIX_STATUS.正常;
            /// <summary>
            /// 线体编号
            /// </summary>
            public int LineNo = 0;
            /// <summary>
            /// 线体名称
            /// </summary>
            public string LineName = string.Empty;
            /// <summary>
            /// 机种名称
            /// </summary>
            public string Model = string.Empty;
            /// <summary>
            /// 工单号
            /// </summary>
            public string OrderName = string.Empty;
            /// <summary>
            /// 当前流程
            /// </summary>
            public string CurFlowName = string.Empty;
            /// <summary>
            /// 客户MES
            /// </summary>
            public int MesFlag = 0;
        }
        /// <summary>
        /// 条码信息
        /// </summary>
        public class CSN_INFO
        {
            /// <summary>
            /// 条码
            /// </summary>
            public string SerialNo = string.Empty;
            /// <summary>
            /// 当前流程编号
            /// </summary>
            public int CurFlowId = 0;
            /// <summary>
            /// 当前流程名称
            /// </summary>
            public string CurFlowName = "Banding";
            /// <summary>
            /// 当前设备名称
            /// </summary>
            public string CurStatName = "Banding";
            /// <summary>
            /// 当前流程结果
            /// </summary>
            public int CurResult = 0;
        }
        /// <summary>
        /// 治具绑定信息
        /// </summary>
        public class CFIX_BAND
        {
            /// <summary>
            /// 基本信息
            /// </summary>
            public CFIX_INFO Base = new CFIX_INFO();
            /// <summary>
            /// 参数信息
            /// </summary>
            public List<CSN_INFO> Para = new List<CSN_INFO>();
        }
        /// <summary>
        /// 产品信息
        /// </summary>
        public class CSN_BASE
        {
            /// <summary>
            /// 治具ID
            /// </summary>
            public string IdCard = string.Empty;
            /// <summary>
            /// 当前流程
            /// </summary>
            public string CurFlowName = string.Empty;
            /// <summary>
            /// 当前站别
            /// </summary>
            public string CurStatName = string.Empty;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime = string.Empty;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime = string.Empty;
            /// <summary>
            /// 当前位置
            /// </summary>
            public string LocalName = string.Empty;
            /// <summary>
            /// 条码检查
            /// </summary>
            public int CheckSn = 0;
        }
        /// <summary>
        /// 产品结果
        /// </summary>
        public class CSN_RESULT
        {
            /// <summary>
            /// 产品条码
            /// </summary>
            public string SerialNo=string.Empty;
            /// <summary>
            /// 测试结果
            /// </summary>
            public int CurResult=0;
            /// <summary>
            /// 测试数据
            /// </summary>
            public string CurTestData=string.Empty;
        }
        /// <summary>
        /// 治具结果
        /// </summary>
        public class CFIX_RESULT
        {
            public CSN_BASE Base = new CSN_BASE();

            public List<CSN_RESULT> Para = new List<CSN_RESULT>();
        }
        /// <summary>
        /// 工位产能
        /// </summary>
        public class CSTAT_YIELD
        {
            public string StatName = string.Empty;

            public int FlowId = 0;

            public string FlowName = string.Empty;

            public int TTNum = 0;

            public int FailNum = 0;
        }
        /// <summary>
        /// 治具在站别产能
        /// </summary>
        public class CFIX_STAT_YIELD
        {
            public int SlotNo = 0;

            public string StatName = string.Empty;

            public int FlowId = 0;

            public string FlowName = string.Empty;

            public int TTNum = 0;

            public int FailNum = 0;

            public int ConFailNum = 0;

        }
        /// <summary>
        /// 治具产能
        /// </summary>
        public class CFIX_YIELD
        {
            /// <summary>
            /// 治具ID
            /// </summary>
            public string IdCard = string.Empty;
            /// <summary>
            /// 总数
            /// </summary>
            public int TTNum = 0;
            /// <summary>
            /// 不良数
            /// </summary>
            public int FailNum = 0;
            /// <summary>
            /// 连续不良数
            /// </summary>
            public int ConFailNum = 0;
            /// <summary>
            /// 工位槽位产能
            /// </summary>
            public List<CFIX_STAT_YIELD> Stat = new List<CFIX_STAT_YIELD>();
        }
        /// <summary>
        /// 条码测试结果
        /// </summary>
        public class CSN_TESTDATA
        {
            /// <summary>
            /// 产品条码
            /// </summary>
            public string SerialNo = string.Empty;
            /// <summary>
            /// 流程编号
            /// </summary>
            public int CurFlowId = 0;
            /// <summary>
            /// 流程名称
            /// </summary>
            public string CurFlowName = string.Empty;
            /// <summary>
            /// 站别名称
            /// </summary>
            public string CurStatName = string.Empty;
            /// <summary>
            /// 测试结果
            /// </summary>
            public int CurResult = 0;
            /// <summary>
            /// 测试数据
            /// </summary>
            public string CurTestData = string.Empty;
            /// <summary>
            /// 开始时间
            /// </summary>
            public string StartTime = string.Empty;
            /// <summary>
            /// 结束时间
            /// </summary>
            public string EndTime = string.Empty;
            /// <summary>
            /// 线体编号
            /// </summary>
            public int LineNo = 0;
            /// <summary>
            /// 线体名称
            /// </summary>
            public string LineName = string.Empty;
            /// <summary>
            /// 机种名称
            /// </summary>
            public string Model = string.Empty;
            /// <summary>
            /// 工单号 
            /// </summary>
            public string OrderName = string.Empty;
            /// <summary>
            /// 治具位置
            /// </summary>
            public string Fixture = string.Empty;
            /// <summary>
            /// 测试位置
            /// </summary>
            public string LocalName = string.Empty;
            /// <summary>
            /// MES返回信息
            /// </summary>
            public string MESInfo = string.Empty;
            /// <summary>
            /// MES数据
            /// </summary>
            public string MESXml = string.Empty;
        }
        /// <summary>
        /// 站别运行状态
        /// </summary>
        public class CSTAT_RUN
        {
           /// <summary>
           /// 站别
           /// </summary>
           public string StatName=string.Empty;
          /// <summary>
          /// 状态
          /// </summary>
           public ESTAT_RUN RunStatus=ESTAT_RUN.空闲;
           /// <summary>
           /// 设置时间
           /// </summary>
           public string StatusTime=string.Empty;
           /// <summary>
           /// 报警信息
           /// </summary>
           public string ErrCode=string.Empty;
        }
        /// <summary>
        /// 治具运行状态
        /// </summary>
        public class CFIX_RUN
        {
            public string IdCard = string.Empty;

            public string StatName = string.Empty;

            public int FlowId = 0;

            public string FlowName = string.Empty;

            public EFIX_RUN RunStatus = EFIX_RUN.空闲;

            public string StatusTime = string.Empty;
        }
        /// <summary>
        /// 点检治具
        /// </summary>
        public class CFIX_SAMPLE
        {
            public bool IsSample = false;

            public string IdCard = string.Empty;

            public int CurFlowId = 0;

            public string CurFlowName = string.Empty;

            public int SetEnable = 0;

            public List<string> SerialNo = new List<string>();

            public List<int> Result = new List<int>();
        }
        /// <summary>
        /// 治具信息
        /// </summary>
        public class CFIX_USE
        {
            /// <summary>
            /// 治具ID
            /// </summary>
            public string IdCard = string.Empty;
            /// <summary>
            /// 状态
            /// </summary>
            public EFIX_STATUS UseStatus = EFIX_STATUS.正常;
            /// <summary>
            /// 总数
            /// </summary>
            public int TTNum = 0;
            /// <summary>
            /// 不良数
            /// </summary>
            public int FailNum = 0;
            /// <summary>
            /// 连续不良数
            /// </summary>
            public int ConFailNum = 0;
        }
        /// <summary>
        /// PIN使用次数管控
        /// </summary>
        public class CPIN
        {
            /// <summary>
            /// 流程编号
            /// </summary>
            public int FlowId = 0;
            /// <summary>
            /// 工位名称
            /// </summary>
            public string StatName = string.Empty;
            /// <summary>
            /// PIN位置
            /// </summary>
            public string PosName = string.Empty;
            /// <summary>
            /// 使用次数
            /// </summary>
            public int UseNum = -1;
            /// <summary>
            /// 报警次数
            /// </summary>
            public int AlarmTime = 0;
            /// <summary>
            /// 寿命次数
            /// </summary>
            public int LimitTime = 0;
            /// <summary>
            /// 检查日期
            /// </summary>
            public string CheckDate = string.Empty;
            /// <summary>
            /// 检查次数
            /// </summary>
            public int CheckTime = 0;
            /// <summary>
            /// 检查人员
            /// </summary>
            public string CheckName = string.Empty;

        }
        /// <summary>
        /// 治具CheckList
        /// </summary>
        public class CRFIDList
        {
            /// <summary>
            /// 治具ID
            /// </summary>
            public string IdCard = string.Empty;
            /// <summary>
            /// 报警次数
            /// </summary>
            public int AlarmTime = 0;
            /// <summary>
            /// 寿命次数
            /// </summary>
            public int LimitTime = 0;
            /// <summary>
            /// 检查日期
            /// </summary>
            public string CheckDate = string.Empty;
            /// <summary>
            /// 更换内容
            /// </summary>
            public string CheckContext = string.Empty;
            /// <summary>
            /// 检查次数
            /// </summary>
            public int CheckTime = 0;
            /// <summary>
            /// 检查人员
            /// </summary>
            public string CheckName = string.Empty;
        }
        /// <summary>
        /// 产能统计
        /// </summary>
        public class CYieldRequest
        {
            public string FlowName = string.Empty;

            public string StartTime = string.Empty;

            public string EndTime = string.Empty;
        }

        public class CYieldReponse
        {
            public string FlowName = string.Empty;

            public int TTNum = 0;

            public int FailNum = 0;
        }
        #endregion

        #region 字段
        /// <summary>
        /// Web网站
        /// </summary>
        private static string _ulrWeb = "";
        /// <summary>
        /// 同步锁
        /// </summary>
        private static ReaderWriterLock webLock = new ReaderWriterLock();
        #endregion

        #region 属性
        /// <summary>
        /// Web网站
        /// </summary>
        public static string ulrWeb
        {
            set { _ulrWeb = value; }
            get { return _ulrWeb; }
        }
        #endregion

        #region 基础方法
        /// <summary>
        /// 检查Web接口
        /// </summary>
        /// <param name="ulrWeb">Web地址</param>
        /// <param name="version">当前Web版本</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool CheckSystem(string ulrWeb, out string version, out string er)
        {
            version = string.Empty;

            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                _ulrWeb = ulrWeb;

                er = string.Empty;

                string requestName = "CheckSystem";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                version = DataXML;

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
        /// 指定功能消息
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="requestXml"></param>
        /// <param name="reponseXml"></param>
        /// <returns></returns>
        public static bool PostFunction(string requestName, string requestXml, out string reponseXml,out string er)
        {
            er = string.Empty;

            reponseXml = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                string strXML = string.Empty;

                string DataXML = string.Empty;

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                reponseXml = DataXML;

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
        /// 获取流程列表
        /// </summary>
        /// <param name="ulrWeb"></param>
        /// <param name="er"></param>
        public static bool GetFlowList(out List<CFLOW> flowList, out string er)
        {
            er = string.Empty;

            flowList = new List<CFLOW>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetFlowList";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CFLOW flow = new CFLOW();
                        flow.Id = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["FlowId"].ToString());
                        flow.Name = out_ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();
                        flow.Disable = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["FlowDisable"].ToString());
                        flowList.Add(flow);
                    }
                }

                er = DataXML;

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
        /// 获取站别列表
        /// </summary>
        /// <param name="ulrWeb"></param>
        /// <param name="er"></param>
        public static bool GetStatList(out List<CSTAT> statList, out string er)
        {
            er = string.Empty;

            statList = new List<CSTAT>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetStatList";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CSTAT stat = new CSTAT();
                        stat.Id = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["StatId"].ToString());
                        stat.Name = out_ds.Tables[C_XML_PARA].Rows[i]["StatName"].ToString();
                        stat.Disable = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["StatDisable"].ToString());
                        statList.Add(stat);
                    }
                }

                er = DataXML;

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
        /// 获取站别与流程列表
        /// </summary>
        /// <param name="ulrWeb"></param>
        /// <param name="er"></param>
        public static bool GetStatFlowList(out List<CSTAT_FLOW> statFlowList, out string er)
        {
            er = string.Empty;

            statFlowList = new List<CSTAT_FLOW>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetStatFlowList";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CSTAT_FLOW statFlow = new CSTAT_FLOW();
                        statFlow.StatId = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["StatId"].ToString());
                        statFlow.StatName = out_ds.Tables[C_XML_PARA].Rows[i]["StatName"].ToString();
                        statFlow.FlowId = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["FlowId"].ToString());
                        statFlow.FlowName = out_ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();
                        statFlow.Disable = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["MapDisable"].ToString());
                        statFlowList.Add(statFlow);
                    }
                }

                er = DataXML;

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
        /// 设置流程列表
        /// </summary>
        /// <param name="flowList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool SetFlowList(List<CFLOW> flowList, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                string requestName = "SetFlowList";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表

                DataSet in_ds = new DataSet(C_XML_HEADER);

                //参数表单
                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowId");
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowName");
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowDisable");

                for (int i = 0; i < flowList.Count; i++)
                    in_ds.Tables[C_XML_PARA].Rows.Add(flowList[i].Id,flowList[i].Name,flowList[i].Disable);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 设置站别列表
        /// </summary>
        /// <param name="flowList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool SetStatList(List<CSTAT> statList, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                string requestName = "SetStatList";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表

                DataSet in_ds = new DataSet(C_XML_HEADER);

                //参数表单
                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("StatId");
                in_ds.Tables[C_XML_PARA].Columns.Add("StatName");
                in_ds.Tables[C_XML_PARA].Columns.Add("StatDisable");

                for (int i = 0; i < statList.Count; i++)
                    in_ds.Tables[C_XML_PARA].Rows.Add(statList[i].Id, statList[i].Name, statList[i].Disable);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;
                
                er = DataXML;

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
        /// 设置站别对应流程列表
        /// </summary>
        /// <param name="flowList"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool SetStatToFlowList(List<CSTAT_FLOW> statFlowList, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                string requestName = "SetStatToFlowList";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表

                DataSet in_ds = new DataSet(C_XML_HEADER);

                //参数表单
                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("StatId");
                in_ds.Tables[C_XML_PARA].Columns.Add("StatName");
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowId");
                in_ds.Tables[C_XML_PARA].Columns.Add("FlowName");
                in_ds.Tables[C_XML_PARA].Columns.Add("MapDisable");

                for (int i = 0; i < statFlowList.Count; i++)
                    in_ds.Tables[C_XML_PARA].Rows.Add(statFlowList[i].StatId, statFlowList[i].StatName,
                                                      statFlowList[i].FlowId,statFlowList[i].FlowName,
                                                      statFlowList[i].Disable);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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

        #region 一般方法
        /// <summary>
        /// 绑定治具条码信息
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool BandSnToFixture(CFIX_BAND fixture, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "BandSnToFixture";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("MaxSlot");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("Model");
                in_ds.Tables[C_XML_BASE].Columns.Add("OrderName");
                in_ds.Tables[C_XML_BASE].Columns.Add("MesFlag");
                in_ds.Tables[C_XML_BASE].Columns.Add("UseStatus");

                in_ds.Tables[C_XML_BASE].Rows.Add(fixture.Base.IdCard, fixture.Base.MaxSlot, fixture.Base.LineNo,
                                                  fixture.Base.LineName, fixture.Base.Model, fixture.Base.OrderName,
                                                  fixture.Base.MesFlag,(int)fixture.Base.UseStatus);

                //参数表单
                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                for (int i = 0; i < fixture.Para.Count; i++)
                {
                    in_ds.Tables[C_XML_PARA].Rows.Add(i, fixture.Para[i].SerialNo);
                }

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 绑定治具条码信息
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool BandFlowAndSnToFixture(CFIX_BAND fixture, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "BandFlowAndSnToFixture";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("MaxSlot");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("Model");
                in_ds.Tables[C_XML_BASE].Columns.Add("OrderName");
                in_ds.Tables[C_XML_BASE].Columns.Add("MesFlag");
                in_ds.Tables[C_XML_BASE].Columns.Add("UseStatus");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurFlowName");

                in_ds.Tables[C_XML_BASE].Rows.Add(fixture.Base.IdCard, fixture.Base.MaxSlot, fixture.Base.LineNo,
                                                  fixture.Base.LineName, fixture.Base.Model, fixture.Base.OrderName,
                                                  fixture.Base.MesFlag, (int)fixture.Base.UseStatus, fixture.Base.CurFlowName);

                //参数表单
                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                for (int i = 0; i < fixture.Para.Count; i++)
                {
                    in_ds.Tables[C_XML_PARA].Rows.Add(i, fixture.Para[i].SerialNo);
                }

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 解除治具绑定功能
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="deleteId">0:解除 1：删除</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool ReleaseSnFromFixture(string idCard, int deleteId,out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "ReleaseSnFromFixture";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("DeleteId");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard, deleteId);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 重组治具条码信息
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool RegroupSnToFixture(CFIX_BAND fixture, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "RegroupSnToFixture";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("MaxSlot");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("Model");
                in_ds.Tables[C_XML_BASE].Columns.Add("OrderName");
                in_ds.Tables[C_XML_BASE].Columns.Add("MesFlag");

                in_ds.Tables[C_XML_BASE].Rows.Add(fixture.Base.IdCard, fixture.Base.MaxSlot, fixture.Base.LineNo,
                                                  fixture.Base.LineName, fixture.Base.Model, fixture.Base.OrderName,
                                                  fixture.Base.MesFlag);

                //参数表单
                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("CurFlowName");
                in_ds.Tables[C_XML_PARA].Columns.Add("CurStatName");
                in_ds.Tables[C_XML_PARA].Columns.Add("CurResult");
                for (int i = 0; i < fixture.Para.Count; i++)
                {
                    in_ds.Tables[C_XML_PARA].Rows.Add(i, fixture.Para[i].SerialNo, fixture.Para[i].CurFlowName,
                                                         fixture.Para[i].CurStatName, fixture.Para[i].CurResult);
                }

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 获取治具状态信息
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetInfoFromFixture(string idCard, out CFIX_BAND fixture, out string er)
        {
            er = string.Empty;

            fixture = new CFIX_BAND();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetInfoFromFixture";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_BASE].Rows.Count; i++)
                    {
                        fixture.Base.IdCard = out_ds.Tables[C_XML_BASE].Rows[i]["IdCard"].ToString();
                        fixture.Base.LineNo = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["LineNo"].ToString());
                        fixture.Base.LineName = out_ds.Tables[C_XML_BASE].Rows[i]["LineName"].ToString();
                        fixture.Base.MaxSlot = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["MaxSlot"].ToString());
                        fixture.Base.Model = out_ds.Tables[C_XML_BASE].Rows[i]["Model"].ToString();
                        fixture.Base.OrderName = out_ds.Tables[C_XML_BASE].Rows[i]["OrderName"].ToString();
                        fixture.Base.MesFlag = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["MesFlag"].ToString());
                        fixture.Base.UseStatus = (EFIX_STATUS)System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["UseStatus"].ToString());
                    }

                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        int idNo = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["SlotNo"].ToString());

                        CSN_INFO Sn=new CSN_INFO();

                        Sn.SerialNo = out_ds.Tables[C_XML_PARA].Rows[i]["SerialNo"].ToString();

                        Sn.CurFlowId = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["CurFlowId"].ToString());

                        Sn.CurFlowName = out_ds.Tables[C_XML_PARA].Rows[i]["CurFlowName"].ToString();

                        Sn.CurStatName = out_ds.Tables[C_XML_PARA].Rows[i]["CurStatName"].ToString();

                        Sn.CurResult = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["CurResult"].ToString());

                        fixture.Para.Add(Sn); 
                    }
                }

                er = DataXML;

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
        /// 由条码获取治具状态信息
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetInfoFromSn(string serialNo, out CFIX_BAND fixture, out string er)
        {
            er = string.Empty;

            fixture = new CFIX_BAND();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetInfoFromSn";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_BASE].Rows.Add(serialNo);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_BASE].Rows.Count; i++)
                    {
                        fixture.Base.IdCard = out_ds.Tables[C_XML_BASE].Rows[i]["IdCard"].ToString();
                        fixture.Base.LineNo = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["LineNo"].ToString());
                        fixture.Base.LineName = out_ds.Tables[C_XML_BASE].Rows[i]["LineName"].ToString();
                        fixture.Base.MaxSlot = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["MaxSlot"].ToString());
                        fixture.Base.Model = out_ds.Tables[C_XML_BASE].Rows[i]["Model"].ToString();
                        fixture.Base.OrderName = out_ds.Tables[C_XML_BASE].Rows[i]["OrderName"].ToString();
                        fixture.Base.MesFlag = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["MesFlag"].ToString());
                        fixture.Base.UseStatus = (EFIX_STATUS)System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["UseStatus"].ToString());
                    }

                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        int idNo = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["SlotNo"].ToString());

                        CSN_INFO Sn = new CSN_INFO();

                        Sn.SerialNo = out_ds.Tables[C_XML_PARA].Rows[i]["SerialNo"].ToString();

                        Sn.CurFlowId = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["CurFlowId"].ToString());

                        Sn.CurFlowName = out_ds.Tables[C_XML_PARA].Rows[i]["CurFlowName"].ToString();

                        Sn.CurStatName = out_ds.Tables[C_XML_PARA].Rows[i]["CurStatName"].ToString();

                        Sn.CurResult = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["CurResult"].ToString());

                        fixture.Para.Add(Sn);
                    }
                }

                er = DataXML;

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
        /// 获取治具当前流程条码信息
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetFlowSnFromFixture(string idCard, string curFlowName, out CFIX_BAND fixture, out string er)
        {
            er = string.Empty;

            fixture = new CFIX_BAND();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetFlowSnFromFixture";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurFlowName");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard, curFlowName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_BASE].Rows.Count; i++)
                    {
                        fixture.Base.IdCard = out_ds.Tables[C_XML_BASE].Rows[i]["IdCard"].ToString();
                        fixture.Base.LineNo = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["LineNo"].ToString());
                        fixture.Base.LineName = out_ds.Tables[C_XML_BASE].Rows[i]["LineName"].ToString();
                        fixture.Base.MaxSlot = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["MaxSlot"].ToString());
                        fixture.Base.Model = out_ds.Tables[C_XML_BASE].Rows[i]["Model"].ToString();
                        fixture.Base.OrderName = out_ds.Tables[C_XML_BASE].Rows[i]["OrderName"].ToString();
                        fixture.Base.MesFlag = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["MesFlag"].ToString());
                        fixture.Base.UseStatus = (EFIX_STATUS)System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["UseStatus"].ToString());
                    }

                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        int idNo = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["SlotNo"].ToString());

                        CSN_INFO Sn = new CSN_INFO();

                        Sn.SerialNo = out_ds.Tables[C_XML_PARA].Rows[i]["SerialNo"].ToString();

                        Sn.CurFlowName = out_ds.Tables[C_XML_PARA].Rows[i]["CurFlowName"].ToString();

                        fixture.Para.Add(Sn); 
                    }
                }

                er = DataXML;

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
        /// 上传治具测试结果
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool UpdateFixtureResult(CFIX_RESULT fixture, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "UpdateFixtureResult";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurFlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurStatName");
                in_ds.Tables[C_XML_BASE].Columns.Add("StartTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("EndTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurLocalName");
                in_ds.Tables[C_XML_BASE].Columns.Add("CheckSn");

                in_ds.Tables[C_XML_BASE].Rows.Add(fixture.Base.IdCard, fixture.Base.CurFlowName, fixture.Base.CurStatName,
                                                  fixture.Base.StartTime, fixture.Base.EndTime, fixture.Base.LocalName,
                                                  fixture.Base.CheckSn);

                //参数表单
                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("CurResult");
                in_ds.Tables[C_XML_PARA].Columns.Add("CurTestData");
                for (int i = 0; i < fixture.Para.Count; i++)
                {
                    in_ds.Tables[C_XML_PARA].Rows.Add(i, fixture.Para[i].SerialNo,
                                                         fixture.Para[i].CurResult,
                                                         fixture.Para[i].CurTestData);
                }

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 清除治具上不良条码
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool ClrFailSnFromFixture(string idCard, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "ClrFailSnFromFixture";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard);
              
                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 获取站别产能
        /// </summary>
        /// <param name="statName">为空返回所有站别产能</param>
        /// <param name="yields"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetStatYield(string statName, out List<CSTAT_YIELD> yields, out string er)
        {
            er = string.Empty;

            yields =new List<CSTAT_YIELD>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetStatYield";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Rows.Add(statName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CSTAT_YIELD yield = new CSTAT_YIELD();

                        yield.StatName = out_ds.Tables[C_XML_PARA].Rows[i]["StatName"].ToString();

                        yield.FlowId = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["FlowId"].ToString());

                        yield.FlowName = out_ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();

                        yield.TTNum = System.Convert.ToInt32(out_ds.Tables[C_XML_PARA].Rows[i]["TTNum"].ToString());

                        yield.FailNum = System.Convert.ToInt32(out_ds.Tables[C_XML_PARA].Rows[i]["FailNum"].ToString());

                        yields.Add(yield); 
                    }
                }

                er = DataXML;

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
        /// 清除测试工位产能统计
        /// </summary>
        /// <param name="statName">为空清除所有站别产能</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool ClrStatYield(string statName, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "ClrStatYield";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Rows.Add(statName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 获取治具在工位的测试次数
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="statName">为空返回所有站别</param>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetIdCardYield(string idCard, string statName, out CFIX_YIELD fixture, out string er)
        {
            er = string.Empty;

            fixture = new CFIX_YIELD();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetIdCardYield";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard,statName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_BASE].Rows.Count; i++)
                    {
                        fixture.IdCard = out_ds.Tables[C_XML_BASE].Rows[i]["IdCard"].ToString();

                        fixture.TTNum = System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["TTNum"].ToString());

                        fixture.FailNum = System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["FailNum"].ToString());

                        fixture.ConFailNum = System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["ConFailNum"].ToString());
                    }

                    if (out_ds.Tables.Contains(C_XML_PARA))
                    {
                        for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                        {
                            CFIX_STAT_YIELD yield = new CFIX_STAT_YIELD();

                            yield.StatName = out_ds.Tables[C_XML_PARA].Rows[i]["StatName"].ToString();

                            yield.FlowId = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["FlowId"].ToString());

                            yield.FlowName = out_ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();

                            yield.SlotNo = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["SlotNo"].ToString());

                            yield.TTNum = System.Convert.ToInt32(out_ds.Tables[C_XML_PARA].Rows[i]["TTNum"].ToString());

                            yield.FailNum = System.Convert.ToInt32(out_ds.Tables[C_XML_PARA].Rows[i]["FailNum"].ToString());

                            yield.ConFailNum = System.Convert.ToInt32(out_ds.Tables[C_XML_PARA].Rows[i]["ConFailNum"].ToString());

                            fixture.Stat.Add(yield); 

                        }
                    }
                }

                er = DataXML;

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
        /// 获取治具使用次数
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="statName">为空返回所有站别</param>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetIdCardInfo(string idCard, out List<CFIX_USE> fixture, out string er)
        {
            er = string.Empty;

            fixture = new List<CFIX_USE>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetIdCardInfo";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_BASE].Rows.Count; i++)
                    {
                        CFIX_USE fix = new CFIX_USE();

                        fix.IdCard = out_ds.Tables[C_XML_BASE].Rows[i]["IdCard"].ToString();

                        fix.UseStatus = (EFIX_STATUS)System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["UseStatus"].ToString());

                        fix.TTNum = System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["TTNum"].ToString());

                        fix.FailNum = System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["FailNum"].ToString());

                        fix.ConFailNum = System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["ConFailNum"].ToString());

                        fixture.Add(fix); 
                    }
                }

                er = DataXML;

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
        /// 清除治具在工位的测试次数
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool ClrIdCardYield(string idCard, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "ClrIdCardYield";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 设置治具使用状态
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="status"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool SetFixtureUseStaus(string idCard, EFIX_STATUS status, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "SetFixtureUseStaus";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("UseStatus");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard, (int)status);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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

        #region 扩展功能
        /// <summary>
        /// 获取条码测试信息
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="rData"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetSnTestData(string serialNo, out List<CSN_TESTDATA> rData, out string er)
        {
            er = string.Empty;

            rData=new List<CSN_TESTDATA>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetSnTestData";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_BASE].Rows.Add(serialNo);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {                   
                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CSN_TESTDATA Sn = new CSN_TESTDATA();

                        Sn.SerialNo = out_ds.Tables[C_XML_PARA].Rows[i]["SerialNo"].ToString();

                        Sn.CurFlowId = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["CurFlowId"].ToString());

                        Sn.CurFlowName = out_ds.Tables[C_XML_PARA].Rows[i]["CurFlowName"].ToString();

                        Sn.CurStatName = out_ds.Tables[C_XML_PARA].Rows[i]["CurStatName"].ToString();

                        Sn.CurResult = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["CurResult"].ToString());

                        Sn.CurTestData = out_ds.Tables[C_XML_PARA].Rows[i]["CurTestData"].ToString();

                        Sn.StartTime = out_ds.Tables[C_XML_PARA].Rows[i]["StartTime"].ToString();

                        Sn.EndTime = out_ds.Tables[C_XML_PARA].Rows[i]["EndTime"].ToString();

                        Sn.LineNo = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["LineNo"].ToString());

                        Sn.LineName = out_ds.Tables[C_XML_PARA].Rows[i]["LineName"].ToString();

                        Sn.Model = out_ds.Tables[C_XML_PARA].Rows[i]["Model"].ToString();

                        Sn.OrderName = out_ds.Tables[C_XML_PARA].Rows[i]["OrderName"].ToString();

                        Sn.Fixture = out_ds.Tables[C_XML_PARA].Rows[i]["Fixture"].ToString();

                        Sn.LocalName = out_ds.Tables[C_XML_PARA].Rows[i]["LocalName"].ToString();

                        rData.Add(Sn); 
                    }
                }

                er = DataXML;

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
        /// 检查条码过站
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="flowName"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool CheckSn(string serialNo, string flowName, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "CheckSn";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Rows.Add(serialNo,flowName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 上传条码测试结果
        /// </summary>
        /// <param name="Sn"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool TranSnResult(CSN_TESTDATA Sn, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "TranSnResult";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurFlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurStatName");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurResult");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurTestData");
                in_ds.Tables[C_XML_BASE].Columns.Add("StartTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("EndTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineNo");
                in_ds.Tables[C_XML_BASE].Columns.Add("LineName");
                in_ds.Tables[C_XML_BASE].Columns.Add("Model");
                in_ds.Tables[C_XML_BASE].Columns.Add("OrderName");
                in_ds.Tables[C_XML_BASE].Columns.Add("Fixture");
                in_ds.Tables[C_XML_BASE].Columns.Add("LocalName");
                in_ds.Tables[C_XML_BASE].Rows.Add(Sn.SerialNo,Sn.CurFlowName,Sn.CurStatName,Sn.CurResult,
                                                  Sn.CurTestData,Sn.StartTime,Sn.EndTime,Sn.LineNo,
                                                  Sn.LineName,Sn.Model,Sn.OrderName,Sn.Fixture,
                                                  Sn.LocalName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 设置站别运行状态
        /// </summary>
        /// <param name="Stat"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool SetStatRunStatus(CSTAT_RUN Stat, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "SetStatRunStatus";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Columns.Add("RunStatus");
                in_ds.Tables[C_XML_BASE].Columns.Add("StatusTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("ErrCode");
                in_ds.Tables[C_XML_BASE].Rows.Add(Stat.StatName, (int)Stat.RunStatus,
                                                  Stat.StatusTime,Stat.ErrCode);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 获取站别运行状态
        /// </summary>
        /// <param name="Stat"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetStatRunStatus(string statName, out CSTAT_RUN Stat, out string er)
        {
            er = string.Empty;

            Stat = new CSTAT_RUN();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetStatRunStatus";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Rows.Add(statName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        Stat.StatName = out_ds.Tables[C_XML_PARA].Rows[i]["StatName"].ToString();

                        Stat.RunStatus = (ESTAT_RUN)System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["RunStatus"].ToString());

                        Stat.StatusTime = out_ds.Tables[C_XML_PARA].Rows[i]["StatusTime"].ToString();

                        Stat.ErrCode = out_ds.Tables[C_XML_PARA].Rows[i]["ErrCode"].ToString();
                    }
                }

                er = DataXML;

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
        /// 设置治具ID运行状态
        /// </summary>
        /// <param name="Stat"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool SetIdCardStatus(CFIX_RUN fixture, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "SetIdCardStatus";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("RunStatus");
                in_ds.Tables[C_XML_BASE].Rows.Add(fixture.IdCard,fixture.StatName,
                                                  fixture.FlowName,(int)fixture.RunStatus);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 获取治具ID运行状态
        /// </summary>
        /// <param name="Stat"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetIdCardInStat(string idCard, out CFIX_RUN fixture, out string er)
        {
            er = string.Empty;

            fixture = new CFIX_RUN();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetIdCardInStat";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        fixture.IdCard = idCard;

                        fixture.StatName = out_ds.Tables[C_XML_PARA].Rows[i]["StatName"].ToString();

                        fixture.FlowId = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["FlowId"].ToString());

                        fixture.FlowName = out_ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();

                        fixture.RunStatus = (EFIX_RUN)System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["RunStatus"].ToString());

                        fixture.StatusTime = out_ds.Tables[C_XML_PARA].Rows[i]["StatusTime"].ToString();
                    }
                }

                er = DataXML;

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
        /// 获取当前工位治具列表
        /// </summary>
        /// <param name="statName"></param>
        /// <param name="fixtures"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetStatIdCardList(string statName, out List<CFIX_RUN> fixtures, out string er)
        {
            er = string.Empty;

            fixtures = new List<CFIX_RUN>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetStatIdCardList";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Rows.Add(statName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                    {
                        CFIX_RUN fixture = new CFIX_RUN();

                        fixture.IdCard = out_ds.Tables[C_XML_PARA].Rows[i]["IdCard"].ToString();

                        fixture.StatName = out_ds.Tables[C_XML_PARA].Rows[i]["StatName"].ToString();

                        fixture.FlowId = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["FlowId"].ToString());

                        fixture.FlowName = out_ds.Tables[C_XML_PARA].Rows[i]["FlowName"].ToString();

                        fixture.RunStatus = (EFIX_RUN)System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["RunStatus"].ToString());

                        fixture.StatusTime = out_ds.Tables[C_XML_PARA].Rows[i]["StatusTime"].ToString();

                        fixtures.Add(fixture); 
                    }
                }

                er = DataXML;

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
        /// 删除治具条码信息和过期数据
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="dayLimit">天数</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool DeleteIdCardInfo(string idCard, int dayLimit, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "DeleteIdCardInfo";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("DayLimit");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard, dayLimit);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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

        #region 点检治具
        /// <summary>
        /// 设置点检治具
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool SetFixtureToSample(CFIX_SAMPLE fixture, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "SetFixtureToSample";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("MaxSlot");
                in_ds.Tables[C_XML_BASE].Columns.Add("CurFlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("SetEnable");
                in_ds.Tables[C_XML_BASE].Rows.Add(fixture.IdCard,fixture.SerialNo.Count,fixture.CurFlowName,fixture.SetEnable);

                //参数表单
                in_ds.Tables.Add(C_XML_PARA);
                in_ds.Tables[C_XML_PARA].Columns.Add("SlotNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("SerialNo");
                in_ds.Tables[C_XML_PARA].Columns.Add("SetResult");
                for (int i = 0; i < fixture.SerialNo.Count; i++)
                {
                    in_ds.Tables[C_XML_PARA].Rows.Add(i, fixture.SerialNo[i],fixture.Result[i]);
                }

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 设置点检治具
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool DeleteFixtureToSample(string idCard, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "DeleteFixtureToSample";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 获取点检治具列表
        /// </summary>
        /// <param name="fixtures"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool QuerySampleList(out List<CFIX_SAMPLE> fixtures, out string er)
        {
            er = string.Empty;

            fixtures = new List<CFIX_SAMPLE>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "QueryFixtureIsSample";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add("");

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_BASE].Rows.Count; i++)
                    {
                        CFIX_SAMPLE fix = new CFIX_SAMPLE();

                        fix.IdCard = out_ds.Tables[C_XML_BASE].Rows[i]["IdCard"].ToString();

                        fix.CurFlowId = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["CurFlowId"].ToString());

                        fix.CurFlowName = out_ds.Tables[C_XML_BASE].Rows[i]["CurFlowName"].ToString();

                        fix.SetEnable = System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["SetEnable"].ToString());

                        fixtures.Add(fix); 
                    }
                }

                er = DataXML;

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
        /// 获取点检治具列表
        /// </summary>
        /// <param name="fixtures"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool QuerySampleFixture(string idCard, out CFIX_SAMPLE fixture, out string er)
        {
            er = string.Empty;

            fixture = new CFIX_SAMPLE();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "QueryFixtureIsSample";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    if (out_ds.Tables[C_XML_BASE] != null)
                    {
                        for (int i = 0; i < out_ds.Tables[C_XML_BASE].Rows.Count; i++)
                        {
                            fixture.IdCard = out_ds.Tables[C_XML_BASE].Rows[i]["IdCard"].ToString();

                            fixture.CurFlowId = System.Convert.ToInt16(out_ds.Tables[C_XML_BASE].Rows[i]["CurFlowId"].ToString());

                            fixture.CurFlowName = out_ds.Tables[C_XML_BASE].Rows[i]["CurFlowName"].ToString();

                            fixture.SetEnable = System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["SetEnable"].ToString());

                            fixture.IsSample = true;
                        }
                    }

                    if (out_ds.Tables[C_XML_PARA] != null)
                    {
                        int SlotMax = out_ds.Tables[C_XML_PARA].Rows.Count;

                        for (int i = 0; i < SlotMax; i++)
                        {
                            fixture.SerialNo.Add("");
                            fixture.Result.Add(0);  
                        }

                        for (int i = 0; i < out_ds.Tables[C_XML_PARA].Rows.Count; i++)
                        {
                            int slotNo = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["SlotNo"].ToString());

                            if (slotNo < SlotMax)
                            {
                                fixture.SerialNo[slotNo] = out_ds.Tables[C_XML_PARA].Rows[i]["SerialNo"].ToString();

                                fixture.Result[slotNo] = System.Convert.ToInt16(out_ds.Tables[C_XML_PARA].Rows[i]["SetResult"].ToString());
                            }
                        }
                    }
                }

                er = DataXML;

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

        #region 工位CheckList管控
        /// <summary>
        /// 增加PIN使用次数,自动加1
        /// </summary>
        /// <param name="FlowId"></param>
        /// <param name="StatName"></param>
        /// <param name="PosName"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool AddPinNum(int FlowId, string StatName, string PosName, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "AddPinNum";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowId");
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Columns.Add("PosName");
                in_ds.Tables[C_XML_BASE].Rows.Add(FlowId, StatName, PosName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 设置PIN管控信息--UseNum=-1,则不更新使用次数
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool SetNewPinInfo(CPIN pin, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "SetNewPinInfo";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowId");
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Columns.Add("PosName");
                in_ds.Tables[C_XML_BASE].Columns.Add("UseNum");
                in_ds.Tables[C_XML_BASE].Columns.Add("AlarmTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("LimitTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("CheckDate");
                in_ds.Tables[C_XML_BASE].Columns.Add("CheckName");

                in_ds.Tables[C_XML_BASE].Rows.Add(pin.FlowId, pin.StatName, pin.PosName,
                                                  pin.UseNum,pin.AlarmTime,pin.LimitTime,
                                                  pin.CheckDate,pin.CheckName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 获取工位PIN管控信息 statName为空获取所有工位
        /// </summary>
        /// <param name="statName"></param>
        /// <param name="posName"></param>
        /// <param name="pin"></param>
        /// <returns></returns>
        public static bool GetPinInfo(string statName,string posName,out List<CPIN> pin, out string er)
        {
            er = string.Empty;

            pin = new List<CPIN>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetPinInfo";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("StatName");
                in_ds.Tables[C_XML_BASE].Columns.Add("PosName");
                in_ds.Tables[C_XML_BASE].Rows.Add(statName, posName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null && out_ds.Tables.Contains(C_XML_BASE))
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_BASE].Rows.Count; i++)
                    {
                        CPIN p = new CPIN();

                        p.FlowId = System.Convert.ToInt16(out_ds.Tables[0].Rows[i]["FlowId"].ToString());

                        p.StatName = out_ds.Tables[0].Rows[i]["StatName"].ToString();

                        p.PosName = out_ds.Tables[0].Rows[i]["PosName"].ToString();

                        p.UseNum = System.Convert.ToInt32(out_ds.Tables[0].Rows[i]["UseNum"].ToString());

                        p.AlarmTime = System.Convert.ToInt32(out_ds.Tables[0].Rows[i]["AlarmTime"].ToString());

                        p.LimitTime = System.Convert.ToInt32(out_ds.Tables[0].Rows[i]["LimitTime"].ToString());

                        p.CheckDate = out_ds.Tables[0].Rows[i]["CheckDate"].ToString();

                        p.CheckTime = System.Convert.ToInt32(out_ds.Tables[0].Rows[i]["CheckTime"].ToString());

                        p.CheckName = out_ds.Tables[0].Rows[i]["CheckName"].ToString();

                        pin.Add(p); 
                    }
                }

                er = DataXML;

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
        /// 设置治具CheckList
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool SetIdCardCheckListInfo(CRFIDList idList, out string er)
        {
            er = string.Empty;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "SetIdCardCheckListInfo";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Columns.Add("AlarmTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("LimitTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("CheckDate");
                in_ds.Tables[C_XML_BASE].Columns.Add("CheckContext");
                in_ds.Tables[C_XML_BASE].Columns.Add("CheckTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("CheckName");

                in_ds.Tables[C_XML_BASE].Rows.Add(idList.IdCard,idList.AlarmTime,idList.LimitTime,
                                                  idList.CheckDate,idList.CheckContext,idList.CheckTime,
                                                  idList.CheckName);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

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
        /// 获取工位PIN管控信息 statName为空获取所有工位
        /// </summary>
        /// <param name="statName"></param>
        /// <param name="posName"></param>
        /// <param name="pin"></param>
        /// <returns></returns>
        public static bool GetIdCardCheckListInfo(string idCard, out List<CRFIDList> idList, out string er)
        {
            er = string.Empty;

            idList = new List<CRFIDList>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetIdCardCheckListInfo";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("IdCard");
                in_ds.Tables[C_XML_BASE].Rows.Add(idCard);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null && out_ds.Tables.Contains(C_XML_BASE))
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_BASE].Rows.Count; i++)
                    {
                        CRFIDList rfid = new CRFIDList();

                        rfid.IdCard = out_ds.Tables[0].Rows[i]["IdCard"].ToString();

                        rfid.AlarmTime = System.Convert.ToInt32(out_ds.Tables[0].Rows[i]["AlarmTime"].ToString());

                        rfid.LimitTime = System.Convert.ToInt32(out_ds.Tables[0].Rows[i]["LimitTime"].ToString());

                        rfid.CheckDate = out_ds.Tables[0].Rows[i]["CheckDate"].ToString();

                        rfid.CheckContext = out_ds.Tables[0].Rows[i]["CheckContext"].ToString();

                        rfid.CheckTime = System.Convert.ToInt32(out_ds.Tables[0].Rows[i]["CheckTime"].ToString());

                        rfid.CheckName = out_ds.Tables[0].Rows[i]["CheckName"].ToString();

                        idList.Add(rfid);
                    }
                }

                er = DataXML;

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

        #region 对外功能
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="CallBack"></param>
        /// <param name="dt"></param>
        /// <param name="er"></param>
        public bool ExcuteSQLCmd(string Command, int CallBack, out DataSet ds, out string er)
        {
            er = string.Empty;

            ds = null;

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "ExcuteSQLCmd";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("Command");
                in_ds.Tables[C_XML_BASE].Columns.Add("CallBack");
                in_ds.Tables[C_XML_BASE].Rows.Add(Command, CallBack);

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                er = DataXML;

                if (CallBack == 1)
                {
                    ds = ConvertXMLToDataSet(DataXML);
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
                webLock.ReleaseWriterLock();
            }        
        }
        #endregion

        #region 工位产能查询
                /// <summary>
        /// 获取治具状态信息
        /// </summary>
        /// <param name="idCard"></param>
        /// <param name="fixture"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool GetYieldInStationAndTime(List<CYieldRequest> request, out List<CYieldReponse> reponse, out string er)
        {
            er = string.Empty;

            reponse = new List<CYieldReponse>();

            try
            {
                webLock.AcquireWriterLock(-1);

                er = string.Empty;

                string requestName = "GetYieldInStationAndTime";

                string requestXml = string.Empty;

                string strXML = string.Empty;

                string reponseXml = string.Empty;

                string DataXML = string.Empty;

                //输入表单列表
                DataSet in_ds = new DataSet(C_XML_HEADER);

                //基本表单
                in_ds.Tables.Add(C_XML_BASE);
                in_ds.Tables[C_XML_BASE].Columns.Add("FlowName");
                in_ds.Tables[C_XML_BASE].Columns.Add("StartTime");
                in_ds.Tables[C_XML_BASE].Columns.Add("EndTime");
                for (int i = 0; i < request.Count; i++)
                {
                    in_ds.Tables[C_XML_BASE].Rows.Add(request[i].FlowName,request[i].StartTime,request[i].EndTime);
                }                

                requestXml = ConvertDataTableToXML(in_ds);

                //格式化XML
                strXML = getRequestXml(requestName, requestXml);

                if (!PostMessageToHttp(strXML, ref reponseXml, out er))
                    return false;

                if (!getReponseXml(reponseXml, ref DataXML, out er))
                    return false;

                DataSet out_ds = ConvertXMLToDataSet(DataXML);

                if (out_ds != null)
                {
                    for (int i = 0; i < out_ds.Tables[C_XML_BASE].Rows.Count; i++)
                    {
                        CYieldReponse rep = new CYieldReponse()
                        {
                            FlowName = out_ds.Tables[C_XML_BASE].Rows[i]["FlowName"].ToString(),
                            TTNum = System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["TTNum"].ToString()),
                            FailNum = System.Convert.ToInt32(out_ds.Tables[C_XML_BASE].Rows[i]["FailNum"].ToString())
                        };
                        reponse.Add(rep);
                    }                 
                }

                er = DataXML;

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
        /// 初始化Http请求Xml
        /// </summary>
        /// <param name="requestName"></param>
        /// <param name="requestXml"></param>
        /// <returns></returns>
        private static string getRequestXml(string requestName, string requestXml)
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
        private static bool getReponseXml(string reponseXml, ref string xmlData, out  string er)
        {
            try
            {
                er = string.Empty;

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

        #region Http方法
        /// <summary>
        /// 向HTTP发消息和接收消息
        /// </summary>
        /// <param name="requestXml"></param>
        /// <param name="reponseXml"></param>
        /// <param name="er"></param>
        /// <returns></returns>
        private static bool PostMessageToHttp(string requestXml, ref string reponseXml, out string er)
        {
            try
            {

                er = string.Empty;

                //发送数据转换为Bytes
                byte[] sendByte = System.Text.Encoding.Default.GetBytes(requestXml);

                //发送HTTP的POST请求
                HttpWebRequest request = (HttpWebRequest)(HttpWebRequest.Create(_ulrWeb));
                //Post请求方式
                request.Method = "POST";
                //内容类型
                request.ContentType = "text/xml; charset=GB2312";
                //设置请求的 ContentLength
                request.ContentLength = sendByte.Length;
                //获得请求流
                Stream fs = request.GetRequestStream();
                fs.Write(sendByte, 0, sendByte.Length);
                fs.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                XmlTextReader xmlReader = new XmlTextReader(s);
                xmlReader.MoveToContent();
                string recv = xmlReader.ReadInnerXml();
                //recv = recv.Replace("&amp;", "&");
                //recv = recv.Replace("&lt;", "<");
                //recv = recv.Replace("&gt;", ">");
                //recv = recv.Replace("&apos;", "'");
                //recv = recv.Replace("&quot;", "\""); //((char)34) 双引号
                reponseXml = recv;
                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return false;
            }
        }
        /// <summary>
        /// xml->DataSet
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        private static DataSet GetDataSetByXml(string xmlData)
        {
            try
            {
                DataSet ds = new DataSet();
                using (StringReader xmlSR = new StringReader(xmlData))
                {
                    ds.ReadXml(xmlSR, XmlReadMode.InferTypedSchema);
                    //忽视任何内联架构，从数据推断出强类型架构并加载数据,如果无法推断，则解释成字符串数据
                    if (ds.Tables.Count > 0)
                        return ds;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 表单集转XML
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <returns></returns>
        private static string ConvertDataTableToXML(DataSet xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                stream = new MemoryStream();
                writer = new XmlTextWriter(stream,Encoding.Default);
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);
                return System.Text.Encoding.Default.GetString(arr);
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }
        /// <summary>
        /// 表单转XML
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <returns></returns>
        private static string ConvertDataTableToXML(DataTable xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                stream = new MemoryStream();
                writer = new XmlTextWriter(stream, Encoding.Default);
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);
                return System.Text.Encoding.Default.GetString(arr);
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }
        /// <summary>
        /// XML转表单
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        private static DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch (Exception ex)
            {
                string strTest = ex.Message;
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        #endregion
    }
}
