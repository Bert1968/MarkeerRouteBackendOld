namespace MarkeerRouteBackend.Models
{
    public class KlokPartijLijst
    {
        public List<KlokPartij> KlokPartijen { get; set; }

        public string KlokNummer { get; set; }

        public int GemiddeldeTijdPerPartij { get; set; }
    }
}
