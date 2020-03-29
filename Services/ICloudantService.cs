using loginservice.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace loginservice.Services
{
    /// <summary>
    /// This is the interface for 
    /// </summary>
    public interface ICloudantService
    {
        /// <summary>
        /// Create a new record
        /// </summary>
        /// <param name="item">the record to be added.</param>
        /// <param name="dbname">the db table to be updated</param>
        /// <returns>returns the status of the add record</returns>
        Task<dynamic> CreateAsync(dynamic item, string dbname);

        /// <summary>
        /// Update given record
        /// </summary>
        /// <param name="item">the record to be updated</param>
        /// <param name="dbname">the db table to be updated</param>
        /// <returns>returns the status of the updated record</returns>
        Task<dynamic> UpdateAsync(dynamic item, string dbname);

        /// <summary>
        /// Returns the list of all records in the database
        /// </summary>
        /// <returns>Returns the list of all records in the database</returns>
        Task<dynamic> GetAllAsync(string dbname);

        /// <summary>
        /// Returns the record for the given id.
        /// </summary>
        /// <param name="id">id of the record to be retrieved</param>
        /// <returns>returns the record for given id</returns>
        Task<dynamic> GetByIdAsync(string id, string dbname);

        /// <summary>
        /// Deletes the record for the given id
        /// </summary>
        /// <param name="id">id of the record to be deleted</param>
        /// <param name="rev">latest revision number of the record to be deleted</param>
        /// <returns>returns </returns>
        Task<dynamic> DeleteAsync(string id, string rev, string dbname);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<dynamic> BulkUpload();
    }
}