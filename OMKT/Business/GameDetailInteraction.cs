using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OMKT.Business
{
    public class GameDetailInteraction
    {
        public int GameDetailInteractionId { get; set; }

        public int GameDetailID { get; set; }

        public virtual GameDetail GameDetail { get; set; }

        public Boolean Win { get; set; }

    }
}