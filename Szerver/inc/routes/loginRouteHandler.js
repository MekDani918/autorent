const express = require('express');
const router = express.Router();
const jwt = require('jsonwebtoken');

const { getUserByUsername } = require("../db_connect.js");

router.post('/', async(req, res, next) => {
    try{
        const username = req.body.username;
        const password = req.body.password;
    
        let user = await getUserByUsername(username);
        //console.log(user);
        if(!user || username != user.username || password != user.password){
            const err = new Error("Invalid username or password");
            err.status = 400;
            throw err;
        }
    
        const token = jwt.sign(
            {
                id: user.id,
                username: user.username,
                role: user.is_admin ? 'admin' : 'user'
            },
            process.env.JWT_KEY || 'secret',
            {
                expiresIn: "1h"
            }
        );
        
        res.status(200).json({
            message: "Auth successful",
            token: token
        });
    }
    catch(e){
        next(e);
    }
});

module.exports = router;