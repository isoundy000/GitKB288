using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Race ��ժҪ˵����
	/// </summary>
	public class Race
	{
		private readonly BCW.DAL.Game.Race dal=new BCW.DAL.Game.Race();
		public Race()
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
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int userid)
        {
            return dal.Exists(ID, userid);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int Types, int paytype)
        {
            return dal.Exists(ID, Types, paytype);
        }
                
        /// <summary>
        /// ����ĳ�û����췢����������
        /// </summary>
        public int GetTodayCount(int userid)
        {
            return dal.GetTodayCount(userid);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.Race model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Race model)
		{
			dal.Update(model);
		}
    
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Updatetotime(int ID, DateTime totime)
        {
            dal.Updatetotime(ID, totime);
        }
     
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Updatepaytype(int ID, int paytype)
        {
            dal.Updatepaytype(ID, paytype);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdatetopPrice(int ID, long topPrice, int winID, string winName, int paytype)
        {
            dal.UpdatetopPrice(ID, topPrice, winID, winName, paytype);
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
		public BCW.Model.Game.Race GetRace(int ID)
		{
			
			return dal.GetRace(ID);
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
        /// <returns>IList Race</returns>
        public IList<BCW.Model.Game.Race> GetRaces(int SizeNum, string strWhere)
        {
            return dal.GetRaces(SizeNum, strWhere);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Race</returns>
		public IList<BCW.Model.Game.Race> GetRaces(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetRaces(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

