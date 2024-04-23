const express = require('express');
const router = express.Router();

const { createCategory, getCategoryById, getCategories } = require("../db_connect.js");
const checkAuth = require("../authenticate_user.js");
const authorizeAdmin = require("../authorize_user.js");


router.get('/', async(req, res, next) => {
    try{
        res.status(200).json(await getCategories());
    }
    catch(e){
        next(e);
    }
});
router.post('/', checkAuth, authorizeAdmin, async(req, res, next) => {
    try{
        const name = req.body.name;

        if(!name){
            const err = new Error("Invalid data supplied!");
            err.status = 400;
            throw err;
        }

        let category = await createCategory(name);

        if(!category){
            return res.status(400).json({
                message: 'Category already exists!'
            });
        }
        
        res.status(201).json(category);
    }
    catch(e){
        next(e);
    }
});
router.delete('/:categoryId', checkAuth, authorizeAdmin, async(req, res, next) => {
    try{
        const categoryId = req.params.categoryId;
        let cat = await getCategoryById(categoryId);
        
        if(!cat){
            next();
            return;
        }
        
        await cat.destroy();

        res.status(200).json({
            message: 'Deleted successfully!'
        });
    }
    catch(e){
        next(e);
    }
});

module.exports = router;