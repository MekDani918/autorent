const express = require('express');
const router = express.Router();

const { createSale, getSaleByCarId, getSales } = require("../db_connect.js");
const checkAuth = require("../authenticate_user.js");
const authorizeAdmin = require("../authorize_user.js");


router.get('/', async(req, res, next) => {
    try{
        res.status(200).json(await getSales());
    }
    catch(e){
        next(e);
    }
});
router.post('/', checkAuth, authorizeAdmin, async(req, res, next) => {
    try{
        const carId = req.body.carId;
        const description = req.body.description;
        const percent = req.body.percent;

        if(!carId || !percent){
            const err = new Error("Invalid data supplied!");
            err.status = 400;
            throw err;
        }

        let sale = await createSale(carId, description, percent);

        if(!sale){
            return res.status(400).json({
                message: 'Sale already exists for this car'
            });
        }
        
        res.status(201).json(sale);
    }
    catch(e){
        next(e);
    }
});
router.delete('/:saleId', checkAuth, authorizeAdmin, async(req, res, next) => {
    try{
        const saleId = req.params.saleId;
        let sale = await getSaleByCarId(saleId);
        
        if(!sale){
            next();
            return;
        }
        
        await sale.destroy();

        res.status(200).json({
            message: 'Deleted successfully!'
        });
    }
    catch(e){
        next(e);
    }
});

module.exports = router;