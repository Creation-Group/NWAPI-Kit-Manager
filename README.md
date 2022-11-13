
# Kit Manager Plugin

This plugin provide a kit manager for SCP-SL servers.
You can give a batch of items to a player in a single command.

*[This solution is using the official server-side plugin system for SCP: Secret Laboratory game. NW-API](https://github.com/northwood-studios/NwPluginAPI)*

-----

## How to build the plugin without existing project:

1) Download the repository

2) Then integrate the **[NwPluginAPI](https://github.com/northwood-studios/NwPluginAPI)** folder

3) Run the NWAPI-Kit-Manager.sln

4) Open any file in the Kit Manager namespace

5) Generate Kit Manager

-----

## List of commands

### givepredefinedinventory [GivePredefinedInventory.cs]

*Aliases :* gpi, kit

*Description :* Give a predefined inventory to a player. It works like a kit.
                A folder contains a Inventory.txt 
                Each line is an inventory "**<name>**:**<itemsID>.<...>**
                Up to 8 items (maxInventory). A '.' is used to separate each item.

*Usage :* command %player% %PredefinedInventoryID%

-----
