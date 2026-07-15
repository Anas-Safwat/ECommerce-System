using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProductStockFromBoolToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "Stock",
            //    table: "Products",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(bool),
            //    oldType: "boolean");

            migrationBuilder.Sql("ALTER TABLE \"Products\" ALTER COLUMN \"Stock\" TYPE Integer USING \"Stock\"::integer;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<bool>(
            //    name: "Stock",
            //    table: "Products",
            //    type: "boolean",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "integer");

            migrationBuilder.Sql("ALTER TABLE \"Products\" ALTER COLUMN \"Stock\" TYPE boolean USING \"Stock\"::boolean;");

        }
    }
}
