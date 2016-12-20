using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���flowmyzz ��ժҪ˵����
	/// </summary>
	public class flowmyzz
	{
		private readonly BCW.DAL.Game.flowmyzz dal=new BCW.DAL.Game.flowmyzz();
		public flowmyzz()
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
        public bool Exists(int UsID, int Types, int zid)
        {
            return dal.Exists(UsID, Types, zid);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.flowmyzz model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.flowmyzz model)
		{
			dal.Update(model);
		}
        
        /// <summary>
        /// �������ӻ�
        /// </summary>
        public void Updateznum(int UsID, int zid, int Types, int znum)
        {

            dal.Updateznum(UsID, zid, Types, znum);
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
        public BCW.Model.Game.flowmyzz Getflowmyzz(int UsID, int Types, int zid)
        {

            return dal.Getflowmyzz(UsID, Types, zid);
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
		/// <returns>IList flowmyzz</returns>
		public IList<BCW.Model.Game.flowmyzz> Getflowmyzzs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getflowmyzzs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

