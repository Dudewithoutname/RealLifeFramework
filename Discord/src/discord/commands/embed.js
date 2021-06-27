const {MessageEmbed} = require('discord.js'); 

module.exports = {
    name: "embed",
    type: "server",
    run: async (client, message, args) => {
        let emb = new MessageEmbed()
            .setColor("#ffffff")
            .setAuthor("stats debug")

        message.channel.send(emb)
    }
}