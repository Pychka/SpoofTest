namespace SpoofTestApi.Models;

public abstract class LoginModel : BaseEntity
{
    public string Login { get; set; } = null!;
}
