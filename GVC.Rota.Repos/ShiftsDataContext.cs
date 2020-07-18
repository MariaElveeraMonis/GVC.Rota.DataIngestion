﻿using GVC.Rota.Models;
using GVC.Rota.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GVC.Shifts.Repos
{
    public class ShiftsDataContext: DbContext
    {
        public ShiftsDataContext(DbContextOptions<ShiftsDataContext> options) : base(options)
        {
        }
        public DbSet<Channels> Channels { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<Scheduler> Scheduler { get; set; }

        public DbSet<GVC.Rota.Models.Shifts> Shifts { get; set; }
        public DbSet<SharedShift> SharedShift { get; set; }
        public DbSet<DraftShift> DraftShift { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Contracts> Contracts { get; set; }
        public DbSet<Leaves> Leaves { get; set; }
        
    }
}
