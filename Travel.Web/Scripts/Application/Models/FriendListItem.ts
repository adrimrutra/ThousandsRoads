/// <reference path="../_references.ts" />
module Application {
    export class FriendListItem {
        Id: number;
        CustomerList: FriendsList;
        OwnerId: number;
        Owner: User;
    }
}   