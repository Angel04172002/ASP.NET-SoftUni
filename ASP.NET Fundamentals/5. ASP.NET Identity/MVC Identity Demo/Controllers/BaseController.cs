using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp.Net_Identity.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {

    }
}
