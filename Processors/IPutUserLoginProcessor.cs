using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loginservice.Models;
using loginservice.Services;

namespace loginservice.Processors
{
    public interface IPutUserRightsProcessor
    {
        Task<UpdateUserRightsResponse> PutExistingUserRecord(UserRights usersRightsUpdateRequest, ICloudantService cloudantService = null);
    }
}
