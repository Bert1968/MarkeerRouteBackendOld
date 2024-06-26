using MarkeerRouteBackend.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Reflection;

namespace MarkeerRouteBackend.Data
{
    public class DummyDataRepository
    {
        public static List<DummyDataItem> Repository;
        public static List<Guid> GemarkeerdePartijen;



        public List<GesorteerdeGemarkeerdePartij> GetAankomendeGemarkeerdePartijen(int timestamp, List<KlokPartijLijst> partijLijsten)
        {
            var gemarkeerdePartijen = new List<GesorteerdeGemarkeerdePartij>();
            foreach (var klokPartijen in partijLijsten)
            {
                gemarkeerdePartijen.AddRange(
                    klokPartijen.KlokPartijen.Where(p => GemarkeerdePartijen.Contains(p.Id))
                        .Select(p => new GesorteerdeGemarkeerdePartij
                        {
                            KlokNummer = klokPartijen.KlokNummer,
                            KlokPartijId = p.Id,
                            Prioriteit = 1,
                            VeilVolgordeKlok = p.VeilVolgorde,
                            GeschatteTijdTotOnderKlok = p.VeilVolgorde * klokPartijen.GemiddeldeTijdPerPartij
                        }
                 ));
            }
            gemarkeerdePartijen = gemarkeerdePartijen.OrderBy(p => p.GeschatteTijdTotOnderKlok).ToList();
            for(int i = 0; i < gemarkeerdePartijen.Count; i++)
            {
                gemarkeerdePartijen[i].RouteVolgnummer = i;
            }
            return gemarkeerdePartijen;
        }
                


        public List<KlokPartij> GetAankomendePartijen(string klok, int timestamp, int gemiddeldeVertraging)
        {
            var alleKlokPartijen = Repository.Where(p => p.AuctionInformationClockNumber == klok)
                .Select(p => new KlokPartij
                {
                    AantalInPartij = p.CurrentNumberOfPieces,
                    AanvoerderNaam = p.SupplierOrganizationName,
                    Id = p.Id,
                    ProductNaam = p.VbnProductName,
                    WerkelijkeTijdPartij = gemiddeldeVertraging
                });
            int aantalPartijenAlGeweest = timestamp / gemiddeldeVertraging;
            var aankomendeKlokPartijen = alleKlokPartijen.Skip(aantalPartijenAlGeweest).ToList();

            for(int i = 0; i < aankomendeKlokPartijen.Count; i++)
            {
                aankomendeKlokPartijen[i].VeilVolgorde = i;
            }

            return aankomendeKlokPartijen;
        }


        public void CreateRepository()
        {
            string jsonFilePath = "dummyData.json";
            if (Repository == null)
            {
                ProcessDemoDataFile(jsonFilePath);

                GemarkeerdePartijen = GetRandomItems<DummyDataItem>(Repository, 10).Select(p => p.Id).ToList();
            }
        }


        private static List<T> GetRandomItems<T>(List<T> list, int count)
        {
            if (count > list.Count / 2)
            {
                throw new ArgumentException("Number of items too great.");
            }

            Random random = new Random();
            List<T> randomItems = new List<T>();

            for (int i = 0; i < count; i++)
            {
                int index = random.Next(list.Count);
                if (randomItems.Contains(list[index]))
                {
                    count--;
                }
                else
                {
                    randomItems.Add(list[index]);
                }
            }

            return randomItems;
        }
    

    private void ProcessDemoDataFile(string resourceName)
        {
            List<DummyDataItem>? demoClockSupplyLineDtos;
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = assembly.GetManifestResourceNames().Single(resource => resource.EndsWith(resourceName));
            var stream = assembly.GetManifestResourceStream(resourcePath)!;
            using (stream)
            {
                using StreamReader reader = new(stream);
                var result = reader.ReadToEnd();
                Repository = JsonConvert.DeserializeObject<List<DummyDataItem>>(result, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }

        }

        }
}
