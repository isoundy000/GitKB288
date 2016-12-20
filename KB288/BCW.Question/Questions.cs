using System;
using System.Collections.Generic;
using System.Collections;
//using System.Linq;
using System.Text;
using BCW.Common;
using BCW.Model;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.IO;

namespace BCW.Question
{
   public  class Questions
    {




        ///通过count问题数量，diffcult问题难度，style问题类型
        ///返回对应的问题列表
        public string getQuestionsList(int count, int deficult, int style)
        {
            string strWhere = " deficult=" + deficult + " and " + "type=" + style + " and deficult=" + deficult;
            DataSet ds = new BCW.BLL.tb_QuestionsList().GetList(" * ", " ");
          //  string numList = getNumlist(count, ds.Tables[0].Rows.Count);
          
            ArrayList idList= new ArrayList(); ;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    idList.Add(ds.Tables[0].Rows[i]["ID"].ToString());
                }
            }
            string getID = getNumlist(count, ds.Tables[0].Rows.Count,idList);
            return getID;


           // return "";
        }

        //获取问题列表的ID列
        public string getNumlist(int count, int dsCount,ArrayList list)
        {
            int num = count;
            //数组容器
            ArrayList arr = list;
            Random ran = new Random(unchecked((int)DateTime.Now.Ticks));
            string questID = "";
           
            for (int i = 1; i <= count;i++)
            {
                int r = ran.Next(0, arr.Count);
                questID += arr[r] + "#";
                arr.Remove(arr[r]);
            }
            return questID;
        }
    }
}
