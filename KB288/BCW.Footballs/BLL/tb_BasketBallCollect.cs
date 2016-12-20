using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���tb_BasketBallCollect ��ժҪ˵����
	/// </summary>
	public class tb_BasketBallCollect
	{
		private readonly BCW.DAL.tb_BasketBallCollect dal=new BCW.DAL.tb_BasketBallCollect();
		public tb_BasketBallCollect()
		{}
		#region  ��Ա����
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
        public bool ExistsUsIdAndBaskId(int UsId, int BaskId)
        {
            return dal.ExistsUsIdAndBaskId(UsId, BaskId);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public int  Add(BCW.Model.tb_BasketBallCollect model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.tb_BasketBallCollect model)
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
        /// ɾ��һ������  �۹��� 20161004 ����ɾ���ղ�
        /// </summary>
        public void Delete(int ID, int usid)
        {

            dal.Delete(ID, usid);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_BasketBallCollect Gettb_BasketBallCollect(int ID)
		{
			
			return dal.Gettb_BasketBallCollect(ID);
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
		/// <returns>IList tb_BasketBallCollect</returns>
		public IList<BCW.Model.tb_BasketBallCollect> Gettb_BasketBallCollects(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_BasketBallCollects(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

