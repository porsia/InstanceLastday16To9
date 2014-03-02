using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Configuration;
using Boodoll.PageBL;
using Newtonsoft.Json;
using Boodoll.PageBL.ProductSearch;

namespace InstanceLastday16To9
{
         internal class Program
{
    // Fields
    public static List<ob_v_visitreport> allProductName = getAllProductIDName();
    public static string apiurl = ConfigurationSettings.AppSettings["apiurl"].ToString();
    public static int fromHour = int.Parse(ConfigurationSettings.AppSettings["fromHour"].ToString());
    public static int intent = int.Parse(ConfigurationSettings.AppSettings["intent"].ToString());
    public static int toHour = int.Parse(ConfigurationSettings.AppSettings["toHour"].ToString());
    public static string token = ConfigurationSettings.AppSettings["token"].ToString();

    // Methods
    public static string createBody(List<UserVisitInfo> u)
    {
        string bd = "<html><body><H3>前一天16:00 到今天9:00数据报表click.muyingzhijia.com</H3>";
        List<string> cateNames = (from c in u select c.Catename).Distinct<string>().ToList<string>();
        for (int i = 0; i < cateNames.Count; i++)
        {
            string head = " <H3>" + cateNames[i] + "</H3><table border = 1>   <tr>     <th> 会员号 </th> <th>手机号  </th><th> 浏览商品 </th>  <th> 浏览时间</th></tr>";
            foreach (UserVisitInfo a in from c in u
                where c.Catename == cateNames[i]
                select c)
            {
                string tmp = head;
                head = tmp+ "<tr><td>" + a.Userid + "</td><td>" + a.Mobile + "</td><td>" + a.Url.Replace("'", "") + "</td><td>" + a.LastVisitTime + "</td><td></tr>";
            }
            head = head + "</table>";
            bd = bd + head;
        }
        return (bd + "</body></html>");
    }

    public static string CreatePID(string url)
    {
        url = url.ToLower();
        if (url.Contains(".html"))
        {
            string Pattern = "[0-9]*.html";
            MatchCollection Matches = Regex.Matches(url, Pattern, RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase);
            int i = 0;
            string[] sUrlList = new string[Matches.Count];
            foreach (Match match in Matches)
            {
                int productid = int.Parse(match.Value.Replace(".html", ""));
                if (Enumerable.Any<ob_v_visitreport>(allProductName, (Func<ob_v_visitreport, bool>)(a => (a.productid == productid))))
                {
                    Enumerable.FirstOrDefault<ob_v_visitreport>(allProductName, (Func<ob_v_visitreport, bool>)(a => (a.productid == productid)));
                    sUrlList[i++] = string.Format("{0},{1}", Enumerable.FirstOrDefault<ob_v_visitreport>(allProductName, (Func<ob_v_visitreport, bool>)(a => (a.productid == productid))).productName, Enumerable.FirstOrDefault<ob_v_visitreport>(allProductName, (Func<ob_v_visitreport, bool>)(a => (a.productid == productid))).category);
                }
            }

            return string.Join(",", sUrlList);
        }
        return "";
    }

    public static void CreateReport(List<UserVisitInfo> au)
    {
        List<UserVisitInfo> u1 = (from c in au
            where c.Catename == "合生元"
            select c).ToList<UserVisitInfo>();
        List<UserVisitInfo> u2 = (from c in au
            where !Enumerable.Any<UserVisitInfo>(u1, (Func<UserVisitInfo, bool>) (u => (u.Userid == c.Userid)))
            select c).ToList<UserVisitInfo>();
        List<string> sendto1 = ConfigurationSettings.AppSettings["sendto1"].ToString().Split(new char[] { ';' }).ToList<string>();
        List<string> sendto2 = ConfigurationSettings.AppSettings["sendto2"].ToString().Split(new char[] { ';' }).ToList<string>();
        SendMail(sendto1, "前一天16:00 到今天9:00数据报表click.muyingzhijia.com", createBody(u1));
        SendMail(sendto2, "前一天16:00 到今天9:00数据报表click.muyingzhijia.com", createBody(u2));
    }

    public List<string> GetAllOutboundProduct()
    {
        return (from c in new offlineBbhomeDataContext().ob_v_visitreports select c.productid.ToString()).ToList<string>();
    }

    public static List<ob_v_visitreport> getAllProductIDName()
    {
        return new offlineBbhomeDataContext().ob_v_visitreports.Distinct<ob_v_visitreport>().ToList<ob_v_visitreport>();
    }

    public static string getIDName(int type)
    {
        switch (type)
        {
            case 1:
                return "童床";

            case 2:
                return "童车";

            case 3:
                return "汽车座椅";

            case 4:
                return "床品";

            case 5:
                return "值300元以上的玩具";

            case 6:
                return "吸奶器";

            case 7:
                return "消毒锅";

            case 8:
                return "LG地垫";

            case 9:
                return "法贝儿";

            case 10:
                return "施巴";

            case 11:
                return "和光堂";
        }
        return "";
    }

    public static string GetProductCode(int productid)
    {
        string productCode = "";
        HolycaDataContext ctx = new HolycaDataContext();
        if (Queryable.Any<Pdt_Base_Info>(ctx.Pdt_Base_Infos, p => p.intProductID == productid))
        {
            productCode = Queryable.FirstOrDefault<Pdt_Base_Info>(ctx.Pdt_Base_Infos, p => p.intProductID == productid).vchproductcode;
        }
        return productCode;
    }

    public static string getUserInfo(string guid, string usrid)
    {
        string result = "";
        int uid = -1;
        Converter.ParseInt(usrid, -1);
        if (uid < 0)
        {
            offlineBbhomeDataContext octx = new offlineBbhomeDataContext();
            if (Queryable.Any<Ga_guidUserID>(octx.Ga_guidUserIDs, c => c.guid == guid))
            {
                uid = Queryable.FirstOrDefault<Ga_guidUserID>(octx.Ga_guidUserIDs, c => c.guid == guid).uid;
            }
        }
        if (uid > 0)
        {
            base_t_member member = Queryable.FirstOrDefault<base_t_member>(new BbhomeDataContext().base_t_members, b => b.membNo == uid);
            result = string.Format("{0},{1}", member.userCode, member.mobileTel);
        }
        return result;
    }

    public static List<string> GetVisitProductByType(int type)
    {
        HolycaDataContext ctx = new HolycaDataContext();
        List<string> tmpProducts = new List<string>();
        switch (type)
        {
            case 1:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where (c.intFirstCategory == 10) && (c.intSecondCategory == 0x40)
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();

            case 2:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where (c.intFirstCategory == 10) && (c.intSecondCategory == 0x3e)
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();

            case 3:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where (c.intFirstCategory == 10) && (c.intSecondCategory == 0x3f)
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();

            case 4:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where (c.intFirstCategory == 2) && (c.intSecondCategory == 0x19)
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();

            case 5:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where (c.intFirstCategory == 11) && (c.intScore >= 300)
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();

            case 6:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where ((c.intFirstCategory == 6) && (c.intSecondCategory == 40)) && (c.vchProductName.Contains("吸奶器") || c.vchProductName.Contains("吸乳器"))
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();

            case 7:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where ((c.intFirstCategory == 6) && (c.intSecondCategory == 0x29)) && (c.vchProductName.Contains("消毒锅") || c.vchProductName.Contains("消毒器"))
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();

            case 8:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where ((c.intFirstCategory == 11) && (c.intSecondCategory == 70)) && (c.intBrandID == 0x1bb)
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();

            case 9:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where c.intBrandID == 0x256
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();

            case 10:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where c.intBrandID == 0x202
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();

            case 11:
                return (from c in ctx.Vi_Web_Pdt_Lists
                    where c.intBrandID == 0x13f
                    select string.Format("pdtid={0}", c.intProductID)).Distinct<string>().ToList<string>();
        }
        return tmpProducts;
    }

    private static void Main(string[] args)
    {
        List<UserVisitInfo> userList = new List<UserVisitInfo>();
        string writeFile = string.Format(@"{0}\result{1}.txt", Thread.GetDomain().BaseDirectory, DateTime.Now.ToFileTimeUtc().ToString());
        Console.WriteLine("开始分析数据:");
        for (int mi = -1; mi < 1; mi++)
        {
            long maxVisitID = 0;
            string dtStr = DateTime.Now.AddDays((double) mi).ToShortDateString();
            int exitFlag = 0;
            for (int run = 0; run < 0x7fffffff; run++)
            {
                if (exitFlag > 5)
                {
                    exitFlag = 0;
                    break;
                }
                string url = string.Format(apiurl, intent, dtStr, token);
                if (maxVisitID > 0)
                {
                    url = url + "&maxIdVisit=" + maxVisitID;
                }
                JavaScriptArray jsonObject = (JavaScriptArray) JavaScriptConvert.DeserializeObject(ProductSearchBLL.GetHtml(url, Encoding.GetEncoding("GB2312")));
                int count = jsonObject.Count<object>();
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        string referrerUrl;
                        string referrerType;
                        string message;
                        JavaScriptObject qcount = (JavaScriptObject) jsonObject[i];
                        JavaScriptArray actionDetails = (JavaScriptArray) qcount["actionDetails"];
                        if (i == 0)
                        {
                            maxVisitID = Convert.ToInt64(qcount["idVisit"]);
                        }
                        else
                        {
                            maxVisitID = (Convert.ToInt64(qcount["idVisit"]) > maxVisitID) ? maxVisitID : Convert.ToInt64(qcount["idVisit"]);
                        }
                        string lastActionDateTime = qcount["serverDate"].ToString() + " " + qcount["serverTimePretty"].ToString();
                        if (mi == 0)
                        {
                            if (Convert.ToDateTime(lastActionDateTime) > DateTime.Parse(DateTime.Now.ToShortDateString()).AddHours((double) toHour))
                            {
                                continue;
                            }
                            if (Convert.ToDateTime(lastActionDateTime) < DateTime.Parse(DateTime.Now.ToShortDateString()))
                            {
                                exitFlag++;
                                continue;
                            }
                        }
                        if ((mi == -1) && (Convert.ToDateTime(lastActionDateTime) < DateTime.Parse(DateTime.Now.AddDays(-1.0).ToShortDateString()).AddHours((double) fromHour)))
                        {
                            exitFlag++;
                        }
                        else
                        {
                            referrerUrl = "";
                            referrerType = "";
                            if (qcount["referrerUrl"] != null)
                            {
                                referrerUrl = qcount["referrerUrl"].ToString();
                            }
                            if (qcount["referrerType"] != null)
                            {
                                referrerType = qcount["referrerType"].ToString();
                            }
                            message = "";
                            if (actionDetails.Count > 0)
                            {
                                string userid = "";
                                string guid = "";
                                JavaScriptObject customVariables = null;
                                try
                                {
                                    customVariables = (JavaScriptObject) qcount["customVariables"];
                                    userid = new Dictionary<string, object>((JavaScriptObject) new Dictionary<string, object>(customVariables).ElementAt<KeyValuePair<string, object>>(0).Value).ElementAt<KeyValuePair<string, object>>(1).Value.ToString();
                                    guid = new Dictionary<string, object>((JavaScriptObject) new Dictionary<string, object>(customVariables).ElementAt<KeyValuePair<string, object>>(1).Value).ElementAt<KeyValuePair<string, object>>(1).Value.ToString();
                                    message = getUserInfo(guid, userid);
                                }
                                catch (Exception)
                                {
                                    continue;
                                }
                                if (!string.IsNullOrEmpty(message))
                                {
                                   
                                    actionDetails.ForEach(delegate (object item) {
                                        JavaScriptObject itemobject = (JavaScriptObject) item;
                                        if (itemobject.Keys.Contains<string>("url") && (itemobject["url"] != null))
                                        {
                                   
                                            string tmpurl = itemobject["url"].ToString().ToLower();
                                   
                                            if (tmpurl.Contains(".html") && allProductName.Any(c => tmpurl.Contains(string.Format("{0}.html", c.productid))))
                                            {
                                                UserVisitInfo vinfo = new UserVisitInfo {
                                                    Url = CreatePID(itemobject["url"].ToString())
                                                };
                                                if (!string.IsNullOrEmpty(vinfo.Url))
                                                {
                                                    vinfo.Catename = vinfo.Url.Split(new char[] { ',' })[1];
                                                    vinfo.ReferrerType = referrerType;
                                                    vinfo.Mobile = message.Split(new char[] { ',' })[1];
                                                    vinfo.Userid = message.Split(new char[] { ',' })[0];
                                                    vinfo.Guid = guid;
                                                    vinfo.LastVisitTime = lastActionDateTime;
                                                    vinfo.ReferrerUrl = referrerUrl;
                                                    if (itemobject.Keys.Contains<string>("timeSpent"))
                                                    {
                                                        vinfo.Spent = Converter.ParseString(itemobject["timeSpent"], "");
                                                    }
                                                    userList.Add(vinfo);
                                                }
                                            }
                                        }
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                Console.WriteLine(string.Concat(new object[] { dtStr, ",", mi, ",,", run, "finish.", maxVisitID, ",", DateTime.Now }));
            }
        }
        userList = (from c in userList.Distinct<UserVisitInfo>()
            orderby c.Catename descending
            select c).ToList<UserVisitInfo>();
        CreateReport(userList);
        writeLog(writeFile, userList);
        Console.WriteLine("数据生成完毕");
    }

    public static bool SendMail(List<string> lstMail, string subject, string body)
    {
        string sendAddress = "mybaby@muyingzhijia.com";
        string sendPwd = "mybb@)!)";
        string name = "母婴之家";
        try
        {
            SmtpClient smtp = new SmtpClient {
                Host = "mail.muyingzhijia.com",
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(sendAddress, sendPwd)
            };
            MailMessage mail = new MailMessage {
                From = new MailAddress(sendAddress, name)
            };
            if ((lstMail != null) && (lstMail.Count > 0))
            {
                foreach (string item in lstMail)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        mail.To.Add(new MailAddress(item));
                    }
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
        catch (Exception)
        {
            return false;
        }
    }

    public static void writeLog(string writeFile, List<UserVisitInfo> u)
    {
        try
        {
            FileStream fs = new FileStream(writeFile, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter wr = new StreamWriter(fs);
            foreach (UserVisitInfo a in u)
            {
                wr.WriteLine(a.Userid + "," + a.Mobile + "," + a.Url + "," + a.LastVisitTime + "," + a.Catename);
            }
            wr.Flush();
            wr.Close();
            fs.Close();
        }
        catch
        {
        }
    }
}



    
}
