using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���flowuser ��ժҪ˵����
	/// </summary>
	public class flowuser
	{
		private readonly BCW.DAL.Game.flowuser dal=new BCW.DAL.Game.flowuser();
		public flowuser()
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
        /// �õ�����
        /// </summary>
        public int GetTop(int UsID, string Field)
        {
            return dal.GetTop(UsID, Field);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.flowuser model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.flowuser model)
		{
			dal.Update(model);
		}
              
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateScore(int UsID, int Types, int Score)
        {
            dal.UpdateScore(UsID, Types, Score);
        }
               
        /// <summary>
        /// ���»�������
        /// </summary>
        public void UpdateiFlows(int UsID, int iFlows)
        {
            dal.UpdateiFlows(UsID, iFlows);
        }
              
        /// <summary>
        /// ���½��챻�����
        /// </summary>
        public void UpdateiBw(int UsID, int iBw)
        {
            dal.UpdateiBw(UsID, iBw);
        }


		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
                      
        /// <summary>
        /// �õ���������
        /// </summary>
        public int GetiFlows(int UsID)
        {

            return dal.GetiFlows(UsID);
        } 

        /// <summary>
        /// �õ����ܻ���
        /// </summary>
        public int GetScore(int UsID)
        {

            return dal.GetScore(UsID);
        }

        /// <summary>
        /// �õ���ɻ���
        /// </summary>
        public int GetScore2(int UsID)
        {

            return dal.GetScore2(UsID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
        public BCW.Model.Game.flowuser Getflowuser(int UsID)
		{

            return dal.Getflowuser(UsID);
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
		/// <returns>IList flowuser</returns>
		public IList<BCW.Model.Game.flowuser> GetflowusersTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetflowusersTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

