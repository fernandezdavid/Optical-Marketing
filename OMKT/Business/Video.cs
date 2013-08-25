namespace OMKT.Business
{
    public class Video : Advert
    {
        public string URL { get; set; }

        public string Path { get; set; }

        public string Caption { get; set; }

        public string Title { get; set; }

        public string Size { get; set; }

        public string Extension { get; set; }

        public int ProductImageId { get; set; }
        public virtual ProductImage ProductImage { get; set; }

    }
}