using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using Abp.UI;
using Broker.Authentication.External;
using Broker.Authentication.JwtBearer;
using Broker.Authorization;
using Broker.Authorization.Users;
using Broker.Models.TokenAuth;
using Broker.MultiTenancy;
using Broker.Customers;
using Abp.Domain.Repositories;
using Broker.Users;
using Broker.Customers.Dto;
using Broker.Helpers;
using Broker.Models;
using System.Threading;
using Broker.Lookups;
using Abp.Domain.Uow;
using static Broker.Authorization.PermissionNames;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using Broker.Users.Dto;
using static System.Net.WebRequestMethods;
using System.Security.Policy;
using Abp.Web.Models;
using AutoMapper;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Broker.Configuration;
using Broker.Users.Dto;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace Broker.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : BrokerControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly ITenantCache _tenantCache;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly UserRegistrationManager _userRegistrationManager;

        private readonly IRepository<Seeker, long> _seekerRepository;
        private readonly IRepository<Owner, long> _ownerRepository;
        private readonly IRepository<BrokerPerson, long> _brokerPersonRepository;
        private readonly IRepository<Company, long> _companyRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IUserAppService _userAppService;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _env;
        private readonly ISettingAppService _settingAppService;
        private readonly IUserDeviceAppService _userDeviceAppService;
        public TokenAuthController(
            LogInManager logInManager,
            ITenantCache tenantCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthManager externalAuthManager,
            UserRegistrationManager userRegistrationManager,

            IRepository<Seeker, long> seekerRepository,
            IRepository<Owner, long> ownerRepository,
            IRepository<BrokerPerson, long> brokerPersonRepository,
            IRepository<Company, long> companyRepository,
            IRepository<User, long> userRepository
           , IUserAppService userAppService
            , IWebHostEnvironment env,
             ISettingAppService settingAppService,
             IUserDeviceAppService userDeviceAppService)
        {
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;

            _seekerRepository = seekerRepository;
            _ownerRepository = ownerRepository;
            _brokerPersonRepository = brokerPersonRepository;
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _userAppService = userAppService;
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
            _settingAppService = settingAppService;
            _userDeviceAppService = userDeviceAppService;

        }

        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == model.UserNameOrEmailAddress || x.UserName == model.UserNameOrEmailAddress || x.PhoneNumber == model.UserNameOrEmailAddress);

            if (user == null && !string.IsNullOrEmpty(model.Password))
                throw new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
            ClaimsIdentity identity = new ClaimsIdentity();

            var loginResult = await GetLoginResultAsync(
                model.UserNameOrEmailAddress,
                model.Password,
                GetTenancyNameOrNull()
            );

            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id,
                LoginType = await GetLoginType(user.Id)
            };
        }

        [HttpPost]
        public async Task<AuthenticateResultModel> AuthenticateInMobile([FromBody] AuthenticateModel model)
        {
            try
            {

                var user = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == model.UserNameOrEmailAddress || x.UserName == model.UserNameOrEmailAddress || x.PhoneNumber == model.UserNameOrEmailAddress);

                if (user == null && !string.IsNullOrEmpty(model.Password))
                    throw new UserFriendlyException(L("LoginFailed"), L("InvalidUserNameOrPassword"));
                ClaimsIdentity identity = new ClaimsIdentity();

                var loginResult = await GetLoginResultAsync(
                    user.UserName,
                    model.Password,
                    GetTenancyNameOrNull()
                );
                identity = loginResult.Identity;

                var accessToken = CreateAccessToken(CreateJwtClaims(identity));
                 if (!string.IsNullOrEmpty(model.RegistrationDevice))// if (deviceInfo != null)
                 {
               
                     await _userDeviceAppService.Check(new CheckRegisteredDeviceInput()
                     {
                         RegistrationToken = model.RegistrationDevice,
                         UserId = user.Id
                     });
                 }
                return new AuthenticateResultModel
                {
                    AccessToken = accessToken,
                    EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                    ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                    UserId = user.Id,
                    LoginType = await GetLoginType(user.Id)
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }


        async Task<LoginType> GetLoginType(long id)
        {
            LoginType result = new LoginType();
            var seeker = await _seekerRepository.FirstOrDefaultAsync(x => x.UserId == id);
            var owner = await _ownerRepository.FirstOrDefaultAsync(x => x.UserId == id);
            var broker = await _brokerPersonRepository.FirstOrDefaultAsync(x => x.UserId == id);
            var company = await _companyRepository.FirstOrDefaultAsync(x => x.UserId == id);

            if (seeker != null)
            {
                result = LoginType.seeker;

            }
            else if (owner != null)
            {
                result = LoginType.owner;

            }
            else if (broker != null)
            {
                result = LoginType.broker;

            }
            else if (company != null)
            {
                result = LoginType.company;

            }

            return result;
        }

        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        {
            var externalUser = await GetExternalUserInfo(model);

            var loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    {
                        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));
                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                        };
                    }
                case AbpLoginResultType.UnknownExternalLogin:
                    {
                        var newUser = await RegisterExternalUserAsync(externalUser);
                        if (!newUser.IsActive)
                        {
                            return new ExternalAuthenticateResultModel
                            {
                                WaitingForActivation = true
                            };
                        }

                        // Try to login again with newly registered user!
                        loginResult = await _logInManager.LoginAsync(new UserLoginInfo(model.AuthProvider, model.ProviderKey, model.AuthProvider), GetTenancyNameOrNull());
                        if (loginResult.Result != AbpLoginResultType.Success)
                        {
                            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                                loginResult.Result,
                                model.ProviderKey,
                                GetTenancyNameOrNull()
                            );
                        }

                        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

                        return new ExternalAuthenticateResultModel
                        {
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                        };
                    }
                default:
                    {
                        throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                            loginResult.Result,
                            model.ProviderKey,
                            GetTenancyNameOrNull()
                        );
                    }
            }
        }

        private async Task<User> RegisterExternalUserAsync(ExternalAuthUserInfo externalUser)
        {
            var user = await _userRegistrationManager.RegisterAsync(
                externalUser.Name,
                externalUser.Surname,
                externalUser.EmailAddress,
                externalUser.EmailAddress,
                Authorization.Users.User.CreateRandomPassword(),
                true
            );

            user.Logins = new List<UserLogin>
            {
                new UserLogin
                {
                    LoginProvider = externalUser.Provider,
                    ProviderKey = externalUser.ProviderKey,
                    TenantId = user.TenantId
                }
            };

            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        private async Task<ExternalAuthUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            if (userInfo.ProviderKey != model.ProviderKey)
            {
                throw new UserFriendlyException(L("CouldNotValidateExternalUser"));
            }

            return userInfo;
        }

        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);
            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
            //if (loginResult.User != null && loginResult.User.IsMobileUser == true)
            //{
            //    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(AbpLoginResultType.InvalidUserNameOrEmailAddress, usernameOrEmailAddress, tenancyName);
            //}
            //else
            //{
            //    switch (loginResult.Result)
            //    {
            //        case AbpLoginResultType.Success:
            //            return loginResult;
            //        default:
            //            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            //    }
            //}
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultInMobileAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            try
            {
                var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);
                if (loginResult.User != null && loginResult.User.IsMobileUser == false)
                {
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(AbpLoginResultType.InvalidUserNameOrEmailAddress, usernameOrEmailAddress, tenancyName);
                }
                else
                {
                    switch (loginResult.Result)
                    {
                        case AbpLoginResultType.Success:
                            return loginResult;
                        default:
                            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                //expires: now.Add(expiration ?? _configuration.Expiration),
                expires: now.AddYears(1),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken);
        }

                              //********Wessage Misr whatsUp****************
        //[HttpPost]
        //public async Task<OtpWessageResponseModel> CheckPhoneNumberOtp([FromBody] OtpRequestModel model)
        //{
    
        //        var otp = _userAppService.GenerateOtp();
        //        var response =await _userAppService.SendWessageWhatsupLink("2" + model.PhoneNumber, otp);
        //        if (response.Contains("5500"))
        //        {
        //        // Parse JSON string
        //        JObject json = JObject.Parse(response);
        //        // Extract the Clickable URL
        //        string clickableUrl = (string)json["Clickable"];
        //        Console.WriteLine("Clickable URL: " + clickableUrl);

        //        return new OtpWessageResponseModel { IsSuccess = true,Otp = otp,Link= clickableUrl, Message = L("Success") };
        //        }
        //        else
        //        {
        //        return new OtpWessageResponseModel { IsSuccess = false, Otp = null, Link = null, Message = L("Fail") };
        //        }
        //}

        //********Sms Misr****************
        [HttpPost]
        public async Task<OtpResponseModel> CheckPhoneNumberOtp([FromBody] OtpRequestModel model)
        {

            var otp = _userAppService.GenerateOtp();
            var msgsucess = await _userAppService.SendSMSOTP("2" + model.PhoneNumber, otp);
            if (msgsucess.Contains("4901"))
            {
                return new OtpResponseModel { IsSuccess = true, Otp = otp, Message = L("OtpSentSuccessfuly") };
            }
            else
            {
                return new OtpResponseModel { IsSuccess = false, Otp = null, Message = L("FailToSendOtp") };
            }
        }

        [HttpPost]
        public async Task<OtpResponseModel> CheckPhoneNumberOtpForUpdatePhone([FromBody] OtpRequestModel model)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);

            if (user == null)
            {
                var otp = _userAppService.GenerateOtp();
                var msgsucess = await _userAppService.SendSMSOTP("2" + model.PhoneNumber, otp);
                if (msgsucess.Contains("4901"))
                {
                    return new OtpResponseModel { IsSuccess = true, Otp = otp, Message = L("OtpSentSuccessfuly") };
                }
                else
                {
                    return new OtpResponseModel { IsSuccess = false, Otp = null, Message = L("FailToSendOtp") };
                }
            }
            else
                return new OtpResponseModel { IsSuccess = false, Otp = null, Message = L("Pages.Users.Error.PhoneNumberAlreadyExist") };

        }

        //********Payment PayMob****************
        [HttpPost]
        public async Task<PaymentResponseModel> GetPaymentUrl([FromBody] PaymentRequestModel model)
        {
            var userInfo = new UserDto();
           if(model.userId>0)
              userInfo =await _userAppService.GetAsync(new Abp.Application.Services.Dto.EntityDto<long> { Id = model.userId });
           
            #region First ==> Token and Profile
            using (var client = new HttpClient())
            {

                var p = new { api_key = "ZXlKaGJHY2lPaUpJVXpVeE1pSXNJblI1Y0NJNklrcFhWQ0o5LmV5SmpiR0Z6Y3lJNklrMWxjbU5vWVc1MElpd2ljSEp2Wm1sc1pWOXdheUk2T0RFeE1UQTNMQ0p1WVcxbElqb2lhVzVwZEdsaGJDSjkuRjJzblQ3T2N2TmUyYnVSY0o5Umt3cXJESF94YTBjNnRMS3FlMG1DZkpqbGlBa3NjZ3Nabkx1X2NQdU1CbEhVNW5zVUdoZFhVSDdOMko5R3ZoMUFEN2c=" };
                //var p = new { api_key = "ZXlKaGJHY2lPaUpJVXpVeE1pSXNJblI1Y0NJNklrcFhWQ0o5LmV5SmpiR0Z6Y3lJNklrMWxjbU5vWVc1MElpd2ljSEp2Wm1sc1pWOXdheUk2T0RFeE1UQTNMQ0p1WVcxbElqb2lhVzVwZEdsaGJDSjkuRjJzblQ3T2N2TmUyYnVSY0o5Umt3cXJESF94YTBjNnRMS3FlMG1DZkpqbGlBa3NjZ3Nabkx1X2NQdU1CbEhVNW5zVUdoZFhVSDdOMko5R3ZoMUFEN2c=" };
                
                client.BaseAddress = new Uri("https://accept.paymob.com/");
                var response = client.PostAsJsonAsync("api/auth/tokens", p).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    var AcceptViewModel = JsonConvert.DeserializeObject<AcceptModel>(responseBody);
                    #endregion first
                    
                    #region Second ==> order_id
                    using (var client_2 = new HttpClient())
                    {
                        var p_2 = new
                        {
                            auth_token = AcceptViewModel.token, //1
                            delivery_needed = "false",
                            merchant_id = AcceptViewModel.profile.Id,//1 ////******
                            amount_cents = (model.amount * 100).ToString(),
                            //amount_cents = amount.ToString(),
                            currency = "EGP",
                            items = new List<object>() { },
                        };
                        client_2.BaseAddress = new Uri("https://accept.paymob.com/"); //https://accept.paymobsolutions.com/
                        var response_2 = client_2.PostAsJsonAsync("api/ecommerce/orders", p_2).Result;
                        if (response_2.IsSuccessStatusCode)
                        {
                            string responseBody_2 = response_2.Content.ReadAsStringAsync().Result;
                            var OrdersAcceptViewModel = JsonConvert.DeserializeObject<OrdersAcceptModel>(responseBody_2);
                            #endregion Second
                            using (var client_3 = new HttpClient())
                            {
                                var p_3 = new
                                {
                                    auth_token = AcceptViewModel.token,
                                    amount_cents = (model.amount * 100).ToString(),
                                    // amount_cents = amount.ToString(),
                                    expiration = 3600,
                                    order_id = OrdersAcceptViewModel.id,
                                   // owner =2,// userInfo!=null? userInfo.Id:0,
                                     billing_data = new
                                     {
                                         apartment = "803",
                                         email = userInfo.EmailAddress != null ? userInfo.EmailAddress : "claudette09@exa.com",
                                         floor = "42",
                                         first_name = userInfo.Name != null ? userInfo.Name : "Broker",
                                         street =  /* userInfo.Address1 != null ? shippingModel.Address1 : */"Ethan Land",
                                         building = "8028",
                                         phone_number = userInfo.PhoneNumber != null ? userInfo.PhoneNumber : "01021882144",
                                         shipping_method = "PKG",
                                         postal_code = /*shippingModel.ZipPostalCode != null ? shippingModel.ZipPostalCode :*/ "01898",
                                         city = /*shippingModel.City != null ? shippingModel.City :*/ "Jaskolskiburgh",
                                         country = /*country != null ? country.TwoLetterIsoCode :*/ "EG",
                                         last_name = userInfo.Surname != null ? userInfo.Surname : "Broker",
                                         state = /*StateProvince != null ? StateProvince.Name : */"Cairo",
                                     },
                                    currency = "EGP",
                                    integration_id = "4442386",// "3875936",//3875936 //3875846
                                    lock_order_when_paid = "false"//********
                                };
                                client_3.BaseAddress = new Uri("https://accept.paymob.com/");//solutions
                                var response_3 = client_2.PostAsJsonAsync("api/acceptance/payment_keys", p_3).Result;
                                if (response_3.IsSuccessStatusCode)
                                {
                                    string responseBody_3 = response_3.Content.ReadAsStringAsync().Result;
                                    var token = JsonConvert.DeserializeObject<TokenModel>(responseBody_3);

                                    //string url = "https://accept.paymob.com/api/acceptance/iframes/371916?payment_token=" + token.token;
                                    string url = "https://accept.paymob.com/api/acceptance/iframes/764133?payment_token=" + token.token;
                                    return new PaymentResponseModel { IsSuccess = true, Url = url };
                                }
                            }
                        }
                    }
                }
            }
            return new PaymentResponseModel { IsSuccess = false, Url = null };
        }
        [HttpPost]
        [AllowAnonymous]
        public virtual  async Task<bool> ConfirmPaymentAccept(bool success)
        {
            return success;
        }


        //[HttpPost]
        //public async Task<JsonResult> ForgetPassword([FromBody]ForgetPasswordModel model)
        //{
        //    try
        //    {
        //        var user = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == model.UserNameOrEmailAddress || x.UserName == model.UserNameOrEmailAddress || x.PhoneNumber == model.UserNameOrEmailAddress);

        //        var host = _appConfiguration["App:ClientRootAddress"];
        //        var host2 = "https://broker.nahrdev.website/";
        //        var restCode = await _userAppService.ForgetPassword(new ForgetPasswordDto { UserNameOrEmailAddress = user.EmailAddress });
        //        if (restCode == "Failed")
        //        {
        //            throw new UserFriendlyException("Only users may reset its passwords.");
        //        }
        //        var path = host2 + "#/account/resetpassword/" + user.EmailAddress + "/" + restCode;
        //        string html_mail_body = $@"
        //              <html>
        //                  <body>
        //                          <p>{L("Common.Welcome")} {user.EmailAddress},</p><br />
        //                          <p>{L("Pages.Register.ResetPasswordCode")}</p><br />
        //                          <a href='{path}'><b>{path}</b></a>
        //                  </body>
        //              </html>";

        //        var mailSettingsData = await _settingAppService.GetMailSetting();

        //        //var mailSettingsData = await _settingAppService.GetMailSetting();//|| x.PhoneNumber == model.UserNameOrEmailAddress
        //        //var user = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == model.EmailAddress || x.UserName == model.EmailAddress );

        //        var message = new MailMessage(mailSettingsData.Sender, user.EmailAddress)
        //        {
        //            Subject = "Reset Password | Broker",
        //            Body = html_mail_body,
        //            IsBodyHtml = true
        //        };  
        //     //  var message = new MailMessage(mailSettingsData.Sender, user.EmailAddress)
        //     //  {
        //     //      Subject = L("Pages.Register.ResetPassword") + " | " + L("Common.SystemTitle"),
        //     //      Body = html_mail_body,
        //     //      IsBodyHtml = true
        //     //
        //     //  };
        //        await Mailer.SendEmailAsync(message, mailSettingsData);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new AjaxResponse { Success = false, Error = new ErrorInfo(L("Common.Error.InvalidSetting")) });
        //    }
        //    return Json(new AjaxResponse { Success = true, Result = L("Pages.Register.CheckEmailToResetPassword") });
        //}

        [HttpPost]
        public async Task<JsonResult> ForgetPassword([FromBody] ForgetPasswordModel model)
        {
            try
            {
                var user = await _userRepository.FirstOrDefaultAsync(x => x.EmailAddress == model.UserNameOrEmailAddress || x.UserName == model.UserNameOrEmailAddress || x.PhoneNumber == model.UserNameOrEmailAddress);
               
                if (user==null)
                    return Json(new AjaxResponse { Success = false, Error = new ErrorInfo(L("Common.Error.UserWithNumberNotFound")) });
               
                var host = _appConfiguration["App:ClientRootAddress"];
                var host2 = "https://broker.nahrdev.website/";
                var restCode = await _userAppService.ForgetPassword(new ForgetPasswordDto { UserNameOrEmailAddress = user.EmailAddress });
                if (restCode == "Failed")
                {
                    throw new UserFriendlyException("Only users may reset its passwords.");
                }
                var path = host2 + "#/account/resetpassword/" + user.EmailAddress + "/" + restCode;
                string html_mail_body = $@"
                      <html>
                          <body>
                                  <p>{L("Common.Welcome")} {user.EmailAddress},</p><br />
                                  <p>{L("Pages.Register.ResetPasswordCode")}</p><br />
                                  <a href='{path}'><b>{path}</b></a>
                          </body>
                      </html>";

                var mailSettingsData = await _settingAppService.GetMailSetting();

                var message = new MailMessage(mailSettingsData.Sender, user.EmailAddress)
                {
                    Subject = L("Pages.Register.ResetPassword") /*+ " | " + L("Common.SystemTitle")*/,
                    Body = html_mail_body,
                    IsBodyHtml = true

                };
                await Mailer.SendEmailAsync(message, mailSettingsData);
            }
            catch (Exception ex)
            {
                return Json(new AjaxResponse { Success = false, Error = new ErrorInfo(L("Common.Error.InvalidSetting")) });
            }
            return Json(new AjaxResponse { Success = true, Result = L("Pages.Register.CheckEmailToResetPassword") });
        }
    }

}
