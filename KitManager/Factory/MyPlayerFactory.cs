namespace CommandExpansion.Factory
{
	using PluginAPI.Core.Factories;
	using PluginAPI.Core.Interfaces;
	using System;

	public class MyPlayerFactory : PlayerFactory
	{
		public override Type BaseType { get; } = typeof(MyPlayer);

		public override IPlayer Create(IGameComponent component) => new MyPlayer(component);
	}
}
