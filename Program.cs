using System;
using System.IO;

namespace Actual_Expense_Calculator
{
    class Program
    {
        #region Static Fields
        public static int Balance {get; set;}
        #endregion

        static class SaveFile
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

          }
        }

        class Expense
        {
          protected int Value {get; set;}
          protected int Name {get; set;}
          protected Date Date {get; set;}

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

        }

        class OneTime : Expense
        {

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
