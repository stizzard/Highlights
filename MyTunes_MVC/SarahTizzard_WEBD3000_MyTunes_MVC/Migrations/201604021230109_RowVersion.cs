namespace SarahTizzard_WEBD3000_MyTunes_MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RowVersion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Album",
                c => new
                    {
                        AlbumId = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ArtistId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AlbumId)
                .ForeignKey("dbo.Artist", t => t.ArtistId, cascadeDelete: true)
                .Index(t => t.ArtistId);
            
            CreateTable(
                "dbo.Track",
                c => new
                    {
                        TrackId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        AlbumId = c.Int(nullable: false),
                        MediaTypeId = c.Int(nullable: false),
                        GenreId = c.Int(nullable: false),
                        Composer = c.String(),
                        Milliseconds = c.Int(nullable: false),
                        Bytes = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.TrackId)
                .ForeignKey("dbo.Album", t => t.AlbumId, cascadeDelete: true)
                .ForeignKey("dbo.Genre", t => t.GenreId, cascadeDelete: true)
                .ForeignKey("dbo.MediaType", t => t.MediaTypeId, cascadeDelete: true)
                .Index(t => t.AlbumId)
                .Index(t => t.MediaTypeId)
                .Index(t => t.GenreId);
            
            CreateTable(
                "dbo.Genre",
                c => new
                    {
                        GenreId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.GenreId);
            
            CreateTable(
                "dbo.MediaType",
                c => new
                    {
                        MediaTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        MediaCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MediaTypeId)
                .ForeignKey("dbo.MediaCategory", t => t.MediaCategoryId, cascadeDelete: true)
                .Index(t => t.MediaCategoryId);
            
            CreateTable(
                "dbo.MediaCategory",
                c => new
                    {
                        MediaCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.MediaCategoryId);
            
            CreateTable(
                "dbo.Artist",
                c => new
                    {
                        ArtistId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ArtistId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Album", "ArtistId", "dbo.Artist");
            DropForeignKey("dbo.Track", "MediaTypeId", "dbo.MediaType");
            DropForeignKey("dbo.MediaType", "MediaCategoryId", "dbo.MediaCategory");
            DropForeignKey("dbo.Track", "GenreId", "dbo.Genre");
            DropForeignKey("dbo.Track", "AlbumId", "dbo.Album");
            DropIndex("dbo.MediaType", new[] { "MediaCategoryId" });
            DropIndex("dbo.Track", new[] { "GenreId" });
            DropIndex("dbo.Track", new[] { "MediaTypeId" });
            DropIndex("dbo.Track", new[] { "AlbumId" });
            DropIndex("dbo.Album", new[] { "ArtistId" });
            DropTable("dbo.Artist");
            DropTable("dbo.MediaCategory");
            DropTable("dbo.MediaType");
            DropTable("dbo.Genre");
            DropTable("dbo.Track");
            DropTable("dbo.Album");
        }
    }
}
