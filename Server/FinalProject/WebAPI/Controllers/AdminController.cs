using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Data;
using Twilio;
using System.Configuration;
using System.Net.Mail;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "ADMIN")]
    [RoutePrefix("api/Admin")]
    public class AdminController : ApiController
    {
        private static readonly string[] VietnameseSigns = new string[]
{

"aAeEoOuUiIdDyY",

"áàạảãâấầậẩẫăắằặẳẵ",

"ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

"éèẹẻẽêếềệểễ",

"ÉÈẸẺẼÊẾỀỆỂỄ",

"óòọỏõôốồộổỗơớờợởỡ",

"ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

"úùụủũưứừựửữ",

"ÚÙỤỦŨƯỨỪỰỬỮ",

"íìịỉĩ",

"ÍÌỊỈĨ",

"đ",

"Đ",

"ýỳỵỷỹ",

"ÝỲỴỶỸ"

};

        public static string locDau(string str)
        {

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {

                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);

            }

            return str;

        }
        public FOODEntities db = new FOODEntities();
        [Route("FindFood")]
        [HttpGet]
        public IEnumerable<usp_TimKiemMonAn_Result> FindFood(string name, string row, string count)
        {
            if(!string.IsNullOrEmpty(name))
            {
                name = locDau(name);
            }
            int convertrow = -1, convertcount = -1;

            if (!string.IsNullOrEmpty(row))
            {
                convertrow = int.Parse(row);
            }
            if (!string.IsNullOrEmpty(count))
            {
                convertrow = int.Parse(count);
            }
            if (!string.IsNullOrEmpty(row) && !string.IsNullOrEmpty(count))
            {
                return db.usp_TimKiemMonAn(name, convertrow, convertcount);
            }
            return db.usp_TimKiemMonAn(name, null, null);
        }
        [Route("AddFood")]
        [HttpPost]
        public bool AddFood(FOOD food)
        {
            db.usp_ThemMonAn(food.NAME, food.DECRIPTION, food.ISSALE, food.IDTYPE, food.IMGFOOD, food.PRICE, food.NUMBER);
            return true;
        }
        [Route("DeleteFood")]
        [HttpDelete]
        public bool DeleteFood(int id)
        {
            db.usp_XoaMonAn(id);
            return true;
        }
        [Route("EditFood")]
        [HttpPut]
        public bool EditFood(FOOD food)
        {   
            db.Entry(food).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
        [Route("F")]
        [HttpGet]
        public void SendSMS(string Phone)
        {
            try
            {
                string AccountSid = ConfigurationManager.AppSettings.Get("Account_id"); ;
                string AuthToken = ConfigurationManager.AppSettings.Get("Auth_token"); ;
                var twilio = new TwilioRestClient(AccountSid, AuthToken);
                var sms = twilio.SendSmsMessage("(201) 546-9880", "+"+Phone, "hj SMS", "");

            }
            catch (Exception ex)
            { 
            }
        }
    }
}
