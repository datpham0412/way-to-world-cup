using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using SplashKitSDK;

/// <summary>
/// This class drawing board and their function, which player interact with
/// </summary>
/// 
namespace Custom_Program
{

    public class Board : DrawableObject, MouseClickedEvent
    {
        // in order to control the board, it has to control every cells of the board
        private List<Cell> _cells; // a list of all cells
        // Cell Factory helps create cells
        private CellFactory _cellFactory; // Factory Design Pattern for Cell
        // Draw Options for cells
        private int _width, _height; 
        private AffordableCell _worldcupCell; // the cell that is holding world cup
        private Bitmap _worldcupSymbol; // the symbol of world cup
        public Board() : base(0, 0)
        {
            _cells = new List<Cell>();
            _cellFactory = new CellFactory();
            _worldcupCell = null;
            _worldcupSymbol = null;
        }
        public int CellNumber => _cells.Count; // The number of cells that the board controls
        // get and set width of the board
        public int Width => _width;
        // The Cell Factory of the board
        public CellFactory CellFactory => _cellFactory;
        public AffordableCell WorldCupCell
        {
            get => _worldcupCell;
            set => _worldcupCell = value; 
        }
        public Bitmap WorldCupSymbol // take property of the worldcup symbol
        {
            get => _worldcupSymbol;
            set => _worldcupSymbol = value;
        }
        private void AddCell(Cell c) => _cells.Add(c); // Addding cells to the board

        // Find a cell based on its coordinate in the board
        public Cell FindCell(int coordinate)
        {
            if (coordinate < 0 || coordinate >= _cells.Count) // if out of range --> return nothing 
                return null;
            return _cells[coordinate];
        }
        public Cell FindCell<Type>() where Type : Cell => _cells.Find(c => c is Type); // Find the first cell based on its type

        public Cell ClickCell(Point2D pt) // Select a cell 
        {
            Cell result = null;
            for (int i = 0; i < _cells.Count; i++)
            {
                Cell c = _cells[i];
                c.IsClicked = c.IsAt(pt);
                if (c.IsClicked)
                    result = c;
            }
            return result;
        }

        public void Load(string filename) // Load the board setup file ( in bin > Debug > netcoreapp5.0) - BoardSetup.txt
        {
            /* 
             * Load BoardSetup.txt file ( file txt ) - ( based on DbInitializeSql.txt)
             * Board Setup in the file has information respectively:
             * - x, y position - First Cell of the Board; ( E.g : 20 , 20 - the position of the first cell)
             * - Width and Height of the board in the window ( E.g : 705 for both width and height )
             * - Amount of cell to for row and column ( 10 for each )
             * - For each cell:
             * + Type Cell ( Cell ID ) - for example : first one is the 26 - base on the SQL file 26 indicates start cell)
             * + Coordinate x,y of the cell : (615 - 615)
             * + Cell's name : (Start) 
             * - Other information for special cell:
             * + property: info about type of the property ( in DbInializeSql.txt file)
             */
            StreamReader reader = new StreamReader(filename); // read the file name 
            Database db = Database.GetDatabase();
            try
            {
                X = reader.ReadFloat();
                Y = reader.ReadFloat();
                _width = reader.ReadInteger();
                _height = reader.ReadInteger();
                int widthCells = reader.ReadInteger();
                int heightCells = reader.ReadInteger();
                int cellID;
                string name;
                float x, y;
                Cell c;
                for (int i = 0; i < (2 * (widthCells + heightCells)) - 4; i++)
                {
                    cellID = reader.ReadInteger();
                    x = reader.ReadFloat();
                    y = reader.ReadFloat();
                    name = reader.ReadLine();
                    QueryResult qr = db.Query("SELECT * FROM cells WHERE cellID = " + cellID + ";");
                    c = _cellFactory.CreateCell(qr.QueryColumnForString(1), X + x, Y + y, name, this);
                    c.Coordinate = i;
                    c.Load(qr);
                    if (i >= widthCells && i <= widthCells + heightCells - 3)  
                        c.Angle = 90; // cells in the right of the board
                    else
                    {
                        if (i >= 2 * widthCells + heightCells - 2 && i <= 2 * widthCells + 2 * heightCells - 5) // cells on the left of the board
                            c.Angle = -90;
                    }
                    // the default angle is 0
                    AddCell(c);
                }
            }
            finally
            {
                reader.Close();
            }
        }
        public override void Draw()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                Cell c = _cells[i];
                c.Draw();
            }
            // Draw the world cup symbol on selected sell
            if (_worldcupSymbol != null && _worldcupCell != null)
                SplashKit.DrawBitmapOnWindow(SplashKit.CurrentWindow(), _worldcupSymbol, _worldcupCell.X, _worldcupCell.Y, SplashKit.OptionRotateBmp(_worldcupCell.Angle));
        }
        public bool IsAt(Point2D point)
        {
            return SplashKit.PointInRectangle(
                point, new Rectangle() { X = X, Y = Y, Width = _width, Height = _height }
            );
        }
        public event EventHandler ClickEvent;
        public void OnClick(EventArgs e)
        {
            ClickEvent?.Invoke(this, e);
        }
    }












}
