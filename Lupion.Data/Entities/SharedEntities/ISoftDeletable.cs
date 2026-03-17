namespace Lupion.Data.Entities.SharedEntities;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}
