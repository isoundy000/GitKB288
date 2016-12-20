using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类File 的摘要说明。
	/// </summary>
	public class File
	{
		private readonly BCW.DAL.File dal=new BCW.DAL.File();
		public File()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
                
        /// <summary>
        /// 计算某主题的文件数
        /// </summary>
        public int GetCount(int NodeId)
        {
            return dal.GetCount(NodeId);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.File model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.File model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// 更新下载次数
        /// </summary>
        public void UpdateDownNum(int ID, int DownNum)
        {
            dal.UpdateDownNum(ID, DownNum);
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
        public void Delete2(int NodeId)
        {
            dal.Delete2(NodeId);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.File GetFile(int ID)
		{
			
			return dal.GetFile(ID);
		}
      
        /// <summary>
        /// 得到一个Files
        /// </summary>
        public string GetFiles(int ID)
        {
            return dal.GetFiles(ID);
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
		/// <returns>IList File</returns>
		public IList<BCW.Model.File> GetFiles(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetFiles(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

