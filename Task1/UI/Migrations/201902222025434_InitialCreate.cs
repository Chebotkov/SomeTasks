namespace UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RandomRows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        LatinSequence = c.String(nullable: false, maxLength: 10),
                        CyrillicSequence = c.String(nullable: false, maxLength: 10),
                        IntegerNumber = c.Int(nullable: false),
                        DoubleNumber = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RandomRows");
        }
    }
}
