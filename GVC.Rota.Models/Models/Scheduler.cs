using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVC.Rota.Models
{
    public class Scheduler
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SchedulerId
        {
            get; set;
        }
        public string ShopName { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public string MailNickName { get; set; }
    }
}
