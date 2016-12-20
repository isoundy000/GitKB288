using System;
using System.Web;

namespace TPR.Common
{
	/// <summary>
	/// 竞猜转换类
	/// </summary>
    public class GCK
    {

        /// <summary>
        /// 让与受让
        /// </summary>
        /// <param name="p_intPn"></param>
        /// <returns></returns>
        public static string getZqPn(int p_intPn)
        {
            if (p_intPn == 1)
                return "让";
            else
                return "受让";
        }

        /// <summary>
        /// 让球盘转换
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static string getPkName(int p_strVal)
        {
            string strVal = "";
            switch (p_strVal)
            {
                case 1:
                    strVal = "平手";
                    break;
                case 2:
                    strVal = "平手/半球";
                    break;
                case 3:
                    strVal = "半球";
                    break;
                case 4:
                    strVal = "半球/一球";
                    break;
                case 5:
                    strVal = "一球";
                    break;
                case 6:
                    strVal = "一球/球半";
                    break;
                case 7:
                    strVal = "球半";
                    break;
                case 8:
                    strVal = "球半/二球";
                    break;
                case 9:
                    strVal = "二球";
                    break;
                case 10:
                    strVal = "二球/二球半";
                    break;
                case 11:
                    strVal = "二球半";
                    break;
                case 12:
                    strVal = "二球半/三球";
                    break;
                case 13:
                    strVal = "三球";
                    break;
                case 14:
                    strVal = "三球/三球半";
                    break;
                case 15:
                    strVal = "三球半";
                    break;
                case 16:
                    strVal = "三球半/四球";
                    break;
                case 17:
                    strVal = "四球";
                    break;
                case 18:
                    strVal = "四球/四球半";
                    break;
                case 19:
                    strVal = "四球半";
                    break;
                case 20:
                    strVal = "四球半/五球";
                    break;
                case 21:
                    strVal = "五球";
                    break;
            }
            return strVal;
        }

        /// <summary>
        /// 大小球转换
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static string getDxPkName(int p_strVal)
        {
            string strVal = "";
            switch (p_strVal)
            {
                case 1:
                    strVal = "1.5";
                    break;
                case 2:
                    strVal = "1.5/2.0";
                    break;
                case 3:
                    strVal = "2.0";
                    break;
                case 4:
                    strVal = "2/2.5";
                    break;
                case 5:
                    strVal = "2.5";
                    break;
                case 6:
                    strVal = "2.5/3.0";
                    break;
                case 7:
                    strVal = "3.0";
                    break;
                case 8:
                    strVal = "3/3.5";
                    break;
                case 9:
                    strVal = "3.5";
                    break;
                case 10:
                    strVal = "3.5/4.0";
                    break;
                case 11:
                    strVal = "4.0";
                    break;
                case 12:
                    strVal = "4/4.5";
                    break;
                case 13:
                    strVal = "4.5";
                    break;
                case 14:
                    strVal = "4.5/5.0";
                    break;
                case 15:
                    strVal = "5.0";
                    break;
                case 16:
                    strVal = "5/5.5";
                    break;
                case 17:
                    strVal = "5.5";
                    break;
                case 18:
                    strVal = "5.5/6.0";
                    break;
                case 19:
                    strVal = "6.0";
                    break;
                case 20:
                    strVal = "0.5";
                    break;
                case 21:
                    strVal = "0.5/1.0";
                    break;
                case 22:
                    strVal = "1.0";
                    break;
                case 23:
                    strVal = "1.0/1.5";
                    break;
                case 24:
                    strVal = "6.0/6.5";
                    break;
                case 25:
                    strVal = "6.5";
                    break;
                case 26:
                    strVal = "6.5/7.0";
                    break;
                case 27:
                    strVal = "7.0";
                    break;
                case 28:
                    strVal = "7.0/7.5";
                    break;
                case 29:
                    strVal = "7.5";
                    break;
                case 30:
                    strVal = "7.5/8.0";
                    break;
                case 31:
                    strVal = "8.0";
                    break;
                case 32:
                    strVal = "8.0/8.5";
                    break;
                case 33:
                    strVal = "8.5";
                    break;
                case 34:
                    strVal = "8.5/9.0";
                    break;
                case 35:
                    strVal = "9.0";
                    break;
                case 36:
                    strVal = "9.0/9.5";
                    break;
                case 37:
                    strVal = "9.5";
                    break;
                case 38:
                    strVal = "9.5/10.0";
                    break;
                case 39:
                    strVal = "10.0";
                    break;
            }
            return strVal;
        }

        /// <summary>
        /// 让球倒转换
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static decimal getPkNum(string p_strVal)
        {
            decimal strVal = 0;
            switch (p_strVal)
            {
                case "0":
                    strVal = 1;
                    break;
                case "0.25":
                    strVal = 2;
                    break;
                case "0.5":
                    strVal = 3;
                    break;
                case "0.75":
                    strVal = 4;
                    break;
                case "1":
                    strVal = 5;
                    break;
                case "1.25":
                    strVal = 6;
                    break;
                case "1.5":
                    strVal = 7;
                    break;
                case "1.75":
                    strVal = 8;
                    break;
                case "2":
                    strVal = 9;
                    break;
                case "2.25":
                    strVal = 10;
                    break;
                case "2.5":
                    strVal = 11;
                    break;
                case "2.75":
                    strVal = 12;
                    break;
                case "3":
                    strVal = 13;
                    break;
                case "3.25":
                    strVal = 14;
                    break;
                case "3.5":
                    strVal = 15;
                    break;
                case "3.75":
                    strVal = 16;
                    break;
                case "4":
                    strVal = 17;
                    break;
                case "4.25":
                    strVal = 18;
                    break;
                case "4.5":
                    strVal = 19;
                    break;
                case "4.75":
                    strVal = 20;
                    break;
                case "5":
                    strVal = 21;
                    break;
            }
            return strVal;
        }

        /// <summary>
        /// 让球倒转换2
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static decimal getPkNum2(string p_strVal)
        {
            decimal strVal = 0;
            switch (p_strVal)
            {
                case "平手":
                    strVal = 1;
                    break;
                case "平/半":
                    strVal = 2;
                    break;
                case "半球":
                    strVal = 3;
                    break;
                case "半/一":
                    strVal = 4;
                    break;
                case "一球":
                    strVal = 5;
                    break;
                case "一球/球半":
                    strVal = 6;
                    break;
                case "球半":
                    strVal = 7;
                    break;
                case "球半/二球":
                case "球半/两球":
                    strVal = 8;
                    break;
                case "二球":
                case "两球":
                    strVal = 9;
                    break;
                case "二球/二球半":
                case "两球/两球半":
                    strVal = 10;
                    break;
                case "二球半":
                case "两球半":
                    strVal = 11;
                    break;
                case "二球半/三球":
                case "两球半/三球":
                    strVal = 12;
                    break;
                case "三球":
                    strVal = 13;
                    break;
                case "三球/三球半":
                    strVal = 14;
                    break;
                case "三球半":
                    strVal = 15;
                    break;
                case "三球半/四球":
                    strVal = 16;
                    break;
                case "四球":
                    strVal = 17;
                    break;
                case "四球/四球半":
                    strVal = 18;
                    break;
                case "四球半":
                    strVal = 19;
                    break;
                case "四球半/五球":
                    strVal = 20;
                    break;
                case "五球":
                    strVal = 21;
                    break;
            }
            return strVal;
        }


        /// <summary>
        /// 大小球倒转换
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static decimal getDxPkNum(string p_strVal)
        {
            decimal strVal = 0;
            switch (p_strVal)
            {
                case "0.5":
                    strVal = 20;
                    break;
                case "0.75":
                    strVal = 21;
                    break;
                case "1":
                    strVal = 22;
                    break;
                case "1.25":
                    strVal = 23;
                    break;
                case "1.5":
                    strVal = 1;
                    break;
                case "1.75":
                    strVal = 2;
                    break;
                case "2":
                    strVal = 3;
                    break;
                case "2.25":
                    strVal = 4;
                    break;
                case "2.5":
                    strVal = 5;
                    break;
                case "2.75":
                    strVal = 6;
                    break;
                case "3":
                    strVal = 7;
                    break;
                case "3.25":
                    strVal = 8;
                    break;
                case "3.5":
                    strVal = 9;
                    break;
                case "3.75":
                    strVal = 10;
                    break;
                case "4":
                    strVal = 11;
                    break;
                case "4.25":
                    strVal = 12;
                    break;
                case "4.5":
                    strVal = 13;
                    break;
                case "4.75":
                    strVal = 14;
                    break;
                case "5":
                    strVal = 15;
                    break;
                case "5.25":
                    strVal = 16;
                    break;
                case "5.5":
                    strVal = 17;
                    break;
                case "5.75":
                    strVal = 18;
                    break;
                case "6":
                    strVal = 19;
                    break;
                case "6.25":
                    strVal = 24;
                    break;
                case "6.5":
                    strVal = 25;
                    break;
                case "6.75":
                    strVal = 26;
                    break;
                case "7":
                    strVal = 27;
                    break;
                case "7.25":
                    strVal = 28;
                    break;
                case "7.5":
                    strVal = 29;
                    break;
                case "7.75":
                    strVal = 30;
                    break;
                case "8":
                    strVal = 31;
                    break;
                case "8.25":
                    strVal = 32;
                    break;
                case "8.5":
                    strVal = 33;
                    break;
                case "8.75":
                    strVal = 34;
                    break;
                case "9":
                    strVal = 35;
                    break;
                case "9.25":
                    strVal = 36;
                    break;
                case "9.5":
                    strVal = 37;
                    break;
                case "9.75":
                    strVal = 38;
                    break;
                case "10":
                    strVal = 39;
                    break;
            }
            return strVal;
        }
    }

}
