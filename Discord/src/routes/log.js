const express = require('express')
const main = require('../main.js')
const {MessageEmbed} = require('discord.js'); 
const fetch = require("node-fetch")

let router = express.Router()

const characterLogId = '854344663704010763'

/*
    steamId - string
    name - string
    age - int
    gender - string
    label - string 
*/
router.post('/logs/character', async (req, res) => {
    const obj = req.body
    res.end()

    const channel = await main.disClient.guilds.cache.get(main.guild).channels.cache.get(characterLogId)
    const player = await getSteamUser(obj.steamId)

    const embed = new MessageEmbed()
        .setColor("#e8d44f")
        .setAuthor(`${player.personaname} (${obj.steamId})`, player.avatar, `https://steamcommunity.com/profiles/${obj.steamId}`)
        .addField('**Meno**', obj.name, true)
        .addField('**Vek**', obj.age, true)
        .addField('**Pohlavie**', obj.gender, true)
        .setFooter(obj.label, 'https://i.ibb.co/qpsLPb3/lowrez.png')

    channel.send(embed)
})


const chatLogId = '854346215838711849'

/*
    steamId - string *
    color - string
    name - string *
    avatar - string
    message - string *
*/
router.post('/logs/chat', async (req, res) => {
    const obj = req.body
    res.end()
    if(obj.message == null || obj.message == ' ') return
    
    const channel = await main.disClient.guilds.cache.get(main.guild).channels.cache.get(chatLogId)

    const embed = new MessageEmbed()
        .setColor(obj.color)
        .setAuthor(obj.name, obj.avatar, `https://steamcommunity.com/profiles/${obj.steamId}`)
        .setDescription(obj.message)

    channel.send(embed)
})

const reportLogId = '867206290467192863'

/*
    steamId - string 
    name - string 
    message - string 
*/
router.post('/logs/report', async (req, res) => {
    const obj = req.body
    res.end()
    if(obj.message == null || obj.message == ' ') return

    const channel = await main.disClient.guilds.cache.get(main.guild).channels.cache.get(reportLogId)
    const player = await getSteamUser(obj.steamId)

    const embed = new MessageEmbed()
        .setColor("#ff0808")
        .setAuthor(obj.name, player.avatar, `https://steamcommunity.com/profiles/${obj.steamId}`)
        .setDescription(obj.message)

    channel.send(embed)
})

// ano viem lenivy som 
async function getSteamUser(steamId){
    const SteamUserURI = `https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=${main.steamAPIKey}&steamids=${steamId}`
    const steamUser = await fetch(SteamUserURI).then( (body) => body.json())

    return steamUser.response.players[0]
}

module.exports = router