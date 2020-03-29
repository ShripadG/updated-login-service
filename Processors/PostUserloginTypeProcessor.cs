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
    public class PostUserLoginTypeProcessor : IPostUserLoginTypeProcessor
    {
        public async Task<UpdateLoginUserTypesResponse> PostNewUserRecord(LoginUserTypeAddRequest loginUserTypeAddRequest, ICloudantService cloudantService = null)
        {
            //AuditData auditData = new AuditData();
            //auditData.eventname = "Add";
            //auditData.loginid = loginuserAddRequest.loginid;
            //auditData.datetime = System.DateTime.UtcNow.ToString();
            //auditData.empid = employeeAddRequest.FormattedEmployeedId;

            if (cloudantService != null)
            {
                var response = await cloudantService.CreateAsync(loginUserTypeAddRequest, DBNames.loginusertype.ToString());
               // var audit = await cloudantService.CreateAsync(auditData, DBNames.auditdata.ToString());
                return JsonConvert.DeserializeObject<UpdateLoginUserTypesResponse>(response);
            }
            else
            {
                return new UpdateLoginUserTypesResponse();
            }
        }
    }
}
