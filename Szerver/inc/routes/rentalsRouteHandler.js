const express = require('express');
const router = express.Router();

const { inserRental, getRentalsByUserId } = require("../db_connect.js")

router.get('/', async(req, res, next) => {
    try{
        const userId = 1;
        res.status(200).json(await getRentalsByUserId(userId));
    }
    catch(e){
        next(e);
    }
});

router.post('/', async(req, res, next) => {
    try{
        const userId = 1;
        const carId = req.body.carId;
        const from = req.body.from;
        const to = req.body.to;
        console.log(req.body);
        let rentId = await inserRental(userId, carId, from, to);
        if(rentId == null){
            const error = new Error("Car is not available for selected period");
            error.status = 400;
            throw error;
        }
    
        res.status(201).json({
            message: 'Car rental successful',
            id: rentId.id
        });
    }
    catch(e){
        next(e);
    }
});


module.exports = router;