using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Bogus;
using App.Models;

#nullable disable

namespace App.Migrations
{
    /// <inheritdoc />
    public partial class initdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => x.Id);
                });

                Randomizer.Seed =new Random(8675309);
                    var FakerArticle =new Faker<Article>();
                    FakerArticle.RuleFor(a=>a.Title,f=>f.Lorem.Sentence(5,5));
                    FakerArticle.RuleFor(a=>a.Created,f=>f.Date.Between(new DateTime(2022,1,1), new DateTime(2022,12,31)));
                    FakerArticle.RuleFor(a=>a.Content,f=>f.Lorem.Paragraphs(1,4));

                for (int i = 0; i < 150; i++)
                    {
                        
                    Article article=FakerArticle.Generate();
                    migrationBuilder.InsertData(
                        table:"articles",
                        columns: new []{"Id","Title","Created","Content"},
                        values: new object[]{
                            i,
                            article.Title,
                            article.Created,
                            article.Content
                        }
                    );
                }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articles");
        }
    }
}
