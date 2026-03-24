Run instruction: dotnet run

# Database:
## Interfaces:
### User,Equipment - abstract classes,we do not create instance of them directly,but we use all of its properties for each model

## Models:
### Concrete examples of abstract classes (User,Equipment),they inherit everything from their parent class and have
### thier own unique fields,methods,etc...

# Services:
## Interfaces
### SimplePenaltyPolicy - a concrete example of PenaltyPolicy that implements IPenatlyPolicy interface
## RentalService - core of bussiness logic in the project,that tracks all actions (add,remove,update) related to Equipment,Users
### RentalService supports both Interfaces (User,Equipment),so adding a new Equipment(ElectroBike) or a new type of User,will
###    be automatically supported by the service.Also any other implementation of IPenaltyPolicy interface can be used in RentalService.
###    RentalService has weak relation to User,Equipment Interfaces,and if new fields,methods are added to them,RentalService
###    wouldn't require any changes to work properly.



