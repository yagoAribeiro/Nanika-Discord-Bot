// Require the necessary discord.js classes
const { Client, Events, GatewayIntentBits, REST, Routes } = require('discord.js');
const { token, clientId} = require('./config.json');
const test_command = require('./slash_commands/test/test.js');


// Create a new client instance
const client = new Client({ intents: [GatewayIntentBits.Guilds] });

// When the client is ready, run this code (only once).
// The distinction between `client: Client<boolean>` and `readyClient: Client<true>` is important for TypeScript developers.
// It makes some properties non-nullable.
client.once(Events.ClientReady, readyClient => {
	console.log(`Ready! Logged in as ${readyClient.user.tag}`);
});

client.on(Events.InteractionCreate, async interaction => {
		console.log(interaction);
		await test_command.execute(interaction);
});

const rest = new REST().setToken(token);

(async() =>{
	await rest.put(
	Routes.applicationCommands(clientId),
	{ body: [test_command.data.toJSON()] },
);
})();
// Log in to Discord with your client's token
client.login(token);
