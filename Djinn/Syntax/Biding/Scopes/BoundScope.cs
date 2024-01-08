using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Djinn.Syntax.Biding.Scopes.Types;
using Djinn.Syntax.Biding.Scopes.Variables;
using LLVMSharp;
using Microsoft.CodeAnalysis;

namespace Djinn.Syntax.Biding.Scopes;

[DebuggerDisplay("Scope=[{Namespace}]")]
public record BoundScope
{
    public string Namespace { get; private init; }
    public Optional<BoundScope> ParentScope { get; private init; }

    public BoundScope(string nameSpace, Optional<BoundScope> parentScope = default)
    {
        Namespace = GenerateUniqueScopeId(this, nameSpace);
        ParentScope = parentScope;
    }

    private IDictionary<string, BoundType> _types = new ConcurrentDictionary<string, BoundType>();
    private IDictionary<string, BoundVariable> _variables = new ConcurrentDictionary<string, BoundVariable>();

    public ReadOnlyDictionary<string, BoundVariable> Variables => _variables.AsReadOnly();
    public ReadOnlyDictionary<string, BoundType> Types => _types.AsReadOnly();
    public void CreateVariable(BoundVariable boundVariable)
    {
        _variables.Add(boundVariable.Identifier, boundVariable);
    }

    public void CreateType(BoundType boundType)
    {
        _types.Add(boundType.Identifier, boundType);
    }

    public virtual Optional<BoundType> FindType(string identifier)
    {
        if (_types.TryGetValue(identifier, out var type)) return type;

        return ParentScope.HasValue ? ParentScope.Value.FindType(identifier) : default;
    }

    public virtual Optional<BoundVariable> FindVariable(string identifier)
    {
        if (_variables.TryGetValue(identifier, out var variable)) return variable;

        return ParentScope.HasValue ? ParentScope.Value.FindVariable(identifier) : default;
    }

    public static string GenerateUniqueScopeId(BoundScope boundScope, string @namespace)
    {
        return $"__{boundScope.GetType().Name}_{@namespace}";
    }


    // public virtual LLVMValueRef ReadVariable(string identifier)
    // {
    //     // var test =LLVM.BuildAdd(Builder, LLVM.GetParam(function, (uint)0), LLVM.GetParam(function, (uint)1),"test")
    //     var boundVariable = FindVariable(identifier);
    //     if (!boundVariable.HasValue) throw new ArgumentException();
    //
    //     var variable = boundVariable.Value;
    //     
    //     return default;
    // }
}