using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// ҵ���߼���ShuMu ��ժҪ˵����
	/// </summary>
	public class ShuMu
	{
		private readonly Book.DAL.ShuMu dal=new Book.DAL.ShuMu();
		public ShuMu()
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
        /// �Ƿ����ĳ�����¼
        /// </summary>
        public bool ExistsNode(int nid)
        {
            return dal.ExistsNode(nid);
        }
               
        /// <summary>
        /// ����δ��˵��鱾����
        /// </summary>
        public int GetCount(int nid)
        {
            return dal.GetCount(nid);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(Book.Model.ShuMu model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(Book.Model.ShuMu model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ����һ������(��̨ʹ��)
        /// </summary>
        public void Update2(Book.Model.ShuMu model)
        {
            dal.Update2(model);
        }
               
        /// <summary>
        /// �Ƽ��鱾
        /// </summary>
        public void UpdateGood(int id)
        {
            dal.UpdateGood(id);
        }
              
        /// <summary>
        /// �ָ���ɾ��
        /// </summary>
        public void Updateisdel(int id, int isdel)
        {
            dal.Updateisdel(id, isdel);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateClick(int id)
        {
            dal.UpdateClick(id);
        }

        /// <summary>
        /// ����鱾
        /// </summary>
        public void Updatestate(int id, int state)
        {
            dal.Updatestate(id, state);
        }

        /// <summary>
        /// д�����ʱ��
        /// </summary>
        public void Updategxtime(int id)
        {
            dal.Updategxtime(id);
        }
              
        /// <summary>
        /// д������ID
        /// </summary>
        public void Updategxids(int id, string gxids)
        {
            dal.Updategxids(id, gxids);
        }
                   
        /// <summary>
        /// д��������Ŀ
        /// </summary>
        public void Updatepl(int id, int pl)
        {
            dal.Updatepl(id, pl);
        }

        /// <summary>
        /// ���´���ID��ת���ã�
        /// </summary>
        public void Updatenid(int id, int nid)
        {
            dal.Updatenid(id, nid);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int id)
		{
			
			dal.Delete(id);
		}
        
        /// <summary>
        /// �õ�����ID
        /// </summary>
        public int Getnid(int id)
        {
            return dal.Getnid(id);
        }
             
        /// <summary>
        /// �õ����ѵĻ�ԱID
        /// </summary>
        public string Getgxids(int id)
        {
            return dal.Getgxids(id);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Book.Model.ShuMu GetShuMu(int id)
		{
			
			return dal.GetShuMu(id);
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
		/// <returns>IList ShuMu</returns>
		public IList<Book.Model.ShuMu> GetShuMus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetShuMus(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList ShuMu</returns>
        public IList<Book.Model.ShuMu> GetShuMus(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetShuMus(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
               
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList ShuMu</returns>
        public IList<Book.Model.ShuMu> GetShuMusTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetShuMusTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

