using CityVoxWeb.Data.Models.GeoEntities;
using CityVoxWeb.Data.Models.UserEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityVoxWeb.Data.Configurations
{
    public class MunicipalityConfiguration : IEntityTypeConfiguration<Municipality>
    {
        public void Configure(EntityTypeBuilder<Municipality> builder)
        {
            builder
                .HasOne(m => m.Region)
                .WithMany(r => r.Municipalities)
                .HasForeignKey(m => m.RegionId)
                .OnDelete(DeleteBehavior.NoAction);

            SeedData(builder);
        }

        public static void SeedData(EntityTypeBuilder<Municipality> builder)
        {
            builder.HasData
                (
                   new Municipality
                   {
                       Id = Guid.Parse("4D5C8490-D8D5-4B13-A8AA-6D5D53D4892B"),
                       MunicipalityName = "Vitosha",
                       OpenStreetMapCode = "3759447",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("C4CE0D3B-0ADD-4DD4-99F6-CFB3C036B982"),
                       MunicipalityName = "Pancharevo",
                       OpenStreetMapCode = "3759439",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("B3CCA990-B657-4E8D-86CD-AEDCDA2F8AB5"),
                       MunicipalityName = "Mladost",
                       OpenStreetMapCode = "3759434",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("C53BD2D0-FB89-4AA1-9888-035F571B5C35"),
                       MunicipalityName = "Iskar",
                       OpenStreetMapCode = "3759427",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("0C8B02B2-5171-4F2D-9EDB-E0CD14F1DA12"),
                       MunicipalityName = "Kremikovci",
                       OpenStreetMapCode = "3759431",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("910A4325-D42B-4894-9E38-DAD5F60DBF29"),
                       MunicipalityName = "Novi Iskar",
                       OpenStreetMapCode = "3759436",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("C6251449-6EE4-4D5D-8B81-88C64CF12049"),
                       MunicipalityName = "Serdika",
                       OpenStreetMapCode = "3759441",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id= Guid.Parse("6C967841-3AAC-4C6F-9065-514ABFF87D55"),
                       MunicipalityName = "Vrabnitsa",
                       OpenStreetMapCode = "3759448",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("D447FA1B-D3A6-462B-9554-28C27FC48085"),
                       MunicipalityName = "Nadejda",
                       OpenStreetMapCode = "3759435",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("9B6306E8-0102-4E3E-BEF0-B212E7D06680"),
                       MunicipalityName = "Liulin",
                       OpenStreetMapCode = "3759432",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("05C2FDE5-E9C1-4C2E-88AC-5378BF90F7FE"),
                       MunicipalityName = "Bankia",
                       OpenStreetMapCode = "3759425",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("BFD4CFB4-8451-4A29-A180-D859E20B8421"),
                       MunicipalityName = "Ovcha kupel",
                       OpenStreetMapCode = "3759438",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("C0BF4173-BA2D-4F82-A796-5C75F02BF8D6"),
                       MunicipalityName = "Krasna poliana",
                       OpenStreetMapCode = "3759429",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("D6A334AE-856A-46D4-94B3-92A547A1DF96"),
                       MunicipalityName = "Krasno selo",
                       OpenStreetMapCode = "3759430",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   }, 
                   new Municipality
                   {
                       Id = Guid.Parse("1C752650-D449-49E2-B5AD-6EA4FFAF89F0"),
                       MunicipalityName = "Triadica",
                       OpenStreetMapCode = "3759445",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("B56B1A2A-3042-4182-A2B5-9CB46AA6B5C7"),
                       MunicipalityName = "Lozenets",
                       OpenStreetMapCode = "3759433",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("37E9A5F3-48A5-4B00-99A8-438DC1F49F6E"),
                       MunicipalityName = "Sredec",
                       OpenStreetMapCode = "3759443",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("DD82B90C-C418-4BE9-AC20-CB69CEB5A119"),
                       MunicipalityName = "Oborishte",
                       OpenStreetMapCode = "3759437",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("6BA3B7F0-2CBD-4E0F-A97E-447939EE1D8A"),
                       MunicipalityName = "Poduyane",
                       OpenStreetMapCode = "3759440",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("93BF9E90-3ED0-4FCA-8628-68F213CF9D67"),
                       MunicipalityName = "Slatina",
                       OpenStreetMapCode = "3759442",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("FE732CE1-09B8-4374-9E97-7C9431ACA5DA"),
                       MunicipalityName = "Vazrajdane",
                       OpenStreetMapCode = "3759446",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("F808947E-CEE5-4C0E-B24A-3E6DADC89668"),
                       MunicipalityName = "Ilinden",
                       OpenStreetMapCode = "3759426",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("145A7A39-5007-4676-B988-264403A6B154"),
                       MunicipalityName = "Studentski",
                       OpenStreetMapCode = "3759444",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   },
                   new Municipality
                   {
                       Id = Guid.Parse("C758BE0B-1753-459E-B11B-D97FEBF17ECC"),
                       MunicipalityName = "Izgrev",
                       OpenStreetMapCode = "3759428",
                       RegionId = Guid.Parse("D0282A05-BDB5-4B8D-B90C-AB62A3543ED8"),
                   }

                );
        }
    }
}
