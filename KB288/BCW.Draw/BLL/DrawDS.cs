using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Draw.Model;
namespace BCW.Draw.BLL
{
	/// <summary>
	/// ҵ���߼���DrawDS ��ժҪ˵����
	/// </summary>
	public class DrawDS
	{
		private readonly BCW.Draw.DAL.DrawDS dal=new BCW.Draw.DAL.DrawDS();
		public DrawDS()
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
		public int  Add(BCW.Draw.Model.DrawDS model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Draw.Model.DrawDS model)
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
        /// ����GoodsCountsȡ�ģӣɣ�
        /// </summary>
        public int GetDSID(int GoodsCounts)
        {
            return dal.GetDSID(GoodsCounts);
        }

        /// <summary>
        /// ����GoodsCountsȡ�ģӣɣ�
        /// </summary>
        public string GetDS(int GoodsCounts)
        {
            return dal.GetDS(GoodsCounts);
        }

        /// <summary>
        /// ����GoodsCountsȡ��Ϸ����
        /// </summary>
        public string GetGN(int GoodsCounts)
        {
            return dal.GetGN(GoodsCounts);
        }
		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Draw.Model.DrawDS GetDrawDS(int ID)
		{
			
			return dal.GetDrawDS(ID);
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
		/// <returns>IList DrawDS</returns>
		public IList<BCW.Draw.Model.DrawDS> GetDrawDSs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDrawDSs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

