using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Web.Security;
using OMKT.Business;

namespace OMKT.Context
{
    public class DataContextInitializer : DropCreateDatabaseIfModelChanges<OMKTDB>
    {
        protected override void Seed(OMKTDB context)
        {
            #region users
            //Users
            Roles.CreateRole("Administrador");
            //Roles.CreateRole("Usuario");
            //Users
            MembershipCreateStatus status;
            Membership.CreateUser("David", "kepler", "fernandezdavideduardo@gmail.com", null, null, true, out status);

            Roles.AddUserToRole("David", "Administrador");
            #endregion

            #region product types
            var types = new List<CommercialProductType>
            {
                new CommercialProductType{ Description = "Deporte"}
                
            };
            foreach (var t in types)
            {
                context.CommercialProductTypes.Add(t);
            }
            #endregion

            #region products
            var products = new List<CommercialProduct>();
            #region 5 productos Catálogo Botines de Fútbol
            products.Add(new CommercialProduct
            {
                ProductName = "Predator Absolado Lz Trx Fg",
                Description = "Los depredadores han nacido para ser letales, y estos adidas Predator Absolado TRX LZ FG tienen cinco zonas letales para la maestría técnica en campos de tierra firme. También tienen una ligera amortiguación superior y EVA.",
                Price = 300,
                Stock = 500,
                ProductImage = new ProductImage
                {
                    Caption = "Predator-Absolado",
                    CreatedDate = DateTime.Now,
                    Extension = "png",
                    Path = "~/brand/product-images/V21078_F_p3.png",//"~/Content/productImages/Predator-Absolado.png",
                    Size = "",
                    Title = "Predator-Absolado"
                },
                VideoPath = "",
                CommercialProductType = types[0],
                Customer = context.Customers.Find(1)
            });
            products.Add(new CommercialProduct
            {
                ProductName = "11Nova Trx Tf J Leather",
                Description = "Éstos adidas 11Nova TRX TF cuentan con una correa de cuero con recubrimiento superior, para un ajuste glovelike, tacto del balón superior y suela TRAXION ® TF para agarre en canchas de césped.",
                Price = 300,
                Stock = 500,
                ProductImage = new ProductImage
                {
                    Caption = "11Nova-Trx",
                    CreatedDate = DateTime.Now,
                    Extension = "png",
                    Path = "~/brand/product-images/G63818_F_p3.png",//"~/Content/productImages/11Nova-Trx.png",
                    Size = "",
                    Title = "11Nova-Trx"
                },
                VideoPath = "",
                CommercialProductType = types[0],
                Customer = context.Customers.Find(1)
            });
            products.Add(new CommercialProduct
            {
                ProductName = "11Core Trx Fg",
                Description = "Si eres una presencia explosiva en el campo de fútbol, éstos adidas 11Core Trx llevan tus habilidades al centro de atención. Poseen un cuero superior con una buena flexibilidad y suela TRAXION ® FG para la velocidad en campos de tierra firme.",
                Price = 300,
                Stock = 500,
                ProductImage = new ProductImage
                {
                    Caption = "11Core-Trx",
                    CreatedDate = DateTime.Now,
                    Extension = "png",
                    Path = "~/brand/product-images/G60009_F_p3.png",//"~/Content/productImages/11Core-Trx.png",
                    Size = "",
                    Title = "11Core-Trx"
                },
                VideoPath = "",
                CommercialProductType = types[0],
                Customer = context.Customers.Find(1)
            });
            products.Add(new CommercialProduct
            {
                ProductName = "F10 Trx Tf",
                Description = "Elaborados con un cuero sintético ligero superior, éstos adidas F10 Trx Tf cuentan con amortiguación EVA, adiPRENE ® apoyo del talón y una suela TRAXION ™ TF para un mejor agarre y velocidad en el campo.",
                Price = 300,
                Stock = 500,
                ProductImage = new ProductImage
                {
                    Caption = "F10-Trx",
                    CreatedDate = DateTime.Now,
                    Extension = "png",
                    Path = "~/brand/product-images/V21334_F_p3.png",//"~/Content/productImages/F10-Trx.png",
                    Size = "",
                    Title = "F10-Trx"
                },
                VideoPath = "",
                CommercialProductType = types[0],
                Customer = context.Customers.Find(1)
            });
            products.Add(new CommercialProduct
            {
                ProductName = "F30 Trx Fg",
                Description = "Éstos F30 Trx cuentan con una alta densidad de tres capas superiores de excelente tacto al balón, SprintWeb para cortes rápidos y suela exterior TRAXION ® 2.0 FG para un mejor agarre.",
                Price = 300,
                Stock = 500,
                ProductImage = new ProductImage
                {
                    Caption = "F30-Trx",
                    CreatedDate = DateTime.Now,
                    Extension = "png",
                    Path = "~/brand/product-images/V21350_F_p3.png",//"~/Content/productImages/F30-Trx.png",
                    Size = "",
                    Title = "F10-Trx"
                },
                VideoPath = "",
                CommercialProductType = types[0],
                Customer = context.Customers.Find(1)
            });
            #endregion

            foreach (CommercialProduct p in products)
            {
                context.CommercialProducts.Add(p);
            }
            #endregion

            #region customers

            List<Customer> customers = new List<Customer>
                                               {
                                                   new Customer {Name="Adidas Argentina SA", ContactPerson="Andrés Ponte", Address="9 de julio 2322 piso 15", CP="5000", CompanyNumber="5600012", City="Buenos Aires", Phone1="5600022", Email="info@adidas.com.ar"},
                                                   //new Customer {Name="ACME International S.L", ContactPerson="Miguel Pérez", Address="12 Stree NY", CP="232323", CompanyNumber="3424324342", City="New York", Phone1="223-23232323", Email="hello@hello.com"},
                                                   //new Customer {Name="Apple Inc.", ContactPerson="Juan Rodriguez", Address="1233 Street NY", CP="232323", CompanyNumber="23232323", City="NN CA", Phone1="343-23232323", Email="apple@hello.com"},
                                                   //new Customer {Name="Zaragoza Activa", ContactPerson="José Ángel García", Address="Edificio: Antigua Azucarera, Mas de las Matas, 20 Planta B", CP="50015", CompanyNumber="BBBBBB", City="Zaragoza", Phone1="343-23232323", Email="zaragozaactiva@hello.com"},
                                                   //new Customer {Name="Conecta S.L", ContactPerson="Rocío Ruíz", Address="C/ San Flores 213", CP="50800", CompanyNumber="BBBBBB", City="Zaragoza", Phone1="343-23232323", Email="contacta@hello.com"},
                                                   //new Customer {Name="VitaminasDev", ContactPerson="Antonio Roy", Address="C/ San Pedro 79 2", CP="50800", CompanyNumber="29124609", City="Zuera, Zaragoza", Phone1="654 249068", Email="hola@vitaminasdev.com"}
                                               };

            foreach (Customer c in customers)
            {
                context.Customers.Add(c);
            }
            #endregion

            #region sort types
            var methods = new List<SortType>
                              {
                                  new SortType{ Name = "Ascendente"},
                                  new SortType{ Name ="Descendente" },
                                  new SortType{ Name = "Aleatorio"},
                                  new SortType{ Name = "Relevancia"},
                                  new SortType{ Name = "Proporción"}
                              };
            foreach (var sortType in methods)
            {
                context.SortTypes.Add(sortType);
            }
            #endregion

            #region catalogs
            var catalog = new Catalog();
            catalog.Name = "Botines de Fútbol 2012";
            catalog.CreatedDate = new DateTime(DateTime.Now.Year, 1, new Random().Next(1, 28)); //random date (this month)
            catalog.EndDatetime = catalog.CreatedDate.AddDays(90);
            catalog.StartDatetime = catalog.CreatedDate;
            catalog.LastUpdate = DateTime.Now;
            catalog.AdvertState = new AdvertState { Description = "Activo" };
            catalog.AdvertType = new AdvertType { Description = "Catálogo" };
            catalog.SortType = methods[new Random().Next(0, 4)];
            var position = 0;
            foreach (var prod in products)
            {
                catalog.AdvertDetails.Add(new AdvertDetail
                                               {
                                                   CommercialProduct = prod,
                                                   Position = position,
                                                   CreatedDate = catalog.CreatedDate,
                                                   LastUpdate = DateTime.Now,
                                                   Likes = new Random(position).Next(30, 100),
                                                   Views = new Random(position).Next(30, 1100),
                                               });
                position++;
            }
            //catalogs.Add(catalog);
            context.Catalogs.Add(catalog);
            #endregion

            #region campaign states
            var campaignState = new List<CampaignState>
                                    {
                                        new CampaignState{ Description = "Cancelada"},
                                        new CampaignState{ Description = "Pospuesta"},
                                        new CampaignState{ Description = "Pausada"},
                                        new CampaignState{ Description = "Terminada" }
                                    };
            foreach (var cs in campaignState)
            {
                context.CampaignStates.Add(cs);
            }
            #endregion

            #region campaign locations
            var locations = new List<CampaignLocation>
                                {
                                   new CampaignLocation{Description = "Todas las ubicaciones"},
                                   new CampaignLocation{Description = "Ubicaciones privadas"},
                                   new CampaignLocation{Description = "Ubicaciones públicas"}
                                };
            foreach (var loc in locations)
            {
                context.CampaignLocations.Add(loc);
            }
            #endregion

            #region campaigns
            var campaign = new AdvertCampaign();
            campaign.Customer = context.Customers.Find(1);
            campaign.Name = "Campaña #1";
            campaign.CampaignType = new CampaignType { Name = "Estándar", Description = "Emisión de anuncios en todos los puestos de difusión de la red Optical Marketing" };
            campaign.CampaignState = new CampaignState { Description = "Vigente" };
            campaign.Estimate = new Random(1).Next(10, 100);
            campaign.CreatedDate = DateTime.Now.AddDays(-15);
            campaign.EndDatetime = DateTime.Now.AddDays(15);
            campaign.Network = new Network { Description = "Red de difusión de Optical Marketing" };
            campaign.CampaignLocation = locations[0];
            campaign.StartDatetime = campaign.CreatedDate;
            campaign.CustomerId = campaign.Customer.CustomerID;
            campaign.LastUpdate = DateTime.Now;
            campaign.AdvertCampaignDetails.Add(new AdvertCampaignDetail
                {
                    Advert = catalog,
                    StartDate = campaign.StartDatetime,
                    EndDate = campaign.EndDatetime
                });

            #region interactions

            foreach (var cmp in campaign.AdvertCampaignDetails)
            {
                for (int j = 0; j < 7; j++)
                {
                    var views = new Random(j).Next(2000, 3000);
                    cmp.Interactions.Add(new Interaction
                                             {
                                                 StartDateTime = DateTime.Now.AddDays(-j).AddHours(-10),
                                                 EndDateTime = DateTime.Now.AddDays(-j),
                                                 Impressions = views,
                                                 Traffic = new Random(j).Next(10000, 15000),
                                                 Snapshot = new Snapshot(),
                                                 AdvertCampaignDetail = cmp
                                             });

                }

            }
            #endregion

            context.AdvertCampaigns.Add(campaign);

            #endregion
           
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx) //debug errors
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Console.Write("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }


        }
    }
}