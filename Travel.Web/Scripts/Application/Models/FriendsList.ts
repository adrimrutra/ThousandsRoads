/// <reference path="../_references.ts" />
module Application {

    export class FriendsList {
        Id: number;
        Customers: Array<User>;
        ParentList: FriendListItem;
        ParentListId: number;
    }
}  