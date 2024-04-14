using Android.Content;
using Android.Util;
using Android.Views;
using Java.Interop;
using TestAndroid1.player;
using GameManager = TestAndroid1.game.GameManager;

namespace TestAndroid1
{


    [Activity(Label = "menu", MainLauncher = true)]
    public class Menu : Android.App.Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_menu);

            DisplayMetrics metrics = new DisplayMetrics();

            Display.GetMetrics(metrics);

            int h = metrics.HeightPixels;
            int w = metrics.WidthPixels;


            EditText ij1 = FindViewById<EditText>(Resource.Id.nomJoueur1);
            EditText ij2 = FindViewById<EditText>(Resource.Id.nomJoueur2);

            ViewGroup.LayoutParams param = ij1.LayoutParameters;
            param.Width = w;

            param.Height = h / 10;

            ij1.LayoutParameters = param;
            ij2.LayoutParameters = param;


            TextView sj1 = FindViewById<TextView>(Resource.Id.textView1Switch);
            TextView sj2 = FindViewById<TextView>(Resource.Id.textView2Switch);

            ViewGroup.LayoutParams ps = sj1.LayoutParameters;

            ps.Width = w / 3;

            ps.Height = h / 10;

            sj1.LayoutParameters = ps;
            sj2.LayoutParameters = ps;


            Switch s = FindViewById<Switch>(Resource.Id.switchStart);
            Space ss1 = FindViewById<Space>(Resource.Id.sspace1);
            Space ss2 = FindViewById<Space>(Resource.Id.sspace2);

            ViewGroup.LayoutParams pss = s.LayoutParameters;

            pss.Width = w / 9;
            pss.Height = h / 10;

            s.LayoutParameters = pss;
            ss1.LayoutParameters = pss;
            ss2.LayoutParameters = pss;
        }

        [Export("startGame")]
        public void startGame(View view)
        {

            EditText ij1 = FindViewById<EditText>(Resource.Id.nomJoueur1);
            EditText ij2 = FindViewById<EditText>(Resource.Id.nomJoueur2);
            Switch s = FindViewById<Switch>(Resource.Id.switchStart);

            if (ij1.Text != "")
            {
                PlayersManager.instance.setPlayerName(0, ij1.Text);
            }

            if (ij2.Text != "")
            {
                PlayersManager.instance.setPlayerName(1, ij2.Text);
            }

            if (s.Checked)
            {
                GameManager.startPlayer = 1; // j2
            }else
            {
                GameManager.startPlayer = 0; // j1
            }
            if (GameManager.currentInstance != null)
            {
                GameManager.currentInstance.clearGame();
            }else
            {
                PlayersManager.instance.clearGame(GameManager.startPlayer);
            }

            Intent game = new Intent(this, typeof(MainActivity));
            StartActivity(game);
        }
    }

    [Activity(Label = "morpion", MainLauncher = false)]
    public class MainActivity : Activity
    {
        private TextView textJoueur;

        private RelativeLayout mainPage;

        protected override void OnResume()
        {
            base.OnResume();
            RefreshText();
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            DisplayMetrics metrics = new DisplayMetrics();

            Display.GetMetrics(metrics);

            int h = metrics.HeightPixels;
            int w = metrics.WidthPixels;

            Button rejouer = FindViewById<Button>(Resource.Id.rejouer);

            ViewGroup.LayoutParams paramRejouer = rejouer.LayoutParameters;

            paramRejouer.Width = w / 3;
            paramRejouer.Height = ViewGroup.LayoutParams.WrapContent;

            rejouer.LayoutParameters = paramRejouer;

            FrameLayout l1 = FindViewById<FrameLayout>(Resource.Id.frameLayout1);
            FrameLayout l2 = FindViewById<FrameLayout>(Resource.Id.frameLayout2);
            FrameLayout l3 = FindViewById<FrameLayout>(Resource.Id.frameLayout3);
            FrameLayout l4 = FindViewById<FrameLayout>(Resource.Id.frameLayout4);
            FrameLayout l5 = FindViewById<FrameLayout>(Resource.Id.frameLayout5);
            FrameLayout l6 = FindViewById<FrameLayout>(Resource.Id.frameLayout6);
            FrameLayout l7 = FindViewById<FrameLayout>(Resource.Id.frameLayout7);
            FrameLayout l8 = FindViewById<FrameLayout>(Resource.Id.frameLayout8);
            FrameLayout l9 = FindViewById<FrameLayout>(Resource.Id.frameLayout9);

            mainPage = FindViewById<RelativeLayout>(Resource.Id.AllPage);

            textJoueur = FindViewById<TextView>(Resource.Id.joueurText);

            IDictionary<FrameLayout, int> map = new Dictionary<FrameLayout, int>();

            map.Add(l1, 0);
            map.Add(l2, 1);
            map.Add(l3, 2);
            map.Add(l4, 3);
            map.Add(l5, 4);
            map.Add(l6, 5);
            map.Add(l7, 6);
            map.Add(l8, 7);
            map.Add(l9, 8);

            foreach (KeyValuePair<FrameLayout, int> item in map)
            {
                ViewGroup.LayoutParams param = item.Key.LayoutParameters;
                param.Width = Math.Min(w / 3 - 10, h / 3 - 10);
                param.Height = Math.Min(w / 3 - 10, h / 3 - 10);
                item.Key.LayoutParameters = param;
                item.Key.Background = GameManager.DEFAULT_COLOR;
            }

            GameManager.init(map);
            RefreshText();
        }

        [Export("rejouer")]
        public void rejouer(View view)
        {
            GameManager.currentInstance.clearGame();
            RefreshText();
        }

        [Export("morpionClick")]
        public void morpionClick(View view)
        {
            FrameLayout cliker = (FrameLayout)view;

            if (GameManager.currentInstance.victory)
            {
                return;
            }

            if (GameManager.currentInstance.playFrame(cliker))
            {

                if (GameManager.currentInstance.victory)
                {
                    textJoueur.Text = "victoire de : " + PlayersManager.instance.getCurrentPlayer().pseudo;
                }
                else
                {
                    PlayersManager.instance.nextPlayer();
                    RefreshText();
                }
            }
        }
        private void RefreshText()
        {
            if (GameManager.currentInstance.victory)
            {
                textJoueur.Text = "victoire de : " + PlayersManager.instance.getCurrentPlayer().pseudo;
            }else if (PlayersManager.instance.isComplit())
            {
                textJoueur.Text = "egalité";
            }
            else
            {
                textJoueur.Text = "tours de : " + PlayersManager.instance.getCurrentPlayer().pseudo;

                mainPage.SetBackgroundColor(PlayersManager.instance.getCurrentPlayer().color);
            }
        }
    }
}