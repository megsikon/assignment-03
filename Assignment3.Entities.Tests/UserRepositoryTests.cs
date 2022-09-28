namespace Assignment3.Entities.Tests;

public class UserRepositoryTests
{

    private readonly KanbanContext _context;
    private readonly UserRepository _repo;

    public UserRepositoryTests() {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Users.AddRange(new User() { Id = 1,  Name = "Jens Larsen", Email = "jenslarsen@hotmail.dk", Tasks = new List<Task>()}, new User() { Id = 2, Name = "Belinda Vinter", Email = "belindavinter@gmail.com", Tasks = new List<Task>()}); 
        context.SaveChanges();

        _context = context;
        _repo = new UserRepository(_context);
    }
/*
    [Fact]
    public void Update_given_non_existing_User_returns_NotFound() => _repo.Update(new UserUpdateDTO(42,"UserName","Test@Email.com")).Should().Be(NotFound); 
    [Fact]
    public void Delete_given_non_existing_UserId_returns_NotFound() => _repo.Delete(42).Should().Be(NotFound);
*/


    public void Dispose()
    {
        _context.Dispose();
    }
}
