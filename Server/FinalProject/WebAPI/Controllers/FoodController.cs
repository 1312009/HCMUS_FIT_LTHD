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
        /// <summary>
        /// Lấy danh sách thức ăn
        /// </summary>
        /// <returns></returns>
        [Route("GetAllFoods")]
        [HttpGet]
        public List<FOOD> GetFOODs()
        {
            var foods = from a in db.FOODs
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

