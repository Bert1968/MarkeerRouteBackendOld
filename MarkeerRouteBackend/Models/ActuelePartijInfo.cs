
namespace MarkeerRouteBackend.Models
{
    public class ActuelePartijInfo
    {
        public DateTime ATijd { get; set; }
        public int ATimestamp { get; set; }
        public List<KlokPartijLijst> KlokAankomendePartijen { get; set; }

        public List<GesorteerdeGemarkeerdePartij> GemarkeerdePartijen { get; set; }

    }
}
