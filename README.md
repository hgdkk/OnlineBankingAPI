# OnlineBankingAPI

1- API Published to Azure you can access from link below.
Link : https://onlinebankingapi.azurewebsites.net/swagger/index.html

![image](https://user-images.githubusercontent.com/13198189/167101321-5561a238-5372-4b36-bab8-f93c1744ca02.png)


2- Access token needed to access methods.
Token can be create from below url and parameters
Link : https://dev-sgck8myf.us.auth0.com/oauth/token
Method : Post
Body (Raw-JsonFormat): 
{
    "client_id":"nAmHNwHRTw0LPGP3TkQkjI5iKsmZvT5B",
    "client_secret":"dWfxNwkY0x7FupmtYinj5lhoupqNuxF9yMexDuVKBYg9LhKKA4BLfWKmN95qisa0",
    "audience":"https://gringottsonlinebanking.com",
    "grant_type":"client_credentials"
}

![image](https://user-images.githubusercontent.com/13198189/167100696-156d3949-3ef2-46cf-9cbd-a7e6f836b5f9.png)


3- After token created copy it and on the swagger page click authorize button on the right-top of the page and paste token.

![image](https://user-images.githubusercontent.com/13198189/167101452-d25cdd57-b749-4796-a901-d6b02591916a.png)
![image](https://user-images.githubusercontent.com/13198189/167101514-cd84d941-e6d9-45d8-a769-d390090fc235.png)


Now you can access to methods.
