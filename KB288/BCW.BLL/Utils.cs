using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// ==================================
/// 重做Utils函数 参考原有 BCW.Common.dll
/// 
/// 黄国军 20151231
/// ==================================
/// </summary>
namespace BCW.BLL
{
    public class NewUtils
    {
        #region 金币格式化函数
        /// <summary>
        /// 金币格式化函数
        /// </summary>
        /// <param name="i">金币数</param>
        /// <returns>已格式化字符</returns>
        public static string ConvertGold(long i)
        {
            if (true)
            {
            }
            string result = "";
            while (true)
            {
                bool flag = true; // !Utils.GetTopDomain().Contains("kb288.net");
                int num = 8;
                while (true)
                {
                    switch (num)
                    {
                        case 0:
                            result = i.ToString();
                            num = 5;
                            continue;
                        case 1:
                            result = Math.Round(Convert.ToDouble(Convert.ToDouble(i) / 10000.0), 2) + "万";
                            num = 3;
                            continue;
                        case 2:
                            result = Math.Round(Convert.ToDouble(Convert.ToDouble(i) / 100000000.0), 2) + "亿";
                            num = 7;
                            continue;
                        case 3:
                            return result;
                        case 4:
                            if (!flag)
                            {
                                num = 2;
                                continue;
                            }
                            flag = (i < 10000L);
                            num = 6;
                            continue;
                        case 5:
                            return result;
                        case 6:
                            if (!flag)
                            {
                                num = 1;
                                continue;
                            }
                            result = i.ToString();
                            num = 9;
                            continue;
                        case 7:
                            return result;
                        case 8:
                            if (!flag)
                            {
                                num = 0;
                                continue;
                            }
                            flag = (i < 100000000L);
                            num = 4;
                            continue;
                        case 9:
                            return result;
                    }
                    break;
                }
            }
            return result;
        }
        #endregion

        #region 获得域名 反编辑得到，需修改
        
        
        /// <summary>
        /// 获得域名 反编辑得到，需修改
        /// </summary>
        /// <returns></returns>
        public static string NewGetDomain()
        {
            string result="";
            while (true)
            {
                string text = HttpContext.Current.Request.Url.Authority;
                bool flag = text == null;
                int num = 2;
                while (true)
                {
                    string arg_B8_0;
                    switch (num)
                    {
                        case 0:
                            arg_B8_0 = text.ToString();
                            goto IL_B8;
                        case 1:
                            goto IL_D1;
                        case 2:
                            if (!flag)
                            {
                                num = 3;
                                continue;
                            }
                            goto IL_D1;
                        case 3:
                            text = text.Replace(":13599", "").Replace(":13678", "");
                            num = 1;
                            continue;
                        case 4:
                            arg_B8_0 = "127.0.0.1";
                            goto IL_B8;
                        case 5:
                            return result;
                        case 6:
                            if (text != null)
                            {
                                num = 7;
                                continue;
                            }
                            num = 4;
                            continue;
                        case 7:
                            num = 0;
                            continue;
                    }
                    break;
                IL_B8:
                    result = arg_B8_0;
                    if (true)
                    {
                    }
                    num = 5;
                    continue;
                IL_D1:
                    num = 6;
                }
            }
            return result;
        }
        #endregion
    }
}
