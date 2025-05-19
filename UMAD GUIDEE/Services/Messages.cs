using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMAD_GUIDEE.Services
{
    public class RefreshMessage(bool value) : ValueChangedMessage<bool>(value)
    {
    }
}
