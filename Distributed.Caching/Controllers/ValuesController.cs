using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Distributed.Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        readonly IDistributedCache _distributedCache;

        public ValuesController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        [HttpGet("set")]
        public async Task<IActionResult> Set(string name, string surname)
        {
            //await _distributedCache.SetStringAsync("namekey", name);
            //await _distributedCache.SetAsync("surnamekey", Encoding.UTF8.GetBytes(surname));

            await _distributedCache.SetStringAsync("namekey", name, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });

            await _distributedCache.SetAsync(
                key: "namekey",
                value: Encoding.UTF8.GetBytes(surname),
                options: new()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(5)
                });


            return Ok();

        }
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            var surname = await _distributedCache.GetStringAsync("surname");
            var nameBinary = await _distributedCache.GetAsync("name");
            var name = Encoding.UTF8.GetString(nameBinary);

            return Ok(name + surname);

        }

        [NonAction]
        public void TryRedis()
        {
            using var connection = ConnectionMultiplexer.Connect("");
            var db = connection.GetDatabase();



        }


    }
}
