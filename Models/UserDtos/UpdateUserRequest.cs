using System.ComponentModel.DataAnnotations;

namespace backend.Models.UserDtos;

public class UpdateUserRequest
{
    [EmailAddress]
    public string? Email { get; set; }

    [StringLength(100, MinimumLength = 2)]
    public string? FirstName { get; set; }

    [StringLength(100, MinimumLength = 2)]
    public string? LastName { get; set; }

    [StringLength(100, MinimumLength = 6)]
    public string? Password { get; set; }
}
