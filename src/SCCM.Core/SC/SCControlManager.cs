namespace SCCM.Core.SC;

// TODO write tests for this class

public class ControlManager : ControlManagerBase
{
    protected override string GameConfigPath => System.IO.Path.Combine(this.GameConfigLocation, Constants.SC_ACTIONMAPS_XML_NAME);
    protected override string MappingDataSavePath => System.IO.Path.Combine(this.AppSaveLocation, Constants.SCCM_SCMAPPINGS_JSON_NAME);

    public override string GameType => "Star Citizen";

    private readonly ISCFolders _folders;

    public ControlManager(IPlatform platform, ISCFolders folders) : base(platform)
    {
        this._folders = folders;
        Initialize();
    }

    private void Initialize()
    {
        this.GameConfigLocation = this._folders.ActionMapsDir;
        this.AppSaveLocation = this._folders.SccmDir;
    }

    protected override MappingImporter CreateImporter()
    {
        var importer = new MappingImporter(this.Platform, this.GameConfigPath);
        importer.StandardOutput += WriteLineStandard;
        importer.WarningOutput += WriteLineWarning;
        importer.DebugOutput += WriteLineDebug;
        return importer;
    }

    protected override MappingImportMerger CreateMerger()
    {
        var merger = new MappingImportMerger();
        merger.StandardOutput += WriteLineStandard;
        merger.WarningOutput += WriteLineWarning;
        merger.DebugOutput += WriteLineDebug;
        return merger;
    }

    protected override MappingExporter CreateExporter()
    {
        var exporter = new MappingExporter(this.Platform, this._folders, GameConfigPath);
        exporter.StandardOutput += WriteLineStandard;
        exporter.WarningOutput += WriteLineWarning;
        exporter.DebugOutput += WriteLineDebug;
        return exporter;
    }
}