using System.Collections.Generic;
using Wox.Plugin.StackOverlow.Infrascructure;

namespace Wox.Plugin.StackOverlow
{
	public class Main : IPlugin, IPluginI18n
    {
	    private UserQueryExecutor _executor;

	    private PluginInitContext _context;

	    public void Init(PluginInitContext context)
	    {
	        _context = context;

	        _executor = UserQueryExecutorFactory.Create(context);
	    }

	    public List<Result> Query(Query query)
		{
		    return _executor.Execute(query); 
		}

	    public string GetTranslatedPluginTitle()
	    {
            return _context.API.GetTranslation("wox_plugin_so_plugin_name");
        }

	    public string GetTranslatedPluginDescription()
	    {
            return _context.API.GetTranslation("wox_plugin_so_plugin_description");
        }
    }
}