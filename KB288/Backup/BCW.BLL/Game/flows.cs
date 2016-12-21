using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���flows ��ժҪ˵����
	/// </summary>
	public class flows
	{
		private readonly BCW.DAL.Game.flows dal=new BCW.DAL.Game.flows();
		public flows()
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
        /// �Ƿ���ڻ����¼
        /// </summary>
        public bool Exists(int UsID)
        {
            return dal.Exists(UsID);
        }

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
        public bool Exists(int ID, int UsID)
		{
            return dal.Exists(ID, UsID);
		}
        
        /// <summary>
        /// ����ĳ��Ա(��\�ǿ�\����)��������
        /// </summary>
        public int GetCount(int UsID, int State)
        {
            return dal.GetCount(UsID, State);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.flows model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.flows model)
		{
			dal.Update(model);
		}
  
        /// <summary>
        /// ����AddTime����������ǰ�ջ�
        /// </summary>
        public void UpdateAddTime(int ID, DateTime AddTime)
        {
            dal.UpdateAddTime(ID, AddTime);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(int ID, int state)
        {
            dal.Update(ID, state);
        }
                     
        /// <summary>
        /// ����ˮ��
        /// </summary>
        public void Updatewater(int ID, int water)
        {
            dal.Updatewater(ID, water);
        }

        /// <summary>
        /// �����Ӳ�
        /// </summary>
        public void Updateweeds(int ID, int weeds)
        {
            dal.Updateweeds(ID, weeds);
        }
        /// <summary>
        /// �����Ӳ�
        /// </summary>
        public void Updateweeds2(int UsID, int weeds)
        {
            dal.Updateweeds2(UsID, weeds);
        }  
        /// <summary>
        /// ���º���
        /// </summary>
        public void Updateworm(int ID, int worm)
        {
            dal.Updateworm(ID, worm);
        }

        /// <summary>
        /// ���º���
        /// </summary>
        public void Updateworm2(int UsID, int worm)
        {
            dal.Updateworm2(UsID, worm);
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
		public BCW.Model.Game.flows Getflows(int ID)
		{
			
			return dal.Getflows(ID);
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
		/// <returns>IList flows</returns>
		public IList<BCW.Model.Game.flows> Getflowss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getflowss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
               
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList flows</returns>
        public IList<BCW.Model.Game.flows> Getflowss2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount, int uid)
        {
            return dal.Getflowss2(p_pageIndex, p_pageSize, strWhere, out p_recordCount, uid);

        }

        /// <summary>
        /// ȡ����(��)һ����¼
        /// </summary>
        public BCW.Model.Game.flows GetPreviousNextflows(int ID, int UsID, bool p_next,bool p_ismy)
        {
            return dal.GetPreviousNextflows(ID, UsID, p_next, p_ismy);
        }

        /// <summary>
        /// ȡ����(��)һ����¼
        /// </summary>
        public BCW.Model.Game.flows GetPreviousNextflows(int ID, int UsID, bool p_next)
        {
            return dal.GetPreviousNextflows(ID, UsID, p_next, true);
        }

		#endregion  ��Ա����
	}
}

