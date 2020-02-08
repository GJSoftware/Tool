using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing.PieChart;

namespace GJ.UI
{
    [ToolboxBitmap(typeof(udcChartPie), "Chart_Pie.bmp")]
    public partial class udcChartPie : UserControl
    {
        #region 构造函数
        public udcChartPie()
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
            try
            {
                _pieChart = new PieChartControl();

                _pieChart.LeftMargin = 2;
                _pieChart.RightMargin = 2;
                _pieChart.TopMargin = 2;
                _pieChart.BottomMargin = 2;

                _pieChart.ShadowStyle = ShadowStyle.GradualShadow;
                _pieChart.EdgeColorType = EdgeColorType.DarkerDarkerThanSurface;
                _pieChart.EdgeLineWidth = 1;
                _pieChart.FitChart = true;
                _pieChart.SliceRelativeHeight = 0.12f;
                _pieChart.InitialAngle = -30.0f;
                _pieChart.EdgeLineWidth = 1;
                _pieChart.Font = new Font("宋体", 10F);
                _pieChart.ForeColor = SystemColors.WindowText;

                ArrayList _colors = new ArrayList();

                _colors.Add(Color.FromArgb(120, Color.LimeGreen));

                _colors.Add(Color.FromArgb(120, Color.Red));

                _pieChart.Colors = (Color[])_colors.ToArray(typeof(Color));

                float[] _offset = new float[] { 0f, 0.15f };

                _pieChart.SliceRelativeDisplacements = _offset;

                _pieChart.Dock = DockStyle.Fill;

                _pieChart.Margin = new Padding(0);

                this.Controls.Add(_pieChart);

                SetValue(0, 0); 

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region 字段
        private int _idNo = 0;
        private string _name = string.Empty;
        private PieChartControl _pieChart = null;
        #endregion

        #region 属性
        public int idNo
        {
            get { return _idNo; }
            set { _idNo = value; }
        }
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="ttNum"></param>
        /// <param name="failNum"></param>
        public void SetValue(int ttNum, int failNum)
        {

            int _passNum = ttNum - failNum;
            int _ttNum = ttNum;

            if(ttNum==0)
            {
               _ttNum = 100;
               _passNum=100;
               failNum=0;
            }

            decimal[] _value = new decimal[] { _passNum, failNum };

            string[] _text = new string[]{  
                                         ((double)_passNum/(double)_ttNum).ToString("P1"), 
                                         ((double)failNum/(double)_ttNum).ToString("P1")
                                          };
            string[] _tip = new string[]{
                                         string.Format("良品数/总数:{0}/{1}",ttNum - failNum,ttNum),
                                         string.Format("不良数/总数:{0}/{1}",failNum,ttNum),
                                        };
            _pieChart.Values = _value;

            _pieChart.Texts = _text;

            _pieChart.ToolTips = _tip;
        
        }
        #endregion

    }
}
