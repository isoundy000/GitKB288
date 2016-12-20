using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Applelist ��ժҪ˵����
	/// </summary>
	public class Applelist
	{
		private readonly BCW.DAL.Game.Applelist dal=new BCW.DAL.Game.Applelist();
		public Applelist()
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
		public bool Exists(int State,int ID)
		{
			return dal.Exists(State,ID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.Applelist model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Applelist model)
		{
			dal.Update(model);
		}
                       
        /// <summary>
        /// ���±��ڼ�¼
        /// </summary>
        public void Update2(BCW.Model.Game.Applelist model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// ��������ע��
        /// </summary>
        public void Update(int ID, long PayCent, int Types, int Num)
        {
            dal.Update(ID, PayCent, Types, Num);
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
		public BCW.Model.Game.Applelist GetApplelist(int ID)
		{
			
			return dal.GetApplelist(ID);
		}

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Applelist GetApplelistBQ(int State)
        {

            return dal.GetApplelistBQ(State);
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
		/// <returns>IList Applelist</returns>
		public IList<BCW.Model.Game.Applelist> GetApplelists(int p_pageIndex, int p_pageSize, string strWhere, int Num, out int p_recordCount)
		{
			return dal.GetApplelists(p_pageIndex, p_pageSize, strWhere, Num, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

