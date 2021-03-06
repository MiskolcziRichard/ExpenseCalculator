﻿using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace Actual_Expense_Calculator
{
    class Program
    {
        #region Static Fields
        class Profile
        {
          public int Cash;
          public int Card;
          public string Currency;

          public Profile(bool silent = false)
          {
            if (!silent)
            {
              do
              {
                try
                {
                  this.Card = Convert.ToInt32(cli.Input("What is your account balance?"));
                  this.Cash = Convert.ToInt32(cli.Input("What is your cash amount?"));
                  break;
                } catch (FormatException)
                {
                  cli.Alert("You can only enter Numbers!");
                  continue;
                }
              } while (true);

              this.Currency = cli.Input("What currency do you use?");
            }
          }

          public void List()
          {
            cli.Title("Your balances:");

            Console.ForegroundColor = Evaluate(this.Card);
            Console.WriteLine("{0} {1}\t<== Account balance\n", this.Card, this.Currency);

            Console.ForegroundColor = Evaluate(this.Cash);
            System.Console.WriteLine("{0} {1}\t<== Cash amount\n", this.Cash, this.Currency);

            Console.ForegroundColor = Evaluate(this.Card + this.Cash);
            Console.WriteLine("\n{0} {1}\t<== Total", this.Card+this.Cash, this.Currency);
            Console.ResetColor();
          }

          private ConsoleColor Evaluate(int value)
          {
            return value >= 0 ? ConsoleColor.Green : ConsoleColor.Red;
          }

          public void Pay(Expense item)
          {
            if (item.Method == PaymentMethod.Card)
            {
              profile.Card -= item.Value;
            }
            else
            {
              profile.Cash -= item.Value;
            }
          }

          public void Refund(Expense item)
          {
            if (item.Method == PaymentMethod.Card)
            {
              profile.Card += item.Value;
            }
            else
            {
              profile.Cash += item.Value;
            }
          }
        }

        static List<OneTime> oneTime = new List<OneTime>();
        static List<Scheduled> scheduled = new List<Scheduled>();

        //I'm very ashamed of these
        static Profile profile = new Profile(true);
        static FileManagment fileManager = new FileManagment();
        static Cli cli = new Cli();
        static Util util = new Util();
        #endregion

        sealed class Util
        {
          public void Exchange(bool dir) //when dir is false, its a deposit, if true, its a withdrawal
          {
            while (true)
            {
              try
              {
                int val = 0;
                if (dir)
                {
                  System.Console.Write("(Note: leave blank to balance out a debt!)\nEnter how much would you like to withdraw: ");

		  string tmp = Console.ReadLine();

                  if (tmp != "")
                  {
                    val = Convert.ToInt32(tmp);
                  }
                  else
                  {
                    if (profile.Cash >= 0)
                    {
                      System.Console.WriteLine("You are in no debt!");
                    }
                    else
                    {
                      val = (profile.Cash * -1);
                    }
                  }

                  if (profile.Card >= val)
                  {
                    profile.Card -= val;
                    profile.Cash += val;
                  }
                  else
                  {
                    cli.Alert("You don't have enough credits for the transaction!");
                  }

                  break;
                }
                else
                {
                  System.Console.Write("(Note: leave blank to balance out a debt!)\nEnter how much would you like to deposit: ");

                  string tmp = Console.ReadLine();

                  if (tmp != "")
                  {
                    val = Convert.ToInt32(tmp);
                  }
                  else
                  {
                    if (profile.Card >= 0)
                    {
                      System.Console.WriteLine("You are in no debt!");
                    }
                    else
                    {
                      val = (profile.Card * -1);
                    }
                  }

                  if (profile.Cash >= val)
                  {
                    profile.Cash -= val;
                    profile.Card += val;
                  }
                  else
                  {
                    cli.Alert("You don't have enough credits for the transaction!");
                  }

                  break;
                }
              }
              catch (FormatException)
              {
                cli.Alert("You entered an incorrect Format, please try again");
                continue;
              }
            }
          }

          public void EditTransaction()
          {

            if (util.GetExpenseType("modify") == typeof(OneTime))
            {
              util.ListTransactions(typeof(OneTime));

              string name = cli.Input("Which transaction would you like to modify?").ToLower();

              foreach (OneTime i in oneTime)
              {
                if (i.Name.ToLower() == name)
                {
                  OneTime obj = oneTime[oneTime.IndexOf(i)]; //just so i can refund it
                  profile.Refund(obj);
                  obj = new OneTime();
                  oneTime[oneTime.IndexOf(i)] = obj;
                  break;
                }
              }
            }
            else
            {
              util.ListTransactions(typeof(Scheduled));

              string name = cli.Input("Which transaction would you like to modify?").ToLower();

              foreach (Scheduled i in scheduled)
              {
                if (i.Name.ToLower() == name)
                {
                  Scheduled obj = scheduled[scheduled.IndexOf(i)];
                  profile.Refund(obj);
                  obj = new Scheduled();
                  scheduled[scheduled.IndexOf(i)] = obj; //what if i just do this instead? EDIT: oh wow this actually works
                  break;
                }
              }
            }
          }

          public Type GetExpenseType(string action)
          {
            cli.Title("To what type does the transaction you would like to "+action+" belong?");

            string[] tmp = {"One-time purchase", "Regular expense"};
            cli.ListOptions(tmp);

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
              } catch (ArgumentException)
              {
                cli.Alert("You must enter a valid option!");
              }
              catch (FormatException)
              {
                cli.Alert("You must enter a valid format!");
              }
            }

            return input == 1 ? typeof(OneTime) : typeof(Scheduled);
          }

          public void ListTransactions(Type t)
          {
            if (t == typeof(OneTime))
            {
              if (oneTime.Count > 0)
              {
                cli.Title("One-time transactions:");
                int sum = 0;
                foreach (OneTime purchase in oneTime)
                {
                  purchase.List();
                  sum += purchase.Value;
                }

                Console.WriteLine("{0} {1}\t<== Total", sum, profile.Currency);
              }
            }
            else
            {
              if (scheduled.Count > 0)
              {
                cli.Title("Scheduled purchases:");
                int sum = 0;
                foreach (Scheduled purchase in scheduled)
                {
                  purchase.List();
                  sum += purchase.Value;
                }

                Console.WriteLine("{0} {1}\t<== Total", sum, profile.Currency);
              }
            }
          }

          public void List()
          {
            profile.List();

            // util.ListTransactions<OneTime>(oneTime);
            util.ListTransactions(typeof(OneTime));

            // util.ListTransactions<Scheduled>(scheduled);
            util.ListTransactions(typeof(Scheduled));
          }

          public void RemoveTransaction()
          {
              string inputString;

              Type t = util.GetExpenseType("delete");
              if (t == typeof(OneTime))
              {
                util.ListTransactions(typeof(OneTime));
                inputString = cli.Input("Enter the name of the transaction you want to delete").ToLower();
                foreach (OneTime item in oneTime)
                {
                  if (item.Name.ToLower() == inputString)
                  {
                    switch (cli.Prompt("Would you like the value of the transaction to be refunded?"))
                    {
                      case "y":
                        profile.Refund(item);
                        break;
                    }

                    oneTime.Remove(item);
                    break;
                  }
                }
              }
              else
              {
                util.ListTransactions(typeof(Scheduled));
                inputString = cli.Input("Enter the name of the expense you want to delete").ToLower();
                foreach (Scheduled item in scheduled)
                {
                  if (item.Name.ToLower() == inputString)
                  {
                    switch (cli.Prompt("Would you like the value of the transaction to be refunded?"))
                    {
                      case "y":
                        profile.Refund(item);
                        break;
                    }

                    scheduled.Remove(item);
                    break;
                  }
                }
              }
          }

          public void RecordTransaction()
          {
            cli.Title("Which type of transaction would you like to put on record?");

            string[] tmpOpt = {"One-time purchase", "Regular expense"};
            cli.ListOptions(tmpOpt);

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
                cli.Alert("You must enter a valid format!");
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

          public void Introduction()
          {
            Console.WriteLine("Hello!\n");
            Thread.Sleep(1000);

            Console.WriteLine("We would like to ask for some of your financial information.");
            Thread.Sleep(1000);

            profile = new Profile();

            Thread.Sleep(500);
            System.Console.WriteLine("Thank you! Please enjoy, and feel free to submit your feedback on my GitHub at https://github.com/MiskolcziRichard/ExpenseCalculator!\n");

            Console.WriteLine("- Press any key to continue -");
            Console.ReadKey();
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
          public void ListOptions(string[] options)
          {
            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 1; i <= options.Length; i++)
            {
              Console.WriteLine("{0}.) {1}", i, options[i - 1]);
            }

            Console.ResetColor();
          }

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
                Console.Write("Save current session? ('y' or 'n'): ");
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
                w.WriteLine(profile.Card);
                w.WriteLine(profile.Cash);
                w.WriteLine(profile.Currency);

                foreach (OneTime i in oneTime)
                {
                  w.WriteLine(i.Name);
                  w.WriteLine(i.Value);
                  w.WriteLine(i.PurchaseDate);
                  w.WriteLine((int)i.Method);
                }

                w.WriteLine("#--#");

                foreach (Scheduled i in scheduled)
                {
                  w.WriteLine(i.Name);
                  w.WriteLine(i.Value);
                  w.WriteLine(i.PurchaseDate);
                  w.WriteLine(i.Interval.Num);
                  w.WriteLine(i.Interval.Format);
                  w.WriteLine((int)i.Method);
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

              util.List();
            }

            void Phase1(StreamReader sr)
            {
              profile.Card = Convert.ToInt32(sr.ReadLine());
              profile.Cash = Convert.ToInt32(sr.ReadLine());
              profile.Currency = sr.ReadLine();
            }

            void Phase2(StreamReader sr)
            {
              while (true)
              {
                string streamInput = sr.ReadLine();
                if (streamInput != "#--#")
                {
                  OneTime tmp = new OneTime(true);
                  tmp.Name = streamInput;
                  tmp.Value = Convert.ToInt32(sr.ReadLine());
                  tmp.PurchaseDate = Convert.ToDateTime(sr.ReadLine());
                  tmp.Method = (PaymentMethod)Convert.ToInt32(sr.ReadLine());
                  oneTime.Add(tmp);
                }
                else
                {
                  break;
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
                tmp.PurchaseDate = Convert.ToDateTime(sr.ReadLine());
                tmp.Interval.Num = Convert.ToInt32(sr.ReadLine());
                tmp.Interval.Format = Convert.ToChar(sr.ReadLine());
                tmp.Method = (PaymentMethod)Convert.ToInt32(sr.ReadLine());

                scheduled.Add(tmp);
              }
            }
          }
        }

        public enum PaymentMethod {Cash = 1, Card};

        class Expense
        {
          public PaymentMethod Method {get; set;}
          public int Value {get; set;}
          public string Name {get; set;}

          public Expense(bool silent = false)
          {
            if (!silent)
            {
              while (true)
              {
                try
                {
                  Console.Write("Enter the name of the new expense: ");
                  this.Name = Console.ReadLine();

                  Console.Write("Enter the value of the expense: ");
                  this.Value = Convert.ToInt32(Console.ReadLine());

                  Console.WriteLine("What was your payment method?");
                  string[] tmp = {"Cash", "Card"};
                  cli.ListOptions(tmp);

                  this.Method = (PaymentMethod)Convert.ToInt32(cli.Input());

                  break;
                }
                catch (FormatException)
                {
                  cli.Alert("You entered an incorrect format, please try again");
                  continue;
                }
              }

              profile.Pay(this);
            }
          }

          public virtual void List()
          {
            Console.WriteLine("- {0}:", this.Name);
            Console.WriteLine("\t- {0} {1}", this.Value, profile.Currency);

            string method = Enum.GetName(typeof(PaymentMethod), this.Method);
            Console.WriteLine("\t- " + method);
          }
        }

        class Scheduled : OneTime
        {
          public struct Info
          {
            public int Num;
            public char Format; //"d" as 'day', 'm' as month and 'y' as 'year'
          }

          public Info Interval = new Info();

          public Scheduled(bool silent = false) : base(silent) //this will pass the silent parameter to the base class constructor
          {
            if (!silent)
            {
              while (true)
              {
                Console.WriteLine("\nHow often will you make this purchase?");
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

          public override void List()
          {
            base.List();

            string Format = "";
            switch (this.Interval.Format)
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

            Console.WriteLine("\tRegularity: every {0} {1}", this.Interval.Num, Format + "\n");
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
                this.PurchaseDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
              }
              else
              {
                Console.WriteLine("Please specify the date of the purchase in the following Format:");

                do
                {
                  try
                  {
                    Console.Write("MM/dd/yyyy: ");
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

          public override void List()
          {
            base.List();
            Console.WriteLine("\t- " + this.PurchaseDate.Date.ToShortDateString() + "\n");
          }
        }

        static void Main(string[] args)
        {
            Console.Clear();
            Console.CursorVisible = true;

            // try
            // {
            //   fileManager.Load();
            // }
            // catch (Exception e)
            // {
            //   cli.Alert("An error occured while loading your save. Did you edit the save file manually?");
            //   System.Console.WriteLine(e.StackTrace); //TODO: Remove once issue has been resolved
            // }
            fileManager.Load();

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
                fileManager.Save(true);
                loopCondition = false;
                break;
              case "erase":
                fileManager.Erase();
                break;
              case "list":
                util.List();
                break;
              case "remove":
                util.RemoveTransaction();
                break;
              case "edit":
                util.EditTransaction();
                break;
              case "profile":
                profile = new Profile();
                break;
              case "withdraw":
                util.Exchange(true);
                break;
              case "deposit":
                util.Exchange(false);
                break;
              case "clear":
                Console.Clear();
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
