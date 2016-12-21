using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Bslist ��ժҪ˵����
	/// </summary>
	public class Bslist
	{
		private readonly BCW.DAL.Game.Bslist dal=new BCW.DAL.Game.Bslist();
		public Bslist()
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
		public int  Add(BCW.Model.Game.Bslist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Bslist model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateMoney(int ID, long Money)
        {
            dal.UpdateMoney(ID, Money);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateClick(int ID)
        {
            dal.UpdateClick(ID);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateBasic(BCW.Model.Game.Bslist model)
        {
            dal.UpdateBasic(model);
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
		public BCW.Model.Game.Bslist GetBslist(int ID)
		{
			
			return dal.GetBslist(ID);
		}
        
        /// <summary>
        /// �õ�һ��GetID
        /// </summary>
        public int GetID(int UsID, int BzType)
        {
            return dal.GetID(UsID, BzType);
        }
                
        /// <summary>
        /// �õ�һ��Title
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
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
        /// <param name="strOrder">��������</param>
        /// <returns>IList Bslist</returns>
        public IList<BCW.Model.Game.Bslist> GetBslists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
			return dal.GetBslists(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

