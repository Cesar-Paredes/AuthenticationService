using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Models;

public class CSRAgentAuthServiceContext : DbContext
{
    public CSRAgentAuthServiceContext(DbContextOptions<CSRAgentAuthServiceContext> options)
        : base(options)
    {
    }

    public DbSet<AuthenticationService.Models.CSRAgent> CSRAgent { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //no need for seed data
        //modelBuilder.Entity<CSRAgent>()
        //    .HasData(
        //    new CSRAgent { Id = 1, Username = "User1", Password = "password1" },
        //    new CSRAgent { Id = 2, Username = "User2", Password = "password2" }
        //    );
    }
}
