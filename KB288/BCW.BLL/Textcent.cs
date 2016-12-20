using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Textcent ��ժҪ˵����
	/// </summary>
    /// ���ӷ����齱���0528Ҧ־��
    /// ���ӵ�ֵ�齱��� 20160823 ���ڽ�
	public class Textcent
	{
		private readonly BCW.DAL.Textcent dal=new BCW.DAL.Textcent();
		public Textcent()
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
        /// ���������õ�����ʹ����̳������ͱҶ�
        /// </summary>
        public long GetForrmCents(int BID, int BzType, int ToID)
        {
            return dal.GetForrmCents(BID,BzType,ToID);
        }  
        /// <summary>
        /// ���������õ�������ͱҶ�
        /// </summary>
        public long GetCents(int BID, int BzType, int UsID)
        {
            return dal.GetCents(BID, BzType, UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Textcent model)
		{
			//return dal.Add(model);
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
                string username = model.UsName;
                string Notes = "��������";
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

            try
            {
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "���Ӵ���";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//��ֵ�齱
            }
            catch { }
                return ID;

		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Textcent model)
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
		public BCW.Model.Textcent GetTextcent(int ID)
		{
			
			return dal.GetTextcent(ID);
		}
                
        /// <summary>
        /// �õ����һ������ʵ�壬���������
        /// </summary>
        public BCW.Model.Textcent GetTextcentLast(int BID)
        {

            return dal.GetTextcentLast(BID);
        }
        /// <summary>
        /// �õ�һ�����Ͷ���ʵ�壬�ظ������
        /// </summary>
        public BCW.Model.Textcent GetTextcentReply(int ToID,int Floor, int BID)
        {

            return dal.GetTextcentReplyFloor(ToID,Floor,BID);
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
		/// <returns>IList Textcent</returns>
		public IList<BCW.Model.Textcent> GetTextcents(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetTextcents(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
               
        /// <summary>
        /// ȡ�����а�
        /// </summary>
        /// <param name="Types">�������</param>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Textcent> GetTextcentsTop(int Types, int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetTextcentsTop(Types, p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  ��Ա����
	}
}

