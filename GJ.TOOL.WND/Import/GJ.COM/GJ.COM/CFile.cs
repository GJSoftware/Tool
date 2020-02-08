using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace GJ.COM
{ 
    /// <summary>
    /// 文件操作
    /// </summary>
    public class CFile
    {
        /// <summary>
        /// 删除文件目录过期文件（以当前日期基准）
        /// </summary>
        /// <param name="folder">文件目录</param>
        /// <param name="fileExtend">文件扩展名(txt,csv),为空则为全部</param>
        /// <param name="dayLimit">过期天数</param>
        /// <returns></returns>
        public static bool Del_Overdue_Files(string folder, string fileExtend, int dayLimit,out string er)
        {
            er = string.Empty;

            try
            {
                if (!Directory.Exists(folder))
                    return false;
                string[] fileName;
                if(fileExtend!=string.Empty)
                   fileName = Directory.GetFiles(folder, fileExtend);
                else
                   fileName = Directory.GetFiles(folder);
                for (int i = 0; i < fileName.Length; i++)
                {
                    DateTime dt = File.GetCreationTime(fileName[i]);
                    TimeSpan t = DateTime.Now.Subtract(dt);
                    if (t.Days > dayLimit)
                        File.Delete(fileName[i]);
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
        /// 程序是否在运行中
        /// </summary>
        /// <param name="processName">exe的进程名</param>
        /// <returns></returns>
        public static bool Check_Program(string progName)
        {
            try
            {
                Process[] myProces = Process.GetProcessesByName(progName);
                if (myProces.Length != 0)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 运行可执行程序
        /// </summary>
        /// <param name="filePath">可执行文件路径</param>
        public static void Start_Program(string filePath)
        {
            Process pro = new Process();
            FileInfo file = new FileInfo(filePath);
            pro.StartInfo.WorkingDirectory = file.Directory.FullName;
            pro.StartInfo.FileName = filePath;
            pro.StartInfo.CreateNoWindow = false;
            pro.Start();
            pro.WaitForExit();
        }
        /// <summary>
        /// 停止可执行程序
        /// </summary>
        /// <param name="processName">exe的进程名</param>
        public static void Stop_Program(string processName)
        {
            Process[] allProgresse = System.Diagnostics.Process.GetProcessesByName(processName);
            foreach (Process closeProgress in allProgresse)
            {
                if (closeProgress.ProcessName.Equals(processName))
                {
                    closeProgress.Kill();
                    closeProgress.WaitForExit();
                    break;
                }
            }  
        }
    }
}
