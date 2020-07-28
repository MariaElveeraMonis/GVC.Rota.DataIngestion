using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GVC.Rota.Models.Models
{
    public class ScheduleGroupPersonnel
    {
        [Key]
        public string UserId
        {
            get; set;
        }
        public int ShopId { get; set; }
        [NotMapped]
        public string ShopName { get; set; }
        [NotMapped]
        public string UserName { get; set; }
    }
}
