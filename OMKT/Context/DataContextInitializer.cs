﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Web.Security;
using OMKT.Business;
using System.Linq;

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
            #region 5 productos -> Botines de Fútbol
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
                    ThumbnailPath = "~/brand/product-images/V21078_F_p3.png",
                    Size = "",
                    Title = "Predator-Absolado"
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
                    ThumbnailPath = "~/brand/product-images/V21078_F_p3.png",
                    Path = "~/brand/product-images/V21350_F_p3.png",//"~/Content/productImages/F30-Trx.png",
                    Size = "",
                    Title = "F10-Trx"
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
                    ThumbnailPath = "~/brand/product-images/V21078_F_p3.png",
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
                    ThumbnailPath = "~/brand/product-images/V21078_F_p3.png",
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
                    ThumbnailPath = "~/brand/product-images/V21078_F_p3.png",
                    Path = "~/brand/product-images/V21334_F_p3.png",//"~/Content/productImages/F10-Trx.png",
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
            var customer = new Customer { Name = "Adidas Argentina SA", ContactPerson = "Andrés Ponte", Address = "9 de julio 2322 piso 15", CP = "5000", CompanyNumber = "5600012", City = "Buenos Aires", Phone1 = "5600022", Email = "info@adidas.com.ar" };
            context.Customers.Add(customer);
            //List<Customer> customers = new List<Customer>
            //                                   {
            //                                       new Customer {Name="Adidas Argentina SA", ContactPerson="Andrés Ponte", Address="9 de julio 2322 piso 15", CP="5000", CompanyNumber="5600012", City="Buenos Aires", Phone1="5600022", Email="info@adidas.com.ar"},
            //                                       //new Customer {Name="ACME International S.L", ContactPerson="Miguel Pérez", Address="12 Stree NY", CP="232323", CompanyNumber="3424324342", City="New York", Phone1="223-23232323", Email="hello@hello.com"},
            //                                       //new Customer {Name="Apple Inc.", ContactPerson="Juan Rodriguez", Address="1233 Street NY", CP="232323", CompanyNumber="23232323", City="NN CA", Phone1="343-23232323", Email="apple@hello.com"},
            //                                       //new Customer {Name="Zaragoza Activa", ContactPerson="José Ángel García", Address="Edificio: Antigua Azucarera, Mas de las Matas, 20 Planta B", CP="50015", CompanyNumber="BBBBBB", City="Zaragoza", Phone1="343-23232323", Email="zaragozaactiva@hello.com"},
            //                                       //new Customer {Name="Conecta S.L", ContactPerson="Rocío Ruíz", Address="C/ San Flores 213", CP="50800", CompanyNumber="BBBBBB", City="Zaragoza", Phone1="343-23232323", Email="contacta@hello.com"},
            //                                       //new Customer {Name="VitaminasDev", ContactPerson="Antonio Roy", Address="C/ San Pedro 79 2", CP="50800", CompanyNumber="29124609", City="Zuera, Zaragoza", Phone1="654 249068", Email="hola@vitaminasdev.com"}
            //                                   };

            //foreach (Customer c in customers)
            //{
            //    context.Customers.Add(c);
            //}
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

            #region states
            var estado = new AdvertState { Description = "Activo" };
            context.AdvertStates.Add(estado);
            #endregion

            #region game

            var game = new Game();
            game.Name = "Memory";
            game.CreatedDate = new DateTime(DateTime.Now.Year, 1, new Random().Next(1, 28));
            game.AdvertState = estado;
            game.Oportunities = 3;
            game.AdvertStateId = game.AdvertState.AdvertstateId;
            game.AdvertType = new AdvertType { Description = "Juego Interactivo" };
            game.AdvertTypeId = game.AdvertType.AdvertTypeId;
            var counter = 0;
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            foreach (var prod in products)
            {
                var random = new Random(counter);
                var result = new string(
                    Enumerable.Repeat(chars, 8)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());
                game.GameDetails.Add(new GameDetail
                {
                    Game = game,
                    AdvertId = game.AdvertId,
                    CommercialProduct = prod,
                    CreatedDate = game.CreatedDate,
                    LastUpdate = DateTime.Now,
                    Discount = new Random(counter).Next(10, 20),
                    QRCode = result,
                });
                counter++;
            }
            context.Games.Add(game);

            #endregion

            #region catalogs
            var catalog = new Catalog();
            catalog.Name = "Botines de Fútbol 2012";
            catalog.CreatedDate = new DateTime(DateTime.Now.Year, 1, new Random().Next(1, 28)); //random date (this month)
            if (catalog.CreatedDate != null)
                catalog.EndDatetime = Convert.ToDateTime(catalog.CreatedDate).AddDays(90);
            catalog.StartDatetime = catalog.CreatedDate;
            catalog.LastUpdate = DateTime.Now;
            catalog.AdvertState = estado;
            catalog.AdvertStateId = catalog.AdvertState.AdvertstateId;
            catalog.AdvertType = new AdvertType { Description = "Catálogo" };
            catalog.AdvertTypeId = catalog.AdvertType.AdvertTypeId;
            catalog.SortType = methods[new Random().Next(0, 4)];
            var gchars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            for (int id = 0; id < products.Count; id++)
            {
                var discount = new Random(id).Next(10, 21);
                var random = new Random(id);
                var result = new string(
                    Enumerable.Repeat(gchars, 8)
                              .Select(s => s[random.Next(s.Length)])
                              .ToArray());
                catalog.AdvertDetails.Add(new CatalogDetail
                {
                    Catalog = catalog,
                    AdvertId = catalog.AdvertId,
                    CommercialProduct = products[id],
                    Position = id,
                    CreatedDate = catalog.CreatedDate,
                    LastUpdate = DateTime.Now,
                    QRCode = result,
                    Discount = discount
                });               
            }
            foreach (var prod in products)
            {
                               
            }
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
            //Locations
            var loc1 = new Location { Latitude = "-31.419677", Longitude = "-64.1878", Detail = "Patio Olmos" };
            context.Locations.Add(loc1);
            var loc2 = new Location { Latitude = "-31.413133", Longitude = "-64.204357", Detail = "Nuevo Shopping" };
            context.Locations.Add(loc2);
            var loc3 = new Location { Latitude = "-31.442696", Longitude = "-64.194072", Detail = "Universidad Tecnológica Nacional" };
            context.Locations.Add(loc3);
            //Advert hosts categories
            var cat1 = new AdvertHostCategory { Name = "Premium" };
            context.AdvertHostCategories.Add(cat1);
            var cat2 = new AdvertHostCategory { Name = "Estandar" };
            context.AdvertHostCategories.Add(cat2);
            var cat3 = new AdvertHostCategory { Name = "Gratutita" };
            context.AdvertHostCategories.Add(cat3);
            //Advert hosts
            var host1 = new AdvertHost { AdvertHostName = "Patio Olmos" };
            host1.Location = loc1;
            host1.LocationId = host1.Location.LocationId;
            host1.AdvertHostCategory = cat1;
            host1.AdvertHostCategoryId = host1.AdvertHostCategory.AdvertHostCategoryId;
            context.AdvertHosts.Add(host1);
            var host2 = new AdvertHost { AdvertHostName = "Nuevo centro" };
            host2.Location = loc2;
            host2.LocationId = host2.Location.LocationId;
            host2.AdvertHostCategory = cat2;
            host2.AdvertHostCategoryId = host2.AdvertHostCategory.AdvertHostCategoryId;
            context.AdvertHosts.Add(host2);
            var host3 = new AdvertHost { AdvertHostName = "UTN" };
            host3.Location = loc3;
            host3.LocationId = host3.Location.LocationId;
            host3.AdvertHostCategory = cat3;
            host3.AdvertHostCategoryId = host3.AdvertHostCategory.AdvertHostCategoryId;
            context.AdvertHosts.Add(host3);

            var campLoc1 = new CampaignLocation { Description = "Todas las ubicaciones" };
            campLoc1.AdvertHosts.Add(host1);
            campLoc1.AdvertHosts.Add(host2);
            campLoc1.AdvertHosts.Add(host3);
            context.CampaignLocations.Add(campLoc1);

            var locations = new List<CampaignLocation>
								{  new CampaignLocation{Description = "Ubicaciones privadas"},
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
            campaign.CustomerId = campaign.Customer.CustomerID;
            campaign.Name = "Campaña #1";
            campaign.CampaignType = new CampaignType { Name = "Estándar", Description = "Emisión de anuncios en todos los puestos de difusión de la red Optical Marketing" };
            campaign.CampaignTypeId = campaign.CampaignType.CampaignTypeId;
            campaign.CampaignState = new CampaignState { Description = "Vigente" };
            campaign.CampaignStateId = campaign.CampaignState.CampaignStateId;
            campaign.Estimate = new Random(1).Next(10, 100);
            campaign.CreatedDate = DateTime.Now.AddDays(-15);
            campaign.EndDatetime = DateTime.Now.AddDays(15);
            campaign.Network = new Network { Description = "Red de difusión de Optical Marketing" };
            campaign.NetworkId = campaign.Network.NetworkId;
            campaign.CampaignLocation = campLoc1;
            campaign.CampaignLocationId = campaign.CampaignLocation.CampaignLocationId;
            campaign.StartDatetime = campaign.CreatedDate;
            campaign.LastUpdate = DateTime.Now;
            campaign.AdvertCampaignDetails.Add(new AdvertCampaignDetail
                {
                    Advert = catalog,
                    StartDate = campaign.StartDatetime,
                    EndDate = campaign.EndDatetime
                });
            campaign.AdvertCampaignDetails.Add(new AdvertCampaignDetail
                {
                    Advert = game,
                    StartDate = campaign.StartDatetime,
                    EndDate = campaign.EndDatetime
                });

            context.AdvertCampaigns.Add(campaign);

            #endregion

            #region Interactions
            /**
			 * Let the magic begin
			 * Fill the interactions tables
			 * 
			 **/
            //for (int z = 0; z < 30; z++) // last 15 days
            //{
            //    var startDate = DateTime.Now.AddDays(-z).AddMinutes(-5);
            //    var endDate = DateTime.Now.AddDays(-z);
            //    #region Campaigns intereactions
            //    var ct = new AdvertCampaignInteraction
            //    {
            //        AdvertCampaign = campaign,
            //        AdvertCampaignID = campaign.AdvertCampaignId,
            //        StartDatetime = startDate,
            //        EndDatetime = endDate,
            //        TimeElapsed = endDate.Subtract(startDate).Seconds

            //    };
            //    var count = 0;
            //    Console.WriteLine(campaign.AdvertCampaignDetails.Count());
            //    foreach (var detail in campaign.AdvertCampaignDetails) // only 2 (catalog and game)
            //    {
            //        Console.WriteLine(detail.Advert.Name);
            //        // 30 times a day
            //        count++;
            //        var times = new Random().Next(15, 25);
            //        for (int i = 0; i < times; i++)
            //        {
            //            #region DetailsCampaign interactions
            //            var startDa = DateTime.Now.AddDays(-i).AddMinutes(-5);
            //            var endDa = DateTime.Now.AddDays(-i);
            //            decimal h = (new Random().Next(5, 9)) / 10;
            //            var at = new AdvertCampaignDetailInteraction
            //            {
            //                Advert = detail.Advert,
            //                AdvertID = detail.AdvertID,
            //                StartDatetime = startDa,
            //                EndDatetime = endDa,
            //                TimeElapsed = endDa.Subtract(startDa).Seconds,
            //                Height = (h + 1),

            //            };
            //            ct.AdvertCampaignDetailInteractions.Add(at);

            //            #region catalogDetails interactions
            //            var sDa = DateTime.Now.AddDays(-i).AddMinutes(-5);
            //            var eDa = DateTime.Now.AddDays(-i);
            //            if (count > 1)
            //            {  //rusticidad para cargar fucking details

            //                var totimes = new Random().Next(1, catalog.AdvertDetails.Count()+1);
            //                var detalles = catalog.AdvertDetails.ToArray();
            //                for (int j = 0; j < totimes; j++)
            //                {
            //                    var r = new Random().Next(0, 2);
            //                    var li = (r == 1) ? true : false;
            //                    var nt = new CatalogDetailInteraction
            //                    {
            //                        CatalogDetail = detalles[j],
            //                        CatalogDetailID = detalles[j].CatalogDetailId,
            //                        StartDatetime = sDa,
            //                        EndDatetime = eDa,
            //                        TimeElapsed = eDa.Subtract(sDa).Seconds,
            //                        View = true,
            //                        Like = li,
            //                    };
            //                    context.CatalogDetailInteractions.Add(nt);
            //                }


            //            }
            //            else
            //            {
            //                var totimes = new Random().Next(1, game.Oportunities);
            //                var randProd = new Random().Next(1, game.GameDetails.Count());
            //                var detalles = game.GameDetails.ToArray();
            //                for (int v = 0; v < totimes; v++)
            //                {
            //                    var r = new Random().Next(0, 2);
            //                    var li = (r == 1) ? true : false;
            //                    var gt = new GameDetailInteraction
            //                    {
            //                        GameDetail = detalles[randProd],
            //                        GameDetailID = detalles[randProd].GameDetailId,
            //                        StartDatetime = sDa,
            //                        EndDatetime = eDa,
            //                        TimeElapsed = eDa.Subtract(sDa).Seconds,
            //                        Win = li

            //                    };
            //                    context.GameDetailInteractions.Add(gt);
            //                }
            //            }
            //            #endregion
            //            #endregion

            //        }

            //    }

            //    context.AdvertCampaignInteractions.Add(ct);
            //    #endregion
            //}

            #endregion

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
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