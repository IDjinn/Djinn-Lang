namespace Djinn.Utils;

public readonly record struct Diagnostic(Position Position, string? Message, DiagnosticSeverity Severity = DiagnosticSeverity.Warning);