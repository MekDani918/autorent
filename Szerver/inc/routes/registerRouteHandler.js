const express = require('express');
const router = express.Router();

const { getUserByUsername, registerUser } = require("../db_connect.js");

router.post('/', async(req, res, next) => {
    try{
        const username = req.body.username;
        const name = req.body.name || ""
        const password = req.body.password;

        if(!username || !password){
            const err = new Error("No username and/or password!");
            err.status = 400;
            throw err;
        }
    
        let user = await getUserByUsername(username);
        if(user && username == user.username){
            const err = new Error("Username already taken!");
            err.status = 400;
            throw err;
        }
    
        user = await registerUser(username, name, password);
        
        res.status(201).json({
            message: "Registration successful!"
        });
    }
    catch(e){
        next(e);
    }
});

module.exports = router;