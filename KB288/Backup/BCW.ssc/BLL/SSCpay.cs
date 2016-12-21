using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
namespace BCW.ssc.BLL
{
	/// <summary>
	/// 业务逻辑类SSCpay 的摘要说明。
	/// </summary>
	public class SSCpay
	{
		private readonly BCW.ssc.DAL.SSCpay dal=new BCW.ssc.DAL.SSCpay();
		public SSCpay()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
                
        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
        /// <summary>
        /// 存在机器人
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="WinUserID"></param>
        /// <returns></returns>
        public bool ExistsReBot(int ID, int UsID)
        {
            return dal.ExistsReBot(ID, UsID);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.ssc.Model.SSCpay model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add2(BCW.ssc.Model.SSCpay model)
        {
            return dal.Add2(model);
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.ssc.Model.SSCpay model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
        }
        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState1(int ID, int State)
        {
            dal.UpdateState1(ID, State);
        }
        /// <summary>
        /// 更新赢钱
        /// </summary>
        public void UpdateWincent(int ID, long  Wincent)
        {
            dal.UpdateWincent(ID, Wincent);
        }   
        /// <summary>
        /// 更新时时彩开奖结果
        /// </summary>
        public void UpdateResult(int SSCId, string Result)
        {
            dal.UpdateResult(SSCId, Result);
        }
        /// <summary>
        /// 更新时时彩开奖结果
        /// </summary>
        public void UpdateResult1(int SSCId, string Result)
        {
            dal.UpdateResult1(SSCId, Result);
        }
        /// <summary>
        /// 更新时时彩开奖结果
        /// </summary>
        public void UpdateWinNotes(int ID, string WinNotes)
        {
            dal.UpdateWinNotes(ID, WinNotes);
        }
                
        /// <summary>
        /// 更新游戏开奖得币
        /// </summary>
        public void UpdateWinCent(int ID, int SSCId, int UsID, string UsName,int Types, long WinCent, string WinNotes)
        {
               string GameName = ub.GetSub("SSCName", "/Controls/ssc.xml");
            if (new BCW.ssc.BLL.SSCpay().GetIsSpier(ID) != 1)//ub.GetSub("SSCGuestSet", "/Controls/ssc.xml") == "0" && 
            {
                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的[url=/bbs/game/ssc.aspx]" + GameName + "[/url]:" + SSCId + "期" + OutType(Types) + "已经开奖，获得了" + WinCent + ub.Get("SiteBz") + "[url=/bbs/game/ssc.aspx?act=case]>>马上兑奖[/url]");//开奖提示信息,1表示开奖信息
            }

            dal.UpdateWinCent(ID, WinCent, WinNotes);
            //机器人自动兑奖
            if (new BCW.ssc.BLL.SSCpay().GetIsSpier(ID) == 1)
            {
                new BCW.ssc.BLL.SSCpay().UpdateState(ID, 2);
               BCW.ssc.Model.SSClist n=  new BCW.ssc.BLL.SSClist().GetSSClistbySSCId(SSCId);
                new BCW.BLL.User().UpdateiGold(UsID, UsName, WinCent, 11, "" + GameName + "-第[url=./game/ssc.aspx?act=view&amp;id=" + n.ID + "&amp;ptype=2]" + SSCId + "[/url]期-兑奖|标识ID" + ID + "");
            }
        }
        #region 下注类型 OutType
        /// <summary>
        /// 下注类型
        /// </summary>
        /// <param name="Types"></param>
        /// <returns></returns>
        private string OutType(int Types)
        {
            string ptypey = string.Empty;
            string payname1 = string.Empty;
            string odds1 = string.Empty;
            string oddsc1 = string.Empty;
            string rule1 = string.Empty;
            for (int i = 1; i < 57; i++)
            {
                ptypey = ub.GetSub("ptype" + i + "", "/Controls/ssc.xml");
                string[] ptypef = ptypey.Split('#');
                payname1 += "#" + ptypef[0];
                odds1 += "#" + ptypef[1];
                oddsc1 += "#" + ptypef[2];
                rule1 += "#" + ptypef[3];
            }
            string[] payname2 = payname1.Split('#');
            string[] odds2 = odds1.Split('#');
            string[] oddsc2 = oddsc1.Split('#');
            string[] rule2 = rule1.Split('#');
            string pText = string.Empty;

            for (int i = 1; i < 57; i++)
            {
                if (Types == i)
                    pText = payname2[i];
            }

            return pText;
        }
        #endregion

        ///<summary>
        ///某期某种投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypes(int Types, int SSCId)
        {
            return dal.GetSumPricebyTypes(Types, SSCId);
        }
        ///<summary>
        ///某期有牛投了多少币
        /// </summary>
        public long GetSumPriceby23(int Types, int SSCId,int X)
        {
            return dal.GetSumPriceby23(Types, SSCId,X);
        }
        ///<summary>
        ///某期五门投了多少币
        /// </summary>
        public long GetSumPriceby27(int Types, int SSCId,int X)
        {
            return dal.GetSumPriceby27(Types, SSCId,X);
        }

        ///<summary>
        ///某期大小投了多少币
        /// </summary>
        public long GetSumPricebyDX(int Types, int SSCId, int X)
        {
            return dal.GetSumPricebyDX(Types, SSCId, X);
        }

        ///<summary>
        ///某期单双投了多少币
        /// </summary>
        public long GetSumPricebyDS(int Types, int SSCId, int X)
        {
            return dal.GetSumPricebyDS(Types, SSCId, X);
        }

        ///<summary>
        ///某期总和大小投了多少币
        /// </summary>
        public long GetSumPricebyHD(int Types, int SSCId, int X)
        {
            return dal.GetSumPricebyHD(Types, SSCId, X);
        }

        ///<summary>
        ///某期和单双投了多少币
        /// </summary>
        public long GetSumPricebyHDx(int Types, int SSCId, int X)
        {
            return dal.GetSumPricebyHDx(Types, SSCId, X);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }
                
        /// <summary>
        /// 某期某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int SSCId)
        {
            return dal.GetSumPrices(UsID, SSCId);
        }

        /// <summary>
        /// 某期某投注方式某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int SSCId,int ptype)
        {
            return dal.GetSumPrices(UsID, SSCId,ptype);
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
        /// <summary>
        /// 得到一个State
        /// </summary>
        public long GetState(int ID)
        {
            return dal.GetState(ID);
        }

        /// <summary>
        /// 得到一个IsSpier
        /// </summary>
        public int GetIsSpier(int ID)
        {
            return dal.GetIsSpier(ID);
        }

        /// <summary>
        /// 根据条件计算币本金值
        /// </summary>
        public long GetSumPrices(string strWhere)
        {
            return dal.GetSumPrices(strWhere);
        }

        /// <summary>
        /// 根据条件计算返彩值
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            return dal.GetSumWinCent(strWhere);
        }    
        
        /// <summary>
        /// 得到一个WinCentNotes
        /// </summary>
        public string GetWinNotes(int ID)
        {

            return dal.GetWinNotes(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.ssc.Model.SSCpay GetSSCpay(int ID)
		{
			
			return dal.GetSSCpay(ID);
		}

        public int GetRecordCount(string strWhere)
        {
            return dal.GetRecordCount(strWhere);
        }

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList SSCpay</returns>
		public IList<BCW.ssc.Model.SSCpay> GetSSCpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSSCpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}


        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.ssc.Model.SSCpay> GetSSCpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSSCpaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
		#endregion  成员方法
	}
}

