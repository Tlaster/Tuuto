using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuuto.Common.Notifications
{
    class NotificationRegistry : Registry
    {
        public NotificationRegistry()
        {
            Schedule<NotificationJob>().ToRunNow().AndEvery(30).Seconds();
        }
    }
}
