Project Attractions
i kursen Dataåtkomster
av Tai Lenke Enarsson, Teknikhögskolan Gävle





Val om att undanta unik seedning och indexering har gjorts för att underlätta testning i detta projekt, detta är en del av projektet som behöver förbättras för att kunna tas i bruk.
     Även mer specifika DTOs för "snyggare" modeller utan mängden nästling skulle viljas ha i ett mer optimerat API. Ex attraction med cityId, cityName och countryId, countryName som direkta properties istället för nästlade via attraction -> city -> country och på det sättet även kunna slippa de tomma listorna som cirkulerar tillbaka till de övre objekten (lista av cities i country-objektet).