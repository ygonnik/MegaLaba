namespace MegaLaba
{
	/// <summary>
	/// Базовый класс объектов
	/// </summary>
	abstract class GameObject
	{
        public GameObjectLocation GameObjectLocation { get; set; }

        public char Figure { get; set; }
    }
}