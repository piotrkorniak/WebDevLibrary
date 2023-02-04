using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers.Abstract
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected int GetCurrentUserId()
        {
            var userIdInString = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdInString == null) throw new Exception("User id claim not found");

            return int.Parse(userIdInString);
        }
    }
}