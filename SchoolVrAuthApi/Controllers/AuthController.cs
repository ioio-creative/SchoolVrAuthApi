using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolVrAuthApi.Exceptions;
using SchoolVrAuthApi.Models;
using SchoolVrAuthApi.Utilities.DateTimeUtils;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SchoolVrAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string ApiAuthCode = "ZpQrkvmSH5jgfpqPM3F9MCTc";

        private bool IsAuthorised(string authCode)
        {
            return ApiAuthCode == authCode;
        }


        private readonly AuthContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;

        // https://stackoverflow.com/questions/51116403/how-to-get-client-ip-address-in-asp-net-core-2-1/51245326
        public AuthController(AuthContext dbContext, IHttpContextAccessor httpContext)
        {
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        //// GET: api/Auth
        //[HttpGet]
        //public IEnumerable<MacAddress> GetMacAddresses()
        //{
        //    return _dbContext.MacAddresses;
        //}

        //// GET: api/Auth/5
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetMacAddress([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var macAddress = await _dbContext.MacAddresses.FindAsync(id);

        //    if (macAddress == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(macAddress);
        //}

        //// PUT: api/Auth/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMacAddress([FromRoute] string id, [FromBody] MacAddress macAddress)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != macAddress.MacAddressId)
        //    {
        //        return BadRequest();
        //    }

        //    _dbContext.Entry(macAddress).State = EntityState.Modified;

        //    try
        //    {
        //        await _dbContext.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MacAddressExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Auth
        //[HttpPost]
        //public async Task<IActionResult> PostMacAddress([FromBody] MacAddress macAddress)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    _dbContext.MacAddresses.Add(macAddress);
        //    await _dbContext.SaveChangesAsync();

        //    return CreatedAtAction("GetMacAddress", new { id = macAddress.MacAddressId }, macAddress);
        //}

        //// DELETE: api/Auth/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMacAddress([FromRoute] string id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var macAddress = await _dbContext.MacAddresses.FindAsync(id);
        //    if (macAddress == null)
        //    {
        //        return NotFound();
        //    }

        //    _dbContext.MacAddresses.Remove(macAddress);
        //    await _dbContext.SaveChangesAsync();

        //    return Ok(macAddress);
        //}

        private bool MacAddressExists(string id)
        {
            return _dbContext.MacAddresses.Any(e => e.MacAddressId == id);
        }


        // POST: api/[controller]/authenticatelicense
        [HttpPost("authenticatelicense")]
        public async Task<IActionResult> AuthenticateLicense([FromBody][Bind("ApiAuthCode", "LicenseKey", "MacAddress")] AuthRequestBody authRequestBody)
        {
            if (!ModelState.IsValid)
            {
                throw new ModelStateInvalidException(authRequestBody as object, ModelState);                
            }

            if (!IsAuthorised(authRequestBody.ApiAuthCode))
            {
                throw AuthCodeFailException.AuthCodeFailExceptionFactory(authRequestBody.ApiAuthCode);                
            }

            var authResponseBody = new AuthResponseBody
            {
                IsAuthenticated = false
            };

            authRequestBody.MacAddress = NormalizeMacAddress(authRequestBody.MacAddress);

            var macAddressFromDb = await _dbContext.MacAddresses.FindAsync(authRequestBody.MacAddress);
    
            if (macAddressFromDb != null)
            {
                _dbContext.MacAddresses.Remove(macAddressFromDb);
                // save changes so that it won't affect licenseKeyFromDb.NumOfMacAddressesAllowed check later
                await _dbContext.SaveChangesAsync();
            }

            authResponseBody.IsAuthenticated = await AddNewMacAddressAsync(authRequestBody.MacAddress, authRequestBody.LicenseKey);
            await _dbContext.SaveChangesAsync();

            if (!authResponseBody.IsAuthenticated)
            {
                throw new AuthenticateLicenseFailException(authResponseBody);
            }

            return Ok(authResponseBody);
        }

        // https://community.spiceworks.com/topic/932330-mac-address-format-what-s-the-standard-why-don-t-vendors-stick-to-it
        // remove any '-' or ':' or white space from mac address
        // use upper case
        private string NormalizeMacAddress(string macAddress)
        {
            return Regex.Replace(macAddress, @"-|:|\s+", "").ToUpperInvariant();
        }

        // return bool isAuthenticated
        private async Task<bool> AddNewMacAddressAsync(string newMacAddressId, string associatedLicenseKeyId)
        {
            var licenseKeyFromDb = await _dbContext.LicenseKeys.FindAsync(associatedLicenseKeyId);

            if (licenseKeyFromDb == null || !licenseKeyFromDb.IsActive)
            {
                // reject
                return false;
            }

            // check licenseKeyFromDb.NumOfMacAddressesAllowed
            // 0 means unlimited
            if (licenseKeyFromDb.NumOfMacAddressesAllowed != 0)
            {
                var numOfMacAddressesAlreadyLinked = licenseKeyFromDb.MacAddresses.Count;

                if (numOfMacAddressesAlreadyLinked >= licenseKeyFromDb.NumOfMacAddressesAllowed)
                {
                    // reject
                    return false;
                }
            }            

            MacAddress newMacAddress = new MacAddress
            {
                MacAddressId = newMacAddressId,
                //AssignedDt = DateTime.Now,
                AssignedDt = DateTimeWithZone.Now,

                LicenseKeyId = associatedLicenseKeyId,
                LicenseKey = licenseKeyFromDb
            };
            await _dbContext.MacAddresses.AddAsync(newMacAddress);            

            // accept
            return true;
        }
    }
}