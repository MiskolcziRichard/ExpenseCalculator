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

        //I'm very ashamed of these two
        static Balance balance;
        static FileManagment fileManagment = new FileManagment();
        #endregion

        static void RecordTransaction()
        {
          Console.WriteLine("\n------------------------------------------------------------\nWhich type of transaction would" +
          " you like to put on record?\n------------------------------------------------------------\n");

          Console.WriteLine("1.) One-time purchase\n");
          Console.WriteLine("2.) Regular expense\n\n");

          int input;

          while (true)
          {
            Console.Write("Your option: ");
            input = Convert.ToInt32(Console.ReadLine());

            if (input != 1 && input != 2)
            {
              Alert("You must enter a valid option!");
              continue;
            }
            else
            {
              break;
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

        static void Alert(string message = "Alert")
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.Write("\n|!| ");
          Console.ResetColor();
          Console.Write(message);
          Console.ForegroundColor = ConsoleColor.Red;
          Console.Write(" |!|\n");
          Console.ResetColor();
        }

        static void Introduction()
        {
          Console.WriteLine("Hello!\n");
          Thread.Sleep(1000);

          Console.WriteLine("We would like to ask for some of your financial information.");
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
              Console.WriteLine("You can only enter numbers!");
              continue;
            }
          } while (true);

          Thread.Sleep(500);
          Instructions();
        }

        sealed class FileManagment
        {

          public void Erase()
          {
            Alert("Proceeding will delete your save and you will lose all your data!");
            string input;

            while (true)
            {
              Console.Write("\nAre you sure you want to proceed? ('yes' or 'no'): ");
              input = Console.ReadLine().ToLower();

              if (input != "yes" && input != "no")
              {
                Alert("You must enter a valid option!");
                continue;
              }
              else
              {
                break;
              }
            }

            if (input == "yes")
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
                  Alert("You must enter a valid option!");
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
                }

                for (int i = 0; i < scheduled.Count; i++)
                {
                  w.WriteLine(scheduled[i].Name);
                  w.WriteLine(scheduled[i].Value);
                  w.WriteLine(scheduled[i].Interval.num);
                  w.WriteLine(scheduled[i].Interval.format);
                }
              }
            }
          }

          public void Load()
          {
            if (!File.Exists(".save"))
            {
              Introduction();
            }
            else
            {
              using (StreamReader sr = new StreamReader(".save"))
              {
                balance.Card = Convert.ToInt32(sr.ReadLine());
                balance.Cash = Convert.ToInt32(sr.ReadLine());
              }
            }
          }

        //   public void LogError(Exception )
        //   {
        //     do
        //     {
        //       try
        //       {
        //         using (StreamWriter w = File.AppendText("./.errorlog"))
        //         {
        //           w.WriteLine(e + "\n");
        //         }
        //         break;
        //       }
        //       catch (FileNotFoundException)
        //       {
        //         File.Create("./.errorlog");
        //         continue;
        //       }
        //     } while (true);
        //   }
        }

        class Expense
        {
          public int Value {get; set;}
          public string Name {get; set;}

          public Expense()
          {
            Console.Write("Enter the name of the new expense: ");
            this.Name = Console.ReadLine();

            Console.Write("Enter the value of the expense: ");
            this.Value = Convert.ToInt32(Console.ReadLine());
          }
        }

        class Scheduled : Expense
        {
          public struct Info
          {
            public char format; //"d" as 'day', 'm' as month and 'y' as 'year'
            public int num; //TODO: take input for this value
          }

          public Info Interval = new Info();

          public Scheduled()
          {
            Console.WriteLine("How often do you make this purchase?");
            Console.WriteLine("Select one of these options:");
            Console.WriteLine("\nDay - 'd'");
            Console.WriteLine("\nMonth - 'm'");
            Console.WriteLine("\nYear - 'y'\n");

            while (true)
            {
              Console.Write("//Your option: ");

              try
              {
                this.Interval.format = Convert.ToChar(Console.ReadLine());
                if (this.Interval.format != 'y' && this.Interval.format != 'm' && this.Interval.format != 'd')
                {
                  throw new IndexOutOfRangeException(); //TODO: change the exception type
                }
                break;
              }
              catch (FormatException)
              {
                Alert("You entered an incorrect format, please try again");
                continue;
              }
              catch (IndexOutOfRangeException)
              {
                Alert("You need to enter one of these options: d/m/y");
              }
            }
          }
        }

        class OneTime : Expense
        {
          public DateTime PurchaseDate {get; set;}

          public OneTime()
          {
            char userInput;

            do
            {
              Console.Write("Did you make this purchase today? (y/n): ");

              try
              {
                userInput = Convert.ToChar(Console.ReadLine().ToLower());
                break;
              } catch (FormatException)
              {
                Console.WriteLine("You need to enter either 'y' or 'n' as an answer!");
                continue;
              }

            } while (true);

            if (userInput == 'y')
            {
              this.PurchaseDate = DateTime.Today;
            }
            else
            {
              Console.WriteLine("Please specify the date of the purchase in the following format:");

              do
              {
                try
                {
                  Console.Write("dd/MM/yyyy: ");
                  this.PurchaseDate = Convert.ToDateTime(Console.ReadLine());

                  break;
                } catch (FormatException)
                {
                  Console.WriteLine("You need to enter the date in the specified format!");
                  continue;
                }
              } while (true);
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

        static void Instructions()
        {
          Console.Clear();
          Console.WriteLine("\n"+File.ReadAllText(".info"));
          Console.WriteLine("- Press any key to continue -");
          Console.CursorVisible = false;
          Console.ReadKey(true);
          Console.Clear();
          Console.CursorVisible = true;
        }

        static void TakeInput()
        {
          bool loopCondition = true;

          while (loopCondition)
          {
            Console.Write("$ ");
            string input = Console.ReadLine();

            switch (input.ToLower())
            {
              case "add":
                RecordTransaction();
                break;
              case "help":
                Instructions();
                break;
              case "exit":
                fileManagment.Save(true);
                loopCondition = false;
                break;
              case "erase":
                fileManagment.Erase();
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
