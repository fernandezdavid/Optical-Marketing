﻿using System;
using System.Collections.Generic;

namespace OMKT.Business
{
    public class AdvertCampaignDetailInteraction
    {
        public int AdvertCampaignDetailInteractionID { get; set; }

        public int AdvertID { get; set; }

        public virtual Advert Advert { get; set; }

        public double Height { get; set; }

        public DateTime? StartDatetime { get; set; }

        public DateTime? EndDatetime { get; set; }

    }
}