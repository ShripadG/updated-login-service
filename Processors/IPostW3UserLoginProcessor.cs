using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loginservice.Models;
using loginservice.Services;

namespace loginservice.Processors
{
    public interface IPostW3UserLoginProcessor
    {
        Task<UpdateLoginUsersResponse> PostW3NewUserRecord(LoginUsersAddRequest loginUserAddRequest, ICloudantService cloudantService);
    }
}
