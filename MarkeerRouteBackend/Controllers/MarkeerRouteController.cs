using MarkeerRouteBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarkeerRouteBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarkeerRouteController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<MarkeerRouteController> _logger;

        public MarkeerRouteController(ILogger<MarkeerRouteController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetVeilVolgorde")]
        public ActuelePartijInfo Get()
        {
            Guid GemarkeerdePartijId = Guid.NewGuid();
            return new ActuelePartijInfo
            {
                GemarkeerdePartijen = new List<GesorteerdeGemarkeerdePartij>
                {
                    new GesorteerdeGemarkeerdePartij()
                    {
                        KlokNummer = "C02",
                        KlokPartijId = GemarkeerdePartijId,
                        Prioriteit = 1,
                        VeilVolgordeKlok = 0,
                        GeschatteTijdTotOnderKlok = 2
                    }
                },
                KlokAankomendePartijen = new List<KlokPartijLijst>
                {
                    new KlokPartijLijst()
                    {
                        KlokNummer = "C01",
                        GemiddeldeTijdPerPartij = 3,
                        KlokPartijen = new List<KlokPartij>
                        {
                            new KlokPartij
                            {
                                Id = GemarkeerdePartijId,
                                ProductNaam = "Tulp",
                                AanvoerderNaam = "Jip",
                                AantalInPartij = 5,
                                WerkelijkeTijdPartij = 2,
                                VeilVolgorde = 0
                            }
                        }
                    },
                    new KlokPartijLijst()
                    {
                        KlokNummer = "C02",
                        GemiddeldeTijdPerPartij = 3,
                        KlokPartijen = new List<KlokPartij>
                        {
                            new KlokPartij
                            {
                                Id = Guid.NewGuid(),
                                ProductNaam = "Roos",
                                AanvoerderNaam = "Janneke",
                                AantalInPartij = 10,
                                WerkelijkeTijdPartij = 2,
                                VeilVolgorde = 0
                            }
                        }
                    }
                }

            };
            
        }
    }
}
