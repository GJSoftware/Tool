using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aladdin.HASP;
using System.Threading;

namespace GJ.DEV.SafeDog
{
    public class CSentinel : IDog
    {
        #region 构造函数
        public CSentinel(int idNo = 0, string name = "CSentinel")
        {
            this._idNo = idNo;
            this._name = name;
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 编号
        /// </summary>
        public int idNo
        {
            get { return _idNo; }
            set { _idNo = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = "CSentinel";
        private const string defaultScope = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?> " + "<haspscope/> ";
        private Hasp hasp = new Hasp(HaspFeature.Default);
        private string scope = string.Empty;
        #endregion

        #region 配置文件（请勿修改）
        /// <summary>
        /// strVendorCode
        /// </summary>
        private const string strVendorCode = "LASml2iHdlvpiL5UorrUpxukHeP6fJrlAY9fteHHQ+H6pPItnbkRUY368UDMB4E8KCrX/l33Uo5mEUxu" +
        "MTXx8u+GGSDMwesnz6ysxcxoRMlJ8blxO7osy28HBwyb+3MQFxNuFtrRtlE7K7Ht3bzvSckIj0y3PH91" +
        "AGdsekf4b29a/q1IkWAWfzZv6izPUfAvziKn6a0UmMmXU0r5C696fsPY8CAeAMNWzt0zGPUO440uk5j/" +
        "8RqbDf2ZelqOa/a/ngWQ8oMffcNDWOVbXJwR/JAog7D3lIE4PUW+OoFdGyxOg83+kOqxmZwXQX+M5LXd" +
        "xlasfsbW6aRdSbpEjgvkh46i5GKrBuLEnIDr5R63qPo1Z5Mn9qMlIwx+s6Am4XwO1mNGCESqyffHGqEs" +
        "WfVhtZx5vODqi5vSKuuYHGm1XtTYqOZKSF9vxiF0/jWRBYR1CTdQQraS+zXHyD/2GXf/p7mvTN0p3LI5" +
        "DbKXqItwrXwLeELPUh1ggLg0axW+fsDS91xb7lzLYSFewXVjhZYwrg+W9KIeZkyYKQD8cKOa3zfQUHQ6" +
        "OpqsDxWT0G8gVCldfa3Ee9lMIVAPxwZhQpLKEbrkeJBeAB4+rRtJ7sVBFOh2GCSFQb+ENsG44m7FwKRR" +
        "WLb5RiZFTNQKg0Qw0/p/56LKyRIQT9EIgeAiS6XqdNaSx5SiKbtJotQPGHHSisOSajVRyL52KT4BNbky" +
        "39Bh/0UffTNmER+Xr1d+RWYRAexxqsiA8n3C7pohV6S2SJzsWK9EWG3aXsI33UuA+wZoaoVpyRtlfw2n" +
        "pIM8ouWFZ5atjhy1K+Aj25oAXj/2ib3JyfQZwqIyT+I4CUQ1e7DSGlcXguZgQ2js6FW9y+6DNUsoPBBh" +
        "EaCzHLstdIUJXtFCr1pc0ocj8xlts9lRWTk0To91w0Ih+raW0Z6h+aaPS88jr7+TeYTaf+qifSX2SxMH" +
        "9xRNiL55yXtNNCcB80WBzQ==";
        #endregion

        #region 共享方法
        /// <summary>
        /// 打开加密狗
        /// </summary>
        /// <param name="tz">厂商</param>
        /// <param name="pwr">天数</param>
        /// <param name="er">错误代码</param>
        /// <returns></returns>
        public HaspStatus ClassCS(int tz, out int leftDays, out int er)
        {
            leftDays = -1;

            er = -1;

            try
            {
                string pwr = string.Empty;

                scope = defaultScope;

                HaspFeature feature = HaspFeature.FromFeature(tz);

                hasp = new Hasp(feature);

                HaspStatus status = hasp.Login(strVendorCode, scope);

                if (status != HaspStatus.StatusOk)
                {
                    pwr = "0";

                    er = (int)status;

                    return status;
                }

                DateTime time3 = new DateTime();


                hasp.GetRtc(ref time3);
                int len = 10;
                byte[] bytes = new byte[len];
                HaspFile file = hasp.GetFile(HaspFileId.ReadWrite);
                status = file.Read(bytes, 0, bytes.Length);
                pwr = System.Text.Encoding.UTF8.GetString(bytes);
                pwr = pwr.Replace("\0", "");

                DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                long lTime = long.Parse(pwr);

                TimeSpan toNow = new TimeSpan(lTime);
                DateTime dtResult = dtStart.Add(toNow);

                int dayTime = (int)(Convert.ToInt32(pwr) / 3600 / 24);

                DateTime time4 = dtResult.AddDays(dayTime);

                TimeSpan sp = time4.Subtract(time3);

                leftDays = sp.Days;

                er = (int)status;

                return status;
            }
            catch (Exception)
            {
                return HaspStatus.SystemError;
            }

        }
        /// <summary>
        /// 返回ID
        /// </summary>
        /// <param name="tz">厂商</param>
        /// <param name="id">ID</param>
        /// <param name="er">错误代码</param>
        /// <returns></returns>
        public HaspStatus time(int tz, out string id, out string er)
        {
            id = string.Empty;

            er = string.Empty;

            try
            {
                string DateTime = string.Empty;
                scope = defaultScope;

                HaspFeature feature = HaspFeature.FromFeature(tz);

                hasp = new Hasp(feature);

                HaspStatus status = hasp.Login(strVendorCode, scope);

                if (status != HaspStatus.StatusOk)
                {
                    er = status.ToString();
                    return status;
                }
                er = Convert.ToString(status);
                string xlmFile = "<haspformat root=" + "\"" + "hasp_info" + "\"" + ">" +
                                  "<hasp>" +
                                  "<attribute name=" + "\"" + "id" + "\"" + "/>" +
                                  "<feature>" +
                                  "<element name=" + "\"" + "license" + "\"" + "/>" +
                                  "</feature>" +
                                  "</hasp>" +
                                  "</haspformat>";

                string info = string.Empty;

                string Time2 = hasp.GetSessionInfo(xlmFile, ref info).ToString();

                string[] sArray = info.Split(new char[2] { '"', '"' });

                if (sArray.Length > 6)
                {
                    id = sArray[5];
                }

                //释放内存
                status = hasp.Logout();

                return status;
            }
            catch (Exception ex)
            {
                er = ex.ToString();
                return HaspStatus.SystemError;
            }

        }
        /// <summary>
        /// 返回天数
        /// </summary>
        /// <param name="tz">厂商</param>
        /// <param name="pwr">激活码</param>
        /// <param name="day">天数</param>
        /// <returns></returns>
        public HaspStatus ActivationTime(int tz, string pwr, out int day)
        {

            scope = defaultScope;

            HaspFeature feature = HaspFeature.FromFeature(tz);

            hasp = new Hasp(feature);

            HaspStatus status = hasp.Login(strVendorCode, scope);

            if (pwr.Length != 16)
            {
                status = HaspStatus.TimeError;
                day = 0;
                return status;
            }
            if (status != HaspStatus.StatusOk)
            {

                day = 0;
                return status;
            }

            DateTime time = new DateTime();

            hasp.GetRtc(ref time);

            HaspFile file = hasp.GetFile(HaspFileId.ReadWrite);

            byte[] _bytes = new byte[16];

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

            if (pwr.Contains("6216"))
            {
                String[] arr = new String[5];
                byte[] bytes = new byte[80];
                file.FilePos = 16;
                status = file.Read(bytes, 0, bytes.Length);
                string _pwr = System.Text.Encoding.UTF8.GetString(bytes);

                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = _pwr.Substring(i * 16, 16);
                    if (pwr.Equals(arr[i]))
                    {
                        file.FilePos = (16 * i) + 16;
                        Thread.Sleep(10);
                        status = file.Write(_bytes, 0, _bytes.Length);

                        DateTime time3 = time.AddDays(30);

                        long time2 = (long)(time3 - dtStart).TotalSeconds;

                        string _time = time2.ToString();

                        byte[] decoded = System.Text.Encoding.UTF8.GetBytes(_time);

                        file.FilePos = 0;

                        status = file.Write(decoded, 0, decoded.Length);

                        day = 30;

                        return status;
                    }
                }
            }
            else if (pwr.Contains("6217"))
            {
                byte[] bytes = new byte[80];
                file.FilePos = 96;
                status = file.Read(bytes, 0, bytes.Length);
                string _pwr = System.Text.Encoding.UTF8.GetString(bytes);


                String[] arr = new String[5];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = _pwr.Substring(i * 16, 16);

                    if (pwr.Equals(arr[i]))
                    {
                        file.FilePos = (16 * i) + 96;
                        Thread.Sleep(10);
                        status = file.Write(_bytes, 0, _bytes.Length);

                        DateTime time3 = time.AddDays(60);

                        long time2 = (long)(time3 - dtStart).TotalSeconds;

                        string _time = time2.ToString();

                        byte[] decoded = System.Text.Encoding.UTF8.GetBytes(_time);

                        file.FilePos = 0;

                        status = file.Write(decoded, 0, decoded.Length);

                        day = 60;

                        return status;

                    }
                }

            }
            else if (pwr.Contains("6218"))
            {
                byte[] bytes = new byte[80];
                file.FilePos = 176;
                status = file.Read(bytes, 0, bytes.Length);

                string _pwr = System.Text.Encoding.UTF8.GetString(bytes);


                String[] arr = new String[8];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = _pwr.Substring(i * 16, 16);

                    if (pwr.Equals(arr[i]))
                    {
                        file.FilePos = (16 * i) + 176;
                        Thread.Sleep(10);
                        status = file.Write(_bytes, 0, _bytes.Length);

                        DateTime time3 = time.AddDays(90);

                        long time2 = (long)(time3 - dtStart).TotalSeconds;

                        string _time = time2.ToString();

                        byte[] decoded = System.Text.Encoding.UTF8.GetBytes(_time);

                        file.FilePos = 0;
                        status = file.Write(decoded, 0, decoded.Length);

                        day = 90;

                        return status;

                    }
                }
            }
            else if (pwr.Contains("6219"))
            {
                byte[] bytes = new byte[16];
                file.FilePos = 256;
                status = file.Read(bytes, 0, bytes.Length);

                string _pwr = System.Text.Encoding.UTF8.GetString(bytes);


                String[] arr = new String[1];
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = _pwr.Substring(i * 16, 16);

                    if (pwr.Equals(arr[i]))
                    {
                        DateTime time3 = time.AddDays(999);

                        long time2 = (long)(time3 - dtStart).TotalSeconds;

                        string _time = time2.ToString();

                        byte[] decoded = System.Text.Encoding.UTF8.GetBytes(_time);

                        file.FilePos = 0;
                        status = file.Write(decoded, 0, decoded.Length);

                        day = 999;

                        return status;

                    }
                }
            }
            else
            {
                status = HaspStatus.TimeError;
                day = 0;
                return status;
            }

            status = HaspStatus.SystemError;
            day = 0;
            return status;
        }
        #endregion

        #region 专用功能
        /// <summary>
        /// 密码狗检索
        /// </summary>
        /// <param name="er"></param>
        /// <returns></returns>
        public bool check_safe_dog(int tz, out int leftDays, out string idno, out string er)
        {
            idno = string.Empty;
            leftDays = -1;
            er = "7";
            int ers = 0;
            try
            {
                if (time(tz, out idno, out er) != HaspStatus.StatusOk)
                {
                    return false;
                }
                if (ClassCS(tz, out leftDays, out ers) != HaspStatus.StatusOk)
                {
                    return false;
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
        /// 激活
        /// </summary>
        /// <returns></returns>
        public bool check_safe_dog(int EID, string pwr, out int leftDays, out string er)
        {
            leftDays = -1;

            er = string.Empty;

            try
            {
                HaspStatus status = ActivationTime(EID, pwr, out leftDays);

                if (status != HaspStatus.StatusOk)
                {
                    er = status.ToString();
                    return false;
                }
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
