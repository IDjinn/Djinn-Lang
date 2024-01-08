using Djinn.Compile.Scopes;
using LLVMSharp;

namespace Djinn.Compile;

public record CompilationContext
{
    public readonly Stack<LLVMValueRef> Stack = new();

    public CompilationContext(IScope Scope,
        LLVMModuleRef Module,
        LLVMBuilderRef Builder,
        LLVMContextRef Context)
    {
        this.Scope = Scope;
        this.Module = Module;
        this.Builder = Builder;
        this.Context = Context;
    }

    public IScope Scope { get; init; }
    public LLVMModuleRef Module { get; init; }
    public LLVMBuilderRef Builder { get; init; }
    public LLVMContextRef Context { get; init; }

    public void Deconstruct(out IScope Scope, out LLVMModuleRef Module, out LLVMBuilderRef Builder, out LLVMContextRef Context)
    {
        Scope = this.Scope;
        Module = this.Module;
        Builder = this.Builder;
        Context = this.Context;
    }
}