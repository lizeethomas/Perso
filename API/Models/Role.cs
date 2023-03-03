using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite.Models;

[Table("roles")]
public class Role
{
    [Column("id")]
    public int Id { get; set; }

    [Column("role")]
    public string? RoleUser { get; set; }
}