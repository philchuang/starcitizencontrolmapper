using System.Text;
using SSCM.Core;

namespace SSCM.StarCitizen;

public class MappingReporter : IMappingReporter<SCMappingData>
{
    private const string INPUT_HEADER = @"Id,Type,Name,Preserve,SettingNames";
    private const string MAPPING_HEADER = @"Group,Action,Preserve,InputType,Binding,Options";

    public MappingReporter()
    {
    }

    public string Report(SCMappingData data, bool preservedOnly)
    {
        var sb = new StringBuilder();

        ReportInputs(data, preservedOnly, sb);
        ReportMappings(data, preservedOnly, sb);

        return sb.ToString();
    }

    public string ReportInputs(SCMappingData data, bool preservedOnly)
    {
        var sb = new StringBuilder();
        ReportInputs(data, preservedOnly, sb);
        return sb.ToString();
    }

    private static void ReportInputs(SCMappingData data, bool preservedOnly, StringBuilder sb)
    {
        if (!data.Inputs.Any()) return;
        
        if (sb.Length > 0)
        {
            sb.AppendLine("\n");
        }

        sb.AppendLine(INPUT_HEADER);

        foreach (var input in data.Inputs.Where(i => !preservedOnly || i.Preserve))
        {
            WriteInput(input, sb);
        }
    }

    private static void WriteInput(SCInputDevice input, StringBuilder sb)
    {
        sb.Append($"{input.Id},{input.Type},{input.Product},{input.Preserve},");
        if (input.Settings.Any()) sb.Append($"\"{string.Join(", ", input.Settings.Select(s => s.Name).OrderBy(s => s))}\"");
        sb.AppendLine();
    }

    public string ReportMappings(SCMappingData data, bool preservedOnly)
    {
        var sb = new StringBuilder();
        ReportMappings(data, preservedOnly, sb);
        return sb.ToString();
    }

    private static void ReportMappings(SCMappingData data, bool preservedOnly, StringBuilder sb)
    {
        if (!data.Mappings.Any()) return;
        
        if (sb.Length > 0)
        {
            sb.AppendLine("\n");
        }

        sb.AppendLine(MAPPING_HEADER);

        foreach (var mapping in data.Mappings.Where(m => !preservedOnly || m.Preserve))
        {
            WriteMapping(mapping, sb);
        }
    }

    private static void WriteMapping(SCMapping mapping, StringBuilder sb)
    {
        sb.Append($"{mapping.ActionMap},{mapping.Action},{mapping.Preserve},{mapping.InputType},{mapping.Input},");
        if (mapping.MultiTap != null) sb.Append($"\"MultiTap: {mapping.MultiTap}\"");
        sb.AppendLine();
    }
}