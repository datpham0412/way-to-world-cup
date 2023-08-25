using SplashKitSDK;

namespace Custom_Program
{
    /// <summary>
    /// Resort is a city without houses and has special rules
    /// </summary>
    public class TrainStation : AffordableCell
    {
        private const int MaxTrainStation = 4;
        public TrainStation(float x, float y, string name, Bitmap image, Board board) : base(x, y, name, image, board) 
        {
            _rentCosts = new int[MaxTrainStation];
        }
        public override int ActualRent 
        { 
            get
            {
                int rent = 0;
                if (BelongTo != null)
                {
                    // the number of resorts with same onwer
                    int numTrainStations = BelongTo.GetCities<TrainStation>().Count;
                    if (numTrainStations <= MaxTrainStation)
                        rent = _rentCosts[numTrainStations - 1];
                    else
                        rent = _rentCosts[MaxTrainStation - 1];
                    if (_board.WorldCupCell == this)
                        rent *= 2;
                }
                return rent; 
            }
        }
        public override string Description
        {
            get
            {
                string res = "Current Rent: " + ActualRent + "\n";
                for (int i = 0; i < _rentCosts.Length; i++)
                {
                    res += "Rent With " + i + " Train Station: " + _rentCosts[i] + "\n";
                }
                res += "Train Station Cost: " + _cost;
                return res;
            }
        }
        public override string OnCellFunction(Player player)
        {
            string res = base.OnCellFunction(player);
            if (BelongTo == player)
            {
                player.EndTurn();
                return "Cannot build more!";
            }
            return res;
        }
        public override void Load(QueryResult qr)
        {
            _cost = qr.QueryColumnForInt(3);
            _rentCosts[0] = qr.QueryColumnForInt(4);
            _rentCosts[1] = qr.QueryColumnForInt(5);
            _rentCosts[2] = qr.QueryColumnForInt(6);
            _rentCosts[3] = qr.QueryColumnForInt(7);
        }
        public override void Draw()
        {
            base.Draw();
            DrawOwner();
        }
        // draw the owner symbol of the train station
        private void DrawOwner()
        {
            if (BelongTo != null)
            {
                // the position of the center of house drawing
                float hX = X + 10;
                float hY = Y + Image.Height - 10;
                // the center of the cell
                float cX = X + Image.Width / 2;
                float cY = Y + Image.Height / 2;
                Point2D pt1 = GamingTools.FindRotatePoint(cX, cY, hX, hY - 6, Angle);
                Point2D pt2 = GamingTools.FindRotatePoint(cX, cY, hX - 6, hY + 3, Angle);
                Point2D pt3 = GamingTools.FindRotatePoint(cX, cY, hX + 6, hY + 3, Angle);
                SplashKit.FillTriangle(BelongTo.Color, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y);
            }
        }
    } 
}
