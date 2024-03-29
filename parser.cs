using Microsoft.Data.Analysis;

namespace game_pract
{
    public class Parser
    {
        string filename;
        DataFrame df;
        int LENGTH = 16598;
        
        public List<string> GenresList = new List<string>
        {
        "Sports",
        "Platform",
        "Racing",
        "Role-Playing",
        "Puzzle",
        "Misc",
        "Shooter",
        "Simulation",
        "Action",
        "Fighting",
        "Adventure",
        "Strategy"
        };

        public Parser(string filename)
        {
            this.filename = filename;
            this.df = DataFrame.LoadCsv(filename);
        }
        private void print(DataFrame input)
        {
            int colCount = input.Columns.Count;

        }

        private List<string> PublisherList()
        {
            var pubs = this.df.Columns["Publisher"].ValueCounts();
            List<string> PublisherList = new List<string>();
            foreach(string pub in pubs.Columns[0])
            {
                PublisherList.Add(pub.ToString());
            }
            return PublisherList;
        }

        public List<string> GetAllRows()
        {
            var rows = this.df.Rows;
            List<string> result = new List<string>();

            Console.WriteLine(df.Head(0));
            foreach (var row in rows) {
                var elm = row.AsEnumerable().ToList();
                string name = (string) elm[1];
                if (name.Length > 50) {
                    name = name.Substring(0, 50);
                    name += "...";
                }
                elm[1] = (object) name;

                Console.WriteLine(String.Format("{1, -60}{2, -5}{3, -5}{4, -20}{5, -30}{6, -6}{7, -6: }{8, -6: 0.0}{9, -6: 0.0}{10, -6: 0.0}", elm.ToArray()));
                result.Add(row.ToString());
            }
            return result;
        }
        public Dictionary<string,double> AvgPlatSales()
        {
            var platforms = this.df.Columns["Platform"].ValueCounts();
            Dictionary<String, double> sales = new Dictionary<String, double>();

            Console.WriteLine(String.Format("{0, -20} {1} in millions", "Platform", "Average Sales"));

            foreach (string platform in platforms.Columns[0])
            {
                var filtered =  df.Filter(df.Columns["Platform"].ElementwiseEquals(platform))["Global_Sales"];
                double sum = 0;
                foreach (string n in filtered) {
                    sum += double.Parse(n, System.Globalization.CultureInfo.InvariantCulture);
                }
                double num = filtered.Length;
                sales.Add(platform, sum / num);
                Console.WriteLine(String.Format("{0, -20} {1: 0.##} M", platform, sales[platform]));
            }
            return sales;

        }
        public void TopTenGames()
        {
            df.OrderByDescending("Global_Sales");// I hope that'll work

            //Checking if the column is already sortied
            /*
            for (int i = 0; i < df.Columns.Count;i++) 
            {
                if (Convert.ToDouble(df[i, 10]) < Convert.ToDouble(df[i + 1, 10])) { Console.WriteLine("Bloody Hell"); break; }
            }
            */
            //Output
            Console.WriteLine("\nThere's a ten best selling games around the world:\n");
            String s = String.Format("{0,-18} {1,52}\n\n", "Game", "Global Sales in millions");
            for (int i = 0; i < 10;i++)
            {
                s += String.Format("{0,-25} {1,36}\n",
                      df[i,1], df[i,10] + " M");
            }
            Console.WriteLine($"\n{s}");
        }
        
        public void ShowThisGenreGames()
        {
            ShowGenres:
            int GenresCount = 1;
            foreach(string GenreName in GenresList)
            {
                Console.WriteLine($"{GenresCount++}: {GenreName}");
            }

            Console.WriteLine("\nChoose the Genre from the list and enter the number of it\n");
            Int32 input = Int32.Parse(Console.ReadLine()!);
            Console.WriteLine("\n\n");
            if (input > 0 && input <= GenresList.Count && GenresList.Contains(GenresList[input]))
            {
                string Genre = GenresList[input-1];
                for (int i = 0; i < LENGTH; i++) 
                    if (df[i, 4].ToString() == Genre) Console.WriteLine(df[i, 1].ToString());
            }
            else { Console.WriteLine("\n\tTry again\n"); goto ShowGenres; }
        }
        public void ShowThisGenreGames(string Genre)
        {
            if (GenresList.Contains(Genre))
            {
                for (int i = 0; i < LENGTH; i++)
                    if (df[i, 4].ToString() == Genre) Console.WriteLine(df[i, 1].ToString());
            }
            else { Console.WriteLine("\n\tTry again\n"); }
        }


        public void GetCertainGames()
        {
            Console.WriteLine("\nEnter publisher name\n");
            string Publisher = Console.ReadLine()!;
            Console.WriteLine("\n");
            if (PublisherList().Contains(Publisher))
            {
                for (int i = 0; i < LENGTH; i++)
                {
                    if (this.df[i, 5].ToString() == Publisher) Console.WriteLine(this.df[i, 1].ToString());
                }
            }
            else {Console.WriteLine("\nTry again\n");}
        }

        public void GetCertainGames(string Publisher)
        {
            if (PublisherList().Contains(Publisher))
            {
                for (int i = 0; i < LENGTH; i++)
                {
                    if (this.df[i, 5].ToString() == Publisher) Console.WriteLine(this.df[i, 1].ToString());
                }
            }
            else { Console.WriteLine("\nTry again\n"); }
        }

        public void GamesRatio()
        {
            Console.WriteLine(String.Format("{0, -40} {1, -20} {2}", "Genre", "Count", "Percent"));

            foreach(string genre in GenresList)
            {
                int sum = 0;
                for(int i = 0; i < LENGTH; i++)
                {
                    if(df[i, 4].ToString() == genre.ToString())
                    {
                        sum += 1;
                    }
                }
                float per = (float)sum / (float)LENGTH * 100;
                Console.WriteLine(String.Format("{0, -40} {1, -20} {2: #.##}%", genre, sum, per));
            }
        }

    }
}