'use strict';

var Alexa = require('alexa-sdk');




var handlers = {
  
  // LaunchRequest is called if the customer opens the skill without parameters. For example: "Alexa, start coffeemachine"
  'LaunchRequest': function() {
	  
	  // As we dont have a user interface so far, we go through to the next Intent
      this.emit('AnleitungIntent');
  },
  
  // OrderIntent is called if the customer says something what we have defined in the intent-section of Amazon Developer Account and tagged with "OrderIntent"
  'OrderIntent': function () {
	  
		  var quantity = this.event.request.intent.slots.SpecificNumber.value;
		  var drink = this.event.request.intent.slots.Drink.value;
		  
		
		  switch (quantity){
				case "einen":
					quantity = 1;
					break;
				case "ein":
					quantity = 1;
					break;
				case "zwei":
					quantity = 2;
					break;
				default:
					this.emit(':tell', 'Tut mir leid, die Kaffeemaschine kann nur eine oder zwei Tassen gleichzeitig zubereiten.');
		 }
		 
		 switch (drink){
				case "kaffee":
					drink = "Kaffee"
					break;
				case "espresso":
					drink = "Espresso"
					break;
				case "espressi":
					drink = "Espresso"
					break;
				default:
					this.emit(':tell', 'Tut mir leid, die Kaffeemaschine kann nur Kaffee oder Espresso zubereiten');
		 }
		 
        this.emit('ConfirmationIntent', quantity, drink);
    },
	
	// MixedOrderIntent is called if the customer says something what we have defined in the intent-section of Amazon Developer Account and tagged with "MixedOrderIntent"
  'MixedOrderIntent': function() {
	  
	  var quantityCoffee = 1;
	  var quantityEspresso = 1;
	  
	  httpGet(quantityCoffee, quantityEspresso, (statusCode) => {
                
				if (statusCode == 200){
					this.emit(':tell', 'Gerne, ich werde Ihnen einen Kaffee und eine Tasse Espresso machen' );
				}
				else
					this.emit(':tell', 'Tut mir Leid, die Bestellung hat nicht funktioniert');

            }
        );
	
	  
	  
  },
  
  // Here we just say: "You have to give your order if you start the coffeemachine. For Example: Alexa, start coffeemachine and make 2 coffee"
  'AnleitungIntent': function() {
      this.emit(':tell', 'Sie müssen beim Start der Kaffeemaschine deine Bestellung mitgeben. Zum Beispiel, Alexa, starte Kaffeemaschine und mache 2 Kaffee');
  },
  
  'ConfirmationIntent': function(quantity, drink) {
		var quantityEspresso = 0;
		var quantityCoffee = 0;
		
		if (drink == "Espresso") {
			quantityEspresso = quantity;
		}
		else
			quantityCoffee = quantity;
	  
		
		httpGet(quantityCoffee, quantityEspresso, (statusCode) => {
                
				if (statusCode == 200){
					this.emit(':tell', 'Alles klar, ich werde Ihnen ' + (quantity == 2 ? "zwei Tassen" : "eine Tasse") + ' ' + drink + ' zubereiten' );
				}
				else
					this.emit(':tell', 'Tut mir Leid, die Bestellung hat nicht funktioniert');

            }
        );
		
  }  

};



exports.handler = function(event, context, callback){
    var alexa = Alexa.handler(event, context);
    alexa.registerHandlers(handlers);
    alexa.execute();
};


var http = require('http');


function httpGet(quantityCoffee, quantityEspresso, callback) {

    
    var options = {
        host: 'coffeeinairstream.hopto.org',
        port: 80,
        path: '/cgi-bin/coffeeCGI.py?coffee=' + quantityCoffee + '&espresso=' + quantityEspresso,
        method: 'GET',

        
    };

    var req = http.request(options, res => {
        res.setEncoding('utf8');
        
		
		var returnData = "";

		
        res.on('data', chunk => {
            returnData = returnData + chunk;
        });

        res.on('end', () => {
            
            

			// We need the statusCode to know wheter the call was correctly
            callback(res.statusCode);  

        });

    });
    req.end();

}

