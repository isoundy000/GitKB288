using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Mebook ��ժҪ˵����
	/// </summary>
	public class Mebook
	{
		private readonly BCW.DAL.Mebook dal=new BCW.DAL.Mebook();
		public Mebook()
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
        public bool Exists(int ID, int UsID)
        {
            return dal.Exists(ID, UsID);
        }

        /// <summary>
        /// me_�Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int ID, int UsID)
        {
            return dal.Exists2(ID, UsID);
        }

        /// <summary>
        /// ����ĳ��ԱID���Ա�������
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }
                
        /// <summary>
        /// ����ĳ��ԱID�����������
        /// </summary>
        public int GetIDCount(int MID)
        {
            return dal.GetIDCount(MID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Mebook model)
		{
			int ID= dal.Add(model);        
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
                string ActionText = (ub.GetSub("ActionText", xmlPath));//Action���
                string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action��俪��
                int usid = model.MID;
                string username = model.MName;
                string Notes = "�ռ�����";
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
		public void Update(BCW.Model.Mebook model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// ���»ظ�����
        /// </summary>
        public void UpdateReText(int ID, string ReText)
        {
            dal.UpdateReText(ID, ReText);
        }
               
        /// <summary>
        /// �����Ƿ��ö�
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {
            dal.UpdateIsTop(ID, IsTop);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
        /// <summary>
        /// �۹��� 20160526 ����ɾ��ũ������
        /// ɾ��ũ����������
        /// </summary>
        public void Delete_farm(int ID)
        {

            dal.Delete_farm(ID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int UsID, int MID)
        {

            dal.Delete(UsID, MID);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// �õ�һ��MID
        /// </summary>
        public int GetMID(int ID)
        {
            return dal.GetMID(ID);
        }

        /// <summary>
        /// �õ�һ��IsTop
        /// </summary>
        public int GetIsTop(int ID)
        {
            return dal.GetIsTop(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Mebook GetMebook(int ID)
		{
			
			return dal.GetMebook(ID);
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Mebook</returns>
        public IList<BCW.Model.Mebook> GetMebooks(int SizeNum, string strWhere)
        {
            return dal.GetMebooks(SizeNum, strWhere);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Mebook</returns>
		public IList<BCW.Model.Mebook> GetMebooks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetMebooks(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

