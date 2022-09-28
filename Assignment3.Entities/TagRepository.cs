namespace Assignment3.Entities;

public class TagRepository : ITagRepository
{
    private readonly KanbanContext _context;
    public TagRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response response, int TagId) Create(TagCreateDTO tag)
    {
        var entity = _context.Tags.FirstOrDefault(t => t.Name == tag.Name);
        Response res;
        if(entity is null){
            entity = new Tag();
            _context.Tags.Add(entity);
            _context.SaveChanges();
            res = Response.Created;
        }else{
            res = Response.Conflict;
        }
        var created = new TagDTO(entity.Id,entity.Name);
        return (res,created.Id);
    }
    
    public IReadOnlyCollection<TagDTO> ReadAll()
    {
        throw new NotImplementedException();
    }
    
    public TagDTO Read(int tagId)
    {
        throw new NotImplementedException();
    }
    
    public Response Update(TagUpdateDTO tag)
    {
        var entity = _context.Tags.Find(tag.Id);
        Response response;

        if(entity is null){
            response = Response.NotFound;
        }else{
            entity.Name = tag.Name;
            _context.SaveChanges();
            response = Response.Updated;
        }
        return response;
    }

    public Response Delete(int tagId, bool force = false)
    {
        var tag = _context.Tags.Include(t=>t.Id);//mangler noget her 
        Response response;
        if(tag is null){
            response = Response.NotFound;
        } else if(tag.Any()){
            response = Response.Conflict;
        }else{
            _context.Tags.Remove().tag;
            _context.SaveChanges();
            response = Response.Updated;
        }
    }
}
