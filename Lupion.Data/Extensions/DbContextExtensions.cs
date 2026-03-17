using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text;
using System.Text.Json;

namespace Lupion.Data.Extensions;

public static class DbContextExtensions
{
    public static async Task<T> ExecuteJsonAsync<T>(this DbContext context, string sql, params SqlParameter[] parameters)
    {
        await using var conn = context.Database.GetDbConnection() ??
                               context.Database.GetService<IRelationalConnection>().DbConnection;
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        foreach (var p in parameters)
            cmd.Parameters.Add(p);

        using var reader = await cmd.ExecuteReaderAsync();
        var sb = new StringBuilder();

        while (await reader.ReadAsync())
        {
            sb.Append(reader.GetString(0));
        }

        var json = sb.ToString();

        return string.IsNullOrWhiteSpace(json)
            ? Activator.CreateInstance<T>()
            : JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }

}
