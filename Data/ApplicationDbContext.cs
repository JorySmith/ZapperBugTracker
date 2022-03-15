﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZapperBugTracker.Models;

namespace ZapperBugTracker.Data
{
  // Add ZUser type context to IdentityDbContext
  public class ApplicationDbContext : IdentityDbContext<ZUser>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
  }
}