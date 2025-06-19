$clientId = "2db41b45-3b40-4bb7-87fd-acf8ad60d883"
$redirectUri = "http://localhost"
$scope = "api://2db41b45-3b40-4bb7-87fd-acf8ad60d883/.default" # or "openid profile email" for user info
$authUrl = "https://login.microsoftonline.com/organizations/oauth2/v2.0/authorize?client_id=$clientId&response_type=code&redirect_uri=$redirectUri&response_mode=query&scope=$scope&state=12345"

Start-Process $authUrl