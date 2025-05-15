namespace ProductAPI.Application.DTOs
{
    public record ProductResponseDTO(int Id, string Name, string Description, decimal Price, int Stock);
}
