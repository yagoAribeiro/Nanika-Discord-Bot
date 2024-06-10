const { SlashCommandBuilder } = require('discord.js');
const {RngSampleCalculator, RngSample} = require('../../game_builder/backend/bin/rng_calculator_imp.js')
module.exports = {
    data: new SlashCommandBuilder()
		.setName('pull')
		.setDescription('pull'),
    async execute(interaction) {
        let calc = new RngSampleCalculator(new RngSample(0, 400000, "Muito Comum", null),
        new RngSample(1, 320000, "Comum", null),
        new RngSample(2, 150000, "Incomum", null),
        new RngSample(3, 78000, "Raro", null),
        new RngSample(4, 34000, "Muito Raro", 0.1),
        new RngSample(5, 11500, "Épico", 0.25),
        new RngSample(6, 5500, "Exótico", 0.5),
        new RngSample(7, 1000, "Lendário", 1));
        await interaction.reply(calc.rollAndPull().event_name);
    }
}

