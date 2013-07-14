using System.Data.Entity;
using OMKT.Business;
using System.Data.Entity.Infrastructure;

namespace OMKT.Context
{
    public class OMKTDB : DbContext
    {
        //Initilize the database context   
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetails> InvoiceDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AdvertCampaign> AdvertCampaigns { get; set; }
        public DbSet<AdvertCampaignDetail> AdvertCampaignDetails { get; set; }
        public DbSet<AdvertHost> AdvertHosts { get; set; }
        public DbSet<AdvertState> AdvertStates { get; set; }
        public DbSet<AdvertHostCategory> AdvertHostCategories { get; set; }
        public DbSet<Inbox> Inboxes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<CampaignLocation> CampaignLocations { get; set; }
        public DbSet<Network> Networks { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<AdvertDetail> CatalogDetails { get; set; }
        public DbSet<CommercialProduct> CommercialProducts { get; set; }
        public DbSet<CommercialProductType> CommercialProductTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<CampaignState> CampaignStates { get; set; }
        public DbSet<CampaignType> CampaignTypes { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<SortType> SortTypes { get; set; }
        public DbSet<AdvertType> AdvertTypes { get; set; }
        public DbSet<AdvertInteraction> AdvertInteractions { get; set; }
        public DbSet<AdvertDetailInteraction> AdvertDetailInteractions { get; set; }
        public DbSet<Monitoring> Monitoring { get; set; }
        //public DbSet<Zone> Zones { get; set; }
        //public DbSet<Email> Emails { get; set; }
        //public DbSet<Snapshot> Snapshots { get; set; }

    }

}
