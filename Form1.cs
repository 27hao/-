using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Drawing.Printing;
namespace 订单管理
{

    public partial class Form1 : Form
    {
        int pageSize = 12;//每页数量
        int pageCount = 0;//页的数量
        int currentPage = 0;//当前页
        int count = 0;//数据量
        DataSet ds=new DataSet();
        DataTable dtInfo = new DataTable("caidan");

        public Form1()
        {
            InitializeComponent();
        }

        private void initData(string sqlStr)
        {
            ds.Clear();
            dtInfo.Clear();
            try
            {
                OleDbConnection conn = new OleDbConnection(Const.connectString);
                OleDbDataAdapter adapter = new OleDbDataAdapter(sqlStr, conn);
                adapter.Fill(ds, "caidan");
                dtInfo = ds.Tables[0];
            }catch(Exception e){
                MessageBox.Show("连接错误!请检查数据库!");
            }
            count = dtInfo.Rows.Count;
            pageCount = count % pageSize == 0 ? count / pageSize : count / pageSize+1;
            currentPage = 1;  
        }

        private void loadData()
        {
            DataTable dt = dtInfo.Copy();
            dt.Clear();
            for (int i = 0; i <pageSize; i++)
            {
                if ((currentPage-1) * pageSize + i < count)
                {
                    DataRow newdr = dt.NewRow();
                    DataRow dr = dtInfo.Rows[(currentPage-1) * pageSize + i];
                    foreach (DataColumn column in dt.Columns)
                    {
                        newdr[column.ColumnName] = dr[column.ColumnName];
                    }
                    // newdr = dr;
                    dt.Rows.Add(newdr);
                }
            }
            this.dataGridView1.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Const.dianName =Const.DecodeBase64( Const.readConfig()[0]);
            Const.userName = Const.DecodeBase64(Const.readConfig()[1]);
            Const.address = Const.DecodeBase64(Const.readConfig()[2]);
            Const.call = Const.DecodeBase64(Const.readConfig()[3]);
            Rectangle rect = Screen.GetWorkingArea(this);
            Point p = new Point((rect.Width - this.Size.Width) / 2, (rect.Height - this.Size.Height) / 2);
            this.Location = p;
            initData("select *  from caidan");
            loadData();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listView1.Items.Remove(listView1.SelectedItems[0]);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != 3)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                DataGridViewCell cell = row.Cells[e.ColumnIndex];
                string[] data = new string[]{
                row.Cells[0].Value+"",
                (string)row.Cells[1].Value,
                row.Cells[2].Value+"",
                row.Cells[3].Value+"",
                Convert.ToDouble(row.Cells[2].Value)*Convert.ToDouble(row.Cells[3].Value)+""
                };
                listView1.Items.Add(new ListViewItem(data));
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar > '0' && e.KeyChar < '9'||e.KeyChar==8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter&&!textBox1.Text.Equals(""))
            {
                int num = Convert.ToInt32(textBox1.Text);
                if (num <= pageCount)
                {
                    currentPage = num;
                    loadData();
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            textBox1.Text = "1";
            loadData();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                currentPage = Convert.ToInt32(textBox1.Text) - 1;
                if (currentPage < 1)
                {
                    currentPage = 1;
                }
                textBox1.Text = currentPage + "";
                loadData();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Equals(""))
            {
                currentPage = Convert.ToInt32(textBox1.Text) + 1;
                if (currentPage > pageCount)
                {
                    currentPage = pageCount;
                }
                textBox1.Text = currentPage + "";
                loadData();
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            currentPage = pageCount;
            textBox1.Text = pageCount+ "";
            loadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox2.Text;
            str = Regex.Replace(str, @"['%;=&|!#^*()<>?~]", "");
            initData("select *  from caidan where name like '%"+str+"%'");
            loadData();
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            initData("select *  from caidan");
            loadData();
        }


        int row = 0;//记录点击当前行
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3&&e.RowIndex>=0)
            {
                numericUpDown1.Value =Convert.ToInt32( dataGridView1.Rows[e.RowIndex].Cells[3].Value);
                numericUpDown1.Visible = true;
                numericUpDown1.Size = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Size;
                int cellLeft = dataGridView1.Left + 2;
                for (int i = this.dataGridView1.FirstDisplayedCell.ColumnIndex; i < e.ColumnIndex; i++)
                {
                    cellLeft = cellLeft + this.dataGridView1.Columns[i].Width;

                }
                int cellTop = dataGridView1.Top + 22;
                for (int j = this.dataGridView1.FirstDisplayedCell.RowIndex; j < e.RowIndex; j++)
                {
                    cellTop = cellTop + this.dataGridView1.Rows[j].Height;
                }
                numericUpDown1.Location = new Point(cellLeft, cellTop);
                row = e.RowIndex%pageSize;
            }
            else
            {
                numericUpDown1.Visible = false;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows[row].Cells[3].Value = numericUpDown1.Value;
        }

        private void 配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Config().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                try
                {
                    printDocument1.Print();
                    insertData();
                }
                catch (InvalidPrinterException)
                {
                    MessageBox.Show("请确保打印机存在且正确安装！");
                }
            }
            else
            {
                MessageBox.Show("订单里为空！！请选择商品");
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void insertData()
        {
            OleDbConnection conn=null;
            try
            {
                conn = new OleDbConnection(Const.connectString);
                conn.Open();
                Decimal total =new Decimal();
                foreach (ListViewItem item in listView1.Items)
                {
                    total += Convert.ToDecimal(item.SubItems[4].Text);
                }
                OleDbCommand cmd1 = new OleDbCommand("insert into info(收益,时间) values(" + total + ",'" + DateTime.Now.ToString() + "')", conn);
                if (cmd1.ExecuteNonQuery() > 0)
                {
                    cmd1 = new OleDbCommand("select MAX(ID) from info", conn);
                    int id = (int)cmd1.ExecuteScalar();
                    foreach (ListViewItem item in listView1.Items)
                    {
                        cmd1 = new OleDbCommand("insert into detail(菜名,单价,数量,订单信息ID) values('" + item.SubItems[1].Text + "','" + item.SubItems[2].Text + "','" + item.SubItems[3].Text + "','" + id + "')", conn);
                        cmd1.ExecuteNonQuery();
                    }
                }
            }
            catch (OleDbException)
            {
                MessageBox.Show("连接数据库错误！");
            }finally{
                conn.Close();
            }
        }
        private void 后台管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            houtai h=new houtai();
            h.Location = this.Location;
            h.Show();
            this.Visible = false;
        }


        public string GetPrintStr()
        {

            StringBuilder sb = new StringBuilder();

            string tou = Const.dianName;

            string address = "xxx";

            int count = 0;

            decimal total = 0.00M;
            int i = 1;
            string call = Const.call;

            sb.Append("\t\t" + tou + "\t\n");

            sb.Append("\n");

            sb.Append("日期:\t" + DateTime.Now.ToString()+"\n");

            sb.Append("------------------------------------------------------------------\n");

            sb.Append("序号\t" + "菜名\t\t"+ "单价\t" + "数量\t" + "小计" + "\n");
            foreach (ListViewItem item in listView1.Items)
            {
                    total += Convert.ToDecimal(item.SubItems[4].Text);
                    count += Convert.ToInt32(item.SubItems[3].Text);
                    sb.Append(i + "\t"+ item.SubItems[1].Text+ "\t\t" + item.SubItems[2].Text + "\t" + item.SubItems[3].Text + "\t"+item.SubItems[4].Text+"\n");
                    i++;
            }

            sb.Append("\n");

            sb.Append("数量: " + count +"\t"+ " 合计:   " + total + "元\n");

            sb.Append("------------------------------------------------------------------\n");

            sb.Append("电话：" + call + "\n");

            sb.Append("地址：" + address + "\n");

            sb.Append("                   谢谢惠顾欢迎下次光临                    ");
            /*
            Font drawFont = new Font("Arial", 9);
            float x = 3; float y = 3;
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            Bitmap b = new Bitmap(400, 600);
            this.pictureBox1.Image = b;
            Graphics gra = Graphics.FromImage(this.pictureBox1.Image);
            gra.DrawString(sb.ToString(), drawFont, drawBrush, x, y);
            this.pictureBox1.Refresh();
            */
            return sb.ToString();

        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            Font drawFont = new Font("Arial", 9);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            float x = 3; float y = 3;
            e.Graphics.DrawString(GetPrintStr(), drawFont, drawBrush, x, y);
            
        }

    } 
}
