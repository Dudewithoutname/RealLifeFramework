const { MessageEmbed } = require('discord.js')
const prefix = '!'

async function errorMessage (text, isChannel, message) {
    const mess = new MessageEmbed()
    .setColor("#ffbb00")
    .setTitle(":x: Error")
    .setDescription("**"+text+"**")

    (isChannel) ? await message.channel.send(mess) : await message.author.send(mess)    
}

module.exports = {
    handleCommand: async (client, message) => {
        const args = message.content.slice(prefix.length).trim().split(/ +/g)
        const cmd = args.shift().toLowerCase()
        const command = client.commands.get(cmd)

        if (!message.content.startsWith(prefix)) return
        
        if (!message.guild){  
            if(command.type === "private" && command)              
                command.run(client, message, args)
            else
                errorMessage("Tento príkaz môže byť použitý iba na serveri!", true, message)
        }
        else{
            if(command.type === "server" && command)                               
                command.run(client, message, args)
            else
                errorMessage("Tento príkaz môže byť použitý iba na serveri!", false, message)
        }
    }
}