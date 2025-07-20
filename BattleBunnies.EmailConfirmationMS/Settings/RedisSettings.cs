using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleBunnies.EmailConfirmationMS.Settings;

public class RedisSettings
{
    public string ConnectionString { get; set; } = default!;
    public int Db { get; set; } = 0;
    public int TtlMinutes { get; set; } = 10;
}
