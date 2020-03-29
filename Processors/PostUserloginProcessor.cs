using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loginservice.Models;
using loginservice.Services;
using Newtonsoft.Json;
using employeeservice.Common;

namespace loginservice.Processors
{
    public class PostuserLoginProcessor : IPostUserLoginProcessor
    {
        public async Task<UpdateLoginUsersResponse> PostNewUserRecord(LoginUsersAddRequest loginuserAddRequest, ICloudantService cloudantService = null)
        {
            //AuditData auditData = new AuditData();
            //auditData.eventname = "Add";
            //auditData.loginid = loginuserAddRequest.loginid;
            //auditData.datetime = System.DateTime.UtcNow.ToString();
            //auditData.empid = employeeAddRequest.FormattedEmployeedId;

            if (cloudantService != null)
            {
                var response = await cloudantService.CreateAsync(loginuserAddRequest, DBNames.loginusers.ToString());
               // var audit = await cloudantService.CreateAsync(auditData, DBNames.auditdata.ToString());
                return JsonConvert.DeserializeObject<UpdateLoginUsersResponse>(response);
            }
            else
            {
                return new UpdateLoginUsersResponse();
            }
        }
    }
}
