using System;

namespace MegaLaba
{
    class UIController
    {
        public event EventHandler LeftArrowPressed;
        public event EventHandler RightArrowPressed;
        public event EventHandler SpacebarPressed; //  События на нажатие клавиши Space

		/// <summary>
		/// Метод начала детектирования нажатий на клавиши
		/// </summary>
		public void StartListening()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true); // Считывание клавиши без вывода на экран

                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey();
                    }

                    if (key.Key.Equals(ConsoleKey.LeftArrow))
                    {
                        LeftArrowPressed(this, new EventArgs());
                    }

                    if (key.Key.Equals(ConsoleKey.RightArrow))
                    {
                        RightArrowPressed(this, new EventArgs());
                    }

                    if (key.Key.Equals(ConsoleKey.Spacebar))
                    {
                        SpacebarPressed(this, new EventArgs());
                    }
                }
            }

        }
    }
}
