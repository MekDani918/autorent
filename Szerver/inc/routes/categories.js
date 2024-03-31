const express = require('express');
const router = express.Router();

const { getCategories } = require("../db_connect.js")


router.get('/', (req, res, next) => {
    res.status(200).json(getCategories());
});

module.exports = router;