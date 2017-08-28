using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    class FileService
    {
        private string _path;

        public FileService(string path)
        {
            _path = path;
        }

        public void Write(List<Cell> generation)
        {
            using (StreamWriter sw = File.AppendText(_path))
            {
                foreach (var cell in generation)
                {
                    sw.Write($"{cell.X} {cell.Y}&");
                }
                sw.WriteLine();
            }
        }

        public List<Cell> Read()
        {
            List<Cell> result = new List<Cell>();
            using (StreamReader sr = File.OpenText(_path))
            {
                string row = string.Empty;
                while ((row = sr.ReadLine()) != null)
                {
                    var cells = row.Split('&');
                    foreach(var cell in cells)
                    {
                        var points = cell.Split(' ');
                        if(points.Count() > 1)
                        {
                            result.Add(new Cell()
                            {
                                X = int.Parse(points[0]),
                                Y = int.Parse(points[1]),
                                Neighbours = 0,
                                State = State.ALive
                            });
                        }
                    }
                }
                return result;
            }
        }
    }
}
