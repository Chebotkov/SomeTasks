namespace TestServer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Class")]
    public partial class Class
    {
        public int ClassId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public int? FileId { get; set; }
        public File File { get; set; }

        public virtual ICollection<BalanceNumber> Balances { get; set; }
    }
}
