using EPiServer.Authorization;
using EPiServer.Shell.Navigation;

namespace PluginWithServerSentEvents.Business.AdminTools
{
	[MenuProvider]
	public class AdminMenuProvider : IMenuProvider
	{
		public IEnumerable<MenuItem> GetMenuItems()
		{
			yield return new UrlMenuItem(
				"Custom File Importer",
				MenuPaths.Global + "/cms/admintools",
				"/episerver/admintools/importer")
				{
					SortIndex = SortIndex.First + 25,
					AuthorizationPolicy = CmsPolicyNames.CmsAdmin
				};
		}
	}
}
