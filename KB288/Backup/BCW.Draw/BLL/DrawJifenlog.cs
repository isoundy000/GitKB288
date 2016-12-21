using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Draw.BLL;
namespace BCW.Draw.BLL
{
	/// <summary>
	/// ҵ���߼���Goldlog ��ժҪ˵����
	/// </summary>
	public class DrawJifenlog
	{
        private readonly BCW.Draw.DAL.DrawJifenlog dal = new BCW.Draw.DAL.DrawJifenlog();
        public DrawJifenlog()
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
		public int  Add(BCW.Draw.Model.DrawJifenlog model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// N���ڣ���ID�Ƿ����ѹ�
        /// </summary>
        public bool ExistsUsID(int UsID, int Sec)
        {
            return dal.ExistsUsID(UsID, Sec);
        }

        /// <summary>
        /// �õ�һ����ĳ���û��Ǿ����������ɳ齱��ֵ��ֵ
        /// </summary>
        public int GetJfbyTz(int UsId)
        {
            return dal.GetJfbyTz(UsId);
        }

        /// <summary>
        /// �õ�һ����ĳ���û��Ǿ�����Ϸ����������ɳ齱��ֵ��ֵ
        /// </summary>
        public int GetJfbyGame(int UsId)
        {
            return dal.GetJfbyGame(UsId);
        }

        /// <summary>
        /// �õ�һ����ĳ���û��Ǿ������ϳ�ֵ�������ɳ齱��ֵ��ֵ
        /// </summary>
        public int GetJfbyCz(int UsId)
        {
            return dal.GetJfbyCz(UsId);
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
		public BCW.Draw.Model.DrawJifenlog GetJifenlog(int ID)
		{

            return dal.GetJifenlog(ID);
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
		/// <returns>IList Goldlog</returns>
		public IList<BCW.Draw.Model.DrawJifenlog> GetJifenlogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
            return dal.GetJifenlogs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// ��Ϣ��־�ظ���ѯ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Draw.Model.DrawJifenlog> GetJifenlogsCF(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetJifenlogsCF(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

