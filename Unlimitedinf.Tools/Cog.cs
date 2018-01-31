using Newtonsoft.Json;
using System;

namespace Unlimitedinf.Tools
{
    /// <summary>
    /// A wrapper for the standard <see cref="Console.WriteLine(string)"/> method that has shortcuts for printing in colors.
    /// Does not use the <see cref="Log"/> style of logging.
    /// </summary>
    public static class Cog
    {
        /// <summary>
        /// Log a default colored message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Def(string message)
            => Log.WriteLine(message);
        /// <summary>
        /// Log a default colored message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Def(object obj)
            => Def(JsonConvert.SerializeObject(obj));
        /// <summary>
        /// Log a default colored message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Default(string message)
            => Def(message);
        /// <summary>
        /// Log a default colored message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Default(object obj)
            => Def(JsonConvert.SerializeObject(obj));

        /// <summary>
        /// Log a <see cref="ConsoleColor.Gray"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Gra(string message)
            => Log.WriteLine(message, ConsoleColor.Gray);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Gray"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Gra(object obj)
            => Gra(JsonConvert.SerializeObject(obj));
        /// <summary>
        /// Log a <see cref="ConsoleColor.Gray"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Gray(string message)
            => Gra(message);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Gray"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Gray(object obj)
            => Gra(JsonConvert.SerializeObject(obj));

        /// <summary>
        /// Log a <see cref="ConsoleColor.DarkGray"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void DGr(string message)
            => Log.WriteLine(message, ConsoleColor.DarkGray);
        /// <summary>
        /// Log a <see cref="ConsoleColor.DarkGray"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void DGr(object obj)
            => DGr(JsonConvert.SerializeObject(obj));
        /// <summary>
        /// Log a <see cref="ConsoleColor.DarkGray"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void DarkGray(string message)
            => DGr(message);
        /// <summary>
        /// Log a <see cref="ConsoleColor.DarkGray"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void DarkGray(object obj)
            => DGr(JsonConvert.SerializeObject(obj));

        /// <summary>
        /// Log a <see cref="ConsoleColor.Blue"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Blu(string message)
            => Log.WriteLine(message, ConsoleColor.Blue);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Blue"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Blu(object obj)
            => Blu(JsonConvert.SerializeObject(obj));
        /// <summary>
        /// Log a <see cref="ConsoleColor.Blue"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Blue(string message)
            => Blu(message);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Blue"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Blue(object obj)
            => Blu(JsonConvert.SerializeObject(obj));

        /// <summary>
        /// Log a <see cref="ConsoleColor.Green"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Gre(string message)
            => Log.WriteLine(message, ConsoleColor.Green);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Green"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Gre(object obj)
            => Gre(JsonConvert.SerializeObject(obj));
        /// <summary>
        /// Log a <see cref="ConsoleColor.Green"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Green(string message)
            => Gre(message);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Green"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Green(object obj)
            => Gre(JsonConvert.SerializeObject(obj));

        /// <summary>
        /// Log a <see cref="ConsoleColor.Cyan"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Cya(string message)
            => Log.WriteLine(message, ConsoleColor.Cyan);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Cyan"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Cya(object obj)
            => Cya(JsonConvert.SerializeObject(obj));
        /// <summary>
        /// Log a <see cref="ConsoleColor.Cyan"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Cyan(string message)
            => Cya(message);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Cyan"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Cyan(object obj)
            => Cya(JsonConvert.SerializeObject(obj));

        /// <summary>
        /// Log a <see cref="ConsoleColor.Red"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Red(string message)
            => Log.WriteLine(message, ConsoleColor.Red);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Red"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Red(object obj)
            => Red(JsonConvert.SerializeObject(obj));

        /// <summary>
        /// Log a <see cref="ConsoleColor.Magenta"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Mag(string message)
            => Log.WriteLine(message, ConsoleColor.Magenta);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Magenta"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Mag(object obj)
            => Mag(JsonConvert.SerializeObject(obj));
        /// <summary>
        /// Log a <see cref="ConsoleColor.Magenta"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Magenta(string message)
            => Mag(message);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Magenta"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Magenta(object obj)
            => Mag(JsonConvert.SerializeObject(obj));

        /// <summary>
        /// Log a <see cref="ConsoleColor.Yellow"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Yel(string message)
            => Log.WriteLine(message, ConsoleColor.Yellow);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Yellow"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Yel(object obj)
            => Yel(JsonConvert.SerializeObject(obj));
        /// <summary>
        /// Log a <see cref="ConsoleColor.Yellow"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Yellow(string message)
            => Yel(message);
        /// <summary>
        /// Log a <see cref="ConsoleColor.Yellow"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Yellow(object obj)
            => Yel(JsonConvert.SerializeObject(obj));

        /// <summary>
        /// Log a <see cref="ConsoleColor.White"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Whi(string message)
            => Log.WriteLine(message, ConsoleColor.White);
        /// <summary>
        /// Log a <see cref="ConsoleColor.White"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void Whi(object obj)
            => Whi(JsonConvert.SerializeObject(obj));
        /// <summary>
        /// Log a <see cref="ConsoleColor.White"/> message.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void White(string message)
            => Whi(message);
        /// <summary>
        /// Log a <see cref="ConsoleColor.White"/> message.
        /// </summary>
        /// <param name="obj">The object to log with the default <see cref="JsonConvert.SerializeObject(object)"/>.</param>
        public static void White(object obj)
            => Whi(JsonConvert.SerializeObject(obj));

    }
}
