using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GJ.Iot
{

    #region 枚举
    /// <summary>
    /// 节点定义
    /// </summary>
    public enum ENodeType
    {
        客户端,
        主控端
    }
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum EMessageType
    {
        状态消息,     
        报警消息,
        抽样消息,
        广播指令,
        应答指令
    }
    /// <summary>
    /// 设备类型
    /// </summary>
    public enum EDeviceType
    {
        测试设备,
        组装设备,
        中转设备,
        传感器
    }
    /// <summary>
    /// 报警等级
    /// </summary>
    public enum EAlarmLevel
    {
        解除,
        告警,
        报警,
        停线
    }
    /// <summary>
    /// 指令代号
    /// </summary>
    public enum ECmdType
    { 
       /// <summary>
       /// CORE->NODE
       /// </summary>
       上报状态,
       /// <summary>
       /// CORE->NODE
       /// </summary>
       控制指令,
       /// <summary>
       /// NODE->CORE
       /// </summary>
       请求指示,
       /// <summary>
       /// NODE->CORE
       /// </summary>
       应答指令
    }
    /// <summary>
    /// 设备状态
    /// </summary>
    public enum EDevRunStatus
    {
      未运行,
      运行,
      停止,
      报警,
      停线
    }
    #endregion

    /// <summary>
    /// 设备列表
    /// </summary>
    public class CDevList
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string idNo = string.Empty;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name = string.Empty;
    }

    /// <summary>
    /// 消息头
    /// </summary>
    [DataContract]
    public class CHeader
    {
        /// <summary>
        ///工控机编号 -- 全球唯一
        /// </summary>
        [DataMember]
        public string ID { get; set; }
        /// <summary>
        /// 工控机名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 工控机类型 0:客户端 1：主控端
        /// </summary>
        [DataMember]
        public int Type { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        public string Time { get; set; }
    }
    /// <summary>
    /// 消息类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    public class CMessage<T> where T:class
    {
        /// <summary>
        /// 消息头
        /// </summary>
        [DataMember]
        public CHeader Header { get; set; }
        /// <summary>
        /// 消息信息
        /// </summary>
        [DataMember]
        public List<T> Data { get; set; }
    }
    /// <summary>
    /// 状态消息
    /// </summary>
    [DataContract]
    public class CData_Status
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [DataMember]
        public string ID { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 设备类型 0：测试设备 1:组装设备 2：中转设备 3:传感器
        /// </summary>
        [DataMember]
        public int Type { get; set; }
        /// <summary>
        /// 运行状态 0:未运行 1：运行 2：停止 3：报警
        /// </summary>
        [DataMember]
        public int RunStatus { get; set; }
        /// <summary>
        /// 投入总数
        /// </summary>
        [DataMember]
        public int TTNum { get; set; }
        /// <summary>
        /// 投入不良
        /// </summary>
        [DataMember]
        public int FailNum { get; set; }
        /// <summary>
        /// 报警等级 0：解除1：告警 2：报警 3：停线
        /// </summary>
        [DataMember]
        public int AlarmLevel { get; set; }
        /// <summary>
        /// 报警代号
        /// </summary>
        [DataMember]
        public string AlarmCode { get; set; }
        /// <summary>
        /// 报警信息
        /// </summary>
        [DataMember]
        public string AlarmInfo { get; set; }
        /// <summary>
        /// 备注1
        /// </summary>
        [DataMember]
        public string Remark1 { get; set; }
        /// <summary>
        /// 备注2
        /// </summary>
        [DataMember]
        public string Remark2 { get; set; }
    }
    /// <summary>
    /// 命令消息
    /// </summary>
    [DataContract]
    public class CData_Cmd
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        [DataMember]
        public string ID { get; set; }
        /// <summary>
        /// 设备名称
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 命令类型
        /// </summary>
        [DataMember]
        public int CmdType { get; set; }
        /// <summary>
        /// 指令名称
        /// </summary>
        [DataMember]
        public string CmdName { get; set; }
        /// <summary>
        /// 指令信息
        /// </summary>
        [DataMember]
        public string CmdInfo { get; set; }
        /// <summary>
        /// 备注1
        /// </summary>
        [DataMember]
        public string Remark1 { get; set; }
        /// <summary>
        /// 备注2
        /// </summary>
        [DataMember]
        public string Remark2 { get; set; }
    }
}
