using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GVC.Rota.Models.Models
{
    public class ContactDetails
    {
        [Key]
        public int CDId
        {
            get; set;
        }
        [NotMapped]
        public string UserId { get; set; }
        [NotMapped]
        public string UserName { get; set; }
        public int ContractId { get; set; }
        public string WeekDay { get; set; }
        public int Working { get; set; }
        public TimeSpan TradingStartTimes { get; set; }
        public TimeSpan TradingEndTimes { get; set; }
        public int ELPW { get; set; }
    }
}
