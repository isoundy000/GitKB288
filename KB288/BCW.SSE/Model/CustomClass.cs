using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;


#region  一些自己定义的结构类
namespace SseManageClass
{
    //订单类.
    public class ManagerOrder 
    {
        public int id;
        public byte orderType;
        public int sseNo;
        public DateTime orderDateTime;
        public int userId;
        public bool buyType;
        public decimal buyMoney;
        public bool isGet;
        public DateTime openDateTime;
        public decimal prizeVal;
        public string state;

        public ManagerOrder()
        {
        }
    }


    //奖池概览
    public class ManagerPoolOveriew
    {
        public int sseNo;
        public decimal poolVal;
        public string openResult;
    }

    //奖池明细
    public class ManagerPoolDetial
    {
        public int poolType;            //金钱类型
        public int operType;            //操作精英 
        public DateTime changeTime;     //变动时间
        public decimal  changeMoney;       //变动金额
        public decimal totalMoney;         //结余金额   
        public string bz;               //变动说明
        public int sseNo;               //所属期数
    }


}



#endregion