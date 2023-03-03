using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebsite.Models
{
    [Table("pictures")]
    public class Picture
    {
        private int id;
        private string title;
        private string description;
        private string pictureUrl;

        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("pictureUrl")]
        public string PictureUrl { get; set;}

    }
}
