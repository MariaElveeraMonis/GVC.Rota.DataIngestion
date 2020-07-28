using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GVC.Rota.Models.Models
{
    public class Members
    {
        [Key]
        public string UserId { get; set; }
        public string Locations { get; set; }
        public bool IsOwner { get; set; }
    }
}
