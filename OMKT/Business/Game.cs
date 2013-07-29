using System.Collections.Generic;

namespace OMKT.Business
{
    public class Game : Advert
    {
        public ICollection<GameDetail> GameDetails { get; set; }

        public Game()
        {
            GameDetails = new List<GameDetail>();
        }
    }
}