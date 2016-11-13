using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;
using WebAPI.ViewModel;
using System.Net;
using System.Linq;
using JWT;
using System.Text;
using WebAPI.Data;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using WebAPI.Results;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using WebAPI.Models;
using System.Configuration;
using System.Data.Entity;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        public FOODEntities1 db = new FOODEntities1();
        public string CreateCode()
        {
            string _allowedChars = "ABCDEFGHIJKMNOPQRSTUVWXYZ0123456789";

            Random randNum = new Random();

            char[] chars = new char[6];

            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < 6; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
                if (chars[i] == '0' || chars[i] == '1' || chars[i] == '2' || chars[i] == '3' || chars[i] == '4'
              || chars[i] == '5' || chars[i] == '6' || chars[i] == '7' || chars[i] == '8' || chars[i] == '9')
                {
                    _allowedChars = "ABCDEFGHIJKMNOPQRSTUVWXYZ";
                }
            }
            return new string(chars);
        }
        [AllowAnonymous]
        [Route("ForgetPassword")]
        [HttpGet]
        public bool Main(string id)
        {
            int idaccount = int.Parse(id);
            if(db.EXTERNALACCOUNTs.FirstOrDefault(x=>x.IDUSER==idaccount)!=null)
            {
                return false;
            }
            ACCOUNT account = db.ACCOUNTs.FirstOrDefault(x => x.ID == idaccount);

            MailMessage mailMessag = new MailMessage(ConfigurationManager.AppSettings.Get("Email"),account.EMAIL);
            mailMessag.Subject = "Gửi lại mật khẩu";
            mailMessag.Body = "Mã khẩu của bạn là: "+CreateCode();
            SmtpClient client = new SmtpClient();
            client.Send(mailMessag);
            return true;
        }
        //LOGIN BÌNH THƯỜNG
        [AllowAnonymous]
        [Route("signin")]
        [HttpPost]
        public HttpResponseMessage Login(LoginViewModel model)
        {

            HttpResponseMessage response = null;
            if (ModelState.IsValid)
            {
                var existingUser = db.ACCOUNTs.FirstOrDefault(u => u.EMAIL == model.Email);

                if (existingUser == null)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    var loginSuccess =
                        string.Equals(EncryptPassword(model.Password, existingUser.SALT),
                            existingUser.PASSWORDHASH);

                    if (loginSuccess)
                    {
                        object dbUser;
                        var token = CreateTokenLogin(existingUser, out dbUser);
                        response = Request.CreateResponse(new { dbUser, token });
                        
                    }
                }
            }
            else
            {
                response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            return response;
        }

        [AllowAnonymous]
        [Route("signup")]
        [HttpPost]
        public HttpResponseMessage Register(RegisterViewModel model)
        {
            HttpResponseMessage response;
            if (ModelState.IsValid)
            {
                var existingUser = db.ACCOUNTs.FirstOrDefault(u => u.EMAIL == model.Email);
                if (existingUser != null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User already exist.");
                }

                //Create user and save to database
                var user = CreateUser(model);

                object dbUser;

                //Create token
                var token = CreateToken(user, out dbUser);

                response = Request.CreateResponse(new { dbUser, token });
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
            }

            return response;
        }
        [AllowAnonymous]
        [Route("ChangPass")]
        [HttpPut]
        public HttpResponseMessage Changpassword(ChangePasswordBindingModel model)
        {
            HttpResponseMessage response;
            if (ModelState.IsValid)
            {
                var existingUser = db.ACCOUNTs.FirstOrDefault(u => u.ID==model.ID);

                if (existingUser == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User not exist.");
                }

                //Update user and save to database
                var passwordSalt = CreateSalt();
                existingUser.SALT = passwordSalt;
                existingUser.PASSWORDHASH = EncryptPassword(model.NewPassword, passwordSalt);
                db.Entry(existingUser).State = EntityState.Modified;
                db.SaveChanges();
                object dbUser;

                //Create token
                var token = CreateToken(existingUser, out dbUser);

                response = Request.CreateResponse(new { dbUser, token });
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
            }
            return response;
        }

        /// <summary>
        /// Create a Jwt with user information
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dbUser"></param>
        /// <returns></returns>
        private static string CreateToken(ACCOUNT user, out object dbUser)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);


            var payload = new Dictionary<string, object>
            {
                {"email", user.EMAIL},
                {"userId", user.ID},
                {"role", "CUSTOMER"  },
                {"sub", user.ID},
                {"nbf", notBefore},
                {"iat", issuedAt},
                {"exp", expiry}
            };

            //var secret = ConfigurationManager.AppSettings.Get("jwtKey");
            const string apikey = "secretKey";

            var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            dbUser = new { user.EMAIL, user.ID };
            return token;
        }
        private static string CreateTokenLogin(ACCOUNT user, out object dbUser)
        {
            FOODEntities1 db = new FOODEntities1();
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);
            ACCOUNT_ROLE account= db.ACCOUNT_ROLE.FirstOrDefault(x => x.IDUSER == user.ID);
            LIST_ROLE listrole = db.LIST_ROLE.FirstOrDefault(x => x.ID == account.IDROLE);
            var payload = new Dictionary<string, object>
            {
                {"email", user.EMAIL},
                {"userId", user.ID},
                {"role", listrole.NAME  },
                {"sub", user.ID},
                {"nbf", notBefore},
                {"iat", issuedAt},
                {"exp", expiry}
            };

            //var secret = ConfigurationManager.AppSettings.Get("jwtKey");
            const string apikey = "secretKey";

            var token = JsonWebToken.Encode(payload, apikey, JwtHashAlgorithm.HS256);

            dbUser = new { user.EMAIL, user.ID };
            return token;
        }
        /// <summary>
        /// Create a new user and saves it to the database
        /// </summary>
        /// <param name="registerDetails"></param>
        /// <returns></returns>
        private ACCOUNT CreateUser(RegisterViewModel registerDetails)
        {
            var passwordSalt = CreateSalt();
            var user = new ACCOUNT
            {
                NAME = registerDetails.Username,
                BIRTHDATE = registerDetails.BirthDay,
                GENDER = registerDetails.Gender,
                SALT = passwordSalt,
                EMAIL = registerDetails.Email,
                PASSWORDHASH = EncryptPassword(registerDetails.Password, passwordSalt)
            };

            var Customer_Role = db.LIST_ROLE.FirstOrDefault(d => d.NAME == "CUSTOMER");

            user.ACCOUNT_ROLE.Add(new ACCOUNT_ROLE
            {
                ACCOUNT = user,
                LIST_ROLE = Customer_Role,
            });

            db.ACCOUNTs.Add(user);
            db.SaveChanges();

            return user;
        }


        /// <summary>
        ///     Creates a random salt to be used for encrypting a password
        /// </summary>
        /// <returns></returns>
        public static string CreateSalt()
        {
            var data = new byte[0x10];
            using (var cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                cryptoServiceProvider.GetBytes(data);
                return Convert.ToBase64String(data);
            }
        }

        /// <summary>
        ///     Encrypts a password using the given salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncryptPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}{1}", salt, password);
                var saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }
        // LOGIN FACEBOOK
        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            string redirectUri = string.Empty;

            if (error != null)
            {
                return BadRequest(Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var redirectUriValidationResult = ValidateClientAndRedirectUri(this.Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
            {
                return BadRequest(redirectUriValidationResult);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }
        

           EXTERNALACCOUNT user= db.EXTERNALACCOUNTs.FirstOrDefault(x => x.PROVIDERKEY == externalLogin.ProviderKey & x.LOGINPROVIDER==externalLogin.LoginProvider);
           // IdentityUser user = await _repo.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            redirectUri = string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.LoginProvider,
                                            hasRegistered.ToString(),
                                            externalLogin.UserName);

            return Redirect(redirectUri);

        }
        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {

            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            bool validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
            {
                return "redirect_uri is invalid";
            }

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
            {
                return "client_Id is required";
            }
            int converid = int.Parse(clientId);
            var client = db.ACCOUNTs.Find(converid);

            if (client == null)
            {
                return string.Format("Client_id '{0}' is not registered in the system.", clientId);
            }

           
            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;

        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }
        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string ExternalAccessToken { get; set; }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer) || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name),
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                };
            }
        }
        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {               
                var appToken = "243001122781125|iaT0O107l5tTNLvb3gsIjMdzQGc";
                verifyTokenEndPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
                else if (provider == "Google")
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (!string.Equals(Startup.googleAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }

                }

            }

            return parsedToken;
        }
      
        [AllowAnonymous]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            //IdentityUser user = await _repo.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));
            EXTERNALACCOUNT user = db.EXTERNALACCOUNTs.FirstOrDefault(x => x.PROVIDERKEY == verifiedAccessToken.user_id & x.LOGINPROVIDER==model.Provider);


            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                return BadRequest("External user is already registered");
            }
            EXTERNALACCOUNT external = new EXTERNALACCOUNT();
            external.IDUSER = db.ACCOUNTs.Count()+1;
            external.LOGINPROVIDER = model.Provider;
            external.PROVIDERKEY = verifiedAccessToken.user_id;
            db.EXTERNALACCOUNTs.Add(external);
            db.SaveChanges();
            object dbUser;
            ACCOUNT usermain=new ACCOUNT();
            usermain.SALT = CreateSalt();
            //Create token
            var token = CreateToken(usermain, out dbUser);            
            return Ok(token);
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {

            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
            {
                return BadRequest("Provider or external access token is not sent");
            }

            var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            EXTERNALACCOUNT user = db.EXTERNALACCOUNTs.FirstOrDefault(x => x.PROVIDERKEY == verifiedAccessToken.user_id & x.LOGINPROVIDER == provider);

            bool hasRegistered = user != null;

            if (!hasRegistered)
            {
                return BadRequest("External user is not registered");
            }
            ACCOUNT temp = db.ACCOUNTs.Find(user.IDUSER);
            //generate access token response
            object dbUser;
           
            ACCOUNT usermain = new ACCOUNT();
            //Create token
            var token = CreateToken(usermain, out dbUser);
                     
            var response = Request.CreateResponse(new { dbUser, token });
            //var accessTokenResponse = GenerateLocalAccessTokenResponse(temp.NAME);

            return Ok(response);

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

