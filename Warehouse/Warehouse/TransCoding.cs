using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Warehouse
{
    public class TransCoding : Form
    {
        public string readbuffer = "";//读缓冲区
        public string writebuffer = "";//写缓冲区

        public string instruction = "";//数据报指令部分
        public string device = "";//设备码
        /// <summary>
        /// 检测收到的数据
        /// </summary>



        unsafe static char Cal_crc8(char* ptr, int len)
        {//生成CRC8校验码函数
            char crc;
            crc = (char)0;
            int j = 0;
            while ((len--) > 0)
            {
                crc ^= *ptr++;
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x80) != 0)
                    {
                        crc = (char)((crc << 1) ^ 0x07);//检验序列x8+x2+x1+1，即100000111，舍高位，即0x07
                    }
                    else crc <<= 1;
                }
            }
            return crc;
        }
        public string Crc(string str1, string str2, string str3, string str4)
        {//将textbox中的字符串转变成可进行crc的char[]并生成CRC8校验码
            //str1 = 12345, str2=0x05,str3=0x02, str4=0x0618
            //str1表示厂区码，str2表示设备地址，str3表示指令码，str4表示参数
            //MessageBox.Show(str1 + "\n" + str2 + "\n" + str3 + "\n" + str4);
            string str = str1;
            int text2 = Convert.ToInt32(str2, 16);//将str2中的字符串转为十六进制
            int text4 = Convert.ToInt32(str3, 16);//将str3中的字符串转为十六进制
            //long tt = Convert.ToInt64(str4, 10);//tt是str4中string的十进制int表示
            //string st = tt.ToString("x4");//st是t的十六进制
            string st = str4;
            string t1 = "";

            long t1i ;//十六进制的前两位转成十进制
            char[] cc = new char[str1.Length + str2.Length / 2 + str3.Length / 2 + str4.Length];
            for (int i = 0; i < str.Length; i++)
            {
                cc[i] = (char)(str[i] - '0');
            }//厂区码添加到cc中
            cc[5] = (char)(text2);
            cc[6] = (char)(text4);

            if (str4.Length == 4)
            {
                str4 = str4.PadLeft(str4.Length * 2, '0');
            }
            for (int i = 0; i < (str4.Length) / 2; i++)
            {
                t1 = (str4[2 * i] + "" + str4[2 * i + 1]).ToUpper();
                t1i = Int32.Parse(t1, System.Globalization.NumberStyles.HexNumber);
                cc[7 + i] = (char)(t1i);
            }
            string crc = "";
            unsafe
            {
                char* ch = stackalloc char[cc.Length];
                for (int i = 0; i < cc.Length; i++)
                {
                    ch[i] = cc[i];
                    int t = ch[i];
                }
                int res = Cal_crc8(ch, cc.Length);
                crc = res.ToString("x2");
                crc = (crc[crc.Length - 2] + "" + crc[crc.Length - 1]);
            }
            return crc.ToUpper();
        }


        
        /// <summary>
        /// 将12345变成0102030405
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string oneTotwo(string str)
        {
            string retstr = "";
            for (int i = 0; i < str.Length; i++)
            {
                retstr += "0" + str[i];
            }
            return retstr;
        }
        /// <summary>
        /// 字符串转16进制字节数组
        /// 十六进制转成十进制
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        public string Data(string str1, string str2, string str3, string str4)
        {
            
            string str = "";
            string s4 = "";
            int int2 = int.Parse(str2);
            string str2_16 = int2.ToString("x2");
            int int3 = int.Parse(str3);
            string str3_16 = int3.ToString("x2");
            str4 = str4.PadLeft(4, '0');
            for (int i = 0; i < str4.Length / 4; i++)
            {
                long int4 = long.Parse(str4[4 * i] + "" + str4[4 * i + 1] + "" + str4[4 * i + 2] + "" + str4[4 * i + 3]);
                s4 += int4.ToString("x4");
            }
            //str4 = long.Parse(str4).ToString("x4");
            string crc = Crc(str1, str2_16, str3_16, s4.ToUpper());//CRC的十六进制表示
            //long data = Convert.ToInt64(s4.ToUpper());
            //int data = Int32.Parse(s4, System.Globalization.NumberStyles.AllowHexSpecifier);
            if(s4.Length == 4)
                s4 = s4.PadLeft(8, '0');
            str = oneTotwo(str1) + str2_16.ToUpper() + str3_16.ToUpper() + s4.ToUpper() + crc;
            str = ":0C" + str + "\r";
            return str;//要发送的数据报
        }

        /// <summary>
        /// 解码函数
        /// </summary>
        /// <param name="str"></param>
        public string decoding(string str)
        {
            
            string len = str[1] +""+ str[2];
            int l = Int32.Parse(len, System.Globalization.NumberStyles.HexNumber);
            string crc = str[str.Length - 4] + "" + str[str.Length - 3];
            if (l != (str.Length - 5) / 2)
            {
                return "0";
            }

            string fac = "";
            for (int i = 0; i < 5; i++)
            {
                string s = str[2*i + 3] +""+ str[2*i + 4];
                int a = Int32.Parse(s);
                fac += a.ToString();
            }

            string equip = str[13] +""+ str[14];
            //int equip_int = Int32.Parse(equip, System.Globalization.NumberStyles.HexNumber);
            //equip = equip_int.ToString();
            string oper = str[15] +""+ str[16];
            string data = "";
            string crc_data = "";//用于测试crc的data数据
            if (oper == "23")
            {
                for (int i = 0; i < 2; i++)
                {
                    //d表示整数位，f表示小数位
                    string d = str[6 * i + 17] + "" + str[6 * i + 18] + "" + str[6 * i + 19] + "" + str[6 * i + 20] ;
                    string f = str[6 * i + 21] + "" + str[6 * i + 22];
                    float d_int = Int32.Parse(d, System.Globalization.NumberStyles.HexNumber);
                    float f_int = Int32.Parse(f, System.Globalization.NumberStyles.HexNumber);
                    data+=((d_int+f_int/100).ToString()+"+");
                }
            }
            else if (oper == "11")
            {
                string temp1 = str[17] + "" + str[18];//温度的整数部分
                string temp2 = str[19] + "" + str[20];//温度的小数部分
                crc_data += (temp1 + temp2);
                int temp1_int = Int32.Parse(temp1, System.Globalization.NumberStyles.HexNumber);//温度整数部分转化为十进制
                float temp2_int = Int32.Parse(temp2, System.Globalization.NumberStyles.HexNumber);//温度小数部分转化为十进制
                temp1 = Convert.ToString(temp1_int, 2);//temp的二进制表示
                temp1 = temp1.PadLeft(8, '0');//温度二进制的格式化表示

                if (temp1[0] == '1')
                {
                    temp1_int = temp1_int - 128;
                    temp1_int = -temp1_int;
                    data = ((temp1_int+temp2_int/100).ToString());
                }
                else
                {
                    data += ((temp1_int+temp2_int/100).ToString());
                }
                data += "+";
                string hum1 = str[21] + "" + str[22];//湿度的整数部分
                string hum2 = str[23] + "" + str[24];//湿度的小数部分
                crc_data += (hum1 + hum2);
                float hum1_int = Int32.Parse(hum1, System.Globalization.NumberStyles.HexNumber);//湿度整数部分转化为十进制
                float hum2_int = Int32.Parse(hum2, System.Globalization.NumberStyles.HexNumber);//湿度小数部分转化为十进制
                data+=((hum1_int+hum2_int/100).ToString());
            }
            else if (oper == "25")
            {
                string angle = str[17] + "" + str[18];//角度值
                string distance = str[19] + "" + str[20] + "" + str[21] + "" + str[22];//距离，单位厘米
                string schedule = str[23] + "" + str[24];//进度，完成是0x64

                int angle_int = Int32.Parse(angle, System.Globalization.NumberStyles.HexNumber);
                int distance_int = Int32.Parse(distance, System.Globalization.NumberStyles.HexNumber);
                int schedule_int = Int32.Parse(schedule, System.Globalization.NumberStyles.HexNumber);

                data = angle_int.ToString()+"+"+distance_int.ToString()+"+"+schedule_int.ToString();
            }
            else if (oper == "21")
            {
                string complet = str[17] + "" + str[18];//是否盘库完成
                string schedule_hex = str[19] + "" + str[20];//盘库进度
                string status_hex = str[23] + "" + str[24];//料仓状态
                int schedule_int = Int32.Parse(schedule_hex, System.Globalization.NumberStyles.HexNumber);
                int status_int = Int32.Parse(status_hex, System.Globalization.NumberStyles.HexNumber);
                data = complet + "+" + status_int.ToString().PadLeft(2, '0') + "+" + schedule_int.ToString();
                //for (int i = 0; i < (l - 8) / 2; i++)
                //{
                //    string d =  str[4 * i + 19] + "" + str[4 * i + 20];

                //    crc_data += d;
                //    int a = Int32.Parse(d, System.Globalization.NumberStyles.HexNumber);
                //    data += (a.ToString().PadLeft(4, '0')) + "";
                //}
                //data = complet + "+" + data;
            }
            else if(oper == "0B")
            {
                String Margin = str[17] +""+ str[18]+""+str[19]+""+str[20];//边距
                String Top = str[21] + "" + str[22] + "" + str[23] + "" + str[24];//顶高度
                String Wheelbase = str[25] + "" + str[26] + "" + str[27] + "" + str[28];//轴距
                int Margin_int = Int32.Parse(Margin, System.Globalization.NumberStyles.HexNumber);
                int Top_int = Int32.Parse(Top, System.Globalization.NumberStyles.HexNumber);
                int Wheelbase_int = Int32.Parse(Wheelbase, System.Globalization.NumberStyles.HexNumber);
                data = Margin_int.ToString() + "+" + Top_int.ToString() + "+" + Wheelbase_int.ToString();
            }
            else if (oper == "0D")
            {
                String Speed = str[17] + "" + str[18];
                int Speed_int = Int32.Parse(Speed, System.Globalization.NumberStyles.HexNumber);
                switch (Speed_int)
                {
                    case 0:
                        Speed_int = 300;
                        break;
                    case 1:
                        Speed_int = 1200;
                        break;
                    case 2:
                        Speed_int = 2400;
                        break;
                    case 3:
                        Speed_int = 4800;
                        break;
                    case 4:
                        Speed_int = 9600;
                        break;
                    case 5:
                        Speed_int = 19200;
                        break;
                }

                String Channel = str[19] + "" + str[20];
                String ModelAddress = str[21] + "" + str[22]+""+str[23]+""+str[24];
                int Channel_int = Int32.Parse(Channel, System.Globalization.NumberStyles.HexNumber);
                int Address_int = Int32.Parse(ModelAddress, System.Globalization.NumberStyles.HexNumber);

                data = Speed_int.ToString() + "+" + Channel_int.ToString() + "+" + Address_int.ToString();

            }
            else if (oper == "0F")
            {
                String sign_Correction = str[17] + "" + str[18];
                String Corr_vertical1 = str[19] + "" + str[20];//校准角度的整数部分
                String Corr_vertical2  = str[21] + "" + str[22];//校准角度的小数部分
                int Corr2_int = Int32.Parse(Corr_vertical2, System.Globalization.NumberStyles.HexNumber);
                String Correction_per = str[23] + "" + str[24];//比例校正百分比
                String Correction = str[25]+""+str[26];//加减校正值
                String temp = Corr_vertical1 + "." + Corr2_int.ToString();
                //MessageBox.Show(temp);
                //int vertical = Int32.Parse(temp, System.Globalization.NumberStyles.HexNumber);
                if (sign_Correction.Equals("01"))
                {
                    temp = "-" + temp;
                }

                int Corr_percent = Int32.Parse(Correction_per, System.Globalization.NumberStyles.HexNumber);
                int Corr_int = Int32.Parse(Correction, System.Globalization.NumberStyles.HexNumber);
                if (Corr_int > 128)
                {
                    Corr_int = -(Corr_int - 128);
                }

                data = temp.ToString() + "+" + Corr_percent + "+" + Corr_int;
            }
            else
            {
                for (int i = 0; i < (l - 8) / 2; i++)
                {
                    string d = str[4 * i + 17] + "" + str[4 * i + 18] + "" + str[4 * i + 19] + "" + str[4 * i + 20];
                    
                    crc_data += d;
                    int a = Int32.Parse(d, System.Globalization.NumberStyles.HexNumber);
                    data += (a.ToString().PadLeft(4, '0'))+"";
                }
                if (oper != "01")
                {
                    int data_int = Int32.Parse(crc_data, System.Globalization.NumberStyles.HexNumber);
                    crc_data = data_int.ToString("x4");//.PadLeft(4, '0');
                    if (crc.Equals(Crc(fac, equip, oper, crc_data)) == false)
                    {
                        return "0";
                    }
                }
                
            }
            
            //MessageBox.Show(fac + " " + equip + " " + oper + " " + data);
            return fac + " " + equip + " " + oper + " " + data;
            //返回的是厂区码+设备地址的十六进制+操作码的十六进制+数据的十进制
        }

    }
}
