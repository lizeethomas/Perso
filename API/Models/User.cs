using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace MyWebsite.Models
{
    [Table("users")]
    public class User
    {
        private int id;
        private string username;
        private string email;
        private string password;

        [Column("id")]
        public int Id
        {
            get => id;
            set => id = value;
        }

        [Column("username")]
        public string Username
        {
            get => username;
            set => username = value ?? throw new ArgumentNullException(nameof(username));
        }

        [Column("email")]
        public string Email
        {
            get => email;
            set => email = value ?? throw new ArgumentNullException(nameof(email));
        }

        [Column("password")]
        public string Password
        {
            get => password;
            set => password = value ?? throw new ArgumentNullException(nameof(password));
        }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [Column("role_id")]
        public int RoleId { get; set; }

        public User()
        {
        }
    }
}
