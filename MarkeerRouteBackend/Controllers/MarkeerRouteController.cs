using MarkeerRouteBackend.Data;
using MarkeerRouteBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarkeerRouteBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarkeerRouteController : ControllerBase
    {
        private readonly ILogger<MarkeerRouteController> _logger;

        private const int _veilLoopTijd = 180;
        private DummyDataRepository _repo;

        public MarkeerRouteController(ILogger<MarkeerRouteController> logger)
        {
            _logger = logger;
            _repo = new DummyDataRepository();
            _repo.CreateRepository();


        }

        [HttpGet(Name = "GetVeilVolgorde")]
        public ActuelePartijInfo Get()
        {
            int timestamp = (DateTime.Now.Minute * 60 + DateTime.Now.Second) % _veilLoopTijd;
            _logger.LogInformation(DateTime.Now.TimeOfDay + " , " + timestamp);

            Guid GemarkeerdePartijId = Guid.NewGuid();
            var partijInfo =  new ActuelePartijInfo
            {
                DebugTijd = DateTime.Now,
                DebugTimestamp = timestamp,
                KlokAankomendePartijen = new List<KlokPartijLijst>
                {
                    _repo.GetAankomendePartijen("C01",timestamp, 6),
                    _repo.GetAankomendePartijen("C02",timestamp, 5)
                }

            };

            partijInfo.GemarkeerdePartijen = _repo.GetAankomendeGemarkeerdePartijen(timestamp, partijInfo.KlokAankomendePartijen);
            return partijInfo;


        }
    }
}
