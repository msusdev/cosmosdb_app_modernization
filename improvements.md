# Improvements

## Presentation

- Monitor queries to determine how often we are doing fan-out
- Operator behavior in Cosmos DB SQL Queries (indexed operators)
- SQL Query performance
- Jupyter notebooks
- Deeper dive into migration and OSS APIs
- The analytics story in Cosmos DB (reporting, materialized views, etc)

## Additional feedback

- Cosmos DB backend for Spark (Change Feed)
  - How do we capture this in a challenge?
  - Materialized Views? other stuff that can only be done well in SQL API
  - Show a better alternative to Cassandra/Monogo APIs
- Backend for ML applications
  - Can this fit our scenario?
- More time spent in the Data Explorer
- Enterprise/DevOps components
  - How do we implement this at-scale
  - Working with a SDK to write code
  - CI/CD pipelines
- Add management/control-plane challenge
  - Potentially automate deployment
  - More than just ARM (CLI, PowerShell, etc)
- Monitoring content
  - Reviewing request headers
  - Azure Monitor
- Challenges around writing data
  - Maybe data import tasking
  - Insert & Update (Upsert)

## Challenge 1

- Add submission logging for locations and rooms
- Add container variants for Node.js

## Challenge 2

- Significantly increase size of data set to force decision to have impact
- Make data naturally git a single partition key for top 4 results
- Change Id from int to string property in SQL (update EF Core)
  - <https://stackoverflow.com/questions/32983524/how-to-use-string-property-as-primary-key-in-entity-framework>

## Challenge 3

- Fix ARM Template
- Fix Seed Document

## Challenge 4

- Fix ARM Template
