
/// <reference path="../_references.ts" />
module Application {
    export class Message {
        Id: number;
        UserId: number;
        UserEmail: string;
        MessengerId: number;
        MessengerDisplayName: string;
        MessengerAvatar: string;
        MessengerEmail: string;
        TravelId: number;
        Theme: string;
        MessageText: string;
        State: boolean;
        Direction: DirectionType;
        Luggage: LuggageType;
        PersonCount: number;
        Type: MessageType;
    }
    export enum LuggageType {
        Default = 0, Min = 1, Norm = 2, Max = 3
    }
    export enum DirectionType {
        One = 1 ,Two = 2
    }
    export enum MessageType {
        Subscribe = 1, Accept = 2, Decline = 3, Inner = 4, Outer = 5
    }
}










