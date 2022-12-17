namespace KitManager.Commands
{
	using CommandSystem;
	using InventorySystem;
	using InventorySystem.Items;
	using InventorySystem.Items.Firearms;
	using InventorySystem.Items.Firearms.Attachments;
	using PluginAPI.Core;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Utils;

	/// <summary>
	/// Give a predefined inventory to a player
	/// - KitManager Command
	/// </summary>
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class GivePredefinedInventory : ICommand, IUsageProvider
	{
		public string Command { get; } = "givepredefinedinventory";

		public string[] Aliases { get; } = new string[] { "gpi", "kit" };

		public string Description { get; } = "Give a predefined inventory to a player";

		public string[] Usage { get; } = new string[2] { "%player%", "InventoryID" };

		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			if (!sender.CheckPermission(PlayerPermissions.GivingItems, out response))
			{
				return false;
			}

			if (arguments.Count >= 2)
			{
				string[] inventoryID;
				string itemslist = string.Empty;
				string arg = string.Empty;
				int num = 0;
				int num2 = 0;

				List<ReferenceHub> list = RAUtils.ProcessPlayerIdOrNamesList(arguments, 0, out inventoryID);

				if (inventoryID == null || inventoryID.Length == 0)
				{
					response = "You must specify the inventory ID to give.";
					return false;
				}
				// It's the folder that'll contain your own inventory sets
				string[] lines = System.IO.File.ReadAllLines(@"Data/Inventories.txt");

				foreach (string line in lines)
				{
					if (inventoryID[0].Equals(line.Split(':')[0]))
					{
						itemslist=line.Split(':')[1];
					}
				}
				ItemType[] array = ParseItems(itemslist).ToArray();
				if (array.Length == 0)
				{
					response = "You didn't input a valid inventoryID (example: kit <playerID> testInventory).";
					return false;
				}

				Log.Info($"Giving the {inventoryID[0]} inventory to ({sender.LogName})");
				foreach (ReferenceHub item in list)
				{
					try
					{
						foreach (ItemType id in array)
						{
							AddItem(item, sender, id);
						}
					}
					catch (Exception ex)
					{
						num++;
						arg = ex.Message;
						continue;
					}

					num2++;
				}


				response = ((num == 0) ? string.Format("Done! The request affected {0} player{1}", num2, (num2 == 1) ? "!" : "s!") : $"Failed to execute the command! Failures: {num}\nLast error log:\n{arg}");
				return true;
			}

			response = "To execute this command provide at least 2 arguments!\nUsage: " + arguments.Array[0] + " " + this.DisplayCommandUsage();
			return false;
		}
		private IEnumerable<ItemType> ParseItems(string argument)
		{
			string[] array = argument.Split('.');
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				if (int.TryParse(array2[i], out var result) && Enum.IsDefined(typeof(ItemType), result))
				{
					yield return (ItemType)result;
				}
			}
		}

		private void AddItem(ReferenceHub ply, ICommandSender sender, ItemType id)
		{
			ItemBase itemBase = ply.inventory.ServerAddItem(id, 0);
			ServerLogs.AddLog(ServerLogs.Modules.Administrative, $"{sender.LogName} gave {id} to {ply.LoggedNameFromRefHub()}.", ServerLogs.ServerLogType.RemoteAdminActivity_GameChanging);
			if (itemBase == null)
			{
				throw new NullReferenceException($"Could not add {id}. Inventory is full or the item is not defined.");
			}

			Firearm firearm;
			if ((object)(firearm = itemBase as Firearm) != null)
			{
				if (AttachmentsServerHandler.PlayerPreferences.TryGetValue(ply, out var value) && value.TryGetValue(itemBase.ItemTypeId, out var value2))
				{
					firearm.ApplyAttachmentsCode(value2, reValidate: true);
				}

				FirearmStatusFlags firearmStatusFlags = FirearmStatusFlags.MagazineInserted;
				if (firearm.HasAdvantageFlag(AttachmentDescriptiveAdvantages.Flashlight))
				{
					firearmStatusFlags |= FirearmStatusFlags.FlashlightEnabled;
				}

				firearm.Status = new FirearmStatus(firearm.AmmoManagerModule.MaxAmmo, firearmStatusFlags, firearm.GetCurrentAttachmentsCode());
			}
		}
	}
}