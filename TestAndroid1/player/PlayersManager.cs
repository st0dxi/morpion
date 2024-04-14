using Android.Graphics;

namespace TestAndroid1.player
{
    internal class PlayersManager
    {
        Player[] players;

        Player currentPlayer;

        private int currentPlayerIndex;

        public static PlayersManager instance { get; } = new PlayersManager();
        private PlayersManager()
        {
            players = new Player[2];

            players[0] = new Player("1", Color.Red);
            players[1] = new Player("2", Color.Blue);

            currentPlayerIndex = (int)(Java.Lang.Math.Random() * 2);

            currentPlayer = players[currentPlayerIndex];
        }

        public void nextPlayer()
        {
            currentPlayer = players[++currentPlayerIndex % players.Length];
        }

        public Player getCurrentPlayer()
        {
            return currentPlayer;
        }

        internal void clearGame(int startPlayer)
        {
            foreach (Player player in players)
            {
                player.resetJeu();
                if (startPlayer == -1)
                    currentPlayerIndex = (int)(Java.Lang.Math.Random() * 2);
                else
                    currentPlayerIndex = startPlayer;
                currentPlayer = players[currentPlayerIndex];
            }
        }

        public bool isComplit()
        {
            return 0b111111111 == players[0].jeu + players[1].jeu;
        }

        public void setPlayerName(int index, string name)
        {
            players[index].pseudo = name;
        }
    }
}
