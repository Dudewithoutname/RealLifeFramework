const express = require('express')
const main = require('../main.js')
const {MessageEmbed} = require('discord.js'); 
const fetch = require("node-fetch")

let router = express.Router()



const tabId = '859447328004243516'

/*
    players - int
    time - string
    night - bool
*/
router.post('/info/tab', async (req, res) => {
    const obj = req.body
    res.end()

    if(obj.time == "offline")
    {
        await main.disClient.user.setActivity('Dudeturned (^.^) | Server Offline', {type:'WATCHING'})
        return
    }

    if (!obj.night)
        await main.disClient.user.setActivity(`| ${obj.players} 👥 | ${obj.time} 🌞 |`).catch(console.error);
    else
        await main.disClient.user.setActivity(`| ${obj.players} 👥 | ${obj.time} 🌙 |`).catch(console.error);
})


const bansChannelId = '858749993829400627'

/*
    steamId - string
    characterName - string
    provider - string
    reason - string
    time - int 
*/
router.post('/info/bans', async (req, res) => {
    const obj = req.body
    res.end()

    const channel = await main.disClient.guilds.cache.get(main.guild).channels.cache.get(bansChannelId)
    const player = await getSteamUser(obj.steamId)
    const time = (parseInt(obj.time) <= 78800000)? formatTime(obj.time) : 'Permanentný'
    const date = new Date().toJSON().slice(0,10).replace(/-/g,'.');

    const embed = new MessageEmbed()
        .setColor("#ff1717")
        .setAuthor(`Ban | ${obj.characterName}`, player.avatar, `https://steamcommunity.com/profiles/${obj.steamId}`)
        .addField('🤵 Admin ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍', obj.provider, true)
        .addField('🕐 Dĺžka banu ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍', time, true)
        .addField('📖 Dvôvod', obj.reason, false)
        .setFooter(`Dátum • ${date}`, 'https://cdn.discordapp.com/avatars/843181656025333800/f636cdac55d0c5404ec8e614c30a7635.png?size=64')

    channel.send(embed)
})


const statsChannelId = '849008891110883328'
const statsMessageId = '859135258057768960'
let IP = '69.420.69.420'
let port = '27015'

/*
    online - bool*
    players - int
    ems - int
    pd - int
    serverIP - string
    serverPort - string
*/
router.post('/info/stats', async (req, res) => {
    const obj = req.body
    res.end()

    const channel = await main.disClient.guilds.cache.get(main.guild).channels.cache.get(statsChannelId)
    let embed;

    if(obj.online){
        IP = obj.serverIP
        port = obj.serverPort

        embed = new MessageEmbed()
            .setColor("#fb9d8f")
            .setAuthor(`‏CZ/SK | DudeTurned Roleplay`, 'https://i.ibb.co/qpsLPb3/lowrez.png')
            .addField(':video_game: IP ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍', IP, true)
            .addField(':keyboard: Port ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍', port, true)    
            .addField(':green_circle: Status ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍', 'Online', true)
            .addField('👥 Hráči', obj.players, true)
            .addField('🚓 PD', obj.pd, true)
            .addField('🚑 EMS', obj.ems, true);      
    }
    else{
        embed = new MessageEmbed()
        .setColor("#fb9d8f")
        .setAuthor(`‏CZ/SK | DudeTurned Roleplay‎`, 'https://i.ibb.co/qpsLPb3/lowrez.png')
        .addField(':video_game: IP ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍', IP, true)
        .addField(':keyboard: Port ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍', port, true)    
        .addField(':red_circle: Status ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍‍‍‍‍‍‍‍‍‍ ‍‍ ‍‍‍‍‍‍‍‍‍‍‍', 'Offline', true)
        .addField('👥 Hráči', `0`, true)
        .addField('🚓 PD', `0`, true)
        .addField('🚑 EMS', `0`, true);    
    }
    
    channel.messages.fetch(statsMessageId).then(mess => mess.edit(embed)) 
})


function formatTime(rawTime){
    const time = parseInt(rawTime)
    const d = Math.floor(time / (3600*24))
    const h = Math.floor(time % (3600*24) / 3600)
    const m = Math.floor(time % 3600 / 60)

    return `${d} Dní ${h} Hodín ${m} Minút`
}

async function getSteamUser(steamId){
    const SteamUserURI = `https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=${main.steamAPIKey}&steamids=${steamId}`
    const steamUser = await fetch(SteamUserURI).then( (body) => body.json())

    return steamUser.response.players[0];
}

module.exports = router