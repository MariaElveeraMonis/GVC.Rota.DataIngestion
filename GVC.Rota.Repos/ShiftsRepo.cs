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
                        GivenName = dataTable.Rows[i][j + 1].ToString(),
                        Surname = dataTable.Rows[i][j + 2].ToString(),
                        UserPrincipalName = dataTable.Rows[i][j + 3].ToString(),
                        Email = dataTable.Rows[i][j + 4].ToString(),
                        Member = new Members()
                        {
                            UserId = dataTable.Rows[i][j + 5].ToString(),
                            Locations = dataTable.Rows[i][j + 6].ToString(),
                            IsOwner = Boolean.Parse(dataTable.Rows[i][j + 7].ToString())
                        }
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var user in UserParameters)
                    {
                        var result = await dbContext.AddAsync(user);
                        if (result.State == EntityState.Added)
                        {

                            InsertIntoMembers(user.Member, serviceScopeFactory);
                        }
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
        private void InsertIntoMembers(Members members, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    dbContext.AddAsync(members);
                    dbContext.SaveChangesAsync();
                }
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
                var LeavesParameters = new List<Leaves>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var j = 0;
                    LeavesParameters.Add(new Leaves()
                    {
                        LeavesId = 0,
                        LeaveType = dataTable.Rows[i][j].ToString(),
                        FromDate = DateTime.Parse(dataTable.Rows[i][j + 1].ToString()),
                        ToDate = DateTime.Parse(dataTable.Rows[i][j + 2].ToString()),
                        NoOfHours = int.Parse(dataTable.Rows[i][j + 3].ToString()),
                        NoOfDays = int.Parse(dataTable.Rows[i][j + 4].ToString()),
                        UserName = dataTable.Rows[i][j+5].ToString()
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var leave in LeavesParameters)
                    {
                        var userDetails = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.GivenName == leave.UserName);
                        if (userDetails != null)
                        {
                            leave.UserId = userDetails.Id;
                        }
                        await dbContext.AddAsync(leave);
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
        public async Task<int> InsertIntoHoliday(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                int rowCount = 0;
                var holidayParameters = new List<Holiday>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var j = 0;
                    holidayParameters.Add(new Holiday()
                    {
                        Id = 0,
                        HolidayDate = DateTime.Parse(dataTable.Rows[i][j].ToString()),
                        HolidayType = dataTable.Rows[i][j + 1].ToString(),
                        IsActive = true
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var holiday in holidayParameters)
                    {
                        await dbContext.AddAsync(holiday);
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
        public async Task<int> InsertIntoStoreShift(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                int rowCount = 0;
                var storeShiftParameters = new List<StoreShifts>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var j = 0;
                    storeShiftParameters.Add(new StoreShifts()
                    {
                        Id = 0,
                        ShopName = dataTable.Rows[i][j].ToString(),
                        ShiftName = dataTable.Rows[i][j+1].ToString(),
                        ShiftStartTime = DateTime.Parse(dataTable.Rows[i][j + 2].ToString()).TimeOfDay,
                        ShiftEndTime = DateTime.Parse(dataTable.Rows[i][j + 3].ToString()).TimeOfDay,
                        IsActive = Boolean.Parse(dataTable.Rows[i][j+4].ToString()),
                        WorkingHours = dataTable.Rows[i][j+5].ToString()
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var storeShift in storeShiftParameters)
                    {
                        var storeDetails = (await dbContext.Scheduler.AsNoTracking().FirstOrDefaultAsync(x => x.ShopName == storeShift.ShopName));
                        if(storeDetails != null)
                        {
                            storeShift.StoreId = storeDetails.SchedulerId;
                        }
                        
                        await dbContext.AddAsync(storeShift);
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
        public async Task<List<Dictionary<int, string>>> InsertIntoScheduler(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                List<Dictionary<int, string>> result = new List<Dictionary<int, string>>();
                var schedulerParameters = new List<Scheduler>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int j = 0;
                    schedulerParameters.Add(new Scheduler()
                    {
                        SchedulerId = 0,
                        ShopName = dataTable.Rows[i][j + 0].ToString(),
                        IsActive = Boolean.Parse(dataTable.Rows[i][j + 1].ToString()),
                        MailNickName = dataTable.Rows[i][j + 2].ToString()

                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var count = 0;
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var scheduler in schedulerParameters)
                    {
                        count++;
                        var availableLocation = await dbContext.Locations.AsNoTracking().FirstOrDefaultAsync(x => x.MailNickName == scheduler.MailNickName);
                        if (availableLocation != null)
                        {
                            if (availableLocation.MailNickName == scheduler.MailNickName)
                            {
                                await dbContext.AddAsync(scheduler);
                                await dbContext.SaveChangesAsync();
                                result.Add(new Dictionary<int, string> { { count, "successful" } });
                            }
                            else
                            {
                                result.Add(new Dictionary<int, string> { { count, "Failed, MailNickName not available in Location Table" } });
                            }
                        }
                        else
                        {
                            result.Add(new Dictionary<int, string> { { count, "Failed, MailNickName not available in Location Table" } });
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
        public async Task<int> InsertIntoScheduleGroupPersonnel(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {

            try
            {
                int rowCount = 0;
                var scheduleGroups = new List<ScheduleGroupPersonnel>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var j = 0;
                    scheduleGroups.Add(new ScheduleGroupPersonnel()
                    {
                        UserName = dataTable.Rows[i][j].ToString(),
                        ShopName = dataTable.Rows[i][j + 1].ToString()
                        
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var scheduleGroup in scheduleGroups)
                    {
                        var UserDetails = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.GivenName == scheduleGroup.UserName);
                        if (UserDetails != null)
                        {
                            scheduleGroup.UserId = UserDetails.Id;
                        }
                            var scheduler = (await dbContext.Scheduler.AsNoTracking().FirstOrDefaultAsync(x => x.ShopName == scheduleGroup.ShopName));
                        if (scheduler != null)
                        {
                            scheduleGroup.ShopId = scheduler.SchedulerId;
                        }
                        await dbContext.AddAsync(scheduleGroup);
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
                        UserName = dataTable.Rows[i][j].ToString(),
                        WorkingHours = int.Parse(dataTable.Rows[i][j + 1].ToString()),
                        WorkingDays = int.Parse(dataTable.Rows[i][j + 2].ToString())
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var contract in ContractsParameters)
                    {
                        var userDetails = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.GivenName == contract.UserName);
                        if(userDetails != null)
                        {
                            contract.UserId = userDetails.Id;
                        }
                        await dbContext.AddAsync(contract);
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
        public async Task<int> InsertIntoContractDetails(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {

            try
            {
                int rowCount = 0;
                var ContractDetailsParameters = new List<ContactDetails>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var j = 0;
                    ContractDetailsParameters.Add(new ContactDetails()
                    {
                        CDId = 0,
                        UserName = dataTable.Rows[i][j].ToString(),
                        WeekDay = dataTable.Rows[i][j + 1].ToString(),
                        Working = int.Parse(dataTable.Rows[i][j + 2].ToString()),
                        TradingStartTimes = (DateTime.Parse(dataTable.Rows[i][j + 3].ToString())).TimeOfDay,
                        TradingEndTimes = (DateTime.Parse(dataTable.Rows[i][j + 4].ToString())).TimeOfDay,
                        ELPW = int.Parse(dataTable.Rows[i][j + 2].ToString())
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var contractdetail in ContractDetailsParameters)
                    {
                        var UserDetails = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.GivenName == contractdetail.UserName);
                        if (UserDetails != null)
                        {
                            var contractValue = await dbContext.Contracts.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == UserDetails.Id);
                            if (contractValue != null)
                            {
                                contractdetail.ContractId = contractValue.ContractsId;
                            }
                        }
                        await dbContext.AddAsync(contractdetail);
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
                        MailNickName = dataTable.Rows[i][j + 3].ToString(),
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
                        var availableLocationRecord = await dbContext.Locations.AsNoTracking().FirstOrDefaultAsync(x => x.MailNickName == location.MailNickName);
                        if (availableLocationRecord == null)
                        { 
                            location.LocationId = 0;
                            await dbContext.AddAsync(location);
                            await dbContext.SaveChangesAsync();
                            result.Add(new Dictionary<int, string> { { count, "successful" } });
                        }
                        else 
                        {
                            if (availableLocationRecord.MailNickName != location.MailNickName)
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
        public async Task<List<Dictionary<int, string>>> InsertIntoShift(DataTable dataTable, IServiceScopeFactory serviceScopeFactory)
        {
            try
            {
                List<Dictionary<int, string>> result = new List<Dictionary<int, string>>();
                var shiftParameters = new List<GVC.Rota.Models.Shifts>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    int j = 0;
                    shiftParameters.Add(new GVC.Rota.Models.Shifts()
                    {
                        ShiftsId = 0,
                        PersonnelId = dataTable.Rows[i][j].ToString(),
                        ShopId = dataTable.Rows[i][j+1].ToString(),
                        ShiftName = dataTable.Rows[i][j+2].ToString(),
                        ShiftNote = dataTable.Rows[i][j+3].ToString(),
                        ShiftStartDate = DateTime.Parse(dataTable.Rows[i][j + 4].ToString()),
                        ShiftEndDate = DateTime.Parse(dataTable.Rows[i][j + 5].ToString()),
                        IsShared = Boolean.Parse(dataTable.Rows[i][j + 6].ToString()),
                        IsPublished = Boolean.Parse(dataTable.Rows[i][j + 7].ToString()),
                        Theme = int.Parse(dataTable.Rows[i][j + 8].ToString()),
                        ShiftCount = int.Parse(dataTable.Rows[i][j + 9].ToString()),
                        Shift = new List<Activity>(){ new Activity() { isPaid = Boolean.Parse(dataTable.Rows[i][j+10].ToString()),
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
                                                    new Activity() { isPaid = Boolean.Parse(dataTable.Rows[i][j+25].ToString()),
                                                                        startDateTime = DateTime.Parse(dataTable.Rows[i][j+26].ToString()),
                                                                        endDateTime = DateTime.Parse(dataTable.Rows[i][j+27].ToString()),
                                                                        activityName = dataTable.Rows[i][j+28].ToString(),
                                                                        theme = int.Parse(dataTable.Rows[i][j+29].ToString())},
                        }
                    });
                }

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    int count = 0;
                    var dbContext = scope.ServiceProvider.GetService<ShiftsDataContext>();
                    foreach (var shift in shiftParameters)
                    {
                        count++;
                        await dbContext.AddAsync(shift);
                        await dbContext.SaveChangesAsync();
                        result.Add(new Dictionary<int, string> { { count, "successful" } });
                        foreach (var act in shift.Shift)
                        {
                            act.ShiftId = shift.ShiftsId;
                            await InsertIntoSharedShift(act, serviceScopeFactory);
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
        

        
    }
}
