using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Draw.Model;
namespace BCW.Draw.BLL
{
	/// <summary>
	/// ҵ���߼���Drawlist ��ժҪ˵����
	/// </summary>
	public class Drawlist
	{
		private readonly BCW.Draw.DAL.Drawlist dal=new BCW.Draw.DAL.Drawlist();
		public Drawlist()
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
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Draw.Model.Drawlist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Draw.Model.Drawlist model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ����Jifen��ֵ
        /// </summary>
        public int GetJifensum(string  Jifen, string strWhere)
        {
            return dal.GetJifensum(Jifen, strWhere);
        }
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Draw.Model.Drawlist GetDrawlist(int ID)
		{
			
			return dal.GetDrawlist(ID);
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
		/// <returns>IList Drawlist</returns>
        public IList<BCW.Draw.Model.Drawlist> GetDrawlists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetDrawlists(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

