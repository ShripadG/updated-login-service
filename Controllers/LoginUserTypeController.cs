using Microsoft.AspNetCore.Mvc;
using loginservice.Models;
using loginservice.Services;
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
    public class LoginUsersTypeController : Controller
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly ICloudantService _cloudantService;
        private readonly IPostUserLoginTypeProcessor _postUserLoginTypeProcessor;
        private readonly IPutUserLoginTypeProcessor _putUserLoginTypeProcessor;

        public LoginUsersTypeController(HtmlEncoder htmlEncoder, IPostUserLoginTypeProcessor postUserLoginTypeProcessor, IPutUserLoginTypeProcessor putUserLoginTypeProcessor, ICloudantService cloudantService = null)
        {
            _cloudantService = cloudantService;
            _postUserLoginTypeProcessor = postUserLoginTypeProcessor;
            _putUserLoginTypeProcessor = putUserLoginTypeProcessor                                                                                                                  ;
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
                return await _cloudantService.GetAllAsync(DBNames.loginusertype.ToString());
            }
        }

      

        /// <summary>
        /// Get record by ID
        /// </summary>
        /// <param name="id">ID to be selected</param>
        /// <returns>record for the given id</returns>
        [HttpGet("id")]
        public async Task<dynamic> GetByID(string id)
        {
            if (_cloudantService == null)
            {
                return new string[] { "No database connection" };
            }
            else
            {
                return await _cloudantService.GetByIdAsync(id, DBNames.loginusers.ToString());
            }
        }

        /// <summary>
        /// Create a new record
        /// </summary>
        /// <param name="employee">New record to be created</param>
        /// <returns>status of the newly added record</returns>
        [HttpPost]
        public async Task<UpdateLoginUserTypesResponse> Post([FromBody]LoginUserTypeAddRequest loginusertype)
        {
            if (_postUserLoginTypeProcessor != null)
            {
                return await _postUserLoginTypeProcessor.PostNewUserRecord(loginusertype, _cloudantService);
            }
            else
            {
                return new UpdateLoginUserTypesResponse();
            }
        }

        /// <summary>
        /// Update an existing record by giving _id and _rev values
        /// </summary>
        /// <param name="employee">record to be updated for given _id and _rev</param>
        /// <returns>status of the record updated</returns>
        [HttpPut]
        public async Task<dynamic> Update([FromBody]LoginUserType loginusertype)
        {
            if (_postUserLoginTypeProcessor != null)
            {
                return await _putUserLoginTypeProcessor.PutExistingUserRecord(loginusertype, _cloudantService);
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
                return await _cloudantService.DeleteAsync(id, rev, DBNames.loginusertype.ToString());
                //Console.WriteLine("Update RESULT " + response);
                //return new string[] { employee.IBMEmailID, employee._id, employee._rev };
                //return JsonConvert.DeserializeObject<UpdateEmployeeResponse>(response.Result);
            }
            else
            {
                return new string[] { "No database connection" };
            }
        }

         /// <summary>
        /// This method bulk uploads the given data from json file into cloudant
        /// </summary>
        /// <returns>status of the bulk upload</returns>
        //[HttpGet("upload")]
        //public async Task<dynamic> BulkUpload()
        //{
        //    if (_cloudantService != null)
        //    {
        //        return await _cloudantService.BulkUpload();
        //    }
        //    else
        //    {
        //        return new string[] { "No database connection" };
        //    }
        //}
    }
}
