using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace BooksApi.Tests.Filter;

/// <summary>
/// Minimal in-memory ISession implementation for exercising
/// session-based action filters in unit tests.
/// </summary>
internal sealed class TestSession : ISession
{
    private readonly Dictionary<string, byte[]> _store = new();

    public bool IsAvailable => true;
    public string Id { get; } = Guid.NewGuid().ToString();
    public IEnumerable<string> Keys => _store.Keys;

    public void Clear() => _store.Clear();
    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public void Remove(string key) => _store.Remove(key);
    public void Set(string key, byte[] value) => _store[key] = value;

    public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value) =>
        _store.TryGetValue(key, out value);
}
