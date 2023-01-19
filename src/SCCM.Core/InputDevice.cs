namespace SCCM.Core;

public class InputDevice
{
    public string? Type { get; set; }
    public int Instance { get; set; }
    public string? Product { get; set; }
    public IList<InputDeviceSetting> Settings { get; set; } = new List<InputDeviceSetting>();
}