using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        IMemoryCache _memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("[action]")]
        public IActionResult Get()
        {


            return Ok(_memoryCache.Get<string>("name"));

        }
        [HttpGet("[action]")]
        public void Set()
        {


            _memoryCache.Set("name", "emir");
        }
        [HttpGet("[action]")]
        public IActionResult SetandGetAbsoluteSlide()
        {
            //_memoryCache.GetOrCreate("date", entry =>
            //{
            //    //slide edile edile cache süresi uzayacak ama 30 oldugunda daha uzamaz , eğer vermeseydik sonsuza kadar uzardı
            //    entry.AbsoluteExpiration = DateTime.Now.AddSeconds(30);

            //    //slide edile edile cache süresi uzatılıyor
            //    entry.SlidingExpiration = TimeSpan.FromSeconds(5);
            //    DateTime value = DateTime.Now;
            //    return value;

            //});

            _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
            {
                //slide edile  cache süresi uzayacak ama 30 oldugunda daha uzamaz , eğer vermeseydik sonsuza kadar uzardı
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),

                //slide edile edile cache süresi uzatılıyor
                SlidingExpiration = TimeSpan.FromSeconds(5),
            });

            return Ok(_memoryCache.Get<DateTime>("date"));

        }


    }
}
