using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Draw.Model;
namespace BCW.Draw.BLL
{
	/// <summary>
	/// ҵ���߼���DrawUser ��ժҪ˵����
	/// </summary>
	public class DrawUser
	{
		private readonly BCW.Draw.DAL.DrawUser dal=new BCW.Draw.DAL.DrawUser();
		public DrawUser()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}
        //-----------------------------------------------------//
        /// <summary>
        /// �õ���
        /// </summary>
        public BCW.Draw.Model.DrawUser GetOnTime(int GoodsCounts)
        {
            return dal.GetOnTime(GoodsCounts);
        }
        public BCW.Draw.Model.DrawUser GetInTime(int GoodsCounts)
        {
            return dal.GetInTime(GoodsCounts);
        }
        /// <summary>
        /// ���ݱ�ŵõ���Ʒ����
        /// </summary>
        public int GetMyGoodsType(int GoodsCounts)
        {
            return dal.GetMyGoodsType(GoodsCounts);
        }
        public int GetMyStatue(int GoodsCounts)
        {
            return dal.GetMyStatue(GoodsCounts);
        }
        /// <summary>
        /// ���ݱ�ŵõ���Ʒ����
        /// </summary>
        public DateTime Getontime(int GoodsCounts)
        {
            return dal.Getontime(GoodsCounts);
        }
        /// <summary>
        /// ���ݱ�ŵõ���Ʒ��ֵ
        /// </summary>
        public int GetMyGoodsValue(int GoodsCounts)
        {
            return dal.GetMyGoodsValue(GoodsCounts);
        }
        /// <summary>
        /// �õ�
        /// </summary>
        public string  GetMyGoods(int GoodsCounts)
        {
            return dal.GetMyGoods(GoodsCounts);
        }


        /// <summary>
        /// �õ���
        /// </summary>
        public BCW.Draw.Model.DrawUser GetOnTimebynum(int Num)
        {
            return dal.GetOnTimebynum(Num);
        }
        public BCW.Draw.Model.DrawUser GetInTimebynum(int Num)
        {
            return dal.GetInTimebynum(Num);
        }
        /// <summary>
        /// ���ݱ�ŵõ���Ʒ����
        /// </summary>
        public int GetMyGoodsTypebynum(int Num)
        {
            return dal.GetMyGoodsTypebynum(Num);
        }
        public int GetMyStatuebynum(int Num)
        {
            return dal.GetMyStatuebynum(Num);
        }
        /// <summary>
        /// ���ݱ�ŵõ���Ʒ����
        /// </summary>
        public DateTime Getontimebynum(int Num)
        {
            return dal.Getontimebynum(Num);
        }
        /// <summary>
        /// ���ݱ�ŵõ���Ʒ��ֵ
        /// </summary>
        public int GetMyGoodsValuebynum(int Num)
        {
            return dal.GetMyGoodsValuebynum(Num);
        }
        /// <summary>
        /// ���ݱ�ŵõ���Ʒ��ֵ
        /// </summary>
        public int GetMyGoodsNumbynum(int Num)
        {
            return dal.GetMyGoodsNumbynum(Num);
        }
        /// <summary>
        /// �õ�
        /// </summary>
        public string GetMyGoodsbynum(int Num)
        {
            return dal.GetMyGoodsbynum(Num);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateMessage(int GoodsCounts, string Address, string Phone, string Email,string RealName , int MyGoodsStatue)
        {
            dal.UpdateMessage(GoodsCounts, Address, Phone, Email,RealName , MyGoodsStatue);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateExpress(int GoodsCounts, string Express, string Numbers, int MyGoodsStatue)
        {
            dal.UpdateExpress(GoodsCounts, Express, Numbers, MyGoodsStatue);
        }

        public void UpdateIntime(int GoodsCounts, DateTime InTime)
        {
            dal.UpdateIntime(GoodsCounts ,InTime);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateMyGoodsStatue(int GoodsCounts, int MyGoodsStatue)
        {
            dal.UpdateMyGoodsStatue(GoodsCounts, MyGoodsStatue);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateMessagebynum(int Num, string Address, string Phone, string Email, string RealName, int MyGoodsStatue)
        {
            dal.UpdateMessagebynum(Num, Address, Phone, Email, RealName, MyGoodsStatue);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateExpressbynum(int Num, string Express, string Numbers, int MyGoodsStatue)
        {
            dal.UpdateExpressbynum(Num, Express, Numbers, MyGoodsStatue);
        }

        public void UpdateIntimebynum(int Num, DateTime InTime)
        {
            dal.UpdateIntimebynum(Num, InTime);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateMyGoodsStatuebynum(int Num, int MyGoodsStatue)
        {
            dal.UpdateMyGoodsStatuebynum(Num, MyGoodsStatue);
        }

        //-----------------------------------------------------//
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
        public bool Existsnum(int Num)
        {
            return dal.Existsnum(Num);
        }
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(BCW.Draw.Model.DrawUser model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Draw.Model.DrawUser model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// me_��ʼ��ĳ���ݱ�
        /// </summary>
        /// <param name="TableName">���ݱ�����</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
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
		public BCW.Draw.Model.DrawUser GetDrawUser(int ID)
		{
			
			return dal.GetDrawUser(ID);
		}

     

        ///--------------------------���ݱ��ȡ����
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Draw.Model.DrawUser GetDrawUserbyCounts(int GoodsCounts)
        {

            return dal.GetDrawUserbyCounts(GoodsCounts);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Draw.Model.DrawUser GetDrawUserbynum(int Num)
        {

            return dal.GetDrawUserbynum(Num);
        }
        /// <summary>
        /// me_�����ֵ���ִ���������
        /// </summary>
        public IList<BCW.Draw.Model.DrawUser> Get_UsID(string _where)
        {
            return dal.Get_UsID(_where);
        }


        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.Draw.Model.DrawUser> GetUserTop(int p_pageIndex, int p_pageSize,string _where, string strWhere, out int p_recordCount)
        {
            return dal.GetUserTop(p_pageIndex, p_pageSize, _where,strWhere, out p_recordCount);
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
		/// <returns>IList DrawUser</returns>
        public IList<BCW.Draw.Model.DrawUser> GetDrawUsers(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetDrawUsers(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
		}

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList DrawUser</returns>
        public IList<BCW.Draw.Model.DrawUser> GetDrawUsers1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder,int iSCounts, out int p_recordCount)
        {
            return dal.GetDrawUsers1(p_pageIndex, p_pageSize, strWhere, strOrder,iSCounts, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

