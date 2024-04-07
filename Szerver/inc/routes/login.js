const express = require('express');
const router = express.Router();

const { getUserByUsername } = require("../db_connect.js")

router.post('/', (req, res, next) => {
    const username = req.body.username;
    const password = req.body.password;

    let user = getUserByUsername(username);
    if(username != user.username || password != user.password){
        const err = new Error("Invalid username or password");
        err.status = 400;
        throw err;
    }

    res.status(200).json({
        message: "Auth successful",
        token: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
    });
});

module.exports = router;