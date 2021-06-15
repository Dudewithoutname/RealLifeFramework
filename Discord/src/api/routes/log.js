const express = require('express')
const main = require('../../main.js')

let router = express.Router()

const CharacterChannel = "";

router.post('/logs/character', async (req, res) => {
    const obj = req.body
    res.end()
    const tab = await main.disClient.guilds.cache.get(main.guild).channels.cache.get(tabCathegoryID)

    if (obj.raining)
        await tab.setName(`| ${obj.players} 👥 | ${obj.time} ⌚| 🌞 |`)
    else
        await tab.setName(`| ${obj.players} 👥 | ${obj.time} ⌚| ☔ |`)   
})

module.exports = router