using System.IO.Ports;
using System.Windows.Forms;
namespace Warehouse
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //int SW = Screen.PrimaryScreen.Bounds.Width;
            //int SH = Screen.PrimaryScreen.Bounds.Height;
            int SW = SystemInformation.WorkingArea.Width;
            int SH = SystemInformation.WorkingArea.Height;
            double SW_percent = (double)SW / (double)1366;
            double SH_percent = (double)SH / (double)738;
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();//菜单栏
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();//菜单栏中料仓操作菜单
            this.料仓盘库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料仓操作中的料仓盘库
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();//料仓操作菜单栏中的盘库按钮
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();//料仓操作菜单栏中的定时盘库按钮
            this.料位监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料仓操作菜单栏中的料位监控按钮
            this.进入监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料位监控下的进入监控
            this.退出监控ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料位监控下的退出监控按钮
            this.打开监控界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料位监控下的打开监控界面按钮
            this.查询温度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料仓操作下的查询温度按钮
            this.清洁镜头ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料仓操作下的清洁镜头按钮
            this.镜头除尘ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//清洁镜头下的镜头除尘
            this.镜头除湿ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//清洁镜头下的镜头除湿
            this.取消当前操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料仓操作下的取消当前操作按钮
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();//料仓数据菜单
            this.当前数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料仓数据下的当前数据按钮
            this.历史数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料仓数据下的历史数据按钮
            this.图标显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//料仓数据下的数据分析按钮
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();//菜单栏中的用户管理按钮
            this.管理员密码修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//用户管理中的密码修改按钮
            this.普通用户管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//用户管理中的普通用户管理按钮
            this.用户添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//普通用户管理下的用户添加
            this.用户删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//普通用户管理下的用户删除
            this.用户初始化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//普通用户管理下的用户初始化
            this.退出登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//用户管理中的退出登录按钮
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();//菜单栏中的系统管理菜单
            this.串口选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//通信管理中的通信设置按钮
            this.软件升级ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//通信管理中的软件升级按钮
            this.pC管理软件升级ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//软件升级下的PC管理软件升级按钮
            this.料仓主控软件升级ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//软件升级下的料仓主控软件升级按钮
            this.测量前端软件升级ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//软件升级下的测量前端软件升级按钮
            this.服务器设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//系统管理下的服务器设置按钮
            this.系统使用帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//系统管理下的系统使用帮助按钮
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//系统管理下的关于按钮
            this.数据库管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//系统管理下的数据库管理按钮
            this.导出数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//数据库管理下的导出数据按钮
            this.导入数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//数据管理下的导入数据按钮
            this.开启回传数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//系统管理下的开启回传数据按钮
            this.料位图形显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//系统管理下的料位图形显示按钮
            this.获取基础信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//系统管理下的获取基础信息按钮
            this.直接查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//系统管理下的直接查询按钮
            this.groupBox1 = new System.Windows.Forms.GroupBox();//常用功能区域
            this.button4 = new System.Windows.Forms.Button();//历史数据按钮
            this.button3 = new System.Windows.Forms.Button();//当前数据按钮
            this.button2 = new System.Windows.Forms.Button();//定时盘库按钮
            this.button1 = new System.Windows.Forms.Button();//盘库按钮
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();//在线料仓显示区域
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);//在线料仓显示右键菜单
            this.test2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//右键菜单的修改参数选单
            this.仓筒直径ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//修改参数中的仓筒直径按钮
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();//修改直径输入框
            this.仓筒高度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//修改参数中的仓筒高度按钮
            this.toolStripTextBox3 = new System.Windows.Forms.ToolStripTextBox();//仓筒高度输入框
            this.下锥高度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//修改参数中的下锥高度按钮
            this.toolStripTextBox5 = new System.Windows.Forms.ToolStripTextBox();//下锥高度输入框
            this.物料密度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//修改参数中的物料密度按钮
            this.toolStripTextBox4 = new System.Windows.Forms.ToolStripTextBox();//物料密度输入框
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//右键菜单中的刷新按钮
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//右键菜单中的删除按钮
            this.显示数据信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//右键菜单中的显示数据信息按钮
            this.显示参数信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//右键菜单中的显示参数信息
            this.添加料仓ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//右键菜单中的添加料仓按钮
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();//添加料仓的料仓编号输入框
            this.更改名称ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//右键菜单中的更改名称按钮
            this.toolStripTextBox6 = new System.Windows.Forms.ToolStripTextBox();//更改名称的新名称输入框
            this.显示盘库时间ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//右键菜单中的显示盘库时间按钮
            this.groupBox3 = new System.Windows.Forms.GroupBox();//操作和显示区域
            this.button7 = new System.Windows.Forms.Button();//清空按钮
            this.button8 = new System.Windows.Forms.Button();//图表关闭按钮，界面上的“X”
            this.bdnInfo = new System.Windows.Forms.BindingNavigator(this.components);//表格下面的操作菜单
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();//表格当前页的数据个数
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();//跳转到当前页的第一个数据
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();//上一个数据
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();//操作菜单中自带的一个控件，没用到
            this.toolStripTextBox7 = new System.Windows.Forms.ToolStripTextBox();//数据跳转输入框
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();//操作菜单中自带的一个控件，没用到
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();//下一个数据
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();//跳转到当前页中最后一个数据
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();//上一页按钮
            this.txtCurrentPage = new System.Windows.Forms.ToolStripTextBox();//当前页
            this.lblPageCount = new System.Windows.Forms.ToolStripLabel();//总页数
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();//下一页
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();//表格操作菜单的关闭按钮
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();//表格操作菜单的删除数据按钮
            this.groupBox_pic = new System.Windows.Forms.GroupBox();//显示当前数据区域
            this.label29 = new System.Windows.Forms.Label();//当前数据区域中显示料仓名标签
            this.label27 = new System.Windows.Forms.Label();//当前数据区域中湿度数值显示
            this.label24 = new System.Windows.Forms.Label();//当前数据区域中温度数值显示
            this.label21 = new System.Windows.Forms.Label();//当前数据区域中重量数值显示
            this.label31 = new System.Windows.Forms.Label();//当前数据区域料仓总体积数值显示
            this.label5 = new System.Windows.Forms.Label();//当前数据区域当前体积数值显示
            this.label26 = new System.Windows.Forms.Label();//当前数据区域中“湿度”文字显示
            this.label23 = new System.Windows.Forms.Label();//当前数据区域中“温度”文字显示
            this.label20 = new System.Windows.Forms.Label();//当前数据区域中“重量”文字显示
            this.label30 = new System.Windows.Forms.Label();//当前数据区域中“料仓体积”文字显示
            this.label4 = new System.Windows.Forms.Label();//当前数据区域中“体积”文字显示
            this.pictureBox1 = new System.Windows.Forms.PictureBox();//显示图片控件
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();//历史信息输出框
            this.dataGridView1 = new System.Windows.Forms.DataGridView();//数据表显示控件
            this.groupBox_serial = new System.Windows.Forms.GroupBox();//串口设置显示区域
            this.button6 = new System.Windows.Forms.Button();//串口显示区域关闭按钮
            this.button5 = new System.Windows.Forms.Button();//串口显示区域确定按钮
            this.comboBox2 = new System.Windows.Forms.ComboBox();//串口波特率显示
            this.comboBox1 = new System.Windows.Forms.ComboBox();//串口名显示
            this.label2 = new System.Windows.Forms.Label();//串口“波特率”显示标签
            this.label1 = new System.Windows.Forms.Label();//串口“名称”显示标签
            this.groupBox_changepass = new System.Windows.Forms.GroupBox();//修改密码显示区域
            this.textBox7 = new System.Windows.Forms.TextBox();//确认密码输入框
            this.textBox6 = new System.Windows.Forms.TextBox();//新密码输入框
            this.textBox1 = new System.Windows.Forms.TextBox();//原密码输入框
            this.button11 = new System.Windows.Forms.Button();//修改密码中取消按钮
            this.button10 = new System.Windows.Forms.Button();//修改密码中确认按钮
            this.label10 = new System.Windows.Forms.Label();//修改密码中“确认密码”显示标签
            this.label9 = new System.Windows.Forms.Label();//修改密码中“新密码”显示标签
            this.label8 = new System.Windows.Forms.Label();//修改密码中“原密码”显示标签
            this.groupBox_adduser = new System.Windows.Forms.GroupBox();//用户添加显示区域
            this.textBox10 = new System.Windows.Forms.TextBox();//用户添加中的确认密码输入框
            this.textBox9 = new System.Windows.Forms.TextBox();//用户添加中的用户密码输入框
            this.textBox8 = new System.Windows.Forms.TextBox();//用户添加中的用户名输入框
            this.button13 = new System.Windows.Forms.Button();//用户添加中的取消按钮
            this.button12 = new System.Windows.Forms.Button();//用户添加中的确认按钮
            this.label13 = new System.Windows.Forms.Label();//用户添加中的“确认密码”标签
            this.label12 = new System.Windows.Forms.Label();//用户添加中的“用户密码”标签
            this.label11 = new System.Windows.Forms.Label();//用户添加中的“用户名”标签
            this.groupBox_deleteuser = new System.Windows.Forms.GroupBox();//删除用户区域显示
            this.comboBox7 = new System.Windows.Forms.ComboBox();//用户删除中的用户名选单
            this.button15 = new System.Windows.Forms.Button();//用户删除中的取消按钮
            this.button14 = new System.Windows.Forms.Button();//用户删除中的确认按钮
            this.textBox12 = new System.Windows.Forms.TextBox();//用户删除中的确认按钮
            this.label15 = new System.Windows.Forms.Label();//用户删除中的“管理员密码”显示标签
            this.label14 = new System.Windows.Forms.Label();//用户删除中的“用户名”显示标签
            this.groupBox_init = new System.Windows.Forms.GroupBox();//初始化用户显示区域
            this.comboBox8 = new System.Windows.Forms.ComboBox();//用户初始化中的用户名选单
            this.textBox14 = new System.Windows.Forms.TextBox();//用户初始化中的新密码输入框
            this.button17 = new System.Windows.Forms.Button();//用户初始化中的取消按钮
            this.button16 = new System.Windows.Forms.Button();//用户初始化中的确认按钮
            this.label17 = new System.Windows.Forms.Label();//用户初始化中的“新密码”标签
            this.label16 = new System.Windows.Forms.Label();//用户初始化中的“用户名”标签
            this.groupBox4 = new System.Windows.Forms.GroupBox();//当前厂区显示区域
            this.comboBox4 = new System.Windows.Forms.ComboBox();//厂区码显示列表
            this.groupBox8 = new System.Windows.Forms.GroupBox();//当前用户显示区域
            this.label7 = new System.Windows.Forms.Label();//当前用户显示标签
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);//串口控件
            this.timer1 = new System.Windows.Forms.Timer(this.components);//自动盘库定时器
            this.OnlineTimer = new System.Windows.Forms.Timer(this.components);//五分钟检测料仓的状态
            this.bdsInfo = new System.Windows.Forms.BindingSource(this.components);
            this.comboBox3 = new System.Windows.Forms.ComboBox();//正在盘库列表显示
            this.comboBox5 = new System.Windows.Forms.ComboBox();//正在清洁镜头列表显示
            this.comboBox6 = new System.Windows.Forms.ComboBox();//正在监控列表显示
            this.label3 = new System.Windows.Forms.Label();//“正在盘库”标签
            this.label33 = new System.Windows.Forms.Label();//“正在清洁镜头”标签
            this.label34 = new System.Windows.Forms.Label();//“正在监控”标签
            this.label35 = new System.Windows.Forms.Label();//“在线料仓列表显示”标签
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();//不在线料仓显示区域
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);//不在线料仓区域右键菜单
            this.重试连接ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//不在线料仓区域右键菜单重试连接按钮
            this.显示数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//不在线料仓区域右键菜单显示数据按钮
            this.显示参数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//不在线料仓区域右键菜单显示参数按钮
            this.删除料仓ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//不在线料仓区域右键菜单删除料仓按钮
            this.show_timer = new System.Windows.Forms.Timer(this.components);//显示盘库进度定时器
            this.label18 = new System.Windows.Forms.Label();//“不在线料仓列表显示”标签
            this.timer2 = new System.Windows.Forms.Timer(this.components);//盘库过程查状态，判断是否盘库完成
            this.clean_timer = new System.Windows.Forms.Timer(this.components);//清洁镜头过程倒计时
            this.progressBar1 = new Warehouse.VerticalProgressBar();//料仓体积容量垂直进度条显示
            this.groupBox2 = new System.Windows.Forms.GroupBox();//盘库进度显示区域
            this.label6 = new System.Windows.Forms.Label();//盘库进度料仓名显示标签
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);//状态栏中的图标显示
            this.progressBar2 = new System.Windows.Forms.ProgressBar();//操作进度的进度条显示
            this.label19 = new System.Windows.Forms.Label();//操作进度的文字显示
            this.label22 = new System.Windows.Forms.Label();//操作进度的类型显示
            this.重启设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//重启设备按钮
            this.重启全套设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//重启中控扫描
            this.重启中控设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();//重启中控设备
            
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdnInfo)).BeginInit();
            this.bdnInfo.SuspendLayout();
            this.groupBox_pic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox_serial.SuspendLayout();
            this.groupBox_changepass.SuspendLayout();
            this.groupBox_adduser.SuspendLayout();
            this.groupBox_deleteuser.SuspendLayout();
            this.groupBox_init.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsInfo)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem4,
            this.toolStripMenuItem1,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size((int)(1354*SW_percent), (int)(27*SH_percent));
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.料仓盘库ToolStripMenuItem,
            this.料位监控ToolStripMenuItem,
            this.查询温度ToolStripMenuItem,
            this.清洁镜头ToolStripMenuItem,
            this.取消当前操作ToolStripMenuItem});
            this.toolStripMenuItem4.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size((int)(77*SW_percent), (int)(23*SH_percent));
            this.toolStripMenuItem4.Text = "料仓操作";
            // 
            // 料仓盘库ToolStripMenuItem
            // 
            this.料仓盘库ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem2});
            this.料仓盘库ToolStripMenuItem.Name = "料仓盘库ToolStripMenuItem";
            this.料仓盘库ToolStripMenuItem.Size = new System.Drawing.Size((int)(162*SW_percent), (int)(24*SH_percent));
            this.料仓盘库ToolStripMenuItem.Text = "料仓盘库";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.toolStripMenuItem3.Text = "盘库";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.toolStripMenuItem2.Text = "定时盘库";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // 料位监控ToolStripMenuItem
            // 
            this.料位监控ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.进入监控ToolStripMenuItem,
            this.退出监控ToolStripMenuItem,
            this.打开监控界面ToolStripMenuItem});
            this.料位监控ToolStripMenuItem.Name = "料位监控ToolStripMenuItem";
            this.料位监控ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.料位监控ToolStripMenuItem.Text = "料位监控";
            // 
            // 进入监控ToolStripMenuItem
            // 
            this.进入监控ToolStripMenuItem.Name = "进入监控ToolStripMenuItem";
            this.进入监控ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.进入监控ToolStripMenuItem.Text = "进入监控";
            this.进入监控ToolStripMenuItem.Click += new System.EventHandler(this.进入监控ToolStripMenuItem_Click);
            // 
            // 退出监控ToolStripMenuItem
            // 
            this.退出监控ToolStripMenuItem.Name = "退出监控ToolStripMenuItem";
            this.退出监控ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.退出监控ToolStripMenuItem.Text = "退出监控";
            this.退出监控ToolStripMenuItem.Click += new System.EventHandler(this.退出监控ToolStripMenuItem_Click);
            // 
            // 打开监控界面ToolStripMenuItem
            // 
            this.打开监控界面ToolStripMenuItem.Name = "打开监控界面ToolStripMenuItem";
            this.打开监控界面ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.打开监控界面ToolStripMenuItem.Text = "打开监控界面";
            this.打开监控界面ToolStripMenuItem.Click += new System.EventHandler(this.打开监控界面ToolStripMenuItem_Click);
            // 
            // 查询温度ToolStripMenuItem
            // 
            this.查询温度ToolStripMenuItem.Name = "查询温度ToolStripMenuItem";
            this.查询温度ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.查询温度ToolStripMenuItem.Text = "查询温湿度";
            this.查询温度ToolStripMenuItem.Click += new System.EventHandler(this.查询温度ToolStripMenuItem_Click);
            // 
            // 清洁镜头ToolStripMenuItem
            // 
            this.清洁镜头ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.镜头除尘ToolStripMenuItem,
            this.镜头除湿ToolStripMenuItem});
            this.清洁镜头ToolStripMenuItem.Name = "清洁镜头ToolStripMenuItem";
            this.清洁镜头ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.清洁镜头ToolStripMenuItem.Text = "清洁镜头";
            this.清洁镜头ToolStripMenuItem.Click += new System.EventHandler(this.清洁镜头ToolStripMenuItem_Click);
            // 
            // 镜头除尘ToolStripMenuItem
            // 
            this.镜头除尘ToolStripMenuItem.Name = "镜头除尘ToolStripMenuItem";
            this.镜头除尘ToolStripMenuItem.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.镜头除尘ToolStripMenuItem.Text = "镜头除尘";
            this.镜头除尘ToolStripMenuItem.Click += new System.EventHandler(this.镜头除尘ToolStripMenuItem_Click);
            // 
            // 镜头除湿ToolStripMenuItem
            // 
            this.镜头除湿ToolStripMenuItem.Name = "镜头除湿ToolStripMenuItem";
            this.镜头除湿ToolStripMenuItem.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.镜头除湿ToolStripMenuItem.Text = "镜头除湿";
            this.镜头除湿ToolStripMenuItem.Click += new System.EventHandler(this.镜头除湿ToolStripMenuItem_Click);
            // 
            // 取消当前操作ToolStripMenuItem
            // 
            this.取消当前操作ToolStripMenuItem.Name = "取消当前操作ToolStripMenuItem";
            this.取消当前操作ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.取消当前操作ToolStripMenuItem.Text = "取消当前操作";
            this.取消当前操作ToolStripMenuItem.Click += new System.EventHandler(this.取消当前操作ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.当前数据ToolStripMenuItem,
            this.历史数据ToolStripMenuItem,
            this.图标显示ToolStripMenuItem});
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size((int)(77 * SW_percent), (int)(23 * SH_percent));
            this.toolStripMenuItem1.Text = "料仓数据";
            // 
            // 当前数据ToolStripMenuItem
            // 
            this.当前数据ToolStripMenuItem.Name = "当前数据ToolStripMenuItem";
            this.当前数据ToolStripMenuItem.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.当前数据ToolStripMenuItem.Text = "当前数据";
            this.当前数据ToolStripMenuItem.Click += new System.EventHandler(this.当前数据ToolStripMenuItem_Click);
            // 
            // 历史数据ToolStripMenuItem
            // 
            this.历史数据ToolStripMenuItem.Name = "历史数据ToolStripMenuItem";
            this.历史数据ToolStripMenuItem.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.历史数据ToolStripMenuItem.Text = "历史数据";
            this.历史数据ToolStripMenuItem.Click += new System.EventHandler(this.历史数据ToolStripMenuItem_Click);
            // 
            // 图标显示ToolStripMenuItem
            // 
            this.图标显示ToolStripMenuItem.Name = "图标显示ToolStripMenuItem";
            this.图标显示ToolStripMenuItem.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.图标显示ToolStripMenuItem.Text = "数据分析";
            this.图标显示ToolStripMenuItem.Click += new System.EventHandler(this.图标显示ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.管理员密码修改ToolStripMenuItem,
            this.普通用户管理ToolStripMenuItem,
            this.退出登录ToolStripMenuItem});
            this.toolStripMenuItem5.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size((int)(77 * SW_percent), (int)(23 * SH_percent));
            this.toolStripMenuItem5.Text = "用户管理";
            // 
            // 管理员密码修改ToolStripMenuItem
            // 
            this.管理员密码修改ToolStripMenuItem.Name = "管理员密码修改ToolStripMenuItem";
            this.管理员密码修改ToolStripMenuItem.Size = new System.Drawing.Size(162, 24);
            this.管理员密码修改ToolStripMenuItem.Text = "密码修改";
            this.管理员密码修改ToolStripMenuItem.Click += new System.EventHandler(this.管理员密码修改ToolStripMenuItem_Click);
            // 
            // 普通用户管理ToolStripMenuItem
            // 
            this.普通用户管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.用户添加ToolStripMenuItem,
            this.用户删除ToolStripMenuItem,
            this.用户初始化ToolStripMenuItem});
            this.普通用户管理ToolStripMenuItem.Name = "普通用户管理ToolStripMenuItem";
            this.普通用户管理ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.普通用户管理ToolStripMenuItem.Text = "普通用户管理";
            // 
            // 用户添加ToolStripMenuItem
            // 
            this.用户添加ToolStripMenuItem.Name = "用户添加ToolStripMenuItem";
            this.用户添加ToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(24 * SH_percent));
            this.用户添加ToolStripMenuItem.Text = "用户添加";
            this.用户添加ToolStripMenuItem.Click += new System.EventHandler(this.用户添加ToolStripMenuItem_Click);
            // 
            // 用户删除ToolStripMenuItem
            // 
            this.用户删除ToolStripMenuItem.Name = "用户删除ToolStripMenuItem";
            this.用户删除ToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(24 * SH_percent));
            this.用户删除ToolStripMenuItem.Text = "用户删除";
            this.用户删除ToolStripMenuItem.Click += new System.EventHandler(this.用户删除ToolStripMenuItem_Click);
            // 
            // 用户初始化ToolStripMenuItem
            // 
            this.用户初始化ToolStripMenuItem.Name = "用户初始化ToolStripMenuItem";
            this.用户初始化ToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(24 * SH_percent));
            this.用户初始化ToolStripMenuItem.Text = "用户初始化";
            this.用户初始化ToolStripMenuItem.Click += new System.EventHandler(this.用户初始化ToolStripMenuItem_Click);
            // 
            // 退出登录ToolStripMenuItem
            // 
            this.退出登录ToolStripMenuItem.Name = "退出登录ToolStripMenuItem";
            this.退出登录ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.退出登录ToolStripMenuItem.Text = "退出登录";
            this.退出登录ToolStripMenuItem.Click += new System.EventHandler(this.退出登录ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripMenuItem6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.串口选择ToolStripMenuItem,
            this.软件升级ToolStripMenuItem,
            this.服务器设置ToolStripMenuItem,
            this.系统使用帮助ToolStripMenuItem,
            this.关于ToolStripMenuItem,
            this.数据库管理ToolStripMenuItem,
            this.开启回传数据ToolStripMenuItem,
            this.重启设备ToolStripMenuItem,
            this.获取基础信息ToolStripMenuItem,
            this.直接查询ToolStripMenuItem,
            this.料位图形显示ToolStripMenuItem});
            this.toolStripMenuItem6.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size((int)(77 * SW_percent), (int)(23 * SH_percent));
            this.toolStripMenuItem6.Text = "系统管理";
            // 
            // 串口选择ToolStripMenuItem
            // 
            this.串口选择ToolStripMenuItem.Name = "串口选择ToolStripMenuItem";
            this.串口选择ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.串口选择ToolStripMenuItem.Text = "通信设置";
            this.串口选择ToolStripMenuItem.Visible = false;
            this.串口选择ToolStripMenuItem.Click += new System.EventHandler(this.串口选择ToolStripMenuItem_Click);
            // 
            // 软件升级ToolStripMenuItem
            // 
            this.软件升级ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pC管理软件升级ToolStripMenuItem,
            this.料仓主控软件升级ToolStripMenuItem,
            this.测量前端软件升级ToolStripMenuItem});
            this.软件升级ToolStripMenuItem.Name = "软件升级ToolStripMenuItem";
            this.软件升级ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.软件升级ToolStripMenuItem.Text = "软件升级";
            this.软件升级ToolStripMenuItem.Visible = false;
            // 
            // pC管理软件升级ToolStripMenuItem
            // 
            this.pC管理软件升级ToolStripMenuItem.Name = "pC管理软件升级ToolStripMenuItem";
            this.pC管理软件升级ToolStripMenuItem.Size = new System.Drawing.Size((int)(190 * SW_percent), (int)(24 * SH_percent));
            this.pC管理软件升级ToolStripMenuItem.Text = "PC管理软件升级";
            this.pC管理软件升级ToolStripMenuItem.Click += new System.EventHandler(this.pC管理软件升级ToolStripMenuItem_Click);
            // 
            // 料仓主控软件升级ToolStripMenuItem
            // 
            this.料仓主控软件升级ToolStripMenuItem.Name = "料仓主控软件升级ToolStripMenuItem";
            this.料仓主控软件升级ToolStripMenuItem.Size = new System.Drawing.Size((int)(190 * SW_percent), (int)(24 * SH_percent));
            this.料仓主控软件升级ToolStripMenuItem.Text = "料仓主控软件升级";
            this.料仓主控软件升级ToolStripMenuItem.Visible = false;
            // 
            // 测量前端软件升级ToolStripMenuItem
            // 
            this.测量前端软件升级ToolStripMenuItem.Name = "测量前端软件升级ToolStripMenuItem";
            this.测量前端软件升级ToolStripMenuItem.Size = new System.Drawing.Size((int)(190 * SW_percent), (int)(24 * SH_percent));
            this.测量前端软件升级ToolStripMenuItem.Text = "测量前端软件升级";
            this.测量前端软件升级ToolStripMenuItem.Visible = false;
            // 
            // 服务器设置ToolStripMenuItem
            // 
            this.服务器设置ToolStripMenuItem.Name = "服务器设置ToolStripMenuItem";
            this.服务器设置ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.服务器设置ToolStripMenuItem.Text = "服务器设置";
            this.服务器设置ToolStripMenuItem.Visible = false;
            // 
            // 系统使用帮助ToolStripMenuItem
            // 
            this.系统使用帮助ToolStripMenuItem.Name = "系统使用帮助ToolStripMenuItem";
            this.系统使用帮助ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.系统使用帮助ToolStripMenuItem.Text = "系统使用帮助";
            this.系统使用帮助ToolStripMenuItem.Click += new System.EventHandler(this.系统使用帮助ToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 数据库管理ToolStripMenuItem
            // 
            this.数据库管理ToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.数据库管理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出数据ToolStripMenuItem,
            this.导入数据ToolStripMenuItem});
            this.数据库管理ToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.数据库管理ToolStripMenuItem.Name = "数据库管理ToolStripMenuItem";
            this.数据库管理ToolStripMenuItem.RightToLeftAutoMirrorImage = true;
            this.数据库管理ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.数据库管理ToolStripMenuItem.Text = "数据库管理";
            this.数据库管理ToolStripMenuItem.Visible = false;
            // 
            // 导出数据ToolStripMenuItem
            // 
            this.导出数据ToolStripMenuItem.Name = "导出数据ToolStripMenuItem";
            this.导出数据ToolStripMenuItem.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.导出数据ToolStripMenuItem.Text = "导出数据";
            this.导出数据ToolStripMenuItem.Click += new System.EventHandler(this.导出数据ToolStripMenuItem_Click);
            // 
            // 导入数据ToolStripMenuItem
            // 
            this.导入数据ToolStripMenuItem.Name = "导入数据ToolStripMenuItem";
            this.导入数据ToolStripMenuItem.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.导入数据ToolStripMenuItem.Text = "导入数据";
            this.导入数据ToolStripMenuItem.Click += new System.EventHandler(this.导入数据ToolStripMenuItem_Click);
            // 
            // 开启回传数据ToolStripMenuItem
            // 
            this.开启回传数据ToolStripMenuItem.Name = "开启回传数据ToolStripMenuItem";
            this.开启回传数据ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.开启回传数据ToolStripMenuItem.Text = "开启回传数据";
            this.开启回传数据ToolStripMenuItem.Visible = false;
            this.开启回传数据ToolStripMenuItem.Click += new System.EventHandler(this.开启回传数据ToolStripMenuItem_Click);
            // 
            // 获取基础信息ToolStripMenuItem
            // 
            this.获取基础信息ToolStripMenuItem.Name = "获取基础信息ToolStripMenuItem";
            this.获取基础信息ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.获取基础信息ToolStripMenuItem.Text = "获取基础信息";
            this.获取基础信息ToolStripMenuItem.Visible = false;
            this.获取基础信息ToolStripMenuItem.Click += new System.EventHandler(this.获取基础信息ToolStripMenuItem_Click);
            // 
            // 料位图形显示ToolStripMenuItem
            // 
            this.料位图形显示ToolStripMenuItem.Name = "料位图形显示ToolStripMenuItem";
            this.料位图形显示ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.料位图形显示ToolStripMenuItem.Text = "料位图形显示";
            this.料位图形显示ToolStripMenuItem.Visible = true;
            this.料位图形显示ToolStripMenuItem.Click += new System.EventHandler(this.料位图形显示ToolStripMenuItem_Click);
            // 
            // 直接查询ToolStripMenuItem
            // 
            this.直接查询ToolStripMenuItem.Name = "直接查询ToolStripMenuItem";
            this.直接查询ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.直接查询ToolStripMenuItem.Text = "直接查询";
            this.直接查询ToolStripMenuItem.Visible = false;
            this.直接查询ToolStripMenuItem.Click += new System.EventHandler(this.直接查询ToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.ForeColor = System.Drawing.Color.Red;
            this.groupBox1.Location = new System.Drawing.Point((int)(12 * SW_percent), (int)(30 * SH_percent));
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size((int)(562 * SW_percent), (int)(74 * SH_percent));
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "常用功能";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.Control;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button4.Image = ((System.Drawing.Image)(resources.GetObject("button4.Image")));
            this.button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button4.Location = new System.Drawing.Point((int)(419 * SW_percent), (int)(23 * SH_percent));
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size((int)(137 * SW_percent), (int)(41 * SH_percent));
            this.button4.TabIndex = 3;
            this.button4.Text = "历史数据";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.Control;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button3.Location = new System.Drawing.Point((int)(271 * SW_percent), (int)(23 * SH_percent));
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size((int)(137 * SW_percent), (int)(41 * SH_percent));
            this.button3.TabIndex = 2;
            this.button3.Text = "当前数据";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Control;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point((int)(117 * SW_percent), (int)(23 * SH_percent));
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size((int)(137 * SW_percent), (int)(41 * SH_percent));
            this.button2.TabIndex = 1;
            this.button2.Text = "定时功能";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point((int)(11 * SW_percent), (int)(24 * SH_percent));
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(41 * SH_percent));
            this.button1.TabIndex = 0;
            this.button1.Text = "盘库";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.checkedListBox1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Location = new System.Drawing.Point((int)(23 * SW_percent), (int)(142 * SH_percent));
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size((int)(209 * SW_percent), (int)(292 * SH_percent));
            this.checkedListBox1.TabIndex = 0;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            this.checkedListBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.checkedListBox1_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.test2ToolStripMenuItem,
            this.刷新ToolStripMenuItem,
            this.testToolStripMenuItem,
            this.显示数据信息ToolStripMenuItem,
            this.显示参数信息ToolStripMenuItem,
            this.添加料仓ToolStripMenuItem,
            this.更改名称ToolStripMenuItem,
            this.显示盘库时间ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size((int)(149 * SW_percent), (int)(180 * SH_percent));
            // 
            // test2ToolStripMenuItem
            // 
            this.test2ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.仓筒直径ToolStripMenuItem,
            this.仓筒高度ToolStripMenuItem,
            this.下锥高度ToolStripMenuItem,
            this.物料密度ToolStripMenuItem});
            this.test2ToolStripMenuItem.Name = "test2ToolStripMenuItem";
            this.test2ToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(22 * SH_percent));
            this.test2ToolStripMenuItem.Text = "修改参数";
            // 
            // 仓筒直径ToolStripMenuItem
            // 
            this.仓筒直径ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox2});
            this.仓筒直径ToolStripMenuItem.Name = "仓筒直径ToolStripMenuItem";
            this.仓筒直径ToolStripMenuItem.Size = new System.Drawing.Size((int)(165 * SW_percent), (int)(22 * SH_percent));
            this.仓筒直径ToolStripMenuItem.Text = "仓筒直径(米)";
            // 
            // toolStripTextBox2
            // 
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            this.toolStripTextBox2.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(23 * SH_percent));
            this.toolStripTextBox2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox2_KeyUp);
            this.toolStripTextBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolStripTextBox2_MouseMove);
            // 
            // 仓筒高度ToolStripMenuItem
            // 
            this.仓筒高度ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox3});
            this.仓筒高度ToolStripMenuItem.Name = "仓筒高度ToolStripMenuItem";
            this.仓筒高度ToolStripMenuItem.Size = new System.Drawing.Size((int)(165 * SW_percent), (int)(22 * SH_percent));
            this.仓筒高度ToolStripMenuItem.Text = "仓筒高度(米)";
            // 
            // toolStripTextBox3
            // 
            this.toolStripTextBox3.Name = "toolStripTextBox3";
            this.toolStripTextBox3.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(23 * SH_percent));
            this.toolStripTextBox3.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox3_KeyUp);
            this.toolStripTextBox3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolStripTextBox3_MouseMove);
            // 
            // 下锥高度ToolStripMenuItem
            // 
            this.下锥高度ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox5});
            this.下锥高度ToolStripMenuItem.Name = "下锥高度ToolStripMenuItem";
            this.下锥高度ToolStripMenuItem.Size = new System.Drawing.Size((int)(165 * SW_percent), (int)(22 * SH_percent));
            this.下锥高度ToolStripMenuItem.Text = "下锥高度(米)";
            // 
            // toolStripTextBox5
            // 
            this.toolStripTextBox5.Name = "toolStripTextBox5";
            this.toolStripTextBox5.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(23 * SH_percent));
            this.toolStripTextBox5.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox5_KeyUp);
            this.toolStripTextBox5.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolStripTextBox5_MouseMove);
            // 
            // 物料密度ToolStripMenuItem
            // 
            this.物料密度ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox4});
            this.物料密度ToolStripMenuItem.Name = "物料密度ToolStripMenuItem";
            this.物料密度ToolStripMenuItem.Size = new System.Drawing.Size((int)(165 * SW_percent), (int)(22 * SH_percent));
            this.物料密度ToolStripMenuItem.Text = "物料密度(吨/m³)";
            // 
            // toolStripTextBox4
            // 
            this.toolStripTextBox4.Name = "toolStripTextBox4";
            this.toolStripTextBox4.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(23 * SH_percent));
            this.toolStripTextBox4.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox4_KeyUp);
            this.toolStripTextBox4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolStripTextBox4_MouseMove);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(22 * SH_percent));
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(22 * SH_percent));
            this.testToolStripMenuItem.Text = "删除料仓";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // 显示数据信息ToolStripMenuItem
            // 
            this.显示数据信息ToolStripMenuItem.Name = "显示数据信息ToolStripMenuItem";
            this.显示数据信息ToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(22 * SH_percent));
            this.显示数据信息ToolStripMenuItem.Text = "显示数据信息";
            this.显示数据信息ToolStripMenuItem.Click += new System.EventHandler(this.显示数据信息ToolStripMenuItem_Click);
            // 
            // 显示参数信息ToolStripMenuItem
            // 
            this.显示参数信息ToolStripMenuItem.Name = "显示参数信息ToolStripMenuItem";
            this.显示参数信息ToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(22 * SH_percent));
            this.显示参数信息ToolStripMenuItem.Text = "显示参数信息";
            this.显示参数信息ToolStripMenuItem.Click += new System.EventHandler(this.显示详细信息ToolStripMenuItem_Click);
            // 
            // 添加料仓ToolStripMenuItem
            // 
            this.添加料仓ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1});
            this.添加料仓ToolStripMenuItem.Name = "添加料仓ToolStripMenuItem";
            this.添加料仓ToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(22 * SH_percent));
            this.添加料仓ToolStripMenuItem.Text = "添加料仓";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(23 * SH_percent));
            this.toolStripTextBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox1_KeyUp);
            this.toolStripTextBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolStripTextBox1_MouseMove);
            // 
            // 更改名称ToolStripMenuItem
            // 
            this.更改名称ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox6});
            this.更改名称ToolStripMenuItem.Name = "更改名称ToolStripMenuItem";
            this.更改名称ToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(22 * SH_percent));
            this.更改名称ToolStripMenuItem.Text = "更改名称";
            // 
            // toolStripTextBox6
            // 
            this.toolStripTextBox6.Name = "toolStripTextBox6";
            this.toolStripTextBox6.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(23 * SH_percent));
            this.toolStripTextBox6.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox6_KeyUp);
            // 
            // 显示盘库时间ToolStripMenuItem
            // 
            this.显示盘库时间ToolStripMenuItem.Name = "显示盘库时间ToolStripMenuItem";
            this.显示盘库时间ToolStripMenuItem.Size = new System.Drawing.Size((int)(148 * SW_percent), (int)(22 * SH_percent));
            this.显示盘库时间ToolStripMenuItem.Text = "显示定时时间";
            this.显示盘库时间ToolStripMenuItem.Click += new System.EventHandler(this.显示盘库时间ToolStripMenuItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.button7);
            this.groupBox3.Controls.Add(this.button8);
            this.groupBox3.Controls.Add(this.bdnInfo);
            this.groupBox3.Controls.Add(this.groupBox_pic);
            this.groupBox3.Controls.Add(this.richTextBox1);
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Controls.Add(this.groupBox_serial);
            this.groupBox3.Controls.Add(this.groupBox_changepass);
            this.groupBox3.Controls.Add(this.groupBox_adduser);
            this.groupBox3.Controls.Add(this.groupBox_deleteuser);
            this.groupBox3.Controls.Add(this.groupBox_init);
            this.groupBox3.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox3.Location = new System.Drawing.Point((int)(255 * SW_percent), (int)(119 * SH_percent));
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size((int)(1452 * SW_percent), (int)(511 * SH_percent));
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "操作和显示区";
            // 
            // button7
            // 
            this.button7.AllowDrop = true;
            this.button7.Location = new System.Drawing.Point((int)(970 * SW_percent), (int)(477 * SH_percent));
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button7.TabIndex = 16;
            this.button7.Text = "清空";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point((int)(723 * SW_percent), (int)(8 * SH_percent));
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size((int)(22 * SW_percent), (int)(25 * SH_percent));
            this.button8.TabIndex = 13;
            this.button8.Text = "X";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Visible = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // bdnInfo
            // 
            this.bdnInfo.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.bdnInfo.AddNewItem = null;
            this.bdnInfo.CountItem = this.toolStripLabel1;
            this.bdnInfo.DeleteItem = null;
            this.bdnInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bdnInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripTextBox7,
            this.toolStripLabel1,
            this.toolStripSeparator2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.txtCurrentPage,
            this.lblPageCount,
            this.toolStripButton6,
            this.toolStripButton7,
            this.toolStripButton8});
            this.bdnInfo.Location = new System.Drawing.Point((int)(3 * SW_percent), (int)(483 * SH_percent));
            this.bdnInfo.MoveFirstItem = this.toolStripButton1;
            this.bdnInfo.MoveLastItem = this.toolStripButton4;
            this.bdnInfo.MoveNextItem = this.toolStripButton3;
            this.bdnInfo.MovePreviousItem = this.toolStripButton2;
            this.bdnInfo.Name = "bdnInfo";
            this.bdnInfo.PositionItem = this.toolStripTextBox7;
            this.bdnInfo.Size = new System.Drawing.Size((int)(973 * SW_percent), (int)(25 * SH_percent));
            this.bdnInfo.TabIndex = 15;
            this.bdnInfo.Text = "bindingNavigator1";
            this.bdnInfo.Visible = false;
            this.bdnInfo.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.bdnInfo_ItemClicked);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size((int)(32 * SW_percent), (int)(22 * SH_percent));
            this.toolStripLabel1.Text = "/ {0}";
            this.toolStripLabel1.ToolTipText = "总项数";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.RightToLeftAutoMirrorImage = true;
            this.toolStripButton1.Size = new System.Drawing.Size((int)(23 * SW_percent), (int)(22 * SH_percent));
            this.toolStripButton1.Text = "移到第一条记录";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.RightToLeftAutoMirrorImage = true;
            this.toolStripButton2.Size = new System.Drawing.Size((int)(23 * SW_percent), (int)(22 * SH_percent));
            this.toolStripButton2.Text = "移到上一条记录";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size((int)(6 * SW_percent), (int)(25 * SH_percent));
            // 
            // toolStripTextBox7
            // 
            this.toolStripTextBox7.AccessibleName = "位置";
            this.toolStripTextBox7.AutoSize = false;
            this.toolStripTextBox7.Name = "toolStripTextBox7";
            this.toolStripTextBox7.Size = new System.Drawing.Size((int)(50 * SW_percent), (int)(23 * SH_percent));
            this.toolStripTextBox7.Text = "0";
            this.toolStripTextBox7.ToolTipText = "当前位置";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size((int)(6 * SW_percent), (int)(25 * SH_percent));
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.RightToLeftAutoMirrorImage = true;
            this.toolStripButton3.Size = new System.Drawing.Size((int)(23 * SW_percent), (int)(22 * SH_percent));
            this.toolStripButton3.Text = "移到下一条记录";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.RightToLeftAutoMirrorImage = true;
            this.toolStripButton4.Size = new System.Drawing.Size((int)(23 * SW_percent), (int)(22 * SH_percent));
            this.toolStripButton4.Text = "移到最后一条记录";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size((int)(48 * SW_percent), (int)(22 * SH_percent));
            this.toolStripButton5.Text = "上一页";
            // 
            // txtCurrentPage
            // 
            this.txtCurrentPage.AutoSize = false;
            this.txtCurrentPage.Name = "txtCurrentPage";
            this.txtCurrentPage.Size = new System.Drawing.Size((int)(50 * SW_percent), (int)(25 * SH_percent));
            this.txtCurrentPage.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCurrentPage_KeyUp);
            // 
            // lblPageCount
            // 
            this.lblPageCount.Name = "lblPageCount";
            this.lblPageCount.Size = new System.Drawing.Size((int)(20 * SW_percent), (int)(22 * SH_percent));
            this.lblPageCount.Text = "/5";
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size((int)(48 * SW_percent), (int)(22 * SH_percent));
            this.toolStripButton6.Text = "下一页";
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size((int)(36 * SW_percent), (int)(22 * SH_percent));
            this.toolStripButton7.Text = "关闭";
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size((int)(60 * SW_percent), (int)(22 * SH_percent));
            this.toolStripButton8.Text = "删除数据";
            this.toolStripButton8.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // groupBox_pic
            // 
            this.groupBox_pic.AutoSize = true;
            this.groupBox_pic.BackColor = System.Drawing.Color.White;
            this.groupBox_pic.Controls.Add(this.label29);
            this.groupBox_pic.Controls.Add(this.label27);
            this.groupBox_pic.Controls.Add(this.label24);
            this.groupBox_pic.Controls.Add(this.label21);
            this.groupBox_pic.Controls.Add(this.label31);
            this.groupBox_pic.Controls.Add(this.label5);
            this.groupBox_pic.Controls.Add(this.label26);
            this.groupBox_pic.Controls.Add(this.label23);
            this.groupBox_pic.Controls.Add(this.label20);
            this.groupBox_pic.Controls.Add(this.label30);
            this.groupBox_pic.Controls.Add(this.label4);
            this.groupBox_pic.Controls.Add(this.progressBar1);
            this.groupBox_pic.Controls.Add(this.pictureBox1);
            this.groupBox_pic.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox_pic.Location = new System.Drawing.Point((int)(12 * SW_percent), (int)(26 * SH_percent));
            this.groupBox_pic.Name = "groupBox_pic";
            this.groupBox_pic.Size = new System.Drawing.Size((int)(703 * SW_percent), (int)(485 * SH_percent));
            this.groupBox_pic.TabIndex = 11;
            this.groupBox_pic.TabStop = false;
            this.groupBox_pic.Visible = false;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label29.Location = new System.Drawing.Point((int)(97 * SW_percent), (int)(77 * SH_percent));
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size((int)(103 * SW_percent), (int)(26 * SH_percent));
            this.label29.TabIndex = 9;
            this.label29.Text = "       ";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label27.Location = new System.Drawing.Point((int)(369 * SW_percent), (int)(331 * SH_percent));
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size((int)(76 * SW_percent), (int)(22 * SH_percent));
            this.label27.TabIndex = 7;
            this.label27.Text = "      ";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label24.Location = new System.Drawing.Point((int)(369 * SW_percent), (int)(280 * SH_percent));
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size((int)(76 * SW_percent), (int)(22 * SH_percent));
            this.label24.TabIndex = 7;
            this.label24.Text = "      ";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.Location = new System.Drawing.Point((int)(369 * SW_percent), (int)(228 * SH_percent));
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size((int)(76 * SW_percent), (int)(22 * SH_percent));
            this.label21.TabIndex = 7;
            this.label21.Text = "      ";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label31.Location = new System.Drawing.Point((int)(364 * SW_percent), (int)(137 * SH_percent));
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size((int)(65 * SW_percent), (int)(22 * SH_percent));
            this.label31.TabIndex = 7;
            this.label31.Text = "     ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point((int)(368 * SW_percent), (int)(179 * SH_percent));
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size((int)(76 * SW_percent), (int)(22 * SH_percent));
            this.label5.TabIndex = 7;
            this.label5.Text = "      ";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label26.Location = new System.Drawing.Point((int)(263 * SW_percent), (int)(330 * SH_percent));
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size((int)(65 * SW_percent), (int)(22 * SH_percent));
            this.label26.TabIndex = 6;
            this.label26.Text = "湿度:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label23.Location = new System.Drawing.Point((int)(263 * SW_percent), (int)(280 * SH_percent));
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size((int)(65 * SW_percent), (int)(22 * SH_percent));
            this.label23.TabIndex = 6;
            this.label23.Text = "温度:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label20.Location = new System.Drawing.Point((int)(263 * SW_percent), (int)(226 * SH_percent));
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size((int)(65 * SW_percent), (int)(22 * SH_percent));
            this.label20.TabIndex = 6;
            this.label20.Text = "重量:";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label30.Location = new System.Drawing.Point((int)(263 * SW_percent), (int)(137 * SH_percent));
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size((int)(109 * SW_percent), (int)(22 * SH_percent));
            this.label30.TabIndex = 6;
            this.label30.Text = "料仓体积:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point((int)(263 * SW_percent), (int)(178 * SH_percent));
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size((int)(65 * SW_percent), (int)(22 * SH_percent));
            this.label4.TabIndex = 6;
            this.label4.Text = "物料体积:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point((int)(36 * SW_percent), (int)(21 * SH_percent));
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size((int)(256 * SW_percent), (int)(420 * SH_percent));
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point((int)(907 * SW_percent), (int)(23 * SH_percent));
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size((int)(180 * SW_percent), (int)(448 * SH_percent));
            this.richTextBox1.TabIndex = 12;
            this.richTextBox1.Text = "";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point((int)(12 * SW_percent), (int)(23 * SH_percent));
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size((int)(886 * SW_percent), (int)(450 * SH_percent));
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.Visible = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            // 
            // groupBox_serial
            // 
            this.groupBox_serial.AutoSize = true;
            this.groupBox_serial.Controls.Add(this.button6);
            this.groupBox_serial.Controls.Add(this.button5);
            this.groupBox_serial.Controls.Add(this.comboBox2);
            this.groupBox_serial.Controls.Add(this.comboBox1);
            this.groupBox_serial.Controls.Add(this.label2);
            this.groupBox_serial.Controls.Add(this.label1);
            this.groupBox_serial.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox_serial.Location = new System.Drawing.Point((int)(316 * SW_percent), (int)(73 * SH_percent));
            this.groupBox_serial.Name = "groupBox_serial";
            this.groupBox_serial.Size = new System.Drawing.Size((int)(264 * SW_percent), (int)(171 * SH_percent));
            this.groupBox_serial.TabIndex = 12;
            this.groupBox_serial.TabStop = false;
            this.groupBox_serial.Text = "串口设置";
            this.groupBox_serial.Visible = false;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point((int)(141 * SW_percent), (int)(103 * SH_percent));
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button6.TabIndex = 4;
            this.button6.Text = "关闭";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point((int)(38 * SW_percent), (int)(103 * SH_percent));
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button5.TabIndex = 4;
            this.button5.Text = "确定";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "9600",
            "19200",
            "56000",
            "115200"});
            this.comboBox2.Location = new System.Drawing.Point((int)(87 * SW_percent), (int)(62 * SH_percent));
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size((int)(121 * SW_percent), (int)(23 * SH_percent));
            this.comboBox2.TabIndex = 3;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point((int)(87 * SW_percent), (int)(28 * SH_percent));
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size((int)(121 * SW_percent), (int)(23 * SH_percent));
            this.comboBox1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point((int)(14 * SW_percent), (int)(66 * SH_percent));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size((int)(67 * SW_percent), (int)(15 * SH_percent));
            this.label2.TabIndex = 1;
            this.label2.Text = "波特率：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point((int)(14 * SW_percent), (int)(33 * SH_percent));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size((int)(67 * SW_percent), (int)(15 * SH_percent));
            this.label1.TabIndex = 0;
            this.label1.Text = "串口名：";
            // 
            // groupBox_changepass
            // 
            this.groupBox_changepass.Controls.Add(this.textBox7);
            this.groupBox_changepass.Controls.Add(this.textBox6);
            this.groupBox_changepass.Controls.Add(this.textBox1);
            this.groupBox_changepass.Controls.Add(this.button11);
            this.groupBox_changepass.Controls.Add(this.button10);
            this.groupBox_changepass.Controls.Add(this.label10);
            this.groupBox_changepass.Controls.Add(this.label9);
            this.groupBox_changepass.Controls.Add(this.label8);
            this.groupBox_changepass.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox_changepass.Location = new System.Drawing.Point((int)(35 * SW_percent), (int)(45 * SH_percent));
            this.groupBox_changepass.Name = "groupBox_changepass";
            this.groupBox_changepass.Size = new System.Drawing.Size((int)(356 * SW_percent), (int)(255 * SH_percent));
            this.groupBox_changepass.TabIndex = 0;
            this.groupBox_changepass.TabStop = false;
            this.groupBox_changepass.Text = "修改密码";
            this.groupBox_changepass.Visible = false;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point((int)(123 * SW_percent), (int)(103 * SH_percent));
            this.textBox7.Name = "textBox7";
            this.textBox7.PasswordChar = '*';
            this.textBox7.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(24 * SH_percent));
            this.textBox7.TabIndex = 7;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point((int)(123 * SW_percent), (int)(69 * SH_percent));
            this.textBox6.Name = "textBox6";
            this.textBox6.PasswordChar = '*';
            this.textBox6.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(24 * SH_percent));
            this.textBox6.TabIndex = 6;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point((int)(123 * SW_percent), (int)(38 * SH_percent));
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(24 * SH_percent));
            this.textBox1.TabIndex = 5;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point((int)(133 * SW_percent), (int)(146 * SH_percent));
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button11.TabIndex = 4;
            this.button11.Text = "取消";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point((int)(40 * SW_percent), (int)(146 * SH_percent));
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button10.TabIndex = 3;
            this.button10.Text = "确认";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point((int)(3 * SW_percent), (int)(110 * SH_percent));
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size((int)(82 * SW_percent), (int)(15 * SH_percent));
            this.label10.TabIndex = 2;
            this.label10.Text = "确认密码：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point((int)(14 * SW_percent), (int)(73 * SH_percent));
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size((int)(67 * SW_percent), (int)(15 * SH_percent));
            this.label9.TabIndex = 1;
            this.label9.Text = "新密码：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point((int)(11 * SW_percent), (int)(39 * SH_percent));
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size((int)(67 * SW_percent), (int)(15 * SH_percent));
            this.label8.TabIndex = 0;
            this.label8.Text = "原密码：";
            // 
            // groupBox_adduser
            // 
            this.groupBox_adduser.Controls.Add(this.textBox10);
            this.groupBox_adduser.Controls.Add(this.textBox9);
            this.groupBox_adduser.Controls.Add(this.textBox8);
            this.groupBox_adduser.Controls.Add(this.button13);
            this.groupBox_adduser.Controls.Add(this.button12);
            this.groupBox_adduser.Controls.Add(this.label13);
            this.groupBox_adduser.Controls.Add(this.label12);
            this.groupBox_adduser.Controls.Add(this.label11);
            this.groupBox_adduser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox_adduser.Location = new System.Drawing.Point((int)(36 * SW_percent), (int)(45 * SH_percent));
            this.groupBox_adduser.Name = "groupBox_adduser";
            this.groupBox_adduser.Size = new System.Drawing.Size((int)(355 * SW_percent), (int)(228 * SH_percent));
            this.groupBox_adduser.TabIndex = 8;
            this.groupBox_adduser.TabStop = false;
            this.groupBox_adduser.Text = "用户添加";
            this.groupBox_adduser.Visible = false;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point((int)(146 * SW_percent), (int)(125 * SH_percent));
            this.textBox10.Name = "textBox10";
            this.textBox10.PasswordChar = '*';
            this.textBox10.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(24 * SH_percent));
            this.textBox10.TabIndex = 7;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point((int)(147 * SW_percent), (int)(84 * SH_percent));
            this.textBox9.Name = "textBox9";
            this.textBox9.PasswordChar = '*';
            this.textBox9.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(24 * SH_percent));
            this.textBox9.TabIndex = 6;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point((int)(148 * SW_percent), (int)(41 * SH_percent));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(24 * SH_percent));
            this.textBox8.TabIndex = 5;
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point((int)(140 * SW_percent), (int)(175 * SH_percent));
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button13.TabIndex = 4;
            this.button13.Text = "取消";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point((int)(39 * SW_percent), (int)(175 * SH_percent));
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button12.TabIndex = 3;
            this.button12.Text = "确认";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point((int)(17 * SW_percent), (int)(131 * SH_percent));
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size((int)(82 * SW_percent), (int)(15 * SH_percent));
            this.label13.TabIndex = 2;
            this.label13.Text = "确认密码：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point((int)(17 * SW_percent), (int)(90 * SH_percent));
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size((int)(82 * SW_percent), (int)(15 * SH_percent));
            this.label12.TabIndex = 1;
            this.label12.Text = "用户密码：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point((int)(22 * SW_percent), (int)(48 * SH_percent));
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size((int)(67 * SW_percent), (int)(15 * SH_percent));
            this.label11.TabIndex = 0;
            this.label11.Text = "用户名：";
            // 
            // groupBox_deleteuser
            // 
            this.groupBox_deleteuser.Controls.Add(this.comboBox7);
            this.groupBox_deleteuser.Controls.Add(this.button15);
            this.groupBox_deleteuser.Controls.Add(this.button14);
            this.groupBox_deleteuser.Controls.Add(this.textBox12);
            this.groupBox_deleteuser.Controls.Add(this.label15);
            this.groupBox_deleteuser.Controls.Add(this.label14);
            this.groupBox_deleteuser.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox_deleteuser.Location = new System.Drawing.Point((int)(36 * SW_percent), (int)(45 * SH_percent));
            this.groupBox_deleteuser.Name = "groupBox_deleteuser";
            this.groupBox_deleteuser.Size = new System.Drawing.Size((int)(380 * SW_percent), (int)(280 * SH_percent));
            this.groupBox_deleteuser.TabIndex = 8;
            this.groupBox_deleteuser.TabStop = false;
            this.groupBox_deleteuser.Text = "用户删除";
            this.groupBox_deleteuser.Visible = false;
            // 
            // comboBox7
            // 
            this.comboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox7.Font = new System.Drawing.Font("宋体", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Location = new System.Drawing.Point((int)(146 * SW_percent), (int)(41 * SH_percent));
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size((int)(102 * SW_percent), (int)(25 * SH_percent));
            this.comboBox7.TabIndex = 6;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point((int)(147 * SW_percent), (int)(146 * SH_percent));
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button15.TabIndex = 5;
            this.button15.Text = "取消";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point((int)(47 * SW_percent), (int)(147 * SH_percent));
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button14.TabIndex = 4;
            this.button14.Text = "确认";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point((int)(148 * SW_percent), (int)(86 * SH_percent));
            this.textBox12.Name = "textBox12";
            this.textBox12.PasswordChar = '*';
            this.textBox12.Size = new System.Drawing.Size((int)(100 * SW_percent), (int)(24 * SH_percent));
            this.textBox12.TabIndex = 3;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point((int)(3 * SW_percent), (int)(90 * SH_percent));
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size((int)(97 * SW_percent), (int)(15 * SH_percent));
            this.label15.TabIndex = 1;
            this.label15.Text = "管理员密码：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point((int)(17 * SW_percent), (int)(43 * SH_percent));
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size((int)(67 * SW_percent), (int)(15 * SH_percent));
            this.label14.TabIndex = 0;
            this.label14.Text = "用户名：";
            // 
            // groupBox_init
            // 
            this.groupBox_init.Controls.Add(this.comboBox8);
            this.groupBox_init.Controls.Add(this.textBox14);
            this.groupBox_init.Controls.Add(this.button17);
            this.groupBox_init.Controls.Add(this.button16);
            this.groupBox_init.Controls.Add(this.label17);
            this.groupBox_init.Controls.Add(this.label16);
            this.groupBox_init.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox_init.Location = new System.Drawing.Point((int)(37 * SW_percent), (int)(44 * SH_percent));
            this.groupBox_init.Name = "groupBox_init";
            this.groupBox_init.Size = new System.Drawing.Size((int)(385 * SW_percent), (int)(278 * SH_percent));
            this.groupBox_init.TabIndex = 9;
            this.groupBox_init.TabStop = false;
            this.groupBox_init.Text = "用户初始化";
            this.groupBox_init.Visible = false;
            // 
            // comboBox8
            // 
            this.comboBox8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox8.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox8.FormattingEnabled = true;
            this.comboBox8.Location = new System.Drawing.Point((int)(139 * SW_percent), (int)(31 * SH_percent));
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Size = new System.Drawing.Size((int)(121 * SW_percent), (int)(27 * SH_percent));
            this.comboBox8.TabIndex = 6;
            // 
            // textBox14
            // 
            this.textBox14.Location = new System.Drawing.Point((int)(137 * SW_percent), (int)(77 * SH_percent));
            this.textBox14.Name = "textBox14";
            this.textBox14.PasswordChar = '*';
            this.textBox14.Size = new System.Drawing.Size((int)(124 * SW_percent), (int)(24 * SH_percent));
            this.textBox14.TabIndex = 5;
            // 
            // button17
            // 
            this.button17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button17.Location = new System.Drawing.Point((int)(145 * SW_percent), (int)(132 * SH_percent));
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button17.TabIndex = 3;
            this.button17.Text = "取消";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button16
            // 
            this.button16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button16.Location = new System.Drawing.Point((int)(64 * SW_percent), (int)(132 * SH_percent));
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size((int)(75 * SW_percent), (int)(23 * SH_percent));
            this.button16.TabIndex = 2;
            this.button16.Text = "确认";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label17.Location = new System.Drawing.Point((int)(20 * SW_percent), (int)(82 * SH_percent));
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size((int)(67 * SW_percent), (int)(15 * SH_percent));
            this.label17.TabIndex = 1;
            this.label17.Text = "新密码：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label16.Location = new System.Drawing.Point((int)(17 * SW_percent), (int)(34 * SH_percent));
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size((int)(67 * SW_percent), (int)(15 * SH_percent));
            this.label16.TabIndex = 0;
            this.label16.Text = "用户名：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBox4);
            this.groupBox4.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox4.ForeColor = System.Drawing.Color.Red;
            this.groupBox4.Location = new System.Drawing.Point((int)(583 * SW_percent), (int)(30 * SH_percent));
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size((int)(123 * SW_percent), (int)(74 * SH_percent));
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "当前厂区";
            // 
            // comboBox4
            // 
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.Enabled = false;
            this.comboBox4.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Location = new System.Drawing.Point((int)(14 * SW_percent), (int)(28 * SH_percent));
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size((int)(97 * SW_percent), (int)(29 * SH_percent));
            this.comboBox4.TabIndex = 0;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label7);
            this.groupBox8.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox8.ForeColor = System.Drawing.Color.Red;
            this.groupBox8.Location = new System.Drawing.Point((int)(712 * SW_percent), (int)(30 * SH_percent));
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size((int)(123 * SW_percent), (int)(74 * SH_percent));
            this.groupBox8.TabIndex = 5;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "当前用户";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label7.Location = new System.Drawing.Point((int)(7 * SW_percent), (int)(24 * SH_percent));
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size((int)(91 * SW_percent), (int)(33 * SH_percent));
            this.label7.TabIndex = 0;
            // 
            // serialPort1
            // 
            this.serialPort1.PortName = "COM8";
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            //
            //OnlineTimer
            //
            this.OnlineTimer.Interval = 1000 * 60;//1分钟
            this.OnlineTimer.Tick += new System.EventHandler(this.OnlineTimer_Tick);
            this.OnlineTimer.Enabled = true;
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point((int)(129 * SW_percent), (int)(636 * SH_percent));
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size((int)(121 * SW_percent), (int)(29 * SH_percent));
            this.comboBox3.TabIndex = 13;
            this.comboBox3.Visible = false;
            // 
            // comboBox5
            // 
            this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox5.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Location = new System.Drawing.Point((int)(444 * SW_percent), (int)(636 * SH_percent));
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size((int)(121 * SW_percent), (int)(29 * SH_percent));
            this.comboBox5.TabIndex = 14;
            this.comboBox5.Visible = false;
            // 
            // comboBox6
            // 
            this.comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox6.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Location = new System.Drawing.Point((int)(682 * SW_percent), (int)(636 * SH_percent));
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size((int)(121 * SW_percent), (int)(29 * SH_percent));
            this.comboBox6.TabIndex = 15;
            this.comboBox6.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point((int)(27 * SW_percent), (int)(643 * SH_percent));
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size((int)(88 * SW_percent), (int)(16 * SH_percent));
            this.label3.TabIndex = 16;
            this.label3.Text = "正在盘库：";
            this.label3.Visible = false;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label33.Location = new System.Drawing.Point((int)(293 * SW_percent), (int)(643 * SH_percent));
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size((int)(120 * SW_percent), (int)(16 * SH_percent));
            this.label33.TabIndex = 17;
            this.label33.Text = "正在清洁镜头：";
            this.label33.Visible = false;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label34.Location = new System.Drawing.Point((int)(586 * SW_percent), (int)(643 * SH_percent));
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size((int)(88 * SW_percent), (int)(16 * SH_percent));
            this.label34.TabIndex = 18;
            this.label34.Text = "正在监控：";
            this.label34.Visible = false;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label35.Location = new System.Drawing.Point((int)(24 * SW_percent), (int)(119 * SH_percent));
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size((int)(142 * SW_percent), (int)(15 * SH_percent));
            this.label35.TabIndex = 19;
            this.label35.Text = "在线料仓列表显示：";
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.CheckOnClick = true;
            this.checkedListBox2.ContextMenuStrip = this.contextMenuStrip2;
            this.checkedListBox2.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.Location = new System.Drawing.Point((int)(23 * SW_percent), (int)(467 * SH_percent));
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size((int)(209 * SW_percent), (int)(148 * SH_percent));
            this.checkedListBox2.TabIndex = 20;
            this.checkedListBox2.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox2_ItemCheck);
            this.checkedListBox2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.checkedListBox2_MouseUp);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重试连接ToolStripMenuItem,
            this.删除料仓ToolStripMenuItem,
            this.显示数据ToolStripMenuItem,
            this.显示参数ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size((int)(125 * SW_percent), (int)(48 * SH_percent));
            // 
            // 重试连接ToolStripMenuItem
            // 
            this.重试连接ToolStripMenuItem.Name = "重试连接ToolStripMenuItem";
            this.重试连接ToolStripMenuItem.Size = new System.Drawing.Size((int)(124 * SW_percent), (int)(22 * SH_percent));
            this.重试连接ToolStripMenuItem.Text = "重试连接";
            this.重试连接ToolStripMenuItem.Click += new System.EventHandler(this.重试连接ToolStripMenuItem_Click);
            // 
            // 删除料仓ToolStripMenuItem
            // 
            this.删除料仓ToolStripMenuItem.Name = "删除料仓ToolStripMenuItem";
            this.删除料仓ToolStripMenuItem.Size = new System.Drawing.Size((int)(124 * SW_percent), (int)(22 * SH_percent));
            this.删除料仓ToolStripMenuItem.Text = "删除料仓";
            this.删除料仓ToolStripMenuItem.Click += new System.EventHandler(this.删除料仓ToolStripMenuItem_Click);
            //
            //显示数据ToolStripMenuItem
            //
            this.显示数据ToolStripMenuItem.Name = "显示数据ToolStripMenuItem";
            this.显示数据ToolStripMenuItem.Size = new System.Drawing.Size((int)(124 * SW_percent), (int)(22 * SH_percent));
            this.显示数据ToolStripMenuItem.Text = "显示数据信息";
            this.显示数据ToolStripMenuItem.Click += new System.EventHandler(this.显示数据信息ToolStripMenuItem_Click);
            //
            //显示参数ToolStripMenuItem
            //
            this.显示参数ToolStripMenuItem.Name = "显示参数ToolStripMenuItem";
            this.显示参数ToolStripMenuItem.Size = new System.Drawing.Size((int)(124 * SW_percent), (int)(22 * SH_percent));
            this.显示参数ToolStripMenuItem.Text = "显示参数信息";
            this.显示参数ToolStripMenuItem.Click += new System.EventHandler(this.显示详细信息ToolStripMenuItem_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label18.Location = new System.Drawing.Point((int)(25 * SW_percent), (int)(444 * SH_percent));
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size((int)(157 * SW_percent), (int)(15 * SH_percent));
            this.label18.TabIndex = 21;
            this.label18.Text = "不在线料仓列表显示：";
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 30000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // clean_timer
            // 
            this.clean_timer.Enabled = true;
            this.clean_timer.Interval = 1000;
            this.clean_timer.Tick += new System.EventHandler(this.clean_timer_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point((int)(114 * SW_percent), (int)(157 * SH_percent));
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size((int)(18 * SW_percent), (int)(177 * SH_percent));
            this.progressBar1.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.progressBar2);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point((int)(884 * SW_percent), (int)(30 * SH_percent));
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size((int)(466 * SW_percent), (int)(74 * SH_percent));
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作进度";
            this.groupBox2.Visible = false;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point((int)(7 * SW_percent), (int)(28 * SH_percent));
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size((int)(91 * SW_percent), (int)(23 * SH_percent));
            this.label6.TabIndex = 0;
            this.label6.Text = "";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point((int)(115 * SW_percent), (int)(28 * SH_percent));
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size((int)(120 * SW_percent), (int)(23 * SH_percent));
            this.progressBar2.TabIndex = 1;
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point((int)(250 * SW_percent), (int)(28 * SH_percent));
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size((int)(39 * SW_percent), (int)(23 * SH_percent));
            this.label19.TabIndex = 0;
            this.label19.Text = "";
            //
            //show_timer
            //
            this.show_timer.Enabled = true;
            this.show_timer.Interval = 3000;
            this.show_timer.Tick += new System.EventHandler(this.show_timer_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "warehouse";
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // label22
            // 
            this.label22.Location = new System.Drawing.Point((int)(295 * SW_percent), (int)(28 * SH_percent));
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size((int)(159 * SW_percent), (int)(23 * SH_percent));
            this.label22.TabIndex = 2;
            this.label22.Text = "";
            // 
            // 重启设备ToolStripMenuItem
            // 
            this.重启设备ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重启全套设备ToolStripMenuItem,
            this.重启中控设备ToolStripMenuItem});
            this.重启设备ToolStripMenuItem.Name = "重启设备ToolStripMenuItem";
            this.重启设备ToolStripMenuItem.Size = new System.Drawing.Size((int)(162 * SW_percent), (int)(24 * SH_percent));
            this.重启设备ToolStripMenuItem.Text = "重启设备";
            // 
            // 重启全套设备ToolStripMenuItem
            // 
            this.重启全套设备ToolStripMenuItem.Name = "重启全套设备ToolStripMenuItem";
            this.重启全套设备ToolStripMenuItem.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.重启全套设备ToolStripMenuItem.Text = "重启中控扫描";
            this.重启全套设备ToolStripMenuItem.Click += new System.EventHandler(this.重启全套设备ToolStripMenuItem_Click);
            // 
            // 重启中控设备ToolStripMenuItem
            // 
            this.重启中控设备ToolStripMenuItem.Name = "重启中控设备ToolStripMenuItem";
            this.重启中控设备ToolStripMenuItem.Size = new System.Drawing.Size((int)(134 * SW_percent), (int)(24 * SH_percent));
            this.重启中控设备ToolStripMenuItem.Text = "重启中控设备";
            this.重启中控设备ToolStripMenuItem.Click += new System.EventHandler(this.重启中控设备ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size((int)(1354 * SW_percent), (int)(666 * SH_percent));
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.checkedListBox2);
            this.Controls.Add(this.label35);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.comboBox6);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.comboBox5);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "仓库管理系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdnInfo)).EndInit();
            this.bdnInfo.ResumeLayout(false);
            this.bdnInfo.PerformLayout();
            this.groupBox_pic.ResumeLayout(false);
            this.groupBox_pic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox_serial.ResumeLayout(false);
            this.groupBox_serial.PerformLayout();
            this.groupBox_changepass.ResumeLayout(false);
            this.groupBox_changepass.PerformLayout();
            this.groupBox_adduser.ResumeLayout(false);
            this.groupBox_adduser.PerformLayout();
            this.groupBox_deleteuser.ResumeLayout(false);
            this.groupBox_deleteuser.PerformLayout();
            this.groupBox_init.ResumeLayout(false);
            this.groupBox_init.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bdsInfo)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem 数据库管理ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolStripMenuItem 用户添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 用户初始化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 管理员密码修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 软件升级ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pC管理软件升级ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 料仓主控软件升级ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 测量前端软件升级ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 服务器设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 系统使用帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label7;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.GroupBox groupBox_changepass;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.GroupBox groupBox_adduser;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox_deleteuser;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox_init;
        private System.Windows.Forms.TextBox textBox14;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox_pic;
        private System.Windows.Forms.ToolStripMenuItem 退出登录ToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.ToolStripMenuItem 导出数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入数据ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 串口选择ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox_serial;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem test2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 仓筒直径ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 仓筒高度ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 下锥高度ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 物料密度ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示参数信息ToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ToolStripMenuItem 添加料仓ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示数据信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer OnlineTimer;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox3;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox5;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox4;
        private System.Windows.Forms.ToolStripMenuItem 更改名称ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示盘库时间ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private VerticalProgressBar progressBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ToolStripMenuItem 查询温度ToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox6;
        private System.Windows.Forms.ToolStripMenuItem 清洁镜头ToolStripMenuItem;
        private System.Windows.Forms.BindingSource bdsInfo;
        private System.Windows.Forms.BindingNavigator bdnInfo;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripTextBox txtCurrentPage;
        private System.Windows.Forms.ToolStripLabel lblPageCount;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripMenuItem 进入监控ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消当前操作ToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.ComboBox comboBox5;
        private System.Windows.Forms.ComboBox comboBox6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.ToolStripMenuItem 退出监控ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图标显示ToolStripMenuItem;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.ToolStripMenuItem 打开监控界面ToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ToolStripMenuItem 开启回传数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 料位图形显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 获取基础信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 直接查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 镜头除尘ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 镜头除湿ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 重试连接ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除料仓ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示参数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 料仓盘库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 料位监控ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 当前数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 历史数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 普通用户管理ToolStripMenuItem;
        private System.Windows.Forms.ComboBox comboBox7;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.ComboBox comboBox8;
        private System.Windows.Forms.Timer clean_timer;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Timer show_timer;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ToolStripMenuItem 重启设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重启全套设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重启中控设备ToolStripMenuItem;

    }
}