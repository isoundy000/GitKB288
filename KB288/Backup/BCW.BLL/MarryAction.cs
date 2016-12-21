using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
using System.Text.RegularExpressions;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���MarryAction ��ժҪ˵����
    /// ���������Ծ�齱--Ҧ־��20160528
	/// </summary>
	public class MarryAction
	{
		private readonly BCW.DAL.MarryAction dal=new BCW.DAL.MarryAction();
		public MarryAction()
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
		/// ����һ������
        /// ������̬�����Ծ�齱20160528
		/// </summary>
		public int  Add(BCW.Model.MarryAction model)
		{
			int id= dal.Add(model);
            string Notes = model.Content;
            int UsId = 0;
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
            string ActionText = (ub.GetSub("ActionText", xmlPath));//Action���
            string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action��俪��
            //��Ծ�齱����
            if (WinnersStatus != "1" && WinnersOpenOrClose == "1")
            {
                try
                {
                    if (UsId == 0)//��ԱIDΪ�շ���3
                    {
                        //url=/bbs/uinfo.aspx?uid=" + meid +             
                        Match m;
                        Match m1;
                        string reg = "uid=[0-9]\\d*";
                        string reg1 = "[0-9]\\d*";
                        m = Regex.Match(Notes, reg);
                        m1 = Regex.Match(m.Groups[0].ToString(), reg1);
                        UsId = Convert.ToInt32(m1.Groups[0].ToString());
                        try
                        {
                            if (!new BCW.BLL.tb_WinnersLists().ExistsUserID(UsId))
                            {
                                return id;
                            }
                        }
                        catch { }
                    }
                    if (UsId == 0)//��ԱIDΪ�շ���
                    { return id; }
                    //�Ƿ��н�������1�н�
                    string UsName = new BCW.BLL.User().GetUsName(UsId);
                    int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, UsId, UsName, Notes, id);
                    if (isHit == 1)
                    {
                        if (WinnersGuessOpen == "1")
                        {
                            new BCW.BLL.Guest().Add(0, UsId, UsName, TextForUbb);//�����ߵ���ID
                            //if (ActionOpen == "1")
                            //{
                            //    Add(UsId, ActionText);
                            //}
                        }
                    }
                    return id;
                }
                catch
                {
                    return id;
                }
            }
            else
            {
                return id;
            }
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.MarryAction model)
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
		public BCW.Model.MarryAction GetMarryAction(int ID)
		{
			
			return dal.GetMarryAction(ID);
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
        /// <returns>IList MarryAction</returns>
        public IList<BCW.Model.MarryAction> GetMarryActions(int SizeNum, string strWhere)
        {
            return dal.GetMarryActions(SizeNum, strWhere);
        }

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList MarryAction</returns>
		public IList<BCW.Model.MarryAction> GetMarryActions(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetMarryActions(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

