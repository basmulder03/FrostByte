namespace FrostByte.Domain.Runs;

public enum RunOutcome
{
    Success,
    CompileError,
    RuntimeError,
    Timeout,
    Cancelled
}
