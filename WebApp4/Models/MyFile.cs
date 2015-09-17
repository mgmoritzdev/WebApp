using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApp4.Models
{
    public class MyFile
    {
        [Key]
        public int id { get; set; }
        public string dir { get; set; }
        public string name { get; set; }
        public int size { get; set; }
    }

    public class ConfirmMove
    {
        [Key]
        public int id { get; set; }
        public int myFileId { get; set; }
        public bool confirmed { get; set; }
    }

    public class MyFileDbContext : DbContext
    {
        public DbSet<MyFile> MyFiles { get; set; }
        public DbSet<ConfirmMove> ConfirmMoves { get; set; }
    }
}