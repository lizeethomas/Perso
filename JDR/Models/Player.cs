using JDR.DTOs;

namespace JDR.Models
{
    public class Player
    {
        public Player(int id, string name, int age, Properties properties)
        {
            Id = id;
            Name = name;
            Age = age;
            Properties = properties;
        }

        public int Id { get; set; } 
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Money { get; set; }
        public List<Item> Items { get; set; }
        public Properties Properties { get; set; }

        public PlayerDTO getInfo()
        {
            var info = new PlayerDTO();
        }

    }
}
