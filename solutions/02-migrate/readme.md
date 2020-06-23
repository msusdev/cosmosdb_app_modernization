# Challenge 2: Solution

## Walkthrough

After reviewing the existing application and dataset, there are a few observations you can make:

- The **room** data is only ever queried as a ``JOIN`` to the **location** data
- We can safely embed the **room** child documents into a **location** document
- The remaining data in Azure SQL Database are of predictable data types that map easily to JSON

So, using an example SQL query result:

---

| Id | Name | Longitude | Latitude | MailingAddress | ParkingIncluded | ConferenceRoomsIncluded | ReceptionIncluded| PublicAccess | LastRenovationDate | Image |
| --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- | --- |  
| 1 | McGlynn - Kunze Square | 47.621201 | -122.338181 | 320 Westlake Ave N Seattle, WA 98109 | True | True | True | False | 1982-05-08T00:00:00.0000000+00:00 | 601e072a-dfea-4928-ad87-dbf75d1d6448.png |

---

| Id | Description | MonthlyRate | Seats | PrivateFacilities | PhoneIncluded | Windows | Corner | LocationId |
|--- |--- |--- |--- |--- |--- |--- |--- |--- |
| 1 | Coworking Office | 6120.92 | 5 | False | True | False | False | 1 |
| 170 | Coworking Corner Suite | 7220.40 | 5 | True | True | False | True | 1 |
| 171 | Coworking Office | 8924.69 | 5 | False | False | False | False | 1 |
| 172 | Pair Office | 3788.99 | 2 | False | True | False | False | 1 |
| 173 | Pair Corner Office | 2948.00 | 2 | False | True | False | True | 1 |

---

We, can shape this as a JSON document like [this sample document](./example.json) that has been abbreviated:

```json
{
    "name": "McGlynn - Kunze Square",
    "longitude": 47.621201,
    "latitude": -122.338181,
    "mailingAddress": "320 Westlake Ave N Seattle, WA 98109",
    ...
    "image": "601e072a-dfea-4928-ad87-dbf75d1d6448.png",
    "rooms": [
        {
            "description": "Coworking Office", "monthlyRate": 6120.92, "seats": 5, ...
        },
        {
            "description": "Coworking Corner Suite", "monthlyRate": 7220.40, "seats": 5, ..
        },
        {
            "description": "Coworking Office", "monthlyRate": 8924.69, "seats": 5, ..
        },
        {
            "description": "Pair Office", "monthlyRate": 3788.99, "seats": 2, ...
        },
        {
            "description": "Pair Corner Office", "monthlyRate": 2948.00, "seats": 2, ..
        }
    ]
}
```

Now that you have a basic JSON schema, you can begin to consider some of the other things you need before migrating to Azure Cosmos DB:

- A unique identifier for each document
- A partition key field.

A unique identifier can be created by simply generating an integer and printing it to a string. For the partition key field, you may need to create a synthetic partition key. As an example solution (among many others), we decided to create a synthetic key named ``territory`` that is parsed from the address string. For example, if we have this JSON document:

```json
{
    "name": "McGlynn - Kunze Square",
    "mailingAddress": "320 Westlake Ave N Seattle, WA 98109"
}
```

We can update the document by adding ``id`` and ``territory`` properties:

```json
{
    "id": "1",
    "name": "McGlynn - Kunze Square",
    "mailingAddress": "320 Westlake Ave N Seattle, WA 98109",
    "territory": "Washington",
}
```

Once your design tasks are complete, you can move forward with migrating the data.

For this challenge, it's easiest to migrate using the [Data Migration Tool instructions found here](https://docs.microsoft.com/azure/cosmos-db/import-data#SQL).

## Deployment Template

Use this template to deploy a pre-baked solution to your Azure subscription:

[![Deploy to Azure](https://docs.microsoft.com/en-us/azure/templates/media/deploy-to-azure.svg)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fgithub.com%2FMSUSDEV%2Fcosmosdb_app_modernization%2Fblob%2F02-solution%2Farmdeploy.json)
