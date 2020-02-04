using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangOut.Models.Feedbk
{
    public class FeedbkResponse
    {
        public int QID { get; set; }
        public int ResponseType { get; set; }
        public int FeedbkFormId { get; set; }
        public int StarCnt { get; set; }
        public string Subject { get; set; }
        public int LikeCnt { get; set; }
        public int DislikeCnt { get; set; }
        public int NormalOkCnt { get; set; }
        public int FeedbkId { get; set; }
        public string ObjectiveOptions {get;set;}
        public DateTime  CreateDate { get; set; }
        public int CID { get; set; }
    }
}