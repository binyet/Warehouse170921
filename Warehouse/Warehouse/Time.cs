using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Warehouse
{
    public partial class Time : Form
    {
        public int change = 0;
        public Time()
        {
            InitializeComponent();
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            

        }
        /// <summary>
        /// 根据BinName找BinID
        /// </summary>
        /// <returns></returns>
        private string selectID(string str)
        {
            string sql = "select * from [bininfo] where [BinName] = '" + str + "'";
            try
            {
                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;

                string ret = "";
                db.Dr = db.command.ExecuteReader();
                while (db.Dr.Read())
                {
                    ret = db.Dr["BinID"].ToString();
                }
                db.Dr.Close();
                return ret;
            }
            catch (SqlException se)
            {
                string message_error = se.ToString();
                string path = System.Windows.Forms.Application.StartupPath;
                FileStream fs = new FileStream(path + "\\log.txt", FileMode.Create | FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("错误信息是：" + message_error + " 时间是：" + DateTime.Now.ToString());
                sw.Flush();
                sw.Close();
                fs.Close();
                MessageBox.Show("定时盘库查询料仓地址时数据库连接失败","提示");
                return "";
            }
            
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            if (checkBox1.CheckState == CheckState.Checked)
            {
                if (textBox1.Text.Equals(""))
                {
                    MessageBox.Show("请输入日期","提示");
                }
                else
                {
                    try
                    {
                        int date = Int32.Parse(textBox1.Text);
                        if(date > 31||date< 1)
                        {
                            MessageBox.Show("请输入有效的日期","提示");
                            return;
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("请输入有效的日期", "提示");
                        return;
                    }
                    string t_str = comboBox1.Text.PadLeft(2, '0') +":"+comboBox2.Text.PadLeft(2, '0')+":00";
                    try
                    {
                        int hour_int = Int32.Parse(comboBox1.Text);
                        int min_int = Int32.Parse(comboBox2.Text);
                        if (hour_int >= 24 || hour_int < 0 || min_int >= 60 || min_int < 0)
                            MessageBox.Show("请检查时间输入格式","提示");
                        else
                        {
                            if (comboBox4.Text.Equals("料仓盘库"))
                            {
                                Add_time(selectID(comboBox3.Text), t_str, textBox1.Text, comboBox3.Text, "料仓盘库");
                            }
                            else if (comboBox4.Text.Equals("镜头除尘"))
                            {
                                Add_time(selectID(comboBox3.Text), t_str, textBox1.Text, comboBox3.Text, "镜头除尘");
                            }
                            else if (comboBox4.Text.Equals("镜头除湿"))
                            {
                                Add_time(selectID(comboBox3.Text), t_str, textBox1.Text, comboBox3.Text, "镜头除湿");

                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show("请检查时间输入格式","提示");
                    }
                }
            }
            else
            {
                string t_str = comboBox1.Text.PadLeft(2, '0')+ ":" + comboBox2.Text.PadLeft(2, '0') + ":00";
                try
                {
                    int hour_int = Int32.Parse(comboBox1.Text);
                    int min_int = Int32.Parse(comboBox2.Text);
                    if (hour_int >= 24 || hour_int < 0 || min_int >= 60 || min_int < 0)
                        MessageBox.Show("请检查时间输入格式", "提示");
                    else
                    {
                        if (comboBox4.Text.Equals("料仓盘库"))
                        {
                            Add_time(selectID(comboBox3.Text), t_str, "0", comboBox3.Text, "料仓盘库");
                        }
                        else if (comboBox4.Text.Equals("镜头除尘"))
                        {
                            Add_time(selectID(comboBox3.Text), t_str, "0", comboBox3.Text, "镜头除尘");
                        }
                        else if (comboBox4.Text.Equals("镜头除湿"))
                        {
                            Add_time(selectID(comboBox3.Text), t_str, "0", comboBox3.Text, "镜头除湿");

                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show("请检查时间输入格式", "提示");
                }
            }
        }

        /// <summary>
        /// type标志是盘库还是清洁镜头
        /// 1表示盘库
        /// 2表示镜头除尘
        /// 3表示镜头除湿
        /// </summary>
        /// <param name="id"></param>
        /// <param name="time"></param>
        /// <param name="date"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        private void Add_time(string id, string time, string date, string name, string type)
        {
            
            try
            {
                DataBase db_select = new DataBase();
                string sql_select = "select * from [binauto] where [BinID] = "+id;
                db_select.command.CommandText = sql_select;
                db_select.command.Connection = db_select.connection;
                db_select.Dr = db_select.command.ExecuteReader();
                while (db_select.Dr.Read())
                {
                    if (db_select.Dr["Time"].ToString().Equals(time))
                    {
                        MessageBox.Show("此料仓在这个时间已经设置了定时功能","提示");
                        return;
                    }
                }
            
            }
            catch (FormatException fexc)
            {
                MessageBox.Show("请查看时间格式或关闭定时窗口重新打开", "提示");
            }
            catch (SqlException sec)
            {
                MessageBox.Show("请相关人员检查数据库设置","提示");
            }

            string sql = "insert into [binauto] values("+id+", '"+time+"',"+date+", '"+name+"', '"+type+"')";
            try
            {
                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                try
                {
                    if (db.command.ExecuteNonQuery() > 0)
                    {
                        change = 1;
                        MessageBox.Show("定时 "+comboBox4.Text+" 时间添加成功", "提示");
                    }

                }
                catch (FormatException exc)
                {
                    MessageBox.Show("请输入正确的时间","提示");
                }
            }
            catch (SqlException se)
            {
                string message_error = se.ToString();
                string path = System.Windows.Forms.Application.StartupPath;
                FileStream fs = new FileStream(path + "\\log.txt", FileMode.Create | FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("错误信息是：" + message_error + " 时间是：" + DateTime.Now.ToString());
                sw.Flush();
                sw.Close();
                fs.Close();
                MessageBox.Show("定时盘库添加时间时数据库连接失败","提示");
            }
            
        }

        private void Time_Load(object sender, EventArgs e)
        {
            comboBox4.Text = comboBox4.Items[0].ToString();
            DataBase db = new DataBase();
            string sql = "select * from [bininfo]";
            db.command.CommandText = sql;
            db.command.Connection = db.connection;
            db.Dr = db.command.ExecuteReader();
            while (db.Dr.Read())
            {
                comboBox3.Items.Add(db.Dr["BinName"].ToString());
            }
            db.Dr.Close();
            db.Close();
            if (comboBox3.Items.Count > 0)
            {
                comboBox3.Text = comboBox3.Items[0].ToString();
            }
        }


        /// <summary>
        /// 停止按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
            }
        }

        private void Time_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Dispose();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

    }
}
