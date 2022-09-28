namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private readonly KanbanContext _context;
    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var taskTagsList = task.Tags.ToList();
        var tagList = new List<Tag>();
        foreach (var t in taskTagsList)
        {
            tagList.Add(new Tag{Name = t});
        }
        var user = _context.Users.Find(task.AssignedToId);
        if (user == null)
        {
            return (Response.BadRequest, 0);
        }
        var entity = new Task {
            Title = task.Title,
            AssignedTo = user,
            Description = task.Description,
            Created = System.DateTime.UtcNow,
            StateUpdated = System.DateTime.UtcNow,
            Tags = tagList,
        }; 
        _context.Tasks.Add(entity);
        _context.SaveChanges();
        return (Response.Created, entity.Id);
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        throw new NotImplementedException();
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        throw new NotImplementedException();
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        throw new NotImplementedException();
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        throw new NotImplementedException();
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        throw new NotImplementedException();
    }
    
    public TaskDetailsDTO Read(int taskId)
    {
        throw new NotImplementedException();
    }
    
    public Response Update(TaskUpdateDTO task)
    {
        throw new NotImplementedException();
    }
    
    public Response Delete(int taskId)
    {
        throw new NotImplementedException();
    }
    
}
