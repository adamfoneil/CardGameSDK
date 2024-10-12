using Dapper;
using System.Data;

namespace BlazorApp.Extensions;

internal static class DapperExtensions
{
	internal static async Task<TMessage?> DequeueAsync<TMessage>(this IDbConnection connection, string tableName, string criteria, object? parameters = null)
	{
		string sql = $"DELETE TOP (1) FROM {tableName} WITH (ROWLOCK, READPAST) OUTPUT [deleted].*";
		if (!string.IsNullOrEmpty(criteria)) sql += $" WHERE {criteria}";
		return await connection.QuerySingleOrDefaultAsync<TMessage>(sql, parameters);
	}
}
