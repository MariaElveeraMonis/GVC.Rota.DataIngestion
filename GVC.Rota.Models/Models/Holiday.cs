using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GVC.Rota.Models.Models
{
    public class Holiday
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime HolidayDate { get; set; }
        public string HolidayType { get; set; }
        public bool IsActive { get; set; }
    }
}
