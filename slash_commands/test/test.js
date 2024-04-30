const { SlashCommandBuilder } = require('discord.js');

module.exports = {
    data: new SlashCommandBuilder()
		.setName('bora')
		.setDescription('bora?'),
    async execute(interaction) {
        await interaction.reply('BILLLLLLLL!!!!!!!!!!!!!!!!!');
    }
}

