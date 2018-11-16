﻿using ItsAllAboutTheGame.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItsAllAboutTheGame.Data
{
    public class ItsAllAboutTheGameDbContext : IdentityDbContext<User>
    {
        public ItsAllAboutTheGameDbContext(DbContextOptions<ItsAllAboutTheGameDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
