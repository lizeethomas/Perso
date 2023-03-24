namespace JDR.Models
{
    public class Properties
    {
        public Properties(int combativité, int survie, int erudition, int mental)
        {
            Combativité = combativité;
            Survie = survie;
            Erudition = erudition;
            Mental = mental;
        }

        public int Combativité { get; set; }
        public int Survie { get; set; }
        public int Erudition { get; set; }
        public int Mental { get; set; } 


    }
}
