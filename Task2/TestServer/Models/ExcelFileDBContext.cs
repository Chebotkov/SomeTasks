namespace TestServer.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ExcelFileDBContext : DbContext
    {
        public ExcelFileDBContext()
            : base("name=ExcelFileDBContext")
        {
        }

        public virtual DbSet<BalanceNumber> BalanceNumber { get; set; }
        public virtual DbSet<Class> Class { get; set; }
        public virtual DbSet<File> File { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BalanceNumber>()
                .Property(e => e.AssetBalance)
                .HasPrecision(28, 9);

            modelBuilder.Entity<BalanceNumber>()
                .Property(e => e.PassiveBalance)
                .HasPrecision(28, 9);

            modelBuilder.Entity<BalanceNumber>()
                .Property(e => e.AssetOutgoingBalance)
                .HasPrecision(28, 9);

            modelBuilder.Entity<BalanceNumber>()
                .Property(e => e.PassiveOutgoingBalance)
                .HasPrecision(28, 9);

            modelBuilder.Entity<BalanceNumber>()
                .Property(e => e.TurnoverLoan)
                .HasPrecision(28, 9);

            modelBuilder.Entity<BalanceNumber>()
                .Property(e => e.TurnoverDebit)
                .HasPrecision(28, 9);
        }
    }
}
