Command for obtaining needed info for admin curl commands:
 ->  curl -X POST https://DOMAIN_NAME/oauth/token -H "Content-Type: application/json" -d "{\"client_id\":\"CLIENT_ID\",\"client_secret\":\"CLIENT_SECRET\",\"audience\":\"AUDIENCE\",\"grant_type\":\"client_credentials\"}"
 ->  response is in format:
	{"access_token":"example_token",
	"scope":"start:round close:round store:results",
	"expires_in":86400,
	"token_type":"Bearer"}  which will be usen in the next commands as Bearer token value

IMPORTANT: “The real credentials will be provided privately; replace the placeholders above with them when testing the admin endpoints.”

Admin adds, closes rounds and gives drawn numbers with curl commands (all commands are for CMD):

1) Add a round:
curl -i -X POST https://lotoapp-fer-web2-firstassignment.onrender.com/new-round -H "Authorization: Bearer example_token"

2) Close a round:
curl -i -X POST https://lotoapp-fer-web2-firstassignment.onrender.com/close -H "Authorization: Bearer example_token"

3) Gives drawn numbers (List of int values):
curl -i -X POST https://lotoapp-fer-web2-firstassignment.onrender.com/store-results -H "Authorization: Bearer example_token" -H "Content-Type: application/json" -d "{\"numbers\":[5,12,27,34,41,40]}"
