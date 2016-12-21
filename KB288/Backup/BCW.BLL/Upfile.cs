using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Upfile 的摘要说明。
	/// </summary>
	public class Upfile
	{
		private readonly BCW.DAL.Upfile dal=new BCW.DAL.Upfile();
		public Upfile()
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
        public bool Exists(int ID, int UsID, int Types)
        {
            return dal.Exists(ID, UsID, Types);
        }
                
        /// <summary>
        /// 计算某用户今天上传文件数量
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            return dal.GetTodayCount(UsID);
        }
               
        /// <summary>
        /// 计算某用户文件数量
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// 计算某用户某相集相片数量
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            return dal.GetCount(UsID, NodeId);
        }
 
        /// <summary>
        /// 计算某用户某相集相片数量
        /// </summary>
        public int GetCount(int UsID, int Types, int NodeId)
        {
            return dal.GetCount(UsID, Types, NodeId);
        }

		/// <summary>
		/// 增加一条数据
        /// 上存照片活跃抽奖入口---姚志光
		/// </summary>
		public int  Add(BCW.Model.Upfile model)
		{
			//return dal.Add(model);
            int ID = dal.Add(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "上传照片";
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
		public void Update(BCW.Model.Upfile model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// 更新描述信息
        /// </summary>
        public void Update(int ID, string Content)
        {
            dal.Update(ID, Content);
        }
                     
        /// <summary>
        /// 审核文件
        /// </summary>
        public void UpdateIsVerify(int ID, int IsVerify)
        {
            dal.UpdateIsVerify(ID, IsVerify);
        }

        /// <summary>
        /// 更新下载次数
        /// </summary>
        public void UpdateDownNum(int ID, int DownNum)
        {
            dal.UpdateDownNum(ID, DownNum);
        }

        /// <summary>
        /// 更新某相集相片成为默认分类
        /// </summary>
        public void UpdateNodeIds(int UsID, int Types, int NodeId)
        {
            dal.UpdateNodeIds(UsID, Types, NodeId);
        }
               
        /// <summary>
        /// 转移文件
        /// </summary>
        public void UpdateNodeId(int UsID, int ID, int NodeId)
        {
            dal.UpdateNodeId(UsID, ID, NodeId);
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
        public void Delete(int UsID, int Types, int NodeId)
        {
            dal.Delete(UsID, Types, NodeId);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 得到一个Title
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }
                
        /// <summary>
        /// 得到一个Files
        /// </summary>
        public string GetFiles(int ID)
        {
            return dal.GetFiles(ID);
        }

        /// <summary>
        /// 得到一个用户ID
        /// </summary>
        public int GetUsID(int ID)
        {
            return dal.GetUsID(ID);
        }
           
        /// <summary>
        /// 得到一个Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }

        /// <summary>
        /// 得到一个NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Upfile GetUpfileMe(int ID)
        {
            return dal.GetUpfileMe(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Upfile GetUpfile(int ID)
		{
			
			return dal.GetUpfile(ID);
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Upfile GetUpfile(int ID, int Types)
        {

            return dal.GetUpfile(ID, Types);
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
        /// <returns>IList Upfile</returns>
        public IList<BCW.Model.Upfile> GetUpfiles(int SizeNum, string strWhere)
        {
            return dal.GetUpfiles(SizeNum, strWhere);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList Upfile</returns>
		public IList<BCW.Model.Upfile> GetUpfiles(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetUpfiles(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

