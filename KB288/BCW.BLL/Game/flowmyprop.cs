using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���flowmyprop ��ժҪ˵����
	/// </summary>
	public class flowmyprop
	{
		private readonly BCW.DAL.Game.flowmyprop dal=new BCW.DAL.Game.flowmyprop();
		public flowmyprop()
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
        public bool Exists(int UsID, int did)
        {
            return dal.Exists(UsID, did);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.flowmyprop model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.flowmyprop model)
		{
			dal.Update(model);
		}        
        /// <summary>
        /// ʹ�õ���
        /// </summary>
        public void Update(int ID, int dnum, DateTime ExTime, int znum)
        {
            dal.Update(ID, dnum, ExTime, znum);
        
        }
             
        /// <summary>
        /// ʹ�õ���
        /// </summary>
        public void Update(int ID, int znum)
        {
            dal.Update(ID, znum);
        
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

              
        /// <summary>
        /// �õ�ĳ��������
        /// </summary>
        public int Getdnum(int id)
        {
            return dal.Getdnum(id);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.flowmyprop Getflowmyprop(int ID)
		{
			
			return dal.Getflowmyprop(ID);
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
		/// <returns>IList flowmyprop</returns>
		public IList<BCW.Model.Game.flowmyprop> Getflowmyprops(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getflowmyprops(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

