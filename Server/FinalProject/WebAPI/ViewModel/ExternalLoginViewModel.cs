using System;
using System.ComponentModel.DataAnnotations;


namespace WebAPI.ViewModel
{
    public class ParsedExternalAccessToken
    {
        public string user_id { get; set; }
        public string app_id { get; set; }
    }
    public class RegisterExternalBindingModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        public string ExternalAccessToken { get; set; }
    }
    public class GoogleUserOutputData
    {
        public string name { get; set; }
        public string gender { get; set; }
        public string birthday { get; set; }
        public string email { get; set; }
        public string picture { get; set; }
    }
}