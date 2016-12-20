using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Marry ��ժҪ˵����
	/// </summary>
	public class Marry
	{
		private readonly BCW.DAL.Marry dal=new BCW.DAL.Marry();
		public Marry()
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
        public bool Exists(int UsID, int ReID, int Types)
        {
            return dal.Exists(UsID, ReID, Types);
        }
        
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int UsID, int ReID)
        {
            return dal.Exists(UsID, ReID);
        }
                        
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int UsID, int ReID)
        {
            return dal.Exists2(UsID, ReID);
        }

        
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int UsID, int ReID, int Types)
        {
            return dal.Exists2(UsID, ReID, Types);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists3(int UsID, int ReID, int State)
        {
            return dal.Exists3(UsID, ReID, State);
        }
                
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists4(int UsID, int ReID, int State)
        {
            return dal.Exists4(UsID, ReID, State);
        }

        /// <summary>
        /// �Ƿ����������¼
        /// </summary>
        public bool ExistsLove(int UsID)
        {
            return dal.ExistsLove(UsID);
        }
                
        /// <summary>
        /// �Ƿ���ڽ���¼
        /// </summary>
        public bool ExistsMarry(int UsID)
        {
            return dal.ExistsMarry(UsID);
        }
            
        /// <summary>
        /// ĳ��Ա�Ƿ���ڷ����ļ�¼
        /// </summary>
        public bool ExistsLostMarry(int UsID)
        {
            return dal.ExistsLostMarry(UsID);
        }

        /// <summary>
        /// ����HomeClick
        /// </summary>
        public void UpdateHomeClick(int ID, int HomeClick)
        {
            dal.UpdateHomeClick(ID, HomeClick);
        }

        /// <summary>
        /// ����LoveStat
        /// </summary>
        public void UpdateLoveStat(int ID, string LoveStat)
        {
            dal.UpdateLoveStat(ID, LoveStat);
        }

                
        /// <summary>
        /// ���»�԰����
        /// </summary>
        public void UpdateHomeName(int ID, string HomeName)
        {
            dal.UpdateHomeName(ID, HomeName);
        }
                
        /// <summary>
        /// ����������
        /// </summary>
        public void UpdateOath(int ID, string Oath)
        {
            dal.UpdateOath(ID, Oath);
        }

        /// <summary>
        /// ����Ů����
        /// </summary>
        public void UpdateOath2(int ID, string Oath2)
        {
            dal.UpdateOath2(ID, Oath2);
        }
               
        /// <summary>
        /// ����FlowStat��FlowTimes���ʻ�����
        /// </summary>
        public void UpdateFlowStat(int ID, string FlowStat, string FlowTimes, int FlowNum)
        {
            dal.UpdateFlowStat(ID, FlowStat, FlowTimes, FlowNum);
        }

        /// <summary>
        /// ��Ϊ����
        /// </summary>
        public void UpdateMarry(int UsID, int ReID, string Oath)
        {
            dal.UpdateMarry(UsID, ReID, Oath);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "��Ϊ����";
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
            catch {}

        }
                
        /// <summary>
        /// ��Ϊ����
        /// </summary>
        public void UpdateMarry(int UsID, int ReID)
        {
            dal.UpdateMarry(UsID, ReID);
        }

        /// <summary>
        /// ȡ���������
        /// </summary>
        public void UpdateMarry2(int UsID, int ReID)
        {
            dal.UpdateMarry2(UsID, ReID);
        }

             
        /// <summary>
        /// ��Ϊ���
        /// </summary>
        public void UpdateLost(int UsID, int ReID, string Oath2)
        {
            dal.UpdateLost(UsID, ReID, Oath2);
        }
                
        /// <summary>
        /// ��Ϊ���
        /// </summary>
        public void UpdateLost(int UsID, int ReID)
        {
            dal.UpdateLost(UsID, ReID);
        }
                
        /// <summary>
        /// ȡ���������
        /// </summary>
        public void UpdateLost2(int UsID, int ReID)
        {
            dal.UpdateLost2(UsID, ReID);
        }

		/// <summary>
		/// ����һ������
        /// �����Ծ�齱���---20160528
		/// </summary>
		public int  Add(BCW.Model.Marry model)
		{
		//	return dal.Add(model);
            int ID = dal.Add(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "���";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//�����ߵ���ID
                    }
                }
                return ID;
            }
            catch { return ID; }
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Marry model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// ��Ϊ����
        /// </summary>
        public void UpdateLove(int UsID, int ReID)
        {
            dal.UpdateLove(UsID, ReID);
          
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                int usid = UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "��Ϊ����";
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
            catch { }
        }

        
        /// <summary>
        /// ���½��֤��ַ
        /// </summary>
        public void UpdateMarryPk(int ID, string MarryPk)
        {
            dal.UpdateMarryPk(ID, MarryPk);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {
            dal.Delete(strWhere);
        }

        /// <summary>
        /// �õ��ʻ�����
        /// </summary>
        public int GetFlowNumTop(int ID)
        {
            return dal.GetFlowNumTop(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Marry GetMarry(int ID)
		{
			
			return dal.GetMarry(ID);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
                
        /// <summary>
        /// ȡ�����а��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList Marry</returns>
        public IList<BCW.Model.Marry> GetMarrysTop(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetMarrysTop(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Marry</returns>
		public IList<BCW.Model.Marry> GetMarrys(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetMarrys(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

