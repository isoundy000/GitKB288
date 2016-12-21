using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Draw.Model;
namespace BCW.Draw.BLL
{
	/// <summary>
	/// ҵ���߼���DrawBox ��ժҪ˵����
	/// </summary>
	public class DrawBox
	{
		private readonly BCW.Draw.DAL.DrawBox dal=new BCW.Draw.DAL.DrawBox();
		public DrawBox()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

//----------------------------------------------------------------------------------------------//
        /// <summary>
        /// �õ���
        /// </summary>
        public string GetGoodsName(int GoodsCounts)
        {
            return dal.GetGoodsName(GoodsCounts);
        }
        /// <summary>
        /// �õ���
        /// </summary>
        public string GetExplain(int GoodsCounts)
        {
            return dal.GetExplain(GoodsCounts);
        }    /// <summary>
        /// �õ���
        /// </summary>
        public string GetGoodsImg(int GoodsCounts)
        {
            return dal.GetGoodsImg(GoodsCounts);
        }    /// <summary>
        /// �õ���
        /// </summary>
        public int GetGoodsType(int GoodsCounts)
        {
            return dal.GetGoodsType(GoodsCounts);
        }    /// <summary>
        /// �õ���
        /// </summary>
        public int GetGoodsValue(int GoodsCounts)
        {
            return dal.GetGoodsValue(GoodsCounts);
        }  
        /// <summary>
        /// �õ���
        /// </summary>
        public int GetGoodsNum(int GoodsCounts)
        {
            return dal.GetGoodsNum(GoodsCounts);
        }
        /// <summary>
        /// �õ���
        /// </summary>
        public int GetRank(int GoodsCounts)
        {
            return dal.GetRank(GoodsCounts);
        }
        /// <summary>
        /// �õ���
        /// </summary>
        public int GetID(int GoodsCounts)
        {
            return dal.GetID(GoodsCounts);
        }
        /// <summary>
        /// �õ�һ����Ʒ����
        /// </summary>
        public int GetAllNumbyC(int GoodsCounts)
        {
            return dal.GetAllNumbyC(GoodsCounts);
        }
        /// <summary>
        /// �õ����ڽ����еĽ�Ʒ����
        /// </summary>
        public int GetAllNum(int lun)
        {
            return dal.GetAllNum(lun);
        }
        /// <summary>
        /// �õ����ڽ����еĽ�Ʒ����
        /// </summary>
        public int GetAllNumS(int lun)
        {
            return dal.GetAllNumS(lun );
        }
        /// <summary>
        /// �õ��ڼ��ֽ���
        /// </summary>
        public int Getlun()
        {
            return dal.Getlun();
        }
        /// <summary>
        /// �õ���
        /// </summary>
        public int GetStatue(int GoodsCounts)
        {
            return dal.GetStatue(GoodsCounts);
        }
 //----------------------------------------------------------------------------------------------//

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
        /// <summary>
        /// �Ƿ���ڸü�¼���ݱ��
        /// </summary>
        public bool CountsExists(int GoodsCounts)
        {
            return dal.CountsExists(GoodsCounts);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(BCW.Draw.Model.DrawBox model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Draw.Model.DrawBox model)
		{
			dal.Update(model);
		}

        //////---------------

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateStatue(int GoodsCounts,int Statue)
        {
            dal.UpdateStatue(GoodsCounts,Statue);
        }
        /// <summary>
        /// ����������Ʒ�¼�
        /// </summary>
        public void UpdateStatueAllgo()
        {
            dal.UpdateStatueAllgo();
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateGoodsNum(int GoodsCounts, int GoodsNum)
        {
            dal.UpdateGoodsNum(GoodsCounts, GoodsNum);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateGoodsNumgo(int lun)
        {
            dal.UpdateGoodsNumgo(lun);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateOverTime(int GoodsCounts, DateTime OverTime)
        {
            dal.UpdateOverTime(GoodsCounts, OverTime);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateReceiveTime(int GoodsCounts, DateTime ReceiveTime)
        {
            dal.UpdateReceiveTime(GoodsCounts, ReceiveTime);
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
		public BCW.Draw.Model.DrawBox GetDrawBox(int ID)
		{
			
			return dal.GetDrawBox(ID);
		}
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Draw.Model.DrawBox GetDrawBoxbyC(int GoodsCounts)
        {

            return dal.GetDrawBoxbyC(GoodsCounts);
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
		/// <returns>IList DrawBox</returns>
        public IList<BCW.Draw.Model.DrawBox> GetDrawBoxs(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetDrawBoxs(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

