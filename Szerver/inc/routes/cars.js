const express = require('express');
const router = express.Router();

const { getCars, getCarById } = require("../db_connect.js")


router.get('/', (req, res, next) => {
    let category = req.query.category || null;
    res.status(200).json(getCars(category));
});
router.get('/:carId', (req, res, next) => {
    const carId = req.params.carId;
    let car = getCarById(carId);
    
    if(!car){
        next();
        return;
    }
    
    res.status(200).json(car);
});

module.exports = router;