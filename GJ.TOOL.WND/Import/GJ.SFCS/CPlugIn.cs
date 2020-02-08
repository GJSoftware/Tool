using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using System.IO;
namespace GJ.SFCS
{
    /// <summary>
    /// 定义插件标注名
    /// </summary>
    public interface IPlugClass
    {
        string PlugName { get; }   //插件标识名
    }
    /// <summary>
    /// 自动加载插件类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PlugBase<T>
    {
        /// <summary>
        /// 获取类
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public T GetClass(string className)
        {
            if (Names.Contains(className))
            {
                var plug = PlugList.Where(i => i.Metadata.PlugName == className).Select(p => p.Value).FirstOrDefault();

                return (T)plug;
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// 获取插件类名列表
        /// </summary>
        public List<string> Names
        {
            get
            {
                List<string> names = new List<string>();

                foreach (var item in PlugList)
                {
                    names.Add(item.Metadata.PlugName);
                }

                return names;
            }
        }
        /// <summary>
        /// 插件列表
        /// </summary>
        [ImportMany]
        private List<Lazy<T, IPlugClass>> PlugList = new List<Lazy<T, IPlugClass>>();
        /// <summary>
        /// 文件夹加载
        /// </summary>
        /// <param name="subFolderName"></param>
        public PlugBase(string subFolderName)
        {
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\" + subFolderName);
            var catelog = new AggregateCatalog();
            //AssemblyCatalog assemblyCataLog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            catelog.Catalogs.Add(new DirectoryCatalog(subFolderName));
            var container = new CompositionContainer(catelog);
            container.ComposeParts(this);
        }
        /// <summary>
        /// 加载本目录
        /// </summary>
        public PlugBase()
        {
            var catelog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var container = new CompositionContainer(catelog);
            container.ComposeParts(this);
        }
    }
}
