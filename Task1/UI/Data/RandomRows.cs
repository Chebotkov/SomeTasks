namespace UI.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// Represents model of the rows in database.
    /// </summary>
    public partial class RandomRows
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(10)]
        public string LatinSequence { get; set; }

        [Required]
        [StringLength(10)]
        public string CyrillicSequence { get; set; }

        public int IntegerNumber { get; set; }

        public double DoubleNumber { get; set; }
    }
}
