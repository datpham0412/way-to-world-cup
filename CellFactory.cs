using System;
using System.Collections.Generic;
using System.Collections;
using SplashKitSDK;

namespace Custom_Program
{
    public class CellFactory
    {
        
        private Dictionary<string, Type> _cellRegistry = new(); // Dictionary for keeping all type of cells 
        private Dictionary<string, string> _imageRegistry = new(); // Dictionary for keeping all type of images 
        public void RegisterCell(string typeName, Type type) => _cellRegistry[typeName] = type; // Register cell type to create it 
        public void RegisterImg(string typeName, string filename) => _imageRegistry[typeName] = filename; // Register cell type which one suit for their bitmap
        // Create a new cell from its type name
        public Cell CreateCell(string typeName, float x, float y, string name, Board board)
        {
            Bitmap image = null;
            if (_imageRegistry[typeName] != null) // if it is not null, load it from the file
            {

                image = SplashKit.LoadBitmap(name, _imageRegistry[typeName]);
            }

            return (Cell)Activator.CreateInstance(_cellRegistry[typeName], new Object[] { x, y, name, image, board }); // use Cell class ( Cell depends on CellFactory)
        }
    }
}
