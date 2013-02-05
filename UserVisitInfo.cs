using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstanceLastday16To9
{
    public class UserVisitInfo
    {
        string userid;
        string url;
        int type;
        string lastVisitTime;
        string guid;
        string mobile;
        string referrerUrl;  //: "http://so.360.cn/s?ie=utf-8&src=hao_search&_re=0&q=%E6%AF%8D%E5%A9%B4%E4%B9%8B%E5%AE%B6"

        string referrerType;
        string spent;
        string catename;

        public string Catename
        {
            get { return catename; }
            set { catename = value; }
        }

        public string Spent
        {
            get { return spent; }
            set { spent = value; }
        }
        public string ReferrerType
        {
            get { return referrerType; }
            set { referrerType = value; }
        }


        public string ReferrerUrl
        {
            get { return referrerUrl; }
            set { referrerUrl = value; }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public string Userid
        {
            get { return userid; }
            set { userid = value; }
        }


        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }


        public string Url
        {
            get { return url; }
            set { url = value; }
        }



        public string LastVisitTime
        {
            get { return lastVisitTime; }
            set { lastVisitTime = value; }
        }



        public int Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}
