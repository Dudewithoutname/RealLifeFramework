const mysql = require('mysql')

const pool = mysql.createPool(
    {
        connectionLimit: 32,
        host: '127.0.0.1',
        user: 'root',
        // password: '',
        database: 'unturned'
    }
)

pool.getConnection( (er, connection) => {
    if(!er)
    {
        if(connection)
        {
            connection.release()
            console.log('[Database Manager] : Successfuly connected to unturned database')

            tables.forEach( (sqlCommand) => connection.query(sqlCommand) )
        }
    }
    else{
        console.log(`[Database Manager] : Error in connection ${er}`)            
    }
})

module.exports = {
    execute: async (sqlCommand, isQuery) =>{
        if(isQuery || isQuery == null){
            return new Promise( (resolve, reject) => {
                pool.query(sqlCommand, (er, rows, fields) => {
                    (!er) ? resolve(rows) : reject(new Error(`in query execute ${er}`)) 
                })
            })
        }
        else{
            return new Promise( (resolve, reject) => {
                pool.query(sqlCommand, (er, rows, fields) => {
                    (!er) ? resolve() : reject(new Error(`in non-query execute ${er}`)) 
                })
            })
        }
    }
}

