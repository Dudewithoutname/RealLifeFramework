const {MessageEmbed} = require('discord.js'); 

module.exports = {
    name: "cicina",
    type: "server",
    run: async (client, message, args) => {

        const dlzka = Math.floor((Math.random() * 35)+ 1);

        const cicinaMSG = new MessageEmbed()
            .setColor("#fb9d8f")
            .setAuthor(`Tvoja cicina meria ${dlzka} cm`);
                        
        message.channel.send(cicinaMSG);
    }
}