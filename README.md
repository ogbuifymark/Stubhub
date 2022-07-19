# Stubhub

1. TASK
                1. What should be your approach to getting the list of events? 
                    The list of events can be added using prompt to ask the user to inputs the name and city of the event. and then added to the list of Event.
                2. How would you call the AddToEmail method in order to send the events in an email? Then i will loop through list of Events and call the AddToEmail function 
                3. What is the expected output if we only have the client John Smith? On the Console the output will be something like this "{CustomerName}: {EventName}  in {CityName}"
                4. Do you believe there is a way to improve the code you first wrote? i could use IEmunerable to handle the list 
             
             
2. TASK
                1. What should be your approach to getting the distance between the customer’s city and
                    the other cities on the list? i will loop through the list of events and in each case call the GetDistance function and save it in a list of EventDistance
                2. How would you get the 5 closest events and how would you send them to the client in an
                    email? I will filter the list of EventDistance and then sort the list in ascending order, then i will add the first five in a new list of EventDistance called sortedByDistance,
                    then i will loop through the list and then call the AddToEmail function
                3. What is the expected output if we only have the client John Smith? On the Console the output will be something like this "{CustomerName}: {EventName}  in {CityName} {(Distance)}"
                4. Do you believe there is a way to improve the code you first wrote? break the part of the code into a new function for readability, also use dictionary to map event name to the distance 
