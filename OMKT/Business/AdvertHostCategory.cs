using System.ComponentModel.DataAnnotations;

namespace OMKT.Business
{
    public class AdvertHostCategory
    {
        public int AdvertHostCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Detail { get; set; }
    }
}