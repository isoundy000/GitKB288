using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Draw.Model;
namespace BCW.Draw.BLL
{
	/// <summary>
	/// 业务逻辑类DrawJifen 的摘要说明。
	/// </summary>
	public class DrawJifen
	{
		private readonly BCW.Draw.DAL.DrawJifen dal=new BCW.Draw.DAL.DrawJifen();
		public DrawJifen()
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
        /// 得到用户积分
        /// </summary>
        public int GetJifen(int UsID)
        {
            return dal.GetJifen(UsID);
        }

        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <param name="iGold">操作币</param>
        public void UpdateJifen(int UsID, int Jifen)
        {
            dal.UpdateJifen(UsID, Jifen);
        }

        public void UpdateJifen(int UsID, int Jifen, string AcText)
        {
            string UsName = dal.GetUsName(UsID);
            UpdateJifen(UsID, UsName, Jifen, AcText);
        }

        public void UpdateJifen(int UsID, string UsName, int Jifen, string AcText)
        {
            if (new BCW.Draw.BLL.DrawJifen().ExistsUsID(UsID))
            {
                //更新用户虚拟币
                dal.UpdateJifen(UsID, Jifen);
                //更新消费记录
                BCW.Draw.Model.DrawJifenlog model = new BCW.Draw.Model.DrawJifenlog();
                if (AcText.Contains("签到"))
                {
                    model.Types = 1;//签到类型为1
                }
                else
                {
                    model.Types = 0;
                }
                model.PUrl = Utils.getPageUrl();//操作的文件名
                model.UsId = UsID;
                model.UsName = UsName;
                model.AcGold = Jifen;
                model.AfterGold = GetJifen(UsID);//更新后的币数
                model.AcText = AcText;
                model.AddTime = DateTime.Now;
                new BCW.Draw.BLL.DrawJifenlog().Add(model);
            }
        }

        /// <summary>
        /// 更新累计签到天数
        /// </summary>
        public void UpdateQd(int UsID,int Qd)
        {
            dal.UpdateQd(UsID,Qd);
        }
        /// <summary>
        /// 更新连续签到天数
        /// </summary>
        public void UpdateQdweek(int UsID,int Qdweek)
        {
            dal.UpdateQdweek(UsID,Qdweek);
        }
        /// <summary>
        /// 更新上次签到时间
        /// </summary>
        public void UpdateQdTime(int UsID ,DateTime QdTime)
        {
            dal.UpdateQdTime(UsID, QdTime);
        }

        /// <summary>
        /// 是否存在该usid记录
        /// </summary>
        public bool ExistsUserID(int ID)
        {
            return dal.ExistsUserID(ID);
        }
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
        /// <summary>
        /// 是否存在该UsID
        /// </summary>
        public bool ExistsUsID(int UsID)
        {
            return dal.ExistsUsID(UsID);
        }

        /// <summary>
        /// 得到每日签到结果
        /// </summary>
        public int getsQd(int UsID)
        {
            return dal.getsQd(UsID);
        }

        /// <summary>
        /// 得到连续签到结果
        /// </summary>
        public int getsQdweek(int UsID)
        {
            return dal.getsQdweek(UsID);
        }
        /// <summary>
        /// 得到签到时间
        /// </summary>
        public DateTime getQdTime(int UsID)
        {
            return dal.getQdTime(UsID);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Draw.Model.DrawJifen model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Draw.Model.DrawJifen model)
		{
			dal.Update(model);
		}
  

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Draw.Model.DrawJifen GetDrawJifen(int ID)
		{
			
			return dal.GetDrawJifen(ID);
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
		/// <returns>IList DrawJifen</returns>
        public IList<BCW.Draw.Model.DrawJifen> GetDrawJifens(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetDrawJifens(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
		}

		#endregion  成员方法
	}
}

