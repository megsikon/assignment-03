namespace Assignment3.Entities;

using System.ComponentModel.DataAnnotations;
public class Task
{
    public int Id{get;set;}

    [StringLength(100)]
    public string Title{get;set;}
    public User AssignedTo{get;set;}
    public string Description {get;set;}
    

}
