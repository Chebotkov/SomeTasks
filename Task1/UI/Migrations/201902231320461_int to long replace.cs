namespace UI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inttolongreplace : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RandomRows", "IntegerNumber", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RandomRows", "IntegerNumber", c => c.Int(nullable: false));
        }
    }
}
