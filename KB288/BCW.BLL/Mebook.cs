using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Mebook 的摘要说明。
	/// </summary>
	public class Mebook
	{
		private readonly BCW.DAL.Mebook dal=new BCW.DAL.Mebook();
		public Mebook()
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
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }

        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists2(int ID, int UsID)
        {
            return dal.Exists2(ID, UsID);
        }

        /// <summary>
        /// 计算某会员ID留言本留言数
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }
                
        /// <summary>
        /// 计算某会员ID发表的留言数
        /// </summary>
        public int GetIDCount(int MID)
        {
            return dal.GetIDCount(MID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Mebook model)
		{
			int ID= dal.Add(model);        
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                string ActionText = (ub.GetSub("ActionText", xmlPath));//Action语句
                string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action语句开关
                int usid = model.MID;
                string username = model.MName;
                string Notes = "空间留言";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//发内线到该ID
                    }
                }
                return ID;
            }
            catch { return ID; }
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Mebook model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// 更新回复内容
        /// </summary>
        public void UpdateReText(int ID, string ReText)
        {
            dal.UpdateReText(ID, ReText);
        }
               
        /// <summary>
        /// 更新是否置顶
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {
            dal.UpdateIsTop(ID, IsTop);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
        /// <summary>
        /// 邵广林 20160526 增加删除农场留言
        /// 删除农场留言数据
        /// </summary>
        public void Delete_farm(int ID)
        {

            dal.Delete_farm(ID);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(int UsID, int MID)
        {

            dal.Delete(UsID, MID);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 得到一个MID
        /// </summary>
        public int GetMID(int ID)
        {
            return dal.GetMID(ID);
        }

        /// <summary>
        /// 得到一个IsTop
        /// </summary>
        public int GetIsTop(int ID)
        {
            return dal.GetIsTop(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Mebook GetMebook(int ID)
		{
			
			return dal.GetMebook(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Mebook</returns>
        public IList<BCW.Model.Mebook> GetMebooks(int SizeNum, string strWhere)
        {
            return dal.GetMebooks(SizeNum, strWhere);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList Mebook</returns>
		public IList<BCW.Model.Mebook> GetMebooks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetMebooks(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

