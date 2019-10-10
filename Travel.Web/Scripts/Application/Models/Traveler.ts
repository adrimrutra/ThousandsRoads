/// <reference path="../_references.ts" />
module Application {
    export class Traveler {
        Id: number;
        UserId: number;
        User: User;
        TravelId: number;
        Travel: Travel;
        Usertype: UserType;
    }
    export enum UserType {
        Driver = 1, Passenger = 2
    }
} 
	