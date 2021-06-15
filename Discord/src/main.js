const Discord = require('discord.js')
const fs = require('fs');
const { Client, RichEmbed,  Collection } = require('discord.js')
const { config } = require('dotenv')
const express = require('express')
const app = express()
const PORT = 3003

const chat = require('./discord/chat.js');

config({ path: `${__dirname}/.env` })

const client = new Client({ disableEveryone: true })

client.commands = new Collection();

["command"].forEach(handler => {
    require(`./discord/${handler}`)(client)
})

app.use(express.json())

app.use( (req, res, next) => { (req.body.token != '2a8d9023i0our897u9wqf') ? res.end('Invalid token ty kokot! :)') : next() } )

app.get('/', async (req, res) => res.end() )


client.on('ready', () =>{
    client.user.setActivity('Dudeturned (^.^)', {type:'WATCHING'})
    console.log('[Bot] is ready m8 :) ')
    setupRoutes()
})

client.on('message', async message => {
    if(!message.author.bot)
        chat.handleCommand(client, message)
})

client.login(process.env.TOKEN)
app.listen(PORT, () => console.log(`[API] : ${PORT}`))

function setupRoutes(){
    const serverInfo = require('./api/routes/serverInfo')
    app.use(serverInfo)

    const log = require('./api/routes/log')
    app.use(log)
}

module.exports = {
    disClient: client,
    guild: "830839683549757511",
}