const express = require('express')
const app = express()
const main = require('../../main.js')

app.post('/tab', async (req, res) => {
    let rawMessage = req.body
    res.end()
    main.client.guilds.cache.map(guild => guild.id)
})

