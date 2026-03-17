using Microsoft.EntityFrameworkCore;

namespace Lupion.Business.Helpers;

public static class EfExtensions
{
    public static void AttachGraphAsModifiedExistingOnly(this DbContext ctx, object root)
    {
        ctx.ChangeTracker.TrackGraph(root, node =>
        {
            if (node.Entry.IsKeySet)
                node.Entry.State = EntityState.Modified;
            else
                node.Entry.State = EntityState.Unchanged;
        });
    }
}
