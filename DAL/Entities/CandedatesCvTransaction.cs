using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("CandedatesCvTransactions")]
    public class CandedatesCvTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransId { get; set; }
        public int Candedateid { get; set; }
        public string Candedatename { get; set; }
        public string AddedFileText { get; set; }
        public string Transdate { get; set; }
        public string AddedFilePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        [ForeignKey("Candedateid")]
        public virtual Candidate Candidate { get; set; }

    }

}
