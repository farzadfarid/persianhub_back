using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersianHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFullProductDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingBookmarks_MarketplaceListings_MarketplaceListingId",
                table: "ListingBookmarks");

            migrationBuilder.AddColumn<int>(
                name: "ShareType",
                table: "ShareLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ReferredUserId",
                table: "Referrals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Referrals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "ReferralCodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "Invites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAtUtc",
                table: "FeaturedPlacements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "BusinessId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "ContactRequests",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AddColumn<int>(
                name: "ContactType",
                table: "ContactRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsConverted",
                table: "ContactRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "ContactRequests",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DailyOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: true),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    DiscountValue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    DiscountedPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    StartsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndsAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyOffers_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    ReferenceType = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Interactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: true),
                    InteractionType = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interactions_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Interactions_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    ActivityType = table.Column<int>(type: "int", nullable: false),
                    ReferenceType = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<int>(type: "int", nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivities_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareLogs_CreatedAtUtc",
                table: "ShareLogs",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_FeaturedPlacements_IsActive_EndsAtUtc",
                table: "FeaturedPlacements",
                columns: new[] { "IsActive", "EndsAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_BusinessId",
                table: "Events",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactRequests_CreatedAtUtc",
                table: "ContactRequests",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_DailyOffers_BusinessId",
                table: "DailyOffers",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyOffers_IsActive_IsPublished",
                table: "DailyOffers",
                columns: new[] { "IsActive", "IsPublished" });

            migrationBuilder.CreateIndex(
                name: "IX_DailyOffers_Slug",
                table: "DailyOffers",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyOffers_StartsAtUtc",
                table: "DailyOffers",
                column: "StartsAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_AppUserId_ReferenceType_ReferenceId",
                table: "Favorites",
                columns: new[] { "AppUserId", "ReferenceType", "ReferenceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_CreatedAtUtc",
                table: "Favorites",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_AppUserId",
                table: "Interactions",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_BusinessId",
                table: "Interactions",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Interactions_CreatedAtUtc",
                table: "Interactions",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_AppUserId",
                table: "UserActivities",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_CreatedAtUtc",
                table: "UserActivities",
                column: "CreatedAtUtc");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Businesses_BusinessId",
                table: "Events",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingBookmarks_MarketplaceListings_MarketplaceListingId",
                table: "ListingBookmarks",
                column: "MarketplaceListingId",
                principalTable: "MarketplaceListings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Businesses_BusinessId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingBookmarks_MarketplaceListings_MarketplaceListingId",
                table: "ListingBookmarks");

            migrationBuilder.DropTable(
                name: "DailyOffers");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Interactions");

            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropIndex(
                name: "IX_ShareLogs_CreatedAtUtc",
                table: "ShareLogs");

            migrationBuilder.DropIndex(
                name: "IX_FeaturedPlacements_IsActive_EndsAtUtc",
                table: "FeaturedPlacements");

            migrationBuilder.DropIndex(
                name: "IX_Events_BusinessId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_ContactRequests_CreatedAtUtc",
                table: "ContactRequests");

            migrationBuilder.DropColumn(
                name: "ShareType",
                table: "ShareLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Referrals");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "ReferralCodes");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "Invites");

            migrationBuilder.DropColumn(
                name: "UpdatedAtUtc",
                table: "FeaturedPlacements");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ContactType",
                table: "ContactRequests");

            migrationBuilder.DropColumn(
                name: "IsConverted",
                table: "ContactRequests");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "ContactRequests");

            migrationBuilder.AlterColumn<int>(
                name: "ReferredUserId",
                table: "Referrals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "ContactRequests",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingBookmarks_MarketplaceListings_MarketplaceListingId",
                table: "ListingBookmarks",
                column: "MarketplaceListingId",
                principalTable: "MarketplaceListings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
