﻿# ef migrations

# infrastructure root // -s (means startup project)
# Add
dotnet ef migrations add AddUserFields -s ../PlaceRentalApp.API

# update
dotnet ef database update -s ../PlaceRentalApp.API