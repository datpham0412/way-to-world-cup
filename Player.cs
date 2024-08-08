using System;
using System.Collections.Generic;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Represents the player on the board
    /// </summary>
    public class Player : DrawableObject
    {
        private const int JailFee = 100; // Fee to escape jail 
        private readonly int[,] PlayerPosOnCell = new int[4, 2]
        {
            {15, 15}, // coordinate of the point ( red point on the cell ) - Player 1
            {15, -15}, // Coordinate of the point ( blue point on the cell ) - Player 2
            {-15, 15}, // Coordinate of the point ( yellow point on the cell ) - Player 3
            {-15, -15} // Coordinate of the point ( green point on the cell ) - Player 4
        };
        // contains the in-game actions of the player, each corresponds a button 
        private readonly static Dictionary<string, TemporaryButton> _actions = new Dictionary<string, TemporaryButton>()
        {
            { "Purchase", new TemporaryButton(204, 350, 100, 50, "Purchase", 25, Color.LightPink, Color.HotPink, Color.SteelBlue) },
            { "Build", new TemporaryButton(204, 350, 100, 50, "Build", 25, Color.LightPink, Color.HotPink, Color.SteelBlue) },
            { "Skip", new TemporaryButton(442, 350, 100, 50, "Skip", 25, Color.LightPink, Color.HotPink, Color.SteelBlue) },
            { "Sell", new TemporaryButton(323, 350, 100, 50, "Sell", 25, Color.LightPink, Color.HotPink, Color.SteelBlue) },
            { "Pay", new TemporaryButton(305, 370, 140, 50, "Pay", 50, Color.LightPink, Color.HotPink, Color.SteelBlue) },
            { "Roll", new TemporaryButton(305, 485, 140, 50, "Roll", 50, Color.LightPink, Color.HotPink, Color.SteelBlue) }
        };
        // get the action 
        public static TemporaryButton GetAction(string name)
        {
            if (_actions.ContainsKey(name))
                return _actions[name];
            return null;
        }
        // deactivate all action buttons
        public static void DeactivateAllActions() 
        {
            foreach (TemporaryButton button in _actions.Values)
            {
                button.Deactivated = true;
            }
        }
        
        public static void SubscribeToMIManager(MouseInputManager m) //subscribe mouse input manager action buttons
        {
            foreach (TemporaryButton button in _actions.Values)
                m.Add(button);
        }
        private int _money; // the current money
        private int _coordinate; // the player's coordinate on the board
        private int _playerNum; // the player's number 
        private string _name; // the player's name
        private float _radius; // radius for player drawing
        private Color _color; // color that represents the player
        private bool _isInTurn; // check if the player is in turn 
        private bool _isSelling; // check if the player is selling some citys
        private bool _isBankrupt; // check if the player has been bankrupt
        private bool _isChoosingWorldCup; // check if the player is choosing festival
        private int _turnsInJail; // check if the player is in jail
        private int _sameDice; // the number of times that two dices have the same number
        private List<AffordableCell> _cities = new List<AffordableCell>();// the cities that belong to the player
        private List<AffordableCell> _temporaryCities = new List<AffordableCell>(); // the citys that are for later use 
        public Player(float x, float y, float radius, string name, Color color, int money, int playerNum) : base(x, y)
        {
            _money = money;
            _coordinate = 0;
            _radius = radius;
            _name = name;
            _color = color;
            _isInTurn = false;
            _isSelling = false;
            _isBankrupt = false;
            _turnsInJail = 0;
            _sameDice = 0;
            _playerNum = playerNum;
        }
        // add city to _citys
        public void AddCity(AffordableCell city)
        {
            _cities.Add(city);
            city.BelongTo = this;
        }
        public int CityCount => _cities.Count; // get number of citys owned
        
        public List<Type> GetCities<Type>() where Type : AffordableCell // get city by type 
        {
            List<Type> result = new List<Type>();
            for (int i = 0; i < _cities.Count; i++)
            {
                AffordableCell city = _cities[i];
                if (city is Type)
                    result.Add((Type)city);
            }
            return result ;
        }
        public void UpdateTemporaryCities() // update the _temporaryCities
        {
            for (int i = 0; i < _cities.Count; i++)
            {
                AffordableCell city = _cities[i];
                if (city.IsClicked)
                {
                    if (!_temporaryCities.Contains(city))
                        _temporaryCities.Add(city);
                    else
                        _temporaryCities.Remove(city);
                }
            }
        }
        public List<AffordableCell> TemporaryCities => _temporaryCities; // show the temporary cities
        public int Coordinate // Coordinate of the player ( Exact position of the dot on cell ( 0 is the start line))
        {
            get => _coordinate;
            set => _coordinate = value;
        }
        public int Money
        {
            get => _money;
            set => _money = value;
        }
        public string Name => _name;
        public Color Color => _color;
        public string Description
        {
            get
            {
                if (_isBankrupt)
                    return _name + ": Bankrupt";
                return _name + ": $" + _money;
            }
        }
        
        public int SellTotal // the total value of _temporaryCities
        {
            get
            {
                int total = _money;
                foreach (AffordableCell city in _temporaryCities)
                {
                    total += city.Value;
                }
                return total;
            }
        }
        public int TotalMoney // The total value of current money and all cities
        {
            get
            {
                int total = _money;
                foreach (AffordableCell city in _cities)
                    total += city.Value;
                return total;
            }
        }
        public bool IsInTurn
        {
            get => _isInTurn; 
            set =>  _isInTurn = value; 
        }
        public bool IsSelling
        {
            get => _isSelling; 
            set  => _isSelling = value; 
        }
        public bool IsChoosingWorldCup
        {
            get =>  _isChoosingWorldCup; 
            set => _isChoosingWorldCup = value; 
        }
        public bool IsBankrupt
        {
            get =>  _isBankrupt; 
            set => _isBankrupt = value; 
        }
        public int TurnsInJail
        {
            get =>  _turnsInJail; 
            set => _turnsInJail = value; 
        }
        public int SameDice
        {
            get =>  _sameDice; 
            set =>  _sameDice = value; 
        }
        public void MoveTo(Board board, Cell c) // the player moves to another cell on the board
        {
            _coordinate = c.Coordinate;
            float playerX = (c.X + c.Image.Width / 2) + PlayerPosOnCell[_playerNum, 0];
            float playerY = (c.Y + c.Image.Height / 2) + PlayerPosOnCell[_playerNum, 1];
            X = playerX;
            Y = playerY;
        }
        public void Purchase(AffordableCell c) // player purchase a city
        {
            _money -= c.Cost;
            AddCity(c);
        }
        public void PurchasePlan(AffordableCell c) // plan of Purchaseing cities
        {
            var purchaseBtn = _actions["Purchase"];
            var skipBtn = _actions["Skip"];
            purchaseBtn.Deactivated = false;
            skipBtn.Deactivated = false;
            EventHandler onPurchase = (object sender, EventArgs e) =>
            {
               Purchase(c);   // use Event to Implement the Purchase option
                EndTurn();
            };
            EventHandler onSkip = (object sender, EventArgs e) => EndTurn();
            EventHandler removeEvent = (object sender, EventArgs e) => // Remove these events ( after implementing Skip or Purchase, it will remove this action )
            {
                purchaseBtn.ClickEvent -= onPurchase;
                skipBtn.ClickEvent -= onSkip;
            }; 
            purchaseBtn.ClickEvent += onPurchase;
            skipBtn.ClickEvent += onSkip;
            purchaseBtn.ClickEvent += removeEvent;
            skipBtn.ClickEvent += removeEvent;
        }
        // player build a house on a property
        public void Build(Property c, int cost)
        {
            _money -= cost;
            c.Houses += 1;
        }
        // Plan of building a house
        public void BuildPlan(Property c, int cost)
        {
            var buildBtn = _actions["Build"];
            var skipBtn = _actions["Skip"];
            buildBtn.Deactivated = false;
            skipBtn.Deactivated = false;
            EventHandler onBuild = (object sender, EventArgs e) =>
            {
                Build(c, cost);
                EndTurn();
            };
            EventHandler onSkip = (object sender, EventArgs e) => EndTurn();
            EventHandler removeEvent = (object sender, EventArgs e) =>
            {
                buildBtn.ClickEvent -= onBuild;
                skipBtn.ClickEvent -= onSkip;
            };
            buildBtn.ClickEvent += onBuild;
            skipBtn.ClickEvent += onSkip;
            buildBtn.ClickEvent += removeEvent;
            skipBtn.ClickEvent += removeEvent;
        }
        public void Sell() // player sells multiple cities
        {
            foreach (AffordableCell city in _temporaryCities)
            {
                _money += city.Value;
                city.Reset();
                _cities.Remove(city);
            }
        }
        // player pays money to other player
        public void Pay(Player receiver, int loan)
        {
            _money -= loan;
            if (receiver != null) // receiver can be null e.g when pay tax
                receiver.Money += loan;
        }
        public void SellPlan(Player receiver, int loan) // plan of selling multiple citys
        {
            var sellBtn = _actions["Sell"];
            sellBtn.Deactivated = false;
            EventHandler onSell = (object sender, EventArgs e) =>
            {
                if (SellTotal >= loan)
                {
                    Sell();
                    Pay(receiver, loan);
                    EndTurn();
                }
                else
                {
                    SplashKit.DisplayDialog("", "Beyond player's affordable", SplashKit.FontNamed("CellFont"), 15);
                    SellPlan(receiver, loan);
                }
            };
            sellBtn.ClickEvent += onSell;
            sellBtn.ClickEvent += (object sender, EventArgs e) => sellBtn.ClickEvent -= onSell;
        }
        
        public void JailPlan(Dice dice1, Dice dice2) // Decide player gonna get out of the jail or not // Plan of escaping jail
        {
            if (_money < JailFee)
            {
                dice1.Deactivated = false;
                dice2.Deactivated = false;
                RollPlan(dice1, dice2);
            }
            else
            {
                var payBtn = _actions["Pay"]; // Pay button
                var rollBtn = _actions["Roll"]; // Roll button
                payBtn.Deactivated = false;
                rollBtn.Deactivated = false;
                EventHandler onPay = (object sender, EventArgs e) =>
                {
                    _money -= JailFee;
                    _turnsInJail = 0;
                    payBtn.Deactivated = true;
                    dice1.Deactivated = false;
                    dice2.Deactivated = false;
                };
                EventHandler onRoll = (object sender, EventArgs e) =>
                {
                    payBtn.Deactivated = true;
                    payBtn.ClickEvent -= onPay;
                    dice1.Deactivated = false;
                    dice2.Deactivated = false;
                    dice1.Roll();
                    dice2.Roll();
                    rollBtn.Deactivated = true;
                };
                payBtn.ClickEvent += onPay;
                rollBtn.ClickEvent += onRoll;
                payBtn.ClickEvent += (object sender, EventArgs e) => payBtn.ClickEvent -= onPay;
                rollBtn.ClickEvent += (object sender, EventArgs e) => rollBtn.ClickEvent -= onRoll;
            }
        }
        
        public void RollPlan(Dice dice1, Dice dice2) // Plan of roll the dice
        {
            var btn = _actions["Roll"];
            btn.Deactivated = false;
            EventHandler onBtnClick = (object sender, EventArgs e) =>
            {
                dice1.Roll();
                dice2.Roll();
                btn.Deactivated = true;
            };
            btn.ClickEvent += onBtnClick;
            btn.ClickEvent += (object sender, EventArgs e) => btn.ClickEvent -= onBtnClick;
        }
        public void EndTurn() // player ends the turn
        {
            DeactivateAllActions();
            _isInTurn = false;
            _temporaryCities.Clear();
            _isChoosingWorldCup = false;
            _isSelling = false;
        }
        public void SellAll() // player sells all cities
        {
            foreach (AffordableCell city in _cities)
            {
                _money += city.Value;
                city.Reset();
            }
            _cities.Clear();
        }
        public override void Draw()
        {
            SplashKit.FillCircle(_color, X, Y, _radius);
            // Draw Action Buttons
            foreach (Button btn in _actions.Values)
                btn.Draw();
            // Draw Outline for selected cities
            foreach (AffordableCell c in _temporaryCities)
                c.DrawOutline(Color.HotPink);
        }
    }
}
