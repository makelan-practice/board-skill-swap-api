namespace SkillSwap.Api.Services;

public class ApiFailureModeService
{
    private int _enabled;

    public bool IsEnabled => _enabled == 1;

    public bool SetEnabled(bool enabled)
    {
        _enabled = enabled ? 1 : 0;
        return IsEnabled;
    }
}
