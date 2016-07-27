using System.Collections.Generic;
using Wox.Plugin.StackOverlow.Infrascructure;

namespace Wox.Plugin.StackOverlow
{
	public class Main : IPlugin
	{
	    private UserQueryExecutor _executor;

	    public void Init(PluginInitContext context)
        {
            _executor = UserQueryExecutorFactory.Create(context);
        }

		public List<Result> Query(Query query)
		{
		    return _executor.Execute(query);
		}
	}
}