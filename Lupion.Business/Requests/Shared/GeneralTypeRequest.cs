namespace Lupion.Business.Requests.Shared
{
    public class GeneralTypeRequest
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}
