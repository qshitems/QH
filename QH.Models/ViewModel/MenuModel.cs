using System;
using System.Collections.Generic;
using System.Text;

namespace QH.Models.ViewModel
{
   public class MenuModel
    {
        public long id { get; set; }

        public string title { get; set; }
        /// <summary>
        /// 1菜单 
        /// </summary>
        public int type { get; set; }
        public string icon { get; set; }

        public string href { get; set; }

        public long parentid { get; set; }
        /// <summary>
        /// _iframe /_blank
        /// </summary>
        public string openType { get; set; } 

        public List<MenuModel> children { get; set; } = new  List<MenuModel>();
    }

    public class childrenModel
    {
        public int id { get; set; }

        public string title { get; set; }
        /// <summary>
        /// 1菜单 
        /// </summary>
        public int type { get; set; }

        public string icon { get; set; }

        public string href { get; set; }

    
    }
}
