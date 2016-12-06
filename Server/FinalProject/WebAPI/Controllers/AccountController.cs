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
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using WebAPI.Models;
using WebAPI.Data;
using System.Configuration;
using System.Data.Entity;
using Facebook;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        public string imgnormal = "http://res.cloudinary.com/dqabuxewl/image/upload/v1480963523/default-user-icon-profile_ex6q2v.png";
        public FOODEntities db = new FOODEntities();
        /// <summary>
        /// Tạo mới password khi người dùng quên mật khẩu.
        /// </summary>
        /// <returns>Trả về password mới do hệ thống tự động cấp phát</returns>
        public string CreatePassword()
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

        /// <summary>
        /// Chức năng tạo mật khẩu người dùng sau đó send mail
        /// </summary>
        /// <param name="email">Xác định email có trong csdl để xác nhận và gửi email kèm theo password mới.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("ForgetPassword")]
        [HttpGet]
        public bool ForegetPassword(string email)
        {
            ACCOUNT account = db.ACCOUNTs.FirstOrDefault(x => x.EMAIL == email);
            if (account == null)
            {
                return false;
            }
            string temp = CreatePassword();
            MailMessage mailMessag = new MailMessage(ConfigurationManager.AppSettings.Get("Email"), account.EMAIL);
            mailMessag.Subject = "Gửi lại mật khẩu";
            mailMessag.Body = "Mật khẩu mới của bạn là: " + temp;
            SmtpClient client = new SmtpClient();
            client.Send(mailMessag);
            var passwordSalt = CreateSalt();
            account.SALT = passwordSalt;
            account.PASSWORDHASH = EncryptPassword(temp, passwordSalt);
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();
            return true;
        }
        
        /// <summary>
        /// LOGIN BÌNH THƯỜNG
        /// </summary>
        /// <param name="model">Model login chứa các thông tin cần thiết để đăng nhập bao gồm : Email, Password</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("SignIn")]
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
        
        /// <summary>
        /// Đăng kí tài khoản
        /// </summary>
        /// <param name="model">Model đăng kí tài khoản</param>
        /// <returns>response bao gồm email và token</returns>
        [AllowAnonymous]
        [Route("SignUp")]
        [HttpPost]
        public HttpResponseMessage Register(RegisterViewModel model)
        {
            HttpResponseMessage response;
            if (ModelState.IsValid)
            {
                string convert = model.Email.ToString();
                var existingUser = db.ACCOUNTs.FirstOrDefault(u => u.EMAIL ==convert);
                if (existingUser != null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User already exist.");
                }

                //Create user and save to database
                var user = CreateUser(model);

                object dbUser;

                //Create token
                var token = CreateTokenLogin(user, out dbUser);

                response = Request.CreateResponse(new { dbUser, token });
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
            }

            return response;
        }

        /// <summary>
        /// Đổi password
        /// </summary>
        /// <param name="model">Model đổi password</param>
        /// <returns>respone bao gồm token với kết quả trả về</returns>
        [AllowAnonymous]
        [Route("ChangePassword")]
        [HttpPut]
        public HttpResponseMessage Changpassword(ChangePasswordBindingModel model)
        {
            HttpResponseMessage response;
            if (ModelState.IsValid)
            {
                var existingUser = db.ACCOUNTs.FirstOrDefault(u => u.EMAIL == model.Email);

                if (existingUser == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "User not exist.");
                }
                var loginSuccess =
                        string.Equals(EncryptPassword(model.OldPassword, existingUser.SALT),
                            existingUser.PASSWORDHASH);
                    if(loginSuccess)
                    {
                    //Update user and save to database
                    var passwordSalt = CreateSalt();
                    existingUser.SALT = passwordSalt;
                    existingUser.PASSWORDHASH = EncryptPassword(model.NewPassword, passwordSalt);
                    db.Entry(existingUser).State = EntityState.Modified;
                    db.SaveChanges();             
                  }
                object dbUser;
                //Create token
                var token = CreateTokenLogin(existingUser, out dbUser);
                response = Request.CreateResponse(new { dbUser, token });
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false });
            }
            return response;
        }

        
        /// <summary>
        /// Tạo token cho người dùng đăng nhập facebook, google...
        /// </summary>
        /// <param name="user">Thông tin cơ bản người dùng</param>
        /// <param name="dbUser">Thông báo User trong Response</param>
        /// <returns></returns>
        private static string CreateTokenLogin(ACCOUNT user, out object dbUser)
        {
            FOODEntities db = new FOODEntities();
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var expiry = Math.Round((DateTime.UtcNow.AddHours(2) - unixEpoch).TotalSeconds);
            var issuedAt = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);
            var notBefore = Math.Round((DateTime.UtcNow.AddMonths(6) - unixEpoch).TotalSeconds);
            ACCOUNT_ROLE account = db.ACCOUNT_ROLE.FirstOrDefault(x => x.IDUSER == user.ID);
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
        /// Tạo người dùng lưu vào Database
        /// <param name="registerDetails">Model đăng kí tài khoản mới</param>
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
                PASSWORDHASH = EncryptPassword(registerDetails.Password, passwordSalt),
                IMAGEACC = imgnormal
            };

            
            DbContextTransaction dt = db.Database.BeginTransaction();
            try
            {
                db.ACCOUNTs.Add(user);
                db.SaveChanges();
                ACCOUNT_ROLE role = new ACCOUNT_ROLE();
                role.IDROLE = 2;
                role.IDUSER = user.ID;
                db.ACCOUNT_ROLE.Add(role);
                db.SaveChanges();
                dt.Commit();
            }
            catch (Exception ex)
            {
                dt.Rollback();
            }
           
            return user;
        }


        /// <summary>
        /// Tạo mã để mã hóa password
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
        /// Giải mã password
        /// </summary>
        /// <param name="password">Password người dùng.</param>
        /// <param name="salt">Mã salt để encrypt pass</param>
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
                    parsedToken.app_id= jObj["data"]["app_id"];
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
            string name = "";
            string birthday = "";
            string gender = "";
            string email = "";
            string picture = "";
            dynamic myInfo = "";
            var token = "";
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }
            EXTERNALACCOUNT user = db.EXTERNALACCOUNTs.FirstOrDefault
            (x => x.PROVIDERKEY == verifiedAccessToken.user_id & x.LOGINPROVIDER == model.Provider);
            bool hasRegistered = user != null;
            object dbUser;
            if (hasRegistered)
            {
                ACCOUNT account1 = db.ACCOUNTs.FirstOrDefault(x => x.ID == user.IDUSER);
                token = CreateTokenLogin(account1, out dbUser);
                return Ok(token);
            }
            if (model.Provider == "Facebook")
            {
                var fb = new FacebookClient(model.ExternalAccessToken);
                myInfo = fb.Get("/me?fields=name,id,gender,birthday,email,picture");
                if (myInfo["email"] != "")
                {
                    email = myInfo["email"];
                }
                if ((myInfo["name"] != ""))
                {
                    name = myInfo["name"];
                }
                try
                {
                    birthday = myInfo["birthday"];
                }
                catch (Exception ex)
                { }
                try
                {
                    gender = myInfo["gender"];
                    gender = gender.ToUpper();
                }
                catch (Exception ex)
                { }
                try
                { picture = picture = String.Format("https://graph.facebook.com/{0}/picture?width=200&height=200", verifiedAccessToken.user_id);
                }
                catch(Exception ex)
                { }
            }

            if (model.Provider == "Google")
            {
                HttpClient client = new HttpClient();
                var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + model.ExternalAccessToken;
                client.CancelPendingRequests();
                HttpResponseMessage output = await client.GetAsync(urlProfile);
                if (output.IsSuccessStatusCode)
                {
                    string outputData = await output.Content.ReadAsStringAsync();
                    GoogleUserOutputData serStatus = JsonConvert.DeserializeObject<GoogleUserOutputData>(outputData);

                    if (serStatus != null)
                    {
                        if (!string.IsNullOrEmpty(serStatus.email))
                        {
                            email = serStatus.email;
                        }
                        if (!string.IsNullOrEmpty(serStatus.name))
                        {
                            name = serStatus.name;
                        }
                        if (!string.IsNullOrEmpty(serStatus.birthday))
                        {
                            birthday = serStatus.birthday;
                        }
                        if (!string.IsNullOrEmpty(serStatus.gender))
                        {
                            gender = serStatus.gender;
                            gender = gender.ToUpper();
                        }
                        if(!string.IsNullOrEmpty(serStatus.picture))
                        {
                            picture = serStatus.picture;
                        }
                    }
                }
            }

            EXTERNALACCOUNT external = new EXTERNALACCOUNT();
            ACCOUNT usermain = new ACCOUNT();
            ACCOUNT acc = new ACCOUNT();
            acc = db.ACCOUNTs.FirstOrDefault(x => x.EMAIL == email);
            if (acc != null)
            {
                external.PROVIDERKEY = verifiedAccessToken.user_id;
                external.IDUSER = acc.ID;
                external.LOGINPROVIDER = model.Provider;
                db.EXTERNALACCOUNTs.Add(external);
                db.SaveChanges();
            }
            else
            {
                external.PROVIDERKEY = verifiedAccessToken.user_id;
                external.IDUSER = db.ACCOUNTs.Count() + 1;
                external.LOGINPROVIDER = model.Provider;
                if (!string.IsNullOrEmpty(birthday))
                {
                    DateTime convert = Convert.ToDateTime(birthday);
                    usermain.BIRTHDATE = convert;
                }

                if (!string.IsNullOrEmpty(email))
                {
                    usermain.EMAIL = email;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    usermain.NAME = name;
                }
                if (!string.IsNullOrEmpty(gender))
                {
                    if (gender == "MALE")
                    {
                        usermain.GENDER = "NAM";
                    }
                    else
                    {
                        usermain.GENDER = "NỮ";
                    }

                }
                usermain.ID = external.IDUSER;
                ACCOUNT_ROLE role = new ACCOUNT_ROLE();
                role.IDUSER = external.IDUSER;
                role.IDROLE = 2;
                if (!string.IsNullOrEmpty(picture))
                {
                    Account account = new Account("dqabuxewl", "198449299438919", "SRASj3YoFcfLsetrHFNNwGVF4qQ");
                    CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account);

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(picture)
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);
                    usermain.IMAGEACC = uploadResult.Uri.OriginalString;


                }
                else
                {
                    usermain.IMAGEACC = imgnormal;
                }
                DbContextTransaction dt = db.Database.BeginTransaction();
                try
                {
                    db.EXTERNALACCOUNTs.Add(external);
                    db.SaveChanges();
                    db.ACCOUNTs.Add(usermain);
                    db.SaveChanges();
                    db.ACCOUNT_ROLE.Add(role);
                    db.SaveChanges();
                    dt.Commit();
                }
                catch (Exception ex)
                {
                    dt.Rollback();
                    return BadRequest("Error");
                }
            }
           
            //Create token
            usermain = new ACCOUNT();
            usermain = db.ACCOUNTs.FirstOrDefault(x => x.EMAIL == email);
            token = CreateTokenLogin(usermain, out dbUser);
           
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
            ACCOUNT temp = db.ACCOUNTs.FirstOrDefault(x=>x.ID==user.IDUSER);
            //generate access token response
            object dbUser;
           
            //Create token
            var token = CreateTokenLogin(temp, out dbUser);

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

