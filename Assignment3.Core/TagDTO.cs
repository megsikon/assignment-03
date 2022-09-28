namespace Assignment3.Core;

public record TagDTO(int Id, string Name);

public record TagCreateDTO([Required, StringLength(50)]string Name);

public record TagUpdateDTO(int Id, [StringLength(50)]string Name);
