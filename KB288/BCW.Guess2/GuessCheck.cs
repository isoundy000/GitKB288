using System;
using System.Web;

namespace TPR2.Common
{
	/// <summary>
	/// ¾º²Â×ª»»Àà
	/// </summary>
    public class GCK
    {

   /// <summary>
        /// ´óÐ¡Çòµ¹×ª»»
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static decimal getDxPkNum2(string p_strVal)
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

        /// <summary>
        /// ´óÐ¡Çòµ¹×ª»»(Ð´ÈëÇ°Ê¹ÓÃ£¬8²¨µçÄÔÍø)
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        /// <summary>
        public static string getDxPkNameZH(int p_strVal)
        {
            string strVal = "";
            switch (p_strVal)
            {
                case 1:
                    strVal = "0.25";
                    break;
                case 2:
                    strVal = "0.5";
                    break;
                case 3:
                    strVal = "0.75";
                    break;
                case 4:
                    strVal = "1";
                    break;
                case 5:
                    strVal = "1.25";
                    break;
                case 6:
                    strVal = "1.5";
                    break;
                case 7:
                    strVal = "1.75";
                    break;
                case 8:
                    strVal = "2";
                    break;
                case 9:
                    strVal = "2.25";
                    break;
                case 10:
                    strVal = "2.5";
                    break;
                case 11:
                    strVal = "2.75";
                    break;
                case 12:
                    strVal = "3";
                    break;
                case 13:
                    strVal = "3.25";
                    break;
                case 14:
                    strVal = "3.5";
                    break;
                case 15:
                    strVal = "3.75";
                    break;
                case 16:
                    strVal = "4";
                    break;
                case 17:
                    strVal = "4.25";
                    break;
                case 18:
                    strVal = "4.5";
                    break;
                case 19:
                    strVal = "4.75";
                    break;
                case 20:
                    strVal = "5";
                    break;
                case 21:
                    strVal = "5.25";
                    break;
                case 22:
                    strVal = "5.5";
                    break;
                case 23:
                    strVal = "5.75";
                    break;
                case 24:
                    strVal = "6";
                    break;
                case 25:
                    strVal = "6.25";
                    break;
                case 26:
                    strVal = "6.5";
                    break;
                case 27:
                    strVal = "6.75";
                    break;
                case 28:
                    strVal = "7";
                    break;
                case 29:
                    strVal = "7.25";
                    break;
                case 30:
                    strVal = "7.5";
                    break;
                case 31:
                    strVal = "7.75";
                    break;
                case 32:
                    strVal = "8";
                    break;
                case 33:
                    strVal = "8.25";
                    break;
                case 34:
                    strVal = "8.5";
                    break;
                case 35:
                    strVal = "8.75";
                    break;
                case 36:
                    strVal = "9";
                    break;
                case 37:
                    strVal = "9.25";
                    break;
                case 38:
                    strVal = "9.5";
                    break;
                case 39:
                    strVal = "9.75";
                    break;
                case 40:
                    strVal = "10";
                    break;
            }
            return strVal;
        }

        /// <summary>
        /// ÈÃÓëÊÜÈÃ
        /// </summary>
        /// <param name="p_intPn"></param>
        /// <returns></returns>
        public static string getZqPn(int p_intPn)
        {
            if (p_intPn == 1)
                return "ÈÃ";
            else
                return "ÊÜÈÃ";
        }

        /// <summary>
        /// ÈÃÇòÅÌ×ª»»
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static string getPkName(int p_strVal)
        {
            string strVal = "";
            switch (p_strVal)
            {
                case 1:
                    strVal = "Æ½ÊÖ";
                    break;
                case 2:
                    strVal = "Æ½ÊÖ/°ëÇò";
                    break;
                case 3:
                    strVal = "°ëÇò";
                    break;
                case 4:
                    strVal = "°ëÇò/Ò»Çò";
                    break;
                case 5:
                    strVal = "Ò»Çò";
                    break;
                case 6:
                    strVal = "Ò»Çò/Çò°ë";
                    break;
                case 7:
                    strVal = "Çò°ë";
                    break;
                case 8:
                    strVal = "Çò°ë/¶þÇò";
                    break;
                case 9:
                    strVal = "¶þÇò";
                    break;
                case 10:
                    strVal = "¶þÇò/¶þÇò°ë";
                    break;
                case 11:
                    strVal = "¶þÇò°ë";
                    break;
                case 12:
                    strVal = "¶þÇò°ë/ÈýÇò";
                    break;
                case 13:
                    strVal = "ÈýÇò";
                    break;
                case 14:
                    strVal = "ÈýÇò/ÈýÇò°ë";
                    break;
                case 15:
                    strVal = "ÈýÇò°ë";
                    break;
                case 16:
                    strVal = "ÈýÇò°ë/ËÄÇò";
                    break;
                case 17:
                    strVal = "ËÄÇò";
                    break;
                case 18:
                    strVal = "ËÄÇò/ËÄÇò°ë";
                    break;
                case 19:
                    strVal = "ËÄÇò°ë";
                    break;
                case 20:
                    strVal = "ËÄÇò°ë/ÎåÇò";
                    break;
                case 21:
                    strVal = "ÎåÇò";
                    break;
                case 22:
                    strVal = "ÎåÇò/ÎåÇò°ë";
                    break;
                case 23:
                    strVal = "ÎåÇò°ë";
                    break;
                case 24:
                    strVal = "ÎåÇò°ë/ÁùÇò";
                    break;
                case 25:
                    strVal = "ÁùÇò";
                    break;
                case 26:
                    strVal = "ÁùÇò/ÁùÇò°ë";
                    break;
                case 27:
                    strVal = "ÁùÇò°ë";
                    break;
                case 28:
                    strVal = "ÁùÇò°ë/ÆßÇò";
                    break;
                case 29:
                    strVal = "ÆßÇò";
                    break;
                case 30:
                    strVal = "ÆßÇò/ÆßÇò°ë";
                    break;
                case 31:
                    strVal = "ÆßÇò°ë";
                    break;
                case 32:
                    strVal = "ÆßÇò°ë/°ËÇò";
                    break;
                case 33:
                    strVal = "°ËÇò";
                    break;
            }
            return strVal;
        }

        /// <summary>
        /// ´óÐ¡Çò×ª»»
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static string getDxPkName(int p_strVal)
        {
            string strVal = "0";
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
        /// ÈÃÇòµ¹×ª»»
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
                case "5.25":
                    strVal = 22;
                    break;
                case "5.5":
                    strVal = 23;
                    break;
                case "5.75":
                    strVal = 24;
                    break;
                case "6":
                    strVal = 25;
                    break;
                case "6.25":
                    strVal = 26;
                    break;
                case "6.5":
                    strVal = 27;
                    break;
                case "6.75":
                    strVal = 28;
                    break;
                case "7":
                    strVal = 29;
                    break;
                case "7.25":
                    strVal = 30;
                    break;
                case "7.5":
                    strVal = 31;
                    break;
                case "7.75":
                    strVal = 32;
                    break;
                case "8":
                    strVal = 33;
                    break;
            }
            return strVal;
        }

        /// <summary>
        /// ÈÃÇòµ¹×ª»»2
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static decimal getPkNum2(string p_strVal)
        {
            decimal strVal = 0;
            switch (p_strVal)
            {
                case "Æ½ÊÖ":
                    strVal = 1;
                    break;
                case "Æ½/°ë":
                    strVal = 2;
                    break;
                case "°ëÇò":
                    strVal = 3;
                    break;
                case "°ë/Ò»":
                    strVal = 4;
                    break;
                case "Ò»Çò":
                    strVal = 5;
                    break;
                case "Ò»/Çò°ë":
                    strVal = 6;
                    break;
                case "Çò°ë":
                    strVal = 7;
                    break;
                case "Çò°ë/¶þ":
                    strVal = 8;
                    break;
                case "¶þÇò":
                    strVal = 9;
                    break;
                case "¶þ/¶þÇò°ë":
                    strVal = 10;
                    break;
                case "¶þÇò°ë":
                    strVal = 11;
                    break;
                case "¶þÇò°ë/Èý":
                    strVal = 12;
                    break;
                case "ÈýÇò":
                    strVal = 13;
                    break;
                case "Èý/ÈýÇò°ë":
                    strVal = 14;
                    break;
                case "ÈýÇò°ë":
                    strVal = 15;
                    break;
                case "ÈýÇò°ë/ËÄ":
                    strVal = 16;
                    break;
                case "ËÄÇò":
                    strVal = 17;
                    break;
                case "ËÄ/ËÄÇò°ë":
                    strVal = 18;
                    break;
                case "ËÄÇò°ë":
                    strVal = 19;
                    break;
                case "ËÄÇò°ë/Îå":
                    strVal = 20;
                    break;
                case "ÎåÇò":
                    strVal = 21;
                    break;
                case "Îå/ÎåÇò°ë":
                    strVal = 22;
                    break;
                case "ÎåÇò°ë":
                    strVal = 23;
                    break;
                case "ÎåÇò°ë/Áù":
                    strVal = 24;
                    break;
                case "ÁùÇò":
                    strVal = 25;
                    break;
                case "Áù/ÁùÇò°ë":
                    strVal = 26;
                    break;
                case "ÁùÇò°ë":
                    strVal = 27;
                    break;
                case "ÁùÇò°ë/Æß":
                    strVal = 28;
                    break;
                case "ÆßÇò":
                    strVal = 29;
                    break;
                case "Æß/ÆßÇò°ë":
                    strVal = 30;
                    break;
                case "ÆßÇò°ë":
                    strVal = 31;
                    break;
                case "ÆßÇò°ë/°Ë":
                    strVal = 32;
                    break;
                case "°ËÇò":
                    strVal = 33;
                    break;

            }
            return strVal;
        }


        ///// <summary>
        ///// ´óÐ¡Çòµ¹×ª»»
        ///// </summary>
        ///// <param name="p_strVal"></param>
        ///// <returns></returns>
        //public static decimal getDxPkNum(string p_strVal)
        //{
        //    decimal strVal = 0;
        //    switch (p_strVal)
        //    {
        //        case "0.5":
        //            strVal = 20;
        //            break;
        //        case "0.75":
        //            strVal = 21;
        //            break;
        //        case "1":
        //            strVal = 22;
        //            break;
        //        case "1.25":
        //            strVal = 23;
        //            break;
        //        case "1.5":
        //            strVal = 1;
        //            break;
        //        case "1.75":
        //            strVal = 2;
        //            break;
        //        case "2":
        //            strVal = 3;
        //            break;
        //        case "2.25":
        //            strVal = 4;
        //            break;
        //        case "2.5":
        //            strVal = 5;
        //            break;
        //        case "2.75":
        //            strVal = 6;
        //            break;
        //        case "3":
        //            strVal = 7;
        //            break;
        //        case "3.25":
        //            strVal = 8;
        //            break;
        //        case "3.5":
        //            strVal = 9;
        //            break;
        //        case "3.75":
        //            strVal = 10;
        //            break;
        //        case "4":
        //            strVal = 11;
        //            break;
        //        case "4.25":
        //            strVal = 12;
        //            break;
        //        case "4.5":
        //            strVal = 13;
        //            break;
        //        case "4.75":
        //            strVal = 14;
        //            break;
        //        case "5":
        //            strVal = 15;
        //            break;
        //        case "5.25":
        //            strVal = 16;
        //            break;
        //        case "5.5":
        //            strVal = 17;
        //            break;
        //        case "5.75":
        //            strVal = 18;
        //            break;
        //        case "6":
        //            strVal = 19;
        //            break;
        //        case "6.25":
        //            strVal = 24;
        //            break;
        //        case "6.5":
        //            strVal = 25;
        //            break;
        //        case "6.75":
        //            strVal = 26;
        //            break;
        //        case "7":
        //            strVal = 27;
        //            break;
        //        case "7.25":
        //            strVal = 28;
        //            break;
        //        case "7.5":
        //            strVal = 29;
        //            break;
        //        case "7.75":
        //            strVal = 30;
        //            break;
        //        case "8":
        //            strVal = 31;
        //            break;
        //        case "8.25":
        //            strVal = 32;
        //            break;
        //        case "8.5":
        //            strVal = 33;
        //            break;
        //        case "8.75":
        //            strVal = 34;
        //            break;
        //        case "9":
        //            strVal = 35;
        //            break;
        //        case "9.25":
        //            strVal = 36;
        //            break;
        //        case "9.5":
        //            strVal = 37;
        //            break;
        //        case "9.75":
        //            strVal = 38;
        //            break;
        //        case "10":
        //            strVal = 39;
        //            break;
        //    }
        //    return strVal;
        //}


        /// <summary>
        /// ´óÐ¡Çò×ª»»
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static decimal getDxPkNum(string p_strVal)
        {
            int strVal = 0;
            switch (p_strVal)
            {
                case "1.5":
                    strVal = 1;
                    break;
                case "1.5/2":
                    strVal = 2;
                    break;
                case "2":
                    strVal = 3;
                    break;
                case "2/2.5":
                    strVal = 4;
                    break;
                case "2.5":
                    strVal = 5;
                    break;
                case "2.5/3":
                    strVal = 6;
                    break;
                case "3":
                    strVal = 7;
                    break;
                case "3/3.5":
                    strVal = 8;
                    break;
                case "3.5":
                    strVal = 9;
                    break;
                case "3.5/4":
                    strVal = 10;
                    break;
                case "4":
                    strVal = 11;
                    break;
                case "4/4.5":
                    strVal = 12;
                    break;
                case "4.5":
                    strVal = 13;
                    break;
                case "4.5/5":
                    strVal = 14;
                    break;
                case "5":
                    strVal = 15;
                    break;
                case "5/5.5":
                    strVal = 16;
                    break;
                case "5.5":
                    strVal = 17;
                    break;
                case "5.5/6":
                    strVal = 18;
                    break;
                case "6":
                    strVal = 19;
                    break;
                case "0.5":
                    strVal = 20;
                    break;
                case "0.5/1":
                    strVal = 21;
                    break;
                case "1":
                    strVal = 22;
                    break;
                case "1/1.5":
                    strVal = 23;
                    break;
                case "6/6.5":
                    strVal = 24;
                    break;
                case "6.5":
                    strVal = 25;
                    break;
                case "6.5/7":
                    strVal = 26;
                    break;
                case "7":
                    strVal = 27;
                    break;
                case "7/7.5":
                    strVal = 28;
                    break;
                case "7.5":
                    strVal = 29;
                    break;
                case "7.5/8":
                    strVal = 30;
                    break;
                case "8":
                    strVal = 31;
                    break;
                case "8/8.5":
                    strVal = 32;
                    break;
                case "8.5":
                    strVal = 33;
                    break;
                case "8.5/9":
                    strVal = 34;
                    break;
                case "9":
                    strVal = 35;
                    break;
                case "9/9.5":
                    strVal = 36;
                    break;
                case "9.5":
                    strVal = 37;
                    break;
                case "9.5/10":
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
