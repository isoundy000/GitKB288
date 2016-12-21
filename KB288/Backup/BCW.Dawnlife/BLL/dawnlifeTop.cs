using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���dawnlifeTop ��ժҪ˵����
	/// </summary>
	public class dawnlifeTop
	{
		private readonly BCW.DAL.dawnlifeTop dal=new BCW.DAL.dawnlifeTop();
		public dawnlifeTop()
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
        public bool Existscoin(long coin)
        {
            return dal.Existscoin(coin);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.dawnlifeTop model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.dawnlifeTop model)
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
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.dawnlifeTop GetdawnlifeTop(int ID)
		{
			
			return dal.GetdawnlifeTop(ID);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
        //����һ���ֶ�
        public void Updatesum(int ID, int sum)
        {
            dal.Updatesum(ID, sum);
        }

        //����һ���ֶ�
        public void Updatecum(int ID, int cum)
        {
            dal.Updatecum(ID, cum);
        }
        /// ���ݲ�ѯӰ�������
        /// </summary>
        public int GetRowByUsID(int UsID, long coin,long money)
        {
            return dal.GetRowByUsID(UsID, coin, money);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList dawnlifeTop</returns>
		public IList<BCW.Model.dawnlifeTop> GetdawnlifeTops(int p_pageIndex, int p_pageSize, string strWhere,string strOrder,out int p_recordCount)
		{
			return dal.GetdawnlifeTops(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
		}
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList dawnlifeTop</returns>
        public IList<BCW.Model.dawnlifeTop> GetdawnlifeTops1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, int iSCounts, out int p_recordCount)
        {
            return dal.GetdawnlifeTops1(p_pageIndex, p_pageSize, strWhere, strOrder, iSCounts, out p_recordCount);
        }


		#endregion  ��Ա����
	}
}

