using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GVC.Rota.Models.Models
{
    public class StoreShifts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [NotMapped]
        public string ShopName { get; set; }
        public int StoreId { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan ShiftStartTime{ get; set; }
        public TimeSpan ShiftEndTime { get; set; }
        public bool IsActive { get; set; }
        public string WorkingHours { get; set; }
    }
}
