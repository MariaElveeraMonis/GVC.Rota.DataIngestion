using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace GVC.Shifts.Repos.RepoInterface
{
    public interface IShiftRepo
    {
        DataTable GetDatatableFromCSV(string channelUrl);
        Task<int> InsertIntoUser(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoLeave(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoHoliday(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoStoreShift(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<List<Dictionary<int, string>>> InsertIntoScheduler(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoScheduleGroupPersonnel(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoContract(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoContractDetails(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);

        Task<List<Dictionary<int, string>>> InsertIntoLocation(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoChannel(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<List<Dictionary<int, string>>> InsertIntoShift(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        
    }
}
