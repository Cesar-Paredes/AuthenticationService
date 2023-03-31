using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Models;

public class UserAuthServiceContext : DbContext
{
    public UserAuthServiceContext(DbContextOptions<UserAuthServiceContext> options)
        : base(options)
    {
    }

    public DbSet<AuthenticationService.Models.AuthUser> AuthUser { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AuthUser>()
            .HasData(
            new AuthUser { Id = 1, Username = "User1", Password = "password1" },
            new AuthUser { Id = 2, Username = "User2", Password = "password2" }
            );
    }


}
