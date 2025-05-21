using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybridisms.Client.Native.Services;

public class RemoteAvailabilityWatcher
{
    private static readonly TimeSpan MinRetryTime = TimeSpan.FromSeconds(30);

    DateTimeOffset lastFailTime = DateTimeOffset.MinValue;

    public TimeSpan LastUnavailable => DateTimeOffset.UtcNow - lastFailTime;

    public bool IsRemoteAvailable => LastUnavailable < MinRetryTime;

    public void MarkRemoteUnavailable()
    {
        lastFailTime = DateTimeOffset.UtcNow;
    }

    public void MarkRemoteAvailable()
    {
        lastFailTime = DateTimeOffset.MinValue;
    }
}
