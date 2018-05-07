# PollutionNotifier
An Azure Function aplication which sends daily e-mail notifications
about the state of air pollution in a given area. The users' data are downloaded from the mssql database.
Additionally, using the Google Geocoding api, the coordinates are obtained based on the address
which is needed to retrieve the air status information. The data is collected from two companies: looko2 and Airly.
