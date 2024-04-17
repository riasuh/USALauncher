using System.Collections.Generic;
using System.Linq;

namespace USALauncher;

public class PlayerCollection : List<Player>
{
	public PlayerCollection()
	{
	}

	public PlayerCollection(IEnumerable<Player> collection)
		: base(collection)
	{
	}

	public static PlayerCollection Parse(string data)
	{
		string text = "";
		string[] array = data.Split(default(char));
		int num = 0;
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			if (num == 0 && text2 != "")
			{
				text = text + text2 + "\0";
			}
			num++;
			if (num == 4)
			{
				num = 0;
			}
		}
		return new PlayerCollection(from s in text.Split(default(char))
			select new Player(s));
	}
}
