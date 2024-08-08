using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Contains main game logics including update, input handle and draw
    /// </summary>
    public class GameImplementation
    {
       
        private readonly Color[] PlayerColor = new Color[4] // the color assigned to players
        {
            Color.Black, // Player 1 
            Color.Blue, // Player 2 
            Color.Yellow, // Player 3 
            Color.Green, // Player 4
        };
        private int _turn; // indicate the player's turn
        private Bitmap _sideBarImage; // the side bar bitmap that represents a chosen cell
        private MouseInputManager _MIManager; // mouse click manager
        private GameNotifications _notiBox; // the main noti box appear in the board
        private GameNotifications _sideNotiBox; // the side noti box appear in the side of the board
        private List<Player> _players = new List<Player>(); // list of players
        private Board _board; // the main game board
        private Dice _dice1; // dice used for game
        private Dice _dice2;
        private const int LapMoney = 500; // Cross the starting line to earn $500

        public GameImplementation(List<String> players)
        {
            Database db = Database.GetDatabase();
            try
            {
                // Initialize fonts
                SplashKit.LoadFont("GameFont", "Bangers.ttf");
                SplashKit.LoadFont("CellFont", "OpenSans.ttf");

                // Initialize turn
                _turn = 0;

                // Initialize side bar image
                _sideBarImage = null;

                // Initialize mouse input manager
                _MIManager = new MouseInputManager();

                // Initialize dices
                Bitmap diceImg = SplashKit.LoadBitmap("DiceImg", "dice.png");
                SplashKit.BitmapSetCellDetails(diceImg, 136, 145, 6, 1, 6);
                AnimationScript diceScript = SplashKit.LoadAnimationScript("DiceScript", "dice.txt");
                _dice1 = new Dice(224, 250, diceImg);
                _dice1.Anim = diceScript.CreateAnimation("DiceRoll");
                _dice2 = new Dice(395, 250, diceImg);
                _dice2.Anim = diceScript.CreateAnimation("DiceRoll");

                // Initialize property images
                Property.RegisterImg(1, "Bangladesh.png");
                Property.RegisterImg(2, "Korea.png");
                Property.RegisterImg(3, "Vietnam.png");
                Property.RegisterImg(4, "AUS.png");
                Property.RegisterImg(5, "UK.png");
                Property.RegisterImg(6, "France.png");
                Property.RegisterImg(7, "USA.png");
                Property.RegisterImg(8, "CAN.png");

                // Initialize mysterious cards and cards
                Mystery.AddCard<MoneyCard>();
                Mystery.AddCard<MoveCard>();

                // Initialize board and cells
                _board = new Board();
                _board.CellFactory.RegisterCell("property", typeof(Property));
                _board.CellFactory.RegisterCell("start", typeof(Start));
                _board.CellFactory.RegisterCell("jail", typeof(Jail));
                _board.CellFactory.RegisterCell("worldcup", typeof(WorldCup));
                _board.CellFactory.RegisterCell("tax", typeof(Tax));
                _board.CellFactory.RegisterCell("mystery", typeof(Mystery));
                _board.CellFactory.RegisterCell("airport", typeof(Airport));
                _board.CellFactory.RegisterCell("trainstation", typeof(TrainStation));
                _board.CellFactory.RegisterImg("start", "start.png");
                _board.CellFactory.RegisterImg("property", null);
                _board.CellFactory.RegisterImg("jail", "jail.png");
                _board.CellFactory.RegisterImg("worldcup", "worldcup.png");
                _board.CellFactory.RegisterImg("tax", "tax.png");
                _board.CellFactory.RegisterImg("mystery", "mystery.png");
                _board.CellFactory.RegisterImg("airport", "airport.png");
                _board.CellFactory.RegisterImg("trainstation", "trainstation.png");
                _board.Load("BoardSetup.txt");
                _board.ClickEvent += (object sender, EventArgs e) => BoardClickHandle();
                _MIManager.Add(_board);

                // Initialize world cup symbol
                _board.WorldCupSymbol = SplashKit.LoadBitmap("worldcup symbol", "worldcup_symbol.png");

                // Initialize players
                List<String> playerNames = players;
                for (int i = 0; i < playerNames.Count; i++)
                {
                    Player p = new Player(0, 0, 10, playerNames[i], PlayerColor[i], 3000, i);
                    p.MoveTo(_board, _board.FindCell<Start>());
                    _players.Add(p);
                }
                _players[_turn].IsInTurn = true;
                _players[_turn].RollPlan(_dice1, _dice2);
                Player.SubscribeToMIManager(_MIManager);

                // Initialize message box and side message box
                _notiBox = new GameNotifications(_board.X, 185, _board.Width, 25);
                _sideNotiBox = new GameNotifications(749, 382, 425, 20);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                // Free Database
                db.FreeDB();
            }
        }
        public void Update()
        {
            if (PlayersLeft.Count <= 1) // the one who not bankrupt at the end is the winner
            {
                _notiBox.Noti = "Congratulations\n" + PlayersLeft[0].Name + " Wins!";
                _dice1.Deactivated = true;
                _dice2.Deactivated = true;
                Player.DeactivateAllActions(); 
            }
            else
            {
                if (!_players[_turn].IsInTurn) //when a player closes a turn by defaulting to or clicking anything
                {
                    NewTurn();
                }
                
                if (_dice1.EndRolling() && _dice2.EndRolling()) // Player moves to the new cell and plays the game according to the rules after the dice stop rolling.
                {
                    InTurn();
                }
            }
        }
        public void Draw()
        {
            SplashKit.DrawLine(Color.White, 749, 238, 1174, 238);
            SplashKit.DrawText("Click the cell for more information", Color.White, "GameFont", 25, 810, 240);
            if (_sideBarImage != null)
                SplashKit.DrawBitmap(_sideBarImage, 749 + (425 - _sideBarImage.Width) / 2, 280);
            // Draw basic objects
            _board.Draw();
            _dice1.Draw();
            _dice2.Draw();
            // Draw the player information
            for (int i = 0; i < _players.Count; i++)
            {
                if (!_players[i].IsBankrupt)
                    _players[i].Draw();
                SplashKit.DrawText(_players[i].Description, Color.White, "GameFont", 25, 760, 25 + i * 50); // Draw player's information
                SplashKit.FillCircle(PlayerColor[i], 1100, 35 + i * 50, 10);
            }

            // Insert the noti text box
            _notiBox.Draw();
            _sideNotiBox.Draw();
        }
        public void HandleInput()
        {
            if (PlayersLeft.Count > 1)
                _MIManager.NotifyObservers();
        }
        public List<Player> PlayersLeft => _players.FindAll(p => !p.IsBankrupt); // check how many players left in game // check Game is over

        public Color[] PlayerColor1 => PlayerColor;

        // deal with actions when interacting with the board
        private void BoardClickHandle()
        {

            Cell c = _board.ClickCell(SplashKit.MousePosition());
            if (c == null)
            {
                _sideNotiBox.Noti = "";
                _sideBarImage = null;
            }
            else
            {
                // the side noti box shows the cell description
                _sideNotiBox.Noti = c.Description;
                _sideBarImage = c.Image;
                Player curPlayer = _players[_turn];
                if (curPlayer.IsSelling) // when player is selling some cities
                {
                    BoardClickSell(curPlayer);
                }
                if (curPlayer.IsChoosingWorldCup)
                {
                    BoardClickWorldCup(curPlayer);
                }
            }
        }
        private void BoardClickSell(Player player) // selling logic when board is clicked
        {
            player.UpdateTemporaryCities();
            // update the noti box
            string[] notiLines = _notiBox.Noti.Split("\n");
            _notiBox.Noti = _notiBox.Noti.Replace(notiLines[notiLines.Length - 1], "Sell Total: " + player.SellTotal + "$");
        }
        // world cup logic when board is clicked
        private void BoardClickWorldCup(Player player)
        {
            player.UpdateTemporaryCities();

            if (player.TemporaryCities.Count == 1)
            {
                _board.WorldCupCell = player.TemporaryCities[0];
                _notiBox.Noti = "World Cup is being held in " + _board.WorldCupCell.Name;
                player.EndTurn();
            }
        }
        private void NewTurn() // handle the changeover between the previous and subsequent turns.
        {
            GamingTools.DisplayDelay(1000); // The time to keep the message on the screen ( 5000 a little bit long so the user able to read the message if they step on the mysterious card) 
            _notiBox.Noti = "";
            if (_players[_turn].SameDice != 0) // if the player roll the same dice
                _notiBox.Noti = _players[_turn].Name + " rolled the same dice\n";
            else
            {
                do
                {
                    _turn = (_turn + 1) % _players.Count;
                } while (_players[_turn].IsBankrupt);
            }
            Player curPlayer = _players[_turn];
            curPlayer.IsInTurn = true;
            _notiBox.Noti += curPlayer.Name + "'s turn";
            if (curPlayer.TurnsInJail > 0)
            {
                // if the player is in jail
                curPlayer.JailPlan(_dice1, _dice2);
            }
            else
            {
                _dice1.Deactivated = false;
                _dice2.Deactivated = false;
                curPlayer.RollPlan(_dice1, _dice2);
            }

        }
        private void InTurn()  // When rolling finishes, take appropriate action (in-turn)
        {
            Player curPlayer = _players[_turn];
            Cell tempCell = null;
            
            if (_dice1.Value != _dice2.Value)
                curPlayer.SameDice = 0;
            else
                curPlayer.SameDice++; // if the dice number is the same, player can keep moving
            if (curPlayer.SameDice == 3)  // if the number of times that dices have the same number is 3, player goes to jail
            {
                _notiBox.Noti = curPlayer.Name + " goes to jail because of 3 same number rolls";
                SplashKit.Delay(2);
                curPlayer.SameDice = 0;
                curPlayer.TurnsInJail = 3;
                curPlayer.MoveTo(_board, _board.FindCell<Jail>());
                curPlayer.IsInTurn = false;
            }
            else if (curPlayer.TurnsInJail > 0 && curPlayer.SameDice > 0)
            {
                // if the player is in jail but rolls the same dice
                _notiBox.Noti = curPlayer.Name + " has got out of jail!";
                GamingTools.DisplayDelay(2000);  
                curPlayer.TurnsInJail = 0;
                curPlayer.IsInTurn = false;
            }
            else if (curPlayer.TurnsInJail > 0 && curPlayer.SameDice == 0)
            {
                // if the player is in jail but does not roll the same dice
                _notiBox.Noti = curPlayer.Name + " has " + curPlayer.TurnsInJail + " turns to get out of jail!";
                GamingTools.DisplayDelay(2000);
                curPlayer.TurnsInJail--;
                curPlayer.IsInTurn = false;
            }
            else
            {
                // the movement of the current player
                for (int i = 1; i <= (_dice1.Value + _dice2.Value); i++) 
                {
                    tempCell = _board.FindCell((curPlayer.Coordinate + 1) % _board.CellNumber);
                    curPlayer.MoveTo(_board, tempCell);
                    AddLapMoney(curPlayer);
                    GamingTools.DisplayDelay(400, curPlayer.Draw); // Delay for moving the dot of the player ( Time movement)
                }
                // update noti box
                _notiBox.Noti = tempCell.OnCellFunction(curPlayer);
            }
            // reset the dice
            _dice1.Reset();
            _dice2.Reset();
        }

        
        private void AddLapMoney(Player player) // player earns money after a lap (go through start)
        {
            if (player.Coordinate == 0) // Start line 
                player.Money += LapMoney;
        }
    }
}