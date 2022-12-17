namespace KitManager
{
	using PluginAPI.Core;
	using PluginAPI.Core.Attributes;
	using PluginAPI.Events;

	public class MainClass
	{
		[PluginEntryPoint("KitManager", "1.0.0", "This plugin provide a kit manager for SCP-SL servers.", "Majorfox")]
		void LoadPlugin()
		{
			Log.Info("Loading KitManager...");
			EventManager.RegisterEvents(this);
			EventManager.RegisterEvents<EventHandlers>(this);
		}

		[PluginConfig]
		public Config PluginConfig;
	}
}
