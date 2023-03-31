using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Models;

public class AdminAuthServiceContext : DbContext
{
    public AdminAuthServiceContext(DbContextOptions<AdminAuthServiceContext> options)
        : base(options)
    {
    }

    public DbSet<AuthenticationService.Models.Admin> Admin { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //seed data, hardcoded
        modelBuilder.Entity<Admin>()
            .HasData(
            new Admin { Id = 1, Name="Cesar", Username = "admin", Password = "admin" }
            
            );
    }
}
