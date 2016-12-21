using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���tb_QuestionControl ��ժҪ˵����
	/// </summary>
	public class tb_QuestionControl
	{
		private readonly BCW.DAL.tb_QuestionControl dal=new BCW.DAL.tb_QuestionControl();
		public tb_QuestionControl()
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
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.tb_QuestionControl model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.tb_QuestionControl model)
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
        public int GetIDForUsid(int usid)
        {
          return  dal.GetIDForUsid(usid);
        }
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_QuestionControl Gettb_QuestionControl(int ID)
		{
			
			return dal.Gettb_QuestionControl(ID);
		}
        /// <summary>
        ///����answerList
        /// </summary>
        public void UpdateAnswerList(int ID, string answerList)
        {
             dal.UpdateAnswerList(ID, answerList);
        }
        
        /// <summary>
        ///����isDone
        /// </summary>
        public void UpdateIsFlase(int ID, int isFalse)
        {
            dal.UpdateIsFlase(ID, isFalse);
        }
        /// <summary>
        ///����isDone
        /// </summary>
        public void UpdateIsDone(int ID, int isDone)
        {
            dal.UpdateIsDone(ID, isDone);
        }
        /// <summary>
        ///����judge
        /// </summary>
        //public void UpdateJudge(int ID, string judge)
        //{
        //    dal.UpdateJudge(ID, judge);
        //}
        /// <summary>
        ///����isDone
        /// </summary>
        public void UpdateIsTrue(int ID, int isTrue)
        {
            dal.UpdateIsTrue(ID, isTrue);
        }

        /// <summary>
        ///����qNow
        /// </summary>
        public void UpdateqNow(int ID, int qNow)
        {
            dal.UpdateqNow(ID,qNow);
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
		/// <returns>IList tb_QuestionControl</returns>
		public IList<BCW.Model.tb_QuestionControl> Gettb_QuestionControls(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_QuestionControls(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

