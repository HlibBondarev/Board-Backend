using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Board.Common.Services.DTOs;

public record UserFromClaimsDto(
    [Required]
    [MaxLength(64)]
    [property: JsonPropertyName("sub")]
    string Id,

    [Required]
    [EmailAddress]
    string Email,

    [Required]
    [StringLength(20, MinimumLength = 3)]
    string Name
);