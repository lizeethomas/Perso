using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite.Models
{
    [Table("cv")]
    public class CV
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("subtitle")]
        public string Subtitle { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("url")]
        public string URL { get; set; }
        [Column("begin")]
        public DateTime Begin { get; set; }
        [Column("end")]
        public DateTime End { get; set; }
        public CV()
        {
            Title = "";
            Subtitle = "";
            Description = "";
            URL = "";
            Begin = DateTime.Now;
            End = DateTime.Now;
        }
    }

    //title?:string;
    //subtitle?:string[];
    //description?:string[];
    //url?:string[];
    //dateBegin?:Date[];
    //dateEnd?:Date[];
}
