namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repo;
    
    public TaskRepositoryTests() {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();

        _context = context;
        _repo = new TaskRepository(_context);

    }

    [Fact]
    public void Update_given_non_existing_Task_returns_NotFound() => _repo.Update(new TaskUpdateDTO(42,"SomeTitle",42,"Empty Description",new List<string> {"one", "two"},State.Active)).Should().Be(NotFound);

    [Fact]
    public void Delete_given_non_existing_TaskId_returns_NotFound() => _repo.Delete(42).Should().Be(NotFound);

    [Fact]
    public void Test1() {
        //arrange
        var exp = Response.Created;
        _context.Users.Add(new User{Id = 1, Name = "Alice"});
        
        //act
        var res = _repo.Create(new TaskCreateDTO(
            "title",
            1,
            "description",
            new List<string> {"one", "two"}
        ));

        //assert
        res.Response.Should().Be(exp);

    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
