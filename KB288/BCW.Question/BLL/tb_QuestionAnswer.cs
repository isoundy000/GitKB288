using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// ҵ���߼���tb_QuestionAnswer ��ժҪ˵����
	/// </summary>
	public class tb_QuestionAnswer
	{
		private readonly BCW.DAL.tb_QuestionAnswer dal=new BCW.DAL.tb_QuestionAnswer();
		public tb_QuestionAnswer()
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
		public int  Add(BCW.Model.tb_QuestionAnswer model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.tb_QuestionAnswer model)
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
        /// �õ�ÿ�˻񽱴����ܴ���
        /// </summary>
        public int GetAllCounts(int usid)
        {
            return dal.GetAllCounts(usid);
        }
        /// <summary>
        /// �õ�ÿ�˻񽱴�����ܴ���
        /// </summary>
        public int GetTrueCounts(int usid)
        {
            return dal.GetTrueCounts(usid);
        }
        /// <summary>
        /// �õ�ÿ�˻񽱴�����ܴ���
        /// </summary>
        public int GetflaseCounts(int usid)
        {
            return dal.GetflaseCounts(usid);
        }
        ///// <summary>
        ///// �õ�ÿ�˻��ܽ���
        ///// </summary>
        //public int GetAllAwardGold(int usid)
        //{
        //    return dal.GetAllAwardGold(usid);
        //}
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_QuestionAnswer Gettb_QuestionAnswer(int ID)
		{
			
			return dal.Gettb_QuestionAnswer(ID);
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
		/// <returns>IList tb_QuestionAnswer</returns>
		public IList<BCW.Model.tb_QuestionAnswer> Gettb_QuestionAnswers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_QuestionAnswers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  ��Ա����
	}
}

