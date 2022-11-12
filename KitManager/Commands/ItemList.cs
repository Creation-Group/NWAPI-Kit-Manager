namespace CommandExpansion.Commands
{
	using CommandSystem;
	using System;

	/// <summary>
	/// Returns the list of available items and their ID
	/// - KitManager Command
	/// </summary>
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class ItemList : ICommand
	{
		public string Command { get; } = "itemlist";

		public string[] Aliases { get; } = new string[] { "itemdetails" };

		public string Description { get; } = "Returns the list of available items and their ID";

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			string itemlist = "\n";

			for (int i = 0; i < Enum.GetNames(typeof(ItemType)).Length-1; i++)
			{
				itemlist += $"[<color=green>ID: {i}</color>";
				itemlist += string.Format((i < 10) ? " " : "");
				itemlist += $"] - { (ItemType)i}\n";
			}

			response = itemlist;
			return true;
		}
	}
}