const jwt = require('jsonwebtoken');

module.exports = (req, res, next) => {
    try{        
        let token = req.headers['authorization'];
        if(token){
            token = token.split(' ')[1];
        }
        const key = process.env.JWT_KEY || 'secret';
        //console.log(token);

        const decoded = jwt.verify(token, key);
        req.userData = decoded;
        next();
    }
    catch (error){
        return res.status(401).json({
            message:"Invalid token"
        });
    }
};