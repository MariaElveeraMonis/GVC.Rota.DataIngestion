using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GVC.Rota.Models
{
    public class Channels
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChannelId
        {
            get; set;
        }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string TeamId { get; set; }
    }
}
