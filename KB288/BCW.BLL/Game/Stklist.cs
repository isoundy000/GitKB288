using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Stklist ��ժҪ˵����
	/// </summary>
	public class Stklist
	{
		private readonly BCW.DAL.Game.Stklist dal=new BCW.DAL.Game.Stklist();
		public Stklist()
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
        public bool Exists()
        {
            return dal.Exists();
        }

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
                
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(DateTime EndTime)
        {
            return dal.Exists(EndTime);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.Stklist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Stklist model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// ���±��ڼ�¼
        /// </summary>
        public void Update2(BCW.Model.Game.Stklist model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// ������Ѻע���
        /// </summary>
        public void UpdatePool(int ID, long Pool)
        {
            dal.UpdatePool(ID, Pool);
        }

        /// <summary>
        /// ������Ѻע���2
        /// </summary>
        public void UpdateWinPool(int ID, long WinPool)
        {
            dal.UpdateWinPool(ID, WinPool);
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
        public DateTime GetEndTime(int ID)
        {
            return dal.GetEndTime(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Stklist GetStklist()
        {

            return dal.GetStklist();
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.Stklist GetStklist(int ID)
		{
			
			return dal.GetStklist(ID);
		}

        ///<summary>
        ///�������ڵõ�����
        /// </summary>
        public int GetIDbyDate(string stk)
        {
            return dal.GetIDbyDate(stk);
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
		/// <returns>IList Stklist</returns>
		public IList<BCW.Model.Game.Stklist> GetStklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetStklists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

