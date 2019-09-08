using System;
using System.IO;

namespace Actual_Expense_Calculator
{
    class Program
    {
        #region Static Fields
        static struct Balance
        {
          public int Cash {get; set;}
          public int Card {get; set;}
        }
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
              Balance.Card = Convert.ToInt32(Console.ReadLine());

              Console.Write("What is your cash amount?: ");
              Balance.Cash = Convert.ToInt32(Console.ReadLine());
              break;
            } catch (FormatException e)
            {
              Console.WriteLine("You can only enter numbers!");
              FileManagment.LogError(e);
              continue;
            }
          } while (true);
        }

        static class FileManagment
        {
          public void Create()
          {
            File saveFile = new File("./.save");
            saveFile.Close();
          }

          public void Erase()
          {

          }

          public void Save()
          {

          }

          public void Load()
          {
            File save = new File("./.save");

            if (!save.Exists)
            {
              Introduction();
            }
          }

          public void LogError(Exception e)
          {
            do
            {
              try
              {
                using (StreamWriter w = File.AppendText("./.errorLog"))
                {
                  w.WriteLine(e + "\n");
                }
                break;
              }
              catch (FileNotFoundException)
              {
                File errorLog = new File("./.errorLog");
                continue;
              }
            } while (true);
          }
        }

        class Expense
        {
          protected int Value {get; set;}
          protected int Name {get; set;}

          public Expense()
          {
            Console.Write("Enter the name of the new expense: ");
            this.Name = Console.ReadLine();

            Console.Write("Enter the value of the expense: ");
            this.Value = Console.ReadLine();
          }
        }

        class Constant : Expense
        {
          private int Interval {get; set;}

          public Constant()
          {
            private int Interval {get; set;}
            Console.Write("");
          }
        }

        class OneTime : Expense
        {
          private DateTime PurchaseDate {get; set;}

          public OneTime()
          {
            do
            {
              Console.Write("Did you make this purchase today? (y/n): ");

              try
              {
                char userInput = Convert.ToChar(Console.ReadLine().ToLower());
                break;
              } catch (FormatException e)
              {
                Console.WriteLine("You need to enter either 'y' or 'n' as an answer!");

                FileManagment.LogError(e);
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
                  FileManagment.LogError(e)
                  continue;
                }
              } while (true);
            }
          }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
