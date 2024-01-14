using Djinn.Compile.Scopes;
using LLVMSharp;

namespace Djinn.Compile;

public record CompilationContext
{
    public readonly Stack<LLVMValueRef> Stack = new();

    public CompilationContext(
        IScope Scope,
        LLVMModuleRef Module,
        LLVMBuilderRef Builder,
        LLVMContextRef Context,
        LLVMExecutionEngineRef ExecutionEngine
        )
    {
        this.Scope = Scope;
        this.Module = Module;
        this.Builder = Builder;
        this.Context = Context;
        this.ExecutionEngine = ExecutionEngine;
    }

    public IScope Scope { get; init; }
    public LLVMModuleRef Module { get; init; }
    public LLVMBuilderRef Builder { get; init; }
    public LLVMContextRef Context { get; init; }
    public LLVMExecutionEngineRef ExecutionEngine { get; }
}