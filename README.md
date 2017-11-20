# BotFramework-ChatHistoryBot
**Getting started**
1. Download the project
2. in the web.config of the ChatHistoryBot project fill out the values for `MicrosoftAppId`, `MicrosoftAppPassword`, and `BotChatSecret`(your webchat secret from [dev.botframework.com](https://dev.botframework.com/)) 
3. Create a database and put your connection string in the web.config for the ChatHistoryBot project and the app.config for the ChatHistoryBot.data **Note**: you do not need any tables in your database they will be created in the next step
4. in your Package Manager Console select ChatHistoryBot.Data as the Default project then run the `update-database` command
5. Run the project and navigate to http://localhost:3980/WebChat/Bot 
