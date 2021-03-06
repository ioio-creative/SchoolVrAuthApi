﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolVrAuthApi.Exceptions;
using SchoolVrAuthApi.Models;
using SchoolVrAuthApi.Utilities;
using System;
using System.Threading.Tasks;

namespace SchoolVrAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContext;        

        // https://stackoverflow.com/questions/51116403/how-to-get-client-ip-address-in-asp-net-core-2-1/51245326
        public ErrorController(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;            
        }

        [Route("")]
        public async Task<IActionResult> DefaultErrorHandler()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
            {
                string emailNotificationErrMsg = "";

                try
                {
                    await ExceptionUtils.NotifySystemOps(exceptionHandlerPathFeature.Error,
                        _httpContext);
                }
                catch (Exception exc)
                {
                    // silence error
                    emailNotificationErrMsg = exc.ToString();
                }

                if (exceptionHandlerPathFeature.Error is ModelStateInvalidException)
                {
                    return BadRequest((exceptionHandlerPathFeature.Error as ModelStateInvalidException).ModelState);
                }

                if (exceptionHandlerPathFeature.Error is AuthCodeFailException)
                {
                    return Unauthorized();
                }

                if (exceptionHandlerPathFeature.Error is AuthenticateLicenseFailException)
                {
                    AuthResponseBody authResponseBody = (exceptionHandlerPathFeature.Error as AuthenticateLicenseFailException).AuthResponse;
                    // for testing sending email at production server
                    authResponseBody.EmailNotificationErrMsg = emailNotificationErrMsg;
                    return Ok(authResponseBody);
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}