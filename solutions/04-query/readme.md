# Challenge 4: Solution

## Walkthrough

In this challenge, we can solve the problem using a user-defined function.

### UDF

The easiest part of this implementation is to write the actual UDF code. In this example, we will create a udf named **getRoomsWithNewRate**.

This UDF will iterate over the array of rooms and increase their individual rates:

```js
function getRoomsWithNewRate(rooms){
    rooms.forEach(room =>
        room.monthlyRate = room.monthlyRate * 1.25
    );
    return rooms;
}
```

### Updated Query

You can then refer to this udf using the ``udf.getRoomsWithNewRate`` expression in your SQL query.

Unfortunately, our base query is too simple:

```sql
SELECT * FROM locations l WHERE l.id = '{id}'
```

To make this work, we will need to expand the query so we can reference the ``monthlyRate`` property directly:

```sql
SELECT VALUE {
    "id": l.id,
    "name": l.name,
    "longitude": l.longitude,
    "latitude": l.latitude,
    "mailingAddress": l.mailingAddress,
    "territory": l.territory,
    "parkingIncluded": l.parkingIncluded,
    "conferenceRoomsIncluded": l.conferenceRoomsIncluded,
    "rooms": l.rooms
} FROM locations l WHERE l.id = '{id}'
```

Now, we can make use of the UDF:

```sql
SELECT VALUE {
    "id": l.id,
    "name": l.name,
    "longitude": l.longitude,
    "latitude": l.latitude,
    "mailingAddress": l.mailingAddress,
    "territory": l.territory,
    "parkingIncluded": l.parkingIncluded,
    "conferenceRoomsIncluded": l.conferenceRoomsIncluded,
    "rooms": udf.getRoomsWithRate(l.rooms)
} FROM locations l WHERE l.id = '{id}'
```

## Deployment Template

Use this template to deploy a pre-baked solution to your Azure subscription:

[![Deploy to Azure](https://docs.microsoft.com/en-us/azure/templates/media/deploy-to-azure.svg)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fgithub.com%2FMSUSDEV%2Fcosmosdb_app_modernization%2Fblob%2F04-solution%2Farmdeploy.json)
