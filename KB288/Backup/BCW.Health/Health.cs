using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.Management;
using System.Data.SqlClient;
using BCW.Data;
namespace BCW.Health.Providers
{
    public class SqlWebEventProvider : WebEventProvider
    {
        public SqlWebEventProvider()
            : base()
        {
        }

        public override void Flush()
        {
        }

        public override void Shutdown()
        {
        }

        public override void ProcessEvent(WebBaseEvent p_eventRaised)
        {
            Add(p_eventRaised);
        }

        private void Add(WebBaseEvent p_eventRaised)
        {
            Exception errorException = null;
            WebRequestInformation requestInformation = null;

            if (p_eventRaised is WebRequestEvent)
                requestInformation = ((WebRequestEvent)p_eventRaised).RequestInformation;

            else if (p_eventRaised is WebRequestErrorEvent)
                requestInformation = ((WebRequestErrorEvent)p_eventRaised).RequestInformation;

            else if (p_eventRaised is WebErrorEvent)
                requestInformation = ((WebErrorEvent)p_eventRaised).RequestInformation;

            else if (p_eventRaised is WebAuditEvent)
                requestInformation = ((WebAuditEvent)p_eventRaised).RequestInformation;

            if (p_eventRaised is WebBaseErrorEvent)
                errorException = ((WebBaseErrorEvent)p_eventRaised).ErrorException;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Health(");
            strSql.Append("EventCode,Message,EventTime,RequestUrl,ExceptionType,ExceptionMessage)");
            strSql.Append(" values (");
            strSql.Append("@EventCode,@Message,@EventTime,@RequestUrl,@ExceptionType,@ExceptionMessage)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@EventCode", SqlDbType.Int,4),
					new SqlParameter("@Message", SqlDbType.NVarChar,50),
					new SqlParameter("@EventTime", SqlDbType.DateTime),
					new SqlParameter("@RequestUrl", SqlDbType.NVarChar,255),
					new SqlParameter("@ExceptionType", SqlDbType.NVarChar,255),
					new SqlParameter("@ExceptionMessage", SqlDbType.NVarChar,255)};
            parameters[0].Value = p_eventRaised.EventCode;
            parameters[1].Value = p_eventRaised.Message;
            parameters[2].Value = p_eventRaised.EventTime;
            parameters[3].Value = (requestInformation != null) ? requestInformation.RequestUrl : Convert.DBNull;
            parameters[4].Value = (errorException != null) ? errorException.GetType().ToString() : Convert.DBNull;
            parameters[5].Value = (errorException != null) ? errorException.Message : Convert.DBNull;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);

        }
    }
}