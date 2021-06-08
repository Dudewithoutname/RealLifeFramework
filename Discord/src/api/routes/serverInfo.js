const express = require('express')
const main = require('../../main.js')

let router = express.Router()

const tabCathegoryID = "849002618793885717"
router.post('/tab', async (req, res) => {
    const obj = req.body
    res.end()
    // time , players. weather, temperature
    const tab = await main.disClient.guilds.cache.get(main.guild).channels.cache.get(tabCathegoryID)
    await tab.setName(`| ${obj.players} 👥 | ${obj.time} ⌚| 23℃ | ⛈ |`)
})

module.exports = router