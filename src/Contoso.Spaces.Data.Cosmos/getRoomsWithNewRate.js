function getRoomsWithNewRate(rooms){
    rooms.forEach(room =>
        room.monthlyRate = room.monthlyRate * 1.25
    );
    return rooms;
}
