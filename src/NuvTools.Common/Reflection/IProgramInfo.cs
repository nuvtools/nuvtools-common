namespace NuvTools.Common.Reflection;

public interface IProgramInfo
{
    string Name { get; }
    string? Description { get; }
    
    string? Version { get; }
    List<string>? Components { get; }
}