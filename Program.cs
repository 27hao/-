﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 订单管理
{
    static class Program
    {
        public static Form1 form;
        
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(form = new Form1());
        }
    }
}
