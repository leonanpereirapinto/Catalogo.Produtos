using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Catalogo.Data.Migrations
{
    public partial class AdicionarProdutos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Estoque", "Nome", "Valor" },
                values: new object[,]
                {
                    { new Guid("955bd18e-9050-44ad-9bd7-8cc7fc410d53"), 10, "Produto de teste 1", 100m },
                    { new Guid("b492e203-0209-4110-b373-17b0201c6f52"), 20, "Produto de teste 2", 200m },
                    { new Guid("424cd108-6702-4fa4-ae5f-d5d736480f85"), 30, "Produto de teste 3", 300m },
                    { new Guid("4d709a0b-fc55-488e-b902-707cc10d06a2"), 40, "Produto de teste 4", 400m },
                    { new Guid("0aca01f3-aaac-4b6b-9c2b-f675868ab8d2"), 50, "Produto de teste 5", 500m }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: new Guid("0aca01f3-aaac-4b6b-9c2b-f675868ab8d2"));

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: new Guid("424cd108-6702-4fa4-ae5f-d5d736480f85"));

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: new Guid("4d709a0b-fc55-488e-b902-707cc10d06a2"));

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: new Guid("955bd18e-9050-44ad-9bd7-8cc7fc410d53"));

            migrationBuilder.DeleteData(
                table: "Produtos",
                keyColumn: "Id",
                keyValue: new Guid("b492e203-0209-4110-b373-17b0201c6f52"));
        }
    }
}
