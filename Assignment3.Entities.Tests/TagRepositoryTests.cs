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


    //create
    [Fact]
    public void Create_Tag_returns_Created_with_tag(){
        var(response,created) = _repo.Create(new TagCreateDTO("Created Tak"));
        response.Should().Be(Created);
        created.Should().Be(new TagDTO(32,"Created Tag"));


    }
    [Fact]
    public void Create_tag_given_exisiting_tag_returns_Conflict(){
        var(response,tag) = _repo.Create(new TagCreateDTO("TagNameHere"));
        response.Should().Be(Conflict);
        tag.Should().Be(new TagDTO(1,"TagNameHere"));    

    }

    //read
    


    //update
    [Fact]
    public void Update_Tag_returns_Updated_with_tag(){
        var response = _repo.Update(new TagDTO(3,"Updated Tag"));
        response.Should().Be(Updated);
        var entity = _context.Tags.Find(3)!;
        entity.Name.Should().Be("Updated Tag");
    }

    [Fact]
    public void Update_tag_given_exisiting_tag_returns_Conflict(){
        var response = _repo.Update(new TagDTO(1,"ConflictUpdateTag"));
        response.Should().Be(Conflict);
        var entity = _context.Tags.Find(1)!;
        entity.Name.Should().Be("TagNameHere");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
