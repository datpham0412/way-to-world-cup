using System;
using System.Collections.Generic;
using System.IO;
using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Property is a city with houses and rents
    /// </summary>
    public class Property : AffordableCell
    {
        private const int MaxHouse = 4; // Max house each player can own is 3 
        private int _type; // group that the same country property belongs to 1 player 
        private int _rentResort, _houseCost, _resortCost;
        private int _houses; // the number of houses built on the city, resort is considered the last house
        // a list that holds the number of properties in the board categorized by type
        private static Dictionary<int, int> _typeRecords = new Dictionary<int, int>();
        private void UpdateTypeRecord()
        {
            if (!_typeRecords.ContainsKey(_type))
                _typeRecords.Add(_type, 1); // add new type 
            else
                _typeRecords[_type]++; // increases the number of properties with same type by 1
        }
        
        private static Dictionary<int, string> _propertiesImgRegistry = new Dictionary<int, string>(); // a list that holds the images of Properties based on their properties' types
        public static void RegisterImg(int type, string filename) => _propertiesImgRegistry[type] = filename; // register which bitmap is for which property type

        public Property(float x, float y, string name, Bitmap img, Board board) : base(x, y, name, img, board)
        {
            _rentCosts = new int[MaxHouse];
            _houses = 0;
        }
        public int Houses
        {
            get => _houses;
            set => _houses = value; 
        }
        public int Type => _type;
        public override int Value => _cost + _houseCost * _houses;
        public override int ActualRent
        {
            get
            {
                int rent = 0;
                if (BelongTo != null)
                {
                    if (_houses == MaxHouse)
                        rent = _rentResort; // the resort renting cost
                    else
                        rent = _rentCosts[_houses]; // the house renting cost
                    // if all properties with same country belong to one owner, the rent is triple
                    if (BelongTo.GetCities<Property>().FindAll(p => p.Type == _type).Count == _typeRecords[_type])
                        rent *= 3;
                    if (_board.WorldCupCell == this) // if hosting world cup on the owned area, the renting cost will triple
                        rent *= 3;
                }
                return rent;
            }
        }
        public override string Description
        {
            get
            {
                string result = "Actual Rent: " + ActualRent + "\n";
                for (int i = 0; i < _rentCosts.Length; i++)
                {
                    result += "Rent With " + i + " Houses: " + _rentCosts[i] + "\n";
                }
                result += "Property Cost: " + _cost + "\n"
                    + "House Cost: " + _houseCost + "\n"
                    + "resort Cost: " + _resortCost;
                return result;
            }
        }
        public override string OnCellFunction(Player player)
        {
            string result = base.OnCellFunction(player);
            if (BelongTo == player)
            {
                if (_houses == MaxHouse - 1) 
                {
                    if (player.Money >= _resortCost)
                    {
                        // player can Purchase a resort 
                        player.BuildPlan(this, _resortCost);
                        return "Do you want to build a resort?\nCost: $" + _resortCost;
                    }
                    // player does not have enough money to Purchase a resort
                    player.EndTurn();
                    return player.Name + " does not have money to Purchase a resort";
                }
                else if (_houses < MaxHouse - 1)
                {
                    if (player.Money >= _houseCost)
                    {
                        // player can Purchase a house
                        player.BuildPlan(this, _houseCost);
                        return "Do you want to build a house?\nCost: $" + _houseCost;
                    }
                    // player does not have enough money to Purchase a house
                    player.EndTurn();
                    return player.Name + " does not have money to Purchase a house";
                }
                // player does not have anything to build
                player.EndTurn();
                return "Cannot build more!";
            }
            return result;
        }
        public override void Reset()
        {
            base.Reset();
            _houses = 0;
        }
        // load the properties information from the database
        public override void Load(QueryResult qr)
        {
            _type = qr.QueryColumnForInt(2);
            UpdateTypeRecord();
            _image = SplashKit.LoadBitmap(Name, _propertiesImgRegistry[_type]);
            _cost = qr.QueryColumnForInt(3);
            _rentCosts[0] = qr.QueryColumnForInt(4);
            _rentCosts[1] = qr.QueryColumnForInt(5);
            _rentCosts[2] = qr.QueryColumnForInt(6);
            _rentCosts[3] = qr.QueryColumnForInt(7);
            _rentResort = qr.QueryColumnForInt(8);
            _houseCost = qr.QueryColumnForInt(9);
            _resortCost = qr.QueryColumnForInt(10);
        }
        public override void Draw()
        {
            base.Draw();
            DrawHouse();
        }
        
        private void DrawHouse() // Draw houses on the property
        {
            if (BelongTo != null)
            {
                // the coordinate of the center of house drawing
                float hX = X + 10;
                float hY = Y + Image.Height - 10;
                // the center of the cell
                float cX = X + Image.Width / 2;
                float cY = Y + Image.Height / 2;
                if (_houses == 0) // triangle indicates the owner
                {
                    Point2D pt1 = GamingTools.FindRotatePoint(cX, cY, hX, hY - 6, Angle);
                    Point2D pt2 = GamingTools.FindRotatePoint(cX, cY, hX - 6, hY + 3, Angle);
                    Point2D pt3 = GamingTools.FindRotatePoint(cX, cY, hX + 6, hY + 3, Angle);
                    SplashKit.FillTriangle(BelongTo.Color, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y);
                }
                else if (_houses < MaxHouse) //  circle indicates the houses
                {
                    for (int i = 0; i < _houses; i++)
                    {
                        Point2D center = GamingTools.FindRotatePoint(cX, cY, hX + 12 * i, hY, Angle);
                        SplashKit.FillCircle(BelongTo.Color, center.X, center.Y, 5);
                    }
                }
                else if (_houses == MaxHouse) // square indicates the resort
                {
                    Point2D pt1 = GamingTools.FindRotatePoint(cX, cY, hX - 5, hY - 5, Angle);
                    Point2D pt2 = GamingTools.FindRotatePoint(cX, cY, hX + 5, hY - 5, Angle);
                    Point2D pt3 = GamingTools.FindRotatePoint(cX, cY, hX - 5, hY + 5, Angle);
                    Point2D pt4 = GamingTools.FindRotatePoint(cX, cY, hX + 5, hY + 5, Angle);
                    SplashKit.FillQuad(BelongTo.Color, new Quad() { Points = new Point2D[] { pt1, pt2, pt3, pt4 } });
                }
            }
        }
    }
}
