using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace GJ.UI
{
    [ToolboxBitmap(typeof(udcChartOnOff), "Chart_Bar.bmp")]
    public partial class udcChartOnOff : UserControl
    {
        #region 构造函数
        public udcChartOnOff()
        {
            InitializeComponent();

            InitialControl();

            SetDoubleBuffered();

        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitialControl()
        {
            this.Paint += new System.Windows.Forms.PaintEventHandler(OnPaint);
        }
        /// <summary>
        /// 设置双缓冲,防止界面闪烁
        /// </summary>
        private void SetDoubleBuffered()
        {


        }
        #endregion

        #region 类定义
        /// <summary>
        /// ONOFF时序类
        /// </summary>
        public class COnOff
        {
            /// <summary>
            /// 当前输入电压
            /// </summary>
            public int curVolt = 0;
            /// <summary>
            /// ONOFF时间(秒)
            /// </summary>
            public int onoffTimes = 0;
            /// <summary>
            /// ON时间(秒)
            /// </summary>
            public int onTimes = 0;
            /// <summary>
            /// OFF时间(秒)
            /// </summary>
            public int offTimes = 0;
            /// <summary>
            /// 输出规格
            /// </summary>
            public int outPutType = 0;
        }
        #endregion

        #region 字段
        /// <summary>
        /// 类编号ID
        /// </summary>
        private int _idNo = 0;
        /// <summary>
        /// 类名称
        /// </summary>
        private string _name = string.Empty;
        /// <summary>
        /// 老化时间(H)
        /// </summary>
        private double _biTime = 0;
        /// <summary>
        /// ONOFF时序
        /// </summary>
        private List<COnOff> _onoff = new List<COnOff>();
        /// <summary>
        /// 最大输入电压
        /// </summary>
        private int _maxVolt = 220;

        /// <summary>
        /// X轴边界
        /// </summary>
        private const int Xv = 20;
        /// <summary>
        /// Y轴边界
        /// </summary>
        private const int Yv = 12;
        /// <summary>
        /// 当前X轴
        /// </summary>
        private float curX = 0;
        /// <summary>
        /// 当前Y轴
        /// </summary>
        private float curY = 0;
        /// <summary>
        /// Y轴ON坐标
        /// </summary>
        private float curYon = 0;
        /// <summary>
        /// Y轴OFF坐标
        /// </summary>
        private float curYoff = 0;
        /// <summary>
        /// 单位时间坐标值
        /// </summary>
        private float unitTv = 0;
        /// <summary>
        /// 运行中
        /// </summary>
        private bool runStart = false; 
        /// <summary>
        /// ON标志
        /// </summary>
        private bool runOnOffFlag = true;
        /// <summary>
        /// 边沿标志
        /// </summary>
        private bool raiseOnOff = true;
        /// <summary>
        /// 上升电压
        /// </summary>
        private int raiseACVolt = 0;
        /// <summary>
        /// 初始时间
        /// </summary>
        private int iniTime = 0;
        /// <summary>
        /// 当前运行时间
        /// </summary>
        private int runningTime = 0;
        /// <summary>
        /// 当前AC电压
        /// </summary>
        private int runCurACVolt = 0;
        /// <summary>
        /// 当前输出规格
        /// </summary>
        private int runOutPutType = 0;
        /// <summary>
        /// 当前X轴
        /// </summary>
        private float curXing = 0;
        /// <summary>
        /// 当前Y轴
        /// </summary>
        private float curYing = 0;
        /// <summary>
        /// 运行中
        /// </summary>
        private bool runStarting = false;
        /// <summary>
        /// ON标志
        /// </summary>
        private bool runOnOffFlaging = true;
        /// <summary>
        /// 边沿标志
        /// </summary>
        private bool raiseOnOffing = true;
        /// <summary>
        /// 上升电压
        /// </summary>
        private int raiseACVolting = 0;
        /// <summary>
        /// 复位AC
        /// </summary>
        private bool resetRun = false;
        /// <summary>
        /// 开始时间
        /// </summary>
        private string startTime = string.Empty;
        /// <summary>
        /// 运行中电压
        /// </summary>
        private int runACVolting = 0;
        /// <summary>
        /// 运行结束
        /// </summary>
        private bool runEnd = false;
        #endregion

        #region 属性
        /// <summary>
        /// ID编号
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("编号ID")] 
        public int idNo
        {
            get { return _idNo; }
            set { _idNo = value; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("类名称")] 
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 老化时间(H)
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("老化时间(H)")] 
        public double biTime
        {
            get { return _biTime; }
            set { _biTime = value; }
        }
        /// <summary>
        /// ONOFF时序段
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("ONOFF时序")] 
        public List<COnOff> onoff
        {
            set {
                 
                 _onoff.Clear();

                 for (int i = 0; i < value.Count; i++)
                 {
                     COnOff para = new COnOff();

                     para.curVolt = value[i].curVolt;

                     para.onoffTimes = value[i].onoffTimes;

                     para.onTimes = value[i].onTimes;

                     para.offTimes = value[i].offTimes;

                     para.outPutType = value[i].outPutType; 

                     _onoff.Add(para); 
                 }

                }
        }
        /// <summary>
        /// 最大输入电压
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("最大输入电压")]
        public int maxVolt
        {
            get { return _maxVolt; }
            set { _maxVolt = value; }
        }
        /// <summary>
        /// 当前电压
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("当前电压")]
        public int runVolt
        {
            get { return runCurACVolt; }
        }
        /// <summary>
        /// 当前输出规格
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("当前输出规格")]
        public int outPutType
        {
            get { return runOutPutType; }
        }
        /// <summary>
        /// 初始运行时间(S)
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("初始运行时间")]
        public int iniTimes
        {
            get { return iniTime; }

            set { iniTime = value; }
        }
        /// <summary>
        /// 当前运行时间(S)
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("当前运行时间")]
        public int runTimes
        {
            get { return runningTime; }

            set { runningTime = value; }
        }
        #endregion

        #region 私有方法
        private void runWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    if (this.IsDisposed)
                        return;

                    if (runWorker.CancellationPending)
                        return;

                    TimeSpan s1 = new TimeSpan(System.Convert.ToDateTime(startTime).Ticks);
                    
                    TimeSpan s2 = new TimeSpan(System.Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")).Ticks);
                    
                    TimeSpan s = s2.Subtract(s1);

                    int calRunTimes = s.Seconds;

                    if (calRunTimes >= 1)
                    {
                        runningTime += calRunTimes;

                        if (runningTime >= (int)(_biTime * 3600))
                            runningTime = (int)(_biTime * 3600);
                        
                        startTime = DateTime.Now.ToString();

                        onRefreshUI(resetRun);
                    
                        if (runningTime == (int)(_biTime * 3600))
                            break;
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
            }
        }
        /// <summary>
        /// 计算ONOFF时序曲线
        /// </summary>
        /// <param name="inputAC"></param>
        /// <param name="runTime"></param>
        /// <param name="runOnTime"></param>
        /// <param name="runOffTime"></param>
        private void calOnOffWave(int inputAC, int runTime, int runOnTime, int runOffTime)
        {
            try
            {
                if (runOnTime == 0 && runOffTime == 0) //该段无效
                    return;

                bool alertnate = false;

                while (runTime > 0)
                {
                    int runInputAC = 0;

                    int runWaveTime = 0;

                    if (!alertnate)
                    {
                        if (runOnTime > 0)
                        {
                            if (runTime > runOnTime)
                                runWaveTime = runOnTime;
                            else
                                runWaveTime = runTime;
                            runInputAC = inputAC;
                            runOnOffFlag = true;
                        }
                    }
                    else
                    {
                        if (runTime > runOffTime)
                            runWaveTime = runOffTime;
                        else
                            runWaveTime = runTime;

                        runInputAC = 0;

                        runOnOffFlag = false;
                    }

                    runTime -= runWaveTime;

                    if (runTime < 0)
                        runTime = 0;

                    drawOnOff(runOnOffFlag, runInputAC, runWaveTime);
                    
                    alertnate = !alertnate;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 画ONOFF时序曲线
        /// </summary>
        /// <param name="wOnOff"></param>
        /// <param name="inputAC"></param>
        /// <param name="waveTime"></param>
        private void drawOnOff(bool wOnOff, int inputAC, float waveTime)
        {
            try
            {
                Graphics g = Graphics.FromHwnd(this.Handle);

                Pen pen = Pens.Red;

                if (runStart)
                {
                    if (inputAC == 0)
                    {
                        g.DrawLine(pen, curX, curY, curX, curYoff);
                    }
                    else
                    {
                        if (inputAC == raiseACVolt)
                            g.DrawLine(pen, curX, curY, curX, curY);
                        else
                        {
                            float y_v = curYoff - inputAC * (curYoff - curYon) / _maxVolt;
                            g.DrawLine(pen, curX, curY, curX, y_v);
                        }
                    }
                }

                float x_T = unitTv * waveTime;

                if (wOnOff)
                {
                    float y_v = (_maxVolt - inputAC) * (curYoff - curYon) / _maxVolt + curYon;
                    g.DrawLine(pen, curX, y_v, curX + x_T, y_v);
                    raiseACVolt = inputAC;
                    curY = y_v;
                }
                else
                {
                    g.DrawLine(pen, curX, curYoff, curX + x_T, curYoff);
                    raiseACVolt = 0;
                    curY = curYoff;
                }

                curX += x_T;

                raiseOnOff = wOnOff;

                runStart = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 计算正在ONOFF时序曲线
        /// </summary>
        /// <param name="inputAC"></param>
        /// <param name="runTime"></param>
        /// <param name="runOnTime"></param>
        /// <param name="runOffTime"></param>
        private void calOnOffWaveing(int inputAC, int runTime, int runOnTime, int runOffTime)
        {
            try
            {
                int runWaveTime = 0;

                int runInputAC = 0;

                while (runTime > 0)
                {
                    if (runOnTime == 0 && runOffTime == 0) //该段无效
                        return;

                    if (runOnTime == 0)   //全Off时间
                    {
                        runOnOffFlaging = false;
                        runWaveTime = runTime;
                        runInputAC = 0;
                    }
                    else if (runOffTime == 0) //全On时间
                    {
                        runOnOffFlaging = true;
                        runWaveTime = runTime;
                        runInputAC = inputAC;
                    }
                    else
                    {
                        if (runOnOffFlaging)  //ON
                        {
                            if (runTime > runOnTime)
                                runWaveTime = runOnTime;
                            else
                                runWaveTime = runTime;
                            runInputAC = inputAC;
                            runACVolting = inputAC;
                        }
                        else                //OFF
                        {
                            if (runTime > runOffTime)
                                runWaveTime = runOffTime;
                            else
                                runWaveTime = runTime;
                            runInputAC = 0;
                            runACVolting = 0;
                        }
                    }

                    runTime -= runWaveTime;

                    if (runTime < 0)
                        runTime = 0;

                    drawOnOffing(runOnOffFlaging, runInputAC, runWaveTime);

                    runOnOffFlaging = !runOnOffFlaging;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 画正在ONOFF时序曲线
        /// </summary>
        /// <param name="wOnOff"></param>
        /// <param name="inputAC"></param>
        /// <param name="waveTime"></param>
        private void drawOnOffing(bool wOnOff, int inputAC, float waveTime)
        {
            try
            {

                Graphics g = Graphics.FromHwnd(this.Handle);

                Pen pen = Pens.Green;

                if (runStarting && raiseOnOffing != wOnOff)
                {
                    if (inputAC == 0)
                    {
                        g.DrawLine(pen, curXing, curYing, curXing, curYoff);
                    }
                    else
                    {
                        if (inputAC == raiseACVolting)
                            g.DrawLine(pen, curXing, curYing, curXing, curYing);
                        else
                        {
                            float y_v = curYoff - inputAC * (curYoff - curYon) / _maxVolt;
                            g.DrawLine(pen, curXing, curYing, curXing, y_v);
                        }
                    }
                }

                float x_T = unitTv * waveTime;

                if (wOnOff)
                {
                    float y_v = (_maxVolt - inputAC) * (curYoff - curYon) / _maxVolt + curYon;
                    g.DrawLine(pen, curXing, y_v, curXing + x_T, y_v);  //Pens.Green
                    raiseACVolting = inputAC;
                    curYing = y_v;
                }
                else
                {
                    g.DrawLine(pen, curXing, curYoff, curXing + x_T, curYoff); //Pens.Green
                    raiseACVolting = 0;
                    curYing = curYoff;
                }

                curXing += x_T;

                raiseOnOffing = wOnOff;

                runStarting = true;
            }
            catch (Exception)
            {
                //throw;
            }
        }
        private void onRefreshUI(bool reset = false)
        {
            try
            {
                if (this.InvokeRequired)
                    this.Invoke(new Action<bool>(onRefreshUI), reset);
                else
                    if (!reset)
                        refreshUI();
                    else
                        this.Refresh();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 更新曲线
        /// </summary>
        public void refreshUI()
        {
            if (runningTime == 0 || _biTime == 0 || _onoff.Count == 0)
                return;

            //防止没设置时间段造成死循环
            int calOnOffTime = 0;
            for (int i = 0; i < _onoff.Count; i++)
                calOnOffTime += _onoff[i].onoffTimes;
            if (calOnOffTime == 0)
                return;

            calOnOffTime = 0;
            for (int i = 0; i < _onoff.Count; i++)
            {
                calOnOffTime += _onoff[i].onTimes;
                calOnOffTime += _onoff[i].offTimes;
            }
            if (calOnOffTime == 0)
                return;

            runOnOffFlaging = true;

            raiseOnOffing = true;

            runStarting = false;

            int runOutPutTyping = 0;

            int biTime=(int)(_biTime * 3600);

            //运行时间
            if (runningTime > biTime)
                runningTime = biTime;

            int runTime = runningTime;

            int runTime1 = runTime;

            if (runTime1 > biTime - iniTime)
                runTime1 = biTime - iniTime;

            int runTime2 = runTime - (biTime - iniTime);

            //前段时间
            if (runTime1 > 0)
            {
                float x_T = unitTv * iniTime;

                curXing = Xv + x_T; 

                do
                {
                    int runonoffTime = 0;

                    int firstTime = -1;

                    for (int i = 0; i < _onoff.Count; i++)
                    {
                        if (_onoff[i].onTimes + _onoff[i].offTimes == 0)
                            continue;
                        if (_onoff[i].onoffTimes == 0)
                            continue;

                        runonoffTime += _onoff[i].onoffTimes;

                        if (runonoffTime < iniTime)
                            continue;

                        if (firstTime == -1)
                        {
                            firstTime = runonoffTime - iniTime;

                            firstTime = _onoff[i].onoffTimes - firstTime;
                        }

                        int runWaveTime = 0;

                        CalOnOffWave(firstTime, runTime1, _onoff[i], out runWaveTime);

                        firstTime = 0;

                        runTime1 -= runWaveTime;

                        if (runTime1 <= 0)
                        {
                            runTime1 = 0;
                            break;
                        }

                        runOutPutTyping = _onoff[i].outPutType;
                    }

                } while (runTime1 > 0);

            }

            //返回时间
            if (runTime2 > 0)
            {
                runOnOffFlaging = true;

                raiseOnOffing = true;

                runStarting = false;

                curXing = Xv;

                do
                {
                    for (int i = 0; i < _onoff.Count; i++)
                    {
                        if (_onoff[i].onTimes + _onoff[i].offTimes == 0)
                            continue;

                        if (_onoff[i].onoffTimes == 0)
                            continue;

                        int runWaveTime = 0;

                        CalOnOffWave(0, runTime2, _onoff[i], out runWaveTime);

                        runTime2 -= runWaveTime;

                        if (runTime2 <= 0)
                        {
                            runTime2 = 0;
                            break;
                        }

                        runOutPutTyping = _onoff[i].outPutType;
                    }

                } while (runTime2 > 0);
            }

            /****************检测ON/OFF状态***************/

            if (resetRun || runCurACVolt != runACVolting || runOutPutType != runOutPutTyping)
            {
                resetRun = false;

                if (runningTime >= (int)(_biTime * 3600))
                {
                    runEnd = true;
                    OnChangeACVolted(new COnOffArgs(runACVolting,runOutPutTyping, runningTime, runEnd));
                }
                else
                {
                    if (runWorker.IsBusy)    //运行中
                        OnChangeACVolted(new COnOffArgs(runACVolting, runOutPutTyping, runningTime));
                }

            }
            else
            {
                if (!runEnd && (runningTime >= (int)(_biTime * 3600)))
                {
                    runEnd = true;
                    OnChangeACVolted(new COnOffArgs(runACVolting,runOutPutTyping, runningTime, runEnd));
                }
            }

            runCurACVolt = runACVolting;

            runOutPutType = runOutPutTyping;
        }
        /// <summary>
        /// 计算正在ONOFF时序曲线
        /// </summary>
        /// <param name="inputAC"></param>
        /// <param name="runningTime"></param>
        /// <param name="runOnTime"></param>
        /// <param name="runOffTime"></param>
        private void CalOnOffWave(int firstTime, int runningTime, COnOff OnOff, out int runningWave)
        {
            try
            {
                runningWave = 0;

                int runWaveTime = 0;

                int runInputAC = 0;

                if (OnOff.onoffTimes == 0)
                    return;

                if (runningTime > OnOff.onoffTimes - firstTime)
                    runningTime = OnOff.onoffTimes - firstTime;

                while (runningTime > 0)
                {
                    if (OnOff.onTimes == 0)   //全Off时间
                    {
                        runOnOffFlaging = false;
                        runWaveTime = runningTime;
                        runInputAC = 0;
                    }
                    else if (OnOff.offTimes == 0) //全On时间
                    {
                        runOnOffFlaging = true;
                        runWaveTime = runningTime;
                        runInputAC = OnOff.curVolt;
                    }
                    else
                    {
                        if (runOnOffFlaging)  //ON
                        {
                            if (runningTime + firstTime > OnOff.onTimes)
                            {
                                runWaveTime = OnOff.onTimes - firstTime;

                                if (runWaveTime < 0)
                                    runWaveTime = 0;
                            }
                            else
                            {
                                runWaveTime = runningTime;
                            }
                            runInputAC = OnOff.curVolt;
                            runACVolting = OnOff.curVolt;
                        }
                        else                //OFF
                        {
                            if (runningTime > OnOff.offTimes)
                            {
                                runWaveTime = OnOff.offTimes - runningTime;

                                if (runWaveTime < 0)
                                    runWaveTime = 0;
                            }
                            else
                            {
                                runWaveTime = runningTime;
                            }
                            runInputAC = 0;
                            runACVolting = 0;
                        }
                    }

                    if (runWaveTime > 0)
                    {
                        runningTime -= runWaveTime;

                        runningWave += runWaveTime;

                        drawOnOffing(runOnOffFlaging, runInputAC, runWaveTime);
                    }

                    if (runningTime < 0)
                        runningTime = 0;

                    runOnOffFlaging = !runOnOffFlaging;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 刷新曲线
        /// </summary>
        private void OnPaint(object sender, PaintEventArgs e)
        {
            try
            {
                Pen penXY= Pens.DarkGray;

                Pen penGrid = Pens.LightGray;

                Brush brush = Brushes.Black;

                e.Graphics.Clear(Color.White);

                //写X轴单位
                for (int i = 0; i < 11; i++)
                {
                    float x = Xv + (this.Width - Xv) * i / 10;

                    float y = this.Height - Yv;

                    e.Graphics.DrawLine(penGrid, x, y, x, 0);

                    int maxOnOffTime = 0;

                    if (_biTime != 0)
                        maxOnOffTime = (int)(_biTime * 3600);

                    int unitX = maxOnOffTime * i / 600;

                    SizeF size = e.Graphics.MeasureString(unitX.ToString(), new Font("宋体", 10));
                    if (i == 0)
                        e.Graphics.DrawString(unitX.ToString(), new Font("宋体", 10), brush,
                                              x, this.Height - size.Height + 3);
                    else if (i == 10)
                        e.Graphics.DrawString(unitX.ToString(), new Font("宋体", 10), brush,
                                              x - size.Width, this.Height - size.Height + 3);
                    else
                        e.Graphics.DrawString(unitX.ToString(), new Font("宋体", 10), brush,
                                              x - size.Width / 2, this.Height - size.Height + 3);
                }
                //写Y轴单位
                for (int i = 1; i < 4; i++)
                {
                    float x = Xv;
                    float y = (this.Height - Yv) * i / 4;
                    e.Graphics.DrawLine(penGrid, x, y, this.Width, y);
                    int unitY = 0;
                    if (i == 1)
                        unitY = _maxVolt;
                    else if (i == 2)
                        unitY = _maxVolt / 2;
                    SizeF size = e.Graphics.MeasureString(unitY.ToString(), new Font("宋体", 10));
                    if (i != 3)
                        e.Graphics.DrawString(unitY.ToString(), new Font("宋体", 10), brush, -2, y - size.Height / 2);
                    else
                        e.Graphics.DrawString("  0", new Font("宋体", 10), brush, -2, y - size.Height / 2);
                }

                //画X轴坐标
                e.Graphics.DrawLine(penXY, new Point(0, this.Height - Yv), new Point(this.Width, this.Height - Yv));

                //画Y轴坐标
                e.Graphics.DrawLine(penXY, new Point(Xv, this.Height), new Point(Xv, 0));

                InitalUI();

                refreshUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }          
        }
        #endregion

        #region 共享方法
        /// <summary>
        /// 初始化曲线
        /// </summary>
        public void InitalUI()
        {
            try
            {

                if (_biTime == 0 || _onoff.Count == 0)
                    return;

                //防止没设置时间段造成死循环

                int calOnOffTime = 0;

                for (int i = 0; i < _onoff.Count; i++)
                    calOnOffTime += _onoff[i].onoffTimes;
                
                if (calOnOffTime == 0)
                    return;
                
                calOnOffTime = 0;

                for (int i = 0; i < _onoff.Count; i++)
                {
                    calOnOffTime += _onoff[i].onTimes;
                    calOnOffTime += _onoff[i].offTimes;
                }

                if (calOnOffTime == 0)
                    return;

                int totalTime = (int)(_biTime * 3600);

                float waveWith = this.Width - Xv;

                float waveHeight = this.Height - Yv;

                curX = Xv;
                
                curYon = (this.Height - Yv) / 4;
                
                curYoff = (this.Height - Yv) * 3 / 4;
                
                unitTv = waveWith / totalTime;

                int runTime = totalTime;

                runOnOffFlag = true;
                
                raiseOnOff = true;
                
                runStart = false;
                
                do
                {
                    for (int i = 0; i <_onoff.Count; i++)
                    {
                        if (runTime >= _onoff[i].onoffTimes)
                        {
                            calOnOffWave(_onoff[i].curVolt, _onoff[i].onoffTimes, _onoff[i].onTimes, _onoff[i].offTimes);
                            runTime -= _onoff[i].onoffTimes;
                        }
                        else
                        {
                            calOnOffWave(_onoff[i].curVolt, runTime, _onoff[i].onTimes, _onoff[i].offTimes);
                            runTime = 0;
                        }
                        if (runTime < 0)
                            runTime = 0;
                    }

                } while (runTime > 0);
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 启动计时
        /// </summary>
        /// <param name="runTime"></param>
        /// <param name="continueRun"></param>
        public void startRun(int runTime = 0, int iniTime = 0)
        {
            try
            {
                this.iniTime = iniTime;
                runningTime = runTime;
                resetRun = true;
                runEnd = false;
                startTime = DateTime.Now.ToString();
                if (!runWorker.IsBusy)
                    runWorker.RunWorkerAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }
        /// <summary>
        /// 继续计时
        /// </summary>
        public void continuRun()
        {
            try
            {
                startTime = DateTime.Now.ToString();
                runEnd = false;
                resetRun = true;
                if (!runWorker.IsBusy)
                    runWorker.RunWorkerAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

            }
        }
        /// <summary>
        /// 停止计时
        /// </summary>
        public void stopRun()
        {
            try
            {
                if (runWorker.IsBusy)
                    runWorker.CancelAsync();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
            }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 事件定义
        /// </summary>
        public class COnOffArgs : EventArgs
        {
            public COnOffArgs(int curACVolt,int curOutPut, int curRunTime, bool endBI = false)
            {
                this.curACVolt = curACVolt;
                this.curOutPut = curOutPut;
                this.curRunTime = curRunTime;
                this.endBI = endBI;
            }
            public readonly int curACVolt;
            public readonly int curOutPut;
            public readonly int curRunTime;
            public readonly bool endBI;
        }
        public delegate void OnChangeACVoltHandler(object sender, COnOffArgs e);
        public event OnChangeACVoltHandler OnChangeACVolt;
        private void OnChangeACVolted(COnOffArgs e)
        {
            if (OnChangeACVolt != null)
                OnChangeACVolt(this, e);
        }
        #endregion
    }
}
