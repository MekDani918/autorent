const express = require('express');
const app = express();
const bodyParser = require('body-parser');

const loginRouteHandler = require('./inc/routes/login');
const carsRouteHandler = require('./inc/routes/cars');

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
app.use('/cars', carsRouteHandler);


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