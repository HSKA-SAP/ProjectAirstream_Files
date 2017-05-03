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
				case "heißes wasser":
					drink = "heißes Wasser"
					break;
				case "heiße wasser":
					drink = "heißes Wasser"
					break;
				case "kaffee":
					drink = "Kaffee"
					break;
				case "espresso":
					drink = "Espresso"
					break;
				default:
					this.emit(':tell', 'Tut mir leid, die Kaffeemaschine kann nur Kaffee, Espresso oder heißes Wasser zubereiten');
		 }
		 
        this.emit('ConfirmationIntent', quantity, drink);
    },
	
	// MixedOrderIntent is called if the customer says something what we have defined in the intent-section of Amazon Developer Account and tagged with "MixedOrderIntent"
  'MixedOrderIntent': function() {
	  
	  //To-DO Call function with parameters for one coffee and one hot water
	  var speechOut = 'Gerne, ich werde dir einen Kaffee und eine Tasse heißes Wasser machen';
	  this.emit(':tell', speechOut);
	  
  },
  
  // Here we just say: "You have to give your order if you start the coffeemachine. For Example: Alexa, start coffeemachine and make 2 coffee"
  'AnleitungIntent': function() {
      this.emit(':tell', 'Du musst beim Start der Kaffeemaschine deine Bestellung mitgeben. Zum Beispiel, Alexa, starte Kaffeemaschine und mache 2 Kaffee');
  },
  
  'ConfirmationIntent': function(quantity, drink) {
	  
	  
	  //To-Do Call the function for making coffee with parameters "quantity" and "drink"
	  var speechOut = 'Alles klar, ich werde dir ' + (quantity == 2 ? "zwei Tassen" : "eine Tasse") + ' ' + drink + ' zubereiten';
      this.emit(':tell', speechOut);
  }  
};

exports.handler = function(event, context, callback){
    var alexa = Alexa.handler(event, context);
    alexa.registerHandlers(handlers);
    alexa.execute();
};