namespace Assignment3.Entities;
using System.ComponentModel.DataAnnotations;
public class User
{
    public virtual int Id{get;set;}
    [StringLength(100)]
    public virtual string Title{get;set;}
    
    [StringLength(100)]
    [Key]
    public virtual string Email{get;set;}

}
