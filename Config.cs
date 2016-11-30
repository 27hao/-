using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace 订单管理
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            string [] config=Const.readConfig();
            if ("0".Equals(config[4]))
            {
                textBox1.Text =Const.DecodeBase64(config[0]);
                textBox2.Text =Const.DecodeBase64(config[1]);
                textBox3.Text = Const.DecodeBase64(config[2]);
                textBox4.Text = Const.DecodeBase64(config[3]);
            }
            else
            {
                MessageBox.Show("读取配置文件失败！请检查");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] config = new string[] { Const.EncodeBase64(textBox1.Text), Const.EncodeBase64(textBox2.Text), Const.EncodeBase64(textBox3.Text), Const.EncodeBase64(textBox4.Text) };
            if (Const.writeConfig(config))
            {
                MessageBox.Show("写入配置文件成功！");
            }
            else
            {
                MessageBox.Show("写入配置文件失败，请重试！");
            }
        }
    }
}
