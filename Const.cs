using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace 订单管理
{
    class Const
    {
        public static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=AppData\\dingdan.accdb;Persist Security Info=False;";
        public static string userName = "";
        public static string dianName = "";
        public static string call = "";
        public static string address="";

        public static bool writeConfig(string[] config)
        {
            StreamWriter write = null;
            try
            {
                write = new StreamWriter("config\\config.cfg");
                foreach (string str in config)
                {
                    write.WriteLine(str);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                write.Close();
            }
            return true;
        }

        public static string[] readConfig()
        {
            string[] config = new string[] { "", "", "", "", "0" };
            StreamReader read = null;
            try
            {
                read = new StreamReader("config\\config.cfg");
                config[0] = read.ReadLine();
                config[1] = read.ReadLine();
                config[2] = read.ReadLine();
                config[3] = read.ReadLine();
            }
            catch (Exception e)
            {
                config[4] = "1";
                return config;
            }
            finally
            {
                read.Close();
            }
            return config;
        }
        //编码
        public static string EncodeBase64( string code)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(code);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = code;
            }
            return encode;
        } 
        //解码
        public static string DecodeBase64( string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = Encoding.GetEncoding("utf-8").GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        } 

    }
}
