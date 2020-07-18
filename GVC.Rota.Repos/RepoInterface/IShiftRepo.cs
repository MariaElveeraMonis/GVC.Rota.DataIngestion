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
        Task<int> InsertIntoChannel(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoGroup(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoScheduler(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoShift(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoUser(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoContract(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
        Task<int> InsertIntoLeave(DataTable dataTable, IServiceScopeFactory serviceScopeFactory);
    }
}
