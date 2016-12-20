using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Horselist ��ժҪ˵����
	/// </summary>
	public class Horselist
	{
		private readonly BCW.DAL.Game.Horselist dal=new BCW.DAL.Game.Horselist();
		public Horselist()
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
		public int  Add(BCW.Model.Game.Horselist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Horselist model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// ���±��ڼ�¼
        /// </summary>
        public void Update2(BCW.Model.Game.Horselist model)
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
        /// ���¿�������
        /// </summary>
        public void UpdateWinNum(int ID, int WinNum)
        {
            dal.UpdateWinNum(ID, WinNum);
        }
                
        /// <summary>
        /// ���¿�������
        /// </summary>
        public void UpdateWinData(int ID, string WinData)
        {

            dal.UpdateWinData(ID, WinData);
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
        public BCW.Model.Game.Horselist GetHorselistBf()
        {

            return dal.GetHorselistBf();
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Horselist GetHorselist()
        {

            return dal.GetHorselist();
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.Horselist GetHorselist(int ID)
		{
			
			return dal.GetHorselist(ID);
		}
                
        /// <summary>
        /// �õ�һ��WinNum
        /// </summary>
        public int GetWinNum(int ID)
        {

            return dal.GetWinNum(ID);
        }
                
        /// <summary>
        /// �õ�WinData
        /// </summary>
        public string GetWinData(int ID)
        {

            return dal.GetWinData(ID);
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
        /// <returns>IList Horselist</returns>
        public IList<BCW.Model.Game.Horselist> GetHorselists(int SizeNum, string strWhere)
        {
            return dal.GetHorselists(SizeNum, strWhere);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Horselist</returns>
		public IList<BCW.Model.Game.Horselist> GetHorselists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetHorselists(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

