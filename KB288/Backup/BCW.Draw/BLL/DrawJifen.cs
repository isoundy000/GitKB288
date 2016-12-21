using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Draw.Model;
namespace BCW.Draw.BLL
{
	/// <summary>
	/// ҵ���߼���DrawJifen ��ժҪ˵����
	/// </summary>
	public class DrawJifen
	{
		private readonly BCW.Draw.DAL.DrawJifen dal=new BCW.Draw.DAL.DrawJifen();
		public DrawJifen()
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
        /// �õ��û�����
        /// </summary>
        public int GetJifen(int UsID)
        {
            return dal.GetJifen(UsID);
        }

        /// <summary>
        /// �����û�����
        /// </summary>
        /// <param name="ID">�û�ID</param>
        /// <param name="iGold">������</param>
        public void UpdateJifen(int UsID, int Jifen)
        {
            dal.UpdateJifen(UsID, Jifen);
        }

        public void UpdateJifen(int UsID, int Jifen, string AcText)
        {
            string UsName = dal.GetUsName(UsID);
            UpdateJifen(UsID, UsName, Jifen, AcText);
        }

        public void UpdateJifen(int UsID, string UsName, int Jifen, string AcText)
        {
            if (new BCW.Draw.BLL.DrawJifen().ExistsUsID(UsID))
            {
                //�����û������
                dal.UpdateJifen(UsID, Jifen);
                //�������Ѽ�¼
                BCW.Draw.Model.DrawJifenlog model = new BCW.Draw.Model.DrawJifenlog();
                if (AcText.Contains("ǩ��"))
                {
                    model.Types = 1;//ǩ������Ϊ1
                }
                else
                {
                    model.Types = 0;
                }
                model.PUrl = Utils.getPageUrl();//�������ļ���
                model.UsId = UsID;
                model.UsName = UsName;
                model.AcGold = Jifen;
                model.AfterGold = GetJifen(UsID);//���º�ı���
                model.AcText = AcText;
                model.AddTime = DateTime.Now;
                new BCW.Draw.BLL.DrawJifenlog().Add(model);
            }
        }

        /// <summary>
        /// �����ۼ�ǩ������
        /// </summary>
        public void UpdateQd(int UsID,int Qd)
        {
            dal.UpdateQd(UsID,Qd);
        }
        /// <summary>
        /// ��������ǩ������
        /// </summary>
        public void UpdateQdweek(int UsID,int Qdweek)
        {
            dal.UpdateQdweek(UsID,Qdweek);
        }
        /// <summary>
        /// �����ϴ�ǩ��ʱ��
        /// </summary>
        public void UpdateQdTime(int UsID ,DateTime QdTime)
        {
            dal.UpdateQdTime(UsID, QdTime);
        }

        /// <summary>
        /// �Ƿ���ڸ�usid��¼
        /// </summary>
        public bool ExistsUserID(int ID)
        {
            return dal.ExistsUserID(ID);
        }
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
        /// <summary>
        /// �Ƿ���ڸ�UsID
        /// </summary>
        public bool ExistsUsID(int UsID)
        {
            return dal.ExistsUsID(UsID);
        }

        /// <summary>
        /// �õ�ÿ��ǩ�����
        /// </summary>
        public int getsQd(int UsID)
        {
            return dal.getsQd(UsID);
        }

        /// <summary>
        /// �õ�����ǩ�����
        /// </summary>
        public int getsQdweek(int UsID)
        {
            return dal.getsQdweek(UsID);
        }
        /// <summary>
        /// �õ�ǩ��ʱ��
        /// </summary>
        public DateTime getQdTime(int UsID)
        {
            return dal.getQdTime(UsID);
        }
		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Draw.Model.DrawJifen model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Draw.Model.DrawJifen model)
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
		public BCW.Draw.Model.DrawJifen GetDrawJifen(int ID)
		{
			
			return dal.GetDrawJifen(ID);
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
		/// <returns>IList DrawJifen</returns>
        public IList<BCW.Draw.Model.DrawJifen> GetDrawJifens(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			return dal.GetDrawJifens(p_pageIndex, p_pageSize, strWhere,strOrder, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

