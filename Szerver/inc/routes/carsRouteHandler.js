const express = require('express');
const router = express.Router();

const { getCars, getCarById } = require("../db_connect.js")


router.get('/', async(req, res, next) => {
    try{
        let category = req.query.category || null;
        res.status(200).json(await getCars(category));
    }
    catch(e){
        next(e);
    }
});
router.get('/:carId', async(req, res, next) => {
    try{
        const carId = req.params.carId;
        let car = await getCarById(carId);
        
        if(!car){
            next();
            return;
        }
        
        res.status(200).json(car);
    }
    catch(e){
        next(e);
    }
});

module.exports = router;