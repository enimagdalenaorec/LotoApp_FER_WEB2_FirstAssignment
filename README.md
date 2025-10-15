Admin adds, closes rounds and gives drawn numbers with curl commands (all commands are for CMD):

1) Add a round:
curl -X POST https://localhost:7271/Admin/new-round -k

2) Close a round:
curl -X POST https://localhost:7271/Admin/close -k

3) Gives drawn numbers (List of int values):
curl -X POST https://localhost:7271/Admin/store-results -H "Content-Type: application/json" -d "{\"numbers\":[3,11,22,25,38,44]}" -k

