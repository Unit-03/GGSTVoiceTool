using Eto.Forms;

namespace Harmony
{
	public interface IModule
	{
		void SetupLayout(DynamicLayout layout);

		bool CacheAssets();
		bool GenerateMod();
	}
}
