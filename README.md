# Discord-Chat-Importer
Discord-Chat-Importer allows you to import Discord channels (exported with DiscordChatExporter) across Discord servers.

# Steps
## Exporting a channel to .json
* Download DiscordChatExporter: https://github.com/Tyrrrz/DiscordChatExporter
* Use DiscordChatExporter to export a channel to .json:
<br></br> [![Export Tutorial](https://i.imgur.com/78Ejkhp.jpg)](https://m.youtube.com/watch?v=tt-TBOWLyJk)

## Creating the bot
* Go to: https://discord.com/developers/applications
* Click "New Application" on the top-right.
* Give it a name and press `Create`
* On the left panel, click `OAuth2`
* Under Scopes, check `Bot`
* Under Bot Permissions, check `View Channels, Send Messages, Manage Messages, Embed Links, Attach Files, Read Message History`
* Copy the URL generated under Scopes and paste it into your browser
* Select the server you want the bot in
* Go back to the applicaiton page
* On the left panel, click `Bot`
* Click `Add Bot`
* Copy the Token, you'll need this for the next step.

**WARNING:** DO NOT try to use the bot in multiple servers at once as it was designed to only support 1. Create more bots if you have to.

## Starting the bot
* Download the executable from: https://github.com/Ethorbit/Discord-Chat-Importer/releases
* Open the Command Prompt or Terminal
* Run the executable with the argument as the token
   * Windows example: `"C:\Users\Ethorbit\source\repos\Discord-Channel-Importer\bin\Debug\netcoreapp3.1\Discord-Channel-Importer.exe" "MQENWQKNQWKRNWQRQOWR.WQEOJQWEODN3AKDNAKNXeIA"`


## Importing the .json
* Upload the .json to a url that will display its raw text
   * Example: https://www.dropbox.com/s/c5rvnig3pzeackp/Ethorbit%27s%20server%20-%20Text%20Channels%20-%20fkin-general%20%5B892291310927622204%5D.json?dl=0&raw=1
(Dropbox copied link with &raw=1 added at the end)

* Enter the command in: `!importer import <url> #channel`
<br></br> ![Import Preview](https://i.imgur.com/SZ1bOq9.png)

## Commands
You need the Manage Messages and Manage Channels permissions.
<br></br>Type `!importer help` for the command list.
* ![Command Preview](https://i.imgur.com/I684Agh.png)
