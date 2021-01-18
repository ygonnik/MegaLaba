using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Office = Microsoft.Office.Interop;


namespace ComObjects
{
    [Guid("47025754-810C-4860-B3E7-8648111E0F02")]
    [ComVisible(true)]
    public interface ISettings
    {
        int ConsoleWidth { get; set; } // Ширина экрана косоли
        int ConsoleHeight { get; set; } // Высота экрана консоли
       //---
        int NumberOfSwarmRows { get; set; } // Кол-во рядов захватчиков
        int NumberOfSwarm { get; set; } // Кол-во захватчиков
      //---
        int SwarmStartX { get; set; } // Координата Х первоначальной отрисовки захватчика
        int SwarmStartY { get; set; } // Координата У первоначальной отрисовки захватчика
       //---
        char Invader { get; set; } // Первоначальная форма захватчика
        int SwarmSpeed { get; set; }  // Скорость передвижения захватчиков
       //---
        int PlayerStartX { get; set; } // Координата Х первоначальной отрисовки пользовательского корабля
        int PlayerStartY { get; set; } // Координата У первоначальной отрисовки пользовательского корабля
        char Player { get; set; }  // Первоначальная форма пользовательского корабля
        int PlayerLifes { get; set; } 
        int MaxPlayerLifes { get; set; } 
        int PlayerLifeRecoveryTime { get; set; } 
       //---
        int GroundStartX { get; set; }// Координата Х для земли
        int GroundStartY { get; set; }  // Координата У для земли
        char Ground { get; set; } // Первоначальная форма земли
        int NumberOfGroundRows { get; set; } // Кол-во рядов земли
        int NumberOfGround { get; set; }// Кол-во земли
        //---
        char Missle { get; set; } // Первоначальная форма ракеты
        int MissleDamage { get; set; } 
        int MissleSpeed { get; set; }  // Скорость передвижения ракеты
        int MissleFrequency { get; set; }  // Частота спауна ракет (1/ кол-во миллисекунд)
        int MaxInvaderMissles { get; set; }  // Максимальное кол-во ракет пришельцев
        //---
        int GameSpeed { get; set; } // Скорость отрисовки игры

        List<dynamic> GetSettings(string path);
    }

    [Guid("74C56FC5-3037-43B3-B1D2-14434CEC17BF")]
    [ComVisible(true)]
    public interface IGameObjectPlayer
    {
        int X { get; set; }
        int Y { get; set; }

        char Figure { get; set; }
    }

    [Guid("BD6D7944-245F-4883-B8AB-D08C83066910")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class GameSettings : ISettings
    {
        public int ConsoleWidth { get; set; } = 80; // Ширина экрана косоли
        public int ConsoleHeight { get; set; } = 30; // Высота экрана консоли
        //---
        public int NumberOfSwarmRows { get; set; } = 2; // Кол-во рядов захватчиков
        public int NumberOfSwarm { get; set; } = 60; // Кол-во захватчиков
        //---
        public int SwarmStartX { get; set; } = 10; // Координата Х первоначальной отрисовки захватчика
        public int SwarmStartY { get; set; } = 2; // Координата У первоначальной отрисовки захватчика
        //---
        public char Invader { get; set; } = 'H'; // Первоначальная форма захватчика
        public int SwarmSpeed { get; set; } = 3000; // Скорость передвижения захватчиков
        //---
        public int PlayerStartX { get; set; } = 40; // Координата Х первоначальной отрисовки пользовательского корабля
        public int PlayerStartY { get; set; } = 19; // Координата У первоначальной отрисовки пользовательского корабля
        public char Player { get; set; } = '^'; // Первоначальная форма пользовательского корабля
        public int PlayerLifes { get; set; } = 3;
        public int MaxPlayerLifes { get; set; } = 3;
        public int PlayerLifeRecoveryTime { get; set; } = 5000;
        //---
        public int GroundStartX { get; set; } = 1; // Координата Х для земли
        public int GroundStartY { get; set; } = 20; // Координата У для земли
        public char Ground { get; set; } = '-'; // Первоначальная форма земли
        public int NumberOfGroundRows { get; set; } = 1; // Кол-во рядов земли
        public int NumberOfGround { get; set; } = 80; // Кол-во земли
        //---
        public char Missle { get; set; } = '|'; // Первоначальная форма ракеты
        public int MissleDamage { get; set; } = 1;
        public int MissleSpeed { get; set; } = 100; // Скорость передвижения ракеты
        public int MissleFrequency { get; set; } = 700; // Частота спауна ракет (1/ кол-во миллисекунд)
        public int MaxInvaderMissles { get; set; } = 3; // Максимальное кол-во ракет пришельцев
        //---
        public int GameSpeed { get; set; } = 5; // Скорость отрисовки игры

        public List<dynamic> GetSettings(string path)
        {
            List<dynamic> list = new List<dynamic>();
            Office.Excel.Application excel = new Office.Excel.Application();
            Office.Excel.Workbook workbook = excel.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                 Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            Office.Excel.Worksheet worksheet = (Office.Excel.Worksheet)workbook.Sheets[1]; // Переходим к листу
            var lastCell = worksheet.Cells.SpecialCells(Office.Excel.XlCellType.xlCellTypeLastCell); // Последняя ячейка

            for (int i = 1; i <= (int)lastCell.Row; i++) // По всем строкам
            {
                list.Add(worksheet.Cells[i, 2].Text.ToString());
            }

            return list;
        }
    }

    [Guid("FCF93056-2906-438F-B410-E6C1D2FCCE14")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComVisible(true)]
    public class Player : IGameObjectPlayer
    {
        public Player() { }

        public Player(int x, int y, char figure)
        {
            X = x;
            Y = y;
            Figure = figure;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public char Figure { get; set; }
    }

}
