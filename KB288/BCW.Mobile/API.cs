using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Data;
using BCW.Mobile.Home;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
 

namespace BCW.Mobile
{
  


    public enum ERequestResult                
    {
        success=0,
        faild,
    }

    public enum EPageType
    {
        eHome,
        eBbs
    }



    /// <summary>
    /// 轮播内容. 
    /// </summary>
    public class sliderPicItem
    {
        public string url;
        public string content_type;
        public string[] arryParams; 
    }


    /// <summary>
    /// 轮播对象
    /// </summary>
    public class sliderPic
    {
        private int count;

        [JsonProperty]
        private List<sliderPicItem> items;

        public sliderPic()
        {
            items = new List<sliderPicItem>();
        }

        public void InitData(EPageType type)
        {
            DataSet _ds = new BCW.MobileSlider.BLL.MobileSlider().GetList( "ptype=" + ( int ) type );

            for( int i = 0; i < _ds.Tables[0].Rows.Count; i++ )
            {
                sliderPicItem _item = new sliderPicItem();
                _item.url ="http://"+Utils.GetDomain()+_ds.Tables[ 0 ].Rows[ i ][ "url" ].ToString();
                _item.content_type = _ds.Tables[ 0 ].Rows[ i ][ "contentType" ].ToString();
                _item.arryParams = _ds.Tables[ 0 ].Rows[ i ][ "param" ].ToString().Split( '|' );
                items.Add( _item );
            }
        }
    }

     


 


}
