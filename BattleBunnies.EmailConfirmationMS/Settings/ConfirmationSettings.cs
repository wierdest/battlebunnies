using System;

namespace BattleBunnies.EmailConfirmationMS.Settings;

public class ConfirmationSettings
{
    public string ConfirmationBaseURL { get; set; } = default!;
    public string CodeSecretKey { get; set; } = default!;
}
