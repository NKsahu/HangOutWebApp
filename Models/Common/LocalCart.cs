﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace HangOut.Models.Common
{
    public class LocalCart
    {
      public  int AppType { get; set; }
        public List<Cart> OrderList { get; set; }
    }
}