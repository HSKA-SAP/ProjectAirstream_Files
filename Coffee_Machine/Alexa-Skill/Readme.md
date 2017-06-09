# Step-by-Step Guide

## Step 1: Create an Amazon Developer Account

## Step 2: Create a new Alexa App in Amazon Developer Account

- Once you have logged in, click in the header navigation on "Alexa"

- Then click the "Alexa Skills Kit".

- Click "Add a new skill"

### Fill in the Skill Information Tab with following data:

> Skill Type: "Custom Interaction Model"

> Language: "German"

> Name: "kaffeemaschine"

> Invocation Name: "kaffeemaschine"

> Leave the Checkbox for Audio Player at "No"`

- Save and go to the Interaction Model

### Fill in the Interaction Model with following data:

> Copy the content from speechAssets/IntentSchema.json to the formular of Intent Schema

- Following values are the words, customer can say and Alexa will understand this

> Add a Custom Slot Type named "LIST_OF_NUMBERS" with values "einen", "ein", "zwei", "drei", "vier"

> Add a Custom Slot Type named "LIST_OF_DRINKS" with values "heißes Wasser", "heiße Wasser", "Wasser", "Kaffee", "Kaffees", "Espresso", "Espressi", "Teewasser"

> Copy the content from speechAssets/Utterances.txt into the formular for Sample Utternances

- Click on Save and go to the Configuration Tab

### Configuration Section

- Click the Checkbox for AWS Lambda ARN 

- Click the Checkbox for Europe

- Fill in in the textbox below the ARN Number: 

> arn:aws:lambda:eu-west-1:757669133698:function:kaffeemaschine

The ARN Number references the Code behind of the Alexa-Skill which is hosted as Lambda Function on Amazon Webservice Account. If you want to edit the code, you need to change that and deploy that on this account. How that works, you can read in Point 3 of this Guide.

- Leave the Checkbox for Account Linking on "No"

- Dont check anything in the Permission section

- Click Save and go to the Test-Tab

### Testing in Amazon Developer Account and on the device

- Make sure, you have enabled the slider at top to test

- In the section named Service Simulator, you can test the Utterances we have defined in step 2

- For example type in "und mache mir zwei Kaffee"

- Click "Ask Kaffeemaschine"

- The response should work without errors

- Also you can add this Alexa Skill on your own Alexa-Device if your Email Adress on Alexa-Account and Amazon-Developer Account are the same.

- Just ask your Alexa "Alexa, starte Kaffeemaschine und mache mir zwei Kaffee"

## Step 3 (Optional): Edition and Deployment of Skill-Code


- Since the node package alexa-skill is imported in this directory, I think you dont have to install nodejs and the alexa-sdk. 

- Just copy/pull the directories "speechAssets" and "src" in your local directory and please make changes in a new branch

- To edit something, open the src/index.js file and make your changes

- After your changes make sure, you are in the src directory and make a src.zip file that contains the directory "node_modules", "index.js" and "package.json". Its important that you dont zip the src-directory itself.

- Go in your Amazon Webservice Account (if you need my Account, just ask in the WhatsApp Group for a password)

- Choose "Services" in the top of the navigation

- Navigate to "Compute" and choose there "Lambda"

- Open the Lambda Function named "kaffeemaschine"

- In the code tab you can upload your zip-package

- Click on "Save and test"

- If you did not change or extend the intents then you are done and have nothing to change in Amazon Developer Account







