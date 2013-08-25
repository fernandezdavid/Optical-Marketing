using System.Collections.Generic;

namespace OMKT.Business
{
    public class Game : Advert
    {
        public ICollection<GameDetail> GameDetails { get; set; }

        public int Oportunities { get; set; }

        public string VideoURL { get; set; }

        public Game()
        {
            GameDetails = new List<GameDetail>();
        }
    }
}