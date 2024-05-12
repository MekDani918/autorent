const express = require('express');
const app = express();
const bodyParser = require('body-parser');

const wsServer = require('./app_ws');

const registerRouteHandler = require('./inc/routes/registerRouteHandler');
const loginRouteHandler = require('./inc/routes/loginRouteHandler');
const categoriesRouteHandler = require('./inc/routes/categoriesRouteHandler')(wsServer);
const carsRouteHandler = require('./inc/routes/carsRouteHandler')(wsServer);
const rentalsRouteHandler = require('./inc/routes/rentalsRouteHandler');
const salesRouteHandler = require('./inc/routes/salesRouteHandler')(wsServer);

app.use(bodyParser.urlencoded({extended: false}));
app.use(bodyParser.json());

app.use((req, res, next) => {
    res.header('Access-Control-Allow-Origin', '*');
    res.header('Access-Control-Allow-Headers', '*');
    if(req.method === 'OPTIONS'){
        res.header('Access-Control-Allow-Methods', 'GET, POST, PATCH, DELETE, OPTIONS');
        return res.status(200).json({});
    }
    next();
});


app.use('/register', registerRouteHandler);
app.use('/login', loginRouteHandler);
app.use('/categories', categoriesRouteHandler);
app.use('/cars', carsRouteHandler);
app.use('/rentals', rentalsRouteHandler);
app.use('/sales', salesRouteHandler);


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


module.exports.app = app;
module.exports.wsServer = wsServer;