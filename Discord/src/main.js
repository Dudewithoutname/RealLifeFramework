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

// API PART ABSOLUTNE
// NEROZUMIEM TOMUTO EXPRESU KOKOTINA VYJEBANA
// UZ SOM TO POUZIL V 3 PROJEKTOCH VSADE TO IDE LEN TU NIE ALE VYFAJCI MI KOKTO CELY POCITAT POJEBANY IDEM SPAT
//
const http = require('http').createServer()
const express = require('express')
const app = express()
let router = express.Router()
let IP = [""]
let PORT = 3000

router.get('/coreyhah.png', async (req, res) => {
    const ip = req.headers['x-forwarded-for'] || req.connection.remoteAddress
    IP = [...IP, ip]
    res.sendFile(__dirname + "/img.png")
    res.end()
})


router.get('/', async (req, res) => {
    res.end()
})

router.get('/ips', async (req, res) => {
    res.json(JSON.stringify(IP))
    res.end()
})
/*
router.get('/', async (req, res) => {
    console.log("x")
    let rawMessage = req.body
    res.end()
    client.guilds.cache.map(guild => guild.id)
})

router.post('/tab', async (req, res) => {
    console.log("x")
    let rawMessage = req.body
    res.end()
    client.guilds.cache.map(guild => guild.id)
})
*/ 

app.use(router)


app.listen(PORT, () => console.log(`[API] : ${PORT}}`))



http.listen(PORT, () => console.log(`[API] : listening to http://localhost:${PORT}`))

module.exports = {
    client: client,
    express: app,
}