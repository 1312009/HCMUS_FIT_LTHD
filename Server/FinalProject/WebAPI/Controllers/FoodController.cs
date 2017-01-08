using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using WebAPI.Data;
using System.Web.Http;
using WebAPI.ViewModel;
using System.Net.Mail;
using System.Configuration;
using System.Data.Entity;
using Twilio;
using HtmlAgilityPack;


namespace WebAPI.Controllers
{
    [RoutePrefix("api/foods")]
    public class FOODController : ApiController
    {
        public FOODEntities db = new FOODEntities();
        public void SendSMS(string Phone, string message)
        {
            try
            {
                string AccountSid = ConfigurationManager.AppSettings.Get("Account_id"); ;
                string AuthToken = ConfigurationManager.AppSettings.Get("Auth_token"); ;
                var twilio = new TwilioRestClient(AccountSid, AuthToken);
                var sms = twilio.SendSmsMessage("(201) 546-9880", "+" + Phone, message, "");
            }
            catch (Exception ex)
            {
            }
        }
        [Route("SendMessage")]
        [HttpPost]
        public bool SendMessage(MessageViewModel message)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }
            try
            {
                MailMessage mailMessag = new MailMessage(ConfigurationManager.AppSettings.Get("Email"), message.Email);
                mailMessag.Subject = "Gửi thông tin thành công. Sau đây là nội dung tin nhắn của bạn: ";
                mailMessag.Body = message.Message;
                SmtpClient client = new SmtpClient();
                client.Send(mailMessag);
                SendSMS("+84981103589", "Có người tên "+message.Username+" góp ý với nội dung như sau: " + message.Message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        [Route("Ruttrichthongtin")]
        [HttpGet]
        public List<FOOD> RutTrichThongTin()
        {
            List<FOOD> Data = new List<FOOD>();
            List<String> Href = new List<string>();

            var webSite = new HtmlWeb();
            var doc = webSite.Load("http://www.knorr.com.vn/recipes/canh/297426");
            var metaTags = doc.DocumentNode.SelectNodes("//div[@class='c601 recipe-search-results']//div[@class='viewRecipeList']//a[@ct-type='recipeInfo']");
            int dem = 0;
            if (metaTags != null)
            {
                foreach (var tag in metaTags)
                {
                    if (dem % 2 == 0)
                        if (tag.Attributes["href"] != null)
                            Href.Add(@"http://www.knorr.com.vn" + tag.Attributes["href"].Value);
                    dem++;
                }
            }

            foreach (string href in Href)
            {
                FOOD MA = new FOOD();

                HtmlWeb Web = new HtmlWeb();
                HtmlDocument HtmlDoc = Web.Load(href);

                // Hình ảnh
                var HinhAnh = HtmlDoc.DocumentNode.SelectNodes("//div[@class='recipe-content-header']//div[@class='image']//img[@itemprop='image']");
                if (HinhAnh != null)
                {
                    foreach (var tag in HinhAnh)
                    {
                        if (tag.Attributes["src"] != null)
                            MA.IMGFOOD = tag.Attributes["src"].Value;
                    }
                }

                // Món ăn
                IEnumerable<HtmlNode> nodes = HtmlDoc.DocumentNode.SelectNodes("//div[@class='recipe-content-header']//h1[@itemprop='name']");
                foreach (HtmlNode node in nodes)
                {
                    MA.NAME = node.InnerText;
                }

                // Nguyên liệu
                nodes = HtmlDoc.DocumentNode.SelectNodes("//ul[@class='recipe-ingredients-list']//li[@itemprop='ingredients']");
                string nl = "";
                foreach (HtmlNode node in nodes)
                {
                    nl += node.InnerText + "\n";
                }
                MA.DECRIPTION = nl;
                MA.ISSALE = 0;
                MA.NUMBER = 0;
                MA.PRICE = 3000;
                Data.Add(MA);
            }
            return Data;
        }
        /// <summary>
        /// Lấy danh sách thức ăn
        /// </summary>
        /// <returns></returns>
        [Route("GetAllFoods")]
        [HttpGet]
        public List<FOOD> GetFOODs()
        {
            int t=0;
            var foods = from a in db.FOODs
                        where a.ISSALE!=0
                        select a;
            return foods.ToList();
        }
        [Route("GetFood")]
        [HttpGet]
        public FOOD GetFood(string id)
        {
            int convert = -1;
            if(!string.IsNullOrEmpty(id))
            {
                convert = int.Parse(id);
            }
            return db.FOODs.Find(convert);
        }
        [Route("TopLike")]
        [HttpGet]
        public IEnumerable<usp_TopMonAnThich_Result> GetTopLike()
        {
            return db.usp_TopMonAnThich();
        }
      

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FOODExists(int id)
        {
            return db.FOODs.Count(e => e.ID == id) > 0;
        }
    }
}

