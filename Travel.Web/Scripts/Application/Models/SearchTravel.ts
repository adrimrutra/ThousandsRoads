/// <reference path="../_references.ts" />
module Application
{
    export class SearchTravel {
        
        Id: number;     
        Startdate: Date;
        Starttime: Date;
        Enddate: Date;
        StartAddress: string;
        Startpoint: MapPoint;
        EndAddress: string;
        Endpoint: MapPoint;
        Capacity: number;
        Direction: DirectionType;
        Luggage: LuggageType;
        Wait: WaitType;
        Early: EarlyType;
    }
    export enum WaitType {
        Default = 0, Five = 1, Ten = 2, Fifteen = 3, Twenty = 4
    }
    export enum EarlyType {
        Default = 0, Five = 1, Ten = 2, Fifteen = 3, Twenty = 4
    }
} 