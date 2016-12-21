using System;
using System.Collections.Generic;
using System.Text;

namespace TPR.Common.Guess
{
    public class LqClass
    {
        /// <summary>
        /// 篮球让球逻辑类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string getLqsxCase(TPR.Model.guess.BaPay model)
        {
            string strVal = "";
            /*---------------------------让球盘----------------------------------------------*/
            if (model.p_result_one - model.p_result_two == model.p_pk)
                strVal = model.payCent + "|平盘";//平盘
            else if ((model.p_result_one - model.p_result_two) - model.p_pk>0 && model.PayType==1)
                strVal = model.payCent * model.payonLuone + "|全赢";//上盘全赢
            else if ((model.p_result_one - model.p_result_two) - model.p_pk < 0 && model.PayType == 2)
                strVal = model.payCent * model.payonLutwo + "|全赢";//下盘全赢
            /*--------------------------------------------------------------------------------*/
            return strVal;
        }

        public static string getLqdxCase(TPR.Model.guess.BaPay model)
        {
            string strVal = "";
            /*---------------------------大小盘----------------------------------------------*/
            if (model.p_result_one + model.p_result_two - model.p_dx_pk == 0)
                strVal = model.payCent + "|平盘";//平盘
            else if ((model.p_result_one + model.p_result_two) - model.p_dx_pk > 0 && model.PayType == 3)
                strVal = model.payCent * model.payonLuone + "|全赢";//大盘全赢
            else if ((model.p_result_one + model.p_result_two) - model.p_dx_pk < 0 && model.PayType == 4)
                strVal = model.payCent * model.payonLutwo + "|全赢";//小盘全赢
            /*-------------------------------------------------------------------------------*/
            return strVal;
        }
    }
}
