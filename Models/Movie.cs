using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EngineerTest.Models
{
    public class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public int Id { get; set; }
        public double VoteAverage { get; set; }
        public double Popularity { get; set; }
        public string PosterPath { get; set; }
        public string OriginalLanguage { get; set; }
        public string Overview { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsTopRated { get; set; }
        public DateTime	SysDatetime { get; set; }
    }
}
