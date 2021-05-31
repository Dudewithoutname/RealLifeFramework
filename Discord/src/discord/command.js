const { readdirSync } = require("fs");
const ascii = require("ascii-table");
const table = new ascii().setHeading("File", "Status").setBorder(".");


module.exports = (client) => {
    const commands = readdirSync('./src/discord/commands/').filter(f => f.endsWith(".js"));

    for (let file of commands) {
        const pull = require(`./commands/${file}`);
        
        if (pull.name) {
            table.addRow(file, `OK`);
            client.commands.set(pull.name, pull);
        } 
        else {
            table.addRow(file, `ERROR`);
            continue;
        }
    }
}