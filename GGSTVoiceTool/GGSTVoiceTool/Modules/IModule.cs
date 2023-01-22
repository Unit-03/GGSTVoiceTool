using Eto.Forms;

namespace GGSTVoiceTool
{
	public interface IModule
	{
		void SetupLayout(DynamicLayout layout);
		bool GenerateMod();
	}
}
