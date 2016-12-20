using System;
using System.Collections.Generic;
using System.Text;
namespace TPR2.Common.Guess
{


    public class KzZqClass
    {
        /// <summary>
        /// 让球盘逻辑类
        //p_pk 让受球盘口
        //p_dx_pk 大小球盘口
        //p_pn 让或者受让
        //p_result_one  结果1
        //p_result_two  结果2
        //payType 会员选择上下，大小，标准盘
        //payCent  本金
        //payonLuone  赔率1
        //payonLutwo  赔率2
        //payonLuthr 赔率3
        /// <param name="model"></param>
        /// <returns></returns>
        public static string getZqsxCase(TPR2.Model.guess.BaPayMe model)
        {
            string strVal = "";
            int intone, inttwo, iTypeone, iTypetwo;
            decimal iTypeonelu, iTypetwolu;

            if (model.p_pn == 1)
            {//让
                iTypeone = 1;
                iTypetwo = 2;
                iTypeonelu = Convert.ToDecimal(model.payonLuone);
                iTypetwolu = Convert.ToDecimal(model.payonLutwo);
                intone = Convert.ToInt32(model.p_result_one);
                inttwo = Convert.ToInt32(model.p_result_two);
            }
            else
            {//受让
                iTypeone = 2;
                iTypetwo = 1;
                iTypeonelu = Convert.ToDecimal(model.payonLutwo);
                iTypetwolu = Convert.ToDecimal(model.payonLuone);
                intone = Convert.ToInt32(model.p_result_two);
                inttwo = Convert.ToInt32(model.p_result_one);
            }

            /*---------------------------平手----------------------------------------------*/
            if (model.p_pk == 1)
            {
                if (intone == inttwo)
                    strVal = model.payCent + "|平盘";//平盘
                else if (intone > inttwo && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘赢盘
                else if (intone < inttwo && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘赢盘
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------平手/半球----------------------------------------------*/
            else if (model.p_pk == 2)
            {
                if (intone == inttwo && model.PayType == iTypeone)
                    strVal = model.payCent / 2 + "|输半";//上盘输半
                else if (intone == inttwo && model.PayType == iTypetwo)
                    strVal = (model.payCent * iTypetwolu - model.payCent) / 2 + model.payCent + "|赢半";//下盘赢半
                else if (intone - inttwo>=1 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (inttwo - intone>=0 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------半球----------------------------------------------*/
            else if (model.p_pk == 3)
            {
                if (intone - inttwo>=1 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo<1 && model.PayType == iTypetwo)
                    strVal = model.payCent*iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------半球/一球----------------------------------------------*/
            else if (model.p_pk == 4)
            {
                if (intone - inttwo==1 && model.PayType == iTypeone)
                    strVal = (model.payCent * iTypeonelu - model.payCent) / 2 + model.payCent + "|赢半";//上盘赢半
                else if (intone - inttwo==1 && model.PayType == iTypetwo)
                    strVal = model.payCent / 2 + "|输半";//下盘输半
                else if (intone - inttwo>=2 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (inttwo - intone>=0 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 一球----------------------------------------------*/
            else if (model.p_pk == 5)
            {
                if (intone - inttwo == 1)
                    strVal = model.payCent + "|平盘";//平盘
                else if (intone - inttwo >= 2 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo < 2 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 一球/球半----------------------------------------------*/
            else if (model.p_pk == 6)
            {
                if (intone - inttwo == 1 && model.PayType == iTypeone)
                    strVal = model.payCent / 2 + "|输半";//上盘输半
                else if (intone - inttwo == 1 && model.PayType == iTypetwo)
                    strVal = (model.payCent * iTypetwolu - model.payCent) / 2 + model.payCent + "|赢半";//下盘赢半
                else if (intone - inttwo >= 2 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 1 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------球半----------------------------------------------*/
            else if (model.p_pk == 7)
            {
                if (intone - inttwo >= 2 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo < 2 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------球半/二球----------------------------------------------*/
            else if (model.p_pk == 8)
            {
                if (intone - inttwo == 2 && model.PayType == iTypeone)
                    strVal = (model.payCent * iTypeonelu - model.payCent) / 2 + model.payCent + "|赢半";//上盘赢半
                else if (intone - inttwo == 2 && model.PayType == iTypetwo)
                    strVal = model.payCent / 2 + "|输半";//下盘输半
                else if (intone - inttwo >= 3 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 1 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 二球----------------------------------------------*/
            else if (model.p_pk == 9)
            {
                if (intone - inttwo == 2)
                    strVal = model.payCent + "|平盘";//平盘
                else if (intone - inttwo >= 3 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 1 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 二球/二球半----------------------------------------------*/
            else if (model.p_pk == 10)
            {
                if (intone - inttwo == 2 && model.PayType == iTypeone)
                    strVal = model.payCent / 2 + "|输半";//上盘输半
                else if (intone - inttwo == 2 && model.PayType == iTypetwo)
                    strVal = (model.payCent * iTypetwolu - model.payCent) / 2 + model.payCent + "|赢半";//下盘赢半
                else if (intone - inttwo >= 3 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 1 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------二球半----------------------------------------------*/
            else if (model.p_pk == 11)
            {
                if (intone - inttwo >= 3 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 2 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------二球半/三球----------------------------------------------*/
            else if (model.p_pk == 12)
            {
                if (intone - inttwo == 3 && model.PayType == iTypeone)
                    strVal = (model.payCent * iTypeonelu - model.payCent) / 2 + model.payCent + "|赢半";//上盘赢半
                else if (intone - inttwo == 3 && model.PayType == iTypetwo)
                    strVal = model.payCent / 2 + "|输半";//下盘输半
                else if (intone - inttwo >= 4 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 2 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 三球----------------------------------------------*/
            else if (model.p_pk == 13)
            {
                if (intone - inttwo == 3)
                    strVal = model.payCent + "|平盘";//平盘
                else if (intone - inttwo >= 4 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 2 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 三球/三球半----------------------------------------------*/
            else if (model.p_pk == 14)
            {
                if (intone - inttwo == 3 && model.PayType == iTypeone)
                    strVal = model.payCent / 2 + "|输半";//上盘输半
                else if (intone - inttwo == 3 && model.PayType == iTypetwo)
                    strVal = (model.payCent * iTypetwolu - model.payCent) / 2 + model.payCent + "|赢半";//下盘赢半
                else if (intone - inttwo >= 4 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 2 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------三球半----------------------------------------------*/
            else if (model.p_pk == 15)
            {
                if (intone - inttwo >= 4 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 3 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------三球半/四球----------------------------------------------*/
            else if (model.p_pk == 16)
            {
                if (intone - inttwo == 4 && model.PayType == iTypeone)
                    strVal = (model.payCent * iTypeonelu - model.payCent) / 2 + model.payCent + "|赢半";//上盘赢半
                else if (intone - inttwo == 4 && model.PayType == iTypetwo)
                    strVal = model.payCent / 2 + "|输半";//下盘输半
                else if (intone - inttwo >= 5 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 3 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 四球----------------------------------------------*/
            else if (model.p_pk == 17)
            {
                if (intone - inttwo == 4)
                    strVal = model.payCent + "|平盘";//平盘
                else if (intone - inttwo >= 5 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 3 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 四球/四球半----------------------------------------------*/
            else if (model.p_pk == 18)
            {
                if (intone - inttwo == 4 && model.PayType == iTypeone)
                    strVal = model.payCent / 2 + "|输半";//上盘输半
                else if (intone - inttwo == 4 && model.PayType == iTypetwo)
                    strVal = (model.payCent * iTypetwolu - model.payCent) / 2 + model.payCent + "|赢半";//下盘赢半
                else if (intone - inttwo >= 5 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 3 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------四球半----------------------------------------------*/
            else if (model.p_pk == 19)
            {
                if (intone - inttwo >= 5 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 4 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------四球半/五球----------------------------------------------*/
            else if (model.p_pk == 20)
            {
                if (intone - inttwo == 5 && model.PayType == iTypeone)
                    strVal = (model.payCent * iTypeonelu - model.payCent) / 2 + model.payCent + "|赢半";//上盘赢半
                else if (intone - inttwo == 5 && model.PayType == iTypetwo)
                    strVal = model.payCent / 2 + "|输半";//下盘输半
                else if (intone - inttwo >= 6 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 4 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 五球----------------------------------------------*/
            else if (model.p_pk == 21)
            {
                if (intone - inttwo == 5)
                    strVal = model.payCent + "|平盘";//平盘
                else if (intone - inttwo >= 6 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 4 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/


            /*--------------------------- 五球/五球半----------------------------------------------*/
            else if (model.p_pk == 22)
            {
                if (intone - inttwo == 5 && model.PayType == iTypeone)
                    strVal = model.payCent / 2 + "|输半";//上盘输半
                else if (intone - inttwo == 5 && model.PayType == iTypetwo)
                    strVal = (model.payCent * iTypetwolu - model.payCent) / 2 + model.payCent + "|赢半";//下盘赢半
                else if (intone - inttwo >= 6 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 4 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------五球半----------------------------------------------*/
            else if (model.p_pk == 23)
            {
                if (intone - inttwo >= 6 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 5 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------五球半/六球----------------------------------------------*/
            else if (model.p_pk == 24)
            {
                if (intone - inttwo == 6 && model.PayType == iTypeone)
                    strVal = (model.payCent * iTypeonelu - model.payCent) / 2 + model.payCent + "|赢半";//上盘赢半
                else if (intone - inttwo == 6 && model.PayType == iTypetwo)
                    strVal = model.payCent / 2 + "|输半";//下盘输半
                else if (intone - inttwo >= 7 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 5 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 六球----------------------------------------------*/
            else if (model.p_pk == 25)
            {
                if (intone - inttwo == 6)
                    strVal = model.payCent + "|平盘";//平盘
                else if (intone - inttwo >= 7 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 5 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 六球/六球半----------------------------------------------*/
            else if (model.p_pk == 26)
            {
                if (intone - inttwo == 6 && model.PayType == iTypeone)
                    strVal = model.payCent / 2 + "|输半";//上盘输半
                else if (intone - inttwo == 6 && model.PayType == iTypetwo)
                    strVal = (model.payCent * iTypetwolu - model.payCent) / 2 + model.payCent + "|赢半";//下盘赢半
                else if (intone - inttwo >= 7 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 5 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------六球半----------------------------------------------*/
            else if (model.p_pk == 27)
            {
                if (intone - inttwo >= 7 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 6 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------六球半/七球----------------------------------------------*/
            else if (model.p_pk == 28)
            {
                if (intone - inttwo == 7 && model.PayType == iTypeone)
                    strVal = (model.payCent * iTypeonelu - model.payCent) / 2 + model.payCent + "|赢半";//上盘赢半
                else if (intone - inttwo == 7 && model.PayType == iTypetwo)
                    strVal = model.payCent / 2 + "|输半";//下盘输半
                else if (intone - inttwo >= 8 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 6 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 七球----------------------------------------------*/
            else if (model.p_pk == 29)
            {
                if (intone - inttwo == 7)
                    strVal = model.payCent + "|平盘";//平盘
                else if (intone - inttwo >= 8 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 6 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 七球/七球半----------------------------------------------*/
            else if (model.p_pk == 30)
            {
                if (intone - inttwo == 7 && model.PayType == iTypeone)
                    strVal = model.payCent / 2 + "|输半";//上盘输半
                else if (intone - inttwo == 7 && model.PayType == iTypetwo)
                    strVal = (model.payCent * iTypetwolu - model.payCent) / 2 + model.payCent + "|赢半";//下盘赢半
                else if (intone - inttwo >= 8 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 6 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------七球半----------------------------------------------*/
            else if (model.p_pk == 31)
            {
                if (intone - inttwo >= 8 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 7 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------七球半/八球----------------------------------------------*/
            else if (model.p_pk == 32)
            {
                if (intone - inttwo == 8 && model.PayType == iTypeone)
                    strVal = (model.payCent * iTypeonelu - model.payCent) / 2 + model.payCent + "|赢半";//上盘赢半
                else if (intone - inttwo == 8 && model.PayType == iTypetwo)
                    strVal = model.payCent / 2 + "|输半";//下盘输半
                else if (intone - inttwo >= 9 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 7 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*--------------------------- 八球----------------------------------------------*/
            else if (model.p_pk == 33)
            {
                if (intone - inttwo == 8)
                    strVal = model.payCent + "|平盘";//平盘
                else if (intone - inttwo >= 9 && model.PayType == iTypeone)
                    strVal = model.payCent * iTypeonelu + "|全赢";//上盘全赢
                else if (intone - inttwo <= 7 && model.PayType == iTypetwo)
                    strVal = model.payCent * iTypetwolu + "|全赢";//下盘全赢
            }
            /*-------------------------------------------------------------------------*/

            return strVal;
        }

        /// <summary>
        /// 大小盘逻辑类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string getZqdxCase(TPR2.Model.guess.BaPayMe model)
        {
            string strVal = "";
            int intone, inttwo;
            intone = Convert.ToInt32(model.p_result_one);
            inttwo = Convert.ToInt32(model.p_result_two);

            /*---------------------------0.5/1.0----------------------------------------------*/
            if (model.p_dx_pk == 21)
            {
                if (intone + inttwo == 1 && model.PayType == 3)
                    strVal = (model.payCent * model.payonLuone - model.payCent) / 2 + model.payCent + "|赢半";//大盘赢半
                else if (intone + inttwo == 1 && model.PayType == 4)
                    strVal = model.payCent / 2 + "|输半";//小盘输半
                else if (intone + inttwo >= 2 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 0 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------1.0/1.5----------------------------------------------*/
            else if (model.p_dx_pk == 23)
            {
                if (intone + inttwo == 1 && model.PayType == 3)
                    strVal = model.payCent / 2 + "|输半";//大盘输半
                else if (intone + inttwo == 1 && model.PayType == 4)
                    strVal = (model.payCent * model.payonLutwo - model.payCent) / 2 + model.payCent + "|赢半";//小盘赢半
                else if (intone + inttwo >= 2 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 0 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------1.5/2.0----------------------------------------------*/
            else if (model.p_dx_pk == 2)
            {
                if (intone + inttwo == 2 && model.PayType == 3)
                    strVal = (model.payCent * model.payonLuone - model.payCent) / 2 + model.payCent + "|赢半";//大盘赢半
                else if (intone + inttwo == 2 && model.PayType == 4)
                    strVal = model.payCent / 2 + "|输半";//小盘输半
                else if (intone + inttwo >= 3 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 1 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------2.0/2.5----------------------------------------------*/
            else if (model.p_dx_pk == 4)
            {
                if (intone + inttwo == 2 && model.PayType == 3)
                    strVal = model.payCent / 2 + "|输半";//大盘输半
                else if (intone + inttwo == 2 && model.PayType == 4)
                    strVal = (model.payCent * model.payonLutwo - model.payCent) / 2 + model.payCent + "|赢半";//小盘赢半
                else if (intone + inttwo >= 3 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 1 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------2.5/3.0----------------------------------------------*/
            else if (model.p_dx_pk == 6)
            {
                if (intone + inttwo == 3 && model.PayType == 3)
                    strVal = (model.payCent * model.payonLuone - model.payCent) / 2 + model.payCent + "|赢半";//大盘赢半
                else if (intone + inttwo == 3 && model.PayType == 4)
                    strVal = model.payCent / 2 + "|输半";//小盘输半
                else if (intone + inttwo >= 4 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 2 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------3.0/3.5----------------------------------------------*/
            else if (model.p_dx_pk == 8)
            {
                if (intone + inttwo == 3 && model.PayType == 3)
                    strVal = model.payCent / 2 + "|输半";//大盘输半
                else if (intone + inttwo == 3 && model.PayType == 4)
                    strVal = (model.payCent * model.payonLutwo - model.payCent) / 2 + model.payCent + "|赢半";//小盘赢半
                else if (intone + inttwo >= 4 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 2 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------3.5/4.0----------------------------------------------*/
            else if (model.p_dx_pk == 10)
            {
                if (intone + inttwo == 4 && model.PayType == 3)
                    strVal = (model.payCent * model.payonLuone - model.payCent) / 2 + model.payCent + "|赢半";//大盘赢半
                else if (intone + inttwo == 4 && model.PayType == 4)
                    strVal = model.payCent / 2 + "|输半";//小盘输半
                else if (intone + inttwo >= 5 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 3 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------4.0/4.5----------------------------------------------*/
            else if (model.p_dx_pk == 12)
            {
                if (intone + inttwo == 4 && model.PayType == 3)
                    strVal = model.payCent / 2 + "|输半";//大盘输半
                else if (intone + inttwo == 4 && model.PayType == 4)
                    strVal = (model.payCent * model.payonLutwo - model.payCent) / 2 + model.payCent + "|赢半";//小盘赢半
                else if (intone + inttwo >= 5 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 3 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------4.5/5.0----------------------------------------------*/
            else if (model.p_dx_pk == 14)
            {
                if (intone + inttwo == 5 && model.PayType == 3)
                    strVal = (model.payCent * model.payonLuone - model.payCent) / 2 + model.payCent + "|赢半";//大盘赢半
                else if (intone + inttwo == 5 && model.PayType == 4)
                    strVal = model.payCent / 2 + "|输半";//小盘输半
                else if (intone + inttwo >= 6 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 4 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------5.0/5.5----------------------------------------------*/
            else if (model.p_dx_pk == 16)
            {
                if (intone + inttwo == 5 && model.PayType == 3)
                    strVal = model.payCent / 2 + "|输半";//大盘输半
                else if (intone + inttwo == 5 && model.PayType == 4)
                    strVal = (model.payCent * model.payonLutwo - model.payCent) / 2 + model.payCent + "|赢半";//小盘赢半
                else if (intone + inttwo >= 6 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 4 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------5.5/6.0----------------------------------------------*/
            else if (model.p_dx_pk == 18)
            {
                if (intone + inttwo == 6 && model.PayType == 3)
                    strVal = (model.payCent * model.payonLuone - model.payCent) / 2 + model.payCent + "|赢半";//大盘赢半
                else if (intone + inttwo == 6 && model.PayType == 4)
                    strVal = model.payCent / 2 + "|输半";//小盘输半
                else if (intone + inttwo >= 7 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 5 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------6.0/6.5----------------------------------------------*/
            else if (model.p_dx_pk == 24)
            {
                if (intone + inttwo == 6 && model.PayType == 3)
                    strVal = model.payCent / 2 + "|输半";//大盘输半
                else if (intone + inttwo == 6 && model.PayType == 4)
                    strVal = (model.payCent * model.payonLutwo - model.payCent) / 2 + model.payCent + "|赢半";//小盘赢半
                else if (intone + inttwo >= 7 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 5 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------6.5/7.0----------------------------------------------*/
            else if (model.p_dx_pk == 26)
            {
                if (intone + inttwo == 7 && model.PayType == 3)
                    strVal = (model.payCent * model.payonLuone - model.payCent) / 2 + model.payCent + "|赢半";//大盘赢半
                else if (intone + inttwo == 7 && model.PayType == 4)
                    strVal = model.payCent / 2 + "|输半";//小盘输半
                else if (intone + inttwo >= 8 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 6 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------7.0/7.5----------------------------------------------*/
            else if (model.p_dx_pk == 28)
            {
                if (intone + inttwo == 7 && model.PayType == 3)
                    strVal = model.payCent / 2 + "|输半";//大盘输半
                else if (intone + inttwo == 7 && model.PayType == 4)
                    strVal = (model.payCent * model.payonLutwo - model.payCent) / 2 + model.payCent + "|赢半";//小盘赢半
                else if (intone + inttwo >= 8 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 6 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------7.5/8.0----------------------------------------------*/
            else if (model.p_dx_pk == 30)
            {
                if (intone + inttwo == 8 && model.PayType == 3)
                    strVal = (model.payCent * model.payonLuone - model.payCent) / 2 + model.payCent + "|赢半";//大盘赢半
                else if (intone + inttwo == 8 && model.PayType == 4)
                    strVal = model.payCent / 2 + "|输半";//小盘输半
                else if (intone + inttwo >= 9 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 7 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------8.0/8.5----------------------------------------------*/
            else if (model.p_dx_pk == 32)
            {
                if (intone + inttwo == 8 && model.PayType == 3)
                    strVal = model.payCent / 2 + "|输半";//大盘输半
                else if (intone + inttwo == 8 && model.PayType == 4)
                    strVal = (model.payCent * model.payonLutwo - model.payCent) / 2 + model.payCent + "|赢半";//小盘赢半
                else if (intone + inttwo >= 9 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 7 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            /*---------------------------8.5/9.0----------------------------------------------*/
            else if (model.p_dx_pk == 34)
            {
                if (intone + inttwo == 9 && model.PayType == 3)
                    strVal = (model.payCent * model.payonLuone - model.payCent) / 2 + model.payCent + "|赢半";//大盘赢半
                else if (intone + inttwo == 9 && model.PayType == 4)
                    strVal = model.payCent / 2 + "|输半";//小盘输半
                else if (intone + inttwo >= 10 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 8 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/


            /*---------------------------9.0/9.5----------------------------------------------*/
            else if (model.p_dx_pk == 36)
            {
                if (intone + inttwo == 9 && model.PayType == 3)
                    strVal = model.payCent / 2 + "|输半";//大盘输半
                else if (intone + inttwo == 9 && model.PayType == 4)
                    strVal = (model.payCent * model.payonLutwo - model.payCent) / 2 + model.payCent + "|赢半";//小盘赢半
                else if (intone + inttwo >= 10 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 8 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/


            /*---------------------------9.5/10.0----------------------------------------------*/
            else if (model.p_dx_pk == 38)
            {
                if (intone + inttwo == 10 && model.PayType == 3)
                    strVal = (model.payCent * model.payonLuone - model.payCent) / 2 + model.payCent + "|赢半";//大盘赢半
                else if (intone + inttwo == 10 && model.PayType == 4)
                    strVal = model.payCent / 2 + "|输半";//小盘输半
                else if (intone + inttwo >= 11 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo <= 9 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/


            /*---------------------------大小盘单盘口----------------------------------------------*/
            else
            {
                decimal dx_pk = Convert.ToDecimal(GCK.getDxPkName(Convert.ToInt32(model.p_dx_pk)));
                if (intone + inttwo - dx_pk == 0)
                    strVal = model.payCent + "|平盘";//平盘
                else if (intone + inttwo - dx_pk > 0 && model.PayType == 3)
                    strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
                else if (intone + inttwo - dx_pk < 0 && model.PayType == 4)
                    strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            }
            /*-------------------------------------------------------------------------*/

            return strVal;
        }

        /// <summary>
        /// 标准盘逻辑类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string getZqbzCase(TPR2.Model.guess.BaPayMe model)
        {
            string strVal = "";
            int intone, inttwo;
            intone = Convert.ToInt32(model.p_result_one);
            inttwo = Convert.ToInt32(model.p_result_two);
            /*---------------------------标准欧盘----------------------------------------------*/
            if (intone > inttwo && model.PayType == 5)
                strVal = model.payCent * model.payonLuone + "|全赢";//主胜全赢
            if (intone == inttwo && model.PayType == 6)
                strVal = model.payCent * model.payonLutwo + "|全赢";//平手全赢
            if (intone < inttwo && model.PayType == 7)
                strVal = model.payCent * model.payonLuthr + "|全赢";//客胜全赢
            /*---------------------------标准欧盘----------------------------------------------*/
            return strVal;
        }
    }
}
