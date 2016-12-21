using System;
using System.Web;

namespace TPR2.Common
{
	/// <summary>
	/// ����ת����
	/// </summary>
    public class GCK
    {

   /// <summary>
        /// ��С��ת��
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
        /// ��С��ת��(д��ǰʹ�ã�8��������)
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
        /// ��������
        /// </summary>
        /// <param name="p_intPn"></param>
        /// <returns></returns>
        public static string getZqPn(int p_intPn)
        {
            if (p_intPn == 1)
                return "��";
            else
                return "����";
        }

        /// <summary>
        /// ������ת��
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static string getPkName(int p_strVal)
        {
            string strVal = "";
            switch (p_strVal)
            {
                case 1:
                    strVal = "ƽ��";
                    break;
                case 2:
                    strVal = "ƽ��/����";
                    break;
                case 3:
                    strVal = "����";
                    break;
                case 4:
                    strVal = "����/һ��";
                    break;
                case 5:
                    strVal = "һ��";
                    break;
                case 6:
                    strVal = "һ��/���";
                    break;
                case 7:
                    strVal = "���";
                    break;
                case 8:
                    strVal = "���/����";
                    break;
                case 9:
                    strVal = "����";
                    break;
                case 10:
                    strVal = "����/�����";
                    break;
                case 11:
                    strVal = "�����";
                    break;
                case 12:
                    strVal = "�����/����";
                    break;
                case 13:
                    strVal = "����";
                    break;
                case 14:
                    strVal = "����/�����";
                    break;
                case 15:
                    strVal = "�����";
                    break;
                case 16:
                    strVal = "�����/����";
                    break;
                case 17:
                    strVal = "����";
                    break;
                case 18:
                    strVal = "����/�����";
                    break;
                case 19:
                    strVal = "�����";
                    break;
                case 20:
                    strVal = "�����/����";
                    break;
                case 21:
                    strVal = "����";
                    break;
                case 22:
                    strVal = "����/�����";
                    break;
                case 23:
                    strVal = "�����";
                    break;
                case 24:
                    strVal = "�����/����";
                    break;
                case 25:
                    strVal = "����";
                    break;
                case 26:
                    strVal = "����/�����";
                    break;
                case 27:
                    strVal = "�����";
                    break;
                case 28:
                    strVal = "�����/����";
                    break;
                case 29:
                    strVal = "����";
                    break;
                case 30:
                    strVal = "����/�����";
                    break;
                case 31:
                    strVal = "�����";
                    break;
                case 32:
                    strVal = "�����/����";
                    break;
                case 33:
                    strVal = "����";
                    break;
            }
            return strVal;
        }

        /// <summary>
        /// ��С��ת��
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
        /// ����ת��
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
        /// ����ת��2
        /// </summary>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public static decimal getPkNum2(string p_strVal)
        {
            decimal strVal = 0;
            switch (p_strVal)
            {
                case "ƽ��":
                    strVal = 1;
                    break;
                case "ƽ/��":
                    strVal = 2;
                    break;
                case "����":
                    strVal = 3;
                    break;
                case "��/һ":
                    strVal = 4;
                    break;
                case "һ��":
                    strVal = 5;
                    break;
                case "һ/���":
                    strVal = 6;
                    break;
                case "���":
                    strVal = 7;
                    break;
                case "���/��":
                    strVal = 8;
                    break;
                case "����":
                    strVal = 9;
                    break;
                case "��/�����":
                    strVal = 10;
                    break;
                case "�����":
                    strVal = 11;
                    break;
                case "�����/��":
                    strVal = 12;
                    break;
                case "����":
                    strVal = 13;
                    break;
                case "��/�����":
                    strVal = 14;
                    break;
                case "�����":
                    strVal = 15;
                    break;
                case "�����/��":
                    strVal = 16;
                    break;
                case "����":
                    strVal = 17;
                    break;
                case "��/�����":
                    strVal = 18;
                    break;
                case "�����":
                    strVal = 19;
                    break;
                case "�����/��":
                    strVal = 20;
                    break;
                case "����":
                    strVal = 21;
                    break;
                case "��/�����":
                    strVal = 22;
                    break;
                case "�����":
                    strVal = 23;
                    break;
                case "�����/��":
                    strVal = 24;
                    break;
                case "����":
                    strVal = 25;
                    break;
                case "��/�����":
                    strVal = 26;
                    break;
                case "�����":
                    strVal = 27;
                    break;
                case "�����/��":
                    strVal = 28;
                    break;
                case "����":
                    strVal = 29;
                    break;
                case "��/�����":
                    strVal = 30;
                    break;
                case "�����":
                    strVal = 31;
                    break;
                case "�����/��":
                    strVal = 32;
                    break;
                case "����":
                    strVal = 33;
                    break;

            }
            return strVal;
        }


        ///// <summary>
        ///// ��С��ת��
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
        /// ��С��ת��
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
