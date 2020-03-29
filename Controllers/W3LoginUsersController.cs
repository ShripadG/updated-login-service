using Microsoft.AspNetCore.Mvc;
using loginservice.Models;
using loginservice.Services;
using loginservice.Common;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.IO;
using ExcelDataReader;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Collections.Generic; 
using Microsoft.Extensions.PlatformAbstractions;
using loginservice.Processors;
using employeeservice.Common;

namespace loginservice.Controllers
{
    [Route("api/[controller]")]
    public class W3LoginUsersController : Controller
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly ICloudantService _cloudantService;
        private readonly IPostW3UserLoginProcessor _postW3UserLoginProcessor;
        private readonly IPutW3UserLoginProcessor _putW3UserLoginProcessor;

        public W3LoginUsersController(HtmlEncoder htmlEncoder, IPostW3UserLoginProcessor postW3UserLoginProcessor, IPutW3UserLoginProcessor putW3UserLoginProcessor, ICloudantService cloudantService = null)
        {
            _cloudantService = cloudantService;
            _postW3UserLoginProcessor = postW3UserLoginProcessor;
            _putW3UserLoginProcessor = putW3UserLoginProcessor;
            _htmlEncoder = htmlEncoder;
        }

        /// <summary>
        /// Get all the records
        /// </summary>
        /// <returns>returns all records from database</returns>
        [HttpGet]
        public async Task<dynamic> Get()
        {
            if (_cloudantService == null)
            {
                return new string[] { "No database connection" };
            }
            else
            {
                return await _cloudantService.GetAllAsync(DBNames.W3loginusers.ToString());
            }
        }       



        /// <summary>
        /// Create a new record
        /// </summary>
        /// <param name = "Loginuser" > New record to be created</param>
        /// <returns>status of the newly added record</returns>

            [HttpPost("Registration")]
        //public async Task<UpdateLoginUsersResponse> Post([FromBody]LoginUsersAddRequest loginuser)
        public async Task<dynamic> Post([FromBody]LoginUsersAddRequest loginuser)
        {
            UpdateLoginUsersResponse registrationResponse = new UpdateLoginUsersResponse();
            //var response = await _cloudantService.GetAllAsync(DBNames.W3loginusers.ToString());
            //BulkData loginusers = JsonConvert.DeserializeObject<BulkData>(response);
            //var IsUserExist= loginusers.rows.FirstOrDefault(a => a.doc.Email == loginuser.Email);
            //if (IsUserExist == null)
            if (_postW3UserLoginProcessor != null)
            {
                //return await _postUserLoginProcessor.PostNewUserRecord(loginuser, _cloudantService);

                //HashSalt hashSalt = Helper.GenerateSaltedHash(64, loginuser.Password);
                //loginuser.Password = hashSalt.Hash;
                //loginuser.Passwordsalt = hashSalt.Salt;
                _postW3UserLoginProcessor.PostW3NewUserRecord(loginuser, _cloudantService);
                 return new string[] { "SUCCESS" };
            }
            else
            {
                 return new string[] { "No database connection" };             
            }

        }





        /// <summary>
        /// Delete the record for the given id
        /// </summary>
        /// <param name="id">record id to bb deleted</param>
        /// <param name="rev">revision number of the record to be deleted</param>
        /// <returns>status of the record deleted</returns>
        [HttpDelete]
        public async Task<dynamic> Delete(string id, string rev)
        {
            if (_cloudantService != null)
            {
                return await _cloudantService.DeleteAsync(id, rev, DBNames.W3loginusers.ToString());
                
            }
            else
            {
                return new string[] { "No database connection" };
            }
        }
        /// <summary>
        /// Check if valid user and return response with roles for valid user
        /// </summary>
        /// <param name="LoginUser">New record to be created</param>
        ///// <returns>status of the newly added record</returns>
        [HttpPost("W3login")]
        public async Task<UpdateLoginUsersvalidateResponse> PostLogin([FromBody]ValidateLoginUsersAddRequest validateloginuser)
        {
            var response = await _cloudantService.GetAllAsync(DBNames.W3loginusers.ToString());
            BulkData loginusers = JsonConvert.DeserializeObject<BulkData>(response);

            //if (validateloginuser.Username != null)
            //{
                //var User = loginusers.rows.FirstOrDefault(a => a.doc.Username == validateloginuser.Username);
                var validateuser = loginusers.rows.FirstOrDefault(a => a.doc.Email == validateloginuser.Email);// && a.doc.Password == validateloginuser.Password);
               

                if (validateuser != null)
                {
                    var responsetype = await _cloudantService.GetAllAsync(DBNames.userrights.ToString());
                    BulkDataUserRights userrights = JsonConvert.DeserializeObject<BulkDataUserRights>(responsetype);
                    var usertyperesponse = userrights.rows.FirstOrDefault(a => a.doc.logintypes == validateuser.doc.Type);
                    UpdateLoginUsersvalidateResponse validloginuser = new UpdateLoginUsersvalidateResponse();
                    validloginuser.logintypes = usertyperesponse.doc.logintypes;
                    validloginuser.EmployeeMaster = usertyperesponse.doc.EmployeeMaster;
                    validloginuser.ILCMaster = usertyperesponse.doc.ILCMaster;
                    validloginuser.ForcastMaster = usertyperesponse.doc.ForcastMaster;
                    validloginuser.Financials = usertyperesponse.doc.Financials;
                    validloginuser.Reports = usertyperesponse.doc.Reports;
                    validloginuser.LoginMaster = usertyperesponse.doc.LoginMaster;
                    validloginuser.Id = usertyperesponse.doc._id;
                    validloginuser.Rev = usertyperesponse.doc._rev;
                    usertyperesponse.doc.ok = "Success";
                    validloginuser.ok = usertyperesponse.doc.ok;
                    //validloginuser.lateston = usertyperesponse.doc.lateston;
                    return validloginuser;
                }
                else
                {
                    UpdateLoginUsersvalidateResponse validloginuser = new UpdateLoginUsersvalidateResponse();
                    validloginuser.ok = "Wrong Password";
                    return validloginuser;
                }




                //UpdateLoginUsersvalidateResponse validloginuser = new UpdateLoginUsersvalidateResponse();
                //validloginuser.ok = "Invalid user name";
                //return validloginuser;

            //}

        }
        }
}
