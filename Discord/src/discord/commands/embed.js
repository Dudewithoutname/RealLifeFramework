const {MessageEmbed} = require('discord.js'); 

module.exports = {
    name: "embed",
    type: "server",
    run: async (client, message, args) => {
        let emb = new MessageEmbed()
            .setColor("#fb9d8f")
            .setAuthor("debug")

        message.channel.send(emb)

    }
}