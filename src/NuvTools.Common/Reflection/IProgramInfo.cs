namespace NuvTools.Common.Reflection;

public interface IProgramInfo
{
    List<string> Components { get; }
    string Description { get; }
    string Name { get; }
    string Version { get; }
}