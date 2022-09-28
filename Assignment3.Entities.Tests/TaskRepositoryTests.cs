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


    //CREATE

    [Fact]
    public void Return_Created_Response_When_Creating_A_CreateTaskDTO() {
        //arrange
        var exp = Response.Created;
        _context.Users.Add(new User{Id = 1, Name = "Alice", Email = "al@ice.org"});
        
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

    [Fact]
    public void Create_new_Task_sets_Status_New()
    {
        //Arrange
        var expState = State.New;
        _context.Users.Add(new User{Id = 1, Name = "Alice", Email = "al@ice.org"});

        //Act
        var res = _repo.Create(new TaskCreateDTO(
            "someTitle",
            1,
            "someDescription",
            new List<String> {"one", "two"}
        )); 

        //Assert
        var entity = _context.Tasks.Find(1);
        entity.State.Should().Be(expState); 
    }


    [Fact]
    public void Create_new_Task_sets_Created_to_current_time()
    {
        //Arrange
        var expTime = DateTime.UtcNow;
        _context.Users.Add(new User{Id = 1, Name = "Alice", Email = "al@ice.org"});

        //Act
        var res = _repo.Create(new TaskCreateDTO(
            "someTitle",
            1,
            "someDescription",
            new List<String> {"one", "two"}
        )); 

        //Assert
        var entity = _context.Tasks.Find(1)!;
        entity.Created.Should().BeCloseTo(expTime, precision: TimeSpan.FromSeconds(5));
    }


    /// DELETE

    [Fact]
    public void Delete_given_non_existing_TaskId_returns_NotFound() => _repo.Delete(42).Should().Be(NotFound);

    [Fact]
    public void Delete_given_existing_Task_with_Status_New_deletes_Task()
    {
        //Arrange
         _context.Tasks.Add(new Task{Id = 42, Title = "SomeTitle", Description = "Empty Description", State = State.New});
        
        //Act
        var response = _repo.Delete(42);

        //Assert
        response.Should().Be(Deleted);
        var entity = _context.Tasks.Find(42);
        entity.Should().BeNull();

    }

    [Fact]
    public void Delete_given_existing_Task_with_Status_Active_should_change_Status_to_Removed()
    {
        //Arrange
        _context.Tasks.Add(new Task{Id = 42, Title = "SomeTitle", Description = "Empty Description", State = State.Active});
         var exp = State.Removed; 
        
        //Act
        _repo.Delete(42);

        //Assert
        var entity = _context.Tasks.Find(42)!; 
        entity.State.Should().Be(exp); 
        _context.Tasks.Find(42).Should().NotBeNull();
    }

    [Fact]
    public void Delete_given_existing_Task_with_Status_Resolved_should_return_Conflict()
    {
        //Arrange
        _context.Tasks.Add(new Task{Id = 42, Title = "SomeTitle", Description = "Empty Description", State = State.Resolved});
        
        //Act
        var response = _repo.Delete(42);

        //Assert
        response.Should().Be(Conflict);
        _context.Tasks.Find(42).Should().NotBeNull();
    }

#region Update
    [Fact]
    public void Update_State_of_existing_Task_sets_StateChanged_to_current_time()
    {
        //Arrange
        var expTime = DateTime.UtcNow;
        _context.Tasks.Add(new Task{Id = 2, Title = "SomeTitle", Description = "Empty Description", State = State.Resolved});

        _repo.Update(new TaskUpdateDTO(
            2,
            "NewTitle",
            2,
            "someDescription",
            new List<String> {"one", "two"},
            State.Active
            ));

        //Assert
        var entity = _context.Tasks.Find(2)!;
        entity.StateUpdated.Should().BeCloseTo(expTime, precision: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Update_given_non_existing_Task_returns_NotFound() => _repo.Update(new TaskUpdateDTO(42,"SomeTitle",42,"Empty Description",new List<string> {"one", "two"},State.Active)).Should().Be(NotFound);

#endregion
    
    public void Dispose()
    {
        _context.Dispose();
    }
}
