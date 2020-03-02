using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.DynamicList
{
    public class JoinCls
    {
        public string Id { get; set; }
        public string ProductName  {get;set;}
       // ublic string ProductType { get; set; }  //  0: none , 1 restuarn/ Cafe ,2: room service ,3: theater ,4 other
        public static  List<JoinCls> List = new List<JoinCls>();
        
        public  List<JoinCls> ListProductType()
        {
            JoinCls join0=new JoinCls { Id ="0", ProductName = "" };
            JoinCls join1 = new JoinCls { Id = "1", ProductName = "Restuarn/ Cafe" };
            JoinCls join2 = new JoinCls { Id = "2", ProductName = "Room Service" };
            JoinCls join3 = new JoinCls { Id = "3", ProductName = "Theater" };
            JoinCls join4 = new JoinCls { Id = "4", ProductName = "Other" };
            List<JoinCls> list = new List<JoinCls>();
            list.Add(join0);
            list.Add(join1);
            list.Add(join2);
            list.Add(join3);
            list.Add(join4);
            return list;
        }

    }
}