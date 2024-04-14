using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using TestAndroid1.player;

namespace TestAndroid1.game
{
    internal class GameManager
    {
        readonly public static ColorDrawable DEFAULT_COLOR = new ColorDrawable(Color.Gray);
        public static GameManager currentInstance { get; private set; }

        public bool victory { get; private set; }

        public static int startPlayer { get; set; } = -1;

        private IDictionary<FrameLayout, int> map;

        private static int[] victoryCondition { get; } =
            [0b111000000, 0b000111000, 0b000000111, 0b100100100,
            0b010010010, 0b001001001, 0b100010001, 0b001010100];

        public GameManager(IDictionary<FrameLayout, int> map)
        {
            this.map = map;
            victory = false;
        }

        public static void init(IDictionary<FrameLayout, int> map)
        {
            currentInstance = new GameManager(map);
        }

        public bool playFrame(FrameLayout frame)
        {
            if (victory)
            {
                return false;
            }

            int lvl;

            if (map.TryGetValue(frame, out lvl))
            {
                if (frame.Background.Equals(DEFAULT_COLOR))
                {
                    frame.Background = new ColorDrawable(PlayersManager.instance.getCurrentPlayer().color);

                    PlayersManager.instance.getCurrentPlayer().playAtLvl(lvl);

                    if (haveWin(PlayersManager.instance.getCurrentPlayer().jeu))
                    {
                        victory = true;
                    }
                    return true;
                }
            }
            else
            {
                Log.Warn("parameterError", "la frame n'est pas dans la map");
            }
            return false;
        }

        public bool haveWin(int jeu)
        {
            foreach (int condition in victoryCondition)
            {
                if (condition == (jeu & condition))
                {
                    return true;
                }
            }
            return false;
        }

        public void clearGame()
        {
            foreach (KeyValuePair<FrameLayout, int> item in map)
            {
                item.Key.Background = DEFAULT_COLOR;
            }
            PlayersManager.instance.clearGame(startPlayer);
            victory = false;
        }
    }
}
