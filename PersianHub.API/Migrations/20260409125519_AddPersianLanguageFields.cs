using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersianHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPersianLanguageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionFa",
                table: "SubscriptionPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameFa",
                table: "SubscriptionPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionFa",
                table: "MarketplaceListings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleFa",
                table: "MarketplaceListings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLineFa",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityFa",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionFa",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationNameFa",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrganizerNameFa",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionFa",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleFa",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionFa",
                table: "EventCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameFa",
                table: "EventCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionFa",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TermsAndConditionsFa",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleFa",
                table: "Deals",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionFa",
                table: "DealCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameFa",
                table: "DealCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionFa",
                table: "DailyOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleFa",
                table: "DailyOffers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameFa",
                table: "BusinessTags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLineFa",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CityFa",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionFa",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameFa",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionFa",
                table: "Businesses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionFa",
                table: "BusinessCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameFa",
                table: "BusinessCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BioFa",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayNameFa",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstNameFa",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastNameFa",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "BusinessCategories",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DealCategories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DealCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DealCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DealCategories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DealCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DealCategories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DealCategories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DealCategories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "DealCategories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EventCategories",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "SubscriptionPlans",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DescriptionFa", "NameFa" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionFa",
                table: "SubscriptionPlans");

            migrationBuilder.DropColumn(
                name: "NameFa",
                table: "SubscriptionPlans");

            migrationBuilder.DropColumn(
                name: "DescriptionFa",
                table: "MarketplaceListings");

            migrationBuilder.DropColumn(
                name: "TitleFa",
                table: "MarketplaceListings");

            migrationBuilder.DropColumn(
                name: "AddressLineFa",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "CityFa",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DescriptionFa",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LocationNameFa",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OrganizerNameFa",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RegionFa",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TitleFa",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DescriptionFa",
                table: "EventCategories");

            migrationBuilder.DropColumn(
                name: "NameFa",
                table: "EventCategories");

            migrationBuilder.DropColumn(
                name: "DescriptionFa",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "TermsAndConditionsFa",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "TitleFa",
                table: "Deals");

            migrationBuilder.DropColumn(
                name: "DescriptionFa",
                table: "DealCategories");

            migrationBuilder.DropColumn(
                name: "NameFa",
                table: "DealCategories");

            migrationBuilder.DropColumn(
                name: "DescriptionFa",
                table: "DailyOffers");

            migrationBuilder.DropColumn(
                name: "TitleFa",
                table: "DailyOffers");

            migrationBuilder.DropColumn(
                name: "NameFa",
                table: "BusinessTags");

            migrationBuilder.DropColumn(
                name: "AddressLineFa",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "CityFa",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "DescriptionFa",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "NameFa",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "RegionFa",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "DescriptionFa",
                table: "BusinessCategories");

            migrationBuilder.DropColumn(
                name: "NameFa",
                table: "BusinessCategories");

            migrationBuilder.DropColumn(
                name: "BioFa",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "DisplayNameFa",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "FirstNameFa",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "LastNameFa",
                table: "AppUsers");
        }
    }
}
