using System;
using System.IO;

namespace MECTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "save.txt";
            bool[] expensesPin = new bool[50];
            int salary = 0;
            int taxesG = 0;
            int taxesO = 0;
            int collect = 0;
            string userInput = "";
            string[] expensesString = new string[50];
            int[] expensesInt = new int[50];
            int expensesSum = 0;
            int expensesCount = 0;
            int buySum = 0;
            int buyCount = 0;
            int[] buyInt = new int[50];
            string[] buyString = new string[50];
            string[] help = { "'help'/'h' - Display this message", "'ms' - (Modify Salary), modify monthly salary",
            "'me' - (Modify Expenses) modify your other constant expenses (food, debt etc)", "'mt' - (Modify Taxes) modify your taxes",
            "'stats'/'sts' - Show current statistics", "'buy' - Register an item you just bought", "'msl' - (Modify Shopping List), modify list of bought items",
            "'clear'/'clr' - clear the screen", "'pe' - (Plan Expenses), record your pre-planned monthly expenses", "'mpe' - (Modify Planned Expenses), modify your pre-planned expenses",
            "'pin' - pin one of your planned expenses so they won't be erased when a new month is started",
            "'mc' - (Modify Collect), modify the amount of money you wish to save each month", "'nm' - (New Month), start a new month (deletes contents of planned expenses and shopping list",
            "'wipe' - wipe all your data and restart the program", "'exit' - Exit the program", };
            bool loop = true;

            Startup(path, ref salary, ref taxesG, ref taxesO, ref collect, ref buyString, ref buyInt, ref expensesString, ref expensesInt, ref buyCount, ref expensesCount, ref expensesPin);

            Console.WriteLine("Type 'help' or 'h' to view list of available commands.");

            do
            {
                bool noException = false;
                try
                {
                    StreamWriter sw = new StreamWriter(path);
                    sw.WriteLine(salary);
                    sw.WriteLine(taxesG);
                    sw.WriteLine(taxesO);
                    sw.WriteLine(collect);
                    sw.WriteLine(buyCount);
                    sw.WriteLine(expensesCount);

                    for (int i = 0; i < buyInt.Length; i++)
                    {
                        sw.WriteLine(buyString[i]);
                        sw.WriteLine(buyInt[i]);
                    }

                    for (int i = 0; i < expensesInt.Length; i++)
                    {
                        sw.WriteLine(expensesString[i]);
                        sw.WriteLine(expensesInt[i]);
                    }

                    for (int i = 0; i < expensesPin.Length; i++)
                    {
                        sw.WriteLine(expensesPin[i]);
                    }

                    sw.Close();
                }
                catch (FileNotFoundException)
                {
                    StreamWriter createSaveFile = new StreamWriter(path);
                    createSaveFile.Close();
                }

                buySum = 0;
                expensesSum = 0;
                for (int i = 0; i < buyInt.Length; i++)
                {
                    buySum += buyInt[i];
                }

                for (int i = 0; i < expensesInt.Length; i++)
                {
                    expensesSum += expensesInt[i];
                }

                int remainder = salary - (taxesG + taxesO + buySum + expensesSum);

                userInput = Console.ReadLine();

                if ((userInput.ToLower() == "help") || (userInput.ToLower() == "h"))
                {
                    Console.WriteLine("");
                    for (int i = 0; i < help.Length; i++)
                    {
                        Console.WriteLine("----------------------------------------------------------------------------------------------");
                        Console.WriteLine(help[i]);
                    }
                    Console.WriteLine("----------------------------------------------------------------------------------------------");
                    Console.WriteLine("");
                }
                else if (userInput.ToLower() == "pe")
                {
                    PE(ref expensesString, ref expensesInt, ref expensesSum, salary, userInput, taxesO, taxesG, ref remainder, ref expensesCount, buySum);
                }
                else if (userInput.ToLower() == "ms")
                {
                    do
                    {
                        try
                        {
                            Console.WriteLine("Your new salary: ");
                            salary = Convert.ToInt32(Console.ReadLine());
                            noException = true;
                        }
                        catch (FormatException)
                        {
                            Console.Beep();
                            Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                        }
                    } while (noException == false);
                }
                else if (userInput.ToLower() == "me")
                {
                    do
                    {
                        try
                        {
                            Console.WriteLine("Your other constant monthly expenses (debt, food, etc.): ");
                            taxesO = Convert.ToInt32(Console.ReadLine());
                            noException = true;
                        }
                        catch (FormatException)
                        {
                            Console.Beep();
                            Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                        }
                    } while (noException == false);
                }
                else if (userInput.ToLower() == "mt")
                {
                    do
                    {
                        try
                        {
                            Console.WriteLine("New value of taxes: ");
                            taxesG = Convert.ToInt32(Console.ReadLine());
                            noException = true;
                        }
                        catch (FormatException)
                        {
                            Console.Beep();
                            Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                        }
                    } while (noException == false);
                }
                else if (userInput.ToLower() == "pin")
                {
                    Pin(expensesInt, expensesString, ref expensesPin);
                }
                else if (userInput.ToLower() == "mpe")
                {
                    MPE(expensesInt, expensesString, userInput, remainder, salary, taxesG, taxesO, buyInt, buyString, ref expensesCount);
                }
                else if (userInput.ToLower() == "stats" || userInput.ToLower() == "sts")
                {
                    Stats(salary, taxesG, taxesO, expensesInt, expensesString, buyInt, buyString, remainder, collect, expensesPin);
                }
                else if (userInput.ToLower() == "buy")
                {
                    Buy(ref buyInt, ref buyString, ref userInput, ref remainder, salary, taxesO, taxesG, expensesSum, buySum, ref buyCount);
                }
                else if (userInput.ToLower() == "msl")
                {
                    MSL(ref buyInt, ref buyString, userInput, remainder, salary, taxesG, taxesO, ref buyCount, ref buySum);
                }
                else if (userInput.ToLower() == "nm")
                {
                    NM(ref userInput, ref expensesInt, ref expensesString, ref buyInt, ref buyString, ref expensesSum, ref buySum, ref buyCount, ref expensesCount, expensesPin);
                }
                else if (userInput.ToLower() == "exit")
                {
                    break;
                }
                else if (userInput.ToLower() == "clear" || userInput.ToLower() == "clr")
                {
                    Console.Clear();
                }
                else if (userInput.ToLower() == "mc")
                {
                    do
                    {
                        try
                        {
                            Console.Write("Enter the amount of money you wish to save each month: ");
                            collect = Convert.ToInt32(Console.ReadLine());
                            noException = true;
                        }
                        catch (FormatException)
                        {
                            Console.Beep();
                            Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                        }
                    } while (noException == false);
                }
                else if (userInput.ToLower() == "wipe")
                {
                    Wipe(path, ref userInput, ref buyInt, ref buyString, ref buyCount, ref buySum, ref expensesInt, ref expensesString, ref expensesCount, ref expensesSum, ref salary, ref taxesO, ref taxesG, ref collect, ref expensesPin);
                }
                else
                {
                    Console.WriteLine("'{0}' is an invalid command! Type 'help' or 'h' to view list of available commands.", userInput.ToLower());
                }

            } while (loop == true);
        }
        public static void Pin(int[] expensesInt, string[] expensesString, ref bool[] expensesPin)
        {
            bool noException = false;
            bool loop = true;
            do
            {
                Console.WriteLine("Your current planned monthly expenses:");
                Console.WriteLine("");

                for (int i = 0; i < expensesInt.Length; i++)
                {
                    if (expensesInt[i] > 0)
                    {
                        if (expensesPin[i] != true)
                        {
                            Console.WriteLine("Item No. {0} - {1}: {2}", i + 1, expensesString[i], expensesInt[i]);
                        }
                        else
                        {
                            Console.WriteLine("Item No. {0} - {1}: {2} +", i + 1, expensesString[i], expensesInt[i]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                Console.WriteLine("");

                Console.WriteLine("Enter 'cancel' to cancel");
                Console.Write("Would you like to pin or to unpin an item? (p/u): ");
                string userInput = Console.ReadLine();

                if ((userInput.ToLower() == "p") || (userInput.ToLower() == "pin"))
                {
                    Console.Write("Enter the number of the item you wish to pin, or enter 'all' to pin all items. Enter 'cancel' to cancel: ");
                    userInput = Console.ReadLine();
                    int pin = 0;
                    if (userInput.ToLower() != "cancel")
                    {
                        if (userInput.ToLower() != "all")
                        {
                            do
                            {
                                try
                                {
                                    pin = Convert.ToInt32(userInput) - 1;
                                    noException = true;
                                }
                                catch (FormatException)
                                {
                                    Console.Beep();
                                    Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                                    Console.Write("Enter the number of the item you wish to pin, or enter 'all' to pin all items. Enter 'cancel' to cancel: ");
                                    userInput = Console.ReadLine();
                                }
                            } while (noException == false);
                        }
                        else
                        {
                            for (int i = 0; i < expensesPin.Length; i++)
                            {
                                expensesPin[i] = true;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }

                    expensesPin[pin] = true;
                }
                else if ((userInput.ToLower() == "u") || (userInput.ToLower() == "unpin"))
                {
                    Console.Write("Enter the number of the item you wish to unpin, or enter 'all' to unpin all items. Enter 'cancel' to cancel: ");
                    userInput = Console.ReadLine();
                    int pin = 0;
                    if (userInput.ToLower() != "cancel")
                    {
                        if (userInput.ToLower() != "all")
                        {
                            do
                            {
                                try
                                {
                                    pin = Convert.ToInt32(userInput) - 1;
                                    noException = true;
                                }
                                catch (FormatException)
                                {
                                    Console.Beep();
                                    Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                                    Console.Write("Enter the number of the item you wish to unpin, or enter 'all' to unpin all items. Enter 'cancel' to cancel: ");
                                    userInput = Console.ReadLine();
                                }
                            } while (noException == false);
                        }
                        else
                        {
                            for (int i = 0; i < expensesPin.Length; i++)
                            {
                                expensesPin[i] = false;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }

                    expensesPin[pin] = false;
                }
                else if (userInput.ToLower() == "cancel")
                {
                    break;
                }
            } while (loop == true);
        }
        public static void Wipe(string path, ref string userInput, ref int[] buyInt, ref string[] buyString, ref int buyCount, ref int buySum, ref int[] expensesInt, ref string[] expensesString, ref int expensesCount, ref int expensesSum, ref int salary, ref int taxesO, ref int taxesG, ref int collect, ref bool[] expensesPin)
        {
            Console.Write("Are you sure you want to erase all your data? (y/n): ");
            userInput = Console.ReadLine();

            if (userInput.ToLower() == "y")
            {
                try
                {
                    StreamWriter sw = new StreamWriter(path);
                    sw.WriteLine(0);
                    sw.Close();
                }
                catch (FileNotFoundException)
                {
                    StreamWriter createSaveFile = new StreamWriter(path);
                    createSaveFile.Close();
                }
                Console.Clear();
                Startup(path, ref salary, ref taxesG, ref taxesO, ref collect, ref buyString, ref buyInt, ref expensesString, ref expensesInt, ref buyCount, ref expensesCount, ref expensesPin);
            }
        }
        public static void NM(ref string userInput, ref int[] expensesInt, ref string[] expensesString, ref int[] buyInt, ref string[] buyString, ref int expensesSum, ref int buySum, ref int buyCount, ref int expensesCount, bool[] expensesPin)
        {
            Console.Write("Are you sure you want to start a new month? (y/n) ");
            userInput = Console.ReadLine();

            if (userInput.ToLower() == "y")
            {

                for (int i = 0; i < expensesInt.Length; i++)
                {
                    if (expensesPin[i] != true)
                    {
                        expensesInt[i] = 0;
                        expensesString[i] = " ";
                    }
                }

                for (int i = 0; i < buyInt.Length; i++)
                {
                    buyInt[i] = 0;
                    buyString[i] = " ";
                }

                buySum = 0;
                expensesSum = 0;
                buyCount = 0;
                expensesCount = 0;

                for (int i = 0; i < expensesPin.Length; i++)
                {
                    if (expensesPin[i] == true)
                    {
                        expensesCount++;
                    }
                }
                Console.WriteLine("New month started!");
            }
        }
        public static void MSL(ref int[] buyInt, ref string[] buyString, string userInput, int remainder, int salary, int taxesG, int taxesO, ref int buyCount, ref int buySum)
        {
            bool looop = true;
            do
            {
                Console.WriteLine("Your current shopping list:");
                Console.WriteLine("");

                for (int i = 0; i < buyInt.Length; i++)
                {
                    if (buyInt[i] > 0)
                    {
                        Console.WriteLine("Item No. {0} - {1}: {2}", i + 1, buyString[i], buyInt[i]);
                    }
                    else
                    {
                        break;
                    }
                }

                Console.WriteLine("");

                Console.WriteLine("Enter 'cancel' to cancel");
                Console.Write("Would you like to rename, delete or edit the value of an item? (r/d/e): ");
                userInput = Console.ReadLine();
                if (userInput.ToLower() == "e")
                {
                    bool noException = false;

                    do
                    {
                        try
                        {
                            Console.Write("Enter the number of the item you wish to modify: ");
                            userInput = Console.ReadLine();
                            Console.Write("Enter new value of {0}: ", buyString[(Convert.ToInt32(userInput)) - 1]);
                            buyInt[(Convert.ToInt32(userInput)) - 1] = Convert.ToInt32(Console.ReadLine());
                            noException = true;
                        }
                        catch (FormatException)
                        {
                            Console.Beep();
                            Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                        }
                    } while (noException == false);
                }
                else if (userInput.ToLower() == "r")
                {
                    int rename = 0;

                    bool noException = false;
                    do
                    {
                        Console.Write("Enter the number of the item you wish to rename: ");
                        userInput = Console.ReadLine();
                        try
                        {
                            rename = Convert.ToInt32(userInput);
                            noException = true;
                        }
                        catch (FormatException)
                        {
                            Console.Beep();
                            Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                        }
                    } while (noException == false);

                    Console.Write("Enter new name of {0}: ", buyString[rename - 1]);
                    buyString[rename - 1] = Console.ReadLine();
                }
                else if (userInput.ToLower() == "d")
                {
                    bool noException = false;
                    int del = 0;

                    Console.Write("Enter the number of the item you wish to delete, or enter 'all' to delete all items: ");
                    string UserInput = Console.ReadLine();

                    if (UserInput.ToLower() != "all")
                    {
                        do
                        {
                            try
                            {
                                del = Convert.ToInt32(UserInput);
                                noException = true;
                            }
                            catch (FormatException)
                            {
                                Console.Beep();
                                Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                                Console.Write("Enter the number of the item you wish to delete, or enter 'all' to delete all items: ");
                                UserInput = Console.ReadLine();
                            }
                        } while (noException == false);

                        Console.WriteLine("Are you sure you want to delete the following item from your shopping list?");
                        Console.WriteLine("- {0}: {1}", buyString[del - 1], buyInt[del - 1]);
                        Console.Write("(y/n): ");
                        userInput = Console.ReadLine();

                        if (userInput.ToLower() == "y")
                        {
                            buySum -= buyInt[del - 1];
                            do
                            {
                                buyString[del - 1] = buyString[del];
                                buyInt[del - 1] = buyInt[del];

                                del++;
                            } while ((del - 1) <= 28);

                            buyInt[29] = 0;
                            buyString[29] = " ";
                            buyCount--;

                            if (buyCount < 0)
                            {
                                buyCount = 0;  //A tételek száma nem lehet kisebb 1-nél (tömbök végett 0)
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Are you sure you want to delete all items from this list?");
                        if (Console.ReadLine().ToLower() == "y")
                        {
                            for (int i = 0; i < buyInt.Length; i++)
                            {
                                buyInt[i] = 0;
                                buyString[i] = "";
                            }
                        }
                        else
                        {
                            continue;
                        }
                        buyCount = 0;
                    }
                }
                else if (userInput.ToLower() == "cancel")
                {
                    break;
                }
            } while (looop == true);
        }
        public static void Buy(ref int[] buyInt, ref string[] buyString, ref string userInput, ref int remainder, int salary, int taxesO, int taxesG, int expensesSum, int buySum, ref int buyCount)
        {
            bool noException = false;
            Console.WriteLine("Note - enter 'cancel' when you have no more products you wish to regiser");

            for (int i = buyCount; i < buyInt.Length; i++)
            {
                Console.Write("Name of item No. {0}: ", i + 1);
                userInput = Console.ReadLine();
                if (userInput.ToLower() != "cancel")
                {
                    buyString[i] = userInput.ToLower();
                    do
                    {
                        try
                        {
                            Console.Write("Value of {0}: ", buyString[i]);
                            buyInt[i] = Convert.ToInt32(Console.ReadLine());
                            buyCount++;
                            buySum += buyInt[i];
                            noException = true;
                        }
                        catch (FormatException)
                        {
                            Console.Beep();
                            Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                            continue;
                        }
                    } while (noException == false);
                }
                else
                {
                    break;
                }
            }
        }
        public static void Stats(int salary, int taxesG, int taxesO, int[] expensesInt, string[] expensesString, int[] buyInt, string[] buyString, int remainder, int collect, bool[] expensesPin)
        {
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Salary: {0}", salary);
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Taxes: {0}", taxesG);
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Constant monthly expenses: {0}", taxesO);
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Expenses in current month:");
            Console.WriteLine("");
            Console.WriteLine("");

            if (expensesInt[0] != 0)
            {
                for (int i = 0; i < expensesInt.Length; i++)
                {
                    if (expensesInt[i] != 0)
                    {
                        if (expensesPin[i] != true)
                        {
                            Console.WriteLine("Item No. {0} - {1}: {2}", i + 1, expensesString[i], expensesInt[i]);
                        }
                        else
                        {
                            Console.WriteLine("Item No. {0} - {1}: {2} +", i + 1, expensesString[i], expensesInt[i]);
                        }
                        if (expensesInt[i + 1] != 0)
                        {
                            Console.WriteLine("");
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                Console.WriteLine("-----------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("You have no expenses planned for this month!");
                Console.WriteLine("-----------------------------------------------------------");
            }

            Console.WriteLine("");
            Console.WriteLine("Your shopping list:");
            Console.WriteLine("");
            Console.WriteLine("");
            if (buyInt[0] != 0)
            {
                for (int i = 0; i < buyInt.Length; i++)
                {
                    if (buyInt[i] != 0)
                    {
                        Console.WriteLine("Item No. {0} - {1}: {2}", i + 1, buyString[i], buyInt[i]);
                        if (buyInt[i + 1] != 0)
                        {
                            Console.WriteLine("");
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                Console.WriteLine("-----------------------------------------------------------");
            }
            else
            {
                Console.WriteLine("You have no items in your shopping list!");
                Console.WriteLine("-----------------------------------------------------------");
            }

            Console.WriteLine("");
            Console.WriteLine("Money left by the end of current month: {0}", remainder);
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Money to be collected each month: {0}", collect);
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("");
            if (remainder > (collect / 100 * 150)) //Figyelmeztet, ha a megadott limited 150 százalékát elérted, ha pontosan elérted azt illetve ha túlléped
            {
                Console.WriteLine("You still have money you can freely spend for this month!");
            }
            else if (((collect / 100 * 150) >= remainder) && (remainder > collect))
            {
                Console.WriteLine("You are running low on money for this month!");
            }
            else if (remainder == collect)
            {
                Console.WriteLine("You ran out of money for this month!");

            }
            else if (remainder < collect)
            {
                Console.WriteLine("You exceeded your monthly limit!");
            }
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("");
        }
        public static void MPE(int[] expensesInt, string[] expensesString, string userInput, int remainder, int salary, int taxesG, int taxesO, int[] buyInt, string[] buyString, ref int expensesCount)
        {
            bool l00p = true;
            bool noException = false;
            do
            {
                Console.WriteLine("Your current planned monthly expenses:");
                Console.WriteLine("");

                for (int i = 0; i < expensesInt.Length; i++)
                {
                    if (expensesInt[i] > 0)
                    {
                        Console.WriteLine("Item No. {0} - {1}: {2}", i + 1, expensesString[i], expensesInt[i]);
                    }
                    else
                    {
                        break;
                    }
                }

                Console.WriteLine("");

                Console.WriteLine("Enter 'cancel' to cancel");
                Console.Write("Would you like to rename, delete or edit the value of an item? (r/d/e): ");
                userInput = Console.ReadLine();
                if (userInput.ToLower() == "e")
                {
                    do
                    {
                        try
                        {
                            Console.Write("Enter the number of the item you wish to modify: ");
                            userInput = Console.ReadLine();

                            Console.Write("Enter new value of {0}: ", expensesString[(Convert.ToInt32(userInput)) - 1]);
                            expensesInt[(Convert.ToInt32(userInput)) - 1] = Convert.ToInt32(Console.ReadLine());
                            noException = true;
                        }
                        catch (FormatException)
                        {
                            Console.Beep();
                            Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                        }
                    } while (noException == false);

                    remainder = salary - (taxesG + taxesO);

                    for (int i = 0; i < buyInt.Length; i++)
                    {
                        if (expensesInt[i] != 0)
                        {
                            remainder -= buyInt[i];
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (userInput.ToLower() == "r")
                {
                    do
                    {
                        try
                        {
                            Console.Write("Enter the number of the item you wish to rename: ");
                            userInput = Console.ReadLine();

                            Console.Write("Enter new name of {0}: ", expensesString[(Convert.ToInt32(userInput)) - 1]);
                            expensesString[(Convert.ToInt32(userInput)) - 1] = Console.ReadLine();
                            noException = true;
                        }
                        catch (FormatException)
                        {
                            Console.Beep();
                            Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                        }
                    } while (noException == false);
                }
                else if (userInput.ToLower() == "d")
                {
                    Console.Write("Enter the number of the item you wish to delete, or enter 'all' to delete all items: ");
                    string UserInput = Console.ReadLine();
                    int del = 0;

                    if (UserInput.ToLower() != "all")
                    {
                        do
                        {
                            try
                            {
                                del = Convert.ToInt32(UserInput);
                                noException = true;
                            }
                            catch (FormatException)
                            {
                                Console.Beep();
                                Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                                Console.Write("Enter the number of the item you wish to delete, or enter 'all' to delete all items: ");
                                UserInput = Console.ReadLine();
                            }
                        } while (noException == false);

                        Console.WriteLine("Are you sure you want to delete the following item from your shopping list?");
                        Console.WriteLine("- {0}: {1}", expensesString[del - 1], expensesInt[del - 1]);
                        Console.Write("(y/n): ");
                        userInput = Console.ReadLine();

                        if (userInput.ToLower() == "y")
                        {
                            do
                            {
                                expensesString[del - 1] = expensesString[del];
                                expensesInt[del - 1] = expensesInt[del];

                                del++;
                            } while ((del - 1) <= 18);

                            expensesInt[19] = 0;
                            expensesString[19] = " ";
                            expensesCount--;

                            if (expensesCount < 0)
                            {
                                expensesCount = 0; //A tételek száma nem lehet kisebb 1-nél (tömbök végett 0)
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Are you sure you want to delete all items from this list?");
                        if (Console.ReadLine().ToLower() == "y")
                        {
                            for (int i = 0; i < expensesInt.Length; i++)
                            {
                                expensesInt[i] = 0;
                                expensesString[i] = "";
                            }
                            expensesCount = 0;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                else if (userInput.ToLower() == "cancel")
                {
                    break;
                }

            } while (l00p == true);
        }
        public static void Startup(string path, ref int salary, ref int taxesG, ref int taxesO, ref int collect, ref string[] expensesString, ref int[] expensesInt, ref string[] buyString, ref int[] buyInt, ref int buyCount, ref int expensesCount, ref bool[] expensesPin)
        {
            bool startup = true;
            do
            {
                StreamReader variableMethod = new StreamReader(path);

                try
                {

                    if (Convert.ToInt32(variableMethod.ReadLine()) > 0)
                    {
                        variableMethod.Close();
                        bool noException = false;
                        do
                        {
                            try
                            {
                                StreamReader sr = new StreamReader(path);
                                salary = Convert.ToInt32(sr.ReadLine());
                                taxesG = Convert.ToInt32(sr.ReadLine());
                                taxesO = Convert.ToInt32(sr.ReadLine());
                                collect = Convert.ToInt32(sr.ReadLine());
                                buyCount = Convert.ToInt32(sr.ReadLine());
                                expensesCount = Convert.ToInt32(sr.ReadLine());

                                for (int i = 0; i < expensesInt.Length; i++)
                                {
                                    expensesString[i] = sr.ReadLine();
                                    expensesInt[i] = Convert.ToInt32(sr.ReadLine());
                                }

                                for (int i = 0; i < buyInt.Length; i++)
                                {
                                    buyString[i] = sr.ReadLine();
                                    buyInt[i] = Convert.ToInt32(sr.ReadLine());
                                }

                                for (int i = 0; i < expensesPin.Length; i++)
                                {
                                    expensesPin[i] = Convert.ToBoolean(sr.ReadLine());
                                }

                                sr.Close();

                                startup = false;
                                noException = true;
                            }
                            catch (FileNotFoundException)
                            {
                                StreamWriter createSaveFile = new StreamWriter(path);
                                createSaveFile.Close();
                                continue;
                            }
                            catch (FormatException)
                            {
                                Console.Clear();
                                Console.WriteLine("It seems your save file has been edited by someone manually. They did something wrong though. Please check if the save file's contents are" +
                                    " valid. For example, if there are any letters in a line which should only contain numbers.");
                                Console.ReadKey();
                            }
                        } while (noException == false);
                    }
                    else
                    {
                        variableMethod.Close();
                        bool noException = false;
                        do
                        {
                            try
                            {
                                StreamWriter sw = new StreamWriter(path);
                                Console.Write("Your salary: ");
                                salary = Convert.ToInt32(Console.ReadLine());
                                sw.WriteLine(salary);
                                Console.Write("Your general taxes (sum): ");
                                taxesG = Convert.ToInt32(Console.ReadLine());
                                sw.WriteLine(taxesG);
                                Console.Write("Your other constant monthly expenses (debt, food, etc.): ");
                                taxesO = Convert.ToInt32(Console.ReadLine());
                                sw.WriteLine(taxesO);
                                Console.WriteLine("Money left by the end of each month (no other expenses): {0}", salary - (taxesG + taxesO));
                                Console.Write("Enter how much money you wish to keep by the end of each month: ");
                                collect = Convert.ToInt32(Console.ReadLine());
                                sw.WriteLine(collect);
                                sw.Close();

                                noException = true;
                            }
                            catch (FormatException)
                            {
                                Console.Beep();
                                Console.WriteLine("Something went wrong while recording your details. Did you enter letters instead of numbers somewhere?");
                                continue;
                            }
                            catch (FileNotFoundException)
                            {
                                Console.Beep();
                                Console.WriteLine("It seems that your save file got deleted while recording your details by something/someone. Creating a new one for you now...");

                                StreamWriter createSaveFile = new StreamWriter("save.txt");
                                createSaveFile.Close();
                                continue;
                            }
                        } while (noException == false);

                        Console.WriteLine("Details saved!");

                        startup = false;
                    }
                }
                catch (IOException)
                {
                    Console.Beep();
                    Console.WriteLine("Another process is using your save file, thus MEC can't access it. Close any applicationg using the file, then press any key to continue");
                    Console.ReadKey();
                }
            } while (startup == true);
        }
        public static void PE(ref string[] expensesString, ref int[] expensesInt, ref int expensesSum, int salary, string userInput, int taxesO, int taxesG, ref int remainder, ref int expensesCount, int buySum)
        {
            bool noException = false;
            Console.WriteLine("Note - enter 'cancel' when you have no more expenses you wish to register");
            Console.WriteLine("Expenses of your choise for this month (20 at most):");

            for (int i = expensesCount; i < expensesInt.Length; i++)
            {
                Console.Write("Name of expense No. {0}: ", expensesCount + 1);
                userInput = Console.ReadLine();
                if (userInput.ToLower() != "cancel")
                {
                    expensesString[i] = userInput.ToLower();
                    do
                    {
                        try
                        {
                            Console.Write("Value of {0}: ", expensesString[expensesCount]);
                            userInput = Console.ReadLine();
                            expensesInt[i] = Convert.ToInt32(userInput);
                            expensesSum += expensesInt[i];
                            expensesCount++;
                            noException = true;
                        }
                        catch (FormatException)
                        {
                            Console.Beep();
                            Console.WriteLine("You entered an invalid format! Perhaps you entered letters instead of numbers?");
                            continue;
                        }
                    }
                    while (noException == false);
                }
                else
                {
                    break;
                }
            }


            for (int i = 0; i < expensesCount; i++)
            {
                expensesSum += expensesInt[i];
            }

            remainder = (salary - (taxesG + taxesO + expensesSum + buySum));
        }
    }
}