const Discord = require('discord.js')
const fs = require('fs');
const { Client, RichEmbed,  Collection } = require('discord.js')
const { config } = require('dotenv')
const chat = require('./components/chat.js');

config({ path: `${__dirname}/.env` })

const client = new Client({ disableEveryone: true })

client.commands = new Collection();

["command"].forEach(handler => {
    require(`./components/${handler}`)(client)
})

client.on('ready', () =>{
    client.user.setActivity('Dudeturned (^.^)', {type:'WATCHING'})
    console.log('[Bot] is m8 :) ')
})

client.on('message', async message => {
    if(!message.author.bot)
        chat.handleCommand(client, message)
})

client.login(process.env.TOKEN)