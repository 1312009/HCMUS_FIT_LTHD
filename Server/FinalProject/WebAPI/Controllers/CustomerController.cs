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
    [Authorize(Roles = "CUSTOMER")]
    [RoutePrefix("api/Customer")]
    public class CustomerController : ApiController
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
        public bool Mail(OdersUser odersUser)
        {
            int iduser = int.Parse(odersUser.iduser);
            ACCOUNT account = db.ACCOUNTs.Find(iduser);
            if (account == null)
            {
                return false;
            }
            MailMessage mailMessagCus = new MailMessage(ConfigurationManager.AppSettings.Get("Email"), account.EMAIL);
            mailMessagCus.Subject = "Gửi đơn đặt hàng";
            mailMessagCus.Body = "Danh sách các món được đặt hàng bởi người dùng có id " + account.ID+" ,tên: "+account.NAME+" và email : "+account.EMAIL;
            double sum = 0;
            foreach (Oders i in odersUser.listoders)
            {
                FOOD food = new FOOD();
                int tempid = int.Parse(i.id);
                int tempnum = int.Parse(i.number);
                food = db.FOODs.Find(tempid);
                mailMessagCus.Body = mailMessagCus.Body + "Món : Tên món ăn: " + food.NAME + " ,số lượng: " + tempnum + " , giá" + food.PRICE * tempnum;
                sum = sum + food.PRICE.Value * tempnum;
            }
            mailMessagCus.Body = mailMessagCus.Body + " Tổng tiền: " + sum;
            mailMessagCus.Body = mailMessagCus.Body + "Địa chỉ nhận đơn hàng : "+odersUser.Address+","+odersUser.Ward+","+odersUser.District;
            SmtpClient client = new SmtpClient();
            client.Send(mailMessagCus);
            SendSMS("+84981103589", mailMessagCus.Body);

            return true;
        }
        [Route("Order")]
        [HttpPost]
        public bool Order([FromBody] OdersUser odersUser)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }
            foreach(Oders i in odersUser.listoders)
            {
                FOOD food = new FOOD();
                int tempid = int.Parse(i.id);
                int tempnum = int.Parse(i.number);
                food = db.FOODs.Find(tempid);
                int check = food.NUMBER.Value - tempnum;
                if(check<0)
                {
                    return false;
                }
                food.NUMBER = check;
                if(food.NUMBER==0)
                {
                    food.ISSALE = 0;
                }
                FOODORDER foododers = new FOODORDER();
                int tempusser = int.Parse(odersUser.iduser);
                foododers.IDUSER = tempusser;
                foododers.IDFOOD = tempid;
                foododers.NUMBER = tempnum;
                DbContextTransaction dt = db.Database.BeginTransaction();
                try
                {
                    db.Entry(food).State = EntityState.Modified;
                    db.SaveChanges();
                    db.FOODORDERs.Add(foododers);
                    db.SaveChanges();
                    dt.Commit();
                }
                catch(Exception ex)
                {
                    dt.Rollback();
                    return false;
                }
                Mail(odersUser);
            }
            return true;
        }
    }
}
