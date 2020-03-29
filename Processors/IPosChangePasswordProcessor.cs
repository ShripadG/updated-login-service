using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loginservice.Models;
using loginservice.Services;

namespace loginservice.Processors
{
    interface IPosChangePasswordProcessor
    {

        Task<ChangePassword> PostChangePassword(LoginUsersAddRequest loginUserAddRequest, ICloudantService cloudantService);
    }
}
