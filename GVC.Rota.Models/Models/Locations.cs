using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GVC.Rota.Models
{
    
    public class Locations
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationId
        {
            get; set;
        }
        public string GroupDescription { get; set; }
        public string GroupDisplayName { get; set; }
        public string MailNickName { get; set; }
        public bool MailEnabled { get; set; }
        public bool IsPublished { get; set; }
        public bool IsShiftLinked { get; set; }
    }
}
