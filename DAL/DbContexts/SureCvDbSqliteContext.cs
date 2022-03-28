using DAL.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DAL.DbContext
{
    //SureCvDb
    public class SureCvDbSqliteContext : SureCvDbContext
    {
        // this CTOR just for migrations 
        public SureCvDbSqliteContext() : base() { }
        public SureCvDbSqliteContext(DbContextOptions opt) : base(opt)
        {
            Console.WriteLine("***************** SqliteContext" + "********");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder opt)
        {
            string path = Directory.GetCurrentDirectory();
            _ = opt.UseSqlite(@"Data Source=|DataDirectory|\\Db\\LocalSqlitDb\\surecvs.db"
                             .Replace("|DataDirectory|", path)
                             .Replace("DAL", "SureCvs"));
        }

        protected override void OnModelCreating(ModelBuilder mdBuilder)
        {
            base.OnModelCreating(mdBuilder);
            // seed for mock data in sqlite 
          
        }

        
    }
}
