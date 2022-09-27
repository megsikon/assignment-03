namespace Assignment3.Entities;

public class KanbanContext : DbContext
{
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Task> Tasks => Set<Task>();
    public DbSet<User> Users => Set<User>();

    public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();

        modelBuilder.Entity<Task>().Property(t => t.State).HasConversion(new EnumToStringConverter<State>());

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
    }
}
