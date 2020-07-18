using GVC.Rota.Models;
using GVC.Rota.Models.Models;
using GVC.Shifts.Repos.RepoInterface;
using LumenWorks.Framework.IO.Csv;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GVC.Shifts.Repos
{
    public class ShiftsRepo : IShiftRepo
    {
        public ShiftsRepo()
        {

        }
        public DataTable GetDatatableFromCSV(string channelUrl)
        {
            try
            {
                channelUrl = channelUrl.Replace('/', '\\');
                using (CsvReader csv = new CsvReader(new StreamReader(channelUrl), true))
                {
                    DataTable tablecsv = new DataTable();
                    int fieldCount = csv.FieldCount;
                    string[] headers = csv.GetFieldHeaders();
                    foreach (var header in headers)
                    {
                        tablecsv.Columns.Add(header);
                    }
                    foreach (var row in csv)
                    {
                        tablecsv.Rows.Add();
                        int count = 0;
                        foreach (string FileRec in row)
                        {
                            tablecsv.Rows[tablecsv.Rows.Count - 1][count] = FileRec;

                            count++;
                            Console.WriteLine(tablecsv.Rows.ToString());
                        }
                    }
                    return tablecsv;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<int> InsertIntoChannel(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                int rowCount = 0;
                var channelParameters = new List<Channels>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int j = 0;
                    channelParameters.Add(new Channels()
                    {
                        ChannelId = 0,
                        DisplayName = dataTable.Rows[i][j].ToString(),
                        Description = dataTable.Rows[i][j+1].ToString(),
                        TeamId = dataTable.Rows[i][j+2].ToString()
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var chan in channelParameters)
                    {
                        await dbContext.AddAsync(chan);
                        await dbContext.SaveChangesAsync();
                        rowCount++;
                    }
                }
                return rowCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> InsertIntoGroup(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                int rowCount = 0;
                var GroupParameters = new List<Groups>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int j = 0;
                    GroupParameters.Add(new Groups()
                    {
                        GroupId = 0,
                        GroupDisplayName = dataTable.Rows[i][j].ToString(),
                        GroupDescription = dataTable.Rows[i][j+1].ToString(),
                        MailEnabled = Boolean.Parse(dataTable.Rows[i][j+2].ToString()),
                        MailNickname = dataTable.Rows[i][j+3].ToString(),

                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var group in GroupParameters)
                    {
                        await dbContext.AddAsync(group);
                        await dbContext.SaveChangesAsync();
                        rowCount++;
                    }
                }
                return rowCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> InsertIntoScheduler(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                int rowCount = 0;
                var schedulerParameters = new List<Scheduler>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int j = 0;
                    schedulerParameters.Add(new Scheduler()
                    {
                        SchedulerId = 0,
                        TeamId = dataTable.Rows[i][j].ToString(),
                        ShopName = dataTable.Rows[i][j+1].ToString(),
                        IsActive = Boolean.Parse(dataTable.Rows[i][j+2].ToString())

                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var scheduler in schedulerParameters)
                    {
                        await dbContext.AddAsync(scheduler);
                        await dbContext.SaveChangesAsync();
                        rowCount++;
                    }
                }
                return rowCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> InsertIntoShift(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                int rowCount = 0;
                var shiftParameters = new List<GVC.Rota.Models.Shifts>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int j = 0;
                    shiftParameters.Add(new GVC.Rota.Models.Shifts()
                    {
                        ShiftsId = 0,
                        PersonnelId = dataTable.Rows[i][j].ToString(),
                        ShopId = dataTable.Rows[i][j+1].ToString(),
                        AreaId = dataTable.Rows[i][j+2].ToString(),
                        SharedShift = new SharedShift()
                        {
                            SharedShiftId = 0,
                            SharedShiftName = dataTable.Rows[i][j+3].ToString(),
                            SharedShiftNote = dataTable.Rows[i][j+4].ToString(),
                            StartDateTime = DateTime.Parse(dataTable.Rows[i][j+5].ToString()),
                            EndDateTime = DateTime.Parse(dataTable.Rows[i][j+6].ToString()),
                            Theme = int.Parse(dataTable.Rows[i][j + 7].ToString())

                        },
                        DraftShift = new DraftShift()
                        {
                            DraftShiftId = 0,
                            DraftShiftName = dataTable.Rows[i][j + 8].ToString(),
                            DraftShiftNote = dataTable.Rows[i][j + 9].ToString(),
                            StartDateTime = DateTime.Parse(dataTable.Rows[i][j + 10].ToString()),
                            EndDateTime = DateTime.Parse(dataTable.Rows[i][j + 11].ToString()),
                            Theme = int.Parse(dataTable.Rows[i][j + 12].ToString())
                        }

                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var shift in shiftParameters)
                    {
                        shift.SharedshiftId = await InsertIntoSharedShift(shift.SharedShift, serviceScopeFactory);
                        shift.DraftshiftId = await InsertIntoDraftShift(shift.DraftShift, serviceScopeFactory);
                        await dbContext.AddAsync(shift);
                        await dbContext.SaveChangesAsync();
                        rowCount++;
                    }
                }
                return rowCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<int> InsertIntoSharedShift(SharedShift SharedShift, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    SharedShift.SharedShiftId = 0;
                    await dbContext.AddAsync(SharedShift);
                    await dbContext.SaveChangesAsync();
                    return SharedShift.SharedShiftId;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            
        }
        public async Task<int> InsertIntoDraftShift(DraftShift draftShift, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    draftShift.DraftShiftId = 0;
                    await dbContext.AddAsync(draftShift);
                    await dbContext.SaveChangesAsync();
                    return draftShift.DraftShiftId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public async Task<int> InsertIntoUser(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {

            try
            {
                int rowCount = 0;
                var UserParameters = new List<Users>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var j = 0;
                    UserParameters.Add(new Users()
                    {
                        UsersId = 0,
                        Id = dataTable.Rows[i][j].ToString(),
                        GivenName = dataTable.Rows[i][j+1].ToString(),
                        Surname = dataTable.Rows[i][j+2].ToString(),
                        UserPrincipalName = dataTable.Rows[i][j+3].ToString(),
                        Email = dataTable.Rows[i][j+4].ToString(),

                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var user in UserParameters)
                    {
                        await dbContext.AddAsync(user);
                        await dbContext.SaveChangesAsync();
                        rowCount++;
                    }
                }
                return rowCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<int> InsertIntoContract(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {

            try
            {
                int rowCount = 0;
                var ContractsParameters = new List<Contracts>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var j = 0;
                    ContractsParameters.Add(new Contracts()
                    {
                        ContractsId = 0,
                        UserId = dataTable.Rows[i][j].ToString(),
                        WorkingHours = int.Parse(dataTable.Rows[i][j + 1].ToString())

                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var Contract in ContractsParameters)
                    {
                        await dbContext.AddAsync(Contract);
                        await dbContext.SaveChangesAsync();
                        rowCount++;
                    }
                }
                return rowCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<int> InsertIntoLeave(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {

            try
            {
                int rowCount = 0;
                var ContractsParameters = new List<Leaves>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var j = 0;
                    ContractsParameters.Add(new Leaves()
                    {
                        LeavesId = 0,
                        LeaveType = dataTable.Rows[i][j].ToString(),
                        FromDate = DateTime.Parse(dataTable.Rows[i][j+1].ToString()),
                        ToDate = DateTime.Parse(dataTable.Rows[i][j + 2].ToString()),
                        NoOfHours = int.Parse(dataTable.Rows[i][j + 3].ToString()),
                        NoOfDays = int.Parse(dataTable.Rows[i][j + 4].ToString())

                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var Contract in ContractsParameters)
                    {
                        await dbContext.AddAsync(Contract);
                        await dbContext.SaveChangesAsync();
                        rowCount++;
                    }
                }
                return rowCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
