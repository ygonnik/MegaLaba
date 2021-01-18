namespace MegaLaba
{
    /// <summary>
    /// Класс настроек игры
    /// </summary>
    static class GameSettings
    {
        public static int ConsoleWidth { get; set; } = 80; // Ширина экрана косоли
        public static int ConsoleHeight { get; set; } = 30; // Высота экрана консоли
        //---
        public static int NumberOfSwarmRows { get; set; } = 2; // Кол-во рядов захватчиков
        public static int NumberOfSwarm { get; set; } = 60; // Кол-во захватчиков
        //---
        public static int SwarmStartX { get; set; } = 10; // Координата Х первоначальной отрисовки захватчика
        public static int SwarmStartY { get; set; } = 2; // Координата У первоначальной отрисовки захватчика
        //---
        public static char Invader { get; set; } = 'H'; // Первоначальная форма захватчика
        public static int SwarmSpeed { get; set; } = 3000; // Скорость передвижения захватчиков
        //---
        public static int PlayerStartX { get; set; } = 40; // Координата Х первоначальной отрисовки пользовательского корабля
        public static int PlayerStartY { get; set; } = 19; // Координата У первоначальной отрисовки пользовательского корабля
		public static char Player { get; set; } = '^'; // Первоначальная форма пользовательского корабля
        public static int PlayerLifes { get; set; } = 3;
        public static int MaxPlayerLifes { get; set; } = 3;
        public static int PlayerLifeRecoveryTime { get; set; } = 5000;
        //---
        public static int GroundStartX { get; set; } = 1; // Координата Х для земли
		public static int GroundStartY { get; set; } = 20; // Координата У для земли
		public static char Ground { get; set; } = '-'; // Первоначальная форма земли
        public static int NumberOfGroundRows { get; set; } = 1; // Кол-во рядов земли
        public static int NumberOfGround { get; set; } = 80; // Кол-во земли
        //---
        public static char Missle { get; set; } = '|'; // Первоначальная форма ракеты
        public static int MissleDamage { get; set; } = 1;
        public static int MissleSpeed { get; set; } = 100; // Скорость передвижения ракеты
        public static int MissleFrequency { get; set; } = 700; // Частота спауна ракет (1/ кол-во миллисекунд)
        public static int MaxInvaderMissles { get; set; } = 3; // Максимальное кол-во ракет пришельцев
        //---
        public static int GameSpeed { get; set; } = 5; // Скорость отрисовки игры

    }
}