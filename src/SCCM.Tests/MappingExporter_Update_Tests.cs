using NUnit.Framework;
using SCCM.Core;
using SCCM.Tests.Mocks;
using System.Xml.Linq;
using System.Xml.XPath;
using static SCCM.Tests.Extensions;

namespace SCCM.Tests;

[TestFixture]
public class MappingExporter_Update_Tests
{
    private readonly MappingExporter _updater;
    private readonly IPlatform _platform;
    private readonly IFolders _folders;
    private MappingData _data = new MappingData();
    private XDocument? _originalXml = null;
    private XDocument? _updatedXml = null;
    private XElement? _actionMapsElement = null;
    private XElement? _actionProfilesDefaultElement = null;

    private string GetTestXmlPath()
    {
        return new System.IO.FileInfo(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "actionmaps.xml")).FullName;
    }

    private async Task<XDocument> LoadTestXml()
    {
        using (var fs = new FileStream(this.GetTestXmlPath(), FileMode.Open))
        {
            var ct = new CancellationToken();
            return await XDocument.LoadAsync(fs, LoadOptions.None, ct);
        }
    }


    public MappingExporter_Update_Tests()
    {
        this._platform = new PlatformForTest(DateTime.UtcNow);
        this._folders = new FoldersForTest();
        System.IO.File.Copy(Samples.GetActionMapsXmlPath(), this.GetTestXmlPath(), true);
        this._updater = new MappingExporter(this._platform, this._folders, this.GetTestXmlPath());
    }

    [SetUp]
    protected async Task Init()
    {
        this._originalXml = await this.LoadTestXml();
    }

    private async Task Act()
    {
        await this._updater.Update(this._data);
        this._updatedXml = await this.LoadTestXml();
        if (this._updatedXml != null)
        {
            if (this._updatedXml.Root == null)
            {
                throw new InvalidDataException($"Expecting <ActionMaps>, found nothing!");
            }

            if (!this._updatedXml.Root.Name.LocalName.Equals("ActionMaps"))
            {
                throw new InvalidDataException($"Expecting <ActionMaps>, found <{this._updatedXml.Root.Name.LocalName}>!");
            }

            this._actionMapsElement = this._updatedXml.Root;
            this._actionProfilesDefaultElement = this._actionMapsElement.GetChildren("ActionProfiles").Single(ap => ap.GetAttribute("profileName") == "default");
        }
    }

    private void Arrange_Default_MappingData()
    {
        this._data = new MappingData {
            Inputs = {
                new InputDevice { Type = "keyboard", Instance = 1, Preserve = true, Product = "Keyboard  {6F1D2B61-D5A0-11CF-BFC7-444553540000}" },
                new InputDevice { Type = "gamepad", Instance = 1, Preserve = true, Product = "Controller (Gamepad)", Settings = new InputDeviceSetting[] {
                    new InputDeviceSetting { Name = "flight_view", Preserve = true, Properties = new Dictionary<string, string> { { "exponent", "1" } } }
                } },
                new InputDevice { Type = "joystick", Instance = 1, Preserve = true, Product = " VKB-Sim Gladiator NXT R    {0200231D-0000-0000-0000-504944564944}" , Settings = new InputDeviceSetting[] {
                    new InputDeviceSetting { Name = "flight_move_pitch", Preserve = true, Properties = new Dictionary<string, string>() },
                } },
                new InputDevice { Type = "joystick", Instance = 2, Preserve = true, Product = " VKBsim Gladiator EVO OT  L SEM   {3205231D-0000-0000-0000-504944564944}", Settings = new InputDeviceSetting[] {
                    new InputDeviceSetting { Name = "flight_move_strafe_vertical", Preserve = true, Properties = new Dictionary<string, string> { { "invert", "1" } } },
                    new InputDeviceSetting { Name = "flight_move_strafe_longitudinal", Preserve = true, Properties = new Dictionary<string, string> { { "invert", "1" } } },
                } },
            },
            Mappings = {
                new Mapping { ActionMap = "seat_general", Action = "v_toggle_mining_mode", Input = "js2_button56", Preserve = true },
                new Mapping { ActionMap = "seat_general", Action = "v_toggle_quantum_mode", Input = "js2_button19", Preserve = true },
                new Mapping { ActionMap = "seat_general", Action = "v_toggle_scan_mode", Input = "js2_button54", Preserve = true },
                new Mapping { ActionMap = "spaceship_general", Action = "v_close_all_doors", Input = "js2_button49", Preserve = true },
                new Mapping { ActionMap = "spaceship_general", Action = "v_flightready", Input = "js2_button52", Preserve = true },
                new Mapping { ActionMap = "spaceship_general", Action = "v_lock_all_doors", Input = "js2_button46", Preserve = true },
                new Mapping { ActionMap = "spaceship_general", Action = "v_open_all_doors", Input = "js2_button51", Preserve = true },
                new Mapping { ActionMap = "spaceship_general", Action = "v_toggle_all_doorlocks", Input = "js2_button47", Preserve = true },
                new Mapping { ActionMap = "spaceship_general", Action = "v_toggle_all_doors", Input = "js2_button50", Preserve = true },
                new Mapping { ActionMap = "spaceship_general", Action = "v_unlock_all_doors", Input = "js2_button48", Preserve = true },
                new Mapping { ActionMap = "spaceship_view", Action = "v_view_cycle_fwd", Input = "js2_button1", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_afterburner", Input = "js2_button3", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_atc_request", Input = "js2_button8", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_autoland", Input = "js2_button10", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_ifcs_speed_limiter_reset_scm", Input = "js2_hat1_right", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_ifcs_toggle_cruise_control", Input = "js2_hat1_left", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_ifcs_toggle_vector_decoupling", Input = "js2_button4", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_roll", Input = "js1_x", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_space_brake", Input = "js2_button5", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_speed_range_down", Input = "js2_hat1_down", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_speed_range_up", Input = "js2_hat1_up", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_strafe_lateral", Input = "js2_x", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_strafe_longitudinal", Input = "js2_y", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_strafe_vertical", Input = "js2_rotz", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_toggle_landing_system", Input = "js2_button7", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_toggle_relative_mouse_mode", Input = "kb1_slash", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_toggle_vtol", Input = "js2_button9", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_transform_deploy", Input = "js2_button61", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_transform_retract", Input = "js2_button58", Preserve = true },
                new Mapping { ActionMap = "spaceship_movement", Action = "v_yaw", Input = "js1_rotz", Preserve = true },
            }
        };
    }

    private void AssertBasics()
    {
        Assert.NotNull(this._updatedXml);
        Assert.NotNull(this._actionMapsElement);
        Assert.NotNull(this._actionProfilesDefaultElement);
    }

    private XElement GetActionRebindElement(XDocument xd, Mapping mapping)
    {
        return xd.XPathSelectElements($"/ActionMaps/ActionProfiles[@profileName='default']/actionmap[@name='{mapping.ActionMap}']/action[@name='{mapping.Action}']/rebind").SingleOrDefault();
    }

    [Test]
    public async Task Update_overwrites_mapping_change()
    {
        // Arrange
        this.Arrange_Default_MappingData();
        this._data.Mappings.ToList().ForEach(m => m.Preserve = false);
        var mapping = this._data.Mappings.Single(m => m.ActionMap == "spaceship_movement" && m.Action == "v_ifcs_toggle_cruise_control");
        mapping.Preserve = true;
        var actionRebindElement = this.GetActionRebindElement(this._originalXml, mapping);
        var originalInput = actionRebindElement.GetAttribute("input");
        mapping.Input = mapping.Input == originalInput ? RandomString() : mapping.Input;

        // Act
        await this.Act();

        // Assert
        this.AssertBasics();
        // silly code to prevent warnings
        if (this._updatedXml == null || this._actionMapsElement == null || this._actionProfilesDefaultElement == null) return;

        var changedActionRebindElement = this.GetActionRebindElement(this._updatedXml, mapping);
        Assert.NotNull(changedActionRebindElement, nameof(changedActionRebindElement));
        Assert.AreEqual(mapping.Input, changedActionRebindElement.GetAttribute("input"));
    }

    [Test]
    public async Task Update_ignores_mapping_change()
    {
        // Arrange
        this.Arrange_Default_MappingData();
        this._data.Mappings.ToList().ForEach(m => m.Preserve = false);
        var mapping = this._data.Mappings.Single(m => m.ActionMap == "spaceship_movement" && m.Action == "v_ifcs_toggle_cruise_control");
        mapping.Preserve = false;
        var actionRebindElement = this.GetActionRebindElement(this._originalXml, mapping);
        var originalInput = actionRebindElement.GetAttribute("input");
        mapping.Input = mapping.Input == originalInput ? RandomString() : mapping.Input;

        // Act
        await this.Act();

        // Assert
        this.AssertBasics();
        // silly code to prevent warnings
        if (this._updatedXml == null) return;

        var changedActionRebindElement = this.GetActionRebindElement(this._updatedXml, mapping);
        Assert.NotNull(changedActionRebindElement, nameof(changedActionRebindElement));
        Assert.AreEqual(originalInput, changedActionRebindElement.GetAttribute("input"));
    }

    [Test]
    public async Task Update_adds_actionmap_and_action()
    {
        // Arrange
        this.Arrange_Default_MappingData();
        this._data.Mappings.ToList().ForEach(m => m.Preserve = false);
        var mapping = new Mapping { ActionMap = RandomString(), Action = RandomString(), Input = $"js2_{RandomString()}", Preserve = true };
        this._data.Mappings.Add(mapping);

        // Act
        await this.Act();

        // Assert
        this.AssertBasics();
        // silly code to prevent warnings
        if (this._updatedXml == null || this._originalXml == null) return;

        var originalActionRebindElement = this.GetActionRebindElement(this._originalXml, mapping);
        Assert.IsNull(originalActionRebindElement, nameof(originalActionRebindElement));
        var addedActionRebindElement = this.GetActionRebindElement(this._updatedXml, mapping);
        Assert.NotNull(addedActionRebindElement, nameof(addedActionRebindElement));
        Assert.AreEqual(mapping.Input, addedActionRebindElement.GetAttribute("input"));
    }

    [Test]
    public async Task Update_adds_action()
    {
        // Arrange
        this.Arrange_Default_MappingData();
        this._data.Mappings.ToList().ForEach(m => m.Preserve = false);
        var mapping = new Mapping { ActionMap = this._data.Mappings.First().ActionMap, Action = RandomString(), Input = $"js2_{RandomString()}", Preserve = true };
        this._data.Mappings.Add(mapping);

        // Act
        await this.Act();

        // Assert
        this.AssertBasics();
        // silly code to prevent warnings
        if (this._updatedXml == null || this._originalXml == null) return;

        var originalActionRebindElement = this.GetActionRebindElement(this._originalXml, mapping);
        Assert.IsNull(originalActionRebindElement, nameof(originalActionRebindElement));
        var addedActionRebindElement = this.GetActionRebindElement(this._updatedXml, mapping);
        Assert.NotNull(addedActionRebindElement, nameof(addedActionRebindElement));
        Assert.AreEqual(mapping.Input, addedActionRebindElement.GetAttribute("input"));
    }

    [Test]
    public async Task Update_overwrites_input_setting()
    {
        // Arrange
        this.Arrange_Default_MappingData();

        // Act
        await this.Act();

        // Assert
        this.AssertBasics();
        // silly code to prevent warnings
        if (this._updatedXml == null) return;

        Assert.Fail();
    }

    [Test]
    public async Task Update_ignores_input_setting()
    {
        Assert.Fail();
    }

    [Test]
    public async Task Update_overwrites_inputs()
    {
        Assert.Fail();
    }
}