using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GJ.COM
{
    public class CStuct<T> where T : struct
    {
        /// <summary>
        /// 结构体转化为字节
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static byte[] StructToBytes(T sender)
        {
            int res = System.Runtime.InteropServices.Marshal.SizeOf(sender);
            byte[] b_array = new byte[res];
            IntPtr buffPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(res);
            System.Runtime.InteropServices.Marshal.StructureToPtr(sender, buffPtr, false);
            System.Runtime.InteropServices.Marshal.Copy(buffPtr, b_array, 0, res);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(buffPtr);
            return b_array;
        }
        /// <summary>
        /// 字节转化为结构体
        /// </summary>
        /// <param name="arrByte"></param>
        /// <returns></returns>
        public static T BytesToStruct(byte[] arrByte)
        {
            int size = arrByte.Length;
            T struReturn = new T();
            System.IntPtr ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            System.Runtime.InteropServices.Marshal.Copy(arrByte, 0, ptr, arrByte.Length);
            struReturn = (T)System.Runtime.InteropServices.Marshal.PtrToStructure(ptr, struReturn.GetType());
            System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
            return struReturn;
        }
        /// <summary>
        /// 字节转化为结构体
        /// </summary>
        /// <param name="arrByte"></param>
        /// <returns></returns>
        public static T BytesToStruct(byte[] arrByte, Type type)
        {
            int size = arrByte.Length;
            System.IntPtr ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
            System.Runtime.InteropServices.Marshal.Copy(arrByte, 0, ptr, arrByte.Length);
            T struReturn = (T)System.Runtime.InteropServices.Marshal.PtrToStructure(ptr, type);
            System.Runtime.InteropServices.Marshal.FreeHGlobal(ptr);
            return struReturn;
        }
        /// <summary>
        /// 显示结构体内容
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static string ShowInfo(T sender)
        {
            string structInfo = string.Empty;

            Type t = sender.GetType();

            System.Reflection.FieldInfo[] fields = t.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                structInfo += field.Name + "=" + field.GetValue(sender);
                structInfo += "|";
            }
            return structInfo;
        }
        /// <summary>
        /// 返回结构体字节长度
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static int GetStuctLen(T sender)
        {
            return System.Runtime.InteropServices.Marshal.SizeOf(sender);
        }
    }
}
