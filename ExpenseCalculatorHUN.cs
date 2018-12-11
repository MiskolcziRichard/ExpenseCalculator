using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kitchen_Buddy
{
    class Program
    {
        static void Main(string[] args)
        {
            Welcome();

            //Szükséges értéktömbök létrehozása:


            //Termékek nevei:
            string[] shoppingList = new string[100];
            string[] fridgeHealthyContent = new string[100];
            string[] aboutToExpire = new string[100];

            //Termékek lejárati dátuma:
            DateTime[] shoppingListDate = new DateTime[100];
            DateTime[] fridgeHealthContentDate = new DateTime[100];
            DateTime[] aboutToExpireDate = new DateTime[100];

            //Termékek számai:
            int shoppingListCount = 0;
            int fridgeHealthyContentCount = 0;
            int aboutToExpireCount = 0;

            LoadSave(ref shoppingList, ref fridgeHealthyContent, ref aboutToExpire,
                ref shoppingListDate, ref fridgeHealthContentDate, ref aboutToExpireDate,
                ref shoppingListCount, ref fridgeHealthyContentCount, ref aboutToExpireCount);


            bool loop = true;

            do
            {
                AutoSave(ref shoppingList, ref fridgeHealthyContent, ref aboutToExpire,
                ref shoppingListDate, ref fridgeHealthContentDate, ref aboutToExpireDate,
                ref shoppingListCount, ref fridgeHealthyContentCount, ref aboutToExpireCount);

                Console.Clear();

                Stats(shoppingList, fridgeHealthyContent, aboutToExpire,
                shoppingListDate, fridgeHealthyContentDate, aboutToExpireDate,
                shoppingListCount, fridgeHealthyContentCount, aboutToExpireCount);

                Help();

                Console.Write("Parancs: ");

                string userInput = Console.ReadLine().ToLower();

                if (userInput == "add")
                {
                    Console.Write("Adja meg a termék nevét: ");
                    string productName = Console.ReadLine();

                    Categorise(DetailInput(), productName, fridgeHealthyContentCount, aboutToExpireCount, fridgeHealthContentDate, aboutToExpireDate, fridgeHealthyContent, aboutToExpire);

                }
                else if (userInput == "sts" || userInput == "stats")
                {
                    Stats(shoppingList, fridgeHealthyContent, aboutToExpire,
                        shoppingListDate, fridgeHealthContentDate, aboutToExpireDate,
                        shoppingListCount, fridgeHealthyContentCount, aboutToExpireCount);
                }
                else if (userInput == "exit")
                {
                    loop = false; //vagy 'break;' , ugyanaz
                }
            } while (loop == true);
        }

        public static void Stats(string[] shoppingList, string[] fridgeHealthyContent, string[] aboutToExpire,
            DateTime[] shoppingListDate, DateTime[] fridgeHealthyContentDate, DateTime[] aboutToExpireDate,
            int shoppingListCount, int fridgeHealthyContentCount, int aboutToExpireCount)
        {
            Console.WriteLine("\n\n--------------------------------\n");
            Console.WriteLine("A hűtő egészséges tartalma:\n");

            for (int i = 0; i < fridgeHealthyContentCount; i++)
            {
                Console.WriteLine("- " + fridgeHealthyContent[i] + "...................." + fridgeHealthyContentDate[i] + "\n");
            }

            Console.WriteLine("\n\n--------------------------------\n");
            Console.WriteLine("Hamarosan lejáró termékek:\n");

            for (int i = 0; i < aboutToExpireCount; i++)
            {
                Console.WriteLine("- " + aboutToExpire[i] + "...................." + aboutToExpireDate[i] + "\n");
            }

            Console.WriteLine("\n\n--------------------------------\n");
            Console.WriteLine("Lejárt termékek:\n");

            for (int i = 0; i < shoppingListCount; i++)
            {
                Console.WriteLine("- " + fridgeHealthyContent[i] + "...................." + fridgeHealthyContentDate[i] + "\n");
            }            
        }

        public static void Help()
        {
            string[] commands = { "add" };

            string[] meanings = { "Új termék regisztrálása" };

            for (int i = 0; i < commands.Length; i++)
            {
                Console.WriteLine("\n---------------------------------------");
                Console.WriteLine(commands[i] + " - " + meanings[i]);
            }

            Console.WriteLine("\n");
        }

        public static void AutoSave(
            ref string[] shoppingList, ref string[] fridgeHealthyContent, ref string[] aboutToExpire,
            ref DateTime[] shoppingListDate, ref DateTime[] fridgeHealthyContentDate, ref DateTime[] aboutToExpireDate,
            ref int shoppingListCount, ref int fridgeHealthyContentCount, ref int aboutToExpireCount)
        {
            bool noException = false;

            do
            {
                StreamWriter save;
                try
                {
                    save = new StreamWriter("shoppingList.txt");

                    for (int i = 0; i < 100; i++)
                    {
                        save.WriteLine(shoppingList[i]);

                        save = new StreamWriter("fridgeHealthyContent.txt");
                        save.WriteLine(fridgeHealthyContent[i]);

                        save = new StreamWriter("aboutToExpire.txt");
                        save.WriteLine(aboutToExpire[i]);

                        save = new StreamWriter("shoppingListDate.txt");
                        save.WriteLine(shoppingListDate[i]);

                        save = new StreamWriter("fridgeHealthyContentDate.txt");
                        save.WriteLine(fridgeHealthyContentDate[i]);

                        save = new StreamWriter("aboutToExpireDate.txt");
                        save.WriteLine(aboutToExpireDate[i]);

                        save = new StreamWriter("shoppingList.txt");
                    }

                    save = new StreamWriter("counters.txt");

                    save.WriteLine(shoppingListCount);
                    save.WriteLine(fridgeHealthyContentCount);
                    save.WriteLine(aboutToExpireCount);

                    save.Close();

                    noException = true;
                }
                catch (Exception e)
                {
                    StreamWriter errorLog = new StreamWriter("errorLog.txt", true);
                    errorLog.WriteLine(e + "\n");
                    errorLog.Close();

                    StreamWriter createSaveFiles = new StreamWriter("shoppingList.txt");
                    createSaveFiles = new StreamWriter("fridgeHealthyContent.txt");
                    createSaveFiles = new StreamWriter("aboutToExpire.txt");
                    createSaveFiles = new StreamWriter("shoppingListDate.txt");
                    createSaveFiles = new StreamWriter("fridgeHealthyContentDate.txt");
                    createSaveFiles = new StreamWriter("aboutToExpireDate.txt");
                    createSaveFiles = new StreamWriter("counters.txt");

                    createSaveFiles.Close();
                }
            } while (noException == false);
        }

        public static void Categorise(DateTime expiryDate, string productName, int fridgeHealthyContentCount, int aboutToExpireCount,
            DateTime[] fridgeHealthyContentDate, DateTime[] aboutToExpireDate,
            string[] fridgeHealthyContent, string[] aboutToExpire)
        {
            int difference = Convert.ToInt32(expiryDate.Subtract(DateTime.Now).Days);

            if (difference > 3)
            {
                fridgeHealthyContentDate[fridgeHealthyContentCount] = expiryDate;
                fridgeHealthyContent[fridgeHealthyContentCount] = productName;

                fridgeHealthyContentCount++;
            }
            else if (difference <= 3)
            {
                Console.Beep();
                Console.WriteLine("Ez a termék rövidesen lejár!");
                Console.WriteLine("Lejáratig hátralévő idő: {0} nap"), difference);

                Console.Write("Így is regiszrálni szeretné? (y/n): ");
                string userInput = Console.ReadLine().ToLower();

                if (userInput == "y")
                {
                    aboutToExpireDate[aboutToExpireCount] = expiryDate;
                    aboutToExpire[aboutToExpireCount] = productName;

                    aboutToExpireCount++;
                }
            }
            else if (difference <= 0)
            {
                Console.WriteLine("Ez a termék már lejárt!");
            }
        }

        public static DateTime DetailInput()
        {
            Console.WriteLine("Kérem, adja meg a termék lejárati dátumát!");

            Console.Write("Év: ");
            int year = Convert.ToInt32(Console.ReadLine());

            Console.Write("Hónap: ");
            int month = Convert.ToInt32(Console.ReadLine());

            Console.Write("Nap: ");
            int day = Convert.ToInt32(Console.ReadLine());

            string resultString = (year + ". " + month + ". " + day);

            DateTime result = Convert.ToDateTime(resultString);
            Console.WriteLine("A termék lejáratáig még {0} nap van hátra!", result.Subtract(DateTime.Now).Days);

            return result;
        }

        public static void LoadSave(
            ref string[] shoppingList, ref string[] fridgeHealthyContent, ref string[] aboutToExpire,
            ref DateTime[] shoppingListDate, ref DateTime[] fridgeHealthyContentDate, ref DateTime[] aboutToExpireDate,
            ref int shoppingListCount, ref int fridgeHealthyContentCount, ref int aboutToExpireCount)
        {
            bool noException = false;

            do
            {
                StreamReader load;
                try
                {
                    load = new StreamReader("shoppingList.txt");

                    for (int i = 0; i < 100; i++)
                    {
                        if (!load.EndOfStream)
                        {
                            shoppingList[i] = load.ReadLine();

                            load = new StreamReader("fridgeHealthyContent.txt", true);
                            fridgeHealthyContent[i] = load.ReadLine();

                            load = new StreamReader("aboutToExpire.txt", true);
                            aboutToExpire[i] = load.ReadLine();

                            load = new StreamReader("shoppingListDate.txt", true);
                            shoppingListDate[i] = Convert.ToDateTime(load.ReadLine());

                            load = new StreamReader("fridgeHealthyContentDate.txt", true);
                            fridgeHealthyContentDate[i] = Convert.ToDateTime(load.ReadLine());

                            load = new StreamReader("aboutToExpireDate.txt", true);
                            aboutToExpireDate[i] = Convert.ToDateTime(load.ReadLine());

                            load = new StreamReader("shoppingList.txt", true);
                        }
                    }

                    load = new StreamReader("counters.txt");

                    shoppingListCount = Convert.ToInt32(load.ReadLine());
                    fridgeHealthyContentCount = Convert.ToInt32(load.ReadLine());
                    aboutToExpireCount = Convert.ToInt32(load.ReadLine());

                    load.Close();

                    ////próba
                    //for (int i = 0; i < 100; i++)
                    //{
                    //    Console.WriteLine(shoppingList[i]);
                    //    Console.WriteLine(fridgeHealthyContent[i]);
                    //    Console.WriteLine(aboutToExpire[i]);
                    //    Console.WriteLine(shoppingListDate[i]);
                    //    Console.WriteLine(fridgeHealthyContentDate[i]);
                    //    Console.WriteLine(aboutToExpireDate[i]);
                    //}

                    //Console.WriteLine(shoppingListCount);
                    //Console.WriteLine(fridgeHealthyContentCount);
                    //Console.WriteLine(aboutToExpireCount);

                    //Console.ReadKey();
                    ////próba

                    noException = true;
                }
                catch (Exception e)
                {
                    StreamWriter errorLog = new StreamWriter("errorLog.txt", true);
                    errorLog.WriteLine(e + "\n");
                    errorLog.Close();

                    StreamWriter createSaveFiles = new StreamWriter("shoppingList.txt");
                    createSaveFiles = new StreamWriter("fridgeHealthyContent.txt");
                    createSaveFiles = new StreamWriter("aboutToExpire.txt");
                    createSaveFiles = new StreamWriter("shoppingListDate.txt");
                    createSaveFiles = new StreamWriter("fridgeHealthyContentDate.txt");
                    createSaveFiles = new StreamWriter("aboutToExpireDate.txt");
                    createSaveFiles = new StreamWriter("counters.txt");

                    createSaveFiles.Close();
                }
            } while (noException == false);
        }

        public static void Welcome()
        {
            int time = Convert.ToInt32(DateTime.Now.Hour);     //A jelenlegi idő órában   

            Console.WriteLine(DateTime.Now + "\n");

            if (time < 8)
            {
                Console.WriteLine("Jó reggelt!");
            }
            else if (time > 10 && time < 17)
            {
                Console.WriteLine("Jó napot!");
            }
            else if (time > 17)
            {
                Console.WriteLine("Jó estét");
            }

            Console.WriteLine("\n\n");
        }
    }
}
