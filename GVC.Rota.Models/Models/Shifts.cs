using GVC.Rota.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GVC.Rota.Models
{
    public class Shifts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftsId
        {
            get; set;
        }
        public string PersonnelId { get; set; }
        public string ShiftName { get; set; }
        public string ShiftNote { get; set; }
        public DateTime ShiftStartDate { get; set; }
        public DateTime ShiftEndDate { get; set; }
        [NotMapped]
        public List<Activity> Shift { get; set; }

    }
    public class Activity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityId
        {
            get; set;
        }
        public bool isPaid { get; set; }
        public DateTime startDateTime { get; set; }
        public DateTime endDateTime { get; set; }
        public string code { get; set; }
        public string activityName { get; set; }
        public int theme { get; set; }
        public int ShiftId { get; set; }

    }

}
