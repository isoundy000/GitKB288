using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// ҵ���߼���SSCpay ��ժҪ˵����
	/// </summary>
	public class SSCpay
	{
		private readonly BCW.DAL.Game.SSCpay dal=new BCW.DAL.Game.SSCpay();
		public SSCpay()
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
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(BCW.Model.Game.SSCpay model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add2(BCW.Model.Game.SSCpay model)
        {
            return dal.Add2(model);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Game.SSCpay model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
        }
                
        /// <summary>
        /// ����ʱʱ�ʿ������
        /// </summary>
        public void UpdateResult(int SSCId, string Result)
        {
            dal.UpdateResult(SSCId, Result);
        }
                
        /// <summary>
        /// ������Ϸ�����ñ�
        /// </summary>
        public void UpdateWinCent(int ID, int SSCId, int UsID, string UsName, long WinCent, string WinNotes)
        {
            if (ub.GetSub("SSCGuestSet", "/Controls/ssc.xml") == "0")
            {
                new BCW.BLL.Guest().Add(1, UsID, UsName, "����" + SSCId + "��ʱʱ��:" + WinNotes.Replace("|", "�н�") + "" + ub.Get("SiteBz") + "��[url=/bbs/game/ssc.aspx?act=case]�ҽ�[/url]");
            }

            dal.UpdateWinCent(ID, WinCent, WinNotes);
            //�������Զ��ҽ�
            if (new BCW.BLL.Game.SSCpay().GetIsSpier(ID) == 1)
            {
                new BCW.BLL.Game.SSCpay().UpdateState(ID, 2);

                new BCW.BLL.User().UpdateiGold(UsID, UsName, WinCent, 11);
            }
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
        /// ĳ��ĳID��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPrices(int UsID, int SSCId)
        {
            return dal.GetSumPrices(UsID, SSCId);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
        
        /// <summary>
        /// �õ�һ��IsSpier
        /// </summary>
        public int GetIsSpier(int ID)
        {
            return dal.GetIsSpier(ID);
        }

        /// <summary>
        /// ������������ұ���ֵ
        /// </summary>
        public long GetSumPrices(string strWhere)
        {
            return dal.GetSumPrices(strWhere);
        }

        /// <summary>
        /// �����������㷵��ֵ
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            return dal.GetSumWinCent(strWhere);
        }    
        
        /// <summary>
        /// �õ�һ��WinCentNotes
        /// </summary>
        public string GetWinNotes(int ID)
        {

            return dal.GetWinNotes(ID);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.SSCpay GetSSCpay(int ID)
		{
			
			return dal.GetSSCpay(ID);
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
		/// <returns>IList SSCpay</returns>
		public IList<BCW.Model.Game.SSCpay> GetSSCpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSSCpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}


        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.Model.Game.SSCpay> GetSSCpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSSCpaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
		#endregion  ��Ա����
	}
}

