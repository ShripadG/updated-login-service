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
    public class UsersRightsController : Controller
    {
        private readonly HtmlEncoder _htmlEncoder;
        private readonly ICloudantService _cloudantService;
        private readonly IPostUserRightsProcessor _postUserRightsProcessor;
        private readonly IPutUserRightsProcessor _putUserRightsProcessor;

        public UsersRightsController(HtmlEncoder htmlEncoder, IPostUserRightsProcessor postUserRightsProcessor, IPutUserRightsProcessor putUserRightsProcessor, ICloudantService cloudantService = null)
        {
            _cloudantService = cloudantService;
            _postUserRightsProcessor = postUserRightsProcessor;
            _putUserRightsProcessor = putUserRightsProcessor;
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
                return await _cloudantService.GetAllAsync(DBNames.userrights.ToString());
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
        /// <param name="userRights">New record to be created</param>
        /// <returns>status of the newly added record</returns>
        [HttpPost]
        public async Task<UpdateUserRightsResponse> Post([FromBody]UserRightsAddRequest userRights)
        {
            if (_postUserRightsProcessor != null)
            {
                return await _postUserRightsProcessor.PostNewUserRecord(userRights, _cloudantService);
            }
            else
            {
                return new UpdateUserRightsResponse();
            }
        }

        /// <summary>
        /// Update an existing record by giving _id and _rev values
        /// </summary>
        /// <param name="userRights">record to be updated for given _id and _rev</param>
        /// <returns>status of the record updated</returns>
        [HttpPut]
        public async Task<dynamic> Update([FromBody]UserRights userRights)
        {
            if (_postUserRightsProcessor != null)
            {
                return await _putUserRightsProcessor.PutExistingUserRecord(userRights, _cloudantService);
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
                return await _cloudantService.DeleteAsync(id, rev, DBNames.userrights.ToString());
                //Console.WriteLine("Update RESULT " + response);
                //return new string[] { employee.IBMEmailID, employee._id, employee._rev };
                //return JsonConvert.DeserializeObject<UpdateEmployeeResponse>(response.Result);
            }
            else
            {
                return new string[] { "No database connection" };
            }
        }

        
    }
}
