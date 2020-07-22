using GVC.Rota.Models;
using GVC.Rota.Models.Models;
using GVC.Shifts.Repos.RepoInterface;
using LumenWorks.Framework.IO.Csv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
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
        public async Task<List<Dictionary<int,string>>> InsertIntoLocation(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                List<Dictionary<int,string>> result = new List<Dictionary<int, string>>();
                var locationParameters = new List<Locations>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int j = 0;
                    locationParameters.Add(new Locations()
                    {
                        LocationId = 0,
                        GroupDisplayName = dataTable.Rows[i][j].ToString(),
                        GroupDescription = dataTable.Rows[i][j + 1].ToString(),
                        MailEnabled = Boolean.Parse(dataTable.Rows[i][j + 2].ToString()),
                        MailNickname = dataTable.Rows[i][j + 3].ToString(),
                        IsPublished = Boolean.Parse(dataTable.Rows[i][j+4].ToString()),
                        IsShiftLinked = Boolean.Parse(dataTable.Rows[i][j+5].ToString())
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    int count = 0;
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var location in locationParameters)
                    {
                        count++;
                        var availableLocationRecord = await dbContext.Locations.AsNoTracking().FirstOrDefaultAsync(x => x.MailNickname == location.MailNickname);
                        if (availableLocationRecord == null)
                        { 
                            location.LocationId = 0;
                            await dbContext.AddAsync(location);
                            await dbContext.SaveChangesAsync();
                            result.Add(new Dictionary<int, string> { { count, "successful" } });
                        }
                        else 
                        {
                            if (availableLocationRecord.MailNickname != location.MailNickname)
                            {
                                await dbContext.AddAsync(location);
                                await dbContext.SaveChangesAsync();
                                result.Add(new Dictionary<int, string> { { count, "successful" } });
                            }
                            else
                            {
                                result.Add(new Dictionary<int, string> { { count, "MailNickName already exists" } });
                            }
                            
                        }
                        
                       
                        
                    }
                }
                return result;
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
                        ShiftName = dataTable.Rows[i][j+1].ToString(),
                        ShiftNote = dataTable.Rows[i][j+2].ToString(),
                        ShiftStartDate = DateTime.Parse(dataTable.Rows[i][j + 3].ToString()),
                        ShiftEndDate = DateTime.Parse(dataTable.Rows[i][j + 4].ToString()),
                        Shift = new List<Activity>(){ new Activity() { isPaid = Boolean.Parse(dataTable.Rows[i][j+5].ToString()),
                                                                        startDateTime = DateTime.Parse(dataTable.Rows[i][j+6].ToString()),
                                                                        endDateTime = DateTime.Parse(dataTable.Rows[i][j+7].ToString()),
                                                                        activityName = dataTable.Rows[i][j+8].ToString(),
                                                                        theme = int.Parse(dataTable.Rows[i][j+9].ToString())},
                                                    new Activity() { isPaid = Boolean.Parse(dataTable.Rows[i][j+10].ToString()),
                                                                        startDateTime = DateTime.Parse(dataTable.Rows[i][j+11].ToString()),
                                                                        endDateTime = DateTime.Parse(dataTable.Rows[i][j+12].ToString()),
                                                                        activityName = dataTable.Rows[i][j+13].ToString(),
                                                                        theme = int.Parse(dataTable.Rows[i][j+14].ToString())},
                                                    new Activity() { isPaid = Boolean.Parse(dataTable.Rows[i][j+15].ToString()),
                                                                        startDateTime = DateTime.Parse(dataTable.Rows[i][j+16].ToString()),
                                                                        endDateTime = DateTime.Parse(dataTable.Rows[i][j+17].ToString()),
                                                                        activityName = dataTable.Rows[i][j+18].ToString(),
                                                                        theme = int.Parse(dataTable.Rows[i][j+19].ToString())},
                                                    new Activity() { isPaid = Boolean.Parse(dataTable.Rows[i][j+20].ToString()),
                                                                        startDateTime = DateTime.Parse(dataTable.Rows[i][j+21].ToString()),
                                                                        endDateTime = DateTime.Parse(dataTable.Rows[i][j+22].ToString()),
                                                                        activityName = dataTable.Rows[i][j+23].ToString(),
                                                                        theme = int.Parse(dataTable.Rows[i][j+24].ToString())},
                        }
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var shift in shiftParameters)
                    {
                        await dbContext.AddAsync(shift);
                        await dbContext.SaveChangesAsync();
                        foreach (var act in shift.Shift)
                        {
                            act.ShiftId = shift.ShiftsId;
                            await InsertIntoSharedShift(act, serviceScopeFactory);
                        }
                        
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
        public async Task<int> InsertIntoSharedShift(Activity activity, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    activity.ActivityId = 0;
                    await dbContext.AddAsync(activity);
                    await dbContext.SaveChangesAsync();
                    return activity.ActivityId;
                }
            }
            catch(Exception ex)
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
