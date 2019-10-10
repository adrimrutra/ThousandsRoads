/// <reference path="../_references.ts" />
module Application
{
    export class Travel {
        Capacity: number;
        Id: number;
        DriverId: number;
        Startdate: Date;
        Enddate: Date;
        MapPoints: Array<MapPoint>;
        Startpoint: string;
        Endpoint: string;
        Travelers: Array<Traveler>;
        DisplayName: string;
        Mapsnapshot: string;
        Friends: Array<MyFriend>;
        Luggage: LuggageType;
    }
} 