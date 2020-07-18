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
        public string AreaId { get; set; }
        public int SharedshiftId { get; set; }
        public int DraftshiftId { get; set; }
        [NotMapped]
        public SharedShift SharedShift { get; set; }
        [NotMapped]
        public DraftShift DraftShift { get; set; }

    }

}
