const express = require('express');
const router = express.Router();

const { getCategories } = require("../db_connect.js")


router.get('/', async(req, res, next) => {
    try{
        res.status(200).json(await getCategories());
    }
    catch(e){
        next(e);
    }
});

module.exports = router;