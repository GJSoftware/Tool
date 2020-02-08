using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace GJ.DEV.BARCODE
{
    /// <summary>
    /// 条码枪工厂类
    /// </summary>
    public class CBarCOM
    {
        #region 构造函数
        public CBarCOM(EBarType barType,int idNo=0,string name="IBarCode")
        {
            this._idNo = idNo;

            this._name = name;

            this._barType = barType; 

            object obj = load(barType.ToString());

            if (obj != null)
            {
                _devBar = (IBarCode)obj;

                _devBar.OnRecved += new OnRecvHandler(onRecvTriger);

                this._comMode = _devBar.comMode; 
            }

        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = string.Empty;
        private EBarType _barType = EBarType.KS100;
        private EComMode _comMode = EComMode.SerialPort; 
        private IBarCode _devBar = null;
        #endregion

        #region 事件
        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event OnRecvHandler OnRecved;
        void OnRecv(CRecvArgs e)
        {
            if (OnRecved != null)
            {
                OnRecved(this, e);
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 设备ID
        /// </summary>
        public int idNo
        {
            get { return _idNo; }
            set { _idNo = value; }
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 设备类型 
        /// </summary>
        public EBarType barType
        {
            get { return _barType; }
        }
        /// <summary>
        /// 通信接口
        /// </summary>
        public EComMode comMode
        {
            get { return _comMode; }
        }
        #endregion

        #region 共享方法
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="comName"></param>
        /// <param name="er"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public bool Open(string comName, out string er, string setting = "115200,n,8,1", bool recvThreshold = false)
        {
            er = string.Empty;

            try
            {
                if (_devBar == null)
                {
                    er = _barType.ToString() + "未找到程序集,请检查";
                    return false;
                }

                return _devBar.Open(comName, out er, setting, recvThreshold);
            }
            catch (Exception e)
            {
                er = e.ToString();
                return false;
            }
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            if (_devBar != null)
            {
                _devBar.Close();
            }
        }
        /// <summary>
        /// 读取条码
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="er"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool Read(out string serialNo, out string er, int rLen = 0, int timeOut = 500)
        {
            return _devBar.Read(out serialNo, out er, rLen,timeOut);
        }
        /// <summary>
        /// 连续读取条码
        /// </summary>
        /// <param name="serialNo"></param>
        /// <param name="er"></param>
        /// <param name="SOI"></param>
        /// <param name="rLen"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool Read(out string serialNo, out string er, string SOI, int rLen, int timeOut = 500)
        {
            return _devBar.Read(out serialNo, out er,SOI, rLen, timeOut);
        }
        /// <summary>
        /// 触发条码枪接收数据
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool Triger_Start(out string er)
        {
            return _devBar.Triger_Start(out er); 
        }
        /// <summary>
        /// 触发接收串口数据
        /// </summary>
        /// <param name="serialNo"></param>
        /// <returns></returns>
        public bool Triger_End(out string er)
        {
            return _devBar.Triger_End(out er); 
        }
        #endregion

        #region 方法
        /// <summary>
        /// 加载设备类型
        /// </summary>
        /// <param name="devType"></param>
        /// <returns></returns>
        private object load(string devType)
        {
            try
            {
                object obj = null;

                string _devName = "C" + devType;

                Assembly asb = Assembly.GetExecutingAssembly();

                object[] parameters = new object[2];

                parameters[0] = _idNo;

                parameters[1] = _name;

                foreach (Type t in asb.GetTypes())
                {
                    if (t.Name == _devName)
                    {
                        obj = asb.CreateInstance(t.FullName, true, System.Reflection.BindingFlags.Default, null, parameters, null, null);
                        break;
                    }
                }

                return obj;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 事件触发
        private void onRecvTriger(object sender, CRecvArgs e)
        {
            OnRecv(e); 
        }
        #endregion

    }
}
