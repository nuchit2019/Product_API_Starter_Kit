namespace ProductAPI.Application.DTOs
{
    public record ProductUpdateDTO(int Id, string Name, string Description, decimal Price, int Stock);
}
