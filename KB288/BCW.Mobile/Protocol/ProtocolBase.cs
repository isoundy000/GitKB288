using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BCW.Mobile.Home;

namespace BCW.Mobile.Protocol
{
    public abstract class ProtocolBase
    {
        public string SerializeObject()
        {
           return JsonConvert.SerializeObject(this);
        }

        public string SerializeObject(bool ignoreNull)
        {
            JsonSerializerSettings _settings = new JsonSerializerSettings();
            _settings.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this,Formatting.Indented,_settings);
        }
    }

     public abstract class ReqProtocolBase
    {
        public int userId;
        public string userKey;
    }

    public abstract class RspProtocolBase : ProtocolBase
    {
        public Header header;

        public RspProtocolBase()
        {
            header = new Header();
        }
    }
}
