using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Net;
using System.Net.Mail;


using Boodoll.PageBL;
using Newtonsoft.Json;

namespace InstanceLastday16To9
{
         

    class Program
    {

        public static  List<ob_v_visitreport> allProductName = getAllProductIDName();
        public static int intent = 50;

        static void Main(string[] args)
        {
            //List<string> sendto1 = System.Configuration.ConfigurationSettings.AppSettings["sendto1"].ToString().Split(';').ToList();
            //SendMail(sendto1, "前一天16:00 到今天9:00数据报表click.muyingzhijia.com",DateTime.Now.ToString());


            //return;
           List<UserVisitInfo> userList = new List<UserVisitInfo>();
           string writeFile = string.Format("{0}\\result{1}.txt", System.Threading.Thread.GetDomain().BaseDirectory, DateTime.Now.ToFileTimeUtc().ToString());
            //数据读取完毕退出
             Console.WriteLine("开始分析数据:");

             for (int mi = -1; mi < 1; mi++)
             {
                 Int64 maxVisitID = 0;
                 string dtStr = DateTime.Now.AddDays(mi).ToShortDateString();

                 int exitFlag = 0;
                 for (int run = 0; run < int.MaxValue; run++)
                 {
                     #region 循环读取数据

                     if (exitFlag>5)
                     {
                         exitFlag = 0;
                         break;
                     }
                     int filter_limit = intent * (run + 1);
                     int filter_offset = intent * run;

                     string url = string.Format("http://10.0.0.131:920/index.php?module=API&filter_limit={0}&filter_offset={1}&method=Live.getLastVisitsDetails&format=json&idSite=1&period=day&date={2}&expanded=1&token_auth=453170c79e8f0ad5dcd1f0b2ce1ecf23", filter_limit,filter_offset,dtStr);

                     if (maxVisitID > 0)
                         url = url + "&maxIdVisit=" + maxVisitID;

                     string xml = Boodoll.PageBL.ProductSearch.ProductSearchBLL.GetHtml(url, Encoding.GetEncoding("GB2312"));

                     Newtonsoft.Json.JavaScriptArray jsonObject = (Newtonsoft.Json.JavaScriptArray)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(xml);

                     int count = jsonObject.Count();
                     for (int i = 0; i < count; i++)
                     {
                         try
                         {
                             JavaScriptObject qcount = (JavaScriptObject)jsonObject[i];
                             JavaScriptArray actionDetails = (JavaScriptArray)qcount["actionDetails"];

                             if (i == 0)
                                 maxVisitID = Convert.ToInt64(qcount["idVisit"]);
                             else
                                 maxVisitID = Convert.ToInt64(qcount["idVisit"]) > maxVisitID ? maxVisitID : Convert.ToInt64(qcount["idVisit"]);

                             string lastActionDateTime = qcount["serverDate"].ToString() + " " + qcount["serverTimePretty"].ToString();
                             if (mi == 0)
                             {
                                 if (Convert.ToDateTime(lastActionDateTime) > DateTime.Parse(DateTime.Now.ToShortDateString()).AddHours(9))
                                 {
                                   //  exitFlag = true;
                                     continue;

                                 }
                                 else if(Convert.ToDateTime(lastActionDateTime) < DateTime.Parse(DateTime.Now.ToShortDateString()))
                                 {
                                     exitFlag ++;
                                     continue;
                                 }
                                       
                             }

                             if (mi == -1)
                             {
                                 if (Convert.ToDateTime(lastActionDateTime) < DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString()).AddHours(16))
                                 {
                                     exitFlag++;
                                     continue;

                                 }

                             }
                             string referrerUrl = "";

                             string referrerType = "";
                             if (qcount["referrerUrl"] != null)
                                 referrerUrl = qcount["referrerUrl"].ToString();

                             if (qcount["referrerType"] != null)
                                 referrerType = qcount["referrerType"].ToString();

                             string message = "";
                             if (actionDetails.Count > 0)
                             {


                                 string userid = "";
                                 string guid = "";
                                 JavaScriptObject customVariables = null;
                                 try
                                 {
                                     customVariables = (JavaScriptObject)qcount["customVariables"];
                                     userid = ((new Dictionary<string, object>(((Newtonsoft.Json.JavaScriptObject)((new Dictionary<string, object>(customVariables)).ElementAt(0).Value)))).ElementAt(1).Value.ToString());
                                     guid = (new Dictionary<string, object>(((Newtonsoft.Json.JavaScriptObject)((new Dictionary<string, object>(customVariables)).ElementAt(1).Value)))).ElementAt(1).Value.ToString();
                                     message = getUserInfo(guid, userid);
                                 }
                                 catch (Exception ex)
                                 {
                                     continue;
                                 }

                                 if (!string.IsNullOrEmpty(message))
                                 {

                                     actionDetails.ForEach(item =>
                                     {
                                         JavaScriptObject itemobject = (JavaScriptObject)item;
                                         if (itemobject.Keys.Contains("url") && itemobject["url"] != null)
                                         {
                                             string tmpurl = itemobject["url"].ToString().ToLower();
                                             if (tmpurl.Contains("pdtid=") && allProductName.Any(c => tmpurl.Contains("pdtid=" + c.productid)))
                                             {
                                                 UserVisitInfo vinfo = new UserVisitInfo();
                                                 vinfo.Url = CreatePID(itemobject["url"].ToString());
                                           
                                                 if (!string.IsNullOrEmpty(vinfo.Url))
                                                 {
                                                     vinfo.Catename = vinfo.Url.Split(',')[1];
                                                     vinfo.ReferrerType = referrerType;
                                                     vinfo.Mobile = message.Split(',')[1];
                                                     vinfo.Userid = message.Split(',')[0];
                                                     vinfo.Guid = guid;
                                                     vinfo.LastVisitTime = lastActionDateTime;
                                                     vinfo.ReferrerUrl = referrerUrl;

                                                     if (itemobject.Keys.Contains("timeSpent"))
                                                         vinfo.Spent = Converter.ParseString(itemobject["timeSpent"], "");

                                                     userList.Add(vinfo);
                                                     Console.WriteLine(vinfo.Userid + "," + vinfo.Guid + "," + vinfo.Url + "," + vinfo.ReferrerUrl + "," + vinfo.LastVisitTime);
                                                 }
                                             }
                                         }

                                     });

                                 }



                             }


                         }
                         catch (Exception ex)
                         {
                             continue;
                             Console.WriteLine(ex.Message);
                         }

                     }
                 }

                     #endregion
             }
         
            userList = userList.Distinct().OrderByDescending(c=>c.Catename).ToList();
            CreateReport(userList);
            writeLog( writeFile,userList);
            Console.WriteLine("数据生成完毕");
         //   Console.ReadLine();
        }

   

        public static string CreatePID(string url)
        {
            
            url = url.ToLower();

          
           
                string Pattern = "pdtid=[0-9]*"; // @"pdtID";
                MatchCollection Matches = Regex.Matches(url, Pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);

                int i = 0;
                string[] sUrlList = new string[Matches.Count];

                // 取得匹配项列表
                foreach (Match match in Matches)
                {
                     
                      string tmpstr = "";
                     int productid=int.Parse(match.Value.Replace("pdtid=", ""));

                    if(allProductName.Any(a=>a.productid==productid))
                    {
                        allProductName.FirstOrDefault(a => a.productid == productid);
                        sUrlList[i++] = string.Format("{0},{1}", allProductName.FirstOrDefault(a => a.productid == productid).productName, allProductName.FirstOrDefault(a => a.productid == productid).category);
                    }

                }

                return string.Join(",", sUrlList);          
        }


        public static string getUserInfo(string guid, string usrid)
        {
            string result = "";
            int uid = -1;
            Converter.ParseInt(usrid, -1);

             
            if (uid < 0)
            {
                offlineBbhomeDataContext octx = new offlineBbhomeDataContext();
                if (octx.Ga_guidUserIDs.Any(c => c.guid == guid))
                {
                    uid = octx.Ga_guidUserIDs.FirstOrDefault(c => c.guid == guid).uid;
                }

              
            }

              if (uid > 0)
                {
                    base_t_member member = new BbhomeDataContext().base_t_members.FirstOrDefault(b => b.membNo == uid);

                    result = string.Format("{0},{1}",member.userCode,member.mobileTel);
                }

            return result;
        }


        public static void CreateReport(List<UserVisitInfo> au)
        {
      
            List<UserVisitInfo> u1 = au.Where(c => c.Catename == "合生元").ToList();
            List<UserVisitInfo> u2 = au.Where(c=>!u1.Any(u=>u.Userid==c.Userid)).ToList();
       
        
            List<string> sendto1 = System.Configuration.ConfigurationSettings.AppSettings["sendto1"].ToString().Split(';').ToList();

            List<string> sendto2 = System.Configuration.ConfigurationSettings.AppSettings["sendto2"].ToString().Split(';').ToList();

             SendMail(sendto1, "前一天16:00 到今天9:00数据报表click.muyingzhijia.com", createBody(u1));
             SendMail(sendto2, "前一天16:00 到今天9:00数据报表click.muyingzhijia.com", createBody(u2));
            //EmailServiceClient esc = new EmailServiceClient();
            //esc.Open();
            //esc.SendCmail(new WCFService.WcfMail() { Body = body, Subject = "前一天16:00 到今天9:00数据报表click.muyingzhijia.com", MailTo = ("yxw1309@muyingzhijia.com;wm1240@muyingzhijia.com; ws632@muyingzhijia.com; sd211@muyingzhijia.com;porsia@muyingzhijia.com;yxd1279@muyingzhijia.com;wh971@muyingzhijia.com;lyq942@muyingzhijia.com; cfzmp@163.com".Split(new char[] { ',', ';' })), IsHtml = true });
            //esc.Close();


        }

        public static string createBody(List<UserVisitInfo> u)
        {
            string bd = "<html><body><H3>前一天16:00 到今天9:00数据报表click.muyingzhijia.com</H3>";
            List<string> cateNames = u.Select(c => c.Catename).Distinct().ToList();
            for (int i = 0; i < cateNames.Count; i++)
            {
                string head = " <H3>" + cateNames[i] + "</H3><table border = 1>   <tr>     <th> 会员号 </th> <th>手机号  </th><th> 浏览商品 </th>  <th> 浏览时间</th></tr>";
                foreach (UserVisitInfo a in u.Where(c => c.Catename == cateNames[i]))
                {
                    head += ("<tr><td>" + a.Userid + "</td><td>" + a.Mobile + "</td><td>" + a.Url.Replace("'", "") + "</td><td>" + a.LastVisitTime + "</td><td></tr>");//开始写入值

                }
                head += "</table>";
                bd += head;
            }
            bd += "</body></html>";

            return bd;
        }

        /// <summary>
        /// 发送邮件，邮件内容大小无限制，比较灵活，但有可能被163、sin等邮箱屏蔽掉
        /// </summary>
        /// <param name="lstMail">邮件体，可多个</param>
        /// <param name="sendAddress">发送人</param>
        /// <param name="sendPwd">邮件标题</param>
        /// <param name="name">名称</param>
        /// <param name="msg">异常信息</param>
        /// <returns></returns>
        public static bool SendMail(List<string> lstMail, string subject, string body)
        {

            // <add key="sendMail" value="mybaby@muyingzhijia.com,mybaby,mybb@)!),222.66.166.253" />
            string sendAddress = "mybaby@muyingzhijia.com";
            string sendPwd = "mybb@)!)";
            string name = "母婴之家";
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.muyingzhijia.com";
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(sendAddress, sendPwd);


                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(sendAddress, name);

                if (lstMail != null && lstMail.Count > 0)
                {
                    foreach (var item in lstMail)
                    {
                        if (!string.IsNullOrEmpty(item))
                            mail.To.Add(new MailAddress(item));
                    }
                }

                mail.Subject = subject;
                mail.Body = body;
                mail.SubjectEncoding = Encoding.UTF8;
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = true;
                smtp.Send(mail);

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        public static void writeLog(string writeFile,List<UserVisitInfo> u)
        {
            try
            {
                FileStream fs = new FileStream(writeFile, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter wr = new StreamWriter(fs);

                foreach (UserVisitInfo a in u)
                {
                    wr.WriteLine(a.Userid + "," + a.Mobile + "," + a.Url + "," + a.LastVisitTime + ","+a.Catename);//开始写入值
                    //   wr.WriteLine(a.Userid + "," + a.Guid + "," + a.Url + "," + a.ReferrerType + "," + a.ReferrerUrl + "," + a.LastVisitTime);//开始写入值
                }
                wr.Flush();

                wr.Close();
                fs.Close();
            }
            catch
            { }
        }

        public static List<ob_v_visitreport> getAllProductIDName()
        {
            return new offlineBbhomeDataContext().ob_v_visitreports.Distinct().ToList();
             //var q=new offlineBbhomeDataContext().ob_v_visitreports.Distinct().ToList();
         
             //  Dictionary<int, string> dics = new Dictionary<int, string>();

             //  q.ForEach(c =>
             //      {
             //          if(!dics.Keys.Contains(c.productid))
             //          dics.Add( c.productid,c.productName);
             //      }
             //      );
             //  return dics;
             
        }

        public static string getIDName(int type)
        {
    
            switch (type)
            {
                ///童床 10 and cateId2=64
                case 1:
                    return "童床";
                    break;
                // 童车
                case 2:
                    return "童车"; break;

                // 汽车座椅
                case 3:
                    return "汽车座椅"; break;
                //床品
                case 4:
                    return "床品"; break;

                //值300元以上的玩具
                case 5:
                    return "值300元以上的玩具"; break;
                //吸奶器  cateId1=6 and cateId2=40 and (productName like '%吸奶器%' or productName like '%吸乳器%'
                case 6:
                    return "吸奶器"; break;

                //消毒锅 cateId1=6 and cateId2=41 and (productName like '%消毒锅%' or productName like '%消毒器%')
                case 7:
                    return "消毒锅"; break;

                //LG地垫cateId1=11 and cateId2=70 and brandid=443
                case 8:
                    return "LG地垫"; break;

                //法贝儿 brandid=598
                case 9:
                    return "法贝儿"; break;

                //施巴 brandid=514 
                case 10:
                    return "施巴"; break;

                //和光堂 brandid=319 
                case 11:
                    return "和光堂"; break;

                default:

                    break;


            }


            return "";
        }


        public List<string> GetAllOutboundProduct()
        {
            return new offlineBbhomeDataContext().ob_v_visitreports.Select(c => c.productid.ToString()).ToList();
        }



        public static List<string> GetVisitProductByType(int type)
        {
            HolycaDataContext ctx=new HolycaDataContext();


            List<string> tmpProducts = new List<string>();
            
            switch(type)
            {
                ///童床 10 and cateId2=64
                case 1:
                 tmpProducts=ctx.Vi_Web_Pdt_Lists.Where(c => c.intFirstCategory == 10 && c.intSecondCategory == 64).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;
                    // 童车
                case 2:
                 tmpProducts = ctx.Vi_Web_Pdt_Lists.Where(c => c.intFirstCategory == 10 && c.intSecondCategory == 62).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;

                // 汽车座椅
                case 3:
                 tmpProducts = ctx.Vi_Web_Pdt_Lists.Where(c => c.intFirstCategory == 10 && c.intSecondCategory == 63).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;

                //床品
                case 4:
                 tmpProducts = ctx.Vi_Web_Pdt_Lists.Where(c => c.intFirstCategory == 2 && c.intSecondCategory == 25).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;
                
                //值300元以上的玩具
                case 5:
                 tmpProducts = ctx.Vi_Web_Pdt_Lists.Where(c => c.intFirstCategory == 11 && c.intScore > 300).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;
                //吸奶器  cateId1=6 and cateId2=40 and (productName like '%吸奶器%' or productName like '%吸乳器%'
                case 6:
                 tmpProducts = ctx.Vi_Web_Pdt_Lists.Where(c => c.intFirstCategory == 6 && c.intSecondCategory == 40 && (c.vchProductName.Contains("吸奶器") || c.vchProductName.Contains("吸乳器"))).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;

                //消毒锅 cateId1=6 and cateId2=41 and (productName like '%消毒锅%' or productName like '%消毒器%')
                case 7:
                 tmpProducts = ctx.Vi_Web_Pdt_Lists.Where(c => c.intFirstCategory == 6 && c.intSecondCategory == 41 && (c.vchProductName.Contains("消毒锅") || c.vchProductName.Contains("消毒器"))).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;

                //LG地垫cateId1=11 and cateId2=70 and brandid=443
                case 8:
                 tmpProducts = ctx.Vi_Web_Pdt_Lists.Where(c => c.intFirstCategory == 11 && c.intSecondCategory == 70 && c.intBrandID == 443).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;

                //法贝儿 brandid=598
                case 9:
                 tmpProducts = ctx.Vi_Web_Pdt_Lists.Where(c => c.intBrandID == 598).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;

                //施巴 brandid=514 
                case 10:
                 tmpProducts = ctx.Vi_Web_Pdt_Lists.Where(c => c.intBrandID == 514).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;

                //和光堂 brandid=319 
                case 11:
                 tmpProducts = ctx.Vi_Web_Pdt_Lists.Where(c => c.intBrandID == 319).Select(c => string.Format("pdtid={0}", c.intProductID)).Distinct().ToList();
                 break;

                default:

                 break;
           

             }


            return tmpProducts;
        }

        public static string GetProductCode(int productid)
        {
            
            string productCode = "";
            HolycaDataContext ctx = new HolycaDataContext();
            if (ctx.Pdt_Base_Infos.Any(p => p.intProductID == productid))
            {
                productCode = ctx.Pdt_Base_Infos.FirstOrDefault(p => p.intProductID == productid).vchproductcode;

            }

            return productCode;
        }

    }
}
