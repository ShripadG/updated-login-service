//using Microsoft.AspNetCore.Mvc;
//using loginservice.Models;
//using loginservice.Services;
//using loginservice.Common;
//using System.Threading.Tasks;
//using System.Text.Encodings.Web;
//using System;
//using Newtonsoft.Json;
//using System.Net;
//using System.Text;
//using System.IO;
//using ExcelDataReader;
//using System.Data.Common;
//using System.Data.OleDb;
//using System.Linq;
//using System.Collections.Generic; 
//using Microsoft.Extensions.PlatformAbstractions;
//using loginservice.Processors;
//using employeeservice.Common;

//namespace loginservice.Controllers
//{
//    [Route("api/[controller]")]
//    public class LoginUsersController : Controller
//    {
//        private readonly HtmlEncoder _htmlEncoder;
//        private readonly ICloudantService _cloudantService;
//        private readonly IPostUserLoginProcessor _postUserLoginProcessor;
//        private readonly IPutUserLoginProcessor _putUserLoginProcessor;

//        public LoginUsersController(HtmlEncoder htmlEncoder, IPostUserLoginProcessor postUserLoginProcessor, IPutUserLoginProcessor putUserLoginProcessor, ICloudantService cloudantService = null)
//        {
//            _cloudantService = cloudantService;
//            _postUserLoginProcessor = postUserLoginProcessor;
//            _putUserLoginProcessor = putUserLoginProcessor;
//            _htmlEncoder = htmlEncoder;
//        }

//        /// <summary>
//        /// Get all the records
//        /// </summary>
//        /// <returns>returns all records from database</returns>
//        [HttpGet]
//        public async Task<dynamic> Get()
//        {
//            if (_cloudantService == null)
//            {
//                return new string[] { "No database connection" };
//            }
//            else
//            {
//                return await _cloudantService.GetAllAsync(DBNames.loginusers.ToString());
//            }
//        }       

//        /// <summary>
//        /// Get record by ID
//        /// </summary>
//        /// <param name="id">ID to be selected</param>
//        /// <returns>record for the given id</returns>
//        [HttpGet("id")]
//        public async Task<dynamic> GetByID(string id)
//        {
//            if (_cloudantService == null)
//            {
//                return new string[] { "No database connection" };
//            }
//            else
//            {
//                return await _cloudantService.GetByIdAsync(id, DBNames.loginusers.ToString());
//            }
//        }

//        /// <summary>
//        /// Create a new record
//        /// </summary>
//        /// <param name="Loginuser">New record to be created</param>
//        /// <returns>status of the newly added record</returns>
//        [HttpPost("Registration")]
//        //public async Task<UpdateLoginUsersResponse> Post([FromBody]LoginUsersAddRequest loginuser)
//        public async Task<dynamic> Post([FromBody]LoginUsersAddRequest loginuser)
//        {
//            UpdateLoginUsersResponse registrationResponse = new UpdateLoginUsersResponse();
//            var response = await _cloudantService.GetAllAsync(DBNames.loginusers.ToString());
//            BulkData loginusers = JsonConvert.DeserializeObject<BulkData>(response);
//            var IsUserExist= loginusers.rows.FirstOrDefault(a => a.doc.Username == loginuser.Username);
//            if (IsUserExist == null)
//                if (_postUserLoginProcessor != null)
//                {
//                    //return await _postUserLoginProcessor.PostNewUserRecord(loginuser, _cloudantService);
                    
//                    HashSalt hashSalt = Helper.GenerateSaltedHash(64, loginuser.Password);
//                    loginuser.Password = hashSalt.Hash;
//                    loginuser.Passwordsalt = hashSalt.Salt;
//                    _postUserLoginProcessor.PostNewUserRecord(loginuser, _cloudantService);
//                    return new string[] { "SUCCESS" };
//                }
//                else
//                {
//                    //return new UpdateLoginUsersResponse();
//                    return new string[] { "No database connection" };
//                }
//            else
//            {
               
//                return new string[] { "USER_ALREADY_EXIST" };
//            }

//        }


//        /// <summary>
//        /// Update an existing record by giving _id and _rev values
//        /// </summary>
//        /// <param name="LoginUser">record to be updated for given _id and _rev</param>
//        /// <returns>status of the record updated</returns>
//        [HttpPut]
//        public async Task<dynamic> Update([FromBody]LoginUsers loginuser)
//        {
//            if (_postUserLoginProcessor != null)
//            {
//                return await _putUserLoginProcessor.PutExistingUserRecord(loginuser, _cloudantService);
//            }
//            else
//            {
//                return new string[] { "No database connection" };
//            }
//        }

//        [HttpPut("EditDetails")]
//        public async Task<dynamic> EditDetails([FromBody]EditDetails editDetails)
//        {
//            var response = await _cloudantService.GetAllAsync(DBNames.loginusers.ToString());
//            BulkData loginusers = JsonConvert.DeserializeObject<BulkData>(response);
//            var UpdatedLoginDetails = loginusers.rows.FirstOrDefault(a => a.doc.Username == editDetails.Username);
//            LoginUsers loginuser = new LoginUsers();
//            if (editDetails.Password != "")
//            {
//                HashSalt hashSalt = Helper.GenerateSaltedHash(64, editDetails.Password);
//                loginuser.Password = hashSalt.Hash;
//                loginuser.Passwordsalt = hashSalt.Salt;
//            }
//            else
//            {
//                loginuser.Password = UpdatedLoginDetails.doc.Password;
//                loginuser.Passwordsalt = UpdatedLoginDetails.doc.Passwordsalt;
//            }
//            if (editDetails.EmailID != "")
//            { loginuser.EmailID = editDetails.EmailID; }
//            else
//            { loginuser.EmailID = UpdatedLoginDetails.doc.EmailID; }

//            if (editDetails.Type != "")
//            { loginuser.Type = editDetails.Type; }
//            else
//            { loginuser.Type = UpdatedLoginDetails.doc.Type; }

//            loginuser.Username = UpdatedLoginDetails.doc.Username;            
//            loginuser._id = UpdatedLoginDetails.doc._id;
//            loginuser.Id = UpdatedLoginDetails.doc.Id;             
//            loginuser._rev = UpdatedLoginDetails.doc._rev;

            
//            if (_postUserLoginProcessor != null)
//            {
//                return await _putUserLoginProcessor.PutExistingUserRecord(loginuser, _cloudantService);
//            }
//            else
//            {
//                return new string[] { "No database connection" };
//            }
//        }

//        /// <summary>
//        /// Update an existing record by giving _id and _rev values
//        /// </summary>
//        /// <param name="validateloginuser">record to be updated for given _id and _rev</param>
//        /// <returns>status of the record updated</returns>
//        [HttpPut("Changepassword")]
//        public async Task<dynamic> ChangePassword([FromBody]ValidateLoginUsersAddRequest validateloginuser)
//        {
//            UpdateLoginUsersResponse registrationResponse = new UpdateLoginUsersResponse();
//            var response = await _cloudantService.GetAllAsync(DBNames.loginusers.ToString());
//            BulkData loginusers = JsonConvert.DeserializeObject<BulkData>(response);
//            var UpdatedLoginDetails = loginusers.rows.FirstOrDefault(a => a.doc.Username == validateloginuser.Username);

//            HashSalt hashSalt = Helper.GenerateSaltedHash(64, validateloginuser.Password);
            
//            LoginUsers loginuser = new LoginUsers();
//            loginuser.Username = UpdatedLoginDetails.doc.Username;
//            loginuser.Password = hashSalt.Hash;
//            loginuser.Passwordsalt = hashSalt.Salt;
//            loginuser._id = UpdatedLoginDetails.doc._id;
//            loginuser.Id = UpdatedLoginDetails.doc.Id;
//            loginuser.EmailID = UpdatedLoginDetails.doc.EmailID;
//            loginuser.Type = UpdatedLoginDetails.doc.Type;
//            loginuser._rev = UpdatedLoginDetails.doc._rev;

//            if (_postUserLoginProcessor != null)
//            {
//                return await _putUserLoginProcessor.PutExistingUserRecord(loginuser, _cloudantService);
//            }
//            else
//            {
//                return new string[] { "No database connection" };
//            }
//        }
//        /// <summary>
//        /// Delete the record for the given id
//        /// </summary>
//        /// <param name="id">record id to bb deleted</param>
//        /// <param name="rev">revision number of the record to be deleted</param>
//        /// <returns>status of the record deleted</returns>
//        [HttpDelete]
//        public async Task<dynamic> Delete(string id, string rev)
//        {
//            if (_cloudantService != null)
//            {
//                return await _cloudantService.DeleteAsync(id, rev, DBNames.loginusers.ToString());
                
//            }
//            else
//            {
//                return new string[] { "No database connection" };
//            }
//        }
//        /// <summary>
//        /// Create a new record
//        /// </summary>
//        /// <param name="LoginUser">New record to be created</param>
//        ///// <returns>status of the newly added record</returns>
//        [HttpPost("login")]
//        public async Task<UpdateLoginUsersvalidateResponse> PostLogin([FromBody]ValidateLoginUsersAddRequest validateloginuser)
//        {
//            var response = await _cloudantService.GetAllAsync(DBNames.loginusers.ToString());
//            BulkData loginusers = JsonConvert.DeserializeObject<BulkData>(response);

//            //if (validateloginuser.Username != null)
//            //{
//                var User = loginusers.rows.FirstOrDefault(a => a.doc.Username == validateloginuser.Username);
//                var validateuser = loginusers.rows.FirstOrDefault(a => a.doc.Username == validateloginuser.Username);// && a.doc.Password == validateloginuser.Password);
//                bool isPasswordMatched = Helper.VerifyPassword(validateloginuser.Password, User.doc.Password, User.doc.Passwordsalt);

//                if (isPasswordMatched)
//                {
//                    var responsetype = await _cloudantService.GetAllAsync(DBNames.userrights.ToString());
//                    BulkDataUserRights userrights = JsonConvert.DeserializeObject<BulkDataUserRights>(responsetype);
//                    var usertyperesponse = userrights.rows.FirstOrDefault(a => a.doc.logintypes == validateuser.doc.Type);
//                    UpdateLoginUsersvalidateResponse validloginuser = new UpdateLoginUsersvalidateResponse();
//                    validloginuser.logintypes = usertyperesponse.doc.logintypes;
//                    validloginuser.EmployeeMaster = usertyperesponse.doc.EmployeeMaster;
//                    validloginuser.ILCMaster = usertyperesponse.doc.ILCMaster;
//                    validloginuser.ForcastMaster = usertyperesponse.doc.ForcastMaster;
//                    validloginuser.Financials = usertyperesponse.doc.Financials;
//                    validloginuser.Reports = usertyperesponse.doc.Reports;
//                    validloginuser.LoginMaster = usertyperesponse.doc.LoginMaster;
//                    validloginuser.Id = usertyperesponse.doc._id;
//                    validloginuser.Rev = usertyperesponse.doc._rev;
//                    usertyperesponse.doc.ok = "Success";
//                    validloginuser.ok = usertyperesponse.doc.ok;
//                    //validloginuser.lateston = usertyperesponse.doc.lateston;
//                    return validloginuser;
//                }
//                else
//                {
//                    UpdateLoginUsersvalidateResponse validloginuser = new UpdateLoginUsersvalidateResponse();
//                    validloginuser.ok = "Wrong Password";
//                    return validloginuser;
//                }




//                //UpdateLoginUsersvalidateResponse validloginuser = new UpdateLoginUsersvalidateResponse();
//                //validloginuser.ok = "Invalid user name";
//                //return validloginuser;

//            //}

//        }
//        }
//}
