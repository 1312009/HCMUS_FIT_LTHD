using System;
using System.ComponentModel.DataAnnotations;


namespace WebAPI.ViewModel
{
    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }
    public class ParsedExternalAccessToken
    {
        public string user_id { get; set; }
        public string app_id { get; set; }
    }
    public class RegisterExternalBindingModel
    {
        public string Username { get; set; }
 
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        public DateTime BirthDay { get; set; }
     
        public string Gender { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        public string ExternalAccessToken { get; set; }
    }
}