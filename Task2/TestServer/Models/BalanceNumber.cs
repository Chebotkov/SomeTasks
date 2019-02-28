namespace TestServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BalanceNumber")]
    public partial class BalanceNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BalanceId { get; set; }

        public decimal AssetBalance { get; set; }

        public decimal PassiveBalance { get; set; }

        public decimal AssetOutgoingBalance { get; set; }

        public decimal PassiveOutgoingBalance { get; set; }

        public decimal TurnoverLoan { get; set; }

        public decimal TurnoverDebit { get; set; }

        public int? ClassId { get; set; }

        public Class Class { get; set; }
    }
}
