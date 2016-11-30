using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace 订单管理
{
    public partial class houtai : Form
    {
        DataSet ds1 = new DataSet();
        DataSet ds2 = new DataSet();
        OleDbDataAdapter adapter1;
        OleDbDataAdapter adapter2;
        OleDbDataAdapter adapter3;
        OleDbConnection conn = null;
        public houtai()
        {
            InitializeComponent();
        }

        private void houtai_Load(object sender, EventArgs e)
        {
            initDataSet();
        }

        private void initDataSet()
        {
            OleDbCommandBuilder builder1=null;
            OleDbCommandBuilder builder2 = null;

            OleDbCommandBuilder builder3 = null;
            
            ds1.Clear();
            ds2.Clear();
            try
            {
                conn= new OleDbConnection(Const.connectString);
                adapter1 = new OleDbDataAdapter("select * from caidan", conn);
                builder1= new OleDbCommandBuilder(adapter1);
                builder1.QuotePrefix = "[";
                builder1.QuoteSuffix = "]";
                adapter1.DeleteCommand = builder1.GetDeleteCommand();
                adapter1.InsertCommand = builder1.GetInsertCommand();
                adapter1.UpdateCommand = builder1.GetUpdateCommand();
                adapter1.Fill(ds1, "caidan");


                adapter2 = new OleDbDataAdapter("select * from info", conn);
                builder2 = new OleDbCommandBuilder(adapter2);
                builder2.QuotePrefix = "[";
                builder2.QuoteSuffix = "]";
                adapter2.DeleteCommand = builder2.GetDeleteCommand();
                try
                {
                    adapter2.Fill(ds2, "info");
                }
                catch (Exception)
                {
                    dataGridView2.DataSource = ds1;
                    dataGridView2.DataMember = "caidan";
                    dataGrid2.SetDataBinding(ds2, "info");
                }
                adapter3 = new OleDbDataAdapter("select * from detail", conn);
                builder3 = new OleDbCommandBuilder(adapter3);
                builder3.QuotePrefix = "[";
                builder3.QuoteSuffix = "]";
                adapter3.DeleteCommand = builder3.GetDeleteCommand();
                adapter3.Fill(ds2, "detail");
                try
                {
                    ds2.Relations.Add("详细信息", ds2.Tables["info"].Columns["ID"], ds2.Tables["detail"].Columns["订单信息ID"]);
                }
                catch (Exception)
                {
                    dataGridView2.DataSource = ds1;
                    dataGridView2.DataMember = "caidan";
                    dataGrid2.SetDataBinding(ds2, "info");
                }
            }
            catch (OleDbException e)
            {
                MessageBox.Show("连接数据库失败！");
            }
            
            dataGridView2.DataSource = ds1;
            dataGridView2.DataMember = "caidan";
            dataGrid2.SetDataBinding(ds2, "info");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult a = MessageBox.Show("确定更新订单数据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (a == DialogResult.Yes)
            {
                try
                {
                    adapter1.Update(ds1, "caidan");
                    initDataSet();
                    MessageBox.Show("更新数据库成功！");
                }
                catch (Exception)
                {
                    MessageBox.Show("更新数据库失败！请重试");
                    return;
                }
            }

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0&&e.RowIndex<dataGridView2.Rows.Count-1)
            {
                dataGridView2.Rows.Remove(dataGridView2.Rows[e.RowIndex]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult a = MessageBox.Show("确定清空订单数据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (a == DialogResult.Yes)
            {
                try
                {
                    OleDbConnection conn = new OleDbConnection(Const.connectString);
                    OleDbCommand cmd1 = new OleDbCommand("delete from info", conn);
                    OleDbCommand cmd2 = new OleDbCommand("delete from detail", conn);
                    conn.Open();
                    if (cmd1.ExecuteNonQuery() > 0 && cmd2.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("已清空订单数据！！");
                    }
                    conn.Close();
                    initDataSet();
                }
                catch (Exception)
                {
                    MessageBox.Show("清空订单数据失败！！");
                }
            }
        }

        private void houtai_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Close();
           Program.form.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            initDataSet();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            initDataSet();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult a = MessageBox.Show("确定清空菜单数据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (a == DialogResult.Yes)
            {
                try
                {
                    OleDbConnection conn = new OleDbConnection(Const.connectString);
                    OleDbCommand cmd1 = new OleDbCommand("delete from caidan", conn);
                    conn.Open();
                    if (cmd1.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("已清空菜单数据！！");
                    }
                    conn.Close();
                    initDataSet();
                }
                catch (Exception)
                {
                    MessageBox.Show("清空菜单数据失败！！请重试");
                }
            }
            
        }
    }
}
