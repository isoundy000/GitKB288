using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// ҵ���߼���Favorites ��ժҪ˵����
	/// </summary>
	public class Favorites
	{
		private readonly Book.DAL.Favorites dal=new Book.DAL.Favorites();
		public Favorites()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}
                
        /// <summary>
        /// �Ƿ������ܼ�¼
        /// </summary>
        public bool Exists2(int usid, int nid, int sid)
        {
            return dal.Exists2(usid, nid, sid);
        }

        /// <summary>
        /// �Ƿ������ǩ��¼
        /// </summary>
        public bool Exists3(int usid, int nid, int sid, string purl)
        {
            return dal.Exists3(usid, nid, sid, purl);
        }

            
        /// <summary>
        /// ����ĳ����ղ�����
        /// </summary>
        public int GetCount(int favid)
        {
            return dal.GetCount(favid);
        }
            
        /// <summary>
        /// ����ĳ�û���ǩ�ղ�����
        /// </summary>
        public int GetCount(int usid, int types)
        {
            return dal.GetCount(usid, types);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(Book.Model.Favorites model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(Book.Model.Favorites model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int id)
		{
			
			dal.Delete(id);
		}
             
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Book.Model.Favorites GetFavorites(int id)
		{
			
			return dal.GetFavorites(id);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Favorites</returns>
		public IList<Book.Model.Favorites> GetFavoritess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetFavoritess(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

