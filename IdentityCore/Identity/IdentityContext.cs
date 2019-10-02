using System;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class IdentityContext :
    IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(b =>
        {
            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            //// Each User can have many UserLogins
            //b.HasMany(e => e.Logins)
            //    .WithOne()
            //    .HasForeignKey(ul => ul.UserId)
            //    .IsRequired();

            //// Each User can have many UserTokens
            //b.HasMany(e => e.Tokens)
            //    .WithOne()
            //    .HasForeignKey(ut => ut.UserId)
            //    .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        modelBuilder.Entity<ApplicationRole>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        });

        //Custom Table Mappings..

        //modelBuilder.Entity<IdentityUser>(b =>
        //{
        //    b.ToTable("Users");
        //});

        //modelBuilder.Entity<IdentityUserClaim<string>>(b =>
        //{
        //    b.ToTable("MyUserClaims");
        //});

        //modelBuilder.Entity<IdentityUserLogin<string>>(b =>
        //{
        //    b.ToTable("MyUserLogins");
        //    b.HasKey(x => x.UserId);
        //});

        //modelBuilder.Entity<IdentityUserToken<string>>(b =>
        //{
        //    b.HasKey(x => new { x.UserId });
        //    b.ToTable("MyUserTokens");
        //});

        //modelBuilder.Entity<IdentityRole>(b =>
        //{
        //    b.ToTable("MyRoles");
        //});

        //modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
        //{
        //    b.ToTable("MyRoleClaims");
        //});

        modelBuilder.Entity<IdentityUserRole<string>>(b =>
        {
            b.HasKey(x => new { x.RoleId, x.UserId });
            b.ToTable("MyUserRoles");
        });

        //Validation
        modelBuilder.Entity<IdentityUser>(b =>
        {
            b.Property(u => u.UserName).HasMaxLength(128);
            b.Property(u => u.NormalizedUserName).HasMaxLength(128);
            b.Property(u => u.Email).HasMaxLength(128);
            b.Property(u => u.NormalizedEmail).HasMaxLength(128);
        });

        //modelBuilder.Entity<IdentityUserToken<string>>(b =>
        //{
        //    b.Property(t => t.LoginProvider).HasMaxLength(128);
        //    b.Property(t => t.Name).HasMaxLength(128);
        //});
    }
}