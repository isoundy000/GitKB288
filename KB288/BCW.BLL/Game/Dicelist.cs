using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Dicelist ��ժҪ˵����
	/// </summary>
	public class Dicelist
	{
		private readonly BCW.DAL.Game.Dicelist dal=new BCW.DAL.Game.Dicelist();
		public Dicelist()
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
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.Dicelist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Dicelist model)
		{
			dal.Update(model);
		}
                        
        /// <summary>
        /// ���±��ڼ�¼
        /// </summary>
        public void Update2(BCW.Model.Game.Dicelist model)
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
        /// �õ���һ�ڶ���ʵ��
        /// </summary>
        public BCW.Model.Game.Dicelist GetDicelistBf(int ID)
        {
            return dal.GetDicelistBf(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Dicelist GetDicelist()
        {
            return dal.GetDicelist();
        }
                
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public int GetWinNum(int ID)
        {
            return dal.GetWinNum(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.Dicelist GetDicelist(int ID)
		{
			
			return dal.GetDicelist(ID);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Dicelist</returns>
        public IList<BCW.Model.Game.Dicelist> GetDicelists(int SizeNum, string strWhere)
        {
            return dal.GetDicelists(SizeNum, strWhere);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Dicelist</returns>
		public IList<BCW.Model.Game.Dicelist> GetDicelists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDicelists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

