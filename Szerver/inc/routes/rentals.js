const express = require('express');
const router = express.Router();

const { inserRental, getRentalsByUserId } = require("../db_connect.js")

router.get('/', (req, res, next) => {
    res.status(200).json(getRentalsByUserId());
});

router.post('/', (req, res, next) => {
    const id = req.params.carId;
    let rentId = inserRental(id, '1711042568', '1711483593');
    if(rentId == null){
        const error = new Error("Car is not available for selected period");
        error.status = 400;
        throw error;
    }

    res.status(201).json({
        message: 'Car rental successful',
        id: rentId
    });
});


module.exports = router;