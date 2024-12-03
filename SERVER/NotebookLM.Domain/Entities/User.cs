using Microsoft.AspNetCore.Identity;

namespace NotebookLM.Domain.Entities;

public class User : IdentityUser<int>
{
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public virtual ICollection<File> Files { get; set; } = new List<File>();

}
