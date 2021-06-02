const {MessageEmbed} = require('discord.js'); 

module.exports = {
    name: "embed",
    type: "server",
    run: async (client, message, args) => {
        const dlzka = Math.floor((Math.random() * 35) + 1);

        let cicinaMSG = new MessageEmbed()
            .setColor("#ffffff")
            .setAuthor("Tvoja cicina meria "+ dlzka+ " cm")

        console.log(cicinaMSG)
    }
}