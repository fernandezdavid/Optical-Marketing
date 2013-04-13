using System;

namespace OMKT.Business
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }

        public string Caption { get; set; }

        public string Title { get; set; }

        public string Size { get; set; }

        public string Extension { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string Path { get; set; }

        public string ThumbnailPath { get; set; }
    }
}