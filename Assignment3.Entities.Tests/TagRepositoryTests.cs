namespace Assignment3.Entities.Tests;

public class TagRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repo;
public TagRepositoryTests() {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        context.Tags.Add(new Tag(){Id = 1,Name="TagNameHere",Tasks = new List<Task>()});

        context.SaveChanges();
        _context = context;
        _repo = new TagRepository(_context);

    }

    [Fact]
    public void Update_given_non_existing_Tag_returns_NotFound() => _repo.Update(new TagUpdateDTO(42,"TagName")).Should().Be(NotFound);

    [Fact]
    public void Delete_given_non_existing_TagId_returns_NotFound() => _repo.Delete(42).Should().Be(NotFound);

    [Fact]
    public void First_Test_Name_Here(){
        // var(response,created) = _repo.Create(new TagCreateDTO(""));
        // response.Should().Be(Created);
        // created.Should().Be();
    }
       [Fact]
    public void Second_Test_Name_Here(){
       


    }
       [Fact]
    public void Third_Test_Name_Here(){
        

    }


    public void Dispose()
    {
        _context.Dispose();
    }
}
