const express = require('express');
const app = express();
const bodyParser = require('body-parser');

const loginRouteHandler = require('./inc/routes/login');
const categoriesRouteHandler = require('./inc/routes/categories');
const carsRouteHandler = require('./inc/routes/cars');
const rentalsRouteHandler = require('./inc/routes/rentals');

app.use(bodyParser.urlencoded({extended: false}));
app.use(bodyParser.json());

app.use((req, res, next) => {
    res.header('Access-Control-Allow-Origin', '*');
    res.header('Access-Control-Allow-Headers', '*');
    if(req.method === 'OPTIONS'){
        res.header('Access-Control-Allow-Methods', 'GET, POST, OPTIONS');
        return res.status(200).json({});
    }
    next();
});


app.use('/login', loginRouteHandler);
app.use('/categories', categoriesRouteHandler);
app.use('/cars', carsRouteHandler);
app.use('/rentals', rentalsRouteHandler);


app.use((req, res, next) => {
    const error = new Error("Not found!");
    error.status = 404;
    next(error);
});
app.use((error, req, res, next) => {
    res.status(error.status || 500);
    res.json({
        message: error.message
    });
});


module.exports = app;