# Train-the-trainer

## Challenge 1

- Make sure the team takes their time reviewing the application
- Encourage the team to walk through the resources in Azure
- Make sure the teams are looking at the configuration for the resources
- If the team is comfortable, suggest looking at the ARM template
- Encourage the team to run the Function App's code
- The team should determine how a specific location is found (query string) or what a featured location is (top 4 orderby lastrenovationdaate desc)
- Encourage team to review Azure Container Instances logs
- Team doesn't have to review or understand the code for the:
  - Azure Web App
  - Azure Container Instance[s]

## Challenge 2

- [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio/)
- Think through embedding vs. referencing (consider future growth)
- Think through indexing policy
- Think through thoughput choies (during and post migration)
- Make sure all data types map "cleanly" to Azure Cosmos DB
- Make sure ``Description`` and ``Image`` fields don't surpass max document size (2 MB)
- The partition key choice should be made before any migration code is written
- The team should think about how this solution can scale if the company ever grows
- The team should also consider how the company currently uses data as some aspects of the existing application may limit changes to the data
  - Example: The application expects integers for the "Id" field
- Partition key candidates
  - ``/id``
  - Last digit of ``/id``
  - synthetic key generated from ``/lastRenovationDate`` (just get month or day)
  - synthetic key generate from ``/mailingAddress`` (just get state)
- Remind team that they can "recover" from a poor partition key choice using a live migration: <https://devblogs.microsoft.com/cosmosdb/how-to-change-your-partition-key/>
- Some tables may want to analyze the "featured" locations to see if they can fit them in a single partition using a query like: ``SELECT TOP 4 * FROM locations l ORDER BY l.lastRenovationDate DESC``

## Challenge 3

- [Azure Functions Core Tools](https://docs.microsoft.com/azure/azure-functions/functions-run-local)
