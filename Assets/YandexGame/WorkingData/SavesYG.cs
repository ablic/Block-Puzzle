
namespace YG
{
	[System.Serializable]
	public class SavesYG
	{
		// "Технические сохранения" для работы плагина (Не удалять)
		public int idSave;
		public bool isFirstSession = true;
		public string language = "ru";
		public bool promptDone;

		public bool isSoundOn = true;
		public bool isMusicOn = true;
		public int bestScore = 0;

		// Вы можете выполнить какие то действия при загрузке сохранений
		public SavesYG()
		{
			// Допустим, задать значения по умолчанию для отдельных элементов массива

		}
	}
}
