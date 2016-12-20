using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Transfer ��ժҪ˵����
	/// </summary>
	public class Transfer
	{
		private readonly BCW.DAL.Transfer dal=new BCW.DAL.Transfer();
		public Transfer()
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
        /// �Ƿ񶩵����Ƿ��ظ�
        /// </summary>
        public bool Exists(int FromId, string zfbNo)
        {
            return dal.Exists(FromId, zfbNo);
        }
                
        /// <summary>
        /// ����ĳ�û�������Ҷ�
        /// </summary>
        public long GetAcCents(int FromID, int Types)
        {   
            return dal.GetAcCents(FromID, Types);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Transfer model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
        /// ���������ӳ齱���---Ҧ־��
		/// </summary>
		public void Update(BCW.Model.Transfer model)
		{
			dal.Update(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = model.FromId;
                string username = model.FromName;
                string Notes = "����";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
            }
            catch {  }
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(string strWhere)
		{
			
			dal.Delete(strWhere);
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Transfer GetTransfer(int ID)
		{
			
			return dal.GetTransfer(ID);
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
		/// <returns>IList Transfer</returns>
		public IList<BCW.Model.Transfer> GetTransfers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetTransfers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// ��������������Ҵ���
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }
		#endregion  ��Ա����
	}
}

