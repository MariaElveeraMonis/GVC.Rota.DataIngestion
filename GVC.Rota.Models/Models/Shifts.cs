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
        public string ShopId { get; set; }
        public string ShiftName { get; set; }
        public string ShiftNote { get; set; }
        public DateTime ShiftStartDate { get; set; }
        public DateTime ShiftEndDate { get; set; }
        public bool IsShared { get; set; }
        public int Theme { get; set; }
        public bool IsPublished { get; set; }
        public int ShiftCount { get; set; }
        [NotMapped]
        public List<Activity> Shift { get; set; }

    }

}
