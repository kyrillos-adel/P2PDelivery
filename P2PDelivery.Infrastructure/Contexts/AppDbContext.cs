using Microsoft.EntityFrameworkCore;
using P2PDelivery.Domain.Entities;

namespace P2PDelivery.Infrastructure.Contexts;

public class AppDbContext : DbContext
{
    public DbSet<Application> Applications { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<DeliveryRequest> DeliveryRequests { get; set; }
    public DbSet<DeliveryRequestUpdate> DeliveryRequestUpdates { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer("Server=.;Database=P2PDelivery;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=True;");
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>()
            .HasOne(c => c.UserA)
            .WithMany(u => u.Chats)
            .HasForeignKey(c => c.UserAId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Chat>()
            .HasOne(c => c.UserB)
            .WithMany()
            .HasForeignKey(c => c.UserBId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Payer)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.PayerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Payee)
            .WithMany()
            .HasForeignKey(p => p.PayeeId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Application>()
            .HasOne(a => a.User)
            .WithMany(u => u.Applications)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<DeliveryRequest>()
            .HasOne(dr => dr.User)
            .WithMany(u => u.DeliveryRequests)
            .HasForeignKey(dr => dr.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.DeliveryRequest)
            .WithOne(d => d.Match)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(cm => cm.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(cm => cm.ChatId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(cm => cm.Sender)
            .WithOne()
            .OnDelete(DeleteBehavior.NoAction);
    }
}