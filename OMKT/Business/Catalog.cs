using System.Collections.Generic;

namespace OMKT.Business
{
    public class Catalog : Advert
    {
        public int SortTypeId { get; set; }
        public virtual SortType SortType { get; set; }

        public ICollection<AdvertDetail> AdvertDetails { get; set; }

        public Catalog()
        {
            AdvertDetails = new List<AdvertDetail>();
        }
    }
}