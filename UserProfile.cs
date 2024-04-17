using System.Linq;

namespace USALauncher;

internal class UserProfile
{
	public string path;

	public UserProfile(string path)
	{
		this.path = path;
	}

	public override string ToString()
	{
		return path.Split('\\').Last().Replace("%20", " ");
	}
}
