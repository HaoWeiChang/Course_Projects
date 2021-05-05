var express = require('express')

var app = express();

var port =8888;

app.get('/*',express.static(__dirname+'/app')); 

app.listen(port,function(){
    console.log('Listening on port:'+port);
});
