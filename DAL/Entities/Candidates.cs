using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Entities

{
    [Table("Candidates")]
    public class Candidate
    {
        [Key]
        public int ID { get; set; }
        public string  Email1     {get ; set;}
       public string  Email2     {get ; set;}
       public string   F_name    {get ; set;}
       public string  Name     {get ; set;}
       public string  L_name     {get ; set;}
       public string  Phone      { get; set; }
       public string Phone2 { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastModifiedAt { get; set; } = DateTime.Now;

        public virtual ICollection<CandedatesCvTransaction> CandedatesCvTransactions { get; set; }

    }

}
