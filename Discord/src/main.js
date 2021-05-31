const Discord = require('discord.js')
const fs = require('fs');
const { Client, RichEmbed,  Collection } = require('discord.js')
const { config } = require('dotenv')
const chat = require('./discord/chat.js');

config({ path: `${__dirname}/.env` })

const client = new Client({ disableEveryone: true })

client.commands = new Collection();

["command"].forEach(handler => {
    require(`./discord/${handler}`)(client)
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

// API PART
const express = require('express')
const http = require('http').createServer()
const cors = require('cors')
const app = express()
const PORT = 3003 || process.env.PORT

app.use(cors({
    origin: '*://localhost:*/*',
    optionsSuccessStatus: 200
}))

app.use(express.json({ limit: '4mb'}))

http.listen(PORT, () => console.log(`[API] : listening to http://localhost:${PORT}`))
