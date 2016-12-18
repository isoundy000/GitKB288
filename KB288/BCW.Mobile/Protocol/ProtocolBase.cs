using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BCW.Mobile.Home;

namespace BCW.Mobile.Protocol
{
    public class ProtocolBase
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

     public abstract class ReqBase
    {
        public int userId;
        public string userKey;
    }

    public class RspBase : ProtocolBase
    {
        public Header header;

        public RspBase()
        {
            header = new Header();
        }
    }
}
