using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
using System.Text.RegularExpressions;
namespace BCW.BLL
{
    /// <summary>
    /// ҵ���߼���Action ��ժҪ˵����
    /// </summary>
    public class Action
    {
        private readonly BCW.DAL.Action dal = new BCW.DAL.Action();
        public Action()
        { }
        #region  ��Ա����

        #region ��ϷAction���෵��
        // BCW.User.AppCase
        public string CaseAction(int Types)
        {
            string text = string.Empty;
            switch (Types)
            {
                #region δ֪ID
                //δ֪ 
                case 4:
                case 40:
                    {
                        text = "δ֪ID:" + Types.ToString();
                    }; break;
                #endregion

                #region 2015������
                case 5:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/guessbc/default.aspx?backurl=" + Utils.PostPage(1)) + "\">����</a>";
                    }; break;
                case 6:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/luck28.aspx?backurl=" + Utils.PostPage(1)) + "\">����</a>";
                    }; break;
                case 9:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/dice.aspx?backurl=" + Utils.PostPage(1)) + "\">�ڱ�</a>";
                    }; break;
                case 10:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/horse.aspx?backurl=" + Utils.PostPage(1)) + "\">����</a>";
                    }; break;
                case 11:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/stkguess.aspx?backurl=" + Utils.PostPage(1)) + "\">��֤</a>";
                    }; break;
                case 12:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?backurl=" + Utils.PostPage(1)) + "\">�̳�</a>";
                    }; break;
                case 13:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/bigsmall.aspx?backurl=" + Utils.PostPage(1)) + "\">��С</a>";
                    }; break;
                case 14:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/brag.aspx?backurl=" + Utils.PostPage(1)) + "\">��ţ</a>";
                    }; break;
                case 15:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/marry.aspx?backurl=" + Utils.PostPage(1)) + "\">���</a>";
                    }; break;
                case 18:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/dxdice.aspx?backurl=" + Utils.PostPage(1)) + "\">����</a>";
                    }; break;
                case 19:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/flows.aspx?backurl=" + Utils.PostPage(1)) + "\">ʰ��</a>";
                    }; break;
                case 20:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/fruit28.aspx?backurl=" + Utils.PostPage(1)) + "\">ˮ��</a>";
                    }; break;
                case 22:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/ssc.aspx?backurl=" + Utils.PostPage(1)) + "\">ʱʱ��</a>";
                    }; break;
                case 23:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/guessbc/default.aspx?backurl=" + Utils.PostPage(1)) + "\">����</a>";
                    }; break;
                #endregion

                #region 2016����Ϸ����
                //2016 ����Ϸ
                case 1001:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/xk3.aspx?backurl=" + Utils.PostPage(1)) + "\">�¿���</a>";
                    }; break;
                case 1002:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/xk3sw.aspx?backurl=" + Utils.PostPage(1)) + "\">�¿��������</a>";
                    }; break;
                case 1003:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/hp3.aspx?backurl=" + Utils.PostPage(1)) + "\">�����˿���</a>";
                    }; break;
                case 1004:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/hp3Sw.aspx?backurl=" + Utils.PostPage(1)) + "\">�����˿��������</a>";
                    }; break;
                case 1005:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/cmg.aspx?backurl=" + Utils.PostPage(1)) + "\">�������</a>";
                    }; break;
                case 1006:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/klsf.aspx?backurl=" + Utils.PostPage(1)) + "\">����ʮ��</a>";
                    }; break;
                case 1007:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/kbyg.aspx?backurl=" + Utils.PostPage(1)) + "\">һԪ�ƹ�</a>";
                    }; break;
                case 1008:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/Dawnlife.aspx?backurl=" + Utils.PostPage(1)) + "\">��������</a>";
                    }; break;
                case 1009:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/baccarat.aspx?backurl=" + Utils.PostPage(1)) + "\">�ټ���</a>";
                    }; break;
                case 1010:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/chatroom.aspx?backurl=" + Utils.PostPage(1)) + "\">���</a>";
                    }; break;
                case 1011:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/farm.aspx?backurl=" + Utils.PostPage(1)) + "\">�ᱬũ��</a>";
                    }; break;
                case 1012:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/dzpk.aspx?backurl=" + Utils.PostPage(1)) + "\">�����˿�</a>";
                    }; break;
                case 1013:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/draw.aspx?backurl=" + Utils.PostPage(1)) + "\">�齱</a>";
                    }; break;
                case 1014:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/jqc.aspx?backurl=" + Utils.PostPage(1)) + "\">�����</a>";
                    }; break;
                case 1015:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/hc1.aspx?backurl=" + Utils.PostPage(1)) + "\">�ò�һ</a>";
                    }; break;
                case 1016:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/sfc.aspx?backurl=" + Utils.PostPage(1)) + "\">ʤ����</a>";
                    }; break;
                case 1017:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/bqc.aspx?backurl=" + Utils.PostPage(1)) + "\">6����</a>";
                    }; break;
                case 1018:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/winners.aspx?backurl=" + Utils.PostPage(1)) + "\">��Ծ�齱</a>";
                    }; break;
                case 1019:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/ssc.aspx?backurl=" + Utils.PostPage(1)) + "\">ʱʱ��</a>";
                    }; break;
                case 1020:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/luck28.aspx?backurl=" + Utils.PostPage(1)) + "\">���˶���</a>";
                    }; break;
                case 1030:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/PK10.aspx?backurl=" + Utils.PostPage(1)) + "\">��������</a>";
                    }; break;
                #endregion

                #region 2015����ID
                case -2:
                case -1:
                case 25:
                case 26:
                case 30:
                case 31:
                case 33:
                case 998:
                case 999:
                case 2501:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?backurl=" + Utils.PostPage(1)) + "\">����</a>";
                    }; break;
                    #endregion
            }
            return text;
        }
        #endregion

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
        /// </summary>
        public int Add(BCW.Model.Action model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// ����һ������
        /// ��Ծ�齱���1
        /// </summary>
        public int Add(int UsId, string Notes)
        {
            string UsName = new BCW.BLL.User().GetUsName(UsId);
            int id = dal.Add(0, 0, UsId, UsName, Notes);
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

                    string games = new BCW.winners.winners().getTypesForGameName(Notes);
                    if (games.Contains("���˼���") || games.Contains("��������") || games.Contains("���") || games.Contains("����") || games.Contains("��Ʊ") || games.Contains("����28") || games.Contains("���˶���") || games.Contains("�ڱ�") || games.Contains("��Сׯ") || games.Contains("����") || games.Contains("��ָ֤��") || games.Contains("����") || games.Contains("ʰ��") || games.Contains("ʱʱ��") || games.Contains("��ţ") || games.Contains("�������") || games.Contains("����ȫ��") || games.Contains("�¿�3") || games.Contains("�����˿�3") || games.Contains("�ƹ�") || games.Contains("�����˿�") || games.Contains("ũ��") || games.Contains("��Ծ�齱") || games.Contains("��ֵ�齱") || games.Contains("�ټһ���") || games.Contains("����") || games.Contains("���Ⱥ��") || games.Contains("��С����"))
                    { return id; }
                    else
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
                        //�Ƿ��н�������1�н�
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
        /// ��Ծ��̳�齱���2_20160518-Ҧ־��
        /// </summary>
        /// <param name="UsId"></param>
        /// <param name="UsName"></param>
        /// <param name="Notes"></param>
        /// <returns></returns>
        public int Add(int UsId, string UsName, string Notes)
        {
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ 
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2�������� 
            string ActionText = (ub.GetSub("ActionText", xmlPath));//Action���
            string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action��俪��
            int id = dal.Add(0, 0, UsId, UsName, Notes);
            //��Ծ�齱����
            if (WinnersStatus != "1" && WinnersOpenOrClose == "1")
            {
                try
                {
                    string games = new BCW.winners.winners().getTypesForGameName(Notes);
                    if (games.Contains("���˼���") || games.Contains("��������") || games.Contains("���") || games.Contains("����") || games.Contains("��Ʊ") || games.Contains("���˶���") || games.Contains("����28") || games.Contains("�ڱ�") || games.Contains("��Сׯ") || games.Contains("����") || games.Contains("��ָ֤��") || games.Contains("����") || games.Contains("ʰ��") || games.Contains("ʱʱ��") || games.Contains("��ţ") || games.Contains("�������") || games.Contains("����ȫ��") || games.Contains("�¿�3") || games.Contains("�����˿�3") || games.Contains("�ƹ�") || games.Contains("�����˿�") || games.Contains("ũ��") || games.Contains("��Ծ�齱") || games.Contains("��ֵ�齱") || games.Contains("�ټһ���") || games.Contains("����") || games.Contains("���Ⱥ��") || games.Contains("��С����"))
                    { return id; }
                    else
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
                        //�Ƿ��н�������1�н�
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
        //��Ϸ�Ȳ��ר��
        /// <summary>
        /// ��Ծ�齱��Ϸ�����3 20160518--Ҧ־��
        /// </summary>
        /// <param name="Types"></param>
        /// <param name="NodeId"></param>
        /// <param name="UsId"></param>
        /// <param name="UsName"></param>
        /// <param name="Notes"></param>
        /// <returns></returns>
        public int Add(int Types, int NodeId, int UsId, string UsName, string Notes)
        {
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//����������ʾ������
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//״̬1ά��2����0����
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|ֹͣ���ͻ���|1|�������ͻ���
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1ȫ����2����3����Ϸ
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1������2��������  
            string ActionText = (ub.GetSub("ActionText", xmlPath));//Action���
            string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action��俪��
            int id = dal.Add(Types, NodeId, UsId, UsName, Notes);
            //��Ծ�齱���� 1ά�� 
            if (WinnersStatus != "1" && WinnersOpenOrClose == "1")
            {
                try
                {
                    string games = new BCW.winners.winners().getTypesForGameName(Notes);
                    if (games.Contains("���˼���") || games.Contains("��������") || games.Contains("���") || games.Contains("����") || games.Contains("��Ʊ") || games.Contains("����28") || games.Contains("���˶���") || games.Contains("�ڱ�") || games.Contains("��Сׯ") || games.Contains("����") || games.Contains("��ָ֤��") || games.Contains("����") || games.Contains("ʰ��") || games.Contains("ʱʱ��") || games.Contains("��ţ") || games.Contains("�������") || games.Contains("����ȫ��") || games.Contains("�¿�3") || games.Contains("�����˿�3") || games.Contains("�ƹ�") || games.Contains("�����˿�") || games.Contains("ũ��") || games.Contains("��Ծ�齱") || games.Contains("��ֵ�齱") || games.Contains("�ټһ���") || games.Contains("����") || games.Contains("���Ⱥ��") || games.Contains("��С����"))
                    { return id; }
                    else
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
                        //�Ƿ��н�������1�н�
                        int isHit = new BCW.winners.winners().CheckActionForAll(Types, NodeId, UsId, UsName, Notes, id);
                        if (isHit == 1)
                        {
                            if (WinnersGuessOpen == "1")
                            {
                                new BCW.BLL.Guest().Add(0, UsId, UsName, TextForUbb);//�����ߵ���ID
                                //if (ActionOpen == "1")
                                //{
                                //     Add(UsId,ActionText);
                                //}
                            }
                        }
                        return id;
                    }
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

        ///
        ///ԭ����
        ///
        //public int Add(int UsId, string UsName, string Notes)
        //{
        //    return dal.Add(0, 0, UsId, UsName, Notes);
        //}
        ////��Ϸ�Ȳ��ר��
        //public int Add(int Types, int NodeId, int UsId, string UsName, string Notes)
        //{
        //    return dal.Add(Types, NodeId, UsId, UsName, Notes);
        //}
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Action model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }

        /// <summary>
        /// ���һ������
        /// </summary>
        public void Clear(int Types)
        {

            dal.Clear(Types);
        }

        /// <summary>
        /// ���һ������
        /// </summary>
        public void Clear(int Types, int NodeId)
        {

            dal.Clear(Types, NodeId);
        }

        /// <summary>
        /// ���ȫ������
        /// </summary>
        public void Clear()
        {

            dal.Clear();
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Action GetAction(int ID)
        {

            return dal.GetAction(ID);
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
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActions(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetActions(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActions(int SizeNum, string strWhere)
        {
            return dal.GetActions(SizeNum, strWhere);
        }

        /// <summary>
        /// ȡ�ú��Ѷ�̬��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActionsFriend(int Types, int uid, int SizeNum)
        {
            return dal.GetActionsFriend(Types, uid, SizeNum);
        }

        /// <summary>
        /// ȡ�ú��Ѷ�̬��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActionsFriend(int Types, int uid, int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetActionsFriend(Types, uid, p_pageIndex, p_pageSize, out p_recordCount);
        }

        #endregion  ��Ա����
    }
}
