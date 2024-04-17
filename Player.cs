namespace USALauncher;

public class Player
{
	public string Name { get; set; }

	public Player()
		: this(null)
	{
	}

	public Player(string name)
	{
		Name = name;
	}
}
