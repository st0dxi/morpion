using Android.Graphics;

namespace TestAndroid1.player
{
    internal class Player
    {
        public string pseudo { get; set; }
        public Color color { get; }

        public int nbVictoire { get; private set; }

        public int jeu { get; private set; }

        public Player(string pseudo, Color color)
        {
            this.pseudo = pseudo;
            this.color = color;
            this.jeu = 0;
        }

        public void playAtLvl(int lvl)
        {
            jeu += (0b1 << lvl);
            //Console.WriteLine("lvl : "+lvl+"a ajoute aux "+jeu+" la valeur "+(0b1<<lvl));
        }

        public void resetJeu()
        {
            jeu = 0;
        }
    }
}
