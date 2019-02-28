namespace TestServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BalanceNumber",
                c => new
                    {
                        BalanceId = c.Int(nullable: false),
                        AssetBalance = c.Decimal(nullable: false, precision: 28, scale: 9),
                        PassiveBalance = c.Decimal(nullable: false, precision: 28, scale: 9),
                        AssetOutgoingBalance = c.Decimal(nullable: false, precision: 28, scale: 9),
                        PassiveOutgoingBalance = c.Decimal(nullable: false, precision: 28, scale: 9),
                        TurnoverLoan = c.Decimal(nullable: false, precision: 28, scale: 9),
                        TurnoverDebit = c.Decimal(nullable: false, precision: 28, scale: 9),
                        ClassId = c.Int(),
                    })
                .PrimaryKey(t => t.BalanceId)
                .ForeignKey("dbo.Class", t => t.ClassId)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Class",
                c => new
                    {
                        ClassId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        FileId = c.Int(),
                    })
                .PrimaryKey(t => t.ClassId)
                .ForeignKey("dbo.File", t => t.FileId)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        BankName = c.String(nullable: false, maxLength: 100),
                        FromDate = c.DateTime(nullable: false, storeType: "date"),
                        ToDate = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.FileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Class", "FileId", "dbo.File");
            DropForeignKey("dbo.BalanceNumber", "ClassId", "dbo.Class");
            DropIndex("dbo.Class", new[] { "FileId" });
            DropIndex("dbo.BalanceNumber", new[] { "ClassId" });
            DropTable("dbo.File");
            DropTable("dbo.Class");
            DropTable("dbo.BalanceNumber");
        }
    }
}
