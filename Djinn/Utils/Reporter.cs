using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Djinn.Utils;

public class Reporter
{
    private readonly List<Diagnostic> _diagnostics;

    public Reporter(List<Diagnostic> diagnostics)
    {
        _diagnostics = diagnostics;
    }

    public Reporter()
    {
        _diagnostics = new List<Diagnostic>();
    }

    public IReadOnlyList<Diagnostic> Diagnostics => _diagnostics.AsReadOnly();

    [return: NotNullIfNotNull(nameof(defaultValue))]
    public T? Error<T>(string message, T? defaultValue = default(T))
    {
        Diagnostic(message, DiagnosticSeverity.Error);
        return defaultValue;
    }

    public void Diagnostic(string message, DiagnosticSeverity severity)
    {
        Debug.Fail(message);
        _diagnostics.Add(new Diagnostic(new Position(), message, severity));
    }
}