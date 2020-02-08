using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GJ.UI
{
    [ToolboxBitmap(typeof(udcChartLine), "Chart_Line.bmp")]
    public partial class udcChartLine : UserControl
    {
        #region 构造函数
        public udcChartLine()
        {
            InitializeComponent();

            SetDoubleBuffered();

            InitialControl();
        }
        public override string ToString()
        {
            return _name;
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 设置双缓冲,防止界面闪烁
        /// </summary>
        private void SetDoubleBuffered()
        {

         
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitialControl()
        {

        }
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = string.Empty;
        private string _title = "温度(℃)";
        private Color _backColor = Color.White;
        private Color _chartColor = Color.Black;
        private float _chartSize = 0;
        private string _valueUnit = "℃";
        private int _lineWidth = 1;
        private Color _lineColor = Color.Red; 
        private int _markWidth = 1;
        private Color _markColor = Color.Red;
        private Color _gridColor = Color.Gray;
        private int _axisX_MinNum = 0;
        private int _axisX_MaxNum = int.MaxValue;
        private int _axisX_InterVal = 1;
        private int _axisX_VisNum = 30;
        private int _axisY_MinNum = 0;
        private int _axisY_MaxNum = 80;
        private int _axisY_InterVal = 10;
        private int _axisY_VisNum = 80;
        /// <summary>
        /// Points数量
        /// </summary>
        private int _count = 0;
        #endregion

        #region 属性
        /// <summary>
        /// 编号ID
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
        /// 类名称
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
        /// 标题名称
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("标题名称")]  
        public string title
        {
            get { return _title; }
            set {
                  _title=value;
                 if (chart1.Series.Count > 0)
                 {
                    chart1.Series[0].Name = _title;
                    chart1.ChartAreas[0].Name = _title; 
                 }                
                }
        }
        /// <summary>
        /// 背景色
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("背景色")]  
        public Color backColor
        {
            get { return _backColor; }
            set {
                 _backColor = value;
                 chart1.BackColor = _backColor;
                }
        }
        /// <summary>
        /// 图表背景色
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("图表背景色")]  
        public Color chartColor
        {
            get { return _chartColor; }
            set {
                  _chartColor=value;
                  if (chart1.Series.Count > 0)
                      chart1.ChartAreas[0].BackColor = _chartColor;
                }
        }
        /// <summary>
        /// 图表占比例(0-100)
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("图表占比例(0-100)")] 
        public float chartSize
        {
            get { return _chartSize; }
            set {
                _chartSize = value;
                if (chart1.Series.Count > 0)
                    chart1.ChartAreas[0].Position.Height = _chartSize;
               }
        }
        /// <summary>
        /// 单位值
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("单位值")] 
        public string valueUnit
        {
            get { return _valueUnit; }
            set {  _valueUnit=value; }
        }
        /// <summary>
        /// 线宽
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("线宽")] 
        public int lineWidth
        {
            get { return _lineWidth; }
            set { 
                 _lineWidth = value;
                 if (chart1.Series.Count > 0)
                     chart1.Series[0].BorderWidth = _lineWidth;
                }
        }
        /// <summary>
        /// 线颜色
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("线颜色")] 
        public Color lineColor
        {
            get { return _lineColor; }
            set { 
                  _lineColor = value;
                  if (chart1.Series.Count > 0)
                      chart1.Series[0].Color = _lineColor;
                }
        }
        /// <summary>
        /// 标记点大小
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("标记点大小")] 
        public int markWidth
        {
            get { return _markWidth; }
            set { 
                  _markWidth = value;
                 if(chart1.Series.Count>0)
                     chart1.Series[0].MarkerBorderWidth = _markWidth;
                }
        }
        /// <summary>
        /// 标记点颜色
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("标记点颜色")] 
        public Color markColor
        {
            get { return _markColor; }
            set {
                 _markColor = value;
                 if (chart1.Series.Count > 0)
                     chart1.Series[0].MarkerColor = _markColor;
                }
        }
        /// <summary>
        /// 网格颜色
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("网格颜色")]
        public Color gridColor
        {
            get { return _gridColor; }
            set {
                 _gridColor = value;
                 if (chart1.Series.Count > 0)
                     chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = _gridColor;
                }
        }
        /// <summary>
        /// X轴可视起始坐标
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("X轴可视起始坐标")]
        public int axisX_MinNum
        {
            get { return _axisX_MinNum; }
            set { 
                  _axisX_MinNum=value;
                  if (chart1.Series.Count > 0)
                      chart1.ChartAreas[0].AxisX.Minimum = _axisX_MinNum;
                }
        }
        /// <summary>
        /// X轴可视结束坐标
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("X轴可视结束坐标")]
        public int axisX_MaxNum
        {
            get { return _axisX_MaxNum; }
            set { 
                  _axisX_MaxNum=value;
                  if (chart1.Series.Count > 0)
                      chart1.ChartAreas[0].AxisX.Maximum = _axisX_MaxNum;
                }
        }
        /// <summary>
        /// X轴可视间隔坐标
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("X轴可视间隔坐标")]
        public int axisX_InterVal
        {
            get { return _axisX_InterVal; }
            set {
                  _axisX_InterVal=value;
                  if (chart1.Series.Count > 0)
                  {
                      chart1.ChartAreas[0].AxisX.MajorGrid.Interval = _axisX_InterVal;
                      chart1.ChartAreas[0].AxisX.Interval = _axisX_InterVal;
                  }
                }
        }
        /// <summary>
        /// X轴可视坐标
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("X轴可视坐标")]
        public int axisX_VisNum
        {
            get { return _axisX_VisNum; }
            set
            {
                _axisX_VisNum=value;
                if (chart1.Series.Count > 0)
                    chart1.ChartAreas[0].AxisX.ScaleView.Size = _axisX_VisNum;//可视区域数据点数 
            }
        }
        /// <summary>
        /// Y轴可视起始坐标
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("Y轴可视起始坐标")]
        public int axisY_MinNum
        {
            get { return _axisY_MinNum; }
            set { 
                  _axisY_MinNum=value;
                  if (chart1.Series.Count > 0)
                      chart1.ChartAreas[0].AxisY.Minimum = _axisY_MinNum;
                }
        }
        /// <summary>
        /// Y轴可视结束坐标
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("Y轴可视结束坐标")]
        public int axisY_MaxNum
        {
            get { return _axisY_MaxNum; }
            set {
                  _axisY_MaxNum = value;
                  if (chart1.Series.Count > 0)
                      chart1.ChartAreas[0].AxisY.Maximum = _axisY_MaxNum;
                }
        }
        /// <summary>
        /// Y轴可视间隔坐标
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("Y轴可视间隔坐标")]
        public int axisY_InterVal
        {
            get { return _axisY_InterVal; }
            set { 
                  _axisY_InterVal=value;
                  if (chart1.Series.Count > 0)
                  {
                      chart1.ChartAreas[0].AxisY.MajorGrid.Interval = _axisY_InterVal;
                      chart1.ChartAreas[0].AxisY.Interval = _axisY_InterVal;
                  }
                }
        }
        /// <summary>
        /// Y轴可视坐标
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("Y轴可视坐标")]
        public int axisY_VisNum
        {
            get { return _axisY_VisNum; }
            set
            {
                _axisY_VisNum = value;
                if (chart1.Series.Count > 0)
                    chart1.ChartAreas[0].AxisY.ScaleView.Size = _axisY_VisNum;//可视区域数据点数 
            }
        }
        /// <summary>
        /// Point数量
        /// </summary>
        [Localizable(false)]
        [Bindable(false)]
        [Browsable(true)]
        [Category("自定义")]
        [Description("Point数量")]
        public int Count
        {
            get { return _count; }
        }
        #endregion

        #region 私有方法
        private void chart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = string.Format("{1}{2}", dp.XValue, dp.YValues[0],_valueUnit);
            }
        }
        #endregion

        #region 共享方法
        /// <summary>
        /// 初始化
        /// </summary>
        public void Initial()
        {
            chart1.Series.Clear();

            chart1.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart_GetToolTipText);

            Series s = new Series();
            //图形名称
            s.Name = _title;
            //图形类型
            s.ChartType = SeriesChartType.Spline;
            //线条颜色
            s.Color = _lineColor;
            //线条粗细
            s.BorderWidth = _lineWidth;
            //标记点边框颜色      
            s.MarkerBorderColor = _markColor;
            //标记点边框大小
            s.MarkerBorderWidth = _markWidth;
            //标记点中心颜色
            s.MarkerColor = _markColor;
            //标记点大小
            s.MarkerSize = _markWidth;
            //标记点类型     
            s.MarkerStyle = MarkerStyle.Circle;
            //将文字移到外侧            
            s["PieLabelStyle"] = "Outside";
            //绘制黑色的连线
            s["PieLineColor"] = "Black";           
            //s.Label = "#VAL";
            //s.IsValueShownAsLabel = true;
            chart1.Series.Add(s);

            //背景设置 
            chart1.BackColor = _backColor;            
            chart1.ChartAreas[0].BackColor =_chartColor;

            //X轴
            chart1.ChartAreas[0].AxisX.Title = "";
            chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Horizontal;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = _gridColor;
            chart1.ChartAreas[0].AxisX.TitleForeColor = Color.Green;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = _axisX_InterVal; //设置图表网格 
            chart1.ChartAreas[0].AxisX.Interval = _axisX_InterVal;  //设置横坐标间隔为1，使得每个刻度间均匀分开的。
            chart1.ChartAreas[0].AxisX.Minimum = _axisX_MinNum;//X轴起始点 
            chart1.ChartAreas[0].AxisX.Maximum = _axisX_MaxNum;//X轴结束点    
            chart1.ChartAreas[0].AxisX.ScaleView.Size = _axisX_VisNum;//可视区域数据点数 

            //Y轴
            chart1.ChartAreas[0].AxisY.Title = "";
            chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Horizontal;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = _gridColor;
            chart1.ChartAreas[0].AxisY.TitleForeColor = Color.Green;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.MajorGrid.Interval = _axisY_InterVal; //设置图表网格 
            chart1.ChartAreas[0].AxisY.Interval = _axisY_InterVal;  //设置横坐标间隔为1，使得每个刻度间均匀分开的。
            chart1.ChartAreas[0].AxisY.Minimum = _axisY_MinNum;//X轴起始点 
            chart1.ChartAreas[0].AxisY.Maximum = _axisY_MaxNum;//X轴结束点
            chart1.ChartAreas[0].AxisY.ScaleView.Size = _axisY_VisNum;//可视区域数据点数 

            //滚动条            
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 12;
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            chart1.ChartAreas[0].AxisX.ScrollBar.BackColor = Color.White;
            chart1.ChartAreas[0].AxisX.ScrollBar.LineColor = Color.Black;
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.FromArgb(224, 224, 224);           
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
          
            //放大与缩小           
            //chart1.ChartAreas[0].CursorX.IsUserEnabled = true;  //设置坐标轴可以用鼠标点击放大，可以看到更小的刻度
            //chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true; //用户可以选择从那里放大
            //chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All; // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
         
            //// 设置自动放大与缩小的最小量
            //chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            //chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 2;

            //图形位置
            if (_chartSize == 0)
                _chartSize = (float)((chart1.Height - 45) * 100) / (float)chart1.Height; 
            chart1.ChartAreas[0].Position.Auto = false;
            chart1.ChartAreas[0].Position.X = 0;
            chart1.ChartAreas[0].Position.Y = 0;
            chart1.ChartAreas[0].Position.Height = _chartSize;
            chart1.ChartAreas[0].Position.Width = 100;

            //标题栏位置设置
            chart1.Legends[0].Alignment = StringAlignment.Far;
            chart1.Legends[0].BackColor = Color.Transparent;
            chart1.Legends[0].Docking = Docking.Bottom;

            _count = 0;

        }
        /// <summary>
        /// 刷新XY轴数据
        /// </summary>
        /// <param name="xy"></param>
        public void BindXY(List<double> x, List<double> y)
        {
            try
            {
                if (chart1.Series.Count == 0)
                    return;

                chart1.Series[0].Points.Clear();

                chart1.Series[0].Points.DataBindXY(x,y);

                _axisX_MaxNum = _count+1;

                chart1.ChartAreas[0].AxisX.Maximum = _axisX_MaxNum;//X轴结束点 
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        /// <summary>
        /// 增加XY点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddXY(double x, double y)
        {
            try
            {
                 if (chart1.Series.Count == 0)
                    return;

                 _count++;

                 chart1.Series[0].Points.AddXY(x, y);

                 if (_count > chart1.ChartAreas[0].AxisX.ScaleView.Size)
                 {
                     chart1.ChartAreas[0].AxisX.ScaleView.Position = _count - chart1.ChartAreas[0].AxisX.ScaleView.Size;
                 }

                 _axisX_MaxNum = _count+1;

                 chart1.ChartAreas[0].AxisX.Maximum = _axisX_MaxNum;//X轴结束点 
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        #endregion

    }
}
