﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bo;

namespace Dal
{
    public class Context : DbContext, IDbContext
    {
        public DbSet<Event> Events
        {
            get; set;
        }

        public DbSet<Picture> Pictures
        {
            get; set;
        }

        public DbSet<Thema> Themas
        {
            get; set;
        }

        public DbSet<User> Users
        {
            get; set;
        }
    }
}