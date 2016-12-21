using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using Book.Model;
namespace Book.BLL
{
	/// <summary>
	/// ҵ���߼���ShuBbs ��ժҪ˵����
    /// 
    /// 2016-08-23 ���ӵ�ֵ�齱���������ӿ� ���ڽ�
	/// </summary>
	public class ShuBbs
	{
		private readonly Book.DAL.ShuBbs dal=new Book.DAL.ShuBbs();
		public ShuBbs()
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
		public int  Add(Book.Model.ShuBbs model)
		{
			int id= dal.Add(model);

            try
            {
                int usid = model.usid;
                string username = model.usname;
                string Notes = "��������";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//��ֵ�齱
            }
            catch { }

            return id;
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(Book.Model.ShuBbs model)
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
		/// �õ�һ������ʵ��
		/// </summary>
		public Book.Model.ShuBbs GetShuBbs(int id)
		{
			
			return dal.GetShuBbs(id);
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
		/// <returns>IList ShuBbs</returns>
		public IList<Book.Model.ShuBbs> GetShuBbss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetShuBbss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

