using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApp4.Models
{
    public class Preference
    {
        [Key]
        public int id { get; set; }
        public string selectedFolder { get; set; }
    }

    public class PreferenceContext : DbContext
    {
        public DbSet<Preference> Preferences { get; set; }
    }
}