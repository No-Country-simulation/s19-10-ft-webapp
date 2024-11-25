using NotebookLM.Domain.Enums;

namespace NotebookLM.Domain.Utilities;

public static class RoleExtensions
{
    public static string ToStringEnum(this Role role)
    {
        return role switch
        {
            Role.Admin => "Admin",
            Role.User => "User",
            _ => role.ToString()
        };
    }
}
