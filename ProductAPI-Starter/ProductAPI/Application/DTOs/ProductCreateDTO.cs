namespace ProductAPI.Application.DTOs
{
    public record ProductCreateDTO(string Name, string Description, decimal Price, int Stock);
}
