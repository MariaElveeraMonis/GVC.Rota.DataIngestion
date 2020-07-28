using System;
using System.IO;
using System.Security.Policy;
using System.Threading.Tasks;
using GVC.Shifts.Repos.RepoInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace GVC.Shifts.API.Controllers
{
    /// <summary>
    /// Shifts Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        readonly IShiftRepo _shiftsRepo;

        /// <summary>
        /// Shifts Controller
        /// </summary>
        /// <param name="shiftsRepo"></param>
        /// <param name="serviceScopeFactory"></param>
        public ShiftsController(IShiftRepo shiftsRepo, IServiceScopeFactory serviceScopeFactory)
        {
            _shiftsRepo = shiftsRepo;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Create Users Record in Users Table from CSV file
        /// the CSV file should contain the following:
        /// Id,GivenName,SurName,UserPrincipalName,Email,Locations,IsOwner
        /// </summary>
        /// <param name="Users_FilePath"></param>
        /// <returns></returns>
        [HttpPost("Users")]
        public IActionResult CreateUsersRecord(string Users_FilePath)
        {
            try
            {
                var csvResult = _shiftsRepo.GetDatatableFromCSV(Users_FilePath);
                var count = _shiftsRepo.InsertIntoUser(csvResult, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create Leaves Record in Leaves Table from CSV file
        /// the CSV file should contain the following:
        /// LeaveType,FromDate,ToDate,NoOfHours,NoOfDays
        /// </summary>
        /// <param name="Leaves_FilePath"></param>
        /// <returns></returns>
        [HttpPost("Leaves")]
        public IActionResult CreateLeavesRecord(string Leaves_FilePath)
        {
            try
            {
                var csvResult = _shiftsRepo.GetDatatableFromCSV(Leaves_FilePath);
                var count = _shiftsRepo.InsertIntoLeave(csvResult, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create Holiday Record in Holiday Table from CSV file
        /// the CSV file should contain the following:
        /// HolidayDate,HolidayType
        /// </summary>
        /// <param name="Holiday_FilePath"></param>
        /// <returns></returns>
        [HttpPost("Holiday")]
        public IActionResult CreateHolidayRecord(string Holiday_FilePath)
        {
            try
            {
                var csvResult = _shiftsRepo.GetDatatableFromCSV(Holiday_FilePath);
                var count = _shiftsRepo.InsertIntoHoliday(csvResult, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create StoreShifts Record in StoreShifts Table from CSV file
        /// the CSV file should contain the following:
        /// Shop Name,ShiftType,ShiftStartTime,ShiftEndTime,IsActive,WorkingHours
        /// </summary>
        /// <param name="StoreShifts_FilePath"></param>
        /// <returns></returns>
        [HttpPost("StoreShifts")]
        public IActionResult CreateStoreShiftsRecord(string StoreShifts_FilePath)
        {
            try
            {
                var csvResult = _shiftsRepo.GetDatatableFromCSV(StoreShifts_FilePath);
                var count = _shiftsRepo.InsertIntoStoreShift(csvResult, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create Scheduler Record in Scheduler Table from CSV file.
        ///  the CSV file should contain the following:
        /// ShopName, IsActive, MailNickName
        /// </summary>
        /// <param name="SchedulerFilePath"></param>
        /// <returns></returns>
        [HttpPost("Scheduler")]
        public IActionResult CreateSchedulerRecord(string SchedulerFilePath)
        {
            try
            {
                var csvResult = _shiftsRepo.GetDatatableFromCSV(SchedulerFilePath);
                var count = _shiftsRepo.InsertIntoScheduler(csvResult, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create ScheduleGroupPersonnel Record in ScheduleGroupPersonnel Table from CSV file.
        ///  the CSV file should contain the following:
        /// UserId,ShopName
        /// </summary>
        /// <param name="SchedulerGroupPersonnel_FilePath"></param>
        /// <returns></returns>
        [HttpPost("SchedulerGroupPersonnel")]
        public IActionResult CreateSchedulerGroupPersonnelRecord(string SchedulerGroupPersonnel_FilePath)
        {
            try
            {
                var csvResult = _shiftsRepo.GetDatatableFromCSV(SchedulerGroupPersonnel_FilePath);
                var count = _shiftsRepo.InsertIntoScheduleGroupPersonnel(csvResult, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create Contract Record in Contract Table from CSV file
        /// the CSV file should contain the following:
        /// UserId,WorkingHours,WorkingDays
        /// </summary>
        /// <param name="Contract_FilePath"></param>
        /// <returns></returns>
        [HttpPost("Contracts")]
        public IActionResult CreateContractRecord(string Contract_FilePath)
        {
            try
            {
                var csvResult = _shiftsRepo.GetDatatableFromCSV(Contract_FilePath);
                var count = _shiftsRepo.InsertIntoContract(csvResult, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create ContractDetails Record in ContractDetails Table from CSV file
        /// the CSV file should contain the following:
        /// UserId,WeekDay,IsWorking,StartTime,EndTime,ELPW
        /// </summary>
        /// <param name="ContractDetails_FilePath"></param>
        /// <returns></returns>
        [HttpPost("ContractDetails")]
        public IActionResult CreateContractDetailsRecord(string ContractDetails_FilePath)
        {
            try
            {
                var csvResult = _shiftsRepo.GetDatatableFromCSV(ContractDetails_FilePath);
                var count = _shiftsRepo.InsertIntoContractDetails(csvResult, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// Create Channel Record in Channels Table  from CSV file.
        /// the CSV file should contain the following:
        /// DisplayName, Description, TeamId
        /// </summary>
        /// <param name="channel_FilePath"></param>
        /// <returns></returns>
        [HttpPost("Channel")]
        public IActionResult CreateChannelRecord(string channel_FilePath)
        {
            try
            {
                var result = _shiftsRepo.GetDatatableFromCSV(channel_FilePath);
                var count = _shiftsRepo.InsertIntoChannel(result, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create Location Record in Locations Table from CSV file. 
        /// the CSV file should contain the following:
        /// LocationDisplayName, LocationDescription, MainEnabled, MailNickName,IsPublished,IsShiftLinked
        /// </summary>
        /// <param name="Location_FilePath"></param>
        /// <returns></returns>
        [HttpPost("CreateLocation")]
        public IActionResult CreateLocationRecord(string Location_FilePath)
        {
            try
            {
                var result = _shiftsRepo.GetDatatableFromCSV(Location_FilePath);
                var count = _shiftsRepo.InsertIntoLocation(result, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Create Shifts Record in Shifts Table from CSV file
        ///  the CSV file should contain the following:
        ///PersonnelId,ShopId,
        ///ShiftName,ShiftNote,ShiftStartDate,ShiftEndDate,IsShared,IsPublished,Theme,
        ///Activity1IsPaid,Activity1StartDateTime,Activity1EndDateTime,Activity1Name,Activity1Theme,
        ///Activity2IsPaid,Activity2StartDateTime,Activity2EndDateTime,Activity2Name,Activity2Theme,
        ///Activity3IsPaid,Activity3StartDateTime,Activity3EndDateTime,Activity3Name,Activity3Theme,
        ///Activity4IsPaid,Activity4StartDateTime,Activity4EndDateTime,Activity4Name,Activity4Theme
        ///  </summary>
        /// <param name="Shifts_FilePath"></param>
        /// <returns></returns>
        [HttpPost("Shift")]
        public IActionResult CreateShiftsecord(string Shifts_FilePath)
        {
            try
            {
                var csvResult = _shiftsRepo.GetDatatableFromCSV(Shifts_FilePath);
                
                var count = _shiftsRepo.InsertIntoShift(csvResult, serviceScopeFactory);
                return Ok(count.Result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        

        
    }
}