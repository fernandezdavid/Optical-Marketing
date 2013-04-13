using System;
using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public abstract class Advert
    {
        public int AdvertId { get; set; }

        [Required]
        public string Name { get; set; }

        public int AdvertTypeId { get; set; }

        public virtual AdvertType AdvertType { get; set; }

        public DateTime? LastUpdate { get; set; }

        public DateTime EndDatetime { get; set; }

        public DateTime StartDatetime { get; set; }

        public DateTime CreatedDate { get; set; }

        public int AdvertStateId { get; set; }

        public virtual AdvertState AdvertState { get; set; }
    }
}