using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Windows.Forms;

namespace GJ.COM
{
    /// <summary>
    /// XML文档操作类
    /// </summary>
    public class CXml
    {
        #region XML通用方法
        /// <summary>
        /// 节点类型
        /// </summary>
        public enum XmlNodeType
        {
            File,
            Content
        }
        /// <summary>
        /// 创建XML文档
        /// </summary>
        /// <param name="name">根节点名称</param>
        /// <param name="type">根节点的一个属性值</param>
        /// <returns>XmlDocument对象</returns>  
        public static XmlDocument CreateXmlDocument(string name, string type)
        {
            XmlDocument doc;
            try
            {
                doc = new XmlDocument();
                doc.LoadXml("<" + name + "/>");
                var rootEle = doc.DocumentElement;
                rootEle.SetAttribute("type", type);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
            return doc;
        }
        /// <summary>
        /// 读取XML节点数据
        /// </summary>
        /// <param name="path">XML文件路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// <returns>string</returns>
        public static string Read(string path, string node, string attribute)
        {
            var value = "";
            try
            {
                var doc = new XmlDocument();
                doc.Load(path);
                var xn = doc.SelectSingleNode(node);
                if (xn != null && xn.Attributes != null)
                    value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
            return value;
        }
        /// <summary>
        /// 插入XML节点数据
        /// </summary>
        /// <param name="path">XML文件路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(path);
                var xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        var xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    var xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(path);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
        }
        /// <summary>
        /// 修改节点数据
        /// </summary>
        /// <param name="path">XML文件路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(path);
                var xn = doc.SelectSingleNode(node);
                var xe = (XmlElement)xn;
                if (attribute.Equals(""))
                {
                    if (xe != null) xe.InnerText = value;
                }
                else
                {
                    xe.SetAttribute(attribute, value);
                }
                doc.Save(path);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">XML文件路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <returns></returns>
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(path);
                var xn = doc.SelectSingleNode(node);
                var xe = (XmlElement)xn;
                if (attribute.Equals(""))
                {
                    xn.ParentNode.RemoveChild(xn);
                }
                else
                {
                    xe.RemoveAttribute(attribute);
                }
                doc.Save(path);
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
        }
        /// <summary>
        /// 获得xml文件中指定节点的节点数据
        /// </summary>
        /// <param name="path">XML文件路径</param>
        /// <param name="nodeName">节点</param>
        /// <returns></returns>
        public static string GetNodeInfoByNodeName(string path, string nodeName)
        {
            var xmlString = string.Empty;
            try
            {
                var xml = new XmlDocument();
                xml.Load(path);
                var root = xml.DocumentElement;
                if (root == null) return xmlString;
                var node = root.SelectSingleNode("//" + nodeName);
                if (node != null)
                {
                    xmlString = node.InnerText;
                }
            }
            catch (Exception er)
            {
                throw new Exception(er.ToString());
            }
            return xmlString;
        }
        /// <summary> 
        /// 功能:读取指定节点的指定属性值  
        /// </summary>
        /// <param name="path">XML文件路径</param>
        /// <param name="strNode">节点名称</param> 
        /// <param name="strAttribute">此节点的属性</param> 
        /// <returns></returns> 
        public static string GetXmlNodeAttributeValue(string path, string strNode, string strAttribute)
        {
            var strReturn = "";
            try
            {
                var xml = new XmlDocument();
                xml.Load(path);
                //根据指定路径获取节点 
                var xmlNode = xml.SelectSingleNode(strNode);
                if (xmlNode != null)
                {
                    //获取节点的属性，并循环取出需要的属性值 
                    var xmlAttr = xmlNode.Attributes;
                    if (xmlAttr == null) return strReturn;
                    for (var i = 0; i < xmlAttr.Count; i++)
                    {
                        if (xmlAttr.Item(i).Name != strAttribute) continue;
                        strReturn = xmlAttr.Item(i).Value;
                        break;
                    }
                }
            }
            catch (XmlException xmle)
            {
                throw new Exception(xmle.Message);
            }
            return strReturn;
        }
        /// <summary> 
        /// 功能:设置节点的属性值  
        /// </summary>
        /// <param name="path">XML文件路径</param>
        /// <param name="xmlNodePath">节点名称</param> 
        /// <param name="xmlNodeAttribute">属性名称</param> 
        /// <param name="xmlNodeAttributeValue">属性值</param> 
        public static void SetXmlNodeAttributeValue(string path, string xmlNodePath, string xmlNodeAttribute, string xmlNodeAttributeValue)
        {
            try
            {
                var xml = new XmlDocument();
                xml.Load(path);
                //可以批量为符合条件的节点的属性付值 
                var xmlNode = xml.SelectNodes(xmlNodePath);
                if (xmlNode == null) return;
                foreach (var xmlAttr in from XmlNode xn in xmlNode select xn.Attributes)
                {
                    if (xmlAttr == null) return;
                    for (var i = 0; i < xmlAttr.Count; i++)
                    {
                        if (xmlAttr.Item(i).Name != xmlNodeAttribute) continue;
                        xmlAttr.Item(i).Value = xmlNodeAttributeValue;
                        break;
                    }
                }

            }
            catch (XmlException xmle)
            {
                throw new Exception(xmle.Message);
            }
        }
        /// <summary>
        /// 读取XML资源中的指定节点内容
        /// </summary>
        /// <param name="source">XML资源</param>
        /// <param name="xmlType">XML资源类型：文件，字符串</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>节点内容</returns>
        public static object GetNodeValue(string source, XmlNodeType xmlType, string nodeName)
        {
            var xd = new XmlDocument();
            if (xmlType == XmlNodeType.File)
            {
                xd.Load(source);
            }
            else
            {
                xd.LoadXml(source);
            }
            var xe = xd.DocumentElement;
            XmlNode xn = null;
            if (xe != null)
            {
                xn = xe.SelectSingleNode("//" + nodeName);

            }
            return xn.InnerText;
        }
        /// <summary>
        /// 更新XML文件中的指定节点内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="nodeName">节点名称</param>
        /// <param name="nodeValue">更新内容</param>
        /// <returns>更新是否成功</returns>
        public static bool UpdateNode(string filePath, string nodeName, string nodeValue)
        {
            try
            {
                bool flag;
                var xd = new XmlDocument();
                xd.Load(filePath);
                var xe = xd.DocumentElement;
                if (xe == null) return false;
                var xn = xe.SelectSingleNode("//" + nodeName);
                if (xn != null)
                {
                    xn.InnerText = nodeValue;
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// XML数据转换为DataSet
        /// </summary>
        /// <param name="xmlData">XML字符串</param>
        /// <returns>DataSet数据</returns>
        public static DataSet GetDataSetByXml(string xmlData)
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
        /// 表单集转XML字符串
        /// </summary>
        /// <param name="xmlDS">表单集合</param>
        /// <returns>XML字符串</returns>
        public static string ConvertDataTableToXML(DataSet xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                Encoding gb2312 = Encoding.GetEncoding("GB2312");
                stream = new MemoryStream();
                writer = new XmlTextWriter(stream, gb2312);
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);
                return gb2312.GetString(arr).Trim();
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
        /// <param name="xmlDS">表单</param>
        /// <returns>XML字符串</returns>
        public static string ConvertDataTableToXML(DataTable xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;
            try
            {
                stream = new MemoryStream();
                writer = new XmlTextWriter(stream, Encoding.UTF8);
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);
                //UTF8Encoding utf = new UTF8Encoding();
                //return utf.GetString(arr).Trim();
                return System.Text.Encoding.Default.GetString(arr).Trim();
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
        public static DataSet ConvertXMLToDataSet(string xmlData)
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
            catch (Exception)
            {
                throw;
            }   
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        #endregion

        #region TreeView XML
        /// <summary>
        /// TreeView保存为Xml文件
        /// </summary>
        /// <param name="treeViewControl">TreeView控件</param>
        /// <param name="xmlFile">XML文件名</param>
        /// <param name="titleName">XML标题名称</param>
        public static void SaveTreeViewToXml(TreeView treeViewControl, string xmlFile, string titleName = "冠佳电子")
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<" + titleName + "></" + titleName + ">");
                XmlNode root = doc.DocumentElement;
                //doc.InsertBefore(doc.CreateXmlDeclaration("1.0", "utf-8", "yes"), root);
                TreeNodeToXml(treeViewControl.Nodes, root);
                doc.Save(xmlFile);
            }
            catch (Exception)
            {
                throw;
            }   
        }
        /// <summary>
        /// 加载xml到TreeView
        /// </summary>
        /// <param name="xmlFile">Xml文件</param>
        /// <param name="treeViewControl">TreeView控件</param>
        public static void LoadXmlToTreeView(string xmlFile, TreeView treeViewControl)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(xmlFile);
                TreeToXml(xmlDocument.ChildNodes, treeViewControl.Nodes);
            }
            catch (Exception)
            {
                throw;
            }   
        }
        /// <summary>
        /// 树节点转XML节点
        /// </summary>
        /// <param name="treeNodes"></param>
        /// <param name="xmlNode"></param>
        private static void TreeNodeToXml(TreeNodeCollection treeNodes, XmlNode xmlNode)
        {
            try
            {
                XmlDocument doc = xmlNode.OwnerDocument;
                foreach (TreeNode treeNode in treeNodes)
                {
                    //创建一个xml元素（element）
                    XmlNode element = doc.CreateNode("element", treeNode.Text, "");
                    //创建一个属性Name
                    XmlAttribute attr = doc.CreateAttribute("Name");
                    //为属性赋值
                    attr.Value = treeNode.Text;
                    //为该元素添加属性
                    element.Attributes.Append(attr);
                    //添加元素
                    xmlNode.AppendChild(element);
                    if (treeNode.Nodes.Count > 0)
                    {
                        TreeNodeToXml(treeNode.Nodes, element);
                    }
                }
            }
            catch (Exception)
            {                
                throw;
            }           
        }
        /// <summary>
        /// XML节点转树节点
        /// </summary>
        /// <param name="xmlNode"></param>
        /// <param name="nodes"></param>
        private static void TreeToXml(XmlNodeList xmlNode, TreeNodeCollection nodes)
        {
            try
            {
                foreach (XmlNode node in xmlNode)
                {
                    string strNode;
                    if (node.Name == "冠佳电子")
                        TreeToXml(node.ChildNodes, nodes);
                    if (node != null)
                    {
                        if (node.Attributes != null)
                            if (node.Attributes.Count > 0)
                                strNode = node.Attributes[0].Value;
                            else
                                strNode = node.Name;
                        else
                            strNode = node.Value;
                    }
                    else
                        strNode = node.Name;
                    if (strNode == "冠佳电子")
                        return;
                    TreeNode new_Child = new TreeNode(CLanguage.Lan(strNode));
                    nodes.Add(new_Child);
                    TreeToXml(node.ChildNodes, new_Child.Nodes);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        #endregion

    }
}
