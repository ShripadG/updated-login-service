using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace employeeservice.Common
{
    public enum DBNames
    {
        /// <summary>
        /// Name of the table/doc to store employee master data
        /// </summary>
        [Description("mydb")]
        mydb,

        /// <summary>
        /// Name of the table/doc to store audit data
        /// </summary>
        [Description("auditdata")]
        auditdata,
              /// <summary>
             /// Name of the table/doc to store loginusers data
             /// </summary>
        [Description("loginusers")]
        loginusers,
        /// <summary>
        /// Name of the table/doc to store loginusers data
        /// </summary>
        [Description("W3loginusers")]
        W3loginusers,
        /// <summary>
        /// Name of the table/doc to store loginusers data
        /// </summary>
        [Description("loginusertype")]
        loginusertype,
        /// <summary>
        /// Name of the table/doc to store loginusers data
        /// </summary>
        [Description("userights")]
        userrights
    }
}
