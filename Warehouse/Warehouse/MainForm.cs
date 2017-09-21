using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Drawing;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Update;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;


namespace Warehouse
{
    public partial class MainForm : Form
    {

        public static int TIME = 2;//向状态表中添加的时间
        private static int TIME_WAIT = 2;//半双工等待时间
        private static int s_Produce = 3600;//时间戳时间

        Users curr_user = new Users();
        Time f = new Time();//不适用动态创建的原因是：添加定时功能时间的时候需要使用它的一个变量

        public TransCoding Data = new TransCoding();
        private int pageSize = 0;     //每页显示行数
        private int nMax = 0;         //总记录数
        private int pageCount = 0;    //页数＝总记录数/每页显示行数
        private int pageCurrent = 0;   //当前页号
        private int nCurrent = 0;      //当前记录行
        DataSet ds = new DataSet();
        DataTable dtInfo = new DataTable();

        private CircularQueue<string> cirQueue = new CircularQueue<string>(3000);//循环队列，用于处理接收到的数据
        private List<FacMessage> list_status = new List<FacMessage>();//将回应信息加入到这个链表中
        private System.Timers.Timer t_monitor;//用于监控料仓时定时向中控发送请求获取料的高度的定时器
        System.Media.SoundPlayer player = new System.Media.SoundPlayer();
        private int alarm = 0;//超过预警阈值的料仓数量
        private int SqlConnect = 0;//判断是否连接上数据库
        private int ins_num = 1;//用于记录指令状态的指令序号
        private Mutex list_mutex = new Mutex();//用于更改链表时进行异步操作的互斥锁

        private Mutex file_mutex = new Mutex();//文件互斥锁
        private string backdata_path = "";//回传数据的存储路径

        private List<FacMessage> send_ins = new List<FacMessage>();//盘库时，每30秒发送一次查询状态函数，这个链表标记哪个料仓需要查询
        private List<FacMessage> searchdata_ins = new List<FacMessage>();//存放查询盘库结果指令

        private string writeToFile_buffer = "";//回传数据缓冲区
        private int recv_num = 0;//接收到回传数据的个数

        private string adddress = "";//记录查询指令是否接收到

        /*定义一个发送指令队列oper_ins，主要应用于盘库、监控、清洁镜头指令的发送检测
         * 用法：点击盘库（清洁镜头，监控）按钮后，先发送查询指令
         * 但是程序不能等待查询指令回复之后再做出相应的操作
         * 因此，只能将盘库（清洁镜头、监控）指令添加到循环队列中，而不发送
         * 当查询指令回复结果后，根据回复信息，进行下一步操作
         * 例如，若接收到“料仓无操作”信息，则在此队列中找出关于这个料仓要进行何种操作的指令，发送出去
         * 若接收到“料仓正忙”信息，则提示用户料仓的当前状态，并将这个队列中关于这个料仓的所有的操作指令删除
         * 此队列中，仅包含监控、清洁镜头、监控这三类指令
         */
        private Queue<FacMessage> oper_ins = new Queue<FacMessage>();

        //指令发送队列
        private Queue<FacMessage> sendIns_queue = new Queue<FacMessage>();

        //目的指令缓冲区
        private Queue<FacMessage> aim_ins = new Queue<FacMessage>();//将查询指令的目的指令放入这个缓冲区中，发送完查询指令后将目的指令放入oper_ins中

        private System.Timers.Timer t_status;//用于遍历状态链表的定时器

        private List<Clean> clean_list = new List<Clean>();//用于存放哪些料仓正在清洁镜头

        private bool isplaying = false;//判断是否正在播放音乐

        private int port_mask = 0;//屏蔽串口

        private BackData[] backdata = new BackData[200];//回传数据
        private int back_complet = -1;//是否回传完成

        private int port_isopen = 0;

        static private Form_Login form_login = null;

        private int flag_threadout = 1;//退出登录时，将这个标识改为0，所有的线程将阻塞

        private int it_oper = 0;//盘库列表的索引
        private List<FacMessage> CalcVol_list = new List<FacMessage>();//正在盘库链表

        private List<OperMsg> Auto_list = new List<OperMsg>();//定时链表
        private int timer1_mask = 0;

        private List<FacMessage> NoAckList = new List<FacMessage>();//记录未接收到的指令次数,其中的节点存两个信息，料仓编号和未接收到指令的次数

        private int msgBoxNum = 0;//MessageBox编号，每弹出一个MessageBox自增1，大于2000变为0

        public MainForm(string name, string admin, Form_Login frm)
        {
            InitializeComponent();
            curr_user.name = name;
            curr_user.admin = admin;
            form_login = frm;
        }


        /// <summary>
        /// 软件载入时初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {//打开这个窗体时执行
            Console.WriteLine("fdsfdsf");
            label7.Text = curr_user.name;
            if (curr_user.admin.Equals("1"))
            {//根据用户权限来处理空间的显示或者隐藏
                label7.ForeColor = Color.Red;
                普通用户管理ToolStripMenuItem.Visible = true;
                管理员密码修改ToolStripMenuItem.Visible = true;
                test2ToolStripMenuItem.Visible = true;
                testToolStripMenuItem.Visible = true;
                添加料仓ToolStripMenuItem.Visible = true;
                更改名称ToolStripMenuItem.Visible = true;
                串口选择ToolStripMenuItem.Visible = true;
                软件升级ToolStripMenuItem.Visible = true;
                服务器设置ToolStripMenuItem.Visible = true;
                数据库管理ToolStripMenuItem.Visible = true;
                开启回传数据ToolStripMenuItem.Visible = true;
                获取基础信息ToolStripMenuItem.Visible = true;
                直接查询ToolStripMenuItem.Visible = false;
            }
            else
            {
                label7.ForeColor = Color.Black;
                普通用户管理ToolStripMenuItem.Visible = false;
                管理员密码修改ToolStripMenuItem.Visible = true;
                test2ToolStripMenuItem.Visible = false;
                testToolStripMenuItem.Visible = false;
                添加料仓ToolStripMenuItem.Visible = false;
                更改名称ToolStripMenuItem.Visible = false;
                串口选择ToolStripMenuItem.Visible = false;
                软件升级ToolStripMenuItem.Visible = false;
                服务器设置ToolStripMenuItem.Visible = false;
                数据库管理ToolStripMenuItem.Visible = false;
                开启回传数据ToolStripMenuItem.Visible = false;
                获取基础信息ToolStripMenuItem.Visible = false;
                直接查询ToolStripMenuItem.Visible = false;
            }

            string contosql = conTosql();
            if (contosql.Equals("1"))
            {//检测到数据库中的表已经创建齐全
                SqlConnect = 1;
            }
            else
            {//创建未创建的数据表
                Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                thread_file.Start("数据表没有创建齐全" + contosql.ToString());

            }
            string path = System.Windows.Forms.Application.StartupPath;
            if (File.Exists(path + "\\serialPort.txt") == false)//创建保存串口信息的文件
                File.Create(path + "\\serialPort.txt").Close();

            StreamReader sr = new StreamReader(path + "\\serialPort.txt", Encoding.Default);
            String line = sr.ReadLine();
            if (line == null)
            {
                //MessageBox.Show("请先设置串口", "提示");
                new Thread(new ParameterizedThreadStart(showBox)).Start("请先设置串口");

                new Thread(display_noport).Start();
            }
            else
            {
                string[] serial = line.Split('+');//文件中串口信息按照"+"分隔
                try
                {
                    //this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
                    serialPort1.PortName = serial[0];
                    //this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
                    serialPort1.BaudRate = int.Parse(serial[1]);
                    serialPort1.Open();
                    port_isopen = 1;


                    //刷新ToolStripMenuItem.PerformClick();
                }
                catch (Exception exc)
                {
                    new Thread(new ParameterizedThreadStart(showBox)).Start("串口设置已失效，请重新设置");
                }

                Thread thread_takeData = new Thread(takeData);//解析数据
                thread_takeData.Start();
                if (port_isopen == 1)
                {//串口正常
                    Thread load_fac = new Thread(OpenMainForm);//启动时显示料仓线程
                    load_fac.Start();
                }
                else
                {
                    new Thread(display_noport).Start();
                }
            }
            sr.Close();
            //新建监控窗口，但是不显示
            monitor = new Monitor();


            //asc.controllInitializeSize(this);

            t_monitor = new System.Timers.Timer(30000);//定时器，每隔30秒向中控获取监控获得的高度值
            //这个定时器常开，打开以后不关闭
            t_monitor.Elapsed += new System.Timers.ElapsedEventHandler(inquire_height);
            t_monitor.AutoReset = true;
            t_monitor.Enabled = false;

            try
            {//检测数据库是否可以连接
                DataBase db = new DataBase();
                SqlConnect = 1;
            }
            catch (SqlException se)
            {
                Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                thread_file.Start(se.ToString());
                new Thread(new ParameterizedThreadStart(showBox)).Start("数据库连接异常\r\n");

            }

            if (SqlConnect == 1)
            {
                t_status = new System.Timers.Timer(1000);//状态链表使用的定时器，判断是否超时
                t_status.Elapsed += new System.Timers.ElapsedEventHandler(getStatus);
                t_status.AutoReset = true;
                t_status.Enabled = true;
                timer1.Enabled = true;//清洁镜头时间的定时器，默认关闭，数据库可以连接时打开

                Thread sendins_thread = new Thread(SendIns);//发送指令线程
                sendins_thread.Start();


                //加载定时信息线程
                Thread loadAuto = new Thread(LoadAuto);
                loadAuto.Start();

                //new Thread(StartKiller).Start();

            }

        }

        /// <summary>
        /// 弹出MessageBox，在线程中执行，避免阻塞当前线程
        /// </summary>
        /// <param name="obj"></param>
        private void showBox(object obj)
        {
            string message = (string)obj;
            if (msgBoxNum >= 2000)
            {
                msgBoxNum = 0;
            }
            MyMessageBox MymsgBox = new MyMessageBox(message, (msgBoxNum++).ToString());
            MymsgBox.ShowBox();
        }

        /// <summary>
        /// 加载定时信息
        /// </summary>
        /// <param name="obj"></param>
        private void LoadAuto(object obj)
        {
            timer1_mask = 1;
            Auto_list.Clear();
            string sql = "select * from [binauto]";
            try
            {
                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                db.Dr = db.command.ExecuteReader();
                while (db.Dr.Read())
                {
                    OperMsg msg = new OperMsg(db.Dr["BinID"].ToString(), db.Dr["Time"].ToString(), db.Dr["Date"].ToString(), db.Dr["Operation"].ToString(), 0);
                    Auto_list.Add(msg);
                }
                f.change = 0;
                timer1_mask = 0;
            }
            catch (Exception e) { }
        }

        private void display_noport(object obj)
        {//窗口连接不上时，将所有料仓表显示在不在线列表
            if (checkedListBox1.Items.Count != 0)
            {
                checkedListBox1.Items.Clear();
            }
            if (checkedListBox2.Items.Count != 0)
                checkedListBox2.Items.Clear();

            if (comboBox4.Text.Equals(""))
            {
                string sql = "select * from [config]";
                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                db.Dr = db.command.ExecuteReader();
                while (db.Dr.Read())
                {
                    comboBox4.Items.Add(db.Dr["FactoryID"].ToString());

                }
                db.Dr.Close();
                db.Close();
                if (comboBox4.Items.Count > 0)
                {
                    comboBox4.Text = comboBox4.Items[0].ToString();
                }
                else
                {
                    //MessageBox.Show("未保存厂区码", "提示");
                    new Thread(new ParameterizedThreadStart(showBox)).Start("未保存厂区码");

                }
            }
            try
            {
                DataBase db = new DataBase();
                string sql = "select * from [bininfo]";
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                db.Dr = db.command.ExecuteReader();
                while (db.Dr.Read())
                {
                    Thread.Sleep(20);
                    checkedListBox2.Items.Remove(db.Dr["BinName"].ToString());
                    checkedListBox2.Items.Add(db.Dr["BinName"].ToString());

                }
                SortCheckedList(checkedListBox2);

                db.Dr.Close();
                db.Close();
            }
            catch (Exception exc)
            {
                //MessageBox.Show("请检查数据库设置", "提示");
                new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库设置");

            }

        }

        /// <summary>
        /// 发送指令函数
        /// </summary>
        /// <param name="obj"></param>
        private void SendIns(object obj)
        {
            while (flag_threadout == 1)
            {
                //richTextBox1.AppendText("sendIns_queue : "+sendIns_queue.Count+"\r\n");
                Thread.Sleep(30);
                if (sendIns_queue.Count != 0)
                {
                    Thread.Sleep(20);
                    FacMessage ele = sendIns_queue.Dequeue();
                    if (ele.ProduceTime >= 0)//时间每一秒减减，如果过了这个时间，就只取指令不发指令
                    {
                        serialPort_WriteLine(ele);
                    }
                    else
                    {//当产生时间<0
                        if (ele.ins_answer.Equals("21"))
                        {//并且指条指令是查询指令时，应该将目的指令一并删除
                            aim_ins.Dequeue();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 测试数据库中的数据表是否全，如果不全则补全不存在的数据表
        /// </summary>
        /// <returns></returns>
        private string conTosql()
        {
            int sum_table = 7;//一共需要七个表，存在一个就减减
            string[] tables = { "bininfo", "binauto", "bindata", "binlog", "config", "server", "user" };
            string sql = "SELECT TABLE_NAME FROM Factory.INFORMATION_SCHEMA.TABLES";
            DataBase db = new DataBase();
            db.command.CommandText = sql;
            db.command.Connection = db.connection;
            db.Dr = db.command.ExecuteReader();
            while (db.Dr.Read())
            {
                for (int i = 0; i < tables.Length; i++)
                {
                    if (db.Dr["TABLE_NAME"].ToString().Equals(tables[i]))
                    {
                        tables[i] = "";
                        sum_table--;
                    }
                }
            }
            //db.Dr.Close();
            //db.Close();

            if (sum_table == 0)
            {
                return "1";
            }
            else
            {
                string table = "";
                for (int i = 0; i < 7; i++)
                {
                    if (tables[i].Equals("") != true)
                    {
                        table += (tables[i] + "|");
                        DataBase database = new DataBase();
                        string sql_createT = "";
                        string sql_initdata = "";
                        if (tables[i].Equals("binauto"))
                        {
                            sql_createT = "create table [binauto] (BinID int, Time nvarchar(MAX), " +
                                "Date int, BinName nvarchar(MAX), Operation nvarchar(MAX));";
                        }
                        else if (tables[i].Equals("bindata"))
                        {
                            sql_createT = "create table [bindata] (BinID int, Volume float, Weight float, " +
                                "Temp float, Hum float, DateTime nvarchar(MAX));";
                        }
                        else if (tables[i].Equals("bininfo"))
                        {
                            sql_createT = "create table [bininfo] (BinID int, BinName nvarchar(MAX), Diameter float, " +
                                "CylinderH float, PyramidH float, Density float);";
                        }
                        else if (tables[i].Equals("binlog"))
                        {
                            sql_createT = "create table [binlog] (Address int, Dataytpe nvarchar(MAX), Data nvarchar(MAX), " +
                                "Message nvarchar(MAX), Time nvarchar(MAX));";
                        }
                        else if (tables[i].Equals("config"))
                        {
                            sql_createT = "create table [config] (DistrictID nvarchar(MAX), FactoryID nvarchar(MAX));";
                            sql_initdata = "insert into [config] values('0102', '00001')";

                        }
                        else if (tables[i].Equals("server"))
                        {
                            sql_createT = "create table [server] (ServerIp nvarchar(MAX), ServerPort int, " +
                                "UpdateServ nvarchar(MAX), DataServ nvarchar(MAX));";
                        }
                        else if (tables[i].Equals("user"))
                        {
                            sql_createT = "create table [user] (UserName nvarchar(MAX), PassWord nvarchar(MAX), Admin int);";
                        }

                        database.command.CommandText = sql_createT;
                        database.command.Connection = db.connection;
                        database.command.ExecuteNonQuery();
                        if (sql_initdata.Equals("") != true)
                        {
                            database.command.CommandText = sql_initdata;
                            database.command.ExecuteNonQuery();
                        }
                    }
                }
                //MessageBox.Show("数据库表格已完善", "提示");
                new Thread(new ParameterizedThreadStart(showBox)).Start("数据库表格已完善");

                return sum_table.ToString() + "|" + table;
            }
        }


        private void method_file(object obj)
        {//数据库连接不上时，将这个错误信息添加到日志文件中
            string message_error = obj.ToString();
            string path = System.Windows.Forms.Application.StartupPath;
            FileStream fs = new FileStream(path + "\\log.txt", FileMode.Create | FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("错误信息是：" + message_error + " 时间是：" + DateTime.Now.ToString());
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        private void getStatus(object sender, System.Timers.ElapsedEventArgs e)
        {//循环获取状态函数，在定时器中实现

            foreach (FacMessage FacInfo in sendIns_queue)
            {//遍历发送指令队列，并将指令的产生时间加一
                FacInfo.ProduceTime--;
            }

            for (int i = 0; i < list_status.Count; i++)
            {
                if (list_status[i].sign_answer == false)
                {//表示未接收到回应
                    list_status[i].life_time--;
                    //Console.WriteLine(list_status[i].ins_num + ": lifetime " + list_status[i].life_time);
                    if (list_status[i].life_time <= 0)
                    {//超时未响应，创建线程处理
                        string msg = list_status[i].message;
                        FacMessage ele = new FacMessage(list_status[i].ins_num, list_status[i].ins_answer,
                            list_status[i].fac_num, list_status[i].sign_answer,
                            list_status[i].life_time, list_status[i].message,
                            list_status[i].instruction, list_status[i].resend - 1, list_status[i].ProduceTime);
                        new Thread(new ParameterizedThreadStart(statusTimeout)).Start(ele);
                        //new Thread(new ParameterizedThreadStart(statusTimeout)).Start(i);
                        /*
                         不能传递下标索引来删除节点的原因：
                         * 当状态链表的最大长度是5时，状态链表最大的索引下标是4
                         * 当下标4传递到线程时，可能list_status[0]处理完成，节点已经删除
                         * 而此时状态链表list_status的长度为4
                         * 再通过list_status.Remove(4)来删除节点，会出现下标索引越界，
                         * 因而会出现异常
                         * 而传递节点后，每次删除节点时，会再互斥锁中遍历节点
                         * 然后确定唯一的符合要求的节点将其删除
                         * 定不会出现下标索引越界问题
                         */
                    }
                }
                else
                {//状态标志为true，表示已经接收到回应并做出了处理，可以删除节点
                    list_mutex.WaitOne();
                    list_status.RemoveAt(i);
                    list_mutex.ReleaseMutex();
                }
            }
        }

        /// <summary>
        /// 超时处理函数
        /// </summary>
        /// <param name="index"></param>
        private void statusTimeout(object index)
        {
            try
            {
                FacMessage fac = (FacMessage)index;
                list_mutex.WaitOne();//再删除节点之前加锁
                //Console.WriteLine("i = "+ i+" sum = "+list_status.Count+" Time: "+ DateTime.Now);
                //list_status.RemoveAt(i);
                for (int i = 0; i < list_status.Count; i++)
                {
                    if (fac.fac_num.Equals(list_status[i].fac_num) && fac.ins_answer.Equals(list_status[i].ins_answer))
                    {
                        list_mutex.WaitOne();
                        list_status.RemoveAt(i);
                        list_mutex.ReleaseMutex();
                    }
                }
                list_mutex.ReleaseMutex();//删除节点之后解锁

                int FacExist = 0;//是否是第一次未收到指令
                for (int i = NoAckList.Count - 1; i >= 0; i--)
                {//遍历未收到指令的链表
                    if (NoAckList[i].fac_num.Equals(fac.fac_num))
                    {//如果找到说明不是第一次未接收到指令，
                        FacExist = 1;
                        NoAckList[i].life_time++;

                        if (NoAckList[i].life_time >= 10)
                        {//如果是第十次未接收到指令，将设备设置为离线，删除NoAckList节点，也不用接着查状态获取盘库数据
                            //for (int j = send_ins.Count - 1; j >= 0; j--)
                            //{
                            //    if (send_ins[j].fac_num.Equals(NoAckList[i].fac_num.PadLeft(2, '0')))
                            //    {//如果这个料仓在正在盘库链表中，则停止发送查询结果指令并删除
                            //        send_ins.RemoveAt(j);
                            //        break;
                            //    }

                            //}
                            //for (int j = CalcVol_list.Count - 1; i >= 0; i--)
                            //{//盘库进度显示列表删除
                            //    if (NoAckList[i].fac_num.ToString().PadLeft(2, '0').Equals(CalcVol_list[i].fac_num))
                            //    {
                            //        CalcVol_list.RemoveAt(i);
                            //        break;
                            //    }
                            //}
                            checkedListBox1.Items.Remove(getName(NoAckList[i].fac_num));
                            //checkedListBox2.Items.Remove(getName(NoAckList[i].fac_num));
                            //checkedListBox2.Items.Add(getName(NoAckList[i].fac_num));
                            if (checkedListBox2.Items.Contains(getName(NoAckList[i].fac_num)) == false)
                            {
                                checkedListBox2.Items.Add(getName(NoAckList[i].fac_num));
                            }
                            SortCheckedList(checkedListBox2);
                            //checkedListBox1.Sorted = true;

                            NoAckList.RemoveAt(i);
                        }
                    }
                }
                if (0 == FacExist)
                {
                    NoAckList.Add(new FacMessage(fac.fac_num, 1, 0));
                }


                if (fac.ins_answer.Equals("01"))
                {//如果01号指令没有回应，说明料仓不存在或不在线，将其添加到不在线列表中

                    string fac_name = getName(fac.fac_num);
                    if (fac_name.Equals("") == false)
                    {
                        //先remove的目的时防止料仓重复
                        //Invoke(new MethodInvoker(delegate {
                        //    checkedListBox1.Items.Remove(fac_name);
                        //    checkedListBox2.Items.Remove(fac_name);
                        //    checkedListBox2.Items.Add(fac_name);
                        //}));
                    }
                    else
                    {
                        //MessageBox.Show("请输入有效的料仓编号", "提示");
                        new Thread(new ParameterizedThreadStart(showBox)).Start("请输入有效的料仓编号");

                    }

                }
                else if (fac.ins_answer.Equals("11"))
                {//如果查询温湿度指令没有恢复，就把查询温湿度按钮变为可用
                    查询温度ToolStripMenuItem.Enabled = true;
                }
                else if (fac.ins_answer.Equals("21"))
                {//表示查询状态指令没有回应，需要重发

                    if (fac.resend > 0)
                    {
                        aim_ins.Enqueue(oper_ins.Dequeue());
                        sendIns_queue.Enqueue(fac);
                    }
                    else
                        oper_ins.Clear();
                }
                else if (fac.ins_answer.Equals("13"))
                {
                    if (fac.resend > 0)
                        sendIns_queue.Enqueue(fac);
                }

                string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                DataBase db = new DataBase();
                string sql = "insert into [binlog] values('" + fac.fac_num + "', '回应超时', '" + fac.instruction + "', '" + fac.message + "', '" + time + "')";
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                db.command.ExecuteNonQuery();
                db.Close();

            }
            catch (SqlException se)
            {
                Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));//数据库异常存入文件
                thread_file.Start(se.ToString());
                //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");

            }


        }

        private void InitDataSet()
        {
            pageSize = 18;      //设置页面行数
            nMax = dtInfo.Rows.Count;

            pageCount = (nMax / pageSize);    //计算出总页数

            if ((nMax % pageSize) > 0) pageCount++;

            pageCurrent = 1;    //当前页数从1开始
            nCurrent = 0;       //当前记录数从0开始

            LoadData();
        }
        private void LoadData()
        {
            int nStartPos = 0;   //当前页面开始记录行
            int nEndPos = 0;     //当前页面结束记录行

            DataTable dtTemp = dtInfo.Clone();   //克隆DataTable结构框架

            if (pageCurrent == pageCount)
                nEndPos = nMax;
            else
                nEndPos = pageSize * pageCurrent;

            nStartPos = nCurrent;

            lblPageCount.Text = pageCount.ToString();
            txtCurrentPage.Text = Convert.ToString(pageCurrent);

            //从元数据源复制记录行
            for (int i = nStartPos; i < nEndPos; i++)
            {
                if (dtInfo.Rows.Count > 0)
                {//判读表中是否有内容
                    dtTemp.ImportRow(dtInfo.Rows[i]);
                    nCurrent++;

                }

            }
            bdsInfo.DataSource = dtTemp;
            bdnInfo.BindingSource = bdsInfo;
            dataGridView1.DataSource = bdsInfo;
        }

        /// <summary>
        /// 显示料仓名称
        /// </summary>
        /// <param name="obj"></param>
        private void display(object obj)
        {//主要功能是向中控发送每一个料仓的查询指令
            Thread.Sleep(30);
            string factory = "";

            if (checkedListBox1.Items.Count != 0)
            {
                checkedListBox1.Items.Clear();
            }
            if (checkedListBox2.Items.Count != 0)
                checkedListBox2.Items.Clear();

            factory = comboBox4.Text;

            if (factory.Equals(""))
            {
                string sql = "select * from [config]";
                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                db.Dr = db.command.ExecuteReader();
                while (db.Dr.Read())
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        comboBox4.Items.Add(db.Dr["FactoryID"].ToString());
                    }));

                }
                db.Dr.Close();
                db.Close();
                if (comboBox4.Items.Count > 0)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        comboBox4.Text = comboBox4.Items[0].ToString();
                        factory = comboBox4.Text;
                    }));
                }
                else
                {
                    //MessageBox.Show("未保存厂区码", "提示");
                    new Thread(new ParameterizedThreadStart(showBox)).Start("未保存厂区码");

                }
            }

            if (factory.Equals("") != true)
            {
                string sql = "select * from [bininfo]";
                //DataBase db;
                Queue<FacMessage> send_queue = new Queue<FacMessage>();
                try
                {
                    DataBase db = new DataBase();

                    db.command.CommandText = sql;
                    db.command.Connection = db.connection;
                    db.Dr = db.command.ExecuteReader();

                    while (db.Dr.Read())
                    {//显示列表，向存在于数据库中所有的料仓发送"00"号指令，检测料仓是否存在
                        Thread.Sleep(10);
                        string id = db.Dr["BinID"].ToString();
                        string data = "";
                        Invoke(new MethodInvoker(delegate
                        {
                            data = Data.Data(comboBox4.Text, id, "00", "0000");
                        }));
                        send_queue.Enqueue(new FacMessage(ins_num++, "01", id, false, TIME, "查询测试/添加料仓功能", data, s_Produce));

                        //serialPort_WriteLine(data, new FacMessage(ins_num++, "01", id, false, TIME, "查询测试/添加料仓功能", data));
                    }
                    db.Dr.Close();
                    db.Close();
                }
                catch (Exception exc)
                {
                    Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                    thread_file.Start(exc.ToString());
                    //MessageBox.Show("请检查数据库设置", "提示");
                    new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库设置");
                }
                FacMessage ele;
                while (send_queue.Count != 0)
                {
                    ele = send_queue.Dequeue();
                    sendIns_queue.Enqueue(ele);
                }
            }


        }

        /// <summary>
        /// 刚开始打开界面时，将所有料仓添加到不在线列表
        /// </summary>
        /// <param name="obj"></param>
        private void OpenMainForm(object obj)
        {//主要功能是向中控发送每一个料仓的查询指令
            Thread.Sleep(30);
            string factory = "";

            if (checkedListBox1.Items.Count != 0)
            {
                checkedListBox1.Items.Clear();
            }
            if (checkedListBox2.Items.Count != 0)
                checkedListBox2.Items.Clear();

            factory = comboBox4.Text;

            if (factory.Equals(""))
            {
                string sql = "select * from [config]";
                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                db.Dr = db.command.ExecuteReader();
                while (db.Dr.Read())
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        comboBox4.Items.Add(db.Dr["FactoryID"].ToString());
                    }));

                }
                db.Dr.Close();
                db.Close();
                if (comboBox4.Items.Count > 0)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        comboBox4.Text = comboBox4.Items[0].ToString();
                        factory = comboBox4.Text;
                    }));
                }
                else
                {
                    //MessageBox.Show("未保存厂区码", "提示");
                    new Thread(new ParameterizedThreadStart(showBox)).Start("未保存厂区码");
                }
            }

            if (factory.Equals("") != true)
            {
                string sql = "select * from [bininfo]";
                //DataBase db;
                Queue<FacMessage> send_queue = new Queue<FacMessage>();
                try
                {
                    DataBase db = new DataBase();

                    db.command.CommandText = sql;
                    db.command.Connection = db.connection;
                    db.Dr = db.command.ExecuteReader();

                    while (db.Dr.Read())
                    {//显示列表，向存在于数据库中所有的料仓发送"00"号指令，检测料仓是否存在
                        Thread.Sleep(10);
                        string id = db.Dr["BinID"].ToString();
                        checkedListBox2.Items.Add(getName(id));

                        //serialPort_WriteLine(data, new FacMessage(ins_num++, "01", id, false, TIME, "查询测试/添加料仓功能", data));
                    }
                    SortCheckedList(checkedListBox2);
                    db.Dr.Close();
                    db.Close();
                }
                catch (Exception exc)
                {
                    Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                    thread_file.Start(exc.ToString());
                    //MessageBox.Show("请检查数据库设置", "提示");
                    new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库设置");
                }

                OnlineCheak();

            }


        }

        /// <summary>
        /// 管理员密码修改功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        private void 管理员密码修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox_changepass.Visible = true;
            groupBox_adduser.Visible = false;
            groupBox_deleteuser.Visible = false;
            groupBox_init.Visible = false;
            textBox1.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
        }

        /// <summary>
        /// 取消修改按钮功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            groupBox_changepass.Visible = false;
        }

        /// <summary>
        /// 确认修改按钮功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                //MessageBox.Show("请输入原密码", "提示");
                new Thread(new ParameterizedThreadStart(showBox)).Start("请输入原密码");
            }
            else if (textBox6.Text.Equals(""))
            {
                MessageBox.Show("请输入新密码", "提示");
            }
            else if (textBox7.Text.Equals(""))
            {
                MessageBox.Show("请输入确认密码", "提示");
            }
            else
            {
                if (textBox6.Text.Equals(textBox7.Text) == false)
                {
                    MessageBox.Show("两次输入的密码不一致", "提示");
                }
                else
                {
                    try
                    {
                        string sql_find = "select * from [user] where UserName = '" + curr_user.name + "'";
                        DataBase database = new DataBase();
                        database.command.CommandText = sql_find;
                        database.command.Connection = database.connection;
                        database.Dr = database.command.ExecuteReader();
                        string old_passwd = "";
                        while (database.Dr.Read())
                        {
                            old_passwd = database.Dr["PassWord"].ToString();
                            break;
                        }
                        if (old_passwd.Equals(textBox1.Text))
                        {
                            string sql = "update [user] set PassWord='" + textBox6.Text + "' where UserName ='" + curr_user.name + "';";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            if (db.command.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("修改密码成功", "提示");
                                db.Close();
                            }
                            else
                            {
                                MessageBox.Show("修改密码失败", "提示");
                            }
                        }
                        else
                        {
                            MessageBox.Show("原密码输入错误", "提示");
                        }

                    }
                    catch (Exception exc)
                    {
                        //MessageBox.Show("请检查数据库连接设置", "提示");
                        new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库设置");
                    }
                }
            }
        }

        /// <summary>
        /// 用户添加功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 用户添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button8.PerformClick();
            groupBox_changepass.Visible = false;
            groupBox_adduser.Visible = true;
            groupBox_deleteuser.Visible = false;
            groupBox_init.Visible = false;
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
        }

        /// <summary>
        /// 取消添加用户按钮功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button13_Click(object sender, EventArgs e)
        {
            groupBox_adduser.Visible = false;
        }

        /// <summary>
        /// 确认添加用户按钮功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            if (textBox8.Text.Equals(""))
            {
                MessageBox.Show("请输入用户名", "提示");
            }
            else if (textBox9.Text.Equals(""))
            {
                MessageBox.Show("请输入密码", "提示");
            }
            else if (textBox10.Text.Equals(""))
            {
                MessageBox.Show("请确认密码", "提示");
            }
            else if (textBox10.Text.Equals(textBox9.Text) != true)
            {
                MessageBox.Show("两次输入的密码不一致", "提示");
            }
            else
            {
                try
                {
                    string sql = "select * from [user]";
                    DataBase db = new DataBase();
                    db.command.CommandText = sql;
                    db.command.Connection = db.connection;
                    db.Dr = db.command.ExecuteReader();
                    bool isExit = false;
                    while (db.Dr.Read())
                    {
                        //richTextBox1.AppendText(db.Dr["UserName"].ToString() + "   " + textBox8.Text + "\r\n");
                        if (db.Dr["UserName"].ToString().Equals(textBox8.Text.Trim()))
                        {
                            isExit = true;
                            break;
                        }
                    }
                    db.Dr.Close();
                    if (isExit)
                    {
                        MessageBox.Show("用户已存在", "提示");
                    }
                    else
                    {
                        sql = "insert into [user] values('" + textBox8.Text.Trim() + "','" + textBox9.Text + "',0);";
                        db.command.CommandText = sql;
                        db.command.Connection = db.connection;
                        if (db.command.ExecuteNonQuery() > 0)
                        {
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + "添加用户成功\r\n\r\n");
                            db.Close();
                        }
                        else
                        {
                            MessageBox.Show("添加失败", "提示");
                        }
                    }
                }
                catch (Exception exc)
                {
                    //MessageBox.Show("请检查数据库是否创建好", "提示");
                    new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                }


            }
        }

        /// <summary>
        /// 删除用户功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 用户删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button8.PerformClick();
            groupBox_deleteuser.Visible = true;
            groupBox_changepass.Visible = false;
            groupBox_adduser.Visible = false;
            groupBox_init.Visible = false;
            textBox12.Text = "";
            comboBox7.Items.Clear();
            try
            {
                DataBase db = new DataBase();
                string sql = "select * from [user]";
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                db.Dr = db.command.ExecuteReader();
                while (db.Dr.Read())
                {
                    if (db.Dr["UserName"].ToString().Equals("root"))
                        continue;
                    else
                    {
                        comboBox7.Items.Add(db.Dr["UserName"].ToString());
                    }
                }
            }
            catch (Exception exc)
            {
                //MessageBox.Show("数据库连接失败", "提示");
                new Thread(new ParameterizedThreadStart(showBox)).Start("数据库连接失败");
            }
        }

        /// <summary>
        /// 取消删除用户功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            groupBox_deleteuser.Visible = false;
        }

        /// <summary>
        /// 确认删除用户按钮功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            if (comboBox7.Text.Equals(""))
            {
                MessageBox.Show("请输入用户名", "提示");
            }
            else if (textBox12.Text.Equals(""))
            {
                MessageBox.Show("请输入管理员密码", "提示");
            }
            else
            {
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确认要删除用户 " + comboBox7.Text + " 吗？", "提示", messButton);
                if (dr == DialogResult.OK)
                {
                    try
                    {
                        string sql1 = "select * from [user] where UserName = 'root';";
                        DataBase db = new DataBase();
                        db.command.CommandText = sql1;
                        db.command.Connection = db.connection;
                        db.Dr = db.command.ExecuteReader();
                        string str = "";
                        while (db.Dr.Read())
                        {
                            str = db.Dr["PassWord"].ToString();
                        }
                        db.Dr.Close();

                        if (str.Equals(textBox12.Text))
                        {
                            string sql = "delete [user] where UserName='" + comboBox7.Text + "';";
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            if (db.command.ExecuteNonQuery() > 0)
                            {
                                //让文本框获取焦点，不过注释这行也能达到效果
                                richTextBox1.Focus();
                                //设置光标的位置到文本尾   
                                richTextBox1.Select(richTextBox1.TextLength, 0);
                                //滚动到控件光标处   
                                richTextBox1.ScrollToCaret();
                                richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + "删除用户成功\r\n\r\n");
                                groupBox_deleteuser.Visible = false;
                                db.Close();
                            }
                            else
                            {
                                MessageBox.Show("用户不存在", "提示");
                            }
                        }
                        else
                        {
                            MessageBox.Show("管理员密码错误", "提示");
                        }
                    }
                    catch (Exception exc)
                    {
                        //MessageBox.Show("请检查数据库是否创建好", "提示");
                        new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                    }
                }


            }
        }

        /// <summary>
        /// 用户初始化功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 用户初始化ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button8.PerformClick();
            groupBox_deleteuser.Visible = false;
            groupBox_changepass.Visible = false;
            groupBox_adduser.Visible = false;
            groupBox_init.Visible = true;
            textBox14.Text = "";
            comboBox8.Items.Clear();
            try
            {
                string sql = "select * from [user]";
                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                db.Dr = db.command.ExecuteReader();
                while (db.Dr.Read())
                {
                    if (db.Dr["UserName"].ToString().Equals("root"))
                        continue;
                    comboBox8.Items.Add(db.Dr["UserName"].ToString());
                }
                db.Dr.Close();
                db.Close();
            }
            catch (Exception exc)
            {
                //MessageBox.Show("请检查数据库设置", "提示");
                new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库设置");
            }
        }

        /// <summary>
        /// 取消用户初始化按钮功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button17_Click(object sender, EventArgs e)
        {
            groupBox_init.Visible = false;
        }

        /// <summary>
        /// 确认用户初始化按钮功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button16_Click(object sender, EventArgs e)
        {
            if (comboBox8.Text.Equals(""))
            {
                MessageBox.Show("请输入用户名", "提示");
            }
            else if (textBox14.Text.Equals(""))
            {
                MessageBox.Show("请输入密码", "提示");
            }
            else
            {
                try
                {

                    string sql = "update [user] set PassWord='" + textBox14.Text + "' where UserName='" + comboBox8.Text.Trim() + "';";
                    DataBase db = new DataBase();
                    db.command.CommandText = sql;
                    db.command.Connection = db.connection;
                    if (db.command.ExecuteNonQuery() > 0)
                    {
                        //让文本框获取焦点，不过注释这行也能达到效果
                        richTextBox1.Focus();
                        //设置光标的位置到文本尾   
                        richTextBox1.Select(richTextBox1.TextLength, 0);
                        //滚动到控件光标处   
                        richTextBox1.ScrollToCaret();
                        richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + "用户初始化成功\r\n\r\n");
                        db.Close();
                    }
                    else
                    {
                        MessageBox.Show("用户初始化失败", "提示");
                    }
                }
                catch (Exception exc)
                {
                    //MessageBox.Show("请检查数据库设置", "提示");
                    new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库设置");
                }
            }

        }

        /// <summary>
        /// 串口发送指令函数
        /// </summary>
        /// <param name="facmsg"></param>
        private void serialPort_WriteLine(object facmsg)
        {
            FacMessage ele = (FacMessage)facmsg;
            //thread.Start(ins_answer);
            while (flag_threadout == 1)
            {//没有点击退出按钮
                Thread.Sleep(30);
                if (port_mask == 0)
                {//发出类似于盘库这样的操作指令时，需要先将发送函数阻塞，来发送盘库指令
                    if (oper_ins.Count == 0 && list_status.Count == 0)
                    {//操作指令队列为空（表示操作指令已经取出并发出）并且状态链表为空（所有发出去的指令需要都回应了或者超时处理了）
                        try
                        {
                            if (aim_ins.Count != 0)
                            {
                                if (ele.ins_answer.Equals("21"))
                                {
                                    oper_ins.Enqueue(aim_ins.Dequeue());
                                }
                            }
                            serialPort1.WriteLine(ele.instruction);
                            ele.life_time = 2;
                            list_status.Add(ele);

                        }
                        catch (Exception exc)
                        {
                            if (ele.ins_answer.Equals("21"))
                            {
                                oper_ins.Clear();
                                //oper_ins.Enqueue(aim_ins.Dequeue());
                            }
                            //MessageBox.Show("", "提示");
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查无线设备是否接触不良\r\n请重插无线模块并重新设置通信后重试");
                        }
                        break;//发送指令后退出循环，指令一定会发出，因为有超时函数来清空状态链表和准备发送指令队列
                    }

                }

            }
        }



        /// <summary>
        /// 串口接收到数据功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                int bytecount = serialPort1.BytesToRead;

                byte[] readBuffer = new byte[bytecount];
                serialPort1.Read(readBuffer, 0, readBuffer.Length);
                string readstr = Encoding.UTF8.GetString(readBuffer);
                //richTextBox1.AppendText(readstr+"\r\n");

                cirQueue.In(readstr);
                data_buffer += cirQueue.Out();
                cirQueue.Clear();

            }
            catch (Exception exc)
            {
                richTextBox1.AppendText(exc.ToString() + "\r\n");
            }
        }

        private string data_buffer = "";//从循环队列中取出的数据先存入缓冲区中
        private string data_take = "";//从数据缓冲区取出数据报进行处理
        /// <summary>
        /// 从循环队列中获取数据
        /// </summary>
        /// <param name="obj"></param>
        private void takeData(object obj)
        {
            try
            {
                while (flag_threadout == 1)
                {
                    //加sleep
                    Thread.Sleep(50);
                    while (data_buffer.Equals("") != true)
                    {
                        int i = 0;//i记录出现":"的位置
                        for (i = 0; i < data_buffer.Length; i++)
                        {
                            if (data_buffer[i] == ':')
                                break;
                        }
                        int j = 0;//j记录出现"\n"的位置
                        for (j = i; j < data_buffer.Length; j++)
                        {
                            if (data_buffer[j] == '\n')
                            {
                                data_take = data_buffer.Substring(i, j - i + 1);
                                new Thread(new ParameterizedThreadStart(trans)).Start(data_take);
                                data_buffer = data_buffer.Remove(i, data_take.Length);
                                data_take = "";
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                //richTextBox1.AppendText(exc.ToString()+"\r\n");
            }
        }
        /// <summary>
        /// 根据料仓编号（地址）获取料仓名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string getName(string id)
        {
            if (SqlConnect == 1)
            {
                string name = "";
                string sql = "select * from [bininfo] where BinID = " + id;

                try
                {
                    DataBase db = new DataBase();
                    db.command.CommandText = sql;
                    db.command.Connection = db.connection;
                    db.Dr = db.command.ExecuteReader();

                    while (db.Dr.Read())
                    {
                        name = db.Dr["BinName"].ToString();
                    }
                    db.Dr.Close();
                    db.Close();
                    return name;
                }
                catch (SqlException se)
                {
                    Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                    thread_file.Start(se.ToString());
                    //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                    new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                    return "";
                }

            }
            else
            {
                //MessageBox.Show("请检查数据库是否创建好", "提示");
                new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                return "";
            }

        }

        /// <summary>
        /// 对解析出来的指令进行操作
        /// </summary>
        /// <param name="obj"></param>
        private void trans(object obj)
        {
            try
            {
                string ins = obj.ToString();
                string str = Data.decoding(ins);
                if (str.Length <= 1)
                {
                    return;
                }

                string[] s = str.Split(' ');
                int equip = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);//料仓地址的十进制表示
                string data = s[3];
                if (s[0].Equals(comboBox4.Text) != true)
                {
                    //MessageBox.Show("收到非本厂区数据，请更换通信频道", "提示");
                    new Thread(new ParameterizedThreadStart(showBox)).Start("收到非本厂区数据，请更换通信频道");
                    return;
                }

                new Thread(new ParameterizedThreadStart(setSign)).Start(equip.ToString() + "+" + s[2]);
                for (int i = NoAckList.Count - 1; i >= 0; i--)
                {
                    if (NoAckList[i].fac_num.PadLeft(2, '0').Equals(equip.ToString().PadLeft(2, '0')))
                    {
                        NoAckList.RemoveAt(i);
                        break;
                    }
                }
                //接收到指令后， 新建一个线程来处理状态链表，将应答标志改成true
                //richTextBox1.AppendText(equip.ToString() + "+" + s[2]+"\r\n");
                if (s[2].Equals("01"))
                {//回应仓库参数,添加料仓后回应
                    if (SqlConnect == 1)
                    {
                        adddress = s[1];//接收到查询指令,这个地址记录
                        //分别表示直径， 仓筒高度， 下锥高度， 物料密度
                        string Diameter = "", CylinderH = "", PyramidH = "", Density = "";
                        Diameter = data.Substring(0, 4);
                        CylinderH = data.Substring(4, 4);
                        PyramidH = data.Substring(8, 4);
                        Density = data.Substring(12, 4);
                        float diameter = float.Parse(Diameter);
                        float cylinderH = float.Parse(CylinderH);
                        float pyramidH = float.Parse(PyramidH);
                        float density = float.Parse(Density);
                        float diameterInSql = 0;
                        float cylinderHInSql = 0;
                        float pyramidHInsSql = 0;
                        float densityInSql = 0;

                        string addr_eq = "";//保存在数据库中的设备地址
                        string sql = "select * from [bininfo] where [BinID]=" + equip.ToString().PadLeft(2, '0');
                        try
                        {//检测数据库是否可以连接
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.Dr = db.command.ExecuteReader();
                            while (db.Dr.Read())
                            {
                                addr_eq = db.Dr["BinID"].ToString();
                                diameterInSql = float.Parse(db.Dr["Diameter"].ToString());
                                cylinderHInSql = float.Parse(db.Dr["CylinderH"].ToString());
                                pyramidHInsSql = float.Parse(db.Dr["PyramidH"].ToString());
                                densityInSql = float.Parse(db.Dr["Density"].ToString());
                            }
                            db.Dr.Close();
                            //conn.Close();
                            if (addr_eq.Length == 0)
                            {//表示数据库中没有这个料仓，需要向数据库中添加
                                sql = "insert into [bininfo] (BinID, BinName, Diameter, CylinderH, PyramidH, Density) values(" + equip.ToString().PadLeft(2, '0') + ", " + equip.ToString().PadLeft(2, '0') + ", " + (diameter / 100).ToString() + ", " + (cylinderH / 100).ToString() + ", " + (pyramidH / 100).ToString() + ", " + (density / 1000).ToString() + ")";
                                db.command.CommandText = sql;

                                if (db.command.ExecuteNonQuery() > 0)
                                {
                                    Invoke(new MethodInvoker(delegate
                                    {
                                        //先移除料仓，目的是避免料仓重复显示，
                                        //Remove在移除不存在的项时，不发生任何操作和异常
                                        //checkedListBox1.Items.Remove(getName(equip.ToString().PadLeft(2, '0')));
                                        checkedListBox2.Items.Remove(getName(equip.ToString().PadLeft(2, '0')));

                                        if (checkedListBox1.Items.Contains(getName(equip.ToString().PadLeft(2, '0'))) == false)
                                        {
                                            checkedListBox1.Items.Add(getName(equip.ToString().PadLeft(2, '0')));
                                        }
                                        SortCheckedList(checkedListBox1);
                                     }));
                                }
                                db.Close();
                            }
                            else
                            {//如果有这个料仓，就设置参数,并在在线列表中显示
                                string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                if (diameter / 100 != diameterInSql)
                                {//如果发现有不相等的情况，记录下时间
                                    string saveMsg = diameterInSql.ToString() + "-->" + (diameter / 100).ToString();
                                    DataBase dbSaveLog = new DataBase();
                                    string sqlSaveLog = "insert into [binlog] values('" + equip.ToString() + "', '仓筒直径被修改', '" + ins + "', '"+saveMsg+"', '" + time + "')";
                                    dbSaveLog.command.CommandText = sqlSaveLog;
                                    dbSaveLog.command.Connection = dbSaveLog.connection;
                                    dbSaveLog.command.ExecuteNonQuery();
                                    db.Close();
                                }
                                if (cylinderH / 100 != cylinderHInSql)
                                {
                                    string saveMsg = cylinderHInSql.ToString() + "-->" + (cylinderH / 100).ToString();
                                    DataBase dbSaveLog = new DataBase();
                                    string sqlSaveLog = "insert into [binlog] values('" + equip.ToString() + "', '仓筒高度被修改', '" + ins + "', '" + saveMsg + "', '" + time + "')";
                                    dbSaveLog.command.CommandText = sqlSaveLog;
                                    dbSaveLog.command.Connection = dbSaveLog.connection;
                                    dbSaveLog.command.ExecuteNonQuery();
                                    db.Close();
                                }
                                if (pyramidH / 100 != pyramidHInsSql)
                                {
                                    string saveMsg = pyramidHInsSql.ToString() + "-->" + (pyramidH / 100).ToString();
                                    DataBase dbSaveLog = new DataBase();
                                    string sqlSaveLog = "insert into [binlog] values('" + equip.ToString() + "', '下锥高度被修改', '" + ins + "', '" + saveMsg + "', '" + time + "')";
                                    dbSaveLog.command.CommandText = sqlSaveLog;
                                    dbSaveLog.command.Connection = dbSaveLog.connection;
                                    dbSaveLog.command.ExecuteNonQuery();
                                    db.Close();
                                }
                                if (density / 1000 != densityInSql)
                                {
                                    string saveMsg = densityInSql.ToString() + "-->" + (density / 100).ToString();
                                    DataBase dbSaveLog = new DataBase();
                                    string sqlSaveLog = "insert into [binlog] values('" + equip.ToString() + "', '物料密度被修改', '" + ins + "', '" + saveMsg + "', '" + time + "')";
                                    dbSaveLog.command.CommandText = sqlSaveLog;
                                    dbSaveLog.command.Connection = dbSaveLog.connection;
                                    dbSaveLog.command.ExecuteNonQuery();
                                    db.Close();
                                }
                                parameter("Diameter", (diameter / 100).ToString(), equip.ToString().PadLeft(2, '0'));
                                parameter("CylinderH", (cylinderH / 100).ToString(), equip.ToString().PadLeft(2, '0'));
                                parameter("PyramidH", (pyramidH / 100).ToString(), equip.ToString().PadLeft(2, '0'));
                                parameter("Density", (density / 1000).ToString(), equip.ToString().PadLeft(2, '0'));
                                Invoke(new MethodInvoker(delegate
                                {
                                    //先移除料仓，目的是避免料仓重复显示，
                                    //Remove在移除不存在的项时，不发生任何操作和异常
                                    //checkedListBox1.Items.Remove(getName(equip.ToString().PadLeft(2, '0')));
                                    
                                    checkedListBox2.Items.Remove(getName(equip.ToString().PadLeft(2, '0')));
                                    if (checkedListBox1.Items.Contains(getName(equip.ToString().PadLeft(2, '0'))) == false)
                                    {
                                        checkedListBox1.Items.Add(getName(equip.ToString().PadLeft(2, '0')));
                                    }
                                    SortCheckedList(checkedListBox1);
                                }));

                            }
                        }
                        catch (Exception se)
                        {//如果数据库连接失败，则抛出异常.,并将异常写入文件中
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                        }

                    }
                    else
                    {//如果是一开始数据库没有连接好，抛出异常
                        //MessageBox.Show("请检查数据库是否创建好", "提示");
                        new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                    }



                }
                else if (s[2].Equals("03"))
                {//回应设置直径
                    float diameter = float.Parse(data.Substring(4, 4));

                    int value = parameter("Diameter", (diameter / 100).ToString(), equip.ToString());
                    if (value == 1)
                    {
                        Invoke(new MethodInvoker(delegate()
                        {
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + "设置直径成功\r\n\r\n");
                        }));
                    }
                }
                else if (s[2].Equals("05"))
                {//回应设置高度
                    float cylinderH = float.Parse(data.Substring(4, 4));

                    int value = parameter("CylinderH", (cylinderH / 100).ToString(), equip.ToString());
                    if (value == 1)
                    {
                        Invoke(new MethodInvoker(delegate()
                        {
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + "设置仓筒高度成功\r\n\r\n");
                        }));

                    }
                }
                else if (s[2].Equals("07"))
                {//回应设置下锥高度
                    float pyramidH = float.Parse(data.Substring(4, 4));
                    int value = parameter("PyramidH", (pyramidH / 100).ToString(), equip.ToString());
                    if (value == 1)
                    {
                        Invoke(new MethodInvoker(delegate()
                        {
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + "设置下锥高度成功\r\n\r\n");
                        }));

                    }
                }
                else if (s[2].Equals("09"))
                {//回应设置密度
                    float density = float.Parse(data.Substring(4, 4));
                    int value = parameter("Density", (density / 1000).ToString(), equip.ToString());
                    if (value == 1)
                    {
                        Invoke(new MethodInvoker(delegate()
                        {
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + "设置物料密度成功\r\n\r\n");
                        }));
                    }
                }
                else if (s[2].Equals("0B"))
                {//设备数据
                    String[] datas = data.Split('+');
                    if (datas.Length != 3)
                    {
                        return;
                    }
                    String Margin = datas[0];//边距
                    String Top = datas[1];//顶高度
                    String Wheelbase = datas[2];//轴距
                    richTextBox1.AppendText(Margin + " " + Top + " " + Wheelbase + "\r\n");
                    string path = System.Windows.Forms.Application.StartupPath;
                    FileStream fs = new FileStream(path + "\\distance.txt", FileMode.Create | FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(equip.ToString().PadLeft(2, '0') + "\t" + Margin + "\t" + Top + "\t" + Wheelbase);
                    sw.Flush();
                    sw.Close();
                    fs.Close();

                }
                else if (s[2].Equals("0D"))
                {//设备中的串口数据
                    String[] datas = data.Split('+');
                    if (datas.Length != 3)
                    {
                        return;
                    }
                    String Speed = datas[0];
                    String Channel = datas[1];
                    String ModelAddress = datas[2];
                    richTextBox1.AppendText(Speed + " " + Channel + " " + ModelAddress + "\r\n");

                    string path = System.Windows.Forms.Application.StartupPath;
                    FileStream fs = new FileStream(path + "\\com.txt", FileMode.Create | FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(equip.ToString().PadLeft(2, '0') + "\t" + Speed + "\t" + Channel + "\t" + ModelAddress);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
                else if (s[2].Equals("0F"))
                {//设备中的校正数据
                    String[] datas = data.Split('+');
                    if (datas.Length != 3)
                    {
                        return;
                    }
                    String Vertical = datas[0];
                    String Corr_percent = datas[1];
                    String Corrent = datas[2];
                    richTextBox1.AppendText(Vertical + " " + Corr_percent + " " + Corrent + "\r\n");

                    string path = System.Windows.Forms.Application.StartupPath;
                    FileStream fs = new FileStream(path + "\\corrent.txt", FileMode.Create | FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(equip.ToString().PadLeft(2, '0') + "\t" + Vertical + "\t" + Corr_percent + "\t" + Corrent);
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
                else if (s[2].Equals("11"))
                {//回应温度湿度
                    查询温度ToolStripMenuItem.Enabled = true;
                    string[] d = data.Split('+');
                    string id = Convert.ToInt32(s[1], 16).ToString();
                    string temp = d[0];//温度
                    string hum = d[1];//湿度
                    if (temp.Equals("0") && hum.Equals("0"))
                        return;

                    DateTime now = DateTime.Now;
                    string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    string sql = "insert into [bindata] (BinID, Temp, Hum, DateTime) values(" + id + ", " + temp + ", " + hum + ", '" + time + "')";
                    label24.Text = temp + "  ℃";
                    label27.Text = hum + "  %";
                    try
                    {//检测数据库能否连接成功
                        DataBase db = new DataBase();
                        db.command.CommandText = sql;
                        db.command.Connection = db.connection;
                        if (db.command.ExecuteNonQuery() > 0)
                        {
                            Invoke(new MethodInvoker(delegate()
                            {
                                //让文本框获取焦点，不过注释这行也能达到效果
                                richTextBox1.Focus();
                                //设置光标的位置到文本尾   
                                richTextBox1.Select(richTextBox1.TextLength, 0);
                                //滚动到控件光标处   
                                richTextBox1.ScrollToCaret();
                                richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + "接收到温湿度信息并保存\r\n\r\n");
                            }));
                            db.Close();
                        }
                    }
                    catch (Exception se)
                    {//数据库连接失败时，抛出异常
                        Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                        thread_file.Start(se.ToString());
                        //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                        new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                        //richTextBox1.AppendText(se.ToString() + "\r\n");
                    }
                }
                else if (s[2].Equals("13"))
                {//回应确认 盘点库容
                    Invoke(new MethodInvoker(delegate()
                    {
                        comboBox3.Visible = true;
                        label3.Visible = true;
                        groupBox2.Visible = true;
                        //让文本框获取焦点，不过注释这行也能达到效果
                        richTextBox1.Focus();
                        //设置光标的位置到文本尾   
                        richTextBox1.Select(richTextBox1.TextLength, 0);
                        //滚动到控件光标处   
                        richTextBox1.ScrollToCaret();
                        richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + getName(equip.ToString().PadLeft(2, '0')) + "开始盘库\r\n\r\n");
                    }));
                    string eq_name = getName(equip.ToString());
                    if (eq_name.Equals("") != true)
                    {
                        comboBox5.Items.Remove(eq_name);
                        comboBox6.Items.Remove(eq_name);
                        comboBox3.Items.Remove(eq_name);
                        comboBox3.Items.Add(eq_name);
                        if (comboBox3.Items.Count == 1)
                        {
                            comboBox3.Text = comboBox3.Items[0].ToString();
                        }
                        if (comboBox6.Items.Count == 0)
                        {
                            player.Stop();
                        }
                        //接收到盘库信息，将料仓信息加入链表中
                        FacMessage calc = new FacMessage(equip.ToString().PadLeft(2, '0'), 0, 1);
                        CalcVol_list.Add(calc);
                    }
                    string data_find = Data.Data(comboBox4.Text, equip.ToString().PadLeft(2, '0'), "32", "0000");
                    send_ins.Add(new FacMessage(ins_num++, "21", equip.ToString().PadLeft(2, '0'), false, 40, "回应状态", data_find, 3, s_Produce - 3595));

                }
                else if (s[2].Equals("23"))
                {//盘库结果

                    string volume = (data.Split('+'))[0];

                    string weight = (data.Split('+'))[1];
                    //MessageBox.Show(data+" "+ volume+"  "+weight);
                    float vol_f = float.Parse(volume);
                    float wei_f = float.Parse(weight);
                    float diameter = 0, cylinderh = 0, pyramidh = 0;

                    try
                    {

                        string sql = "select * from [bininfo] where [BinID] = " + equip;
                        DataBase db = new DataBase();
                        db.command.CommandText = sql;
                        db.command.Connection = db.connection;
                        db.Dr = db.command.ExecuteReader();
                        while (db.Dr.Read())
                        {
                            diameter = float.Parse(db.Dr["Diameter"].ToString());
                            cylinderh = float.Parse(db.Dr["CylinderH"].ToString());
                            pyramidh = float.Parse(db.Dr["PyramidH"].ToString());
                        }
                        db.Dr.Close();

                        DataBase dbLastData = new DataBase();//查询当前数据库中最新的数据
                        sql = "select Weight from [bindata] where [BinID] = " + equip + " order by [DateTime] desc";
                        float LastWeight = 0;
                        dbLastData.command.CommandText = sql;
                        dbLastData.command.Connection = dbLastData.connection;
                        dbLastData.Dr = dbLastData.command.ExecuteReader();
                        while (dbLastData.Dr.Read())
                        {
                            if (dbLastData.Dr["Weight"].ToString().Equals("") == false)
                            {//获取到第一个重量不为NULL的值，记录为最新数据,并且跳出循环
                                LastWeight = float.Parse(dbLastData.Dr["Weight"].ToString());
                                break;
                            }
                        }
                        dbLastData.Dr.Close();
                        float Vol_ware = (float)((diameter / 2) * (diameter / 2) * (3.14) * (pyramidh / 3 + cylinderh));
                        if (Vol_ware <= vol_f || (LastWeight!=0 &&Math.Abs(wei_f - LastWeight) >10))
                        {//如果接收到的数据比仓库体积还大, 或者重量与最新数据相比相差10吨，就开启回传数据
                            string databack = Data.Data(comboBox4.Text, equip.ToString().PadLeft(2, '0'), "38", "0001");
                            sendIns_queue.Enqueue(new FacMessage(1, "27", equip.ToString().PadLeft(2, '0'), false, TIME_WAIT, "开启回传数据", databack, s_Produce - 3595));
                        }
                    }
                    catch (Exception e) { }

                    if (vol_f == 0)
                        return;
                    string id = Convert.ToInt32(s[1], 16).ToString();
                    try
                    {
                        string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                        string sql = "insert into [bindata] (BinID, Volume, Weight, DateTime) values(" + id + ", " + volume + ", " + weight + ", '" + time + "')";

                        DataBase db_save = new DataBase();
                        db_save.command.CommandText = sql;
                        db_save.command.Connection = db_save.connection;
                        if (db_save.command.ExecuteNonQuery() > 0)
                        {
                            Invoke(new MethodInvoker(delegate()
                            {
                                //让文本框获取焦点，不过注释这行也能达到效果
                                richTextBox1.Focus();
                                //设置光标的位置到文本尾   
                                richTextBox1.Select(richTextBox1.TextLength, 0);
                                //滚动到控件光标处   
                                richTextBox1.ScrollToCaret();
                                richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n查询到  " + getName(equip.ToString().PadLeft(2, '0')) + "  料仓数据并保存\r\n\r\n");
                            }));
                            db_save.Close();

                            DataBase db_log = new DataBase();
                            string sql_log = "insert into [binlog] values('" + equip.ToString() + "', '盘库成功', '" + ins + "', '盘库完成', '" + time + "')";

                            //查询到体积后查询温湿度信息存数据库
                            string searchTemp = Data.Data(comboBox4.Text, equip.ToString().PadLeft(2, '0'), "16", "0000");
                            sendIns_queue.Enqueue(new FacMessage(ins_num++, "11", id, false, TIME, "查询温湿度", searchTemp, s_Produce));


                            for (int i = send_ins.Count - 1; i >= 0; i--)
                            {//从后往前遍历，避免删除节点后数组越界
                                if (send_ins[i].fac_num.Equals(equip.ToString().PadLeft(2, '0')))
                                {
                                    comboBox3.Items.Remove(getName(send_ins[i].fac_num));
                                    send_ins.RemoveAt(i);
                                    if (comboBox3.Items.Count == 0)
                                    {
                                        comboBox3.Visible = false;
                                        label3.Visible = false;
                                        //groupBox2.Visible = false;
                                        progressBar2.Value = 0;
                                        label19.Text = "0";
                                    }
                                    else
                                        comboBox3.Text = comboBox3.Items[0].ToString();
                                    break;

                                }
                            }
                            for (int i = CalcVol_list.Count - 1; i >= 0; i--)
                            {
                                if (equip.ToString().PadLeft(2, '0').Equals(CalcVol_list[i].fac_num))
                                {
                                    CalcVol_list.RemoveAt(i);
                                    break;
                                }
                            }

                        }
                    }
                    catch (SqlException se)
                    {
                        Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                        thread_file.Start(se.ToString());
                        //MessageBox.Show("请检查数据库设置", "提示");
                        new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库设置");
                    }


                }
                else if (s[2].Equals("17"))
                {//回应确认清洁镜头
                    string eq_name = getName(equip.ToString());

                    Invoke(new MethodInvoker(delegate()
                    {
                        //让文本框获取焦点，不过注释这行也能达到效果
                        richTextBox1.Focus();
                        //设置光标的位置到文本尾   
                        richTextBox1.Select(richTextBox1.TextLength, 0);
                        //滚动到控件光标处   
                        richTextBox1.ScrollToCaret();
                        richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + getName(equip.ToString().PadLeft(2, '0')) + "  开始清洁镜头\r\n\r\n");
                    }));
                    clean_list.Add(new Clean(equip.ToString(), eq_name, 100));

                    if (eq_name.Equals("") != true)
                    {
                        comboBox5.Items.Remove(eq_name);
                        comboBox6.Items.Remove(eq_name);
                        comboBox3.Items.Remove(eq_name);
                        comboBox5.Items.Add(eq_name);
                        if (comboBox5.Items.Count == 1)
                        {
                            comboBox5.Text = comboBox5.Items[0].ToString();
                        }
                        if (comboBox6.Items.Count == 0)
                        {
                            player.Stop();
                        }
                    }
                    FacMessage calc = new FacMessage(equip.ToString().PadLeft(2, '0'), 0, 2);
                    CalcVol_list.Add(calc);

                }

                else if (s[2].Equals("1B"))
                {//回复确认 进入或退出监控
                    if (data.Substring(6, 2).Equals("01"))
                    {//开始监控
                        Invoke(new MethodInvoker(delegate()
                        {
                            label34.Visible = true;
                            comboBox6.Visible = true;
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + getName(equip.ToString().PadLeft(2, '0')) + "  进入监控\r\n\r\n");
                        }));
                        string eq_name = getName(equip.ToString());
                        comboBox6.Items.Remove(eq_name);
                        comboBox3.Items.Remove(eq_name);
                        comboBox5.Items.Remove(eq_name);
                        comboBox6.Items.Add(eq_name);
                        monitor.userControl11.Add(getName(equip.ToString()));
                        if (comboBox6.Items.Count == 1)
                        {
                            comboBox6.Text = comboBox6.Items[0].ToString();
                        }


                    }
                    else if (data.Substring(6, 2).Equals("00"))
                    {//退出监控
                        Invoke(new MethodInvoker(delegate()
                        {
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + getName(equip.ToString().PadLeft(2, '0')) + "  退出监控\r\n\r\n");
                        }));
                        string eq_name = getName(equip.ToString());
                        //在监控状态栏删除节点，
                        comboBox6.Items.Remove(eq_name);
                        if (comboBox6.Items.Count == 0)
                        {
                            comboBox6.Visible = false;
                            label34.Visible = false;
                            player.Stop();
                        }
                        else
                            comboBox6.Text = comboBox6.Items[0].ToString();
                        string distance = monitor.userControl11.getHeight(getName(equip.ToString()));
                        try
                        {
                            //if (float.Parse(distance) / 100 < float.Parse(monitor.userControl11.getTxt(getName(equip.ToString()))))
                            if (monitor.userControl11.changeColor(getName(equip.ToString().PadLeft(2, '0'))))
                            {//关闭一个超过预警阈值的料仓，超过预警阈值的料仓个数就减一
                                alarm--;
                            }
                            if (alarm == 0)
                            {//如果监控的料仓没有值超过预警阈值，关闭音效
                                player.Stop();
                                isplaying = false;
                            }
                            if (comboBox6.Items.Count == 0)
                            {//点击退出监控，如果没有要监控的料仓，就把监控料仓的定时器关闭
                                t_monitor.Enabled = false;
                                player.Stop();
                            }
                            Invoke(new MethodInvoker(delegate
                            {
                                monitor.userControl11.Delete(getName(equip.ToString()));
                            }));

                        }
                        catch (FormatException exc)
                        {
                        }


                    }
                }
                else if (s[2].Equals("1D"))
                {//回复当前测量值
                    if (SqlConnect == 1)
                    {
                        //MessageBox.Show("1D" + s[3]);
                        string distance = data.Substring(4, 4);
                        if (float.Parse(distance) == 0)
                        {
                            return;
                        }

                        string name = getName(equip.ToString());

                        string sql = "select * from [bininfo] where [BinName] = '" + name + "'";
                        float height_sum = 0;
                        try
                        {
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.Dr = db.command.ExecuteReader();
                            while (db.Dr.Read())
                            {
                                height_sum = float.Parse(db.Dr["CylinderH"].ToString()) + float.Parse(db.Dr["PyramidH"].ToString());
                            }
                            db.Dr.Close();
                            db.Close();
                            //richTextBox1.AppendText(height_sum + "   " + distance + "\r\n");

                            try
                            {
                                monitor.userControl11.setValue(name, distance, (int)((height_sum * 100 - float.Parse(distance)) / height_sum));
                                if (float.Parse(distance) / 100 < float.Parse(monitor.userControl11.getTxt(getName(equip.ToString()))))
                                {//超过高度阈值进度条颜色变红， 响铃
                                    monitor.userControl11.setColor(getName(equip.ToString()));
                                    alarm++;
                                }
                                else
                                {

                                    if (monitor.userControl11.changeColor(getName(equip.ToString())))
                                    {//检测是否是改变了高度阈值
                                        alarm--;
                                        if (alarm <= 0)
                                        {
                                            player.Stop();
                                            isplaying = false;
                                        }
                                    }
                                    monitor.userControl11.setGreen(getName(equip.ToString()));
                                }

                                if (alarm > 0)
                                {
                                    if (isplaying == false)
                                    {
                                        player.SoundLocation = Application.StartupPath + "//alarm.wav";
                                        player.Load();
                                        player.PlayLooping();
                                        isplaying = true;
                                    }
                                }
                            }
                            catch (FormatException exc)
                            {
                                //MessageBox.Show("请确认已经进入监控状态", "提示");
                                new Thread(new ParameterizedThreadStart(showBox)).Start("请确认已经进入监控状态");
                            }
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                        }

                    }
                    else
                    {
                        //MessageBox.Show("请检查数据库是否创建好", "提示");
                        new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                    }


                }
                else if (s[2].Equals("1F"))
                {//回复确认  取消操作
                    if (send_ins.Count != 0)
                    {

                        for (int i = send_ins.Count - 1; i >= 0; i--)
                        {
                            if (send_ins[i].fac_num.Equals(equip.ToString().PadLeft(2, '0')))
                            {
                                send_ins.RemoveAt(i);
                            }
                        }
                    }
                    string fac_name = getName(equip.ToString());
                    comboBox3.Items.Remove(fac_name);//正在盘库列表删除该节点
                    comboBox5.Items.Remove(fac_name);//正在清洁镜头列表删除该节点
                    comboBox6.Items.Remove(fac_name);//正在监控列表删除该节点
                    if (comboBox3.Items.Count == 0)
                    {
                        comboBox3.Visible = false;
                        label3.Visible = false;
                        //groupBox2.Visible = false;
                        progressBar2.Value = 0;
                        label19.Text = "0";
                    }
                    if (comboBox5.Items.Count == 0)
                    {
                        try
                        {
                            comboBox5.Visible = false;
                            label33.Visible = false;
                            //groupBox2.Visible = false;
                        }
                        catch (Exception exc) { }
                    }
                    if (comboBox6.Items.Count == 0)
                    {
                        comboBox6.Visible = false;
                        label34.Visible = false;
                    }
                    if (monitor.userControl11.changeColor(fac_name))
                        alarm--;
                    if (alarm == 0 || comboBox6.Items.Count == 0)
                    {
                        player.Stop();
                        isplaying = false;
                    }
                    monitor.userControl11.Delete(fac_name);

                    for (int j = CalcVol_list.Count - 1; j >= 0; j--)
                    {
                        if (CalcVol_list[j].fac_num.PadLeft(2, '0').Equals(equip.ToString().PadLeft(2, '0')))
                        {
                            CalcVol_list.RemoveAt(j);
                            break;
                        }
                    }

                    //让文本框获取焦点，不过注释这行也能达到效果
                    richTextBox1.Focus();
                    //设置光标的位置到文本尾   
                    richTextBox1.Select(richTextBox1.TextLength, 0);
                    //滚动到控件光标处   
                    richTextBox1.ScrollToCaret();
                    richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + getName(equip.ToString().PadLeft(2, '0')) + "  已停止当前操作\r\n\r\n");

                }

                else if (s[2].Equals("21"))
                {//接收到工作状态

                    int issendins = 0;//判断是否发出了指令
                    string[] data_array = data.Split('+');
                    if (data_array.Length != 3)
                        return;
                    string complet = data_array[0];
                    data = data_array[1];
                    string schedule = data_array[2];
                    int schedule_int = Int32.Parse(schedule);
                    //richTextBox1.AppendText("接收到21\r\nsendIns_list长度  "+sendIns_list.Count.ToString() + "\r\n");
                    int data_int = Int32.Parse(data, System.Globalization.NumberStyles.HexNumber);

                    if (data_int == 2 && schedule_int == 100)
                    {
                        progressBar2.Value = 0;
                    }
                    //接收到状态后，将进度信息更改到正在盘库列表中
                    for (int i = 0; i < CalcVol_list.Count; i++)
                    {
                        if (equip.ToString().PadLeft(2, '0').Equals(CalcVol_list[i].fac_num))
                        {
                            CalcVol_list[i].life_time = schedule_int;
                        }
                    }
                    FacMessage ele;
                    if (oper_ins.Count != 0)
                    {
                        ele = oper_ins.Dequeue();
                        string fac_num = ele.fac_num.PadLeft(2, '0');

                        if (data_int != 2 && complet.Equals("01"))
                        {//表示盘库已经完成，可以获取数据

                            //string data_find = Data.Data(comboBox4.Text, equip.ToString().PadLeft(2, '0'), "34", "0000");
                            if (ele.ins_answer.Equals("23"))
                            {
                                port_mask = 1;//将发送函数屏蔽掉，
                                while (true)
                                {
                                    Thread.Sleep(30);
                                    if (list_status.Count == 0)
                                    {
                                        try
                                        {
                                            serialPort1.WriteLine(ele.instruction);
                                            //list_status.Add(new FacMessage(ins_num++, "23", equip.ToString().PadLeft(2, '0'), false, TIME_WAIT, "查询结果", data));
                                            list_status.Add(ele);
                                            issendins = 1;
                                            break;
                                        }
                                        catch (Exception exc) { }

                                    }
                                }
                                port_mask = 0;//解除屏蔽
                            }

                            //return;
                        }
                        else if (data_int != 2 && complet.Equals("00"))
                        {
                            if (send_ins.Count != 0)
                            {
                                for (int i = send_ins.Count - 1; i >= 0; i--)
                                {
                                    if (send_ins[i].fac_num.Equals(equip.ToString().PadLeft(2, '0')))
                                    {

                                        comboBox3.Items.Remove(getName(send_ins[i].fac_num));

                                        for (int it = CalcVol_list.Count - 1; it >= 0; it--)
                                        {
                                            if (equip.ToString().PadLeft(2, '0').Equals(CalcVol_list[it].fac_num))
                                            {
                                                CalcVol_list.RemoveAt(it);
                                                break;
                                            }
                                        }

                                        if (comboBox3.Items.Count == 0)
                                        {
                                            comboBox3.Visible = false;
                                            label3.Visible = false;
                                            //groupBox2.Visible = false;
                                            progressBar2.Value = 0;
                                            label19.Text = "0";
                                        }
                                        else
                                            comboBox3.Text = comboBox3.Items[0].ToString();

                                        String name = getName(send_ins[i].fac_num);

                                        send_ins.RemoveAt(i);

                                        issendins = 1;

                                        DateTime now = DateTime.Now;
                                        string sql = "insert into [binlog] values('" + equip.ToString() + "', '硬件故障', '" + ins + "', '盘库被取消', '" + now.ToString("yyyy/MM/dd HH:mm:ss") + "')";
                                        DataBase db = new DataBase();
                                        db.command.CommandText = sql;
                                        db.command.Connection = db.connection;
                                        db.command.ExecuteNonQuery();
                                        db.Close();

                                        //MessageBox.Show("料仓 " + name + " 被取消盘库", "提示");
                                        new Thread(new ParameterizedThreadStart(showBox)).Start("料仓"+name+" 被取消盘库");



                                        //让文本框获取焦点，不过注释这行也能达到效果
                                        richTextBox1.Focus();
                                        //设置光标的位置到文本尾   
                                        richTextBox1.Select(richTextBox1.TextLength, 0);
                                        //滚动到控件光标处   
                                        richTextBox1.ScrollToCaret();
                                        richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n料仓 " + name + " 被取消盘库" + "\r\n\r\n");

                                        //return;
                                    }
                                }

                            }
                        }
                        //for (int i = 0; i <sendIns_list.Count; i++)
                        if (issendins == 0)
                        {
                            if (fac_num.Equals(equip.ToString().PadLeft(2, '0')))
                            {
                                //serialPort_WriteLine(ele);//直接发送，不往发送队列中添加
                                //richTextBox1.AppendText("进入到判断\r\n");
                                if (data_int == 0)
                                {//表示此料仓无操作，可以执行盘库等操作
                                    if (ele.instruction.Equals("delete"))
                                    {
                                        string fac_name = getName(fac_num);
                                        int d = delete(fac_name);
                                        if (d > 0)
                                        {
                                            Invoke(new MethodInvoker(delegate
                                            {
                                                //让文本框获取焦点，不过注释这行也能达到效果
                                                richTextBox1.Focus();
                                                //设置光标的位置到文本尾   
                                                richTextBox1.Select(richTextBox1.TextLength, 0);
                                                //滚动到控件光标处   
                                                richTextBox1.ScrollToCaret();
                                                richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n删除了料仓  " + fac_name + "\r\n\r\n");
                                                if (d != 0)
                                                {
                                                    checkedListBox1.Items.Remove(fac_name);
                                                }
                                            }));
                                        }

                                    }
                                    else
                                    {
                                        //serialPort_WriteLine(ele);
                                        port_mask = 1;//将发送函数屏蔽掉，
                                        while (true)
                                        {
                                            Thread.Sleep(30);
                                            if (list_status.Count == 0)
                                            {
                                                try
                                                {
                                                    serialPort1.WriteLine(ele.instruction);
                                                    list_status.Add(ele);
                                                    break;
                                                }
                                                catch (Exception exc) { }

                                            }
                                        }
                                        port_mask = 0;//解除屏蔽
                                        //serialPort1.WriteLine(ele.instruction);
                                        //list_status.Add(ele);
                                        //return;
                                    }


                                }

                                else if (data_int == 1)
                                {
                                    if (ele.ins_answer.Equals("1B"))
                                    {//退出监控状态
                                        string[] ins_send = Data.decoding(ele.instruction + "\n").Split(' ');
                                        string ins_data;
                                        if (ins_send.Length <= 1)
                                            return;
                                        ins_data = ins_send[3];
                                        if (Int32.Parse(ins_data, System.Globalization.NumberStyles.HexNumber) == 0)
                                        {//表示向正在监控的料仓发送退出监控指令

                                            //list_status.Add(ele);
                                            //serialPort1.WriteLine(ele.instruction);
                                            port_mask = 1;//将发送函数屏蔽掉，
                                            while (true)
                                            {
                                                Thread.Sleep(30);
                                                if (list_status.Count == 0)
                                                {
                                                    try
                                                    {
                                                        serialPort1.WriteLine(ele.instruction);
                                                        list_status.Add(ele);
                                                        break;

                                                    }
                                                    catch (Exception exc)
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                            port_mask = 0;//解除屏蔽
                                        }
                                        else
                                        {
                                            new Thread(new ParameterizedThreadStart(showBox)).Start(" 料仓  " + getName(fac_num) + "  正在进料监控， 请稍后操作");
                                        }
                                            //MessageBox.Show(, "提示");
                                    }
                                    else
                                    {
                                        new Thread(new ParameterizedThreadStart(showBox)).Start(" 料仓  " + getName(fac_num) + "  正在进料监控， 请稍后操作");
                                    }
                                        //MessageBox.Show(, "提示");
                                }
                                else if (data_int == 2)
                                {
                                    bool isoperating = false;
                                    for (int i = 0; i < send_ins.Count; i++)
                                    {
                                        if (send_ins[i].fac_num.Equals(equip.ToString().PadLeft(2, '0')))
                                        {
                                            isoperating = true;
                                            break;
                                        }
                                    }
                                    //if(isoperating == false || ele.instruction.Equals("delete"))
                                    //richTextBox1.AppendText(isoperating.ToString() + "  " + ele.ins_answer+"  "+ele.instruction+"\r\n");
                                    //接收到的状态信息是2， 表示料仓正在进行盘库，isoperating为false是指人工进行了盘库
                                    //但是软件部分不知道，delete是指要进行删除料仓操作，需要提示用户料仓正在盘库
                                    if (isoperating == false || ele.instruction.Equals("delete")){
                                        new Thread(new ParameterizedThreadStart(showBox)).Start(" 料仓  " + getName(fac_num) + "  正在盘库， 请稍后操作示");
                                    }
                                        //MessageBox.Show();

                                    //料仓正在进行盘库，但是操作指令不是查询数据指令
                                    else if (isoperating == true && (ele.ins_answer.Equals("23") == false))
                                    {
                                        new Thread(new ParameterizedThreadStart(showBox)).Start("料仓  " + getName(fac_num) + "  正在盘库， 请稍后操作");
                                    }
                                        //MessageBox.Show(" ", "提示");
                                }
                                else if (data_int == 3)
                                {
                                    new Thread(new ParameterizedThreadStart(showBox)).Start(" 料仓  " + getName(fac_num) + "  正在清洁镜头， 请稍后操作");
                                    //MessageBox.Show("", "提示");
                                }

                            }
                        }


                    }
                }
                else if (s[2].Equals("25"))
                {//回传数据

                    port_mask = 1;
                    back_complet = 2;
                    string[] d = data.Split('+');
                    //richTextBox1.AppendText("指令是：" + ins + "\r\n");
                    //richTextBox1.AppendText("角度是：" + d[0] + "\r\n 距离是：" + d[1] + "\r\n 进度是： " + d[2] + "\r\n");

                    if (backdata_path.Length <= 1)
                    {
                        backdata_path = "data.txt";
                    }
                    writeToFile_buffer += d[0] + " " + d[1] + " " + d[2] + "\r\n";
                    recv_num--;
                    int angle_int = Int32.Parse(d[0]);
                    backdata[angle_int].length = d[1];
                    backdata[angle_int].schedule = d[2];

                }
                else if (s[2].Equals("27"))
                {//开始回传数据
                    int data_int = Int32.Parse(data);
                    DateTime now = DateTime.Now;
                    backdata_path = "data_" + equip.ToString().PadLeft(2, '0') + "_" + now.Month.ToString().PadLeft(2, '0') + now.Day.ToString().PadLeft(2, '0') + "_" + now.Hour.ToString().PadLeft(2, '0') + now.Minute.ToString().PadLeft(2, '0')+"_"+data_int + ".txt";
                    recv_num = data_int;
                    for (int i = 0; i < 200; i++)
                    {
                        backdata[i] = new BackData();
                    }
                }
                else if (s[2].Equals("FF"))
                {
                    DateTime now = DateTime.Now;
                    //string time = now.Year + "/" + now.Month + "/" + now.Day + " " +
                    //    now.Hour.ToString().PadLeft(2, '0') + ":" + now.Minute.ToString().PadLeft(2, '0') + ":" + now.Second.ToString().PadLeft(2, '0');
                    string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    int data_int = Int32.Parse(data);

                    if (data_int == 0)
                    {
                        try
                        {
                            string sql = "insert into [binlog] values('" + equip.ToString() + "', '硬件故障', '" + ins + "', '激光头无回应', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;

                            db.command.ExecuteNonQuery();
                            db.Close();
                            new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  激光头无回应,请清洁镜头后重试");
                            //MessageBox.Show(", "硬件故障");
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + getName(equip.ToString().PadLeft(2, '0')) + " 激光头无回应\r\n\r\n");


                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("\r\n", "提示");
                        }


                    }
                    else if (data_int == 1)
                    {
                        try
                        {

                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '硬件故障', '" + ins + "', '激光头回应数据异常', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  激光头回应数据异常,请清洁镜头后重试");
                            //MessageBox.Show(", "硬件故障");
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + getName(equip.ToString().PadLeft(2, '0')) + " 激光头回应数据异常\r\n\r\n");

                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("\r\n", "提示");

                        }


                    }
                    else if (data_int == 2)
                    {
                        try
                        {

                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','硬件故障,角度计无回应', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '硬件故障', '" + ins + "', '角度计无回应', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  角度计无回应,请相关技术人员检测");
                            //MessageBox.Show(", "硬件故障");
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + getName(equip.ToString().PadLeft(2, '0')) + " 角度计无回应\r\n\r\n");

                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("\r\n", "提示");
                        }

                    }
                    else if (data_int == 3)
                    {
                        try
                        {

                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','硬件故障,温度计无回应', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '硬件故障', '" + ins + "', '温度计没回应', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  温度计无回应,请相关技术人员检测");
                            //MessageBox.Show(", "硬件故障");
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + getName(equip.ToString().PadLeft(2, '0')) + " 温度计无回应\r\n\r\n");

                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("\r\n", "提示");
                        }

                    }
                    else if (data_int == 16)
                    {
                        try
                        {
                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,电机正忙或正在执行其他操作', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '硬件故障', '" + ins + "', '电机正忙或正在执行其他操作', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  电机正忙或正在执行其他操作");
                            //MessageBox.Show(", "软件故障");
                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n" + getName(equip.ToString().PadLeft(2, '0')) + " 电机正忙\r\n\r\n");


                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("\r\n", "提示");
                        }

                    }
                    else if (data_int == 17)
                    {
                        try
                        {
                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,没配置  直径', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '软甲故障', '" + ins + "', '没配置  直径', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  没配置  直径");
                            //MessageBox.Show(", "软件故障");
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("\r\n", "提示");
                        }

                    }
                    else if (data_int == 18)
                    {
                        try
                        {
                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,没配置  高度', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '软件故障', '" + ins + "', '没配置  高度', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            new Thread(new ParameterizedThreadStart(showBox)).Start(equip.ToString().PadLeft(2, '0') + "  没配置  高度");
                            //MessageBox.Show(getName(", "软件故障");
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));

                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("\r\n", "提示");
                        }

                    }
                    else if (data_int == 19)
                    {
                        try
                        {

                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,没配置  下锥高度', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '软件故障', '" + ins + "', '没配置  下锥高度', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  没配置  下锥高度");
                            //MessageBox.Show(", "软件故障");
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("\r\n", "提示");
                        }

                    }
                    else if (data_int == 20)
                    {
                        try
                        {

                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,没配置  安装距离到顶高度', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '软件故障', '" + ins + "', '没配置  安装距离到顶高度', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  没配置  安装距离到顶高度");
                            //MessageBox.Show(", "软件故障");
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("\r\n", "提示");
                        }

                    }
                    else if (data_int == 21)
                    {
                        try
                        {

                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,没配置  密度', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '软件故障', '" + ins + "', '没配置  密度', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  没配置  密度");
                            //MessageBox.Show(", "软件故障");
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                        }

                    }
                    else if (data_int == 64)
                    {
                        try
                        {

                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,没配置  密度', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '盘库过程出错', '" + ins + "', '垂直测量失败，取消盘库', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            //new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  没配置  密度");
                            //MessageBox.Show(", "软件故障");
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                        }
                    }
                    else if (data_int == 65)
                    {
                        try
                        {
                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,没配置  密度', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '盘库过程出错', '" + ins + "', '累计测量失败10个点，取消盘库', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            //new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  没配置  密度");
                            //MessageBox.Show(", "软件故障");
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                        }
                    }
                    else if (data_int == 66)
                    {
                        try
                        {

                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,没配置  密度', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '盘库过程出错', '" + ins + "', '负值过大', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            //new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  没配置  密度");
                            //MessageBox.Show(", "软件故障");
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                        }
                    }
                    else if (data_int == 67)
                    {
                        try
                        {

                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,没配置  密度', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '盘库过程出错', '" + ins + "', '超过满仓体积', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            //new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  没配置  密度");
                            //MessageBox.Show(", "软件故障");
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                        }
                    }
                    else if (data_int == 68)
                    {
                        try
                        {

                            int addr = Int32.Parse(s[1], System.Globalization.NumberStyles.HexNumber);
                            //string sql = "insert into [binlog] values('" + addr.ToString() + "','软件故障,没配置  密度', '" + DateTime.Now.ToString() + "');";
                            string sql = "insert into [binlog] values('" + addr.ToString() + "', '盘库过程出错', '" + ins + "', '错误数据过多', '" + time + "')";
                            DataBase db = new DataBase();
                            db.command.CommandText = sql;
                            db.command.Connection = db.connection;
                            db.command.ExecuteNonQuery();
                            db.Close();
                            //new Thread(new ParameterizedThreadStart(showBox)).Start(getName(equip.ToString().PadLeft(2, '0')) + "  没配置  密度");
                            //MessageBox.Show(", "软件故障");
                        }
                        catch (SqlException se)
                        {
                            Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                            thread_file.Start(se.ToString());
                            new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                            //MessageBox.Show("请检查数据库是否创建好\r\n", "提示");
                        }
                    }

                }
            }
            catch (Exception exc)
            {
                richTextBox1.AppendText(exc.ToString() + "\r\n");
            }

        }

        private void SortCheckedList(CheckedListBox checkedListBox)
        {
            List<string> SortList = new List<string>();
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                SortList.Add(checkedListBox.Items[i].ToString());
            }
            SortList.Sort();
            checkedListBox.Items.Clear();
            for (int i = 0; i < SortList.Count; i++)
                checkedListBox.Items.Add(SortList[i]);
        }


        private void setSign(object obj)
        {
            //在接收到数据后，经过处理数据函数，已经将料仓编号和指令提取出来，并用‘+’分隔开
            //在这只需要按照‘+’切割就可以得到料仓编号和指令
            string[] receive = ((string)obj).Split('+');
            //接收到的设备地址转为十进制
            int equip_receive = Int32.Parse(receive[0], System.Globalization.NumberStyles.HexNumber);
            //接收到的指令码转为十进制
            int instruction_receive = Int32.Parse(receive[1], System.Globalization.NumberStyles.HexNumber);
            //richTextBox1.AppendText(equip_receive.ToString()+"  "+instruction_receive.ToString()+"-----\r\n");
            list_mutex.WaitOne();
            for (int i = 0; i < list_status.Count; i++)
            {
                //状态链表中的设备地址转化为十进制
                int equip_list = Int32.Parse(list_status[i].fac_num, System.Globalization.NumberStyles.HexNumber);
                //状态链表中的指令码转化为十进制
                int instruction_list = Int32.Parse(list_status[i].ins_answer.ToString(), System.Globalization.NumberStyles.HexNumber);

                if (equip_list == equip_receive && instruction_list == instruction_receive)
                {//只是将状态标志改为true，并不执行删除操作，删除操作统一在遍历链表的定时器中删除
                    list_status[i].sign_answer = true;
                }

            }
            list_mutex.ReleaseMutex();

        }
        /// <summary>
        /// 退出登录按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 退出登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show("确认要退出?", "提示", messButton);
            if (dr == DialogResult.OK)
            {
                curr_user.logout();
                if (form_login == null)
                    form_login = new Form_Login();
                form_login.Show();
                //Form_Login form_Login = new Form_Login();
                //form_Login.Show();
                serialPort1.Close();

                //form_login.Show();
                flag_threadout = 0;
                t_status.Enabled = false;
                Thread.Sleep(300);
                //while (sendIns_queue.Count != 0 || list_status.Count != 0) ;
                this.Dispose();
            }

        }

        private void show_login(object obj)
        {
            Application.Run(new Form_Login());
            this.Close();

        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //asc.controlAutoSize(this);
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;  //不显示在系统任务栏
                notifyIcon1.Visible = true;  //托盘图标可见

            }
        }

        /// <summary>
        /// 多选框选中空白时检测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (checkedListBox1.IndexFromPoint(
            checkedListBox1.PointToClient(Cursor.Position).X,
            checkedListBox1.PointToClient(Cursor.Position).Y) == -1)
            {
                e.NewValue = e.CurrentValue;
            }
        }


        /// <summary>
        /// checkoutlistbox右键点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int posindex = checkedListBox1.IndexFromPoint(new Point(e.X, e.Y));
                checkedListBox1.ContextMenuStrip = null;
                checkedListBox1.SelectedIndex = posindex;
                contextMenuStrip1.Show(checkedListBox1, new Point(e.X, e.Y));
                if (curr_user.admin.Equals("1"))
                {
                    if (checkedListBox1.Text.Equals(""))
                    {
                        test2ToolStripMenuItem.Visible = false;
                        刷新ToolStripMenuItem.Visible = true;
                        if (checkedListBox1.SelectedItems.Count == 0)
                        {
                            testToolStripMenuItem.Visible = false;
                        }
                        else
                            testToolStripMenuItem.Visible = true;

                        显示数据信息ToolStripMenuItem.Visible = true;
                        显示参数信息ToolStripMenuItem.Visible = true;
                        添加料仓ToolStripMenuItem.Visible = true;
                        更改名称ToolStripMenuItem.Visible = false;
                        显示盘库时间ToolStripMenuItem.Visible = true;
                        重试连接ToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        test2ToolStripMenuItem.Visible = true;
                        刷新ToolStripMenuItem.Visible = true;
                        if (checkedListBox1.CheckedItems.Count == 0)
                        {
                            testToolStripMenuItem.Visible = false;
                        }
                        else
                            testToolStripMenuItem.Visible = true;
                        显示数据信息ToolStripMenuItem.Visible = true;
                        显示参数信息ToolStripMenuItem.Visible = true;
                        添加料仓ToolStripMenuItem.Visible = true;
                        更改名称ToolStripMenuItem.Visible = true;
                        显示盘库时间ToolStripMenuItem.Visible = true;
                        重试连接ToolStripMenuItem.Visible = false;
                    }

                }
                else
                {
                    if (checkedListBox1.Text.Equals(""))
                    {
                        test2ToolStripMenuItem.Visible = false;
                        刷新ToolStripMenuItem.Visible = true;
                        testToolStripMenuItem.Visible = false;
                        显示数据信息ToolStripMenuItem.Visible = true;
                        显示参数信息ToolStripMenuItem.Visible = true;
                        添加料仓ToolStripMenuItem.Visible = false;
                        更改名称ToolStripMenuItem.Visible = false;
                        显示盘库时间ToolStripMenuItem.Visible = true;
                        重试连接ToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        test2ToolStripMenuItem.Visible = false;
                        刷新ToolStripMenuItem.Visible = true;
                        testToolStripMenuItem.Visible = false;
                        显示数据信息ToolStripMenuItem.Visible = true;
                        显示参数信息ToolStripMenuItem.Visible = true;
                        添加料仓ToolStripMenuItem.Visible = false;
                        更改名称ToolStripMenuItem.Visible = false;
                        显示盘库时间ToolStripMenuItem.Visible = true;
                        重试连接ToolStripMenuItem.Visible = false;
                    }
                }

            }
            //checkedListBox1.Refresh();
        }


        /// <summary>
        /// 右键点击弹出删除按钮功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count != 0)
            {
                int del = 0;
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确认要删除" + checkedListBox1.CheckedItems.Count + "个料仓吗？", "提示", messButton);
                if (dr == DialogResult.OK)
                {
                    int d = 0;
                    for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
                    {
                        //richTextBox1.AppendText("count:  " + checkedListBox1.Items.Count + "\n");
                        if (checkedListBox1.GetItemChecked(i))
                        {
                            string fac_name = checkedListBox1.Items[i].ToString();
                            string id = selectID(fac_name);
                            string data_search = Data.Data(comboBox4.Text, id, "32", "0000");
                            aim_ins.Enqueue(new FacMessage(ins_num++, "00", id, false, TIME_WAIT, "删除料仓", "delete", s_Produce));
                            sendIns_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, TIME, "删除料仓前查询状态", data_search, 3, s_Produce));

                        }
                    }

                }
            }

        }

        /// <summary>
        /// 删除料仓函数实现
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private int delete(string str)
        {
            int succ = 0;//表示成功删除多少个表中的数据

            string id = selectID(str);
            try
            {
                string sql = "delete from [bindata] where [BinID] = '" + id + "'";
                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                if (db.command.ExecuteNonQuery() > 0)
                {
                    succ += 1;
                }

                sql = "delete from [binauto] where [BinName] = '" + str + "'";
                db.command.CommandText = sql;
                if (db.command.ExecuteNonQuery() > 0)
                {
                    succ += 1;
                }
                sql = "delete from [bininfo] where BinName = '" + str + "'";
                db.command.CommandText = sql;
                if (db.command.ExecuteNonQuery() > 0)
                {
                    succ += 1;
                }

                return succ;

            }
            catch (Exception exc)
            {
                new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库设置");
                //MessageBox.Show("请检查数据库设置", "提示");
                new Thread(new ParameterizedThreadStart(showBox)).Start("");
                return succ;
            }
        }

        /// <summary>
        /// 串口设置按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 串口选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oper_ins.Clear();
            button8.PerformClick();
            groupBox_serial.Visible = true;
            comboBox1.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            if (ports.Length != 0)
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    comboBox1.Items.Add(ports[i]);
                }
                comboBox1.Text = comboBox1.Items[0].ToString();
                comboBox2.Text = comboBox2.Items[0].ToString();
            }

        }

        /// <summary>
        /// 确定串口设置按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            try
            {
                //serialPort1 = new SerialPort(this.components);
                serialPort1.PortName = comboBox1.Text;
                //this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
                serialPort1.BaudRate = Int32.Parse(comboBox2.Text);
                serialPort1.Open();
                //Thread thread_takeData = new Thread(takeData);
                //thread_takeData.Start();

                port_isopen = 1;
                string path = System.Windows.Forms.Application.StartupPath;
                FileStream fs = new FileStream(path + "\\serialPort.txt", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(serialPort1.PortName + "+" + serialPort1.BaudRate);
                sw.Flush();
                sw.Close();
                fs.Close();

                new Thread(new ParameterizedThreadStart(showBox)).Start(serialPort1.PortName + "   " + serialPort1.BaudRate.ToString());
                //MessageBox.Show(, "当前串口设置");
                groupBox_serial.Visible = false;
            }
            catch (Exception exc)
            {
                new Thread(new ParameterizedThreadStart(showBox)).Start("请选择有效的串口");
                //MessageBox.Show("", "提示");
            }


        }



        /// <summary>
        /// 显示详细信息按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 显示详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SW = SystemInformation.WorkingArea.Width;
            int SH = SystemInformation.WorkingArea.Height;
            double SW_percent = (double)SW / (double)1366;
            double SH_percent = (double)SH / (double)738;
            bdnInfo.Visible = true;
            groupBox_pic.Visible = false;
            dataGridView1.Visible = true;
            button8.Location = new Point((int)((886) * SW_percent), button8.Location.Y);
            button8.Visible = true;
            //Thread thread_select = new Thread(new ParameterizedThreadStart(select));
            //thread_select.Start("config");
            select("config");
        }
        //选项数据库内容显示函数
        private void select(object obj)
        {
            string str = (string)obj;
            //if (checkedListBox1.CheckedItems.Count != 0 && checkedListBox2.CheckedItems.Count != 0)
            //{
            if (str.Equals("config"))
            {
                toolStripButton8.Visible = false;
                dataGridView1.DataSource = null;
                string sql = "select * from [bininfo] where [BinName]=''";
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        sql += " or [BinName]='" + checkedListBox1.Items[i].ToString() + "'";
                    }
                }
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    if (checkedListBox2.GetItemChecked(i))
                    {
                        sql += " or [BinName]='" + checkedListBox2.Items[i].ToString() + "'";
                    }
                }
                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;


                db.sda.SelectCommand = db.command;
                //DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                db.sda.Fill(ds, "ds");
                //dataGridView1.DataSource = dt;
                dtInfo = ds.Tables[0];
                InitDataSet();
                dataGridView1.Columns[0].HeaderCell.Value = "料仓编号";
                dataGridView1.Columns[1].HeaderCell.Value = "料仓名称";
                dataGridView1.Columns[2].HeaderCell.Value = "仓筒直径";
                dataGridView1.Columns[3].HeaderCell.Value = "仓筒高度";
                dataGridView1.Columns[4].HeaderCell.Value = "下锥高度";
                dataGridView1.Columns[5].HeaderCell.Value = "物料密度";

                db.Close();

            }
            else if (str.Equals("data"))
            {
                toolStripButton8.Visible = false;
                dataGridView1.DataSource = null;
                string sql = "select BinName, Volume, Weight, Temp, Hum, DateTime from [bindata], [bininfo] where [bindata].[BinID] in (" +
                "select [BinID] from [bininfo] where [BinName]=''";
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        sql += " or [BinName]='" + checkedListBox1.Items[i].ToString() + "'";
                    }
                }
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    if (checkedListBox2.GetItemChecked(i))
                    {
                        sql += " or [BinName]='" + checkedListBox2.Items[i].ToString() + "'";
                    }
                }
                sql += ") AND [bininfo].[BinID]=[bindata].[BinID]  order by [DateTime] desc";

                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;

                db.sda.SelectCommand = db.command;
                //DataTable dt = new DataTable();
                //sda.Fill(dt);
                //dataGridView1.DataSource = dt;
                DataSet ds = new DataSet();
                db.sda.Fill(ds, "ds");
                //dataGridView1.DataSource = dt;
                dtInfo = ds.Tables[0];


                InitDataSet();
                dataGridView1.Columns[0].HeaderCell.Value = "料仓名称";
                dataGridView1.Columns[1].HeaderCell.Value = "物料体积(m³)";
                dataGridView1.Columns[2].HeaderCell.Value = "物料重量(吨)";
                dataGridView1.Columns[3].HeaderCell.Value = "料仓温度(℃)";
                dataGridView1.Columns[4].HeaderCell.Value = "料仓湿度(%RH)";
                dataGridView1.Columns[5].HeaderCell.Value = "时间日期";
                dataGridView1.Columns[5].DefaultCellStyle.Format = "yy/MM/dd HH:mm:ss";
                dataGridView1.Columns[5].Width = 180;

                db.Close();
            }
            else if (str.Equals("time"))
            {
                toolStripButton8.Visible = true;

                dataGridView1.DataSource = null;
                string sql = "select * from [binauto] where [BinID] in (" +
                "select [BinID] from [bininfo] where [BinName]=''";
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        sql += " or [BinName]='" + checkedListBox1.Items[i] + "'";
                    }
                }
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    if (checkedListBox2.GetItemChecked(i))
                    {
                        sql += " or [BinName]='" + checkedListBox2.Items[i].ToString() + "'";
                    }
                }
                sql += ") order by [Time] asc";


                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;

                db.sda.SelectCommand = db.command;
                //DataTable dt = new DataTable();
                //sda.Fill(dt);
                //dataGridView1.DataSource = dt;
                DataSet ds = new DataSet();
                db.sda.Fill(ds, "ds");
                //dataGridView1.DataSource = dt;
                dtInfo = ds.Tables[0];
                InitDataSet();
                dataGridView1.Columns[0].HeaderCell.Value = "料仓编号";
                dataGridView1.Columns[1].HeaderCell.Value = "时间";
                dataGridView1.Columns[2].HeaderCell.Value = "日期";
                dataGridView1.Columns[3].HeaderCell.Value = "料仓名称";
                dataGridView1.Columns[4].HeaderCell.Value = "操作类型";
                db.Close();

            }

            //}
            //else { }
        }

        /// <summary>
        /// 退出表格按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            groupBox_pic.Visible = false;
            dataGridView1.Visible = false;
            button8.Visible = false;
            bdnInfo.Visible = false;
        }


        /// <summary>
        /// 显示数据信息按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 显示数据信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SW = SystemInformation.WorkingArea.Width;
            int SH = SystemInformation.WorkingArea.Height;
            double SW_percent = (double)SW / (double)1366;
            double SH_percent = (double)SH / (double)738;
            bdnInfo.Visible = true;
            groupBox_pic.Visible = false;
            dataGridView1.Visible = true;
            button8.Visible = true;
            button8.Location = new Point((int)(886 * SW_percent), button8.Location.Y);
            select("data");
        }
        /// <summary>
        /// 添加料仓输入框按键事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (toolStripTextBox1.Text.Equals("") != true)
                {
                    try
                    {
                        string data = Data.Data(comboBox4.Text, toolStripTextBox1.Text, "00", "0000");
                        sendIns_queue.Enqueue(new FacMessage(ins_num++, "01", toolStripTextBox1.Text, false, TIME, "查询测试/添加料仓功能", data, s_Produce));

                        //向状态链表中添加状态信息，以此为例，接收到0x01指令时表示数据传输成功
                        if (ins_num > 2000)
                        {
                            ins_num = 1;
                        }
                    }
                    catch (Exception exc)
                    {
                        new Thread(new ParameterizedThreadStart(showBox)).Start("请输入正确的料仓编号");
                        //MessageBox.Show("", "提示");
                    }

                    contextMenuStrip1.Visible = false;
                }
            }
        }

        /// <summary>
        /// 盘库按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            sendIns_queue.Clear();
            oper_ins.Clear();
            list_status.Clear();
            //加上先发送指令查询料仓设备状态，然后检索当前状态列表


            if (checkedListBox1.CheckedItems.Count == 0)
            {
                new Thread(new ParameterizedThreadStart(showBox)).Start("请选择料仓进行盘库");
                //MessageBox.Show("", "提示");
                return;
            }
            Queue<FacMessage> ins_queue = new Queue<FacMessage>();
            for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
            {
                //bool isoperating = false;
                if (checkedListBox1.GetItemChecked(i))
                {
                    //MessageBox.Show(checkedListBox1.Items[i].ToString());
                    string id = selectID(checkedListBox1.Items[i].ToString());
                    string data_search = Data.Data(comboBox4.Text, id, "32", "0000");
                    string d = Data.Data(comboBox4.Text, id, "18", "0000");
                    aim_ins.Enqueue(new FacMessage(ins_num++, "13", id, false, 3, "盘库", d, s_Produce));
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "手动盘库前查询状态", data_search, 3, s_Produce));

                    //向发送链表中添加此指令，但是不发送这条指令，发送的是查询指令

                }
            } //end for
        }

        /// <summary>
        /// 定时盘库按钮点击时间功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {//相当于显示时间窗体
            //创建时间线程，并保持一直打开状态

            Thread time = new Thread(method);
            time.Start();
        }

        /// <summary>
        /// 委托时间功能实现
        /// </summary>
        /// <param name="obj"></param>
        private void method(object obj)
        {
            MethodInvoker MethInvo = new MethodInvoker(showtime);
            BeginInvoke(MethInvo);

        }

        /// <summary>
        /// 显示计时窗体
        /// </summary>
        private void showtime()
        {
            f.Visible = true;
            f.Activate();
        }


        /// <summary>
        /// 菜单栏的定时盘库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            button2.PerformClick();
        }


        /// <summary>
        /// 关闭窗体时将计时器关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
            f.Close();
        }

        /// <summary>
        /// 先检测这个定时器的屏蔽信号（自定义timer1_mask）是否为1,
        /// 然后检测是否更改了定时时间，比如添加删除定时时间
        /// 然后再对内存中的盘库时间列表进行遍历
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {//自动盘库定时器
            //SendMessage(Handle, WM_SYSCOMMAND, SC_MONITORPOWER, -1); //打开显示器;
            SystemSleepManagement.NoCloseScreen();

            if (timer1_mask == 1)
                return;
            back_complet--;
            if (back_complet == 0)
            {
                new Thread(save_backdata).Start();
            }

            if (f.change == 1)
            {
                Thread loadAuto = new Thread(LoadAuto);
                loadAuto.Start();
            }

            DateTime now = DateTime.Now;

            try
            {
                for (int i = 0; i < Auto_list.Count; i++)
                {
                    //if (db.Dr["Date"].ToString().Equals("0"))
                    if (Auto_list[i].date.Equals("0"))
                    {
                        DateTime time = DateTime.ParseExact(Auto_list[i].time.ToString(), "HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

                        if (time.Hour == now.Hour && time.Minute == now.Minute)
                        {
                            if (now.Second <= 10 && 0 == Auto_list[i].state)
                            {
                                if (Auto_list[i].operation.ToString().Equals("料仓盘库"))
                                {
                                    int IsSend = time_on(Auto_list[i].fac_num);//是否将指令发出,发出为1
                                    if (1 == IsSend)
                                        Auto_list[i].state = 1;
                                }
                                else
                                {
                                    int IsSend = time_on_clean(Auto_list[i].fac_num.ToString(), Auto_list[i].operation.ToString());//是否将指令发出,发出为1
                                    if (1 == IsSend)
                                        Auto_list[i].state = 1;
                                }
                            }
                            if (now.Second > 40 && now.Second < 50)
                                Auto_list[i].state = 0;

                        }
                    }
                    else
                    {
                        DateTime time = DateTime.ParseExact(Auto_list[i].time.ToString(), "HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                        if (Auto_list[i].date.PadLeft(2, '0').Equals(now.Day.ToString().PadLeft(2, '0')))
                        {
                            if (time.Hour == now.Hour && time.Minute == now.Minute)
                            {
                                if (now.Second <= 10 && 0 == Auto_list[i].state)
                                {
                                    if (Auto_list[i].operation.ToString().Equals("料仓盘库"))
                                    {
                                        int IsSend = time_on(Auto_list[i].fac_num.ToString());//是否将指令发出,发出为1
                                        if (1 == IsSend)
                                            Auto_list[i].state = 1;
                                    }
                                    else
                                    {
                                        int IsSend = time_on_clean(Auto_list[i].fac_num.ToString(), Auto_list[i].operation.ToString());//是否将指令发出,发出为1
                                        if (1 == IsSend)
                                            Auto_list[i].state = 1;
                                    }
                                }
                                if (now.Second > 40 && now.Second < 50)
                                    Auto_list[i].state = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception exc) { }


        }

        private int time_on_clean(string p1, string p2)
        {
            string id = p1;
            //发送指令查询料仓设备状态

            if (id.Length == 0)
                return 0;
            if (p2.Equals("镜头除尘"))
            {
                string data_search = Data.Data(comboBox4.Text, id, "32", "0000");
                //ins_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));
                string data = Data.Data(comboBox4.Text, id, "22", "0000");
                aim_ins.Enqueue(new FacMessage(0, "17", id, false, 6, "清洁镜头--除尘", data, 0));
                sendIns_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "自动镜头除尘前查询状态", data_search, 3, s_Produce));
            }
            else if (p2.Equals("镜头除湿"))
            {
                string data_search = Data.Data(comboBox4.Text, id, "32", "0000");
                //ins_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));
                string data = Data.Data(comboBox4.Text, id, "22", "0001");
                aim_ins.Enqueue(new FacMessage(0, "17", id, false, 6, "清洁镜头--除湿", data, 0));
                sendIns_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "自动镜头除湿前查询状态", data_search, 3, s_Produce));
            }
            return 1;
        }

        private void save_backdata(object obj)
        {
            file_mutex.WaitOne();
            {
                string path = System.Windows.Forms.Application.StartupPath;
                if (Directory.Exists(path + "\\back_data") == false)
                    Directory.CreateDirectory(path + "\\back_data");
                FileStream fs = new FileStream(path + "\\back_data\\" + backdata_path, FileMode.Create | FileMode.Append);
                if (fs != null)
                {
                    StreamWriter sw = new StreamWriter(fs);
                    for (int i = 0; i < 200; i++)
                    {
                        if (backdata[i].length.Equals("") == false)
                        {
                            sw.Write(i.ToString() + " " + backdata[i].length + " " + backdata[i].schedule + "\r\n");

                        }
                    }
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                    recv_num = 0;
                }
                else
                {
                    richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n文件打开失败\r\n\r\n");
                }
            }
            port_mask = 0;
            file_mutex.ReleaseMutex();
        }

        private int time_on(string obj)
        {//定时盘库，到达设定时间自动发送盘库指令
            string id = (string)obj;
            //发送指令查询料仓设备状态

            if (id.Length == 0)
                return 0;

            string d = Data.Data(comboBox4.Text, id, "18", "0000");

            aim_ins.Enqueue(new FacMessage(0, "13", id, false, TIME_WAIT, "盘库", d, 3, s_Produce));
            string data_search = Data.Data(comboBox4.Text, id, "32", "0000");
            //向发送链表中添加此指令，但是不发送这条指令，发送的是查询指令
            sendIns_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, TIME_WAIT, "自动盘库前查询状态", data_search, 3, s_Produce));

            if (ins_num > 2000)
            {
                ins_num = 1;
            }
            return 1;
        }

        /// <summary>
        /// 根据BinName找BinID
        /// </summary>
        /// <returns></returns>
        private string selectID(string str)
        {
            string ret = "";
            if (SqlConnect == 1)
            {
                string sql = "select * from [bininfo] where [BinName] = '" + str + "'";
                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                db.Dr = db.command.ExecuteReader();
                while (db.Dr.Read())
                {
                    ret = db.Dr["BinID"].ToString();
                }
                db.Dr.Close();
                db.Close();
                return ret;
            }
            else
            {
                new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                //MessageBox.Show("", "提示");
                return "";
            }
        }

        /// <summary>
        /// 修改参数函数
        /// </summary>
        private int parameter(string type, string num, string binid)
        {
            int value = 0;//修改成功为1， 修改失败为0
            string sql = "update [bininfo] set [" + type + "] = " + num + " where [BinID] = " + binid;
            try
            {

                DataBase db = new DataBase();
                db.command.CommandText = sql;
                db.command.Connection = db.connection;
                if (db.command.ExecuteNonQuery() > 0)
                {
                    contextMenuStrip1.Visible = false;
                    value = 1;
                }
                db.Close();
                return value;
            }
            catch (Exception se)
            {
                //Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                //thread_file.Start(se.ToString());
                new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                //MessageBox.Show("\r\n", "提示");
                //richTextBox1.AppendText(se.ToString() + "\r\n");
                return value;
            }

        }

        /// <summary>
        /// 设置直径输入框回车键事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (curr_user.admin.Equals("0"))
                {
                    new Thread(new ParameterizedThreadStart(showBox)).Start("没有管理员权限");
                    //MessageBox.Show("", "提示");
                    contextMenuStrip1.Visible = false;
                }
                else
                {
                    if (toolStripTextBox2.Text.Trim().Equals("") == false)
                    {
                        try
                        {
                            float data = float.Parse(toolStripTextBox2.Text);
                            if (data >= 1 && data < 50)
                            {
                                string str = Data.Data(comboBox4.Text, selectID(checkedListBox1.SelectedItem.ToString()), "02", (data * 100).ToString());
                                serialPort_WriteLine(new FacMessage(ins_num++, "03", selectID(checkedListBox1.SelectedItem.ToString()), false, TIME, "设置直径", str, s_Produce));

                                if (ins_num > 2000)
                                {
                                    ins_num = 1;
                                }

                                Invoke(new MethodInvoker(delegate()
                                {

                                    //让文本框获取焦点，不过注释这行也能达到效果
                                    richTextBox1.Focus();
                                    //设置光标的位置到文本尾   
                                    richTextBox1.Select(richTextBox1.TextLength, 0);
                                    //滚动到控件光标处   
                                    richTextBox1.ScrollToCaret();
                                    richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n对 " + checkedListBox1.SelectedItem.ToString() + "料仓设置了直径\r\n\r\n");
                                }));
                                contextMenuStrip1.Visible = false;
                            }
                            else
                            {

                                MessageBox.Show("直径的范围是0到50(不包含)米", "提示");
                                contextMenuStrip1.Visible = false;
                            }

                        }
                        catch (FormatException fe)
                        {
                            MessageBox.Show("请正确输入直径信息,单位是米", "提示");
                            contextMenuStrip1.Visible = false;
                        }

                    }
                    else
                    {//判断是否输入为空
                        MessageBox.Show("请输入要修改的参数", "提示");
                        contextMenuStrip1.Visible = false;
                    }
                }

            }


        }

        /// <summary>
        /// 设置高度输入框回车键事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (curr_user.admin.Equals("0"))
                {
                    MessageBox.Show("没有管理员权限", "提示");
                }
                else
                {
                    if (toolStripTextBox3.Text.Trim().Equals("") == false)
                    {
                        try
                        {
                            float data = float.Parse(toolStripTextBox3.Text);
                            if (data >= 1 && data < 100)
                            {
                                string str = Data.Data(comboBox4.Text, selectID(checkedListBox1.SelectedItem.ToString()), "04", (data * 100).ToString());
                                //MessageBox.Show(str);
                                serialPort_WriteLine(new FacMessage(ins_num++, "05", selectID(checkedListBox1.SelectedItem.ToString()), false, TIME, "设置高度", str, s_Produce));

                                if (ins_num > 2000)
                                {
                                    ins_num = 1;
                                }

                                Invoke(new MethodInvoker(delegate()
                                {
                                    //让文本框获取焦点，不过注释这行也能达到效果
                                    richTextBox1.Focus();
                                    //设置光标的位置到文本尾   
                                    richTextBox1.Select(richTextBox1.TextLength, 0);
                                    //滚动到控件光标处   
                                    richTextBox1.ScrollToCaret();
                                    richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n对 " + checkedListBox1.SelectedItem.ToString() + "料仓设置了仓筒高度\r\n\r\n");
                                }));
                                contextMenuStrip1.Visible = false;
                            }
                            else
                            {
                                MessageBox.Show("高度的范围是0到100(不包含)米", "提示");
                                contextMenuStrip1.Visible = false;
                            }

                        }
                        catch (FormatException fe)
                        {
                            MessageBox.Show("请正确输入高度信息，单位是米", "提示");
                            contextMenuStrip1.Visible = false;
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入要修改的参数", "提示");
                        contextMenuStrip1.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// 设置下锥输入框回车键事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBox5_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (curr_user.admin.Equals("0"))
                {
                    MessageBox.Show("没有管理员权限", "提示");
                }
                else
                {
                    if (toolStripTextBox5.Text.Trim().Equals("") == false)
                    {
                        try
                        {
                            float data = float.Parse(toolStripTextBox5.Text);
                            if (data > 0 && data < 40)
                            {
                                string str = Data.Data(comboBox4.Text, selectID(checkedListBox1.SelectedItem.ToString()), "06", (data * 100).ToString());
                                //MessageBox.Show(str);
                                serialPort_WriteLine(new FacMessage(ins_num++, "07", selectID(checkedListBox1.SelectedItem.ToString()), false, TIME, "设置下锥高度", str, s_Produce));


                                if (ins_num > 2000)
                                {
                                    ins_num = 1;
                                }

                                Invoke(new MethodInvoker(delegate()
                                {
                                    //让文本框获取焦点，不过注释这行也能达到效果
                                    richTextBox1.Focus();
                                    //设置光标的位置到文本尾   
                                    richTextBox1.Select(richTextBox1.TextLength, 0);
                                    //滚动到控件光标处   
                                    richTextBox1.ScrollToCaret();
                                    richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n对 " + checkedListBox1.SelectedItem.ToString() + "料仓设置了下锥高度\r\n\r\n");
                                }));
                                contextMenuStrip1.Visible = false;
                            }
                            else
                            {
                                MessageBox.Show("下锥高度的范围是0（不包含）到40（不包含）米", "提示");
                                contextMenuStrip1.Visible = false;
                            }

                        }
                        catch (FormatException fe)
                        {
                            MessageBox.Show("请正确输入下锥盖度信息，单位是米", "提示");
                            contextMenuStrip1.Visible = false;
                        }

                    }
                    else
                    {
                        MessageBox.Show("请输入要修改的参数", "提示");
                        contextMenuStrip1.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// 设置密度输入框回车键事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (curr_user.admin.Equals("0"))
                {
                    MessageBox.Show("没有管理员权限", "提示");
                }
                else
                {
                    if (toolStripTextBox4.Text.Trim().Equals("") == false)
                    {
                        try
                        {
                            float data = float.Parse(toolStripTextBox4.Text);
                            string str = Data.Data(comboBox4.Text, selectID(checkedListBox1.SelectedItem.ToString()), "08", (data * 1000).ToString());
                            //MessageBox.Show(str);
                            serialPort_WriteLine(new FacMessage(ins_num++, "09", selectID(checkedListBox1.SelectedItem.ToString()), false, TIME, "设置密度", str, s_Produce));

                            if (ins_num > 2000)
                            {
                                ins_num = 1;
                            }

                            Invoke(new MethodInvoker(delegate()
                            {
                                //让文本框获取焦点，不过注释这行也能达到效果
                                richTextBox1.Focus();
                                //设置光标的位置到文本尾   
                                richTextBox1.Select(richTextBox1.TextLength, 0);
                                //滚动到控件光标处   
                                richTextBox1.ScrollToCaret();
                                richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n对  " + checkedListBox1.SelectedItem.ToString() + "  料仓设置了密度\r\n\r\n");
                            }));
                            contextMenuStrip1.Visible = false;
                        }
                        catch (FormatException fe)
                        {
                            MessageBox.Show("请正确输入密度信息，单位是百分比", "提示");
                        }
                    }
                    else
                    {
                        MessageBox.Show("请输入要修改的参数", "提示");
                    }
                }
            }
        }


        private void 显示盘库时间ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SW = SystemInformation.WorkingArea.Width;
            int SH = SystemInformation.WorkingArea.Height;
            double SW_percent = (double)SW / (double)1366;
            double SH_percent = (double)SH / (double)738;
            bdnInfo.Visible = true;
            groupBox_pic.Visible = false;
            dataGridView1.Visible = true;
            button8.Location = new Point((int)(886 * SW_percent), button8.Location.Y);
            button8.Visible = true;
            //Thread thread_select = new Thread(new ParameterizedThreadStart(select));
            //thread_select.Start("time");
            select("time");
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (curr_user.admin.Equals("1") || curr_user.admin.Equals("0"))
            {
                if (port_isopen == 1)
                {
                    Thread a = new Thread(display);
                    a.Start();

                }
            }
        }


        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {//菜单栏中的盘库按钮
            button1.PerformClick();
        }



        /// <summary>
        /// 料位监控按钮点击事件功能实现
        /// 图形化显示某一个料仓的情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            int select_item = checkedListBox1.CheckedItems.Count + checkedListBox2.CheckedItems.Count;
            if (select_item != 1)
            {
                new Thread(new ParameterizedThreadStart(showBox)).Start("请选择一个料仓进行显示");
                //MessageBox.Show("", "提示");
            }
            else
            {
                int SW = SystemInformation.WorkingArea.Width;
                int SH = SystemInformation.WorkingArea.Height;
                double SW_percent = (double)SW / (double)1366;
                double SH_percent = (double)SH / (double)738;
                button8.PerformClick();
                button8.Visible = true;
                button8.Location = new Point((int)(715 * SW_percent), button8.Location.Y);
                groupBox_pic.Visible = true;
                label5.Text = "0";
                label31.Text = "0";
                label21.Text = "0";
                label24.Text = "0";
                label27.Text = "0";
                if (checkedListBox1.CheckedItems.Count == 1)
                {
                    查询温度ToolStripMenuItem.PerformClick();
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (checkedListBox1.GetItemChecked(i))
                        {
                            if (SqlConnect == 1)
                            {
                                //MessageBox.Show(checkedListBox1.Items[i].ToString());
                                float diameter = 0, cylinderh = 0, pyramidh = 0;
                                label29.Text = checkedListBox1.Items[i].ToString();

                                string id = selectID(checkedListBox1.Items[i].ToString());
                                string sql = "select * from [bininfo] where [BinID] = " + id;
                                try
                                {

                                    DataBase db = new DataBase();
                                    db.command.CommandText = sql;
                                    db.command.Connection = db.connection;
                                    db.Dr = db.command.ExecuteReader();
                                    while (db.Dr.Read())
                                    {
                                        diameter = float.Parse(db.Dr["Diameter"].ToString());
                                        cylinderh = float.Parse(db.Dr["CylinderH"].ToString());
                                        pyramidh = float.Parse(db.Dr["PyramidH"].ToString());
                                    }
                                    db.Dr.Close();
                                    float Vol_ware = (float)((diameter / 2) * (diameter / 2) * (3.14) * (pyramidh / 3 + cylinderh));
                                    label31.Text = Vol_ware.ToString() + "  m³";
                                    //MessageBox.Show(Vol_ware.ToString());
                                    sql = "select * from [bindata] where [BinID] = " + id + "AND [Volume] is not NULL AND [Volume] > 0 order by [DateTime] desc";
                                    db.command.CommandText = sql;
                                    db.Dr = db.command.ExecuteReader();

                                    while (db.Dr.Read())
                                    {
                                        label5.Text = db.Dr["Volume"].ToString() + "  m³";
                                        label21.Text = db.Dr["Weight"].ToString() + "  吨";
                                        float vol_feed = float.Parse(db.Dr["Volume"].ToString());
                                        if (vol_feed < Vol_ware)
                                        {
                                            progressBar1.Value = (int)((vol_feed / Vol_ware) * 100);
                                        }
                                        else
                                        {
                                            progressBar1.Value = 0;
                                            //MessageBox.Show("数据有误", "提示");
                                        }
                                        break;
                                    }
                                    db.Dr.Close();
                                    sql = "select * from [bindata] where [BinID] = " + id + "AND [Temp] is not NULL order by [DateTime] desc";
                                    db.command.CommandText = sql;
                                    db.Dr = db.command.ExecuteReader();
                                    while (db.Dr.Read())
                                    {
                                        label24.Text = db.Dr["Temp"].ToString() + "  ℃";
                                        label27.Text = db.Dr["Hum"].ToString() + "  %";
                                        break;
                                    }

                                    if (label31.Text.Equals("0"))
                                    {
                                        MessageBox.Show("请检测参数设置，当前料仓体积为0", "提示");
                                    }
                                    db.Dr.Close();
                                    db.Close();
                                }
                                catch (SqlException se)
                                {
                                    Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                                    thread_file.Start(se.ToString());
                                    new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                                    //MessageBox.Show("", "提示");
                                }

                            }
                            else//判断数据库是否可以使用else
                            {
                                new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                                //MessageBox.Show("", "提示");
                            }

                        }//判断是否被选中
                    }//循环
                }
                else if (checkedListBox2.CheckedItems.Count == 1)
                {
                    for (int i = 0; i < checkedListBox2.Items.Count; i++)
                    {
                        if (checkedListBox2.GetItemChecked(i))
                        {
                            if (SqlConnect == 1)
                            {
                                //MessageBox.Show(checkedListBox1.Items[i].ToString());
                                float diameter = 0, cylinderh = 0, pyramidh = 0;
                                label29.Text = checkedListBox2.Items[i].ToString();

                                string id = selectID(checkedListBox2.Items[i].ToString());
                                string sql = "select * from [bininfo] where [BinID] = " + id;
                                try
                                {

                                    DataBase db = new DataBase();
                                    db.command.CommandText = sql;
                                    db.command.Connection = db.connection;
                                    db.Dr = db.command.ExecuteReader();
                                    while (db.Dr.Read())
                                    {
                                        diameter = float.Parse(db.Dr["Diameter"].ToString());
                                        cylinderh = float.Parse(db.Dr["CylinderH"].ToString());
                                        pyramidh = float.Parse(db.Dr["PyramidH"].ToString());
                                    }
                                    db.Dr.Close();
                                    float Vol_ware = (float)((diameter / 2) * (diameter / 2) * (3.14) * (pyramidh / 3 + cylinderh));
                                    label31.Text = Vol_ware.ToString() + "  m³";
                                    //MessageBox.Show(Vol_ware.ToString());
                                    sql = "select * from [bindata] where [BinID] = " + id + "AND [Volume] is not NULL AND [Volume] > 0 order by [DateTime] desc";
                                    db.command.CommandText = sql;
                                    db.Dr = db.command.ExecuteReader();

                                    while (db.Dr.Read())
                                    {
                                        label5.Text = db.Dr["Volume"].ToString() + "  m³";
                                        label21.Text = db.Dr["Weight"].ToString() + "  吨";
                                        float vol_feed = float.Parse(db.Dr["Volume"].ToString());
                                        if (vol_feed < Vol_ware)
                                        {
                                            progressBar1.Value = (int)((vol_feed / Vol_ware) * 100);
                                        }
                                        else
                                        {
                                            progressBar1.Value = 0;
                                            //MessageBox.Show("数据有误", "提示");
                                        }
                                        break;
                                    }
                                    db.Dr.Close();
                                    sql = "select * from [bindata] where [BinID] = " + id + "AND [Temp] is not NULL order by [DateTime] desc";
                                    db.command.CommandText = sql;
                                    db.Dr = db.command.ExecuteReader();
                                    while (db.Dr.Read())
                                    {
                                        label24.Text = db.Dr["Temp"].ToString() + "  ℃";
                                        label27.Text = db.Dr["Hum"].ToString() + "  %";
                                        break;
                                    }

                                    if (label31.Text.Equals("0"))
                                    {
                                        MessageBox.Show("请检测参数设置，当前料仓体积为0", "提示");
                                    }
                                    db.Dr.Close();
                                    db.Close();
                                }
                                catch (SqlException se)
                                {
                                    Thread thread_file = new Thread(new ParameterizedThreadStart(method_file));
                                    thread_file.Start(se.ToString());
                                    new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                                    //MessageBox.Show("", "提示");
                                }

                            }
                            else//判断数据库是否可以使用else
                            {
                                new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库是否创建好");
                                //MessageBox.Show("", "提示");
                            }

                        }//判断是否被选中
                    }//循环

                }

            }//判断是否选中一个
        }

        /// <summary>
        /// 库存查询按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            显示数据信息ToolStripMenuItem.PerformClick();
        }

        /// <summary>
        /// 查询温度按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 查询温度ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (checkedListBox1.CheckedItems.Count == 0)
            {
                //MessageBox.Show("", "提示");
                new Thread(new ParameterizedThreadStart(showBox)).Start("请选择料仓查询温湿度");
                return;
            }


            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string id = selectID(checkedListBox1.Items[i].ToString());
                    string data = Data.Data(comboBox4.Text, id, "16", "0000");
                    //serialPort_WriteLine(new FacMessage(ins_num++, "11", id, false, TIME+5, "查询温湿度", data));
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "11", id, false, TIME, "查询温湿度", data, s_Produce));

                }
            }
            查询温度ToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// 更改名称输入回车键功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripTextBox6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (toolStripTextBox6.Text.Equals("") == false)
                {
                    string id = selectID(checkedListBox1.SelectedItem.ToString());
                    string sql = "select * from [bininfo]";
                    bool isExist = false;
                    DataBase db = new DataBase();
                    db.command.CommandText = sql;
                    db.command.Connection = db.connection;
                    db.Dr = db.command.ExecuteReader();
                    while (db.Dr.Read())
                    {
                        if (db.Dr["BinName"].Equals(toolStripTextBox6.Text))
                            isExist = true;
                    }
                    db.Dr.Close();
                    if (isExist == false)
                    {
                        sql = "update [bininfo] set [BinName] = '" + toolStripTextBox6.Text + "' where [BinID] = " + id.ToString();

                        db.command.CommandText = sql;

                        if (db.command.ExecuteNonQuery() > 0)
                        {
                            Invoke(new MethodInvoker(delegate()
                            {
                                //让文本框获取焦点，不过注释这行也能达到效果
                                richTextBox1.Focus();
                                //设置光标的位置到文本尾   
                                richTextBox1.Select(richTextBox1.TextLength, 0);
                                //滚动到控件光标处   
                                richTextBox1.ScrollToCaret();
                                richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n更改料仓名称成功\r\n\r\n");
                            }));



                            //让文本框获取焦点，不过注释这行也能达到效果
                            richTextBox1.Focus();
                            //设置光标的位置到文本尾   
                            richTextBox1.Select(richTextBox1.TextLength, 0);
                            //滚动到控件光标处   
                            richTextBox1.ScrollToCaret();
                            richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n修改料仓名成功\r\n\r\n");

                        }

                        sql = "update [binauto] set [BinName] = '" + toolStripTextBox6.Text + "' where [BinID] = " + id.ToString();
                        db.command.CommandText = sql;
                        db.command.Connection = db.connection;
                        db.command.ExecuteNonQuery();
                        db.Close();
                        checkedListBox1.Items.Remove(checkedListBox1.SelectedItem.ToString());
                        checkedListBox1.Items.Add(toolStripTextBox6.Text);
                        SortCheckedList(checkedListBox1);
                        contextMenuStrip1.Visible = false;
                        toolStripTextBox6.Text = "";
                        //Thread a = new Thread(display);
                        //a.Start();
                    }
                    else
                    {
                        MessageBox.Show("料仓名重复，请重新输入", "提示");
                        toolStripTextBox6.Text = "";
                    }

                }
                else
                {
                    MessageBox.Show("请输入名称", "提示");
                }

            }
        }

        /// <summary>
        /// 清洁镜头按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 清洁镜头ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "关闭")
            {
                button8.PerformClick();
            }
            if (e.ClickedItem.Text == "上一页")
            {
                pageCurrent--;
                if (pageCurrent <= 0)
                {
                    MessageBox.Show("已经是第一页，请点击“下一页”查看！");
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }

                LoadData();
            }
            if (e.ClickedItem.Text == "下一页")
            {
                pageCurrent++;
                if (pageCurrent > pageCount)
                {
                    MessageBox.Show("已经是最后一页，请点击“上一页”查看！");
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }
                LoadData();
            }
        }

        private void txtCurrentPage_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                if (int.Parse(txtCurrentPage.Text) <= pageCount)
                {
                    pageCurrent = int.Parse(txtCurrentPage.Text);
                    nCurrent = pageSize * (pageCurrent - 1);
                    LoadData();
                }
            }
        }

        private void 取消当前操作ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择料仓进行操作", "提示");
                return;
            }
            //for (int i = send_ins.Count - 1; i >= 0;i-- )
            //    send_ins.RemoveAt(i);
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string id = selectID(checkedListBox1.Items[i].ToString());
                    //ins_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));
                    string data = Data.Data(comboBox4.Text, id, "30", "0000");
                    FacMessage facmes = new FacMessage(ins_num++, "1F", id, false, TIME, "取消当前操作", data, 3, s_Produce);
                    sendIns_queue.Enqueue(facmes);

                    if (ins_num > 2000)
                    {
                        ins_num = 1;
                    }

                }
            }
        }

        public Monitor monitor;
        /// <summary>
        /// 进入监控按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 进入监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //oper_ins.Clear();
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择料仓进行监控", "提示");
                return;
            }

            t_monitor.Enabled = true;
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string id = selectID(checkedListBox1.Items[i].ToString());
                    string data_search = Data.Data(comboBox4.Text, id, "32", "0000");
                    //ins_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));
                    string data = Data.Data(comboBox4.Text, id, "26", "0001");
                    aim_ins.Enqueue(new FacMessage(0, "1B", id, false, 6, "进入监控", data, s_Produce - 3595));
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "料仓监控前查询状态", data_search, 3, s_Produce - 3595));
                    //serialPort_WriteLine(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));

                }
            }
        }

        private void show_monitor_method(object obj)
        {
            MethodInvoker meth = new MethodInvoker(show_monitor);
            BeginInvoke(meth);
        }

        private void show_monitor()
        {
            monitor.Visible = true;
        }

        private void inquire_height(object sender, System.Timers.ElapsedEventArgs e)
        {//向中控发送查询高度指令函数
            for (int i = 0; i < comboBox6.Items.Count; i++)
            {
                string data = Data.Data(comboBox4.Text, selectID(comboBox6.Items[i].ToString()), "28", "0000");
                serialPort_WriteLine(new FacMessage(ins_num++, "1D", selectID(comboBox6.Items[i].ToString()), false, TIME, "高度信息", data, s_Produce));

                if (ins_num > 2000)
                {
                    ins_num = 1;
                }

            }
        }

        private void 退出监控ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择料仓进行退出监控", "提示");
                return;
            }


            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string id = selectID(checkedListBox1.Items[i].ToString());
                    string data = Data.Data(comboBox4.Text, id, "26", "0000");
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "1B", id, false, TIME, "退出监控状态", data, s_Produce));
                    if (ins_num > 2000)
                    {
                        ins_num = 1;
                    }

                }
            }
        }


        private void 导出数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfdExport = new SaveFileDialog();
                sfdExport.Filter = "文本文件|*.txt";
                if (sfdExport.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                FileStream fileStream = new FileStream(sfdExport.FileName, FileMode.Append);
                StreamWriter streamWriter = new StreamWriter(fileStream);
                DataBase db = new DataBase();

                int sumLine = 0;
                using (db.command = db.connection.CreateCommand())
                {
                    db.command.CommandText = "select * from [bindata]";
                    using (db.Dr = db.command.ExecuteReader())
                    {
                        while (db.Dr.Read())
                        {
                            sumLine++;
                            string BinID = db.Dr["BinID"].ToString();
                            string Volume = db.Dr["Volume"].ToString();
                            string Weight = db.Dr["Weight"].ToString();
                            string Temp = db.Dr["Temp"].ToString();
                            string Hum = db.Dr["Hum"].ToString();
                            string DateTime = db.Dr["DateTime"].ToString();
                            streamWriter.WriteLine(BinID + "|" + Volume + "|" + Weight + "|" + Temp + "|" + Hum + "|" + DateTime);
                        }
                        db.Dr.Close();
                    }
                    db.Close();
                    streamWriter.Flush();

                }

                //让文本框获取焦点，不过注释这行也能达到效果
                richTextBox1.Focus();
                //设置光标的位置到文本尾   
                richTextBox1.Select(richTextBox1.TextLength, 0);
                //滚动到控件光标处   
                richTextBox1.ScrollToCaret();
                richTextBox1.AppendText(System.DateTime.Now.ToString() + "\r\n导出成功！共导出数据:" + sumLine.ToString() + "\r\n\r\n");

            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败！错误信息：" + ex.Message);
            }
        }

        private ChartForm chart = new ChartForm();

        //显示图表窗体
        private void 图标显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread t_chart = new Thread(showchart_method);
            t_chart.Start();

        }

        private void showchart_method(object obj)
        {
            MethodInvoker meth = new MethodInvoker(show_chart);
            BeginInvoke(meth);
        }

        private void show_chart()
        {

            chart.Show();
            chart.Visible = true;
        }

        private void 打开监控界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread thread_monitor = new Thread(show_monitor_method);
            thread_monitor.Start();
        }

        private void 导入数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int sumLine = 0;//记录读取的行数
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "文本文档|*.txt";

            if (ofd.ShowDialog() != DialogResult.OK)
            {//如果没有选择文件直接返回
                return;
            }

            string fileName = ofd.FileName;
            try
            {
                DateTime startTime = DateTime.Now;

                StreamReader sr = new StreamReader(fileName, Encoding.Default);
                DataBase db = new DataBase();
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    sumLine++;
                    string[] strs = line.Split('|');
                    if (strs.Length != 6)//如果没有分隔成5个字符串，说明读取失败
                        continue;
                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i] == "")
                            strs[i] = "NULL";
                    }
                    string BinID = strs[0];
                    string Volume = strs[1];
                    string Weight = strs[2];
                    string Temp = strs[3];
                    string Hum = strs[4];
                    string dateTime = strs[5];
                    string sql = "insert into [bindata] (BinID, Volume, Weight, Temp, Hum, DateTime)" +
                                            " values (" + BinID + ", " + Volume + ", " + Weight + ", " + Temp + ", " + Hum + ", '" + dateTime + "');";
                    db.command.CommandText = sql;
                    db.command.Connection = db.connection;
                    db.command.ExecuteNonQuery();
                }
                TimeSpan ts = DateTime.Now - startTime;

                //让文本框获取焦点，不过注释这行也能达到效果
                richTextBox1.Focus();
                //设置光标的位置到文本尾   
                richTextBox1.Select(richTextBox1.TextLength, 0);
                //滚动到控件光标处   
                richTextBox1.ScrollToCaret();
                richTextBox1.AppendText(DateTime.Now.ToString() + "\r\n导入数据完成，共花费时间:" + ts.ToString() + "\r\n\r\n");
            }
            catch (Exception exc)
            {
                new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库设置");
                //MessageBox.Show("", "提示");
            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InitializationUpdate iu = new InitializationUpdate();
            iu.NowVersion();
            //iu.DownloadCheckUpdateXml();
            //iu.LatestVersion();

            Version Version = iu.localversion;
            FileInfo finfo = new FileInfo(System.Windows.Forms.Application.StartupPath + "\\Warehouse.exe");
            //MessageBox.Show("新版本功能：");
            new Thread(new ParameterizedThreadStart(showBox)).Start("版本信息: " + iu.localversion + "\r\n\r\n更新时间: " + finfo.LastAccessTime.ToString("yyyy-MM-dd"));
            //MessageBox.Show("版本信息: " + iu.localversion + "\r\n\r\n更新时间: " + finfo.LastAccessTime.ToString("yyyy-MM-dd"));
            

        }
        private void 系统使用帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("C:\\Program Files (x86)\\Internet Explorer\\iexplore.exe",System.Windows.Forms.Application.StartupPath+ "\\UserDoc\\UserDoc.html"); 
            System.Diagnostics.Process.Start("explorer.exe", System.Windows.Forms.Application.StartupPath + "\\UserDoc\\UserDoc.html");

        }
        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (checkedListBox2.IndexFromPoint(
                checkedListBox2.PointToClient(Cursor.Position).X,
                checkedListBox2.PointToClient(Cursor.Position).Y) == -1)
            {
                e.NewValue = e.CurrentValue;
            }
        }

        private void checkedListBox2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

                if (checkedListBox2.Text.Equals(""))
                //if(checkedListBox2.CheckedItems.Count == 0)
                {
                    重试连接ToolStripMenuItem.Visible = false;
                    删除料仓ToolStripMenuItem.Visible = false;
                    显示盘库时间ToolStripMenuItem.Visible = false;
                }
                else
                {
                    重试连接ToolStripMenuItem.Visible = true;
                    删除料仓ToolStripMenuItem.Visible = true;
                    显示盘库时间ToolStripMenuItem.Visible = true;

                }
            }
        }

        private void 重试连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkedListBox2.CheckedItems.Count != 0)
            {
                Queue<FacMessage> ins_queue = new Queue<FacMessage>();
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    if (checkedListBox2.GetItemChecked(i))
                    {
                        string id = selectID(checkedListBox2.Items[i].ToString());
                        string data_search = Data.Data(comboBox4.Text, id, "00", "0000");
                        //ins_queue.Enqueue(new FacMessage(ins_num++, "01", id, false, 3, "查询连接", data_search));
                        sendIns_queue.Enqueue(new FacMessage(ins_num++, "01", id, false, 3, "查询连接", data_search, s_Produce));
                    }
                } //end for
            }
            else
            {
                MessageBox.Show("请选择料仓进行重试连接", "提示");
            }
        }

        /// <summary>
        /// 关闭串口界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            groupBox_serial.Visible = false;
        }

        private void 开启回传数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (comboBox3.Items.Count == 0 && comboBox5.Items.Count == 0 && comboBox6.Items.Count == 0)
            {
                if (checkedListBox1.CheckedItems.Count != 1)
                {
                    MessageBox.Show("请选择一个料仓进行回传数据  " + checkedListBox1.CheckedItems.Count.ToString(), "提示");
                    return;
                }
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        string data = Data.Data(comboBox4.Text, selectID(checkedListBox1.Items[i].ToString()), "38", "0001");
                        while (true)
                        {
                            Thread.Sleep(20);
                            if (list_status.Count == 0)
                            {
                                try
                                {
                                    serialPort1.WriteLine(data);
                                    break;

                                }
                                catch (Exception exc) { break; }
                            }
                        }
                    }
                }
            }
            else
            {
                new Thread(new ParameterizedThreadStart(showBox)).Start("有料仓正在操作，请稍后再试");
                //MessageBox.Show(, "提示");
            }
        }

        /// <summary>
        /// 获取设备的基础信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 获取基础信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string id = selectID(checkedListBox1.Items[i].ToString());

                    //ins_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));
                    string d = Data.Data(comboBox4.Text, id, "10", "0000");
                    //serialPort_WriteLine(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "0B", id, false, 3, "获取料仓信息", d, s_Produce));

                    d = Data.Data(comboBox4.Text, id, "12", "0000");
                    //serialPort_WriteLine(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "0C", id, false, 3, "获取料仓信息", d, s_Produce));

                    d = Data.Data(comboBox4.Text, id, "14", "0000");
                    //serialPort_WriteLine(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "0D", id, false, 3, "获取料仓信息", d, s_Produce));
                }

            }
        }

        private void 直接查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string id = selectID(checkedListBox1.Items[i].ToString());
                    string data = Data.Data(comboBox4.Text, id, "34", "0000");
                    Thread.Sleep(100);
                    while (true)
                    {
                        Thread.Sleep(20);
                        if (list_status.Count == 0)
                        {
                            try
                            {
                                serialPort1.WriteLine(data);
                                break;

                            }
                            catch (Exception exc) { break; }
                        }
                    }
                }
            }

        }

        private void toolStripTextBox2_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripTextBox2.AutoToolTip = false;
            toolStripTextBox2.ToolTipText = "范围是0到50米";
        }

        private void toolStripTextBox3_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripTextBox3.AutoToolTip = false;
            toolStripTextBox3.ToolTipText = "范围是0到100米";
        }

        private void toolStripTextBox5_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripTextBox5.AutoToolTip = false;
            toolStripTextBox5.ToolTipText = "范围是0到40米";
        }

        private void toolStripTextBox4_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripTextBox4.AutoToolTip = false;
            toolStripTextBox4.ToolTipText = "范围是0到15吨/m³";
        }

        /// <summary>
        /// 镜头除尘模式按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 镜头除尘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //oper_ins.Clear();
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择料仓进行清洁镜头", "提示");
                return;
            }


            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string id = selectID(checkedListBox1.Items[i].ToString());
                    string data_search = Data.Data(comboBox4.Text, id, "32", "0000");
                    //ins_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));
                    string data = Data.Data(comboBox4.Text, id, "22", "0000");
                    aim_ins.Enqueue(new FacMessage(0, "17", id, false, 6, "清洁镜头--除尘", data, s_Produce));
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "镜头除尘前查询状态", data_search, 3, s_Produce));
                    //serialPort_WriteLine(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));

                    if (ins_num > 2000)
                    {
                        ins_num = 1;
                    }
                }
            }
        }

        /// <summary>
        /// 镜头除湿模式按钮点击事件功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 镜头除湿ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //oper_ins.Clear();
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请选择料仓进行清洁镜头", "提示");
                return;
            }
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    string id = selectID(checkedListBox1.Items[i].ToString());
                    string data_search = Data.Data(comboBox4.Text, id, "32", "0000");
                    //ins_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));
                    string data = Data.Data(comboBox4.Text, id, "22", "0001");
                    aim_ins.Enqueue(new FacMessage(0, "17", id, false, 6, "清洁镜头--除湿", data, s_Produce));
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "21", id, false, 3, "镜头除湿前查询状态", data_search, 3, s_Produce));
                    //serialPort_WriteLine(new FacMessage(ins_num++, "21", id, false, 3, "查询状态", data_search));

                }
            }
        }

        private void 删除料仓ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (checkedListBox2.CheckedItems.Count != 0)
            {
                int del = 0;
                MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
                DialogResult dr = MessageBox.Show("确认要删除" + checkedListBox2.CheckedItems.Count + "个料仓吗？", "提示", messButton);
                if (dr == DialogResult.OK)
                {
                    int d = 0;
                    for (int i = checkedListBox2.Items.Count - 1; i >= 0; i--)
                    {
                        if (checkedListBox2.GetItemChecked(i))
                        {
                            d = delete(checkedListBox2.GetItemText(checkedListBox2.Items[i].ToString()));
                            del++;
                            Invoke(new MethodInvoker(delegate()
                            {
                                //让文本框获取焦点，不过注释这行也能达到效果
                                richTextBox1.Focus();
                                //设置光标的位置到文本尾   
                                richTextBox1.Select(richTextBox1.TextLength, 0);
                                //滚动到控件光标处   
                                richTextBox1.ScrollToCaret();
                                richTextBox1.AppendText(DateTime.Now.ToString("G") + "\r\n删除了料仓" + checkedListBox2.GetItemText(checkedListBox2.Items[i].ToString()) + "\r\n\r\n");
                                if (d != 0)
                                {
                                    checkedListBox2.Items.RemoveAt(i);
                                }
                            }));
                        }
                    }
                    if (del != 0)
                    {
                        //Thread a = new Thread(display);//启动时显示粮仓线程
                        //a.Start();
                        //让文本框获取焦点，不过注释这行也能达到效果
                        richTextBox1.Focus();
                        //设置光标的位置到文本尾   
                        richTextBox1.Select(richTextBox1.TextLength, 0);
                        //滚动到控件光标处   
                        richTextBox1.ScrollToCaret();
                        richTextBox1.AppendText(DateTime.Now.ToString() + "\r\n成功删除" + del + "个料仓\r\n\r\n");
                    }
                    else
                    {
                        new Thread(new ParameterizedThreadStart(showBox)).Start("存在数据表删除失败，请检查数据库");
                        //MessageBox.Show(, "提示");
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择料仓进行删除", "提示");
            }

        }

        private void 当前数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3.PerformClick();
        }

        private void 历史数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button4.PerformClick();
        }

        /// <summary>
        /// 定时器，盘库时定时查询状态，来确定盘库是否完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            new Thread(timer2_on).Start();
        }

        private void timer2_on(object obj)
        {
            if (send_ins.Count == 0)
                return;

            for (int i = send_ins.Count - 1; i >= 0; i--)
            {
                send_ins[i].life_time--;
                if (send_ins[i].life_time <= 0)
                {//如果到达20分钟，则盘库超时
                    string name = getName(send_ins[i].fac_num);
                    comboBox3.Items.Remove(name);
                    for (int it = CalcVol_list.Count - 1; it >= 0; it--)
                    {
                        if (send_ins[i].fac_num.PadLeft(2, '0').Equals(CalcVol_list[it].fac_num))
                        {
                            CalcVol_list.RemoveAt(it);
                            break;
                        }
                    }
                    string id = selectID(name);
                    // 如果盘库失败 取消料仓操作
                    string data = Data.Data(comboBox4.Text, id, "30", "0000");
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "1F", id, false, TIME, "取消当前操作", data, 2, s_Produce));

                    string searchTemp = Data.Data(comboBox4.Text, id, "16", "0000");
                    //serialPort_WriteLine(new FacMessage(ins_num++, "11", id, false, TIME+5, "查询温湿度", data));
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "11", id, false, TIME, "查询温湿度", searchTemp, s_Produce));

                    send_ins.RemoveAt(i);
                    if (comboBox3.Items.Count == 0)
                    {
                        comboBox3.Visible = false;
                        label3.Visible = false;
                        //groupBox2.Visible = false;
                        progressBar2.Value = 0;
                        label19.Text = "0";
                    }
                    else
                        comboBox3.Text = comboBox3.Items[0].ToString();
                    //让文本框获取焦点，不过注释这行也能达到效果
                    richTextBox1.Focus();
                    //设置光标的位置到文本尾   
                    richTextBox1.Select(richTextBox1.TextLength, 0);
                    //滚动到控件光标处   
                    richTextBox1.ScrollToCaret();
                    richTextBox1.AppendText(DateTime.Now.ToString() + "\r\n" + name + "  盘库超时\r\n\r\n");

                    DateTime now = DateTime.Now;
                    string time = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

                    //超时将体积 重量设置为-1， -1存数据表
                    string sql = "insert into [bindata] (BinID, Volume, Weight, DateTime) values(" + id + ", -1, -1, '" + time + "')";
                    DataBase db = new DataBase();
                    db.command.CommandText = sql;
                    db.command.Connection = db.connection;
                    if (db.command.ExecuteNonQuery() > 0)
                    {
                        new Thread(new ParameterizedThreadStart(showBox)).Start(name + "  盘库超时");
                        //让文本框获取焦点，不过注释这行也能达到效果
                        richTextBox1.Focus();
                        //设置光标的位置到文本尾   
                        richTextBox1.Select(richTextBox1.TextLength, 0);
                        //滚动到控件光标处   
                        richTextBox1.ScrollToCaret();
                        richTextBox1.AppendText(DateTime.Now.ToString() + "\r\n" + name + "  盘库超时\r\n");
            
                        //MessageBox.Show(, "提示");
                    }
                    db.Close();

                    //超时信息存日志表
                    DataBase dbLog = new DataBase();
                    sql = "insert into [binlog] values('" + id + "', '盘库超时', '" + "TimeOut" + "', '盘库超时', '" + time + "')";
                    dbLog.command.CommandText = sql;
                    db.command.Connection = dbLog.connection;
                    dbLog.command.ExecuteNonQuery();
                    dbLog.Close();
                }
                else
                {//如果没有到达盘库超时时间，则发送查询指令
                    string data = Data.Data(comboBox4.Text, send_ins[i].fac_num, "34", "0000");
                    //MessageBox.Show(d);
                    aim_ins.Enqueue(new FacMessage(ins_num++, "23", send_ins[i].fac_num, false, TIME_WAIT, "查询结果", data, s_Produce));

                    string data_search = Data.Data(comboBox4.Text, send_ins[i].fac_num, "32", "0000");
                    //向发送链表中添加此指令，但是不发送这条指令，发送的是查询状态指令
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "21", send_ins[i].fac_num, false, TIME_WAIT, "获取盘库结果前查询状态", data_search, s_Produce));

                    if (ins_num > 2000)
                    {
                        ins_num = 0;
                    }
                }

             

            }
        }

        private void toolStripTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripTextBox1.AutoToolTip = false;
            toolStripTextBox1.ToolTipText = "输入料仓编号";
        }

        /// <summary>
        /// 清洁镜头定时器功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clean_timer_Tick(object sender, EventArgs e)
        {
            if (comboBox5.Items.Count == 0)
            {
                try
                {
                    label33.Visible = false;
                    comboBox5.Visible = false;

                }
                catch (Exception exc) { }
            }
            else
            {
                label33.Visible = true;
                comboBox5.Visible = true;


                if (clean_list.Count != 0)
                {
                    for (int i = clean_list.Count - 1; i >= 0; i--)
                    {
                        clean_list[i].life_time--;

                        //richTextBox1.AppendText(clean_list[i].fac_name + "  " + clean_list[i].life_time + "\r\n");
                        if (clean_list[i].life_time <= 0)
                        {
                            try
                            {
                                comboBox5.Items.Remove(clean_list[i].fac_name);
                                for (int j = CalcVol_list.Count - 1; j >= 0; j--)
                                {
                                    if (CalcVol_list[j].fac_num.PadLeft(2, '0').Equals(clean_list[i].fac_name.PadLeft(2, '0')))
                                    {
                                        CalcVol_list.RemoveAt(j);
                                        break;
                                    }
                                }
                                clean_list.RemoveAt(i);
                            }
                            catch (Exception exc) { }
                        }
                    }
                }
            }

        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Selected)
                {
                    try
                    {
                        DataBase db = new DataBase();
                        db.command.Connection = db.connection;
                        string sql = "delete from [binauto] where [BinID] = " + dataGridView1.Rows[i].Cells["BinID"].Value
                            + " AND [Time] = '" + dataGridView1.Rows[i].Cells["Time"].Value + "'";
                        db.command.CommandText = sql;
                        db.command.ExecuteNonQuery();

                        db.Close();
                    }
                    catch (Exception exc)
                    {
                        new Thread(new ParameterizedThreadStart(showBox)).Start("请检查数据库设置");

                        //MessageBox.Show("", "提示");
                    }
                }
            }
            Thread loadAuto = new Thread(LoadAuto);
            loadAuto.Start();
            显示盘库时间ToolStripMenuItem.PerformClick();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void 料位图形显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread thread_form1 = new Thread(InvokeShowForm1);
            thread_form1.Start();
        }

        /// <summary>
        /// 料位图形显示
        /// </summary>
        /// <param name="obj"></param>
        private void InvokeShowForm1(object obj)
        {
            MethodInvoker MethInvo = new MethodInvoker(show_form1);
            BeginInvoke(MethInvo);
        }

        private void show_form1()
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        /// <summary>
        /// 显示盘库进度的定时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void show_timer_Tick(object sender, EventArgs e)
        {
            if (CalcVol_list.Count != 0)
            {
                if (it_oper >= CalcVol_list.Count)
                {//CalcVol_list的下标it_oper如果大于CalcVol_list的节点个数，就置为0
                    it_oper = 0;
                }
                groupBox2.Visible = true;
                //if (CalcVol_list[it_oper].ins_num == 2)
                //{
                //    CalcVol_list[it_oper].life_time += 3;
                //}
                for (int i = 0; i < CalcVol_list.Count; i++)
                {//遍历找出清洁镜头的节点
                    if (CalcVol_list[i].ins_num == 2)
                        CalcVol_list[i].life_time += 3;
                }
                string fac_num = CalcVol_list[it_oper].fac_num;
                int schedule = CalcVol_list[it_oper].life_time;//操作进度
                int OperType = CalcVol_list[it_oper].ins_num;//操作类型，1表示盘库，2表示清洁镜头
                label6.Text = getName(fac_num);
                if (schedule >= 100)
                {
                    schedule = 100;
                    CalcVol_list.RemoveAt(it_oper);
                }
                progressBar2.Value = schedule;
                label19.Text = schedule.ToString();
                if (1 == OperType)
                {
                    label22.Text = "料仓盘库";
                }
                else if (2 == OperType)
                {
                    label22.Text = "清洁镜头";
                }
                it_oper++;
            }
            else
            {
                it_oper = 0;
                groupBox2.Visible = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                //this.ShowInTaskbar = true;  //显示在系统任务栏
                this.WindowState = FormWindowState.Maximized;  //还原窗体
                notifyIcon1.Visible = false;  //托盘图标隐藏
            }
        }

        private void pC管理软件升级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Thread(ConnectServer).Start();
        }
        /// <summary>
        /// 检查更新
        /// </summary>
        private void ConnectServer()
        {
            //检查更新
            InitializationUpdate iu = new InitializationUpdate();
            iu.NowVersion();
            iu.DownloadCheckUpdateXml();
            iu.LatestVersion();

            //MessageBox.Show("新版本功能：");
            if (iu.latesversion != iu.localversion)
            {
                Process.Start(System.Windows.Forms.Application.StartupPath + "\\Update.exe");
            }
            else
            {
                MessageBox.Show("已经是最新版本，不需要更新");
            }
        }

        /// <summary>
        /// 检测料仓在线的定时器功能实现
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnlineTimer_Tick(object sender, EventArgs e)
        {
            OnlineCheak();
        }

        private void OnlineCheak()
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                string id = selectID(checkedListBox1.Items[i].ToString());
                string data = Data.Data(comboBox4.Text, id, "00", "0000");
                sendIns_queue.Enqueue(new FacMessage(ins_num++, "01", id, false, TIME, "查询测试/添加料仓功能", data, s_Produce));
            }
            for (int i = 0; i < checkedListBox2.Items.Count; i++)
            {
                string id = selectID(checkedListBox2.Items[i].ToString());
                string data = Data.Data(comboBox4.Text, id, "00", "0000");
                sendIns_queue.Enqueue(new FacMessage(ins_num++, "01", id, false, TIME, "查询测试/添加料仓功能", data, s_Produce));
            }
        }

        private void 重启全套设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
            {
                //bool isoperating = false;
                if (checkedListBox1.GetItemChecked(i))
                {
                    //MessageBox.Show(checkedListBox1.Items[i].ToString());
                    string id = selectID(checkedListBox1.Items[i].ToString());
                    string d = Data.Data(comboBox4.Text, id, "20", "0000");
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "15", id, false, 3, "重启全套设备", d, 3, s_Produce));

                }
            } //end for
        }
        private void 重启中控设备ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
            {
                //bool isoperating = false;
                if (checkedListBox1.GetItemChecked(i))
                {
                    //MessageBox.Show(checkedListBox1.Items[i].ToString());
                    string id = selectID(checkedListBox1.Items[i].ToString());
                    string d = Data.Data(comboBox4.Text, id, "24", "0000");
                    sendIns_queue.Enqueue(new FacMessage(ins_num++, "19", id, false, 3, "重启中控设备", d, 3, s_Produce));

                }
            } //end for
        }
    }
}
