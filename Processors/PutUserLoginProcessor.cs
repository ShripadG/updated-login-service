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
    public class PutUserLoginProcessor : IPutUserLoginProcessor
    {
        public async Task<UpdateLoginUsersResponse> PutExistingUserRecord(LoginUsers loginuserUpdateRequest, ICloudantService cloudantService = null)
        {
            //AuditData auditData = new AuditData();
            //auditData.eventname = "edit";
            //auditData.loginid = employeeUpdateRequest.loginid;
            //auditData.datetime = System.DateTime.UtcNow.ToString();
            //auditData.empid = employeeUpdateRequest.FormattedEmployeedId;

            if (cloudantService != null)
            {
                var response = await cloudantService.UpdateAsync(loginuserUpdateRequest, DBNames.loginusers.ToString());
                //var audit = await cloudantService.CreateAsync(auditData, DBNames.auditdata.ToString());
                return JsonConvert.DeserializeObject<UpdateLoginUsersResponse>(response);
            }
            else
            {
                return new UpdateLoginUsersResponse();
            }
        }
    }
}
