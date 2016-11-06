﻿using System;
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
using System.Configuration;

namespace WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly FOODEntities db = new FOODEntities();

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
                        var token = CreateToken(existingUser, out dbUser);
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

