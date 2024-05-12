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

        try{
            if(this.wsServer && this.wsServer.clients){
                for(wsocket of this.wsServer.clients){
                    wsocket.send(JSON.stringify({
                        action:'newsale',
                        message: `Car with the id of ${carId} is now on sale!`,
                        // data: {
                        //     car_id: carId,
                        //     percent: percent,
                        //     description: description
                        // }
                        data: sale
                    }));
                }
            }
        }
        catch(er){
            console.error(er);
        }
    }
    catch(e){
        next(e);
    }
});
router.delete('/:carId', checkAuth, authorizeAdmin, async(req, res, next) => {
    try{
        const carId = req.params.carId;
        let sale = await getSaleByCarId(carId);
        
        if(!sale){
            next();
            return;
        }
        
        await sale.destroy();

        res.status(200).json({
            message: 'Deleted successfully!'
        });

        try{
            if(this.wsServer && this.wsServer.clients){
                for(wsocket of this.wsServer.clients){
                    wsocket.send(JSON.stringify({
                        action:'saleended',
                        message: `Sale for the car with id of ${carId} is now ended!`,
                        data: carId
                    }));
                }
            }
        }
        catch(er){
            console.error(er);
        }
    }
    catch(e){
        next(e);
    }
});

module.exports = (wsServerIn)=>{
    this.wsServer = wsServerIn;
    return router;
};