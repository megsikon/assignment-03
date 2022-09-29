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
        var user = _context.Users.Find(task.AssignedToId);
        if (user == null)
            {
                return (Response.BadRequest, 0);
            }
        
        var taskTagsList = task.Tags.ToList();
        var tagList = new List<Tag>();
        foreach (var t in taskTagsList)
        {
            tagList.Add(new Tag{Name = t});
        }

        var entity = _context.Tasks.FirstOrDefault(t => t.Title == task.Title);
        Response response;
        if (entity is null) {
            entity = new Task {
                Title = task.Title,
                AssignedTo = user,
                Description = task.Description,
                Tags = tagList,
                Created = System.DateTime.UtcNow,
                StateUpdated = System.DateTime.UtcNow
            };
            _context.Tasks.Add(entity);
            _context.SaveChanges();
            response = Response.Created;
        } else {
            response = Response.Conflict;
        }

        return (response, entity.Id);
    }
    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var task = from t in _context.Tasks
                  select new TaskDTO(
                      t.Id,
                      t.Title,
                      t.AssignedTo.Name,
                      t.Tags.Select(t => t.Name).ToList(),
                      t.State);
        return task.ToList();
    
    }
    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        var task = from t in _context.Tasks
                  where t.State == Removed
                  select new TaskDTO(
                      t.Id,
                      t.Title,
                      t.AssignedTo.Name,
                      t.Tags.Select(t => t.Name).ToList(),
                      t.State);
        return task.ToList();
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        throw new NotImplementedException();
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var task = from t in _context.Tasks
                  where t.Id == userId
                  select new TaskDTO(
                      t.Id,
                      t.Title,
                      t.AssignedTo.Name,
                      t.Tags.Select(t => t.Name).ToList(),
                      t.State);
        return task.ToList();
    }
    
    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
       var task = from t in _context.Tasks
                  where t.State == state
                  select new TaskDTO(
                      t.Id,
                      t.Title,
                      t.AssignedTo.Name,
                      t.Tags.Select(t => t.Name).ToList(),
                      t.State);
        return task.ToList();
    }
    
    public TaskDetailsDTO Read(int taskId)
    {
        // throw new NotImplementedException();
        var task = from t in _context.Tasks
                    where t.Id == taskId
                    select new TaskDetailsDTO(
                        t.Id,
                        t.Title,
                        t.Description,
                        t.Created,
                        t.AssignedTo.Name,
                        t.Tags.Select(t => t.Name).ToList(),
                        t.State,
                        t.StateUpdated);
        return task.FirstOrDefault()!;
    }
    
    public Response Update(TaskUpdateDTO task)
    {
        var entity = _context.Tasks.Find(task.Id);
        Response response;

        if(entity is null){
            response = Response.NotFound;
        }else if (_context.Tasks.FirstOrDefault(t => t.Id != task.Id && t.Title == task.Title) != null)
        {
            response = Response.Conflict;
        }else{
            entity.Title = task.Title;
            entity.StateUpdated = System.DateTime.UtcNow; 
            
            _context.SaveChanges();
            response = Response.Updated;
        }
        return response;
    }
        
    public Response Delete(int taskId)
    {
        var entity = _context.Tasks.Find(taskId);
        Response response;

        if(entity is null){
            response = Response.NotFound;
        }else if(entity.State == State.New ){
            _context.Tasks.Remove(entity);
            _context.SaveChanges();
            response = Response.Deleted;
        }else if(entity.State == State.Active){
            entity.State = State.Removed;
            _context.SaveChanges();
            response = Response.Updated; 
        }else{
            response = Response.Conflict;
        }
        return response;
    }
    
    
    
}
