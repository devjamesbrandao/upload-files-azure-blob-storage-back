using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Azure.Core.Interfaces;

namespace Azure.API.Controllers.Base
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        private readonly INotifier _notifier;

        protected MainController(INotifier notifier) => _notifier = notifier;

        protected bool ValidOperation() => !_notifier.HasNotification();

        protected ActionResult CustomResponse(object message = null)
        {
            if(ValidOperation()) return Ok(message);

            return BadRequest(_notifier.GetNotifications().Select(n => n.Message));
        }
    }
}
