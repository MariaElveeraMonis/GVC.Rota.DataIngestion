using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GVC.Rota.Models
{
    
    public class Groups
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupId
        {
            get; set;
        }
        public string GroupDescription { get; set; }
        public string GroupDisplayName { get; set; }
        public string MailNickname { get; set; }
        public bool MailEnabled { get; set; }
    }
}
