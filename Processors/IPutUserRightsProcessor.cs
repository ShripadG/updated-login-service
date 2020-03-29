using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loginservice.Models;
using loginservice.Services;

namespace loginservice.Processors
{
    public interface IPutUserLoginProcessor
    {
        Task<UpdateLoginUsersResponse> PutExistingUserRecord(LoginUsers loginusersUpdateRequest, ICloudantService cloudantService = null);
    }
}
