namespace NuvTools.Common.Reflection;

internal class ProgramInfo : IProgramInfo
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public List<string> Components { get; set; }
}