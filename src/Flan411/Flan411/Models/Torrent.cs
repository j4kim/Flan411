using System;

namespace Flan411.Models
{
    public class Torrent
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
        public string Added { get; set; }
        public int Seeders { get; set; }
        public int Leechers { get; set; }
        public string Size { get; set; }
        public string CategoryName { get; set; }

        public long SizeMB { get { return Convert.ToInt64(Size) / 1000000; } }

        public override string ToString()
        {
            return Name;
        }
    }
}
