using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
namespace GJ.COM
{
    /// <summary>
    /// 中英文切换
    /// </summary>
    public class CLanguage
    {
        #region 类与枚举
        /// <summary>
        /// 语言类型
        /// </summary>
        public enum EL
        {
            中文,
            英语
        }
        #endregion

        #region 字段
        /// <summary>
        /// ini文件
        /// </summary>
        private static string _iniFile = Application.StartupPath + "\\Language.ini";
        /// <summary>
        /// 语言类型
        /// </summary>
        private static EL _LanguageType = EL.中文;
        /// <summary>
        /// 中英文字典
        /// </summary>
        private static Dictionary<string, string> _languageList = new Dictionary<string, string>();
        #endregion

        #region 属性
        /// <summary>
        /// 设置和获取当前语言类型
        /// </summary>
        public static EL languageType
        {
            set 
            {
                _LanguageType = value;
                CIniFile.WriteToIni("Language", "default", ((int)_LanguageType).ToString(), _iniFile);
            }
            get 
            {
                _LanguageType = (EL)System.Convert.ToInt16(CIniFile.ReadFromIni("Language", "default", _iniFile, "0"));
                return _LanguageType;
            }
        }
        #endregion

        #region 共享方法
        /// <summary>
        /// 加载中英文字典
        /// </summary>
        /// <param name="lanFormat">中文与英文对应字典</param>
        /// <param name="er"></param>
        /// <returns></returns>
        public static bool Load(Dictionary<string, string> lanFormat,out string er)
        {
            er = string.Empty;

            try
            {
                _languageList.Clear();

                foreach (string key in lanFormat.Keys)
                {
                    _languageList.Add(key, lanFormat[key]);
                }

                return true;
            }
            catch (Exception ex)
            {
                er = ex.ToString();

                return false;
            }
        }
        /// <summary>
        /// 获取语言类型
        /// </summary>
        public static void LoadLanType()
        {
            _LanguageType = (EL)System.Convert.ToInt16(CIniFile.ReadFromIni("Language", "default", _iniFile, "0"));
        }
        /// <summary>
        /// 设置语言类型
        /// </summary>
        /// <param name="lanType"></param>
        public static void SetLanType(EL lanType)
        {
            _LanguageType = lanType;

            CIniFile.WriteToIni("Language", "default", ((int)lanType).ToString(), _iniFile);
        }
        /// <summary>
        /// 窗口本地化
        /// </summary>
        /// <param name="dlg"></param>
        public static void SetLanguage(Form dlg)
        {
            try
            {
                //更改当前线程的 CultureInfo
                //zh-CN 为中文，更多的关于 Culture 的字符串请查 MSDN
                switch (_LanguageType)
                {
                    case EL.中文:
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
                        break;
                    case EL.英语:
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                        break;
                    default:
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
                        break;
                }
                //对当前窗体应用更改后的资源
                if (dlg != null)
                {
                    string objName = new Guid().ToString();
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(dlg.GetType());
                    resources.ApplyResources(dlg, objName);
                    //resources.ApplyResources(dlg, "$this");
                    appLang(dlg, resources);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 控件本地化
        /// </summary>
        /// <param name="language"></param>
        public static void SetLanguage(UserControl dlg)
        {
            try
            {
                //更改当前线程的 CultureInfo
                //zh-CN 为中文，更多的关于 Culture 的字符串请查 MSDN
                switch (_LanguageType)
                {
                    case EL.中文:
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
                        break;
                    case EL.英语:
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                        break;
                    default:
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
                        break;
                }
                //对当前窗体应用更改后的资源
                if (dlg != null)
                {
                    string objName = new Guid().ToString();
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(dlg.GetType());
                    //resources.ApplyResources(dlg, "$this");
                    resources.ApplyResources(dlg, objName);
                    appLang(dlg, resources);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 控件本地化
        /// </summary>
        /// <param name="language"></param>
        public static void SetLanguage(Control dlg)
        {
            try
            {
                //更改当前线程的 CultureInfo
                //zh-CN 为中文，更多的关于 Culture 的字符串请查 MSDN
                switch (_LanguageType)
                {
                    case EL.中文:
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
                        break;
                    case EL.英语:
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
                        break;
                    default:
                        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("zh-CN");
                        break;
                }
                //对当前窗体应用更改后的资源
                if (dlg != null)
                {
                    string objName = new Guid().ToString();
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(dlg.GetType());
                    //resources.ApplyResources(dlg, "$this");
                    resources.ApplyResources(dlg, objName);
                    appLang(dlg, resources);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 中英文切换
        /// </summary>
        /// <param name="def_Lan"></param>
        /// <returns></returns>
        public static string Lan(string str)
        {
            try
            {
                if (_languageList.Count == 0)
                    return str;

                if (_LanguageType == EL.英语)
                {
                    if (_languageList.Keys.Contains(str))
                    {
                        str = _languageList[str];
                    }
                }
                return str;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 遍历窗体所有控件，针对其设置当前界面语言
        /// </summary>
        /// <param name="control"></param>
        /// <param name="resources"></param>
        private static void appLang(Control control, System.ComponentModel.ComponentResourceManager resources)
        {
            try
            {
                //菜单栏
                if (control is MenuStrip)
                {
                    //将资源应用与对应的属性
                    resources.ApplyResources(control, control.Name);
                    MenuStrip ms = (MenuStrip)control;
                    if (ms.Items.Count > 0)
                    {
                        foreach (ToolStripMenuItem c in ms.Items)
                        {
                            //调用 遍历菜单 设置语言
                            appLang(c, resources);
                        }
                    }
                }
                //工具栏
                if (control.GetType().ToString() == "System.Windows.Forms.ToolStrip")
                {
                    //将资源应用与对应的属性
                    resources.ApplyResources(control, control.Name);
                    ToolStrip ts = (ToolStrip)control;
                    foreach (ToolStripItem item in ts.Items)
                    {
                        resources.ApplyResources(item, item.Name);
                    }
                }
                if (control.GetType().ToString() == "System.Windows.Forms.DataGridView")
                {
                    var dgv = control as DataGridView;
                    foreach (DataGridViewColumn col in dgv.Columns)
                        resources.ApplyResources(col, col.Name);                 
                }
                //控件
                foreach (Control c in control.Controls)
                {
                    if (c.GetType().ToString() != "System.Windows.Forms.TableLayoutPanel")
                        resources.ApplyResources(c, c.Name);
                    appLang(c, resources);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 遍历菜单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="resources"></param>
        private static void appLang(ToolStripMenuItem item, System.ComponentModel.ComponentResourceManager resources)
        {
            try
            {
                if (item is ToolStripMenuItem)
                {
                    resources.ApplyResources(item, item.Name);
                    ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                    if (tsmi.DropDownItems.Count > 0)
                    {
                        foreach (var c in tsmi.DropDownItems)
                        {
                            if (c.GetType().ToString() == "System.Windows.Forms.ToolStripMenuItem")
                            {
                                appLang((ToolStripMenuItem)c, resources);
                            }
                        }
                    }
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
