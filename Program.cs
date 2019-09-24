using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace Actual_Expense_Calculator
{
    class Program
    {
        #region Static Fields
        /*static*/ struct Balance
        {
          public int Cash; /*{get; set;}*/
          public int Card; /*{get; set;}*/
        }

        static List<OneTime> oneTime = new List<OneTime>();
        static List<Scheduled> scheduled = new List<Scheduled>();

        //I'm very ashamed of these
        static Balance balance;
        static FileManagment fileManagment = new FileManagment();
        static Cli cli = new Cli();
        static Util util = new Util();
        #endregion

        sealed class Util
        {
          public void RecordTransaction()
          {
            cli.Title("Which type of transaction would you like to put on record?");

            Console.WriteLine("1.) One-time purchase\n");
            Console.WriteLine("2.) Regular expense\n\n");

            int input;

            while (true)
            {
              try
              {
                input = Convert.ToInt32(cli.Input());

                if (input != 1 && input != 2)
                {
                  throw new ArgumentException();
                }
                else
                {
                  break;
                }
              }
              catch (FormatException)
              {
                cli.Alert("You must enter a valid option!");
                continue;
              }
              catch (ArgumentException)
              {
                cli.Alert("You must enter a valid option!");
                continue;
              }
            }

            switch (input)
            {
              case 1:
                OneTime tmp = new OneTime();
                oneTime.Add(tmp);
                break;
              case 2:
                Scheduled tmp1 = new Scheduled();
                scheduled.Add(tmp1);
                break;
            }
          }

          public void List()
          {
            cli.Title("Your balances:");
            Console.WriteLine("- Account balance: {0}\n\n- Cash amount: {1}\n", balance.Card, balance.Cash);

            cli.Title("One-time purchases:");

            foreach (OneTime item in oneTime)
            {
              Console.WriteLine("- {0}:", item.Name);
              Console.WriteLine("       - " + item.Value);
              Console.WriteLine("       - " + item.PurchaseDate.Date + "\n");
            }

            cli.Title("Scheduled purchases:");

            foreach (Scheduled item in scheduled)
            {
              Console.WriteLine("- {0}:", item.Name);
              Console.WriteLine("       - " + item.Value);

              string Format = "";
              switch (item.Interval.Format)
              {
                case 'd':
                  Format = "days";
                  break;
                case 'm':
                  Format = "months";
                  break;
                case 'y':
                  Format = "years";
                  break;
              }

              Console.WriteLine("       Regularity: every {0} {1}", item.Interval.Num, Format + "\n");
            }
          }

          public void Introduction()
          {
            Console.WriteLine("Hello!\n");
            Thread.Sleep(1000);

            Console.WriteLine("We would like to ask for some of your financial inFormation.");
            Thread.Sleep(1000);

            do
            {
              try
              {
                Console.Write("What is your account balance?: ");
                balance.Card = Convert.ToInt32(Console.ReadLine());

                Console.Write("What is your cash amount?: ");
                balance.Cash = Convert.ToInt32(Console.ReadLine());
                break;
              } catch (FormatException)
              {
                cli.Alert("You can only enter Numbers!");
                continue;
              }
            } while (true);

            Thread.Sleep(500);
            Instructions();
          }

          public void Instructions()
          {
            Console.Clear();
            Console.WriteLine("\n"+File.ReadAllText(".info"));
            Console.WriteLine("- Press any key to continue -");
            Console.CursorVisible = false;
            Console.ReadKey(true);
            Console.Clear();
            Console.CursorVisible = true;
          }
        }

        sealed class Cli
        {
          public void Title(string message, string character = "-")
          {
            string line = "";

            for (int i = 0; i < message.Length; i++)
            {
              line += character;
            }

            Console.WriteLine("\n" + line + "\n" + message + "\n" + line + "\n");
          }

          public string Prompt(string message, string option1 = "y", string option2 = "n")
          {
            string input = "";
            while (true)
            {
              Console.Write("\n{0} ('{1}' or '{2}'): ", message, option1, option2);
              input = Console.ReadLine().ToLower();

              if (input != option1 && input != option2)
              {
                cli.Alert("You must enter a valid option!");
                continue;
              }
              else
              {
                break;
              }
            }

            return input;
          }

          public string Input(string message = "Your option")
          {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("//{0}: ", message);
            Console.ResetColor();
            return Console.ReadLine();
          }

          public void Alert(string message = "Alert")
          {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n|!| ");
            Console.ResetColor();
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" |!|\n");
            Console.ResetColor();
          }
        }

        sealed class FileManagment
        {

          public void Erase()
          {
            cli.Alert("Proceeding will delete your save and you will lose all your data!");

            if (cli.Prompt("Are you sure you want to proceed?", "yes", "no") == "yes")
            {
              File.Delete(".save");

              Console.WriteLine("\nSave file deleted!\nNote that saving your current session when exiting will still\nsave your current data!");
            }
          }

          public void Save(bool prompt = false)
          {
            bool auth = true;
            if (prompt)
            {
              string input;
              while (true)
              {
                Console.Write("Save current changes? ('y' or 'n'): ");
                input = Console.ReadLine().ToLower();

                if (input != "y" && input != "n")
                {
                  cli.Alert("You must enter a valid option!");
                  continue;
                }
                else
                {
                  break;
                }
              }

              if (input == "n")
              {
                auth = false;
              }
            }

            if (auth)
            {
              using (StreamWriter w = new StreamWriter(".save"))
              {
                w.WriteLine(balance.Card);
                w.WriteLine(balance.Cash);

                for (int i = 0; i < oneTime.Count; i++)
                {
                  w.WriteLine(oneTime[i].Name);
                  w.WriteLine(oneTime[i].Value);
                  w.WriteLine(oneTime[i].PurchaseDate);

                  // if (i != (scheduled.Count - 1))
                  // {
                  //   w.WriteLine("-");
                  // }
                }

                w.WriteLine("#");

                for (int i = 0; i < scheduled.Count; i++)
                {
                  w.WriteLine(scheduled[i].Name);
                  w.WriteLine(scheduled[i].Value);
                  w.WriteLine(scheduled[i].Interval.Num);
                  w.WriteLine(scheduled[i].Interval.Format);

                  if (i != (scheduled.Count - 1))
                  {
                    w.WriteLine("-");
                  }
                }
              }
            }
          }

          public void Load()
          {
            if (!File.Exists(".save"))
            {
              util.Introduction();
            }
            else
            {
              using (StreamReader sr = new StreamReader(".save"))
              {
                Phase1(sr); //I'm not sure if 'ref' is needed to progress the StreamReader's iteration
                            //or not, just making sure
                            //Edit: nope
                Phase2(sr);
                Phase3(sr);
              }
            }

            void Phase1(StreamReader sr)
            {
              balance.Card = Convert.ToInt32(sr.ReadLine());
              balance.Cash = Convert.ToInt32(sr.ReadLine());
            }

            void Phase2(StreamReader sr)
            {
              bool auth = true;

              while (auth) //????????
              {
                string streamInput = sr.ReadLine();
                if (streamInput != "#")
                {
                  // oneTime[oneTime.Count].Name = sr.ReadLine();
                  // oneTime[oneTime.Count].Value = Convert.ToInt32(sr.ReadLine());
                  // oneTime[oneTime.Count].PurchaseDate = Convert.ToDateTime(sr.ReadLine());

                  OneTime tmp = new OneTime(true);
                  tmp.Name = streamInput;
                  tmp.Value = Convert.ToInt32(sr.ReadLine());
                  tmp.PurchaseDate = Convert.ToDateTime(sr.ReadLine());
                  oneTime.Add(tmp);
                }
                else
                {
                  auth = false;
                }
              }
            }

            void Phase3(StreamReader sr)
            {
              while (!sr.EndOfStream) //edit if more things will be saved in the future
              {
                Scheduled tmp = new Scheduled(true);
                tmp.Name = sr.ReadLine();
                tmp.Value = Convert.ToInt32(sr.ReadLine());
                tmp.Interval.Num = Convert.ToInt32(sr.ReadLine());
                tmp.Interval.Format = Convert.ToChar(sr.ReadLine());

                scheduled.Add(tmp);
              }
            }
          }
        }

        class Expense
        {
          public int Value {get; set;}
          public string Name {get; set;}

          public Expense(bool silent = false)
          {
            if (!silent)
            {
              Console.Write("Enter the name of the new expense: ");
              this.Name = Console.ReadLine();

              while (true)
              {
                try
                {
                  Console.Write("Enter the value of the expense: ");
                  this.Value = Convert.ToInt32(Console.ReadLine());
                  break;
                }
                catch (FormatException)
                {
                  cli.Alert("You entered an incorrect Format, please try again");
                  continue;
                }
              }
            }
          }
        }

        class Scheduled : Expense
        {
          public struct Info
          {
            public char Format; //"d" as 'day', 'm' as month and 'y' as 'year'
            public int Num; //TODO: take input for this value
          }

          public Info Interval = new Info();

          public Scheduled(bool silent = false) : base(silent) //this will pass the silent parameter to the base class constructor
          {
            if (!silent)
            {
              while (true)
              {
                Console.WriteLine("\nHow often do you make this purchase?");
                Console.WriteLine("Select one of these options:");
                Console.WriteLine("\nDay - 'd'");
                Console.WriteLine("\nMonth - 'm'");
                Console.WriteLine("\nYear - 'y'\n");

                try
                {
                  this.Interval.Format = Convert.ToChar(cli.Input());

                  if (this.Interval.Format != 'y' && this.Interval.Format != 'm' && this.Interval.Format != 'd')
                  {
                    throw new ArgumentException();
                  }

                  Console.WriteLine("Please supply the previously selected Format with a Number!");
                  Console.WriteLine("For example, if you selected 'month' in the previous step,\nand you make this purchase every three months, enter 3 here");
                  this.Interval.Num = Convert.ToInt32(cli.Input());
                  break;
                }
                catch (FormatException)
                {
                  cli.Alert("You entered an incorrect Format, please try again");
                  continue;
                }
                catch (ArgumentException)
                {
                  cli.Alert("You need to enter one of these options: d/m/y");
                }
              }
            }
          }
        }

        class OneTime : Expense
        {
          public DateTime PurchaseDate {get; set;}

          public OneTime(bool silent = false) : base(silent)
          {
            if (!silent)
            {
              if (cli.Prompt("Did you make this purchase today?") == "y")
              {
                this.PurchaseDate = DateTime.Today;
              }
              else
              {
                Console.WriteLine("Please specify the date of the purchase in the following Format:");

                do
                {
                  try
                  {
                    Console.Write("dd/MM/yyyy: ");
                    this.PurchaseDate = Convert.ToDateTime(Console.ReadLine());

                    break;
                  } catch (FormatException)
                  {
                    cli.Alert("You need to enter the date in the specified Format!");
                    continue;
                  }
                } while (true);
              }
            }
          }
        }

        static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible = true;

            fileManagment.Load();

            TakeInput();
        }

        static void TakeInput()
        {
          bool loopCondition = true;

          while (loopCondition)
          {
            string input = cli.Input("Command").ToLower();
            switch (input)
            {
              case "add":
                util.RecordTransaction();
                break;
              case "help":
                util.Instructions();
                break;
              case "exit":
                fileManagment.Save(true);
                loopCondition = false;
                break;
              case "erase":
                fileManagment.Erase();
                break;
              case "list":
                util.List();
                break;
              default:
                Console.WriteLine("\nThere is no such command as '{0}'!\nEnter 'Help' to see available commands!\n", input);
                break;
            }
          }

          Console.Clear();
        }
    }
}
