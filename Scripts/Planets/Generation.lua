
for i = 0, GetRoomNumber() - 1 do
	

	for e = 0, 5 do
		location = GetRandomRoomLocation(GetRooms()[i])
		--print(location); 
		SpawnEntity(location.X, location.Y, 0);
	end
end

