using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���Upfile ��ժҪ˵����
	/// </summary>
	public class Upfile
	{
		private readonly BCW.DAL.Upfile dal=new BCW.DAL.Upfile();
		public Upfile()
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
        public bool Exists(int ID, int UsID, int Types)
        {
            return dal.Exists(ID, UsID, Types);
        }
                
        /// <summary>
        /// ����ĳ�û������ϴ��ļ�����
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            return dal.GetTodayCount(UsID);
        }
               
        /// <summary>
        /// ����ĳ�û��ļ�����
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// ����ĳ�û�ĳ�༯��Ƭ����
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            return dal.GetCount(UsID, NodeId);
        }
 
        /// <summary>
        /// ����ĳ�û�ĳ�༯��Ƭ����
        /// </summary>
        public int GetCount(int UsID, int Types, int NodeId)
        {
            return dal.GetCount(UsID, Types, NodeId);
        }

		/// <summary>
		/// ����һ������
        /// �ϴ���Ƭ��Ծ�齱���---Ҧ־��
		/// </summary>
		public int  Add(BCW.Model.Upfile model)
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
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "�ϴ���Ƭ";
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
		public void Update(BCW.Model.Upfile model)
		{
			dal.Update(model);
		}
                
        /// <summary>
        /// ����������Ϣ
        /// </summary>
        public void Update(int ID, string Content)
        {
            dal.Update(ID, Content);
        }
                     
        /// <summary>
        /// ����ļ�
        /// </summary>
        public void UpdateIsVerify(int ID, int IsVerify)
        {
            dal.UpdateIsVerify(ID, IsVerify);
        }

        /// <summary>
        /// �������ش���
        /// </summary>
        public void UpdateDownNum(int ID, int DownNum)
        {
            dal.UpdateDownNum(ID, DownNum);
        }

        /// <summary>
        /// ����ĳ�༯��Ƭ��ΪĬ�Ϸ���
        /// </summary>
        public void UpdateNodeIds(int UsID, int Types, int NodeId)
        {
            dal.UpdateNodeIds(UsID, Types, NodeId);
        }
               
        /// <summary>
        /// ת���ļ�
        /// </summary>
        public void UpdateNodeId(int UsID, int ID, int NodeId)
        {
            dal.UpdateNodeId(UsID, ID, NodeId);
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
        public void Delete(int UsID, int Types, int NodeId)
        {
            dal.Delete(UsID, Types, NodeId);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// �õ�һ��Title
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }
                
        /// <summary>
        /// �õ�һ��Files
        /// </summary>
        public string GetFiles(int ID)
        {
            return dal.GetFiles(ID);
        }

        /// <summary>
        /// �õ�һ���û�ID
        /// </summary>
        public int GetUsID(int ID)
        {
            return dal.GetUsID(ID);
        }
           
        /// <summary>
        /// �õ�һ��Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }

        /// <summary>
        /// �õ�һ��NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            return dal.GetNodeId(ID);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Upfile GetUpfileMe(int ID)
        {
            return dal.GetUpfileMe(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Upfile GetUpfile(int ID)
		{
			
			return dal.GetUpfile(ID);
		}
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Upfile GetUpfile(int ID, int Types)
        {

            return dal.GetUpfile(ID, Types);
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
        /// <returns>IList Upfile</returns>
        public IList<BCW.Model.Upfile> GetUpfiles(int SizeNum, string strWhere)
        {
            return dal.GetUpfiles(SizeNum, strWhere);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Upfile</returns>
		public IList<BCW.Model.Upfile> GetUpfiles(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetUpfiles(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

