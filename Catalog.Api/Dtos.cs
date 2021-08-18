using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Dtos //This type of DTOS declaration will much simplifier than old one. Also we group up all DTOS in one file.
{
    public record ItemDto(Guid Id, string Name, decimal Price, string Description, DateTimeOffset CreatedDate);
    public record CreateItemDto([Required]string Name, [Range(1,1000)]decimal Price, string Description);
    public record UpdateItemDto([Required]string Name, [Range(1,1000)]decimal Price, string Description);
}