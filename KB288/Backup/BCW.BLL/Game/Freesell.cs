using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���Freesell ��ժҪ˵����
	/// </summary>
	public class Freesell
	{
		private readonly BCW.DAL.Game.Freesell dal=new BCW.DAL.Game.Freesell();
		public Freesell()
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
		public int  Add(BCW.Model.Game.Freesell model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.Freesell model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
   
        /// <summary>
        /// ��������ɾ������
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.Freesell GetFreesell(int ID)
		{
			
			return dal.GetFreesell(ID);
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
		/// <returns>IList Freesell</returns>
		public IList<BCW.Model.Game.Freesell> GetFreesells(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetFreesells(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

