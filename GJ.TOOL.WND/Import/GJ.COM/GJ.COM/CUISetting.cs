using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace GJ.COM
{
    public class CUISetting
    {
        /// <summary>
        /// 设置控件双缓存
        /// </summary>
        /// <param name="ctrlUI"></param>
        public static void SetUIDoubleBuffered(Control control)
        {
            try 
	        {
                if (control.GetType().ToString() == "System.Windows.Forms.TableLayoutPanel" ||
                    control.GetType().ToString() == "System.Windows.Forms.Panel"
                    )
                {
                    control.GetType().GetProperty("DoubleBuffered",
                                                  System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.NonPublic)
                                                  .SetValue(control, true, null);
                }

                foreach (Control c in control.Controls)
                {
                    SetUIDoubleBuffered(c);
                }
	        }
	        catch (Exception)
            {
		
		        throw;
	        }
        }
    }
}
