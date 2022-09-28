namespace Assignment3.Core;

public record UserDTO(int Id, string Name, string Email);

public record UserCreateDTO([Required, StringLength(100)]string Name, [Required, EmailAddress, StringLength(100)]string Email);

public record UserUpdateDTO(int Id, [Required, StringLength(100)]string Name, [Required, EmailAddress, StringLength(100)]string Email);
