const express = require('express');
const router = express.Router();

const { getCars, getCarById, getCustomCarObject, createCar } = require("../db_connect.js");
const checkAuth = require("../authenticate_user.js");
const authorizeAdmin = require("../authorize_user.js");


router.get('/', async(req, res, next) => {
    try{
        let category = req.query.category || null;
        res.status(200).json(await getCars(category));
    }
    catch(e){
        next(e);
    }
});
router.post('/', checkAuth, authorizeAdmin, async(req, res, next) => {
    try{
        const brand = req.body.brand;
        const model = req.body.model;
        const categoryId = req.body.categoryId;
        const dailyPrice = req.body.dailyPrice;

        if(!brand || !model || !categoryId || !dailyPrice){
            const err = new Error("Invalid data supplied!");
            err.status = 400;
            throw err;
        }

        let car = await createCar(brand, model, categoryId, dailyPrice);
        
        res.status(201).json(await getCustomCarObject(car));
    }
    catch(e){
        next(e);
    }
});
router.get('/:carId', async(req, res, next) => {
    try{
        const carId = req.params.carId;
        let car = await getCustomCarObject(await getCarById(carId));
        
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
router.patch('/:carId', checkAuth, authorizeAdmin, async(req, res, next) => {
    try{
        const carId = req.params.carId;
        let car = await getCarById(carId);

        if(!car){
            next();
            return;
        }

        const brand = req.body.brand;
        const model = req.body.model;
        const categoryId = req.body.categoryId;
        const dailyPrice = req.body.dailyPrice;

        if(brand && car.brand != brand) car.brand = brand;
        if(model && car.model != model) car.model = model;
        if(categoryId && car.category_id != categoryId) car.category_id = categoryId;
        if(dailyPrice && car.daily_price != dailyPrice) car.daily_price = dailyPrice;
        
        await car.save();

        res.status(200).json(await getCustomCarObject(car));
    }
    catch(e){
        next(e);
    }
});
router.delete('/:carId', checkAuth, authorizeAdmin, async(req, res, next) => {
    try{
        const carId = req.params.carId;
        let car = await getCarById(carId);
        
        if(!car){
            next();
            return;
        }
        
        await car.destroy();

        res.status(200).json({
            message: 'Deleted successfully!'
        });
    }
    catch(e){
        next(e);
    }
});

module.exports = router;