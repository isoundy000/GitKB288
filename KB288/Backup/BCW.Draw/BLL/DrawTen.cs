using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Draw.Model;
namespace BCW.Draw.BLL
{
	/// <summary>
	/// ҵ���߼���DrawTen ��ժҪ˵����
	/// </summary>
	public class DrawTen
	{
		private readonly BCW.Draw.DAL.DrawTen dal=new BCW.Draw.DAL.DrawTen();
		public DrawTen()
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
        /// �õ���
        /// </summary>
        public int GetCounts(int Rank)
        {
            return dal.GetCounts(Rank);
        }

        /// <summary>
        /// �õ���
        /// </summary>
        public int GetRank(int GoodsCounts)
        {
            return dal.GetRank(GoodsCounts);
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
		public void Add(BCW.Draw.Model.DrawTen model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Draw.Model.DrawTen model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// ���ݵȼ��������±��
        /// </summary>
        public void UpdateGoodsCounts(int rank,int GoodsCounts)
        {

            dal.UpdateGoodsCounts(rank,GoodsCounts);
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
		public BCW.Draw.Model.DrawTen GetDrawTen(int ID)
		{
			
			return dal.GetDrawTen(ID);
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
		/// <returns>IList DrawTen</returns>
		public IList<BCW.Draw.Model.DrawTen> GetDrawTens(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetDrawTens(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

