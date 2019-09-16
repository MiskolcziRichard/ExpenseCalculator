using System;
using System.IO;
using System.Threading;

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

        //I'm very ashamed of these two
        static Balance balance;
        static FileManagment fileManagment = new FileManagment();
        #endregion

        static void Introduction()
        {
          Console.WriteLine("Hello!\n");
          Thread.Sleep(1000);

          Console.WriteLine("We would like to ask for some of your financial informations.");
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
            } catch (FormatException e)
            {
              Console.WriteLine("You can only enter numbers!");
              fileManagment.LogError(e);
              continue;
            }
          } while (true);

          fileManagment.Save();
        }

        class FileManagment
        {

          public void Erase()
          {

          }

          public void Save()
          {
            if (!File.Exists(".save"))
            {
              File.Create(".save");
            }

            using (StreamWriter w = new StreamWriter(".save"))
            {
              w.WriteLine(balance.Cash);
              w.WriteLine(balance.Card);
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
              //load stuff
            }
          }

          public void LogError(Exception e)
          {
            do
            {
              try
              {
                using (StreamWriter w = File.AppendText("./.errorlog"))
                {
                  w.WriteLine(e + "\n");
                }
                break;
              }
              catch (FileNotFoundException)
              {
                File.Create("./.errorlog");
                continue;
              }
            } while (true);
          }
        }

        class Expense
        {
          protected int Value {get; set;}
          protected string Name {get; set;}

          public Expense()
          {
            Console.Write("Enter the name of the new expense: ");
            this.Name = Console.ReadLine();

            Console.Write("Enter the value of the expense: ");
            this.Value = Convert.ToInt32(Console.ReadLine());
          }
        }

        class Constant : Expense
        {
          private int Interval {get; set;}

          public Constant()
          {
            Console.Write("uoo");
          }
        }

        class OneTime : Expense
        {
          private DateTime PurchaseDate {get; set;}

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
              } catch (FormatException e)
              {
                Console.WriteLine("You need to enter either 'y' or 'n' as an answer!");

                fileManagment.LogError(e);
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
                } catch (FormatException e)
                {
                  Console.WriteLine("You need to enter the date in the specified format!");
                  fileManagment.LogError(e);
                  continue;
                }
              } while (true);
            }
          }
        }

        static void Main(string[] args)
        {
            fileManagment.Load();
        }
    }
}
