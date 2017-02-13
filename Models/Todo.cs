using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiAngularStorm.Models
{
  public class Todo
  {
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long Id { get; set; }
    public string Description { get; set; }
    public bool Completed { get; set; }
    public string User { get; set; }
  }
}