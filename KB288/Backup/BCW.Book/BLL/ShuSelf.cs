using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// ҵ���߼���ShuSelf ��ժҪ˵����
	/// </summary>
	public class ShuSelf
	{
		private readonly Book.DAL.ShuSelf dal=new Book.DAL.ShuSelf();
		public ShuSelf()
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
		/// ����һ������
		/// </summary>
		public int  Add(Book.Model.ShuSelf model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(Book.Model.ShuSelf model)
		{
			dal.Update(model);
		}
             
        /// <summary>
        /// ����Ĭ������
        /// </summary>
        public void UpdatePageNum(int aid, int pagenum)
        {
            dal.UpdatePageNum(aid, pagenum);
        }
              
        /// <summary>
        /// д������ID
        /// </summary>
        public void Updategxids(int aid, string gxids)
        {
            dal.Updategxids(aid, gxids);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int id)
		{
			
			dal.Delete(id);
		}
              
        /// <summary>
        /// �õ�Ĭ������
        /// </summary>
        public int GetPageNum(int aid)
        {
            return dal.GetPageNum(aid);
        }
          
        /// <summary>
        /// �õ����ѵ��鱾ID
        /// </summary>
        public string Getgxids(int aid)
        {
            return dal.Getgxids(aid);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Book.Model.ShuSelf GetShuSelf(int aid)
		{
			
			return dal.GetShuSelf(aid);
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
		/// <returns>IList ShuSelf</returns>
		public IList<Book.Model.ShuSelf> GetShuSelfs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetShuSelfs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

