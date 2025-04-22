using System.Reflection.Emit;
using CommertialPortal_WebAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommertialPortal_WebAPI.Infrastructure.Data;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Post> Posts { get; set; }
    public DbSet<BusinessBranch> BusinessBranches { get; set; }
    public DbSet<ClientProfile> ClientProfiles { get; set; }
    public DbSet<BusinessProfile> BusinessProfiles { get; set; }
    public DbSet<ClientSubscription> ClientSubscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<BusinessProfile>()
           .HasOne(bp => bp.User)
           .WithOne(u => u.BusinessProfile)
           .HasForeignKey<BusinessProfile>(bp => bp.UserId);

        builder.Entity<ClientProfile>()
            .HasOne(cp => cp.User)
            .WithOne(u => u.ClientProfile)
            .HasForeignKey<ClientProfile>(cp => cp.UserId);

        builder.Entity<ClientProfile>()
            .HasMany(cp => cp.FavouritePosts)
            .WithMany();

        builder.Entity<BusinessProfile>()
        .HasOne(bp => bp.User)
        .WithOne(u => u.BusinessProfile)
        .HasForeignKey<BusinessProfile>(bp => bp.UserId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ClientProfile>()
            .HasOne(cp => cp.User)
            .WithOne(u => u.ClientProfile)
            .HasForeignKey<ClientProfile>(cp => cp.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BusinessBranch>()
            .HasOne(bb => bb.BusinessProfile)
            .WithMany(bp => bp.Branches)
            .HasForeignKey(bb => bb.BusinessProfileId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<Post>()
            .HasOne(p => p.BusinessProfile)
            .WithMany(bp => bp.Posts)
            .HasForeignKey(p => p.BusinessProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PostBusinessBranch>()
            .HasKey(pt => new { pt.PostId, pt.BusinessBranchId });

        builder.Entity<PostBusinessBranch>()
            .HasOne(pt => pt.Post)
            .WithMany(p => p.PostBranches)
            .HasForeignKey(pt => pt.PostId);

        builder.Entity<PostBusinessBranch>()
            .HasOne(pt => pt.BusinessBranch)
            .WithMany(bb => bb.PostBranches)
            .HasForeignKey(pt => pt.BusinessBranchId);

        builder.Entity<ClientSubscription>(entity =>
        {
            entity.HasKey(cs => cs.Id);

            entity.HasOne(cs => cs.ClientProfile)
                .WithMany(cp => cp.Subscriptions)
                .HasForeignKey(cs => cs.ClientProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cs => cs.BusinessProfile)
                .WithMany() 
                .HasForeignKey(cs => cs.BusinessProfileId)
                .OnDelete(DeleteBehavior.Cascade); 

            entity.HasIndex(cs => new { cs.ClientProfileId, cs.BusinessProfileId }).IsUnique();
        });
        
    }
}

